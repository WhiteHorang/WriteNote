using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WriteNote
{
    // 본문 입력: 글쓰기 단축키 실행, 기호 넣기, 붙여넣기, 한글 조합 처리, 글자 수, 문서 내 검색(Ctrl + F)
    public partial class MainForm
    {
        private int searchIndex = 0;

        private string lastSearchWord = "";


        private void ShowSearchPanel()
        {
            pnlSearch.Visible = true;
            txtSearchWord.Focus();
        }


        // 글자 수 갱신 (설정 > 글쓰기 > 글자 수 기준)
        // - 공백 포함: 줄바꿈만 빼고 센다.
        // - 공백 제외: 띄어쓰기, 탭, 줄바꿈을 모두 빼고 센다.
        private void UpdateCharCount()
        {
            // rtbContent.Text는 한글 조합을 끊으므로 쓰지 않는다. (GetEditorText 설명 참고)
            string text = GetEditorText();
            bool excludeSpaces =
                appSettings.Writing.CharCountMode == CharCountMode.ExcludeSpaces;

            int count = 0;

            foreach (char c in text)
            {
                if (c == '\r' || c == '\n')
                    continue;

                if (excludeSpaces && char.IsWhiteSpace(c))
                    continue;

                count++;
            }

            lblCharCount.Text = excludeSpaces
                ? $"글자 수: {count:N0} (공백 제외)"
                : $"글자 수: {count:N0} (공백 포함)";
        }


        // ───────────── 본문 읽기 (한글 조합을 끊지 않는 방식) ─────────────

        [StructLayout(LayoutKind.Sequential)]
        private struct GETTEXTEX
        {
            public int cb;
            public int flags;
            public int codepage;
            public IntPtr lpDefaultChar;
            public IntPtr lpUsedDefChar;
        }


        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, ref GETTEXTEX wParam, System.Text.StringBuilder lParam);


        [DllImport("imm32.dll")]
        private static extern int ImmGetCompositionString(IntPtr hIMC, int dwIndex, IntPtr lpBuf, int dwBufLen);


        // 본문 전체 글자를 읽는다.
        // RichTextBox.Text는 내부적으로 문서를 통째로 꺼내는 방식(StreamOut)이라
        // 한글을 조합하는 중에 읽으면 조합이 끊긴다. (예: '됐다'가 'ㄷㅙㅆ다'로 입력됨)
        // EM_GETTEXTEX로 글자만 읽으면 조합에 영향을 주지 않는다.
        // 줄바꿈은 RichTextBox.Text와 같게 \n 한 글자로 맞춘다. (글자 위치가 같아야 함)
        private string GetEditorText()
        {
            if (!rtbContent.IsHandleCreated)
                return rtbContent.Text;

            const int EM_GETTEXTEX = 0x0400 + 94;
            const int GT_DEFAULT = 0;
            const int CP_UNICODE = 1200;

            int length = rtbContent.TextLength;
            int capacity = length + 16;

            System.Text.StringBuilder buffer = new System.Text.StringBuilder(capacity);

            GETTEXTEX options = new GETTEXTEX
            {
                cb = capacity * 2,
                flags = GT_DEFAULT,
                codepage = CP_UNICODE
            };

            SendMessage(rtbContent.Handle, EM_GETTEXTEX, ref options, buffer);

            return buffer.ToString().Replace('\r', '\n');
        }


        // 본문에서 한글을 조합하는 중인지 (조합 중인 글자가 있으면 true)
        private bool IsImeComposing()
        {
            if (!rtbContent.IsHandleCreated)
                return false;

            const int GCS_COMPSTR = 0x0008;

            IntPtr context = ImmGetContext(rtbContent.Handle);

            if (context == IntPtr.Zero)
                return false;

            try
            {
                return ImmGetCompositionString(context, GCS_COMPSTR, IntPtr.Zero, 0) > 0;
            }
            finally
            {
                ImmReleaseContext(rtbContent.Handle, context);
            }
        }


        [DllImport("imm32.dll")]
        private static extern IntPtr ImmGetContext(IntPtr hWnd);


        [DllImport("imm32.dll")]
        private static extern bool ImmNotifyIME(IntPtr hIMC, int dwAction, int dwIndex, int dwValue);


        [DllImport("imm32.dll")]
        private static extern bool ImmReleaseContext(IntPtr hWnd, IntPtr hIMC);


        // 본문에서 조합 중인 한글을 확정한다.
        private void CommitImeComposition()
        {
            const int NI_COMPOSITIONSTR = 0x0015;
            const int CPS_COMPLETE = 0x0001;

            IntPtr context = ImmGetContext(rtbContent.Handle);

            if (context == IntPtr.Zero)
                return;

            try
            {
                ImmNotifyIME(context, NI_COMPOSITIONSTR, CPS_COMPLETE, 0);
            }
            finally
            {
                ImmReleaseContext(rtbContent.Handle, context);
            }
        }


        // 서식을 유지한 채 붙여 넣되, 글자색만 현재 테마 색으로 맞춘다.
        // (웹·워드에서 복사한 검은 글자가 어두운 화면에서 안 보이는 문제 방지)
        private void PasteWithThemeTextColor()
        {
            if (rtbContent.ReadOnly)
                return;

            int start = rtbContent.SelectionStart;

            rtbContent.Paste();

            int end = rtbContent.SelectionStart;

            if (end > start)
            {
                rtbContent.Select(start, end - start);
                rtbContent.SelectionColor = rtbContent.ForeColor;
                rtbContent.Select(end, 0);
            }
        }


        // 클립보드의 글자만 본문에 붙여 넣는다. (글꼴, 색 등 서식 제외)
        private void PastePlainText()
        {
            if (rtbContent.ReadOnly)
                return;

            string text;

            try
            {
                if (!Clipboard.ContainsText())
                    return;

                text = Clipboard.GetText();
            }
            catch (Exception)
            {
                // 다른 프로그램이 클립보드를 사용 중이면 무시
                return;
            }

            // RichTextBox는 줄바꿈을 \n 한 글자로 저장한다.
            rtbContent.SelectedText = text.Replace("\r\n", "\n");
        }


        // 본문 변경 이벤트
        private void rtbContent_TextChanged(object sender, EventArgs e)
        {
            UpdateCharCount();

            isModified = true;
            lblSaveStatus.Text = "수정됨";
        }


        // 제목 변경 이벤트
        private void txtTitle_TextChanged(object sender, EventArgs e)
        {
            isModified = true;
            lblSaveStatus.Text = "수정됨";

            // 사이드 패널 카드의 제목은 저장하면 목록을 다시 그릴 때 바뀐다.
        }



        // 키보드 단축키
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            // 글쓰기 단축키 (설정에서 추가·수정·삭제한 목록)
            // 키 조합이 정확히 같을 때만 실행한다. 예: Ctrl + . 과 Ctrl + Shift + . 은 다른 단축키
            // (설정 > 글쓰기에서 '글쓰기 단축키 사용'을 끄면 실행하지 않는다)
            ShortcutItem shortcut = appSettings.Writing.WritingShortcutsEnabled
                ? writingShortcuts.Find(item => item.Keys == e.KeyData)
                : null;

            // 붙여넣기 (Ctrl + V, Shift + Insert)
            // '서식 빼고 글자만 넣기'가 켜져 있으면 본문에 글자만 붙여 넣는다.
            bool isPasteKey =
                e.KeyData == (Keys.Control | Keys.V) ||
                e.KeyData == (Keys.Shift | Keys.Insert);

            if (isPasteKey && rtbContent.Focused)
            {
                if (appSettings.Writing.PastePlainText)
                {
                    PastePlainText();
                }
                else
                {
                    PasteWithThemeTextColor();
                }
            }
            else if (shortcut != null)
            {
                InsertSymbol(shortcut.Content, shortcut.CursorOffset);
            }
            // 변경할 수 없는 기본 단축키
            else if (e.Control && e.Shift && e.KeyCode == Keys.S)
            {
                SaveAsDocument();
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                SaveDocument();
            }
            else if (e.Control && e.KeyCode == Keys.O)
            {
                OpenDocument();
            }
            else if (e.Control && e.KeyCode == Keys.N)
            {
                if (ConfirmSaveChanges())
                {
                    CreateNewDocument();
                }
            }
            else if (e.Control && e.KeyCode == Keys.B)
            {
                btnToggleSidebar.PerformClick();
            }
            else if (e.Control && e.KeyCode == Keys.F)
            {
                if (pnlSearch.Visible)
                {
                    pnlSearch.Visible = false;
                    rtbContent.Focus();
                }
                else
                {
                    ShowSearchPanel();
                }
            }
            else if (e.KeyData == Keys.F1)
            {
                // 설명서 열기
                ShowHelp();
            }
            else
            {
                return;
            }

            e.SuppressKeyPress = true;
            e.Handled = true;
        }



        // 기호 입력
        private void InsertSymbol(string symbol, int cursorOffset = -1)
        {
            // 본문에 포커스가 없으면 본문으로 이동
            rtbContent.Focus();

            // 한글을 조합하는 중이면 그 글자를 먼저 확정한다.
            // (예: '가'를 치는 중에 Ctrl + [ → 「가」가 아니라 「」가 되는 문제 방지)
            CommitImeComposition();

            // RichTextBox는 줄바꿈을 \n 한 글자로 저장하므로
            // \r\n을 그대로 쓰면 커서 위치 계산이 줄 수만큼 어긋난다.
            symbol = (symbol ?? "").Replace("\r\n", "\n");

            // 커서 위치가 내용 길이를 넘으면 내용 끝으로 둔다.
            if (cursorOffset > symbol.Length)
            {
                cursorOffset = -1;
            }

            // 현재 커서 위치에 기호 삽입
            int start = rtbContent.SelectionStart;

            rtbContent.SelectedText = symbol;

            // 삽입 후 커서 위치 설정
            if (cursorOffset >= 0)
            {
                rtbContent.SelectionStart = start + cursorOffset;
            }
            else
            {
                rtbContent.SelectionStart = start + symbol.Length;
            }

            rtbContent.SelectionLength = 0;
        }



        // 다음 검색
        private void btnFindNext_Click(object sender, EventArgs e)
        {
            string searchWord = txtSearchWord.Text;

            if (string.IsNullOrWhiteSpace(searchWord))
            {
                MessageBox.Show("검색어를 입력해 주세요.", "WriteNote", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 검색어가 변경되었는지 확인
            bool isNewSearch = searchWord != lastSearchWord;

            if (isNewSearch)
            {
                lastSearchWord = searchWord;
            }

            // 검색 시작 위치 설정
            int startIndex;

            if (isNewSearch)
            {
                startIndex = 0;
            }
            else
            {
                startIndex = rtbContent.SelectionStart
                           + rtbContent.SelectionLength;
            }

            // 다음 방향으로 검색
            int foundIndex = rtbContent.Find(
                searchWord,
                startIndex,
                RichTextBoxFinds.None
            );

            if (foundIndex >= 0)
            {
                rtbContent.Select(foundIndex, searchWord.Length);

                // 검색 결과를 화면에 표시
                rtbContent.ScrollToCaret();

                // 검색창에 포커스 유지
                txtSearchWord.Focus();

                searchIndex = foundIndex + searchWord.Length;
            }
            else
            {
                MessageBox.Show("더 이상 검색 결과가 없습니다.", "WriteNote", MessageBoxButtons.OK, MessageBoxIcon.Information);
                searchIndex = 0;
            }
        }



        // 이전 검색
        private void btnFindPrevious_Click(object sender, EventArgs e)
        {
            string searchWord = txtSearchWord.Text;

            if (string.IsNullOrWhiteSpace(searchWord))
            {
                MessageBox.Show("검색어를 입력해 주세요.", "WriteNote", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 검색어가 변경되었는지 확인
            bool isNewSearch = searchWord != lastSearchWord;

            if (isNewSearch)
            {
                lastSearchWord = searchWord;
            }

            // 검색 시작 위치 설정
            int endIndex;

            if (isNewSearch)
            {
                // 처음 이전 검색을 누르면 문서 끝에서 시작
                endIndex = rtbContent.TextLength;
            }
            else
            {
                // 현재 선택된 검색어보다 앞에서 검색
                endIndex = rtbContent.SelectionStart - 1;
            }

            if (endIndex < 0)
            {
                MessageBox.Show("이전 검색 결과가 없습니다.", "WriteNote", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 이전 방향으로 검색
            int foundIndex = rtbContent.Find(
                searchWord,
                0,
                endIndex,
                RichTextBoxFinds.Reverse
            );

            if (foundIndex >= 0)
            {
                rtbContent.Select(foundIndex, searchWord.Length);

                rtbContent.ScrollToCaret();

                txtSearchWord.Focus();

                searchIndex = foundIndex;
            }
            else
            {
                MessageBox.Show("이전 검색 결과가 없습니다.", "WriteNote", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



        // 검색창 닫기
        private void btnCloseSearch_Click(object sender, EventArgs e)
        {
            pnlSearch.Visible = false;
            rtbContent.Focus();
        }


        // 검색창 키보드 입력 처리
        private void txtSearchWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (e.Shift)
                {
                    // Shift + Enter : 이전 검색
                    btnFindPrevious.PerformClick();
                }
                else
                {
                    // Enter : 다음 검색
                    btnFindNext.PerformClick();
                }

                e.SuppressKeyPress = true;
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                // Esc : 검색창 닫기
                btnCloseSearch.PerformClick();

                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }
    }
}
