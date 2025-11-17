using Billiard.BLL.Services;
using Billiard.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.QLBan
{
    public partial class QLBanForm : Form
    {
        private readonly BanBiaService _banBiaService;
        private MainForm _mainForm;
        private List<BanBium> _allTables;
        private string _currentAreaFilter = "all";
        private string _currentStatusFilter = "all";
        private string _currentTypeFilter = "all";
        private System.Windows.Forms.Timer _refreshTimer;

        public QLBanForm(BanBiaService banBiaService)
        {
            _banBiaService = banBiaService;
            InitializeComponent();
            InitializeRefreshTimer();

            // Đảm bảo form hiển thị đúng
            this.AutoScroll = false;
            this.AutoSize = false;
        }

        private void InitializeRefreshTimer()
        {
            _refreshTimer = new System.Windows.Forms.Timer();
            _refreshTimer.Interval = 30000; // 30 seconds
            _refreshTimer.Tick += async (s, e) => await LoadBanBia();
        }

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
                // Start timer
                _refreshTimer.Start();

                // Load data
                await LoadBanBia();

                // Force layout update
                this.PerformLayout();
                this.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadBanBia()
        {
            try
            {
                // Show loading cursor
                this.Cursor = Cursors.WaitCursor;

                flpBanBia.Controls.Clear();
                flpBanBia.SuspendLayout(); // Tạm dừng layout để tăng hiệu suất

                _allTables = await _banBiaService.GetAllTablesAsync();
                ApplyFilters();

                flpBanBia.ResumeLayout(); // Tiếp tục layout
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Lỗi tải danh sách bàn: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilters()
        {
            flpBanBia.SuspendLayout();
            flpBanBia.Controls.Clear();

            var filteredTables = _allTables.AsEnumerable();

            // Apply filters
            if (_currentAreaFilter != "all")
            {
                filteredTables = filteredTables.Where(b =>
                    b.MaKhuVucNavigation?.TenKhuVuc == _currentAreaFilter);
            }

            if (_currentStatusFilter != "all")
            {
                filteredTables = filteredTables.Where(b => b.TrangThai == _currentStatusFilter);
            }

            if (_currentTypeFilter != "all")
            {
                filteredTables = filteredTables.Where(b =>
                    b.MaLoaiNavigation?.TenLoai == _currentTypeFilter);
            }

            var searchText = txtSearch.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(searchText))
            {
                filteredTables = filteredTables.Where(b =>
                    b.TenBan.ToLower().Contains(searchText));
            }

            var tables = filteredTables.ToList();

            if (tables.Count == 0)
            {
                ShowEmptyState();
            }
            else
            {
                foreach (var ban in tables)
                {
                    var card = CreateTableCard(ban);
                    flpBanBia.Controls.Add(card);
                }
            }

            flpBanBia.ResumeLayout();
            flpBanBia.PerformLayout();
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
            lblIcon.Location = new Point(
                (pnlEmpty.Width - lblIcon.Width) / 2,
                80
            );

            var lblTitle = new Label
            {
                Text = "Không tìm thấy bàn nào",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                AutoSize = true
            };
            lblTitle.Location = new Point(
                (pnlEmpty.Width - lblTitle.Width) / 2,
                160
            );

            var lblDesc = new Label
            {
                Text = "Thử thay đổi bộ lọc hoặc tìm kiếm khác",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.Gray,
                AutoSize = true
            };
            lblDesc.Location = new Point(
                (pnlEmpty.Width - lblDesc.Width) / 2,
                195
            );

            pnlEmpty.Controls.AddRange(new Control[] { lblIcon, lblTitle, lblDesc });
            flpBanBia.Controls.Add(pnlEmpty);
        }

        private Panel CreateTableCard(BanBium ban)
        {
            var card = new Panel
            {
                Width = 220,
                Height = 280,
                Margin = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand,
                Tag = ban
            };

            // Background color based on status
            card.BackColor = ban.TrangThai switch
            {
                "Trống" => Color.FromArgb(240, 253, 244),
                "Đang chơi" => Color.FromArgb(254, 242, 242),
                "Đã đặt" => Color.FromArgb(255, 251, 235),
                _ => Color.White
            };

            // Border color based on status
            card.Paint += (s, e) =>
            {
                var borderColor = ban.TrangThai switch
                {
                    "Trống" => Color.FromArgb(34, 197, 94),
                    "Đang chơi" => Color.FromArgb(239, 68, 68),
                    "Đã đặt" => Color.FromArgb(234, 179, 8),
                    _ => Color.Gray
                };

                using (var pen = new Pen(borderColor, 3))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                }
            };

            // Table image panel
            var pnlImage = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(220, 140),
                BackColor = Color.FromArgb(248, 250, 252)
            };

            var lblIcon = new Label
            {
                Text = "🎱",
                Font = new Font("Segoe UI", 56F),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(220, 140)
            };
            pnlImage.Controls.Add(lblIcon);

            // Status badge
            var lblStatus = new Label
            {
                Text = ban.TrangThai,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ban.TrangThai switch
                {
                    "Trống" => Color.FromArgb(34, 197, 94),
                    "Đang chơi" => Color.FromArgb(239, 68, 68),
                    "Đã đặt" => Color.FromArgb(234, 179, 8),
                    _ => Color.Gray
                },
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(85, 28),
                Location = new Point(10, 10)
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
                    Location = new Point(135, 10)
                };
                pnlImage.Controls.Add(lblVIP);
            }

            // Table name
            var lblName = new Label
            {
                Text = ban.TenBan,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 150),
                Size = new Size(220, 32)
            };

            // Table info
            var lblInfo = new Label
            {
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 185),
                Size = new Size(220, 22)
            };

            if (ban.TrangThai == "Đang chơi" && ban.GioBatDau.HasValue)
            {
                var duration = DateTime.Now - ban.GioBatDau.Value;
                lblInfo.Text = $"⏱️ {(int)duration.TotalHours}h {duration.Minutes}m";
                lblInfo.ForeColor = Color.FromArgb(239, 68, 68);
                lblInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

                var lblCustomer = new Label
                {
                    Text = $"👤 {ban.MaKhNavigation?.TenKh ?? "Khách lẻ"}",
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = Color.FromArgb(71, 85, 105),
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(0, 210),
                    Size = new Size(220, 22)
                };
                card.Controls.Add(lblCustomer);
            }
            else if (ban.TrangThai == "Đã đặt")
            {
                lblInfo.Text = $"👤 {ban.MaKhNavigation?.TenKh ?? "Khách đặt"}";
            }
            else
            {
                lblInfo.Text = $"📍 {ban.MaKhuVucNavigation?.TenKhuVuc ?? "Khu vực"}";
            }

            // Price
            var lblPrice = new Label
            {
                Text = $"{ban.MaLoaiNavigation?.GiaGio:N0} đ/giờ",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(99, 102, 241),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 235),
                Size = new Size(220, 28)
            };

            card.Controls.AddRange(new Control[] { pnlImage, lblName, lblInfo, lblPrice });

            // Click event - gán cho tất cả controls
            EventHandler clickHandler = (s, e) => ShowTableDetail(ban);
            card.Click += clickHandler;

            foreach (Control ctrl in card.Controls)
            {
                ctrl.Click += clickHandler;
                if (ctrl.HasChildren)
                {
                    foreach (Control subCtrl in ctrl.Controls)
                    {
                        subCtrl.Click += clickHandler;
                    }
                }
            }

            // Hover effect
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
                card.BackColor = ban.TrangThai switch
                {
                    "Trống" => Color.FromArgb(240, 253, 244),
                    "Đang chơi" => Color.FromArgb(254, 242, 242),
                    "Đã đặt" => Color.FromArgb(255, 251, 235),
                    _ => Color.White
                };
            };

            return card;
        }

        private void ShowTableDetail(BanBium ban)
        {
            if (_mainForm == null) return;

            var detailPanel = new Panel
            {
                AutoScroll = true,
                Width = 270,
                Padding = new Padding(10)
            };

            int yPos = 10;

            // Title
            var lblTitle = new Label
            {
                Text = ban.TenBan,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, yPos),
                ForeColor = Color.FromArgb(30, 41, 59)
            };
            detailPanel.Controls.Add(lblTitle);
            yPos += 40;

            // Status Badge
            var pnlStatus = new Panel
            {
                Location = new Point(10, yPos),
                Size = new Size(250, 35),
                BackColor = ban.TrangThai switch
                {
                    "Trống" => Color.FromArgb(220, 252, 231),
                    "Đang chơi" => Color.FromArgb(254, 226, 226),
                    "Đã đặt" => Color.FromArgb(254, 243, 199),
                    _ => Color.LightGray
                }
            };

            var lblStatusBadge = new Label
            {
                Text = ban.TrangThai,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = ban.TrangThai switch
                {
                    "Trống" => Color.FromArgb(21, 128, 61),
                    "Đang chơi" => Color.FromArgb(153, 27, 27),
                    "Đã đặt" => Color.FromArgb(146, 64, 14),
                    _ => Color.Gray
                },
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 0),
                Size = new Size(250, 35)
            };
            pnlStatus.Controls.Add(lblStatusBadge);
            detailPanel.Controls.Add(pnlStatus);
            yPos += 50;

            // Info section
            AddDetailLabel(detailPanel, $"📍 Khu vực: {ban.MaKhuVucNavigation?.TenKhuVuc}", ref yPos);
            AddDetailLabel(detailPanel, $"🎯 Loại bàn: {ban.MaLoaiNavigation?.TenLoai}", ref yPos);
            AddDetailLabel(detailPanel, $"💰 Giá: {ban.MaLoaiNavigation?.GiaGio:N0} đ/giờ", ref yPos);

            if (ban.TrangThai == "Đang chơi" && ban.GioBatDau.HasValue)
            {
                var duration = DateTime.Now - ban.GioBatDau.Value;
                AddDetailLabel(detailPanel, $"⏰ Bắt đầu: {ban.GioBatDau:HH:mm}", ref yPos);
                AddDetailLabel(detailPanel, $"⏱️ Thời gian: {(int)duration.TotalHours}h {duration.Minutes}m", ref yPos);
                AddDetailLabel(detailPanel, $"👤 Khách: {ban.MaKhNavigation?.TenKh ?? "Khách lẻ"}", ref yPos);
            }

            yPos += 15;

            // Action buttons
            var btnPanel = new FlowLayoutPanel
            {
                Location = new Point(10, yPos),
                Width = 250,
                Height = 250,
                FlowDirection = FlowDirection.TopDown
            };

            if (ban.TrangThai == "Trống")
            {
                var btnBatDau = CreateActionButton("▶️ Bắt đầu chơi", Color.FromArgb(34, 197, 94));
                btnBatDau.Click += async (s, e) => await BatDauChoiBan(ban);
                btnPanel.Controls.Add(btnBatDau);
            }
            else if (ban.TrangThai == "Đang chơi")
            {
                var btnThanhToan = CreateActionButton("💳 Thanh toán", Color.FromArgb(59, 130, 246));
                btnThanhToan.Click += (s, e) => ThanhToanBan(ban);
                btnPanel.Controls.Add(btnThanhToan);

                var btnThemDV = CreateActionButton("🍴 Thêm dịch vụ", Color.FromArgb(168, 85, 247));
                btnThemDV.Click += (s, e) => ThemDichVu(ban);
                btnPanel.Controls.Add(btnThemDV);

                var btnDungChoi = CreateActionButton("⏸️ Tạm dừng", Color.FromArgb(234, 179, 8));
                btnDungChoi.Click += async (s, e) => await TamDungBan(ban);
                btnPanel.Controls.Add(btnDungChoi);
            }

            detailPanel.Controls.Add(btnPanel);
            _mainForm.UpdateDetailPanel($"Chi tiết - {ban.TenBan}", detailPanel);
        }

        private void AddDetailLabel(Panel panel, string text, ref int yPos)
        {
            var lbl = new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                Location = new Point(10, yPos),
                MaximumSize = new Size(250, 0),
                ForeColor = Color.FromArgb(71, 85, 105)
            };
            panel.Controls.Add(lbl);
            yPos += 28;
        }

        private Button CreateActionButton(string text, Color backColor)
        {
            var btn = new Button
            {
                Text = text,
                Width = 240,
                Height = 45,
                FlatStyle = FlatStyle.Flat,
                BackColor = backColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 5, 0, 5)
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private async Task BatDauChoiBan(BanBium ban)
        {
            try
            {
                var result = await _banBiaService.StartTableAsync(ban.MaBan, _mainForm.MaNV);

                if (result)
                {
                    MessageBox.Show($"Đã bắt đầu chơi bàn {ban.TenBan}", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadBanBia();
                }
                else
                {
                    MessageBox.Show("Không thể bắt đầu chơi bàn này!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ThanhToanBan(BanBium ban)
        {
            MessageBox.Show($"Chức năng thanh toán bàn {ban.TenBan} đang được phát triển", "Thông báo");
        }

        private void ThemDichVu(BanBium ban)
        {
            MessageBox.Show($"Chức năng thêm dịch vụ cho bàn {ban.TenBan} đang được phát triển", "Thông báo");
        }

        private async Task TamDungBan(BanBium ban)
        {
            var result = MessageBox.Show($"Tạm dừng bàn {ban.TenBan}?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                var success = await _banBiaService.PauseTableAsync(ban.MaBan);
                if (success)
                {
                    MessageBox.Show("Đã tạm dừng bàn", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadBanBia();
                }
                else
                {
                    MessageBox.Show("Không thể tạm dừng bàn!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #region Filter Events

        private void FilterButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            _currentAreaFilter = button.Tag.ToString();

            foreach (Control ctrl in pnlKhuVucFilters.Controls)
            {
                if (ctrl is Button btn)
                {
                    if (btn == button)
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

            ApplyFilters();
        }

        private void StatusFilterButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            _currentStatusFilter = button.Tag.ToString();

            foreach (Control ctrl in pnlTrangThaiFilters.Controls)
            {
                if (ctrl is Button btn)
                {
                    if (btn == button)
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

            ApplyFilters();
        }

        private void TypeFilterButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            _currentTypeFilter = button.Tag.ToString();

            foreach (Control ctrl in pnlLoaiBanFilters.Controls)
            {
                if (ctrl is Button btn)
                {
                    if (btn == button)
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

            ApplyFilters();
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        #endregion

        #region Toolbar Button Events

        private void BtnXemSoDo_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng xem sơ đồ bàn đang được phát triển", "Thông báo");
        }

        private void BtnXemBanDat_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng xem danh sách bàn đặt đang được phát triển", "Thông báo");
        }

        private void BtnDatBan_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng đặt bàn trước đang được phát triển", "Thông báo");
        }

        private void BtnThemBan_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng thêm bàn mới đang được phát triển", "Thông báo");
        }

        #endregion

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _refreshTimer?.Stop();
            _refreshTimer?.Dispose();
            base.OnFormClosing(e);
        }
    }
}