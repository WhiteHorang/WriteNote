using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WriteNote
{
    // WriteNote 설명서 창
    // - 왼쪽 목차를 누르면 그 장으로 이동한다.
    // - 위쪽 찾기 칸에 단어를 넣고 Enter(또는 '찾기')를 누르면 본문에서 찾는다.
    // - 설정 창의 '설명서' 버튼, 글쓰기 화면의 F1로 연다.
    public partial class HelpForm : Form
    {
        // 목차: 장 제목과 본문 안의 시작 위치
        private readonly List<KeyValuePair<string, int>> sections = new List<KeyValuePair<string, int>>();

        private Font headingFont;
        private Font subFont;
        private Font bodyFont;
        private Font boldFont;
        private Font noteFont;
        private Font licenseFont;

        private Color textColor;
        private Color headingColor;
        private Color subColor;
        private Color noteColor;

        public HelpForm()
        {
            InitializeComponent();

            // 디자이너의 어두운 테마 색을 현재 테마로 바꾼다.
            // (설명서 글자색은 아래에서 따로 칠하므로 반드시 먼저 적용한다)
            ThemeManager.Apply(this, AppTheme.Dark);

            lstHelpSections.SelectedIndexChanged += (s, e) => GoToSection(lstHelpSections.SelectedIndex);
            btnHelpFind.Click += (s, e) => FindNext();
            btnHelpClose.Click += (s, e) => Close();

            txtHelpFind.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    FindNext();
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                }
            };

            Disposed += (s, e) =>
            {
                foreach (Font font in new[] { headingFont, subFont, bodyFont, boldFont, noteFont, licenseFont })
                {
                    if (font != null)
                        font.Dispose();
                }
            };

            BuildContent();
        }

        // 목차 제목 목록 (테스트·확인용)
        public IEnumerable<string> SectionTitles
        {
            get
            {
                foreach (KeyValuePair<string, int> section in sections)
                    yield return section.Key;
            }
        }

        public string ContentText
        {
            get { return rtbHelp.Text; }
        }

        // ───────────── 목차 이동 · 찾기 ─────────────

        public void GoToSection(int index)
        {
            if (index < 0 || index >= sections.Count)
                return;

            // 끝으로 갔다가 돌아오면 그 장 제목이 화면 맨 위에 오도록 스크롤된다.
            rtbHelp.Select(rtbHelp.TextLength, 0);
            rtbHelp.ScrollToCaret();
            rtbHelp.Select(sections[index].Value, 0);
            rtbHelp.ScrollToCaret();
        }

        // 찾기 칸의 단어를 지금 위치 다음부터 찾는다. (끝까지 가면 처음부터)
        public bool FindNext()
        {
            string word = txtHelpFind.Text.Trim();

            if (word.Length == 0)
            {
                txtHelpFind.Focus();
                return false;
            }

            int start = rtbHelp.SelectionStart + rtbHelp.SelectionLength;
            int found = start < rtbHelp.TextLength
                ? rtbHelp.Find(word, start, RichTextBoxFinds.None)
                : -1;

            if (found < 0)
                found = rtbHelp.Find(word, 0, RichTextBoxFinds.None);

            if (found < 0)
            {
                MessageBox.Show("'" + word + "'을(를) 찾지 못했습니다.", Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            rtbHelp.ScrollToCaret();
            return true;
        }

        // ───────────── 글 쓰기 도구 ─────────────

        private void Write(string text, Font font, Color color, bool bullet = false, int indent = 0)
        {
            rtbHelp.Select(rtbHelp.TextLength, 0);
            rtbHelp.SelectionFont = font;
            rtbHelp.SelectionColor = color;
            rtbHelp.SelectionBullet = bullet;
            rtbHelp.SelectionIndent = indent;
            rtbHelp.SelectionHangingIndent = bullet ? 18 : 0;
            rtbHelp.SelectedText = text;
        }

        // 장 제목 (목차에 들어감)
        private void Section(string title)
        {
            if (rtbHelp.TextLength > 0)
                Write("\n", bodyFont, textColor);

            sections.Add(new KeyValuePair<string, int>(title, rtbHelp.TextLength));
            lstHelpSections.Items.Add("  " + title);

            Write(title + "\n", headingFont, headingColor);
        }

        private void Sub(string title)
        {
            Write("\n" + title + "\n", subFont, subColor);
        }

        private void Para(string text)
        {
            Write(text + "\n", bodyFont, textColor);
        }

        private void Bullet(string text)
        {
            Write(text + "\n", bodyFont, textColor, true, 14);
        }

        // 굵은 이름 + 설명 (단축키, 메뉴, 설정 항목)
        private void Item(string name, string description)
        {
            Write(name, boldFont, textColor, true, 14);
            Write("  —  " + description + "\n", bodyFont, textColor, true, 14);
        }

        private void Note(string text)
        {
            Write(text + "\n", noteFont, noteColor, false, 14);
        }

        // exe 안에 넣어 둔 라이선스 문서를 그대로 보여 준다.
        private void License(string title, string resourceName)
        {
            Sub(title);

            string text = EmbeddedAssemblies.ReadTextResource(resourceName);

            if (string.IsNullOrEmpty(text))
            {
                Note("(라이선스 문서를 찾을 수 없습니다: " + resourceName + ")");
                return;
            }

            Write(text.Replace("\r\n", "\n").TrimEnd() + "\n", licenseFont, noteColor, false, 14);
        }

        // ───────────── 설명서 내용 ─────────────

        private void BuildContent()
        {
            headingFont = new Font("맑은 고딕", 15f, FontStyle.Bold);
            subFont = new Font("맑은 고딕", 11.5f, FontStyle.Bold);
            bodyFont = new Font("맑은 고딕", 10f, FontStyle.Regular);
            boldFont = new Font("맑은 고딕", 10f, FontStyle.Bold);
            noteFont = new Font("맑은 고딕", 9.5f, FontStyle.Italic);
            licenseFont = new Font("맑은 고딕", 9f, FontStyle.Regular);

            bool light = ThemeManager.Current == AppTheme.Light;
            textColor = ThemeManager.C(Color.FromArgb(224, 224, 224));
            headingColor = light ? Color.FromArgb(90, 70, 160) : Color.FromArgb(180, 160, 240);
            subColor = light ? Color.FromArgb(40, 40, 46) : Color.FromArgb(242, 242, 242);
            noteColor = light ? Color.FromArgb(110, 110, 120) : Color.FromArgb(160, 160, 170);

            rtbHelp.Clear();

            // 1
            Section("1. WriteNote 소개");
            Para("WriteNote는 집중해서 글을 쓰기 위한 글쓰기 프로그램입니다. 문서는 RTF 파일로 저장되고, " +
                 "최근 문서·즐겨찾기·폴더·휴지통으로 관리할 수 있습니다. 자주 쓰는 기호를 단축키로 넣고, " +
                 "맞춤법이 의심되는 단어에는 빨간 물결선이 표시됩니다.");
            Sub("화면 구성");
            Item("위쪽 버튼", "새 문서 / 열기 / 저장 / 다른 이름으로 저장");
            Item("☰ 버튼", "사이드 패널(문서 목록) 열기·닫기 (Ctrl + B)");
            Item("제목 칸", "문서 제목. 처음 저장할 때 파일 이름으로도 쓰입니다.");
            Item("본문", "글을 쓰는 곳");
            Item("아래 상태 표시줄", "글자 수와 저장 상태(새 문서 · 수정됨 · 저장됨 · 자동 저장됨)");

            // 2
            Section("2. 문서 쓰기와 저장");
            Item("새 문서 (Ctrl + N)", "빈 문서를 엽니다. 저장하지 않은 내용이 있으면 먼저 저장할지 묻습니다.");
            Item("저장 (Ctrl + S)", "처음 저장할 때는 저장 위치와 파일 이름을 고릅니다. 이후에는 같은 파일에 덮어씁니다.");
            Item("다른 이름으로 저장 (Ctrl + Shift + S)", "다른 파일 이름이나 위치로 저장합니다.");
            Item("열기 (Ctrl + O)", "RTF 문서를 엽니다. 사이드 패널의 문서 카드를 두 번 클릭해도 열립니다.");
            Item("자동 저장", "설정 → 글쓰기에서 켜면 정해진 시간(1·3·5·10·30분)마다 저장합니다. " +
                           "한 번 이상 저장한 문서만 대상이며, 확인 창이 떠 있을 때는 저장하지 않습니다.");
            Item("닫을 때 확인", "저장하지 않은 내용이 있으면 새 문서, 열기, 창 닫기 전에 '예 / 아니요 / 취소'로 묻습니다.");
            Item("글자 수", "상태 표시줄에 표시됩니다. 설정 → 글쓰기에서 '공백 포함 / 공백 제외'를 고릅니다. 줄바꿈은 세지 않습니다.");
            Note("문서는 .rtf 파일로, 제목·즐겨찾기 같은 정보는 같은 이름의 .json 파일로 함께 저장됩니다.");
            Note("문서를 열면 본문은 설정한 글꼴과 테마 글자색으로 표시됩니다.");

            // 3
            Section("3. 사이드 패널과 문서 목록");
            Para("☰ 버튼이나 Ctrl + B로 엽니다.");
            Sub("분류");
            Item("전체 문서", "문서 폴더(와 하위 폴더)의 문서");
            Item("최근 문서", "최근에 열거나 저장한 문서 (최대 10개)");
            Item("즐겨찾기", "★ 표시한 문서");
            Item("휴지통", "삭제한 문서 (패널 아래쪽, 5장 참고)");
            Sub("찾기와 정렬");
            Item("검색 칸", "제목으로 문서를 찾습니다.");
            Item("정렬", "최근 수정순 / 오래된 수정순 / 이름순");
            Item("폴더 선택 상자", "모든 폴더 / 분류 없음 / 각 폴더 (4장 참고)");
            Sub("문서 카드");
            Para("카드에는 제목(즐겨찾기는 ★), 수정 날짜, 하위 폴더에 있으면 폴더 이름이 표시됩니다. " +
                 "한 번 클릭하면 선택, 두 번 클릭하면 엽니다.");
            Sub("문서 카드 우클릭 메뉴");
            Item("문서 열기", "문서를 엽니다.");
            Item("즐겨찾기에 추가 / 해제", "★ 표시를 붙이거나 뗍니다.");
            Item("파일 위치 열기", "탐색기에서 파일을 선택한 상태로 엽니다.");
            Item("이름 변경", "파일 이름과 제목을 함께 바꿉니다.");
            Item("폴더로 이동", "다른 폴더로 옮깁니다. 같은 이름의 문서가 있으면 덮어쓰지 않습니다.");
            Item("최근 목록에서 제거", "파일은 그대로 두고 최근 목록에서만 뺍니다.");
            Item("휴지통으로 이동", "바로 지우지 않고 휴지통으로 옮깁니다.");
            Item("속성 보기", "제목, 위치, 크기, 날짜, 즐겨찾기 여부, 글자 수를 봅니다.");

            // 4
            Section("4. 폴더별 분류");
            Para("문서 폴더 안의 하위 폴더를 분류로 사용합니다. (한 단계까지)");
            Item("폴더 선택 상자", "'모든 폴더'는 전부, '분류 없음'은 문서 폴더 바로 아래 문서, 폴더 이름은 그 폴더 문서만 보여 줍니다.");
            Item("⋯ 버튼", "새 폴더 만들기 / 폴더 이름 바꾸기 / 폴더 삭제 / 탐색기에서 열기");
            Item("문서 옮기기", "문서 카드 우클릭 → 폴더로 이동. '새 폴더 만들어 이동...'도 고를 수 있습니다.");
            Item("새 문서 저장 위치", "폴더를 골라 둔 상태에서 새 문서를 저장하면 저장 창이 그 폴더에서 열립니다.");
            Note("폴더 이름을 바꾸면 최근 문서, 열린 문서, 휴지통의 복원 위치도 함께 바뀝니다.");
            Note("폴더를 삭제하면 안의 문서는 휴지통으로 옮기고 빈 폴더만 지웁니다. 다른 파일이 남아 있으면 폴더는 그대로 둡니다.");
            Note("'.'으로 시작하는 폴더와 숨김 폴더는 목록에 나오지 않습니다.");

            // 5
            Section("5. 휴지통");
            Para("문서를 삭제하면 바로 지우지 않고 휴지통에 보관합니다. 사이드 패널 아래쪽 '휴지통' 버튼으로 봅니다.");
            Item("복원", "휴지통 카드를 두 번 클릭하거나 우클릭 → 복원. 원래 위치로 돌아가고 제목·즐겨찾기도 유지됩니다.");
            Item("같은 이름이 있을 때", "덮어쓰지 않고 '제목 (2)'처럼 이름을 바꿔 복원합니다.");
            Item("영구 삭제", "우클릭 → 영구 삭제. '예'를 눌러야 지워지며 되돌릴 수 없습니다.");
            Item("휴지통 비우기", "우클릭 → 휴지통 비우기. 휴지통의 문서를 모두 영구 삭제합니다.");
            Note("휴지통은 문서 폴더가 아니라 %AppData%\\WriteNote\\Trash에 있어서, 문서 폴더를 바꿔도 그대로입니다.");

            // 6
            Section("6. 글쓰기 단축키");
            Sub("기본 글쓰기 단축키 (설정에서 바꾸거나 지울 수 있음)");
            Item("Ctrl + Enter", "세 줄 띄우기");
            Item("Ctrl + Shift + .", "구분선  *   *   *");
            Item("Ctrl + .", "말줄임표  ⋯");
            Item("Ctrl + [", "겹낫표  「」  (커서는 괄호 안)");
            Item("Ctrl + '", "큰따옴표  \"\"  (커서는 따옴표 안)");
            Item("Ctrl + Shift + '", "작은따옴표  ''  (커서는 따옴표 안)");
            Sub("바꿀 수 없는 기본 단축키");
            Item("Ctrl + S / Ctrl + Shift + S", "저장 / 다른 이름으로 저장");
            Item("Ctrl + O / Ctrl + N", "열기 / 새 문서");
            Item("Ctrl + B", "사이드 패널 열기·닫기");
            Item("Ctrl + F", "문서 내 검색");
            Item("F1", "이 설명서 열기");
            Item("Ctrl + C · X · V · Z · Y · A", "복사 · 잘라내기 · 붙여넣기 · 실행 취소 · 다시 실행 · 모두 선택");
            Sub("나만의 단축키 만들기");
            Bullet("설정 → 단축키 → '＋ 추가'를 누릅니다.");
            Bullet("기능 이름을 쓰고, 단축키 칸을 누른 뒤 원하는 키 조합을 누릅니다. (Ctrl 또는 Alt가 들어간 조합, 또는 F2~F12)");
            Bullet("입력할 내용을 씁니다. 여러 줄도 됩니다. → 저장 → 설정 창의 '저장'");
            Note("이미 쓰는 키 조합과 겹치면 저장할 수 없습니다. 단축키 칸에서 Backspace는 지우기, Tab은 다음 칸입니다.");
            Note("목록에서 두 번 클릭하거나 Enter로 수정, Delete로 삭제합니다. '기본값 복원'은 기본 단축키 6개로 되돌립니다.");
            Note("설정 → 글쓰기에서 '글쓰기 단축키 사용'을 끄면 글쓰기 단축키만 멈춥니다.");

            // 7
            Section("7. 맞춤법 검사");
            Para("글을 쓰다 잠깐 멈추면, 틀린 것으로 의심되는 단어 아래에 빨간 물결선이 생깁니다. " +
                 "물결선은 화면에만 그려지고 문서에는 저장되지 않습니다.");
            Sub("물결선 단어 우클릭");
            Item("추천 단어", "고르면 그 단어로 바뀝니다. 띄어쓰기를 바꾸는 추천에는 '(띄어쓰기)'가 붙습니다.");
            Item("사전에 추가", "맞는 말로 기억합니다. 다음에 실행해도 유지됩니다.");
            Item("무시", "프로그램을 끌 때까지만 표시하지 않습니다.");
            Sub("사용자 사전");
            Para("설정 → 글쓰기 → '사용자 사전...'에서 추가한 단어를 보고, 새로 넣거나 지울 수 있습니다. " +
                 "'확인'을 누르면 본문 물결선에 바로 반영됩니다.");
            Note("등장인물 이름을 사전에 넣으면 '민수가', '민수에게'처럼 조사가 붙어도 통과합니다.");
            Sub("알아 둘 점");
            Bullet("지금 입력 중인 단어는 1.5초 동안 멈추면 표시됩니다.");
            Bullet("영어와 숫자가 섞인 말(예: 3월)은 검사하지 않습니다.");
            Bullet("단어 단위 검사라서 문맥이 필요한 오류(되요/돼요, 바램/바람)는 잡지 못하고, 띄어쓰기는 일부만 잡습니다.");
            Bullet("설정 → 글쓰기에서 '맞춤법 검사'를 끌 수 있습니다.");
            Note("한국어 사전: hunspell-dict-ko 0.7.94 (GPL-3.0)");

            // 8
            Section("8. 문서 내 검색과 붙여넣기");
            Sub("문서 내 검색 (Ctrl + F)");
            Item("Enter", "다음 찾기");
            Item("Shift + Enter", "이전 찾기");
            Item("Esc 또는 ✕", "검색 닫기");
            Sub("붙여넣기");
            Item("서식 빼고 글자만 넣기 (켜짐)", "웹·워드에서 복사한 글을 본문 글꼴과 색으로 붙여 넣습니다.");
            Item("서식 빼고 글자만 넣기 (꺼짐)", "굵게 등 서식은 유지하되, 글자색은 테마 색으로 맞춥니다.");

            // 9
            Section("9. 설정");
            Para("사이드 패널 아래 '설정' 버튼으로 엽니다. 바꾼 뒤 '저장'을 누르면 다시 실행하지 않아도 바로 적용됩니다.");
            Sub("일반");
            Item("시작할 때 화면", "새 문서로 시작 / 마지막으로 작업한 문서 열기");
            Item("시작할 때 사이드 패널 열기", "켜면 프로그램을 시작할 때 사이드 패널이 열려 있습니다.");
            Sub("단축키");
            Item("단축키 목록", "글쓰기 단축키 추가 · 수정 · 삭제 (6장 참고)");
            Sub("글쓰기");
            Item("자동 저장", "켜기·끄기, 간격 선택");
            Item("글자 수 기준", "공백 포함 / 공백 제외");
            Item("붙여넣을 때 서식 빼고 글자만 넣기", "8장 참고");
            Item("글쓰기 단축키 사용", "글쓰기 단축키 전체 켜기·끄기");
            Item("맞춤법 검사 · 사용자 사전", "7장 참고");
            Sub("저장 및 문서");
            Item("문서 저장 폴더", "찾아보기로 바꾸거나, '기본 위치로'(내 문서\\WriteNote) 되돌리거나, '폴더 열기'로 탐색기에서 엽니다.");
            Note("폴더를 바꾸면 이전 폴더의 문서(하위 폴더 포함)를 새 폴더로 옮길지 묻습니다. 이름이 겹치는 문서는 옮기지 않습니다.");
            Sub("화면 및 테마");
            Item("테마", "어두운 테마 / 밝은 테마 (프로그램 전체)");
            Item("본문 글꼴 · 본문 글자 크기 · 제목 글자 크기", "미리보기 칸에서 바로 확인할 수 있습니다.");
            Sub("공통 버튼");
            Item("저장", "바꾼 설정을 저장하고 적용합니다.");
            Item("취소 (Esc)", "바꾼 내용을 버립니다. 바꾼 게 있으면 한 번 묻습니다.");
            Item("기본값 복원", "지금 보고 있는 페이지만 처음 값으로 되돌립니다. ('저장'을 눌러야 적용)");
            Item("설명서", "이 설명서를 엽니다.");

            // 10
            Section("10. 데이터 저장 위치와 백업");
            Item("문서 (.rtf + .json)", "내 문서\\WriteNote (설정에서 변경 가능), 하위 폴더 = 분류");
            Item("설정", "%AppData%\\WriteNote\\settings.json");
            Item("글쓰기 단축키", "%AppData%\\WriteNote\\shortcuts.json");
            Item("최근 문서 목록", "%AppData%\\WriteNote\\recent_documents.json");
            Item("휴지통", "%AppData%\\WriteNote\\Trash");
            Item("맞춤법 사용자 사전", "%AppData%\\WriteNote\\user_dictionary.txt");
            Note("프로그램 자체는 WriteNote.exe 파일 하나뿐이라, 다른 컴퓨터로 옮길 때는 exe만 복사하면 됩니다.");
            Sub("백업하려면");
            Bullet("문서 폴더와 %AppData%\\WriteNote 폴더를 통째로 복사해 두면 됩니다.");
            Bullet("탐색기 주소창에 %AppData%\\WriteNote를 붙여 넣으면 바로 그 폴더가 열립니다.");
            Note("설정이나 단축키 파일이 손상되면 *.broken.json으로 옮겨 두고 기본값으로 시작합니다.");

            // 11
            Section("11. 문제 해결");
            Sub("빨간 물결선이 안 보여요");
            Bullet("설정 → 글쓰기 → '맞춤법 검사'가 켜져 있는지 확인하세요.");
            Bullet("입력 중인 단어는 1.5초 멈추면 표시됩니다.");
            Bullet("되요/돼요처럼 문맥이 필요한 오류는 원래 잡지 못합니다.");
            Bullet("맞춤법 사전은 WriteNote.exe 안에 들어 있습니다. 시작할 때 사전을 불러오지 못했다는 안내가 나오면 WriteNote.exe를 다시 받아 주세요.");
            Sub("문서가 목록에 안 보여요");
            Bullet("검색 칸이 비어 있는지, 폴더 선택 상자가 '모든 폴더'인지 확인하세요.");
            Bullet("문서 폴더를 바꿨다면 이전 폴더의 문서는 '전체 문서'에 보이지 않습니다. 열기(Ctrl + O)나 최근 문서로 열 수 있습니다.");
            Bullet("삭제한 문서라면 '휴지통'에 있습니다.");
            Sub("단축키가 동작하지 않아요");
            Bullet("설정 → 글쓰기 → '글쓰기 단축키 사용'이 켜져 있는지 확인하세요.");
            Bullet("설정 → 단축키 목록에 그 단축키가 있는지 확인하세요.");
            Sub("붙여 넣은 글의 모양이 이상해요");
            Bullet("설정 → 글쓰기 → '붙여넣을 때 서식 빼고 글자만 넣기'를 켜세요.");
            Sub("설정을 처음 상태로 되돌리고 싶어요");
            Bullet("설정 창에서 그 페이지를 열고 '기본값 복원' → '저장'을 누르세요.");

            // 12
            Section("12. 프로그램 정보");
            Item("버전", "WriteNote 1.0 (C# Windows Forms, .NET Framework 4.7.2)");
            Item("배포 형태", "WriteNote.exe 파일 하나. 필요한 구성 요소와 맞춤법 사전이 exe 안에 들어 있습니다.");
            Item("실행 조건", ".NET Framework 4.7.2 이상 (Windows 10/11에는 기본으로 들어 있음)");
            Sub("사용한 외부 구성 요소");
            Item("Newtonsoft.Json 13.0.4", "MIT");
            Item("WeCantSpell.Hunspell 7.0.1", "MPL 1.1 / GPL 2.0 / LGPL 2.1");
            Item("hunspell-dict-ko 0.7.94 (한국어 맞춤법 사전)", "GPL-3.0 — https://github.com/spellcheck-ko/hunspell-dict-ko");
            Item("System.Memory, System.Buffers, System.Numerics.Vectors, System.Runtime.CompilerServices.Unsafe", "Microsoft, MIT");
            Note("각 구성 요소의 라이선스 전문은 13장에 있습니다.");
            Sub("원본 소스 (모두 수정하지 않고 그대로 사용)");
            Item("hunspell-dict-ko 0.7.94 (GPL-3.0)", "https://github.com/spellcheck-ko/hunspell-dict-ko/tree/0.7.94");
            Item("WeCantSpell.Hunspell 7.0.1 (MPL 1.1 조건으로 사용)", "https://github.com/aarondandy/WeCantSpell.Hunspell/tree/7.0.1");
            Item("Newtonsoft.Json", "https://github.com/JamesNK/Newtonsoft.Json");
            Item("Microsoft 보조 구성 요소", "https://github.com/dotnet/corefx, https://github.com/dotnet/runtime");
            Note("맞춤법 사전은 WriteNote가 실행할 때 데이터로 읽어 쓰는 별개의 저작물이며, GPL-3.0에 따라 누구나 복사·수정·재배포할 수 있습니다.");
            Note("자세한 고지: 저장소의 THIRD-PARTY-NOTICES.md (https://github.com/WhiteHorang/WriteNote)");

            // 13 - exe 안에 넣어 둔 라이선스 문서
            Section("13. 라이선스 전문");
            License("hunspell-dict-ko (한국어 맞춤법 사전)", "WriteNote.Licenses.hunspell-dict-ko-LICENSE.md");
            License("WeCantSpell.Hunspell", "WriteNote.Licenses.WeCantSpell.Hunspell-LICENSE.txt");
            License("Newtonsoft.Json", "WriteNote.Licenses.Newtonsoft.Json-LICENSE.md");
            License("System.Memory · System.Buffers · System.Numerics.Vectors · System.Runtime.CompilerServices.Unsafe",
                    "WriteNote.Licenses.Microsoft-dotnet-LICENSE.txt");
            License("GNU General Public License v3.0 (한국어 맞춤법 사전에 적용)", "WriteNote.Licenses.GPL-3.0.txt");

            // 맨 위에서 시작
            rtbHelp.Select(0, 0);
            rtbHelp.ScrollToCaret();
        }
    }
}
