param($exe, $delay, $prefill, $keys = "ehoTek goTek", $snap = "")
Add-Type -AssemblyName System.Drawing
Add-Type -ReferencedAssemblies System.Drawing -TypeDefinition @"
using System; using System.Text; using System.Threading; using System.Runtime.InteropServices;
public static class Ime {
  delegate bool EnumProc(IntPtr h, IntPtr l);
  [DllImport("user32.dll")] static extern bool EnumWindows(EnumProc f, IntPtr l);
  [DllImport("user32.dll")] static extern bool EnumChildWindows(IntPtr p, EnumProc f, IntPtr l);
  [DllImport("user32.dll")] static extern uint GetWindowThreadProcessId(IntPtr h, out uint pid);
  [DllImport("user32.dll", CharSet=CharSet.Unicode)] static extern int GetWindowText(IntPtr h, StringBuilder s, int n);
  [DllImport("user32.dll", CharSet=CharSet.Unicode)] static extern int GetClassName(IntPtr h, StringBuilder s, int n);
  [DllImport("user32.dll")] static extern bool IsWindowVisible(IntPtr h);
  [DllImport("user32.dll")] public static extern bool SetForegroundWindow(IntPtr h);
  [DllImport("user32.dll")] public static extern IntPtr GetForegroundWindow();
  [DllImport("user32.dll")] static extern bool PostMessageW(IntPtr h, int msg, IntPtr w, IntPtr l);
  [DllImport("user32.dll", CharSet=CharSet.Unicode)] static extern IntPtr SendMessageW(IntPtr h, int msg, IntPtr w, IntPtr l);
  [DllImport("user32.dll", CharSet=CharSet.Unicode)] static extern IntPtr SendMessageW(IntPtr h, int msg, IntPtr w, StringBuilder l);
  [DllImport("imm32.dll")] static extern IntPtr ImmGetDefaultIMEWnd(IntPtr h);
  [DllImport("user32.dll")] static extern uint SendInput(uint n, INPUT[] inputs, int size);
  [StructLayout(LayoutKind.Sequential)] struct KEYBDINPUT { public ushort wVk; public ushort wScan; public uint dwFlags; public uint time; public IntPtr dwExtraInfo; }
  [StructLayout(LayoutKind.Sequential)] struct MOUSEINPUT { public int dx, dy; public uint mouseData, dwFlags, time; public IntPtr dwExtraInfo; }
  [StructLayout(LayoutKind.Explicit)] struct UNION { [FieldOffset(0)] public MOUSEINPUT mi; [FieldOffset(0)] public KEYBDINPUT ki; }
  [StructLayout(LayoutKind.Sequential)] struct INPUT { public uint type; public UNION u; }

  static string Cls(IntPtr h) { var s = new StringBuilder(256); GetClassName(h, s, 256); return s.ToString(); }
  static string Txt(IntPtr h) { var s = new StringBuilder(512); GetWindowText(h, s, 512); return s.ToString(); }
  public static IntPtr FindMain(int pid) { IntPtr r = IntPtr.Zero; EnumWindows((h, x) => { uint p; GetWindowThreadProcessId(h, out p); if (p == pid && IsWindowVisible(h) && Txt(h) == "WriteNote") r = h; return true; }, IntPtr.Zero); return r; }
  public static IntPtr Rich(IntPtr top) { IntPtr r = IntPtr.Zero; EnumChildWindows(top, (h, x) => { if (Cls(h).ToUpper().Contains("RICHEDIT") && IsWindowVisible(h)) r = h; return true; }, IntPtr.Zero); return r; }
  public static void Click(IntPtr h) { PostMessageW(h, 0x0201, (IntPtr)1, (IntPtr)((20 << 16) | 20)); PostMessageW(h, 0x0202, IntPtr.Zero, (IntPtr)((20 << 16) | 20)); }

