using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace WriteNote
{
    // 문서 카드 우클릭 메뉴: 열기, 즐겨찾기, 파일 위치 열기, 이름 변경, 최근 목록에서 제거, 휴지통으로 이동, 속성 보기 (폴더로 이동은 Folders.cs)
    public partial class MainForm
    {
        // 문서 카드 우클릭 메뉴가 다루는 문서
        // (우클릭한 카드의 파일 경로. 메뉴의 각 기능은 이 경로를 쓴다)
        private string menuDocumentPath = "";


        // 우클릭 메뉴 대상 문서. 없으면(또는 파일이 사라졌으면) 안내하고 null
        private string GetMenuDocumentPath(bool mustExist)
        {
            if (string.IsNullOrEmpty(menuDocumentPath))
                return null;

            if (mustExist && !File.Exists(menuDocumentPath))
            {
                MessageBox.Show("파일을 찾을 수 없습니다.\n\n" + menuDocumentPath, "WriteNote",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoadRecentDocuments();
                return null;
            }

            return menuDocumentPath;
        }


        // 문서 열기 메뉴
        private void tsmiOpen_Click(object sender, EventArgs e)
        {
            string filePath = GetMenuDocumentPath(true);

            if (filePath == null)
            {
                return;
            }

            if (!ConfirmSaveChanges())
            {
                return;
            }

            LoadDocument(filePath);
        }


        // ───────────── 문서 메뉴 (파일 작업 오류 처리) ─────────────
        // 다른 프로그램이 파일을 사용 중이거나 권한이 없을 때
        // 프로그램이 종료되지 않도록 오류를 알리고 목록을 새로 고친다.
        private void RunFileAction(string actionName, Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    actionName + " 중 오류가 발생했습니다.\n\n" + ex.Message,
                    "WriteNote",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                try
                {
                    LoadRecentDocuments();
                }
                catch (Exception)
                {
                    // 목록을 새로 고치지 못해도 계속 사용할 수 있다.
                }
            }
        }


        private void tsmiFavorite_Click(object sender, EventArgs e)
        {
            RunFileAction("즐겨찾기 변경", ToggleFavorite);
        }


        private void cmsRecentDocuments_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                UpdateFavoriteMenuText();
            }
            catch (Exception)
            {
                // 문서 정보를 읽지 못하면 메뉴 문구만 그대로 둔다.
            }
        }


        private void tsmiOpenFolder_Click(object sender, EventArgs e)
        {
            RunFileAction("파일 위치 열기", OpenDocumentLocation);
        }


        private void tsmiRename_Click(object sender, EventArgs e)
        {
            RunFileAction("문서 이름 변경", RenameDocument);
        }


        private void tsmiRemoveRecent_Click(object sender, EventArgs e)
        {
            RunFileAction("최근 문서에서 제거", RemoveFromRecentDocuments);
        }


        private void tsmiDeleteDocument_Click(object sender, EventArgs e)
        {
            RunFileAction("문서 삭제", DeleteDocument);
        }


        private void tsmiProperties_Click(object sender, EventArgs e)
        {
            RunFileAction("문서 속성 보기", ShowDocumentProperties);
        }


        // 즐겨찾기 추가 및 해제
        private void ToggleFavorite()
        {
            string filePath = GetMenuDocumentPath(true);

            if (filePath == null) return;

            string jsonPath = Path.ChangeExtension(filePath, ".json");

            if (!File.Exists(jsonPath))
            {
                MessageBox.Show("문서 정보를 찾을 수 없습니다.", "WriteNote", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string json = File.ReadAllText(jsonPath);

            DocumentInfo info =
                JsonConvert.DeserializeObject<DocumentInfo>(json);

            if (info == null) return;

            info.IsFavorite = !info.IsFavorite;

            string updatedJson =
                JsonConvert.SerializeObject(info, Formatting.Indented);

            File.WriteAllText(jsonPath, updatedJson);

            LoadRecentDocuments();

            MessageBox.Show(
                info.IsFavorite
                    ? "즐겨찾기에 추가했습니다."
                    : "즐겨찾기에서 해제했습니다.",
                "WriteNote",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }


        // 오른쪽 클릭 메뉴의 즐겨찾기 문구 변경
        private void UpdateFavoriteMenuText()
        {
            // 기본 문구로 되돌린 뒤, 즐겨찾기 문서면 '해제'로 바꾼다.
            tsmiFavorite.Text = "즐겨찾기에 추가";

            string filePath = GetMenuDocumentPath(false);

            if (filePath == null) return;

            string jsonPath = Path.ChangeExtension(filePath, ".json");

            if (!File.Exists(jsonPath)) return;

            string json = File.ReadAllText(jsonPath);

            DocumentInfo info =
                JsonConvert.DeserializeObject<DocumentInfo>(json);

            if (info == null) return;

            tsmiFavorite.Text = info.IsFavorite
                ? "즐겨찾기 해제"
                : "즐겨찾기에 추가";
        }


        // 파일 위치 열기
        private void OpenDocumentLocation()
        {
            string filePath = GetMenuDocumentPath(true);

            if (filePath == null) return;

            Process.Start(
                "explorer.exe",
                "/select,\"" + filePath + "\""
            );
        }


        // 문서 이름 변경
        private void RenameDocument()
        {
            string filePath = GetMenuDocumentPath(true);

            if (filePath == null) return;

            if (!ConfirmSaveChanges()) return;

            string jsonPath = Path.ChangeExtension(filePath, ".json");

            if (!File.Exists(jsonPath))
            {
                MessageBox.Show("문서 정보를 찾을 수 없습니다.", "WriteNote", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string json = File.ReadAllText(jsonPath);

            DocumentInfo info =
                JsonConvert.DeserializeObject<DocumentInfo>(json);

            if (info == null) return;

            string newName = "";

            using (Form inputForm = new Form())
            {
                inputForm.Text = "문서 이름 변경";
                inputForm.Size = new Size(350, 150);
                inputForm.StartPosition = FormStartPosition.CenterParent;
                inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                inputForm.MaximizeBox = false;
                inputForm.MinimizeBox = false;

                Label lblMessage = new Label();
                lblMessage.Text = "변경할 문서 이름을 입력하세요.";
                lblMessage.Location = new Point(15, 15);
                lblMessage.AutoSize = true;

                TextBox txtNewName = new TextBox();
                txtNewName.Location = new Point(15, 45);
                txtNewName.Size = new Size(300, 25);
                txtNewName.Text = info.Title;

                Button btnOK = new Button();
                btnOK.Text = "확인";
                btnOK.DialogResult = DialogResult.OK;
                btnOK.Location = new Point(155, 80);

                Button btnCancel = new Button();
                btnCancel.Text = "취소";
                btnCancel.DialogResult = DialogResult.Cancel;
                btnCancel.Location = new Point(240, 80);

                inputForm.Controls.Add(lblMessage);
                inputForm.Controls.Add(txtNewName);
                inputForm.Controls.Add(btnOK);
                inputForm.Controls.Add(btnCancel);

                inputForm.AcceptButton = btnOK;
                inputForm.CancelButton = btnCancel;

                if (inputForm.ShowDialog(this) == DialogResult.OK)
                {
                    newName = txtNewName.Text.Trim();
                }
                else
                {
                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(newName)) return;

            foreach (char c in Path.GetInvalidFileNameChars())
            {
                if (newName.Contains(c))
                {
                    MessageBox.Show("파일 이름에 사용할 수 없는 문자가 포함되어 있습니다.", "WriteNote", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            string newFilePath = Path.Combine(
                Path.GetDirectoryName(filePath),
                newName + ".rtf"
            );

            string newJsonPath = Path.ChangeExtension(newFilePath, ".json");

            if (File.Exists(newFilePath) || File.Exists(newJsonPath))
            {
                MessageBox.Show("이미 같은 이름의 문서가 존재합니다.", "WriteNote", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            File.Move(filePath, newFilePath);

            try
            {
                File.Move(jsonPath, newJsonPath);
            }
            catch (Exception)
            {
                // 문서 정보를 옮기지 못하면 문서 이름도 원래대로 되돌린다.
                File.Move(newFilePath, filePath);
                throw;
            }

            RecentDocumentManager.RenameRecentDocument(
                filePath,
                newFilePath
            );

            info.Title = newName;
            info.FileName = Path.GetFileName(newFilePath);
            info.ModifiedAt = DateTime.Now;

            string updatedJson =
                JsonConvert.SerializeObject(info, Formatting.Indented);

            File.WriteAllText(newJsonPath, updatedJson);

            if (string.Equals(
                currentFilePath,
                filePath,
                StringComparison.OrdinalIgnoreCase))
            {
                currentFilePath = newFilePath;
                txtTitle.Text = newName;
                isModified = false;
                lblSaveStatus.Text = "저장됨";
            }

            LoadRecentDocuments();

            MessageBox.Show("문서 이름을 변경했습니다.", "WriteNote", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        // 최근 문서 목록에서 제거
        private void RemoveFromRecentDocuments()
        {
            // 파일이 없어도 목록에서는 뺄 수 있어야 하므로 존재 여부는 확인하지 않는다.
            string filePath = GetMenuDocumentPath(false);

            if (filePath == null)
            {
                return;
            }

            RecentDocumentManager.RemoveRecentDocument(filePath);

            LoadRecentDocuments();

            MessageBox.Show("최근 문서 목록에서 제거했습니다.", "WriteNote", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        // 문서 완전 삭제
        private void DeleteDocument()
        {
            string filePath = GetMenuDocumentPath(true);

            if (filePath == null)
            {
                return;
            }

            bool isOpenDocument = string.Equals(
                currentFilePath,
                filePath,
                StringComparison.OrdinalIgnoreCase);

            // 지금 열려 있는 문서라면 저장하지 않은 내용을 먼저 처리한다.
            // (저장하면 최신 내용이 휴지통에 들어간다)
            if (isOpenDocument && !ConfirmSaveChanges())
            {
                return;
            }

            string title = ReadDocumentTitle(filePath);

            // 바로 지우지 않고 휴지통으로 옮긴다. (휴지통에서 복원 가능)
            TrashManager.MoveToTrash(filePath, title);

            RecentDocumentManager.RemoveRecentDocument(filePath);

            if (isOpenDocument)
            {
                CreateNewDocument();
            }

            LoadRecentDocuments();

            MessageBox.Show(
                "'" + title + "' 문서를 휴지통으로 옮겼습니다.\n\n" +
                "사이드 패널의 '휴지통'에서 복원할 수 있습니다.",
                "휴지통",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }


        // 문서 속성 보기
        private void ShowDocumentProperties()
        {
            string filePath = GetMenuDocumentPath(true);

            if (filePath == null)
            {
                return;
            }

            string jsonPath = Path.ChangeExtension(filePath, ".json");

            if (!File.Exists(jsonPath))
            {
                MessageBox.Show("문서 정보를 찾을 수 없습니다.", "WriteNote", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string json = File.ReadAllText(jsonPath);
            DocumentInfo info = JsonConvert.DeserializeObject<DocumentInfo>(json);

            if (info == null)
            {
                return;
            }

            FileInfo fileInfo = new FileInfo(filePath);

            string documentText = "";

            using (RichTextBox tempBox = new RichTextBox())
            {
                tempBox.LoadFile(filePath, RichTextBoxStreamType.RichText);
                documentText = tempBox.Text;
            }

            // 메인 화면 글자 수와 같은 기준: 줄바꿈은 세지 않는다.
            int totalCharacters = documentText.Count(c => c != '\r' && c != '\n');

            int charactersWithoutSpaces = 0;

            foreach (char c in documentText)
            {
                if (!char.IsWhiteSpace(c))
                {
                    charactersWithoutSpaces++;
                }
            }

            using (Form propertiesForm = new Form())
            {
                propertiesForm.Text = "문서 속성";
                propertiesForm.Size = new Size(400, 330);
                propertiesForm.StartPosition = FormStartPosition.CenterParent;
                propertiesForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                propertiesForm.MaximizeBox = false;
                propertiesForm.MinimizeBox = false;

                Label lblProperties = new Label();

                lblProperties.Text =
                    "문서 제목 : " + info.Title + "\n\n" +
                    "파일 이름 : " + fileInfo.Name + "\n\n" +
                    "저장 위치 : " + fileInfo.DirectoryName + "\n\n" +
                    "파일 크기 : " + (fileInfo.Length / 1024.0).ToString("N2") + " KB\n\n" +
                    // 문서 정보에 만든 날짜가 없으면(다른 곳에서 만든 RTF 등) 파일이 만들어진 날짜
                    "생성 날짜 : " + (info.CreatedAt == default(DateTime) ? fileInfo.CreationTime : info.CreatedAt)
                        .ToString("yyyy-MM-dd HH:mm") + "\n\n" +
                    "마지막 수정 : " + fileInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm") + "\n\n" +
                    "즐겨찾기 : " + (info.IsFavorite ? "등록됨" : "등록되지 않음") + "\n\n" +
                    "공백 포함 글자 수 : " + totalCharacters.ToString("N0") + "자\n\n" +
                    "공백 미포함 글자 수 : " + charactersWithoutSpaces.ToString("N0") + "자";

                lblProperties.Location = new Point(20, 20);
                lblProperties.Size = new Size(345, 300);

                propertiesForm.Size = new Size(400, 450);

                Button btnClose = new Button();
                btnClose.Text = "닫기";
                btnClose.Size = new Size(80, 30);
                btnClose.Location = new Point(290, 350);
                btnClose.DialogResult = DialogResult.OK;

                propertiesForm.Controls.Add(lblProperties);
                propertiesForm.Controls.Add(btnClose);
                propertiesForm.AcceptButton = btnClose;

                propertiesForm.ShowDialog(this);
            }
        }
    }
}
