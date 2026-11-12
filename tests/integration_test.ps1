param($bin, $work)
$ErrorActionPreference = "Stop"
Add-Type -AssemblyName System.Windows.Forms, System.Drawing
[System.Windows.Forms.Application]::EnableVisualStyles()

$appData = Join-Path $work "appdata_test"
$env:WRITENOTE_DATA_DIR = $appData
$backup = $null
$docsA = Join-Path $work "docsA"
$docsB = Join-Path $work "docsB"

# ── 사용자 데이터 백업 ──
Remove-Item $docsA, $docsB -Recurse -Force -ErrorAction SilentlyContinue
# 실제 사용자 데이터 폴더는 쓰지 않으므로 백업·복원이 필요 없다.
$hadAppData = $false
$savedClipboard = $null
try { if ([System.Windows.Forms.Clipboard]::ContainsText()) { $savedClipboard = [System.Windows.Forms.Clipboard]::GetText() } } catch {}

$results = New-Object System.Collections.ArrayList
$log = Join-Path $work "integration_progress.log"
Set-Content $log "" -Encoding UTF8
function Check($name, $ok, $detail = "") {
  $r = $(if ($ok) { "통과" } else { "실패" })
  [void]$results.Add([pscustomobject]@{ Result = $r; Test = $name; Detail = $detail })
  Add-Content $log "$r | $name | $detail" -Encoding UTF8
}

Add-Type -TypeDefinition @"
using System; using System.Text; using System.Threading; using System.Collections.Concurrent; using System.Runtime.InteropServices;
public static class DialogWatchdog {
  delegate bool EnumProc(IntPtr h, IntPtr l);
  [DllImport("user32.dll")] static extern bool EnumWindows(EnumProc f, IntPtr l);
  [DllImport("user32.dll")] static extern bool EnumChildWindows(IntPtr p, EnumProc f, IntPtr l);
  [DllImport("user32.dll")] static extern uint GetWindowThreadProcessId(IntPtr h, out uint pid);
  [DllImport("user32.dll", CharSet=CharSet.Unicode)] static extern int GetClassName(IntPtr h, StringBuilder s, int n);
  [DllImport("user32.dll", CharSet=CharSet.Unicode)] static extern int GetWindowText(IntPtr h, StringBuilder s, int n);
  [DllImport("user32.dll")] static extern bool IsWindowVisible(IntPtr h);
  [DllImport("user32.dll")] static extern bool PostMessage(IntPtr h, uint m, IntPtr w, IntPtr l);
  public static ConcurrentQueue<string> Seen = new ConcurrentQueue<string>();
  static Timer timer; static uint myPid;
  public static void Start() { myPid = (uint)System.Diagnostics.Process.GetCurrentProcess().Id; timer = new Timer(Tick, null, 300, 300); }
  public static void Stop() { if (timer != null) timer.Dispose(); }
  static string Cls(IntPtr h) { var s = new StringBuilder(64); GetClassName(h, s, 64); return s.ToString(); }
  static string Txt(IntPtr h) { var s = new StringBuilder(1024); GetWindowText(h, s, 1024); return s.ToString(); }
  static void Tick(object o) {
    EnumWindows((h, l) => {
      uint pid; GetWindowThreadProcessId(h, out pid);
      if (pid == myPid && IsWindowVisible(h) && (Txt(h) == "문서 속성" || Txt(h) == "문서 이름 변경")) {
        var t2 = new StringBuilder();
        EnumChildWindows(h, (c, l2) => { string tx = Txt(c); if (tx.Length > 0) t2.Append(tx).Append(" "); return true; }, IntPtr.Zero);
        Seen.Enqueue("[" + Txt(h) + "] " + t2.ToString().Replace("\r", " ").Replace("\n", " ").Trim());
        PostMessage(h, 0x0010, IntPtr.Zero, IntPtr.Zero);  // WM_CLOSE
      }
      else if (pid == myPid && IsWindowVisible(h) && Cls(h) == "#32770") {
        var text = new StringBuilder();
        EnumChildWindows(h, (c, l2) => { if (Cls(c) == "Static") text.Append(Txt(c)).Append(" "); return true; }, IntPtr.Zero);
        Seen.Enqueue("[" + Txt(h) + "] " + text.ToString().Replace("\r", " ").Replace("\n", " ").Trim());
        PostMessage(h, 0x0111, (IntPtr)2, IntPtr.Zero);  // IDCANCEL (취소 / 확인만 있는 창)
        PostMessage(h, 0x0111, (IntPtr)7, IntPtr.Zero);  // IDNO (예/아니요 창)
      }
      return true;
    }, IntPtr.Zero);
  }
}
"@
[DialogWatchdog]::Start()
function TakeDialogs { $list = @(); $item = $null; while ([DialogWatchdog]::Seen.TryDequeue([ref]$item)) { $list += $item }; return ,$list }