  // 한글 입력 모드 켜기 (IME 창에 WM_IME_CONTROL)
  public static string SetHangul(IntPtr h) {
    IntPtr ime = ImmGetDefaultIMEWnd(h);
    SendMessageW(ime, 0x0283, (IntPtr)0x0006, (IntPtr)1);            // IMC_SETOPENSTATUS = 열기
    SendMessageW(ime, 0x0283, (IntPtr)0x0002, (IntPtr)1);            // IMC_SETCONVERSIONMODE = 한글(NATIVE)
    long mode = SendMessageW(ime, 0x0283, (IntPtr)0x0001, IntPtr.Zero).ToInt64(); // IMC_GETCONVERSIONMODE
    return "IME창=" + ime + " 변환모드=" + mode;
  }

  static void Key(ushort vk, bool up) {
    var input = new INPUT[1];
    input[0].type = 1;
    input[0].u.ki.wVk = vk;
    input[0].u.ki.dwFlags = up ? 2u : 0u;
    SendInput(1, input, Marshal.SizeOf(typeof(INPUT)));
  }

  // 키를 보낼 때마다 WriteNote가 맨 앞인지 확인. 아니면 멈춘다.
  public static string Type(IntPtr main, string keys, int delayMs) {
    foreach (char k in keys) {
      if (GetForegroundWindow() != main) return "중단: WriteNote가 맨 앞 창이 아님";
      bool shift = char.IsUpper(k);
      ushort vk = (ushort)char.ToUpper(k);
      if (shift) Key(0x10, false);
      Key(vk, false); Key(vk, true);
      if (shift) Key(0x10, true);
      Thread.Sleep(delayMs);
    }
    return "입력 완료";
  }

  public static void Fill(IntPtr h, int n) { var sb = new StringBuilder(); while (sb.Length < n) sb.Append("오늘은 날씨가 정말 좋았다. 그녀는 창밖을 바라보며 조용히 웃었다. "); SendMessageW(h, 0xC2, (IntPtr)1, sb.ToString()); }
  [DllImport("user32.dll", CharSet=CharSet.Unicode)] static extern IntPtr SendMessageW(IntPtr h, int msg, IntPtr w, string l);
  [DllImport("user32.dll")] static extern bool PrintWindow(IntPtr h, IntPtr hdc, uint f);
  [DllImport("user32.dll")] static extern bool GetWindowRect(IntPtr h, out RECT r);
  public struct RECT { public int L, T, R, B; }
  public static void Snap(IntPtr h, string path) { RECT r; GetWindowRect(h, out r); var b = new System.Drawing.Bitmap(r.R - r.L, r.B - r.T); var g = System.Drawing.Graphics.FromImage(b); var dc = g.GetHdc(); PrintWindow(h, dc, 2); g.ReleaseHdc(dc); g.Dispose(); b.Save(path); b.Dispose(); }
  public static string ReadText(IntPtr h) { var sb = new StringBuilder(400000); SendMessageW(h, 0x000D, (IntPtr)400000, sb); return sb.ToString(); }
}
"@
$p = Start-Process $exe -PassThru
Start-Sleep 4
$main = [Ime]::FindMain($p.Id); $rich = [Ime]::Rich($main)
[Ime]::SetForegroundWindow($main) | Out-Null
Start-Sleep -Milliseconds 300
[Ime]::Click($rich); Start-Sleep -Milliseconds 300
"한글 모드: " + [Ime]::SetHangul($rich)
Start-Sleep -Milliseconds 200
# 두벌식: 됐다 = e h o T e k, 공백, 했다 = g o T e k
if ($prefill -gt 0) { [Ime]::Fill($rich, $prefill) }; Start-Sleep -Milliseconds 700
"입력 결과: " + [Ime]::Type($main, $keys, $delay)
Start-Sleep -Milliseconds 800
$txt = [Ime]::ReadText($rich); "본문 끝부분: [" + $txt.Substring([Math]::Max(0, $txt.Length - 12)) + "]"
if ($snap) { Start-Sleep -Milliseconds 2000; [Ime]::Snap($main, $snap) }
Stop-Process $p -Force
