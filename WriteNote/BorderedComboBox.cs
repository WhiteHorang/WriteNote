
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WriteNote
{
    public class BorderedComboBox : ComboBox
    {
        private readonly Color borderColor =
            Color.FromArgb(102, 85, 165);

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(
            IntPtr hWnd,
            IntPtr hDC
        );

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(
            IntPtr hWnd,
            out RECT lpRect
        );

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public BorderedComboBox()
        {
            FlatStyle = FlatStyle.Flat;
            BackColor = ThemeManager.C(Color.FromArgb(48, 51, 59));
            ForeColor = ThemeManager.C(Color.FromArgb(230, 230, 234));
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            // 콤보박스 외곽선 다시 그리기
            if (m.Msg == 0x0085 || m.Msg == 0x000F)
            {
                DrawCustomBorder();
            }
        }


        private void DrawCustomBorder()
        {
            if (!IsHandleCreated)
                return;

            IntPtr hdc = GetWindowDC(Handle);

            if (hdc == IntPtr.Zero)
                return;

            try
            {
                RECT rect;

                if (GetWindowRect(Handle, out rect))
                {
                    using (Graphics g = Graphics.FromHdc(hdc))
                    {
                        int width = rect.Right - rect.Left;
                        int height = rect.Bottom - rect.Top;

                        // 오른쪽 화살표 버튼 영역
                        int buttonWidth = 22;

                        Rectangle buttonRect = new Rectangle(
                            width - buttonWidth - 1,
                            1,
                            buttonWidth,
                            height - 2
                        );

                        // 버튼 배경
                        using (SolidBrush brush = new SolidBrush(
                            ThemeManager.C(Color.FromArgb(48, 51, 59))))
                        {
                            g.FillRectangle(brush, buttonRect);
                        }

                        // 아래 방향 화살표
                        using (Pen arrowPen = new Pen(
                            ThemeManager.C(Color.FromArgb(230, 230, 234)), 2))
                        {
                            arrowPen.StartCap =
                                System.Drawing.Drawing2D.LineCap.Round;

                            arrowPen.EndCap =
                                System.Drawing.Drawing2D.LineCap.Round;

                            int centerX = buttonRect.Left
                                        + buttonRect.Width / 2;

                            int centerY = height / 2;

                            Point[] arrowPoints =
                            {
                        new Point(centerX - 4, centerY - 2),
                        new Point(centerX, centerY + 2),
                        new Point(centerX + 4, centerY - 2)
                    };

                            g.DrawLines(arrowPen, arrowPoints);
                        }

                        // 보라색 외곽선
                        using (Pen borderPen = new Pen(
                            Color.FromArgb(102, 85, 165), 2))
                        {
                            g.DrawRectangle(
                                borderPen,
                                0,
                                0,
                                width - 1,
                                height - 1
                            );
                        }
                    }
                }
            }
            finally
            {
                ReleaseDC(Handle, hdc);
            }
        }

    }
}
