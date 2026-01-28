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
        private readonly List<System.Windows.Forms.Timer> _animationTimers = new List<System.Windows.Forms.Timer>();

        // ✅ THÊM MỚI: Timer và flag cho auto-check
        private System.Windows.Forms.Timer _autoCheckTimer;
        private bool _isCheckingReservation = false;

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

            // ✅ THÊM MỚI: Khởi tạo timer tự động kiểm tra đơn đặt bàn
            _autoCheckTimer = new System.Windows.Forms.Timer();
            _autoCheckTimer.Interval = 30000; // Check mỗi 30 giây
            _autoCheckTimer.Tick += AutoCheckReservation_Tick;
            _autoCheckTimer.Start();
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
            if (_isLoading && !forceReload) return;

            // ✅ THÊM: Dừng timer tạm thời khi reload
            var timerWasRunning = _autoCheckTimer?.Enabled ?? false;
            _autoCheckTimer?.Stop();

            _cts?.Cancel();
            _cts = new System.Threading.CancellationTokenSource();
            var token = _cts.Token;

            _isLoading = true;
            try
            {
                if (forceReload)
                {
                    await Task.Delay(50, token);
                }
                else
                {
                    await Task.Delay(200, token);
                }

                if (token.IsCancellationRequested) return;

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

                // ✅ THÊM: Log trạng thái để debug
                if (_ban.TrangThai != newBan.TrangThai)
                {
                    System.Diagnostics.Debug.WriteLine($"🔄 Bàn {_ban.MaBan}: {_ban.TrangThai} → {newBan.TrangThai}");
                }

                _ban = newBan;

                if (forceReload)
                {
                    await FullReloadContentOptimized(token);
                }
                else if (!HasDataChanged(_ban, newBan) && pnlContent.Controls.Count > 0)
                {
                    // ✅ CHỈ UPDATE NẾU KHÔNG CÓ THAY ĐỔI TRẠNG THÁI
                    await UpdateExistingControlsAsync();
                }
                else
                {
                    await FullReloadContentOptimized(token);
                }

                // ✅ CHỈ KIỂM TRA ĐẶT BÀN KHI BÀN TRỐNG/ĐÃ ĐẶT
                if (_ban.TrangThai == "Trống" || _ban.TrangThai == "Đã đặt")
                {
                    try
                    {
                        var datBanService = Program.GetService<DatBanService>();
                        var nextReservation = await datBanService.GetNextReservationForTableAsync(_ban.MaBan);

                        if (nextReservation != null)
                        {
                            System.Diagnostics.Debug.WriteLine($"✓ Tìm thấy đơn đặt tiếp theo cho bàn {_ban.MaBan}");

                            if (_ban.TrangThai == "Trống")
                            {
                                _ban.TrangThai = "Đã đặt";
                                await FullReloadContentOptimized(token);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ Lỗi khi kiểm tra đơn đặt tiếp theo: {ex.Message}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                System.Diagnostics.Debug.WriteLine("ℹ️ LoadBanDetail bị cancel");
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi tải dữ liệu: {ex.Message}");
            }
            finally
            {
                _isLoading = false;

                // ✅ THÊM: Bật lại timer nếu nó đang chạy trước đó VÀ không bị cancel
                if (timerWasRunning && !token.IsCancellationRequested)
                {
                    _autoCheckTimer?.Start();
                }
            }
        }
        private async Task UpdateExistingControlsAsync()
        {
            UpdateHeaderIfExists();

            // ✅ THÊM: Load lại thông tin bàn mới nhất trước khi check
            try
            {
                var latestBan = await _banBiaService.GetTableByIdAsync(_ban.MaBan);
                if (latestBan != null)
                {
                    _ban = latestBan;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"⚠️ Lỗi khi refresh thông tin bàn: {ex.Message}");
            }

            // Chỉ update timer và payment info nếu bàn VẪN đang chơi
            if (_ban.TrangThai == "Đang chơi")
            {
                try
                {
                    await UpdateTimerIfExists();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"⚠️ Lỗi UpdateTimerIfExists: {ex.Message}");
                }

                try
                {
                    await UpdatePaymentInfoIfExists();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"⚠️ Lỗi UpdatePaymentInfoIfExists: {ex.Message}");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"ℹ️ Bàn không còn 'Đang chơi' ({_ban.TrangThai}), bỏ qua update timer/payment");
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
        private async void AutoCheckReservation_Tick(object sender, EventArgs e)
        {
            if (_isCheckingReservation) return;

            // Chỉ kiểm tra với bàn "Đã đặt" hoặc "Đang chờ"
            if (_ban.TrangThai != "Đã đặt" && _ban.TrangThai != "Đang chờ")
                return;

            _isCheckingReservation = true;
            try
            {
                var datBanService = Program.GetService<DatBanService>();
                var datBans = await datBanService.GetByTableAsync(_ban.MaBan);
                var activeDatBan = datBans.FirstOrDefault(d =>
                    d.TrangThai == "Đang chờ" || d.TrangThai == "Đã đặt");

                if (activeDatBan == null || !activeDatBan.ThoiGianBatDau.HasValue)
                {
                    _isCheckingReservation = false;
                    return;
                }

                var now = DateTime.Now;
                var gioBatDau = activeDatBan.ThoiGianBatDau.Value;
                var minutesLate = (now - gioBatDau).TotalMinutes;

                // ✅ AUTO HỦY SAU 15 PHÚT - KHÔNG CẦN XÁC NHẬN
                if (minutesLate >= 15)
                {
                    System.Diagnostics.Debug.WriteLine($"⏰ Auto hủy đặt bàn #{activeDatBan.MaDat} - Đã quá 15 phút");

                    var success = await _banBiaService.CancelReservationAsync(activeDatBan.MaDat);

                    if (success)
                    {
                        System.Diagnostics.Debug.WriteLine("✓ Đã tự động hủy đặt bàn");

                        // Reload để hiển thị ca tiếp theo hoặc trạng thái trống
                        OnDataChanged?.Invoke(this, EventArgs.Empty);
                        await LoadBanDetail(forceReload: true);

                        // Thông báo cho nhân viên
                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show(
                                $"🔔 ĐÃ TỰ ĐỘNG HỦY ĐẶT BÀN\n\n" +
                                $"Bàn: {_ban.TenBan}\n" +
                                $"Khách: {activeDatBan.TenKhach}\n" +
                                $"Lý do: Khách không đến sau 15 phút\n\n" +
                                $"Hệ thống đã tự động hủy đặt bàn.",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }));
                    }
                }
                // ✅ THÔNG BÁO CẢNH BÁO SAU 10 PHÚT
                else if (minutesLate >= 10)
                {
                    System.Diagnostics.Debug.WriteLine($"⚠️ Cảnh báo: Đơn đặt #{activeDatBan.MaDat} đã quá 10 phút");
                    await LoadBanDetail(forceReload: true);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Lỗi khi kiểm tra đơn đặt: {ex.Message}");
            }
            finally
            {
                _isCheckingReservation = false;
            }
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
                        .FirstOrDefault(l =>
                            l.Text == "Trống" ||
                            l.Text == "Đang chơi" ||
                            l.Text == "Đã đặt" ||
                            l.Text == "Bảo trì");

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
            // ✅ THÊM: Kiểm tra trạng thái trước
            if (_ban.TrangThai != "Đang chơi")
            {
                System.Diagnostics.Debug.WriteLine($"⚠️ UpdateTimerIfExists: Bàn không ở trạng thái 'Đang chơi' ({_ban.TrangThai})");
                return;
            }

            var hoaDon = await _banBiaService.GetActiveInvoiceAsync(_ban.MaBan);

            // ✅ THÊM: Xử lý null an toàn
            if (hoaDon?.ThoiGianBatDau == null)
            {
                System.Diagnostics.Debug.WriteLine($"⚠️ UpdateTimerIfExists: Không tìm thấy hóa đơn hoặc thiếu ThoiGianBatDau");
                return;
            }

            var thoiGianKetThuc = _gioHoatDongService.LayThoiGianKetThucHopLe(hoaDon.ThoiGianBatDau.Value);
            var duration = thoiGianKetThuc - hoaDon.ThoiGianBatDau.Value;
            var totalMinutes = (int)Math.Ceiling(duration.TotalMinutes);
            var hours = totalMinutes / 60;
            var minutes = totalMinutes % 60;

            foreach (Control ctrl in pnlContent.Controls)
            {
                if (ctrl is Panel pnl)
                {
                    var lblTime = pnl.Controls.OfType<Label>()
                        .FirstOrDefault(l => l.Text.Contains("h") && l.Text.Contains("m") && l.Font.Size > 12);

                    if (lblTime != null)
                    {
                        lblTime.Text = $"{hours:D2}h {minutes:D2}m";
                        return;
                    }
                }
            }
        }
        private async Task UpdatePaymentInfoIfExists()
        {
            // ✅ THÊM: Kiểm tra trạng thái trước
            if (_ban.TrangThai != "Đang chơi")
            {
                System.Diagnostics.Debug.WriteLine($"⚠️ UpdatePaymentInfoIfExists: Bàn không ở trạng thái 'Đang chơi' ({_ban.TrangThai})");
                return;
            }

            var hoaDon = await _banBiaService.GetActiveInvoiceAsync(_ban.MaBan);

            // ✅ THÊM: Xử lý null an toàn
            if (hoaDon == null)
            {
                System.Diagnostics.Debug.WriteLine($"⚠️ UpdatePaymentInfoIfExists: Không tìm thấy hóa đơn");
                return;
            }

            // ✅ THÊM: Kiểm tra thời gian bắt đầu
            if (!hoaDon.ThoiGianBatDau.HasValue)
            {
                System.Diagnostics.Debug.WriteLine($"⚠️ UpdatePaymentInfoIfExists: Hóa đơn thiếu ThoiGianBatDau");
                return;
            }

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
                    // ✅ CẬP NHẬT: Thêm await vì phương thức đã đổi thành async
                    yPos = await RenderReservedContent(newContent, yPos, availableWidth);
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
        private async Task<int> RenderWaitingContent(Panel targetPanel, int yPos, int panelWidth)
        {
            DatBan bookingInfo = null;
            try
            {
                var datBanService = Program.GetService<DatBanService>();
                var datBans = await datBanService.GetByTableAsync(_ban.MaBan);

                // ✅ LẤY ĐƠN ĐẶT ĐANG HOẠT ĐỘNG (Đang chờ hoặc Đã đặt)
                bookingInfo = datBans
                    .Where(d => d.TrangThai == "Đang chờ" || d.TrangThai == "Đã đặt")
                    .Where(d => d.ThoiGianBatDau.HasValue && d.ThoiGianKetThuc.HasValue)
                    .OrderBy(d => d.ThoiGianBatDau)
                    .FirstOrDefault();

                System.Diagnostics.Debug.WriteLine($"📋 Tìm thấy {datBans.Count} đơn đặt cho bàn {_ban.MaBan}");
                if (bookingInfo != null)
                {
                    System.Diagnostics.Debug.WriteLine($"   ✓ Đơn đang hoạt động: #{bookingInfo.MaDat} - {bookingInfo.TenKhach}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Lỗi khi lấy thông tin đặt bàn: {ex.Message}");
            }

            // Nếu không có bookingInfo, có thể là đã hết ca
            if (bookingInfo == null)
            {
                System.Diagnostics.Debug.WriteLine($"⚠️ Không có đơn đặt đang hoạt động - Kiểm tra ca tiếp theo");
                await Task.Delay(100);
                OnDataChanged?.Invoke(this, EventArgs.Empty);
                return yPos;
            }

            // ============================================================
            // KIỂM TRA THỜI GIAN TRỄ
            // ============================================================
            var now = DateTime.Now;
            var gioBatDau = bookingInfo.ThoiGianBatDau.Value;
            var minutesLate = (now - gioBatDau).TotalMinutes;

            // ============================================================
            // CARD: CẢNH BÁO TRẠNG THÁI
            // ============================================================
            Color warningColor;
            string warningIcon;
            string warningTitle;
            string warningText;

            if (minutesLate >= 10 && minutesLate < 15)
            {
                // ⚠️ QUÁ 10 PHÚT - CẢNH BÁO NGHIÊM TRỌNG
                warningColor = Color.FromArgb(239, 68, 68); // Đỏ
                warningIcon = "⚠️";
                warningTitle = "CẢNH BÁO - QUÁ 10 PHÚT!";
                warningText = $"Khách chưa đến sau {(int)minutesLate} phút\n" +
                             $"Sẽ TỰ ĐỘNG HỦY sau {15 - (int)minutesLate} phút nữa";
            }
            else if (minutesLate >= 5 && minutesLate < 10)
            {
                // ⏰ QUÁ 5 PHÚT - CẢNH BÁO
                warningColor = Color.FromArgb(234, 179, 8); // Vàng
                warningIcon = "⏰";
                warningTitle = "KHÁCH CHƯA ĐẾN";
                warningText = $"Đã quá {(int)minutesLate} phút từ giờ hẹn\n" +
                             $"Vui lòng liên hệ khách hàng";
            }
            else
            {
                // ⏳ BÌNH THƯỜNG - CHỜ XÁC NHẬN
                warningColor = Color.FromArgb(59, 130, 246); // Xanh dương
                warningIcon = "⏳";
                warningTitle = "CHỜ XÁC NHẬN";
                warningText = "Vui lòng xác nhận khi khách đến";
            }

            var pnlWarning = new Panel
            {
                Location = new Point(15, yPos + 20),
                Size = new Size(panelWidth, 80),
                BackColor = warningColor
            };

            var lblIcon = new Label
            {
                Text = warningIcon,
                Font = new Font("Segoe UI", 28F),
                ForeColor = Color.White,
                Location = new Point(15, 15),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlWarning.Controls.Add(lblIcon);

            var lblWarningText = new Label
            {
                Text = $"{warningTitle}\n{warningText}",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(75, 10),
                Size = new Size(panelWidth - 100, 65),
                BackColor = Color.Transparent
            };
            pnlWarning.Controls.Add(lblWarningText);

            targetPanel.Controls.Add(pnlWarning);
            yPos += 85 + CARD_SPACING;

            // ============================================================
            // CARD 2: THÔNG TIN THỜI GIAN ĐẶT BÀN
            // ============================================================
            var pnlTimeInfo = CreateModernCard(panelWidth);
            pnlTimeInfo.Location = new Point(15, yPos + 20);
            pnlTimeInfo.BackColor = Color.FromArgb(240, 249, 255);

            int cardY = 12;

            var lblTitle = new Label
            {
                Text = "⏰ THỜI GIAN ĐẶT BÀN",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 58, 138),
                Location = new Point(12, cardY),
                AutoSize = true
            };
            pnlTimeInfo.Controls.Add(lblTitle);
            cardY += 35;

            // Giờ bắt đầu
            var lblStartTime = new Label
            {
                Text = $"Bắt đầu: {gioBatDau:HH:mm, dd/MM/yyyy}",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(12, cardY),
                AutoSize = true
            };
            pnlTimeInfo.Controls.Add(lblStartTime);
            cardY += 28;

            // Giờ kết thúc
            var gioKetThuc = bookingInfo.ThoiGianKetThuc.Value;
            var lblEndTime = new Label
            {
                Text = $"Kết thúc: {gioKetThuc:HH:mm, dd/MM/yyyy}",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(12, cardY),
                AutoSize = true
            };
            pnlTimeInfo.Controls.Add(lblEndTime);
            cardY += 28;

            // Thời lượng dự kiến
            var duration = gioKetThuc - gioBatDau;
            var hours = (int)duration.TotalHours;
            var minutes = duration.Minutes;
            var lblDuration = new Label
            {
                Text = $"Thời lượng: {hours}h {minutes}m",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(12, cardY),
                AutoSize = true
            };
            pnlTimeInfo.Controls.Add(lblDuration);
            cardY += 28;

            // Countdown hoặc thông báo trễ
            if (DateTime.Now < gioBatDau)
            {
                var timeUntil = gioBatDau - DateTime.Now;
                var daysUntil = (int)timeUntil.TotalDays;
                var hoursUntil = timeUntil.Hours;
                var minutesUntil = timeUntil.Minutes;

                var pnlCountdown = new Panel
                {
                    Location = new Point(12, cardY),
                    Size = new Size(panelWidth - 24, 40),
                    BackColor = Color.FromArgb(219, 234, 254)
                };

                var lblCountdown = new Label
                {
                    Text = daysUntil > 0
                        ? $"⏳ Còn {daysUntil} ngày {hoursUntil}h {minutesUntil}m"
                        : $"⏳ Còn {hoursUntil}h {minutesUntil}m",
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(30, 64, 175),
                    Location = new Point(10, 10),
                    AutoSize = true,
                    BackColor = Color.Transparent
                };
                pnlCountdown.Controls.Add(lblCountdown);
                pnlTimeInfo.Controls.Add(pnlCountdown);
                cardY += 45;
            }
            else if (minutesLate > 0)
            {
                // Hiển thị thời gian trễ
                var pnlLate = new Panel
                {
                    Location = new Point(12, cardY),
                    Size = new Size(panelWidth - 24, 40),
                    BackColor = minutesLate >= 10
                        ? Color.FromArgb(254, 226, 226) // Đỏ nhạt nếu >= 10p
                        : Color.FromArgb(254, 243, 199) // Vàng nhạt nếu < 10p
                };

                var lblLate = new Label
                {
                    Text = $"⚠️ Trễ {(int)minutesLate} phút - Tự động hủy sau {15 - (int)minutesLate} phút",
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = minutesLate >= 10
                        ? Color.FromArgb(153, 27, 27)
                        : Color.FromArgb(146, 64, 14),
                    Location = new Point(10, 10),
                    AutoSize = true,
                    BackColor = Color.Transparent
                };
                pnlLate.Controls.Add(lblLate);
                pnlTimeInfo.Controls.Add(pnlLate);
                cardY += 45;
            }

            cardY += 8;
            pnlTimeInfo.Height = cardY;
            targetPanel.Controls.Add(pnlTimeInfo);
            yPos += cardY + CARD_SPACING;

            // ============================================================
            // CARD 3: THÔNG TIN KHÁCH HÀNG
            // ============================================================
            var pnlCustomer = CreateModernCard(panelWidth);
            pnlCustomer.Location = new Point(15, yPos + 20);
            pnlCustomer.BackColor = Color.FromArgb(240, 249, 255);

            int customerCardY = 12;

            var lblCustomerTitle = new Label
            {
                Text = "👤 THÔNG TIN KHÁCH HÀNG",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(12, customerCardY),
                AutoSize = true
            };
            pnlCustomer.Controls.Add(lblCustomerTitle);
            customerCardY += 28;

            // Thông tin khách từ booking
            if (bookingInfo.MaKhNavigation != null)
            {
                customerCardY = AddInfoRow(pnlCustomer, "Tên khách",
                    bookingInfo.MaKhNavigation.TenKh, customerCardY, panelWidth);
                customerCardY = AddInfoRow(pnlCustomer, "Điện thoại",
                    bookingInfo.MaKhNavigation.Sdt ?? "-", customerCardY, panelWidth);

                if (!string.IsNullOrEmpty(bookingInfo.MaKhNavigation.Email))
                {
                    customerCardY = AddInfoRow(pnlCustomer, "Email",
                        bookingInfo.MaKhNavigation.Email, customerCardY, panelWidth);
                }
            }
            else
            {
                customerCardY = AddInfoRow(pnlCustomer, "Tên khách",
                    bookingInfo.TenKhach ?? "Khách đặt", customerCardY, panelWidth);
                customerCardY = AddInfoRow(pnlCustomer, "Điện thoại",
                    bookingInfo.Sdt ?? "-", customerCardY, panelWidth);
            }

            // Giá giờ
            var giaGio = _ban.MaLoaiNavigation?.GiaGio ?? 0;
            customerCardY = AddInfoRow(pnlCustomer, "Giá giờ", $"{giaGio:N0}đ/giờ", customerCardY, panelWidth);

            // Tiền dự kiến
            var totalMinutes = (int)Math.Ceiling(duration.TotalMinutes);
            var soGio = (decimal)totalMinutes / 60m;
            var tienDuKien = soGio * giaGio;
            customerCardY = AddInfoRow(pnlCustomer, "Tiền dự kiến", $"{tienDuKien:N0}đ", customerCardY, panelWidth);

            // Ghi chú
            if (!string.IsNullOrEmpty(bookingInfo.GhiChu))
            {
                customerCardY = AddInfoRow(pnlCustomer, "Ghi chú", bookingInfo.GhiChu, customerCardY, panelWidth);
            }

            customerCardY += 8;
            pnlCustomer.Height = customerCardY;
            targetPanel.Controls.Add(pnlCustomer);
            yPos += customerCardY + CARD_SPACING;

            // ============================================================
            // BUTTONS
            // ============================================================
            yPos = RenderWaitingButtons(targetPanel, yPos, panelWidth);
            return yPos;
        }
        private int RenderWaitingButtons(Panel targetPanel, int yPos, int panelWidth)
        {
            var btnConfirm = CreateModernButton("✓ Xác nhận khách đến", Color.FromArgb(34, 197, 94), panelWidth);
            btnConfirm.Location = new Point(15, yPos + 20);
            btnConfirm.Click += BtnConfirm_Click;
            targetPanel.Controls.Add(btnConfirm);
            yPos += 48;

            var btnCancel = CreateModernButton("✕ Hủy đặt bàn", Color.FromArgb(239, 68, 68), panelWidth);
            btnCancel.Location = new Point(15, yPos + 20);
            btnCancel.Click += BtnCancel_Click;
            targetPanel.Controls.Add(btnCancel);
            yPos += 48;

            return yPos;
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
                BackColor = GetStatusColor(_ban.TrangThai), // ✅ Sử dụng GetStatusColor đã cập nhật
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
            yPos = await RenderServiceList(targetPanel, hoaDon.MaHd, yPos, panelWidth, token);
            yPos += SECTION_SPACING;

            // Buttons
            yPos = RenderPlayingButtons(targetPanel, yPos, panelWidth, isQuaGioChoPhep || isDaDongCua);

            return yPos;
        }
        private async Task<int> RenderServiceList(Panel targetPanel, int maHd, int yPos, int panelWidth, System.Threading.CancellationToken token)
        {
            try
            {
                // Lấy danh sách dịch vụ từ database
                var chiTietList = await _banBiaService.GetInvoiceDetailsAsync(maHd);

                if (token.IsCancellationRequested) return yPos;

                // Nếu không có dịch vụ nào, không hiển thị section
                if (chiTietList == null || !chiTietList.Any())
                {
                    return yPos;
                }

                // Card container cho danh sách dịch vụ
                var pnlServices = CreateModernCard(panelWidth);
                pnlServices.Location = new Point(15, yPos + 20);
                pnlServices.AutoSize = false;

                int cardY = 12;

                // Tiêu đề
                var lblTitle = new Label
                {
                    Text = "DỊCH VỤ ĐÃ SỬ DỤNG",
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(30, 41, 59),
                    Location = new Point(12, cardY),
                    AutoSize = true
                };
                pnlServices.Controls.Add(lblTitle);
                cardY += 35;

                // Container cho các service items
                var pnlServiceItems = new Panel
                {
                    Location = new Point(2, cardY),
                    Size = new Size(panelWidth - 4, 0),
                    AutoSize = false,
                    BackColor = Color.White
                };

                int itemY = 0;
                foreach (var item in chiTietList)
                {
                    if (token.IsCancellationRequested) return yPos;

                    itemY = RenderServiceItem(pnlServiceItems, item, itemY, panelWidth - 4);
                }

                pnlServiceItems.Height = itemY + 10;
                pnlServices.Controls.Add(pnlServiceItems);
                cardY += pnlServiceItems.Height;

                // Tổng tiền dịch vụ
                var tongTienDV = chiTietList.Sum(ct => ct.ThanhTien ?? 0);

                var pnlTotal = new Panel
                {
                    Location = new Point(12, cardY + 10),
                    Size = new Size(panelWidth - 24, 35),
                    BackColor = Color.FromArgb(241, 245, 249)
                };

                var lblTotalLabel = new Label
                {
                    Text = "Tổng tiền dịch vụ:",
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(71, 85, 105),
                    Location = new Point(10, 8),
                    AutoSize = true
                };

                var lblTotalValue = new Label
                {
                    Text = $"{tongTienDV:N0}đ",
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(220, 38, 38),
                    AutoSize = false,
                    Size = new Size(120, 25),
                    Location = new Point(panelWidth - 160, 6),
                    TextAlign = ContentAlignment.MiddleRight
                };

                pnlTotal.Controls.AddRange(new Control[] { lblTotalLabel, lblTotalValue });
                pnlServices.Controls.Add(pnlTotal);
                cardY += 45;

                pnlServices.Height = cardY + 10;
                targetPanel.Controls.Add(pnlServices);

                return yPos + cardY + 10;
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi hiển thị dịch vụ: {ex.Message}");
                return yPos;
            }
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
            var hours = (int)duration.TotalHours;
            var minutes = duration.Minutes;
            var seconds = duration.Seconds;

            var pnlTimer = new Panel
            {
                Size = new Size(panelWidth - 40, 100),
                Location = new Point(20, yPos),
                BackColor = Color.White,
                Padding = new Padding(20),
            };

            var lblLabel = new Label
            {
                Text = "⏱️ Thời gian chơi",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(20, 15),
                AutoSize = true
            };

            var lblTime = new Label
            {
                Text = $"{hours:D2}h {minutes:D2}m",
                Font = new Font("Segoe UI Semibold", 18),
                ForeColor = isDenGioDongCua ? Color.FromArgb(220, 38, 38) : Color.FromArgb(15, 23, 42),
                Location = new Point(20, 40),
                AutoSize = true
            };

            pnlTimer.Controls.AddRange(new Control[] { lblLabel, lblTime });
            targetPanel.Controls.Add(pnlTimer);

            // ✅ CHỈ TẠO TIMER KHI BÀN ĐANG CHƠI
            if (_ban.TrangThai == "Đang chơi")
            {
                // ✅ QUAN TRỌNG: Kiểm tra xem timer đã tồn tại chưa
                var existingTimer = _animationTimers.FirstOrDefault(t => t.Interval == 800 && t.Enabled);
                if (existingTimer == null)
                {
                    var timer = new System.Windows.Forms.Timer { Interval = 800 };
                    timer.Tick += async (s, e) =>
                    {
                        // ✅ Kiểm tra trạng thái trước khi update
                        if (_ban.TrangThai != "Đang chơi" || _isLoading)
                        {
                            timer.Stop();
                            return;
                        }

                        await UpdateTimerIfExists();
                    };

                    // ✅ Track timer để dispose sau này
                    _animationTimers.Add(timer);
                    timer.Start();

                    System.Diagnostics.Debug.WriteLine($"✓ Tạo timer animation cho bàn {_ban.MaBan}");
                }
            }

            return yPos + 120;
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

        private async Task<int> RenderReservedContent(Panel targetPanel, int yPos, int panelWidth)
        {
            DatBan bookingInfo = null;
            List<DatBan> allBookings = null;

            try
            {
                var datBanService = Program.GetService<DatBanService>();
                var datBans = await datBanService.GetByTableAsync(_ban.MaBan);

                // ✅ LẤY TẤT CẢ ĐƠN ĐẶT ĐANG HOẠT ĐỘNG
                allBookings = datBans
                    .Where(d =>
                        (d.TrangThai == "Đã xác nhận" || d.TrangThai == "Đã đặt") &&
                        d.ThoiGianBatDau.HasValue &&
                        d.ThoiGianKetThuc.HasValue)
                    .OrderBy(d => d.ThoiGianBatDau)
                    .ToList();

                if (allBookings.Any())
                {
                    var now = DateTime.Now;

                    // ✅ TÌM CA ĐANG DIỄN RA
                    var currentBooking = allBookings.FirstOrDefault(d =>
                        d.ThoiGianBatDau.Value <= now && d.ThoiGianKetThuc.Value >= now);

                    // ✅ NẾU KHÔNG CÓ, LẤY CA SẮP DIỄN RA GẦN NHẤT
                    bookingInfo = currentBooking ?? allBookings.FirstOrDefault(d =>
                        d.ThoiGianBatDau.Value > now);

                    // ✅ NẾU VẪN KHÔNG CÓ, LẤY CA CUỐI CÙNG
                    bookingInfo = bookingInfo ?? allBookings.Last();
                }
            }
            catch { }

            // ============================================================
            // CARD 1: THÔNG TIN THỜI GIAN ĐẶT BÀN (MỚI)
            // ============================================================
            if (bookingInfo != null)
            {
                var pnlTimeInfo = CreateModernCard(panelWidth);
                pnlTimeInfo.Location = new Point(15, yPos + 20);
                pnlTimeInfo.BackColor = Color.FromArgb(239, 246, 255); // Xanh nhạt

                int cardY = 12;

                var lblTitle = new Label
                {
                    Text = "⏰ THỜI GIAN ĐẶT BÀN",
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(30, 58, 138),
                    Location = new Point(12, cardY),
                    AutoSize = true
                };
                pnlTimeInfo.Controls.Add(lblTitle);
                cardY += 35;

                // Giờ bắt đầu
                var gioBatDau = bookingInfo.ThoiGianBatDau.Value;
                var lblStartTime = new Label
                {
                    Text = $"Bắt đầu: {gioBatDau:HH:mm, dd/MM/yyyy}",
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(30, 41, 59),
                    Location = new Point(12, cardY),
                    AutoSize = true
                };
                pnlTimeInfo.Controls.Add(lblStartTime);
                cardY += 28;

                // Giờ kết thúc
                var gioKetThuc = bookingInfo.ThoiGianKetThuc.Value;
                var lblEndTime = new Label
                {
                    Text = $"Kết thúc: {gioKetThuc:HH:mm, dd/MM/yyyy}",
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(30, 41, 59),
                    Location = new Point(12, cardY),
                    AutoSize = true
                };
                pnlTimeInfo.Controls.Add(lblEndTime);
                cardY += 28;

                // Thời lượng dự kiến
                var duration = gioKetThuc - gioBatDau;
                var hours = (int)duration.TotalHours;
                var minutes = duration.Minutes;
                var lblDuration = new Label
                {
                    Text = $"Thời lượng: {hours}h {minutes}m",
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = Color.FromArgb(71, 85, 105),
                    Location = new Point(12, cardY),
                    AutoSize = true
                };
                pnlTimeInfo.Controls.Add(lblDuration);
                cardY += 28;

                // Hiển thị thời gian còn lại đến khi đặt (nếu chưa đến giờ)
                if (DateTime.Now < gioBatDau)
                {
                    var timeUntil = gioBatDau - DateTime.Now;
                    var daysUntil = (int)timeUntil.TotalDays;
                    var hoursUntil = timeUntil.Hours;
                    var minutesUntil = timeUntil.Minutes;

                    var pnlCountdown = new Panel
                    {
                        Location = new Point(12, cardY),
                        Size = new Size(panelWidth - 24, 40),
                        BackColor = Color.FromArgb(254, 249, 195) // Vàng nhạt
                    };

                    var lblCountdown = new Label
                    {
                        Text = daysUntil > 0
                            ? $"⏳ Còn {daysUntil} ngày {hoursUntil}h {minutesUntil}m"
                            : $"⏳ Còn {hoursUntil}h {minutesUntil}m",
                        Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                        ForeColor = Color.FromArgb(113, 63, 18),
                        Location = new Point(10, 10),
                        AutoSize = true,
                        BackColor = Color.Transparent
                    };
                    pnlCountdown.Controls.Add(lblCountdown);
                    pnlTimeInfo.Controls.Add(pnlCountdown);
                    cardY += 45;
                }
                // Nếu đã quá giờ đặt
                else if (DateTime.Now > gioKetThuc)
                {
                    var pnlOverdue = new Panel
                    {
                        Location = new Point(12, cardY),
                        Size = new Size(panelWidth - 24, 40),
                        BackColor = Color.FromArgb(254, 226, 226) // Đỏ nhạt
                    };

                    var lblOverdue = new Label
                    {
                        Text = "⚠️ Đã quá giờ đặt bàn",
                        Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                        ForeColor = Color.FromArgb(153, 27, 27),
                        Location = new Point(10, 10),
                        AutoSize = true,
                        BackColor = Color.Transparent
                    };
                    pnlOverdue.Controls.Add(lblOverdue);
                    pnlTimeInfo.Controls.Add(pnlOverdue);
                    cardY += 45;
                }

                cardY += 8;
                pnlTimeInfo.Height = cardY;
                targetPanel.Controls.Add(pnlTimeInfo);
                yPos += cardY + CARD_SPACING;

            }

            // ============================================================
            // CARD 2: THÔNG TIN KHÁCH HÀNG
            // ============================================================
            var pnlReserved = CreateModernCard(panelWidth);
            pnlReserved.Location = new Point(15, yPos + 20);
            pnlReserved.BackColor = Color.FromArgb(254, 252, 232);

            int reservedCardY = 12;

            var lblReservedTitle = new Label
            {
                Text = "👤 THÔNG TIN KHÁCH HÀNG",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(12, reservedCardY),
                AutoSize = true
            };
            pnlReserved.Controls.Add(lblReservedTitle);
            reservedCardY += 28;

            // ✅ FIX: Lấy thông tin từ bookingInfo thay vì từ _ban
            if (bookingInfo != null)
            {
                // Ưu tiên lấy từ MaKhNavigation (nếu có liên kết với KhachHang)
                if (bookingInfo.MaKhNavigation != null)
                {
                    reservedCardY = AddInfoRow(pnlReserved, "Tên khách",
                        bookingInfo.MaKhNavigation.TenKh, reservedCardY, panelWidth);
                    reservedCardY = AddInfoRow(pnlReserved, "Điện thoại",
                        bookingInfo.MaKhNavigation.Sdt ?? "-", reservedCardY, panelWidth);

                    // Hiển thị email nếu có
                    if (!string.IsNullOrEmpty(_ban.GhiChu))
                    {
                        // Kiểm tra xem có lịch sử giữ bàn không
                        if (_ban.GhiChu.Contains("[Giữ bàn"))
                        {
                            // Tạo panel đặc biệt cho ghi chú giữ bàn
                            var pnlHoldNote = new Panel
                            {
                                Location = new Point(12, reservedCardY),
                                Size = new Size(panelWidth - 24, 60),
                                BackColor = Color.FromArgb(254, 243, 199) // Vàng nhạt
                            };

                            var lblHoldTitle = new Label
                            {
                                Text = "📝 Lịch sử giữ bàn:",
                                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                                ForeColor = Color.FromArgb(113, 63, 18),
                                Location = new Point(8, 6),
                                AutoSize = true
                            };

                            var lblHoldNote = new Label
                            {
                                Text = _ban.GhiChu,
                                Font = new Font("Segoe UI", 8F),
                                ForeColor = Color.FromArgb(120, 53, 15),
                                Location = new Point(8, 24),
                                Size = new Size(panelWidth - 40, 30),
                                AutoEllipsis = true
                            };

                            pnlHoldNote.Controls.AddRange(new Control[] { lblHoldTitle, lblHoldNote });
                            pnlReserved.Controls.Add(pnlHoldNote);
                            reservedCardY += 65;
                        }
                        else
                        {
                            // Ghi chú thường
                            reservedCardY = AddInfoRow(pnlReserved, "Ghi chú", _ban.GhiChu, reservedCardY, panelWidth);
                        }
                    }
                }
                // Nếu không có MaKhNavigation, lấy từ TenKhach và Sdt trong DatBan
                else
                {
                    reservedCardY = AddInfoRow(pnlReserved, "Tên khách",
                        bookingInfo.TenKhach ?? "Khách đặt", reservedCardY, panelWidth);
                    reservedCardY = AddInfoRow(pnlReserved, "Điện thoại",
                        bookingInfo.Sdt ?? "-", reservedCardY, panelWidth);
                }
            }
            // Fallback: nếu không có bookingInfo, mới lấy từ _ban
            else if (_ban.MaKhNavigation != null)
            {
                reservedCardY = AddInfoRow(pnlReserved, "Tên khách",
                    _ban.MaKhNavigation.TenKh, reservedCardY, panelWidth);
                reservedCardY = AddInfoRow(pnlReserved, "Điện thoại",
                    _ban.MaKhNavigation.Sdt ?? "-", reservedCardY, panelWidth);

                if (!string.IsNullOrEmpty(_ban.MaKhNavigation.Email))
                {
                    reservedCardY = AddInfoRow(pnlReserved, "Email",
                        _ban.MaKhNavigation.Email, reservedCardY, panelWidth);
                }
            }

            var giaGio = _ban.MaLoaiNavigation?.GiaGio ?? 0;
            reservedCardY = AddInfoRow(pnlReserved, "Giá giờ", $"{giaGio:N0}đ/giờ", reservedCardY, panelWidth);

            // Tính tiền dự kiến (nếu có thông tin đặt bàn)
            if (bookingInfo != null)
            {
                var duration = bookingInfo.ThoiGianKetThuc.Value - bookingInfo.ThoiGianBatDau.Value;
                var totalMinutes = (int)Math.Ceiling(duration.TotalMinutes);
                var soGio = (decimal)totalMinutes / 60m;
                var tienDuKien = soGio * giaGio;

                reservedCardY = AddInfoRow(pnlReserved, "Tiền dự kiến", $"{tienDuKien:N0}đ", reservedCardY, panelWidth);
            }

            // Ghi chú
            if (!string.IsNullOrEmpty(_ban.GhiChu))
            {
                reservedCardY = AddInfoRow(pnlReserved, "Ghi chú", _ban.GhiChu, reservedCardY, panelWidth);
            }

            reservedCardY += 8;
            pnlReserved.Height = reservedCardY;
            targetPanel.Controls.Add(pnlReserved);
            yPos += reservedCardY + CARD_SPACING;

            // Buttons
            yPos = RenderReservedButtons(targetPanel, yPos, panelWidth);
            return yPos;
        }

        private int RenderReservedButtons(Panel targetPanel, int yPos, int panelWidth)
        {
            // ============================================================
            // NÚT 1: XÁC NHẬN KHÁCH ĐẾN (Màu xanh lá)
            // ============================================================
            var btnConfirm = CreateModernButton("✓ Xác nhận khách đến", Color.FromArgb(34, 197, 94), panelWidth);
            btnConfirm.Location = new Point(15, yPos + 20);
            btnConfirm.Click += BtnConfirm_Click;
            targetPanel.Controls.Add(btnConfirm);
            yPos += 48;

            // ============================================================
            // NÚT 2: GIỮ BÀN - GIA HẠN THỜI GIAN (Màu vàng) ✨ MỚI
            // ============================================================
            var btnHold = CreateModernButton("⏰ Giữ bàn (+15 phút)", Color.FromArgb(234, 179, 8), panelWidth);
            btnHold.Location = new Point(15, yPos + 20);
            btnHold.Click += BtnHold_Click;

            // Tooltip để giải thích chức năng
            var toolTip = new ToolTip();
            toolTip.SetToolTip(btnHold, "Gia hạn thời gian đặt bàn thêm 15 phút khi khách báo đến muộn");

            targetPanel.Controls.Add(btnHold);
            yPos += 48;

            // ============================================================
            // NÚT 3: HỦY ĐẶT BÀN (Màu đỏ)
            // ============================================================
            var btnCancel = CreateModernButton("✕ Hủy đặt bàn", Color.FromArgb(239, 68, 68), panelWidth);
            btnCancel.Location = new Point(15, yPos + 20);
            btnCancel.Click += BtnCancel_Click;
            targetPanel.Controls.Add(btnCancel);
            yPos += 48;

            return yPos;
        }
        private async void BtnHold_Click(object sender, EventArgs e)
        {
            try
            {
                // BƯỚC 1: Xác nhận từ user
                var result = MessageBox.Show(
                    "🕐 GIỮ BÀN - GIA HẠN THỜI GIAN\n\n" +
                    "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                    "Thời gian đặt bàn sẽ được gia hạn thêm 15 phút.\n\n" +
                    "Điều này hữu ích khi:\n" +
                    "  • Khách báo đến muộn\n" +
                    "  • Cần thêm thời gian chuẩn bị\n" +
                    "  • Tránh hệ thống tự động hủy đơn\n\n" +
                    "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                    "Bạn có muốn giữ bàn không?",
                    "⏰ Xác nhận giữ bàn",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;

                // BƯỚC 2: Loading cursor
                this.Cursor = Cursors.WaitCursor;

                // BƯỚC 3: Tìm đơn đặt bàn active
                var datBanService = Program.GetService<DatBanService>();
                var datBans = await datBanService.GetByTableAsync(_ban.MaBan);

                var activeDatBan = datBans
                    .Where(d => d.TrangThai == "Đang chờ" || d.TrangThai == "Đã đặt")
                    .OrderBy(d => d.ThoiGianBatDau)
                    .FirstOrDefault();

                if (activeDatBan == null)
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show(
                        "❌ KHÔNG TÌM THẤY ĐƠN ĐẶT BÀN\n\n" +
                        "Không tìm thấy đơn đặt bàn đang hoạt động.",
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Lưu thời gian cũ
                var oldStartTime = activeDatBan.ThoiGianBatDau;
                var oldEndTime = activeDatBan.ThoiGianKetThuc;

                // BƯỚC 4: Gọi service giữ bàn
                var success = await _banBiaService.HoldReservationAsync(activeDatBan.MaDat, 15);

                this.Cursor = Cursors.Default;

                // BƯỚC 5: Xử lý kết quả
                if (success)
                {
                    var newStartTime = oldStartTime?.AddMinutes(15);
                    var newEndTime = oldEndTime?.AddMinutes(15);

                    MessageBox.Show(
                        "✅ ĐÃ GIỮ BÀN THÀNH CÔNG!\n\n" +
                        "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                        $"🏷️  Bàn: {_ban.TenBan}\n" +
                        $"👤  Khách: {activeDatBan.TenKhach}\n" +
                        $"📞  SĐT: {activeDatBan.Sdt}\n" +
                        "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +
                        "⏰  THỜI GIAN CŨ:\n" +
                        $"    {oldStartTime:HH:mm} → {oldEndTime:HH:mm}\n\n" +
                        "⏰  THỜI GIAN MỚI:\n" +
                        $"    {newStartTime:HH:mm} → {newEndTime:HH:mm}\n\n" +
                        "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                        "Bàn đã được gia hạn thêm 15 phút.",
                        "✅ Giữ bàn thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    // BƯỚC 6: Cập nhật UI
                    OnDataChanged?.Invoke(this, EventArgs.Empty);
                    await Task.Delay(100);
                    await LoadBanDetail(forceReload: true);

                    System.Diagnostics.Debug.WriteLine(
                        $"✅ Đã giữ bàn {_ban.TenBan} - Đơn #{activeDatBan.MaDat} +15p");
                }
                else
                {
                    MessageBox.Show(
                        "❌ KHÔNG THỂ GIỮ BÀN\n\n" +
                        "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                        "Có thể do:\n" +
                        "  • Gia hạn sẽ trùng với đơn đặt khác\n" +
                        "  • Đơn đặt đã bị hủy hoặc xác nhận\n" +
                        "  • Lỗi kết nối cơ sở dữ liệu\n\n" +
                        "Vui lòng thử lại.",
                        "❌ Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;

                System.Diagnostics.Debug.WriteLine($"❌ Exception: {ex.Message}");

                MessageBox.Show(
                    $"❌ LỖI KHI GIỮ BÀN\n\n" +
                    $"Chi tiết: {ex.Message}",
                    "Lỗi hệ thống",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
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
        private Color GetStatusColor(string trangThai)
        {
            return trangThai switch
            {
                "Trống" => Color.FromArgb(34, 197, 94),      // Xanh lá
                "Đang chơi" => Color.FromArgb(239, 68, 68),  // Đỏ
                "Đã đặt" => Color.FromArgb(234, 179, 8),     // Vàng
                "Bảo trì" => Color.FromArgb(148, 163, 184),  // Xám
                _ => Color.FromArgb(100, 116, 139)           // Xám đậm
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
                    // ✅ QUAN TRỌNG: Load lại data ngay để lấy trạng thái mới
                    this.Cursor = Cursors.WaitCursor;

                    // Delay nhỏ để đợi DB commit
                    await Task.Delay(100);

                    // Load lại dữ liệu bàn từ database
                    var updatedBan = await Task.Run(() => _banBiaService.GetTableByIdAsync(_ban.MaBan));

                    if (updatedBan != null)
                    {
                        _ban = updatedBan;

                        // ✅ Trigger event để cập nhật card trong danh sách
                        OnBanUpdated?.Invoke(this, _ban);
                        OnDataChanged?.Invoke(this, EventArgs.Empty);

                        // ✅ Reload toàn bộ UI chi tiết bàn ngay lập tức
                        await LoadBanDetail(forceReload: true);

                        this.Cursor = Cursors.Default;

                        MessageBox.Show($"Đã bắt đầu chơi tại {_ban.TenBan}", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        this.Cursor = Cursors.Default;
                        ShowError("Không thể tải lại thông tin bàn");
                    }
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
                        await LoadBanDetail(forceReload: true);
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
            if (MessageBox.Show(
        "Xác nhận khách đã đến và bắt đầu chơi?",
        "Xác nhận",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question) == DialogResult.Yes)
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

                    if (success)
                    {
                        // ✅ FIX: CHỜ DB commit hoàn toàn trước khi tiếp tục
                        System.Diagnostics.Debug.WriteLine("⏳ Đang chờ DB commit hóa đơn mới...");
                        await Task.Delay(300); // Delay 300ms

                        this.Cursor = Cursors.Default;

                        MessageBox.Show("Đã xác nhận và bắt đầu chơi!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        OnDataChanged?.Invoke(this, EventArgs.Empty);
                        await LoadBanDetail(forceReload: true);
                    }
                    else
                    {
                        this.Cursor = Cursors.Default;
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
        // 🔧 FIX: BtnThemDV_Click
        private async void BtnThemDV_Click(object sender, EventArgs e)
        {
            try
            {
                // ✅ REFRESH trạng thái bàn mới nhất
                var latestBan = await _banBiaService.GetTableByIdAsync(_ban.MaBan);
                if (latestBan != null)
                {
                    _ban = latestBan;
                }

                // ✅ Kiểm tra trạng thái bàn
                if (_ban.TrangThai != "Đang chơi")
                {
                    ShowError($"Bàn đang ở trạng thái '{_ban.TrangThai}'. Không thể thêm dịch vụ!");
                    await LoadBanDetail(forceReload: true);
                    return;
                }

                // ✅ LẤY HÓA ĐƠN (đã có retry logic trong GetActiveInvoiceAsync)
                System.Diagnostics.Debug.WriteLine($"🔍 Đang tìm hóa đơn cho bàn {_ban.MaBan}...");
                var hoaDon = await _banBiaService.GetActiveInvoiceAsync(_ban.MaBan);

                // ✅ FIX: NẾU KHÔNG TÌM THẤY → KIỂM TRA LẠI 1 LẦN CUỐI
                if (hoaDon == null)
                {
                    System.Diagnostics.Debug.WriteLine($"⚠️ Không tìm thấy hóa đơn lần 1, thực hiện kiểm tra bổ sung...");

                    // Reload lại giao diện để đảm bảo đồng bộ
                    await Task.Delay(200);
                    await LoadBanDetail(forceReload: true);

                    // Thử tìm lại
                    hoaDon = await _banBiaService.GetActiveInvoiceAsync(_ban.MaBan);

                    if (hoaDon == null)
                    {
                        // Vẫn không tìm thấy → Báo lỗi chi tiết
                        var errorMsg =
                            $"❌ Không tìm thấy hóa đơn đang hoạt động cho bàn {_ban.TenBan}!\n\n" +
                            $"Trạng thái bàn hiện tại: {_ban.TrangThai}\n" +
                            $"Giờ bắt đầu: {_ban.GioBatDau:HH:mm dd/MM}\n\n" +
                            "Có thể nguyên nhân:\n" +
                            "1. Bàn vừa được xác nhận, vui lòng đợi 1-2 giây\n" +
                            "2. Hóa đơn đã bị xóa/hủy\n" +
                            "3. Lỗi đồng bộ dữ liệu\n\n" +
                            "Vui lòng:\n" +
                            "• Kiểm tra lại trạng thái bàn\n" +
                            "• Hoặc liên hệ IT nếu vấn đề tiếp diễn";

                        ShowError(errorMsg);
                        return;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"✓ Đã tìm thấy hóa đơn HD{hoaDon.MaHd} ở lần thử thứ 2");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"✓ Tìm thấy hóa đơn HD{hoaDon.MaHd} ngay lần đầu");
                }

                // ✅ Tiếp tục xử lý bình thường
                var dichVuService = Program.GetService<DichVuService>();

                using (var themDichVuForm = new ThemDichVuForm(dichVuService, _hoaDonService, hoaDon.MaHd))
                {
                    var result = themDichVuForm.ShowDialog(this);

                    if (result == DialogResult.OK)
                    {
                        this.Cursor = Cursors.WaitCursor;

                        try
                        {
                            await Task.Delay(150);
                            _cts?.Cancel();
                            await LoadBanDetail(forceReload: true);
                            OnDataChanged?.Invoke(this, EventArgs.Empty);
                            this.Cursor = Cursors.Default;

                            MessageBox.Show("Đã thêm dịch vụ thành công!", "Thành công",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            this.Cursor = Cursors.Default;
                            ShowError($"Lỗi khi cập nhật giao diện: {ex.Message}");
                        }
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
                // ✅ REFRESH trạng thái bàn mới nhất
                var latestBan = await _banBiaService.GetTableByIdAsync(_ban.MaBan);
                if (latestBan != null)
                {
                    _ban = latestBan;
                }

                // ✅ Kiểm tra trạng thái
                if (_ban.TrangThai != "Đang chơi")
                {
                    ShowError($"Bàn đang ở trạng thái '{_ban.TrangThai}'. Không thể thanh toán!");
                    await LoadBanDetail(forceReload: true);
                    return;
                }

                var result = MessageBox.Show(
                    $"Xác nhận kết thúc và thanh toán cho {_ban.TenBan}?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes) return;

                var thanhToanService = Program.GetService<ThanhToanService>();
                var vietQRService = Program.GetService<VietQRService>();

                // ✅ LẤY HÓA ĐƠN với retry
                var hoaDon = await _banBiaService.GetActiveInvoiceAsync(_ban.MaBan);

                // ✅ FIX: Kiểm tra lại nếu không tìm thấy
                if (hoaDon == null)
                {
                    System.Diagnostics.Debug.WriteLine($"⚠️ Không tìm thấy hóa đơn lần 1 khi thanh toán");
                    await Task.Delay(200);
                    hoaDon = await _banBiaService.GetActiveInvoiceAsync(_ban.MaBan);
                }

                if (hoaDon == null)
                {
                    MessageBox.Show(
                        "Không tìm thấy hóa đơn đang hoạt động!\n\n" +
                        "Bàn có thể đã được thanh toán hoặc chưa bắt đầu chơi.\n" +
                        "Vui lòng kiểm tra lại trạng thái bàn.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    await LoadBanDetail(forceReload: true);
                    return;
                }

                using (var thanhToanForm = new ThanhToanForm(thanhToanService, vietQRService, hoaDon.MaHd))
                {
                    var thanhToanResult = thanhToanForm.ShowDialog(this);

                    if (thanhToanResult == DialogResult.OK)
                    {
                        _autoCheckTimer?.Stop();
                        _cts?.Cancel();
                        _ban.TrangThai = "Trống";
                        _ban.GioBatDau = null;
                        _ban.MaKh = null;
                        _ban.GhiChu = null;
                        await Task.Delay(300); // Chờ DB commit

                        MessageBox.Show(
                            $"Đã thanh toán thành công!\nBàn {_ban.TenBan} đã được trả về trống.",
                            "Thành công",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        // ✅ Trigger event trước
                        OnDataChanged?.Invoke(this, EventArgs.Empty);

                        await Task.Delay(100);

                        // ✅ RELOAD với cancel token mới
                        await LoadBanDetail(forceReload: true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thanh toán: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                await LoadBanDetail(forceReload: true);
            }
        }
        #endregion
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                System.Diagnostics.Debug.WriteLine($"🗑️ Disposing BanChiTietControl cho bàn {_ban?.MaBan}");

                if (_autoCheckTimer != null)
                {
                    _autoCheckTimer.Stop();
                    _autoCheckTimer.Tick -= AutoCheckReservation_Tick;
                    _autoCheckTimer.Dispose();
                    _autoCheckTimer = null;
                }

                // Cancel operations đang chạy
                if (_cts != null)
                {
                    _cts.Cancel();
                    _cts.Dispose();
                    _cts = null;
                }

                // Dispose components
                components?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}