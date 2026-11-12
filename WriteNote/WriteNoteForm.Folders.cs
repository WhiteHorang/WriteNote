using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace WriteNote
{
    // 폴더별 문서 분류
    // - 문서 폴더 바로 아래의 하위 폴더를 '분류'로 사용한다. (한 단계까지)
    // - 사이드 패널의 폴더 선택 상자로 '전체 문서'·'즐겨찾기' 목록을 폴더별로 볼 수 있다.
    // - '⋯' 버튼: 새 폴더 / 이름 바꾸기 / 삭제 / 탐색기에서 열기
    // - 문서 카드 우클릭 '폴더로 이동'으로 문서를 다른 폴더로 옮긴다.
    public partial class MainForm
    {
        // 폴더 선택 상자의 특별한 값
        private const string FolderFilterAll = "*";   // 모든 폴더
        private const string FolderFilterRoot = "";   // 분류 없음 (문서 폴더 바로 아래)

        // 지금 선택된 폴더 (FolderFilterAll, FolderFilterRoot, 또는 하위 폴더 이름)
        private string currentFolderFilter = FolderFilterAll;

        // 폴더 목록을 채우는 동안 선택 변경 이벤트를 무시
        private bool isFillingFolderList = false;

        private ContextMenuStrip cmsFolder;
        private ToolStripMenuItem tsmiRenameFolder;
        private ToolStripMenuItem tsmiDeleteFolder;
        private ToolStripMenuItem tsmiMoveToFolder;

        // 폴더 선택 상자에 넣는 항목
        private class FolderOption
        {
            public string Text;
            public string Value;

            public override string ToString()
            {
                return Text;
            }
        }

        // 생성자에서 호출
        private void InitializeFolderFeatures()
        {
            cboFolder.SelectedIndexChanged += cboFolder_SelectedIndexChanged;
            btnFolderMenu.Click += (s, e) =>
            {
                UpdateFolderMenuState();
                cmsFolder.Show(btnFolderMenu, new Point(0, btnFolderMenu.Height));
            };

            // '⋯' 메뉴
            cmsFolder = CreateThemedMenu();
            cmsFolder.Items.Add("새 폴더 만들기", null, (s, e) => RunFileAction("새 폴더 만들기", CreateFolder));
            tsmiRenameFolder = (ToolStripMenuItem)cmsFolder.Items.Add("폴더 이름 바꾸기", null,
                (s, e) => RunFileAction("폴더 이름 바꾸기", RenameSelectedFolder));
            tsmiDeleteFolder = (ToolStripMenuItem)cmsFolder.Items.Add("폴더 삭제", null,
                (s, e) => RunFileAction("폴더 삭제", DeleteSelectedFolder));
            cmsFolder.Items.Add(new ToolStripSeparator());
            cmsFolder.Items.Add("탐색기에서 열기", null, (s, e) => RunFileAction("폴더 열기", OpenSelectedFolderInExplorer));
            ApplyMenuItemColors(cmsFolder);
            Disposed += (s, e) => cmsFolder.Dispose();

            // 문서 카드 우클릭 메뉴에 '폴더로 이동' 추가 (이름 바꾸기 아래)
            tsmiMoveToFolder = new ToolStripMenuItem("폴더로 이동");
            tsmiMoveToFolder.ForeColor = cmsRecentDocuments.ForeColor;
            tsmiMoveToFolder.DropDown.BackColor = cmsRecentDocuments.BackColor;
            tsmiMoveToFolder.DropDown.Renderer = cmsRecentDocuments.Renderer;

            int renameIndex = cmsRecentDocuments.Items.IndexOf(tsmiRename);
            cmsRecentDocuments.Items.Insert(renameIndex + 1, tsmiMoveToFolder);

            // 메뉴가 열릴 때마다 폴더 목록으로 하위 메뉴를 만든다.
            cmsRecentDocuments.Opening += (s, e) =>
            {
                try
                {
                    BuildMoveToFolderMenu();
                }
                catch (Exception)
                {
                    tsmiMoveToFolder.Enabled = false;
                }
            };
        }

        // 현재 테마 색을 쓰는 우클릭 메뉴
        private ContextMenuStrip CreateThemedMenu()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Renderer = new ToolStripProfessionalRenderer(new DarkMenuColorTable());
            menu.ShowImageMargin = false;
            menu.ShowCheckMargin = false;
            menu.Font = cmsRecentDocuments.Font;
            menu.BackColor = ThemeManager.C(Color.FromArgb(48, 51, 59));
            menu.ForeColor = ThemeManager.C(Color.FromArgb(230, 230, 234));
            return menu;
        }

        private static void ApplyMenuItemColors(ToolStrip menu)
        {
            foreach (ToolStripItem item in menu.Items)
            {
                item.ForeColor = menu.ForeColor;
            }
        }

        // ───────────── 폴더 목록 ─────────────

        // 문서 폴더 바로 아래의 하위 폴더 이름 (숨김·시스템 폴더와 '.'로 시작하는 폴더 제외)
        private static List<string> GetSubfolderNames(string documentFolder)
        {
            List<string> names = new List<string>();

            if (!Directory.Exists(documentFolder))
                return names;

            foreach (string path in Directory.GetDirectories(documentFolder))
            {
                DirectoryInfo info = new DirectoryInfo(path);

                if (info.Name.StartsWith("."))
                    continue;

                if ((info.Attributes & (FileAttributes.Hidden | FileAttributes.System)) != 0)
                    continue;

                names.Add(info.Name);
            }

            names.Sort(StringComparer.CurrentCulture);
            return names;
        }

        // 문서 폴더 바로 아래 문서 + 하위 폴더의 문서
        private static List<string> GetDocumentsInFolderTree(string documentFolder)
        {
            List<string> documents = new List<string>();

            if (!Directory.Exists(documentFolder))
                return documents;

            documents.AddRange(Directory.GetFiles(documentFolder, "*.rtf"));

            foreach (string name in GetSubfolderNames(documentFolder))
            {
                documents.AddRange(Directory.GetFiles(Path.Combine(documentFolder, name), "*.rtf"));
            }

            return documents;
        }

        // 폴더 선택 상자 다시 채우기. 고를 값이 없어졌으면 '모든 폴더'로 돌아간다.
        private void RefreshFolderList()
        {
            isFillingFolderList = true;

            try
            {
                List<string> names = GetSubfolderNames(DocumentFolderPath);

                cboFolder.Items.Clear();
                cboFolder.Items.Add(new FolderOption { Text = "모든 폴더", Value = FolderFilterAll });
                cboFolder.Items.Add(new FolderOption { Text = "분류 없음", Value = FolderFilterRoot });

                foreach (string name in names)
                {
                    cboFolder.Items.Add(new FolderOption { Text = "  " + name, Value = name });
                }

                bool stillExists = currentFolderFilter == FolderFilterAll ||
                                   currentFolderFilter == FolderFilterRoot ||
                                   names.Contains(currentFolderFilter, StringComparer.OrdinalIgnoreCase);

                if (!stillExists)
                {
                    currentFolderFilter = FolderFilterAll;
                }

                for (int i = 0; i < cboFolder.Items.Count; i++)
                {
                    if (string.Equals(((FolderOption)cboFolder.Items[i]).Value, currentFolderFilter,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        cboFolder.SelectedIndex = i;
                        break;
                    }
                }

                // 최근 문서와 휴지통은 폴더 구분이 없다.
                bool folderApplies = currentDocumentCategory == "all" || currentDocumentCategory == "favorite";
                cboFolder.Enabled = folderApplies;
            }
            finally
            {
                isFillingFolderList = false;
            }
        }

        private void cboFolder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isFillingFolderList)
                return;

            FolderOption option = cboFolder.SelectedItem as FolderOption;

            if (option == null)
                return;

            currentFolderFilter = option.Value;
            LoadRecentDocuments();
        }

        // 지금 고른 폴더 기준으로 문서 목록 가져오기 (전체 문서·즐겨찾기용)
        private List<string> GetDocumentsInCurrentFolder()
        {
            string root = DocumentFolderPath;

            if (currentFolderFilter == FolderFilterAll)
                return GetDocumentsInFolderTree(root);

            if (currentFolderFilter == FolderFilterRoot)
                return Directory.GetFiles(root, "*.rtf").ToList();

            string folder = Path.Combine(root, currentFolderFilter);

            return Directory.Exists(folder)
                ? Directory.GetFiles(folder, "*.rtf").ToList()
                : new List<string>();
        }

        // 새 문서를 저장할 때 처음 열 위치: 폴더를 골라 두었으면 그 폴더
        private string CurrentSaveFolder
        {
            get
            {
                string root = DocumentFolderPath;

                if (currentFolderFilter != FolderFilterAll && currentFolderFilter != FolderFilterRoot)
                {
                    string folder = Path.Combine(root, currentFolderFilter);

                    if (Directory.Exists(folder))
                        return folder;
                }

                return root;
            }
        }

        // 문서가 문서 폴더의 하위 폴더에 있으면 그 폴더 이름, 아니면 null
        private string GetDocumentFolderName(string filePath)
        {
            try
            {
                string parent = Path.GetDirectoryName(filePath);
                string grandParent = Path.GetDirectoryName(parent);

                if (PathHelper.IsSameFolder(grandParent, DocumentFolderPath))
                    return Path.GetFileName(parent);
            }
            catch (Exception)
            {
                // 경로를 해석하지 못하면 폴더 표시를 생략
            }

            return null;
        }

        // ───────────── 폴더 관리 ─────────────

        private string SelectedFolderPath
        {
            get
            {
                if (currentFolderFilter == FolderFilterAll || currentFolderFilter == FolderFilterRoot)
                    return null;

                return Path.Combine(DocumentFolderPath, currentFolderFilter);
            }
        }

        private void UpdateFolderMenuState()
        {
            bool hasFolder = SelectedFolderPath != null;

            tsmiRenameFolder.Enabled = hasFolder;
            tsmiDeleteFolder.Enabled = hasFolder;
        }

        // 폴더 이름 검사. 문제가 없으면 null, 있으면 안내 문구
        private string ValidateFolderName(string name, string currentName)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "폴더 이름을 입력하세요.";

            if (name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                return "폴더 이름에 사용할 수 없는 문자가 있습니다. (\\ / : * ? \" < > |)";

            if (name.StartsWith(".") || name.EndsWith("."))
                return "폴더 이름은 '.'으로 시작하거나 끝날 수 없습니다.";

            bool exists = GetSubfolderNames(DocumentFolderPath)
                .Any(existing => string.Equals(existing, name, StringComparison.OrdinalIgnoreCase) &&
                                 !string.Equals(existing, currentName, StringComparison.OrdinalIgnoreCase));

            if (exists)
                return "같은 이름의 폴더가 이미 있습니다.";

            return null;
        }

        // 간단한 입력 창. 취소하면 null
        private string PromptText(string title, string message, string initialText)
        {
            using (Form form = new Form())
            using (Label label = new Label())
            using (TextBox textBox = new TextBox())
            using (Button okButton = new Button())
            using (Button cancelButton = new Button())
            {
                form.Text = title;
                AppIcon.Apply(form);
                form.ClientSize = new Size(340, 120);
                form.StartPosition = FormStartPosition.CenterParent;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.MaximizeBox = false;
                form.MinimizeBox = false;
                form.ShowInTaskbar = false;

                label.Text = message;
                label.Location = new Point(15, 12);
                label.AutoSize = true;

                textBox.Text = initialText ?? "";
                textBox.Location = new Point(15, 40);
                textBox.Size = new Size(310, 25);

                okButton.Text = "확인";
                okButton.DialogResult = DialogResult.OK;
                okButton.Location = new Point(165, 80);

                cancelButton.Text = "취소";
                cancelButton.DialogResult = DialogResult.Cancel;
                cancelButton.Location = new Point(250, 80);

                form.Controls.AddRange(new Control[] { label, textBox, okButton, cancelButton });
                form.AcceptButton = okButton;
                form.CancelButton = cancelButton;

                textBox.SelectAll();

                return form.ShowDialog(this) == DialogResult.OK ? textBox.Text.Trim() : null;
            }
        }

        // 새 폴더 만들기. 만든 폴더 이름을 돌려준다. (취소하면 null)
        private string CreateFolderCore()
        {
            string name = "";

            while (true)
            {
                name = PromptText("새 폴더", "새 폴더 이름을 입력하세요.", name);

                if (name == null)
                    return null;

                string error = ValidateFolderName(name, null);

                if (error == null)
                    break;

                MessageBox.Show(error, "새 폴더", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            Directory.CreateDirectory(Path.Combine(DocumentFolderPath, name));
            return name;
        }

        private void CreateFolder()
        {
            string name = CreateFolderCore();

            if (name == null)
                return;

            // 만든 폴더를 바로 보여 준다. (새 문서를 저장하면 이 폴더에 저장됨)
            if (currentDocumentCategory != "all" && currentDocumentCategory != "favorite")
            {
                currentDocumentCategory = "all";
                UpdateCategoryButtonStyle();
            }

            currentFolderFilter = name;
            LoadRecentDocuments();
        }

        private void RenameSelectedFolder()
        {
            string oldPath = SelectedFolderPath;

            if (oldPath == null)
                return;

            string oldName = Path.GetFileName(oldPath);
            string newName = oldName;

            while (true)
            {
                newName = PromptText("폴더 이름 바꾸기", "새 폴더 이름을 입력하세요.", newName);

                if (newName == null || newName == oldName)
                    return;

                string error = ValidateFolderName(newName, oldName);

                if (error == null)
                    break;

                MessageBox.Show(error, "폴더 이름 바꾸기", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            string newPath = Path.Combine(DocumentFolderPath, newName);

            // 대소문자만 바꾸는 경우 Windows에서는 한 번에 옮길 수 없어 임시 이름을 거친다.
            if (string.Equals(oldName, newName, StringComparison.OrdinalIgnoreCase))
            {
                string tempPath = Path.Combine(DocumentFolderPath, "." + Guid.NewGuid().ToString("N"));
                Directory.Move(oldPath, tempPath);
                Directory.Move(tempPath, newPath);
            }
            else
            {
                Directory.Move(oldPath, newPath);
            }

            // 이 폴더 안의 문서를 가리키던 경로를 모두 새 경로로 바꾼다.
            RecentDocumentManager.ReplaceFolderPath(oldPath, newPath);
            TrashManager.ReplaceOriginalFolder(oldPath, newPath);
            currentFilePath = PathHelper.ReplaceFolderInPath(currentFilePath, oldPath, newPath);
            selectedDocumentPath = PathHelper.ReplaceFolderInPath(selectedDocumentPath, oldPath, newPath);

            currentFolderFilter = newName;
            LoadRecentDocuments();
        }

        // 폴더 삭제: 안의 문서는 휴지통으로 옮기고, 폴더가 비면 지운다.
        private void DeleteSelectedFolder()
        {
            string folderPath = SelectedFolderPath;

            if (folderPath == null)
                return;

            string folderName = Path.GetFileName(folderPath);
            string[] documents = Directory.Exists(folderPath)
                ? Directory.GetFiles(folderPath, "*.rtf")
                : new string[0];

            string message = documents.Length == 0
                ? "'" + folderName + "' 폴더를 삭제하시겠습니까?"
                : "'" + folderName + "' 폴더를 삭제하시겠습니까?\n\n" +
                  "폴더 안의 문서 " + documents.Length + "개는 휴지통으로 옮깁니다. (휴지통에서 복원 가능)";

            if (MessageBox.Show(message, "폴더 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                != DialogResult.Yes)
            {
                return;
            }

            // 열려 있는 문서가 이 폴더에 있으면 저장하지 않은 내용부터 처리한다.
            bool openDocumentInFolder = PathHelper.IsInFolder(currentFilePath, folderPath);

            if (openDocumentInFolder && !ConfirmSaveChanges())
                return;

            foreach (string document in documents)
            {
                TrashManager.MoveToTrash(document, ReadDocumentTitle(document));
                RecentDocumentManager.RemoveRecentDocument(document);
            }

            if (openDocumentInFolder)
            {
                CreateNewDocument();
            }

            bool deleted = TryDeleteEmptyFolder(folderPath);

            currentFolderFilter = FolderFilterAll;
            LoadRecentDocuments();

            if (!deleted && Directory.Exists(folderPath))
            {
                MessageBox.Show(
                    "문서는 휴지통으로 옮겼지만, 폴더 안에 다른 파일이 남아 있어 폴더는 지우지 않았습니다.\n\n" +
                    folderPath,
                    "폴더 삭제",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        private void OpenSelectedFolderInExplorer()
        {
            string folder = SelectedFolderPath ?? DocumentFolderPath;

            Directory.CreateDirectory(folder);
            System.Diagnostics.Process.Start("explorer.exe", "\"" + folder + "\"");
        }

        // 문서 정보(JSON)의 제목. 읽지 못하면 파일 이름
        private static string ReadDocumentTitle(string rtfPath)
        {
            string title = Path.GetFileNameWithoutExtension(rtfPath);

            try
            {
                string jsonPath = Path.ChangeExtension(rtfPath, ".json");

                if (File.Exists(jsonPath))
                {
                    DocumentInfo info = JsonConvert.DeserializeObject<DocumentInfo>(File.ReadAllText(jsonPath));

                    if (info != null && !string.IsNullOrWhiteSpace(info.Title))
                        title = info.Title;
                }
            }
            catch (Exception)
            {
                // 파일 이름 사용
            }

            return title;
        }

        // ───────────── 문서를 다른 폴더로 이동 ─────────────

        private void BuildMoveToFolderMenu()
        {
            foreach (ToolStripItem old in tsmiMoveToFolder.DropDownItems.Cast<ToolStripItem>().ToList())
            {
                tsmiMoveToFolder.DropDownItems.Remove(old);
                old.Dispose();
            }

            string documentPath = menuDocumentPath;
            bool canMove = !string.IsNullOrEmpty(documentPath) && currentDocumentCategory != "trash";

            tsmiMoveToFolder.Enabled = canMove;

            if (!canMove)
                return;

            string root = DocumentFolderPath;
            string currentFolder = Path.GetDirectoryName(documentPath);

            Color backColor = ThemeManager.C(Color.FromArgb(48, 51, 59));
            Color foreColor = ThemeManager.C(Color.FromArgb(230, 230, 234));

            tsmiMoveToFolder.DropDown.BackColor = backColor;

            List<KeyValuePair<string, string>> targets = new List<KeyValuePair<string, string>>();
            targets.Add(new KeyValuePair<string, string>("분류 없음", root));

            foreach (string name in GetSubfolderNames(root))
            {
                targets.Add(new KeyValuePair<string, string>(name, Path.Combine(root, name)));
            }

            foreach (KeyValuePair<string, string> target in targets)
            {
                string targetFolder = target.Value;

                ToolStripMenuItem item = new ToolStripMenuItem(target.Key);
                item.ForeColor = foreColor;

                // 지금 있는 폴더는 고를 필요가 없다.
                if (PathHelper.IsSameFolder(targetFolder, currentFolder))
                {
                    item.Enabled = false;
                    item.Text += "  (현재)";
                }

                item.Click += (s, e) =>
                    RunFileAction("폴더로 이동", () => MoveDocumentToFolder(documentPath, targetFolder));

                tsmiMoveToFolder.DropDownItems.Add(item);
            }

            tsmiMoveToFolder.DropDownItems.Add(new ToolStripSeparator());

            ToolStripMenuItem newFolderItem = new ToolStripMenuItem("새 폴더 만들어 이동...");
            newFolderItem.ForeColor = foreColor;
            newFolderItem.Click += (s, e) => RunFileAction("폴더로 이동", () =>
            {
                string name = CreateFolderCore();

                if (name != null)
                    MoveDocumentToFolder(documentPath, Path.Combine(DocumentFolderPath, name));
            });

            tsmiMoveToFolder.DropDownItems.Add(newFolderItem);
        }

        // 문서 하나를 다른 폴더로 옮긴다. (같은 이름이 있으면 옮기지 않고 알린다)
        private void MoveDocumentToFolder(string documentPath, string targetFolder)
        {
            List<string> skipped;
            int moved = MoveDocuments(new[] { documentPath }, targetFolder, out skipped);

            LoadRecentDocuments();

            if (moved == 0 && skipped.Count > 0)
            {
                MessageBox.Show(
                    "문서를 옮기지 못했습니다.\n\n" + string.Join("\n", skipped),
                    "폴더로 이동",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }
    }
}
