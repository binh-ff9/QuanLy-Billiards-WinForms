using Billiard.BLL.Services.QLBan;
using Billiard.DAL.Entities;
using Billiard.WinForm.Forms.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.Users
{
    public partial class DatBanDialog : Form
    {
        private readonly DatBanService _datBanService;
        private int _maBan;
        private string _tenBan;

        // Controls
        private DateTimePicker dtpDate;
        private DateTimePicker dtpTime;
        private NumericUpDown nudHour;
        private NumericUpDown nudMinute;

        private Panel pnlTimeline;          // Control Timeline
        private FlowLayoutPanel pnlTimeSlots; // Control Slots (Nút bấm)

        private TextBox txtGhiChu;
        private Button _selectedSlot = null;

        // Dữ liệu dùng chung cho cả 2 view
        private List<DatBan> _bookingsOfTable = new();

        private const int START_HOUR = 8;
        private const int END_HOUR = 27; // 3h sáng hôm sau
        private bool _isUpdatingFromCode = false;


        private bool _isDragging = false;
        private Point _dragStartPoint;
        private Point _dragEndPoint;
        private DateTime _tempDragStartTime;
        private TimeSpan _tempDragDuration;
        private bool _isResizing = false; // <-- BIẾN MỚI
        private int _resizeThreshold = 10; // Khoảng cách (pixel) để nhận diện mép phải

        private DateTime? _hoverTime = null; // Thời gian tại vị trí chuột
        private int _hoverX = -1;

        public DatBanDialog(DatBanService service)
        {
            InitializeComponent();
            _datBanService = service;
            // Bật DoubleBuffered để vẽ Timeline mượt hơn, không nháy
            this.DoubleBuffered = true;
            SetupUI();
        }

        public void SetTableInfo(int maBan, string tenBan)
        {
            _maBan = maBan;
            _tenBan = tenBan;
            this.Text = $"Đặt bàn: {_tenBan}";
        }

        private void SetupUI()
        {
            // 1. Cấu hình Form (Tăng chiều cao để chứa cả 2)
            this.Size = new Size(600, 850);
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            int padding = 25;
            int currentY = 25;

            // 2. INPUT NGÀY GIỜ
            var lblDate = new Label { Text = "📅 Ngày:", Location = new Point(padding, currentY + 3), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            dtpDate = new DateTimePicker
            {
                Location = new Point(lblDate.Right + 5, currentY),
                Width = 110,
                Format = DateTimePickerFormat.Short,
                MinDate = DateTime.Now,
                Font = new Font("Segoe UI", 10)
            };
            dtpDate.ValueChanged += async (s, e) => await LoadAllDataAsync();

            var lblTimeDetail = new Label { Text = "🕒 Bắt đầu:", Location = new Point(dtpDate.Right + 15, currentY + 3), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            dtpTime = new DateTimePicker
            {
                Location = new Point(lblTimeDetail.Right + 5, currentY),
                Width = 70,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "HH:mm",
                ShowUpDown = true,
                Font = new Font("Segoe UI", 10)
            };
            // Khi đổi giờ -> Vẽ lại vùng chọn màu xanh trên Timeline
            dtpTime.ValueChanged += (s, e) => {
                if (!_isUpdatingFromCode) pnlTimeline.Invalidate();
                // Reset slot button nếu giờ chỉnh tay không khớp slot
                if (_selectedSlot != null && !_isUpdatingFromCode)
                {
                    _selectedSlot.BackColor = Color.White;
                    _selectedSlot.ForeColor = Color.Black;
                    _selectedSlot = null;
                }
            };

            this.Controls.AddRange(new Control[] { lblDate, dtpDate, lblTimeDetail, dtpTime });
            currentY += 50;

            // 3. THỜI LƯỢNG
            var lblDuration = new Label { Text = "⏳ Thời gian chơi:", Location = new Point(padding, currentY + 3), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) };

            nudHour = new NumericUpDown { Location = new Point(lblDuration.Right + 55, currentY), Width = 50, Minimum = 0, Maximum = 24, Value = 1, Font = new Font("Segoe UI", 10), TextAlign = HorizontalAlignment.Center };
            var lblH = new Label { Text = "giờ", Location = new Point(nudHour.Right + 2, currentY + 3), AutoSize = true, Font = new Font("Segoe UI", 10) };

            nudMinute = new NumericUpDown { Location = new Point(lblH.Right + 10, currentY), Width = 50, Minimum = 0, Maximum = 59, Value = 0, Increment = 15, Font = new Font("Segoe UI", 10), TextAlign = HorizontalAlignment.Center };
            var lblM = new Label { Text = "phút", Location = new Point(nudMinute.Right + 2, currentY + 3), AutoSize = true, Font = new Font("Segoe UI", 10) };

            // Khi đổi thời lượng -> Vẽ lại vùng chọn trên Timeline để dài ra/ngắn lại
            nudHour.ValueChanged += (s, e) => pnlTimeline.Invalidate();
            nudMinute.ValueChanged += (s, e) => pnlTimeline.Invalidate();

            this.Controls.AddRange(new Control[] { lblDuration, nudHour, lblH, nudMinute, lblM });
            currentY += 50;

            // 4. CHÚ THÍCH MÀU
            CreateLegendItem(padding, currentY, Color.White, "Trống", true);
            CreateLegendItem(padding + 100, currentY, Color.FromArgb(254, 202, 202), "Bận", false);
            CreateLegendItem(padding + 200, currentY, Color.FromArgb(99, 102, 241), "Đang chọn", false);
            currentY += 40;

            // ==========================================================
            // 5. TIMELINE (VIEW TỔNG QUAN)
            // ==========================================================
            var lblTimeline = new Label { Text = "Dòng thời gian (Kéo chọn tự do):", Location = new Point(padding, currentY), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            this.Controls.Add(lblTimeline);
            currentY += 25;

            pnlTimeline = new Panel
            {
                Location = new Point(padding, currentY),
                Size = new Size(530, 80), // Chiều cao vừa đủ vẽ thước
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Cross
            };
            pnlTimeline.Paint += PnlTimeline_Paint;
            pnlTimeline.MouseDown += PnlTimeline_MouseDown; // Bắt đầu kéo
            pnlTimeline.MouseMove += PnlTimeline_MouseMove; // Đang kéo
            pnlTimeline.MouseUp += PnlTimeline_MouseUp;     // Thả chuột (Kết thúc)
            pnlTimeline.MouseEnter += (s, e) => { }; // Không cần làm gì đặc biệt
            pnlTimeline.MouseLeave += (s, e) =>
            {
                _hoverTime = null; // Ra khỏi vùng thì xóa tooltip
                pnlTimeline.Invalidate();
            };
            this.Controls.Add(pnlTimeline);
            currentY += 90;

            // ==========================================================
            // 6. TIME SLOTS (CHỌN NHANH)
            // ==========================================================
            var lblSlots = new Label { Text = "Hoặc chọn khung giờ chẵn:", Location = new Point(padding, currentY), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            this.Controls.Add(lblSlots);
            currentY += 25;

            pnlTimeSlots = new FlowLayoutPanel
            {
                Location = new Point(padding, currentY),
                Size = new Size(540, 220), // Height đủ chứa các nút
                AutoScroll = true,
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(248, 250, 252)
            };
            this.Controls.Add(pnlTimeSlots);
            currentY += 230;

            // 7. GHI CHÚ & BUTTON
            var lblNote = new Label { Text = "📝 Ghi chú:", Location = new Point(padding, currentY), AutoSize = true, Font = new Font("Segoe UI", 10) };
            this.Controls.Add(lblNote);
            currentY += 25;

            txtGhiChu = new TextBox { Location = new Point(padding, currentY), Size = new Size(530, 50), Multiline = true, BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 10) };
            this.Controls.Add(txtGhiChu);

            var btnConfirm = new Button
            {
                Text = "XÁC NHẬN ĐẶT BÀN",
                Location = new Point(padding, this.ClientSize.Height - 70),
                Size = new Size(530, 50),
                BackColor = Color.FromArgb(99, 102, 241),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnConfirm.FlatAppearance.BorderSize = 0;
            btnConfirm.Click += BtnConfirm_Click;
            this.Controls.Add(btnConfirm);

            this.Load += async (s, e) => await LoadAllDataAsync();
        }

        // ============================================================
        // LOGIC LOAD DỮ LIỆU (TỐI ƯU HÓA)
        // ============================================================
        private async Task LoadAllDataAsync()
        {
            DateTime dateBase = dtpDate.Value.Date;

            // 1. Gọi Database một lần
            var bookedList = await _datBanService.GetByDateRangeAsync(dateBase, dateBase.AddDays(2));

            // 2. Lọc dữ liệu của bàn hiện tại
            _bookingsOfTable = bookedList
                .Where(b => b.MaBan == _maBan && b.TrangThai != "Đã hủy")
                .ToList();

            // 3. Cập nhật Timeline
            pnlTimeline.Invalidate();

            // 4. Cập nhật Slot Buttons
            RenderSlots(dateBase);
        }

        private void RenderSlots(DateTime dateBase)
        {
            pnlTimeSlots.Controls.Clear();
            pnlTimeSlots.SuspendLayout();
            _selectedSlot = null;

            for (int i = START_HOUR; i < END_HOUR; i++)
            {
                int displayHour = i % 24;
                DateTime slotDate = (i >= 24) ? dateBase.AddDays(1) : dateBase;
                DateTime slotStart = new DateTime(slotDate.Year, slotDate.Month, slotDate.Day, displayHour, 0, 0);
                DateTime slotEnd = slotStart.AddHours(1);

                var btn = new Button
                {
                    Text = $"{displayHour}:00",
                    Width = 90,
                    Height = 45,
                    Tag = slotStart,
                    Margin = new Padding(5),
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10)
                };
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = Color.LightGray;

                // Kiểm tra trùng lịch dựa trên list đã tải (_bookingsOfTable)
                bool isBusy = _bookingsOfTable.Any(b => b.ThoiGianBatDau < slotEnd && b.ThoiGianKetThuc > slotStart);

                if (isBusy)
                {
                    btn.BackColor = Color.FromArgb(254, 226, 226);
                    btn.ForeColor = Color.Red;
                    btn.Text += "\n(Bận)";
                    btn.Enabled = false;
                }
                else
                {
                    btn.BackColor = Color.White;
                    btn.Click += Slot_Click;

                    // Highlight nếu trùng với giờ đang chọn trên dtpTime
                    if (slotStart == dtpTime.Value)
                    {
                        btn.BackColor = Color.FromArgb(99, 102, 241);
                        btn.ForeColor = Color.White;
                        _selectedSlot = btn;
                    }
                }
                pnlTimeSlots.Controls.Add(btn);
            }
            pnlTimeSlots.ResumeLayout();
        }

        // ============================================================
        // LOGIC TIMELINE (VẼ & CLICK)
        // ============================================================
        private void PnlTimeline_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int totalMinutes = (END_HOUR - START_HOUR) * 60;
            Rectangle drawRect = new Rectangle(0, 0, pnlTimeline.Width, pnlTimeline.Height);

            // 1. Vẽ Nền
            g.FillRectangle(new SolidBrush(Color.FromArgb(241, 245, 249)), drawRect);

            // 2. Vẽ Booking Đỏ (GIỮ NGUYÊN CODE CŨ)
            foreach (var b in _bookingsOfTable)
            {
                if (!b.ThoiGianBatDau.HasValue || !b.ThoiGianKetThuc.HasValue) continue;
                DateTime startVal = b.ThoiGianBatDau.Value;
                DateTime endVal = b.ThoiGianKetThuc.Value;

                double startH = startVal.Hour < START_HOUR ? startVal.Hour + 24 : startVal.Hour;
                double endH = endVal.Hour < START_HOUR ? endVal.Hour + 24 : endVal.Hour;

                int startMin = (int)((startH - START_HOUR) * 60 + startVal.Minute);
                int duration = (int)((endH * 60 + endVal.Minute) - (startH * 60 + startVal.Minute));

                float x = (startMin / (float)totalMinutes) * drawRect.Width;
                float w = (duration / (float)totalMinutes) * drawRect.Width;

                using (Brush brush = new SolidBrush(Color.FromArgb(254, 202, 202))) { g.FillRectangle(brush, x, 0, w, 40); }
                g.DrawRectangle(Pens.Red, x, 0, w, 40);
            }

            // 3. VẼ VÙNG ĐANG CHỌN
            totalMinutes = (END_HOUR - START_HOUR) * 60;
            double currentH = dtpTime.Value.Hour < START_HOUR ? dtpTime.Value.Hour + 24 : dtpTime.Value.Hour;
            int currentStartMin = (int)((currentH - START_HOUR) * 60 + dtpTime.Value.Minute);
            int currentDuration = (int)(nudHour.Value * 60 + nudMinute.Value);

            float xSelect = (currentStartMin / (float)totalMinutes) * pnlTimeline.Width;
            float wSelect = (currentDuration / (float)totalMinutes) * pnlTimeline.Width;

            if (wSelect > 0)
            {
                using (Brush brush = new SolidBrush(Color.FromArgb(100, 99, 102, 241)))
                {
                    g.FillRectangle(brush, xSelect, 0, wSelect, 40);
                }
                g.DrawRectangle(new Pen(Color.Blue, 2), xSelect, 0, wSelect, 40);

                // --- MỚI: VẼ THANH KÉO (HANDLE) Ở CUỐI ---
                float handleX = xSelect + wSelect;
                // Vẽ một vạch đậm hoặc hình tròn nhỏ ở mép phải
                g.FillRectangle(Brushes.Blue, handleX - 3, 0, 6, 40); // Thanh nắm dọc

                // Vẽ dấu mũi tên nhỏ cho đẹp (tùy chọn)
                // g.DrawString("↔", new Font("Segoe UI", 10), Brushes.White, handleX - 8, 10);
            }

            // 4. Vẽ Thước đo (GIỮ NGUYÊN CODE CŨ)
            int rulerY = 45;
            for (int h = START_HOUR; h <= END_HOUR; h++)
            {
                float x = ((h - START_HOUR) * 60f / totalMinutes) * drawRect.Width;
                g.DrawLine(Pens.Gray, x, 40, x, 50);
                if (h % 2 == 0)
                {
                    string label = (h % 24) + "h";
                    g.DrawString(label, new Font("Segoe UI", 8), Brushes.Gray, x - 10, rulerY + 5);
                }
                // Thêm vạch nhỏ cho mỗi giờ lẻ (Zoom chi tiết hơn)
                else
                {
                    g.DrawLine(Pens.LightGray, x, 40, x, 45);
                }
            }
            if (_hoverTime.HasValue && _hoverX >= 0 && _hoverX <= pnlTimeline.Width)
            {
                // 1. Vẽ đường kẻ dọc (Guide Line) theo chuột
                using (Pen dashPen = new Pen(Color.DimGray, 1))
                {
                    dashPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    g.DrawLine(dashPen, _hoverX, 0, _hoverX, pnlTimeline.Height);
                }

                // 2. Chuẩn bị nội dung text
                string timeText = _hoverTime.Value.ToString("HH:mm");

                // Nếu đang kéo dãn (Resize), hiển thị thêm thời lượng
                if (_isResizing || _isDragging)
                {
                    // Tính thời lượng đang chọn
                    TimeSpan duration = TimeSpan.FromHours((double)nudHour.Value) + TimeSpan.FromMinutes((double)nudMinute.Value);
                    timeText += $"\n({(int)duration.TotalMinutes} phút)";
                }

                // 3. Đo kích thước text để vẽ hộp nền
                Font tipFont = new Font("Segoe UI", 9, FontStyle.Bold);
                SizeF textSize = g.MeasureString(timeText, tipFont);
                int padding = 4;
                int tipW = (int)textSize.Width + (padding * 2);
                int tipH = (int)textSize.Height + (padding * 2);

                // 4. Tính vị trí hộp (Mặc định: Phía trên con trỏ chuột một chút)
                int tipX = _hoverX - (tipW / 2); // Căn giữa theo đường kẻ dọc
                int tipY = 5; // Luôn hiển thị ở mép trên cùng timeline

                // 5. Xử lý chống tràn màn hình (Clamping)
                // Nếu sát mép trái quá -> Đẩy sang phải
                if (tipX < 2) tipX = 2;
                // Nếu sát mép phải quá -> Đẩy sang trái
                if (tipX + tipW > pnlTimeline.Width - 2) tipX = pnlTimeline.Width - tipW - 2;

                // 6. Vẽ hộp nền (Background)
                Rectangle tipRect = new Rectangle(tipX, tipY, tipW, tipH);

                // Đổ màu nền tối
                using (Brush bgBrush = new SolidBrush(Color.FromArgb(220, 40, 40, 40))) // Màu xám đen, hơi trong suốt
                {
                    g.FillRectangle(bgBrush, tipRect);
                }
                g.DrawRectangle(Pens.Black, tipRect); // Viền đen

                // 7. Vẽ chữ
                g.DrawString(timeText, tipFont, Brushes.White, tipX + padding, tipY + padding);
            }


        }

        // 1. NHẤN CHUỘT: Bắt đầu chọn
        private void PnlTimeline_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            // Kiểm tra xem có đang click vào mép phải không
            float endX = GetEndXOfCurrentSelection();

            if (Math.Abs(e.X - endX) <= _resizeThreshold && nudHour.Value + nudMinute.Value > 0)
            {
                // -> ĐANG MUỐN KÉO DÃN
                _isResizing = true;
                _isDragging = true;
            }
            else
            {
                // -> ĐANG MUỐN CHỌN MỚI (Code cũ)
                _isResizing = false;
                _isDragging = true;

                // Reset thời lượng về 0 hoặc mặc định khi bắt đầu kéo mới
                _dragStartPoint = e.Location;
                _tempDragStartTime = PixelToTime(e.X);
                dtpTime.Value = _tempDragStartTime; // Set giờ bắt đầu mới

                // Reset độ dài về 0 để kéo từ đầu
                nudHour.Value = 0;
                nudMinute.Value = 0;
            }
        }

        // 2. DI CHUỘT: Vẽ vùng chọn theo thời gian thực
        private void PnlTimeline_MouseMove(object sender, MouseEventArgs e)
        {
            _hoverX = e.X;
            _hoverTime = PixelToTime(e.X);

            if (e.Button == MouseButtons.None)
            {
                // Tính toán vị trí pixel của giờ kết thúc hiện tại
                float endX = GetEndXOfCurrentSelection();

                // Nếu chuột nằm gần mép phải (trong khoảng 10px) -> Đổi con trỏ thành mũi tên 2 chiều
                if (Math.Abs(e.X - endX) <= _resizeThreshold && nudHour.Value + nudMinute.Value > 0)
                {
                    pnlTimeline.Cursor = Cursors.SizeWE; // Icon mũi tên trái phải <->
                }
                else
                {
                    pnlTimeline.Cursor = Cursors.Cross; // Icon mặc định
                }
                return;
            }

            if (!_isDragging) return;

            if (_isResizing)
            {
                // === LOGIC KÉO DÃN (RESIZE) ===
                // Giờ bắt đầu GIỮ NGUYÊN, chỉ tính lại giờ kết thúc dựa trên con trỏ chuột

                DateTime tStart = dtpTime.Value; // Giờ bắt đầu cố định
                DateTime tEndCurrent = PixelToTime(e.X); // Giờ kết thúc mới theo chuột

                // Tính thời lượng mới
                TimeSpan duration = tEndCurrent - tStart;

                // Không cho kéo ngược về trước giờ bắt đầu (tối thiểu 15 phút)
                if (duration.TotalMinutes < 15) duration = TimeSpan.FromMinutes(15);

                // Cập nhật UI (NumericUpDown)
                _isUpdatingFromCode = true;
                int totalMin = (int)Math.Round(duration.TotalMinutes);
                nudHour.Value = Math.Min(24, totalMin / 60);
                nudMinute.Value = totalMin % 60;
                _isUpdatingFromCode = false;
            }
            else
            {
                _dragEndPoint = e.Location;

                // Giới hạn không cho kéo ra ngoài panel
                if (_dragEndPoint.X < 0) _dragEndPoint.X = 0;
                if (_dragEndPoint.X > pnlTimeline.Width) _dragEndPoint.X = pnlTimeline.Width;
            }

            // Nếu đang không kéo thì thôi

            // Cập nhật điểm cuối
           

            // Vẽ lại panel để hiện cái khung màu xanh đang kéo
            pnlTimeline.Invalidate();
        }

        // 3. THẢ CHUỘT: Chốt thời gian
        private void PnlTimeline_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_isDragging) return;

            // Nếu là tạo mới (không phải resize) thì cần tính toán chốt lại lần cuối
            // (Logic giống code cũ của bạn)
            if (!_isResizing)
            {
                DateTime t1 = PixelToTime(_dragStartPoint.X);
                DateTime t2 = PixelToTime(e.X);

                // Xử lý kéo ngược (từ phải sang trái)
                if (t1 > t2) { DateTime temp = t1; t1 = t2; t2 = temp; }

                // Click nhầm (thời gian quá nhỏ) -> Mặc định 1 tiếng
                if ((t2 - t1).TotalMinutes < 5) t2 = t1.AddHours(1);

                _isUpdatingFromCode = true;
                dtpTime.Value = t1;
                int totalMin = (int)(t2 - t1).TotalMinutes;
                nudHour.Value = Math.Min(24, totalMin / 60);
                nudMinute.Value = totalMin % 60;
                _isUpdatingFromCode = false;
            }

            _isDragging = false;
            _isResizing = false;
            pnlTimeline.Invalidate();
        }

        // HÀM TIỆN ÍCH: Chuyển đổi Pixel sang DateTime (Chính xác theo phút)
        private DateTime PixelToTime(int x)
        {
            int totalMinutesOnBar = (END_HOUR - START_HOUR) * 60;

            // Tính tỉ lệ phần trăm
            float percent = (float)x / pnlTimeline.Width;

            // Số phút từ mốc 8h sáng
            int minutesFromBase = (int)(percent * totalMinutesOnBar);

            // --- ZOOM THEO PHÚT (Không làm tròn 15p nữa) ---
            // Nếu bạn muốn làm tròn 5 phút cho dễ chọn thì dùng dòng này:
            // minutesFromBase = (int)(Math.Round(minutesFromBase / 5.0) * 5); 

            int totalMinutesReal = (START_HOUR * 60) + minutesFromBase;
            int hour = (totalMinutesReal / 60) % 24;
            int minute = totalMinutesReal % 60;

            DateTime baseDate = dtpDate.Value.Date;

            // Xử lý qua đêm (0h, 1h, 2h sáng hôm sau)
            if (totalMinutesReal >= 24 * 60) baseDate = baseDate.AddDays(1);

            return new DateTime(baseDate.Year, baseDate.Month, baseDate.Day, hour, minute, 0);
        }
        // Hàm tính toạ độ X của thời điểm kết thúc hiện tại
        private float GetEndXOfCurrentSelection()
        {
            int totalMinutesBar = (END_HOUR - START_HOUR) * 60;

            // Tính thời gian kết thúc hiện tại trên UI
            DateTime endTime = dtpTime.Value.AddHours((double)nudHour.Value).AddMinutes((double)nudMinute.Value);

            // Đổi sang phút so với mốc 8h
            double endH = endTime.Hour < START_HOUR ? endTime.Hour + 24 : endTime.Hour;
            int minutesFromStart = (int)((endH - START_HOUR) * 60 + endTime.Minute);

            // Đổi sang Pixel
            return (minutesFromStart / (float)totalMinutesBar) * pnlTimeline.Width;
        }


        // ============================================================
        // EVENT HANDLER
        // ============================================================
        private void Slot_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (_selectedSlot != null)
            {
                _selectedSlot.BackColor = Color.White;
                _selectedSlot.ForeColor = Color.Black;
            }
            _selectedSlot = btn;
            _selectedSlot.BackColor = Color.FromArgb(99, 102, 241);
            _selectedSlot.ForeColor = Color.White;

            if (btn.Tag is DateTime slotTime)
            {
                _isUpdatingFromCode = true;
                dtpTime.Value = slotTime; // Cái này sẽ trigger Timeline repaint vùng xanh
                _isUpdatingFromCode = false;
                pnlTimeline.Invalidate();
            }
        }

        private async void BtnConfirm_Click(object sender, EventArgs e)
        {
            DateTime startTime = dtpTime.Value;

            // Xử lý logic qua đêm cho input (phòng hờ)
            if (startTime.Hour < 8 && startTime.Date == dtpDate.Value.Date)
                startTime = startTime.AddDays(1);

            if (startTime < DateTime.Now)
            {
                MessageBox.Show("Không thể đặt lùi thời gian!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int hours = (int)nudHour.Value;
            int minutes = (int)nudMinute.Value;
            if (hours == 0 && minutes == 0)
            {
                MessageBox.Show("Vui lòng nhập thời gian chơi!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime endTime = startTime.AddHours(hours).AddMinutes(minutes);

            bool isBusy = await _datBanService.IsTableReservedAsync(_maBan, startTime, endTime);

            if (isBusy)
            {
                MessageBox.Show($"Bị trùng lịch từ {startTime:HH:mm} đến {endTime:HH:mm}.\nVui lòng chọn giờ khác.", "Trùng lịch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                await LoadAllDataAsync(); // Load lại để hiện màu đỏ mới nhất
                return;
            }

            var booking = new DatBan
            {
                MaBan = _maBan,
                MaKh = UserSession.MaKH > 0 ? UserSession.MaKH : (int?)null,
                TenKhach = UserSession.TenKH,
                Sdt = UserSession.Sdt,
                ThoiGianDat = DateTime.Now,
                ThoiGianBatDau = startTime,
                ThoiGianKetThuc = endTime,
                TrangThai = "Đang chờ",
                GhiChu = txtGhiChu.Text.Trim()
            };

            try
            {
                await _datBanService.AddAsync(booking);
                MessageBox.Show("Đặt bàn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void CreateLegendItem(int x, int y, Color color, string text, bool hasBorder)
        {
            var pnlColor = new Panel { Location = new Point(x, y + 3), Size = new Size(20, 20), BackColor = color, BorderStyle = hasBorder ? BorderStyle.FixedSingle : BorderStyle.None };
            var lblText = new Label { Text = text, Location = new Point(x + 25, y + 3), AutoSize = true, Font = new Font("Segoe UI", 9), ForeColor = Color.Gray };
            this.Controls.Add(pnlColor);
            this.Controls.Add(lblText);
        }


    }
}