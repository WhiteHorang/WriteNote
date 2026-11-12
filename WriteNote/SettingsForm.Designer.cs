namespace WriteNote
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlSettingsMenu = new System.Windows.Forms.Panel();
            this.btnAppearance = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnStorage = new System.Windows.Forms.Button();
            this.btnWriting = new System.Windows.Forms.Button();
            this.btnShortcuts = new System.Windows.Forms.Button();
            this.btnGeneral = new System.Windows.Forms.Button();
            this.pnlSettingsContent = new System.Windows.Forms.Panel();
            this.pnlShortcutSettings = new System.Windows.Forms.Panel();
            this.lvShortcuts = new System.Windows.Forms.ListView();
            this.lblShortcutSectionDescription = new System.Windows.Forms.Label();
            this.lblShortcutSectionTitle = new System.Windows.Forms.Label();
            this.pnlAppearanceSettings = new System.Windows.Forms.Panel();
            this.lblAppearanceSectionDescription = new System.Windows.Forms.Label();
            this.lblAppearanceSectionTitle = new System.Windows.Forms.Label();
            this.lblTheme = new System.Windows.Forms.Label();
            this.cboTheme = new WriteNote.BorderedComboBox();
            this.lblContentFont = new System.Windows.Forms.Label();
            this.cboContentFont = new WriteNote.BorderedComboBox();
            this.lblContentFontSize = new System.Windows.Forms.Label();
            this.nudContentFontSize = new System.Windows.Forms.NumericUpDown();
            this.lblTitleFontSize = new System.Windows.Forms.Label();
            this.nudTitleFontSize = new System.Windows.Forms.NumericUpDown();
            this.lblFontPreview = new System.Windows.Forms.Label();
            this.pnlStorageSettings = new System.Windows.Forms.Panel();
            this.lblStorageSectionDescription = new System.Windows.Forms.Label();
            this.lblStorageSectionTitle = new System.Windows.Forms.Label();
            this.lblDocumentFolder = new System.Windows.Forms.Label();
            this.txtDocumentFolder = new System.Windows.Forms.TextBox();
            this.btnBrowseDocumentFolder = new System.Windows.Forms.Button();
            this.btnDefaultDocumentFolder = new System.Windows.Forms.Button();
            this.btnOpenDocumentFolder = new System.Windows.Forms.Button();
            this.lblDocumentFolderHint = new System.Windows.Forms.Label();
            this.pnlWritingSettings = new System.Windows.Forms.Panel();
            this.chkAutoSave = new System.Windows.Forms.CheckBox();
            this.cboAutoSaveInterval = new WriteNote.BorderedComboBox();
            this.lblCharCountMode = new System.Windows.Forms.Label();
            this.cboCharCountMode = new WriteNote.BorderedComboBox();
            this.chkPastePlainText = new System.Windows.Forms.CheckBox();
            this.chkWritingShortcuts = new System.Windows.Forms.CheckBox();
            this.chkSpellCheck = new System.Windows.Forms.CheckBox();
            this.btnUserDictionary = new System.Windows.Forms.Button();
            this.lblWritingHint = new System.Windows.Forms.Label();
            this.lblWritingSectionDescription = new System.Windows.Forms.Label();
            this.lblWritingSectionTitle = new System.Windows.Forms.Label();
            this.pnlSettingsPage = new System.Windows.Forms.Panel();
            this.pnlGeneralSettings = new System.Windows.Forms.Panel();
            this.lblStartupDescription = new System.Windows.Forms.Label();
            this.lblStartupTitle = new System.Windows.Forms.Label();
            this.lblStartupAction = new System.Windows.Forms.Label();
            this.rdoStartNewDocument = new System.Windows.Forms.RadioButton();
            this.rdoStartLastDocument = new System.Windows.Forms.RadioButton();
            this.chkOpenSidebarOnStartup = new System.Windows.Forms.CheckBox();
            this.lblGeneralHint = new System.Windows.Forms.Label();
            this.lblSettingsDescription = new System.Windows.Forms.Label();
            this.lblSettingsTitle = new System.Windows.Forms.Label();
            this.pnlSettingsFooter = new System.Windows.Forms.Panel();
            this.btnResetSettings = new System.Windows.Forms.Button();
            this.btnApplySettings = new System.Windows.Forms.Button();
            this.btnCancelSettings = new System.Windows.Forms.Button();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnAddShortcut = new System.Windows.Forms.Button();
            this.btnEditShortcut = new System.Windows.Forms.Button();
            this.btnDeleteShortcut = new System.Windows.Forms.Button();
            this.pnlSettingsMenu.SuspendLayout();
            this.pnlSettingsContent.SuspendLayout();
            this.pnlShortcutSettings.SuspendLayout();
            this.pnlAppearanceSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudContentFontSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTitleFontSize)).BeginInit();
            this.pnlStorageSettings.SuspendLayout();
            this.pnlWritingSettings.SuspendLayout();
            this.pnlSettingsPage.SuspendLayout();
            this.pnlGeneralSettings.SuspendLayout();
            this.pnlSettingsFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSettingsMenu
            // 
            this.pnlSettingsMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            this.pnlSettingsMenu.Controls.Add(this.btnHelp);
            this.pnlSettingsMenu.Controls.Add(this.btnAppearance);
            this.pnlSettingsMenu.Controls.Add(this.btnStorage);
            this.pnlSettingsMenu.Controls.Add(this.btnWriting);
            this.pnlSettingsMenu.Controls.Add(this.btnShortcuts);
            this.pnlSettingsMenu.Controls.Add(this.btnGeneral);
            this.pnlSettingsMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSettingsMenu.Location = new System.Drawing.Point(0, 0);
            this.pnlSettingsMenu.Name = "pnlSettingsMenu";
            this.pnlSettingsMenu.Size = new System.Drawing.Size(170, 544);
            this.pnlSettingsMenu.TabIndex = 0;
            // 
            // btnAppearance
            // 
            this.btnAppearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(38)))));
            this.btnAppearance.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAppearance.FlatAppearance.BorderSize = 0;
            this.btnAppearance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAppearance.ForeColor = System.Drawing.Color.White;
            this.btnAppearance.Location = new System.Drawing.Point(10, 240);
            this.btnAppearance.Name = "btnAppearance";
            this.btnAppearance.Size = new System.Drawing.Size(150, 45);
            this.btnAppearance.TabIndex = 4;
            this.btnAppearance.Text = "화면 및 테마";
            this.btnAppearance.UseVisualStyleBackColor = false;
            // 
            // btnHelp
            // 
            this.btnHelp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(38)))));
            this.btnHelp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHelp.FlatAppearance.BorderSize = 0;
            this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHelp.ForeColor = System.Drawing.Color.White;
            this.btnHelp.Location = new System.Drawing.Point(10, 295);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(150, 45);
            this.btnHelp.TabIndex = 5;
            this.btnHelp.Text = "설명서";
            this.btnHelp.UseVisualStyleBackColor = false;
            // 
            // btnStorage
            // 
            this.btnStorage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(38)))));
            this.btnStorage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStorage.FlatAppearance.BorderSize = 0;
            this.btnStorage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStorage.ForeColor = System.Drawing.Color.White;
            this.btnStorage.Location = new System.Drawing.Point(10, 185);
            this.btnStorage.Name = "btnStorage";
            this.btnStorage.Size = new System.Drawing.Size(150, 45);
            this.btnStorage.TabIndex = 3;
            this.btnStorage.Text = "저장 및 문서";
            this.btnStorage.UseVisualStyleBackColor = false;
            // 
            // btnWriting
            // 
            this.btnWriting.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(38)))));
            this.btnWriting.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWriting.FlatAppearance.BorderSize = 0;
            this.btnWriting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWriting.ForeColor = System.Drawing.Color.White;
            this.btnWriting.Location = new System.Drawing.Point(10, 130);
            this.btnWriting.Name = "btnWriting";
            this.btnWriting.Size = new System.Drawing.Size(150, 45);
            this.btnWriting.TabIndex = 2;
            this.btnWriting.Text = "글쓰기";
            this.btnWriting.UseVisualStyleBackColor = false;
            // 
            // btnShortcuts
            // 
            this.btnShortcuts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(38)))));
            this.btnShortcuts.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnShortcuts.FlatAppearance.BorderSize = 0;
            this.btnShortcuts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShortcuts.ForeColor = System.Drawing.Color.White;
            this.btnShortcuts.Location = new System.Drawing.Point(10, 75);
            this.btnShortcuts.Name = "btnShortcuts";
            this.btnShortcuts.Size = new System.Drawing.Size(150, 45);
            this.btnShortcuts.TabIndex = 1;
            this.btnShortcuts.Text = "단축키";
            this.btnShortcuts.UseVisualStyleBackColor = false;
            // 
            // btnGeneral
            // 
            this.btnGeneral.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(38)))));
            this.btnGeneral.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGeneral.FlatAppearance.BorderSize = 0;
            this.btnGeneral.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGeneral.ForeColor = System.Drawing.Color.White;
            this.btnGeneral.Location = new System.Drawing.Point(10, 20);
            this.btnGeneral.Name = "btnGeneral";
            this.btnGeneral.Size = new System.Drawing.Size(150, 45);
            this.btnGeneral.TabIndex = 0;
            this.btnGeneral.Text = "일반";
            this.btnGeneral.UseVisualStyleBackColor = false;
            // 
            // pnlSettingsContent
            // 
            this.pnlSettingsContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.pnlSettingsContent.Controls.Add(this.pnlShortcutSettings);
            this.pnlSettingsContent.Controls.Add(this.pnlAppearanceSettings);
            this.pnlSettingsContent.Controls.Add(this.pnlStorageSettings);
            this.pnlSettingsContent.Controls.Add(this.pnlWritingSettings);
            this.pnlSettingsContent.Controls.Add(this.pnlSettingsPage);
            this.pnlSettingsContent.Controls.Add(this.pnlSettingsFooter);
            this.pnlSettingsContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSettingsContent.Location = new System.Drawing.Point(170, 0);
            this.pnlSettingsContent.Name = "pnlSettingsContent";
            this.pnlSettingsContent.Size = new System.Drawing.Size(508, 544);
            this.pnlSettingsContent.TabIndex = 1;
            // 
            // pnlShortcutSettings
            // 
            this.pnlShortcutSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            this.pnlShortcutSettings.Controls.Add(this.btnDeleteShortcut);
            this.pnlShortcutSettings.Controls.Add(this.btnEditShortcut);
            this.pnlShortcutSettings.Controls.Add(this.btnAddShortcut);
            this.pnlShortcutSettings.Controls.Add(this.lvShortcuts);
            this.pnlShortcutSettings.Controls.Add(this.lblShortcutSectionDescription);
            this.pnlShortcutSettings.Controls.Add(this.lblShortcutSectionTitle);
            this.pnlShortcutSettings.Location = new System.Drawing.Point(25, 110);
            this.pnlShortcutSettings.Name = "pnlShortcutSettings";
            this.pnlShortcutSettings.Size = new System.Drawing.Size(480, 350);
            this.pnlShortcutSettings.TabIndex = 2;
            this.pnlShortcutSettings.Visible = false;
            // 
            // lvShortcuts
            // 
            this.lvShortcuts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.lvShortcuts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvShortcuts.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.lvShortcuts.FullRowSelect = true;
            this.lvShortcuts.GridLines = true;
            this.lvShortcuts.HideSelection = false;
            this.lvShortcuts.Location = new System.Drawing.Point(15, 90);
            this.lvShortcuts.MultiSelect = false;
            this.lvShortcuts.Name = "lvShortcuts";
            this.lvShortcuts.Size = new System.Drawing.Size(450, 190);
            this.lvShortcuts.TabIndex = 2;
            this.lvShortcuts.UseCompatibleStateImageBehavior = false;
            this.lvShortcuts.View = System.Windows.Forms.View.Details;
            // 
            // lblShortcutSectionDescription
            // 
            this.lblShortcutSectionDescription.AutoSize = true;
            this.lblShortcutSectionDescription.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblShortcutSectionDescription.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblShortcutSectionDescription.Location = new System.Drawing.Point(20, 50);
            this.lblShortcutSectionDescription.Name = "lblShortcutSectionDescription";
            this.lblShortcutSectionDescription.Size = new System.Drawing.Size(436, 25);
            this.lblShortcutSectionDescription.TabIndex = 1;
            this.lblShortcutSectionDescription.Text = "원하는 키 조합을 눌러 단축키를 변경할 수 있습니다.";
            // 
            // lblShortcutSectionTitle
            // 
            this.lblShortcutSectionTitle.AutoSize = true;
            this.lblShortcutSectionTitle.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblShortcutSectionTitle.Location = new System.Drawing.Point(20, 20);
            this.lblShortcutSectionTitle.Name = "lblShortcutSectionTitle";
            this.lblShortcutSectionTitle.Size = new System.Drawing.Size(139, 28);
            this.lblShortcutSectionTitle.TabIndex = 0;
            this.lblShortcutSectionTitle.Text = "키보드 단축키";
            // 
            // pnlAppearanceSettings
            // 
            this.pnlAppearanceSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            this.pnlAppearanceSettings.Controls.Add(this.lblFontPreview);
            this.pnlAppearanceSettings.Controls.Add(this.nudTitleFontSize);
            this.pnlAppearanceSettings.Controls.Add(this.lblTitleFontSize);
            this.pnlAppearanceSettings.Controls.Add(this.nudContentFontSize);
            this.pnlAppearanceSettings.Controls.Add(this.lblContentFontSize);
            this.pnlAppearanceSettings.Controls.Add(this.cboContentFont);
            this.pnlAppearanceSettings.Controls.Add(this.lblContentFont);
            this.pnlAppearanceSettings.Controls.Add(this.cboTheme);
            this.pnlAppearanceSettings.Controls.Add(this.lblTheme);
            this.pnlAppearanceSettings.Controls.Add(this.lblAppearanceSectionDescription);
            this.pnlAppearanceSettings.Controls.Add(this.lblAppearanceSectionTitle);
            this.pnlAppearanceSettings.Location = new System.Drawing.Point(25, 110);
            this.pnlAppearanceSettings.Name = "pnlAppearanceSettings";
            this.pnlAppearanceSettings.Size = new System.Drawing.Size(480, 350);
            this.pnlAppearanceSettings.TabIndex = 5;
            this.pnlAppearanceSettings.Visible = false;
            // 
            // lblAppearanceSectionDescription
            // 
            this.lblAppearanceSectionDescription.AutoSize = true;
            this.lblAppearanceSectionDescription.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblAppearanceSectionDescription.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblAppearanceSectionDescription.Location = new System.Drawing.Point(20, 50);
            this.lblAppearanceSectionDescription.Name = "lblAppearanceSectionDescription";
            this.lblAppearanceSectionDescription.Size = new System.Drawing.Size(376, 25);
            this.lblAppearanceSectionDescription.TabIndex = 1;
            this.lblAppearanceSectionDescription.Text = "WriteNote의 화면과 글꼴 환경을 설정합니다.";
            // 
            // lblAppearanceSectionTitle
            // 
            this.lblAppearanceSectionTitle.AutoSize = true;
            this.lblAppearanceSectionTitle.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblAppearanceSectionTitle.Location = new System.Drawing.Point(20, 20);
            this.lblAppearanceSectionTitle.Name = "lblAppearanceSectionTitle";
            this.lblAppearanceSectionTitle.Size = new System.Drawing.Size(126, 28);
            this.lblAppearanceSectionTitle.TabIndex = 0;
            this.lblAppearanceSectionTitle.Text = "화면 및 테마";
            //
            // lblTheme
            //
            this.lblTheme.AutoSize = true;
            this.lblTheme.Location = new System.Drawing.Point(20, 98);
            this.lblTheme.Name = "lblTheme";
            this.lblTheme.Size = new System.Drawing.Size(48, 25);
            this.lblTheme.TabIndex = 2;
            this.lblTheme.Text = "테마";
            //
            // cboTheme
            //
            this.cboTheme.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(51)))), ((int)(((byte)(59)))));
            this.cboTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboTheme.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(234)))));
            this.cboTheme.FormattingEnabled = true;
            this.cboTheme.Location = new System.Drawing.Point(200, 95);
            this.cboTheme.Name = "cboTheme";
            this.cboTheme.Size = new System.Drawing.Size(260, 33);
            this.cboTheme.TabIndex = 3;
            //
            // lblContentFont
            //
            this.lblContentFont.AutoSize = true;
            this.lblContentFont.Location = new System.Drawing.Point(20, 145);
            this.lblContentFont.Name = "lblContentFont";
            this.lblContentFont.Size = new System.Drawing.Size(94, 25);
            this.lblContentFont.TabIndex = 4;
            this.lblContentFont.Text = "본문 글꼴";
            //
            // cboContentFont
            //
            this.cboContentFont.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(51)))), ((int)(((byte)(59)))));
            this.cboContentFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboContentFont.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboContentFont.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(234)))));
            this.cboContentFont.FormattingEnabled = true;
            this.cboContentFont.Location = new System.Drawing.Point(200, 142);
            this.cboContentFont.MaxDropDownItems = 15;
            this.cboContentFont.Name = "cboContentFont";
            this.cboContentFont.Size = new System.Drawing.Size(260, 33);
            this.cboContentFont.TabIndex = 5;
            //
            // lblContentFontSize
            //
            this.lblContentFontSize.AutoSize = true;
            this.lblContentFontSize.Location = new System.Drawing.Point(20, 192);
            this.lblContentFontSize.Name = "lblContentFontSize";
            this.lblContentFontSize.Size = new System.Drawing.Size(130, 25);
            this.lblContentFontSize.TabIndex = 6;
            this.lblContentFontSize.Text = "본문 글자 크기";
            //
            // nudContentFontSize
            //
            this.nudContentFontSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(51)))), ((int)(((byte)(59)))));
            this.nudContentFontSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudContentFontSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(234)))));
            this.nudContentFontSize.Location = new System.Drawing.Point(200, 190);
            this.nudContentFontSize.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.nudContentFontSize.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.nudContentFontSize.Name = "nudContentFontSize";
            this.nudContentFontSize.Size = new System.Drawing.Size(120, 31);
            this.nudContentFontSize.TabIndex = 7;
            this.nudContentFontSize.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            //
            // lblTitleFontSize
            //
            this.lblTitleFontSize.AutoSize = true;
            this.lblTitleFontSize.Location = new System.Drawing.Point(20, 239);
            this.lblTitleFontSize.Name = "lblTitleFontSize";
            this.lblTitleFontSize.Size = new System.Drawing.Size(130, 25);
            this.lblTitleFontSize.TabIndex = 8;
            this.lblTitleFontSize.Text = "제목 글자 크기";
            //
            // nudTitleFontSize
            //
            this.nudTitleFontSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(51)))), ((int)(((byte)(59)))));
            this.nudTitleFontSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudTitleFontSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(234)))));
            this.nudTitleFontSize.Location = new System.Drawing.Point(200, 237);
            this.nudTitleFontSize.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.nudTitleFontSize.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.nudTitleFontSize.Name = "nudTitleFontSize";
            this.nudTitleFontSize.Size = new System.Drawing.Size(120, 31);
            this.nudTitleFontSize.TabIndex = 9;
            this.nudTitleFontSize.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            //
            // lblFontPreview
            //
            this.lblFontPreview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            this.lblFontPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFontPreview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblFontPreview.Location = new System.Drawing.Point(20, 283);
            this.lblFontPreview.Name = "lblFontPreview";
            this.lblFontPreview.Size = new System.Drawing.Size(440, 55);
            this.lblFontPreview.TabIndex = 10;
            this.lblFontPreview.Text = "가나다라 ABC 123 ⋯ 「미리보기」";
            this.lblFontPreview.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblFontPreview.UseMnemonic = false;
            //
            // pnlStorageSettings
            // 
            this.pnlStorageSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            this.pnlStorageSettings.Controls.Add(this.lblDocumentFolderHint);
            this.pnlStorageSettings.Controls.Add(this.btnOpenDocumentFolder);
            this.pnlStorageSettings.Controls.Add(this.btnDefaultDocumentFolder);
            this.pnlStorageSettings.Controls.Add(this.btnBrowseDocumentFolder);
            this.pnlStorageSettings.Controls.Add(this.txtDocumentFolder);
            this.pnlStorageSettings.Controls.Add(this.lblDocumentFolder);
            this.pnlStorageSettings.Controls.Add(this.lblStorageSectionDescription);
            this.pnlStorageSettings.Controls.Add(this.lblStorageSectionTitle);
            this.pnlStorageSettings.Location = new System.Drawing.Point(25, 110);
            this.pnlStorageSettings.Name = "pnlStorageSettings";
            this.pnlStorageSettings.Size = new System.Drawing.Size(480, 350);
            this.pnlStorageSettings.TabIndex = 4;
            this.pnlStorageSettings.Visible = false;
            // 
            // lblStorageSectionDescription
            // 
            this.lblStorageSectionDescription.AutoSize = true;
            this.lblStorageSectionDescription.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblStorageSectionDescription.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblStorageSectionDescription.Location = new System.Drawing.Point(20, 50);
            this.lblStorageSectionDescription.Name = "lblStorageSectionDescription";
            this.lblStorageSectionDescription.Size = new System.Drawing.Size(370, 25);
            this.lblStorageSectionDescription.TabIndex = 1;
            this.lblStorageSectionDescription.Text = "문서의 저장 위치와 관리 방식을 설정합니다.";
            // 
            // lblStorageSectionTitle
            // 
            this.lblStorageSectionTitle.AutoSize = true;
            this.lblStorageSectionTitle.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblStorageSectionTitle.Location = new System.Drawing.Point(20, 20);
            this.lblStorageSectionTitle.Name = "lblStorageSectionTitle";
            this.lblStorageSectionTitle.Size = new System.Drawing.Size(126, 28);
            this.lblStorageSectionTitle.TabIndex = 0;
            this.lblStorageSectionTitle.Text = "저장 및 문서";
            //
            // lblDocumentFolder
            //
            this.lblDocumentFolder.AutoSize = true;
            this.lblDocumentFolder.Location = new System.Drawing.Point(20, 95);
            this.lblDocumentFolder.Name = "lblDocumentFolder";
            this.lblDocumentFolder.Size = new System.Drawing.Size(130, 25);
            this.lblDocumentFolder.TabIndex = 2;
            this.lblDocumentFolder.Text = "문서 저장 폴더";
            //
            // txtDocumentFolder
            //
            this.txtDocumentFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.txtDocumentFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDocumentFolder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.txtDocumentFolder.Location = new System.Drawing.Point(20, 125);
            this.txtDocumentFolder.Name = "txtDocumentFolder";
            this.txtDocumentFolder.ReadOnly = true;
            this.txtDocumentFolder.Size = new System.Drawing.Size(440, 31);
            this.txtDocumentFolder.TabIndex = 3;
            //
            // btnBrowseDocumentFolder
            //
            this.btnBrowseDocumentFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(85)))), ((int)(((byte)(165)))));
            this.btnBrowseDocumentFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrowseDocumentFolder.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            this.btnBrowseDocumentFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseDocumentFolder.ForeColor = System.Drawing.Color.White;
            this.btnBrowseDocumentFolder.Location = new System.Drawing.Point(20, 170);
            this.btnBrowseDocumentFolder.Name = "btnBrowseDocumentFolder";
            this.btnBrowseDocumentFolder.Size = new System.Drawing.Size(110, 35);
            this.btnBrowseDocumentFolder.TabIndex = 4;
            this.btnBrowseDocumentFolder.Text = "찾아보기";
            this.btnBrowseDocumentFolder.UseVisualStyleBackColor = false;
            //
            // btnDefaultDocumentFolder
            //
            this.btnDefaultDocumentFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(50)))), ((int)(((byte)(58)))));
            this.btnDefaultDocumentFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDefaultDocumentFolder.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            this.btnDefaultDocumentFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDefaultDocumentFolder.ForeColor = System.Drawing.Color.White;
            this.btnDefaultDocumentFolder.Location = new System.Drawing.Point(140, 170);
            this.btnDefaultDocumentFolder.Name = "btnDefaultDocumentFolder";
            this.btnDefaultDocumentFolder.Size = new System.Drawing.Size(130, 35);
            this.btnDefaultDocumentFolder.TabIndex = 5;
            this.btnDefaultDocumentFolder.Text = "기본 위치로";
            this.btnDefaultDocumentFolder.UseVisualStyleBackColor = false;
            //
            // btnOpenDocumentFolder
            //
            this.btnOpenDocumentFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(50)))), ((int)(((byte)(58)))));
            this.btnOpenDocumentFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenDocumentFolder.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            this.btnOpenDocumentFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenDocumentFolder.ForeColor = System.Drawing.Color.White;
            this.btnOpenDocumentFolder.Location = new System.Drawing.Point(280, 170);
            this.btnOpenDocumentFolder.Name = "btnOpenDocumentFolder";
            this.btnOpenDocumentFolder.Size = new System.Drawing.Size(110, 35);
            this.btnOpenDocumentFolder.TabIndex = 6;
            this.btnOpenDocumentFolder.Text = "폴더 열기";
            this.btnOpenDocumentFolder.UseVisualStyleBackColor = false;
            //
            // lblDocumentFolderHint
            //
            this.lblDocumentFolderHint.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblDocumentFolderHint.Location = new System.Drawing.Point(20, 220);
            this.lblDocumentFolderHint.Name = "lblDocumentFolderHint";
            this.lblDocumentFolderHint.Size = new System.Drawing.Size(440, 110);
            this.lblDocumentFolderHint.TabIndex = 7;
            this.lblDocumentFolderHint.Text = "새 문서를 저장할 때 처음 열리는 위치입니다.\r\n사이드 패널의 \'전체 문서\'와 \'즐겨찾기\' 목록도 이 폴더의 문서를 보여 줍니다.\r\n폴더를 바꾸면 기존 문서를 옮길지 물어봅니다.";
            //
            // pnlWritingSettings
            // 
            this.pnlWritingSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            this.pnlWritingSettings.Controls.Add(this.lblWritingHint);
            this.pnlWritingSettings.Controls.Add(this.btnUserDictionary);
            this.pnlWritingSettings.Controls.Add(this.chkSpellCheck);
            this.pnlWritingSettings.Controls.Add(this.chkWritingShortcuts);
            this.pnlWritingSettings.Controls.Add(this.chkPastePlainText);
            this.pnlWritingSettings.Controls.Add(this.cboCharCountMode);
            this.pnlWritingSettings.Controls.Add(this.lblCharCountMode);
            this.pnlWritingSettings.Controls.Add(this.cboAutoSaveInterval);
            this.pnlWritingSettings.Controls.Add(this.chkAutoSave);
            this.pnlWritingSettings.Controls.Add(this.lblWritingSectionDescription);
            this.pnlWritingSettings.Controls.Add(this.lblWritingSectionTitle);
            this.pnlWritingSettings.Location = new System.Drawing.Point(25, 110);
            this.pnlWritingSettings.Name = "pnlWritingSettings";
            this.pnlWritingSettings.Size = new System.Drawing.Size(480, 350);
            this.pnlWritingSettings.TabIndex = 3;
            this.pnlWritingSettings.Visible = false;
            // 
            // lblWritingSectionDescription
            // 
            this.lblWritingSectionDescription.AutoSize = true;
            this.lblWritingSectionDescription.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblWritingSectionDescription.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblWritingSectionDescription.Location = new System.Drawing.Point(20, 50);
            this.lblWritingSectionDescription.Name = "lblWritingSectionDescription";
            this.lblWritingSectionDescription.Size = new System.Drawing.Size(352, 25);
            this.lblWritingSectionDescription.TabIndex = 1;
            this.lblWritingSectionDescription.Text = "문서 작성 환경과 편의 기능을 설정합니다.";
            // 
            // lblWritingSectionTitle
            // 
            this.lblWritingSectionTitle.AutoSize = true;
            this.lblWritingSectionTitle.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblWritingSectionTitle.Location = new System.Drawing.Point(20, 20);
            this.lblWritingSectionTitle.Name = "lblWritingSectionTitle";
            this.lblWritingSectionTitle.Size = new System.Drawing.Size(119, 28);
            this.lblWritingSectionTitle.TabIndex = 0;
            this.lblWritingSectionTitle.Text = "글쓰기 설정";
            //
            // chkAutoSave
            //
            this.chkAutoSave.AutoSize = true;
            this.chkAutoSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkAutoSave.Location = new System.Drawing.Point(20, 97);
            this.chkAutoSave.Name = "chkAutoSave";
            this.chkAutoSave.Size = new System.Drawing.Size(140, 29);
            this.chkAutoSave.TabIndex = 2;
            this.chkAutoSave.Text = "자동 저장";
            this.chkAutoSave.UseVisualStyleBackColor = true;
            //
            // cboAutoSaveInterval
            //
            this.cboAutoSaveInterval.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(51)))), ((int)(((byte)(59)))));
            this.cboAutoSaveInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAutoSaveInterval.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboAutoSaveInterval.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(234)))));
            this.cboAutoSaveInterval.FormattingEnabled = true;
            this.cboAutoSaveInterval.Location = new System.Drawing.Point(260, 95);
            this.cboAutoSaveInterval.Name = "cboAutoSaveInterval";
            this.cboAutoSaveInterval.Size = new System.Drawing.Size(200, 33);
            this.cboAutoSaveInterval.TabIndex = 3;
            //
            // lblCharCountMode
            //
            this.lblCharCountMode.AutoSize = true;
            this.lblCharCountMode.Location = new System.Drawing.Point(20, 148);
            this.lblCharCountMode.Name = "lblCharCountMode";
            this.lblCharCountMode.Size = new System.Drawing.Size(112, 25);
            this.lblCharCountMode.TabIndex = 4;
            this.lblCharCountMode.Text = "글자 수 기준";
            //
            // cboCharCountMode
            //
            this.cboCharCountMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(51)))), ((int)(((byte)(59)))));
            this.cboCharCountMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCharCountMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboCharCountMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(234)))));
            this.cboCharCountMode.FormattingEnabled = true;
            this.cboCharCountMode.Location = new System.Drawing.Point(260, 145);
            this.cboCharCountMode.Name = "cboCharCountMode";
            this.cboCharCountMode.Size = new System.Drawing.Size(200, 33);
            this.cboCharCountMode.TabIndex = 5;
            //
            // chkPastePlainText
            //
            this.chkPastePlainText.AutoSize = true;
            this.chkPastePlainText.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkPastePlainText.Location = new System.Drawing.Point(20, 195);
            this.chkPastePlainText.Name = "chkPastePlainText";
            this.chkPastePlainText.Size = new System.Drawing.Size(380, 29);
            this.chkPastePlainText.TabIndex = 6;
            this.chkPastePlainText.Text = "붙여넣을 때 서식 빼고 글자만 넣기";
            this.chkPastePlainText.UseVisualStyleBackColor = true;
            //
            // chkWritingShortcuts
            //
            this.chkWritingShortcuts.AutoSize = true;
            this.chkWritingShortcuts.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkWritingShortcuts.Location = new System.Drawing.Point(20, 235);
            this.chkWritingShortcuts.Name = "chkWritingShortcuts";
            this.chkWritingShortcuts.Size = new System.Drawing.Size(380, 29);
            this.chkWritingShortcuts.TabIndex = 7;
            this.chkWritingShortcuts.Text = "글쓰기 단축키 사용 (말줄임표, 겹낫표 등)";
            this.chkWritingShortcuts.UseVisualStyleBackColor = true;
            // 
            // chkSpellCheck
            // 
            this.chkSpellCheck.AutoSize = true;
            this.chkSpellCheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkSpellCheck.Location = new System.Drawing.Point(20, 270);
            this.chkSpellCheck.Name = "chkSpellCheck";
            this.chkSpellCheck.Size = new System.Drawing.Size(260, 29);
            this.chkSpellCheck.TabIndex = 9;
            this.chkSpellCheck.Text = "맞춤법 검사 (빨간 물결선)";
            this.chkSpellCheck.UseVisualStyleBackColor = true;
            // 
            // btnUserDictionary
            // 
            this.btnUserDictionary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(50)))), ((int)(((byte)(58)))));
            this.btnUserDictionary.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUserDictionary.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            this.btnUserDictionary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUserDictionary.ForeColor = System.Drawing.Color.White;
            this.btnUserDictionary.Location = new System.Drawing.Point(300, 266);
            this.btnUserDictionary.Name = "btnUserDictionary";
            this.btnUserDictionary.Size = new System.Drawing.Size(160, 35);
            this.btnUserDictionary.TabIndex = 10;
            this.btnUserDictionary.Text = "사용자 사전...";
            this.btnUserDictionary.UseVisualStyleBackColor = false;
            //
            // lblWritingHint
            //
            this.lblWritingHint.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblWritingHint.Location = new System.Drawing.Point(20, 305);
            this.lblWritingHint.Name = "lblWritingHint";
            this.lblWritingHint.Size = new System.Drawing.Size(440, 45);
            this.lblWritingHint.TabIndex = 8;
            this.lblWritingHint.Text = "자동 저장은 한 번 이상 저장한 문서에만 적용됩니다.\r\n처음 쓰는 문서는 먼저 Ctrl + S로 저장해 주세요.";
            //
            // pnlSettingsPage
            // 
            this.pnlSettingsPage.Controls.Add(this.pnlGeneralSettings);
            this.pnlSettingsPage.Controls.Add(this.lblSettingsDescription);
            this.pnlSettingsPage.Controls.Add(this.lblSettingsTitle);
            this.pnlSettingsPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSettingsPage.Location = new System.Drawing.Point(0, 0);
            this.pnlSettingsPage.Name = "pnlSettingsPage";
            this.pnlSettingsPage.Size = new System.Drawing.Size(508, 479);
            this.pnlSettingsPage.TabIndex = 12;
            // 
            // pnlGeneralSettings
            // 
            this.pnlGeneralSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            this.pnlGeneralSettings.Controls.Add(this.lblGeneralHint);
            this.pnlGeneralSettings.Controls.Add(this.chkOpenSidebarOnStartup);
            this.pnlGeneralSettings.Controls.Add(this.rdoStartLastDocument);
            this.pnlGeneralSettings.Controls.Add(this.rdoStartNewDocument);
            this.pnlGeneralSettings.Controls.Add(this.lblStartupAction);
            this.pnlGeneralSettings.Controls.Add(this.lblStartupDescription);
            this.pnlGeneralSettings.Controls.Add(this.lblStartupTitle);
            this.pnlGeneralSettings.Location = new System.Drawing.Point(25, 110);
            this.pnlGeneralSettings.Name = "pnlGeneralSettings";
            this.pnlGeneralSettings.Size = new System.Drawing.Size(480, 350);
            this.pnlGeneralSettings.TabIndex = 7;
            // 
            // lblStartupDescription
            // 
            this.lblStartupDescription.AutoSize = true;
            this.lblStartupDescription.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblStartupDescription.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblStartupDescription.Location = new System.Drawing.Point(20, 50);
            this.lblStartupDescription.Name = "lblStartupDescription";
            this.lblStartupDescription.Size = new System.Drawing.Size(310, 25);
            this.lblStartupDescription.TabIndex = 1;
            this.lblStartupDescription.Text = "프로그램 실행 시 동작을 설정합니다.";
            // 
            // lblStartupTitle
            // 
            this.lblStartupTitle.AutoSize = true;
            this.lblStartupTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblStartupTitle.Location = new System.Drawing.Point(20, 20);
            this.lblStartupTitle.Name = "lblStartupTitle";
            this.lblStartupTitle.Size = new System.Drawing.Size(126, 25);
            this.lblStartupTitle.TabIndex = 0;
            this.lblStartupTitle.Text = "프로그램 시작";
            //
            // lblStartupAction
            //
            this.lblStartupAction.AutoSize = true;
            this.lblStartupAction.Location = new System.Drawing.Point(20, 95);
            this.lblStartupAction.Name = "lblStartupAction";
            this.lblStartupAction.Size = new System.Drawing.Size(124, 25);
            this.lblStartupAction.TabIndex = 2;
            this.lblStartupAction.Text = "시작할 때 화면";
            //
            // rdoStartNewDocument
            //
            this.rdoStartNewDocument.AutoSize = true;
            this.rdoStartNewDocument.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rdoStartNewDocument.Location = new System.Drawing.Point(35, 128);
            this.rdoStartNewDocument.Name = "rdoStartNewDocument";
            this.rdoStartNewDocument.Size = new System.Drawing.Size(160, 29);
            this.rdoStartNewDocument.TabIndex = 3;
            this.rdoStartNewDocument.TabStop = true;
            this.rdoStartNewDocument.Text = "새 문서로 시작";
            this.rdoStartNewDocument.UseVisualStyleBackColor = true;
            //
            // rdoStartLastDocument
            //
            this.rdoStartLastDocument.AutoSize = true;
            this.rdoStartLastDocument.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rdoStartLastDocument.Location = new System.Drawing.Point(35, 163);
            this.rdoStartLastDocument.Name = "rdoStartLastDocument";
            this.rdoStartLastDocument.Size = new System.Drawing.Size(280, 29);
            this.rdoStartLastDocument.TabIndex = 4;
            this.rdoStartLastDocument.TabStop = true;
            this.rdoStartLastDocument.Text = "마지막으로 작업한 문서 열기";
            this.rdoStartLastDocument.UseVisualStyleBackColor = true;
            //
            // chkOpenSidebarOnStartup
            //
            this.chkOpenSidebarOnStartup.AutoSize = true;
            this.chkOpenSidebarOnStartup.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkOpenSidebarOnStartup.Location = new System.Drawing.Point(20, 215);
            this.chkOpenSidebarOnStartup.Name = "chkOpenSidebarOnStartup";
            this.chkOpenSidebarOnStartup.Size = new System.Drawing.Size(280, 29);
            this.chkOpenSidebarOnStartup.TabIndex = 5;
            this.chkOpenSidebarOnStartup.Text = "시작할 때 사이드 패널 열기";
            this.chkOpenSidebarOnStartup.UseVisualStyleBackColor = true;
            //
            // lblGeneralHint
            //
            this.lblGeneralHint.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblGeneralHint.Location = new System.Drawing.Point(20, 260);
            this.lblGeneralHint.Name = "lblGeneralHint";
            this.lblGeneralHint.Size = new System.Drawing.Size(440, 75);
            this.lblGeneralHint.TabIndex = 6;
            this.lblGeneralHint.Text = "마지막으로 작업한 문서는 최근 문서 목록의 첫 번째 문서입니다.\r\n그 문서를 찾을 수 없으면 새 문서로 시작합니다.";
            //
            // lblSettingsDescription
            // 
            this.lblSettingsDescription.AutoSize = true;
            this.lblSettingsDescription.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSettingsDescription.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblSettingsDescription.Location = new System.Drawing.Point(25, 25);
            this.lblSettingsDescription.Name = "lblSettingsDescription";
            this.lblSettingsDescription.Size = new System.Drawing.Size(316, 25);
            this.lblSettingsDescription.TabIndex = 6;
            this.lblSettingsDescription.Text = "WriteNote의 기본 동작을 설정합니다.";
            // 
            // lblSettingsTitle
            // 
            this.lblSettingsTitle.AutoSize = true;
            this.lblSettingsTitle.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSettingsTitle.Location = new System.Drawing.Point(27, 65);
            this.lblSettingsTitle.Name = "lblSettingsTitle";
            this.lblSettingsTitle.Size = new System.Drawing.Size(159, 45);
            this.lblSettingsTitle.TabIndex = 5;
            this.lblSettingsTitle.Text = "일반 설정";
            // 
            // pnlSettingsFooter
            // 
            this.pnlSettingsFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            this.pnlSettingsFooter.Controls.Add(this.btnResetSettings);
            this.pnlSettingsFooter.Controls.Add(this.btnApplySettings);
            this.pnlSettingsFooter.Controls.Add(this.btnCancelSettings);
            this.pnlSettingsFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSettingsFooter.Location = new System.Drawing.Point(0, 479);
            this.pnlSettingsFooter.Name = "pnlSettingsFooter";
            this.pnlSettingsFooter.Size = new System.Drawing.Size(508, 65);
            this.pnlSettingsFooter.TabIndex = 11;
            // 
            // btnResetSettings
            // 
            this.btnResetSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(38)))));
            this.btnResetSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetSettings.ForeColor = System.Drawing.Color.White;
            this.btnResetSettings.Location = new System.Drawing.Point(124, 18);
            this.btnResetSettings.Name = "btnResetSettings";
            this.btnResetSettings.Size = new System.Drawing.Size(120, 35);
            this.btnResetSettings.TabIndex = 8;
            this.btnResetSettings.Text = "기본값 복원";
            this.btnResetSettings.UseVisualStyleBackColor = false;
            // 
            // btnApplySettings
            // 
            this.btnApplySettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(85)))), ((int)(((byte)(165)))));
            this.btnApplySettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApplySettings.ForeColor = System.Drawing.Color.White;
            this.btnApplySettings.Location = new System.Drawing.Point(376, 18);
            this.btnApplySettings.Name = "btnApplySettings";
            this.btnApplySettings.Size = new System.Drawing.Size(120, 35);
            this.btnApplySettings.TabIndex = 10;
            this.btnApplySettings.Text = "저장";
            this.btnApplySettings.UseVisualStyleBackColor = false;
            // 
            // btnCancelSettings
            // 
            this.btnCancelSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(38)))));
            this.btnCancelSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelSettings.ForeColor = System.Drawing.Color.White;
            this.btnCancelSettings.Location = new System.Drawing.Point(250, 18);
            this.btnCancelSettings.Name = "btnCancelSettings";
            this.btnCancelSettings.Size = new System.Drawing.Size(120, 35);
            this.btnCancelSettings.TabIndex = 9;
            this.btnCancelSettings.Text = "취소";
            this.btnCancelSettings.UseVisualStyleBackColor = false;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "기능";
            this.columnHeader1.Width = 130;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "단축키";
            this.columnHeader2.Width = 160;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "구분";
            this.columnHeader3.Width = 100;
            // 
            // btnAddShortcut
            // 
            this.btnAddShortcut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(85)))), ((int)(((byte)(165)))));
            this.btnAddShortcut.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddShortcut.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            this.btnAddShortcut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddShortcut.ForeColor = System.Drawing.Color.White;
            this.btnAddShortcut.Location = new System.Drawing.Point(15, 295);
            this.btnAddShortcut.Name = "btnAddShortcut";
            this.btnAddShortcut.Size = new System.Drawing.Size(100, 35);
            this.btnAddShortcut.TabIndex = 3;
            this.btnAddShortcut.Text = "＋ 추가";
            this.btnAddShortcut.UseVisualStyleBackColor = false;
            // 
            // btnEditShortcut
            // 
            this.btnEditShortcut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(50)))), ((int)(((byte)(58)))));
            this.btnEditShortcut.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEditShortcut.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            this.btnEditShortcut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditShortcut.ForeColor = System.Drawing.Color.White;
            this.btnEditShortcut.Location = new System.Drawing.Point(125, 295);
            this.btnEditShortcut.Name = "btnEditShortcut";
            this.btnEditShortcut.Size = new System.Drawing.Size(90, 35);
            this.btnEditShortcut.TabIndex = 4;
            this.btnEditShortcut.Text = "수정";
            this.btnEditShortcut.UseVisualStyleBackColor = false;
            // 
            // btnDeleteShortcut
            // 
            this.btnDeleteShortcut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnDeleteShortcut.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeleteShortcut.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            this.btnDeleteShortcut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteShortcut.ForeColor = System.Drawing.Color.White;
            this.btnDeleteShortcut.Location = new System.Drawing.Point(225, 295);
            this.btnDeleteShortcut.Name = "btnDeleteShortcut";
            this.btnDeleteShortcut.Size = new System.Drawing.Size(90, 35);
            this.btnDeleteShortcut.TabIndex = 5;
            this.btnDeleteShortcut.Text = "삭제";
            this.btnDeleteShortcut.UseVisualStyleBackColor = false;
            // 
            // SettingsForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.ClientSize = new System.Drawing.Size(678, 544);
            this.Controls.Add(this.pnlSettingsContent);
            this.Controls.Add(this.pnlSettingsMenu);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(700, 600);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(700, 600);
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "WriteNote 설정";
            this.pnlSettingsMenu.ResumeLayout(false);
            this.pnlSettingsContent.ResumeLayout(false);
            this.pnlShortcutSettings.ResumeLayout(false);
            this.pnlShortcutSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudContentFontSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTitleFontSize)).EndInit();
            this.pnlAppearanceSettings.ResumeLayout(false);
            this.pnlAppearanceSettings.PerformLayout();
            this.pnlStorageSettings.ResumeLayout(false);
            this.pnlStorageSettings.PerformLayout();
            this.pnlWritingSettings.ResumeLayout(false);
            this.pnlWritingSettings.PerformLayout();
            this.pnlSettingsPage.ResumeLayout(false);
            this.pnlSettingsPage.PerformLayout();
            this.pnlGeneralSettings.ResumeLayout(false);
            this.pnlGeneralSettings.PerformLayout();
            this.pnlSettingsFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSettingsMenu;
        private System.Windows.Forms.Panel pnlSettingsContent;
        private System.Windows.Forms.Button btnGeneral;
        private System.Windows.Forms.Button btnAppearance;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Button btnStorage;
        private System.Windows.Forms.Button btnWriting;
        private System.Windows.Forms.Button btnShortcuts;
        private System.Windows.Forms.Label lblSettingsTitle;
        private System.Windows.Forms.Label lblSettingsDescription;
        private System.Windows.Forms.Button btnApplySettings;
        private System.Windows.Forms.Button btnCancelSettings;
        private System.Windows.Forms.Button btnResetSettings;
        private System.Windows.Forms.Panel pnlSettingsFooter;
        private System.Windows.Forms.Panel pnlSettingsPage;
        private System.Windows.Forms.Panel pnlGeneralSettings;
        private System.Windows.Forms.Label lblStartupTitle;
        private System.Windows.Forms.Label lblStartupDescription;
        private System.Windows.Forms.Label lblStartupAction;
        private System.Windows.Forms.RadioButton rdoStartNewDocument;
        private System.Windows.Forms.RadioButton rdoStartLastDocument;
        private System.Windows.Forms.CheckBox chkOpenSidebarOnStartup;
        private System.Windows.Forms.Label lblGeneralHint;
        private System.Windows.Forms.Panel pnlShortcutSettings;
        private System.Windows.Forms.Label lblShortcutSectionTitle;
        private System.Windows.Forms.Label lblShortcutSectionDescription;
        private System.Windows.Forms.Panel pnlWritingSettings;
        private System.Windows.Forms.Label lblWritingSectionDescription;
        private System.Windows.Forms.Label lblWritingSectionTitle;
        private System.Windows.Forms.CheckBox chkAutoSave;
        private BorderedComboBox cboAutoSaveInterval;
        private System.Windows.Forms.Label lblCharCountMode;
        private BorderedComboBox cboCharCountMode;
        private System.Windows.Forms.CheckBox chkPastePlainText;
        private System.Windows.Forms.CheckBox chkWritingShortcuts;
        private System.Windows.Forms.CheckBox chkSpellCheck;
        private System.Windows.Forms.Button btnUserDictionary;
        private System.Windows.Forms.Label lblWritingHint;
        private System.Windows.Forms.Panel pnlStorageSettings;
        private System.Windows.Forms.Label lblStorageSectionDescription;
        private System.Windows.Forms.Label lblStorageSectionTitle;
        private System.Windows.Forms.Label lblDocumentFolder;
        private System.Windows.Forms.TextBox txtDocumentFolder;
        private System.Windows.Forms.Button btnBrowseDocumentFolder;
        private System.Windows.Forms.Button btnDefaultDocumentFolder;
        private System.Windows.Forms.Button btnOpenDocumentFolder;
        private System.Windows.Forms.Label lblDocumentFolderHint;
        private System.Windows.Forms.Panel pnlAppearanceSettings;
        private System.Windows.Forms.Label lblAppearanceSectionDescription;
        private System.Windows.Forms.Label lblAppearanceSectionTitle;
        private System.Windows.Forms.Label lblTheme;
        private BorderedComboBox cboTheme;
        private System.Windows.Forms.Label lblContentFont;
        private BorderedComboBox cboContentFont;
        private System.Windows.Forms.Label lblContentFontSize;
        private System.Windows.Forms.NumericUpDown nudContentFontSize;
        private System.Windows.Forms.Label lblTitleFontSize;
        private System.Windows.Forms.NumericUpDown nudTitleFontSize;
        private System.Windows.Forms.Label lblFontPreview;
        private System.Windows.Forms.ListView lvShortcuts;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button btnAddShortcut;
        private System.Windows.Forms.Button btnDeleteShortcut;
        private System.Windows.Forms.Button btnEditShortcut;
    }
}