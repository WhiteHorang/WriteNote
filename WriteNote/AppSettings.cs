using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WriteNote
{
    // WriteNote 전체 설정 (settings.json)
    // - 설정 창의 페이지별로 묶어서 관리한다.
    // - 글쓰기 단축키는 양이 많고 형식이 달라 shortcuts.json에 따로 저장한다.
    // - 파일에 없는 항목은 아래에 적힌 기본값이 사용되므로,
    //   나중에 항목을 추가해도 예전 설정 파일을 그대로 읽을 수 있다.
    public class AppSettings
    {
        // 설정 파일 형식 버전 (구조가 크게 바뀌면 올린다)
        public int Version { get; set; } = 1;

        public GeneralSettings General { get; set; } = new GeneralSettings();
        public WritingSettings Writing { get; set; } = new WritingSettings();
        public StorageSettings Storage { get; set; } = new StorageSettings();
        public AppearanceSettings Appearance { get; set; } = new AppearanceSettings();

        // 잘못된 값이나 빠진 항목을 기본값으로 바로잡는다.
        public void Normalize()
        {
            if (General == null) General = new GeneralSettings();
            if (Writing == null) Writing = new WritingSettings();
            if (Storage == null) Storage = new StorageSettings();
            if (Appearance == null) Appearance = new AppearanceSettings();

            Writing.Normalize();
            Storage.Normalize();
            Appearance.Normalize();
        }
    }

    // 프로그램을 시작할 때 할 일
    public enum StartupAction
    {
        NewDocument,    // 새 문서로 시작
        LastDocument    // 마지막으로 작업한 문서 열기
    }

    // 일반: 프로그램 기본 동작
    public class GeneralSettings
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public StartupAction StartupAction { get; set; } = StartupAction.NewDocument;

        // 시작할 때 사이드 패널 열기
        public bool OpenSidebarOnStartup { get; set; } = false;
    }

    // 글자 수를 세는 기준
    public enum CharCountMode
    {
        IncludeSpaces,  // 공백 포함 (줄바꿈은 세지 않음)
        ExcludeSpaces   // 공백 제외 (띄어쓰기, 탭, 줄바꿈 모두 세지 않음)
    }

    // 글쓰기: 글쓰기 환경 및 편의 기능
    public class WritingSettings
    {
        // 선택할 수 있는 자동 저장 간격 (분)
        public static readonly int[] AutoSaveIntervals = { 1, 3, 5, 10, 30 };

        // 자동 저장 사용 (한 번 이상 저장한 문서만 대상)
        public bool AutoSaveEnabled { get; set; } = false;

        // 자동 저장 간격 (분)
        public int AutoSaveIntervalMinutes { get; set; } = 5;

        [JsonConverter(typeof(StringEnumConverter))]
        public CharCountMode CharCountMode { get; set; } = CharCountMode.IncludeSpaces;

        // 붙여넣을 때 서식(글꼴, 색 등)을 빼고 글자만 넣기
        public bool PastePlainText { get; set; } = false;

        // 글쓰기 단축키(말줄임표 입력 등) 사용
        public bool WritingShortcutsEnabled { get; set; } = true;

        // 맞춤법 검사 (틀린 것으로 의심되는 단어에 빨간 물결선)
        public bool SpellCheckEnabled { get; set; } = true;

        public void Normalize()
        {
            // 목록에 없는 간격이면 기본값(5분)으로
            if (System.Array.IndexOf(AutoSaveIntervals, AutoSaveIntervalMinutes) < 0)
                AutoSaveIntervalMinutes = 5;
        }
    }

    // 저장 및 문서
    public class StorageSettings
    {
        // 기본 문서 폴더: 내 문서\WriteNote
        public static string DefaultDocumentFolder
        {
            get
            {
                return System.IO.Path.Combine(
                    System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments),
                    "WriteNote");
            }
        }

        // 문서 저장 폴더. 비어 있으면 기본 위치(내 문서\WriteNote)를 사용한다.
        public string DocumentFolder { get; set; } = "";

        // 실제로 사용할 문서 폴더 (비어 있으면 기본 위치)
        [JsonIgnore]
        public string EffectiveDocumentFolder
        {
            get
            {
                return string.IsNullOrEmpty(DocumentFolder)
                    ? DefaultDocumentFolder
                    : DocumentFolder;
            }
        }

        public void Normalize()
        {
            if (DocumentFolder == null)
                DocumentFolder = "";

            DocumentFolder = DocumentFolder.Trim();
        }
    }

    // 화면 및 테마
    public class AppearanceSettings
    {
        public const float MinFontSize = 8f;
        public const float MaxFontSize = 40f;

        // 프로그램 전체 테마
        [JsonConverter(typeof(StringEnumConverter))]
        public AppTheme Theme { get; set; } = AppTheme.Dark;

        // 본문 글꼴
        public string ContentFontName { get; set; } = "맑은 고딕";
        public float ContentFontSize { get; set; } = 12f;

        // 제목 글꼴 크기
        public float TitleFontSize { get; set; } = 16f;

        public void Normalize()
        {
            if (string.IsNullOrWhiteSpace(ContentFontName))
                ContentFontName = "맑은 고딕";

            ContentFontSize = Clamp(ContentFontSize, 12f);
            TitleFontSize = Clamp(TitleFontSize, 16f);
        }

        private static float Clamp(float value, float defaultValue)
        {
            if (float.IsNaN(value) || value <= 0)
                return defaultValue;

            if (value < MinFontSize) return MinFontSize;
            if (value > MaxFontSize) return MaxFontSize;
            return value;
        }
    }
}
