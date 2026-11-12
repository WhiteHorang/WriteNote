using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace WriteNote
{
    // 글쓰기 단축키 목록을 JSON 파일로 저장하고 불러온다.
    // 저장 위치: %AppData%\WriteNote\shortcuts.json
    public static class ShortcutManager
    {
        // WriteNote 데이터 폴더 경로 (%AppData%\WriteNote, 설정 파일과 같은 곳)
        private static string DataFolderPath
        {
            get { return SettingsManager.DataFolderPath; }
        }

        public static string ShortcutFilePath
        {
            get { return Path.Combine(DataFolderPath, "shortcuts.json"); }
        }

        // 변경할 수 없는 기본 단축키 (설정 목록에 표시하지 않음)
        // 사용자 단축키가 이 조합과 겹치면 저장할 수 없다.
        private static readonly Dictionary<Keys, string> ReservedShortcuts =
            new Dictionary<Keys, string>
            {
                { Keys.Control | Keys.S, "저장" },
                { Keys.Control | Keys.Shift | Keys.S, "다른 이름으로 저장" },
                { Keys.Control | Keys.O, "열기" },
                { Keys.Control | Keys.N, "새 문서" },
                { Keys.Control | Keys.B, "사이드 패널 열기/닫기" },
                { Keys.Control | Keys.F, "문서 내 검색" },
                { Keys.F1, "설명서" },
                { Keys.Control | Keys.C, "복사" },
                { Keys.Control | Keys.X, "잘라내기" },
                { Keys.Control | Keys.V, "붙여넣기" },
                { Keys.Control | Keys.Z, "실행 취소" },
                { Keys.Control | Keys.Y, "다시 실행" },
                { Keys.Control | Keys.A, "모두 선택" },
            };

        // 처음 실행했을 때 사용할 글쓰기 기본 단축키
        public static List<ShortcutItem> GetDefaultShortcuts()
        {
            return new List<ShortcutItem>
            {
                new ShortcutItem
                {
                    Id = "default-three-lines",
                    Name = "세 줄 띄우기",
                    Keys = Keys.Control | Keys.Enter,
                    Content = Environment.NewLine + Environment.NewLine + Environment.NewLine,
                    IsDefault = true
                },
                new ShortcutItem
                {
                    Id = "default-divider",
                    Name = "구분선 입력",
                    Keys = Keys.Control | Keys.Shift | Keys.OemPeriod,
                    Content = "*   *   *",
                    IsDefault = true
                },
                new ShortcutItem
                {
                    Id = "default-ellipsis",
                    Name = "말줄임표 입력",
                    Keys = Keys.Control | Keys.OemPeriod,
                    Content = "⋯",
                    IsDefault = true
                },
                new ShortcutItem
                {
                    Id = "default-corner-brackets",
                    Name = "겹낫표 입력",
                    Keys = Keys.Control | Keys.OemOpenBrackets,
                    Content = "「」",
                    CursorOffset = 1,
                    IsDefault = true
                },
                new ShortcutItem
                {
                    Id = "default-double-quotes",
                    Name = "큰따옴표 입력",
                    Keys = Keys.Control | Keys.OemQuotes,
                    Content = "\"\"",
                    CursorOffset = 1,
                    IsDefault = true
                },
                new ShortcutItem
                {
                    Id = "default-single-quotes",
                    Name = "작은따옴표 입력",
                    Keys = Keys.Control | Keys.Shift | Keys.OemQuotes,
                    Content = "''",
                    CursorOffset = 1,
                    IsDefault = true
                },
            };
        }

        // 단축키 목록 불러오기
        // - 파일이 없으면 기본 단축키를 돌려준다.
        // - 파일이 손상되었으면 shortcuts.broken.json으로 옮겨 두고 기본 단축키를 돌려준다.
        public static List<ShortcutItem> Load()
        {
            if (!File.Exists(ShortcutFilePath))
                return GetDefaultShortcuts();

            try
            {
                string json = File.ReadAllText(ShortcutFilePath);

                List<ShortcutItem> items =
                    JsonConvert.DeserializeObject<List<ShortcutItem>>(json);

                if (items == null)
                    return GetDefaultShortcuts();

                // 잘못된 항목은 제외하고, 같은 키가 여러 번 있으면 앞의 것만 사용
                return items
                    .Where(item => item != null &&
                                   ShortcutKeyHelper.IsValidShortcut(item.Keys) &&
                                   GetReservedName(item.Keys) == null &&
                                   !string.IsNullOrEmpty(item.Content))
                    .GroupBy(item => item.Keys)
                    .Select(group => group.First())
                    .ToList();
            }
            catch (Exception)
            {
                BackUpBrokenFile();
                return GetDefaultShortcuts();
            }
        }

        // 단축키 목록 저장
        public static void Save(List<ShortcutItem> items)
        {
            Directory.CreateDirectory(DataFolderPath);

            string json = JsonConvert.SerializeObject(items, Formatting.Indented);

            // 저장 도중 문제가 생겨도 기존 파일이 깨지지 않도록 임시 파일에 먼저 쓴다.
            string tempPath = ShortcutFilePath + ".tmp";
            File.WriteAllText(tempPath, json);

            if (File.Exists(ShortcutFilePath))
                File.Delete(ShortcutFilePath);

            File.Move(tempPath, ShortcutFilePath);
        }

        // 변경할 수 없는 기본 단축키인지 확인. 맞으면 기능 이름을 돌려준다.
        public static string GetReservedName(Keys keys)
        {
            string name;
            return ReservedShortcuts.TryGetValue(keys, out name) ? name : null;
        }

        // 같은 키 조합을 쓰는 항목이 있는지 검사한다.
        // 겹치는 것이 있으면 안내 문구를, 없으면 null을 돌려준다.
        // excludeId: 수정 중인 항목 자신은 검사에서 제외
        public static string FindConflict(
            IEnumerable<ShortcutItem> items, Keys keys, string excludeId)
        {
            string reservedName = GetReservedName(keys);
            if (reservedName != null)
            {
                return "'" + ShortcutKeyHelper.ToDisplayString(keys) + "'은(는) " +
                       "'" + reservedName + "' 기능에 사용되는 기본 단축키라 사용할 수 없습니다.";
            }

            if (items == null)
                return null;

            ShortcutItem duplicate = items.FirstOrDefault(item =>
                item.Keys == keys && item.Id != excludeId);

            if (duplicate != null)
            {
                return "'" + ShortcutKeyHelper.ToDisplayString(keys) + "'은(는) " +
                       "이미 '" + duplicate.Name + "' 기능에 사용 중입니다.";
            }

            return null;
        }

        private static void BackUpBrokenFile()
        {
            try
            {
                string brokenPath = Path.Combine(DataFolderPath, "shortcuts.broken.json");

                if (File.Exists(brokenPath))
                    File.Delete(brokenPath);

                File.Move(ShortcutFilePath, brokenPath);
            }
            catch (Exception)
            {
                // 백업에 실패해도 프로그램은 기본 단축키로 계속 동작한다.
            }
        }
    }
}
