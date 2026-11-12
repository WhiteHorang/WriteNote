using System.Collections.Generic;
using System.Windows.Forms;

namespace WriteNote
{
    // 단축키(Keys) 값을 화면에 보여 줄 문자열로 바꾸고, 사용 가능한 조합인지 검사한다.
    public static class ShortcutKeyHelper
    {
        // 기호 키는 Keys 이름(OemPeriod 등) 대신 실제 기호로 표시한다.
        private static readonly Dictionary<Keys, string> KeyNames = new Dictionary<Keys, string>
        {
            { Keys.OemPeriod, "." },
            { Keys.Oemcomma, "," },
            { Keys.OemQuotes, "'" },
            { Keys.OemSemicolon, ";" },
            { Keys.OemOpenBrackets, "[" },
            { Keys.OemCloseBrackets, "]" },
            { Keys.OemQuestion, "/" },
            { Keys.OemPipe, "\\" },
            { Keys.OemMinus, "-" },
            { Keys.Oemplus, "=" },
            { Keys.Oemtilde, "`" },
            { Keys.Return, "Enter" },
            { Keys.Space, "Space" },
            { Keys.Back, "Backspace" },
            { Keys.Escape, "Esc" },
            { Keys.Prior, "PageUp" },
            { Keys.Next, "PageDown" },
            { Keys.Insert, "Insert" },
            { Keys.Delete, "Delete" },
            { Keys.Home, "Home" },
            { Keys.End, "End" },
            { Keys.Up, "↑" },
            { Keys.Down, "↓" },
            { Keys.Left, "←" },
            { Keys.Right, "→" },
        };

        // 예: Keys.Control | Keys.Shift | Keys.OemPeriod → "Ctrl + Shift + ."
        public static string ToDisplayString(Keys keyData)
        {
            if (keyData == Keys.None)
                return "";

            List<string> parts = new List<string>();

            if ((keyData & Keys.Control) == Keys.Control) parts.Add("Ctrl");
            if ((keyData & Keys.Alt) == Keys.Alt) parts.Add("Alt");
            if ((keyData & Keys.Shift) == Keys.Shift) parts.Add("Shift");

            Keys key = keyData & Keys.KeyCode;

            if (!IsModifierKey(key) && key != Keys.None)
                parts.Add(GetKeyName(key));

            return string.Join(" + ", parts);
        }

        public static string GetKeyName(Keys key)
        {
            string name;
            if (KeyNames.TryGetValue(key, out name))
                return name;

            // D0~D9 → 0~9
            if (key >= Keys.D0 && key <= Keys.D9)
                return ((char)('0' + (key - Keys.D0))).ToString();

            // NumPad0~NumPad9 → Num 0~Num 9
            if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
                return "Num " + (key - Keys.NumPad0);

            return key.ToString();
        }

        // Ctrl, Shift, Alt 자체(조합 대상이 아닌 수식 키)인지 확인
        public static bool IsModifierKey(Keys key)
        {
            return key == Keys.ControlKey || key == Keys.LControlKey || key == Keys.RControlKey
                || key == Keys.ShiftKey || key == Keys.LShiftKey || key == Keys.RShiftKey
                || key == Keys.Menu || key == Keys.LMenu || key == Keys.RMenu
                || key == Keys.LWin || key == Keys.RWin;
        }

        public static bool IsFunctionKey(Keys key)
        {
            return key >= Keys.F1 && key <= Keys.F24;
        }

        // 단축키로 쓸 수 있는 조합인지 검사한다.
        // - Ctrl 또는 Alt가 포함되어야 한다. (Shift + 글자는 일반 입력과 겹침)
        // - F1~F12 같은 기능 키는 단독으로도 허용한다.
        public static bool IsValidShortcut(Keys keyData)
        {
            Keys key = keyData & Keys.KeyCode;

            if (key == Keys.None || IsModifierKey(key) || key == Keys.ProcessKey)
                return false;

            if (IsFunctionKey(key))
                return true;

            bool hasCtrl = (keyData & Keys.Control) == Keys.Control;
            bool hasAlt = (keyData & Keys.Alt) == Keys.Alt;

            return hasCtrl || hasAlt;
        }
    }
}
