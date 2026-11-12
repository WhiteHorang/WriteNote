using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WriteNote
{
    // 메인 화면 본체: 공통 필드, 생성자(이벤트 연결·초기화), 닫기 확인, 설명서 열기
    // (기능별 코드는 WriteNoteForm.*.cs 파일에 나뉘어 있다)
    public partial class MainForm : Form
    {
        private string currentFilePath = "";

        private bool isModified = false;

        private bool isSidebarOpen = false;


        // 현재 선택된 문서 카드
        private Panel selectedDocumentCard = null;


        // 현재 선택된 문서의 파일 경로
        private string selectedDocumentPath = "";


        // 현재 선택된 문서 분류
        private string currentDocumentCategory = "all";


        // 글쓰기 단축키 목록 (설정 창에서 관리, shortcuts.json에 저장)
        private List<ShortcutItem> writingShortcuts = new List<ShortcutItem>();


        // 프로그램 설정 (설정 창에서 관리, settings.json에 저장)
        private AppSettings appSettings = new AppSettings();


        public MainForm()
        {
            InitializeComponent();

            // 저장된 설정과 글쓰기 단축키 불러오기
            appSettings = SettingsManager.Load();
            writingShortcuts = ShortcutManager.Load();
            ApplyWritingSettings();

            // 테마는 화면 색을 정하기 전에 먼저 지정한다. (실제 색 적용은 생성자 끝에서)
            ThemeManager.SetTheme(appSettings.Appearance.Theme);

            btnSettings.Click += btnSettings_Click;

            // 제목을 바꿔도 '수정됨'으로 표시되도록 연결 (디자이너에 연결이 빠져 있었음)
            txtTitle.TextChanged += txtTitle_TextChanged;

            txtSearchWord.KeyDown += txtSearchWord_KeyDown;

            // 정렬 항목 등록
            cboSortDocuments.Items.Clear();

            cboSortDocuments.Items.Add("최근 수정순");
            cboSortDocuments.Items.Add("오래된 수정순");
            cboSortDocuments.Items.Add("이름순");

            // 첫 번째 정렬 항목 선택
            cboSortDocuments.SelectedIndex = 0;

            LoadRecentDocuments();
            UpdateCategoryButtonStyle();

            pnlRecentDocuments.Visible = false;

            flpDocumentList.Layout += (s, e) =>
            {
                HideDocumentScrollBars();
            };

            // 최근 문서 메뉴 다크 테마 적용
            cmsRecentDocuments.Renderer =
                new ToolStripProfessionalRenderer(new DarkMenuColorTable());

            cmsRecentDocuments.ShowImageMargin = false;
            cmsRecentDocuments.ShowCheckMargin = false;
            cmsRecentDocuments.BackColor = ThemeManager.C(Color.FromArgb(48, 51, 59));
            cmsRecentDocuments.ForeColor = ThemeManager.C(Color.FromArgb(230, 230, 234));

            // 휴지통 카드 우클릭 메뉴
            CreateTrashMenu();

            // 폴더별 문서 분류 (폴더 선택 상자, 폴더 관리 메뉴, 폴더로 이동)
            InitializeFolderFeatures();

            // 맞춤법 검사 (빨간 물결선, 우클릭 추천 단어)
            InitializeSpellCheck();

            // 디자이너에 적힌 어두운 테마 색을 설정한 테마로 바꾸고 글꼴을 적용한다.
            ApplyAppearanceSettings(AppTheme.Dark);

            // 시작 동작은 창이 준비된 뒤에 실행한다.
            Load += (s, e) =>
            {
                ApplyStartupSettings();

                // 맞춤법 사전은 창이 준비된 뒤 뒤에서 불러온다.
                ApplySpellCheckSetting();
            };

            // 저장하지 않은 내용이 있으면 닫기 전에 확인한다.
            FormClosing += MainForm_FormClosing;
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // '취소'를 고르면 창을 닫지 않는다.
            if (!ConfirmSaveChanges())
            {
                e.Cancel = true;
                return;
            }

            if (autoSaveTimer != null)
            {
                autoSaveTimer.Stop();
            }
        }


        // 설명서 창 열기 (F1)
        private void ShowHelp()
        {
            using (HelpForm help = new HelpForm())
            {
                help.ShowDialog(this);
            }
        }
    }
}
