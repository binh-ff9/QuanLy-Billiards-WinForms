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

            var detailPanel = new Panel
            {
                AutoScroll = true,
                Width = 450,
                Padding = new Padding(20),
                BackColor = Color.White
            };

            int yPos = 10;

            // ===== HEADER: TÊN BÀN VỚI ICON EDIT VÀ XÓA =====
            var pnlHeader = new Panel
            {
                Location = new Point(0, yPos),
                Size = new Size(410, 50),
                BackColor = Color.Transparent
            };

            var lblTableName = new Label
            {
                Text = ban.TenBan,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(0, 10),
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblTableName);

            // Icon chỉnh sửa
            var btnEdit = new Button
            {
                Text = "✏️",
                Font = new Font("Segoe UI", 16F),
                Size = new Size(45, 45),
                Location = new Point(320, 5),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Click += async (s, e) => await ChinhSuaBan(ban);
            pnlHeader.Controls.Add(btnEdit);

            // Icon xóa
            var btnDelete = new Button
            {
                Text = "🗑️",
                Font = new Font("Segoe UI", 16F),
                Size = new Size(45, 45),
                Location = new Point(365, 5),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += async (s, e) => await XoaBan(ban);
            pnlHeader.Controls.Add(btnDelete);

            detailPanel.Controls.Add(pnlHeader);
            yPos += 60;

            // ===== THÔNG TIN KHÁCH HÀNG =====
            var pnlCustomer = new Panel
            {
                Location = new Point(0, yPos),
                Size = new Size(410, 200),
                BackColor = Color.FromArgb(248, 250, 252),
                Padding = new Padding(15)
            };

            int customerYPos = 15;

            AddDetailRow(pnlCustomer, "Khách hàng:", ban.MaKhNavigation?.TenKh ?? "Khách lẻ", ref customerYPos);
            AddDetailRow(pnlCustomer, "SĐT:", ban.MaKhNavigation?.Sdt ?? "-", ref customerYPos);

            if (ban.TrangThai == "Đang chơi" && ban.GioBatDau.HasValue)
            {
                AddDetailRow(pnlCustomer, "Bắt đầu:", ban.GioBatDau.Value.ToString("HH:mm dd/MM/yyyy"), ref customerYPos);

                var duration = DateTime.Now - ban.GioBatDau.Value;
                var thoiGian = $"{(int)duration.TotalMinutes} giờ {duration.Minutes} phút";
                AddDetailRow(pnlCustomer, "Thời gian:", thoiGian, ref customerYPos);
            }

            var giaGio = ban.MaLoaiNavigation?.GiaGio.ToString("N0") ?? "0";
            AddDetailRow(pnlCustomer, "Giá giờ:", $"{giaGio} đ/giờ", ref customerYPos);

            detailPanel.Controls.Add(pnlCustomer);
            yPos += 220;

            // ===== THÔNG TIN THANH TOÁN =====
            if (ban.TrangThai == "Đang chơi" && ban.GioBatDau.HasValue)
            {
                var pnlPayment = new Panel
                {
                    Location = new Point(0, yPos),
                    Size = new Size(410, 200),
                    BackColor = Color.White,
                    Padding = new Padding(15),
                    BorderStyle = BorderStyle.FixedSingle
                };

                int paymentYPos = 15;

                // Tính tiền bàn
                var duration = DateTime.Now - ban.GioBatDau.Value;
                var soGio = (decimal)duration.TotalMinutes / 60;
                var giaGioDecimal = ban.MaLoaiNavigation?.GiaGio ?? 0;
                var tienBan = Math.Ceiling((soGio * giaGioDecimal) / 1000) * 1000;

                decimal tienDichVu = 0;
                var hoaDon = await _banBiaService.GetActiveInvoiceAsync(ban.MaBan);

                if (hoaDon != null)
                {
                    tienDichVu = hoaDon.TienDichVu ?? 0;
                }

                var tamTinh = tienBan + tienDichVu;
                var tongCong = Math.Ceiling(tamTinh / 1000) * 1000;

                AddPaymentRow(pnlPayment, "Tiền bàn:", $"{tienBan:N0} đ", ref paymentYPos);
                AddPaymentRow(pnlPayment, "Dịch vụ:", $"{tienDichVu:N0} đ", ref paymentYPos);
                AddPaymentRow(pnlPayment, "Tạm tính:", $"{tamTinh:N0} đ", ref paymentYPos);

                paymentYPos += 10;
                var separator = new Panel
                {
                    Location = new Point(0, paymentYPos),
                    Size = new Size(380, 2),
                    BackColor = Color.FromArgb(226, 232, 240)
                };
                pnlPayment.Controls.Add(separator);
                paymentYPos += 15;

                AddTotalRow(pnlPayment, "Tổng cộng:", $"{tongCong:N0} đ", ref paymentYPos);

                detailPanel.Controls.Add(pnlPayment);
                yPos += 220;
            }

            // ===== DỊCH VỤ ĐÃ ORDER =====
            if (ban.TrangThai == "Đang chơi")
            {
                var hoaDon = await _banBiaService.GetActiveInvoiceAsync(ban.MaBan);

                if (hoaDon != null)
                {
                    var chiTietList = await _banBiaService.GetInvoiceDetailsAsync(hoaDon.MaHd);

                    if (chiTietList.Any())
                    {
                        var lblDichVu = new Label
                        {
                            Text = $"Dịch vụ đã order ({chiTietList.Count} món):",
                            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                            ForeColor = Color.FromArgb(30, 41, 59),
                            Location = new Point(0, yPos),
                            AutoSize = true
                        };
                        detailPanel.Controls.Add(lblDichVu);
                        yPos += 35;

                        foreach (var item in chiTietList)
                        {
                            var pnlService = CreateServiceItem(item, ban.MaBan);
                            pnlService.Location = new Point(0, yPos);
                            detailPanel.Controls.Add(pnlService);
                            yPos += 75;
                        }
                    }
                }
            }

            // ===== BUTTONS =====
            yPos += 20;

            if (ban.TrangThai == "Trống")
            {
                var btnBatDau = CreateActionButton("▶️ Bắt đầu chơi", Color.FromArgb(34, 197, 94));
                btnBatDau.Location = new Point(0, yPos);
                btnBatDau.Click += async (s, e) => await BatDauChoiBan(ban);
                detailPanel.Controls.Add(btnBatDau);
                yPos += 55;
            }
            else if (ban.TrangThai == "Đang chơi")
            {
                var btnThemDV = CreateActionButton("➕ Thêm dịch vụ", Color.FromArgb(99, 102, 241));
                btnThemDV.Location = new Point(0, yPos);
                btnThemDV.Click += (s, e) => ThemDichVu(ban);
                detailPanel.Controls.Add(btnThemDV);
                yPos += 55;

                var btnThanhToan = CreateActionButton("💰 Kết thúc & Thanh toán", Color.FromArgb(34, 197, 94));
                btnThanhToan.Location = new Point(0, yPos);
                btnThanhToan.Click += (s, e) => ThanhToanBan(ban);
                detailPanel.Controls.Add(btnThanhToan);
            }

            _mainForm.UpdateDetailPanel("Chi tiết", detailPanel);
        }
        private void AddDetailRow(Panel panel, string label, string value, ref int yPos)
        {
            var lblLabel = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(0, yPos),
                AutoSize = true
            };
            panel.Controls.Add(lblLabel);

            var lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(230, yPos),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleRight
            };
            panel.Controls.Add(lblValue);

            yPos += 35;
        }

        private void AddPaymentRow(Panel panel, string label, string value, ref int yPos)
        {
            var lblLabel = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(0, yPos),
                AutoSize = true
            };
            panel.Controls.Add(lblLabel);

            var lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(230, yPos),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleRight
            };
            panel.Controls.Add(lblValue);

            yPos += 30;
        }

        private void AddTotalRow(Panel panel, string label, string value, ref int yPos)
        {
            var lblLabel = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(0, yPos),
                AutoSize = true
            };
            panel.Controls.Add(lblLabel);

            var lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(239, 68, 68),
                Location = new Point(230, yPos),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleRight
            };
            panel.Controls.Add(lblValue);

            yPos += 35;
        }

        private Panel CreateServiceItem(ChiTietHoaDon item, int maBan)
        {
            var panel = new Panel
            {
                Size = new Size(410, 65),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblName = new Label
            {
                Text = item.MaDvNavigation?.TenDv ?? "Dịch vụ",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(15, 10),
                AutoSize = true
            };
            panel.Controls.Add(lblName);

            var lblQuantity = new Label
            {
                Text = $"{item.SoLuong} x {item.MaDvNavigation?.Gia:N0} đ",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(15, 35),
                AutoSize = true
            };
            panel.Controls.Add(lblQuantity);

            var lblPrice = new Label
            {
                Text = $"{item.ThanhTien:N0} đ",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(239, 68, 68),
                Location = new Point(250, 20),
                AutoSize = true
            };
            panel.Controls.Add(lblPrice);

            var btnDelete = new Button
            {
                Text = "✕",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                Size = new Size(35, 35),
                Location = new Point(360, 15),
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(239, 68, 68),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Tag = new { ChiTietId = item.Id, MaBan = maBan }
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += async (s, e) => await XoaDichVu(item.Id, maBan);
            panel.Controls.Add(btnDelete);

            return panel;
        }

        private async Task XoaDichVu(int chiTietId, int maBan)
        {
            var result = MessageBox.Show("Bạn có chắc muốn xóa dịch vụ này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Sử dụng HoaDonService để xóa dịch vụ
                    var hoaDonService = Program.GetService<HoaDonService>();
                    var success = await hoaDonService.RemoveServiceFromInvoiceAsync(chiTietId);

                    if (success)
                    {
                        MessageBox.Show("Đã xóa dịch vụ thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Reload detail panel
                        var ban = await _banBiaService.GetTableByIdAsync(maBan);
                        if (ban != null)
                        {
                            ShowTableDetail(ban);
                        }

                        await LoadBanBia();
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa dịch vụ này!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa dịch vụ: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async Task XoaBan(BanBium ban)
        {
            if (ban.TrangThai != "Trống")
            {
                MessageBox.Show("Chỉ có thể xóa bàn đang trống!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"Bạn có chắc muốn xóa {ban.TenBan}?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                var success = await _banBiaService.DeleteTableAsync(ban.MaBan);
                if (success)
                {
                    MessageBox.Show("Đã xóa bàn thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _mainForm.HideDetailPanel();
                    await LoadBanBia();
                }
                else
                {
                    MessageBox.Show("Không thể xóa bàn này!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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