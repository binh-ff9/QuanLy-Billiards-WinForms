using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Billiard.BLL.Services.NhanVienService;
using Billiard.DAL.Entities;
using Billiard.DAL.Data;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using System.IO;

namespace Billiard.WinForm.Forms.NhanVien
{
    public partial class ScheduleForm : Form
    {
        #region Fields
        private readonly NhanVienService _nhanVienService;
        private List<DAL.Entities.NhanVien> _allEmployees;
        private DateTime _currentWeekStart;
        private List<LichLamViec> _scheduleData;
        private int _currentUserId;
        private string _currentUserRole;
        private Panel pnlCalendar;
        private Label lblWeekDisplay;

        private readonly int[] _timeSlots = { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22 };

        private DateTime? _selectedDate = null;
        private int? _selectedStartHour = null;
        private int? _selectedEndHour = null;
        private Panel _selectedStartCell = null;
        private Panel _selectedEndCell = null;
        #endregion

        #region Constructor
        public ScheduleForm(NhanVienService nhanVienService)
        {
            _nhanVienService = nhanVienService;
            _scheduleData = new List<LichLamViec>();
            _currentWeekStart = GetWeekStart(DateTime.Now);

            InitializeComponent();
            InitializeCustomControls();
            LoadEmployees();
            LoadScheduleFromDatabase();
        }

        public void SetUserInfo(int userId, string userRole)
        {
            _currentUserId = userId;
            _currentUserRole = userRole;
        }
        #endregion

        #region Initialize
        private void InitializeCustomControls()
        {
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            var lblTitle = new Label
            {
                Text = "📅 Lịch làm việc tuần",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 26, 46),
                AutoSize = true,
                Location = new Point(20, 15)
            };

            var lblInstruction = new Label
            {
                Text = "💡 Hướng dẫn: Click lần 1 chọn giờ bắt đầu, click lần 2 chọn giờ kết thúc",
                Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                ForeColor = Color.FromArgb(107, 114, 128),
                AutoSize = true,
                Location = new Point(20, 50)
            };

            var pnlWeekNav = new Panel
            {
                Location = new Point(400, 75),
                Size = new Size(400, 40),
                BackColor = Color.Transparent
            };

            var btnPrevWeek = new Button
            {
                Text = "◀",
                Size = new Size(40, 40),
                Location = new Point(0, 0),
                Font = new Font("Segoe UI", 12F),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(102, 126, 234),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnPrevWeek.FlatAppearance.BorderSize = 0;
            btnPrevWeek.Click += (s, e) => NavigateWeek(-1);

            lblWeekDisplay = new Label
            {
                Size = new Size(300, 40),
                Location = new Point(50, 0),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 26, 46),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(248, 249, 250)
            };

