using System;
using System.IO;

namespace WriteNote
{
    // 파일·폴더 경로 비교와 바꾸기 (여러 화면에서 함께 쓴다)
    // Windows 경로는 대소문자를 구분하지 않으므로 모두 OrdinalIgnoreCase로 비교한다.
    public static class PathHelper
    {
        // 끝의 \ 또는 / 를 뗀 전체 경로. 경로로 해석할 수 없으면 받은 그대로
        private static string Normalize(string path)
        {
            try
            {
                return Path.GetFullPath(path).TrimEnd('\\', '/');
            }
            catch (Exception)
            {
                return (path ?? "").TrimEnd('\\', '/');
            }
        }

        // 두 폴더가 같은 위치인지 (대소문자, 끝의 \ 무시)
        public static bool IsSameFolder(string a, string b)
        {
            if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b))
                return false;

            return string.Equals(Normalize(a), Normalize(b), StringComparison.OrdinalIgnoreCase);
        }

        // path가 folder 안(하위 폴더 포함)에 있는지
        public static bool IsInFolder(string path, string folder)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(folder))
                return false;

            return path.StartsWith(Normalize(folder) + "\\", StringComparison.OrdinalIgnoreCase);
        }

        // path가 oldFolder 안에 있으면 newFolder 안의 같은 위치로 바꾼다. 아니면 그대로
        // 예: (C:\문서\소설\1장.rtf, C:\문서\소설, C:\문서\장편) → C:\문서\장편\1장.rtf
        public static string ReplaceFolderInPath(string path, string oldFolder, string newFolder)
        {
            if (!IsInFolder(path, oldFolder))
                return path;

            string relative = path.Substring(Normalize(oldFolder).Length + 1);
            return Path.Combine(Normalize(newFolder), relative);
        }
    }
}
