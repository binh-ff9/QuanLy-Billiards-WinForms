using Billiard.BLL.Services.QLBan;
using Billiard.BLL.Services.HoaDonServices;
using Billiard.DAL.Entities;
using Microsoft.EntityFrameworkCore;
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
        private bool _isRefreshing = false;
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
            _refreshTimer.Tick += async (s, e) =>
            {
                if (_isRefreshing) return;

                _isRefreshing = true;
                try
                {
                    await LoadBanBia();
                }
                finally
                {
                    _isRefreshing = false;
                }
            };
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

        // Thêm method này vào QLBanForm.cs để load hình ảnh bàn
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

            // Load image
            if (!string.IsNullOrEmpty(ban.HinhAnh))
            {
                try
                {
                    var projectRoot = Directory.GetParent(Directory.GetParent(Directory.GetParent(
                        Directory.GetParent(Application.StartupPath).FullName).FullName).FullName).FullName;
                    var imagePath = Path.Combine(projectRoot, "Forms", "Resources", "img", "tables", ban.HinhAnh);

                    if (File.Exists(imagePath))
                    {
                        var picTable = new PictureBox
                        {
                            Size = new Size(220, 140),
                            Location = new Point(0, 0),
                            SizeMode = PictureBoxSizeMode.Zoom,
                            BackColor = Color.FromArgb(248, 250, 252)
                        };

                        using (var img = Image.FromFile(imagePath))
                        {
                            picTable.Image = new Bitmap(img);
                        }

                        pnlImage.Controls.Add(picTable);
                    }
                    else
                    {
                        AddDefaultTableIcon(pnlImage);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error loading table image: {ex.Message}");
                    AddDefaultTableIcon(pnlImage);
                }
            }
            else
            {
                AddDefaultTableIcon(pnlImage);
            }

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

            // ===== NÚT CHỈNH SỬA =====
            var btnEdit = new Button
            {
                Text = "✏️",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Size = new Size(35, 35),
                Location = new Point(175, 100),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Tag = ban,
                TabStop = false // Tránh focus khi tab
            };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);

            // Event handler cho nút chỉnh sửa
            btnEdit.Click += async (s, e) =>
            {
                await ChinhSuaBan(ban);
            };

            // Hover effect cho nút edit
            btnEdit.MouseEnter += (s, e) =>
            {
                btnEdit.BackColor = Color.FromArgb(37, 99, 235);
            };

            btnEdit.MouseLeave += (s, e) =>
            {
                btnEdit.BackColor = Color.FromArgb(59, 130, 246);
            };

            pnlImage.Controls.Add(btnEdit);
            btnEdit.BringToFront(); // Đảm bảo nút ở trên cùng

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

            // Click event - gán cho tất cả controls NGOẠI TRỪ nút edit
            EventHandler clickHandler = (s, e) => ShowTableDetail(ban);
            card.Click += clickHandler;

            foreach (Control ctrl in card.Controls)
            {
                if (ctrl == pnlImage)
                {
                    // Với pnlImage, gán cho các control con trừ btnEdit
                    foreach (Control subCtrl in ctrl.Controls)
                    {
                        if (subCtrl != btnEdit)
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

            // Hover effect cho card
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

        // Method xử lý chỉnh sửa bàn
        private async Task ChinhSuaBan(BanBium ban)
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
                        await LoadBanBia();
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
        // Helper method để thêm icon mặc định
        private void AddDefaultTableIcon(Panel pnlImage)
        {
            var lblIcon = new Label
            {
                Text = "🎱",
                Font = new Font("Segoe UI", 56F),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(220, 140),
                BackColor = Color.Transparent
            };
            pnlImage.Controls.Add(lblIcon);
        }
        private async void ShowTableDetail(BanBium ban)
        {
            if (_mainForm == null) return;

            try
            {
                // Lấy HoaDonService từ DI container
                var hoaDonService = Program.GetService<HoaDonService>();

                // Tạo BanChiTietControl (UserControl thay vì Form)
                var chiTietControl = new BanChiTietControl(_banBiaService, hoaDonService, ban, _mainForm.MaNV);

                // Đăng ký event để reload data khi có thay đổi
                chiTietControl.OnDataChanged += async (s, e) =>
                {
                    await LoadBanBia();
                };

                // Hiển thị control trong detail panel
                _mainForm.UpdateDetailPanel($"Chi tiết {ban.TenBan}", chiTietControl);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị chi tiết bàn: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            try
            {
                using (var soDoBanForm = new SoDoBanForm(_banBiaService))
                {
                    soDoBanForm.SetMainForm(_mainForm);
                    soDoBanForm.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở sơ đồ bàn: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnXemBanDat_Click(object sender, EventArgs e)
        {
            try
            {
                if (!btnXemBanDat.Visible)
                {
                    MessageBox.Show("Bạn không có quyền truy cập chức năng này.", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var datBanService = Program.GetService<DatBanService>();

                using (var datBanForm = new DanhSachBanDatForm(datBanService, _banBiaService, _mainForm))
                {
                    datBanForm.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở form Danh sách bàn đặt: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Trong QLBanForm.cs

        private async void BtnDatBan_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra quyền (đã được SetupPermissions xử lý, nhưng nên kiểm tra lại nếu cần)
                if (!btnDatBan.Visible)
                {
                    MessageBox.Show("Bạn không có quyền thực hiện chức năng này.", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy các Service cần thiết từ Program (cần có lớp Program với GetService<T>)
                var datBanService = Program.GetService<DatBanService>();

                // Khởi tạo và hiển thị DatBanForm
                using (var datBanForm = new DatBanForm(_banBiaService, datBanService))
                {
                    var result = datBanForm.ShowDialog(this);

                    if (result == DialogResult.OK)
                    {
                        // Nếu việc đặt bàn thành công, tải lại danh sách bàn để cập nhật trạng thái
                        this.Cursor = Cursors.WaitCursor;
                        await LoadBanBia();
                        this.Cursor = Cursors.Default;

                        MessageBox.Show("Đã đặt bàn thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Lỗi khi mở form đặt bàn: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnThemBan_Click(object sender, EventArgs e)
        {
            try
            {
                var loaiBanService = Program.GetService<LoaiBanService>();
                var khuVucService = Program.GetService<KhuVucService>();

                using (var themBanForm = new ThemBanForm(_banBiaService, loaiBanService, khuVucService))
                {
                    var result = themBanForm.ShowDialog(this);

                    if (result == DialogResult.OK)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        await LoadBanBia();
                        this.Cursor = Cursors.Default;

                        MessageBox.Show("Đã thêm bàn mới thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Lỗi khi mở form thêm bàn: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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