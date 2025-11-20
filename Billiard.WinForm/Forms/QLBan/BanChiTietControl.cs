using Billiard.BLL.Services;
using Billiard.BLL.Services.HoaDonServices;
using Billiard.BLL.Services.QLBan;
using Billiard.BLL.Services.VietQR;
using Billiard.DAL.Entities;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChiTietHoaDonEntity = Billiard.DAL.Entities.ChiTietHoaDon;
using HoaDonEntity = Billiard.DAL.Entities.HoaDon;

namespace Billiard.WinForm.Forms.QLBan
{
    public partial class BanChiTietControl : UserControl
    {
        private readonly BanBiaService _banBiaService;
        private readonly HoaDonService _hoaDonService;
        private BanBium _ban;
        private readonly int _maNV;
        private Panel pnlContent;
        private bool _isLoading = false;
        public event EventHandler OnDataChanged;

        public BanChiTietControl(BanBiaService banBiaService, HoaDonService hoaDonService, BanBium ban, int maNV)
        {
            _banBiaService = banBiaService;
            _hoaDonService = hoaDonService;
            _ban = ban;
            _maNV = maNV;

            InitializeComponent();
            InitializeLayout();
        }

        private void InitializeLayout()
        {
            pnlContent = new Panel
            {
                AutoScroll = true,
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20),
                AutoSize = false
            };

            this.Controls.Add(pnlContent);
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            await LoadBanDetail();
        }

