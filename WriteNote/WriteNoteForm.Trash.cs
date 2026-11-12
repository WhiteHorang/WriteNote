using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WriteNote
{
    // 휴지통: 휴지통 목록, 복원, 영구 삭제, 휴지통 비우기
    public partial class MainForm
    {
        // ───────────── 휴지통 ─────────────

        // 휴지통 카드의 우클릭 메뉴 (코드에서 만든다)
        private ContextMenuStrip cmsTrash;


        // 휴지통 버튼
        private void btnTrashDocuments_Click(object sender, EventArgs e)
        {
            currentDocumentCategory = "trash";

            LoadRecentDocuments();
            UpdateCategoryButtonStyle();
        }


        // 휴지통 우클릭 메뉴 만들기
        private void CreateTrashMenu()
        {
            // 디자이너의 components 컨테이너는 비어 있을 수 있으므로 쓰지 않고, 창이 정리될 때 함께 정리한다.
            cmsTrash = new ContextMenuStrip();
            Disposed += (s, e) => cmsTrash.Dispose();
            cmsTrash.Renderer = new ToolStripProfessionalRenderer(new DarkMenuColorTable());
            cmsTrash.ShowImageMargin = false;
            cmsTrash.ShowCheckMargin = false;
            cmsTrash.Font = cmsRecentDocuments.Font;
            cmsTrash.BackColor = Color.FromArgb(48, 51, 59);
            cmsTrash.ForeColor = Color.FromArgb(230, 230, 234);

            cmsTrash.Items.Add("복원", null, (s, e) => RunFileAction("문서 복원", RestoreSelectedTrashItem));
            cmsTrash.Items.Add("영구 삭제", null, (s, e) => RunFileAction("영구 삭제", DeleteSelectedTrashItemPermanently));
            cmsTrash.Items.Add(new ToolStripSeparator());
            cmsTrash.Items.Add("휴지통 비우기", null, (s, e) => RunFileAction("휴지통 비우기", EmptyTrash));

            foreach (ToolStripItem item in cmsTrash.Items)
            {
                item.ForeColor = cmsTrash.ForeColor;
            }
        }


        // 휴지통 목록을 문서 카드로 표시
        private void LoadTrashList()
        {
            List<TrashItem> items = TrashManager.GetItems();

            // 검색어로 거르기
            string searchText = txtSearch.Text.Trim();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                items = items
                    .Where(item => item.Title.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }

            // 정렬: 최근 삭제순 / 오래된 삭제순 / 이름순
            switch (cboSortDocuments.SelectedIndex)
            {
                case 1:
                    items = items.OrderBy(item => item.DeletedAt).ToList();
                    break;
                case 2:
                    items = items.OrderBy(item => item.Title, StringComparer.CurrentCulture).ToList();
                    break;
                default:
                    items = items.OrderByDescending(item => item.DeletedAt).ToList();
                    break;
            }

            foreach (TrashItem item in items)
            {
                Panel card = CreateDocumentCard(item.Title, item.DeletedAt, item.TrashRtfPath);

                // 날짜 대신 삭제한 날짜를 표시
                Control[] dateLabels = card.Controls.Find("lblDocumentItemDate", false);
                if (dateLabels.Length > 0)
                {
                    dateLabels[0].Text = "삭제 " + GetModifiedDateText(item.DeletedAt);
                }

                if (string.Equals(selectedDocumentPath, item.TrashRtfPath, StringComparison.OrdinalIgnoreCase))
                {
                    selectedDocumentCard = card;
                    card.BackColor = ThemeManager.C(Color.FromArgb(55, 55, 65));
                }

                flpDocumentList.Controls.Add(card);
            }

            if (items.Count == 0)
            {
                lblEmptyDocuments.Text = string.IsNullOrWhiteSpace(searchText)
                    ? "휴지통이 비어 있습니다."
                    : "검색 결과가 없습니다.";

                lblEmptyDocuments.Visible = true;
                lblEmptyDocuments.BringToFront();
            }
        }


        // 지금 선택된 휴지통 항목
        private TrashItem SelectedTrashItem
        {
            get
            {
                if (string.IsNullOrEmpty(selectedDocumentPath))
                    return null;

                return TrashManager.FindByTrashPath(selectedDocumentPath);
            }
        }


        private void RestoreSelectedTrashItem()
        {
            TrashItem item = SelectedTrashItem;

            if (item == null)
                return;

            string restoredPath = TrashManager.Restore(item);

            LoadRecentDocuments();

            string message = "'" + item.Title + "' 문서를 복원했습니다.\n\n위치: " + restoredPath;

            if (!string.Equals(restoredPath, item.OriginalPath, StringComparison.OrdinalIgnoreCase))
            {
                message += "\n\n원래 위치에 같은 이름의 문서가 있어 이름을 바꿔 복원했습니다.";
            }

            if (!PathHelper.IsSameFolder(Path.GetDirectoryName(restoredPath), DocumentFolderPath))
            {
                message += "\n\n지금 문서 폴더와 다른 위치라 '전체 문서'에는 보이지 않습니다. " +
                           "열기 메뉴로 열 수 있습니다.";
            }

            MessageBox.Show(message, "휴지통", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void DeleteSelectedTrashItemPermanently()
        {
            TrashItem item = SelectedTrashItem;

            if (item == null)
                return;

            DialogResult result = MessageBox.Show(
                "'" + item.Title + "' 문서를 영구 삭제하시겠습니까?\n\n영구 삭제한 문서는 복구할 수 없습니다.",
                "영구 삭제",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result != DialogResult.Yes)
                return;

            TrashManager.DeletePermanently(item);
            selectedDocumentPath = "";

            LoadRecentDocuments();
        }


        private void EmptyTrash()
        {
            int count = TrashManager.GetItems().Count;

            if (count == 0)
            {
                MessageBox.Show("휴지통이 비어 있습니다.", "휴지통");
                return;
            }

            DialogResult result = MessageBox.Show(
                "휴지통의 문서 " + count + "개를 모두 영구 삭제하시겠습니까?\n\n영구 삭제한 문서는 복구할 수 없습니다.",
                "휴지통 비우기",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result != DialogResult.Yes)
                return;

            TrashManager.EmptyTrash();
            selectedDocumentPath = "";

            LoadRecentDocuments();
        }
    }
}
