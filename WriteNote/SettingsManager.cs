using System;
using System.IO;
using Newtonsoft.Json;

namespace WriteNote
{
    // WriteNote 설정을 JSON 파일로 저장하고 불러온다.
    // 저장 위치: %AppData%\WriteNote\settings.json
    public static class SettingsManager
    {
        // WriteNote 데이터 폴더 (설정, 단축키, 최근 문서, 휴지통, 사용자 사전)
        // 환경 변수 WRITENOTE_DATA_DIR가 있으면 그 폴더를 쓴다.
        // (자동 테스트가 실제 사용자 데이터를 건드리지 않도록 하기 위한 것)
        public static string DataFolderPath
        {
            get
            {
                string overridePath = Environment.GetEnvironmentVariable("WRITENOTE_DATA_DIR");

                if (!string.IsNullOrWhiteSpace(overridePath))
                    return overridePath;

                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "WriteNote"
                );
            }
        }

        public static string SettingsFilePath
        {
            get { return Path.Combine(DataFolderPath, "settings.json"); }
        }

        // 설정 불러오기
        // - 파일이 없으면 기본 설정을 돌려준다.
        // - 일부 항목만 잘못되었으면 그 항목만 기본값으로 두고 나머지는 그대로 읽는다.
        // - 파일 전체가 손상되었으면 settings.broken.json으로 옮겨 두고 기본 설정을 돌려준다.
        public static AppSettings Load()
        {
            if (!File.Exists(SettingsFilePath))
                return new AppSettings();

            try
            {
                string json = File.ReadAllText(SettingsFilePath);

                // JSON 문법 자체가 깨졌는지 먼저 확인한다. (깨졌으면 예외 → 백업 후 기본값)
                Newtonsoft.Json.Linq.JObject.Parse(json);

                JsonSerializerSettings options = new JsonSerializerSettings
                {
                    // 하위 항목 객체를 새로 만들어 기본값 위에 덮어쓴다.
                    ObjectCreationHandling = ObjectCreationHandling.Replace,

                    // 알 수 없는 값(예: 잘못된 선택지 이름)이 있어도 그 항목만 건너뛴다.
                    Error = (sender, args) => args.ErrorContext.Handled = true
                };

                AppSettings settings =
                    JsonConvert.DeserializeObject<AppSettings>(json, options);

                if (settings == null)
                    return new AppSettings();

                settings.Normalize();
                return settings;
            }
            catch (Exception)
            {
                BackUpBrokenFile();
                return new AppSettings();
            }
        }

        // 설정 저장
        public static void Save(AppSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            settings.Normalize();

            Directory.CreateDirectory(DataFolderPath);

            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);

            // 저장 도중 문제가 생겨도 기존 파일이 깨지지 않도록 임시 파일에 먼저 쓴다.
            string tempPath = SettingsFilePath + ".tmp";
            File.WriteAllText(tempPath, json);

            if (File.Exists(SettingsFilePath))
                File.Delete(SettingsFilePath);

            File.Move(tempPath, SettingsFilePath);
        }

        private static void BackUpBrokenFile()
        {
            try
            {
                string brokenPath = Path.Combine(DataFolderPath, "settings.broken.json");

                if (File.Exists(brokenPath))
                    File.Delete(brokenPath);

                File.Move(SettingsFilePath, brokenPath);
            }
            catch (Exception)
            {
                // 백업에 실패해도 프로그램은 기본 설정으로 계속 동작한다.
            }
        }
    }
}
