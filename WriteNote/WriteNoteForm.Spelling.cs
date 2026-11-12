using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WriteNote
{
    // 맞춤법 검사: 본문에서 틀린 것으로 의심되는 단어 아래에 빨간 물결선을 그린다.
    // - 밑줄은 화면에만 그리고 문서(RTF)에는 저장하지 않는다.
    // - 글을 쓰다 잠깐 멈추면, 지금 화면에 보이는 부분만 검사한다.
    // - 틀린 단어를 우클릭하면 추천 단어 / 사전에 추가 / 무시 메뉴가 나온다.
    public partial class MainForm
    {
        // 지금 화면에 그릴 의심 단어 목록 (본문 글자 위치 기준)
        private List<SpellingError> spellingErrors = new List<SpellingError>();

        // 입력을 멈춘 뒤 검사할 때까지 기다리는 타이머
        private Timer spellCheckTimer;

        // 더 오래 멈췄을 때 입력하던 단어까지 검사하는 타이머
        private Timer spellIdleTimer;

        // 본문 위에 물결선을 덧그리기 위한 창 연결
        private SpellingOverlay spellingOverlay;

        private ContextMenuStrip cmsSpelling;
        private Font spellSuggestionFont;

        // 직전 본문 길이 (입력으로 글자 위치가 밀린 만큼 밑줄 위치를 옮기는 데 사용)
        private int lastSpellTextLength = 0;

        // 사전을 불러오지 못했다는 안내를 이미 했는지
        private bool spellLoadErrorShown = false;

        private bool IsSpellCheckEnabled
        {
            get { return appSettings.Writing.SpellCheckEnabled; }
        }

        // 생성자에서 호출
        private void InitializeSpellCheck()
        {
            spellCheckTimer = new Timer();
            spellCheckTimer.Interval = 450;
            spellCheckTimer.Tick += (s, e) =>
            {
                spellCheckTimer.Stop();
                RunSpellCheck(true);
            };

            // 입력을 1.5초 동안 멈추면, 입력하던 단어(커서 바로 앞 단어)까지 검사한다.
            spellIdleTimer = new Timer();
            spellIdleTimer.Interval = 1500;
            spellIdleTimer.Tick += (s, e) =>
            {
                spellIdleTimer.Stop();
                RunSpellCheck(false);
            };

            spellingOverlay = new SpellingOverlay(this);

            if (rtbContent.IsHandleCreated)
                spellingOverlay.AssignHandle(rtbContent.Handle);

            rtbContent.HandleCreated += (s, e) => spellingOverlay.AssignHandle(rtbContent.Handle);
            rtbContent.HandleDestroyed += (s, e) => spellingOverlay.ReleaseHandle();

            rtbContent.TextChanged += rtbContent_SpellTextChanged;
            rtbContent.VScroll += (s, e) => RestartSpellCheckTimer();
            rtbContent.Resize += (s, e) => RestartSpellCheckTimer();
            rtbContent.SelectionChanged += (s, e) =>
            {
                // 입력하던 단어에서 커서가 벗어나면 그 단어도 검사한다.
                if (spellingErrors != null && IsSpellCheckEnabled)
                    RestartSpellCheckTimer();
            };
            rtbContent.MouseUp += rtbContent_SpellMouseUp;

            // 사용자 사전이 바뀌면(우클릭 '사전에 추가', 설정의 사용자 사전 창) 물결선을 바로 다시 그린다.
            EventHandler onDictionaryChanged = (s, e) =>
            {
                if (!IsDisposed && IsHandleCreated)
                    RunSpellCheck(false);
            };

            SpellCheckService.UserDictionaryChanged += onDictionaryChanged;
            Disposed += (s, e) => SpellCheckService.UserDictionaryChanged -= onDictionaryChanged;

            cmsSpelling = CreateThemedMenu();
            spellSuggestionFont = new Font(cmsSpelling.Font, FontStyle.Bold);
            Disposed += (s, e) =>
            {
                cmsSpelling.Dispose();
                spellSuggestionFont.Dispose();
                spellCheckTimer.Dispose();
                spellIdleTimer.Dispose();
            };
        }

        // 설정에 따라 맞춤법 검사를 켜거나 끈다. (시작할 때, 설정을 저장한 뒤)
        private void ApplySpellCheckSetting()
        {
            if (spellCheckTimer == null)
                return;

            if (!IsSpellCheckEnabled)
            {
                spellCheckTimer.Stop();
                spellIdleTimer.Stop();
                ClearSpellingErrors();
                return;
            }

            if (SpellCheckService.IsReady)
            {
                RunSpellCheck();
                return;
            }

            // 사전을 뒤에서 불러온 뒤 검사한다. (화면이 멈추지 않도록)
            SpellCheckService.LoadAsync().ContinueWith(task =>
            {
                if (IsDisposed || !IsHandleCreated)
                    return;

                BeginInvoke((Action)(() =>
                {
                    if (SpellCheckService.LoadError != null)
                    {
                        if (!spellLoadErrorShown)
                        {
                            spellLoadErrorShown = true;
                            MessageBox.Show(
                                SpellCheckService.LoadError + "\n\n맞춤법 검사 없이 계속 사용할 수 있습니다.",
                                "맞춤법 검사",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                            );
                        }

                        return;
                    }

                    RunSpellCheck();
                }));
            });
        }

        private void RestartSpellCheckTimer()
        {
            if (spellCheckTimer == null || !IsSpellCheckEnabled)
                return;

            spellCheckTimer.Stop();
            spellCheckTimer.Start();

            spellIdleTimer.Stop();
            spellIdleTimer.Start();
        }

        private void ClearSpellingErrors()
        {
            if (spellingErrors.Count == 0)
                return;

            spellingErrors = new List<SpellingError>();
            rtbContent.Invalidate();
        }

        // 본문이 바뀌면 바뀐 위치 뒤의 밑줄을 그만큼 옮기고, 바뀐 곳의 밑줄은 지운다.
        // (정확한 위치는 잠시 뒤 다시 검사할 때 맞춰진다)
        private void rtbContent_SpellTextChanged(object sender, EventArgs e)
        {
            int newLength = rtbContent.TextLength;
            int delta = newLength - lastSpellTextLength;
            lastSpellTextLength = newLength;

            if (spellingErrors.Count > 0)
            {
                // 문서를 새로 열었거나 크게 바뀌었으면 모두 지우고 다시 검사
                if (Math.Abs(delta) > 50)
                {
                    spellingErrors = new List<SpellingError>();
                }
                else
                {
                    int caret = rtbContent.SelectionStart;
                    int editStart = delta >= 0 ? caret - delta : caret;
                    int editEndOld = delta >= 0 ? editStart : editStart - delta;

                    List<SpellingError> moved = new List<SpellingError>();

                    foreach (SpellingError error in spellingErrors)
                    {
                        if (error.End < editStart)
                        {
                            moved.Add(error);
                        }
                        else if (error.Start > editEndOld)
                        {
                            moved.Add(new SpellingError
                            {
                                Start = error.Start + delta,
                                Length = error.Length,
                                Word = error.Word
                            });
                        }
                    }

                    spellingErrors = moved;
                }

                rtbContent.Invalidate();
            }

            RestartSpellCheckTimer();
        }

        // 지금 화면에 보이는 부분을 검사한다.
        // skipCaretWord: true면 지금 입력 중인 단어(커서가 단어 끝에 붙어 있음)는 아직 표시하지 않는다.
        private void RunSpellCheck(bool skipCaretWord = false)
        {
            if (!IsSpellCheckEnabled || !SpellCheckService.IsReady || !rtbContent.IsHandleCreated)
                return;

            // 한글을 조합하는 중이면 지금 검사하지 않고 조금 뒤에 다시 시도한다.
            // (조합 중에 본문을 건드리면 'ㄷㅙㅆ다'처럼 자모가 따로 입력될 수 있다)
            if (IsImeComposing())
            {
                RestartSpellCheckTimer();
                return;
            }

            // rtbContent.Text는 한글 조합을 끊으므로 쓰지 않는다.
            string text = GetEditorText();
            lastSpellTextLength = text.Length;

            if (text.Length == 0)
            {
                ClearSpellingErrors();
                return;
            }

            int first = rtbContent.GetCharIndexFromPosition(new Point(0, 0));
            int last = rtbContent.GetCharIndexFromPosition(
                new Point(rtbContent.ClientSize.Width - 1, rtbContent.ClientSize.Height - 1));

            // 화면 위아래로 조금 더 검사해 두면 스크롤할 때 밑줄이 바로 보인다.
            int start = Math.Max(0, first - 200);
            int end = Math.Min(text.Length, last + 400);

            // 지금 입력 중인 단어(커서가 단어 끝에 붙어 있음)는 아직 표시하지 않는다.
            // (1.5초 더 멈추면 skipCaretWord = false로 다시 검사해 그 단어도 표시)
            int skip = -1;

            if (skipCaretWord && rtbContent.Focused && rtbContent.SelectionLength == 0)
            {
                int caret = rtbContent.SelectionStart;

                if (caret > 0 && caret <= text.Length && char.IsLetterOrDigit(text[caret - 1]) &&
                    (caret == text.Length || !char.IsLetterOrDigit(text[caret])))
                {
                    skip = caret;
                }
            }

            List<SpellingError> found = SpellCheckService.FindErrors(text, start, end - start, skip);

            bool changed = found.Count != spellingErrors.Count ||
                           found.Where((error, i) => error.Start != spellingErrors[i].Start ||
                                                     error.Length != spellingErrors[i].Length).Any();

            spellingErrors = found;

            if (changed)
                rtbContent.Invalidate();
        }

        // 본문을 그린 직후 호출된다. 의심 단어 아래에 빨간 물결선을 그린다.
        private void DrawSpellingErrors()
        {
            if (spellingErrors.Count == 0 || !IsSpellCheckEnabled)
                return;

            int textLength = rtbContent.TextLength;
            int lineHeight = rtbContent.Font.Height;
            Size client = rtbContent.ClientSize;

            // 어두운 테마: 밝은 빨강, 밝은 테마: 진한 빨강 (어느 쪽에서도 잘 보이도록)
            Color waveColor = ThemeManager.Current == AppTheme.Light
                ? Color.FromArgb(215, 30, 30)
                : Color.FromArgb(255, 95, 95);

            using (Graphics g = Graphics.FromHwnd(rtbContent.Handle))
            using (Pen pen = new Pen(waveColor, 1.6f))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                foreach (SpellingError error in spellingErrors)
                {
                    if (error.End > textLength)
                        continue;

                    Point startPoint = rtbContent.GetPositionFromCharIndex(error.Start);
                    Point lastCharPoint = rtbContent.GetPositionFromCharIndex(error.End - 1);

                    // 한 줄 안에 있는 단어만 그린다. (줄이 바뀌는 곳에 걸친 단어는 건너뜀)
                    if (lastCharPoint.Y != startPoint.Y)
                        continue;

                    // 글자 받침과 겹치지 않도록 글자 바로 아래에 그린다.
                    int y = startPoint.Y + lineHeight;

                    if (y < 0 || startPoint.Y > client.Height)
                        continue;

                    int lastCharWidth = TextRenderer.MeasureText(
                        g,
                        error.Word.Substring(error.Word.Length - 1),
                        rtbContent.Font,
                        Size.Empty,
                        TextFormatFlags.NoPadding).Width;

                    int x1 = startPoint.X;
                    int x2 = lastCharPoint.X + lastCharWidth;

                    if (x2 - x1 < 4)
                        continue;

                    // 물결선: 3픽셀 간격으로 위아래 3픽셀
                    List<Point> wave = new List<Point>();

                    for (int x = x1, i = 0; x <= x2; x += 3, i++)
                    {
                        wave.Add(new Point(x, y + (i % 2 == 0 ? 0 : 3)));
                    }

                    if (wave.Count >= 2)
                        g.DrawLines(pen, wave.ToArray());
                }
            }
        }

        // ───────────── 우클릭 메뉴 ─────────────

        private SpellingError FindSpellingErrorAt(int charIndex)
        {
            return spellingErrors.FirstOrDefault(error => charIndex >= error.Start && charIndex < error.End);
        }

        private void rtbContent_SpellMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right || !IsSpellCheckEnabled || !SpellCheckService.IsReady)
                return;

            int index = rtbContent.GetCharIndexFromPosition(e.Location);
            SpellingError error = FindSpellingErrorAt(index);

            if (error == null)
                return;

            BuildSpellingMenu(error);
            cmsSpelling.Show(rtbContent, e.Location);
        }

        private void BuildSpellingMenu(SpellingError error)
        {
            foreach (ToolStripItem old in cmsSpelling.Items.Cast<ToolStripItem>().ToList())
            {
                cmsSpelling.Items.Remove(old);
                old.Dispose();
            }

            Color foreColor = ThemeManager.C(Color.FromArgb(230, 230, 234));
            cmsSpelling.BackColor = ThemeManager.C(Color.FromArgb(48, 51, 59));
            cmsSpelling.ForeColor = foreColor;

            List<string> suggestions = SpellCheckService.Suggest(error.Word, 5);

            if (suggestions.Count == 0)
            {
                ToolStripMenuItem none = new ToolStripMenuItem("(추천 단어 없음)");
                none.Enabled = false;
                cmsSpelling.Items.Add(none);
            }

            foreach (string suggestion in suggestions)
            {
                string replacement = suggestion;

                // 띄어쓰기를 바꾸는 추천은 따로 표시한다. (사전 방식이라 정확도가 낮음)
                string label = replacement.Contains(" ")
                    ? replacement + "   (띄어쓰기)"
                    : replacement;

                ToolStripMenuItem item = new ToolStripMenuItem(label);
                item.Font = spellSuggestionFont;
                item.ForeColor = foreColor;
                item.Click += (s, e) => ReplaceSpellingError(error, replacement);
                cmsSpelling.Items.Add(item);
            }

            cmsSpelling.Items.Add(new ToolStripSeparator());

            ToolStripMenuItem addItem = new ToolStripMenuItem("사전에 추가  ('" + error.Word + "')");
            addItem.ForeColor = foreColor;
            addItem.Click += (s, e) =>
            {
                try
                {
                    SpellCheckService.AddToUserDictionary(error.Word);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("사용자 사전에 저장하지 못했습니다.\n\n" + ex.Message, "맞춤법 검사");
                }

                RunSpellCheck();
            };
            cmsSpelling.Items.Add(addItem);

            ToolStripMenuItem ignoreItem = new ToolStripMenuItem("무시 (프로그램을 끌 때까지)");
            ignoreItem.ForeColor = foreColor;
            ignoreItem.Click += (s, e) =>
            {
                SpellCheckService.Ignore(error.Word);
                RunSpellCheck();
            };
            cmsSpelling.Items.Add(ignoreItem);
        }

        // 의심 단어를 추천 단어로 바꾼다.
        private void ReplaceSpellingError(SpellingError error, string replacement)
        {
            if (error.End > rtbContent.TextLength)
                return;

            rtbContent.Select(error.Start, error.Length);

            // 그 사이 본문이 바뀌어 다른 글자가 선택되었으면 바꾸지 않는다.
            if (rtbContent.SelectedText != error.Word)
            {
                rtbContent.Select(rtbContent.SelectionStart, 0);
                RunSpellCheck();
                return;
            }

            rtbContent.SelectedText = replacement;
            RunSpellCheck();
        }

        // 본문 창의 그리기 메시지를 가로채 물결선을 덧그린다.
        private class SpellingOverlay : NativeWindow
        {
            private const int WM_PAINT = 0x000F;
            private readonly MainForm owner;

            public SpellingOverlay(MainForm owner)
            {
                this.owner = owner;
            }

            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);

                if (m.Msg == WM_PAINT)
                {
                    try
                    {
                        owner.DrawSpellingErrors();
                    }
                    catch (Exception)
                    {
                        // 밑줄을 그리지 못해도 글쓰기에는 영향이 없도록 한다.
                    }
                }
            }
        }
    }
}
