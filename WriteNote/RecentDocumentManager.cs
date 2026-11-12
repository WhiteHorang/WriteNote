
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace WriteNote
{
    public static class RecentDocumentManager
    {
        // 최근 문서 최대 저장 개수
        private const int MaxRecentDocuments = 10;

        // WriteNote 데이터 폴더 경로 (%AppData%\WriteNote, 설정 파일과 같은 곳)
        private static string DataFolderPath
        {
            get { return SettingsManager.DataFolderPath; }
        }

        // 최근 문서 JSON 파일 경로
        private static string RecentFilePath
        {
            get
            {
                return Path.Combine(
                    DataFolderPath,
                    "recent_documents.json"
                );
            }
        }

        // 최근 문서 목록 불러오기
        public static List<string> GetRecentDocuments()
        {
            if (!File.Exists(RecentFilePath))
            {
                return new List<string>();
            }

            List<string> documents;

            try
            {
                string json = File.ReadAllText(RecentFilePath);

                documents =
                    JsonConvert.DeserializeObject<List<string>>(json);
            }
            catch (Exception)
            {
                // 목록 파일이 손상되었으면 빈 목록으로 시작한다.
                // (다음에 문서를 저장하거나 열면 새 목록으로 다시 만들어진다)
                return new List<string>();
            }

            if (documents == null)
            {
                return new List<string>();
            }

            return documents;
        }

        // 최근 문서 등록
        public static void AddRecentDocument(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return;
            }

            // 전체 경로로 변환
            string fullPath = Path.GetFullPath(filePath);

            // 기존 목록 불러오기
            List<string> documents = GetRecentDocuments();

            // 같은 경로가 이미 있다면 제거
            documents.RemoveAll(path =>
                string.Equals(
                    path,
                    fullPath,
                    StringComparison.OrdinalIgnoreCase
                )
            );

            // 가장 최근 문서를 맨 앞에 추가
            documents.Insert(0, fullPath);

            // 최대 개수 제한
            if (documents.Count > MaxRecentDocuments)
            {
                documents.RemoveRange(
                    MaxRecentDocuments,
                    documents.Count - MaxRecentDocuments
                );
            }

            // 변경된 목록 저장
            SaveRecentDocuments(documents);
        }

        // 최근 문서 경로 변경
        public static void RenameRecentDocument(
            string oldPath,
            string newPath)
        {
            if (string.IsNullOrWhiteSpace(oldPath) ||
                string.IsNullOrWhiteSpace(newPath))
            {
                return;
            }

            // 전체 경로로 변환
            string oldFullPath = Path.GetFullPath(oldPath);
            string newFullPath = Path.GetFullPath(newPath);

            // 기존 목록 불러오기
            List<string> documents = GetRecentDocuments();

            // 기존 경로가 있는 위치 찾기
            int index = documents.FindIndex(path =>
                string.Equals(
                    path,
                    oldFullPath,
                    StringComparison.OrdinalIgnoreCase
                )
            );

            // 기존 경로가 없다면 새 경로 등록
            if (index < 0)
            {
                AddRecentDocument(newFullPath);
                return;
            }

            // 기존 경로를 새 경로로 변경
            documents[index] = newFullPath;

            // 중복 경로 제거
            documents = documents
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            // 변경된 목록 저장
            SaveRecentDocuments(documents);
        }

        // 최근 문서 목록에 있는 경우에만 경로를 바꾼다. (목록 순서는 그대로)
        // 문서 폴더를 옮길 때 사용한다.
        public static void ReplacePathIfExists(
            string oldPath,
            string newPath)
        {
            if (string.IsNullOrWhiteSpace(oldPath) ||
                string.IsNullOrWhiteSpace(newPath))
            {
                return;
            }

            string oldFullPath = Path.GetFullPath(oldPath);
            string newFullPath = Path.GetFullPath(newPath);

            List<string> documents = GetRecentDocuments();

            int index = documents.FindIndex(path =>
                string.Equals(
                    path,
                    oldFullPath,
                    StringComparison.OrdinalIgnoreCase
                )
            );

            if (index < 0)
            {
                return;
            }

            documents[index] = newFullPath;

            SaveRecentDocuments(documents);
        }

        // 폴더 이름을 바꿨을 때, 그 폴더 안 문서의 경로를 새 폴더 경로로 바꾼다.
        public static void ReplaceFolderPath(string oldFolder, string newFolder)
        {
            List<string> documents = GetRecentDocuments();
            bool changed = false;

            for (int i = 0; i < documents.Count; i++)
            {
                if (PathHelper.IsInFolder(documents[i], oldFolder))
                {
                    documents[i] = PathHelper.ReplaceFolderInPath(documents[i], oldFolder, newFolder);
                    changed = true;
                }
            }

            if (changed)
            {
                SaveRecentDocuments(documents);
            }
        }

        // 최근 문서 목록 저장
        private static void SaveRecentDocuments(
            List<string> documents)
        {
            // 데이터 폴더가 없으면 생성
            Directory.CreateDirectory(DataFolderPath);

            // JSON 문자열로 변환
            string json = JsonConvert.SerializeObject(
                documents,
                Formatting.Indented
            );

            // 파일 저장
            File.WriteAllText(RecentFilePath, json);
        }

        // 최근 문서 목록에서 제거
        public static void RemoveRecentDocument(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return;
            }

            // 전체 경로로 변환
            string fullPath = Path.GetFullPath(filePath);

            // 기존 목록 불러오기
            List<string> documents = GetRecentDocuments();

            // 해당 경로 제거
            documents.RemoveAll(path =>
                string.Equals(
                    path,
                    fullPath,
                    StringComparison.OrdinalIgnoreCase
                )
            );

            // 변경된 목록 저장
            SaveRecentDocuments(documents);
        }
    }

}
