using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WriteNote
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();

            // Esc를 누르면 '취소'와 같게 닫는다. (저장하지 않은 변경이 있으면 확인)
            CancelButton = btnCancelSettings;

            btnGeneral.Click += (s, e) => ShowSettingsPage("general");
            btnShortcuts.Click += (s, e) => ShowSettingsPage("shortcuts");
            btnWriting.Click += (s, e) => ShowSettingsPage("writing");
            btnStorage.Click += (s, e) => ShowSettingsPage("storage");
            btnAppearance.Click += (s, e) => ShowSettingsPage("appearance");

            // 설명서: 큰 창으로 따로 연다. (설정 창의 저장·취소와 관계없음)
            btnHelp.Click += (s, e) =>
            {
                using (HelpForm help = new HelpForm())
                {
                    help.ShowDialog(this);
                }
            };

            // 단축키 페이지
            btnAddShortcut.Click += (s, e) => AddShortcut();
            btnEditShortcut.Click += (s, e) => EditSelectedShortcut();
            btnDeleteShortcut.Click += (s, e) => DeleteSelectedShortcut();
            lvShortcuts.DoubleClick += (s, e) => EditSelectedShortcut();
            lvShortcuts.KeyDown += lvShortcuts_KeyDown;
            lvShortcuts.SelectedIndexChanged += (s, e) => UpdateShortcutButtons();
            lvShortcuts.ShowItemToolTips = true;

            // 하단 저장·취소·기본값 복원
            btnApplySettings.Click += btnApplySettings_Click;
            btnCancelSettings.Click += (s, e) => Close();
            btnResetSettings.Click += btnResetSettings_Click;
            FormClosing += SettingsForm_FormClosing;

            // 저장 및 문서 페이지
            btnBrowseDocumentFolder.Click += btnBrowseDocumentFolder_Click;
            btnDefaultDocumentFolder.Click += btnDefaultDocumentFolder_Click;
            btnOpenDocumentFolder.Click += btnOpenDocumentFolder_Click;

            // 글쓰기 페이지
            foreach (int minutes in WritingSettings.AutoSaveIntervals)
                cboAutoSaveInterval.Items.Add(minutes + "분마다");

            cboCharCountMode.Items.Add("공백 포함");
            cboCharCountMode.Items.Add("공백 제외");

            chkAutoSave.CheckedChanged += (s, e) => OnWritingControlChanged();
            cboAutoSaveInterval.SelectedIndexChanged += (s, e) => OnWritingControlChanged();
            cboCharCountMode.SelectedIndexChanged += (s, e) => OnWritingControlChanged();
            chkPastePlainText.CheckedChanged += (s, e) => OnWritingControlChanged();
            chkWritingShortcuts.CheckedChanged += (s, e) => OnWritingControlChanged();
            chkSpellCheck.CheckedChanged += (s, e) => OnWritingControlChanged();

            // 사용자 사전 관리 창 (바로 저장되며, 설정 창의 저장·취소와는 별개)
            btnUserDictionary.Click += (s, e) =>
            {
                using (UserDictionaryForm form = new UserDictionaryForm())
                {
                    form.ShowDialog(this);
                }
            };

            // 일반 페이지
            rdoStartNewDocument.CheckedChanged += (s, e) => OnGeneralControlChanged();
            rdoStartLastDocument.CheckedChanged += (s, e) => OnGeneralControlChanged();
            chkOpenSidebarOnStartup.CheckedChanged += (s, e) => OnGeneralControlChanged();

            // 화면 및 테마 페이지
            cboTheme.Items.Add("어두운 테마");
            cboTheme.Items.Add("밝은 테마");
            LoadFontList();

            cboTheme.SelectedIndexChanged += (s, e) => OnAppearanceControlChanged();
            cboContentFont.SelectedIndexChanged += (s, e) => OnAppearanceControlChanged();
            nudContentFontSize.ValueChanged += (s, e) => OnAppearanceControlChanged();
            nudTitleFontSize.ValueChanged += (s, e) => OnAppearanceControlChanged();

            settings = SettingsManager.Load();
            LoadShortcuts();
            ShowStorageSettings();
            ShowWritingSettings();
            ShowAppearanceSettings();
            ShowGeneralSettings();

            // 디자이너의 어두운 테마 색을 현재 테마로 바꾼다.
            ThemeManager.Apply(this, AppTheme.Dark);

            ShowSettingsPage("general");
        }

        // 설정 창에서 편집 중인 설정 (저장 버튼을 눌러야 파일에 반영된다)
        private AppSettings settings;

        // 현재 보고 있는 설정 페이지
        private string currentPage = "general";

        // 현재 페이지의 설정을 기본값으로 되돌린다.
        private void btnResetSettings_Click(object sender, EventArgs e)
        {
            string message;

            switch (currentPage)
            {
                case "shortcuts":
                    message = "글쓰기 단축키를 기본값으로 되돌리시겠습니까?\n\n" +
                              "직접 추가하거나 수정한 단축키는 모두 사라집니다.";
                    break;
                default:
                    message = "'" + lblSettingsTitle.Text + "' 항목을 기본값으로 되돌리시겠습니까?";
                    break;
            }

            DialogResult result = MessageBox.Show(message, "기본값 복원",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            AppSettings defaults = new AppSettings();

            switch (currentPage)
            {
                case "general":
                    settings.General = defaults.General;
                    ShowGeneralSettings();
                    break;
                case "writing":
                    settings.Writing = defaults.Writing;
                    ShowWritingSettings();
                    break;
                case "storage":
                    settings.Storage = defaults.Storage;
                    ShowStorageSettings();
                    break;
                case "appearance":
                    settings.Appearance = defaults.Appearance;
                    ShowAppearanceSettings();
                    break;
                case "shortcuts":
                    shortcuts = ShortcutManager.GetDefaultShortcuts();
                    RefreshShortcutList(null);
                    break;
            }

            // 저장 버튼을 눌러야 파일에 반영된다.
            hasUnsavedChanges = true;
        }

        // ───────────── 일반 설정 ─────────────

        private void ShowGeneralSettings()
        {
            isLoadingControls = true;

            GeneralSettings general = settings.General;

            rdoStartLastDocument.Checked = general.StartupAction == StartupAction.LastDocument;
            rdoStartNewDocument.Checked = general.StartupAction == StartupAction.NewDocument;
            chkOpenSidebarOnStartup.Checked = general.OpenSidebarOnStartup;

            isLoadingControls = false;
        }

        private void OnGeneralControlChanged()
        {
            if (isLoadingControls)
                return;

            GeneralSettings general = settings.General;

            general.StartupAction = rdoStartLastDocument.Checked
                ? StartupAction.LastDocument
                : StartupAction.NewDocument;

            general.OpenSidebarOnStartup = chkOpenSidebarOnStartup.Checked;

            hasUnsavedChanges = true;
        }

        // ───────────── 글쓰기 설정 ─────────────

        // 화면에 값을 채우는 동안에는 '변경됨'으로 처리하지 않는다.
        private bool isLoadingControls = false;

        // 현재 설정값을 화면에 표시
        private void ShowWritingSettings()
        {
            isLoadingControls = true;

            WritingSettings writing = settings.Writing;

            chkAutoSave.Checked = writing.AutoSaveEnabled;
            cboAutoSaveInterval.SelectedIndex =
                Array.IndexOf(WritingSettings.AutoSaveIntervals, writing.AutoSaveIntervalMinutes);
            cboAutoSaveInterval.Enabled = writing.AutoSaveEnabled;

            cboCharCountMode.SelectedIndex =
                writing.CharCountMode == CharCountMode.ExcludeSpaces ? 1 : 0;

            chkPastePlainText.Checked = writing.PastePlainText;
            chkWritingShortcuts.Checked = writing.WritingShortcutsEnabled;
            chkSpellCheck.Checked = writing.SpellCheckEnabled;

            isLoadingControls = false;
        }

        // 화면에서 값을 바꾸면 설정에 반영한다.
        private void OnWritingControlChanged()
        {
            if (isLoadingControls)
                return;

            WritingSettings writing = settings.Writing;

            writing.AutoSaveEnabled = chkAutoSave.Checked;

            if (cboAutoSaveInterval.SelectedIndex >= 0)
                writing.AutoSaveIntervalMinutes =
                    WritingSettings.AutoSaveIntervals[cboAutoSaveInterval.SelectedIndex];

            writing.CharCountMode = cboCharCountMode.SelectedIndex == 1
                ? CharCountMode.ExcludeSpaces
                : CharCountMode.IncludeSpaces;

            writing.PastePlainText = chkPastePlainText.Checked;
            writing.WritingShortcutsEnabled = chkWritingShortcuts.Checked;
            writing.SpellCheckEnabled = chkSpellCheck.Checked;

            // 자동 저장을 끄면 간격은 고를 필요가 없다.
            cboAutoSaveInterval.Enabled = chkAutoSave.Checked;

            hasUnsavedChanges = true;
        }

        // ───────────── 화면 및 테마 설정 ─────────────

        // 설치된 글꼴 목록 채우기
        private void LoadFontList()
        {
            List<string> names = new List<string>();

            using (System.Drawing.Text.InstalledFontCollection fonts =
                new System.Drawing.Text.InstalledFontCollection())
            {
                foreach (FontFamily family in fonts.Families)
                {
                    // 일반(Regular) 모양이 없는 글꼴은 본문에 쓸 수 없다.
                    if (family.IsStyleAvailable(FontStyle.Regular))
                        names.Add(family.Name);
                }
            }

            names.Sort(StringComparer.CurrentCulture);

            cboContentFont.Items.Clear();
            cboContentFont.Items.AddRange(names.ToArray());
        }

        private void ShowAppearanceSettings()
        {
            isLoadingControls = true;

            AppearanceSettings appearance = settings.Appearance;

            cboTheme.SelectedIndex = appearance.Theme == AppTheme.Light ? 1 : 0;

            // 저장된 글꼴이 이 컴퓨터에 없으면 목록 맨 위에 넣어 둔다.
            int fontIndex = cboContentFont.FindStringExact(appearance.ContentFontName);
            if (fontIndex < 0)
            {
                cboContentFont.Items.Insert(0, appearance.ContentFontName);
                fontIndex = 0;
            }
            cboContentFont.SelectedIndex = fontIndex;

            nudContentFontSize.Value = ClampToRange(nudContentFontSize, appearance.ContentFontSize);
            nudTitleFontSize.Value = ClampToRange(nudTitleFontSize, appearance.TitleFontSize);

            isLoadingControls = false;

            UpdateFontPreview();
        }

        private static decimal ClampToRange(NumericUpDown control, float value)
        {
            decimal d = (decimal)Math.Round(value);
            if (d < control.Minimum) return control.Minimum;
            if (d > control.Maximum) return control.Maximum;
            return d;
        }

        private void OnAppearanceControlChanged()
        {
            if (isLoadingControls)
                return;

            AppearanceSettings appearance = settings.Appearance;

            appearance.Theme = cboTheme.SelectedIndex == 1 ? AppTheme.Light : AppTheme.Dark;

            if (cboContentFont.SelectedItem != null)
                appearance.ContentFontName = cboContentFont.SelectedItem.ToString();

            appearance.ContentFontSize = (float)nudContentFontSize.Value;
            appearance.TitleFontSize = (float)nudTitleFontSize.Value;

            hasUnsavedChanges = true;

            UpdateFontPreview();
        }

        // 미리보기: 고른 글꼴·크기와 고른 테마의 본문 색으로 보여 준다.
        private void UpdateFontPreview()
        {
            AppearanceSettings appearance = settings.Appearance;

            lblFontPreview.BackColor = ThemeManager.Convert(
                Color.FromArgb(41, 43, 48), AppTheme.Dark, appearance.Theme);
            lblFontPreview.ForeColor = ThemeManager.Convert(
                Color.FromArgb(224, 224, 224), AppTheme.Dark, appearance.Theme);

            try
            {
                // 미리보기 칸에 맞도록 최대 20pt까지만 키운다.
                float size = Math.Min(appearance.ContentFontSize, 20f);
                Font oldFont = lblFontPreview.Font;
                lblFontPreview.Font = new Font(appearance.ContentFontName, size);

                if (oldFont != Font)
                    oldFont.Dispose();
            }
            catch (Exception)
            {
                // 글꼴을 만들 수 없으면 기존 미리보기 유지
            }
        }

        // ───────────── 저장 및 문서 설정 ─────────────

        // 현재 설정값을 화면에 표시
        private void ShowStorageSettings()
        {
            txtDocumentFolder.Text = settings.Storage.EffectiveDocumentFolder;

            // 이미 기본 위치라면 '기본 위치로' 버튼은 누를 필요가 없다.
            btnDefaultDocumentFolder.Enabled = settings.Storage.DocumentFolder.Length > 0;
        }

        // 문서 폴더 지정. 기본 위치와 같으면 빈 값으로 저장해 '기본 위치 사용' 상태로 둔다.
        private void SetDocumentFolder(string folder)
        {
            string newValue = PathHelper.IsSameFolder(folder, StorageSettings.DefaultDocumentFolder) ? "" : folder;

            if (newValue == settings.Storage.DocumentFolder)
                return;

            settings.Storage.DocumentFolder = newValue;
            hasUnsavedChanges = true;
            ShowStorageSettings();
        }

        private void btnBrowseDocumentFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "WriteNote 문서를 저장할 폴더를 선택하세요.";
                dialog.ShowNewFolderButton = true;

                if (System.IO.Directory.Exists(settings.Storage.EffectiveDocumentFolder))
                    dialog.SelectedPath = settings.Storage.EffectiveDocumentFolder;

                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                string folder = dialog.SelectedPath;
                string error = CheckFolderWritable(folder);

                if (error != null)
                {
                    MessageBox.Show(
                        "이 폴더에는 문서를 저장할 수 없습니다.\n\n" + folder + "\n\n" + error,
                        "문서 폴더", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SetDocumentFolder(folder);
            }
        }

        private void btnDefaultDocumentFolder_Click(object sender, EventArgs e)
        {
            SetDocumentFolder(StorageSettings.DefaultDocumentFolder);
        }

        // 탐색기에서 현재 문서 폴더 열기
        private void btnOpenDocumentFolder_Click(object sender, EventArgs e)
        {
            string folder = settings.Storage.EffectiveDocumentFolder;

            try
            {
                System.IO.Directory.CreateDirectory(folder);
                System.Diagnostics.Process.Start("explorer.exe", "\"" + folder + "\"");
            }
            catch (Exception ex)
            {
                MessageBox.Show("폴더를 열 수 없습니다.\n\n" + folder + "\n\n" + ex.Message,
                    "문서 폴더", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // 폴더에 실제로 파일을 쓸 수 있는지 시험한다. 문제가 없으면 null.
        private static string CheckFolderWritable(string folder)
        {
            try
            {
                System.IO.Directory.CreateDirectory(folder);

                string testFile = System.IO.Path.Combine(
                    folder, ".writenote_test_" + Guid.NewGuid().ToString("N"));

                System.IO.File.WriteAllText(testFile, "");
                System.IO.File.Delete(testFile);

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // ───────────── 단축키 설정 ─────────────

        // 설정 창에서 편집 중인 단축키 목록 (저장 버튼을 눌러야 파일에 반영된다)
        private List<ShortcutItem> shortcuts = new List<ShortcutItem>();

        // 저장하지 않은 변경이 있는지 여부
        private bool hasUnsavedChanges = false;

        private void LoadShortcuts()
        {
            shortcuts = ShortcutManager.Load();
            RefreshShortcutList(null);
        }

        // 목록을 다시 그린다. selectId에 해당하는 항목을 선택 상태로 둔다.
        private void RefreshShortcutList(string selectId)
        {
            lvShortcuts.BeginUpdate();
            lvShortcuts.Items.Clear();

            foreach (ShortcutItem item in shortcuts)
            {
                ListViewItem row = new ListViewItem(item.Name);
                row.SubItems.Add(item.KeysText);
                row.SubItems.Add(item.IsDefault ? "기본" : "사용자");
                row.Tag = item;
                row.ToolTipText = "입력할 내용: " + MakePreview(item.Content);

                lvShortcuts.Items.Add(row);

                if (item.Id == selectId)
                {
                    row.Selected = true;
                    row.Focused = true;
                }
            }

            lvShortcuts.EndUpdate();

            if (lvShortcuts.SelectedItems.Count > 0)
                lvShortcuts.SelectedItems[0].EnsureVisible();

            UpdateShortcutButtons();
        }

        // 툴팁에 보여 줄 내용 (줄바꿈은 ↵로 표시)
        private static string MakePreview(string content)
        {
            string text = (content ?? "").Replace("\r\n", "↵").Replace("\n", "↵");
            return text.Length > 40 ? text.Substring(0, 40) + "⋯" : text;
        }

        private ShortcutItem SelectedShortcut
        {
            get
            {
                if (lvShortcuts.SelectedItems.Count == 0)
                    return null;

                return lvShortcuts.SelectedItems[0].Tag as ShortcutItem;
            }
        }

        // 선택된 항목이 있을 때만 수정·삭제 버튼을 사용할 수 있다.
        private void UpdateShortcutButtons()
        {
            bool hasSelection = SelectedShortcut != null;
            btnEditShortcut.Enabled = hasSelection;
            btnDeleteShortcut.Enabled = hasSelection;
        }

        private void AddShortcut()
        {
            using (ShortcutEditForm form = new ShortcutEditForm(shortcuts))
            {
                if (form.ShowDialog(this) != DialogResult.OK)
                    return;

                ShortcutItem item = form.ToShortcutItem();
                shortcuts.Add(item);
                hasUnsavedChanges = true;

                RefreshShortcutList(item.Id);
            }
        }

        private void EditSelectedShortcut()
        {
            ShortcutItem selected = SelectedShortcut;
            if (selected == null)
                return;

            using (ShortcutEditForm form = new ShortcutEditForm(shortcuts, selected))
            {
                if (form.ShowDialog(this) != DialogResult.OK)
                    return;

                int index = shortcuts.IndexOf(selected);
                shortcuts[index] = form.ToShortcutItem();
                hasUnsavedChanges = true;

                RefreshShortcutList(selected.Id);
            }
        }

        private void DeleteSelectedShortcut()
        {
            ShortcutItem selected = SelectedShortcut;
            if (selected == null)
                return;

            string message = "'" + selected.Name + "' (" + selected.KeysText + ") 단축키를 삭제하시겠습니까?";

            if (selected.IsDefault)
                message += "\n\nWriteNote가 기본으로 제공하는 단축키입니다.";

            DialogResult result = MessageBox.Show(message, "단축키 삭제",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            // 삭제 후 같은 위치의 항목을 선택해 연속 작업을 편하게 한다.
            int index = shortcuts.IndexOf(selected);
            shortcuts.Remove(selected);
            hasUnsavedChanges = true;

            string nextId = null;
            if (shortcuts.Count > 0)
                nextId = shortcuts[Math.Min(index, shortcuts.Count - 1)].Id;

            RefreshShortcutList(nextId);
        }

        private void lvShortcuts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteSelectedShortcut();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                EditSelectedShortcut();
                e.Handled = true;
            }
        }

        // ───────────── 저장 / 취소 ─────────────

        private void btnApplySettings_Click(object sender, EventArgs e)
        {
            try
            {
                SettingsManager.Save(settings);
                settingsFileChanged = true;

                ShortcutManager.Save(shortcuts);
            }
            catch (Exception ex)
            {
                MessageBox.Show("설정을 저장하는 중 오류가 발생했습니다.\n\n" + ex.Message,
                    "설정 저장 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            hasUnsavedChanges = false;
            DialogResult = DialogResult.OK;
            Close();
        }

        // settings.json은 저장했지만 다른 파일 저장에 실패한 경우 true
        // (이때 창을 닫아도 메인 화면이 바뀐 설정을 다시 읽도록 한다)
        private bool settingsFileChanged = false;

        // 저장하지 않고 닫으려 하면 확인한다. (취소 버튼, X 버튼, Esc 모두 해당)
        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (hasUnsavedChanges)
            {
                DialogResult result = MessageBox.Show(
                    "저장하지 않은 변경 사항이 있습니다.\n저장하지 않고 닫으시겠습니까?",
                    "설정", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }

            if (settingsFileChanged)
                DialogResult = DialogResult.OK;
        }

        private void ShowSettingsPage(string page)
        {
            currentPage = page;

            // 설정 화면 표시
            pnlGeneralSettings.Visible = page == "general";
            pnlShortcutSettings.Visible = page == "shortcuts";
            pnlWritingSettings.Visible = page == "writing";
            pnlStorageSettings.Visible = page == "storage";
            pnlAppearanceSettings.Visible = page == "appearance";

            // 제목 및 설명 변경
            switch (page)
            {
                case "general":
                    lblSettingsTitle.Text = "일반 설정";
                    lblSettingsDescription.Text = "WriteNote의 기본 동작을 설정합니다.";
                    break;

                case "shortcuts":
                    lblSettingsTitle.Text = "단축키 설정";
                    lblSettingsDescription.Text = "키보드 단축키를 변경합니다.";
                    break;

                case "writing":
                    lblSettingsTitle.Text = "글쓰기 설정";
                    lblSettingsDescription.Text = "문서 작성 환경과 편의 기능을 설정합니다.";
                    break;

                case "storage":
                    lblSettingsTitle.Text = "저장 및 문서";
                    lblSettingsDescription.Text = "문서의 저장 위치와 관리 방식을 설정합니다.";
                    break;

                case "appearance":
                    lblSettingsTitle.Text = "화면 및 테마";
                    lblSettingsDescription.Text = "화면과 글꼴 환경을 설정합니다.";
                    break;
            }

            // 메뉴 버튼 기본 색상
            System.Drawing.Color normalColor =
                ThemeManager.C(System.Drawing.Color.FromArgb(35, 35, 38));

            System.Drawing.Color normalTextColor =
                ThemeManager.C(System.Drawing.Color.White);

            // 선택된 메뉴는 강조색 배경에 흰 글자 (두 테마 공통)
            System.Drawing.Color selectedColor = ThemeManager.Accent;
            System.Drawing.Color selectedTextColor = System.Drawing.Color.White;

            Button[] menuButtons = { btnGeneral, btnShortcuts, btnWriting, btnStorage, btnAppearance };
            string[] menuPages = { "general", "shortcuts", "writing", "storage", "appearance" };

            for (int i = 0; i < menuButtons.Length; i++)
            {
                bool isSelected = menuPages[i] == page;

                menuButtons[i].BackColor = isSelected ? selectedColor : normalColor;
                menuButtons[i].ForeColor = isSelected ? selectedTextColor : normalTextColor;
            }
        }
    }
}
