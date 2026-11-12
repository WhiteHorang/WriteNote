
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WriteNote
{
    public class RoundedButton : Button
    {
        public int CornerRadius { get; set; } = 7;

        public RoundedButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;

            BackColor = Color.FromArgb(57, 59, 66);
            ForeColor = Color.White;

            Cursor = Cursors.Hand;
            UseVisualStyleBackColor = false;

            UpdateButtonRegion();
        }

        // 버튼 배경과 테두리 그리기
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = ClientRectangle;

            rect.X += 1;
            rect.Y += 1;
            rect.Width -= 3;
            rect.Height -= 3;

            using (GraphicsPath path = CreateRoundedPath(rect))
            {
                // 배경
                using (SolidBrush brush = new SolidBrush(BackColor))
                {
                    g.FillPath(brush, path);
                }

                // 테두리 (어두운 테마: 흰색, 밝은 테마: 회색)
                Color borderColor = ThemeManager.Current == AppTheme.Light
                    ? Color.FromArgb(170, 172, 180)
                    : Color.FromArgb(230, 255, 255, 255);

                using (Pen pen = new Pen(borderColor, 1))
                {
                    g.DrawPath(pen, path);
                }

                // 글자
                TextRenderer.DrawText(
                    g,
                    Text,
                    Font,
                    rect,
                    ForeColor,
                    TextFormatFlags.HorizontalCenter |
                    TextFormatFlags.VerticalCenter
                );
            }
        }

        // 둥근 사각형 경로 생성
        private GraphicsPath CreateRoundedPath(Rectangle rect)
        {
            GraphicsPath path = new GraphicsPath();

            int r = Math.Min(
                CornerRadius * 2,
                Math.Min(rect.Width, rect.Height)
            );

            path.AddArc(rect.X, rect.Y, r, r, 180, 90);
            path.AddArc(rect.Right - r, rect.Y, r, r, 270, 90);
            path.AddArc(rect.Right - r, rect.Bottom - r, r, r, 0, 90);
            path.AddArc(rect.X, rect.Bottom - r, r, r, 90, 90);

            path.CloseFigure();

            return path;
        }

        // 버튼의 실제 표시 영역도 둥글게 설정
        private void UpdateButtonRegion()
        {
            if (Width <= 0 || Height <= 0)
                return;

            using (GraphicsPath path = CreateRoundedPath(
                new Rectangle(0, 0, Width, Height)))
            {
                Region oldRegion = Region;
                Region = new Region(path);

                if (oldRegion != null)
                    oldRegion.Dispose();
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateButtonRegion();
        }
    }
}
