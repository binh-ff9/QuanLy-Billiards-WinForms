using Billiard.DAL.Data;
using Billiard.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.Menu
{
    public partial class QLBanForm : Form
    {
        private readonly BilliardDbContext _context;
        private readonly MainForm _mainForm;
        private List<BanBium> _allTables;
        private string _currentAreaFilter = "all";
        private string _currentStatusFilter = "all";
        private string _currentTypeFilter = "all";

        public QLBanForm(BilliardDbContext context, MainForm mainForm)
        {
            _context = context;
            _mainForm = mainForm;
            InitializeComponent();
            SetupPermissions();
        }

        private void SetupPermissions()
        {
            // Kiểm tra quyền và ẩn/hiện các button
            var chucVu = _mainForm.ChucVu;
            bool isAdmin = chucVu == "Admin";
            bool isQuanLy = chucVu == "Quản lý" || isAdmin;
            bool isThuNgan = chucVu == "Thu ngân" || isQuanLy;

            // Tất cả đều thấy nút xem sơ đồ
            btnXemSoDo.Visible = true;

            // Thu ngân trở lên mới thấy đặt bàn
            btnXemBanDat.Visible = isThuNgan;
            btnDatBan.Visible = isThuNgan;

            // Quản lý trở lên mới thấy thêm bàn
            btnThemBan.Visible = isQuanLy;
        }

        private async void QuanLyBanForm_Load(object sender, EventArgs e)
        {
            try
            {
                await LoadBanBia();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async System.Threading.Tasks.Task LoadBanBia()
        {
            try
            {
                flpBanBia.Controls.Clear();

                // Load tất cả bàn từ database
                _allTables = await _context.BanBia
                    .Include(b => b.MaKhuVucNavigation)
                    .Include(b => b.MaLoaiNavigation)
                    .Include(b => b.MaKhNavigation)
                    .Where(b => b.TrangThai != "Bảo trì")
                    .OrderBy(b => b.TenBan)
                    .ToListAsync();

                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách bàn: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilters()
        {
            flpBanBia.Controls.Clear();

            var filteredTables = _allTables.AsEnumerable();

            // Filter by area
            if (_currentAreaFilter != "all")
            {
                filteredTables = filteredTables.Where(b =>
                    b.MaKhuVucNavigation?.TenKhuVuc == _currentAreaFilter);
            }

            // Filter by status
            if (_currentStatusFilter != "all")
            {
                filteredTables = filteredTables.Where(b => b.TrangThai == _currentStatusFilter);
            }

            // Filter by type
            if (_currentTypeFilter != "all")
            {
                filteredTables = filteredTables.Where(b =>
                    b.MaLoaiNavigation?.TenLoai == _currentTypeFilter);
            }

            // Filter by search text
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
        }

        private void ShowEmptyState()
        {
            var pnlEmpty = new Panel
            {
                Size = new Size(800, 300),
                BackColor = Color.White
            };

            var lblIcon = new Label
            {
                Text = "🎱",
                Font = new Font("Segoe UI", 48F),
                AutoSize = true,
                Location = new Point(350, 80)
            };

            var lblTitle = new Label
            {
                Text = "Không tìm thấy bàn nào",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                AutoSize = true,
                Location = new Point(280, 160)
            };

            var lblDesc = new Label
            {
                Text = "Thử thay đổi bộ lọc hoặc tìm kiếm khác",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(280, 195)
            };

            pnlEmpty.Controls.AddRange(new Control[] { lblIcon, lblTitle, lblDesc });
            flpBanBia.Controls.Add(pnlEmpty);
        }

        private Panel CreateTableCard(BanBium ban)
        {
            var card = new Panel
            {
                Width = 200,
                Height = 240,
                Margin = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand,
                Tag = ban
            };

            // Background color based on status
            card.BackColor = ban.TrangThai switch
            {
                "Trống" => Color.FromArgb(220, 252, 231),
                "Đang chơi" => Color.FromArgb(254, 226, 226),
                "Đã đặt" => Color.FromArgb(254, 243, 199),
                _ => Color.White
            };

            // Table icon/image
            var pnlImage = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(200, 120),
                BackColor = Color.FromArgb(248, 250, 252)
            };

            var lblIcon = new Label
            {
                Text = "🎱",
                Font = new Font("Segoe UI", 48F),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(200, 120)
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
                Size = new Size(80, 25),
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
                    Size = new Size(70, 25),
                    Location = new Point(120, 10)
                };
                pnlImage.Controls.Add(lblVIP);
            }

            // Table name
            var lblName = new Label
            {
                Text = ban.TenBan,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 130),
                Size = new Size(200, 30)
            };

            // Table info
            var lblInfo = new Label
            {
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 165),
                Size = new Size(200, 20)
            };

            if (ban.TrangThai == "Đang chơi" && ban.GioBatDau.HasValue)
            {
                var duration = DateTime.Now - ban.GioBatDau.Value;
                lblInfo.Text = $"⏱️ {(int)duration.TotalHours}h {duration.Minutes}m";
                lblInfo.ForeColor = Color.FromArgb(239, 68, 68);
                lblInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

                // Customer name
                var lblCustomer = new Label
                {
                    Text = $"👤 {ban.MaKhNavigation?.TenKh ?? "Khách lẻ"}",
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = Color.FromArgb(71, 85, 105),
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(0, 190),
                    Size = new Size(200, 20)
                };
                card.Controls.Add(lblCustomer);
            }
            else if (ban.TrangThai == "Đã đặt")
            {
                lblInfo.Text = $"👤 {ban.MaKhNavigation?.TenKh ?? "Khách đặt"}";
            }
            else
            {
                lblInfo.Text = ban.MaLoaiNavigation?.TenLoai ?? "Không rõ";
            }

            // Price
            var lblPrice = new Label
            {
                Text = $"{ban.MaLoaiNavigation?.GiaGio:N0} đ/giờ",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(99, 102, 241),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 210),
                Size = new Size(200, 25)
            };

            card.Controls.AddRange(new Control[] { pnlImage, lblName, lblInfo, lblPrice });

            // Click event
            card.Click += (s, e) => ShowTableDetail(ban);
            foreach (Control ctrl in card.Controls)
            {
                ctrl.Click += (s, e) => ShowTableDetail(ban);
            }

            // Hover effect
            card.MouseEnter += (s, e) => card.BorderStyle = BorderStyle.Fixed3D;
            card.MouseLeave += (s, e) => card.BorderStyle = BorderStyle.FixedSingle;

            return card;
        }

        private void ShowTableDetail(BanBium ban)
        {
            var detailPanel = new Panel { AutoScroll = true, Width = 270 };

            // Title
            var lblTitle = new Label
            {
                Text = ban.TenBan,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(0, 0)
            };
            detailPanel.Controls.Add(lblTitle);

            // Status
            var lblStatus = new Label
            {
                Text = $"Trạng thái: {ban.TrangThai}",
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                Location = new Point(0, 35)
            };
            detailPanel.Controls.Add(lblStatus);

            // Info
            var yPos = 60;
            AddDetailLabel(detailPanel, $"Loại bàn: {ban.MaLoaiNavigation?.TenLoai}", ref yPos);
            AddDetailLabel(detailPanel, $"Khu vực: {ban.MaKhuVucNavigation?.TenKhuVuc}", ref yPos);
            AddDetailLabel(detailPanel, $"Giá: {ban.MaLoaiNavigation?.GiaGio:N0} đ/giờ", ref yPos);

            if (ban.TrangThai == "Đang chơi" && ban.GioBatDau.HasValue)
            {
                var duration = DateTime.Now - ban.GioBatDau.Value;
                AddDetailLabel(detailPanel, $"Bắt đầu: {ban.GioBatDau:HH:mm}", ref yPos);
                AddDetailLabel(detailPanel, $"Thời gian: {(int)duration.TotalHours}h {duration.Minutes}m", ref yPos);
                AddDetailLabel(detailPanel, $"Khách: {ban.MaKhNavigation?.TenKh ?? "Khách lẻ"}", ref yPos);
            }

            // Action buttons
            yPos += 20;
            var btnPanel = new FlowLayoutPanel
            {
                Location = new Point(0, yPos),
                Width = 270,
                Height = 200,
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
                Location = new Point(0, yPos)
            };
            panel.Controls.Add(lbl);
            yPos += 25;
        }

        private Button CreateActionButton(string text, Color backColor)
        {
            return new Button
            {
                Text = text,
                Width = 250,
                Height = 40,
                FlatStyle = FlatStyle.Flat,
                BackColor = backColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 5, 0, 5)
            };
        }

        private async System.Threading.Tasks.Task BatDauChoiBan(BanBium ban)
        {
            try
            {
                ban.TrangThai = "Đang chơi";
                ban.GioBatDau = DateTime.Now;

                var hoaDon = new HoaDon
                {
                    MaBan = ban.MaBan,
                    ThoiGianBatDau = DateTime.Now,
                    TrangThai = "Đang chơi",
                    MaNv = _mainForm.MaNV,
                    TienBan = 0,
                    TienDichVu = 0,
                    TongTien = 0
                };
                _context.HoaDons.Add(hoaDon);

                await _context.SaveChangesAsync();

                MessageBox.Show($"Đã bắt đầu chơi bàn {ban.TenBan}", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                await LoadBanBia();
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

        private async System.Threading.Tasks.Task TamDungBan(BanBium ban)
        {
            var result = MessageBox.Show($"Tạm dừng bàn {ban.TenBan}?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ban.TrangThai = "Trống";
                ban.GioBatDau = null;
                await _context.SaveChangesAsync();
                await LoadBanBia();
            }
        }

        #region Filter Events

        private void FilterButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            _currentAreaFilter = button.Tag.ToString();

            // Update button styles
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
    }
}