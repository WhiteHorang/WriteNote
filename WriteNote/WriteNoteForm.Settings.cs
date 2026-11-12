using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WriteNote
{
    // 설정 창 열기와 설정 적용: 시작 동작, 글쓰기(자동 저장 포함), 테마·글꼴
    public partial class MainForm
    {
        // 일반 설정: 프로그램 시작 시 동작
        private void ApplyStartupSettings()
        {
            GeneralSettings general = appSettings.General;

            if (general.OpenSidebarOnStartup && !isSidebarOpen)
            {
                btnToggleSidebar_Click(btnToggleSidebar, EventArgs.Empty);
            }

            if (general.StartupAction == StartupAction.LastDocument)
            {
                // 최근 문서 목록의 첫 번째(가장 최근) 문서 중 실제로 있는 문서를 연다.
                string lastDocument = null;

                try
                {
                    lastDocument = RecentDocumentManager.GetRecentDocuments()
                        .FirstOrDefault(path => File.Exists(path));
                }
                catch (Exception)
                {
                    // 최근 문서 목록을 읽지 못하면 새 문서로 시작
                }

                if (lastDocument != null)
                {
                    LoadDocument(lastDocument);
                }
            }
        }


        // ───────────── 화면 및 테마 ─────────────

        // 테마와 글꼴 설정을 화면에 적용한다.
        // previousTheme: 지금 화면에 칠해져 있는 테마 (처음 실행 시에는 디자이너 색인 Dark)
        private void ApplyAppearanceSettings(AppTheme previousTheme)
        {
            AppearanceSettings appearance = appSettings.Appearance;

            // 색과 글꼴을 바꾸는 동안 '수정됨'으로 바뀌지 않도록 상태를 보관한다.
            bool wasModified = isModified;
            string saveStatus = lblSaveStatus.Text;

            ThemeManager.SetTheme(appearance.Theme);
            ThemeManager.Apply(this, previousTheme);
            ThemeManager.Apply(cmsRecentDocuments, previousTheme);
            ThemeManager.Apply(cmsTrash, previousTheme);
            ThemeManager.Apply(cmsFolder, previousTheme);

            UpdateCategoryButtonStyle();

            // 제목 글꼴 크기
            try
            {
                Font oldFont = txtTitle.Font;
                txtTitle.Font = new Font(oldFont.FontFamily, appearance.TitleFontSize);
                DisposeIfNotShared(oldFont);
            }
            catch (Exception)
            {
                // 글꼴을 만들 수 없으면 기존 글꼴 유지
            }

            // 본문 글꼴
            try
            {
                Font oldFont = rtbContent.Font;
                rtbContent.Font = new Font(appearance.ContentFontName, appearance.ContentFontSize);
                DisposeIfNotShared(oldFont);
            }
            catch (Exception)
            {
                // 글꼴을 만들 수 없으면 기존 글꼴 유지
            }

            ApplyEditorTextStyle();

            // 글꼴·색 변경은 실행 취소 기록에 남는다.
            // 남겨 두면 Ctrl + Z로 이전 테마의 글자색이 되살아나 글자가 안 보일 수 있다.
            rtbContent.ClearUndo();

            isModified = wasModified;
            lblSaveStatus.Text = saveStatus;
        }


        // 폼 글꼴처럼 다른 컨트롤과 함께 쓰는 글꼴이 아니면 정리한다.
        private void DisposeIfNotShared(Font font)
        {
            if (font != null && font != Font && font != txtTitle.Font && font != rtbContent.Font)
            {
                font.Dispose();
            }
        }


        // 본문 전체 글자를 현재 글꼴과 테마 글자색으로 맞춘다.
        // RTF 문서에는 저장할 때의 글꼴과 글자색이 함께 들어 있으므로,
        // 문서를 불러온 뒤나 테마를 바꾼 뒤에 다시 맞춰 줘야 한다.
        private void ApplyEditorTextStyle()
        {
            bool wasModified = isModified;
            string saveStatus = lblSaveStatus.Text;

            int selectionStart = rtbContent.SelectionStart;
            int selectionLength = rtbContent.SelectionLength;

            rtbContent.SelectAll();
            rtbContent.SelectionFont = rtbContent.Font;
            rtbContent.SelectionColor = rtbContent.ForeColor;
            rtbContent.Select(selectionStart, selectionLength);

            isModified = wasModified;
            lblSaveStatus.Text = saveStatus;
        }


        private void btnSettings_Click(object sender, EventArgs e)
        {
            using (SettingsForm settingsForm = new SettingsForm())
            {
                // 설정을 저장했으면 바뀐 설정과 단축키를 바로 적용한다.
                if (settingsForm.ShowDialog(this) == DialogResult.OK)
                {
                    string oldFolder = appSettings.Storage.EffectiveDocumentFolder;

                    AppTheme previousTheme = ThemeManager.Current;

                    appSettings = SettingsManager.Load();
                    writingShortcuts = ShortcutManager.Load();
                    ApplyWritingSettings();
                    ApplyAppearanceSettings(previousTheme);

                    string newFolder = appSettings.Storage.EffectiveDocumentFolder;

                    // 문서 폴더가 바뀌었으면 기존 문서를 옮길지 묻고 목록을 새로 고친다.
                    if (!PathHelper.IsSameFolder(oldFolder, newFolder))
                    {
                        documentFolderWarningShown = false;
                        OfferToMoveDocuments(oldFolder, newFolder);
                        LoadRecentDocuments();
                    }
                }
            }
        }


        // ───────────── 자동 저장 ─────────────

        private Timer autoSaveTimer;


        [DllImport("user32.dll")]
        private static extern bool IsWindowEnabled(IntPtr hWnd);


        // 글쓰기 설정을 화면에 적용 (프로그램 시작 시, 설정 저장 후)
        private void ApplyWritingSettings()
        {
            if (autoSaveTimer == null)
            {
                autoSaveTimer = new Timer();
                autoSaveTimer.Tick += autoSaveTimer_Tick;
            }

            autoSaveTimer.Stop();

            if (appSettings.Writing.AutoSaveEnabled)
            {
                autoSaveTimer.Interval = appSettings.Writing.AutoSaveIntervalMinutes * 60 * 1000;
                autoSaveTimer.Start();
            }

            UpdateCharCount();

            // 맞춤법 검사 켜기/끄기 (시작할 때는 InitializeSpellCheck 전이라 Load에서 다시 호출됨)
            ApplySpellCheckSetting();
        }


        private void autoSaveTimer_Tick(object sender, EventArgs e)
        {
            // 바뀐 내용이 없거나, 아직 한 번도 저장하지 않은 문서는 건너뛴다.
            if (!isModified || string.IsNullOrEmpty(currentFilePath))
                return;

            // 메시지 창이나 대화 상자가 열려 있는 동안에는 저장하지 않는다.
            // (예: "저장하시겠습니까?"에 '아니요'를 고르려는 중에 저장되는 일을 막는다)
            if (!IsWindowEnabled(Handle))
                return;

            try
            {
                rtbContent.SaveFile(currentFilePath, RichTextBoxStreamType.RichText);
                SaveDocumentInfo(currentFilePath);

                isModified = false;
                lblSaveStatus.Text = "자동 저장됨 " + DateTime.Now.ToString("HH:mm");
            }
            catch (Exception)
            {
                // 자동 저장 실패는 창을 띄우지 않고 상태 표시줄에만 알린다.
                lblSaveStatus.Text = "자동 저장 실패 - 직접 저장해 주세요";
            }
        }
    }
}
