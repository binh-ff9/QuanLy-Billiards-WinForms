using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Billiard.BLL.Services.NhanVienService;
using Billiard.DAL.Entities;
using ClosedXML.Excel;

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

        // Định nghĩa các ca làm việc với màu sắc
        private readonly Dictionary<string, ShiftInfo> _shiftTemplates = new Dictionary<string, ShiftInfo>
        {
            { "Ca sáng", new ShiftInfo(new TimeOnly(8, 0), new TimeOnly(14, 0), Color.FromArgb(251, 191, 36), "☀️ Ca sáng (8h-14h)") },
            { "Ca chiều", new ShiftInfo(new TimeOnly(14, 0), new TimeOnly(20, 0), Color.FromArgb(249, 115, 22), "🌤️ Ca chiều (14h-20h)") },
            { "Ca tối", new ShiftInfo(new TimeOnly(20, 0), new TimeOnly(2, 0), Color.FromArgb(168, 85, 247), "🌙 Ca tối (20h-2h)") },
            { "Full sáng-chiều", new ShiftInfo(new TimeOnly(8, 0), new TimeOnly(20, 0), Color.FromArgb(59, 130, 246), "💪 Full sáng-chiều (8h-20h)") },
            { "Full chiều-tối", new ShiftInfo(new TimeOnly(14, 0), new TimeOnly(2, 0), Color.FromArgb(139, 92, 246), "🔥 Full chiều-tối (14h-2h)") },
            { "Full cả ngày", new ShiftInfo(new TimeOnly(8, 0), new TimeOnly(2, 0), Color.FromArgb(34, 197, 94), "⚡ Full cả ngày (8h-2h)") }
        };
        #endregion

        #region Constructor
        public ScheduleForm(NhanVienService nhanVienService)
        {
            _nhanVienService = nhanVienService;
            _lichLamViecService = new LichLamViecService();
            _scheduleData = new List<DAL.Entities.LichLamViec>();
            _currentWeekStart = GetWeekStart(DateTime.Now);

            InitializeComponent();

            LoadEmployees();
            LoadScheduleFromDatabase();
        }

        public void SetUserInfo(int userId, string userRole)
        {
            _currentUserId = userId;
            _currentUserRole = userRole;
        }
        #endregion

        #region Load Data
        private void LoadEmployees()
        {
            try
            {
                _allEmployees = _nhanVienService.GetEmployeesByStatus("Đang làm");
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
                _scheduleData = _lichLamViecService.GetScheduleByWeek(_currentWeekStart);
                UpdateWeekDisplay();
                RenderCalendar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải lịch làm việc: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Render Calendar
        private void RenderCalendar()
        {
            pnlCalendar.Controls.Clear();

            // Create table layout
            var table = new TableLayoutPanel
            {
                AutoSize = true,
                ColumnCount = 8,
                RowCount = _shiftTemplates.Count + 1,
                Padding = new Padding(0),
                BackColor = Color.White,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
            };

            // Set column styles
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180F)); // Shift name column
            for (int i = 0; i < 7; i++)
            {
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.28F));
            }

            // Set row styles
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F)); // Header row
            for (int i = 0; i < _shiftTemplates.Count; i++)
            {
                table.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F));
            }

            // Create header row
            CreateHeaderCell(table, 0, 0, "CA LÀM VIỆC");

            string[] dayNames = { "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7", "Chủ nhật" };
            for (int day = 0; day < 7; day++)
            {
                var date = _currentWeekStart.AddDays(day);
                var isToday = date.Date == DateTime.Today;
                CreateHeaderCell(table, 0, day + 1, $"{dayNames[day]}\n{date:dd/MM/yyyy}", isToday);
            }

            // Create shift rows
            int row = 1;
            foreach (var shift in _shiftTemplates)
            {
                CreateShiftNameCell(table, row, 0, shift.Key, shift.Value);

                for (int day = 0; day < 7; day++)
                {
                    var date = _currentWeekStart.AddDays(day);
                    CreateShiftCell(table, row, day + 1, date, shift.Key, shift.Value);
                }
                row++;
            }

            // Calculate table size based on form
            int tableWidth = Math.Max(1200, pnlCalendar.Width - 60);
            table.Width = tableWidth;
            table.Location = new Point(0, 0);

            pnlCalendar.Controls.Add(table);
        }

        private void CreateHeaderCell(TableLayoutPanel table, int row, int col, string text, bool isToday = false)
        {
            var cell = new Panel
            {
                Dock = DockStyle.Fill,
                // HÔM NAY: Màu xanh dương nổi bật, ngày thường: màu xám đậm
                BackColor = isToday ? Color.FromArgb(14, 165, 233) : Color.FromArgb(51, 65, 85),
                Padding = new Padding(5)
            };

            var label = new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter
            };

            cell.Controls.Add(label);
            table.Controls.Add(cell, col, row);
        }

        private void CreateShiftNameCell(TableLayoutPanel table, int row, int col, string shiftName, ShiftInfo shiftInfo)
        {
            var cell = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(241, 245, 249),
                Padding = new Padding(10, 5, 10, 5)
            };

            var lblShiftName = new Label
            {
                Text = shiftInfo.DisplayName,
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Height = 50,
                TextAlign = ContentAlignment.MiddleLeft
            };

            var lblTime = new Label
            {
                Text = $"{GetHourDuration(shiftInfo)} giờ",
                Dock = DockStyle.Bottom,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(100, 116, 139),
                Height = 25,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Color indicator
            var colorIndicator = new Panel
            {
                Width = 5,
                Dock = DockStyle.Left,
                BackColor = shiftInfo.Color
            };

            cell.Controls.AddRange(new Control[] { lblTime, lblShiftName, colorIndicator });
            table.Controls.Add(cell, col, row);
        }

        private void CreateShiftCell(TableLayoutPanel table, int row, int col, DateTime date, string shiftName, ShiftInfo shiftInfo)
        {
            var cell = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(5),
                Cursor = Cursors.Hand,
                Tag = new { Date = date, ShiftName = shiftName }
            };

            // KIỂM TRA ĐIỀU KIỆN: Chỉ những ngày sau ngày hiện tại mới được coi là tương lai
            bool isFutureDate = date.Date > DateTime.Today;
            bool isPastOrToday = date.Date <= DateTime.Today;

            // Lấy dữ liệu nhân viên đã xếp (giữ nguyên logic cũ)
            var schedules = GetSchedulesForShift(date, shiftName);
            int employeeCount = schedules.Count;

            // Hiển thị badge số lượng nhân viên
            if (employeeCount > 0)
            {
                var badge = new Label
                {
                    Text = $"👥 {employeeCount}",
                    AutoSize = true,
                    Location = new Point(5, 5),
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = Color.White,
                    // Nếu không cho phép sửa (quá khứ/hôm nay), hiển thị màu xám
                    BackColor = isPastOrToday ? Color.FromArgb(156, 163, 175) : shiftInfo.Color,
                    Padding = new Padding(8, 3, 8, 3)
                };
                cell.Controls.Add(badge);
            }

            // Hiển thị danh sách tên nhân viên (giữ nguyên logic cũ)
            var employeePanel = new FlowLayoutPanel
            {
                Location = new Point(5, employeeCount > 0 ? 30 : 5),
                Size = new Size(cell.Width - 10, cell.Height - (employeeCount > 0 ? 35 : 10)),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Color.Transparent
            };

            foreach (var schedule in schedules)
            {
                var empLabel = new Label
                {
                    Text = "• " + (schedule.NhanVien?.TenNv ?? "N/A"),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 9F),
                    // Làm mờ text nếu là ngày không thể chỉnh sửa
                    ForeColor = isPastOrToday ? Color.FromArgb(148, 163, 184) : Color.FromArgb(51, 65, 85),
                    MaximumSize = new Size(employeePanel.Width - 10, 0)
                };
                employeePanel.Controls.Add(empLabel);
            }
            cell.Controls.Add(employeePanel);

            // --- THIẾT LẬP TƯƠNG TÁC ---

            if (isFutureDate)
            {
                // Chỉ cho phép click nếu là ngày tương lai
                cell.Click += (s, e) => OpenScheduleDialog(date, shiftName);
                foreach (Control ctrl in cell.Controls)
                {
                    ctrl.Click += (s, e) => OpenScheduleDialog(date, shiftName);
                }

                // Hover effect cho ngày tương lai
                cell.MouseEnter += (s, e) => cell.BackColor = Color.FromArgb(240, 253, 244); // Xanh lá nhạt
                cell.MouseLeave += (s, e) => cell.BackColor = Color.White;
            }
            else
            {
                // Khóa tương tác cho quá khứ và hôm nay
                cell.Cursor = Cursors.No; // Hiển thị icon không cho phép
                cell.BackColor = Color.FromArgb(241, 245, 249); // Màu xám nhạt để báo hiệu bị khóa

                // Gắn tooltip để giải thích cho người dùng (tùy chọn)
                var toolTip = new ToolTip();
                toolTip.SetToolTip(cell, "Không thể xếp lịch cho quá khứ hoặc hôm nay.");
            }

            table.Controls.Add(cell, col, row);
        }

        private List<DAL.Entities.LichLamViec> GetSchedulesForShift(DateTime date, string shiftName)
        {
            var shiftInfo = _shiftTemplates[shiftName];
            var dateOnly = DateOnly.FromDateTime(date);

            return _scheduleData
                .Where(s => s.Ngay == dateOnly
                         && s.GioBatDau == shiftInfo.StartTime
                         && s.GioKetThuc == shiftInfo.EndTime)
                .ToList();
        }

        private decimal GetHourDuration(ShiftInfo shiftInfo)
        {
            var hours = shiftInfo.EndTime.Hour - shiftInfo.StartTime.Hour;
            if (hours < 0) hours += 24; // Handle overnight shifts
            return hours;
        }
        #endregion

        #region Schedule Dialog
        private void OpenScheduleDialog(DateTime date, string shiftName)
        {
            var shiftInfo = _shiftTemplates[shiftName];

            var dialog = new Form
            {
                Text = "Xếp lịch làm việc",
                Size = new Size(720, 880),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White
            };

            // ===== HEADER PANEL =====
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = shiftInfo.Color,
                Padding = new Padding(20, 15, 20, 15)
            };

            var lblDialogTitle = new Label
            {
                Text = "🕐 XẾP LỊCH CA LÀM VIỆC",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 12)
            };

            var lblShiftInfo = new Label
            {
                Text = $"{shiftInfo.DisplayName}\n📅 {date:dddd, dd/MM/yyyy}",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 45)
            };

            pnlHeader.Controls.AddRange(new Control[] { lblDialogTitle, lblShiftInfo });

            // ===== FOOTER PANEL =====
            var pnlFooter = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                BackColor = Color.FromArgb(248, 250, 252),
                Padding = new Padding(20, 12, 20, 12)
            };

            var btnSave = new Button
            {
                Text = "💾 Lưu lịch",
                Size = new Size(130, 45),
                Location = new Point(290, 12),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(34, 197, 94),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatAppearance.MouseOverBackColor = Color.FromArgb(22, 163, 74);

            var btnCancel = new Button
            {
                Text = "✖ Hủy",
                Size = new Size(130, 45),
                Location = new Point(540, 12),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(239, 68, 68),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 38, 38);
            btnCancel.Click += (s, e) => dialog.Close();

            var btnClearAll = new Button
            {
                Text = "🗑️ Xóa tất cả",
                Size = new Size(130, 45),
                Location = new Point(40, 12),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(249, 115, 22),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnClearAll.FlatAppearance.BorderSize = 0;
            btnClearAll.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 88, 12);

            pnlFooter.Controls.AddRange(new Control[] { btnClearAll, btnSave, btnCancel });

            // ===== CONTENT PANEL với TableLayoutPanel =====
            var pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20, 15, 20, 15),
                BackColor = Color.White
            };

            // Sử dụng TableLayoutPanel để tránh chồng chéo
            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 5,
                ColumnCount = 1,
                AutoSize = false,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };

            // Phân bổ chiều cao rõ ràng cho từng row
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));   // Row 0: Instruction
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));   // Row 1: Search
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));   // Row 2: Quick buttons
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));   // Row 3: Employee list (chiếm phần còn lại)
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));   // Row 4: Counter

            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            // === ROW 0: INSTRUCTION LABEL ===
            var lblInstruction = new Label
            {
                Text = "💡 Chọn nhân viên cho ca làm việc này (có thể chọn nhiều):",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(0, 5, 0, 0)
            };

            // === ROW 1: SEARCH PANEL ===
            var pnlSearch = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            var lblSearch = new Label
            {
                Text = "Tìm:",
                AutoSize = true,
                Location = new Point(0, 12),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 116, 139)
            };

            var txtSearch = new TextBox
            {
                Location = new Point(80, 9),
                Size = new Size(450, 28),
                Font = new Font("Segoe UI", 10F),
                BorderStyle = BorderStyle.FixedSingle,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
            };

            pnlSearch.Controls.AddRange(new Control[] { lblSearch, txtSearch });

            // === ROW 2: QUICK SELECTION PANEL ===
            var pnlQuickSelect = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            

            var btnSelectAll = new Button
            {
                Text = "Tất cả",
                Size = new Size(80, 34),
                Location = new Point(105, 7),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(34, 197, 94),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnSelectAll.FlatAppearance.BorderSize = 0;
            btnSelectAll.FlatAppearance.MouseOverBackColor = Color.FromArgb(22, 163, 74);

            var btnDeselectAll = new Button
            {
                Text = "Bỏ chọn",
                Size = new Size(85, 34),
                Location = new Point(190, 7),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(239, 68, 68),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnDeselectAll.FlatAppearance.BorderSize = 0;
            btnDeselectAll.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 38, 38);

            var btnSelectByRole = new Button
            {
                Text = "Vai trò",
                Size = new Size(95, 34),
                Location = new Point(280, 7),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnSelectByRole.FlatAppearance.BorderSize = 0;
            btnSelectByRole.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);

            var btnInvertSelection = new Button
            {
                Text = "Đảo",
                Size = new Size(80, 34),
                Location = new Point(380, 7),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(168, 85, 247),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnInvertSelection.FlatAppearance.BorderSize = 0;
            btnInvertSelection.FlatAppearance.MouseOverBackColor = Color.FromArgb(147, 51, 234);

            pnlQuickSelect.Controls.AddRange(new Control[] {
         btnSelectAll, btnDeselectAll, btnSelectByRole, btnInvertSelection
    });

            // === ROW 3: EMPLOYEE CHECKEDLISTBOX ===
            var clbEmployees = new CheckedListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F),
                CheckOnClick = true,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(248, 250, 252),
                ItemHeight = 24,
                IntegralHeight = false
            };

            // Load all employees
            foreach (var emp in _allEmployees.OrderBy(e => e.TenNv))
            {
                clbEmployees.Items.Add(new EmployeeCheckItem
                {
                    Id = emp.MaNv,
                    Name = $"{emp.TenNv} - {emp.MaNhomNavigation?.TenNhom ?? "N/A"}",
                    Employee = emp
                });
            }

            // Pre-select already scheduled employees
            var existingSchedules = GetSchedulesForShift(date, shiftName);
            for (int i = 0; i < clbEmployees.Items.Count; i++)
            {
                var item = (EmployeeCheckItem)clbEmployees.Items[i];
                if (existingSchedules.Any(s => s.MaNv == item.Id))
                {
                    clbEmployees.SetItemChecked(i, true);
                }
            }

            // === ROW 4: COUNTER LABEL ===
            var lblCounter = new Label
            {
                Text = $"✓ Đã chọn: {clbEmployees.CheckedItems.Count} nhân viên",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(34, 197, 94),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(0, 5, 0, 0)
            };

            // === ADD ALL CONTROLS TO TABLELAYOUT ===
            mainLayout.Controls.Add(lblInstruction, 0, 0);
            mainLayout.Controls.Add(pnlSearch, 0, 1);
            mainLayout.Controls.Add(pnlQuickSelect, 0, 2);
            mainLayout.Controls.Add(clbEmployees, 0, 3);
            mainLayout.Controls.Add(lblCounter, 0, 4);

            pnlContent.Controls.Add(mainLayout);

            // ===== EVENT HANDLERS =====

            // Search functionality
            txtSearch.TextChanged += (s, e) =>
            {
                var searchText = txtSearch.Text.ToLower();
                var checkedItems = new HashSet<int>();

                // Remember checked items
                for (int i = 0; i < clbEmployees.Items.Count; i++)
                {
                    if (clbEmployees.GetItemChecked(i))
                    {
                        var item = (EmployeeCheckItem)clbEmployees.Items[i];
                        checkedItems.Add(item.Id);
                    }
                }

                // Clear and reload filtered items
                clbEmployees.Items.Clear();
                foreach (var emp in _allEmployees.OrderBy(e => e.TenNv))
                {
                    var empName = emp.TenNv.ToLower();
                    var roleName = (emp.MaNhomNavigation?.TenNhom ?? "").ToLower();

                    if (string.IsNullOrWhiteSpace(searchText) ||
                        empName.Contains(searchText) ||
                        roleName.Contains(searchText))
                    {
                        var item = new EmployeeCheckItem
                        {
                            Id = emp.MaNv,
                            Name = $"{emp.TenNv} - {emp.MaNhomNavigation?.TenNhom ?? "N/A"}",
                            Employee = emp
                        };

                        int index = clbEmployees.Items.Add(item);

                        // Restore checked state
                        if (checkedItems.Contains(item.Id))
                        {
                            clbEmployees.SetItemChecked(index, true);
                        }
                    }
                }
            };

            // Quick selection buttons
            btnSelectAll.Click += (s, e) =>
            {
                for (int i = 0; i < clbEmployees.Items.Count; i++)
                {
                    clbEmployees.SetItemChecked(i, true);
                }
            };

            btnDeselectAll.Click += (s, e) =>
            {
                for (int i = 0; i < clbEmployees.Items.Count; i++)
                {
                    clbEmployees.SetItemChecked(i, false);
                }
            };

            btnSelectByRole.Click += (s, e) =>
            {
                ShowRoleSelectionMenu(btnSelectByRole, clbEmployees);
            };

            btnInvertSelection.Click += (s, e) =>
            {
                for (int i = 0; i < clbEmployees.Items.Count; i++)
                {
                    clbEmployees.SetItemChecked(i, !clbEmployees.GetItemChecked(i));
                }
            };

            // Update counter when selection changes
            clbEmployees.ItemCheck += (s, e) =>
            {
                this.BeginInvoke(new Action(() =>
                {
                    lblCounter.Text = $"✓ Đã chọn: {clbEmployees.CheckedItems.Count} nhân viên";
                }));
            };

            // Clear all button
            btnClearAll.Click += (s, e) =>
            {
                if (MessageBox.Show(
                    "Xóa tất cả nhân viên khỏi ca này?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    for (int i = 0; i < clbEmployees.Items.Count; i++)
                    {
                        clbEmployees.SetItemChecked(i, false);
                    }
                }
            };

            // Save button
            btnSave.Click += (s, e) => SaveShiftAssignment(date, shiftName, clbEmployees, dialog);

            // ===== ADD ALL PANELS TO DIALOG =====
            dialog.Controls.Add(pnlContent);  // Content ở giữa
            dialog.Controls.Add(pnlFooter);   // Footer ở dưới
            dialog.Controls.Add(pnlHeader);   // Header ở trên

            dialog.ShowDialog(this);
        }
        private void SaveShiftAssignment(DateTime date, string shiftName, CheckedListBox clbEmployees, Form dialog)
        {
            try
            {
                var shiftInfo = _shiftTemplates[shiftName];
                var selectedEmployeeIds = clbEmployees.CheckedItems
                    .Cast<EmployeeCheckItem>()
                    .Select(item => item.Id)
                    .ToList();

                var dateOnly = DateOnly.FromDateTime(date);

                bool success = _lichLamViecService.SaveScheduleForTimeSlot(
                    dateOnly,
                    shiftInfo.StartTime,
                    shiftInfo.EndTime,
                    shiftName,
                    selectedEmployeeIds,
                    _currentUserId
                );

                if (success || selectedEmployeeIds.Count == 0)
                {
                    MessageBox.Show(
                        $"✅ Đã lưu lịch thành công!\n\n" +
                        $"Ca: {shiftName}\n" +
                        $"Ngày: {date:dd/MM/yyyy}\n" +
                        $"Số nhân viên: {selectedEmployeeIds.Count}",
                        "Thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    LoadScheduleFromDatabase();
                    dialog.Close();
                }
                else
                {
                    MessageBox.Show("Không có thay đổi nào được lưu.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu lịch: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowRoleSelectionMenu(Button sourceButton, CheckedListBox clbEmployees)
        {
            var roleMenu = new ContextMenuStrip();

            var roles = _allEmployees
                .Select(e => e.MaNhomNavigation?.TenNhom ?? "N/A")
                .Distinct()
                .OrderBy(r => r)
                .ToList();

            foreach (var role in roles)
            {
                var menuItem = new ToolStripMenuItem(role);
                menuItem.Click += (s, e) =>
                {
                    for (int i = 0; i < clbEmployees.Items.Count; i++)
                    {
                        var item = (EmployeeCheckItem)clbEmployees.Items[i];
                        if ((item.Employee.MaNhomNavigation?.TenNhom ?? "N/A") == role)
                        {
                            clbEmployees.SetItemChecked(i, true);
                        }
                    }
                };
                roleMenu.Items.Add(menuItem);
            }

            roleMenu.Show(sourceButton, new Point(0, sourceButton.Height));
        }
        #endregion

        #region Week Navigation
        private DateTime GetWeekStart(DateTime date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(-1 * diff).Date;
        }

        private void UpdateWeekDisplay()
        {
            var weekEnd = _currentWeekStart.AddDays(6);
            lblWeekDisplay.Text = $"📅 {_currentWeekStart:dd/MM} - {weekEnd:dd/MM/yyyy}";
        }

        private void BtnPrevWeek_Click(object sender, EventArgs e)
        {
            _currentWeekStart = _currentWeekStart.AddDays(-7);
            LoadScheduleFromDatabase();
        }

        private void BtnNextWeek_Click(object sender, EventArgs e)
        {
            _currentWeekStart = _currentWeekStart.AddDays(7);
            LoadScheduleFromDatabase();
        }

        private void BtnToday_Click(object sender, EventArgs e)
        {
            _currentWeekStart = GetWeekStart(DateTime.Now);
            LoadScheduleFromDatabase();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadScheduleFromDatabase();
            MessageBox.Show("✅ Đã làm mới dữ liệu!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region Export Excel
        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                using (var saveDialog = new SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    FileName = $"LichLamViec_{_currentWeekStart:ddMMyyyy}.xlsx",
                    Title = "Xuất lịch làm việc ra Excel"
                })
                {
                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        ExportToExcel(saveDialog.FileName);

                        if (MessageBox.Show(
                            "✅ Xuất Excel thành công!\n\nBạn có muốn mở file không?",
                            "Thành công",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                            {
                                FileName = saveDialog.FileName,
                                UseShellExecute = true
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất Excel: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToExcel(string filePath)
        {
            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("Lịch làm việc");

                // Title
                var weekEnd = _currentWeekStart.AddDays(6);
                ws.Cell(1, 1).Value = "LỊCH LÀM VIỆC NHÂN VIÊN";
                ws.Range(1, 1, 1, 8).Merge();
                ws.Range(1, 1, 1, 8).Style.Font.Bold = true;
                ws.Range(1, 1, 1, 8).Style.Font.FontSize = 16;
                ws.Range(1, 1, 1, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range(1, 1, 1, 8).Style.Fill.BackgroundColor = XLColor.FromArgb(30, 41, 59);
                ws.Range(1, 1, 1, 8).Style.Font.FontColor = XLColor.White;

                ws.Cell(2, 1).Value = $"Tuần: {_currentWeekStart:dd/MM/yyyy} - {weekEnd:dd/MM/yyyy}";
                ws.Range(2, 1, 2, 8).Merge();
                ws.Range(2, 1, 2, 8).Style.Font.Bold = true;
                ws.Range(2, 1, 2, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range(2, 1, 2, 8).Style.Fill.BackgroundColor = XLColor.FromArgb(71, 85, 105);
                ws.Range(2, 1, 2, 8).Style.Font.FontColor = XLColor.White;

                // Headers
                ws.Cell(4, 1).Value = "CA LÀM VIỆC";
                string[] dayNames = { "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7", "Chủ nhật" };

                for (int i = 0; i < 7; i++)
                {
                    var date = _currentWeekStart.AddDays(i);
                    ws.Cell(4, i + 2).Value = $"{dayNames[i]}\n{date:dd/MM}";
                    ws.Cell(4, i + 2).Style.Alignment.WrapText = true;
                }

                ws.Range(4, 1, 4, 8).Style.Font.Bold = true;
                ws.Range(4, 1, 4, 8).Style.Fill.BackgroundColor = XLColor.LightGray;
                ws.Range(4, 1, 4, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range(4, 1, 4, 8).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range(4, 1, 4, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thick;

                // Data rows with color coding
                int row = 5;
                foreach (var shift in _shiftTemplates)
                {
                    ws.Cell(row, 1).Value = shift.Value.DisplayName;
                    ws.Cell(row, 1).Style.Font.Bold = true;

                    // Set shift color
                    var color = shift.Value.Color;
                    ws.Cell(row, 1).Style.Fill.BackgroundColor = XLColor.FromArgb(color.R, color.G, color.B);
                    ws.Cell(row, 1).Style.Font.FontColor = XLColor.White;

                    for (int day = 0; day < 7; day++)
                    {
                        var date = _currentWeekStart.AddDays(day);
                        var schedules = GetSchedulesForShift(date, shift.Key);
                        var employeeNames = schedules.Select(s => s.NhanVien?.TenNv ?? "").ToList();

                        var cellValue = employeeNames.Count > 0
                            ? string.Join("\n", employeeNames)
                            : "-";

                        ws.Cell(row, day + 2).Value = cellValue;
                        ws.Cell(row, day + 2).Style.Alignment.WrapText = true;
                        ws.Cell(row, day + 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;

                        if (employeeNames.Count > 0)
                        {
                            var lightColor = Color.FromArgb(255,
                                Math.Min(255, color.R + 100),
                                Math.Min(255, color.G + 100),
                                Math.Min(255, color.B + 100));
                            ws.Cell(row, day + 2).Style.Fill.BackgroundColor = XLColor.FromArgb(lightColor.R, lightColor.G, lightColor.B);
                        }
                    }

                    ws.Row(row).Height = 60;
                    row++;
                }

                // Borders for all cells
                ws.Range(4, 1, row - 1, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                ws.Range(4, 1, row - 1, 8).Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                // Column widths
                ws.Column(1).Width = 25;
                for (int i = 2; i <= 8; i++)
                {
                    ws.Column(i).Width = 20;
                }

                // Summary statistics
                row += 2;
                ws.Cell(row, 1).Value = "THỐNG KÊ TỔNG QUAN";
                ws.Range(row, 1, row, 8).Merge();
                ws.Range(row, 1, row, 8).Style.Font.Bold = true;
                ws.Range(row, 1, row, 8).Style.Fill.BackgroundColor = XLColor.FromArgb(51, 65, 85);
                ws.Range(row, 1, row, 8).Style.Font.FontColor = XLColor.White;
                ws.Range(row, 1, row, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                row++;
                ws.Cell(row, 1).Value = "Nhân viên";
                ws.Cell(row, 2).Value = "Tổng ca";
                ws.Cell(row, 3).Value = "Tổng giờ";
                ws.Range(row, 1, row, 3).Style.Font.Bold = true;
                ws.Range(row, 1, row, 3).Style.Fill.BackgroundColor = XLColor.LightGray;

                row++;
                var empStats = _scheduleData
                    .GroupBy(s => s.NhanVien?.TenNv ?? "N/A")
                    .Select(g => new
                    {
                        Name = g.Key,
                        TotalShifts = g.Count(),
                        TotalHours = g.Sum(s => {
                            var hours = s.GioKetThuc.Hour - s.GioBatDau.Hour;
                            if (hours < 0) hours += 24;
                            return hours;
                        })
                    })
                    .OrderByDescending(x => x.TotalShifts)
                    .ToList();

                foreach (var stat in empStats)
                {
                    ws.Cell(row, 1).Value = stat.Name;
                    ws.Cell(row, 2).Value = stat.TotalShifts;
                    ws.Cell(row, 3).Value = stat.TotalHours;
                    row++;
                }

                workbook.SaveAs(filePath);
            }
        }
        #endregion
    }

    #region Helper Classes
    public class EmployeeCheckItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DAL.Entities.NhanVien Employee { get; set; }
        public override string ToString() => Name;
    }

    public class ShiftInfo
    {
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public Color Color { get; set; }
        public string DisplayName { get; set; }

        public ShiftInfo(TimeOnly startTime, TimeOnly endTime, Color color, string displayName)
        {
            StartTime = startTime;
            EndTime = endTime;
            Color = color;
            DisplayName = displayName;
        }
    }
    #endregion
}