using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace WriteNote
{
    // exe 하나로 배포하기 위해, 필요한 DLL을 exe 안에 리소스로 넣어 두고
    // 프로그램이 그 DLL을 찾을 때 exe 안에서 꺼내 준다.
    //
    // - 넣어 둔 DLL: Newtonsoft.Json, WeCantSpell.Hunspell, System.Memory,
    //   System.Buffers, System.Numerics.Vectors, System.Runtime.CompilerServices.Unsafe
    //   (WriteNote.csproj의 EmbeddedResource, 이름은 "WriteNote.Libs.{DLL 이름}.dll")
    // - .NET은 DLL을 exe 옆에서 찾지 못하면 AssemblyResolve 이벤트를 부른다.
    //   이때 exe 안의 DLL을 읽어 돌려준다.
    // - 버전이 조금 달라도(예: System.Memory가 Unsafe 4.0.4.1을 찾지만 들어 있는 것은 6.0.0.0)
    //   같은 이름이면 돌려주므로, 버전을 맞추는 .config 파일도 필요 없다.
    // - 반드시 프로그램 시작(Program.Main) 맨 처음, 다른 DLL을 쓰기 전에 Register()를 불러야 한다.
    public static class EmbeddedAssemblies
    {
        private const string ResourcePrefix = "WriteNote.Libs.";

        private static readonly Dictionary<string, Assembly> loaded =
            new Dictionary<string, Assembly>(StringComparer.OrdinalIgnoreCase);

        private static bool registered = false;

        public static void Register()
        {
            if (registered)
                return;

            registered = true;
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
        }

        private static Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            string name = new AssemblyName(args.Name).Name;

            lock (loaded)
            {
                Assembly assembly;

                if (loaded.TryGetValue(name, out assembly))
                    return assembly;

                using (Stream stream = typeof(EmbeddedAssemblies).Assembly
                    .GetManifestResourceStream(ResourcePrefix + name + ".dll"))
                {
                    // exe 안에 없는 DLL(예: 언어 리소스)은 .NET의 원래 처리에 맡긴다.
                    if (stream == null)
                        return null;

                    byte[] bytes = new byte[stream.Length];
                    int offset = 0;

                    while (offset < bytes.Length)
                    {
                        int read = stream.Read(bytes, offset, bytes.Length - offset);

                        if (read <= 0)
                            break;

                        offset += read;
                    }

                    assembly = Assembly.Load(bytes);
                    loaded[name] = assembly;

                    return assembly;
                }
            }
        }

        // exe 안에 넣어 둔 텍스트 파일 읽기 (라이선스 문서 등). 없으면 null
        public static string ReadTextResource(string resourceName)
        {
            using (Stream stream = typeof(EmbeddedAssemblies).Assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    return null;

                using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
