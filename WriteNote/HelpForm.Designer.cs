namespace WriteNote
{
    partial class HelpForm
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
            this.pnlHelpTop = new System.Windows.Forms.Panel();
            this.btnHelpFind = new System.Windows.Forms.Button();
            this.txtHelpFind = new System.Windows.Forms.TextBox();
            this.lblHelpTitle = new System.Windows.Forms.Label();
            this.pnlHelpBottom = new System.Windows.Forms.Panel();
            this.lblHelpHint = new System.Windows.Forms.Label();
            this.btnHelpClose = new System.Windows.Forms.Button();
            this.lstHelpSections = new System.Windows.Forms.ListBox();
            this.pnlHelpContent = new System.Windows.Forms.Panel();
            this.rtbHelp = new System.Windows.Forms.RichTextBox();
            this.pnlHelpTop.SuspendLayout();
            this.pnlHelpBottom.SuspendLayout();
            this.pnlHelpContent.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlHelpTop
            //
            this.pnlHelpTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(38)))));
            this.pnlHelpTop.Controls.Add(this.btnHelpFind);
            this.pnlHelpTop.Controls.Add(this.txtHelpFind);
            this.pnlHelpTop.Controls.Add(this.lblHelpTitle);
            this.pnlHelpTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHelpTop.Location = new System.Drawing.Point(0, 0);
            this.pnlHelpTop.Name = "pnlHelpTop";
            this.pnlHelpTop.Size = new System.Drawing.Size(1000, 64);
            this.pnlHelpTop.TabIndex = 0;
            //
            // btnHelpFind
            //
            this.btnHelpFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHelpFind.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(85)))), ((int)(((byte)(165)))));
            this.btnHelpFind.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHelpFind.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHelpFind.ForeColor = System.Drawing.Color.White;
            this.btnHelpFind.Location = new System.Drawing.Point(892, 15);
            this.btnHelpFind.Name = "btnHelpFind";
            this.btnHelpFind.Size = new System.Drawing.Size(90, 35);
            this.btnHelpFind.TabIndex = 2;
            this.btnHelpFind.Text = "찾기";
            this.btnHelpFind.UseVisualStyleBackColor = false;
            //
            // txtHelpFind
            //
            this.txtHelpFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHelpFind.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(51)))), ((int)(((byte)(59)))));
            this.txtHelpFind.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHelpFind.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(234)))));
            this.txtHelpFind.Location = new System.Drawing.Point(652, 17);
            this.txtHelpFind.Name = "txtHelpFind";
            this.txtHelpFind.Size = new System.Drawing.Size(232, 31);
            this.txtHelpFind.TabIndex = 1;
            //
            // lblHelpTitle
            //
            this.lblHelpTitle.AutoSize = true;
            this.lblHelpTitle.Font = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblHelpTitle.Location = new System.Drawing.Point(18, 14);
            this.lblHelpTitle.Name = "lblHelpTitle";
            this.lblHelpTitle.Size = new System.Drawing.Size(220, 35);
            this.lblHelpTitle.TabIndex = 0;
            this.lblHelpTitle.Text = "WriteNote 설명서";
            //
            // pnlHelpBottom
            //
            this.pnlHelpBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(38)))));
            this.pnlHelpBottom.Controls.Add(this.lblHelpHint);
            this.pnlHelpBottom.Controls.Add(this.btnHelpClose);
            this.pnlHelpBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlHelpBottom.Location = new System.Drawing.Point(0, 636);
            this.pnlHelpBottom.Name = "pnlHelpBottom";
            this.pnlHelpBottom.Size = new System.Drawing.Size(1000, 60);
            this.pnlHelpBottom.TabIndex = 3;
            //
            // lblHelpHint
            //
            this.lblHelpHint.AutoSize = true;
            this.lblHelpHint.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblHelpHint.Location = new System.Drawing.Point(18, 18);
            this.lblHelpHint.Name = "lblHelpHint";
            this.lblHelpHint.Size = new System.Drawing.Size(420, 25);
            this.lblHelpHint.TabIndex = 0;
            this.lblHelpHint.Text = "글쓰기 화면에서 F1을 누르면 언제든 이 설명서를 열 수 있습니다.";
            //
            // btnHelpClose
            //
            this.btnHelpClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHelpClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(50)))), ((int)(((byte)(58)))));
            this.btnHelpClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHelpClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            this.btnHelpClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHelpClose.ForeColor = System.Drawing.Color.White;
            this.btnHelpClose.Location = new System.Drawing.Point(882, 12);
            this.btnHelpClose.Name = "btnHelpClose";
            this.btnHelpClose.Size = new System.Drawing.Size(100, 35);
            this.btnHelpClose.TabIndex = 1;
            this.btnHelpClose.Text = "닫기";
            this.btnHelpClose.UseVisualStyleBackColor = false;
            //
            // lstHelpSections
            //
            this.lstHelpSections.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(38)))));
            this.lstHelpSections.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstHelpSections.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lstHelpSections.Dock = System.Windows.Forms.DockStyle.Left;
            this.lstHelpSections.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lstHelpSections.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(234)))));
            this.lstHelpSections.FormattingEnabled = true;
            this.lstHelpSections.IntegralHeight = false;
            this.lstHelpSections.ItemHeight = 28;
            this.lstHelpSections.Location = new System.Drawing.Point(0, 64);
            this.lstHelpSections.Name = "lstHelpSections";
            this.lstHelpSections.Size = new System.Drawing.Size(240, 572);
            this.lstHelpSections.TabIndex = 4;
            //
            // pnlHelpContent
            //
            this.pnlHelpContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            this.pnlHelpContent.Controls.Add(this.rtbHelp);
            this.pnlHelpContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHelpContent.Location = new System.Drawing.Point(240, 64);
            this.pnlHelpContent.Name = "pnlHelpContent";
            this.pnlHelpContent.Padding = new System.Windows.Forms.Padding(28, 18, 12, 12);
            this.pnlHelpContent.Size = new System.Drawing.Size(760, 572);
            this.pnlHelpContent.TabIndex = 5;
            //
            // rtbHelp
            //
            this.rtbHelp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            this.rtbHelp.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbHelp.DetectUrls = false;
            this.rtbHelp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbHelp.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rtbHelp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.rtbHelp.HideSelection = false;
            this.rtbHelp.Location = new System.Drawing.Point(28, 18);
            this.rtbHelp.Name = "rtbHelp";
            this.rtbHelp.ReadOnly = true;
            this.rtbHelp.Size = new System.Drawing.Size(720, 542);
            this.rtbHelp.TabIndex = 0;
            this.rtbHelp.Text = "";
            //
            // HelpForm
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.CancelButton = this.btnHelpClose;
            this.ClientSize = new System.Drawing.Size(1000, 696);
            this.Controls.Add(this.pnlHelpContent);
            this.Controls.Add(this.lstHelpSections);
            this.Controls.Add(this.pnlHelpBottom);
            this.Controls.Add(this.pnlHelpTop);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 520);
            this.Name = "HelpForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "WriteNote 설명서";
            this.pnlHelpTop.ResumeLayout(false);
            this.pnlHelpTop.PerformLayout();
            this.pnlHelpBottom.ResumeLayout(false);
            this.pnlHelpBottom.PerformLayout();
            this.pnlHelpContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHelpTop;
        private System.Windows.Forms.Button btnHelpFind;
        private System.Windows.Forms.TextBox txtHelpFind;
        private System.Windows.Forms.Label lblHelpTitle;
        private System.Windows.Forms.Panel pnlHelpBottom;
        private System.Windows.Forms.Label lblHelpHint;
        private System.Windows.Forms.Button btnHelpClose;
        private System.Windows.Forms.ListBox lstHelpSections;
        private System.Windows.Forms.Panel pnlHelpContent;
        private System.Windows.Forms.RichTextBox rtbHelp;
    }
}
