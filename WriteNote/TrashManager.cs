using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace WriteNote
{
    // 휴지통에 있는 문서 한 개의 정보
    public class TrashItem
    {
        // 휴지통 안의 파일 이름에 쓰는 고유 값
        public string Id { get; set; } = "";

        // 문서 제목 (목록 표시용)
        public string Title { get; set; } = "";

        // 삭제하기 전 원래 위치 (복원할 때 이 자리로 되돌린다)
        public string OriginalPath { get; set; } = "";

        // 휴지통으로 옮긴 시각
        public DateTime DeletedAt { get; set; }

        // 휴지통 안의 문서 파일 경로
        [JsonIgnore]
        public string TrashRtfPath
        {
            get { return Path.Combine(TrashManager.TrashFolderPath, Id + ".rtf"); }
        }
    }

    // 삭제한 문서를 바로 지우지 않고 휴지통에 보관한다.
    // 저장 위치: %AppData%\WriteNote\Trash
    // - {Id}.rtf        : 문서 내용
    // - {Id}.doc.json   : 문서 정보 (제목, 즐겨찾기 등. 원래 있던 경우만)
    // - {Id}.trash.json : 휴지통 정보 (원래 위치, 삭제 시각)
    public static class TrashManager
    {
        public static string TrashFolderPath
        {
            get { return Path.Combine(SettingsManager.DataFolderPath, "Trash"); }
        }

        private static string DocInfoPath(string id)
        {
            return Path.Combine(TrashFolderPath, id + ".doc.json");
        }

        private static string TrashInfoPath(string id)
        {
            return Path.Combine(TrashFolderPath, id + ".trash.json");
        }

        // 문서를 휴지통으로 옮긴다.
        public static TrashItem MoveToTrash(string rtfPath, string title)
        {
            string fullPath = Path.GetFullPath(rtfPath);
            string jsonPath = Path.ChangeExtension(fullPath, ".json");

            Directory.CreateDirectory(TrashFolderPath);

            TrashItem item = new TrashItem
            {
                Id = Guid.NewGuid().ToString("N"),
                Title = string.IsNullOrWhiteSpace(title)
                    ? Path.GetFileNameWithoutExtension(fullPath)
                    : title,
                OriginalPath = fullPath,
                DeletedAt = DateTime.Now
            };

            // 휴지통 정보를 먼저 기록해 두면, 도중에 문제가 생겨도 무엇을 옮기던 중인지 알 수 있다.
            WriteTrashInfo(item);

            try
            {
                File.Move(fullPath, item.TrashRtfPath);
            }
            catch (Exception)
            {
                File.Delete(TrashInfoPath(item.Id));
                throw;
            }

            if (File.Exists(jsonPath))
            {
                try
                {
                    File.Move(jsonPath, DocInfoPath(item.Id));
                }
                catch (Exception)
                {
                    // 문서 정보를 옮기지 못하면 문서도 원래 자리로 되돌린다.
                    File.Move(item.TrashRtfPath, fullPath);
                    File.Delete(TrashInfoPath(item.Id));
                    throw;
                }
            }

            return item;
        }

        // 휴지통 목록 (최근에 삭제한 순서)
        public static List<TrashItem> GetItems()
        {
            List<TrashItem> items = new List<TrashItem>();

            if (!Directory.Exists(TrashFolderPath))
                return items;

            foreach (string infoPath in Directory.GetFiles(TrashFolderPath, "*.trash.json"))
            {
                try
                {
                    TrashItem item = JsonConvert.DeserializeObject<TrashItem>(
                        File.ReadAllText(infoPath));

                    // 문서 파일이 남아 있는 항목만 보여 준다.
                    if (item != null &&
                        !string.IsNullOrEmpty(item.Id) &&
                        File.Exists(item.TrashRtfPath))
                    {
                        items.Add(item);
                    }
                }
                catch (Exception)
                {
                    // 읽을 수 없는 휴지통 정보는 건너뛴다.
                }
            }

            return items.OrderByDescending(item => item.DeletedAt).ToList();
        }

        // 휴지통 안의 문서 파일 경로로 항목 찾기
        public static TrashItem FindByTrashPath(string trashRtfPath)
        {
            return GetItems().FirstOrDefault(item =>
                string.Equals(item.TrashRtfPath, trashRtfPath, StringComparison.OrdinalIgnoreCase));
        }

        // 문서를 원래 위치로 되돌리고, 실제로 복원된 경로를 돌려준다.
        // 원래 위치에 같은 이름의 문서가 생겼으면 "제목 (2).rtf"처럼 이름을 바꿔서 복원한다.
        public static string Restore(TrashItem item)
        {
            string folder = Path.GetDirectoryName(item.OriginalPath);
            string baseName = Path.GetFileNameWithoutExtension(item.OriginalPath);

            Directory.CreateDirectory(folder);

            string targetPath = item.OriginalPath;
            int number = 2;

            while (File.Exists(targetPath) ||
                   File.Exists(Path.ChangeExtension(targetPath, ".json")))
            {
                targetPath = Path.Combine(folder, baseName + " (" + number + ").rtf");
                number++;
            }

            string targetJsonPath = Path.ChangeExtension(targetPath, ".json");
            string docInfoPath = DocInfoPath(item.Id);

            File.Move(item.TrashRtfPath, targetPath);

            if (File.Exists(docInfoPath))
            {
                try
                {
                    File.Move(docInfoPath, targetJsonPath);
                }
                catch (Exception)
                {
                    // 문서 정보를 옮기지 못하면 문서도 휴지통으로 되돌린다.
                    File.Move(targetPath, item.TrashRtfPath);
                    throw;
                }

                // 이름을 바꿔서 복원했으면 문서 정보의 파일 이름도 맞춘다.
                if (!string.Equals(targetPath, item.OriginalPath, StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        DocumentInfo info = JsonConvert.DeserializeObject<DocumentInfo>(
                            File.ReadAllText(targetJsonPath));

                        if (info != null)
                        {
                            info.FileName = Path.GetFileName(targetPath);
                            File.WriteAllText(targetJsonPath,
                                JsonConvert.SerializeObject(info, Formatting.Indented));
                        }
                    }
                    catch (Exception)
                    {
                        // 파일 이름 정보만 갱신하지 못한 것이므로 복원은 완료된 상태다.
                    }
                }
            }

            DeleteIfExists(TrashInfoPath(item.Id));

            return targetPath;
        }

        // 휴지통에서 영구 삭제 (복구할 수 없음)
        public static void DeletePermanently(TrashItem item)
        {
            DeleteIfExists(item.TrashRtfPath);
            DeleteIfExists(DocInfoPath(item.Id));
            DeleteIfExists(TrashInfoPath(item.Id));
        }

        // 휴지통 비우기. 지운 문서 수를 돌려준다.
        public static int EmptyTrash()
        {
            int count = 0;

            foreach (TrashItem item in GetItems())
            {
                DeletePermanently(item);
                count++;
            }

            return count;
        }

        // 폴더 이름을 바꿨을 때, 그 폴더에서 삭제한 문서의 복원 위치도 새 폴더로 바꾼다.
        public static void ReplaceOriginalFolder(string oldFolder, string newFolder)
        {
            foreach (TrashItem item in GetItems())
            {
                if (PathHelper.IsInFolder(item.OriginalPath, oldFolder))
                {
                    item.OriginalPath = PathHelper.ReplaceFolderInPath(item.OriginalPath, oldFolder, newFolder);
                    WriteTrashInfo(item);
                }
            }
        }

        private static void WriteTrashInfo(TrashItem item)
        {
            File.WriteAllText(TrashInfoPath(item.Id),
                JsonConvert.SerializeObject(item, Formatting.Indented));
        }

        private static void DeleteIfExists(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
