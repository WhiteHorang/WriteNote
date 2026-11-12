namespace WriteNote
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.txtSearchWord = new System.Windows.Forms.TextBox();
            this.btnFindNext = new System.Windows.Forms.Button();
            this.btnFindPrevious = new System.Windows.Forms.Button();
            this.btnCloseSearch = new System.Windows.Forms.Button();
            this.pnlHeaderButtons = new System.Windows.Forms.Panel();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnSaveAs = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnToggleSidebar = new System.Windows.Forms.Button();
            this.lblAppName = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblCharCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSaveStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlRecentDocuments = new System.Windows.Forms.Panel();
            this.btnTrashDocuments = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnFolderMenu = new System.Windows.Forms.Button();
            this.cboFolder = new WriteNote.BorderedComboBox();
            this.cboSortDocuments = new WriteNote.BorderedComboBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnFavoriteDocuments = new System.Windows.Forms.Button();
            this.btnRecentDocuments = new System.Windows.Forms.Button();
            this.btnAllDocuments = new System.Windows.Forms.Button();
            this.flpDocumentList = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlDocumentItem = new System.Windows.Forms.Panel();
            this.lblDocumentItemDate = new System.Windows.Forms.Label();
            this.lblDocumentItemTitle = new System.Windows.Forms.Label();
            this.lblEmptyDocuments = new System.Windows.Forms.Label();
            this.lblRecentTitle = new System.Windows.Forms.Label();
            this.pnlBody = new System.Windows.Forms.Panel();
            this.pnlEditor = new System.Windows.Forms.Panel();
            this.tlpEditor = new System.Windows.Forms.TableLayoutPanel();
            this.rtbContent = new System.Windows.Forms.RichTextBox();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.cmsRecentDocuments = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiOpenFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRename = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiRemoveRecent = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDeleteDocument = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlHeader.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.pnlHeaderButtons.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.pnlRecentDocuments.SuspendLayout();
            this.flpDocumentList.SuspendLayout();
            this.pnlDocumentItem.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.pnlEditor.SuspendLayout();
            this.tlpEditor.SuspendLayout();
            this.cmsRecentDocuments.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            this.pnlHeader.Controls.Add(this.pnlSearch);
            this.pnlHeader.Controls.Add(this.pnlHeaderButtons);
            this.pnlHeader.Controls.Add(this.btnToggleSidebar);
            this.pnlHeader.Controls.Add(this.lblAppName);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(220, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(958, 65);
            this.pnlHeader.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(50)))));
            this.pnlSearch.Controls.Add(this.txtSearchWord);
            this.pnlSearch.Controls.Add(this.btnFindNext);
            this.pnlSearch.Controls.Add(this.btnFindPrevious);
            this.pnlSearch.Controls.Add(this.btnCloseSearch);
            this.pnlSearch.Location = new System.Drawing.Point(122, 9);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(399, 50);
            this.pnlSearch.TabIndex = 7;
            this.pnlSearch.Visible = false;
            // 
            // txtSearchWord
            // 
            this.txtSearchWord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(51)))), ((int)(((byte)(59)))));
            this.txtSearchWord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearchWord.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtSearchWord.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(234)))));
            this.txtSearchWord.Location = new System.Drawing.Point(17, 7);
            this.txtSearchWord.Name = "txtSearchWord";
            this.txtSearchWord.Size = new System.Drawing.Size(220, 31);
            this.txtSearchWord.TabIndex = 0;
            // 
            // btnFindNext
            // 
            this.btnFindNext.Location = new System.Drawing.Point(243, 9);
            this.btnFindNext.Name = "btnFindNext";
            this.btnFindNext.Size = new System.Drawing.Size(55, 30);
            this.btnFindNext.TabIndex = 7;
            this.btnFindNext.Text = "다음";
            this.btnFindNext.UseVisualStyleBackColor = true;
            this.btnFindNext.Click += new System.EventHandler(this.btnFindNext_Click);
            // 
            // btnFindPrevious
            // 
            this.btnFindPrevious.Location = new System.Drawing.Point(304, 9);
            this.btnFindPrevious.Name = "btnFindPrevious";
            this.btnFindPrevious.Size = new System.Drawing.Size(55, 30);
            this.btnFindPrevious.TabIndex = 8;
            this.btnFindPrevious.Text = "이전";
            this.btnFindPrevious.UseVisualStyleBackColor = true;
            this.btnFindPrevious.Click += new System.EventHandler(this.btnFindPrevious_Click);
            // 
            // btnCloseSearch
            // 
            this.btnCloseSearch.Location = new System.Drawing.Point(365, 9);
            this.btnCloseSearch.Name = "btnCloseSearch";
            this.btnCloseSearch.Size = new System.Drawing.Size(30, 30);
            this.btnCloseSearch.TabIndex = 9;
            this.btnCloseSearch.Text = "✕";
            this.btnCloseSearch.UseVisualStyleBackColor = true;
            this.btnCloseSearch.Click += new System.EventHandler(this.btnCloseSearch_Click);
            // 
            // pnlHeaderButtons
            // 
            this.pnlHeaderButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlHeaderButtons.BackColor = System.Drawing.Color.Transparent;
            this.pnlHeaderButtons.Controls.Add(this.btnNew);
            this.pnlHeaderButtons.Controls.Add(this.btnSaveAs);
            this.pnlHeaderButtons.Controls.Add(this.btnOpen);
            this.pnlHeaderButtons.Controls.Add(this.btnSave);
            this.pnlHeaderButtons.Location = new System.Drawing.Point(440, 9);
            this.pnlHeaderButtons.Name = "pnlHeaderButtons";
            this.pnlHeaderButtons.Size = new System.Drawing.Size(515, 45);
            this.pnlHeaderButtons.TabIndex = 6;
            // 
            // btnNew
            // 
            this.btnNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(59)))), ((int)(((byte)(66)))));
            this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNew.ForeColor = System.Drawing.Color.White;
            this.btnNew.Location = new System.Drawing.Point(87, 6);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(90, 35);
            this.btnNew.TabIndex = 8;
            this.btnNew.Text = "새 문서";
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.AutoSize = true;
            this.btnSaveAs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(85)))), ((int)(((byte)(165)))));
            this.btnSaveAs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveAs.ForeColor = System.Drawing.Color.White;
            this.btnSaveAs.Location = new System.Drawing.Point(335, 6);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(180, 35);
            this.btnSaveAs.TabIndex = 4;
            this.btnSaveAs.Text = "다른 이름으로 저장";
            this.btnSaveAs.UseVisualStyleBackColor = false;
            this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(59)))), ((int)(((byte)(66)))));
            this.btnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpen.ForeColor = System.Drawing.Color.White;
            this.btnOpen.Location = new System.Drawing.Point(183, 6);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(70, 35);
            this.btnOpen.TabIndex = 3;
            this.btnOpen.Text = "열기";
            this.btnOpen.UseVisualStyleBackColor = false;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(85)))), ((int)(((byte)(165)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnSave.Location = new System.Drawing.Point(259, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(70, 35);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnToggleSidebar
            // 
            this.btnToggleSidebar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnToggleSidebar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggleSidebar.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnToggleSidebar.ForeColor = System.Drawing.Color.White;
            this.btnToggleSidebar.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnToggleSidebar.Location = new System.Drawing.Point(15, 16);
            this.btnToggleSidebar.Name = "btnToggleSidebar";
            this.btnToggleSidebar.Size = new System.Drawing.Size(40, 35);
            this.btnToggleSidebar.TabIndex = 5;
            this.btnToggleSidebar.Text = "☰";
            this.btnToggleSidebar.UseVisualStyleBackColor = true;
            this.btnToggleSidebar.Click += new System.EventHandler(this.btnToggleSidebar_Click);
            // 
            // lblAppName
            // 
            this.lblAppName.AutoSize = true;
            this.lblAppName.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblAppName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.lblAppName.Location = new System.Drawing.Point(61, 9);
            this.lblAppName.Name = "lblAppName";
            this.lblAppName.Size = new System.Drawing.Size(180, 45);
            this.lblAppName.TabIndex = 0;
            this.lblAppName.Text = "WriteNote";
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblCharCount,
            this.lblSaveStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 712);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1178, 32);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblCharCount
            // 
            this.lblCharCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.lblCharCount.Name = "lblCharCount";
            this.lblCharCount.Size = new System.Drawing.Size(98, 25);
            this.lblCharCount.Text = "글자 수 : 0";
            // 
            // lblSaveStatus
            // 
            this.lblSaveStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.lblSaveStatus.Name = "lblSaveStatus";
            this.lblSaveStatus.Size = new System.Drawing.Size(126, 25);
            this.lblSaveStatus.Text = "저장되지 않음";
            // 
            // pnlRecentDocuments
            // 
            this.pnlRecentDocuments.Controls.Add(this.btnTrashDocuments);
            this.pnlRecentDocuments.Controls.Add(this.btnSettings);
            this.pnlRecentDocuments.Controls.Add(this.btnFolderMenu);
            this.pnlRecentDocuments.Controls.Add(this.cboFolder);
            this.pnlRecentDocuments.Controls.Add(this.cboSortDocuments);
            this.pnlRecentDocuments.Controls.Add(this.txtSearch);
            this.pnlRecentDocuments.Controls.Add(this.btnFavoriteDocuments);
            this.pnlRecentDocuments.Controls.Add(this.btnRecentDocuments);
            this.pnlRecentDocuments.Controls.Add(this.btnAllDocuments);
            this.pnlRecentDocuments.Controls.Add(this.flpDocumentList);
            this.pnlRecentDocuments.Controls.Add(this.lblRecentTitle);
            this.pnlRecentDocuments.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlRecentDocuments.Location = new System.Drawing.Point(0, 0);
            this.pnlRecentDocuments.Name = "pnlRecentDocuments";
            this.pnlRecentDocuments.Size = new System.Drawing.Size(220, 712);
            this.pnlRecentDocuments.TabIndex = 4;
            // 
            // btnTrashDocuments
            // 
            this.btnTrashDocuments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTrashDocuments.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(38)))));
            this.btnTrashDocuments.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTrashDocuments.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnTrashDocuments.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnTrashDocuments.Location = new System.Drawing.Point(12, 625);
            this.btnTrashDocuments.Name = "btnTrashDocuments";
            this.btnTrashDocuments.Size = new System.Drawing.Size(190, 38);
            this.btnTrashDocuments.TabIndex = 5;
            this.btnTrashDocuments.Text = "휴지통";
            this.btnTrashDocuments.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTrashDocuments.UseVisualStyleBackColor = false;
            this.btnTrashDocuments.Click += new System.EventHandler(this.btnTrashDocuments_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSettings.FlatAppearance.BorderSize = 0;
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.ForeColor = System.Drawing.Color.White;
            this.btnSettings.Location = new System.Drawing.Point(12, 669);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(190, 40);
            this.btnSettings.TabIndex = 9;
            this.btnSettings.Text = "설정";
            this.btnSettings.UseVisualStyleBackColor = true;
            // 
            // btnFolderMenu
            // 
            this.btnFolderMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(38)))));
            this.btnFolderMenu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFolderMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFolderMenu.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnFolderMenu.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnFolderMenu.Location = new System.Drawing.Point(166, 170);
            this.btnFolderMenu.Name = "btnFolderMenu";
            this.btnFolderMenu.Size = new System.Drawing.Size(36, 28);
            this.btnFolderMenu.TabIndex = 10;
            this.btnFolderMenu.Text = "⋯";
            this.btnFolderMenu.UseVisualStyleBackColor = false;
            // 
            // cboFolder
            // 
            this.cboFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(51)))), ((int)(((byte)(59)))));
            this.cboFolder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboFolder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(234)))));
            this.cboFolder.FormattingEnabled = true;
            this.cboFolder.Location = new System.Drawing.Point(12, 171);
            this.cboFolder.Name = "cboFolder";
            this.cboFolder.Size = new System.Drawing.Size(150, 26);
            this.cboFolder.TabIndex = 9;
            // 
            // cboSortDocuments
            // 
            this.cboSortDocuments.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(51)))), ((int)(((byte)(59)))));
            this.cboSortDocuments.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSortDocuments.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboSortDocuments.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(234)))));
            this.cboSortDocuments.FormattingEnabled = true;
            this.cboSortDocuments.Location = new System.Drawing.Point(12, 240);
            this.cboSortDocuments.Name = "cboSortDocuments";
            this.cboSortDocuments.Size = new System.Drawing.Size(190, 26);
            this.cboSortDocuments.TabIndex = 8;
            this.cboSortDocuments.SelectedIndexChanged += new System.EventHandler(this.cboSortDocuments_SelectedIndexChanged);
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(51)))), ((int)(((byte)(59)))));
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(234)))));
            this.txtSearch.Location = new System.Drawing.Point(12, 203);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(190, 31);
            this.txtSearch.TabIndex = 6;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // btnFavoriteDocuments
            // 
            this.btnFavoriteDocuments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFavoriteDocuments.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(38)))));
            this.btnFavoriteDocuments.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFavoriteDocuments.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnFavoriteDocuments.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnFavoriteDocuments.Location = new System.Drawing.Point(12, 129);
            this.btnFavoriteDocuments.Name = "btnFavoriteDocuments";
            this.btnFavoriteDocuments.Size = new System.Drawing.Size(190, 38);
            this.btnFavoriteDocuments.TabIndex = 4;
            this.btnFavoriteDocuments.Text = "즐겨찾기";
            this.btnFavoriteDocuments.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFavoriteDocuments.UseVisualStyleBackColor = false;
            this.btnFavoriteDocuments.Click += new System.EventHandler(this.btnFavoriteDocuments_Click);
            // 
            // btnRecentDocuments
            // 
            this.btnRecentDocuments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRecentDocuments.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(38)))));
            this.btnRecentDocuments.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecentDocuments.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRecentDocuments.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnRecentDocuments.Location = new System.Drawing.Point(12, 87);
            this.btnRecentDocuments.Name = "btnRecentDocuments";
            this.btnRecentDocuments.Size = new System.Drawing.Size(190, 38);
            this.btnRecentDocuments.TabIndex = 3;
            this.btnRecentDocuments.Text = "최근 문서";
            this.btnRecentDocuments.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRecentDocuments.UseVisualStyleBackColor = false;
            this.btnRecentDocuments.Click += new System.EventHandler(this.btnRecentDocuments_Click);
            // 
            // btnAllDocuments
            // 
            this.btnAllDocuments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAllDocuments.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(38)))));
            this.btnAllDocuments.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAllDocuments.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAllDocuments.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnAllDocuments.Location = new System.Drawing.Point(12, 45);
            this.btnAllDocuments.Name = "btnAllDocuments";
            this.btnAllDocuments.Size = new System.Drawing.Size(190, 38);
            this.btnAllDocuments.TabIndex = 2;
            this.btnAllDocuments.Text = "전체 문서";
            this.btnAllDocuments.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAllDocuments.UseVisualStyleBackColor = false;
            this.btnAllDocuments.Click += new System.EventHandler(this.btnAllDocuments_Click);
            // 
            // flpDocumentList
            // 
            this.flpDocumentList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpDocumentList.AutoScroll = true;
            this.flpDocumentList.Controls.Add(this.pnlDocumentItem);
            this.flpDocumentList.Controls.Add(this.lblEmptyDocuments);
            this.flpDocumentList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpDocumentList.Location = new System.Drawing.Point(12, 277);
            this.flpDocumentList.Name = "flpDocumentList";
            this.flpDocumentList.Size = new System.Drawing.Size(190, 308);
            this.flpDocumentList.TabIndex = 5;
            this.flpDocumentList.WrapContents = false;
            // 
            // pnlDocumentItem
            // 
            this.pnlDocumentItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(40)))));
            this.pnlDocumentItem.Controls.Add(this.lblDocumentItemDate);
            this.pnlDocumentItem.Controls.Add(this.lblDocumentItemTitle);
            this.pnlDocumentItem.Location = new System.Drawing.Point(0, 0);
            this.pnlDocumentItem.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.pnlDocumentItem.Name = "pnlDocumentItem";
            this.pnlDocumentItem.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.pnlDocumentItem.Size = new System.Drawing.Size(190, 72);
            this.pnlDocumentItem.TabIndex = 6;
            this.pnlDocumentItem.Visible = false;
            // 
            // lblDocumentItemDate
            // 
            this.lblDocumentItemDate.AutoEllipsis = true;
            this.lblDocumentItemDate.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDocumentItemDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.lblDocumentItemDate.Location = new System.Drawing.Point(10, 39);
            this.lblDocumentItemDate.Name = "lblDocumentItemDate";
            this.lblDocumentItemDate.Size = new System.Drawing.Size(155, 18);
            this.lblDocumentItemDate.TabIndex = 7;
            this.lblDocumentItemDate.Text = "2026. 11. 09. 오후 3:00";
            this.lblDocumentItemDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDocumentItemTitle
            // 
            this.lblDocumentItemTitle.AutoEllipsis = true;
            this.lblDocumentItemTitle.AutoSize = true;
            this.lblDocumentItemTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDocumentItemTitle.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblDocumentItemTitle.Location = new System.Drawing.Point(10, 8);
            this.lblDocumentItemTitle.Name = "lblDocumentItemTitle";
            this.lblDocumentItemTitle.Size = new System.Drawing.Size(96, 25);
            this.lblDocumentItemTitle.TabIndex = 6;
            this.lblDocumentItemTitle.Text = "첫 번째 글";
            // 
            // lblEmptyDocuments
            // 
            this.lblEmptyDocuments.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblEmptyDocuments.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.lblEmptyDocuments.Location = new System.Drawing.Point(3, 82);
            this.lblEmptyDocuments.Name = "lblEmptyDocuments";
            this.lblEmptyDocuments.Size = new System.Drawing.Size(175, 60);
            this.lblEmptyDocuments.TabIndex = 7;
            this.lblEmptyDocuments.Text = "아직 작성한 문서가 없습니다.";
            this.lblEmptyDocuments.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblEmptyDocuments.Visible = false;
            // 
            // lblRecentTitle
            // 
            this.lblRecentTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblRecentTitle.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblRecentTitle.ForeColor = System.Drawing.Color.White;
            this.lblRecentTitle.Location = new System.Drawing.Point(12, 12);
            this.lblRecentTitle.Name = "lblRecentTitle";
            this.lblRecentTitle.Size = new System.Drawing.Size(190, 25);
            this.lblRecentTitle.TabIndex = 0;
            this.lblRecentTitle.Text = "최근 문서";
            this.lblRecentTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBody
            // 
            this.pnlBody.Controls.Add(this.pnlEditor);
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Location = new System.Drawing.Point(220, 65);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Size = new System.Drawing.Size(958, 647);
            this.pnlBody.TabIndex = 2;
            // 
            // pnlEditor
            // 
            this.pnlEditor.Controls.Add(this.tlpEditor);
            this.pnlEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlEditor.Location = new System.Drawing.Point(0, 0);
            this.pnlEditor.Name = "pnlEditor";
            this.pnlEditor.Size = new System.Drawing.Size(958, 647);
            this.pnlEditor.TabIndex = 5;
            // 
            // tlpEditor
            // 
            this.tlpEditor.ColumnCount = 1;
            this.tlpEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpEditor.Controls.Add(this.rtbContent, 0, 1);
            this.tlpEditor.Controls.Add(this.txtTitle, 0, 0);
            this.tlpEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpEditor.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tlpEditor.Location = new System.Drawing.Point(0, 0);
            this.tlpEditor.Name = "tlpEditor";
            this.tlpEditor.RowCount = 2;
            this.tlpEditor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpEditor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpEditor.Size = new System.Drawing.Size(958, 647);
            this.tlpEditor.TabIndex = 3;
            // 
            // rtbContent
            // 
            this.rtbContent.AcceptsTab = true;
            this.rtbContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            this.rtbContent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbContent.DetectUrls = false;
            this.rtbContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbContent.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rtbContent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.rtbContent.HideSelection = false;
            this.rtbContent.Location = new System.Drawing.Point(3, 63);
            this.rtbContent.Name = "rtbContent";
            this.rtbContent.Size = new System.Drawing.Size(952, 581);
            this.rtbContent.TabIndex = 11;
            this.rtbContent.Text = "";
            this.rtbContent.TextChanged += new System.EventHandler(this.rtbContent_TextChanged);
            // 
            // txtTitle
            // 
            this.txtTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            this.txtTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTitle.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.txtTitle.Location = new System.Drawing.Point(3, 3);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(952, 50);
            this.txtTitle.TabIndex = 10;
            // 
            // cmsRecentDocuments
            // 
            this.cmsRecentDocuments.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(51)))), ((int)(((byte)(59)))));
            this.cmsRecentDocuments.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.cmsRecentDocuments.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiOpen,
            this.tsmiFavorite,
            this.tsmiSeparator1,
            this.tsmiOpenFolder,
            this.tsmiRename,
            this.tsmiSeparator2,
            this.tsmiRemoveRecent,
            this.tsmiDeleteDocument,
            this.toolStripSeparator1,
            this.tsmiProperties});
            this.cmsRecentDocuments.Name = "cmsRecentDocuments";
            this.cmsRecentDocuments.Size = new System.Drawing.Size(241, 246);
            this.cmsRecentDocuments.Opening += new System.ComponentModel.CancelEventHandler(this.cmsRecentDocuments_Opening);
            // 
            // tsmiOpen
            // 
            this.tsmiOpen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(234)))));
            this.tsmiOpen.Name = "tsmiOpen";
            this.tsmiOpen.Size = new System.Drawing.Size(240, 32);
            this.tsmiOpen.Text = "문서 열기";
            this.tsmiOpen.Click += new System.EventHandler(this.tsmiOpen_Click);
            // 
            // tsmiFavorite
            // 
            this.tsmiFavorite.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(234)))));
            this.tsmiFavorite.Name = "tsmiFavorite";
            this.tsmiFavorite.Size = new System.Drawing.Size(240, 32);
            this.tsmiFavorite.Text = "즐겨찾기에 추가";
            this.tsmiFavorite.Click += new System.EventHandler(this.tsmiFavorite_Click);
            // 
            // tsmiSeparator1
            // 
            this.tsmiSeparator1.Name = "tsmiSeparator1";
            this.tsmiSeparator1.Size = new System.Drawing.Size(237, 6);
            // 
            // tsmiOpenFolder
            // 
            this.tsmiOpenFolder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(234)))));
            this.tsmiOpenFolder.Name = "tsmiOpenFolder";
            this.tsmiOpenFolder.Size = new System.Drawing.Size(240, 32);
            this.tsmiOpenFolder.Text = "파일 위치 열기";
            this.tsmiOpenFolder.Click += new System.EventHandler(this.tsmiOpenFolder_Click);
            // 
            // tsmiRename
            // 
            this.tsmiRename.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(234)))));
            this.tsmiRename.Name = "tsmiRename";
            this.tsmiRename.Size = new System.Drawing.Size(240, 32);
            this.tsmiRename.Text = "이름 변경";
            this.tsmiRename.Click += new System.EventHandler(this.tsmiRename_Click);
            // 
            // tsmiSeparator2
            // 
            this.tsmiSeparator2.Name = "tsmiSeparator2";
            this.tsmiSeparator2.Size = new System.Drawing.Size(237, 6);
            // 
            // tsmiRemoveRecent
            // 
            this.tsmiRemoveRecent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(234)))));
            this.tsmiRemoveRecent.Name = "tsmiRemoveRecent";
            this.tsmiRemoveRecent.Size = new System.Drawing.Size(240, 32);
            this.tsmiRemoveRecent.Text = "최근 목록에서 제거";
            this.tsmiRemoveRecent.Click += new System.EventHandler(this.tsmiRemoveRecent_Click);
            // 
            // tsmiDeleteDocument
            // 
            this.tsmiDeleteDocument.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(234)))));
            this.tsmiDeleteDocument.Name = "tsmiDeleteDocument";
            this.tsmiDeleteDocument.Size = new System.Drawing.Size(240, 32);
            this.tsmiDeleteDocument.Text = "휴지통으로 이동";
            this.tsmiDeleteDocument.Click += new System.EventHandler(this.tsmiDeleteDocument_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(237, 6);
            // 
            // tsmiProperties
            // 
            this.tsmiProperties.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(234)))));
            this.tsmiProperties.Name = "tsmiProperties";
            this.tsmiProperties.Size = new System.Drawing.Size(240, 32);
            this.tsmiProperties.Text = "속성 보기";
            this.tsmiProperties.Click += new System.EventHandler(this.tsmiProperties_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.ClientSize = new System.Drawing.Size(1178, 744);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlRecentDocuments);
            this.Controls.Add(this.statusStrip1);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WriteNote";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlHeaderButtons.ResumeLayout(false);
            this.pnlHeaderButtons.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pnlRecentDocuments.ResumeLayout(false);
            this.pnlRecentDocuments.PerformLayout();
            this.flpDocumentList.ResumeLayout(false);
            this.pnlDocumentItem.ResumeLayout(false);
            this.pnlDocumentItem.PerformLayout();
            this.pnlBody.ResumeLayout(false);
            this.pnlEditor.ResumeLayout(false);
            this.tlpEditor.ResumeLayout(false);
            this.tlpEditor.PerformLayout();
            this.cmsRecentDocuments.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblAppName;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblCharCount;
        private System.Windows.Forms.ToolStripStatusLabel lblSaveStatus;
        private System.Windows.Forms.Panel pnlRecentDocuments;
        private System.Windows.Forms.Label lblRecentTitle;
        private System.Windows.Forms.Panel pnlEditor;
        private System.Windows.Forms.TableLayoutPanel tlpEditor;
        private System.Windows.Forms.RichTextBox rtbContent;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.Panel pnlHeaderButtons;
        private System.Windows.Forms.ContextMenuStrip cmsRecentDocuments;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpen;
        private System.Windows.Forms.ToolStripMenuItem tsmiFavorite;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenFolder;
        private System.Windows.Forms.ToolStripMenuItem tsmiRename;
        private System.Windows.Forms.ToolStripMenuItem tsmiRemoveRecent;
        private System.Windows.Forms.ToolStripMenuItem tsmiProperties;
        private System.Windows.Forms.ToolStripSeparator tsmiSeparator1;
        private System.Windows.Forms.ToolStripSeparator tsmiSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiDeleteDocument;
        private System.Windows.Forms.Button btnAllDocuments;
        private System.Windows.Forms.Button btnFavoriteDocuments;
        private System.Windows.Forms.Button btnTrashDocuments;
        private System.Windows.Forms.Button btnRecentDocuments;
        private System.Windows.Forms.FlowLayoutPanel flpDocumentList;
        private System.Windows.Forms.Panel pnlDocumentItem;
        private System.Windows.Forms.Label lblDocumentItemTitle;
        private System.Windows.Forms.Label lblDocumentItemDate;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblEmptyDocuments;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.TextBox txtSearchWord;
        private System.Windows.Forms.Button btnFindNext;
        private System.Windows.Forms.Button btnFindPrevious;
        private System.Windows.Forms.Button btnCloseSearch;
        private BorderedComboBox cboSortDocuments;
        private BorderedComboBox cboFolder;
        private System.Windows.Forms.Button btnFolderMenu;
        private System.Windows.Forms.Button btnSaveAs;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnToggleSidebar;
        private System.Windows.Forms.Button btnSettings;
    }
}