try {
  # 깨끗한 상태에서 시작 (설정·단축키·최근 문서 없음)
  if (Test-Path $appData) { Remove-Item $appData -Recurse -Force }
  New-Item -ItemType Directory $docsA, $docsB | Out-Null

  Add-Type -TypeDefinition @"
using System; using System.IO; using System.Reflection;
public static class DepResolverT {
  public static void Init(string d) {
    AppDomain.CurrentDomain.AssemblyResolve += (s, e) => {
      string p = Path.Combine(d, new AssemblyName(e.Name).Name + ".dll");
      return File.Exists(p) ? Assembly.LoadFrom(p) : null;
    };
  }
}
"@
  [DepResolverT]::Init($bin)
  $asm = [Reflection.Assembly]::LoadFrom("$bin\WriteNote.exe")
  # exe 안에 넣어 둔 DLL을 쓰도록 등록 (프로그램의 Program.Main이 하는 일과 같음)
  $asm.GetType("WriteNote.EmbeddedAssemblies").GetMethod("Register").Invoke($null, @()) | Out-Null
  $NP = [Reflection.BindingFlags]"NonPublic,Instance"
  $K = [System.Windows.Forms.Keys]
  function F($o, $n) { return ,($o.GetType().GetField($n, $NP).GetValue($o)) }
  function SetF($o, $n, $v) { $o.GetType().GetField($n, $NP).SetValue($o, $v) }
  function M($o, $n, $a = @()) {
    $raw = New-Object 'object[]' $a.Count
    for ($i = 0; $i -lt $a.Count; $i++) {
      $v = $a[$i]
      if ($v -ne $null -and $v -is [System.Management.Automation.PSObject]) { $v = $v.psobject.BaseObject }
      $raw[$i] = $v
    }
    $o.GetType().GetMethod($n, $NP).Invoke($o, $raw)
  }
  function E($typeName, $value) { [Enum]::Parse($asm.GetType("WriteNote.$typeName"), $value) }
  function Hide($f) { $f.StartPosition = "Manual"; $f.Location = New-Object Drawing.Point(-3000, -3000); $f.ShowInTaskbar = $false }
  function Pump { for ($i = 0; $i -lt 5; $i++) { [System.Windows.Forms.Application]::DoEvents() } }
  function Key($main, $keys) {
    $e = New-Object System.Windows.Forms.KeyEventArgs($keys)
    M $main "MainForm_KeyDown" @($main, $e)
    return $e
  }

  # ── 메인 화면 준비 ──
  $main = [Activator]::CreateInstance($asm.GetType("WriteNote.MainForm"))
  Hide $main; $main.Show(); Pump
  $rtb = F $main "rtbContent"; $title = F $main "txtTitle"; $status = F $main "lblSaveStatus"
  $settings = F $main "appSettings"
  $settings.Storage.DocumentFolder = $docsA

  # ════ 1. 글쓰기 단축키 ════
  $rtb.Clear(); Key $main ($K::Control -bor $K::OemPeriod) | Out-Null
  Check "단축키: Ctrl + . → 말줄임표" ($rtb.Text -eq "⋯") "본문=[$($rtb.Text)]"

  $rtb.Clear(); Key $main ($K::Control -bor $K::OemOpenBrackets) | Out-Null
  Check "단축키: Ctrl + [ → 「」, 커서는 괄호 안" ($rtb.Text -eq "「」" -and $rtb.SelectionStart -eq 1) "본문=[$($rtb.Text)] 커서=$($rtb.SelectionStart)"

  $rtb.Clear(); Key $main ($K::Control -bor $K::Shift -bor $K::OemPeriod) | Out-Null
  Check "단축키: Ctrl + Shift + . → 구분선 (Ctrl + . 과 구분)" ($rtb.Text -eq "*   *   *") "본문=[$($rtb.Text)]"

  $rtb.Text = "AB"; $rtb.SelectionStart = 1; Key $main ($K::Control -bor $K::Enter) | Out-Null
  Check "단축키: 문서 중간 Ctrl + Enter → 커서가 빈 줄 바로 뒤" ($rtb.Text -eq "A`n`n`nB" -and $rtb.SelectionStart -eq 4) "커서=$($rtb.SelectionStart) (기대 4)"

  $custom = [Activator]::CreateInstance($asm.GetType("WriteNote.ShortcutItem"))
  $custom.Name = "인사"; $custom.Keys = ($K::Control -bor $K::Alt -bor $K::S); $custom.Content = "안녕`r`n하세요"
  (F $main "writingShortcuts").Add($custom)
  $tmpDoc = Join-Path $docsA "custom_key_guard.rtf"; SetF $main "currentFilePath" $tmpDoc
  $rtb.Clear(); Key $main ($K::Control -bor $K::Alt -bor $K::S) | Out-Null
  Check "단축키: 사용자 단축키 Ctrl + Alt + S가 '저장'보다 먼저 실행" ($rtb.Text -eq "안녕`n하세요" -and -not (Test-Path $tmpDoc)) "본문=[$($rtb.Text)] 저장파일생김=$(Test-Path $tmpDoc)"
  Check "단축키: 여러 줄 내용 입력 후 커서가 내용 끝" ($rtb.SelectionStart -eq $rtb.TextLength) "커서=$($rtb.SelectionStart) 길이=$($rtb.TextLength)"
  SetF $main "currentFilePath" ""

  $settings.Writing.WritingShortcutsEnabled = $false
  $rtb.Clear(); $e = Key $main ($K::Control -bor $K::OemPeriod)
  Check "글쓰기 설정: 단축키 끄면 Ctrl + . 무시" ($rtb.Text -eq "" -and -not $e.Handled) "본문=[$($rtb.Text)]"
  $settings.Writing.WritingShortcutsEnabled = $true

  # ════ 2. 글자 수 ════
  $rtb.Text = "가 나`n다`t라"
  $settings.Writing.CharCountMode = E "CharCountMode" "IncludeSpaces"; M $main "UpdateCharCount"
  $lbl = (F $main "lblCharCount").Text
  Check "글자 수: 공백 포함 (줄바꿈 제외)" ($lbl -eq "글자 수: 6 (공백 포함)") $lbl
  $settings.Writing.CharCountMode = E "CharCountMode" "ExcludeSpaces"; M $main "UpdateCharCount"
  $lbl = (F $main "lblCharCount").Text
  Check "글자 수: 공백 제외" ($lbl -eq "글자 수: 4 (공백 제외)") $lbl
  $settings.Writing.CharCountMode = E "CharCountMode" "IncludeSpaces"

  # ════ 3. 붙여넣기 서식 제거 ════
  $rich = New-Object System.Windows.Forms.DataObject
  $rich.SetData([System.Windows.Forms.DataFormats]::Rtf, "{\rtf1{\colortbl;\red255\green0\blue0;}\cf1 RED}")
  $rich.SetData([System.Windows.Forms.DataFormats]::UnicodeText, "붙여`r`n넣기")
  [System.Windows.Forms.Clipboard]::SetDataObject($rich, $true, 10, 100); Start-Sleep -Milliseconds 100
  $rtb.Clear(); M $main "PastePlainText"
  $rtb.SelectAll()
  Check "붙여넣기: 서식 빼고 글자만" ($rtb.Text -eq "붙여`n넣기" -and $rtb.SelectionColor.ToArgb() -eq $rtb.ForeColor.ToArgb()) "본문=[$($rtb.Text)] 글자색=$($rtb.SelectionColor.Name)"

  # ════ 4. 저장 / 불러오기 ════
  $doc1 = Join-Path $docsA "doc1.rtf"
  M $main "CreateNewDocument"
  $title.Text = "테스트 문서"; $rtb.Text = "첫 줄`n둘째 줄 ⋯ 「인용」"
  SetF $main "currentFilePath" $doc1
  $saved = M $main "SaveDocument"
  $info = Get-Content ([IO.Path]::ChangeExtension($doc1, ".json")) -Raw -Encoding UTF8 | ConvertFrom-Json
  Check "저장: RTF와 문서 정보(JSON) 생성" ($saved -and (Test-Path $doc1) -and $info.Title -eq "테스트 문서") "제목=$($info.Title)"
  Check "저장: 저장 후 상태 '저장됨'" (-not (F $main "isModified") -and $status.Text -eq "저장됨") $status.Text

  M $main "CreateNewDocument"
  M $main "LoadDocument" @($doc1)
  Check "불러오기: 본문·제목 복원" ($rtb.Text -eq "첫 줄`n둘째 줄 ⋯ 「인용」" -and $title.Text -eq "테스트 문서") "제목=$($title.Text)"
  Check "불러오기: '수정됨'으로 바뀌지 않음" (-not (F $main "isModified") -and $status.Text -eq "저장됨") $status.Text
  $recent = [WriteNote.RecentDocumentManager]::GetRecentDocuments()
  Check "최근 문서: 열었던 문서가 맨 앞" ($recent.Count -ge 1 -and $recent[0] -eq $doc1) ($recent -join ", ")

  # ════ 5. 자동 저장 ════
  $settings.Writing.AutoSaveEnabled = $true; M $main "ApplyWritingSettings"
  $timer = F $main "autoSaveTimer"
  Check "자동 저장: 타이머 간격 = 설정값" ($timer.Enabled -and $timer.Interval -eq 5 * 60 * 1000) "간격=$($timer.Interval)"
  $rtb.AppendText("`n자동 저장 내용")
  M $main "autoSaveTimer_Tick" @($null, [EventArgs]::Empty)
  $check = New-Object System.Windows.Forms.RichTextBox; $check.LoadFile($doc1)
  Check "자동 저장: 바뀐 내용이 파일에 저장" ($check.Text.Contains("자동 저장 내용") -and -not (F $main "isModified") -and $status.Text.StartsWith("자동 저장됨")) $status.Text
  $check.Dispose()

  M $main "CreateNewDocument"; $rtb.Text = "아직 저장 안 한 새 문서"
  M $main "autoSaveTimer_Tick" @($null, [EventArgs]::Empty)
  Check "자동 저장: 저장한 적 없는 새 문서는 건너뜀" ((F $main "isModified") -and $status.Text -eq "수정됨") $status.Text
  $settings.Writing.AutoSaveEnabled = $false; M $main "ApplyWritingSettings"
  Check "자동 저장: 끄면 타이머 정지" (-not $timer.Enabled) ""

  # ════ 6. 테마 ════
  M $main "LoadDocument" @($doc1)
  $snapshot = @{}
  function Walk($c, $path) {
    $snapshot["$path|B"] = $c.BackColor.ToArgb(); $snapshot["$path|F"] = $c.ForeColor.ToArgb()
    $i = 0; foreach ($ch in $c.Controls) { Walk $ch "$path/$($ch.Name)#$i"; $i++ }
  }
  function CompareColors($c, $path, [ref]$diff) {
    if ($snapshot["$path|B"] -ne $c.BackColor.ToArgb() -or $snapshot["$path|F"] -ne $c.ForeColor.ToArgb()) { $diff.Value += "$path " }
    $i = 0; foreach ($ch in $c.Controls) { CompareColors $ch "$path/$($ch.Name)#$i" $diff; $i++ }
  }
  Walk $main "main"
  $settings.Appearance.Theme = E "AppTheme" "Light"
  M $main "ApplyAppearanceSettings" @((E "AppTheme" "Dark"))
  $rtb.SelectAll()
  Check "테마: 밝은 테마에서 본문 배경·글자색" ($rtb.BackColor.ToArgb() -eq [Drawing.Color]::FromArgb(252,252,253).ToArgb() -and $rtb.SelectionColor.ToArgb() -eq [Drawing.Color]::FromArgb(28,28,30).ToArgb()) "배경=$($rtb.BackColor) 글자=$($rtb.SelectionColor)"
  Check "테마: 테마를 바꿔도 '수정됨' 아님" (-not (F $main "isModified")) $status.Text

  M $main "CreateNewDocument"; M $main "LoadDocument" @($doc1)
  $rtb.SelectAll()
  Check "테마: 어두운 테마에서 저장한 문서를 밝은 테마에서 열면 글자색 맞춤" ($rtb.SelectionColor.ToArgb() -eq $rtb.ForeColor.ToArgb()) "글자=$($rtb.SelectionColor)"

  $settings.Appearance.Theme = E "AppTheme" "Dark"
  M $main "ApplyAppearanceSettings" @((E "AppTheme" "Light"))
  $diff = ""; CompareColors $main "main" ([ref]$diff)
  Check "테마: 어두운→밝은→어두운 후 모든 컨트롤 색이 원래대로" ($diff -eq "") $diff

  # ════ 7. 글꼴 ════
  $settings.Appearance.ContentFontName = "Arial"; $settings.Appearance.ContentFontSize = 15; $settings.Appearance.TitleFontSize = 20
  M $main "ApplyAppearanceSettings" @((E "AppTheme" "Dark"))
  $rtb.SelectAll()
  $fontInfo = @{}; $allSize15 = $true
  for ($i = 0; $i -lt $rtb.TextLength; $i++) {
    $rtb.Select($i, 1); $f = $rtb.SelectionFont
    if ($rtb.SelectedText -match '\s') { continue }
    if ($f.Size -ne 15) { $allSize15 = $false }
    $kind = if ($rtb.SelectedText -match '[가-힣]') { "한글" } else { "기호·영문" }
    $fontInfo["$kind=$($f.Name)"] = 1
  }
  Check "글꼴: 본문 전체 글자 크기 적용 (15pt)" $allSize15 (($fontInfo.Keys | Sort-Object) -join ", ")
  # 어두운 테마·맑은 고딕으로 저장한 영문 문서를 Arial 설정에서 열기
  $docEn = Join-Path $docsA "english.rtf"
  $tmpRtb = New-Object System.Windows.Forms.RichTextBox; $null = $tmpRtb.Handle; $tmpRtb.Font = New-Object Drawing.Font("맑은 고딕", 12); $tmpRtb.Text = "Hello WriteNote"; $tmpRtb.SaveFile($docEn); $tmpRtb.Dispose()
  M $main "LoadDocument" @($docEn)
  $rtb.SelectAll()
  Check "글꼴: 다른 글꼴로 저장된 문서를 열면 설정 글꼴(Arial)로 표시" ($rtb.SelectionFont -ne $null -and $rtb.SelectionFont.Name -eq "Arial" -and $rtb.SelectionFont.Size -eq 15) "$($rtb.SelectionFont.Name) $($rtb.SelectionFont.Size)"
  M $main "CreateNewDocument"; $rtb.AppendText("typed text"); $rtb.SelectAll()
  Check "글꼴: 새 문서에 입력한 영문도 Arial" ($rtb.SelectionFont.Name -eq "Arial") $rtb.SelectionFont.Name
  Remove-Item $docEn, ([IO.Path]::ChangeExtension($docEn, ".json")) -ErrorAction SilentlyContinue
  Check "글꼴: 제목 크기 적용" ($title.Font.Size -eq 20) "$($title.Font.Size)"

  # ════ 8. 문서 목록 / 폴더 ════
  Set-Content (Join-Path $docsA "doc2.rtf") "{\rtf1 doc2}"
  '{ "Title": "두 번째", "IsFavorite": true }' | Set-Content (Join-Path $docsA "doc2.json") -Encoding UTF8
  M $main "LoadRecentDocuments"
  $flp = F $main "flpDocumentList"; $tpl = F $main "pnlDocumentItem"; $empty = F $main "lblEmptyDocuments"
  $cards = @($flp.Controls | Where-Object { $_ -ne $tpl -and $_ -ne $empty })
  Check "문서 목록: 설정한 폴더의 문서만 표시" ($cards.Count -eq 2) "카드 수=$($cards.Count)"
  $folderPath = $main.GetType().GetProperty("DocumentFolderPath", $NP).GetValue($main)
  Check "문서 폴더: 설정한 폴더 사용" ($folderPath -eq $docsA) $folderPath

  # ════ 9. 문서 옮기기 ════
  Set-Content (Join-Path $docsB "doc2.rtf") "{\rtf1 conflict}"
  SetF $main "currentFilePath" $doc1
  $skipped = $null
  $args9 = New-Object 'object[]' 3; $args9[0] = [string[]]@(Get-ChildItem $docsA -Filter *.rtf | ForEach-Object { $_.FullName }); $args9[1] = [string]$docsB
  $moved = $main.GetType().GetMethod("MoveDocuments", $NP).Invoke($main, $args9)
  $skipped = $args9[2]
  $newDoc1 = Join-Path $docsB "doc1.rtf"
  Check "문서 옮기기: 같은 이름 없는 문서만 이동" ($moved -eq 1 -and $skipped.Count -eq 1) "이동=$moved 건너뜀=$($skipped -join '; ')"
  Check "문서 옮기기: RTF와 JSON 함께 이동" ((Test-Path $newDoc1) -and (Test-Path ([IO.Path]::ChangeExtension($newDoc1, ".json"))) -and -not (Test-Path $doc1)) ""
  Check "문서 옮기기: 겹치는 문서는 덮어쓰지 않음" ((Get-Content (Join-Path $docsB "doc2.rtf") -Raw).Contains("conflict") -and (Test-Path (Join-Path $docsA "doc2.rtf"))) ""
  Check "문서 옮기기: 열린 문서 경로 갱신" ((F $main "currentFilePath") -eq $newDoc1) (F $main "currentFilePath")
  $recent = [WriteNote.RecentDocumentManager]::GetRecentDocuments()
  Check "문서 옮기기: 최근 문서 경로 갱신 (새로 추가는 안 함)" ($recent -contains $newDoc1 -and -not ($recent -contains $doc1)) ($recent -join ", ")

  # ════ 10. 설정 저장 유지 ════
  $s = [Activator]::CreateInstance($asm.GetType("WriteNote.AppSettings"))
  $s.General.StartupAction = E "StartupAction" "LastDocument"; $s.General.OpenSidebarOnStartup = $true
  $s.Writing.AutoSaveEnabled = $true; $s.Writing.AutoSaveIntervalMinutes = 10; $s.Writing.CharCountMode = E "CharCountMode" "ExcludeSpaces"
  $s.Writing.PastePlainText = $true; $s.Writing.WritingShortcutsEnabled = $false
  $s.Storage.DocumentFolder = $docsB; $s.Appearance.Theme = E "AppTheme" "Light"; $s.Appearance.ContentFontName = "Arial"; $s.Appearance.ContentFontSize = 14; $s.Appearance.TitleFontSize = 18
  [WriteNote.SettingsManager]::Save($s)
  $r = [WriteNote.SettingsManager]::Load()
  $same = ($r.General.StartupAction -eq $s.General.StartupAction -and $r.General.OpenSidebarOnStartup -and $r.Writing.AutoSaveEnabled -and $r.Writing.AutoSaveIntervalMinutes -eq 10 -and "$($r.Writing.CharCountMode)" -eq "ExcludeSpaces" -and $r.Writing.PastePlainText -and -not $r.Writing.WritingShortcutsEnabled -and $r.Storage.DocumentFolder -eq $docsB -and "$($r.Appearance.Theme)" -eq "Light" -and $r.Appearance.ContentFontName -eq "Arial" -and $r.Appearance.ContentFontSize -eq 14 -and $r.Appearance.TitleFontSize -eq 18)
  Check "설정 유지: 모든 항목 저장 후 그대로 복원" $same ""

  $list = [WriteNote.ShortcutManager]::GetDefaultShortcuts(); $list.Add($custom)
  [WriteNote.ShortcutManager]::Save($list)
  $loaded = [WriteNote.ShortcutManager]::Load()
  $c2 = $loaded | Where-Object { $_.Name -eq "인사" }
  Check "단축키 유지: 사용자 단축키 저장 후 복원" ($loaded.Count -eq 7 -and $c2 -ne $null -and $c2.Keys -eq $custom.Keys -and $c2.Content -eq $custom.Content) "개수=$($loaded.Count)"

  # ════ 11. 설정 창 ════
  $sfType = $asm.GetType("WriteNote.SettingsForm")
  $sf = [Activator]::CreateInstance($sfType); Hide $sf; $sf.Show(); Pump
  Check "설정 창: 저장된 설정을 화면에 표시" ((F $sf "cboTheme").SelectedIndex -eq 1 -and (F $sf "chkAutoSave").Checked -and (F $sf "rdoStartLastDocument").Checked -and (F $sf "txtDocumentFolder").Text -eq $docsB -and (F $sf "lvShortcuts").Items.Count -eq 7) ""
  Check "설정 창: 열기만 해서는 '변경됨' 아님" (-not (F $sf "hasUnsavedChanges")) ""
  (F $sf "chkPastePlainText").Checked = $false
  Check "설정 창: 값을 바꾸면 '변경됨'" ((F $sf "hasUnsavedChanges") -and -not (F $sf "settings").Writing.PastePlainText) ""
  SetF $sf "hasUnsavedChanges" $false; $sf.Close(); $sf.Dispose()

  # ════ 12. 단축키 입력 창 ════
  $edType = $asm.GetType("WriteNote.ShortcutEditForm")
  $ctorArgs = New-Object 'object[]' 1; $ctorArgs[0] = $loaded; $ed = [Activator]::CreateInstance($edType, $ctorArgs); Hide $ed; $ed.Show(); Pump
  $keyBox = F $ed "txtShortcutKey"; $keyBox.Focus() | Out-Null; Pump
  if ($keyBox.Focused) {
    $pck = $edType.GetMethod("ProcessCmdKey", $NP)
    $msg = New-Object System.Windows.Forms.Message
    function PressKey($keys) { $pa = New-Object 'object[]' 2; $pa[0] = $msg; $pa[1] = [System.Windows.Forms.Keys]$keys; $pck.Invoke($ed, $pa) | Out-Null }
    PressKey ($K::Control -bor $K::Shift -bor $K::D7)
    Check "단축키 입력: Ctrl + Shift + 7 인식" ($keyBox.Text -eq "Ctrl + Shift + 7") $keyBox.Text
    PressKey ($K::Shift -bor $K::A)
    Check "단축키 입력: Shift + A는 거부하고 기존 값 유지" ($keyBox.Text -eq "Ctrl + Shift + 7") $keyBox.Text
    PressKey $K::Back
    Check "단축키 입력: Backspace로 지우기" ($ed.ShortcutKeys -eq $K::None) "$($ed.ShortcutKeys)"
  } else {
    Check "단축키 입력: (화면 밖 창이라 포커스를 줄 수 없어 건너뜀)" $true "직접 확인 필요"
  }
  $ed.Close(); $ed.Dispose()

  # ════ 13. 검토 수정 확인 ════
  $settings.Storage.DocumentFolder = $docsA
  $docOk = Join-Path $docsA "ok.rtf"; $docBad = Join-Path $docsA "badinfo.rtf"; $docBroken = Join-Path $docsA "broken.rtf"
  $tmpRtb = New-Object System.Windows.Forms.RichTextBox; $null = $tmpRtb.Handle
  $tmpRtb.Text = "정상 문서"; $tmpRtb.SaveFile($docOk)
  $tmpRtb.Text = "정보 파일이 깨진 문서"; $tmpRtb.SaveFile($docBad); $tmpRtb.Dispose()
  Set-Content ([IO.Path]::ChangeExtension($docBad, ".json")) "{ 이건 JSON 아님" -Encoding UTF8
  [IO.File]::WriteAllBytes($docBroken, [byte[]](0,1,2,3,4,5))
  TakeDialogs | Out-Null

  M $main "LoadDocument" @($docOk); $rtb.AppendText(" 수정")
  M $main "LoadDocument" @($docBad)
  Check "불러오기: 정보(JSON)가 깨져도 문서는 열리고 제목은 파일 이름" ((F $main "currentFilePath") -eq $docBad -and $title.Text -eq "badinfo" -and $rtb.Text -eq "정보 파일이 깨진 문서") "경로=$(F $main 'currentFilePath') 제목=$($title.Text)"

  M $main "LoadDocument" @($docOk); $rtb.AppendText(" 수정")
  $before = $rtb.Text
  M $main "LoadDocument" @($docBroken)
  $dlg = TakeDialogs
  Check "불러오기: 읽을 수 없는 파일이면 지금 문서와 경로 그대로" ((F $main "currentFilePath") -eq $docOk -and $rtb.Text -eq $before) "경로=$(F $main 'currentFilePath') 메시지=$($dlg -join ' / ')"

  # 밝은 테마: 입력칸 색, 상속 색, 설정 메뉴 글자색, 실행 취소
  $settings.Appearance.Theme = E "AppTheme" "Light"
  M $main "ApplyAppearanceSettings" @((E "AppTheme" "Dark"))
  Check "테마: 바꾼 뒤 실행 취소 기록 비움 (Ctrl+Z로 이전 글자색 복원 방지)" (-not $rtb.CanUndo) ""
  M $main "LoadRecentDocuments"
  $settings.Appearance.Theme = E "AppTheme" "Dark"
  M $main "ApplyAppearanceSettings" @((E "AppTheme" "Light"))
  $settings.Appearance.Theme = E "AppTheme" "Light"
  M $main "ApplyAppearanceSettings" @((E "AppTheme" "Dark"))
  $flp = F $main "flpDocumentList"; $tpl = F $main "pnlDocumentItem"; $empty = F $main "lblEmptyDocuments"
  $card = @($flp.Controls | Where-Object { $_ -ne $tpl -and $_ -ne $empty })[0]
  $card.BackColor = [Drawing.Color]::FromArgb(1, 2, 3)
  Check "테마: 테마 전환 후에도 카드 글자 배경이 카드 색을 따라감 (마우스 올림 표시)" ($card.Controls[0].BackColor.ToArgb() -eq $card.BackColor.ToArgb()) "카드=$($card.BackColor) 글자배경=$($card.Controls[0].BackColor)"

  $ed2Args = New-Object 'object[]' 1; $ed2Args[0] = $loaded
  $ed2 = [Activator]::CreateInstance($asm.GetType("WriteNote.ShortcutEditForm"), $ed2Args)
  $nameBox = F $ed2 "txtShortcutName"
  Check "테마: 밝은 테마에서 단축키 입력 창의 입력칸이 흰 바탕 유지" ($nameBox.BackColor.ToArgb() -eq [Drawing.SystemColors]::Window.ToArgb()) "배경=$($nameBox.BackColor)"
  $ed2.Dispose()

  $sf2 = [Activator]::CreateInstance($asm.GetType("WriteNote.SettingsForm"))
  M $sf2 "ShowSettingsPage" @("writing")
  $bw = F $sf2 "btnWriting"; $bg = F $sf2 "btnGeneral"
  Check "테마: 밝은 테마 설정 창에서 선택 메뉴는 흰 글자, 나머지는 어두운 글자" ($bw.ForeColor.ToArgb() -eq [Drawing.Color]::White.ToArgb() -and $bg.ForeColor.ToArgb() -ne [Drawing.Color]::White.ToArgb()) "선택=$($bw.ForeColor) 기타=$($bg.ForeColor)"
  Check "설정 창: Esc = 취소 버튼" ($sf2.CancelButton -eq (F $sf2 "btnCancelSettings")) ""
  $sf2.Dispose()
  $settings.Appearance.Theme = E "AppTheme" "Dark"
  M $main "ApplyAppearanceSettings" @((E "AppTheme" "Light"))

  # ════ 14. 마무리 단계 수정 확인 ════
  M $main "LoadDocument" @($docOk)
  $title.Text = "제목만 바꿈"
  Check "제목: 제목만 바꿔도 '수정됨'" ((F $main "isModified") -and $status.Text -eq "수정됨") $status.Text
  SetF $main "isModified" $false

  $rich2 = New-Object System.Windows.Forms.DataObject
  $rich2.SetData([System.Windows.Forms.DataFormats]::Rtf, "{\rtf1{\colortbl;\red0\green0\blue0;}\cf1\b BLACK}")
  $rich2.SetData([System.Windows.Forms.DataFormats]::UnicodeText, "BLACK")
  [System.Windows.Forms.Clipboard]::SetDataObject($rich2, $true, 10, 100); Start-Sleep -Milliseconds 100
  $rtb.Clear(); M $main "PasteWithThemeTextColor"; $rtb.SelectAll()
  Check "붙여넣기(서식 유지): 글자색은 테마 색으로, 굵게는 유지" ($rtb.Text -eq "BLACK" -and $rtb.SelectionColor.ToArgb() -eq $rtb.ForeColor.ToArgb() -and $rtb.SelectionFont.Bold) "색=$($rtb.SelectionColor) 굵게=$($rtb.SelectionFont.Bold)"
  SetF $main "isModified" $false

  New-Item -ItemType Directory $appData -Force | Out-Null
  Set-Content (Join-Path $appData "recent_documents.json") "깨진 내용 [" -Encoding UTF8
  $okRecent = $true; try { $rr = [WriteNote.RecentDocumentManager]::GetRecentDocuments() } catch { $okRecent = $false }
  Check "최근 문서: 목록 파일이 깨져도 오류 없이 빈 목록" ($okRecent -and $rr.Count -eq 0) ""

  TakeDialogs | Out-Null
  $act = [Action]{ throw (New-Object System.IO.IOException("다른 프로세스가 파일을 사용 중")) }
  $okRun = $true; try { M $main "RunFileAction" @("테스트 작업", $act) } catch { $okRun = $false }
  Start-Sleep -Milliseconds 400; $dlg = TakeDialogs
  Check "문서 메뉴: 파일 오류가 나도 프로그램이 멈추지 않고 안내" ($okRun -and ($dlg -join " ") -match "테스트 작업 중 오류") "메시지=$($dlg -join ' / ')"

  # ════ 15. 휴지통 ════
  $settings.Storage.DocumentFolder = $docsA
  $docT = Join-Path $docsA "trash_test.rtf"
  $tr = New-Object System.Windows.Forms.RichTextBox; $null = $tr.Handle; $tr.Text = "휴지통 테스트 본문"; $tr.SaveFile($docT); $tr.Dispose()
  '{ "Title": "휴지통 테스트", "IsFavorite": true, "FileName": "trash_test.rtf" }' | Set-Content ([IO.Path]::ChangeExtension($docT, ".json")) -Encoding UTF8
  M $main "LoadDocument" @($docT)
  SetF $main "currentDocumentCategory" "all"; M $main "LoadRecentDocuments"
  SetF $main "menuDocumentPath" $docT
  TakeDialogs | Out-Null
  M $main "DeleteDocument"; Start-Sleep -Milliseconds 400; $dlg = TakeDialogs
  $items = [WriteNote.TrashManager]::GetItems()
  Check "휴지통: 삭제하면 문서 폴더에서 빠지고 휴지통에 들어감" (-not (Test-Path $docT) -and -not (Test-Path ([IO.Path]::ChangeExtension($docT, ".json"))) -and $items.Count -eq 1 -and $items[0].OriginalPath -eq $docT -and $items[0].Title -eq "휴지통 테스트") "메시지=$($dlg -join ' / ')"
  Check "휴지통: 열려 있던 문서를 지우면 새 문서로" ((F $main "currentFilePath") -eq "") ""
  Check "휴지통: 최근 문서에서 제거" (-not ([WriteNote.RecentDocumentManager]::GetRecentDocuments() -contains $docT)) ""

  SetF $main "currentDocumentCategory" "trash"; M $main "LoadRecentDocuments"
  $flp = F $main "flpDocumentList"; $tpl = F $main "pnlDocumentItem"; $empty = F $main "lblEmptyDocuments"
  $cards = @($flp.Controls | Where-Object { $_ -ne $tpl -and $_ -ne $empty })
  $dateText = if ($cards.Count -gt 0) { $cards[0].Controls.Find("lblDocumentItemDate", $false)[0].Text } else { "" }
  Check "휴지통: 휴지통 목록에 카드 표시 (삭제 날짜)" ($cards.Count -eq 1 -and $dateText.StartsWith("삭제")) "카드=$($cards.Count) 날짜=$dateText"

  SetF $main "selectedDocumentPath" $items[0].TrashRtfPath
  TakeDialogs | Out-Null
  M $main "DeleteSelectedTrashItemPermanently"; Start-Sleep -Milliseconds 300; $dlg = TakeDialogs
  Check "휴지통: 영구 삭제 확인에서 '아니요'면 지우지 않음" ([WriteNote.TrashManager]::GetItems().Count -eq 1 -and ($dlg -join " ") -match "영구 삭제") "메시지=$($dlg -join ' / ')"

  SetF $main "selectedDocumentPath" $items[0].TrashRtfPath
  M $main "RestoreSelectedTrashItem"; Start-Sleep -Milliseconds 300; $dlg = TakeDialogs
  $infoBack = Get-Content ([IO.Path]::ChangeExtension($docT, ".json")) -Raw -Encoding UTF8 | ConvertFrom-Json
  Check "휴지통: 복원하면 원래 위치로, 제목·즐겨찾기 유지" ((Test-Path $docT) -and $infoBack.Title -eq "휴지통 테스트" -and $infoBack.IsFavorite -and [WriteNote.TrashManager]::GetItems().Count -eq 0) "메시지=$($dlg -join ' / ')"

  $item2 = [WriteNote.TrashManager]::MoveToTrash($docT, "휴지통 테스트")
  $tr = New-Object System.Windows.Forms.RichTextBox; $null = $tr.Handle; $tr.Text = "같은 이름 새 문서"; $tr.SaveFile($docT); $tr.Dispose()
  $restored = [WriteNote.TrashManager]::Restore($item2)
  $info2 = Get-Content ([IO.Path]::ChangeExtension($restored, ".json")) -Raw -Encoding UTF8 | ConvertFrom-Json
  Check "휴지통: 같은 이름이 있으면 '(2)'로 복원하고 덮어쓰지 않음" ($restored -eq (Join-Path $docsA "trash_test (2).rtf") -and $info2.FileName -eq "trash_test (2).rtf" -and (Test-Path $docT)) $restored

  [WriteNote.TrashManager]::MoveToTrash($docT, "a") | Out-Null
  [WriteNote.TrashManager]::MoveToTrash($restored, "b") | Out-Null
  $n = [WriteNote.TrashManager]::EmptyTrash()
  Check "휴지통: 비우기" ($n -eq 2 -and [WriteNote.TrashManager]::GetItems().Count -eq 0 -and @(Get-ChildItem ([WriteNote.TrashManager]::TrashFolderPath)).Count -eq 0) "지운 수=$n"
  SetF $main "currentDocumentCategory" "all"; SetF $main "isModified" $false

  # ════ 16. 폴더별 분류 ════
  $docsF = Join-Path $work "docsF"; Remove-Item $docsF -Recurse -Force -ErrorAction SilentlyContinue
  New-Item -ItemType Directory (Join-Path $docsF "소설"), (Join-Path $docsF "에세이"), (Join-Path $docsF ".숨김") -Force | Out-Null
  $settings.Storage.DocumentFolder = $docsF
  function MakeDoc($path, $title) {
    $r = New-Object System.Windows.Forms.RichTextBox; $null = $r.Handle; $r.Text = "$title 본문"; $r.SaveFile($path); $r.Dispose()
    "{ ""Title"": ""$title"", ""FileName"": ""$([IO.Path]::GetFileName($path))"" }" | Set-Content ([IO.Path]::ChangeExtension($path, ".json")) -Encoding UTF8
  }
  $rootDoc = Join-Path $docsF "root.rtf"; MakeDoc $rootDoc "루트 문서"
  $novel = Join-Path $docsF "소설\chapter1.rtf"; MakeDoc $novel "1장"
  $essay = Join-Path $docsF "에세이\essay.rtf"; MakeDoc $essay "에세이 한 편"

  SetF $main "currentDocumentCategory" "all"; SetF $main "currentFolderFilter" "*"; M $main "LoadRecentDocuments"
  $cbo = F $main "cboFolder"
  $names = @($cbo.Items | ForEach-Object { $_.ToString().Trim() })
  Check "폴더: 폴더 목록 = 모든 폴더, 분류 없음, 하위 폴더 (숨김 제외)" (($names -join ",") -eq "모든 폴더,분류 없음,소설,에세이") ($names -join ", ")

  function CardTexts { $flp = F $main "flpDocumentList"; $tpl = F $main "pnlDocumentItem"; $empty = F $main "lblEmptyDocuments"; @($flp.Controls | Where-Object { $_ -ne $tpl -and $_ -ne $empty } | ForEach-Object { $_.Controls.Find("lblDocumentItemTitle", $false)[0].Text + "|" + $_.Controls.Find("lblDocumentItemDate", $false)[0].Text }) }
  $all = @(CardTexts)
  Check "폴더: '모든 폴더'에서 하위 폴더 문서까지 표시, 폴더 이름 함께 표시" ($all.Count -eq 3 -and ($all -join " ") -match "1장\|.*·  소설" -and ($all -join " ") -match "에세이 한 편\|.*·  에세이") ($all -join " / ")

  SetF $main "currentFolderFilter" ""; M $main "LoadRecentDocuments"; $r = @(CardTexts)
  Check "폴더: '분류 없음'은 문서 폴더 바로 아래 문서만" ($r.Count -eq 1 -and $r[0].StartsWith("루트 문서")) ($r -join " / ")

  SetF $main "currentFolderFilter" "소설"; M $main "LoadRecentDocuments"; $r = @(CardTexts)
  $saveFolder = $main.GetType().GetProperty("CurrentSaveFolder", $NP).GetValue($main)
  Check "폴더: 폴더를 고르면 그 폴더 문서만, 새 문서 저장 위치도 그 폴더" ($r.Count -eq 1 -and $r[0].StartsWith("1장") -and $saveFolder -eq (Join-Path $docsF "소설")) "$($r -join ' / ') 저장위치=$saveFolder"

  SetF $main "currentDocumentCategory" "recent"; M $main "LoadRecentDocuments"
  Check "폴더: 최근 문서에서는 폴더 선택 상자 꺼짐" (-not $cbo.Enabled) ""
  SetF $main "currentDocumentCategory" "all"; SetF $main "currentFolderFilter" "*"; M $main "LoadRecentDocuments"

  # 폴더 이름 검사
  $v1 = M $main "ValidateFolderName" @("소설", $null); $v2 = M $main "ValidateFolderName" @("a/b", $null); $v3 = M $main "ValidateFolderName" @(".x", $null); $v4 = M $main "ValidateFolderName" @("시", $null); $v5 = M $main "ValidateFolderName" @("소설", "소설")
  Check "폴더: 이름 검사 (중복·금지 문자·'.' 시작 거부, 새 이름·자기 자신 허용)" ($v1 -ne $null -and $v2 -ne $null -and $v3 -ne $null -and $v4 -eq $null -and $v5 -eq $null) "중복=$v1 / 금지=$v2 / 점=$v3"

  # 폴더로 이동 메뉴
  M $main "LoadDocument" @($rootDoc)
  M $main "LoadRecentDocuments"
  SetF $main "menuDocumentPath" $rootDoc
  M $main "BuildMoveToFolderMenu"
  $menu = F $main "tsmiMoveToFolder"
  $menuTexts = @($menu.DropDownItems | Where-Object { $_ -is [System.Windows.Forms.ToolStripMenuItem] } | ForEach-Object { $_.Text + $(if ($_.Enabled) { "" } else { "(x)" }) })
  Check "폴더로 이동: 메뉴에 폴더 목록, 지금 폴더는 비활성" (($menuTexts -join ",") -eq "분류 없음  (현재)(x),소설,에세이,새 폴더 만들어 이동...") ($menuTexts -join ", ")

  M $main "MoveDocumentToFolder" @($rootDoc, (Join-Path $docsF "에세이"))
  $movedPath = Join-Path $docsF "에세이\root.rtf"
  Check "폴더로 이동: 문서와 정보 파일 이동, 열린 문서 경로·최근 문서 갱신" ((Test-Path $movedPath) -and (Test-Path ([IO.Path]::ChangeExtension($movedPath, ".json"))) -and -not (Test-Path $rootDoc) -and (F $main "currentFilePath") -eq $movedPath -and ([WriteNote.RecentDocumentManager]::GetRecentDocuments() -contains $movedPath)) (F $main "currentFilePath")

  # 폴더 이름 변경에 따른 경로 갱신 (입력 창 뒤에서 하는 일)
  $tItem = [WriteNote.TrashManager]::MoveToTrash($novel, "1장")
  $oldSub = Join-Path $docsF "에세이"; $newSub = Join-Path $docsF "수필"
  [IO.Directory]::Move($oldSub, $newSub)
  [WriteNote.RecentDocumentManager]::ReplaceFolderPath($oldSub, $newSub)
  $repl = $asm.GetType("WriteNote.PathHelper").GetMethod("ReplaceFolderInPath").Invoke($null, @([string]$movedPath, [string]$oldSub, [string]$newSub))
  Check "폴더 이름 변경: 최근 문서·열린 문서 경로가 새 폴더로" (([WriteNote.RecentDocumentManager]::GetRecentDocuments() -contains (Join-Path $newSub "root.rtf")) -and $repl -eq (Join-Path $newSub "root.rtf")) $repl
  [WriteNote.TrashManager]::ReplaceOriginalFolder((Join-Path $docsF "소설"), (Join-Path $docsF "장편"))
  $tAfter = [WriteNote.TrashManager]::GetItems() | Where-Object { $_.Id -eq $tItem.Id }
  Check "폴더 이름 변경: 휴지통의 복원 위치도 새 폴더로" ($tAfter.OriginalPath -eq (Join-Path $docsF "장편\chapter1.rtf")) $tAfter.OriginalPath
  [WriteNote.TrashManager]::EmptyTrash() | Out-Null
  SetF $main "currentFilePath" ""

  # 폴더 삭제 확인에서 '아니요'
  SetF $main "currentFolderFilter" "수필"; M $main "LoadRecentDocuments"; TakeDialogs | Out-Null
  M $main "DeleteSelectedFolder"; Start-Sleep -Milliseconds 300; $dlg = TakeDialogs
  Check "폴더 삭제: 확인에서 '아니요'면 폴더와 문서 그대로" ((Test-Path (Join-Path $newSub "root.rtf")) -and ($dlg -join " ") -match "휴지통으로 옮깁니다") "메시지=$($dlg -join ' / ')"

  # 문서 폴더를 바꿀 때 하위 폴더까지 옮기기
  $docsG = Join-Path $work "docsG"; Remove-Item $docsG -Recurse -Force -ErrorAction SilentlyContinue
  $skippedG = $null
  $argsG = New-Object 'object[]' 3; $argsG[0] = [string]$docsF; $argsG[1] = [string]$docsG
  $movedG = $main.GetType().GetMethod("MoveDocumentTree", $NP).Invoke($main, $argsG)
  Check "문서 폴더 변경: 하위 폴더 문서도 같은 폴더 구조로 이동, 빈 이전 폴더 정리" ($movedG -eq 2 -and (Test-Path (Join-Path $docsG "수필\root.rtf")) -and (Test-Path (Join-Path $docsG "수필\essay.rtf")) -and -not (Test-Path $newSub)) "이동=$movedG"
  SetF $main "currentFolderFilter" "*"; SetF $main "isModified" $false

  # ════ 17. 맞춤법 검사 ════
  $svc = $asm.GetType("WriteNote.SpellCheckService")
  $settings.Writing.SpellCheckEnabled = $true
  M $main "ApplySpellCheckSetting"
  $sw = [Diagnostics.Stopwatch]::StartNew()
  while (-not $svc.GetProperty("IsReady").GetValue($null) -and $sw.ElapsedMilliseconds -lt 20000) { Pump; Start-Sleep -Milliseconds 50 }
  Check "맞춤법: 사전을 뒤에서 불러옴" ($svc.GetProperty("IsReady").GetValue($null)) "$($sw.ElapsedMilliseconds)ms 오류=$($svc.GetProperty('LoadError').GetValue($null))"

  $docS = Join-Path $docsA "spell.rtf"
  $tr = New-Object System.Windows.Forms.RichTextBox; $null = $tr.Handle
  $tr.Text = "일이 잘 됬다는 소식을 몇일 뒤에 들었다. 오랫만에 민수가 왔다. English 3월"; $tr.SaveFile($docS); $tr.Dispose()
  M $main "LoadDocument" @($docS)
  M $main "RunSpellCheck" @($false)
  function ErrWords { @((F $main "spellingErrors") | ForEach-Object { $_.Word }) }
  $w = ErrWords
  Check "맞춤법: 틀린 단어만 찾음 (영어·숫자 섞인 말 제외)" (($w -join ",") -eq "됬다는,몇일,오랫만에,민수가") ($w -join ", ")
  Check "맞춤법: 검사만으로는 문서가 '수정됨'이 되지 않음" (-not (F $main "isModified")) $status.Text

  $errs = F $main "spellingErrors"; $before = $errs[1].Start
  $rtb.Select(0, 0); $rtb.SelectedText = "어제 "
  $after = (F $main "spellingErrors")[1].Start
  Check "맞춤법: 앞에 글자를 넣으면 밑줄 위치도 그만큼 이동" ($after -eq $before + 3) "이전=$before 이후=$after"
  M $main "RunSpellCheck" @($false)

  $err = (F $main "spellingErrors") | Where-Object { $_.Word -eq "몇일" } | Select-Object -First 1
  M $main "BuildSpellingMenu" @($err)
  $items = @((F $main "cmsSpelling").Items | ForEach-Object { $_.Text })
  Check "맞춤법 메뉴: 추천 단어, 사전에 추가, 무시" ($items[0] -eq "며칠" -and ($items -join "|") -match "사전에 추가" -and ($items -join "|") -match "무시") ($items -join " | ")

  M $main "ReplaceSpellingError" @($err, "며칠")
  Check "맞춤법: 추천 단어로 바꾸기" ($rtb.Text.Contains("며칠 뒤에") -and -not $rtb.Text.Contains("몇일") -and -not ((ErrWords) -contains "몇일")) $rtb.Text

  $svc.GetMethod("AddToUserDictionary").Invoke($null, @("민수"))
  M $main "RunSpellCheck" @($false)
  $userDict = Join-Path $appData "user_dictionary.txt"
  Check "맞춤법: 사전에 추가하면 '민수가'처럼 조사가 붙어도 통과, 파일에 저장" (-not ((ErrWords) -contains "민수가") -and (Test-Path $userDict) -and ((Get-Content $userDict -Encoding UTF8) -contains "민수")) ((ErrWords) -join ", ")

  $svc.GetMethod("Ignore").Invoke($null, @("오랫만에"))
  M $main "RunSpellCheck" @($false)
  Check "맞춤법: 무시한 단어는 표시하지 않음" (-not ((ErrWords) -contains "오랫만에")) ((ErrWords) -join ", ")

  SetF $main "currentFilePath" $docS; M $main "SaveDocument" | Out-Null
  $raw = Get-Content $docS -Raw
  Check "맞춤법: 밑줄은 화면에만, 저장한 문서에는 밑줄 서식 없음" (-not ($raw -match "\\ul")) ""

  $settings.Writing.SpellCheckEnabled = $false
  M $main "ApplySpellCheckSetting"
  Check "맞춤법: 설정에서 끄면 밑줄 모두 지움" ((F $main "spellingErrors").Count -eq 0) ""
  $settings.Writing.SpellCheckEnabled = $true
  SetF $main "isModified" $false; SetF $main "currentFilePath" ""

  # ════ 18. 사용자 사전 관리 창 ════
  $settings.Writing.SpellCheckEnabled = $true
  M $main "ApplySpellCheckSetting"
  $rtb.Text = "민수가 왔다. 윤서가 웃었다."
  M $main "RunSpellCheck" @($false)
  Check "사용자 사전: 시작 상태 (민수는 사전에 있음, 윤서는 없음)" (((ErrWords) -join ",") -eq "윤서가") ((ErrWords) -join ", ")

  $udType = $asm.GetType("WriteNote.UserDictionaryForm")
  $ud = [Activator]::CreateInstance($udType); Hide $ud; $ud.Show(); Pump
  Check "사용자 사전 창: 저장된 단어 표시" ((@($ud.Words) -join ",") -eq "민수") (@($ud.Words) -join ", ")

  (F $ud "txtNewWord").Text = "윤서"; $ud.AddWord()
  (F $ud "txtNewWord").Text = "윤서"; $ud.AddWord()
  TakeDialogs | Out-Null
  (F $ud "txtNewWord").Text = "두 단어"; $ud.AddWord(); Start-Sleep -Milliseconds 400; $dlg = TakeDialogs
  Check "사용자 사전 창: 추가, 중복은 한 번만, 띄어쓰기 있는 말은 거부" ((@($ud.Words) -join ",") -eq "민수,윤서" -and ($dlg -join " ") -match "띄어쓰기 없이") "단어=$(@($ud.Words) -join ', ') 메시지=$($dlg -join ' / ')"

  $lst = F $ud "lstWords"; $lst.ClearSelected(); $lst.SelectedIndex = $lst.Items.IndexOf("민수")
  $ud.DeleteSelectedWords()
  Check "사용자 사전 창: 선택한 단어 삭제 (아직 파일에는 반영 안 됨)" ((@($ud.Words) -join ",") -eq "윤서" -and ((Get-Content (Join-Path $appData "user_dictionary.txt") -Encoding UTF8) -contains "민수")) ""

  (F $ud "btnOK").PerformClick(); Pump
  $saved = @(Get-Content (Join-Path $appData "user_dictionary.txt") -Encoding UTF8)
  Check "사용자 사전 창: 확인하면 파일 저장" (($saved -join ",") -eq "윤서" -and $ud.IsDisposed -or -not $ud.Visible) ($saved -join ", ")
  Check "사용자 사전 창: 확인하면 본문 물결선 바로 반영 (민수가 표시, 윤서가 통과)" (((ErrWords) -join ",") -eq "민수가") ((ErrWords) -join ", ")

  $ud2 = [Activator]::CreateInstance($udType); Hide $ud2; $ud2.Show(); Pump
  (F $ud2 "txtNewWord").Text = "지호"; $ud2.AddWord()
  TakeDialogs | Out-Null
  $ud2.Close(); Pump; Start-Sleep -Milliseconds 400; $dlg = TakeDialogs
  Check "사용자 사전 창: 저장 안 하고 닫으면 확인, '아니요'면 창 유지" ($ud2.Visible -and ($dlg -join " ") -match "저장하지 않고") "메시지=$($dlg -join ' / ')"
  SetF $ud2 "hasChanges" $false; $ud2.Close(); $ud2.Dispose()
  $saved = @(Get-Content (Join-Path $appData "user_dictionary.txt") -Encoding UTF8)
  Check "사용자 사전 창: 취소한 내용은 저장되지 않음" (($saved -join ",") -eq "윤서") ($saved -join ", ")
  $rtb.Clear(); SetF $main "isModified" $false

  # ════ 19. 설명서 ════
  $hfType = $asm.GetType("WriteNote.HelpForm")
  $hf = [Activator]::CreateInstance($hfType); Hide $hf; $hf.Show(); Pump
  $titles = @($hf.SectionTitles)
  Check "설명서: 목차 13장 (라이선스 전문 포함)" ($titles.Count -eq 13 -and $titles[0] -eq "1. WriteNote 소개" -and $titles[12] -eq "13. 라이선스 전문" -and $hf.ContentText.Contains("GNU GENERAL PUBLIC LICENSE") -and $hf.ContentText.Contains("Newtonsoft")) ($titles -join " | ")
  $text = $hf.ContentText
  $mustHave = @("Ctrl + Enter", "휴지통 비우기", "사용자 사전", "폴더로 이동", "자동 저장", "밝은 테마", "%AppData%\WriteNote\user_dictionary.txt", "F1")
  $missing = @($mustHave | Where-Object { -not $text.Contains($_) })
  Check "설명서: 주요 기능 설명 포함" ($missing.Count -eq 0) ("빠진 말: " + ($missing -join ", "))
  $rtbH = F $hf "rtbHelp"
  $hf.GoToSection(6)
  Check "설명서: 목차로 장 이동 (7. 맞춤법 검사)" ($rtbH.Text.Substring($rtbH.SelectionStart, 9) -eq "7. 맞춤법 검사") $rtbH.Text.Substring($rtbH.SelectionStart, 9)
  (F $hf "txtHelpFind").Text = "영구 삭제"; $rtbH.Select(0, 0)
  $foundOk = $hf.FindNext()
  Check "설명서: 찾기" ($foundOk -and $rtbH.SelectedText -eq "영구 삭제") $rtbH.SelectedText
  TakeDialogs | Out-Null
  (F $hf "txtHelpFind").Text = "없는말없는말"; $hf.FindNext() | Out-Null; Start-Sleep -Milliseconds 400; $dlg = TakeDialogs
  Check "설명서: 없는 말은 안내" (($dlg -join " ") -match "찾지 못했습니다") ($dlg -join " / ")
  $hf.Close(); $hf.Dispose()
  $f1 = [WriteNote.ShortcutManager]::FindConflict($null, [System.Windows.Forms.Keys]::F1, $null)
  Check "설명서: F1은 설명서용 기본 단축키라 글쓰기 단축키로 쓸 수 없음" ($f1 -ne $null -and $f1 -match "설명서") $f1
  $sfH = [Activator]::CreateInstance($asm.GetType("WriteNote.SettingsForm"))
  $bh = F $sfH "btnHelp"
  Check "설명서: 설정 메뉴 '화면 및 테마' 아래에 '설명서' 버튼" ($bh.Text -eq "설명서" -and $bh.Top -gt (F $sfH "btnAppearance").Top) "위치=$($bh.Top)"
  $sfH.Dispose()

  # ════ 20. 문서 카드 우클릭 메뉴 (우클릭한 카드의 경로로 동작) ════
  $docsM = Join-Path $work "docsM"; Remove-Item $docsM -Recurse -Force -ErrorAction SilentlyContinue
  New-Item -ItemType Directory $docsM | Out-Null
  $settings.Storage.DocumentFolder = $docsM
  $mA = Join-Path $docsM "menu_a.rtf"; $mB = Join-Path $docsM "menu_b.rtf"
  foreach ($pair in @(@($mA, "메뉴 A"), @($mB, "메뉴 B"))) {
    $r = New-Object System.Windows.Forms.RichTextBox; $null = $r.Handle; $r.Text = $pair[1] + " 본문"; $r.SaveFile($pair[0]); $r.Dispose()
    "{ ""Title"": ""$($pair[1])"", ""IsFavorite"": false, ""FileName"": ""$([IO.Path]::GetFileName($pair[0]))"" }" | Set-Content ([IO.Path]::ChangeExtension($pair[0], ".json")) -Encoding UTF8
  }
  M $main "CreateNewDocument"; SetF $main "currentDocumentCategory" "all"; SetF $main "currentFolderFilter" "*"
  M $main "LoadRecentDocuments"

  # 카드를 실제로 우클릭한 것처럼: 카드의 제목 라벨에서 MouseDown(오른쪽)
  $flp = F $main "flpDocumentList"; $tpl = F $main "pnlDocumentItem"; $empty = F $main "lblEmptyDocuments"
  $cardB = @($flp.Controls | Where-Object { $_ -ne $tpl -and $_ -ne $empty -and $_.Tag -eq $mB })[0]
  $label = $cardB.Controls.Find("lblDocumentItemTitle", $false)[0]
  $cms = F $main "cmsRecentDocuments"
  $mouse = New-Object System.Windows.Forms.MouseEventArgs([System.Windows.Forms.MouseButtons]::Right, 1, 5, 5, 0)
  M $main "DocumentCard_MouseDown" @($label, $mouse); Pump
  $menuPath = F $main "menuDocumentPath"
  $cms.Close(); Pump
  Check "우클릭 메뉴: 우클릭한 카드의 문서가 메뉴 대상이 됨" ($menuPath -eq $mB -and (F $main "selectedDocumentPath") -eq $mB) $menuPath

  M $main "tsmiOpen_Click" @($null, [EventArgs]::Empty)
  Check "우클릭 메뉴: 문서 열기" ((F $main "currentFilePath") -eq $mB -and $title.Text -eq "메뉴 B") (F $main "currentFilePath")

  TakeDialogs | Out-Null
  M $main "ToggleFavorite"; Start-Sleep -Milliseconds 300; TakeDialogs | Out-Null
  $fav = (Get-Content ([IO.Path]::ChangeExtension($mB, ".json")) -Raw -Encoding UTF8 | ConvertFrom-Json).IsFavorite
  M $main "UpdateFavoriteMenuText"
  Check "우클릭 메뉴: 즐겨찾기 추가, 메뉴 문구는 '즐겨찾기 해제'" ($fav -and (F $main "tsmiFavorite").Text -eq "즐겨찾기 해제") "IsFavorite=$fav 문구=$((F $main 'tsmiFavorite').Text)"
  SetF $main "menuDocumentPath" $mA; M $main "UpdateFavoriteMenuText"
  Check "우클릭 메뉴: 다른 문서로 바꾸면 문구도 그 문서 기준 ('즐겨찾기에 추가')" ((F $main "tsmiFavorite").Text -eq "즐겨찾기에 추가") (F $main "tsmiFavorite").Text

  SetF $main "menuDocumentPath" $mB
  $recentBefore = [WriteNote.RecentDocumentManager]::GetRecentDocuments() -contains $mB
  M $main "RemoveFromRecentDocuments"; Start-Sleep -Milliseconds 300; TakeDialogs | Out-Null
  Check "우클릭 메뉴: 최근 목록에서 제거 (파일은 그대로)" ($recentBefore -and -not ([WriteNote.RecentDocumentManager]::GetRecentDocuments() -contains $mB) -and (Test-Path $mB)) ""

  SetF $main "menuDocumentPath" $mA
  M $main "ShowDocumentProperties"; Start-Sleep -Milliseconds 300; $dlg = TakeDialogs
  Check "우클릭 메뉴: 속성 보기 (대상 문서의 제목, 만든 날짜가 없으면 파일 날짜)" (($dlg -join " ") -match "메뉴 A" -and -not (($dlg -join " ") -match "0001-01-01")) ($dlg -join " / ")

  SetF $main "menuDocumentPath" $mA
  M $main "RenameDocument"; Start-Sleep -Milliseconds 300; $dlg = TakeDialogs
  Check "우클릭 메뉴: 이름 변경 창 열림, 취소하면 그대로" (($dlg -join " ") -match "문서 이름 변경" -and (Test-Path $mA)) ($dlg -join " / ")

  $ghost = Join-Path $docsM "없는문서.rtf"
  SetF $main "menuDocumentPath" $ghost; $openBefore = F $main "currentFilePath"
  M $main "tsmiOpen_Click" @($null, [EventArgs]::Empty); Start-Sleep -Milliseconds 300; $dlg = TakeDialogs
  Check "우클릭 메뉴: 대상 파일이 사라졌으면 안내하고 아무것도 안 함" (($dlg -join " ") -match "파일을 찾을 수 없습니다" -and (F $main "currentFilePath") -eq $openBefore) ($dlg -join " / ")

  SetF $main "menuDocumentPath" $mA
  M $main "DeleteDocument"; Start-Sleep -Milliseconds 300; $dlg = TakeDialogs
  $tItems = [WriteNote.TrashManager]::GetItems()
  Check "우클릭 메뉴: 휴지통으로 이동 (제목은 문서 정보에서)" (-not (Test-Path $mA) -and ($tItems | Where-Object { $_.OriginalPath -eq $mA -and $_.Title -eq "메뉴 A" })) ($dlg -join " / ")
  [WriteNote.TrashManager]::EmptyTrash() | Out-Null
  SetF $main "isModified" $false; SetF $main "menuDocumentPath" ""

  # 닫기 확인
  $rtb.AppendText(" 닫기 전 수정"); TakeDialogs | Out-Null
  $main.Close(); Pump
  $dlg = TakeDialogs
  Check "닫기: 저장하지 않은 내용이 있으면 확인하고, '취소'면 닫지 않음" ($main.Visible -and -not $main.IsDisposed -and ($dlg -join " ") -match "저장") "메시지=$($dlg -join ' / ')"

  SetF $main "isModified" $false
  $main.Close()
}
catch {
  Check "테스트 실행 중 오류" $false ($_.Exception.ToString() -split "`n" | Select-Object -First 4 | Out-String)
}
finally {
  [DialogWatchdog]::Stop()
  $left = TakeDialogs; if ($left.Count -gt 0) { Add-Content $log ("자동으로 닫은 메시지 창: " + ($left -join " / ")) -Encoding UTF8 }
  # ── 사용자 데이터 복원 ──
  if (Test-Path $appData) { Remove-Item $appData -Recurse -Force }
  # (테스트 전용 폴더만 지움)
  try { if ($savedClipboard -ne $null) { [System.Windows.Forms.Clipboard]::SetText($savedClipboard) } else { [System.Windows.Forms.Clipboard]::Clear() } } catch {}
}

$results | Format-Table -AutoSize -Wrap | Out-String -Width 200
"통과 {0} / 실패 {1}" -f @($results | Where-Object Result -eq "통과").Count, @($results | Where-Object Result -eq "실패").Count
"테스트 데이터 폴더: " + $appData + " (실제 %AppData%\WriteNote는 사용하지 않음)"
