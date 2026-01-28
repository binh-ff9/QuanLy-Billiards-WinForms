using Billiard.BLL.Services.HoaDonServices;
using Billiard.BLL.Services.QLBan;
using Billiard.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        private readonly GioHoatDongService _gioHoatDongService;

        // Filters
        private string _currentAreaFilter = "all";
        private string _currentStatusFilter = "all";
        private string _currentTypeFilter = "all";

        // Auto refresh
        private System.Windows.Forms.Timer _refreshTimer;
        private bool _daHienThiCanhBaoQuaGio = false;
        private System.Windows.Forms.Timer _checkReservationTimer;
        private HashSet<int> _processedReservations = new HashSet<int>(); // Tránh hiện thông báo lặp lại

        // Detail Panel Management
        private Panel pnlDetailContainer;
        private Panel pnlDetailHeader;
        private Panel pnlDetailContent;
        private BanChiTietControl _chiTietControl;
        private bool _isDetailVisible = false;
        private bool _showReservationCounts = false;
        private Dictionary<int, int> _reservationCounts = new Dictionary<int, int>();
        private DateTime _currentDisplayDate = DateTime.Today;
        private bool _isReservationViewMode = false;
        private Panel _pnlReservationToggle; // Thêm field ở đầu class
        private Button _btnToggleReservation; // Lưu reference để thay đổi từ bên ngoài
        private Label? _lblReservationBadge;  // Badge hiển thị số đơn đặt
        private int _currentReservationCount = 0;  // Số đơn đặt hiện tại

        // Constants
        private const int DETAIL_PANEL_WIDTH = 430;
        private const int CARD_WIDTH = 250;
        private const int ANIMATION_STEPS = 15;
        private const int ANIMATION_INTERVAL = 8;
        //cảnh báo và tự động hủy
        private const int WARNING_THRESHOLD_MINUTES = 10;     // Bắt đầu cảnh báo
        private const int AUTO_CANCEL_THRESHOLD_MINUTES = 15; // Tự động hủy
        public QLBanForm(BanBiaService banBiaService)
        {
            _banBiaService = banBiaService;
            _gioHoatDongService = new GioHoatDongService();

            InitializeComponent();
            InitializeDetailPanel();
            InitializeRefreshTimer();
            InitializeReservationCheckTimer();
            InitializeKeyboardShortcuts();

            this.AutoScroll = false;
            this.AutoSize = false;
        }

        #region Initialization
        private void InitializeReservationBadge()
        {
            // ════════════════════════════════════════════════════════════
            // TẠO LABEL BADGE
            // ════════════════════════════════════════════════════════════
            _lblReservationBadge = new Label
            {
                AutoSize = false,
                Size = new Size(28, 28),  // Kích thước badge
                Text = "",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false,  // Ẩn mặc định, chỉ hiện khi có đơn đặt
                Name = "lblReservationBadge"
            };

            // ════════════════════════════════════════════════════════════
            // TÍNH TOÁN VỊ TRÍ BADGE
            // ════════════════════════════════════════════════════════════
            // Badge nằm ở góc trên phải của nút, lệch ra ngoài 1 chút
            int badgeX = btnXemBanDat.Right - 14;  // 14 pixel từ cạnh phải nút
            int badgeY = btnXemBanDat.Top - 8;     // 8 pixel phía trên nút

            _lblReservationBadge.Location = new Point(badgeX, badgeY);

            // ════════════════════════════════════════════════════════════
            // VẼ HÌNH TRÒN ĐỎ CHO BADGE
            // ════════════════════════════════════════════════════════════
            _lblReservationBadge.Paint += (s, e) =>
            {
                if (_lblReservationBadge?.Visible != true) return;

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // Vẽ vòng tròn đỏ
                using (var brush = new SolidBrush(Color.FromArgb(239, 68, 68)))  // Màu đỏ
                {
                    e.Graphics.FillEllipse(brush, 0, 0,
                        _lblReservationBadge.Width - 1,
                        _lblReservationBadge.Height - 1);
                }

                // Vẽ viền trắng (optional - làm badge nổi bật hơn)
                using (var pen = new Pen(Color.White, 2))
                {
                    e.Graphics.DrawEllipse(pen, 1, 1,
                        _lblReservationBadge.Width - 3,
                        _lblReservationBadge.Height - 3);
                }

                // Vẽ số
                if (!string.IsNullOrEmpty(_lblReservationBadge.Text))
                {
                    var sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };

                    e.Graphics.DrawString(
                        _lblReservationBadge.Text,
                        _lblReservationBadge.Font,
                        Brushes.White,
                        new RectangleF(0, 0, _lblReservationBadge.Width, _lblReservationBadge.Height),
                        sf);
                }
            };

            // ════════════════════════════════════════════════════════════
            // THÊM BADGE VÀO PANEL TOOLBAR
            // ════════════════════════════════════════════════════════════
            pnlToolbar.Controls.Add(_lblReservationBadge);
            _lblReservationBadge.BringToFront();  // Đưa badge lên trên cùng

            System.Diagnostics.Debug.WriteLine($"✅ Badge đã được tạo tại vị trí ({badgeX}, {badgeY})");
        }
        private void InitializeReservationToggle()
        {
            // ============================================================
            // TẠO PANEL TOGGLE - NẰM TRONG pnlFilters
            // ============================================================
            _pnlReservationToggle = new Panel
            {
                Height = 40,
                BackColor = Color.White,
                Padding = new Padding(0),
                Visible = true, // Luôn hiển thị
                Location = new Point(530, 83), // Cạnh ô search
                Size = new Size(200, 37)
            };

            // ============================================================
            // NÚT TOGGLE
            // ============================================================
            _btnToggleReservation = new Button
            {
                Text = "📊 Hiện số đơn",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(150, 37),
                Location = new Point(0, 0),
                Cursor = Cursors.Hand,
                Name = "btnToggleReservation"
            };
            _btnToggleReservation.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            _btnToggleReservation.FlatAppearance.BorderSize = 1;

            _btnToggleReservation.Click += async (s, e) =>
            {
                await ToggleReservationView();
            };

            // ============================================================
            // BADGE COUNTER (Hiển thị tổng số đơn)
            // ============================================================
            var lblBadge = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(239, 68, 68),
                Size = new Size(24, 24),
                Location = new Point(160, 6),
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false,
                Name = "lblReservationBadge"
            };

            // Bo tròn badge
            lblBadge.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (var brush = new SolidBrush(lblBadge.BackColor))
                {
                    e.Graphics.FillEllipse(brush, 0, 0, lblBadge.Width - 1, lblBadge.Height - 1);
                }
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                e.Graphics.DrawString(lblBadge.Text, lblBadge.Font, Brushes.White,
                    new RectangleF(0, 0, lblBadge.Width, lblBadge.Height), sf);
            };

            _pnlReservationToggle.Controls.AddRange(new Control[] { _btnToggleReservation, lblBadge });

            // ============================================================
            // THÊM VÀO pnlFilters
            // ============================================================
            pnlFilters.Controls.Add(_pnlReservationToggle);
            _pnlReservationToggle.BringToFront();
        }
        private async Task ToggleReservationView()
        {
            _showReservationCounts = !_showReservationCounts;
            _isReservationViewMode = _showReservationCounts;

            if (_showReservationCounts)
            {
                // ✅ BẬT: Màu xanh + hiển thị số
                _btnToggleReservation.BackColor = Color.FromArgb(59, 130, 246);
                _btnToggleReservation.ForeColor = Color.White;

                // Load số đơn đặt
                _currentDisplayDate = DateTime.Today;
                await LoadReservationCountsAndRefresh();

                // ✅ CẬP NHẬT TEXT VỚI SỐ LƯỢNG
                var total = _reservationCounts.Values.Sum();
                _btnToggleReservation.Text = total > 0
                    ? $"📊 Đang hiển thị ({total})"
                    : "📊 Đang hiển thị";

                // Tự động filter bàn đã đặt + chờ xác nhận
                ApplyReservationFilter();
            }
            else
            {
                // ❌ TẮT: Màu trắng + văn bản mặc định
                _btnToggleReservation.BackColor = Color.White;
                _btnToggleReservation.ForeColor = Color.FromArgb(71, 85, 105);
                _btnToggleReservation.Text = "📊 Hiện số đơn";

                // Reset về filter "Tất cả"
                ClearReservationFilter();

                await RefreshTables();
            }
        }
        private void InitializeReservationCheckTimer()
        {
            _checkReservationTimer = new System.Windows.Forms.Timer
            {
                Interval = 30000
            };
            _checkReservationTimer.Tick += async (s, e) => await CheckReservationsNearStartTime();
        }
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
        private async Task LoadReservationCountsAndRefresh()
        {
            try
            {
                _reservationCounts = await _banBiaService.GetReservationCountsForDateAsync(_currentDisplayDate);

                var total = _reservationCounts.Values.Sum();

                // Cập nhật badge
                var lblBadge = _pnlReservationToggle.Controls.Find("lblReservationBadge", false).FirstOrDefault() as Label;
                if (lblBadge != null)
                {
                    if (total > 0)
                    {
                        lblBadge.Text = total > 99 ? "99+" : total.ToString();
                        lblBadge.Visible = true;
                    }
                    else
                    {
                        lblBadge.Visible = false;
                    }
                }

                await RefreshTables();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải số đơn đặt: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #region Detail Panel Management
        private void ApplyReservationFilter()
        {
            // Tắt tất cả filter buttons
            foreach (var btn in pnlTrangThaiFilters.Controls.OfType<Button>())
            {
                btn.BackColor = Color.FromArgb(226, 232, 240);
                btn.ForeColor = Color.FromArgb(51, 65, 85);
            }

            // ✅ KHÔNG SET _currentStatusFilter
            // Thay vào đó, filter trong GetFilteredTables()
        }
        private void ClearReservationFilter()
        {
            _currentStatusFilter = "all";

            // Đánh dấu nút "Tất cả" là active
            foreach (var btn in pnlTrangThaiFilters.Controls.OfType<Button>())
            {
                if (btn.Tag?.ToString() == "all")
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
                // Nếu đang mở cùng bàn -> chỉ refresh data
                if (_isDetailVisible && _chiTietControl != null)
                {
                    var currentBan = _chiTietControl.GetType()
                        .GetField("_ban", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                        ?.GetValue(_chiTietControl) as BanBium;

                    if (currentBan?.MaBan == ban.MaBan)
                    {
                        await _chiTietControl.LoadBanDetail(forceReload: false);
                        return;
                    }
                }

                // Tạo mới control
                var hoaDonService = Program.GetService<HoaDonService>();

                pnlDetailContent.Controls.Clear();
                _chiTietControl?.Dispose();

                _chiTietControl = new BanChiTietControl(_banBiaService, hoaDonService, ban, _mainForm.MaNV);
                _chiTietControl.Dock = DockStyle.Fill;
                _chiTietControl.BackColor = Color.White;

                _chiTietControl.OnDataChanged += async (s, e) => await RefreshTables();
                _chiTietControl.OnBanUpdated += (s, updatedBan) => UpdateCardForBan(updatedBan);

                pnlDetailContent.Controls.Add(_chiTietControl);
                ShowDetailPanel();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                Height = 330,
                Margin = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand,
                Tag = ban,
                Name = $"card_{ban.MaBan}"
            };

            card.BackColor = GetCardBackColor(ban.TrangThai);

            card.Paint += (s, e) =>
            {
                var borderColor = GetStatusColor(ban.TrangThai);
                using (var pen = new Pen(borderColor, 3))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                }
            };

            // ✅ THÊM BADGE SỐ ĐƠN ĐẶT (nếu toggle bật)
            if (_showReservationCounts && _reservationCounts.ContainsKey(ban.MaBan) && _reservationCounts[ban.MaBan] > 0)
            {
                var badge = new Label
                {
                    Text = _reservationCounts[ban.MaBan].ToString(),
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(239, 68, 68), // Đỏ
                    Size = new Size(28, 28),
                    Location = new Point(CARD_WIDTH - 35, 7),
                    TextAlign = ContentAlignment.MiddleCenter,
                    AutoSize = false,
                    Name = "badgeReservationCount"
                };

                // Bo tròn badge
                badge.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    using (var brush = new SolidBrush(badge.BackColor))
                    {
                        e.Graphics.FillEllipse(brush, 0, 0, badge.Width - 1, badge.Height - 1);
                    }

                    var sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    e.Graphics.DrawString(badge.Text, badge.Font, Brushes.White,
                        new RectangleF(0, 0, badge.Width, badge.Height), sf);
                };

                card.Controls.Add(badge);
                badge.BringToFront();
            }

            // Components cơ bản
            var pnlImage = CreateImagePanel(ban);
            var lblName = CreateNameLabel(ban);
            var lblInfo = CreateInfoLabel(ban);

            card.Controls.AddRange(new Control[] { pnlImage, lblName, lblInfo });

            // Hiển thị thông tin theo trạng thái
            if (ban.TrangThai == "Đang chơi")
            {
                var lblTimeInfo = CreateTimeInfoLabel(ban);
                var lblCustomer = CreateCustomerLabel(ban);
                card.Controls.AddRange(new Control[] { lblTimeInfo, lblCustomer });
            }
            else if (ban.TrangThai == "Đã đặt")
            {
                var lblBookingTimeInfo = CreateBookingTimeInfoLabel(ban);
                var lblCustomer = CreateCustomerLabelForReserved(ban);
                card.Controls.AddRange(new Control[] { lblBookingTimeInfo, lblCustomer });
            }

            var lblPrice = CreatePriceLabel(ban);
            card.Controls.Add(lblPrice);

            // ✅ FIX: Dùng đúng method name
            SetupCardClickHandlers(card, ban);
            SetupCardHoverEffects(card, ban);

            return card;
        }
        private System.Drawing.Drawing2D.GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }
        private Label CreateTimeInfoLabel(BanBium ban)
        {
            var lblTimeInfo = new Label
            {
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(71, 85, 105),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 210),
                Size = new Size(CARD_WIDTH, 40),
                Name = "lblTimeInfo"
            };

            UpdateTimeInfoText(lblTimeInfo, ban);
            return lblTimeInfo;
        }

        private async void UpdateTimeInfoText(Label lblTimeInfo, BanBium ban)
        {
            if (ban.TrangThai == "Đang chơi" && ban.GioBatDau.HasValue)
            {
                var gioBatDau = ban.GioBatDau.Value;

                // ✅ LẤY THÔNG TIN BOOKING (nếu có)
                DateTime? gioKetThucBooking = null;
                try
                {
                    var datBanService = Program.GetService<DatBanService>();
                    var datBans = await datBanService.GetByTableAsync(ban.MaBan);
                    var activeBooking = datBans.FirstOrDefault(d =>
                        d.TrangThai == "Đã xác nhận" &&
                        d.ThoiGianKetThuc.HasValue);

                    if (activeBooking != null)
                    {
                        gioKetThucBooking = activeBooking.ThoiGianKetThuc.Value;
                    }
                }
                catch { }

                // ✅ TÍNH ĐÚNG VỚI BOOKING QUA ĐÊM
                var gioKetThuc = _gioHoatDongService.LayThoiGianKetThucHopLe(gioBatDau, gioKetThucBooking);

                // ✅ HIỂN THỊ RÕ RÀNG GIỜ CHƠI
                lblTimeInfo.Text = $"🕐 {gioBatDau:HH:mm} → {gioKetThuc:HH:mm}\n{gioBatDau:dd/MM/yyyy}";
                lblTimeInfo.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            }
        }
        private Label CreateBookingTimeInfoLabel(BanBium ban)
        {
            var lblBookingTimeInfo = new Label
            {
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(71, 85, 105),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 210),
                Size = new Size(CARD_WIDTH, 40),
                Name = "lblBookingTimeInfo"
            };

            UpdateBookingTimeInfoText(lblBookingTimeInfo, ban);
            return lblBookingTimeInfo;
        }

        private async void UpdateBookingTimeInfoText(Label lblBookingTimeInfo, BanBium ban)
        {
            if (ban.TrangThai == "Đã đặt")
            {
                try
                {
                    var datBanService = Program.GetService<DatBanService>();
                    var datBans = await datBanService.GetByTableAsync(ban.MaBan);

                    // ✅ TÌM ĐƠN ĐẶT ĐÃ XÁC NHẬN (cửa hàng đã duyệt)
                    var activeBooking = datBans.FirstOrDefault(d =>
                        (d.TrangThai == "Đã xác nhận" || d.TrangThai == "Đã đặt") &&
                        d.ThoiGianBatDau.HasValue &&
                        d.ThoiGianKetThuc.HasValue);

                    if (activeBooking != null)
                    {
                        var gioBatDau = activeBooking.ThoiGianBatDau.Value;
                        var gioKetThuc = activeBooking.ThoiGianKetThuc.Value;

                        lblBookingTimeInfo.Text = $"🕐 {gioBatDau:HH:mm} → {gioKetThuc:HH:mm}\n{gioBatDau:dd/MM/yyyy}";
                        lblBookingTimeInfo.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
                    }
                    else
                    {
                        lblBookingTimeInfo.Text = "Chưa có thông tin giờ đặt";
                    }
                }
                catch (Exception ex)
                {
                    //lblBookingTimeInfo.Text = "Lỗi tải thông tin đặt bàn";
                    System.Diagnostics.Debug.WriteLine($"Lỗi UpdateBookingTimeInfoText: {ex.Message}");
                }
            }
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

        private async void UpdateInfoLabelText(Label lblInfo, BanBium ban)
        {
            if (ban.TrangThai == "Đang chơi" && ban.GioBatDau.HasValue)
            {
                // ✅ LẤY THÔNG TIN BOOKING (nếu có)
                DateTime? gioKetThucBooking = null;
                try
                {
                    var datBanService = Program.GetService<DatBanService>();
                    var datBans = await datBanService.GetByTableAsync(ban.MaBan);
                    var activeBooking = datBans.FirstOrDefault(d =>
                        d.TrangThai == "Đã xác nhận" &&
                        d.ThoiGianKetThuc.HasValue);

                    if (activeBooking != null)
                    {
                        gioKetThucBooking = activeBooking.ThoiGianKetThuc.Value;
                    }
                }
                catch { }

                // ✅ TÍNH DURATION ĐÚNG VỚI BOOKING QUA ĐÊM
                var thoiGianKetThuc = _gioHoatDongService.LayThoiGianKetThucHopLe(
                    ban.GioBatDau.Value,
                    gioKetThucBooking
                );
                var duration = thoiGianKetThuc - ban.GioBatDau.Value;

                // ✅ LÀM TRÒN LÊN PHÚT
                var totalMinutes = (int)Math.Ceiling(duration.TotalMinutes);
                var hours = totalMinutes / 60;
                var minutes = totalMinutes % 60;

                lblInfo.Text = $"⏱️ {hours}h {minutes}m";
                lblInfo.ForeColor = Color.FromArgb(239, 68, 68);
                lblInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            }
            // ✅ MỚI: Xử lý cho "Đang chờ"
            else if (ban.TrangThai == "Đang chờ")
            {
                lblInfo.Text = $"⏳ Chờ xác nhận • {ban.MaKhuVucNavigation?.TenKhuVuc ?? "Khu vực"}";
                lblInfo.ForeColor = Color.FromArgb(59, 130, 246); // Xanh dương
            }
            // ✅ CẬP NHẬT: Xử lý cho "Đã đặt"
            else if (ban.TrangThai == "Đã đặt")
            {
                lblInfo.Text = $"📅 Đã xác nhận • {ban.MaKhuVucNavigation?.TenKhuVuc ?? "Khu vực"}";
                lblInfo.ForeColor = Color.FromArgb(161, 98, 7); // Vàng đậm
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
                Location = new Point(0, 250),
                Size = new Size(CARD_WIDTH, 22),
                Name = "lblCustomer"
            };
        }

        private Label CreateCustomerLabelForReserved(BanBium ban)
        {
            var lblCustomer = new Label
            {
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(71, 85, 105),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 250),
                Size = new Size(CARD_WIDTH, 22),  // ← Có thể giảm xuống 22
                Name = "lblCustomer"
            };

            UpdateCustomerLabelForReservedText(lblCustomer, ban);
            return lblCustomer;
        }
        private async void UpdateCustomerLabelForReservedText(Label lblCustomer, BanBium ban)
        {
            if (ban.TrangThai == "Đã đặt")
            {
                try
                {
                    var datBanService = Program.GetService<DatBanService>();
                    var datBans = await datBanService.GetByTableAsync(ban.MaBan);
                    var activeBooking = datBans.FirstOrDefault(d =>
                        d.TrangThai == "Đã xác nhận" || d.TrangThai == "Đã đặt");

                    if (activeBooking != null)
                    {
                        // ✅ FIX: Chỉ hiển thị tên, ưu tiên từ MaKhNavigation
                        string tenKhach;

                        if (activeBooking.MaKhNavigation != null)
                        {
                            // Lấy từ bảng KhachHang
                            tenKhach = activeBooking.MaKhNavigation.TenKh;
                        }
                        else
                        {
                            // Lấy từ DatBan (khách vãng lai)
                            tenKhach = activeBooking.TenKhach ?? "Khách đặt";
                        }

                        lblCustomer.Text = $"👤 {tenKhach}";
                    }
                    else
                    {
                        lblCustomer.Text = "Chưa có thông tin khách";
                    }
                }
                catch (Exception ex)
                {
                    lblCustomer.Text = "👤 Khách đặt";
                    System.Diagnostics.Debug.WriteLine($"Lỗi UpdateCustomerLabelForReservedText: {ex.Message}");
                }
            }
        }
        private Label CreatePriceLabel(BanBium ban)
        {
            // ✅ Cả "Đang chơi", "Đang chờ" và "Đã đặt" đều dùng position 275
            int yPos = (ban.TrangThai == "Đang chơi" ||
                        ban.TrangThai == "Đang chờ" ||
                        ban.TrangThai == "Đã đặt") ? 275 : 235;

            return new Label
            {
                Text = $"{ban.MaLoaiNavigation?.GiaGio:N0} đ/giờ",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(99, 102, 241),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, yPos),
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

            // Update height nếu thay đổi trạng thái
            int newHeight = 330;
            if (card.Height != newHeight)
            {
                card.Height = newHeight;
            }

            // Update background
            card.BackColor = GetCardBackColor(updatedBan.TrangThai);

            var pnlImage = card.Controls.OfType<Panel>().FirstOrDefault();
            if (pnlImage == null) return;

            // Update status label
            var lblStatus = pnlImage.Controls.Find("lblStatus", false).FirstOrDefault() as Label;
            if (lblStatus != null)
            {
                lblStatus.Text = updatedBan.TrangThai;
                lblStatus.BackColor = GetStatusColor(updatedBan.TrangThai);
            }

            // Update warning badges
            var lblDongCua = pnlImage.Controls.Find("lblDongCua", false).FirstOrDefault();
            var lblSapDong = pnlImage.Controls.Find("lblSapDong", false).FirstOrDefault();

            if (lblDongCua != null)
            {
                pnlImage.Controls.Remove(lblDongCua);
                lblDongCua.Dispose();
            }
            if (lblSapDong != null)
            {
                pnlImage.Controls.Remove(lblSapDong);
                lblSapDong.Dispose();
            }

            if (updatedBan.TrangThai == "Đang chơi" && updatedBan.GioBatDau.HasValue)
            {
                var isDaDongCua = _gioHoatDongService.DaDenGioDongCua() &&
                                  updatedBan.GioBatDau.Value < _gioHoatDongService.LayThoiDiemDongCua();
                var isSapDongCua = _gioHoatDongService.SapDenGioDongCua();

                if (isDaDongCua)
                {
                    var lblNew = new Label
                    {
                        Text = "🚨 ĐÓNG!",
                        Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                        BackColor = Color.FromArgb(220, 38, 38),
                        ForeColor = Color.White,
                        AutoSize = false,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Size = new Size(75, 26),
                        Location = new Point(CARD_WIDTH - 85, 10),
                        Name = "lblDongCua"
                    };
                    pnlImage.Controls.Add(lblNew);
                    lblNew.BringToFront();

                    var timer = new System.Windows.Forms.Timer { Interval = 600 };
                    var isHighlight = false;
                    timer.Tick += (s, e) =>
                    {
                        if (lblNew.IsDisposed)
                        {
                            timer.Stop();
                            timer.Dispose();
                            return;
                        }
                        isHighlight = !isHighlight;
                        lblNew.BackColor = isHighlight
                            ? Color.FromArgb(239, 68, 68)
                            : Color.FromArgb(220, 38, 38);
                    };
                    timer.Start();
                }
                else if (isSapDongCua)
                {
                    var phutConLai = _gioHoatDongService.TinhSoPhutConLaiDenDongCua();
                    var lblNew = new Label
                    {
                        Text = $"⚠️ {phutConLai}p",
                        Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                        BackColor = Color.FromArgb(234, 179, 8),
                        ForeColor = Color.White,
                        AutoSize = false,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Size = new Size(75, 26),
                        Location = new Point(CARD_WIDTH - 85, 10),
                        Name = "lblSapDong"
                    };
                    pnlImage.Controls.Add(lblNew);
                    lblNew.BringToFront();

                    var timer = new System.Windows.Forms.Timer { Interval = 800 };
                    var isHighlight = false;
                    timer.Tick += (s, e) =>
                    {
                        if (lblNew.IsDisposed)
                        {
                            timer.Stop();
                            timer.Dispose();
                            return;
                        }
                        isHighlight = !isHighlight;
                        lblNew.BackColor = isHighlight
                            ? Color.FromArgb(251, 191, 36)
                            : Color.FromArgb(234, 179, 8);
                    };
                    timer.Start();
                }
            }

            // Update info label
            var lblInfo = card.Controls.Find("lblInfo", false).FirstOrDefault() as Label;
            if (lblInfo != null)
            {
                UpdateInfoLabelText(lblInfo, updatedBan);
            }

            // ✅ Update time info label cho bàn đang chơi
            var lblTimeInfo = card.Controls.Find("lblTimeInfo", false).FirstOrDefault() as Label;
            if (updatedBan.TrangThai == "Đang chơi")
            {
                if (lblTimeInfo == null)
                {
                    lblTimeInfo = CreateTimeInfoLabel(updatedBan);
                    card.Controls.Add(lblTimeInfo);
                }
                else
                {
                    UpdateTimeInfoText(lblTimeInfo, updatedBan);
                }
            }
            else if (lblTimeInfo != null)
            {
                card.Controls.Remove(lblTimeInfo);
                lblTimeInfo.Dispose();
            }

            // ✅ Update booking time info label cho bàn đã đặt
            var lblBookingTimeInfo = card.Controls.Find("lblBookingTimeInfo", false).FirstOrDefault() as Label;
            if (updatedBan.TrangThai == "Đã đặt")
            {
                if (lblBookingTimeInfo == null)
                {
                    lblBookingTimeInfo = CreateBookingTimeInfoLabel(updatedBan);
                    card.Controls.Add(lblBookingTimeInfo);
                }
                else
                {
                    UpdateBookingTimeInfoText(lblBookingTimeInfo, updatedBan);
                }
            }
            else if (lblBookingTimeInfo != null)
            {
                card.Controls.Remove(lblBookingTimeInfo);
                lblBookingTimeInfo.Dispose();
            }

            // Update customer label
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
            // ✅ MỚI: Xử lý cho cả "Đang chờ" và "Đã đặt"
            else if (updatedBan.TrangThai == "Đang chờ" || updatedBan.TrangThai == "Đã đặt")
            {
                if (lblCustomer == null)
                {
                    lblCustomer = CreateCustomerLabelForReserved(updatedBan);
                    card.Controls.Add(lblCustomer);
                }
                else
                {
                    var label = updatedBan.TrangThai == "Đang chờ" ? "Chờ xác nhận" : "Khách đặt";
                    lblCustomer.Text = $"👤 {updatedBan.MaKhNavigation?.TenKh ?? label}";
                }
            }
            else if (lblCustomer != null)
            {
                card.Controls.Remove(lblCustomer);
                lblCustomer.Dispose();
            }
            // Update price label position
            var lblPrice = card.Controls.OfType<Label>()
            .FirstOrDefault(l => l.Text.Contains("đ/giờ"));
            if (lblPrice != null)
            {
                // ✅ Cả "Đang chơi", "Đang chờ" và "Đã đặt" đều dùng position 275
                int yPos = (updatedBan.TrangThai == "Đang chơi" ||
                            updatedBan.TrangThai == "Đang chờ" ||
                            updatedBan.TrangThai == "Đã đặt") ? 275 : 235;
                lblPrice.Location = new Point(0, yPos);
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

            // ✅ TỰ ĐỘNG CẬP NHẬT BADGE
            await UpdateReservationBadgeAsync();

            // Reload detail if visible
            if (_isDetailVisible && _chiTietControl != null)
            {
                await _chiTietControl.LoadBanDetail();
            }
        }
        private void UpdateReservationBadge(int count)
        {
            if (_lblReservationBadge == null)
            {
                System.Diagnostics.Debug.WriteLine("⚠️ Badge chưa được khởi tạo!");
                return;
            }

            _currentReservationCount = count;

            if (count > 0)
            {
                // Hiển thị badge với số đơn đặt
                _lblReservationBadge.Text = count > 99 ? "99+" : count.ToString();
                _lblReservationBadge.Visible = true;
                _lblReservationBadge.Invalidate();  // Vẽ lại badge

                System.Diagnostics.Debug.WriteLine($"✅ Badge cập nhật: {_lblReservationBadge.Text}");
            }
            else
            {
                // Ẩn badge khi không có đơn đặt
                _lblReservationBadge.Visible = false;
                System.Diagnostics.Debug.WriteLine($"ℹ️ Không có đơn đặt → ẩn badge");
            }
        }
        private async Task UpdateReservationBadgeAsync()
        {
            try
            {
                // Load số đơn đặt từ database
                var reservationCounts = await _banBiaService
                    .GetReservationCountsForDateAsync(DateTime.Today);

                // Tính tổng số đơn đặt
                var totalReservations = reservationCounts.Values.Sum();

                System.Diagnostics.Debug.WriteLine($"📊 Tìm thấy {totalReservations} đơn đặt");

                // Cập nhật badge
                UpdateReservationBadge(totalReservations);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Lỗi cập nhật badge: {ex.Message}");
                // Không throw để tránh ảnh hưởng luồng chính
            }
        }
        private async Task RefreshTablesSmooth()
        {
            try
            {
                await KiemTraVaXuLyBanQuaGioChoPhep();
                await KiemTraVaXuLyBanDenGioDongCua();

                var newTables = await _banBiaService.GetAllTablesAsync();

                if (!HasTableChanges(newTables))
                {
                    // ✅ KHÔNG CÓ THAY ĐỔI NHƯNG VẪN CẬP NHẬT BADGE
                    await UpdateReservationBadgeAsync();
                    return;
                }

                _allTables = newTables;
                await UpdateExistingCardsSmooth();

                // ✅ CẬP NHẬT BADGE
                await UpdateReservationBadgeAsync();

                if (_isDetailVisible && _chiTietControl != null)
                {
                    await _chiTietControl.LoadBanDetail(forceReload: false);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi refresh: {ex.Message}");
            }
        }
        private async Task KiemTraVaXuLyBanQuaGioChoPhep()
        {
            try
            {
                var banQuaGio = await _banBiaService.KiemTraBanQuaGioChoPhep();

                if (banQuaGio.Count == 0)
                    return;

                // Đánh dấu bàn cần thanh toán
                foreach (var ban in banQuaGio)
                {
                    await _banBiaService.DanhDauBanCanThanhToan(ban.MaBan, true);
                }

                // Hiển thị cảnh báo (chỉ hiện 1 lần mỗi session)
                if (!_daHienThiCanhBaoQuaGio)
                {
                    _daHienThiCanhBaoQuaGio = true;

                    var danhSachBan = string.Join(", ", banQuaGio.Select(b => b.TenBan));
                    var soGioToiDa = _gioHoatDongService.LaySoGioHoatDongToiDa();

                    var result = MessageBox.Show(
                        $"⛔ CẢNH BÁO KHẨN CẤP!\n\n" +
                        $"Các bàn sau đã chơi QUÁ {soGioToiDa} TIẾNG:\n{danhSachBan}\n\n" +
                        $"Vui lòng THANH TOÁN NGAY LẬP TỨC!\n\n" +
                        $"Bạn có muốn xem danh sách chi tiết?",
                        "Bàn chơi quá giờ cho phép",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Error);

                    if (result == DialogResult.Yes)
                    {
                        HienThiFormBanQuaGio(banQuaGio);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi kiểm tra bàn quá giờ: {ex.Message}");
            }
        }

        private async Task HienThiFormBanQuaGio(List<BanBium> danhSachBan)
        {
            var form = new Form
            {
                Text = "Bàn chơi quá giờ cho phép",
                Size = new Size(650, 450),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var soGioToiDa = _gioHoatDongService.LaySoGioHoatDongToiDa();

            var lblTitle = new Label
            {
                Text = $"⛔ BÀN ĐÃ CHƠI QUÁ {soGioToiDa} TIẾNG",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(153, 27, 27),
                Location = new Point(20, 20),
                AutoSize = true
            };
            form.Controls.Add(lblTitle);

            var lblWarning = new Label
            {
                Text = "Các bàn này ĐÃ VƯỢT QUÁ thời gian hoạt động cho phép của quán!",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(220, 38, 38),
                Location = new Point(20, 55),
                AutoSize = true
            };
            form.Controls.Add(lblWarning);

            var dgv = new DataGridView
            {
                Location = new Point(20, 90),
                Size = new Size(590, 270),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            dgv.Columns.Add("TenBan", "Bàn");
            dgv.Columns.Add("KhuVuc", "Khu vực");
            dgv.Columns.Add("GioBatDau", "Giờ bắt đầu");
            dgv.Columns.Add("SoGioChoi", "Số giờ đã chơi");
            dgv.Columns.Add("TienTamTinh", "Tiền tạm tính");

            foreach (var ban in danhSachBan)
            {
                var duration = DateTime.Now - (ban.GioBatDau ?? DateTime.Now);
                var (tienTamTinh, ghiChu) = await _banBiaService.TinhTienTamThoiBan(ban.MaBan);

                var row = dgv.Rows.Add(
                    ban.TenBan,
                    ban.MaKhuVucNavigation?.TenKhuVuc ?? "-",
                    ban.GioBatDau?.ToString("HH:mm dd/MM") ?? "-",
                    $"{duration.TotalHours:F1}h ⚠️",
                    $"{tienTamTinh:N0} đ"
                );

                dgv.Rows[row].DefaultCellStyle.BackColor = Color.FromArgb(254, 242, 242);
                dgv.Rows[row].DefaultCellStyle.ForeColor = Color.FromArgb(153, 27, 27);
                dgv.Rows[row].DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            }

            form.Controls.Add(dgv);

            var btnDong = new Button
            {
                Text = "Đóng",
                Location = new Point(510, 370),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(148, 163, 184),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnDong.FlatAppearance.BorderSize = 0;
            btnDong.Click += (s, e) => form.Close();
            form.Controls.Add(btnDong);

            form.ShowDialog(this);
        }

        private async Task KiemTraVaXuLyBanDenGioDongCua()
        {
            try
            {
                var banDenGioDongCua = await _banBiaService.KiemTraBanDenGioDongCua();

                if (banDenGioDongCua.Count == 0)
                    return;

                var danhSachBan = string.Join(", ", banDenGioDongCua.Select(b => b.TenBan));

                var result = MessageBox.Show(
                    $"⚠️ ĐÃ ĐẾN GIỜ ĐÓNG CỬA!\n\n" +
                    $"Các bàn sau cần thanh toán NGAY:\n{danhSachBan}\n\n" +
                    $"Bạn có muốn xem danh sách chi tiết?",
                    "Cảnh báo giờ đóng cửa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    HienThiFormBanCanThanhToan(banDenGioDongCua);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi kiểm tra giờ đóng cửa: {ex.Message}");
            }
        }

        private async Task HienThiFormBanCanThanhToan(List<BanBium> danhSachBan)
        {
            var form = new Form
            {
                Text = "Bàn cần thanh toán ngay",
                Size = new Size(600, 400),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lblTitle = new Label
            {
                Text = "⚠️ CÁC BÀN ĐÃ ĐẾN GIỜ ĐÓNG CỬA",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 38, 38),
                Location = new Point(20, 20),
                AutoSize = true
            };
            form.Controls.Add(lblTitle);

            var dgv = new DataGridView
            {
                Location = new Point(20, 60),
                Size = new Size(540, 250),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            dgv.Columns.Add("TenBan", "Bàn");
            dgv.Columns.Add("KhuVuc", "Khu vực");
            dgv.Columns.Add("GioBatDau", "Giờ bắt đầu");
            dgv.Columns.Add("ThoiGianChoi", "Thời gian chơi");
            dgv.Columns.Add("TienTamTinh", "Tiền tạm tính");

            foreach (var ban in danhSachBan)
            {
                var duration = DateTime.Now - (ban.GioBatDau ?? DateTime.Now);
                var (tienTamTinh, ghiChu) = await _banBiaService.TinhTienTamThoiBan(ban.MaBan);

                dgv.Rows.Add(
                    ban.TenBan,
                    ban.MaKhuVucNavigation?.TenKhuVuc ?? "-",
                    ban.GioBatDau?.ToString("HH:mm dd/MM") ?? "-",
                    $"{(int)duration.TotalHours}h {duration.Minutes}m",
                    $"{tienTamTinh:N0} đ"
                );
            }

            form.Controls.Add(dgv);

            var btnDong = new Button
            {
                Text = "Đóng",
                Location = new Point(460, 320),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(148, 163, 184),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnDong.FlatAppearance.BorderSize = 0;
            btnDong.Click += (s, e) => form.Close();
            form.Controls.Add(btnDong);

            form.ShowDialog(this);
        }

        private async Task UpdateExistingCardsSmooth()
        {
            var filteredTables = GetFilteredTables().ToList();
            var existingCards = flpBanBia.Controls.OfType<Panel>()
                .Where(p => p.Tag is BanBium)
                .ToList();

            flpBanBia.SuspendLayout();

            try
            {
                foreach (var card in existingCards)
                {
                    var ban = card.Tag as BanBium;
                    var updatedBan = filteredTables.FirstOrDefault(t => t.MaBan == ban.MaBan);

                    if (updatedBan != null)
                    {
                        UpdateCardForBanSmooth(card, updatedBan);
                    }
                    else
                    {
                        flpBanBia.Controls.Remove(card);
                        card.Dispose();
                    }
                }

                var existingIds = existingCards.Select(c => ((BanBium)c.Tag).MaBan).ToList();
                var newTables = filteredTables.Where(t => !existingIds.Contains(t.MaBan)).ToList();

                if (newTables.Count > 0)
                {
                    foreach (var ban in newTables)
                    {
                        var card = CreateTableCard(ban);
                        flpBanBia.Controls.Add(card);
                    }
                }

                if (flpBanBia.Controls.Count == 0)
                {
                    ShowEmptyState();
                }
            }
            finally
            {
                flpBanBia.ResumeLayout(true);
            }

            await Task.Yield();
        }

        private void UpdateCardForBanSmooth(Panel card, BanBium updatedBan)
        {
            var oldBan = card.Tag as BanBium;

            if (oldBan.TrangThai == updatedBan.TrangThai &&
                oldBan.GioBatDau == updatedBan.GioBatDau &&
                oldBan.MaKh == updatedBan.MaKh)
            {
                return;
            }

            card.SuspendLayout();

            try
            {
                card.Tag = updatedBan;

                if (oldBan.TrangThai != updatedBan.TrangThai)
                {
                    card.BackColor = GetCardBackColor(updatedBan.TrangThai);

                    var pnlImage = card.Controls.OfType<Panel>().FirstOrDefault();
                    var lblStatus = pnlImage?.Controls.Find("lblStatus", false).FirstOrDefault() as Label;
                    if (lblStatus != null)
                    {
                        lblStatus.Text = updatedBan.TrangThai;
                        lblStatus.BackColor = GetStatusColor(updatedBan.TrangThai);
                    }
                }

                var lblInfo = card.Controls.Find("lblInfo", false).FirstOrDefault() as Label;
                if (lblInfo != null)
                {
                    UpdateInfoLabelText(lblInfo, updatedBan);
                }

                // ✅ Update time info cho bàn đang chơi
                var lblTimeInfo = card.Controls.Find("lblTimeInfo", false).FirstOrDefault() as Label;
                if (updatedBan.TrangThai == "Đang chơi")
                {
                    if (lblTimeInfo == null)
                    {
                        lblTimeInfo = CreateTimeInfoLabel(updatedBan);
                        card.Controls.Add(lblTimeInfo);
                    }
                    else
                    {
                        UpdateTimeInfoText(lblTimeInfo, updatedBan);
                    }
                }
                else if (lblTimeInfo != null)
                {
                    card.Controls.Remove(lblTimeInfo);
                    lblTimeInfo.Dispose();
                }

                // ✅ Update booking time info cho bàn đã đặt
                var lblBookingTimeInfo = card.Controls.Find("lblBookingTimeInfo", false).FirstOrDefault() as Label;
                if (updatedBan.TrangThai == "Đã đặt")
                {
                    if (lblBookingTimeInfo == null)
                    {
                        lblBookingTimeInfo = CreateBookingTimeInfoLabel(updatedBan);
                        card.Controls.Add(lblBookingTimeInfo);
                    }
                    else
                    {
                        UpdateBookingTimeInfoText(lblBookingTimeInfo, updatedBan);
                    }
                }
                else if (lblBookingTimeInfo != null)
                {
                    card.Controls.Remove(lblBookingTimeInfo);
                    lblBookingTimeInfo.Dispose();
                }

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
                else if (updatedBan.TrangThai == "Đã đặt")
                {
                    if (lblCustomer == null)
                    {
                        lblCustomer = CreateCustomerLabelForReserved(updatedBan);
                        card.Controls.Add(lblCustomer);
                    }
                    else
                    {
                        lblCustomer.Text = $"👤 {updatedBan.MaKhNavigation?.TenKh ?? "Khách đặt"}";
                    }
                }
                else if (lblCustomer != null)
                {
                    card.Controls.Remove(lblCustomer);
                    lblCustomer.Dispose();
                }
            }
            finally
            {
                card.ResumeLayout(true);
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

            // ✅ NẾU ĐANG Ở RESERVATION VIEW MODE → Chỉ hiện bàn đã đặt/chờ
            if (_isReservationViewMode)
            {
                filtered = filtered.Where(b =>
                    b.TrangThai == "Đã đặt" ||
                    b.TrangThai == "Đang chờ");
            }
            else
            {
                // Filter bình thường theo status
                if (_currentStatusFilter != "all")
                {
                    filtered = filtered.Where(b => b.TrangThai == _currentStatusFilter);
                }
            }

            // Các filter khác vẫn hoạt động bình thường
            if (_currentAreaFilter != "all")
            {
                filtered = filtered.Where(b => b.MaKhuVucNavigation?.TenKhuVuc == _currentAreaFilter);
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
                "Trống" => Color.FromArgb(240, 253, 244),      // Xanh lá nhạt
                "Đang chơi" => Color.FromArgb(254, 242, 242),  // Đỏ nhạt
                "Đá đặt" => Color.FromArgb(254, 252, 232),     // Vàng nhạt
                "Bảo trì" => Color.FromArgb(248, 250, 252),    // Xám nhạt
                _ => Color.White
            };
        }

        private Color GetStatusColor(string status)
        {
            return status switch
            {
                "Trống" => Color.FromArgb(34, 197, 94),        // Xanh lá
                "Đang chơi" => Color.FromArgb(220, 38, 38),    // Đỏ
                "Đã đặt" => Color.FromArgb(234, 179, 8),       // Vàng
                "Bảo trì" => Color.FromArgb(148, 163, 184),    // Xám
                _ => Color.FromArgb(100, 116, 139)             // Xám đậm
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
            try
            {
                using (var soDoBanForm = new SoDoBanForm(_banBiaService))
                {
                    soDoBanForm.SetMainForm(_mainForm);

                    soDoBanForm.OnTableSelected += (s, selectedBan) =>
                    {
                        var selectedCard = flpBanBia.Controls
                            .OfType<Panel>()
                            .FirstOrDefault(p => p.Tag is BanBium ban && ban.MaBan == selectedBan.MaBan);

                        if (selectedCard != null)
                        {
                            flpBanBia.ScrollControlIntoView(selectedCard);
                            ShowTableDetail(selectedBan);
                        }
                    };

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
                // Lấy các service cần thiết từ DI Container
                var datBanService = Program.GetService<DatBanService>();
                var banBiaService = Program.GetService<BanBiaService>();

                using (var danhSachForm = new DanhSachBanDatForm(datBanService, banBiaService, _mainForm))
                {
                    danhSachForm.StartPosition = FormStartPosition.CenterParent;
                    danhSachForm.ShowDialog(this);

                    // Sau khi đóng danh sách, refresh lại danh sách bàn để cập nhật trạng thái nếu có thay đổi
                    _ = RefreshTables();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở danh sách bàn đặt: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDatBan_Click(object sender, EventArgs e)
        {
            try
            {
                var banBiaService = Program.GetService<BanBiaService>();
                var datBanService = Program.GetService<DatBanService>();

                using (var datBanForm = new DatBanForm(banBiaService, datBanService))
                {
                    datBanForm.StartPosition = FormStartPosition.CenterParent;
                    var result = datBanForm.ShowDialog(this);

                    if (result == DialogResult.OK)
                    {
                        // Refresh lại giao diện nếu đặt bàn thành công
                        _ = RefreshTables();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở form đặt bàn: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        private async Task CheckReservationsNearStartTime()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"\n╔═══════════════════════════════════════════════════════════╗");
                System.Diagnostics.Debug.WriteLine($"║  🔍 KIỂM TRA ĐƠN ĐẶT BÀN - {DateTime.Now:HH:mm:ss dd/MM}     ║");
                System.Diagnostics.Debug.WriteLine($"╚═══════════════════════════════════════════════════════════╝");

                // ═══════════════════════════════════════════════════════════
                // 1. LẤY SERVICE VÀ DANH SÁCH ĐƠN ĐẶT
                // ═══════════════════════════════════════════════════════════
                var datBanService = Program.GetService<DatBanService>();
                if (datBanService == null)
                {
                    System.Diagnostics.Debug.WriteLine("❌ Không tìm thấy DatBanService");
                    return;
                }

                // Lấy danh sách đơn đặt trong khoảng ±15 phút
                var nearStartReservations = await datBanService.GetReservationsNearStartTimeAsync();

                if (nearStartReservations == null || !nearStartReservations.Any())
                {
                    System.Diagnostics.Debug.WriteLine("✅ Không có đơn đặt nào cần xử lý");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"📊 Tìm thấy {nearStartReservations.Count} đơn đặt cần kiểm tra\n");

                // ═══════════════════════════════════════════════════════════
                // 2. DUYỆT VÀ XỬ LÝ TỪNG ĐƠN ĐẶT
                // ═══════════════════════════════════════════════════════════
                foreach (var reservation in nearStartReservations)
                {
                    try
                    {
                        // ───────────────────────────────────────────────────
                        // 2.1. KIỂM TRA ĐÃ XỬ LÝ CHƯA
                        // ───────────────────────────────────────────────────
                        if (_processedReservations.Contains(reservation.MaDat))
                        {
                            System.Diagnostics.Debug.WriteLine($"   ⏭️  Đơn #{reservation.MaDat} đã xử lý, bỏ qua");
                            continue;
                        }

                        // ───────────────────────────────────────────────────
                        // 2.2. VALIDATE THỜI GIAN BẮT ĐẦU
                        // ───────────────────────────────────────────────────
                        if (!reservation.ThoiGianBatDau.HasValue)
                        {
                            System.Diagnostics.Debug.WriteLine($"   ⚠️  Đơn #{reservation.MaDat} không có thời gian bắt đầu");
                            continue;
                        }

                        // ───────────────────────────────────────────────────
                        // 2.3. TÍNH TOÁN THỜI GIAN CHÊNH LỆCH
                        // ───────────────────────────────────────────────────
                        var now = DateTime.Now;
                        var startTime = reservation.ThoiGianBatDau.Value;
                        var timeUntilStart = startTime - now;
                        bool isOverdue = timeUntilStart.TotalMinutes < 0; // Đã quá giờ
                        int minutes = Math.Abs((int)timeUntilStart.TotalMinutes);

                        System.Diagnostics.Debug.WriteLine($"\n┌──────────────────────────────────────────────┐");
                        System.Diagnostics.Debug.WriteLine($"│ 🎫 Đơn #{reservation.MaDat}");
                        System.Diagnostics.Debug.WriteLine($"│ 🏷️  Bàn: {reservation.MaBanNavigation?.TenBan ?? "N/A"}");
                        System.Diagnostics.Debug.WriteLine($"│ 👤  Khách: {reservation.TenKhach ?? "N/A"}");
                        System.Diagnostics.Debug.WriteLine($"│ ⏰  Giờ hẹn: {startTime:HH:mm}");
                        System.Diagnostics.Debug.WriteLine($"│ ⏱️  Hiện tại: {now:HH:mm}");
                        System.Diagnostics.Debug.WriteLine($"│ 📊  Trạng thái: {(isOverdue ? $"Quá giờ {minutes} phút" : $"Còn {minutes} phút")}");
                        System.Diagnostics.Debug.WriteLine($"└──────────────────────────────────────────────┘");

                        // ═══════════════════════════════════════════════════════════
                        // 3. XỬ LÝ THEO TÌNH HUỐNG
                        // ═══════════════════════════════════════════════════════════

                        // ┌────────────────────────────────────────────────────────┐
                        // │ TÌNH HUỐNG A: QUÁ GIỜ > 15 PHÚT → TỰ ĐỘNG HỦY         │
                        // └────────────────────────────────────────────────────────┘
                        if (isOverdue && minutes > AUTO_CANCEL_THRESHOLD_MINUTES)
                        {
                            System.Diagnostics.Debug.WriteLine($"   ❌ Quá giờ {minutes} phút (> {AUTO_CANCEL_THRESHOLD_MINUTES}p) → TỰ ĐỘNG HỦY");

                            // Đánh dấu đã xử lý TRƯỚC khi hủy
                            _processedReservations.Add(reservation.MaDat);

                            // Tự động hủy đơn đặt
                            var cancelResult = await datBanService.AutoCancelExpiredReservationAsync(reservation.MaDat);

                            if (cancelResult)
                            {
                                System.Diagnostics.Debug.WriteLine($"   ✅ Đã tự động hủy đơn #{reservation.MaDat}");

                                // Hiển thị thông báo ngắn gọn
                                this.Invoke((MethodInvoker)delegate
                                {
                                    MessageBox.Show(
                                        $"🔔 ĐÃ TỰ ĐỘNG HỦY ĐƠN ĐẶT BÀN\n\n" +
                                        $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                                        $"🏷️  Bàn: {reservation.MaBanNavigation?.TenBan}\n" +
                                        $"👤  Khách: {reservation.TenKhach}\n" +
                                        $"📞  SĐT: {reservation.Sdt}\n" +
                                        $"⏰  Giờ hẹn: {startTime:HH:mm}\n" +
                                        $"⏱️  Quá giờ: {minutes} phút\n" +
                                        $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +
                                        $"Lý do: Khách chưa đến sau {AUTO_CANCEL_THRESHOLD_MINUTES} phút\n" +
                                        $"Bàn đã trở về trạng thái TRỐNG",
                                        "Thông báo tự động hủy",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                                });

                                // Refresh danh sách bàn
                                await RefreshTables();
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine($"   ❌ Lỗi khi tự động hủy đơn #{reservation.MaDat}");
                                _processedReservations.Remove(reservation.MaDat); // Cho phép thử lại
                            }

                            continue; // Tiếp tục với đơn tiếp theo
                        }

                        // ┌────────────────────────────────────────────────────────┐
                        // │ TÌNH HUỐNG B: QUÁ GIỜ 10-15 PHÚT → HỎI GIỮ/HỦY        │
                        // └────────────────────────────────────────────────────────┘
                        if (isOverdue && minutes >= WARNING_THRESHOLD_MINUTES && minutes <= AUTO_CANCEL_THRESHOLD_MINUTES)
                        {
                            System.Diagnostics.Debug.WriteLine($"   ⚠️ Quá giờ {minutes} phút ({WARNING_THRESHOLD_MINUTES}-{AUTO_CANCEL_THRESHOLD_MINUTES}p) → DIALOG");

                            // Đánh dấu đã xử lý TRƯỚC khi hiển thị MessageBox
                            _processedReservations.Add(reservation.MaDat);

                            string message =
                                $"⚠️ KHÁCH CHƯA ĐẾN - ĐÃ QUÁ GIỜ {minutes} PHÚT!\n\n" +
                                $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                                $"🏷️  Bàn: {reservation.MaBanNavigation?.TenBan ?? "N/A"}\n" +
                                $"📍  Khu vực: {reservation.MaBanNavigation?.MaKhuVucNavigation?.TenKhuVuc ?? "N/A"}\n" +
                                $"👤  Khách hàng: {reservation.TenKhach ?? "N/A"}\n" +
                                $"📞  SĐT: {reservation.Sdt ?? "N/A"}\n" +
                                $"⏰  Giờ hẹn: {startTime:HH:mm}\n" +
                                $"⏱️  Hiện tại: {now:HH:mm}\n" +
                                $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +
                                $"⚡ LƯU Ý: Sau {AUTO_CANCEL_THRESHOLD_MINUTES} phút sẽ TỰ ĐỘNG HỦY!\n\n" +
                                $"Bạn muốn:\n\n" +
                                $"• YES = GIỮ BÀN (gia hạn thêm 15 phút)\n" +
                                $"• NO = HỦY ĐẶT BÀN (bàn trở về trạng thái trống)";

                            var result = MessageBox.Show(
                                message,
                                $"⚠️ KHÁCH CHƯA ĐẾN - QUÁ GIỜ {minutes} PHÚT",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning);

                            if (result == DialogResult.Yes)
                            {
                                // ═══════════════════════════════════════════════════
                                // CHỌN GIỮ BÀN - GIA HẠN THÊM 15 PHÚT
                                // ═══════════════════════════════════════════════════
                                System.Diagnostics.Debug.WriteLine($"   ✅ User chọn GIỮ BÀN #{reservation.MaDat}");

                                var keepResult = await datBanService.KeepReservationAsync(reservation.MaDat);

                                if (keepResult)
                                {
                                    var newStartTime = DateTime.Now.AddMinutes(15);

                                    MessageBox.Show(
                                        $"✅ ĐÃ GIỮ ĐƠN ĐẶT BÀN!\n\n" +
                                        $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                                        $"🏷️  Bàn: {reservation.MaBanNavigation?.TenBan}\n" +
                                        $"👤  Khách: {reservation.TenKhach}\n" +
                                        $"⏰  Giờ bắt đầu MỚI: {newStartTime:HH:mm}\n" +
                                        $"⏱️  Thời gian gia hạn: 15 phút\n" +
                                        $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +
                                        $"Hệ thống sẽ kiểm tra lại sau 15 phút.",
                                        "✅ Giữ bàn thành công",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);

                                    // XÓA khỏi processed để có thể check lại sau 15 phút
                                    _processedReservations.Remove(reservation.MaDat);
                                    System.Diagnostics.Debug.WriteLine($"   🔄 Xóa đơn #{reservation.MaDat} khỏi processed để check lại");
                                }
                                else
                                {
                                    MessageBox.Show(
                                        "❌ Không thể giữ đơn đặt bàn!\n\nVui lòng thử lại.",
                                        "Lỗi",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                                    _processedReservations.Remove(reservation.MaDat); // Cho phép thử lại
                                }
                            }
                            else
                            {
                                // ═══════════════════════════════════════════════════
                                // CHỌN HỦY BÀN - XÁC NHẬN LẠI MỘT LẦN NỮA
                                // ═══════════════════════════════════════════════════
                                System.Diagnostics.Debug.WriteLine($"   ⚠️ User chọn HỦY BÀN #{reservation.MaDat}");

                                var cancelConfirm = MessageBox.Show(
                                    $"⚠️ XÁC NHẬN HỦY ĐẶT BÀN?\n\n" +
                                    $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                                    $"🏷️  Bàn: {reservation.MaBanNavigation?.TenBan}\n" +
                                    $"👤  Khách: {reservation.TenKhach}\n" +
                                    $"📞  SĐT: {reservation.Sdt}\n" +
                                    $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +
                                    $"Sau khi hủy, bàn sẽ trở về trạng thái TRỐNG\n" +
                                    $"và khách hàng khác có thể đặt.\n\n" +
                                    $"Bạn có CHẮC CHẮN muốn hủy?",
                                    "⚠️ Xác nhận hủy đặt bàn",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question);

                                if (cancelConfirm == DialogResult.Yes)
                                {
                                    var cancelResult = await datBanService.AutoCancelExpiredReservationAsync(reservation.MaDat);

                                    if (cancelResult)
                                    {
                                        MessageBox.Show(
                                            $"✅ ĐÃ HỦY ĐƠN ĐẶT BÀN!\n\n" +
                                            $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                                            $"🏷️  Bàn: {reservation.MaBanNavigation?.TenBan}\n" +
                                            $"📊  Trạng thái: TRỐNG\n" +
                                            $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +
                                            $"Bàn đã sẵn sàng cho khách hàng khác.",
                                            "✅ Hủy thành công",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);

                                        // Refresh danh sách bàn
                                        await RefreshTables();
                                        System.Diagnostics.Debug.WriteLine($"   ✅ Đã hủy đơn #{reservation.MaDat} và refresh");
                                    }
                                    else
                                    {
                                        MessageBox.Show(
                                            "❌ Không thể hủy đơn đặt bàn!\n\nVui lòng thử lại.",
                                            "Lỗi",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                                        _processedReservations.Remove(reservation.MaDat); // Cho phép thử lại
                                    }
                                }
                                else
                                {
                                    // User không xác nhận hủy - Xóa khỏi processed để có thể hỏi lại
                                    System.Diagnostics.Debug.WriteLine($"   ℹ️ User không xác nhận hủy đơn #{reservation.MaDat}");
                                    _processedReservations.Remove(reservation.MaDat);
                                }
                            }

                            continue; // Tiếp tục với đơn tiếp theo
                        }

                        // ┌────────────────────────────────────────────────────────┐
                        // │ TÌNH HUỐNG C: QUÁ GIỜ < 10 PHÚT → CHƯA XỬ LÝ         │
                        // └────────────────────────────────────────────────────────┘
                        if (isOverdue && minutes < WARNING_THRESHOLD_MINUTES)
                        {
                            System.Diagnostics.Debug.WriteLine($"   ⏳ Chưa đủ {WARNING_THRESHOLD_MINUTES} phút quá giờ, chưa cần xử lý");
                            continue; // Không thêm vào processed, để check lại lần sau
                        }

                        // ┌────────────────────────────────────────────────────────┐
                        // │ TÌNH HUỐNG D: SẮP ĐẾN GIỜ (5-10 PHÚT) → THÔNG BÁO    │
                        // └────────────────────────────────────────────────────────┘
                        if (!isOverdue && minutes >= 5 && minutes <= 10)
                        {
                            System.Diagnostics.Debug.WriteLine($"   🔔 Sắp đến giờ {minutes} phút, hiển thị thông báo");

                            // Đánh dấu đã xử lý
                            _processedReservations.Add(reservation.MaDat);

                            string message =
                                $"🔔 ĐƠN ĐẶT BÀN SẮP ĐẾN GIỜ!\n\n" +
                                $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                                $"⏰  Còn: {minutes} PHÚT\n" +
                                $"🏷️  Bàn: {reservation.MaBanNavigation?.TenBan ?? "N/A"}\n" +
                                $"📍  Khu vực: {reservation.MaBanNavigation?.MaKhuVucNavigation?.TenKhuVuc ?? "N/A"}\n" +
                                $"👤  Khách: {reservation.TenKhach ?? "N/A"}\n" +
                                $"📞  SĐT: {reservation.Sdt ?? "N/A"}\n" +
                                $"⏰  Giờ hẹn: {startTime:HH:mm}\n" +
                                $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +
                                $"Vui lòng CHUẨN BỊ BÀN cho khách!";

                            MessageBox.Show(
                                message,
                                "🔔 Thông báo đơn đặt bàn",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            System.Diagnostics.Debug.WriteLine($"   ✅ Đã thông báo đơn #{reservation.MaDat}");
                        }

                        // ┌────────────────────────────────────────────────────────┐
                        // │ TÌNH HUỐNG E: CHƯA ĐẾN GIỜ (> 10 PHÚT) → KHÔNG LÀM GÌ │
                        // └────────────────────────────────────────────────────────┘
                        // Không cần xử lý gì, chỉ để code rõ ràng

                    }
                    catch (Exception exReservation)
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ Lỗi xử lý đơn #{reservation.MaDat}: {exReservation.Message}");
                        // Tiếp tục với đơn tiếp theo
                    }
                }

                System.Diagnostics.Debug.WriteLine($"\n╚═══════════════════════════════════════════════════════════╝\n");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ LỖI NGHIÊM TRỌNG trong CheckReservationsNearStartTime:");
                System.Diagnostics.Debug.WriteLine($"   Message: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"   Stack trace: {ex.StackTrace}");

                // Không hiển thị MessageBox để tránh spam user trong trường hợp lỗi lặp
                // Chỉ log lỗi và tiếp tục
            }
        }
        private async void QLBanForm_Load(object sender, EventArgs e)
        {
            try
            {
                // ✅ TẠO BADGE TRƯỚC KHI LOAD DATA
                InitializeReservationBadge();

                _refreshTimer.Start();
                _checkReservationTimer.Start();

                await LoadTables();

                // ✅ CẬP NHẬT BADGE SAU KHI LOAD XONG
                await UpdateReservationBadgeAsync();

                await CheckReservationsNearStartTime();

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
            _checkReservationTimer?.Stop();
            _checkReservationTimer?.Dispose();
            _chiTietControl?.Dispose();
            base.OnFormClosing(e);
        }

        #endregion
    }
}