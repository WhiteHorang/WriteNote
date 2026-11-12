using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace WriteNote
{
    // 문서 저장·불러오기·새 문서, 문서 폴더, 문서 폴더를 바꿀 때 문서 옮기기
    public partial class MainForm
    {

        // 기존 문서 폴더의 문서(.rtf와 같은 이름의 .json)를 새 폴더로 옮길지 묻고 옮긴다.
        private void OfferToMoveDocuments(string oldFolder, string newFolder)
        {
            string[] documents;

            try
            {
                if (!Directory.Exists(oldFolder))
                {
                    return;
                }

                // 문서 폴더 바로 아래 문서 + 하위 폴더(분류)의 문서
                documents = GetDocumentsInFolderTree(oldFolder).ToArray();
            }
            catch (Exception)
            {
                return;
            }

            if (documents.Length == 0)
            {
                return;
            }

            DialogResult answer = MessageBox.Show(
                "문서 저장 폴더가 바뀌었습니다.\n\n" +
                "이전 폴더: " + oldFolder + "\n" +
                "새 폴더: " + newFolder + "\n\n" +
                "이전 폴더에 있는 문서 " + documents.Length + "개를 새 폴더로 옮기시겠습니까?\n\n" +
                "옮기지 않으면 이전 문서는 '전체 문서'와 '즐겨찾기' 목록에 보이지 않지만,\n" +
                "열기 메뉴나 최근 문서에서는 계속 열 수 있습니다.",
                "문서 옮기기",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (answer != DialogResult.Yes)
            {
                return;
            }

            List<string> skipped;
            int movedCount;

            try
            {
                movedCount = MoveDocumentTree(oldFolder, newFolder, out skipped);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "새 문서 폴더를 만들 수 없어 문서를 옮기지 못했습니다.\n\n" + ex.Message,
                    "문서 옮기기",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            string message = "문서 " + movedCount + "개를 새 폴더로 옮겼습니다.";

            if (skipped.Count > 0)
            {
                message += "\n\n옮기지 못한 문서 " + skipped.Count + "개 (이전 폴더에 그대로 있습니다):\n- " +
                           string.Join("\n- ", skipped);
            }

            MessageBox.Show(
                message,
                "문서 옮기기",
                MessageBoxButtons.OK,
                skipped.Count > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information
            );
        }


        // 문서 폴더 전체(바로 아래 문서 + 하위 폴더별 문서)를 새 문서 폴더로 옮긴다.
        // 하위 폴더는 새 문서 폴더 안에 같은 이름으로 만든다. 옮긴 문서 수를 돌려준다.
        private int MoveDocumentTree(string oldFolder, string newFolder, out List<string> skipped)
        {
            skipped = new List<string>();

            List<string> skippedHere;
            int movedCount = MoveDocuments(Directory.GetFiles(oldFolder, "*.rtf"), newFolder, out skippedHere);
            skipped.AddRange(skippedHere);

            foreach (string subfolder in GetSubfolderNames(oldFolder))
            {
                string oldSub = Path.Combine(oldFolder, subfolder);
                string[] documents = Directory.GetFiles(oldSub, "*.rtf");

                if (documents.Length == 0)
                    continue;

                movedCount += MoveDocuments(documents, Path.Combine(newFolder, subfolder), out skippedHere);
                skipped.AddRange(skippedHere.Select(item => subfolder + "\\" + item));

                // 다 옮겨서 비었으면 이전 하위 폴더는 정리한다.
                TryDeleteEmptyFolder(oldSub);
            }

            return movedCount;
        }


        // 폴더가 완전히 비어 있으면 지운다. (다른 파일이 있으면 그대로 둔다)
        private static bool TryDeleteEmptyFolder(string folder)
        {
            try
            {
                if (Directory.Exists(folder) && !Directory.EnumerateFileSystemEntries(folder).Any())
                {
                    Directory.Delete(folder);
                    return true;
                }
            }
            catch (Exception)
            {
                // 지우지 못해도 문서에는 영향이 없다.
            }

            return false;
        }


        // 문서(.rtf와 같은 이름의 .json)를 새 폴더로 옮긴다. 옮긴 문서 수를 돌려준다.
        // - 새 폴더에 같은 이름이 있으면 덮어쓰지 않고 skipped에 넣는다.
        // - 최근 문서 목록, 열려 있는 문서, 선택된 문서의 경로도 새 경로로 바꾼다.
        // - 새 폴더를 만들 수 없으면 예외를 던진다.
        private int MoveDocuments(string[] documents, string newFolder, out List<string> skipped)
        {
            int movedCount = 0;
            skipped = new List<string>();

            Directory.CreateDirectory(newFolder);

            foreach (string oldRtfPath in documents)
            {
                string fileName = Path.GetFileName(oldRtfPath);
                string newRtfPath = Path.Combine(newFolder, fileName);

                string oldJsonPath = Path.ChangeExtension(oldRtfPath, ".json");
                string newJsonPath = Path.ChangeExtension(newRtfPath, ".json");

                // 새 폴더에 같은 이름의 문서가 있으면 덮어쓰지 않고 건너뛴다.
                if (File.Exists(newRtfPath) || File.Exists(newJsonPath))
                {
                    skipped.Add(fileName + " (같은 이름의 문서가 이미 있음)");
                    continue;
                }

                try
                {
                    File.Move(oldRtfPath, newRtfPath);
                }
                catch (Exception ex)
                {
                    skipped.Add(fileName + " (" + ex.Message + ")");
                    continue;
                }

                if (File.Exists(oldJsonPath))
                {
                    try
                    {
                        File.Move(oldJsonPath, newJsonPath);
                    }
                    catch (Exception ex)
                    {
                        // 문서 정보(.json)를 옮기지 못하면 문서도 원래 자리로 되돌려
                        // 제목·즐겨찾기 정보와 문서가 갈라지지 않게 한다.
                        try
                        {
                            File.Move(newRtfPath, oldRtfPath);
                        }
                        catch (Exception)
                        {
                            // 되돌리지 못해도 문서 내용은 새 폴더에 그대로 있다.
                        }

                        skipped.Add(fileName + " (" + ex.Message + ")");
                        continue;
                    }
                }

                movedCount++;

                // 옮긴 문서를 가리키던 경로를 모두 새 경로로 바꾼다.
                try
                {
                    RecentDocumentManager.ReplacePathIfExists(oldRtfPath, newRtfPath);
                }
                catch (Exception)
                {
                    // 최근 문서 목록 갱신에 실패해도 문서 이동은 완료된 상태다.
                }

                if (string.Equals(currentFilePath, oldRtfPath, StringComparison.OrdinalIgnoreCase))
                {
                    currentFilePath = newRtfPath;
                }

                if (string.Equals(selectedDocumentPath, oldRtfPath, StringComparison.OrdinalIgnoreCase))
                {
                    selectedDocumentPath = newRtfPath;
                }
            }

            return movedCount;
        }


        

        // 설정한 문서 폴더를 사용할 수 없다는 안내를 이미 했는지 여부
        private bool documentFolderWarningShown = false;


        // 공통 문서 저장 폴더 (설정 > 저장 및 문서에서 변경)
        // 설정한 폴더를 쓸 수 없으면(USB 분리, 권한 없음 등) 기본 위치를 사용한다.
        private string DocumentFolderPath
        {
            get
            {
                string folder = appSettings.Storage.EffectiveDocumentFolder;

                try
                {
                    Directory.CreateDirectory(folder);
                    return folder;
                }
                catch (Exception ex)
                {
                    string defaultFolder = StorageSettings.DefaultDocumentFolder;

                    if (!documentFolderWarningShown)
                    {
                        documentFolderWarningShown = true;

                        MessageBox.Show(
                            "설정한 문서 폴더를 사용할 수 없어 기본 위치를 사용합니다.\n\n" +
                            "설정한 폴더: " + folder + "\n" +
                            "기본 위치: " + defaultFolder + "\n\n" +
                            ex.Message,
                            "문서 폴더",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                    }

                    Directory.CreateDirectory(defaultFolder);
                    return defaultFolder;
                }
            }
        }


        // 문서 저장
        private bool SaveDocument()
        {
            string savePath = currentFilePath;

            try
            {
                // 처음 저장하는 문서라면 저장 위치 선택
                if (string.IsNullOrEmpty(savePath))
                {
                    using (SaveFileDialog dialog = new SaveFileDialog())
                    {
                        dialog.Title = "WriteNote 문서 저장";
                        dialog.Filter = "서식 있는 텍스트 파일 (*.rtf)|*.rtf";
                        dialog.DefaultExt = "rtf";
                        dialog.AddExtension = true;

                        // 기본 저장 위치 지정
                        dialog.InitialDirectory = CurrentSaveFolder;

                        string title = txtTitle.Text.Trim();

                        if (string.IsNullOrEmpty(title))
                        {
                            title = "새 문서";
                        }

                        dialog.FileName = title;

                        if (dialog.ShowDialog() != DialogResult.OK)
                        {
                            return false;
                        }

                        savePath = dialog.FileName;
                    }
                }

                // RTF 파일 저장
                rtbContent.SaveFile(
                    savePath,
                    RichTextBoxStreamType.RichText
                );

                // JSON 문서 정보 저장
                SaveDocumentInfo(savePath);

                // 현재 파일 경로 갱신
                currentFilePath = savePath;

                // 최근 문서 등록 및 목록 갱신
                RegisterRecentDocument(savePath);

                // 저장 성공 후 상태 변경
                isModified = false;
                lblSaveStatus.Text = "저장됨";

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "문서를 저장하는 중 오류가 발생했습니다.\n\n" + ex.Message,
                    "저장 오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return false;
            }
        }


        // 저장 버튼
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveDocument();
        }


        // 열기 버튼
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenDocument();
        }


        // 새 문서 버튼
        private void btnNew_Click(object sender, EventArgs e)
        {
            if (!ConfirmSaveChanges())
            {
                return;
            }

            CreateNewDocument();
        }


        // 다른 이름으로 저장 버튼
        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            SaveAsDocument();
        }




        // 문서 열기
        private void OpenDocument()
        {
            if (!ConfirmSaveChanges())
            {
                return;
            }

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "WriteNote 문서 열기";
                dialog.Filter = "서식 있는 텍스트 파일 (*.rtf)|*.rtf";

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                LoadDocument(dialog.FileName);
            }
        }


        // 공통 문서 불러오기
        private void LoadDocument(string filePath)
        {
            try
            {
                // 먼저 임시 RichTextBox로 읽어 본다.
                // 읽기에 실패하면 지금 화면의 문서와 파일 경로는 그대로 남는다.
                // (실패한 문서 내용이 이전 문서 경로로 저장되는 일을 막는다)
                string rtf;

                using (RichTextBox loader = new RichTextBox())
                {
                    loader.LoadFile(filePath, RichTextBoxStreamType.RichText);
                    rtf = loader.Rtf;
                }

                // 제목은 문서 정보(JSON)에서 읽고, 읽지 못하면 파일 이름을 쓴다.
                string documentTitle =
                    Path.GetFileNameWithoutExtension(filePath);

                string jsonPath =
                    Path.ChangeExtension(filePath, ".json");

                try
                {
                    if (File.Exists(jsonPath))
                    {
                        string json = File.ReadAllText(jsonPath);

                        DocumentInfo info =
                            JsonConvert.DeserializeObject<DocumentInfo>(json);

                        if (info != null &&
                            !string.IsNullOrWhiteSpace(info.Title))
                        {
                            documentTitle = info.Title;
                        }
                    }
                }
                catch (Exception)
                {
                    // 문서 정보가 손상되었으면 파일 이름을 제목으로 사용
                }

                // 여기부터 화면에 반영
                rtbContent.Rtf = rtf;

                // 문서에 저장된 글꼴·글자색 대신 현재 설정의 글꼴과 테마 색으로 표시
                ApplyEditorTextStyle();
                rtbContent.ClearUndo();

                currentFilePath = filePath;
                txtTitle.Text = documentTitle;

                isModified = false;
                lblSaveStatus.Text = "저장됨";

                UpdateCharCount();

                RegisterRecentDocument(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "문서를 불러오는 중 오류가 발생했습니다.\n\n"
                    + ex.Message,
                    "문서 열기 오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }


        // 저장 여부 확인
        private bool ConfirmSaveChanges()
        {
            if (!isModified)
            {
                return true;
            }

            DialogResult result = MessageBox.Show(
                "저장하지 않은 변경 사항이 있습니다.\n저장하시겠습니까?",
                "WriteNote",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                return SaveDocument();
            }
            else if (result == DialogResult.No)
            {
                return true;
            }

            return false;
        }


        // 새 문서 생성
        private void CreateNewDocument()
        {
            currentFilePath = "";

            txtTitle.Clear();
            rtbContent.Clear();

            isModified = false;

            lblSaveStatus.Text = "새 문서";

            UpdateCharCount();
        }


        // 다른 이름으로 저장
        private bool SaveAsDocument()
        {
            try
            {
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.Title = "WriteNote 다른 이름으로 저장";
                    dialog.Filter = "서식 있는 텍스트 파일 (*.rtf)|*.rtf";
                    dialog.DefaultExt = "rtf";
                    dialog.AddExtension = true;
                    dialog.OverwritePrompt = true;

                    dialog.InitialDirectory = CurrentSaveFolder;

                    string title = txtTitle.Text.Trim();

                    if (string.IsNullOrEmpty(title))
                    {
                        title = "새 문서";
                    }

                    dialog.FileName = title;

                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        return false;
                    }

                    rtbContent.SaveFile(
                        dialog.FileName,
                        RichTextBoxStreamType.RichText
                    );

                    SaveDocumentInfo(dialog.FileName);

                    currentFilePath = dialog.FileName;

                    RegisterRecentDocument(dialog.FileName);

                    isModified = false;
                    lblSaveStatus.Text = "저장됨";

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "문서를 다른 이름으로 저장하는 중 오류가 발생했습니다.\n\n"
                    + ex.Message,
                    "저장 오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return false;
            }
        }


        // JSON 문서 정보 저장
        private void SaveDocumentInfo(string rtfPath)
        {
            string jsonPath = Path.ChangeExtension(rtfPath, ".json");

            DocumentInfo info;

            if (File.Exists(jsonPath))
            {
                string existingJson = File.ReadAllText(jsonPath);

                info = JsonConvert.DeserializeObject<DocumentInfo>(
                    existingJson
                );

                if (info == null)
                {
                    info = new DocumentInfo();
                }

                if (info.CreatedAt == default(DateTime))
                {
                    info.CreatedAt = DateTime.Now;
                }
            }
            else
            {
                info = new DocumentInfo();
                info.CreatedAt = DateTime.Now;
                info.IsFavorite = false;
            }

            info.Title = txtTitle.Text.Trim();
            info.FileName = Path.GetFileName(rtfPath);
            info.ModifiedAt = DateTime.Now;

            string json = JsonConvert.SerializeObject(
                info,
                Formatting.Indented
            );

            File.WriteAllText(jsonPath, json);
        }


        // 최근 문서 등록
        private void RegisterRecentDocument(string filePath)
        {
            try
            {
                RecentDocumentManager.AddRecentDocument(filePath);

                LoadRecentDocuments();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "문서는 정상적으로 처리되었지만 최근 문서 목록을 업데이트하지 못했습니다.\n\n"
                    + ex.Message,
                    "최근 문서 오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }
    }
}
