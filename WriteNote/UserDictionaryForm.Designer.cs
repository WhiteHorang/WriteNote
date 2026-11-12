namespace WriteNote
{
    partial class UserDictionaryForm
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
            this.lblInfo = new System.Windows.Forms.Label();
            this.txtNewWord = new System.Windows.Forms.TextBox();
            this.btnAddWord = new System.Windows.Forms.Button();
            this.lstWords = new System.Windows.Forms.ListBox();
            this.lblCount = new System.Windows.Forms.Label();
            this.btnDeleteWord = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblInfo
            //
            this.lblInfo.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblInfo.Location = new System.Drawing.Point(20, 15);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(400, 50);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "맞춤법 검사에서 맞는 말로 볼 단어입니다.\r\n등장인물 이름을 넣으면 \'민수가\'처럼 조사가 붙어도 통과합니다.";
            //
            // txtNewWord
            //
            this.txtNewWord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(51)))), ((int)(((byte)(59)))));
            this.txtNewWord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNewWord.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(234)))));
            this.txtNewWord.Location = new System.Drawing.Point(20, 72);
            this.txtNewWord.Name = "txtNewWord";
            this.txtNewWord.Size = new System.Drawing.Size(290, 31);
            this.txtNewWord.TabIndex = 1;
            //
            // btnAddWord
            //
            this.btnAddWord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(85)))), ((int)(((byte)(165)))));
            this.btnAddWord.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddWord.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            this.btnAddWord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddWord.ForeColor = System.Drawing.Color.White;
            this.btnAddWord.Location = new System.Drawing.Point(320, 70);
            this.btnAddWord.Name = "btnAddWord";
            this.btnAddWord.Size = new System.Drawing.Size(100, 35);
            this.btnAddWord.TabIndex = 2;
            this.btnAddWord.Text = "＋ 추가";
            this.btnAddWord.UseVisualStyleBackColor = false;
            //
            // lstWords
            //
            this.lstWords.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            this.lstWords.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstWords.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.lstWords.FormattingEnabled = true;
            this.lstWords.IntegralHeight = false;
            this.lstWords.ItemHeight = 25;
            this.lstWords.Location = new System.Drawing.Point(20, 117);
            this.lstWords.Name = "lstWords";
            this.lstWords.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstWords.Size = new System.Drawing.Size(400, 255);
            this.lstWords.TabIndex = 3;
            //
            // lblCount
            //
            this.lblCount.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblCount.Location = new System.Drawing.Point(20, 385);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(280, 25);
            this.lblCount.TabIndex = 4;
            this.lblCount.Text = "단어 0개";
            //
            // btnDeleteWord
            //
            this.btnDeleteWord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnDeleteWord.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeleteWord.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            this.btnDeleteWord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteWord.ForeColor = System.Drawing.Color.White;
            this.btnDeleteWord.Location = new System.Drawing.Point(320, 380);
            this.btnDeleteWord.Name = "btnDeleteWord";
            this.btnDeleteWord.Size = new System.Drawing.Size(100, 35);
            this.btnDeleteWord.TabIndex = 5;
            this.btnDeleteWord.Text = "삭제";
            this.btnDeleteWord.UseVisualStyleBackColor = false;
            //
            // btnOK
            //
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(85)))), ((int)(((byte)(165)))));
            this.btnOK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(214, 430);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 35);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "확인";
            this.btnOK.UseVisualStyleBackColor = false;
            //
            // btnCancel
            //
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(38)))));
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(320, 430);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = false;
            //
            // UserDictionaryForm
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(440, 480);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnDeleteWord);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.lstWords);
            this.Controls.Add(this.btnAddWord);
            this.Controls.Add(this.txtNewWord);
            this.Controls.Add(this.lblInfo);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserDictionaryForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "맞춤법 사용자 사전";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.TextBox txtNewWord;
        private System.Windows.Forms.Button btnAddWord;
        private System.Windows.Forms.ListBox lstWords;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Button btnDeleteWord;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}
