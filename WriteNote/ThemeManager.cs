using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WriteNote
{
    public enum AppTheme
    {
        Dark,   // 어두운 테마 (디자이너에 적힌 원래 색)
        Light   // 밝은 테마
    }

    // 프로그램 전체 테마를 관리한다.
    //
    // 화면 색은 디자이너에 '어두운 테마' 기준으로 적혀 있다.
    // 밝은 테마에서는 아래 표를 보고 어두운 색을 대응하는 밝은 색으로 바꿔 칠한다.
    // 새 색을 디자이너에 쓰면 이 표에도 밝은 테마 색을 추가해야 한다.
    public static class ThemeManager
    {
        public static AppTheme Current { get; private set; } = AppTheme.Dark;

        // 강조색 (두 테마 공통)
        public static readonly Color Accent = Color.FromArgb(102, 85, 165);
        public static readonly Color Danger = Color.FromArgb(120, 45, 45);

        // 어두운 색 → 밝은 색 대응표
        private static readonly Dictionary<int, Color> DarkToLight = new Dictionary<int, Color>();
        private static readonly Dictionary<int, Color> LightToDark = new Dictionary<int, Color>();

        static ThemeManager()
        {
            // ── 배경 ──
            Add(Color.FromArgb(32, 33, 36), Color.FromArgb(243, 243, 246));   // 창 배경
            Add(Color.FromArgb(35, 35, 38), Color.FromArgb(236, 236, 240));   // 메뉴·버튼 기본
            Add(Color.FromArgb(35, 35, 40), Color.FromArgb(234, 234, 239));
            Add(Color.FromArgb(40, 40, 40), Color.FromArgb(229, 229, 229));
            Add(Color.FromArgb(41, 43, 48), Color.FromArgb(252, 252, 253));   // 패널·본문
            Add(Color.FromArgb(45, 45, 50), Color.FromArgb(226, 226, 231));
            Add(Color.FromArgb(45, 45, 52), Color.FromArgb(230, 230, 236));   // 문서 카드 마우스 올림
            Add(Color.FromArgb(48, 50, 58), Color.FromArgb(224, 225, 231));   // 보조 버튼
            Add(Color.FromArgb(48, 51, 59), Color.FromArgb(240, 240, 244));   // 콤보박스·메뉴
            Add(Color.FromArgb(55, 55, 65), Color.FromArgb(218, 216, 232));   // 선택된 항목
            Add(Color.FromArgb(57, 59, 66), Color.FromArgb(214, 216, 222));   // 둥근 버튼
            Add(Color.FromArgb(65, 68, 78), Color.FromArgb(200, 202, 210));   // 메뉴 테두리
            Add(Color.FromArgb(80, 82, 90), Color.FromArgb(188, 190, 198));   // 버튼 테두리
            Add(Color.FromArgb(80, 67, 135), Color.FromArgb(222, 215, 245));  // 메뉴 눌림

            // ── 글자 ──
            Add(Color.FromArgb(242, 242, 242), Color.FromArgb(32, 32, 36));
            Add(Color.FromArgb(230, 230, 234), Color.FromArgb(40, 40, 46));
            Add(Color.FromArgb(224, 224, 224), Color.FromArgb(28, 28, 30));   // 본문 글자
            Add(Color.Gainsboro, Color.FromArgb(95, 95, 105));                // 설명 글자
            Add(Color.WhiteSmoke, Color.FromArgb(50, 50, 56));
            Add(Color.White, Color.FromArgb(22, 22, 26));
            Add(Color.FromArgb(150, 150, 150), Color.FromArgb(115, 115, 120)); // 흐린 글자
            Add(Color.FromArgb(130, 130, 145), Color.FromArgb(140, 140, 155)); // 선택 카드 테두리
        }

        private static void Add(Color dark, Color light)
        {
            DarkToLight[dark.ToArgb()] = light;
            LightToDark[light.ToArgb()] = dark;
        }

        public static void SetTheme(AppTheme theme)
        {
            Current = theme;
        }

        // 어두운 테마 기준 색을 현재 테마 색으로 바꾼다.
        // 코드에서 색을 직접 지정할 때 사용: ThemeManager.C(Color.FromArgb(55, 55, 65))
        public static Color C(Color darkColor)
        {
            return Convert(darkColor, AppTheme.Dark, Current);
        }

        // from 테마의 색을 to 테마의 색으로 바꾼다. 표에 없는 색은 그대로 둔다.
        public static Color Convert(Color color, AppTheme from, AppTheme to)
        {
            if (from == to || color.IsEmpty || color.A != 255)
                return color;

            Color result;
            int argb = color.ToArgb();

            if (from == AppTheme.Light && to == AppTheme.Dark)
                return LightToDark.TryGetValue(argb, out result) ? result : color;

            return DarkToLight.TryGetValue(argb, out result) ? result : color;
        }

        // 강조색 버튼 위의 흰 글자처럼 배경이 강조색이면 글자색은 바꾸지 않는다.
        private static bool KeepForeColor(Color backColor)
        {
            int argb = backColor.ToArgb();
            return argb == Accent.ToArgb() || argb == Danger.ToArgb();
        }

        // 폼(또는 컨트롤)과 그 안의 모든 컨트롤 색을 from 테마에서 현재 테마로 바꾼다.
        // - 새로 연 폼: from = Dark (디자이너 색이 어두운 테마 기준)
        // - 테마를 바꾼 경우: from = 이전 테마
        public static void Apply(Control root, AppTheme from)
        {
            if (root == null || from == Current)
                return;

            ApplyControl(root, from, Current);
        }

        // 폼의 Controls에 들어 있지 않은 우클릭 메뉴 등에 사용
        public static void Apply(ToolStrip strip, AppTheme from)
        {
            if (strip == null || from == Current)
                return;

            ApplyToolStrip(strip, from, Current);
        }

        // Control의 내부 메서드: 색을 직접 지정했는지(부모 색을 물려받는 중이 아닌지) 확인
        private static readonly System.Reflection.MethodInfo ShouldSerializeBackColorMethod =
            typeof(Control).GetMethod("ShouldSerializeBackColor",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        private static readonly System.Reflection.MethodInfo ShouldSerializeForeColorMethod =
            typeof(Control).GetMethod("ShouldSerializeForeColor",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // 이 컨트롤에 직접 지정한 색인지 확인한다.
        // 부모 색을 물려받는 컨트롤까지 칠하면 그 뒤로는 부모 색이 바뀌어도 따라가지 않는다.
        // (예: 문서 카드에 마우스를 올리면 카드 안의 글자 배경만 남는 문제)
        private static bool IsColorSet(Control control, System.Reflection.MethodInfo method)
        {
            if (method == null)
                return true;

            try
            {
                return (bool)method.Invoke(control, null);
            }
            catch (Exception)
            {
                return true;
            }
        }

        // 테마로 바꿀 색인지 확인 (시스템 색은 Windows가 정하므로 그대로 둔다)
        // 예: 입력칸 기본 배경인 SystemColors.Window는 흰색과 값이 같지만 흰 글자색과는 다르다.
        private static Color ConvertControlColor(Color color, AppTheme from, AppTheme to)
        {
            if (color.IsSystemColor)
                return color;

            return Convert(color, from, to);
        }

        private static void ApplyControl(Control control, AppTheme from, AppTheme to)
        {
            Color oldBack = control.BackColor;

            // Transparent 등 반투명 색은 Convert에서 그대로 유지된다.
            if (IsColorSet(control, ShouldSerializeBackColorMethod))
                control.BackColor = ConvertControlColor(oldBack, from, to);

            if (!KeepForeColor(oldBack) && IsColorSet(control, ShouldSerializeForeColorMethod))
                control.ForeColor = ConvertControlColor(control.ForeColor, from, to);

            Button button = control as Button;
            if (button != null)
            {
                FlatButtonAppearance flat = button.FlatAppearance;

                if (!flat.BorderColor.IsEmpty)
                    flat.BorderColor = Convert(flat.BorderColor, from, to);
                if (!flat.MouseOverBackColor.IsEmpty)
                    flat.MouseOverBackColor = Convert(flat.MouseOverBackColor, from, to);
                if (!flat.MouseDownBackColor.IsEmpty)
                    flat.MouseDownBackColor = Convert(flat.MouseDownBackColor, from, to);
            }

            ToolStrip strip = control as ToolStrip;
            if (strip != null)
                ApplyToolStrip(strip, from, to);

            if (control.ContextMenuStrip != null)
                ApplyToolStrip(control.ContextMenuStrip, from, to);

            foreach (Control child in control.Controls)
                ApplyControl(child, from, to);

            control.Invalidate();
        }

        private static void ApplyToolStrip(ToolStrip strip, AppTheme from, AppTheme to)
        {
            strip.BackColor = Convert(strip.BackColor, from, to);
            strip.ForeColor = Convert(strip.ForeColor, from, to);

            foreach (ToolStripItem item in strip.Items)
                ApplyToolStripItem(item, from, to);

            strip.Invalidate();
        }

        private static void ApplyToolStripItem(ToolStripItem item, AppTheme from, AppTheme to)
        {
            item.BackColor = Convert(item.BackColor, from, to);
            item.ForeColor = Convert(item.ForeColor, from, to);

            ToolStripDropDownItem dropDown = item as ToolStripDropDownItem;
            if (dropDown != null && dropDown.HasDropDownItems)
            {
                foreach (ToolStripItem child in dropDown.DropDownItems)
                    ApplyToolStripItem(child, from, to);
            }
        }
    }
}
