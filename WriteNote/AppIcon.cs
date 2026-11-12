using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WriteNote
{
    // WriteNote 아이콘
    //
    // - exe 파일 아이콘(탐색기, 바탕 화면)은 csproj의 ApplicationIcon(WriteNote.ico)으로 정한다.
    // - 창 아이콘(제목 표시줄, 작업 표시줄, Alt+Tab, 작업 관리자)은 창마다 따로 정해야 해서
    //   exe 안에 리소스로 넣은 같은 아이콘을 읽어 모든 창에 쓴다.
    //   (WriteNote.ico에는 16~256 크기가 모두 들어 있어 화면 배율에 맞는 크기가 쓰인다)
    public static class AppIcon
    {
        private static Icon icon;
        private static bool loaded = false;

        // 아이콘을 읽지 못하면 null (창은 Windows 기본 아이콘으로 보인다)
        public static Icon Icon
        {
            get
            {
                if (!loaded)
                {
                    loaded = true;

                    try
                    {
                        using (Stream stream = typeof(AppIcon).Assembly
                            .GetManifestResourceStream("WriteNote.AppIcon.ico"))
                        {
                            if (stream != null)
                                icon = new Icon(stream);
                        }
                    }
                    catch (Exception)
                    {
                        icon = null;
                    }
                }

                return icon;
            }
        }

        // 창에 WriteNote 아이콘을 넣는다.
        public static void Apply(Form form)
        {
            if (Icon != null)
                form.Icon = Icon;
        }
    }
}
