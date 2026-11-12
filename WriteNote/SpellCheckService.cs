using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCantSpell.Hunspell;

namespace WriteNote
{
    // 본문에서 찾은 맞춤법 의심 단어
    public class SpellingError
    {
        public int Start;
        public int Length;
        public string Word;

        public int End
        {
            get { return Start + Length; }
        }
    }

    // 한국어 맞춤법 검사 (hunspell 한국어 사전 사용)
    // - 사전: exe 안에 넣어 둔 ko.aff, ko.dic (GPL-3.0, hunspell-dict-ko)
    //         없으면 exe 옆 Dictionaries 폴더에서 찾는다.
    // - 사용자 사전: %AppData%\WriteNote\user_dictionary.txt (한 줄에 한 단어)
    // - 단어 단위 철자 검사만 한다. 문맥이 필요한 오류(예: 되요/돼요)는 잡지 못한다.
    public static class SpellCheckService
    {
        // 뒤에서(다른 스레드) 불러온 뒤 화면 스레드에서 읽으므로 volatile
        private static volatile WordList wordList;
        private static Task loadingTask;

        // 검사 결과 기억 (같은 단어를 다시 검사하지 않도록)
        private static readonly Dictionary<string, bool> cache = new Dictionary<string, bool>();

        // 사용자 사전과 '무시'한 단어
        private static readonly HashSet<string> userWords = new HashSet<string>();
        private static readonly HashSet<string> ignoredWords = new HashSet<string>();
        private static bool userDictionaryLoaded = false;

        // 사용자 사전 단어 뒤에 붙어도 맞는 것으로 볼 조사·어미
        // 예: 사전에 '민수'를 추가하면 '민수가', '민수에게'도 맞는 것으로 본다.
        private static readonly string[] Particles =
        {
            "이", "가", "은", "는", "을", "를", "의", "에", "에게", "에서", "에게서", "한테", "께",
            "께서", "와", "과", "도", "만", "로", "으로", "이나", "나", "이랑", "랑", "하고",
            "까지", "부터", "보다", "처럼", "같이", "야", "아", "여", "이여", "이다", "다",
            "였다", "이었다", "이야", "이에요", "예요", "입니다", "이지만", "지만", "들",
            "들이", "들은", "들을", "들의", "들에게", "씨", "님"
        };

        public static bool IsReady
        {
            get { return wordList != null; }
        }

        // 사전을 불러오지 못했을 때의 이유 (없으면 null)
        public static string LoadError { get; private set; }

        public static string DictionaryFolder
        {
            // WriteNote.exe가 있는 폴더 기준
            get
            {
                return Path.Combine(
                    Path.GetDirectoryName(typeof(SpellCheckService).Assembly.Location),
                    "Dictionaries");
            }
        }

        private static string UserDictionaryPath
        {
            get { return Path.Combine(SettingsManager.DataFolderPath, "user_dictionary.txt"); }
        }

        // 사전을 뒤에서 불러온다. (약 1초, 메모리 약 50MB) 여러 번 불러도 한 번만 읽는다.
        public static Task LoadAsync()
        {
            if (loadingTask == null)
            {
                EnsureUserDictionaryLoaded();

                loadingTask = Task.Run(() =>
                {
                    try
                    {
                        // 1) exe 안에 넣어 둔 사전 (exe 하나로 배포할 때)
                        System.Reflection.Assembly self = typeof(SpellCheckService).Assembly;

                        using (Stream dicStream = self.GetManifestResourceStream("WriteNote.Dictionaries.ko.dic"))
                        using (Stream affStream = self.GetManifestResourceStream("WriteNote.Dictionaries.ko.aff"))
                        {
                            if (dicStream != null && affStream != null)
                            {
                                wordList = WordList.CreateFromStreams(dicStream, affStream);
                                return;
                            }
                        }

                        // 2) exe 옆 Dictionaries 폴더 (예전 배포 방식)
                        string dic = Path.Combine(DictionaryFolder, "ko.dic");
                        string aff = Path.Combine(DictionaryFolder, "ko.aff");

                        if (!File.Exists(dic) || !File.Exists(aff))
                        {
                            LoadError = "맞춤법 사전을 찾을 수 없습니다.";
                            return;
                        }

                        wordList = WordList.CreateFromFiles(dic, aff);
                    }
                    catch (Exception ex)
                    {
                        LoadError = "맞춤법 사전을 불러오지 못했습니다.\n" + ex.Message;
                    }
                });
            }

            return loadingTask;
        }

        // ───────────── 단어 검사 ─────────────

        // 한글 음절(가~힣)이 들어 있는 단어만 검사한다. (영어·숫자만 있는 단어는 건너뜀)
        private static bool ContainsHangul(string word)
        {
            foreach (char c in word)
            {
                if (c >= '가' && c <= '힣')
                    return true;
            }

            return false;
        }

        private static bool IsWordChar(char c)
        {
            return char.IsLetterOrDigit(c);
        }

        public static bool IsCorrect(string word)
        {
            if (!IsReady || string.IsNullOrEmpty(word))
                return true;

            if (!ContainsHangul(word))
                return true;

            // 숫자가 섞인 단어(예: 3월, 2층)는 건너뜀
            if (word.Any(char.IsDigit))
                return true;

            if (ignoredWords.Contains(word) || IsUserWord(word))
                return true;

            bool result;

            if (!cache.TryGetValue(word, out result))
            {
                result = wordList.Check(word);
                cache[word] = result;
            }

            return result;
        }

