using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WriteNote
{
    // 맞춤법 사용자 사전 관리 창
    // - 사전에 추가한 단어를 보고, 새로 추가하거나 지울 수 있다.
    // - '확인'을 누르면 파일(%AppData%\WriteNote\user_dictionary.txt)에 저장하고
    //   본문의 물결선에 바로 반영된다. '취소'를 누르면 바꾼 내용을 버린다.
    public partial class UserDictionaryForm : Form
    {
        // 창 안에서 편집 중인 단어 목록
        private readonly List<string> words;

        private bool hasChanges = false;

        public UserDictionaryForm()
        {
            InitializeComponent();

            words = SpellCheckService.GetUserWords();

            btnAddWord.Click += (s, e) => AddWord();
            btnDeleteWord.Click += (s, e) => DeleteSelectedWords();
            btnOK.Click += btnOK_Click;
            btnCancel.Click += (s, e) => Close();

            // 입력칸에서 Enter = 추가
            txtNewWord.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    AddWord();
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                }
            };

            // 목록에서 Delete = 삭제
            lstWords.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Delete)
                {
                    DeleteSelectedWords();
                    e.Handled = true;
                }
            };

            lstWords.SelectedIndexChanged += (s, e) => UpdateButtons();
            FormClosing += UserDictionaryForm_FormClosing;

            RefreshList(null);

            // 디자이너의 어두운 테마 색을 현재 테마로 바꾼다.
            ThemeManager.Apply(this, AppTheme.Dark);
        }

        // 지금 편집 중인 단어 목록 (테스트·확인용)
        public IReadOnlyList<string> Words
        {
            get { return words; }
        }

        private void RefreshList(string selectWord)
        {
            lstWords.BeginUpdate();
            lstWords.Items.Clear();

            foreach (string word in words)
            {
                lstWords.Items.Add(word);
            }

            lstWords.EndUpdate();

            if (selectWord != null)
            {
                int index = lstWords.Items.IndexOf(selectWord);

                if (index >= 0)
                {
                    lstWords.SelectedIndex = index;
                    lstWords.TopIndex = Math.Max(0, index - 3);
                }
            }

            lblCount.Text = words.Count == 0
                ? "아직 추가한 단어가 없습니다."
                : "단어 " + words.Count + "개";

            UpdateButtons();
        }

        private void UpdateButtons()
        {
            btnDeleteWord.Enabled = lstWords.SelectedItems.Count > 0;
        }

        // 입력칸의 단어를 목록에 추가
        public void AddWord()
        {
            string word = txtNewWord.Text.Trim();
            string error = SpellCheckService.ValidateUserWord(word);

            if (error != null)
            {
                MessageBox.Show(error, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNewWord.Focus();
                return;
            }

            if (words.Contains(word))
            {
                // 이미 있으면 그 단어를 보여 주기만 한다.
                RefreshList(word);
                txtNewWord.SelectAll();
                return;
            }

            words.Add(word);
            words.Sort(StringComparer.CurrentCulture);
            hasChanges = true;

            txtNewWord.Clear();
            RefreshList(word);
            txtNewWord.Focus();
        }

        // 선택한 단어들을 목록에서 지움
        public void DeleteSelectedWords()
        {
            if (lstWords.SelectedItems.Count == 0)
                return;

            int firstIndex = lstWords.SelectedIndices[0];

            foreach (string word in lstWords.SelectedItems.Cast<string>().ToList())
            {
                words.Remove(word);
            }

            hasChanges = true;

            // 지운 자리 근처를 다시 선택해 연속으로 지우기 쉽게 한다.
            string next = words.Count > 0 ? words[Math.Min(firstIndex, words.Count - 1)] : null;
            RefreshList(next);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (hasChanges)
                    SpellCheckService.SetUserWords(words);
            }
            catch (Exception ex)
            {
                MessageBox.Show("사용자 사전을 저장하지 못했습니다.\n\n" + ex.Message, Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            hasChanges = false;
            DialogResult = DialogResult.OK;
            Close();
        }

        // 저장하지 않고 닫으려 하면 확인
        private void UserDictionaryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!hasChanges)
                return;

            DialogResult result = MessageBox.Show(
                "바꾼 내용이 있습니다.\n저장하지 않고 닫으시겠습니까?",
                Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                e.Cancel = true;
        }
    }
}
