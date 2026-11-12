using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace WriteNote
{
    // 사이드 패널 문서 목록: 분류 버튼, 검색·정렬, 문서 카드(선택·더블클릭·우클릭·마우스 올림)
    public partial class MainForm
    {
        // 스크롤바 표시 여부 제어
        [DllImport("user32.dll")]
        private static extern bool ShowScrollBar(
            IntPtr hWnd,
            int wBar,
            bool bShow
        );


        // 문서 목록의 스크롤바 숨기기
        private void HideDocumentScrollBars()
        {
            if (flpDocumentList.IsHandleCreated)
            {
                ShowScrollBar(flpDocumentList.Handle, 3, false);
            }
        }



        // 문서 분류에 따라 목록 불러오기
        private void LoadRecentDocuments()
        {
            try
            {
                string documentFolder = DocumentFolderPath;

                // 기존에 생성된 문서 카드 제거
                foreach (Control control in flpDocumentList.Controls.Cast<Control>().ToList())
                {
                    if (control != pnlDocumentItem && control != lblEmptyDocuments)
                    {
                        flpDocumentList.Controls.Remove(control);
                        control.Dispose();
                    }
                }

                // 디자인 견본과 빈 문서 안내 문구 숨김
                pnlDocumentItem.Visible = false;
                lblEmptyDocuments.Visible = false;

                // 폴더 선택 상자 갱신 (폴더가 바뀌었거나 문서 폴더가 바뀐 경우 반영)
                RefreshFolderList();

                // 휴지통은 문서 폴더가 아니라 휴지통 목록을 보여 준다.
                if (currentDocumentCategory == "trash")
                {
                    LoadTrashList();
                    return;
                }

                if (!Directory.Exists(documentFolder))
                {
                    MessageBox.Show("문서 폴더를 찾을 수 없습니다.", "WriteNote", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                List<string> documents = new List<string>();

                // 문서 분류에 따라 파일 가져오기
                if (currentDocumentCategory == "all")
                {
                    documents = GetDocumentsInCurrentFolder();
                }
                else if (currentDocumentCategory == "recent")
                {
                    documents = RecentDocumentManager.GetRecentDocuments();
                }
                else if (currentDocumentCategory == "favorite")
                {
                    documents = GetDocumentsInCurrentFolder();
                }

                // 존재하는 파일만 가져오기
                documents = documents
                    .Where(file => File.Exists(file))
                    .ToList();

                // 즐겨찾기 분류라면 즐겨찾기 문서만 필터링
                if (currentDocumentCategory == "favorite")
                {
                    documents = documents
                        .Where(file =>
                        {
                            string jsonPath = Path.ChangeExtension(file, ".json");

                            if (!File.Exists(jsonPath))
                                return false;

                            try
                            {
                                string json = File.ReadAllText(jsonPath);

                                DocumentInfo info =
                                    JsonConvert.DeserializeObject<DocumentInfo>(json);

                                return info != null && info.IsFavorite;
                            }
                            catch
                            {
                                return false;
                            }
                        })
                        .ToList();
                }

                // 검색어 가져오기
                string searchText = txtSearch.Text.Trim();

                // 검색어가 있다면 제목으로 필터링
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    documents = documents
                        .Where(file =>
                        {
                            string title = Path.GetFileNameWithoutExtension(file);

                            string jsonPath = Path.ChangeExtension(file, ".json");

                            if (File.Exists(jsonPath))
                            {
                                try
                                {
                                    string json = File.ReadAllText(jsonPath);

                                    DocumentInfo info =
                                        JsonConvert.DeserializeObject<DocumentInfo>(json);

                                    if (info != null &&
                                        !string.IsNullOrWhiteSpace(info.Title))
                                    {
                                        title = info.Title;
                                    }
                                }
                                catch
                                {
                                    // JSON을 읽지 못하면 파일명으로 검색
                                }
                            }

                            return title.IndexOf(
                                searchText,
                                StringComparison.OrdinalIgnoreCase
                            ) >= 0;
                        })
                        .ToList();
                }

                // 정렬 기준에 따라 문서 목록 정렬
                switch (cboSortDocuments.SelectedIndex)
                {
                    case 0: // 최근 수정순
                        documents = documents
                            .OrderByDescending(file => File.GetLastWriteTime(file))
                            .ToList();
                        break;

                    case 1: // 오래된 수정순
                        documents = documents
                            .OrderBy(file => File.GetLastWriteTime(file))
                            .ToList();
                        break;

                    case 2: // 이름순
                        documents = documents
                            .OrderBy(file =>
                            {
                                string title = Path.GetFileNameWithoutExtension(file);

                                string jsonPath = Path.ChangeExtension(file, ".json");

                                if (File.Exists(jsonPath))
                                {
                                    try
                                    {
                                        string json = File.ReadAllText(jsonPath);

                                        DocumentInfo info =
                                            JsonConvert.DeserializeObject<DocumentInfo>(json);

                                        if (info != null &&
                                            !string.IsNullOrWhiteSpace(info.Title))
                                        {
                                            title = info.Title;
                                        }
                                    }
                                    catch
                                    {
                                        // JSON을 읽지 못하면 파일명 사용
                                    }
                                }

                                return title;
                            })
                            .ToList();
                        break;
                }

                // 문서 카드 생성
                foreach (string filePath in documents)
                {
                    string title = Path.GetFileNameWithoutExtension(filePath);

                    string jsonPath = Path.ChangeExtension(filePath, ".json");

                    DocumentInfo info = null;

                    if (File.Exists(jsonPath))
                    {
                        try
                        {
                            string json = File.ReadAllText(jsonPath);

                            info = JsonConvert.DeserializeObject<DocumentInfo>(json);

                            if (info != null &&
                                !string.IsNullOrWhiteSpace(info.Title))
                            {
                                title = info.Title;
                            }
                        }
                        catch
                        {
                            // JSON을 읽지 못하면 파일명을 사용
                        }
                    }

                    // 마지막 수정 날짜 가져오기
                    DateTime modifiedDate = File.GetLastWriteTime(filePath);

                    // 즐겨찾기 문서라면 제목 앞에 별표 표시
                    if (info != null && info.IsFavorite)
                    {
                        title = "★ " + title;
                    }

                    // 문서 카드 생성
                    Panel documentCard = CreateDocumentCard(
                        title,
                        modifiedDate,
                        filePath
                    );

                    // 하위 폴더에 있는 문서는 날짜 옆에 폴더 이름 표시
                    string folderName = GetDocumentFolderName(filePath);

                    if (folderName != null && currentFolderFilter != folderName)
                    {
                        Control[] dateLabels = documentCard.Controls.Find("lblDocumentItemDate", false);

                        if (dateLabels.Length > 0)
                        {
                            dateLabels[0].Text += "  ·  " + folderName;
                        }
                    }

                    // 이전에 선택했던 문서라면 선택 상태 복원
                    if (string.Equals(
                        selectedDocumentPath,
                        filePath,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        selectedDocumentCard = documentCard;
                        documentCard.BackColor = ThemeManager.C(Color.FromArgb(55, 55, 65));
                    }

                    flpDocumentList.Controls.Add(documentCard);
                }

                // 최종 문서 목록이 비어 있으면 안내 문구 표시
                if (documents.Count == 0)
                {
                    if (!string.IsNullOrWhiteSpace(searchText))
                    {
                        lblEmptyDocuments.Text = "검색 결과가 없습니다.";
                    }
                    else if (currentDocumentCategory == "favorite")
                    {
                        lblEmptyDocuments.Text = "즐겨찾기한 문서가 없습니다.";
                    }
                    else if (currentDocumentCategory == "recent")
                    {
                        lblEmptyDocuments.Text = "최근 문서가 없습니다.";
                    }
                    else
                    {
                        lblEmptyDocuments.Text = "아직 작성한 문서가 없습니다.";
                    }

                    lblEmptyDocuments.Visible = true;
                    lblEmptyDocuments.BringToFront();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "문서 목록을 불러오지 못했습니다.\n\n" + ex.Message,
                    "오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }



        // 문서 카드 생성
        private Panel CreateDocumentCard(
            string title,
            DateTime modifiedDate,
            string filePath)
        {
            // 디자인 견본을 참고하여 Panel 생성
            Panel card = new Panel();

            card.Size = pnlDocumentItem.Size;
            card.BackColor = pnlDocumentItem.BackColor;
            card.Margin = pnlDocumentItem.Margin;
            card.Padding = pnlDocumentItem.Padding;
            card.BorderStyle = pnlDocumentItem.BorderStyle;

            // 문서 제목 Label
            Label titleLabel = new Label();
            titleLabel.AutoEllipsis = true;
            titleLabel.TextAlign = ContentAlignment.MiddleLeft;
            titleLabel.UseMnemonic = false;

            titleLabel.Name = "lblDocumentItemTitle";
            titleLabel.Text = title;
            titleLabel.Location = lblDocumentItemTitle.Location;
            titleLabel.Size = lblDocumentItemTitle.Size;
            titleLabel.ForeColor = lblDocumentItemTitle.ForeColor;
            titleLabel.Font = lblDocumentItemTitle.Font;
            titleLabel.AutoSize = false;
            titleLabel.AutoEllipsis = true;
            titleLabel.TextAlign = lblDocumentItemTitle.TextAlign;

            // 수정 날짜 Label
            Label dateLabel = new Label();

            dateLabel.Name = "lblDocumentItemDate";
            dateLabel.Text = GetModifiedDateText(modifiedDate);
            dateLabel.Location = lblDocumentItemDate.Location;
            dateLabel.Size = lblDocumentItemDate.Size;
            dateLabel.ForeColor = lblDocumentItemDate.ForeColor;
            dateLabel.Font = lblDocumentItemDate.Font;
            dateLabel.AutoSize = false;
            dateLabel.AutoEllipsis = true;
            dateLabel.TextAlign = lblDocumentItemDate.TextAlign;

            // 파일 경로 저장
            card.Tag = filePath;
            titleLabel.Tag = filePath;
            dateLabel.Tag = filePath;

            // Label을 Panel 안에 추가
            card.Controls.Add(titleLabel);
            card.Controls.Add(dateLabel);

            // 문서 카드 클릭 이벤트
            card.Click += DocumentCard_Click;
            titleLabel.Click += DocumentCard_Click;
            dateLabel.Click += DocumentCard_Click;
            card.Paint += DocumentCard_Paint;

            // 문서 카드 우클릭 이벤트
            card.MouseDown += DocumentCard_MouseDown;
            titleLabel.MouseDown += DocumentCard_MouseDown;
            dateLabel.MouseDown += DocumentCard_MouseDown;

            // 문서 카드 더블클릭 이벤트
            card.DoubleClick += DocumentCard_DoubleClick;
            titleLabel.DoubleClick += DocumentCard_DoubleClick;
            dateLabel.DoubleClick += DocumentCard_DoubleClick;

            // 마우스 Hover 이벤트
            card.MouseEnter += DocumentCard_MouseEnter;
            titleLabel.MouseEnter += DocumentCard_MouseEnter;
            dateLabel.MouseEnter += DocumentCard_MouseEnter;

            card.MouseLeave += DocumentCard_MouseLeave;
            titleLabel.MouseLeave += DocumentCard_MouseLeave;
            dateLabel.MouseLeave += DocumentCard_MouseLeave;

            return card;
        }


        // 전체 문서 버튼
        private void btnAllDocuments_Click(object sender, EventArgs e)
        {
            currentDocumentCategory = "all";

            LoadRecentDocuments();
            UpdateCategoryButtonStyle();
        }


        // 최근 문서 버튼
        private void btnRecentDocuments_Click(object sender, EventArgs e)
        {
            currentDocumentCategory = "recent";

            LoadRecentDocuments();
            UpdateCategoryButtonStyle();
        }


        // 즐겨찾기 버튼
        private void btnFavoriteDocuments_Click(object sender, EventArgs e)
        {
            currentDocumentCategory = "favorite";

            LoadRecentDocuments();
            UpdateCategoryButtonStyle();
        }


        // 현재 선택된 분류 버튼 스타일 변경
        private void UpdateCategoryButtonStyle()
        {
            Color selectedColor = ThemeManager.C(Color.FromArgb(55, 55, 65));
            Color normalColor = ThemeManager.C(Color.FromArgb(35, 35, 38));

            btnAllDocuments.BackColor =
                currentDocumentCategory == "all"
                    ? selectedColor
                    : normalColor;

            btnRecentDocuments.BackColor =
                currentDocumentCategory == "recent"
                    ? selectedColor
                    : normalColor;

            btnFavoriteDocuments.BackColor =
                currentDocumentCategory == "favorite"
                    ? selectedColor
                    : normalColor;

            btnTrashDocuments.BackColor =
                currentDocumentCategory == "trash"
                    ? selectedColor
                    : normalColor;
        }


        // 사이드바 열기 및 닫기
        private void btnToggleSidebar_Click(object sender, EventArgs e)
        {
            isSidebarOpen = !isSidebarOpen;

            pnlRecentDocuments.Visible = isSidebarOpen;
        }


        // 문서 카드 우클릭 처리
        private void DocumentCard_MouseDown(object sender, MouseEventArgs e)
        {
            // 휴지통에서는 복원·영구 삭제 메뉴를 보여 준다.
            if (e.Button == MouseButtons.Right && currentDocumentCategory == "trash")
            {
                Control trashControl = sender as Control;

                if (trashControl == null)
                    return;

                DocumentCard_Click(trashControl, EventArgs.Empty);
                cmsTrash.Show(trashControl, e.Location);
                return;
            }

            if (e.Button == MouseButtons.Right)
            {
                Control clickedControl = sender as Control;

                if (clickedControl == null)
                    return;

                // 클릭한 컨트롤에서 파일 경로 가져오기
                string filePath = clickedControl.Tag as string;

                if (string.IsNullOrEmpty(filePath))
                    return;

                // 우클릭한 문서를 선택 상태로 변경
                DocumentCard_Click(clickedControl, EventArgs.Empty);

                // 메뉴의 각 기능이 이 문서를 대상으로 동작한다.
                menuDocumentPath = filePath;

                // 우클릭 메뉴 표시
                cmsRecentDocuments.Show(
                    clickedControl,
                    e.Location
                );
            }
        }


        // 문서 카드 더블클릭 처리
        private void DocumentCard_DoubleClick(object sender, EventArgs e)
        {
            Control clickedControl = sender as Control;

            if (clickedControl == null)
                return;

            string filePath = clickedControl.Tag as string;

            if (string.IsNullOrEmpty(filePath))
                return;

            // 휴지통 문서는 바로 열지 않고 복원할지 묻는다.
            if (currentDocumentCategory == "trash")
            {
                DocumentCard_Click(clickedControl, EventArgs.Empty);

                TrashItem item = SelectedTrashItem;

                if (item != null &&
                    MessageBox.Show(
                        "'" + item.Title + "' 문서는 휴지통에 있습니다.\n복원하시겠습니까?",
                        "휴지통",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    RunFileAction("문서 복원", RestoreSelectedTrashItem);
                }

                return;
            }

            if (!File.Exists(filePath))
            {
                MessageBox.Show("문서 파일을 찾을 수 없습니다.", "WriteNote", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoadRecentDocuments();
                return;
            }

            if (!ConfirmSaveChanges())
                return;

            LoadDocument(filePath);
        }


        // 문서 카드 선택 처리
        private void DocumentCard_Click(object sender, EventArgs e)
        {
            Control clickedControl = sender as Control;

            if (clickedControl == null)
                return;

            Panel clickedCard = clickedControl as Panel;

            // Label을 클릭했다면 부모 Panel을 가져옴
            if (clickedCard == null)
            {
                clickedCard = clickedControl.Parent as Panel;
            }

            if (clickedCard == null)
                return;

            // 이전에 선택한 카드 색상 복구
            if (selectedDocumentCard != null &&
                !selectedDocumentCard.IsDisposed)
            {
                selectedDocumentCard.BackColor = pnlDocumentItem.BackColor;
                selectedDocumentCard.Invalidate();
            }

            // 새로 선택한 카드 저장
            selectedDocumentCard = clickedCard;

            // 선택된 문서 경로 저장
            selectedDocumentPath = clickedCard.Tag as string;

            // 선택된 카드 강조
            selectedDocumentCard.BackColor = ThemeManager.C(Color.FromArgb(55, 55, 65));
            selectedDocumentCard.Invalidate();
        }


        // 문서 검색
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadRecentDocuments();
        }


        private void cboSortDocuments_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRecentDocuments();
        }


        private string GetModifiedDateText(DateTime modifiedDate)
        {
            DateTime today = DateTime.Today;

            if (modifiedDate.Date == today)
            {
                return "오늘 " + modifiedDate.ToString("tt h:mm");
            }
            else if (modifiedDate.Date == today.AddDays(-1))
            {
                return "어제 " + modifiedDate.ToString("tt h:mm");
            }
            else if (modifiedDate >= DateTime.Now.AddMinutes(-1))
            {
                return "방금 전";
            }
            else
            {
                return modifiedDate.ToString("yyyy. MM. dd.");
            }
        }


        // 마우스를 카드 위에 올렸을 때
        private void DocumentCard_MouseEnter(object sender, EventArgs e)
        {
            Control control = sender as Control;

            if (control == null) return;

            Panel card = control as Panel;

            if (card == null)
                card = control.Parent as Panel;

            if (card == null) return;

            // 선택된 카드가 아니라면 Hover 색상 적용
            if (card != selectedDocumentCard)
            {
                card.BackColor = ThemeManager.C(Color.FromArgb(45, 45, 52));
            }
        }


        // 마우스가 카드에서 벗어났을 때
        private void DocumentCard_MouseLeave(object sender, EventArgs e)
        {
            Control control = sender as Control;

            if (control == null) return;

            Panel card = control as Panel;

            if (card == null)
                card = control.Parent as Panel;

            if (card == null) return;

            // 선택된 카드가 아니라면 기본 색상으로 복원
            if (card != selectedDocumentCard)
            {
                card.BackColor = pnlDocumentItem.BackColor;
            }
        }


        private void DocumentCard_Paint(object sender, PaintEventArgs e)
        {
            Panel card = sender as Panel;

            if (card == null) return;

            if (card == selectedDocumentCard)
            {
                using (Pen pen = new Pen(ThemeManager.C(Color.FromArgb(130, 130, 145)), 1))
                {
                    Rectangle rect = new Rectangle(
                        0, 0,
                        card.Width - 1,
                        card.Height - 1
                    );

                    e.Graphics.DrawRectangle(pen, rect);
                }
            }
        }
    }
}
