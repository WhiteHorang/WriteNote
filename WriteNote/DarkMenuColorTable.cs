
using System.Drawing;
using System.Windows.Forms;

namespace WriteNote
{
    // 우클릭 메뉴 색상 (현재 테마에 맞춰 색을 돌려준다)
    public class DarkMenuColorTable : ProfessionalColorTable
    {
        // 메뉴 배경
        private static Color Background
        {
            get { return ThemeManager.C(Color.FromArgb(48, 51, 59)); }
        }

        // 마우스를 올린 항목 (밝은 테마에서는 글자가 잘 보이도록 연한 강조색)
        private static Color Hover
        {
            get
            {
                return ThemeManager.Current == AppTheme.Light
                    ? Color.FromArgb(205, 196, 240)
                    : ThemeManager.Accent;
            }
        }

        // 메뉴 테두리·구분선
        private static Color Border
        {
            get { return ThemeManager.C(Color.FromArgb(65, 68, 78)); }
        }

        // 메뉴 전체 배경
        public override Color ToolStripDropDownBackground
        {
            get { return Background; }
        }

        // 왼쪽 아이콘 여백
        public override Color ImageMarginGradientBegin
        {
            get { return Background; }
        }

        public override Color ImageMarginGradientMiddle
        {
            get { return Background; }
        }

        public override Color ImageMarginGradientEnd
        {
            get { return Background; }
        }

        // 마우스를 올렸을 때
        public override Color MenuItemSelected
        {
            get { return Hover; }
        }

        public override Color MenuItemSelectedGradientBegin
        {
            get { return Hover; }
        }

        public override Color MenuItemSelectedGradientEnd
        {
            get { return Hover; }
        }

        // 메뉴 항목을 누르고 있을 때
        public override Color MenuItemPressedGradientBegin
        {
            get { return ThemeManager.C(Color.FromArgb(80, 67, 135)); }
        }

        public override Color MenuItemPressedGradientEnd
        {
            get { return ThemeManager.C(Color.FromArgb(80, 67, 135)); }
        }

        // 메뉴 테두리
        public override Color MenuBorder
        {
            get { return Border; }
        }

        public override Color MenuItemBorder
        {
            get { return ThemeManager.Accent; }
        }

        // 메뉴 구분선
        public override Color SeparatorDark
        {
            get { return Border; }
        }

        public override Color SeparatorLight
        {
            get { return Border; }
        }
    }
}