        public async Task LoadBanDetail()
        {
            if (_isLoading) return;

            _isLoading = true;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                pnlContent.Controls.Clear();

                _ban = await _banBiaService.GetTableByIdAsync(_ban.MaBan);
                if (_ban == null)
                {
                    ShowError("Không tìm thấy thông tin bàn");
                    return;
                }

                int yPos = 0;
                int panelWidth = pnlContent.ClientSize.Width - 40;

                yPos = AddHeader(yPos, panelWidth);

                switch (_ban.TrangThai)
                {
                    case "Đang chơi":
                        await AddPlayingContent(yPos, panelWidth);
                        break;
                    case "Đã đặt":
                        AddReservedContent(yPos, panelWidth);
                        break;
                    case "Trống":
                        AddAvailableContent(yPos, panelWidth);
                        break;
                }

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                ShowError($"Lỗi khi tải dữ liệu: {ex.Message}");
            }
            finally
            {
                _isLoading = false;
            }
        }

        #region Header

        // 2. Sửa lỗi out parameter
        private int AddHeader(int yPos, int panelWidth)
        {
            var pnlHeader = new Panel
            {
                Location = new Point(0, yPos),
                Size = new Size(panelWidth, 50),
                BackColor = Color.White
            };

            var lblTableName = new Label
            {
                Text = _ban.TenBan,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(0, 10),
                AutoSize = true
            };

            var lblStatus = new Label
            {
                Text = _ban.TrangThai,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = GetStatusColor(_ban.TrangThai),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(100, 30),
                Location = new Point(panelWidth - 100, 10)
            };

            // Bo tròn góc cho status badge
            lblStatus.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            };

            pnlHeader.Controls.AddRange(new Control[] { lblTableName, lblStatus });
            pnlContent.Controls.Add(pnlHeader);

            // Thêm đường kẻ phân cách
            var separator = new Panel
            {
                Location = new Point(0, yPos + 55),
                Size = new Size(panelWidth, 1),
                BackColor = Color.FromArgb(226, 232, 240)
            };
            pnlContent.Controls.Add(separator);

            // 3. Trả về giá trị thay vì out
            return yPos + 70;
        }

        #endregion

        #region Playing Content

        private async Task AddPlayingContent(int yPos, int panelWidth)
        {
            // 4. Sửa lỗi type conflict bằng cách sử dụng alias hoặc tên đầy đủ
            var hoaDon = await _banBiaService.GetActiveInvoiceAsync(_ban.MaBan);
            if (hoaDon == null) return;

            var duration = DateTime.Now - hoaDon.ThoiGianBatDau.Value;
            var hours = (int)duration.TotalHours;
            var minutes = duration.Minutes;

            // Customer Info Card
            var pnlCustomer = CreateCard(panelWidth, Color.FromArgb(254, 249, 195));
            pnlCustomer.Location = new Point(0, yPos);

            int cardYPos = 15;

            // Tiêu đề
            var lblTitle = new Label
            {
                Text = "🎮 Thông tin chơi",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(15, cardYPos),
                AutoSize = true
            };
            pnlCustomer.Controls.Add(lblTitle);
            cardYPos += 35;

            // 5. Cập nhật cách gọi các hàm helper có out parameter
            cardYPos = AddInfoRow(pnlCustomer, "👤 Khách hàng:", hoaDon.MaKhNavigation?.TenKh ?? "Khách lẻ", cardYPos, panelWidth, true);

            if (hoaDon.MaKhNavigation != null)
            {
                cardYPos = AddInfoRow(pnlCustomer, "📞 SĐT:", hoaDon.MaKhNavigation.Sdt ?? "-", cardYPos, panelWidth);
            }

            cardYPos = AddInfoRow(pnlCustomer, "🕐 Bắt đầu:", hoaDon.ThoiGianBatDau.Value.ToString("HH:mm dd/MM/yyyy"), cardYPos, panelWidth);
            cardYPos = AddInfoRow(pnlCustomer, "⏱️ Thời gian:", $"{hours} giờ {minutes} phút", cardYPos, panelWidth, false, Color.FromArgb(220, 38, 38));

            var giaGio = _ban.MaLoaiNavigation?.GiaGio ?? 0;
            cardYPos = AddInfoRow(pnlCustomer, "💵 Giá giờ:", $"{giaGio:N0} đ/giờ", cardYPos, panelWidth);

            pnlCustomer.Height = cardYPos + 15;
            pnlContent.Controls.Add(pnlCustomer);
            yPos += cardYPos + 30;

            // Payment Info
            yPos = await AddPaymentInfo(hoaDon, duration, yPos, panelWidth);

            // Service List
            yPos = await AddServiceList(hoaDon.MaHd, yPos, panelWidth);

            // Action Buttons
            AddPlayingButtons(yPos, panelWidth);
        }

        // 6. Sửa lỗi out parameter và dùng alias
        private async Task<int> AddPaymentInfo(HoaDonEntity hoaDon, TimeSpan duration, int yPos, int panelWidth)
        {
            // TÍNH TOÁN CHÍNH XÁC
            // 1. Tính thời gian (làm tròn lên phút)
            var tongPhut = (int)Math.Ceiling(duration.TotalMinutes);
            var soGio = (decimal)tongPhut / 60m;

            // 2. Lấy giá giờ
            var giaGioDecimal = _ban.MaLoaiNavigation?.GiaGio ?? 0;

            // 3. Tính tiền bàn
            var tienBan = soGio * giaGioDecimal;

            // 4. Tính tiền dịch vụ từ CSDL
            var tienDichVu = await _banBiaService.GetInvoiceDetailsAsync(hoaDon.MaHd)
            .ContinueWith(t => t.Result.Sum(ct => ct.ThanhTien));

            // 5. Lấy giảm giá
            var giamGia = hoaDon.GiamGia ?? 0;

            // 6. Tính tạm tính
            var tamTinh = tienBan + tienDichVu - giamGia;

            // 7. Làm tròn lên nghìn
            var tongCong = Math.Ceiling((tamTinh ?? 0m) / 1000m) * 1000m;
            var chenhLech = tongCong - tamTinh;

            // Log để debug
            System.Diagnostics.Debug.WriteLine($"\n=== HIỂN THỊ THANH TOÁN ===");
            System.Diagnostics.Debug.WriteLine($"Thời gian: {tongPhut} phút ({soGio:F4} giờ)");
            System.Diagnostics.Debug.WriteLine($"Giá giờ: {giaGioDecimal:N0} đ");
            System.Diagnostics.Debug.WriteLine($"Tiền bàn: {tienBan:N2} đ");
            System.Diagnostics.Debug.WriteLine($"Tiền dịch vụ: {tienDichVu:N0} đ");
            System.Diagnostics.Debug.WriteLine($"Giảm giá: {giamGia:N0} đ");
            System.Diagnostics.Debug.WriteLine($"Tạm tính: {tamTinh:N2} đ");
            System.Diagnostics.Debug.WriteLine($"Tổng cộng (làm tròn): {tongCong:N0} đ");
            System.Diagnostics.Debug.WriteLine($"Chênh lệch: {chenhLech:N2} đ\n");

            var pnlPayment = CreateCard(panelWidth, Color.FromArgb(248, 250, 252));
            pnlPayment.Location = new Point(0, yPos);

            int payYPos = 15;

            payYPos = AddPaymentRow(pnlPayment, "Tiền bàn:", $"{tienBan:N0} đ", payYPos, panelWidth);
            payYPos = AddPaymentRow(pnlPayment, "Dịch vụ:", $"{tienDichVu:N0} đ", payYPos, panelWidth);

            if (giamGia > 0)
            {
                payYPos = AddPaymentRow(pnlPayment, "Giảm giá:", $"-{giamGia:N0} đ", payYPos, panelWidth);
            }

            if (chenhLech > 0)
            {
                payYPos = AddPaymentRow(pnlPayment, "Tạm tính:", $"{tamTinh:N0} đ", payYPos, panelWidth, true);
                payYPos = AddPaymentRow(pnlPayment, "Làm tròn:", $"+{chenhLech:N0} đ", payYPos, panelWidth, true);
            }

            // Separator
            var separator = new Panel
            {
                Location = new Point(15, payYPos),
                Size = new Size(panelWidth - 30, 2),
                BackColor = Color.FromArgb(226, 232, 240)
            };
            pnlPayment.Controls.Add(separator);
            payYPos += 15;

            // Total
            payYPos = AddTotalRow(pnlPayment, "Tổng cộng:", $"{tongCong:N0} đ", payYPos, panelWidth);

            pnlPayment.Height = payYPos + 15;
            pnlContent.Controls.Add(pnlPayment);
            return yPos + payYPos + 30;
        }

        // 7. Sửa lỗi out parameter
        private async Task<int> AddServiceList(int maHd, int yPos, int panelWidth)
        {
            var chiTietList = await _banBiaService.GetInvoiceDetailsAsync(maHd);

            // Service Header
            var lblService = new Label
            {
                Text = chiTietList.Count > 0 ? $"🍴 Dịch vụ đã order ({chiTietList.Count} món):" : "🍴 Dịch vụ:",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(0, yPos),
                AutoSize = true
            };
            pnlContent.Controls.Add(lblService);
            yPos += 40;

            if (chiTietList.Count > 0)
            {
                foreach (var item in chiTietList)
                {
                    var pnlService = CreateServiceItem(item, panelWidth);
                    pnlService.Location = new Point(0, yPos);
                    pnlContent.Controls.Add(pnlService);
                    yPos += 75;
                }
            }
            else
            {
                var lblNoService = new Label
                {
                    Text = "Chưa có dịch vụ nào",
                    Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                    ForeColor = Color.FromArgb(156, 163, 175),
                    Location = new Point(0, yPos),
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Size = new Size(panelWidth, 60),
                    BackColor = Color.FromArgb(249, 250, 251)
                };
                pnlContent.Controls.Add(lblNoService);
                yPos += 70;
            }

            return yPos;
        }

        private void AddPlayingButtons(int yPos, int panelWidth)
        {
            yPos += 20;

            var btnThemDV = CreateActionButton("➕ Thêm dịch vụ", Color.FromArgb(99, 102, 241), panelWidth);
            btnThemDV.Location = new Point(0, yPos);
            btnThemDV.Click += BtnThemDV_Click;
            pnlContent.Controls.Add(btnThemDV);
            yPos += 55;

            var btnThanhToan = CreateActionButton("💰 Kết thúc & Thanh toán", Color.FromArgb(34, 197, 94), panelWidth);
            btnThanhToan.Location = new Point(0, yPos);
            btnThanhToan.Click += BtnThanhToan_Click;
            pnlContent.Controls.Add(btnThanhToan);
        }

        #endregion

        #region Reserved Content

        private void AddReservedContent(int yPos, int panelWidth)
        {
            // Reserved Info Card
            var pnlReserved = CreateCard(panelWidth, Color.FromArgb(254, 249, 195));
            pnlReserved.Location = new Point(0, yPos);

            int cardYPos = 15;

            // Icon và Title
            var pnlIconTitle = new Panel
            {
                Location = new Point(15, cardYPos),
                Size = new Size(panelWidth - 30, 45),
                BackColor = Color.Transparent
            };

            var lblIcon = new Label
            {
                Text = "📅",
                Font = new Font("Segoe UI", 24F),
                Location = new Point(0, 0),
                AutoSize = true
            };

            var lblTitle = new Label
            {
                Text = "Thông tin đặt bàn",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(50, 8),
                AutoSize = true
            };

            pnlIconTitle.Controls.AddRange(new Control[] { lblIcon, lblTitle });
            pnlReserved.Controls.Add(pnlIconTitle);
            cardYPos += 60;

            if (_ban.MaKhNavigation != null)
            {
                cardYPos = AddInfoRow(pnlReserved, "👤 Khách hàng:", _ban.MaKhNavigation.TenKh, cardYPos, panelWidth, true);
                cardYPos = AddInfoRow(pnlReserved, "📞 SĐT:", _ban.MaKhNavigation.Sdt ?? "-", cardYPos, panelWidth);
            }
            else
            {
                cardYPos = AddInfoRow(pnlReserved, "👤 Khách hàng:", "Khách vãng lai", cardYPos, panelWidth);
            }

            if (!string.IsNullOrEmpty(_ban.GhiChu))
            {
                cardYPos = AddInfoRow(pnlReserved, "📝 Ghi chú:", _ban.GhiChu, cardYPos, panelWidth);
            }

            if (_ban.GioBatDau.HasValue)
            {
                cardYPos = AddInfoRow(pnlReserved, "🕐 Thời gian đặt:", _ban.GioBatDau.Value.ToString("HH:mm dd/MM/yyyy"), cardYPos, panelWidth);
            }

            var giaGio = _ban.MaLoaiNavigation?.GiaGio ?? 0;
            cardYPos = AddInfoRow(pnlReserved, "💰 Giá giờ:", $"{giaGio:N0} đ/giờ", cardYPos, panelWidth);

            pnlReserved.Height = cardYPos + 15;
            pnlContent.Controls.Add(pnlReserved);
            yPos += cardYPos + 30;

            AddReservedButtons(yPos, panelWidth);
        }

        private void AddReservedButtons(int yPos, int panelWidth)
        {
            yPos += 20;

            var btnConfirm = CreateActionButton("✅ Xác nhận khách đã đến", Color.FromArgb(34, 197, 94), panelWidth);
            btnConfirm.Location = new Point(0, yPos);
            btnConfirm.Click += BtnConfirm_Click;
            pnlContent.Controls.Add(btnConfirm);
            yPos += 55;

            var btnCancel = CreateActionButton("❌ Hủy đặt bàn", Color.FromArgb(239, 68, 68), panelWidth);
            btnCancel.Location = new Point(0, yPos);
            btnCancel.Click += BtnCancel_Click;
            pnlContent.Controls.Add(btnCancel);
        }

        #endregion

        #region Available Content

        private void AddAvailableContent(int yPos, int panelWidth)
        {
            // Available Info Card
            var pnlAvailable = CreateCard(panelWidth, Color.FromArgb(240, 253, 244));
            pnlAvailable.Location = new Point(0, yPos);

            int cardYPos = 15;

            // Icon và Title
            var pnlIconTitle = new Panel
            {
                Location = new Point(15, cardYPos),
                Size = new Size(panelWidth - 30, 45),
                BackColor = Color.Transparent
            };

            var lblIcon = new Label
            {
                Text = "🎱",
                Font = new Font("Segoe UI", 24F),
                Location = new Point(0, 0),
                AutoSize = true
            };

            var lblTitle = new Label
            {
                Text = "Bàn đang trống",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(50, 8),
                AutoSize = true
            };

            pnlIconTitle.Controls.AddRange(new Control[] { lblIcon, lblTitle });
            pnlAvailable.Controls.Add(pnlIconTitle);
            cardYPos += 60;

            cardYPos = AddInfoRow(pnlAvailable, "🎯 Loại bàn:", _ban.MaLoaiNavigation?.TenLoai ?? "Không rõ", cardYPos, panelWidth, true);
            cardYPos = AddInfoRow(pnlAvailable, "📍 Khu vực:", _ban.MaKhuVucNavigation?.TenKhuVuc ?? "Không rõ", cardYPos, panelWidth, true);

            var giaGio = _ban.MaLoaiNavigation?.GiaGio ?? 0;
            cardYPos = AddInfoRow(pnlAvailable, "💵 Giá giờ:", $"{giaGio:N0} đ/giờ", cardYPos, panelWidth, false, Color.FromArgb(34, 197, 94));

            if (!string.IsNullOrEmpty(_ban.GhiChu))
            {
                cardYPos = AddInfoRow(pnlAvailable, "📝 Ghi chú:", _ban.GhiChu, cardYPos, panelWidth);
            }

            pnlAvailable.Height = cardYPos + 15;
            pnlContent.Controls.Add(pnlAvailable);
            yPos += cardYPos + 30;

            AddAvailableButtons(yPos, panelWidth);
        }

        private void AddAvailableButtons(int yPos, int panelWidth)
        {
            yPos += 20;

            var btnBatDau = CreateActionButton("▶️ Bắt đầu chơi", Color.FromArgb(34, 197, 94), panelWidth);
            btnBatDau.Location = new Point(0, yPos);
            btnBatDau.Click += BtnBatDau_Click;
            pnlContent.Controls.Add(btnBatDau);
        }

        #endregion

        #region Helper Methods

        private Panel CreateCard(int width, Color bgColor)
        {
            var card = new Panel
            {
                Width = width,
                BackColor = bgColor,
                Padding = new Padding(0)
            };

            card.Paint += (s, e) =>
            {
                var rect = card.ClientRectangle;
                rect.Width -= 1;
                rect.Height -= 1;
                using (var pen = new Pen(Color.FromArgb(226, 232, 240), 1))
                {
                    e.Graphics.DrawRectangle(pen, rect);
                }
            };

            return card;
        }

        // 8. Sửa lỗi out parameter
        private int AddInfoRow(Panel panel, string label, string value, int yPos, int panelWidth, bool bold = false, Color? valueColor = null)
        {
            var rowPanel = new Panel
            {
                Location = new Point(15, yPos),
                Size = new Size(panelWidth - 30, 30),
                BackColor = Color.Transparent
            };

            var lblLabel = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(0, 5),
                AutoSize = true,
                MaximumSize = new Size((panelWidth - 30) / 2, 0)
            };

            var lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 9.5F, bold ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = valueColor ?? Color.FromArgb(30, 41, 59),
                AutoSize = false,
                Size = new Size((panelWidth - 30) / 2, 25),
                Location = new Point((panelWidth - 30) / 2, 5),
                TextAlign = ContentAlignment.MiddleRight
            };

            rowPanel.Controls.AddRange(new Control[] { lblLabel, lblValue });
            panel.Controls.Add(rowPanel);

            return yPos + 30;
        }

        // 9. Sửa lỗi out parameter
        private int AddPaymentRow(Panel panel, string label, string value, int yPos, int panelWidth, bool small = false)
        {
            var fontSize = small ? 9F : 9.5F;
            var fontStyle = small ? FontStyle.Italic : FontStyle.Regular;

            var rowPanel = new Panel
            {
                Location = new Point(15, yPos),
                Size = new Size(panelWidth - 30, 28),
                BackColor = Color.Transparent
            };

            var lblLabel = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", fontSize, fontStyle),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(0, 4),
                AutoSize = true
            };

            var lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", fontSize, fontStyle),
                ForeColor = Color.FromArgb(71, 85, 105),
                AutoSize = false,
                Size = new Size(150, 25),
                Location = new Point(panelWidth - 180, 4),
                TextAlign = ContentAlignment.MiddleRight
            };

            rowPanel.Controls.AddRange(new Control[] { lblLabel, lblValue });
            panel.Controls.Add(rowPanel);

            return yPos + 28;
        }

        // 10. Sửa lỗi out parameter
        private int AddTotalRow(Panel panel, string label, string value, int yPos, int panelWidth)
        {
            var rowPanel = new Panel
            {
                Location = new Point(15, yPos),
                Size = new Size(panelWidth - 30, 35),
                BackColor = Color.Transparent
            };

            var lblLabel = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(0, 6),
                AutoSize = true
            };

            var lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 38, 38),
                AutoSize = false,
                Size = new Size(150, 30),
                Location = new Point(panelWidth - 180, 6),
                TextAlign = ContentAlignment.MiddleRight
            };

            rowPanel.Controls.AddRange(new Control[] { lblLabel, lblValue });
            panel.Controls.Add(rowPanel);

            return yPos + 35;
        }

        // 11. Sửa lỗi type conflict bằng cách sử dụng alias
        private Panel CreateServiceItem(ChiTietHoaDonEntity item, int panelWidth)
        {
            var panel = new Panel
            {
                Size = new Size(panelWidth, 70),
                BackColor = Color.FromArgb(248, 250, 252),
                Padding = new Padding(15)
            };

            panel.Paint += (s, e) =>
            {
                var rect = panel.ClientRectangle;
                rect.Width -= 1;
                rect.Height -= 1;
                using (var pen = new Pen(Color.FromArgb(226, 232, 240), 1))
                {
                    e.Graphics.DrawRectangle(pen, rect);
                }
            };

            var lblName = new Label
            {
                Text = item.MaDvNavigation?.TenDv ?? "Dịch vụ",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(15, 12),
                AutoSize = true,
                MaximumSize = new Size(panelWidth - 200, 0)
            };

            var lblQuantity = new Label
            {
                Text = $"{item.SoLuong} x {item.MaDvNavigation?.Gia:N0} đ",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(15, 37),
                AutoSize = true
            };

            var lblPrice = new Label
            {
                Text = $"{item.ThanhTien:N0} đ",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 38, 38),
                AutoSize = false,
                Size = new Size(120, 25),
                Location = new Point(panelWidth - 175, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            var btnDelete = new Button
            {
                Text = "×",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                Size = new Size(35, 35),
                Location = new Point(panelWidth - 50, 17),
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(220, 38, 38),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Tag = item.Id
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 38, 38);
            btnDelete.MouseEnter += (s, e) => btnDelete.ForeColor = Color.White;
            btnDelete.MouseLeave += (s, e) => btnDelete.ForeColor = Color.FromArgb(220, 38, 38);
            btnDelete.Click += BtnDeleteService_Click;

            panel.Controls.AddRange(new Control[] { lblName, lblQuantity, lblPrice, btnDelete });
            return panel;
        }

        private Button CreateActionButton(string text, Color backColor, int panelWidth)
        {
            var btn = new Button
            {
                Text = text,
                Width = panelWidth,
                Height = 45,
                FlatStyle = FlatStyle.Flat,
                BackColor = backColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(0)
            };
            btn.FlatAppearance.BorderSize = 0;

            var hoverColor = Color.FromArgb(
                Math.Max(0, backColor.R - 20),
                Math.Max(0, backColor.G - 20),
                Math.Max(0, backColor.B - 20)
            );

            btn.MouseEnter += (s, e) => btn.BackColor = hoverColor;
            btn.MouseLeave += (s, e) => btn.BackColor = backColor;

            return btn;
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

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion

        #region Event Handlers

        private async void BtnDeleteService_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var chiTietId = (int)btn.Tag;

            var result = MessageBox.Show("Bạn có chắc muốn xóa dịch vụ này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var success = await _hoaDonService.RemoveServiceFromInvoiceAsync(chiTietId);
                    if (success)
                    {
                        MessageBox.Show("Đã xóa dịch vụ thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        OnDataChanged?.Invoke(this, EventArgs.Empty);
                        await LoadBanDetail();
                    }
                    else
                    {
                        ShowError("Không thể xóa dịch vụ!");
                    }
                }
                catch (Exception ex)
                {
                    ShowError($"Lỗi: {ex.Message}");
                }
            }
        }

        private async void BtnBatDau_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"\n=== BẮT ĐẦU CHƠI BÀN ===");
                System.Diagnostics.Debug.WriteLine($"Bàn: {_ban.TenBan} (MaBan: {_ban.MaBan})");
                System.Diagnostics.Debug.WriteLine($"Trạng thái hiện tại: {_ban.TrangThai}");
                System.Diagnostics.Debug.WriteLine($"Nhân viên: {_maNV}");

                // Hiển thị loading
                this.Cursor = Cursors.WaitCursor;

                var result = await _banBiaService.StartTableAsync(_ban.MaBan, _maNV);

                this.Cursor = Cursors.Default;

                if (result)
                {
                    System.Diagnostics.Debug.WriteLine($"✓ Bắt đầu chơi thành công!");

                    MessageBox.Show($"Đã bắt đầu chơi tại {_ban.TenBan}", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    OnDataChanged?.Invoke(this, EventArgs.Empty);
                    await LoadBanDetail();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"❌ Bắt đầu chơi thất bại!");
                    ShowError("Không thể bắt đầu chơi! Vui lòng kiểm tra lại.");
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                System.Diagnostics.Debug.WriteLine($"❌ Exception trong BtnBatDau_Click:");
                System.Diagnostics.Debug.WriteLine($"Message: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Inner: {ex.InnerException?.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack: {ex.StackTrace}");

                ShowError($"Lỗi: {ex.Message}");
            }
        }

        private async void BtnCancel_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc muốn hủy đặt bàn này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine($"\n=== HỦY ĐẶT BÀN ===");
                    System.Diagnostics.Debug.WriteLine($"Bàn: {_ban.TenBan} (MaBan: {_ban.MaBan})");
                    System.Diagnostics.Debug.WriteLine($"Trạng thái bàn: {_ban.TrangThai}");

                    this.Cursor = Cursors.WaitCursor;

                    // Tìm đơn đặt bàn đang hoạt động
                    var datBanService = Program.GetService<DatBanService>();
                    var datBans = await datBanService.GetByTableAsync(_ban.MaBan);
                    var activeDatBan = datBans.FirstOrDefault(d =>
                        d.TrangThai == "Đang chờ" || d.TrangThai == "Đã đặt");

                    if (activeDatBan == null)
                    {
                        this.Cursor = Cursors.Default;
                        ShowError("Không tìm thấy đơn đặt bàn đang hoạt động!");
                        System.Diagnostics.Debug.WriteLine("❌ Không tìm thấy đơn đặt");
                        return;
                    }

                    System.Diagnostics.Debug.WriteLine($"Đơn đặt: MaDat={activeDatBan.MaDat}, Khách={activeDatBan.TenKhach}");

                    // Gọi CancelReservationAsync thay vì PauseTableAsync
                    var success = await _banBiaService.CancelReservationAsync(activeDatBan.MaDat);

                    this.Cursor = Cursors.Default;

                    if (success)
                    {
                        System.Diagnostics.Debug.WriteLine("✓ Hủy đặt bàn thành công!");

                        MessageBox.Show("Đã hủy đặt bàn thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        OnDataChanged?.Invoke(this, EventArgs.Empty);
                        await LoadBanDetail();
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("❌ Hủy đặt bàn thất bại!");
                        ShowError("Không thể hủy đặt bàn!");
                    }
                }
                catch (Exception ex)
                {
                    this.Cursor = Cursors.Default;
                    System.Diagnostics.Debug.WriteLine($"❌ Exception: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Stack: {ex.StackTrace}");
                    ShowError($"Lỗi: {ex.Message}");
                }
            }
        }

        private async void BtnConfirm_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Xác nhận khách hàng đã đến?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine($"\n=== XÁC NHẬN ĐẶT BÀN ===");
                    System.Diagnostics.Debug.WriteLine($"Bàn: {_ban.TenBan} (MaBan: {_ban.MaBan})");

                    this.Cursor = Cursors.WaitCursor;

                    // Tìm đơn đặt bàn
                    var datBanService = Program.GetService<DatBanService>();
                    var datBans = await datBanService.GetByTableAsync(_ban.MaBan);
                    var activeDatBan = datBans.FirstOrDefault(d => d.TrangThai == "Đang chờ" || d.TrangThai == "Đã đặt");

                    if (activeDatBan == null)
                    {
                        this.Cursor = Cursors.Default;
                        ShowError("Không tìm thấy đơn đặt bàn!");
                        return;
                    }

                    System.Diagnostics.Debug.WriteLine($"Đơn đặt: {activeDatBan.MaDat} - Khách: {activeDatBan.TenKhach}");

                    var success = await _banBiaService.ConfirmReservationAsync(activeDatBan.MaDat, _maNV);

                    this.Cursor = Cursors.Default;

                    if (success)
                    {
                        System.Diagnostics.Debug.WriteLine($"✓ Xác nhận thành công!");

                        MessageBox.Show("Đã xác nhận và bắt đầu chơi!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        OnDataChanged?.Invoke(this, EventArgs.Empty);
                        await LoadBanDetail();
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ Xác nhận thất bại!");
                        ShowError("Không thể xác nhận!");
                    }
                }
                catch (Exception ex)
                {
                    this.Cursor = Cursors.Default;
                    System.Diagnostics.Debug.WriteLine($"❌ Exception: {ex.Message}");
                    ShowError($"Lỗi: {ex.Message}");
                }
            }
        }
        private async void BtnThemDV_Click(object sender, EventArgs e)
        {
            try
            {
                var hoaDon = await _banBiaService.GetActiveInvoiceAsync(_ban.MaBan);
                if (hoaDon == null)
                {
                    ShowError("Không tìm thấy hóa đơn đang hoạt động!");
                    return;
                }

                var dichVuService = Program.GetService<DichVuService>();

                using (var themDichVuForm = new ThemDichVuForm(dichVuService, _hoaDonService, hoaDon.MaHd))
                {
                    var result = themDichVuForm.ShowDialog(this);

                    if (result == DialogResult.OK)
                    {
                        await LoadBanDetail();
                        OnDataChanged?.Invoke(this, EventArgs.Empty);

                        MessageBox.Show("Đã thêm dịch vụ thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi thêm dịch vụ: {ex.Message}");
            }
        }

        private async void BtnThanhToan_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show(
                    $"Xác nhận kết thúc và thanh toán cho {_ban.TenBan}?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes) return;

                // Lấy các service cần thiết
                var thanhToanService = Program.GetService<ThanhToanService>();
                var vietQRService = Program.GetService<VietQRService>();

                // Lấy hóa đơn đang hoạt động
                var hoaDon = await _banBiaService.GetActiveInvoiceAsync(_ban.MaBan);
                if (hoaDon == null)
                {
                    MessageBox.Show("Không tìm thấy hóa đơn đang hoạt động!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Mở form thanh toán
                using (var thanhToanForm = new ThanhToanForm(thanhToanService, vietQRService, hoaDon.MaHd))
                {
                    var thanhToanResult = thanhToanForm.ShowDialog(this);

                    if (thanhToanResult == DialogResult.OK)
                    {
                        MessageBox.Show(
                            $"✓ Đã thanh toán thành công!\nBàn {_ban.TenBan} đã được trả về trống.",
                            "Thành công",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        // Trigger reload data
                        OnDataChanged?.Invoke(this, EventArgs.Empty);
                        await LoadBanDetail();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thanh toán: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}