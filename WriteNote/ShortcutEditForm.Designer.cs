namespace WriteNote
{
    partial class ShortcutEditForm
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
            this.lblShortcutName = new System.Windows.Forms.Label();
            this.txtShortcutName = new System.Windows.Forms.TextBox();
            this.lblShortcutKey = new System.Windows.Forms.Label();
            this.txtShortcutKey = new System.Windows.Forms.TextBox();
            this.lblShortcutContent = new System.Windows.Forms.Label();
            this.txtShortcutContent = new System.Windows.Forms.TextBox();
            this.btnSaveShortcut = new System.Windows.Forms.Button();
            this.btnCancelShortcut = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblShortcutName
            // 
            this.lblShortcutName.AutoSize = true;
            this.lblShortcutName.Location = new System.Drawing.Point(25, 30);
            this.lblShortcutName.Name = "lblShortcutName";
            this.lblShortcutName.Size = new System.Drawing.Size(90, 25);
            this.lblShortcutName.TabIndex = 0;
            this.lblShortcutName.Text = "기능 이름";
            // 
            // txtShortcutName
            // 
            this.txtShortcutName.Location = new System.Drawing.Point(25, 55);
            this.txtShortcutName.Name = "txtShortcutName";
            this.txtShortcutName.Size = new System.Drawing.Size(435, 31);
            this.txtShortcutName.TabIndex = 1;
            // 
            // lblShortcutKey
            // 
            this.lblShortcutKey.AutoSize = true;
            this.lblShortcutKey.Location = new System.Drawing.Point(25, 105);
            this.lblShortcutKey.Name = "lblShortcutKey";
            this.lblShortcutKey.Size = new System.Drawing.Size(66, 25);
            this.lblShortcutKey.TabIndex = 2;
            this.lblShortcutKey.Text = "단축키";
            // 
            // txtShortcutKey
            // 
            this.txtShortcutKey.Location = new System.Drawing.Point(25, 130);
            this.txtShortcutKey.Name = "txtShortcutKey";
            this.txtShortcutKey.Size = new System.Drawing.Size(435, 31);
            this.txtShortcutKey.TabIndex = 3;
            // 
            // lblShortcutContent
            // 
            this.lblShortcutContent.AutoSize = true;
            this.lblShortcutContent.Location = new System.Drawing.Point(25, 180);
            this.lblShortcutContent.Name = "lblShortcutContent";
            this.lblShortcutContent.Size = new System.Drawing.Size(108, 25);
            this.lblShortcutContent.TabIndex = 4;
            this.lblShortcutContent.Text = "입력할 내용";
            // 
            // txtShortcutContent
            // 
            this.txtShortcutContent.Location = new System.Drawing.Point(25, 205);
            this.txtShortcutContent.Multiline = true;
            this.txtShortcutContent.Name = "txtShortcutContent";
            this.txtShortcutContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtShortcutContent.Size = new System.Drawing.Size(435, 90);
            this.txtShortcutContent.TabIndex = 5;
            // 
            // btnSaveShortcut
            // 
            this.btnSaveShortcut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(85)))), ((int)(((byte)(165)))));
            this.btnSaveShortcut.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveShortcut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveShortcut.Location = new System.Drawing.Point(254, 301);
            this.btnSaveShortcut.Name = "btnSaveShortcut";
            this.btnSaveShortcut.Size = new System.Drawing.Size(100, 35);
            this.btnSaveShortcut.TabIndex = 6;
            this.btnSaveShortcut.Text = "저장";
            this.btnSaveShortcut.UseVisualStyleBackColor = false;
            // 
            // btnCancelShortcut
            // 
            this.btnCancelShortcut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(38)))));
            this.btnCancelShortcut.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelShortcut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelShortcut.Location = new System.Drawing.Point(360, 301);
            this.btnCancelShortcut.Name = "btnCancelShortcut";
            this.btnCancelShortcut.Size = new System.Drawing.Size(100, 35);
            this.btnCancelShortcut.TabIndex = 7;
            this.btnCancelShortcut.Text = "취소";
            this.btnCancelShortcut.UseVisualStyleBackColor = false;
            // 
            // ShortcutEditForm
            // 
            this.AcceptButton = this.btnSaveShortcut;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.CancelButton = this.btnCancelShortcut;
            this.ClientSize = new System.Drawing.Size(478, 344);
            this.Controls.Add(this.btnCancelShortcut);
            this.Controls.Add(this.btnSaveShortcut);
            this.Controls.Add(this.txtShortcutContent);
            this.Controls.Add(this.lblShortcutContent);
            this.Controls.Add(this.txtShortcutKey);
            this.Controls.Add(this.lblShortcutKey);
            this.Controls.Add(this.txtShortcutName);
            this.Controls.Add(this.lblShortcutName);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ShortcutEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "단축키 설정";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblShortcutName;
        private System.Windows.Forms.TextBox txtShortcutName;
        private System.Windows.Forms.Label lblShortcutKey;
        private System.Windows.Forms.TextBox txtShortcutKey;
        private System.Windows.Forms.Label lblShortcutContent;
        private System.Windows.Forms.TextBox txtShortcutContent;
        private System.Windows.Forms.Button btnSaveShortcut;
        private System.Windows.Forms.Button btnCancelShortcut;
    }
}