        // 사용자 사전 단어 그 자체이거나, 그 단어 + 조사·어미인지
        private static bool IsUserWord(string word)
        {
            if (userWords.Contains(word))
                return true;

            foreach (string particle in Particles)
            {
                if (word.Length > particle.Length &&
                    word.EndsWith(particle, StringComparison.Ordinal) &&
                    userWords.Contains(word.Substring(0, word.Length - particle.Length)))
                {
                    return true;
                }
            }

            return false;
        }

        // 추천 단어 (최대 count개)
        public static List<string> Suggest(string word, int count)
        {
            if (!IsReady)
                return new List<string>();

            try
            {
                return wordList.Suggest(word).Take(count).ToList();
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }

        // text의 [rangeStart, rangeStart + rangeLength) 구간에서 의심 단어를 찾는다.
        // skipPosition: 이 위치에 걸친 단어는 건너뛴다. (지금 입력 중인 단어, 없으면 -1)
        public static List<SpellingError> FindErrors(string text, int rangeStart, int rangeLength, int skipPosition)
        {
            List<SpellingError> errors = new List<SpellingError>();

            if (!IsReady || string.IsNullOrEmpty(text))
                return errors;

            int start = Math.Max(0, rangeStart);
            int end = Math.Min(text.Length, rangeStart + rangeLength);

            // 구간 시작이 단어 중간이면 단어 처음으로 당긴다.
            while (start > 0 && IsWordChar(text[start - 1]))
                start--;

            int i = start;

            while (i < end)
            {
                if (!IsWordChar(text[i]))
                {
                    i++;
                    continue;
                }

                int wordStart = i;

                while (i < text.Length && IsWordChar(text[i]))
                    i++;

                int wordLength = i - wordStart;

                if (skipPosition >= wordStart && skipPosition <= wordStart + wordLength)
                    continue;

                string word = text.Substring(wordStart, wordLength);

                if (!IsCorrect(word))
                {
                    errors.Add(new SpellingError { Start = wordStart, Length = wordLength, Word = word });
                }
            }

            return errors;
        }

        // ───────────── 사용자 사전 ─────────────

        private static void LoadUserDictionary()
        {
            userWords.Clear();
            userDictionaryLoaded = true;

            try
            {
                if (!File.Exists(UserDictionaryPath))
                    return;

                foreach (string line in File.ReadAllLines(UserDictionaryPath, Encoding.UTF8))
                {
                    string word = line.Trim();

                    if (word.Length > 0)
                        userWords.Add(word);
                }
            }
            catch (Exception)
            {
                // 사용자 사전을 읽지 못하면 기본 사전만 사용
            }
        }

        // 사용자 사전을 아직 읽지 않았으면 읽는다.
        // (맞춤법 사전을 불러오기 전에 설정 창에서 사용자 사전을 열 수도 있으므로)
        private static void EnsureUserDictionaryLoaded()
        {
            if (!userDictionaryLoaded)
                LoadUserDictionary();
        }

        private static void SaveUserDictionary()
        {
            Directory.CreateDirectory(SettingsManager.DataFolderPath);
            File.WriteAllLines(UserDictionaryPath,
                userWords.OrderBy(w => w, StringComparer.Ordinal), new UTF8Encoding(false));
        }

        // 사용자 사전이 바뀌면 알린다. (본문 물결선을 바로 다시 그리기 위해)
        public static event EventHandler UserDictionaryChanged;

        private static void RaiseUserDictionaryChanged()
        {
            EventHandler handler = UserDictionaryChanged;

            if (handler != null)
                handler(null, EventArgs.Empty);
        }

        // 사용자 사전에 넣을 수 있는 단어인지 검사. 문제가 없으면 null, 있으면 안내 문구
        public static string ValidateUserWord(string word)
        {
            word = (word ?? "").Trim();

            if (word.Length == 0)
                return "단어를 입력하세요.";

            if (word.Any(char.IsWhiteSpace))
                return "띄어쓰기 없이 한 단어만 입력하세요.";

            if (word.Length > 50)
                return "단어가 너무 깁니다. (50자까지)";

            return null;
        }

        // 사용자 사전 단어 목록 (가나다순)
        public static List<string> GetUserWords()
        {
            EnsureUserDictionaryLoaded();

            return userWords.OrderBy(w => w, StringComparer.CurrentCulture).ToList();
        }

        // 사용자 사전을 통째로 바꾼다. (사용자 사전 관리 창에서 '확인'을 누를 때)
        public static void SetUserWords(IEnumerable<string> words)
        {
            EnsureUserDictionaryLoaded();

            HashSet<string> cleaned = new HashSet<string>(
                (words ?? Enumerable.Empty<string>())
                    .Select(w => (w ?? "").Trim())
                    .Where(w => ValidateUserWord(w) == null));

            userWords.Clear();
            userWords.UnionWith(cleaned);

            SaveUserDictionary();
            RaiseUserDictionaryChanged();
        }

        // 사전에 추가 (다음 실행에도 유지)
        public static void AddToUserDictionary(string word)
        {
            EnsureUserDictionaryLoaded();

            word = (word ?? "").Trim();

            if (ValidateUserWord(word) != null || !userWords.Add(word))
                return;

            SaveUserDictionary();
            RaiseUserDictionaryChanged();
        }

        // 무시 (프로그램을 끌 때까지만)
        public static void Ignore(string word)
        {
            if (!string.IsNullOrEmpty(word))
                ignoredWords.Add(word);
        }
    }
}
