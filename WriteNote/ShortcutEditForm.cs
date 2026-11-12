using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WriteNote
{
    public partial class ShortcutEditForm : Form
    {
        // 확정된 단축키 조합 (저장 버튼을 누르면 이 값이 결과가 된다)
        private Keys shortcutKeys = Keys.None;

        private const string KeyPlaceholder = "여기를 누르고 단축키를 입력하세요";

        // 중복 검사에 사용할 기존 단축키 목록
        private readonly List<ShortcutItem> existingShortcuts;

        // 수정 중인 항목 (새로 추가할 때는 null)
        private readonly ShortcutItem editingItem;

        // 새 단축키 추가
        public ShortcutEditForm(List<ShortcutItem> existingShortcuts)
            : this(existingShortcuts, null)
        {
        }

        // 기존 단축키 수정
        public ShortcutEditForm(List<ShortcutItem> existingShortcuts, ShortcutItem editingItem)
            : this()
        {
            this.existingShortcuts = existingShortcuts;
            this.editingItem = editingItem;

            if (editingItem != null)
            {
                Text = "단축키 수정";
                ShortcutName = editingItem.Name;
                ShortcutKeys = editingItem.Keys;
                ShortcutContent = editingItem.Content;
            }
            else
            {
                Text = "단축키 추가";
            }
        }

        // 디자이너용 기본 생성자
        public ShortcutEditForm()
        {
            InitializeComponent();
            AppIcon.Apply(this);

            // 입력할 내용 칸에서는 Enter로 줄을 바꿀 수 있게 한다.
            txtShortcutContent.AcceptsReturn = true;

            // 단축키 입력 칸은 글자를 직접 입력하지 않고 키 조합만 받는다.
            txtShortcutKey.ReadOnly = true;
            txtShortcutKey.BackColor = SystemColors.Window;
            txtShortcutKey.ShortcutsEnabled = false;
            txtShortcutKey.ImeMode = ImeMode.Disable;
            txtShortcutKey.KeyUp += txtShortcutKey_KeyUp;
            txtShortcutKey.Enter += (s, e) => UpdateKeyText();
            txtShortcutKey.Leave += (s, e) => UpdateKeyText();

            btnSaveShortcut.Click += btnSaveShortcut_Click;
            btnCancelShortcut.Click += (s, e) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };

            UpdateKeyText();

            // 디자이너의 어두운 테마 색을 현재 테마로 바꾼다.
            ThemeManager.Apply(this, AppTheme.Dark);
        }

        // 다른 화면에서 값을 넣고 꺼낼 때 사용하는 속성
        public string ShortcutName
        {
            get { return txtShortcutName.Text.Trim(); }
            set { txtShortcutName.Text = value ?? ""; }
        }

        public Keys ShortcutKeys
        {
            get { return shortcutKeys; }
            set
            {
                shortcutKeys = value;
                UpdateKeyText();
            }
        }

        public string ShortcutContent
        {
            get { return txtShortcutContent.Text; }
            set { txtShortcutContent.Text = value ?? ""; }
        }

        // 단축키 입력 칸에 포커스가 있을 때 모든 키 입력을 가로챈다.
        // (Enter, Esc, Tab 등은 폼이 먼저 처리하므로 KeyDown 대신 여기서 받는다)
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!txtShortcutKey.Focused)
                return base.ProcessCmdKey(ref msg, keyData);

            Keys key = keyData & Keys.KeyCode;
            Keys modifiers = keyData & Keys.Modifiers;

            // 한글 입력기 처리 중인 키는 무시
            if (key == Keys.ProcessKey)
                return true;

            // 수식 키만 누른 상태: 미리보기만 표시 (예: "Ctrl + Shift + ")
            if (ShortcutKeyHelper.IsModifierKey(key))
            {
                txtShortcutKey.Text = ShortcutKeyHelper.ToDisplayString(modifiers) + " + ";
                return true;
            }

            if (modifiers == Keys.None)
            {
                switch (key)
                {
                    case Keys.Tab:      // 다음 칸으로 이동
                    case Keys.Escape:   // 창 닫기(취소)
                    case Keys.Return:   // 저장
                        return base.ProcessCmdKey(ref msg, keyData);

                    case Keys.Back:     // 입력한 단축키 지우기
                    case Keys.Delete:
                        shortcutKeys = Keys.None;
                        UpdateKeyText();
                        return true;
                }
            }

            // Shift + Tab: 이전 칸으로 이동
            if (keyData == (Keys.Shift | Keys.Tab))
                return base.ProcessCmdKey(ref msg, keyData);

            // Alt + F4: 창 닫기는 그대로 둔다
            if (keyData == (Keys.Alt | Keys.F4))
                return base.ProcessCmdKey(ref msg, keyData);

            if (ShortcutKeyHelper.IsValidShortcut(keyData))
            {
                shortcutKeys = keyData;
                UpdateKeyText();
            }
            else
            {
                // 사용할 수 없는 조합이면 기존 값을 유지한다.
                System.Media.SystemSounds.Beep.Play();
                UpdateKeyText();
            }

            return true;
        }

        // 수식 키만 눌렀다 뗀 경우 미리보기를 지우고 확정된 값으로 되돌린다.
        private void txtShortcutKey_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.None)
                UpdateKeyText();
        }

        private void UpdateKeyText()
        {
            if (shortcutKeys == Keys.None)
            {
                txtShortcutKey.Text = txtShortcutKey.Focused ? "" : KeyPlaceholder;
                txtShortcutKey.ForeColor = Color.Gray;
            }
            else
            {
                txtShortcutKey.Text = ShortcutKeyHelper.ToDisplayString(shortcutKeys);
                txtShortcutKey.ForeColor = SystemColors.WindowText;
            }

            txtShortcutKey.SelectionStart = txtShortcutKey.TextLength;
        }

        private void btnSaveShortcut_Click(object sender, EventArgs e)
        {
            if (ShortcutName.Length == 0)
            {
                MessageBox.Show("기능 이름을 입력하세요.", "단축키 설정",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtShortcutName.Focus();
                return;
            }

            if (shortcutKeys == Keys.None)
            {
                MessageBox.Show(
                    "단축키를 입력하세요.\n\nCtrl 또는 Alt를 포함한 조합이나 F1~F12 키를 사용할 수 있습니다.",
                    "단축키 설정", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtShortcutKey.Focus();
                return;
            }

            // 기본 단축키 및 다른 항목과 겹치는지 검사
            string conflict = ShortcutManager.FindConflict(
                existingShortcuts,
                shortcutKeys,
                editingItem != null ? editingItem.Id : null);

            if (conflict != null)
            {
                MessageBox.Show(conflict + "\n\n다른 키 조합을 입력하세요.", "단축키 중복",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtShortcutKey.Focus();
                return;
            }

            if (string.IsNullOrEmpty(ShortcutContent))
            {
                MessageBox.Show("단축키를 눌렀을 때 입력할 내용을 적어 주세요.", "단축키 설정",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtShortcutContent.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        // 입력 결과를 ShortcutItem으로 만든다.
        // 수정이면 기존 항목의 Id와 기본 단축키 여부를 유지한다.
        public ShortcutItem ToShortcutItem()
        {
            ShortcutItem item = editingItem != null ? editingItem.Clone() : new ShortcutItem();

            // 내용이 바뀌면 기존 커서 위치가 맞지 않을 수 있으므로 내용 끝으로 둔다.
            if (editingItem != null && editingItem.Content != ShortcutContent)
                item.CursorOffset = -1;

            item.Name = ShortcutName;
            item.Keys = shortcutKeys;
            item.Content = ShortcutContent;

            return item;
        }
    }
}
