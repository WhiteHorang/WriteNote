using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace WriteNote
{
    internal static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // exe 안에 넣어 둔 DLL을 쓸 수 있도록 가장 먼저 등록한다. (exe 하나로 배포)
            EmbeddedAssemblies.Register();

            Run();
        }

        // 다른 DLL(Newtonsoft.Json 등)을 쓰는 코드는 Register() 뒤에 실행되도록 따로 둔다.
        // (Main 안에 함께 있으면 Register 전에 DLL을 찾으려 할 수 있다)
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Run()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
