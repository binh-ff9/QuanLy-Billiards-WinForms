using Billiard.BLL.Services.QLBan;
using Billiard.BLL.Services.HoaDonServices;
using Billiard.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.QLBan
{
    /// <summary>
    /// Form quản lý bàn - chịu trách nhiệm hiển thị danh sách bàn và detail panel
    /// </summary>
    public partial class QLBanForm : Form
    {
        private readonly BanBiaService _banBiaService;
        private MainForm _mainForm;
        private List<BanBium> _allTables;

        // Filters
        private string _currentAreaFilter = "all";
        private string _currentStatusFilter = "all";
        private string _currentTypeFilter = "all";

        // Auto refresh
        private System.Windows.Forms.Timer _refreshTimer;

        // Detail Panel Management
        private Panel pnlDetailContainer;
        private Panel pnlDetailHeader;
        private Panel pnlDetailContent;
        private BanChiTietControl _chiTietControl;
        private bool _isDetailVisible = false;

        // Constants
        private const int DETAIL_PANEL_WIDTH = 430;
        private const int CARD_WIDTH = 250;
        private const int ANIMATION_STEPS = 15;
        private const int ANIMATION_INTERVAL = 8;

        public QLBanForm(BanBiaService banBiaService)
        {
            _banBiaService = banBiaService;
            InitializeComponent();
            InitializeDetailPanel();
            InitializeRefreshTimer();
            InitializeKeyboardShortcuts();

            this.AutoScroll = false;
            this.AutoSize = false;
        }

        #region Initialization

        private void InitializeDetailPanel()
        {
            // Main container
            pnlDetailContainer = new Panel
            {
                Width = DETAIL_PANEL_WIDTH,
                BackColor = Color.White,
                Visible = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom
            };

            // Header
            pnlDetailHeader = CreateDetailHeader();

            // Content
            pnlDetailContent = new Panel
            {
                Location = new Point(0, 70),
                Width = DETAIL_PANEL_WIDTH,
                AutoScroll = true,
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                Name = "pnlDetailContent"
            };

            // Border paint
            pnlDetailContainer.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(226, 232, 240), 2))
                {
                    e.Graphics.DrawLine(pen, 0, 0, 0, pnlDetailContainer.Height);
                }
            };

            pnlDetailContainer.Controls.AddRange(new Control[] { pnlDetailContent, pnlDetailHeader });
            this.Controls.Add(pnlDetailContainer);

            pnlDetailContainer.BringToFront();

            // Handle resize
            this.Resize += (s, e) =>
            {
                if (_isDetailVisible)
                {
                    PositionDetailPanel();
                }
            };
        }

        private Panel CreateDetailHeader()
        {
            var header = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(DETAIL_PANEL_WIDTH, 70),
                BackColor = Color.FromArgb(30, 58, 138),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            var lblTitle = new Label
            {
                Text = "CHI TIẾT BÀN",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(15, 23),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            var btnClose = new Button
            {
                Text = "✕",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(45, 45),
                Location = new Point(DETAIL_PANEL_WIDTH - 55, 13),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(79, 70, 229);
            btnClose.Click += (s, e) => HideDetailPanel();

            var toolTip = new ToolTip();
            toolTip.SetToolTip(btnClose, "Đóng chi tiết bàn (ESC)");

            header.Controls.AddRange(new Control[] { lblTitle, btnClose });
            return header;
        }

        private void InitializeRefreshTimer()
        {
            _refreshTimer = new System.Windows.Forms.Timer
            {
                Interval = 30000 // 30 seconds
            };
            _refreshTimer.Tick += async (s, e) => await RefreshTablesSmooth();
        }

        private void InitializeKeyboardShortcuts()
        {
            this.KeyPreview = true;
            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape && _isDetailVisible)
                {
                    HideDetailPanel();
                    e.Handled = true;
                }
            };
        }

        #endregion

        #region Detail Panel Management

        private void PositionDetailPanel()
        {
            if (pnlDetailContainer == null) return;

            var targetX = this.ClientSize.Width - DETAIL_PANEL_WIDTH;
            var targetY = 0;
            var targetHeight = this.ClientSize.Height;

            pnlDetailContainer.Location = new Point(targetX, targetY);
            pnlDetailContainer.Height = targetHeight;

            if (pnlDetailContent != null)
            {
                pnlDetailContent.Height = targetHeight - 70;
            }
        }

        public async void ShowTableDetail(BanBium ban)
        {
            if (_mainForm == null) return;

            try
            {
                var hoaDonService = Program.GetService<HoaDonService>();

                // Clear old control
                pnlDetailContent.Controls.Clear();
                _chiTietControl?.Dispose();

                // Create new control
                _chiTietControl = new BanChiTietControl(
                    _banBiaService,
                    hoaDonService,
                    ban,
                    _mainForm.MaNV
                );

                _chiTietControl.Dock = DockStyle.Fill;
                _chiTietControl.BackColor = Color.White;

                // Subscribe to events
                _chiTietControl.OnDataChanged += async (s, e) =>
                {
                    await RefreshTables();
                };

                _chiTietControl.OnBanUpdated += (s, updatedBan) =>
                {
                    UpdateCardForBan(updatedBan);
                };

                pnlDetailContent.Controls.Add(_chiTietControl);
                ShowDetailPanel();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị chi tiết bàn: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowDetailPanel()
        {
            if (_isDetailVisible) return;

            _isDetailVisible = true;
            flpBanBia.Padding = new Padding(15, 15, DETAIL_PANEL_WIDTH + 25, 15);

            pnlDetailContainer.Visible = true;
            pnlDetailContainer.Width = DETAIL_PANEL_WIDTH;
            PositionDetailPanel();
            pnlDetailContainer.BringToFront();

            // Slide animation
            AnimatePanel(
                startX: this.ClientSize.Width,
                targetX: this.ClientSize.Width - DETAIL_PANEL_WIDTH,
                onComplete: null
            );
        }

        private void HideDetailPanel()
        {
            if (!_isDetailVisible) return;

            var startX = pnlDetailContainer.Location.X;
            var targetX = this.ClientSize.Width;

            AnimatePanel(startX, targetX, () =>
            {
                pnlDetailContainer.Visible = false;
                _isDetailVisible = false;
                flpBanBia.Padding = new Padding(15);

                pnlDetailContent.Controls.Clear();
                _chiTietControl?.Dispose();
                _chiTietControl = null;
            });
        }

        private void AnimatePanel(int startX, int targetX, Action onComplete)
        {
            var timer = new System.Windows.Forms.Timer { Interval = ANIMATION_INTERVAL };
            var step = 0;

            timer.Tick += (s, e) =>
            {
                step++;
                var progress = (double)step / ANIMATION_STEPS;

                // Easing function
                var easedProgress = targetX > startX
                    ? Math.Pow(progress, 2) // Ease out
                    : 1 - Math.Pow(1 - progress, 3); // Ease in

                var newX = startX + (int)((targetX - startX) * easedProgress);
                pnlDetailContainer.Location = new Point(newX, 0);
                pnlDetailContainer.Height = this.ClientSize.Height;

                if (pnlDetailContent != null)
                {
                    pnlDetailContent.Height = pnlDetailContainer.Height - 70;
                }

                if (step >= ANIMATION_STEPS)
                {
                    pnlDetailContainer.Location = new Point(targetX, 0);
                    timer.Stop();
                    timer.Dispose();
                    onComplete?.Invoke();
                }
            };

            timer.Start();
        }

        #endregion

        #region Table Card Management

        private Panel CreateTableCard(BanBium ban)
        {
            var card = new Panel
            {
                Width = CARD_WIDTH,
                Height = 280,
                Margin = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand,
                Tag = ban,
                Name = $"card_{ban.MaBan}"
            };

            card.BackColor = GetCardBackColor(ban.TrangThai);

            // Border paint
            card.Paint += (s, e) =>
            {
                var borderColor = GetStatusColor(ban.TrangThai);
                using (var pen = new Pen(borderColor, 3))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                }
            };

            // Components
            var pnlImage = CreateImagePanel(ban);
            var lblName = CreateNameLabel(ban);
            var lblInfo = CreateInfoLabel(ban);
            var lblPrice = CreatePriceLabel(ban);

            card.Controls.AddRange(new Control[] { pnlImage, lblName, lblInfo, lblPrice });

            // Add customer label for playing tables
            if (ban.TrangThai == "Đang chơi")
            {
                var lblCustomer = CreateCustomerLabel(ban);
                card.Controls.Add(lblCustomer);
            }

            // Click handlers
            SetupCardClickHandlers(card, ban);

            // Hover effects
            SetupCardHoverEffects(card, ban);

            return card;
        }

        private Panel CreateImagePanel(BanBium ban)
        {
            var pnlImage = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(CARD_WIDTH, 140),
                BackColor = Color.FromArgb(248, 250, 252)
            };

            // Load image
            LoadTableImage(pnlImage, ban.HinhAnh);

            // Status badge
            var lblStatus = new Label
            {
                Text = ban.TrangThai,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = GetStatusColor(ban.TrangThai),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(85, 28),
                Location = new Point(10, 10),
                Name = "lblStatus"
            };
            pnlImage.Controls.Add(lblStatus);

            // VIP badge
            if (ban.MaKhuVucNavigation?.TenKhuVuc == "VIP")
            {
                var lblVIP = new Label
                {
                    Text = "⭐ VIP",
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(168, 85, 247),
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Size = new Size(75, 28),
                    Location = new Point(CARD_WIDTH - 85, 10)
                };
                pnlImage.Controls.Add(lblVIP);
            }

            // Edit button
            var btnEdit = CreateEditButton(ban);
            pnlImage.Controls.Add(btnEdit);
            btnEdit.BringToFront();

            return pnlImage;
        }

        private Button CreateEditButton(BanBium ban)
        {
            var btnEdit = new Button
            {
                Text = "✏️",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Size = new Size(35, 35),
                Location = new Point(CARD_WIDTH - 45, 95),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Tag = ban,
                TabStop = false
            };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);

            btnEdit.Click += async (s, e) => await EditTable(ban);

            return btnEdit;
        }

        private Label CreateNameLabel(BanBium ban)
        {
            return new Label
            {
                Text = ban.TenBan,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 150),
                Size = new Size(CARD_WIDTH, 35),
                Name = "lblName"
            };
        }

        private Label CreateInfoLabel(BanBium ban)
        {
            var lblInfo = new Label
            {
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 185),
                Size = new Size(CARD_WIDTH, 28),
                Name = "lblInfo"
            };

            UpdateInfoLabelText(lblInfo, ban);
            return lblInfo;
        }

        private void UpdateInfoLabelText(Label lblInfo, BanBium ban)
        {
            if (ban.TrangThai == "Đang chơi" && ban.GioBatDau.HasValue)
            {
                var duration = DateTime.Now - ban.GioBatDau.Value;
                lblInfo.Text = $"⏱️ {(int)duration.TotalHours}h {duration.Minutes}m";
                lblInfo.ForeColor = Color.FromArgb(239, 68, 68);
                lblInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            }
            else if (ban.TrangThai == "Đã đặt")
            {
                lblInfo.Text = $"👤 {ban.MaKhNavigation?.TenKh ?? "Khách đặt"}";
            }
            else
            {
                lblInfo.Text = $"📍 {ban.MaKhuVucNavigation?.TenKhuVuc ?? "Khu vực"}";
            }
        }

        private Label CreateCustomerLabel(BanBium ban)
        {
            return new Label
            {
                Text = $"👤 {ban.MaKhNavigation?.TenKh ?? "Khách lẻ"}",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(71, 85, 105),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 210),
                Size = new Size(CARD_WIDTH, 22),
                Name = "lblCustomer"
            };
        }

        private Label CreatePriceLabel(BanBium ban)
        {
            return new Label
            {
                Text = $"{ban.MaLoaiNavigation?.GiaGio:N0} đ/giờ",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(99, 102, 241),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 235),
                Size = new Size(CARD_WIDTH, 28)
            };
        }

        private void SetupCardClickHandlers(Panel card, BanBium ban)
        {
            EventHandler clickHandler = (s, e) => ShowTableDetail(ban);
            card.Click += clickHandler;

            foreach (Control ctrl in card.Controls)
            {
                if (ctrl is Panel pnlImage)
                {
                    foreach (Control subCtrl in pnlImage.Controls)
                    {
                        if (subCtrl.Text != "✏️") // Skip edit button
                        {
                            subCtrl.Click += clickHandler;
                        }
                    }
                }
                else
                {
                    ctrl.Click += clickHandler;
                }
            }
        }

        private void SetupCardHoverEffects(Panel card, BanBium ban)
        {
            card.MouseEnter += (s, e) =>
            {
                card.BorderStyle = BorderStyle.Fixed3D;
                var currentColor = card.BackColor;
                card.BackColor = Color.FromArgb(
                    Math.Max(0, currentColor.R - 10),
                    Math.Max(0, currentColor.G - 10),
                    Math.Max(0, currentColor.B - 10)
                );
            };

            card.MouseLeave += (s, e) =>
            {
                card.BorderStyle = BorderStyle.FixedSingle;
                card.BackColor = GetCardBackColor(ban.TrangThai);
            };
        }

        private void LoadTableImage(Panel pnlImage, string hinhAnh)
        {
            if (string.IsNullOrEmpty(hinhAnh))
            {
                AddDefaultTableIcon(pnlImage);
                return;
            }

            try
            {
                var projectRoot = Directory.GetParent(Directory.GetParent(Directory.GetParent(
                    Directory.GetParent(Application.StartupPath).FullName).FullName).FullName).FullName;
                var imagePath = Path.Combine(projectRoot, "Forms", "Resources", "img", "tables", hinhAnh);

                if (File.Exists(imagePath))
                {
                    var picTable = new PictureBox
                    {
                        Size = new Size(CARD_WIDTH, 140),
                        Location = new Point(0, 0),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        BackColor = Color.FromArgb(248, 250, 252)
                    };

                    using (var img = Image.FromFile(imagePath))
                    {
                        picTable.Image = new Bitmap(img);
                    }

                    pnlImage.Controls.Add(picTable);
                    picTable.SendToBack();
                }
                else
                {
                    AddDefaultTableIcon(pnlImage);
                }
            }
            catch
            {
                AddDefaultTableIcon(pnlImage);
            }
        }

        private void AddDefaultTableIcon(Panel pnlImage)
        {
            var lblIcon = new Label
            {
                Text = "🎱",
                Font = new Font("Segoe UI", 56F),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(CARD_WIDTH, 140),
                BackColor = Color.Transparent
            };
            pnlImage.Controls.Add(lblIcon);
            lblIcon.SendToBack();
        }

        private void UpdateCardForBan(BanBium updatedBan)
        {
            var card = flpBanBia.Controls.Find($"card_{updatedBan.MaBan}", false).FirstOrDefault() as Panel;
            if (card == null) return;

            // Update tag
            card.Tag = updatedBan;

            // Update background
            card.BackColor = GetCardBackColor(updatedBan.TrangThai);

            // Update status label
            var pnlImage = card.Controls.OfType<Panel>().FirstOrDefault();
            var lblStatus = pnlImage?.Controls.Find("lblStatus", false).FirstOrDefault() as Label;
            if (lblStatus != null)
            {
                lblStatus.Text = updatedBan.TrangThai;
                lblStatus.BackColor = GetStatusColor(updatedBan.TrangThai);
            }

            // Update info label
            var lblInfo = card.Controls.Find("lblInfo", false).FirstOrDefault() as Label;
            if (lblInfo != null)
            {
                UpdateInfoLabelText(lblInfo, updatedBan);
            }

            // Update/remove customer label
            var lblCustomer = card.Controls.Find("lblCustomer", false).FirstOrDefault() as Label;
            if (updatedBan.TrangThai == "Đang chơi")
            {
                if (lblCustomer == null)
                {
                    lblCustomer = CreateCustomerLabel(updatedBan);
                    card.Controls.Add(lblCustomer);
                }
                else
                {
                    lblCustomer.Text = $"👤 {updatedBan.MaKhNavigation?.TenKh ?? "Khách lẻ"}";
                }
            }
            else if (lblCustomer != null)
            {
                card.Controls.Remove(lblCustomer);
                lblCustomer.Dispose();
            }

            card.Invalidate();
        }

        #endregion

        #region Data Loading & Filtering

        private async Task LoadTables()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                flpBanBia.SuspendLayout();
                flpBanBia.Controls.Clear();

                _allTables = await _banBiaService.GetAllTablesAsync();
                RenderFilteredTables();

                flpBanBia.ResumeLayout();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                flpBanBia.ResumeLayout();
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Lỗi tải danh sách bàn: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task RefreshTables()
        {
            await LoadTables();

            // Reload detail if visible
            if (_isDetailVisible && _chiTietControl != null)
            {
                await _chiTietControl.LoadBanDetail();
            }
        }

        private async Task RefreshTablesSmooth()
        {
            try
            {
                var newTables = await _banBiaService.GetAllTablesAsync();

                if (HasTableChanges(newTables))
                {
                    _allTables = newTables;
                    UpdateExistingCards();
                }

                // Refresh detail if visible
                if (_isDetailVisible && _chiTietControl != null)
                {
                    await _chiTietControl.LoadBanDetail();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi refresh: {ex.Message}");
            }
        }

        private bool HasTableChanges(List<BanBium> newTables)
        {
            if (_allTables == null || _allTables.Count != newTables.Count)
                return true;

            for (int i = 0; i < _allTables.Count; i++)
            {
                var oldTable = _allTables[i];
                var newTable = newTables.FirstOrDefault(t => t.MaBan == oldTable.MaBan);

                if (newTable == null) return true;

                if (oldTable.TrangThai != newTable.TrangThai ||
                    oldTable.GioBatDau != newTable.GioBatDau ||
                    oldTable.MaKh != newTable.MaKh)
                {
                    return true;
                }
            }

            return false;
        }

        private void UpdateExistingCards()
        {
            flpBanBia.SuspendLayout();

            var filteredTables = GetFilteredTables().ToList();
            var existingCards = flpBanBia.Controls.OfType<Panel>()
                .Where(p => p.Tag is BanBium)
                .ToList();

            foreach (var card in existingCards)
            {
                var ban = card.Tag as BanBium;
                var updatedBan = filteredTables.FirstOrDefault(t => t.MaBan == ban.MaBan);

                if (updatedBan != null)
                {
                    UpdateCardForBan(updatedBan);
                }
                else
                {
                    flpBanBia.Controls.Remove(card);
                    card.Dispose();
                }
            }

            // Add new cards
            var existingIds = existingCards.Select(c => ((BanBium)c.Tag).MaBan).ToList();
            var newTables = filteredTables.Where(t => !existingIds.Contains(t.MaBan)).ToList();

            foreach (var ban in newTables)
            {
                var card = CreateTableCard(ban);
                flpBanBia.Controls.Add(card);
            }

            if (flpBanBia.Controls.Count == 0)
            {
                ShowEmptyState();
            }

            flpBanBia.ResumeLayout();
        }

        private void RenderFilteredTables()
        {
            flpBanBia.SuspendLayout();
            flpBanBia.Controls.Clear();

            var filteredTables = GetFilteredTables().ToList();

            if (filteredTables.Count == 0)
            {
                ShowEmptyState();
            }
            else
            {
                foreach (var ban in filteredTables)
                {
                    var card = CreateTableCard(ban);
                    flpBanBia.Controls.Add(card);
                }
            }

            flpBanBia.ResumeLayout();
            flpBanBia.PerformLayout();
        }

        private IEnumerable<BanBium> GetFilteredTables()
        {
            var filtered = _allTables.AsEnumerable();

            if (_currentAreaFilter != "all")
            {
                filtered = filtered.Where(b => b.MaKhuVucNavigation?.TenKhuVuc == _currentAreaFilter);
            }

            if (_currentStatusFilter != "all")
            {
                filtered = filtered.Where(b => b.TrangThai == _currentStatusFilter);
            }

            if (_currentTypeFilter != "all")
            {
                filtered = filtered.Where(b => b.MaLoaiNavigation?.TenLoai == _currentTypeFilter);
            }

            var searchText = txtSearch.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(searchText))
            {
                filtered = filtered.Where(b => b.TenBan.ToLower().Contains(searchText));
            }

            return filtered;
        }

        private void ShowEmptyState()
        {
            var pnlEmpty = new Panel
            {
                Size = new Size(flpBanBia.Width - 40, 300),
                BackColor = Color.White
            };

            var lblIcon = new Label
            {
                Text = "🎱",
                Font = new Font("Segoe UI", 48F),
                AutoSize = true
            };
            lblIcon.Location = new Point((pnlEmpty.Width - lblIcon.Width) / 2, 80);

            var lblTitle = new Label
            {
                Text = "Không tìm thấy bàn nào",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                AutoSize = true
            };
            lblTitle.Location = new Point((pnlEmpty.Width - lblTitle.Width) / 2, 160);

            var lblDesc = new Label
            {
                Text = "Thử thay đổi bộ lọc hoặc tìm kiếm khác",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.Gray,
                AutoSize = true
            };
            lblDesc.Location = new Point((pnlEmpty.Width - lblDesc.Width) / 2, 195);

            pnlEmpty.Controls.AddRange(new Control[] { lblIcon, lblTitle, lblDesc });
            flpBanBia.Controls.Add(pnlEmpty);
        }

        #endregion

        #region Helper Methods

        private Color GetCardBackColor(string status)
        {
            return status switch
            {
                "Trống" => Color.FromArgb(240, 253, 244),
                "Đang chơi" => Color.FromArgb(254, 242, 242),
                "Đã đặt" => Color.FromArgb(255, 251, 235)
            };
        }

        private Color GetStatusColor(string status)
        {
            return status switch
            {
                "Trống" => Color.FromArgb(34, 197, 94),
                "Đang chơi" => Color.FromArgb(220, 38, 38),
                "Đã đặt" => Color.FromArgb(234, 179, 8),
                _ => Color.Gray
            };
        }

        #endregion

        #region Event Handlers - Filters

        private void FilterButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            _currentAreaFilter = button.Tag.ToString();
            UpdateFilterButtons(pnlKhuVucFilters, button);
            RenderFilteredTables();
        }

        private void StatusFilterButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            _currentStatusFilter = button.Tag.ToString();
            UpdateFilterButtons(pnlTrangThaiFilters, button);
            RenderFilteredTables();
        }

        private void TypeFilterButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            _currentTypeFilter = button.Tag.ToString();
            UpdateFilterButtons(pnlLoaiBanFilters, button);
            RenderFilteredTables();
        }

        private void UpdateFilterButtons(Panel filterPanel, Button activeButton)
        {
            foreach (Control ctrl in filterPanel.Controls)
            {
                if (ctrl is Button btn)
                {
                    if (btn == activeButton)
                    {
                        btn.BackColor = Color.FromArgb(99, 102, 241);
                        btn.ForeColor = Color.White;
                    }
                    else
                    {
                        btn.BackColor = Color.FromArgb(226, 232, 240);
                        btn.ForeColor = Color.FromArgb(51, 65, 85);
                    }
                }
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            RenderFilteredTables();
        }

        #endregion

        #region Event Handlers - Toolbar

        private void BtnXemSoDo_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Chức năng 'Xem sơ đồ bàn' đang trong quá trình phát triển.\nVui lòng quay lại sau!",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void BtnXemBanDat_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Chức năng 'Xem bàn đặt' đang trong quá trình phát triển.\nVui lòng quay lại sau!",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void BtnDatBan_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Chức năng 'Đặt bàn trước' đang trong quá trình phát triển.\nVui lòng quay lại sau!",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void BtnThemBan_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Chức năng 'Thêm bàn mới' đang trong quá trình phát triển.\nVui lòng quay lại sau!",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        #endregion

        #region Event Handlers - Table Actions

        private async Task EditTable(BanBium ban)
        {
            try
            {
                var loaiBanService = Program.GetService<LoaiBanService>();
                var khuVucService = Program.GetService<KhuVucService>();

                using (var chinhSuaForm = new ChinhSuaBanForm(_banBiaService, loaiBanService, khuVucService, ban))
                {
                    var result = chinhSuaForm.ShowDialog(this);

                    if (result == DialogResult.OK)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        await RefreshTables();
                        this.Cursor = Cursors.Default;

                        MessageBox.Show("Đã cập nhật bàn thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Lỗi khi mở form chỉnh sửa: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Form Lifecycle

        private void SetupPermissions()
        {
            if (_mainForm == null) return;

            var chucVu = _mainForm.ChucVu;
            bool isAdmin = chucVu == "Admin";
            bool isQuanLy = chucVu == "Quản lý" || isAdmin;
            bool isThuNgan = chucVu == "Thu ngân" || isQuanLy;

            btnXemSoDo.Visible = true;
            btnXemBanDat.Visible = isThuNgan;
            btnDatBan.Visible = isThuNgan;
            btnThemBan.Visible = isQuanLy;
        }

        public void SetMainForm(MainForm mainForm)
        {
            _mainForm = mainForm;
            SetupPermissions();
        }

        private async void QLBanForm_Load(object sender, EventArgs e)
        {
            try
            {
                _refreshTimer.Start();
                await LoadTables();
                this.PerformLayout();
                this.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _refreshTimer?.Stop();
            _refreshTimer?.Dispose();
            _chiTietControl?.Dispose();
            base.OnFormClosing(e);
        }

        #endregion
    }
}