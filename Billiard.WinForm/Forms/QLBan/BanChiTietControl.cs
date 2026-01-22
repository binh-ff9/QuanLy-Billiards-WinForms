using Billiard.BLL.Services;
using Billiard.BLL.Services.HoaDonServices;
using Billiard.BLL.Services.QLBan;
using Billiard.BLL.Services.VietQR;
using Billiard.DAL.Entities;
using Microsoft.Extensions.DependencyInjection;
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
        private System.Threading.CancellationTokenSource _cts;
        private readonly GioHoatDongService _gioHoatDongService;

        public event EventHandler OnDataChanged;
        public event EventHandler<BanBium> OnBanUpdated;

        private const int PADDING = 20;
        private const int CARD_SPACING = 15;
        private const int MIN_PANEL_WIDTH = 400;
        private const int SECTION_SPACING = 20;
        private const int TOP_SPACING = 25;

        public BanChiTietControl(BanBiaService banBiaService, HoaDonService hoaDonService, BanBium ban, int maNV)
        {
            _banBiaService = banBiaService;
            _hoaDonService = hoaDonService;
            _ban = ban;
            _maNV = maNV;
            _gioHoatDongService = new GioHoatDongService(); 

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
                Padding = new Padding(PADDING + 10, TOP_SPACING, PADDING + 5, PADDING),
                AutoSize = false
            };

            // Enable double buffering
            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, pnlContent, new object[] { true });

            this.Controls.Add(pnlContent);
            this.BackColor = Color.FromArgb(248, 250, 252);
        }
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            await LoadBanDetail();
        }

        public async Task LoadBanDetail(bool forceReload = false)
        {
            if (_isLoading) return;

            _cts?.Cancel();
            _cts = new System.Threading.CancellationTokenSource();
            var token = _cts.Token;

            _isLoading = true;
            try
            {
                // ✅ DELAY để đợi transaction hoàn tất
                await Task.Delay(200, token);

                if (token.IsCancellationRequested) return;

                // ✅ RETRY LOGIC để tránh conflict
                BanBium newBan = null;
                int retryCount = 0;
                int maxRetries = 3;

                while (retryCount < maxRetries && newBan == null)
                {
                    try
                    {
                        newBan = await Task.Run(() => _banBiaService.GetTableByIdAsync(_ban.MaBan), token);
                        break;
                    }
                    catch (InvalidOperationException) when (retryCount < maxRetries - 1)
                    {
                        // DbContext conflict - retry sau delay
                        retryCount++;
                        await Task.Delay(100 * retryCount, token);
                    }
                }

                if (token.IsCancellationRequested) return;

                if (newBan == null)
                {
                    ShowError("Không tìm thấy thông tin bàn");
                    return;
                }

                _ban = newBan;

                if (!forceReload && !HasDataChanged(_ban, newBan) && pnlContent.Controls.Count > 0)
                {
                    await UpdateExistingControlsAsync();
                    return;
                }

                await FullReloadContentOptimized(token);
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi tải dữ liệu: {ex.Message}");
            }
            finally
            {
                _isLoading = false;
            }
        }
        private async Task UpdateExistingControlsAsync()
        {
            UpdateHeaderIfExists();

            if (_ban.TrangThai == "Đang chơi")
            {
                await UpdateTimerIfExists();
                await UpdatePaymentInfoIfExists();
            }

            OnBanUpdated?.Invoke(this, _ban);
        }
        private bool HasDataChanged(BanBium oldBan, BanBium newBan)
        {
            if (oldBan.TrangThai != newBan.TrangThai) return true;
            if (oldBan.GioBatDau != newBan.GioBatDau) return true;
            if (oldBan.MaKh != newBan.MaKh) return true;
            return false;
        }

        private async Task UpdateExistingControls()
        {
            UpdateHeaderIfExists();

            if (_ban.TrangThai == "Đang chơi")
            {
                await UpdateTimerIfExists();
                await UpdatePaymentInfoIfExists();
            }

            OnBanUpdated?.Invoke(this, _ban);
        }

        private void UpdateHeaderIfExists()
        {
            foreach (Control ctrl in pnlContent.Controls)
            {
                if (ctrl is Panel pnl && pnl.Controls.Count > 0)
                {
                    var lblStatus = pnl.Controls.OfType<Label>()
                        .FirstOrDefault(l => l.Text == "Trống" || l.Text == "Đang chơi" || l.Text == "Đã đặt");

                    if (lblStatus != null)
                    {
                        lblStatus.Text = _ban.TrangThai;
                        lblStatus.BackColor = GetStatusColor(_ban.TrangThai);
                        return;
                    }
                }
            }
        }

        private async Task UpdateTimerIfExists()
        {
            var hoaDon = await _banBiaService.GetActiveInvoiceAsync(_ban.MaBan);
            if (hoaDon?.ThoiGianBatDau == null) return;

            // ✅ FIXED: Use LayThoiGianKetThucHopLe() - same as UpdateInfoLabelText
            var thoiGianKetThuc = _gioHoatDongService.LayThoiGianKetThucHopLe(hoaDon.ThoiGianBatDau.Value);
            var duration = thoiGianKetThuc - hoaDon.ThoiGianBatDau.Value;

            foreach (Control ctrl in pnlContent.Controls)
            {
                if (ctrl is Panel pnl && pnl.BackColor == Color.FromArgb(220, 38, 38))
                {
                    var lblTime = pnl.Controls.OfType<Label>()
                        .FirstOrDefault(l => l.Font.Size > 12);

                    if (lblTime != null)
                    {
                        lblTime.Text = $"{(int)duration.TotalHours:D2}h {duration.Minutes:D2}m";
                        return;
                    }
                }
            }
        }

        private async Task UpdatePaymentInfoIfExists()
        {
            var hoaDon = await _banBiaService.GetActiveInvoiceAsync(_ban.MaBan);
            if (hoaDon == null) return;

            // ✅ FIXED: Use LayThoiGianKetThucHopLe() for consistency
            var thoiGianKetThuc = _gioHoatDongService.LayThoiGianKetThucHopLe(hoaDon.ThoiGianBatDau.Value);
            var duration = thoiGianKetThuc - hoaDon.ThoiGianBatDau.Value;

            var tongPhut = (int)Math.Ceiling(duration.TotalMinutes);
            var soGio = (decimal)tongPhut / 60m;
            var giaGioDecimal = _ban.MaLoaiNavigation?.GiaGio ?? 0;
            var tienBan = soGio * giaGioDecimal;

            // Get latest service charges
            var chiTietList = await _banBiaService.GetInvoiceDetailsAsync(hoaDon.MaHd);
            var tienDichVu = chiTietList.Sum(ct => ct.ThanhTien);

            var giamGia = hoaDon.GiamGia ?? 0;
            var tamTinh = tienBan + tienDichVu - giamGia;
            var tongCong = Math.Ceiling((tamTinh ?? 0m) / 1000m) * 1000m;

            foreach (Control ctrl in pnlContent.Controls)
            {
                if (ctrl is Panel pnl)
                {
                    var titleLabel = pnl.Controls.OfType<Label>()
                        .FirstOrDefault(l => l.Text == "CHI TIẾT THANH TOÁN");

                    if (titleLabel != null)
                    {
                        var totalPanel = pnl.Controls.OfType<Panel>()
                            .FirstOrDefault(p => p.BackColor == Color.FromArgb(239, 246, 255) ||
                                                p.BackColor == Color.FromArgb(254, 226, 226));

                        if (totalPanel != null)
                        {
                            var lblTotal = totalPanel.Controls.OfType<Label>()
                                .FirstOrDefault(l => l.Font.Bold && l.Text.Contains("đ"));

                            if (lblTotal != null)
                            {
                                lblTotal.Text = $"{tongCong:N0}đ";
                            }
                        }
                        return;
                    }
                }
            }
        }

        private async Task FullReloadContentOptimized(System.Threading.CancellationToken token)
        {
            // Tạo nội dung mới OFF-SCREEN
            var newContent = new Panel
            {
                AutoScroll = true,
                BackColor = Color.FromArgb(248, 250, 252),
                Padding = new Padding(PADDING + 10, TOP_SPACING, PADDING + 5, PADDING),
                AutoSize = false,
                Location = pnlContent.Location,
                Size = pnlContent.Size,
                Dock = DockStyle.Fill,
                Visible = false // QUAN TRỌNG: Ẩn khi tạo
            };

            int availableWidth = Math.Max(MIN_PANEL_WIDTH, pnlContent.ClientSize.Width - (PADDING * 2) - 20);
            int yPos = 0;

            // Build UI trong background
            yPos = RenderHeader(newContent, yPos, availableWidth);
            yPos += SECTION_SPACING;

            switch (_ban.TrangThai)
            {
                case "Đang chơi":
                    yPos = await RenderPlayingContent(newContent, yPos, availableWidth, token);
                    break;
                case "Đã đặt":
                    yPos = RenderReservedContent(newContent, yPos, availableWidth);
                    break;
                case "Trống":
                    yPos = RenderAvailableContent(newContent, yPos, availableWidth);
                    break;
            }

            if (token.IsCancellationRequested) return;

            // ATOMIC SWAP - Nhanh chóng không flickering
            var oldContent = pnlContent;

            this.SuspendLayout();
            this.Controls.Remove(oldContent);
            pnlContent = newContent;
            pnlContent.Visible = true; // Hiện sau khi add
            this.Controls.Add(pnlContent);
            this.ResumeLayout(false);

            // Dispose cũ sau khi đã swap
            await Task.Delay(50); // Cho UI settle
            oldContent?.Dispose();

            OnBanUpdated?.Invoke(this, _ban);
        }

        #region Render Methods - Modified to accept panel parameter

        private int RenderHeader(Panel targetPanel, int yPos, int panelWidth)
        {
            var pnlHeader = new Panel
            {
                Location = new Point(15, yPos + 20),
                Size = new Size(panelWidth, 90),
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
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(15, 12),
                AutoSize = true,
                MaximumSize = new Size(panelWidth - 130, 0)
            };

            var lblSubtitle = new Label
            {
                Text = $"{_ban.MaLoaiNavigation?.TenLoai ?? ""}",
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(15, 42),
                AutoSize = true
            };

            var lblArea = new Label
            {
                Text = $"{_ban.MaKhuVucNavigation?.TenKhuVuc ?? ""}",
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(15, 60),
                AutoSize = true
            };

            var lblStatus = new Label
            {
                Text = _ban.TrangThai,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = GetStatusColor(_ban.TrangThai),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(100, 30),
                Location = new Point(panelWidth - 105, 28)
            };

            pnlHeader.Controls.AddRange(new Control[] { lblTableName, lblSubtitle, lblArea, lblStatus });
            targetPanel.Controls.Add(pnlHeader);

            return yPos + 85;
        }

        private async Task<int> RenderPlayingContent(Panel targetPanel, int yPos, int panelWidth, System.Threading.CancellationToken token)
        {
            var hoaDon = await _banBiaService.GetActiveInvoiceAsync(_ban.MaBan);
            if (hoaDon == null)
            {
                ShowError("Không tìm thấy hóa đơn đang hoạt động!");
                return yPos;
            }

            if (token.IsCancellationRequested) return yPos;

            // ✅ FIXED: Calculate duration using LayThoiGianKetThucHopLe()
            var thoiGianKetThuc = _gioHoatDongService.LayThoiGianKetThucHopLe(hoaDon.ThoiGianBatDau.Value);
            var duration = thoiGianKetThuc - hoaDon.ThoiGianBatDau.Value;

            // Check various warning conditions
            var isQuaGioChoPhep = _gioHoatDongService.KiemTraBanQuaGioChoPhep(hoaDon.ThoiGianBatDau.Value);
            var isDaDongCua = _gioHoatDongService.DaDenGioDongCua();
            var isSapDongCua = _gioHoatDongService.SapDenGioDongCua();

            if (isQuaGioChoPhep)
            {
                // Over allowed hours - no additional warnings needed
            }
            else
            {
                // Show closing time warnings
                if (isDaDongCua && hoaDon.ThoiGianBatDau < _gioHoatDongService.LayThoiDiemDongCua())
                {
                    yPos = RenderClosedWarning(targetPanel, yPos, panelWidth);
                    yPos += CARD_SPACING;
                }
                else if (isSapDongCua)
                {
                    yPos = RenderClosingSoonWarning(targetPanel, yPos, panelWidth);
                    yPos += CARD_SPACING;
                }
            }

            // Timer Card - pass the correctly calculated duration
            yPos = RenderTimerCard(targetPanel, duration, yPos, panelWidth,
                isQuaGioChoPhep || isDaDongCua || isSapDongCua);
            yPos += CARD_SPACING;

            // Customer Info Card
            yPos = RenderCustomerInfoCard(targetPanel, hoaDon, yPos, panelWidth);
            yPos += CARD_SPACING;

            // Payment Info
            yPos = await RenderPaymentInfo(targetPanel, hoaDon, duration, yPos, panelWidth, token);
            yPos += CARD_SPACING;

            if (token.IsCancellationRequested) return yPos;

            // Service List
            //yPos = await RenderServiceList(targetPanel, hoaDon.MaHd, yPos, panelWidth, token);
            yPos += SECTION_SPACING;

            // Buttons
            yPos = RenderPlayingButtons(targetPanel, yPos, panelWidth, isQuaGioChoPhep || isDaDongCua);

            return yPos;
        }
        private int RenderClosingSoonWarning(Panel targetPanel, int yPos, int panelWidth)
        {
            var phutConLai = _gioHoatDongService.TinhSoPhutConLaiDenDongCua();
            var gioDongCua = _gioHoatDongService.LayThoiDiemDongCua();

            var pnlWarning = new Panel
            {
                Location = new Point(15, yPos + 20),
                Size = new Size(panelWidth, 90),
                BackColor = Color.FromArgb(234, 179, 8) // Vàng
            };

            // Icon
            var lblIcon = new Label
            {
                Font = new Font("Segoe UI", 32F),
                ForeColor = Color.White,
                Location = new Point(15, 15),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlWarning.Controls.Add(lblIcon);

            // Warning text
            var lblWarning = new Label
            {
                Text = $"SẮP ĐÓNG CỬA!\nCòn {phutConLai} phút - Vui lòng chuẩn bị thanh toán\n(Đóng cửa lúc {gioDongCua:HH:mm})",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(80, 12),
                Size = new Size(panelWidth - 100, 70),
                BackColor = Color.Transparent
            };
            pnlWarning.Controls.Add(lblWarning);

            // Blinking animation
            var timer = new System.Windows.Forms.Timer { Interval = 800 };
            var isHighlight = false;
            timer.Tick += (s, e) =>
            {
                if (pnlWarning.IsDisposed)
                {
                    timer.Stop();
                    timer.Dispose();
                    return;
                }

                isHighlight = !isHighlight;
                pnlWarning.BackColor = isHighlight
                    ? Color.FromArgb(251, 191, 36)
                    : Color.FromArgb(234, 179, 8);
            };
            timer.Start();

            targetPanel.Controls.Add(pnlWarning);
            return yPos + 90;
        }
        private int RenderClosedWarning(Panel targetPanel, int yPos, int panelWidth)
        {
            var gioDongCua = _gioHoatDongService.LayThoiDiemDongCua();

            var pnlWarning = new Panel
            {
                Location = new Point(15, yPos + 20),
                Size = new Size(panelWidth, 100),
                BackColor = Color.FromArgb(220, 38, 38) // Đỏ đậm
            };

            // Icon
            var lblIcon = new Label
            {
                Text = "🚨",
                Font = new Font("Segoe UI", 36F),
                ForeColor = Color.White,
                Location = new Point(15, 18),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlWarning.Controls.Add(lblIcon);

            // Warning text
            var lblWarning = new Label
            {
                Text = $"QUÁN ĐÃ ĐÓNG CỬA!\nVui lòng THANH TOÁN NGAY\n(Đã đóng cửa lúc {gioDongCua:HH:mm})",
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(85, 18),
                Size = new Size(panelWidth - 100, 70),
                BackColor = Color.Transparent
            };
            pnlWarning.Controls.Add(lblWarning);

            // Pulsing animation
            var timer = new System.Windows.Forms.Timer { Interval = 600 };
            var isHighlight = false;
            timer.Tick += (s, e) =>
            {
                if (pnlWarning.IsDisposed)
                {
                    timer.Stop();
                    timer.Dispose();
                    return;
                }

                isHighlight = !isHighlight;
                pnlWarning.BackColor = isHighlight
                    ? Color.FromArgb(239, 68, 68)
                    : Color.FromArgb(220, 38, 38);
            };
            timer.Start();

            targetPanel.Controls.Add(pnlWarning);
            return yPos + 100;
        }
        private int RenderTimerCard(Panel targetPanel, TimeSpan duration, int yPos, int panelWidth, bool isDenGioDongCua)
        {
            // ✅ SỬ DỤNG duration ĐÃ ĐƯỢC TÍNH TỪ RenderPlayingContent
            // Làm tròn lên phút - giống UpdateInfoLabelText
            var totalMinutes = (int)Math.Ceiling(duration.TotalMinutes);
            var hours = totalMinutes / 60;
            var minutes = totalMinutes % 60;

            var pnlTimer = new Panel
            {
                Location = new Point(15, yPos + 20),
                Size = new Size(panelWidth, 75),
                BackColor = isDenGioDongCua
                    ? Color.FromArgb(153, 27, 27)
                    : Color.FromArgb(220, 38, 38)
            };

            var lblLabel = new Label
            {
                Text = isDenGioDongCua ? "THỜI GIAN TẠM TÍNH" : "THỜI GIAN CHƠI",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = isDenGioDongCua
                    ? Color.FromArgb(254, 202, 202)
                    : Color.FromArgb(254, 202, 202),
                Location = new Point(15, 12),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            // ✅ Hiển thị duration đã tính đúng
            var lblTime = new Label
            {
                Text = $"{hours:D2}h {minutes:D2}m",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(15, 32),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            pnlTimer.Controls.AddRange(new Control[] { lblLabel, lblTime });
            targetPanel.Controls.Add(pnlTimer);

            return yPos + 75;
        }


        private int RenderCustomerInfoCard(Panel targetPanel, HoaDonEntity hoaDon, int yPos, int panelWidth)
        {
            var pnlCustomer = CreateModernCard(panelWidth);
            pnlCustomer.Location = new Point(15, yPos + 20);

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

            cardY = AddInfoRow(pnlCustomer, "Tên khách", customerName, cardY + 15, panelWidth);
            cardY = AddInfoRow(pnlCustomer, "Điện thoại", customerPhone, cardY, panelWidth);
            cardY = AddInfoRow(pnlCustomer, "Bắt đầu", startTime, cardY, panelWidth);

            cardY += 8;
            pnlCustomer.Height = cardY;
            targetPanel.Controls.Add(pnlCustomer);

            return yPos + cardY;
        }

        private async Task<int> RenderPaymentInfo(Panel targetPanel, HoaDonEntity hoaDon, TimeSpan duration, int yPos, int panelWidth, System.Threading.CancellationToken token)
        {
            // ✅ SỬ DỤNG duration ĐÃ ĐƯỢC TÍNH ĐÚNG từ RenderPlayingContent
            // Làm tròn lên phút
            var tongPhut = (int)Math.Ceiling(duration.TotalMinutes);
            var soGio = (decimal)tongPhut / 60m;
            var giaGioDecimal = _ban.MaLoaiNavigation?.GiaGio ?? 0;
            var tienBan = soGio * giaGioDecimal;

            var tienDichVu = await _banBiaService.GetInvoiceDetailsAsync(hoaDon.MaHd)
                .ContinueWith(t => t.Result.Sum(ct => ct.ThanhTien));

            if (token.IsCancellationRequested) return yPos;

            var giamGia = hoaDon.GiamGia ?? 0;
            var tamTinh = tienBan + tienDichVu - giamGia;
            var tongCong = Math.Ceiling((tamTinh ?? 0m) / 1000m) * 1000m;
            var chenhLech = tongCong - tamTinh;

            var pnlPayment = CreateModernCard(panelWidth);
            pnlPayment.Location = new Point(15, yPos + 20);

            int cardY = 12;

            // ============================================================
            // Check closing time status
            // ============================================================
            var gioDongCua = _gioHoatDongService.LayThoiDiemDongCuaTheoBanBatDau(hoaDon.ThoiGianBatDau.Value);
            var isDenGioDongCua = DateTime.Now >= gioDongCua;

            if (isDenGioDongCua)
            {
                // Warning badge for temporary calculation
                var pnlWarning = new Panel
                {
                    Location = new Point(12, cardY),
                    Size = new Size(panelWidth - 24, 75),
                    BackColor = Color.FromArgb(254, 243, 199)
                };

                var lblWarningText = new Label
                {
                    Text = "TẠM TÍNH - Đã đến giờ đóng cửa\nVui lòng thanh toán ngay",
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(146, 64, 14),
                    Location = new Point(50, 8),
                    Size = new Size(panelWidth - 80, 75),
                    BackColor = Color.Transparent
                };

                pnlWarning.Controls.AddRange(new Control[] { lblWarningText });
                pnlPayment.Controls.Add(pnlWarning);
                cardY += 85;

            }

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
                BackColor = isDenGioDongCua
                    ? Color.FromArgb(254, 226, 226)
                    : Color.FromArgb(239, 246, 255)
            };

            var lblTotalLabel = new Label
            {
                Text = isDenGioDongCua ? "TỔNG TẠM TÍNH" : "TỔNG CỘNG",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = isDenGioDongCua
                    ? Color.FromArgb(153, 27, 27)
                    : Color.FromArgb(30, 64, 175),
                Location = new Point(10, 11),
                AutoSize = true
            };

            var lblTotalValue = new Label
            {
                Text = $"{tongCong:N0}đ",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 38, 38),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleRight,
                Size = new Size(panelWidth - 160, 26),
                Location = new Point(panelWidth - 270, 8)
            };

            pnlTotal.Controls.AddRange(new Control[] { lblTotalLabel, lblTotalValue });
            pnlPayment.Controls.Add(pnlTotal);
            cardY += 50;

            pnlPayment.Height = cardY;
            targetPanel.Controls.Add(pnlPayment);

            return yPos + cardY;
        }
        private int RenderServiceItem(Panel parentPanel, ChiTietHoaDonEntity item, int yPos, int panelWidth)
        {
            var pnlService = new Panel
            {
                Location = new Point(10, yPos),
                Size = new Size(panelWidth - 20, 60),
                BackColor = Color.FromArgb(248, 250, 252)
            };

            // QUAN TRỌNG: Sử dụng dữ liệu từ item (đã được load mới từ database)
            var tenDichVu = item.MaDvNavigation?.TenDv ?? "Dịch vụ";
            var soLuong = item.SoLuong;
            var donGia = item.MaDvNavigation?.Gia ?? 0;
            var thanhTien = item.ThanhTien ?? 0;

            var lblName = new Label
            {
                Text = tenDichVu,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(10, 8),
                AutoSize = true,
                MaximumSize = new Size(panelWidth - 160, 0)
            };

            var lblQuantity = new Label
            {
                Text = $"SL: {soLuong} x {donGia:N0}đ",
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(10, 30),
                AutoSize = true
            };

            var lblPrice = new Label
            {
                Text = $"{thanhTien:N0}đ",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 38, 38),
                AutoSize = false,
                Size = new Size(95, 25),
                Location = new Point(panelWidth - 150, 14),
                TextAlign = ContentAlignment.MiddleRight
            };

            var btnDelete = new Button
            {
                Text = "X",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Size = new Size(35, 28),
                Location = new Point(panelWidth - 55, 15),
                BackColor = Color.FromArgb(239, 68, 68),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Tag = item.Id  // QUAN TRỌNG: Lưu ID của chi tiết hóa đơn
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += BtnDeleteService_Click;

            pnlService.Controls.AddRange(new Control[] { lblName, lblQuantity, lblPrice, btnDelete });
            parentPanel.Controls.Add(pnlService);

            return yPos + 72;
        }
        private int RenderPlayingButtons(Panel targetPanel, int yPos, int panelWidth, bool isDenGioDongCua)
        {
            // ============================================================
            // ✅ NÚT THÊM DỊCH VỤ: LUÔN LUÔN BẬT
            // ============================================================
            var btnThemDV = CreateModernButton("Thêm dịch vụ", Color.FromArgb(59, 130, 246), panelWidth);
            btnThemDV.Location = new Point(15, yPos + 20);
            btnThemDV.Click += BtnThemDV_Click;
            // ✅ BỎ: Không disable nút này nữa
            targetPanel.Controls.Add(btnThemDV);
            yPos += 48;

            // ============================================================
            // ✅ NÚT THANH TOÁN: Highlight khi đã đóng cửa
            // ============================================================
            var btnThanhToan = CreateModernButton(
                isDenGioDongCua ? "⚠️ THANH TOÁN NGAY" : "Thanh toán",
                Color.FromArgb(34, 197, 94),
                panelWidth
            );
            btnThanhToan.Location = new Point(15, yPos + 20);
            btnThanhToan.Click += BtnThanhToan_Click;

            // Highlight nút thanh toán khi đóng cửa
            if (isDenGioDongCua)
            {
                btnThanhToan.BackColor = Color.FromArgb(220, 38, 38);
                btnThanhToan.Font = new Font("Segoe UI", 11F, FontStyle.Bold);

                // Pulsing effect
                var timer = new System.Windows.Forms.Timer { Interval = 500 };
                var isHighlight = false;
                timer.Tick += (s, e) =>
                {
                    if (btnThanhToan.IsDisposed)
                    {
                        timer.Stop();
                        timer.Dispose();
                        return;
                    }

                    isHighlight = !isHighlight;
                    btnThanhToan.BackColor = isHighlight
                        ? Color.FromArgb(239, 68, 68)
                        : Color.FromArgb(220, 38, 38);
                };
                timer.Start();
            }

            targetPanel.Controls.Add(btnThanhToan);
            yPos += 48;

            return yPos;
        }

        private int RenderReservedContent(Panel targetPanel, int yPos, int panelWidth)
        {
            var pnlReserved = CreateModernCard(panelWidth);
            pnlReserved.Location = new Point(15, yPos + 20);
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
            targetPanel.Controls.Add(pnlReserved);
            yPos += cardY + CARD_SPACING;

            yPos = RenderReservedButtons(targetPanel, yPos, panelWidth);
            return yPos;
        }

        private int RenderReservedButtons(Panel targetPanel, int yPos, int panelWidth)
        {
            var btnConfirm = CreateModernButton("Xác nhận khách đến", Color.FromArgb(34, 197, 94), panelWidth);
            btnConfirm.Location = new Point(15, yPos + 20);
            btnConfirm.Click += BtnConfirm_Click;
            targetPanel.Controls.Add(btnConfirm);
            yPos += 48;

            var btnCancel = CreateModernButton("Hủy đặt bàn", Color.FromArgb(239, 68, 68), panelWidth);
            btnCancel.Location = new Point(15, yPos + 20);
            btnCancel.Click += BtnCancel_Click;
            targetPanel.Controls.Add(btnCancel);
            yPos += 48;

            return yPos;
        }

        private int RenderAvailableContent(Panel targetPanel, int yPos, int panelWidth)
        {
            var pnlAvailable = CreateModernCard(panelWidth);
            pnlAvailable.Location = new Point(15, yPos + 20);
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
            targetPanel.Controls.Add(pnlAvailable);
            yPos += cardY + CARD_SPACING;

            yPos = RenderAvailableButtons(targetPanel, yPos, panelWidth);
            return yPos;
        }

        private int RenderAvailableButtons(Panel targetPanel, int yPos, int panelWidth)
        {
            var btnBatDau = CreateModernButton("Bắt đầu chơi", Color.FromArgb(34, 197, 94), panelWidth);
            btnBatDau.Location = new Point(15, yPos + 20);
            btnBatDau.Click += BtnBatDau_Click;
            targetPanel.Controls.Add(btnBatDau);
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
                Size = new Size(panelWidth - 24, 35),
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
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = false,
                Size = new Size((panelWidth - 30) / 2, 26),
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

            if (result != DialogResult.Yes) return;

            try
            {
                btn.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                var success = await _hoaDonService.RemoveServiceFromInvoiceAsync(chiTietId);

                if (success)
                {
                    // ✅ DELAY để đợi DB commit
                    await Task.Delay(200);

                    // ✅ Trigger event TRƯỚC để update card ngay
                    OnDataChanged?.Invoke(this, EventArgs.Empty);

                    // ✅ Delay nhỏ để UI update
                    await Task.Delay(100);

                    // Force reload detail
                    await LoadBanDetail(forceReload: true);

                    MessageBox.Show("Đã xóa dịch vụ!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            finally
            {
                btn.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }
        private async void BtnBatDau_Click(object sender, EventArgs e)
{
    try
    {
        this.Cursor = Cursors.WaitCursor;
        
        // Lần gọi đầu tiên - kiểm tra cảnh báo
        var result = await _banBiaService.StartTableAsync(_ban.MaBan, _maNV);
        
        this.Cursor = Cursors.Default;

        // Nếu cần xác nhận từ user
        if (result.needConfirmation)
        {
            var confirmResult = MessageBox.Show(
                result.message,
                "Cảnh báo",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.Yes)
            {
                // Gọi lại với skipWarning = true
                this.Cursor = Cursors.WaitCursor;
                result = await _banBiaService.StartTableAsync(_ban.MaBan, _maNV, skipWarning: true);
                this.Cursor = Cursors.Default;
            }
            else
            {
                return; // User hủy
            }
        }

        if (result.isSuccess)
        {
            MessageBox.Show($"Đã bắt đầu chơi tại {_ban.TenBan}", "Thành công",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            OnDataChanged?.Invoke(this, EventArgs.Empty);
            await LoadBanDetail();
        }
        else
        {
            if (!result.needConfirmation) // Chỉ hiện error nếu không phải confirmation
            {
                ShowError(result.message);
            }
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
                    ShowError("Không tìm thấy hóa đơn!");
                    return;
                }

                var dichVuService = Program.GetService<DichVuService>();

                using (var themDichVuForm = new ThemDichVuForm(dichVuService, _hoaDonService, hoaDon.MaHd))
                {
                    var result = themDichVuForm.ShowDialog(this);

                    if (result == DialogResult.OK)
                    {
                        this.Cursor = Cursors.WaitCursor;

                        // ✅ DELAY để đợi DB commit
                        await Task.Delay(200);

                        // ✅ Trigger event TRƯỚC để update card
                        OnDataChanged?.Invoke(this, EventArgs.Empty);

                        // ✅ Delay nhỏ
                        await Task.Delay(100);

                        // Force reload
                        await LoadBanDetail(forceReload: true);

                        this.Cursor = Cursors.Default;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                ShowError($"Lỗi: {ex.Message}");
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
                        // ✅ DELAY trước khi reload để đợi DB commit
                        await Task.Delay(200);

                        MessageBox.Show(
                            $"Đã thanh toán thành công!\nBàn {_ban.TenBan} đã được trả về trống.",
                            "Thành công",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        // ✅ Trigger event trước để cập nhật list
                        OnDataChanged?.Invoke(this, EventArgs.Empty);

                        // ✅ Delay thêm trước khi reload detail
                        await Task.Delay(100);

                        // ✅ Force reload với scope mới
                        await LoadBanDetail(forceReload: true);
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