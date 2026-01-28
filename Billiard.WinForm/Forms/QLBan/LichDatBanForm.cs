using Billiard.BLL.Services.QLBan;
using Billiard.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.QLBan
{
    public partial class LichDatBanForm : Form
    {
        private readonly DatBanService _datBanService;
        private readonly BanBiaService _banBiaService;
        private readonly MainForm _mainForm;
        private DateTime _currentWeekStart;
        private const int WarningMinutes = 30;
        private const int HourHeight = 70; // Tăng chiều cao mỗi giờ
        private const int DayWidth = 240; // Tăng chiều rộng mỗi ngày

        public LichDatBanForm(DatBanService datBanService, BanBiaService banBiaService, MainForm mainForm)
        {
            _datBanService = datBanService;
            _banBiaService = banBiaService;
            _mainForm = mainForm;
            InitializeComponent();

            _currentWeekStart = GetMonday(DateTime.Today);
        }

        private async void LichDatBanForm_Load(object sender, EventArgs e)
        {
            await LoadWeekViewAsync();
        }

        private DateTime GetMonday(DateTime date)
        {
            // Tính số ngày từ ngày hiện tại đến Thứ 2 gần nhất
            int daysFromMonday = ((int)date.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;

            // Nếu hôm nay là Chủ nhật (DayOfWeek.Sunday = 0)
            if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                daysFromMonday = 6; // Lùi 6 ngày về Thứ 2
            }

            return date.AddDays(-daysFromMonday).Date;
        }

        private async void btnPrevWeek_Click(object sender, EventArgs e)
        {
            _currentWeekStart = _currentWeekStart.AddDays(-7);
            await LoadWeekViewAsync();
        }

        private async void btnNextWeek_Click(object sender, EventArgs e)
        {
            _currentWeekStart = _currentWeekStart.AddDays(7);
            await LoadWeekViewAsync();
        }

        private async void btnToday_Click(object sender, EventArgs e)
        {
            // Chuyển về tuần hiện tại (tuần có hôm nay)
            _currentWeekStart = GetMonday(DateTime.Today);
            await LoadWeekViewAsync();
        }

        private async void btnSelectDate_Click(object sender, EventArgs e)
        {
            // Tạo form chọn ngày
            using (Form datePickerForm = new Form())
            {
                datePickerForm.Text = "Chọn ngày";
                datePickerForm.Width = 350;
                datePickerForm.Height = 350;
                datePickerForm.StartPosition = FormStartPosition.CenterParent;
                datePickerForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                datePickerForm.MaximizeBox = false;
                datePickerForm.MinimizeBox = false;

                // Tạo MonthCalendar
                MonthCalendar calendar = new MonthCalendar
                {
                    Location = new Point(20, 20),
                    MaxSelectionCount = 1
                };

                // Nút OK
                Button btnOk = new Button
                {
                    Text = "Xem tuần này",
                    DialogResult = DialogResult.OK,
                    Location = new Point(20, 220),
                    Width = 140,
                    Height = 40,
                    BackColor = Color.FromArgb(99, 102, 241),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold)
                };

                // Nút Cancel
                Button btnCancel = new Button
                {
                    Text = "Hủy",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(170, 220),
                    Width = 100,
                    Height = 40,
                    BackColor = Color.FromArgb(156, 163, 175),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold)
                };

                datePickerForm.Controls.Add(calendar);
                datePickerForm.Controls.Add(btnOk);
                datePickerForm.Controls.Add(btnCancel);
                datePickerForm.AcceptButton = btnOk;
                datePickerForm.CancelButton = btnCancel;

                if (datePickerForm.ShowDialog() == DialogResult.OK)
                {
                    // Lấy ngày được chọn và chuyển đến tuần của ngày đó
                    DateTime selectedDate = calendar.SelectionStart;
                    _currentWeekStart = GetMonday(selectedDate);
                    await LoadWeekViewAsync();
                }
            }
        }

        private async Task LoadWeekViewAsync()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                pnlCalendar.Controls.Clear();

                DateTime weekEnd = _currentWeekStart.AddDays(6);
                lblWeekRange.Text = $"{GetDayName(_currentWeekStart.DayOfWeek)}, {_currentWeekStart:dd/MM/yyyy} - {GetDayName(weekEnd.DayOfWeek)}, {weekEnd:dd/MM/yyyy}";

                var allDatBans = await _datBanService.GetAllActiveAsync();
                var weekDatBans = allDatBans.Where(d =>
                    d.ThoiGianBatDau >= _currentWeekStart &&
                    d.ThoiGianBatDau < _currentWeekStart.AddDays(7)
                ).ToList();

                CreateCalendarGrid(weekDatBans);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải lịch: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void CreateCalendarGrid(List<DatBan> datBans)
        {
            TableLayoutPanel mainGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 8,
                RowCount = 25,
                AutoScroll = true,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                BackColor = Color.LightGray
            };

            mainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            for (int i = 0; i < 7; i++)
            {
                mainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, DayWidth));
            }

            mainGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
            for (int i = 0; i < 24; i++)
            {
                mainGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, HourHeight));
            }

            mainGrid.Controls.Add(CreateHeaderLabel("Giờ"), 0, 0);

            for (int day = 0; day < 7; day++)
            {
                DateTime currentDay = _currentWeekStart.AddDays(day);
                string dayText = $"{GetDayName(currentDay.DayOfWeek)}\n{currentDay:dd/MM}";

                Label dayLabel = CreateHeaderLabel(dayText);
                if (currentDay.Date == DateTime.Today)
                {
                    dayLabel.BackColor = Color.FromArgb(219, 234, 254);
                    dayLabel.ForeColor = Color.FromArgb(29, 78, 216);
                    dayLabel.Font = new Font(dayLabel.Font, FontStyle.Bold);
                }
                mainGrid.Controls.Add(dayLabel, day + 1, 0);
            }

            for (int hour = 0; hour < 24; hour++)
            {
                Label hourLabel = new Label
                {
                    Text = $"{hour:D2}:00",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.FromArgb(241, 245, 249),
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(71, 85, 105)
                };
                mainGrid.Controls.Add(hourLabel, 0, hour + 1);

                for (int day = 0; day < 7; day++)
                {
                    DateTime currentDay = _currentWeekStart.AddDays(day);
                    DateTime slotStart = currentDay.AddHours(hour);
                    DateTime slotEnd = slotStart.AddHours(1);

                    // Kiểm tra nếu là ngày hiện tại
                    bool isToday = currentDay.Date == DateTime.Today;

                    Panel dayCell = new Panel
                    {
                        Dock = DockStyle.Fill,
                        BackColor = isToday ? Color.FromArgb(239, 246, 255) : Color.White, // Highlight nền xanh nhạt cho hôm nay
                        Padding = new Padding(2),
                        AutoScroll = true
                    };

                    // Tìm các đặt bàn trong khoảng thời gian này
                    var bookingsInSlot = datBans.Where(d =>
                        d.ThoiGianBatDau < slotEnd &&
                        d.ThoiGianKetThuc > slotStart
                    ).ToList();

                    if (bookingsInSlot.Any())
                    {
                        // Kiểm tra nếu có nhiều hơn 1 bàn
                        if (bookingsInSlot.Count > 1)
                        {
                            // Hiển thị tóm tắt số lượng bàn
                            Panel summaryPanel = CreateMultipleBookingSummary(bookingsInSlot, slotStart);
                            dayCell.Controls.Add(summaryPanel);
                        }
                        else
                        {
                            // Chỉ có 1 bàn - hiển thị chi tiết
                            Panel bookingPanel = CreateBookingPanel(bookingsInSlot[0]);
                            bookingPanel.Location = new Point(2, 2);
                            bookingPanel.Width = dayCell.Width - 10;
                            dayCell.Controls.Add(bookingPanel);
                        }
                    }

                    mainGrid.Controls.Add(dayCell, day + 1, hour + 1);
                }
            }

            pnlCalendar.Controls.Add(mainGrid);
        }

        private Panel CreateMultipleBookingSummary(List<DatBan> bookings, DateTime slotTime)
        {
            Panel summaryPanel = new Panel
            {
                Height = 65,
                Width = DayWidth - 10,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(243, 244, 246),
                Cursor = Cursors.Hand,
                Padding = new Padding(8)
            };

            Label lblCount = new Label
            {
                Text = $"📋 {bookings.Count} bàn đặt",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(8, 8),
                ForeColor = Color.FromArgb(31, 41, 55)
            };

            Label lblHint = new Label
            {
                Text = "Click để xem chi tiết...",
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                AutoSize = true,
                Location = new Point(8, 33),
                ForeColor = Color.FromArgb(107, 114, 128)
            };

            summaryPanel.Controls.Add(lblCount);
            summaryPanel.Controls.Add(lblHint);

            // Click để mở danh sách
            summaryPanel.Click += (s, e) => ShowBookingList(bookings, slotTime);
            lblCount.Click += (s, e) => ShowBookingList(bookings, slotTime);
            lblHint.Click += (s, e) => ShowBookingList(bookings, slotTime);

            return summaryPanel;
        }

        private void ShowBookingList(List<DatBan> bookings, DateTime slotTime)
        {
            // Tạo form con để hiển thị danh sách
            Form listForm = new Form
            {
                Text = $"Danh sách đặt bàn - {slotTime:HH:mm dd/MM/yyyy}",
                Width = 600,
                Height = 500,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false,
                FormBorderStyle = FormBorderStyle.FixedDialog
            };

            Panel headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(99, 102, 241),
                Padding = new Padding(15)
            };

            Label headerLabel = new Label
            {
                Text = $"📋 {bookings.Count} bàn đặt trong khung giờ này",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(15, 15)
            };
            headerPanel.Controls.Add(headerLabel);
            listForm.Controls.Add(headerPanel);

            // Panel chứa danh sách
            FlowLayoutPanel flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(10),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };

            // Thêm từng bàn vào danh sách
            foreach (var booking in bookings.OrderBy(b => b.ThoiGianBatDau))
            {
                Panel bookingCard = CreateBookingCard(booking);
                bookingCard.Width = flowPanel.Width - 40;
                flowPanel.Controls.Add(bookingCard);
            }

            listForm.Controls.Add(flowPanel);
            listForm.ShowDialog();
        }

        private Panel CreateBookingCard(DatBan booking)
        {
            Panel card = new Panel
            {
                Height = 140,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(5),
                Padding = new Padding(10),
                Cursor = Cursors.Hand
            };

            // Xác định màu dựa trên trạng thái và thời gian
            Color bgColor = GetBookingColor(booking);
            card.BackColor = bgColor;

            // Thông tin bàn
            Label lblTable = new Label
            {
                Text = $"🎱 Bàn: {booking.MaBanNavigation?.TenBan ?? "N/A"}",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 10),
                ForeColor = Color.FromArgb(17, 24, 39)
            };

            Label lblCustomer = new Label
            {
                Text = $"👤 Khách: {booking.TenKhach} - {booking.Sdt}",
                Font = new Font("Segoe UI", 9F),
                AutoSize = true,
                Location = new Point(10, 35),
                ForeColor = Color.FromArgb(55, 65, 81)
            };

            Label lblTime = new Label
            {
                Text = $"⏰ {booking.ThoiGianBatDau:HH:mm} - {booking.ThoiGianKetThuc:HH:mm}",
                Font = new Font("Segoe UI", 9F),
                AutoSize = true,
                Location = new Point(10, 60),
                ForeColor = Color.FromArgb(55, 65, 81)
            };

            Label lblArea = new Label
            {
                Text = $"📍 {booking.MaBanNavigation?.MaKhuVucNavigation?.TenKhuVuc ?? "N/A"} - {booking.MaBanNavigation?.MaLoaiNavigation?.TenLoai ?? "N/A"}",
                Font = new Font("Segoe UI", 8.5F),
                AutoSize = true,
                Location = new Point(10, 85),
                ForeColor = Color.FromArgb(75, 85, 99)
            };

            Label lblStatus = new Label
            {
                Text = $"📊 {booking.TrangThai}",
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                AutoSize = true,
                Location = new Point(10, 110),
                ForeColor = Color.FromArgb(107, 114, 128)
            };

            card.Controls.Add(lblTable);
            card.Controls.Add(lblCustomer);
            card.Controls.Add(lblTime);
            card.Controls.Add(lblArea);
            card.Controls.Add(lblStatus);

            // Click để xem chi tiết
            card.Click += (s, e) => ShowBookingDetails(booking);
            foreach (Control ctrl in card.Controls)
            {
                ctrl.Click += (s, e) => ShowBookingDetails(booking);
            }

            return card;
        }

        private Label CreateHeaderLabel(string text)
        {
            return new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(99, 102, 241),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Padding = new Padding(8)
            };
        }

        private Panel CreateBookingPanel(DatBan booking)
        {
            Panel panel = new Panel
            {
                Height = 100,
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand,
                Padding = new Padding(6)
            };

            // Xác định màu dựa trên trạng thái và thời gian
            Color bgColor = GetBookingColor(booking);
            panel.BackColor = bgColor;

            // Tạo nội dung
            Label lblTime = new Label
            {
                Text = $"⏰ {booking.ThoiGianBatDau:HH:mm} - {booking.ThoiGianKetThuc:HH:mm}",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(6, 6),
                ForeColor = Color.FromArgb(51, 65, 85)
            };

            Label lblCustomer = new Label
            {
                Text = $"👤 {booking.TenKhach}",
                Font = new Font("Segoe UI", 8.5F),
                AutoSize = true,
                Location = new Point(6, 28),
                ForeColor = Color.FromArgb(71, 85, 105)
            };

            Label lblTable = new Label
            {
                Text = $"🎱 {booking.MaBanNavigation?.TenBan ?? "N/A"}",
                Font = new Font("Segoe UI", 8.5F),
                AutoSize = true,
                Location = new Point(6, 50),
                ForeColor = Color.FromArgb(71, 85, 105)
            };

            Label lblStatus = new Label
            {
                Text = $"📊 {booking.TrangThai}",
                Font = new Font("Segoe UI", 7.5F, FontStyle.Italic),
                AutoSize = true,
                Location = new Point(6, 72),
                ForeColor = Color.FromArgb(100, 116, 139)
            };

            panel.Controls.Add(lblTime);
            panel.Controls.Add(lblCustomer);
            panel.Controls.Add(lblTable);
            panel.Controls.Add(lblStatus);

            // Thêm sự kiện click
            panel.Click += (s, e) => ShowBookingDetails(booking);
            foreach (Control ctrl in panel.Controls)
            {
                ctrl.Click += (s, e) => ShowBookingDetails(booking);
            }

            return panel;
        }

        private Color GetBookingColor(DatBan booking)
        {
            Color bgColor = Color.White;

            // Kiểm tra nếu booking đã qua (quá khứ)
            if (booking.ThoiGianKetThuc < DateTime.Now)
            {
                // Màu xám cho quá khứ
                bgColor = Color.FromArgb(229, 231, 235);
                return bgColor;
            }

            // Booking hiện tại hoặc tương lai
            if (booking.TrangThai == "Đang chờ")
            {
                TimeSpan? timeUntilStart = booking.ThoiGianBatDau - DateTime.Now;
                double totalMinutes = timeUntilStart?.TotalMinutes ?? double.MaxValue;

                if (totalMinutes <= WarningMinutes && totalMinutes > 0)
                {
                    // Sắp đến giờ (< 30 phút): Màu vàng
                    bgColor = Color.FromArgb(254, 249, 195);
                }
                else if (totalMinutes <= 0)
                {
                    // Quá giờ đặt: Màu hồng
                    bgColor = Color.FromArgb(254, 202, 202);
                }
                else
                {
                    // Bình thường: Màu trắng
                    bgColor = Color.White;
                }
            }
            else if (booking.TrangThai == "Đã đặt")
            {
                // Đã xác nhận: Màu xanh lá
                bgColor = Color.FromArgb(187, 247, 208);
            }

            return bgColor;
        }

        private async void ShowBookingDetails(DatBan booking)
        {
            // Kiểm tra nếu là booking quá khứ
            bool isPast = booking.ThoiGianKetThuc < DateTime.Now;

            string message = $"Chi tiết đặt bàn:\n\n" +
                           $"Mã đặt: {booking.MaDat}\n" +
                           $"Khách hàng: {booking.TenKhach}\n" +
                           $"SĐT: {booking.Sdt}\n" +
                           $"Bàn: {booking.MaBanNavigation?.TenBan ?? "N/A"}\n" +
                           $"Loại: {booking.MaBanNavigation?.MaLoaiNavigation?.TenLoai ?? "N/A"}\n" +
                           $"Khu vực: {booking.MaBanNavigation?.MaKhuVucNavigation?.TenKhuVuc ?? "N/A"}\n" +
                           $"Thời gian: {booking.ThoiGianBatDau:dd/MM/yyyy HH:mm} - {booking.ThoiGianKetThuc:HH:mm}\n" +
                           $"Trạng thái: {booking.TrangThai}\n" +
                           $"Ghi chú: {booking.GhiChu ?? "(Không có)"}";

            if (isPast)
            {
                message += "\n\n⚠️ Đặt bàn này đã kết thúc (quá khứ)";
                MessageBox.Show(message, "Chi tiết đặt bàn",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show(
                message + "\n\n[Yes] = Xác nhận/Bắt đầu chơi\n[No] = Hủy đặt\n[Cancel] = Đóng",
                "Chi tiết đặt bàn",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                await HandleConfirmAction(booking);
            }
            else if (result == DialogResult.No)
            {
                await HandleCancelAction(booking);
            }
        }

        private async Task HandleConfirmAction(DatBan booking)
        {
            if (booking.TrangThai == "Đang chờ")
            {
                var confirm = MessageBox.Show(
                    $"Xác nhận đặt bàn {booking.MaBanNavigation?.TenBan}?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    var success = await _datBanService.UpdateStatusAsync(booking.MaDat, "Đã đặt");
                    if (success && booking.MaBan.HasValue)
                    {
                        var ban = await _banBiaService.GetTableByIdAsync(booking.MaBan.Value);
                        if (ban != null)
                        {
                            ban.TrangThai = "Đã đặt";
                            await _banBiaService.UpdateTableAsync(ban);
                        }
                    }

                    MessageBox.Show("Đã xác nhận đặt bàn!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadWeekViewAsync();
                }
            }
            else if (booking.TrangThai == "Đã đặt")
            {
                var confirm = MessageBox.Show(
                    $"Bắt đầu chơi bàn {booking.MaBanNavigation?.TenBan}?",
                    "Bắt đầu chơi",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    int maNv = _mainForm?.MaNV ?? 1;
                    var success = await _banBiaService.ConfirmReservationAsync(booking.MaDat, maNv);

                    if (success)
                    {
                        MessageBox.Show("Đã bắt đầu chơi!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await LoadWeekViewAsync();
                    }
                    else
                    {
                        MessageBox.Show("Không thể bắt đầu chơi!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async Task HandleCancelAction(DatBan booking)
        {
            var confirm = MessageBox.Show(
                $"Hủy đặt bàn {booking.MaBanNavigation?.TenBan}?",
                "Xác nhận hủy",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                var success = await _banBiaService.CancelReservationAsync(booking.MaDat);
                if (success)
                {
                    await _datBanService.UpdateStatusAsync(booking.MaDat, "Đã hủy");
                    MessageBox.Show("Đã hủy đặt bàn!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadWeekViewAsync();
                }
                else
                {
                    MessageBox.Show("Không thể hủy đặt bàn!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string GetDayName(DayOfWeek dayOfWeek)
        {
            return dayOfWeek switch
            {
                DayOfWeek.Monday => "Thứ 2",
                DayOfWeek.Tuesday => "Thứ 3",
                DayOfWeek.Wednesday => "Thứ 4",
                DayOfWeek.Thursday => "Thứ 5",
                DayOfWeek.Friday => "Thứ 6",
                DayOfWeek.Saturday => "Thứ 7",
                DayOfWeek.Sunday => "Chủ nhật",
                _ => ""
            };
        }
    }
}