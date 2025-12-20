using Billiard.BLL.Services;
using Billiard.BLL.Services.HoaDonServices;
using Billiard.BLL.Services.QLBan;
using Billiard.BLL.Services.VietQR;
using Billiard.DAL.Entities;
using System;
using System.Drawing;
using System.Linq;
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

        private const int PADDING = 20;
        private const int CARD_SPACING = 15;
        private const int MIN_PANEL_WIDTH = 400;
        private const int SECTION_SPACING = 20;

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
                BackColor = Color.FromArgb(248, 250, 252),
                Padding = new Padding(PADDING + 10, PADDING, PADDING + 5, PADDING), // Thêm 10px bên trái
                AutoSize = false
            };

            this.Controls.Add(pnlContent);
            this.BackColor = Color.FromArgb(248, 250, 252);
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

                pnlContent.SuspendLayout();
                pnlContent.Controls.Clear();

                _ban = await _banBiaService.GetTableByIdAsync(_ban.MaBan);
                if (_ban == null)
                {
                    ShowError("Không tìm thấy thông tin bàn");
                    return;
                }

                int availableWidth = Math.Max(MIN_PANEL_WIDTH, pnlContent.ClientSize.Width - (PADDING * 2) - 20);
                int yPos = 0;

                yPos = AddHeader(yPos, availableWidth);
                yPos += SECTION_SPACING;

                switch (_ban.TrangThai)
                {
                    case "Đang chơi":
                        yPos = await AddPlayingContent(yPos, availableWidth);
                        break;
                    case "Đã đặt":
                        yPos = AddReservedContent(yPos, availableWidth);
                        break;
                    case "Trống":
                        yPos = AddAvailableContent(yPos, availableWidth);
                        break;
                }

                yPos += 20;

                pnlContent.ResumeLayout();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                pnlContent.ResumeLayout();
                this.Cursor = Cursors.Default;
                ShowError($"Lỗi khi tải dữ liệu: {ex.Message}");
            }
            finally
            {
                _isLoading = false;
            }
        }

        #region Header

        private int AddHeader(int yPos, int panelWidth)
        {
            var pnlHeader = new Panel
            {
                Location = new Point(0, yPos),
                Size = new Size(panelWidth, 85),
                BackColor = Color.White
            };

            pnlHeader.Paint += (s, e) =>
            {
                var rect = pnlHeader.ClientRectangle;
                using (var pen = new Pen(Color.FromArgb(226, 232, 240), 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, rect.Width - 1, rect.Height - 1);
                }
            };

            var lblTableName = new Label
            {
                Text = _ban.TenBan,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(15, 12),
                AutoSize = true,
                MaximumSize = new Size(panelWidth - 130, 0)
            };

            var lblSubtitle = new Label
            {
                Text = $"{_ban.MaLoaiNavigation?.TenLoai ?? ""}",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(15, 42),
                AutoSize = true
            };

            var lblArea = new Label
            {
                Text = $"{_ban.MaKhuVucNavigation?.TenKhuVuc ?? ""}",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(15, 60),
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
                Size = new Size(90, 30),
                Location = new Point(panelWidth - 105, 28)
            };

            pnlHeader.Controls.AddRange(new Control[] { lblTableName, lblSubtitle, lblArea, lblStatus });
            pnlContent.Controls.Add(pnlHeader);

            return yPos + 85;
        }

        #endregion

        #region Playing Content

        private async Task<int> AddPlayingContent(int yPos, int panelWidth)
        {
            var hoaDon = await _banBiaService.GetActiveInvoiceAsync(_ban.MaBan);
            if (hoaDon == null)
            {
                ShowError("Không tìm thấy hóa đơn đang hoạt động!");
                return yPos;
            }

            var duration = DateTime.Now - hoaDon.ThoiGianBatDau.Value;

            yPos = AddTimerCard(duration, yPos, panelWidth);
            yPos += CARD_SPACING;

            yPos = AddCustomerInfoCard(hoaDon, yPos, panelWidth);
            yPos += CARD_SPACING;

            yPos = await AddPaymentInfo(hoaDon, duration, yPos, panelWidth);
            yPos += CARD_SPACING;

            yPos = await AddServiceListWithScroll(hoaDon.MaHd, yPos, panelWidth);
            yPos += SECTION_SPACING;

            yPos = AddPlayingButtons(yPos, panelWidth);

            return yPos;
        }

        private int AddTimerCard(TimeSpan duration, int yPos, int panelWidth)
        {
            var hours = (int)duration.TotalHours;
            var minutes = duration.Minutes;

            var pnlTimer = new Panel
            {
                Location = new Point(0, yPos),
                Size = new Size(panelWidth, 75),
                BackColor = Color.FromArgb(220, 38, 38)
            };

            var lblLabel = new Label
            {
                Text = "THỜI GIAN CHƠI",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(254, 202, 202),
                Location = new Point(15, 12),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            var lblTime = new Label
            {
                Text = $"{hours:D2}h {minutes:D2}m",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(15, 32),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            pnlTimer.Controls.AddRange(new Control[] { lblLabel, lblTime });
            pnlContent.Controls.Add(pnlTimer);

            return yPos + 75;
        }

        private int AddCustomerInfoCard(HoaDonEntity hoaDon, int yPos, int panelWidth)
        {
            var pnlCustomer = CreateModernCard(panelWidth);
            pnlCustomer.Location = new Point(0, yPos);

            int cardY = 12;

            var lblTitle = new Label
            {
                Text = "THÔNG TIN KHÁCH HÀNG",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(12, cardY),
                AutoSize = true
            };
            pnlCustomer.Controls.Add(lblTitle);
            cardY += 28;

            var customerName = hoaDon.MaKhNavigation?.TenKh ?? "Khách lẻ";
            var customerPhone = hoaDon.MaKhNavigation?.Sdt ?? "Không có";
            var startTime = hoaDon.ThoiGianBatDau.Value.ToString("HH:mm, dd/MM/yyyy");

            cardY = AddInfoRow(pnlCustomer, "Tên khách", customerName, cardY, panelWidth);
            cardY = AddInfoRow(pnlCustomer, "Điện thoại", customerPhone, cardY, panelWidth);
            cardY = AddInfoRow(pnlCustomer, "Bắt đầu", startTime, cardY, panelWidth);

            cardY += 8;
            pnlCustomer.Height = cardY;
            pnlContent.Controls.Add(pnlCustomer);

            return yPos + cardY;
        }

        private async Task<int> AddPaymentInfo(HoaDonEntity hoaDon, TimeSpan duration, int yPos, int panelWidth)
        {
            var tongPhut = (int)Math.Ceiling(duration.TotalMinutes);
            var soGio = (decimal)tongPhut / 60m;
            var giaGioDecimal = _ban.MaLoaiNavigation?.GiaGio ?? 0;
            var tienBan = soGio * giaGioDecimal;

            var tienDichVu = await _banBiaService.GetInvoiceDetailsAsync(hoaDon.MaHd)
                .ContinueWith(t => t.Result.Sum(ct => ct.ThanhTien));

            var giamGia = hoaDon.GiamGia ?? 0;
            var tamTinh = tienBan + tienDichVu - giamGia;
            var tongCong = Math.Ceiling((tamTinh ?? 0m) / 1000m) * 1000m;
            var chenhLech = tongCong - tamTinh;

            var pnlPayment = CreateModernCard(panelWidth);
            pnlPayment.Location = new Point(0, yPos);

            int cardY = 12;

            var lblTitle = new Label
            {
                Text = "CHI TIẾT THANH TOÁN",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(12, cardY),
                AutoSize = true
            };
            pnlPayment.Controls.Add(lblTitle);
            cardY += 28;

            cardY = AddPaymentRow(pnlPayment, "Tiền bàn", $"{tienBan:N0}đ", cardY, panelWidth);
            cardY = AddPaymentRow(pnlPayment, "Dịch vụ", $"{tienDichVu:N0}đ", cardY, panelWidth);

            if (giamGia > 0)
            {
                cardY = AddPaymentRow(pnlPayment, "Giảm giá", $"-{giamGia:N0}đ", cardY, panelWidth, Color.FromArgb(34, 197, 94));
            }

            if (chenhLech > 0)
            {
                cardY += 5;
                cardY = AddPaymentRow(pnlPayment, "Tạm tính", $"{tamTinh:N0}đ", cardY, panelWidth, null, true);
                cardY = AddPaymentRow(pnlPayment, "Làm tròn", $"+{chenhLech:N0}đ", cardY, panelWidth, null, true);
            }

            cardY += 8;
            var separator = new Panel
            {
                Location = new Point(12, cardY),
                Size = new Size(panelWidth - 24, 1),
                BackColor = Color.FromArgb(226, 232, 240)
            };
            pnlPayment.Controls.Add(separator);
            cardY += 12;

            var pnlTotal = new Panel
            {
                Location = new Point(12, cardY),
                Size = new Size(panelWidth - 24, 42),
                BackColor = Color.FromArgb(239, 246, 255)
            };

            var lblTotalLabel = new Label
            {
                Text = "TỔNG CỘNG",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 64, 175),
                Location = new Point(10, 11),
                AutoSize = true
            };

            var lblTotalValue = new Label
            {
                Text = $"{tongCong:N0}đ",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 38, 38),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleRight,
                Size = new Size(panelWidth - 160, 26),
                Location = new Point(panelWidth - 170, 8)
            };

            pnlTotal.Controls.AddRange(new Control[] { lblTotalLabel, lblTotalValue });
            pnlPayment.Controls.Add(pnlTotal);
            cardY += 50;

            pnlPayment.Height = cardY;
            pnlContent.Controls.Add(pnlPayment);

            return yPos + cardY;
        }

        private async Task<int> AddServiceListWithScroll(int maHd, int yPos, int panelWidth)
        {
            var chiTietList = await _banBiaService.GetInvoiceDetailsAsync(maHd);

            var lblHeader = new Label
            {
                Text = $"DỊCH VỤ ĐÃ GỌI ({chiTietList.Count})",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(0, yPos),
                AutoSize = true
            };
            pnlContent.Controls.Add(lblHeader);
            yPos += 28;

            if (chiTietList.Count > 0)
            {
                var pnlServicesContainer = new Panel
                {
                    Location = new Point(0, yPos),
                    Size = new Size(panelWidth, 220),
                    BackColor = Color.White,
                    AutoScroll = true,
                    BorderStyle = BorderStyle.FixedSingle
                };

                int serviceY = 8;
                foreach (var item in chiTietList)
                {
                    serviceY = AddServiceItem(pnlServicesContainer, item, serviceY, panelWidth - 25);
                }

                pnlContent.Controls.Add(pnlServicesContainer);
                yPos += 220;
            }
            else
            {
                var pnlEmpty = CreateModernCard(panelWidth);
                pnlEmpty.Location = new Point(0, yPos);
                pnlEmpty.Height = 65;
                pnlEmpty.BackColor = Color.FromArgb(249, 250, 251);

                var lblEmpty = new Label
                {
                    Text = "Chưa có dịch vụ",
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Italic),
                    ForeColor = Color.FromArgb(148, 163, 184),
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                };
                pnlEmpty.Controls.Add(lblEmpty);
                pnlContent.Controls.Add(pnlEmpty);
                yPos += 65;
            }

            return yPos;
        }

        private int AddServiceItem(Panel parentPanel, ChiTietHoaDonEntity item, int yPos, int panelWidth)
        {
            var pnlService = new Panel
            {
                Location = new Point(8, yPos),
                Size = new Size(panelWidth, 58),
                BackColor = Color.FromArgb(248, 250, 252)
            };

            var lblName = new Label
            {
                Text = item.MaDvNavigation?.TenDv ?? "Dịch vụ",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(10, 8),
                AutoSize = true,
                MaximumSize = new Size(panelWidth - 160, 0)
            };

            var lblQuantity = new Label
            {
                Text = $"SL: {item.SoLuong} x {item.MaDvNavigation?.Gia:N0}đ",
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(10, 30),
                AutoSize = true
            };

            var lblPrice = new Label
            {
                Text = $"{item.ThanhTien:N0}đ",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 38, 38),
                AutoSize = false,
                Size = new Size(95, 22),
                Location = new Point(panelWidth - 140, 14),
                TextAlign = ContentAlignment.MiddleRight
            };

            var btnDelete = new Button
            {
                Text = "Xóa",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Size = new Size(40, 28),
                Location = new Point(panelWidth - 45, 15),
                BackColor = Color.FromArgb(239, 68, 68),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Tag = item.Id
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += BtnDeleteService_Click;

            pnlService.Controls.AddRange(new Control[] { lblName, lblQuantity, lblPrice, btnDelete });
            parentPanel.Controls.Add(pnlService);

            return yPos + 62;
        }

        private int AddPlayingButtons(int yPos, int panelWidth)
        {
            var btnThemDV = CreateModernButton("Thêm dịch vụ", Color.FromArgb(59, 130, 246), panelWidth);
            btnThemDV.Location = new Point(0, yPos);
            btnThemDV.Click += BtnThemDV_Click;
            pnlContent.Controls.Add(btnThemDV);
            yPos += 48;

            var btnThanhToan = CreateModernButton("Thanh toán", Color.FromArgb(34, 197, 94), panelWidth);
            btnThanhToan.Location = new Point(0, yPos);
            btnThanhToan.Click += BtnThanhToan_Click;
            pnlContent.Controls.Add(btnThanhToan);
            yPos += 48;

            return yPos;
        }

        #endregion

        #region Reserved Content

        private int AddReservedContent(int yPos, int panelWidth)
        {
            var pnlReserved = CreateModernCard(panelWidth);
            pnlReserved.Location = new Point(0, yPos);
            pnlReserved.BackColor = Color.FromArgb(254, 252, 232);

            int cardY = 12;

            var lblTitle = new Label
            {
                Text = "THÔNG TIN ĐẶT BÀN",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(12, cardY),
                AutoSize = true
            };
            pnlReserved.Controls.Add(lblTitle);
            cardY += 28;

            if (_ban.MaKhNavigation != null)
            {
                cardY = AddInfoRow(pnlReserved, "Khách hàng", _ban.MaKhNavigation.TenKh, cardY, panelWidth);
                cardY = AddInfoRow(pnlReserved, "Điện thoại", _ban.MaKhNavigation.Sdt ?? "-", cardY, panelWidth);
            }

            if (_ban.GioBatDau.HasValue)
            {
                cardY = AddInfoRow(pnlReserved, "Thời gian",
                    _ban.GioBatDau.Value.ToString("HH:mm, dd/MM/yyyy"), cardY, panelWidth);
            }

            var giaGio = _ban.MaLoaiNavigation?.GiaGio ?? 0;
            cardY = AddInfoRow(pnlReserved, "Giá giờ", $"{giaGio:N0}đ", cardY, panelWidth);

            if (!string.IsNullOrEmpty(_ban.GhiChu))
            {
                cardY = AddInfoRow(pnlReserved, "Ghi chú", _ban.GhiChu, cardY, panelWidth);
            }

            cardY += 8;
            pnlReserved.Height = cardY;
            pnlContent.Controls.Add(pnlReserved);
            yPos += cardY + CARD_SPACING;

            yPos = AddReservedButtons(yPos, panelWidth);
            return yPos;
        }

        private int AddReservedButtons(int yPos, int panelWidth)
        {
            var btnConfirm = CreateModernButton("Xác nhận khách đến", Color.FromArgb(34, 197, 94), panelWidth);
            btnConfirm.Location = new Point(0, yPos);
            btnConfirm.Click += BtnConfirm_Click;
            pnlContent.Controls.Add(btnConfirm);
            yPos += 48;

            var btnCancel = CreateModernButton("Hủy đặt bàn", Color.FromArgb(239, 68, 68), panelWidth);
            btnCancel.Location = new Point(0, yPos);
            btnCancel.Click += BtnCancel_Click;
            pnlContent.Controls.Add(btnCancel);
            yPos += 48;

            return yPos;
        }

        #endregion

        #region Available Content

        private int AddAvailableContent(int yPos, int panelWidth)
        {
            var pnlAvailable = CreateModernCard(panelWidth);
            pnlAvailable.Location = new Point(0, yPos);
            pnlAvailable.BackColor = Color.FromArgb(240, 253, 244);

            int cardY = 12;

            var lblTitle = new Label
            {
                Text = "BÀN ĐANG TRỐNG",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(12, cardY),
                AutoSize = true
            };
            pnlAvailable.Controls.Add(lblTitle);
            cardY += 28;

            cardY = AddInfoRow(pnlAvailable, "Loại bàn",
                _ban.MaLoaiNavigation?.TenLoai ?? "Không rõ", cardY, panelWidth);

            cardY = AddInfoRow(pnlAvailable, "Khu vực",
                _ban.MaKhuVucNavigation?.TenKhuVuc ?? "Không rõ", cardY, panelWidth);

            var giaGio = _ban.MaLoaiNavigation?.GiaGio ?? 0;
            cardY = AddInfoRow(pnlAvailable, "Giá giờ",
                $"{giaGio:N0}đ", cardY, panelWidth);

            if (!string.IsNullOrEmpty(_ban.GhiChu))
            {
                cardY = AddInfoRow(pnlAvailable, "Ghi chú", _ban.GhiChu, cardY, panelWidth);
            }

            cardY += 8;
            pnlAvailable.Height = cardY;
            pnlContent.Controls.Add(pnlAvailable);
            yPos += cardY + CARD_SPACING;

            yPos = AddAvailableButtons(yPos, panelWidth);
            return yPos;
        }

        private int AddAvailableButtons(int yPos, int panelWidth)
        {
            var btnBatDau = CreateModernButton("Bắt đầu chơi", Color.FromArgb(34, 197, 94), panelWidth);
            btnBatDau.Location = new Point(0, yPos);
            btnBatDau.Click += BtnBatDau_Click;
            pnlContent.Controls.Add(btnBatDau);
            yPos += 48;

            return yPos;
        }

        #endregion

        #region Helper Methods

        private Panel CreateModernCard(int width)
        {
            var card = new Panel
            {
                Width = width,
                BackColor = Color.White,
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

        private int AddInfoRow(Panel panel, string label, string value, int yPos, int panelWidth)
        {
            var pnlRow = new Panel
            {
                Location = new Point(12, yPos),
                Size = new Size(panelWidth - 24, 26),
                BackColor = Color.Transparent
            };

            var lblLabel = new Label
            {
                Text = label + ":",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(0, 5),
                AutoSize = true,
                MaximumSize = new Size((panelWidth - 40) / 2, 0)
            };

            var lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = false,
                Size = new Size((panelWidth - 40) / 2, 22),
                Location = new Point((panelWidth - 40) / 2 + 8, 5),
                TextAlign = ContentAlignment.TopRight
            };

            pnlRow.Controls.AddRange(new Control[] { lblLabel, lblValue });
            panel.Controls.Add(pnlRow);

            return yPos + 26;
        }

        private int AddPaymentRow(Panel panel, string label, string value, int yPos, int panelWidth,
            Color? customColor = null, bool isSmall = false)
        {
            var fontSize = isSmall ? 8F : 8.5F;
            var fontStyle = isSmall ? FontStyle.Italic : FontStyle.Regular;

            var pnlRow = new Panel
            {
                Location = new Point(12, yPos),
                Size = new Size(panelWidth - 24, 24),
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
                ForeColor = customColor ?? Color.FromArgb(30, 41, 59),
                AutoSize = false,
                Size = new Size(115, 20),
                Location = new Point(panelWidth - 145, 4),
                TextAlign = ContentAlignment.MiddleRight
            };

            pnlRow.Controls.AddRange(new Control[] { lblLabel, lblValue });
            panel.Controls.Add(pnlRow);

            return yPos + 24;
        }

        private Button CreateModernButton(string text, Color backColor, int panelWidth)
        {
            var btn = new Button
            {
                Text = text,
                Width = panelWidth,
                Height = 42,
                FlatStyle = FlatStyle.Flat,
                BackColor = backColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;

            var hoverColor = Color.FromArgb(
                Math.Max(0, backColor.R - 30),
                Math.Max(0, backColor.G - 30),
                Math.Max(0, backColor.B - 30)
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
                this.Cursor = Cursors.WaitCursor;
                var result = await _banBiaService.StartTableAsync(_ban.MaBan, _maNV);
                this.Cursor = Cursors.Default;

                if (result)
                {
                    MessageBox.Show($"Đã bắt đầu chơi tại {_ban.TenBan}", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    OnDataChanged?.Invoke(this, EventArgs.Empty);
                    await LoadBanDetail();
                }
                else
                {
                    ShowError("Không thể bắt đầu chơi! Vui lòng kiểm tra lại.");
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
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
                    this.Cursor = Cursors.WaitCursor;

                    var datBanService = Program.GetService<DatBanService>();
                    var datBans = await datBanService.GetByTableAsync(_ban.MaBan);
                    var activeDatBan = datBans.FirstOrDefault(d =>
                        d.TrangThai == "Đang chờ" || d.TrangThai == "Đã đặt");

                    if (activeDatBan == null)
                    {
                        this.Cursor = Cursors.Default;
                        ShowError("Không tìm thấy đơn đặt bàn đang hoạt động!");
                        return;
                    }

                    var success = await _banBiaService.CancelReservationAsync(activeDatBan.MaDat);
                    this.Cursor = Cursors.Default;

                    if (success)
                    {
                        MessageBox.Show("Đã hủy đặt bàn thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        OnDataChanged?.Invoke(this, EventArgs.Empty);
                        await LoadBanDetail();
                    }
                    else
                    {
                        ShowError("Không thể hủy đặt bàn!");
                    }
                }
                catch (Exception ex)
                {
                    this.Cursor = Cursors.Default;
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
                    this.Cursor = Cursors.WaitCursor;

                    var datBanService = Program.GetService<DatBanService>();
                    var datBans = await datBanService.GetByTableAsync(_ban.MaBan);
                    var activeDatBan = datBans.FirstOrDefault(d =>
                        d.TrangThai == "Đang chờ" || d.TrangThai == "Đã đặt");

                    if (activeDatBan == null)
                    {
                        this.Cursor = Cursors.Default;
                        ShowError("Không tìm thấy đơn đặt bàn!");
                        return;
                    }

                    var success = await _banBiaService.ConfirmReservationAsync(activeDatBan.MaDat, _maNV);
                    this.Cursor = Cursors.Default;

                    if (success)
                    {
                        MessageBox.Show("Đã xác nhận và bắt đầu chơi!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        OnDataChanged?.Invoke(this, EventArgs.Empty);
                        await LoadBanDetail();
                    }
                    else
                    {
                        ShowError("Không thể xác nhận!");
                    }
                }
                catch (Exception ex)
                {
                    this.Cursor = Cursors.Default;
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

                var thanhToanService = Program.GetService<ThanhToanService>();
                var vietQRService = Program.GetService<VietQRService>();

                var hoaDon = await _banBiaService.GetActiveInvoiceAsync(_ban.MaBan);
                if (hoaDon == null)
                {
                    MessageBox.Show("Không tìm thấy hóa đơn đang hoạt động!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (var thanhToanForm = new ThanhToanForm(thanhToanService, vietQRService, hoaDon.MaHd))
                {
                    var thanhToanResult = thanhToanForm.ShowDialog(this);

                    if (thanhToanResult == DialogResult.OK)
                    {
                        MessageBox.Show(
                            $"Đã thanh toán thành công!\nBàn {_ban.TenBan} đã được trả về trống.",
                            "Thành công",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

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