            var btnNextWeek = new Button
            {
                Text = "▶",
                Size = new Size(40, 40),
                Location = new Point(360, 0),
                Font = new Font("Segoe UI", 12F),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(102, 126, 234),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnNextWeek.FlatAppearance.BorderSize = 0;
            btnNextWeek.Click += (s, e) => NavigateWeek(1);

            pnlWeekNav.Controls.AddRange(new Control[] { btnPrevWeek, lblWeekDisplay, btnNextWeek });

            var btnToday = new Button
            {
                Text = "Hôm nay",
                Size = new Size(100, 40),
                Location = new Point(820, 75),
                Font = new Font("Segoe UI", 10F),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnToday.FlatAppearance.BorderSize = 0;
            btnToday.Click += (s, e) => GoToToday();

            var btnExport = new Button
            {
                Text = "📥 Xuất Excel",
                Size = new Size(120, 40),
                Location = new Point(930, 75),
                Font = new Font("Segoe UI", 10F),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(23, 162, 184),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.Click += BtnExport_Click;

            pnlHeader.Controls.AddRange(new Control[] { lblTitle, lblInstruction, pnlWeekNav, btnToday, btnExport });

            pnlCalendar = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(248, 249, 250),
                Padding = new Padding(20)
            };

            this.Controls.Add(pnlCalendar);
            this.Controls.Add(pnlHeader);
        }
        #endregion

        #region Load Data
        private void LoadEmployees()
        {
            try
            {
                _allEmployees = _nhanVienService.GetAllEmployees()
                    .Where(e => e.TrangThai == "DangLam")
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách nhân viên: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _allEmployees = new List<DAL.Entities.NhanVien>();
            }
        }

        private void LoadScheduleFromDatabase()
        {
            try
            {
                using (var context = new BilliardDbContext())
                {
                    var weekEnd = _currentWeekStart.AddDays(7);
                    _scheduleData = context.Set<LichLamViec>()
                        .Include(l => l.NhanVien)
                        .Where(l => l.Ngay >= DateOnly.FromDateTime(_currentWeekStart)
                                 && l.Ngay < DateOnly.FromDateTime(weekEnd))
                        .ToList();
                }

                // Debug: Log số lượng schedule đã load
                System.Diagnostics.Debug.WriteLine($"Loaded {_scheduleData.Count} schedules for week {_currentWeekStart:dd/MM}");
                foreach (var s in _scheduleData)
                {
                    System.Diagnostics.Debug.WriteLine($"  - {s.NhanVien?.TenNv}: {s.Ngay} {s.GioBatDau}-{s.GioKetThuc}");
                }

                RenderCalendar();
                UpdateWeekDisplay();
            }
            catch (Exception ex)
            {
                _scheduleData = new List<LichLamViec>();
                RenderCalendar();
                UpdateWeekDisplay();
                System.Diagnostics.Debug.WriteLine($"LoadSchedule error: {ex.Message}");
            }
        }
        #endregion

        #region Render Calendar
        private void RenderCalendar()
        {
            pnlCalendar.Controls.Clear();
            ResetSelection();

            var calendarTable = new TableLayoutPanel
            {
                ColumnCount = 8,
                RowCount = _timeSlots.Length + 1,
                AutoSize = false,
                Location = new Point(0, 0),
                Width = pnlCalendar.Width - 40,
                BackColor = Color.White,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
            };

            calendarTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));
            for (int i = 0; i < 7; i++)
                calendarTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.28F));

            calendarTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
            foreach (var hour in _timeSlots)
                calendarTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));

            calendarTable.Height = 60 + (_timeSlots.Length * 50) + _timeSlots.Length + 2;

            calendarTable.Controls.Add(CreateTimeHeaderCell(), 0, 0);

            string[] dayNames = { "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7", "CN" };
            for (int day = 0; day < 7; day++)
            {
                var date = _currentWeekStart.AddDays(day);
                var dayHeader = CreateDayHeaderCell(dayNames[day], date);
                calendarTable.Controls.Add(dayHeader, day + 1, 0);
            }

            for (int i = 0; i < _timeSlots.Length; i++)
            {
                int hour = _timeSlots[i];
                calendarTable.Controls.Add(CreateTimeSlotLabel(hour), 0, i + 1);

                for (int day = 0; day < 7; day++)
                {
                    var date = _currentWeekStart.AddDays(day);
                    var cell = CreateTimeSlotCell(date, hour);
                    calendarTable.Controls.Add(cell, day + 1, i + 1);
                }
            }

            pnlCalendar.Controls.Add(calendarTable);
            AddLegend(calendarTable.Height + 20);
        }

        private Panel CreateTimeHeaderCell()
        {
            var panel = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(102, 126, 234), Margin = new Padding(0) };
            var label = new Label { Text = "⏰ Giờ", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.White };
            panel.Controls.Add(label);
            return panel;
        }

        private Panel CreateDayHeaderCell(string dayName, DateTime date)
        {
            var isToday = date.Date == DateTime.Today;
            var panel = new Panel { Dock = DockStyle.Fill, BackColor = isToday ? Color.FromArgb(255, 193, 7) : Color.FromArgb(102, 126, 234), Margin = new Padding(0) };
            var lblDayName = new Label { Text = dayName, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(10, 10) };
            var lblDate = new Label { Text = date.ToString("dd/MM"), Font = new Font("Segoe UI", 9F), ForeColor = Color.White, AutoSize = true, Location = new Point(10, 35) };
            panel.Controls.AddRange(new Control[] { lblDayName, lblDate });
            return panel;
        }

        private Panel CreateTimeSlotLabel(int hour)
        {
            var panel = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(248, 249, 250), Margin = new Padding(0) };
            var label = new Label { Text = $"{hour:00}:00", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(75, 85, 99) };
            panel.Controls.Add(label);
            return panel;
        }

        private Panel CreateTimeSlotCell(DateTime date, int hour)
        {
            var dateOnly = DateOnly.FromDateTime(date);
            var isPast = date.Date < DateTime.Today || (date.Date == DateTime.Today && hour < DateTime.Now.Hour);
            var isToday = date.Date == DateTime.Today;
            var isCurrentHour = isToday && hour == DateTime.Now.Hour;

            var panel = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0), Cursor = isPast ? Cursors.Default : Cursors.Hand, Tag = new TimeSlotInfo { Date = date, Hour = hour } };
            panel.BackColor = GetCellBackgroundColor(isPast, isCurrentHour, isToday);

            // [FIX] Sửa logic kiểm tra nhân viên trong khung giờ
            // Kiểm tra xem hour có nằm trong khoảng [GioBatDau, GioKetThuc) không
            var employeesAtHour = _scheduleData
                .Where(s => s.Ngay == dateOnly &&
                           hour >= s.GioBatDau.Hour &&
                           hour < s.GioKetThuc.Hour)
                .ToList();

            // Debug log
            if (employeesAtHour.Any())
            {
                System.Diagnostics.Debug.WriteLine($"Cell {date:dd/MM} {hour}:00 has {employeesAtHour.Count} employees");
            }

            if (employeesAtHour.Any())
            {
                var tooltip = new ToolTip();
                var names = string.Join("\n", employeesAtHour.Select(e => $"👤 {e.NhanVien?.TenNv ?? "N/A"} ({e.GioBatDau:HH\\:mm}-{e.GioKetThuc:HH\\:mm})"));
                tooltip.SetToolTip(panel, names);

                var lblCount = new Label
                {
                    Text = employeesAtHour.Count.ToString(),
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(102, 126, 234),
                    AutoSize = true,
                    Location = new Point(5, 5),
                    BackColor = Color.Transparent
                };
                panel.Controls.Add(lblCount);

                var lblIcon = new Label
                {
                    Text = "👥",
                    Font = new Font("Segoe UI", 10F),
                    AutoSize = true,
                    Location = new Point(20, 3),
                    BackColor = Color.Transparent
                };
                panel.Controls.Add(lblIcon);

                // Hiển thị màu ca đầu tiên
                var shift = employeesAtHour.First().Ca;
                var shiftIndicator = new Panel
                {
                    Width = 4,
                    Dock = DockStyle.Left,
                    BackColor = GetShiftColor(shift)
                };
                panel.Controls.Add(shiftIndicator);

                // Đổi màu nền cell để dễ nhìn hơn
                panel.BackColor = Color.FromArgb(240, 249, 255);
            }

            panel.Click += (s, e) => HandleCellClick(panel, date, hour, isPast);

            if (!isPast)
            {
                panel.MouseEnter += (s, e) =>
                {
                    if (panel != _selectedStartCell && panel != _selectedEndCell)
                        panel.BackColor = employeesAtHour.Any() ? Color.FromArgb(220, 239, 255) : Color.FromArgb(239, 246, 255);
                };
                panel.MouseLeave += (s, e) =>
                {
                    if (panel != _selectedStartCell && panel != _selectedEndCell)
                        panel.BackColor = employeesAtHour.Any() ? Color.FromArgb(240, 249, 255) : GetCellBackgroundColor(isPast, isCurrentHour, isToday);
                };
            }
            return panel;
        }

        private Color GetCellBackgroundColor(bool isPast, bool isCurrentHour, bool isToday)
        {
            if (isPast) return Color.FromArgb(245, 245, 245);
            if (isCurrentHour) return Color.FromArgb(254, 243, 199);
            if (isToday) return Color.FromArgb(255, 251, 235);
            return Color.White;
        }

        private Color GetShiftColor(string shift) => shift == "Sang" ? Color.FromArgb(251, 191, 36) : Color.FromArgb(139, 92, 246);

        private void AddLegend(int yPosition)
        {
            var pnlLegend = new Panel { Location = new Point(0, yPosition), Size = new Size(pnlCalendar.Width - 40, 80), BackColor = Color.White, Padding = new Padding(15) };
            var lblLegend = new Label { Text = "📌 Chú thích:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), AutoSize = true, Location = new Point(15, 15) };

            var shiftLegends = new[] { (Color.FromArgb(251, 191, 36), "Ca Sáng"), (Color.FromArgb(139, 92, 246), "Ca Tối"), (Color.FromArgb(134, 239, 172), "Đang chọn"), (Color.FromArgb(240, 249, 255), "Có NV") };
            int xPos = 130;
            foreach (var (color, text) in shiftLegends)
            {
                var box = new Panel { Size = new Size(25, 25), Location = new Point(xPos, 13), BackColor = color };
                var lbl = new Label { Text = text, Font = new Font("Segoe UI", 9F), AutoSize = true, Location = new Point(xPos + 32, 15) };
                pnlLegend.Controls.AddRange(new Control[] { box, lbl });
                xPos += 120;
            }

            var lblInfo = new Label { Text = "👥 = Số nhân viên làm việc | Hover để xem chi tiết", Font = new Font("Segoe UI", 9F, FontStyle.Italic), ForeColor = Color.Gray, AutoSize = true, Location = new Point(15, 50) };
            pnlLegend.Controls.AddRange(new Control[] { lblLegend, lblInfo });
            pnlCalendar.Controls.Add(pnlLegend);
        }
        #endregion

        #region Cell Selection Logic
        private void HandleCellClick(Panel cell, DateTime date, int hour, bool isPast)
        {
            if (isPast) return;

            // Nếu đã chọn đủ 2 lần (start và end), reset và bắt đầu lại
            if (_selectedStartHour.HasValue && _selectedEndHour.HasValue)
            {
                ResetSelection();
            }

            // Nếu đang chọn ngày khác, reset
            if (_selectedDate.HasValue && _selectedDate.Value.Date != date.Date)
            {
                ResetSelection();
            }

            if (!_selectedStartHour.HasValue)
            {
                _selectedDate = date;
                _selectedStartHour = hour;
                _selectedStartCell = cell;
                cell.BackColor = Color.FromArgb(134, 239, 172);
                return;
            }

            if (!_selectedEndHour.HasValue)
            {
                if (hour <= _selectedStartHour.Value)
                {
                    MessageBox.Show("⚠️ Giờ kết thúc phải sau giờ bắt đầu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                _selectedEndHour = hour + 1; // Kết thúc là giờ tiếp theo
                _selectedEndCell = cell;
                cell.BackColor = Color.FromArgb(134, 239, 172);
                OpenEmployeeSelectionDialog();
                return;
            }
        }

        private void ResetSelection()
        {
            if (_selectedStartCell != null)
            {
                var info = (TimeSlotInfo)_selectedStartCell.Tag;
                var isPast = info.Date.Date < DateTime.Today || (info.Date.Date == DateTime.Today && info.Hour < DateTime.Now.Hour);
                var dateOnly = DateOnly.FromDateTime(info.Date);
                var hasEmployees = _scheduleData.Any(s => s.Ngay == dateOnly && info.Hour >= s.GioBatDau.Hour && info.Hour < s.GioKetThuc.Hour);
                _selectedStartCell.BackColor = hasEmployees ? Color.FromArgb(240, 249, 255) : GetCellBackgroundColor(isPast, info.Date.Date == DateTime.Today && info.Hour == DateTime.Now.Hour, info.Date.Date == DateTime.Today);
            }
            if (_selectedEndCell != null && _selectedEndCell != _selectedStartCell)
            {
                var info = (TimeSlotInfo)_selectedEndCell.Tag;
                var isPast = info.Date.Date < DateTime.Today || (info.Date.Date == DateTime.Today && info.Hour < DateTime.Now.Hour);
                var dateOnly = DateOnly.FromDateTime(info.Date);
                var hasEmployees = _scheduleData.Any(s => s.Ngay == dateOnly && info.Hour >= s.GioBatDau.Hour && info.Hour < s.GioKetThuc.Hour);
                _selectedEndCell.BackColor = hasEmployees ? Color.FromArgb(240, 249, 255) : GetCellBackgroundColor(isPast, info.Date.Date == DateTime.Today && info.Hour == DateTime.Now.Hour, info.Date.Date == DateTime.Today);
            }
            _selectedDate = null;
            _selectedStartHour = null;
            _selectedEndHour = null;
            _selectedStartCell = null;
            _selectedEndCell = null;
        }
        #endregion

        #region Employee Selection Dialog
        private void OpenEmployeeSelectionDialog()
        {
            if (!_selectedDate.HasValue || !_selectedStartHour.HasValue || !_selectedEndHour.HasValue) return;

            var startTime = new TimeOnly(_selectedStartHour.Value, 0);
            var endTime = new TimeOnly(_selectedEndHour.Value, 0);
            string shift = _selectedStartHour.Value < 14 ? "Sang" : "Toi";

            var dialog = new Form
            {
                Text = $"Đăng ký ca - {_selectedDate.Value:dd/MM/yyyy}",
                Size = new Size(500, 600),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White
            };

            var lblTitle = new Label
            {
                Text = "👥 Chọn nhân viên làm việc",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            var pnlInfo = new Panel
            {
                Location = new Point(20, 60),
                Size = new Size(440, 80),
                BackColor = Color.FromArgb(239, 246, 255),
                Padding = new Padding(15)
            };
            var lblDate = new Label
            {
                Text = $"📅 Ngày: {_selectedDate.Value:dddd, dd/MM/yyyy}",
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                Location = new Point(15, 10)
            };
            var lblTimeRange = new Label
            {
                Text = $"⏰ Giờ: {startTime:HH\\:mm} - {endTime:HH\\:mm}",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(37, 99, 235),
                AutoSize = true,
                Location = new Point(15, 35)
            };
            var lblShiftInfo = new Label
            {
                Text = $"📋 Ca: {(shift == "Sang" ? "Sáng" : "Tối")}",
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                Location = new Point(300, 35)
            };
            pnlInfo.Controls.AddRange(new Control[] { lblDate, lblTimeRange, lblShiftInfo });

            var lblSelectEmp = new Label
            {
                Text = "Chọn nhân viên:",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 160)
            };

            var listBox = new CheckedListBox
            {
                Location = new Point(20, 190),
                Size = new Size(440, 300),
                Font = new Font("Segoe UI", 10F),
                CheckOnClick = true
            };

            var dateOnly = DateOnly.FromDateTime(_selectedDate.Value);

            // [FIX] Tìm nhân viên đã được assign cho khung giờ này
            var assignedIds = _scheduleData
                .Where(s => s.Ngay == dateOnly &&
                           s.GioBatDau == startTime &&
                           s.GioKetThuc == endTime)
                .Select(s => s.MaNv)
                .ToHashSet();

            foreach (var emp in _allEmployees)
            {
                var item = new EmployeeListItem { Id = emp.MaNv, Name = emp.TenNv };
                listBox.Items.Add(item, assignedIds.Contains(emp.MaNv));
            }

            var btnSave = new Button
            {
                Text = "💾 Lưu",
                Size = new Size(120, 45),
                Location = new Point(200, 510),
                BackColor = Color.FromArgb(34, 197, 94),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (s, e) =>
            {
                SaveScheduleToDatabase(dateOnly, startTime, endTime, shift, listBox);
                dialog.Close();
                ResetSelection();
                LoadScheduleFromDatabase(); // Reload dữ liệu sau khi lưu
            };

            var btnCancel = new Button
            {
                Text = "Hủy",
                Size = new Size(100, 45),
                Location = new Point(330, 510),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) =>
            {
                dialog.Close();
                ResetSelection();
                LoadScheduleFromDatabase();
            };

            dialog.Controls.AddRange(new Control[] { lblTitle, pnlInfo, lblSelectEmp, listBox, btnSave, btnCancel });
            dialog.ShowDialog(this);
        }

        private void SaveScheduleToDatabase(DateOnly date, TimeOnly startTime, TimeOnly endTime, string shift, CheckedListBox listBox)
        {
            try
            {
                using (var context = new BilliardDbContext())
                {
                    // Xóa lịch cũ cho khoảng thời gian này
                    var oldSchedules = context.Set<LichLamViec>()
                        .Where(l => l.Ngay == date &&
                                   l.GioBatDau == startTime &&
                                   l.GioKetThuc == endTime)
                        .ToList();
                    context.Set<LichLamViec>().RemoveRange(oldSchedules);

                    // Thêm lịch mới cho từng nhân viên được chọn
                    for (int i = 0; i < listBox.CheckedItems.Count; i++)
                    {
                        var item = (EmployeeListItem)listBox.CheckedItems[i];
                        var newSchedule = new LichLamViec
                        {
                            MaNv = item.Id,
                            Ngay = date,
                            GioBatDau = startTime,
                            GioKetThuc = endTime,
                            Ca = shift,
                            TrangThai = "DaXepLich",
                            NgayTao = DateTime.Now,
                            NguoiTao = _currentUserId
                        };
                        context.Set<LichLamViec>().Add(newSchedule);

                        System.Diagnostics.Debug.WriteLine($"Adding schedule: {item.Name} on {date} from {startTime} to {endTime}");
                    }

                    var saved = context.SaveChanges();
                    System.Diagnostics.Debug.WriteLine($"Saved {saved} records");

                    MessageBox.Show(
                        $"✅ Đã lưu lịch làm việc!\n\n" +
                        $"📅 Ngày: {date:dd/MM/yyyy}\n" +
                        $"⏰ Giờ: {startTime:HH\\:mm} - {endTime:HH\\:mm}\n" +
                        $"👥 Số NV: {listBox.CheckedItems.Count}",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu: {ex.Message}\n\nHãy đảm bảo bảng LichLamViec đã được tạo trong database.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Time Slot Details
        private void ShowTimeSlotDetails(DateTime date, int hour, List<LichLamViec> employees)
        {
            var dialog = new Form
            {
                Text = $"Chi tiết - {date:dd/MM/yyyy} - {hour:00}:00",
                Size = new Size(500, 500),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White
            };

            var pnlContent = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20), AutoScroll = true };
            var lblTitle = new Label { Text = $"📅 {date:dddd, dd/MM/yyyy}", Font = new Font("Segoe UI", 14F, FontStyle.Bold), AutoSize = true, Location = new Point(20, 20) };
            var lblTime = new Label { Text = $"⏰ Khung giờ: {hour:00}:00 - {hour + 1:00}:00", Font = new Font("Segoe UI", 11F), ForeColor = Color.Gray, AutoSize = true, Location = new Point(20, 55) };

            int yPos = 100;
            if (employees.Any())
            {
                var lblEmp = new Label { Text = $"👥 Nhân viên ({employees.Count}):", Font = new Font("Segoe UI", 11F, FontStyle.Bold), AutoSize = true, Location = new Point(20, yPos) };
                pnlContent.Controls.Add(lblEmp);
                yPos += 35;

                foreach (var emp in employees)
                {
                    var card = CreateEmployeeDetailCard(emp, date.Date < DateTime.Today);
                    card.Location = new Point(20, yPos);
                    card.Width = 440;
                    pnlContent.Controls.Add(card);
                    yPos += card.Height + 10;
                }
            }
            else
            {
                var lblEmpty = new Label { Text = "Chưa có nhân viên trong khung giờ này", Font = new Font("Segoe UI", 10F, FontStyle.Italic), ForeColor = Color.Gray, AutoSize = true, Location = new Point(20, yPos) };
                pnlContent.Controls.Add(lblEmpty);
            }

            var btnClose = new Button { Text = "Đóng", Size = new Size(100, 40), Location = new Point(360, yPos + 20), BackColor = Color.FromArgb(108, 117, 125), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => dialog.Close();

            pnlContent.Controls.AddRange(new Control[] { lblTitle, lblTime, btnClose });
            dialog.Controls.Add(pnlContent);
            dialog.ShowDialog(this);
        }

        private Panel CreateEmployeeDetailCard(LichLamViec item, bool isPast)
        {
            var card = new Panel { Height = 80, BackColor = isPast ? Color.FromArgb(240, 240, 240) : Color.FromArgb(249, 250, 251) };
            var shiftBar = new Panel { Width = 5, Height = card.Height, Location = new Point(0, 0), BackColor = GetShiftColor(item.Ca) };
            var lblName = new Label { Text = $"👤 {item.NhanVien?.TenNv ?? "N/A"}", Font = new Font("Segoe UI", 11F, FontStyle.Bold), AutoSize = true, Location = new Point(15, 15) };
            var lblShift = new Label { Text = $"📋 Ca: {(item.Ca == "Sang" ? "Sáng" : "Tối")}", Font = new Font("Segoe UI", 9F), ForeColor = Color.Gray, AutoSize = true, Location = new Point(15, 40) };
            var lblTime = new Label { Text = $"🕐 {item.GioBatDau:HH\\:mm} - {item.GioKetThuc:HH\\:mm}", Font = new Font("Segoe UI", 9F), ForeColor = Color.Gray, AutoSize = true, Location = new Point(15, 58) };

            card.Controls.AddRange(new Control[] { shiftBar, lblName, lblShift, lblTime });

            if (!isPast && (_currentUserRole == "Admin" || _currentUserRole == "Quản lý"))
            {
                var btnDelete = new Button { Text = "✕", Size = new Size(30, 30), Location = new Point(400, 25), BackColor = Color.FromArgb(220, 53, 69), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 11F, FontStyle.Bold), Cursor = Cursors.Hand };
                btnDelete.FlatAppearance.BorderSize = 0;
                btnDelete.Click += (s, e) =>
                {
                    if (MessageBox.Show($"Xóa {item.NhanVien?.TenNv} khỏi ca này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        RemoveScheduleFromDatabase(item.Id);
                        ((Form)card.TopLevelControl).Close();
                    }
                };
                card.Controls.Add(btnDelete);
            }
            return card;
        }

        private void RemoveScheduleFromDatabase(int scheduleId)
        {
            try
            {
                using (var context = new BilliardDbContext())
                {
                    var item = context.Set<LichLamViec>().Find(scheduleId);
                    if (item != null)
                    {
                        context.Set<LichLamViec>().Remove(item);
                        context.SaveChanges();
                        MessageBox.Show("✅ Đã xóa khỏi lịch!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadScheduleFromDatabase();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Navigation
        private void NavigateWeek(int direction)
        {
            _currentWeekStart = _currentWeekStart.AddDays(direction * 7);
            ResetSelection();
            LoadScheduleFromDatabase();
        }

        private void GoToToday()
        {
            _currentWeekStart = GetWeekStart(DateTime.Now);
            ResetSelection();
            LoadScheduleFromDatabase();
        }

        private void UpdateWeekDisplay()
        {
            var weekEnd = _currentWeekStart.AddDays(6);
            lblWeekDisplay.Text = $"📅 {_currentWeekStart:dd/MM} - {weekEnd:dd/MM/yyyy}";
        }

        private DateTime GetWeekStart(DateTime date)
        {
            var dayOfWeek = (int)date.DayOfWeek;
            var diff = dayOfWeek == 0 ? -6 : 1 - dayOfWeek;
            return date.AddDays(diff).Date;
        }
        #endregion

        #region Export Excel
        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                using (var saveDialog = new SaveFileDialog { Filter = "Excel Files|*.xlsx", FileName = $"LichLamViec_{_currentWeekStart:yyyyMMdd}.xlsx" })
                {
                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        ExportToExcel(saveDialog.FileName);
                        MessageBox.Show("✅ Xuất Excel thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất Excel: {ex.Message}\n\nHãy cài đặt package ClosedXML qua NuGet.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToExcel(string filePath)
        {
            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("Lịch làm việc");
                var weekEnd = _currentWeekStart.AddDays(6);
                ws.Cell(1, 1).Value = $"LỊCH LÀM VIỆC TUẦN {_currentWeekStart:dd/MM} - {weekEnd:dd/MM/yyyy}";
                ws.Range(1, 1, 1, 8).Merge().Style.Font.Bold = true;

                ws.Cell(3, 1).Value = "Giờ";
                string[] dayNames = { "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7", "CN" };
                for (int i = 0; i < 7; i++)
                {
                    var date = _currentWeekStart.AddDays(i);
                    ws.Cell(3, i + 2).Value = $"{dayNames[i]}\n{date:dd/MM}";
                }

                int row = 4;
                foreach (var hour in _timeSlots)
                {
                    ws.Cell(row, 1).Value = $"{hour:00}:00";
                    for (int day = 0; day < 7; day++)
                    {
                        var date = DateOnly.FromDateTime(_currentWeekStart.AddDays(day));
                        var employees = _scheduleData
                            .Where(s => s.Ngay == date && hour >= s.GioBatDau.Hour && hour < s.GioKetThuc.Hour)
                            .Select(s => s.NhanVien?.TenNv ?? "")
                            .ToList();
                        ws.Cell(row, day + 2).Value = string.Join(", ", employees);
                    }
                    row++;
                }

                ws.Columns().AdjustToContents();
                workbook.SaveAs(filePath);
            }
        }
        #endregion
    }

    #region Helper Classes
    public class LichLamViec
    {
        public int Id { get; set; }
        public int MaNv { get; set; }
        public DateOnly Ngay { get; set; }
        public TimeOnly GioBatDau { get; set; }
        public TimeOnly GioKetThuc { get; set; }
        public string Ca { get; set; }
        public string TrangThai { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? NguoiTao { get; set; }
        public virtual DAL.Entities.NhanVien NhanVien { get; set; }
    }

    public class TimeSlotInfo
    {
        public DateTime Date { get; set; }
        public int Hour { get; set; }
    }

    public class EmployeeListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public override string ToString() => Name;
    }
    #endregion
}