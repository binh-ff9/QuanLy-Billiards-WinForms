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
        private readonly LichLamViecService _lichLamViecService;
        private List<DAL.Entities.NhanVien> _allEmployees;
        private DateTime _currentWeekStart;
        private List<DAL.Entities.LichLamViec> _scheduleData;
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
            _lichLamViecService = new LichLamViecService();
            _scheduleData = new List<DAL.Entities.LichLamViec>();
            _currentWeekStart = GetWeekStart(DateTime.Now);

            InitializeComponent();
            InitializeCustomControls();

            // CRITICAL: Load employees FIRST before loading schedule
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
                // Load tất cả nhân viên trước
                var allEmployees = _nhanVienService.GetAllEmployees();

                System.Diagnostics.Debug.WriteLine($"=== LoadEmployees ===");
                System.Diagnostics.Debug.WriteLine($"Total employees in DB: {allEmployees.Count}");

                // Debug: Xem trạng thái của từng nhân viên
                foreach (var emp in allEmployees)
                {
                    System.Diagnostics.Debug.WriteLine($"  - ID:{emp.MaNv} {emp.TenNv} Status:'{emp.TrangThai}' ({emp.MaNhomNavigation?.TenNhom ?? "No Role"})");
                }

                // FIX: Lọc nhân viên đang làm việc (xử lý cả 2 định dạng: "DangLam" và "Đang làm")
                _allEmployees = allEmployees
                    .Where(e => e.TrangThai != null &&
                               (e.TrangThai.Equals("DangLam", StringComparison.OrdinalIgnoreCase) ||
                                e.TrangThai.Equals("Đang làm", StringComparison.OrdinalIgnoreCase) ||
                                e.TrangThai.Contains("Đang", StringComparison.OrdinalIgnoreCase)))
                    .ToList();

                System.Diagnostics.Debug.WriteLine($"Active employees (filtered): {_allEmployees.Count}");

                if (_allEmployees.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("WARNING: No active employees found after filtering!");
                    System.Diagnostics.Debug.WriteLine("Check if TrangThai values match the filter criteria");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Active employees list:");
                    foreach (var emp in _allEmployees)
                    {
                        System.Diagnostics.Debug.WriteLine($"  ✓ ID:{emp.MaNv} {emp.TenNv} ({emp.MaNhomNavigation?.TenNhom ?? "No Role"})");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách nhân viên: {ex.Message}\n\nStack: {ex.StackTrace}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _allEmployees = new List<DAL.Entities.NhanVien>();
                System.Diagnostics.Debug.WriteLine($"LoadEmployees ERROR: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack: {ex.StackTrace}");
            }
        }

        private void LoadScheduleFromDatabase()
        {
            try
            {
                // Sử dụng LichLamViecService để lấy dữ liệu
                _scheduleData = _lichLamViecService.GetScheduleByWeek(_currentWeekStart);

                System.Diagnostics.Debug.WriteLine($"=== LoadScheduleFromDatabase ===");
                System.Diagnostics.Debug.WriteLine($"Week start: {_currentWeekStart:dd/MM/yyyy}");
                System.Diagnostics.Debug.WriteLine($"Loaded {_scheduleData.Count} schedules");

                foreach (var s in _scheduleData)
                {
                    System.Diagnostics.Debug.WriteLine($"  - ID:{s.Id} NV:{s.MaNv} ({s.NhanVien?.TenNv ?? "NULL"}): {s.Ngay} {s.GioBatDau}-{s.GioKetThuc}");
                }

                RenderCalendar();
                UpdateWeekDisplay();
            }
            catch (Exception ex)
            {
                _scheduleData = new List<DAL.Entities.LichLamViec>();
                RenderCalendar();
                UpdateWeekDisplay();
                System.Diagnostics.Debug.WriteLine($"LoadSchedule error: {ex.Message}");
                MessageBox.Show($"Lỗi khi tải lịch: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            // Kiểm tra nhân viên trong khung giờ này
            var employeesAtHour = _scheduleData
                .Where(s => s.Ngay == dateOnly &&
                           hour >= s.GioBatDau.Hour &&
                           hour < s.GioKetThuc.Hour)
                .ToList();

            if (employeesAtHour.Any())
            {
                // Tạo tooltip với danh sách nhân viên
                var tooltip = new ToolTip();
                var names = string.Join("\n", employeesAtHour.Select(e =>
                    $"👤 {e.NhanVien?.TenNv ?? $"NV#{e.MaNv}"} ({e.GioBatDau:HH\\:mm}-{e.GioKetThuc:HH\\:mm})"));
                tooltip.SetToolTip(panel, names);

                // Hiển thị số lượng nhân viên
                var lblCount = new Label
                {
                    Text = employeesAtHour.Count.ToString(),
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(102, 126, 234),
                    AutoSize = true,
                    Location = new Point(5, 5),
                    BackColor = Color.Transparent
                };
                panel.Controls.Add(lblCount);

                // Icon nhóm
                var lblIcon = new Label
                {
                    Text = "👥",
                    Font = new Font("Segoe UI", 12F),
                    AutoSize = true,
                    Location = new Point(22, 3),
                    BackColor = Color.Transparent
                };
                panel.Controls.Add(lblIcon);

                // Thanh màu chỉ ca làm
                var shift = employeesAtHour.First().Ca;
                var shiftIndicator = new Panel
                {
                    Width = 4,
                    Dock = DockStyle.Left,
                    BackColor = GetShiftColor(shift)
                };
                panel.Controls.Add(shiftIndicator);

                // Đổi màu nền cell có nhân viên
                panel.BackColor = Color.FromArgb(230, 244, 255);

                // FIX: Single-click để xem chi tiết (không cần double-click nữa)
                panel.Click += (s, e) =>
                {
                    // Nếu click vào cell có nhân viên, hiển thị chi tiết
                    ShowTimeSlotDetails(date, hour, employeesAtHour);
                };

                // Cũng cho children
                foreach (Control ctrl in panel.Controls)
                {
                    ctrl.Click += (s, e) => ShowTimeSlotDetails(date, hour, employeesAtHour);
                    ctrl.Cursor = Cursors.Hand;
                }
            }
            else
            {
                // Cell trống - Click để chọn khung giờ (cho việc thêm mới)
                panel.Click += (s, e) => HandleCellClick(panel, date, hour, isPast);
            }

            if (!isPast)
            {
                panel.MouseEnter += (s, e) =>
                {
                    if (panel != _selectedStartCell && panel != _selectedEndCell)
                        panel.BackColor = employeesAtHour.Any() ? Color.FromArgb(200, 230, 255) : Color.FromArgb(239, 246, 255);
                };
                panel.MouseLeave += (s, e) =>
                {
                    if (panel != _selectedStartCell && panel != _selectedEndCell)
                        panel.BackColor = employeesAtHour.Any() ? Color.FromArgb(230, 244, 255) : GetCellBackgroundColor(isPast, isCurrentHour, isToday);
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
            var pnlLegend = new Panel { Location = new Point(0, yPosition), Size = new Size(pnlCalendar.Width - 40, 100), BackColor = Color.White, Padding = new Padding(15) };

            var lblLegend = new Label { Text = "📌 Chú thích:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), AutoSize = true, Location = new Point(15, 15) };

            var shiftLegends = new[]
            {
                (Color.FromArgb(251, 191, 36), "Ca Sáng"),
                (Color.FromArgb(139, 92, 246), "Ca Tối"),
                (Color.FromArgb(134, 239, 172), "Đang chọn"),
                (Color.FromArgb(230, 244, 255), "Có nhân viên")
            };

            int xPos = 130;
            foreach (var (color, text) in shiftLegends)
            {
                var box = new Panel { Size = new Size(25, 25), Location = new Point(xPos, 13), BackColor = color };
                var lbl = new Label { Text = text, Font = new Font("Segoe UI", 9F), AutoSize = true, Location = new Point(xPos + 32, 15) };
                pnlLegend.Controls.AddRange(new Control[] { box, lbl });
                xPos += 140;
            }

            var lblInfo1 = new Label
            {
                Text = "💡 Hướng dẫn sử dụng:",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(59, 130, 246),
                AutoSize = true,
                Location = new Point(15, 50)
            };

            var lblInfo2 = new Label
            {
                Text = "• Click vào ô có nhân viên (👥) để xem chi tiết và chỉnh sửa ca làm việc",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(75, 85, 99),
                AutoSize = true,
                Location = new Point(15, 70)
            };

            pnlLegend.Controls.AddRange(new Control[] { lblLegend, lblInfo1, lblInfo2 });
            pnlCalendar.Controls.Add(pnlLegend);
        }
        #endregion

        #region Cell Selection Logic
        private void HandleCellClick(Panel cell, DateTime date, int hour, bool isPast)
        {
            if (isPast) return;

            if (_selectedStartHour.HasValue && _selectedEndHour.HasValue)
            {
                ResetSelection();
            }

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
                _selectedEndHour = hour + 1;
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
                Size = new Size(500, 650),
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
                Text = $"Chọn nhân viên ({_allEmployees?.Count ?? 0} người):",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 160)
            };

            var listBox = new CheckedListBox
            {
                Location = new Point(20, 190),
                Size = new Size(440, 340),
                Font = new Font("Segoe UI", 10F),
                CheckOnClick = true
            };

            var dateOnly = DateOnly.FromDateTime(_selectedDate.Value);

            // Load lại schedule data mới nhất trước khi hiển thị
            var currentSchedules = _lichLamViecService.GetScheduleByDate(dateOnly);

            // Tìm nhân viên đã được assign cho khung giờ này
            var assignedIds = currentSchedules
                .Where(s => s.GioBatDau == startTime && s.GioKetThuc == endTime)
                .Select(s => s.MaNv)
                .ToHashSet();

            System.Diagnostics.Debug.WriteLine($"=== OpenEmployeeSelectionDialog ===");
            System.Diagnostics.Debug.WriteLine($"Date: {dateOnly}, Start: {startTime}, End: {endTime}");
            System.Diagnostics.Debug.WriteLine($"Total employees: {_allEmployees?.Count ?? 0}");
            System.Diagnostics.Debug.WriteLine($"Already assigned: {assignedIds.Count}");

            // CRITICAL: Kiểm tra _allEmployees có dữ liệu không
            if (_allEmployees == null || _allEmployees.Count == 0)
            {
                var lblNoEmp = new Label
                {
                    Text = "⚠️ Không có nhân viên nào đang làm việc trong hệ thống!\n\nVui lòng kiểm tra lại danh sách nhân viên.",
                    Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                    ForeColor = Color.Red,
                    Location = new Point(30, 220),
                    Size = new Size(420, 80),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                dialog.Controls.Add(lblNoEmp);
                System.Diagnostics.Debug.WriteLine("ERROR: _allEmployees is null or empty!");

                var btnRefresh = new Button
                {
                    Text = "🔄 Tải lại danh sách",
                    Size = new Size(150, 40),
                    Location = new Point(165, 320),
                    BackColor = Color.FromArgb(102, 126, 234),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnRefresh.FlatAppearance.BorderSize = 0;
                btnRefresh.Click += (s, e) =>
                {
                    dialog.Close();
                    LoadEmployees();
                    OpenEmployeeSelectionDialog();
                };
                dialog.Controls.Add(btnRefresh);
            }
            else
            {
                foreach (var emp in _allEmployees)
                {
                    var item = new EmployeeListItem { Id = emp.MaNv, Name = emp.TenNv };
                    bool isAssigned = assignedIds.Contains(emp.MaNv);
                    listBox.Items.Add(item, isAssigned);
                    System.Diagnostics.Debug.WriteLine($"  - {emp.TenNv} (ID:{emp.MaNv}): {(isAssigned ? "✓ ASSIGNED" : "○ not assigned")}");
                }
            }

            var btnSave = new Button
            {
                Text = "💾 Lưu",
                Size = new Size(120, 45),
                Location = new Point(200, 560),
                BackColor = Color.FromArgb(34, 197, 94),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (s, e) =>
            {
                bool saved = SaveScheduleToDatabase(dateOnly, startTime, endTime, shift, listBox);
                dialog.Close();
                ResetSelection();
                if (saved)
                {
                    LoadScheduleFromDatabase();
                }
            };

            var btnCancel = new Button
            {
                Text = "Hủy",
                Size = new Size(100, 45),
                Location = new Point(330, 560),
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
            };

            dialog.Controls.AddRange(new Control[] { lblTitle, pnlInfo, lblSelectEmp, listBox, btnSave, btnCancel });
            dialog.ShowDialog(this);
        }

        private bool SaveScheduleToDatabase(DateOnly date, TimeOnly startTime, TimeOnly endTime, string shift, CheckedListBox listBox)
        {
            try
            {
                var selectedEmployeeIds = new List<int>();
                for (int i = 0; i < listBox.CheckedItems.Count; i++)
                {
                    var item = (EmployeeListItem)listBox.CheckedItems[i];
                    selectedEmployeeIds.Add(item.Id);
                }

                System.Diagnostics.Debug.WriteLine($"Saving: Date={date}, Start={startTime}, End={endTime}, Shift={shift}");
                System.Diagnostics.Debug.WriteLine($"Selected {selectedEmployeeIds.Count} employees: {string.Join(", ", selectedEmployeeIds)}");

                bool success = _lichLamViecService.SaveScheduleForTimeSlot(
                    date, startTime, endTime, shift, selectedEmployeeIds, _currentUserId);

                if (success || selectedEmployeeIds.Count == 0)
                {
                    MessageBox.Show(
                        $"✅ Đã lưu lịch làm việc!\n\n" +
                        $"📅 Ngày: {date:dd/MM/yyyy}\n" +
                        $"⏰ Giờ: {startTime:HH\\:mm} - {endTime:HH\\:mm}\n" +
                        $"👥 Số NV: {selectedEmployeeIds.Count}",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show("Không có thay đổi nào được lưu.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        #endregion

        #region Time Slot Details
        private void ShowTimeSlotDetails(DateTime date, int hour, List<DAL.Entities.LichLamViec> employees)
        {
            var dialog = new Form
            {
                Text = $"Chi tiết ca làm việc - {date:dd/MM/yyyy}",
                Size = new Size(550, 600),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(248, 249, 250)
            };

            var pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                AutoScroll = true,
                BackColor = Color.FromArgb(248, 249, 250)
            };

            // Header với gradient background
            var pnlHeader = new Panel
            {
                Size = new Size(490, 100),
                Location = new Point(20, 20),
                BackColor = Color.FromArgb(102, 126, 234)
            };

            var lblDate = new Label
            {
                Text = date.ToString("dddd, dd/MM/yyyy"),
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 20)
            };

            var lblTimeRange = new Label
            {
                Text = $"⏰ Khung giờ: {hour:00}:00 - {hour + 1:00}:00",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.FromArgb(230, 230, 255),
                AutoSize = true,
                Location = new Point(20, 50)
            };

            var lblEmployeeCount = new Label
            {
                Text = $"👥 {employees.Count}",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(420, 35)
            };

            pnlHeader.Controls.AddRange(new Control[] { lblDate, lblTimeRange, lblEmployeeCount });

            int yPos = 140;

            if (employees.Any())
            {
                var lblTitle = new Label
                {
                    Text = $"Danh sách nhân viên làm việc:",
                    Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(26, 26, 46),
                    AutoSize = true,
                    Location = new Point(20, yPos)
                };
                pnlContent.Controls.Add(lblTitle);
                yPos += 40;

                // Nhóm nhân viên theo ca làm việc
                var groupedByShift = employees.GroupBy(e => new { e.GioBatDau, e.GioKetThuc, e.Ca });

                foreach (var shiftGroup in groupedByShift)
                {
                    // Shift header
                    var pnlShiftHeader = new Panel
                    {
                        Size = new Size(490, 35),
                        Location = new Point(20, yPos),
                        BackColor = GetShiftColor(shiftGroup.Key.Ca)
                    };

                    var lblShift = new Label
                    {
                        Text = $"📋 Ca {(shiftGroup.Key.Ca == "Sang" ? "Sáng" : "Tối")}: {shiftGroup.Key.GioBatDau:HH\\:mm} - {shiftGroup.Key.GioKetThuc:HH\\:mm}",
                        Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                        ForeColor = Color.White,
                        AutoSize = true,
                        Location = new Point(15, 8)
                    };

                    pnlShiftHeader.Controls.Add(lblShift);
                    pnlContent.Controls.Add(pnlShiftHeader);
                    yPos += 40;

                    // Danh sách nhân viên trong ca này
                    foreach (var emp in shiftGroup)
                    {
                        var card = CreateEmployeeDetailCard(emp, date.Date < DateTime.Today);
                        card.Location = new Point(20, yPos);
                        card.Width = 490;
                        pnlContent.Controls.Add(card);
                        yPos += card.Height + 8;
                    }

                    yPos += 10; // Space between shifts
                }
            }
            else
            {
                var pnlEmpty = new Panel
                {
                    Size = new Size(490, 150),
                    Location = new Point(20, yPos),
                    BackColor = Color.White
                };

                var lblEmptyIcon = new Label
                {
                    Text = "📭",
                    Font = new Font("Segoe UI", 48F),
                    AutoSize = true,
                    Location = new Point(210, 20)
                };

                var lblEmpty = new Label
                {
                    Text = "Chưa có nhân viên trong khung giờ này",
                    Font = new Font("Segoe UI", 11F, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Location = new Point(130, 100)
                };

                pnlEmpty.Controls.AddRange(new Control[] { lblEmptyIcon, lblEmpty });
                pnlContent.Controls.Add(pnlEmpty);
                yPos += 160;
            }

            // Action buttons panel
            var pnlButtons = new Panel
            {
                Size = new Size(490, 50),
                Location = new Point(20, yPos + 10),
                BackColor = Color.Transparent
            };

            var btnEdit = new Button
            {
                Text = "✏️ Chỉnh sửa ca",
                Size = new Size(140, 45),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Visible = (_currentUserRole == "Admin" || _currentUserRole == "Quản lý") && date.Date >= DateTime.Today
            };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Click += (s, e) =>
            {
                dialog.Close();
                // Mở dialog chỉnh sửa ca này
                EditTimeSlot(date, hour, employees);
            };

            var btnClose = new Button
            {
                Text = "Đóng",
                Size = new Size(120, 45),
                Location = new Point(370, 0),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => dialog.Close();

            pnlButtons.Controls.AddRange(new Control[] { btnEdit, btnClose });

            pnlContent.Controls.AddRange(new Control[] { pnlHeader, pnlButtons });
            dialog.Controls.Add(pnlContent);
            dialog.ShowDialog(this);
        }

        private void EditTimeSlot(DateTime date, int hour, List<DAL.Entities.LichLamViec> employees)
        {
            if (employees.Count == 0) return;

            // Lấy thông tin ca làm việc từ nhân viên đầu tiên
            var firstEmp = employees.First();
            var dateOnly = DateOnly.FromDateTime(date);
            var startTime = firstEmp.GioBatDau;
            var endTime = firstEmp.GioKetThuc;
            string shift = firstEmp.Ca;

            var dialog = new Form
            {
                Text = $"Chỉnh sửa ca - {date:dd/MM/yyyy}",
                Size = new Size(500, 650),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White
            };

            var lblTitle = new Label
            {
                Text = "👥 Chỉnh sửa nhân viên làm việc",
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
                Text = $"📅 Ngày: {date:dddd, dd/MM/yyyy}",
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
                Text = $"Chọn nhân viên ({_allEmployees?.Count ?? 0} người):",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 160)
            };

            var listBox = new CheckedListBox
            {
                Location = new Point(20, 190),
                Size = new Size(440, 340),
                Font = new Font("Segoe UI", 10F),
                CheckOnClick = true
            };

            // Load danh sách nhân viên và check những người đã được assign
            var assignedIds = employees.Select(e => e.MaNv).ToHashSet();

            if (_allEmployees != null && _allEmployees.Count > 0)
            {
                foreach (var emp in _allEmployees)
                {
                    var item = new EmployeeListItem { Id = emp.MaNv, Name = emp.TenNv };
                    bool isAssigned = assignedIds.Contains(emp.MaNv);
                    listBox.Items.Add(item, isAssigned);
                }
            }

            var btnSave = new Button
            {
                Text = "💾 Lưu thay đổi",
                Size = new Size(140, 45),
                Location = new Point(180, 560),
                BackColor = Color.FromArgb(34, 197, 94),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (s, e) =>
            {
                bool saved = SaveScheduleToDatabase(dateOnly, startTime, endTime, shift, listBox);
                dialog.Close();
                if (saved)
                {
                    LoadScheduleFromDatabase();
                }
            };

            var btnCancel = new Button
            {
                Text = "Hủy",
                Size = new Size(100, 45),
                Location = new Point(330, 560),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => dialog.Close();

            dialog.Controls.AddRange(new Control[] { lblTitle, pnlInfo, lblSelectEmp, listBox, btnSave, btnCancel });
            dialog.ShowDialog(this);
        }

        private Panel CreateEmployeeDetailCard(DAL.Entities.LichLamViec item, bool isPast)
        {
            var card = new Panel
            {
                Height = 90,
                BackColor = Color.White,
                Cursor = Cursors.Hand
            };

            // Hover effect
            card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(245, 247, 250);
            card.MouseLeave += (s, e) => card.BackColor = Color.White;

            var shiftBar = new Panel
            {
                Width = 5,
                Height = card.Height,
                Location = new Point(0, 0),
                BackColor = GetShiftColor(item.Ca)
            };

            // Avatar placeholder
            var pnlAvatar = new Panel
            {
                Size = new Size(60, 60),
                Location = new Point(15, 15),
                BackColor = Color.FromArgb(102, 126, 234)
            };

            var lblInitial = new Label
            {
                Text = item.NhanVien?.TenNv?.Substring(0, 1).ToUpper() ?? "?",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(60, 60),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlAvatar.Controls.Add(lblInitial);

            var lblName = new Label
            {
                Text = item.NhanVien?.TenNv ?? "N/A",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(85, 18),
                ForeColor = Color.FromArgb(26, 26, 46)
            };

            var lblRole = new Label
            {
                Text = $"👔 {item.NhanVien?.MaNhomNavigation?.TenNhom ?? "N/A"}",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(107, 114, 128),
                AutoSize = true,
                Location = new Point(85, 45)
            };

            var lblTime = new Label
            {
                Text = $"🕐 {item.GioBatDau:HH\\:mm} - {item.GioKetThuc:HH\\:mm}",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(107, 114, 128),
                AutoSize = true,
                Location = new Point(85, 65)
            };

            card.Controls.AddRange(new Control[] { shiftBar, pnlAvatar, lblName, lblRole, lblTime });

            if (!isPast && (_currentUserRole == "Admin" || _currentUserRole == "Quản lý"))
            {
                var btnDelete = new Button
                {
                    Text = "✕",
                    Size = new Size(35, 35),
                    Location = new Point(440, 28),
                    BackColor = Color.FromArgb(220, 53, 69),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnDelete.FlatAppearance.BorderSize = 0;
                btnDelete.MouseEnter += (s, e) => btnDelete.BackColor = Color.FromArgb(185, 28, 28);
                btnDelete.MouseLeave += (s, e) => btnDelete.BackColor = Color.FromArgb(220, 53, 69);
                btnDelete.Click += (s, e) =>
                {
                    if (MessageBox.Show(
                        $"Xóa {item.NhanVien?.TenNv} khỏi ca làm việc này?",
                        "Xác nhận xóa",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
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
                bool success = _lichLamViecService.DeleteSchedule(scheduleId);
                if (success)
                {
                    MessageBox.Show("✅ Đã xóa khỏi lịch!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadScheduleFromDatabase();
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