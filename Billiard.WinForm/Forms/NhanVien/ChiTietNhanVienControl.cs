using Billiard.BLL.Services.NhanVienService;
using Billiard.DAL.Entities;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.NhanVien
{
    public partial class ChiTietNhanVienControl : UserControl
    {
        private readonly NhanVienService _nhanVienService;
        private readonly LichLamViecService _lichLamViecService;
        private DAL.Entities.NhanVien _nhanVien;
        private readonly int _currentUserId;
        private readonly string _currentUserRole;
        private FlowLayoutPanel flowLayout = null!;
        private bool _isLoading = false;

        public event EventHandler? OnDataChanged;
        public event EventHandler? OnEditClicked;
        public event EventHandler? OnDeleted;

        public ChiTietNhanVienControl(NhanVienService nhanVienService, DAL.Entities.NhanVien nhanVien, int currentUserId, string currentUserRole)
        {
            _nhanVienService = nhanVienService;
            _lichLamViecService = new LichLamViecService();
            _nhanVien = nhanVien;
            _currentUserId = currentUserId;
            _currentUserRole = currentUserRole;

            InitializeComponent();
            InitializeLayout();
        }

        private void InitializeLayout()
        {
            // Sử dụng FlowLayoutPanel để tự động grid các card
            flowLayout = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.White,
                Padding = new Padding(20),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = false
            };

            Controls.Add(flowLayout);
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            await LoadNhanVienDetail();
        }

        public async Task LoadNhanVienDetail()
        {
            if (_isLoading) return;

            _isLoading = true;
            try
            {
                Cursor = Cursors.WaitCursor;
                flowLayout.Controls.Clear();

                // Reload data
                var nhanVien = _nhanVienService.GetEmployeeById(_nhanVien.MaNv);
                if (nhanVien == null)
                {
                    ShowError("Không tìm thấy thông tin nhân viên");
                    return;
                }
                _nhanVien = nhanVien;

                // Tính toán chiều rộng card dựa trên chiều rộng control
                int cardWidth = 410;

                // Header
                flowLayout.Controls.Add(CreateHeader(cardWidth));

                // Separator
                flowLayout.Controls.Add(CreateSeparator(cardWidth));

                // Thông tin cơ bản
                flowLayout.Controls.Add(CreateBasicInfo(cardWidth));

                // Thông tin lương
                flowLayout.Controls.Add(CreateSalaryInfo(cardWidth));

                // Chấm công tháng này
                var attendancePanel = await CreateAttendanceInfo(cardWidth);
                flowLayout.Controls.Add(attendancePanel);

                // Lịch làm việc tháng này
                var schedulePanel = await CreateScheduleInfo(cardWidth);
                flowLayout.Controls.Add(schedulePanel);

                // Bảng lương gần nhất
                var salaryPanel = CreateLatestSalary(cardWidth);
                if (salaryPanel != null)
                    flowLayout.Controls.Add(salaryPanel);

                // Action buttons
                if (_currentUserRole == "Admin" || _currentUserRole == "Quản lý")
                {
                    flowLayout.Controls.Add(CreateActionButtons(cardWidth));
                }

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                ShowError($"Lỗi khi tải dữ liệu: {ex.Message}");
            }
            finally
            {
                _isLoading = false;
            }
        }

        #region Header
        private Panel CreateHeader(int width)
        {
            var pnlHeader = new Panel
            {
                Width = width,
                Height = 170,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 0, 15)
            };

            // Avatar placeholder
            var pnlAvatar = new Panel
            {
                Location = new Point((width - 80) / 2, 10),
                Size = new Size(70, 70),
                BackColor = GetRoleColor(_nhanVien.MaNhomNavigation?.TenNhom)
            };

            var lblInitial = new Label
            {
                Text = !string.IsNullOrEmpty(_nhanVien.TenNv) ? _nhanVien.TenNv[..1].ToUpper() : "?",
                Font = new Font("Segoe UI", 32F, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(70, 70),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Load ảnh nếu có
            if (!string.IsNullOrEmpty(_nhanVien.FaceidAnh))
            {
                try
                {
                    var pic = new PictureBox
                    {
                        Size = new Size(70, 70),
                        Image = Image.FromFile($"asset/img/{_nhanVien.FaceidAnh}"),
                        SizeMode = PictureBoxSizeMode.Zoom
                    };
                    pnlAvatar.Controls.Add(pic);
                }
                catch
                {
                    pnlAvatar.Controls.Add(lblInitial);
                }
            }
            else
            {
                pnlAvatar.Controls.Add(lblInitial);
            }

            // Tên nhân viên
            var lblName = new Label
            {
                Text = _nhanVien.TenNv,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 26, 46),
                Location = new Point(0, 85),
                Size = new Size(width, 45),
                TextAlign = ContentAlignment.TopCenter,
                AutoSize = false
            };

            // Nhóm quyền badge (bên dưới tên)
            var lblRole = new Label
            {
                Text = _nhanVien.MaNhomNavigation?.TenNhom ?? "Nhân viên",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(0, 125),
                Size = new Size(width, 20),
                TextAlign = ContentAlignment.TopCenter,
                AutoSize = false
            };

            // Trạng thái badge (góc phải) - Sửa lại để kiểm tra đúng giá trị
            string statusText = "Nghỉ việc";
            Color statusColor = Color.FromArgb(220, 53, 69);

            if (!string.IsNullOrEmpty(_nhanVien.TrangThai))
            {
                // Kiểm tra nhiều biến thể của trạng thái
                var trangThai = _nhanVien.TrangThai.Trim().ToLower();
                if (trangThai == "đang làm" || trangThai == "danglam" || trangThai == "đanglàm")
                {
                    statusText = "Đang làm việc";
                    statusColor = Color.FromArgb(40, 167, 69);
                }
                else if (trangThai == "nghỉ" || trangThai == "nghi" || trangThai == "nghỉ việc" || trangThai == "nghiviec")
                {
                    statusText = "Nghỉ việc";
                    statusColor = Color.FromArgb(220, 53, 69);
                }
            }

            var lblStatus = new Label
            {
                Text = statusText,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = statusColor,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(100, 24),
                Location = new Point(width - 100, 10)
            };

            pnlHeader.Controls.AddRange([pnlAvatar, lblName, lblRole, lblStatus]);

            return pnlHeader;
        }

        private static Panel CreateSeparator(int width)
        {
            return new Panel
            {
                Width = width,
                Height = 1,
                BackColor = Color.FromArgb(233, 236, 239),
                Margin = new Padding(0, 0, 0, 15)
            };
        }
        #endregion

        #region Basic Info
        private Panel CreateBasicInfo(int width)
        {
            var pnlBasic = CreateCard(width, Color.FromArgb(248, 249, 250));

            int yPos = 15;

            // Title
            var lblTitle = new Label
            {
                Text = "👤 Thông tin cơ bản",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 26, 46),
                Location = new Point(15, yPos),
                AutoSize = true
            };
            pnlBasic.Controls.Add(lblTitle);
            yPos += 35;

            // Mã nhân viên
            yPos = AddInfoRow(pnlBasic, "Mã NV:", $"#{_nhanVien.MaNv}", yPos, width);

            // Số điện thoại
            yPos = AddInfoRow(pnlBasic, "Số điện thoại:", _nhanVien.Sdt ?? "Chưa cập nhật", yPos, width);

            // Email
            yPos = AddInfoRow(pnlBasic, "Email:", string.IsNullOrEmpty(_nhanVien.Email) ? "Chưa cập nhật" : _nhanVien.Email, yPos, width);

            // Ca mặc định
            yPos = AddInfoRow(pnlBasic, "Ca mặc định:", GetShiftText(_nhanVien.CaMacDinh ?? "Sang"), yPos, width);

            pnlBasic.Height = yPos + 15;
            pnlBasic.Margin = new Padding(0, 0, 0, 15);

            return pnlBasic;
        }
        #endregion

        #region Salary Info
        private Panel CreateSalaryInfo(int width)
        {
            var pnlSalary = CreateCard(width, Color.FromArgb(240, 249, 255));

            int yPos = 15;

            // Title
            var lblTitle = new Label
            {
                Text = "💰 Thông tin lương",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 26, 46),
                Location = new Point(15, yPos),
                AutoSize = true
            };
            pnlSalary.Controls.Add(lblTitle);
            yPos += 35;

            // Lương cơ bản
            yPos = AddInfoRow(pnlSalary, "Lương cơ bản:", $"{_nhanVien.LuongCoBan ?? 0:N0}đ", yPos, width);

            // Phụ cấp
            yPos = AddInfoRow(pnlSalary, "Phụ cấp:", $"{_nhanVien.PhuCap ?? 0:N0}đ", yPos, width);

            pnlSalary.Height = yPos + 15;
            pnlSalary.Margin = new Padding(0, 0, 0, 15);

            return pnlSalary;
        }
        #endregion

        #region Attendance Info
        private async Task<Panel> CreateAttendanceInfo(int width)
        {
            var pnlAttendance = CreateCard(width, Color.FromArgb(255, 251, 235));

            int yPos = 15;

            // Title
            var lblTitle = new Label
            {
                Text = $"📅 Chấm công tháng {DateTime.Now:MM/yyyy}",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 26, 46),
                Location = new Point(15, yPos),
                AutoSize = true
            };
            pnlAttendance.Controls.Add(lblTitle);
            yPos += 35;

            // Get stats
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            // Sửa lại cách gọi method để lấy stats
            var stats = _nhanVienService.GetMonthlyStats(_nhanVien.MaNv, currentMonth, currentYear);

            // Số ngày công
            yPos = AddInfoRow(pnlAttendance, "Số ngày công:", $"{stats.totalDays} ngày", yPos, width);

            // Tổng giờ làm
            yPos = AddInfoRow(pnlAttendance, "Tổng giờ làm:", $"{stats.totalHours:F1} giờ", yPos, width);

            // Số ngày đi trễ
            yPos = AddInfoRow(pnlAttendance, "Số ngày đi trễ:", $"{stats.lateDays} ngày", yPos, width, false, stats.lateDays > 0 ? Color.FromArgb(220, 53, 69) : Color.FromArgb(40, 167, 69));

            // Button xem chi tiết
            var btnViewAttendance = CreateActionButton("📊 Xem chi tiết chấm công", Color.FromArgb(52, 152, 219), width - 30);
            btnViewAttendance.Location = new Point(15, yPos + 10);
            btnViewAttendance.Click += BtnViewAttendance_Click;
            pnlAttendance.Controls.Add(btnViewAttendance);
            yPos += 60;

            pnlAttendance.Height = yPos + 15;
            pnlAttendance.Margin = new Padding(0, 0, 0, 15);

            return await Task.FromResult(pnlAttendance);
        }
        #endregion

        #region Schedule Info
        private async Task<Panel> CreateScheduleInfo(int width)
        {
            var pnlSchedule = CreateCard(width, Color.FromArgb(243, 244, 246));

            int yPos = 15;

            // Title
            var lblTitle = new Label
            {
                Text = $"📆 Lịch làm việc tháng {DateTime.Now:MM/yyyy}",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 26, 46),
                Location = new Point(15, yPos),
                AutoSize = true
            };
            pnlSchedule.Controls.Add(lblTitle);
            yPos += 35;

            try
            {
                // Lấy thống kê lịch làm việc từ LichLamViecService
                var currentMonth = DateTime.Now.Month;
                var currentYear = DateTime.Now.Year;

                var scheduleStats = _lichLamViecService.GetMonthlyStats(_nhanVien.MaNv, currentMonth, currentYear);

                // Số ca làm việc
                yPos = AddInfoRow(pnlSchedule, "Số ca đã xếp:", $"{scheduleStats.totalShifts} ca", yPos, width);

                // Tổng giờ trong lịch
                yPos = AddInfoRow(pnlSchedule, "Tổng giờ dự kiến:", $"{scheduleStats.totalHours:F1} giờ", yPos, width);

                // Lấy số ca trong tuần này
                var weekStart = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
                var weekSchedules = _lichLamViecService.GetScheduleByEmployee(_nhanVien.MaNv, currentMonth, currentYear)
                    .Where(s => s.Ngay >= DateOnly.FromDateTime(weekStart) && s.Ngay < DateOnly.FromDateTime(weekStart.AddDays(7)))
                    .Count();

                yPos = AddInfoRow(pnlSchedule, "Số ca tuần này:", $"{weekSchedules} ca", yPos, width);
            }
            catch (Exception ex)
            {
                var lblError = new Label
                {
                    Text = "Không thể tải thông tin lịch làm việc",
                    Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                    ForeColor = Color.FromArgb(220, 53, 69),
                    Location = new Point(15, yPos),
                    Size = new Size(width - 30, 25),
                    AutoSize = false
                };
                pnlSchedule.Controls.Add(lblError);
                yPos += 30;
            }

            pnlSchedule.Height = yPos + 15;
            pnlSchedule.Margin = new Padding(0, 0, 0, 15);

            return await Task.FromResult(pnlSchedule);
        }
        #endregion

        #region Latest Salary
        private Panel? CreateLatestSalary(int width)
        {
            var bangLuong = _nhanVienService.GetLatestSalary(_nhanVien.MaNv);
            if (bangLuong == null) return null;

            var pnlBangLuong = CreateCard(width, Color.FromArgb(248, 250, 252));

            int yPos = 15;

            // Title
            var lblTitle = new Label
            {
                Text = $"💵 Bảng lương {bangLuong.Thang}/{bangLuong.Nam}",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 26, 46),
                Location = new Point(15, yPos),
                AutoSize = true
            };
            pnlBangLuong.Controls.Add(lblTitle);
            yPos += 35;

            // Salary details
            yPos = AddInfoRow(pnlBangLuong, "Lương cơ bản:", $"{bangLuong.LuongCoBan:N0}đ", yPos, width);
            yPos = AddInfoRow(pnlBangLuong, "Phụ cấp:", $"{bangLuong.PhuCap:N0}đ", yPos, width);
            yPos = AddInfoRow(pnlBangLuong, "Tổng giờ:", $"{bangLuong.TongGio:F1} giờ", yPos, width);
            yPos = AddInfoRow(pnlBangLuong, "Thưởng:", $"+{bangLuong.Thuong:N0}đ", yPos, width, false, Color.FromArgb(40, 167, 69));
            yPos = AddInfoRow(pnlBangLuong, "Phạt:", $"-{bangLuong.Phat:N0}đ", yPos, width, false, Color.FromArgb(220, 53, 69));

            // Separator
            var separator = new Panel
            {
                Location = new Point(15, yPos + 5),
                Size = new Size(width - 30, 2),
                BackColor = Color.FromArgb(222, 226, 230)
            };
            pnlBangLuong.Controls.Add(separator);
            yPos += 15;

            // Total
            yPos = AddInfoRow(pnlBangLuong, "Tổng lương:", $"{bangLuong.TongLuong:N0}đ", yPos, width, true, Color.FromArgb(220, 38, 38));

            pnlBangLuong.Height = yPos + 15;
            pnlBangLuong.Margin = new Padding(0, 0, 0, 15);

            return pnlBangLuong;
        }
        #endregion

        #region Action Buttons
        private Panel CreateActionButtons(int width)
        {
            var pnlButtons = new Panel
            {
                Width = width,
                Height = 110,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 15, 0, 20)
            };

            // Edit button
            var btnEdit = CreateActionButton("✏️ Chỉnh sửa", Color.FromArgb(102, 126, 234), width);
            btnEdit.Location = new Point(0, 0);
            btnEdit.Click += BtnEdit_Click;
            pnlButtons.Controls.Add(btnEdit);

            // History button
            var btnHistory = CreateActionButton("📜 Xem lịch sử hoạt động", Color.FromArgb(108, 117, 125), width);
            btnHistory.Location = new Point(0, 55);
            btnHistory.Click += BtnHistory_Click;
            pnlButtons.Controls.Add(btnHistory);

            return pnlButtons;
        }
        #endregion

        #region Helper Methods
        private static Panel CreateCard(int width, Color bgColor)
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
                using var pen = new Pen(Color.FromArgb(226, 232, 240), 1);
                e.Graphics.DrawRectangle(pen, rect);
            };

            return card;
        }

        private static int AddInfoRow(Panel panel, string label, string value, int yPos, int panelWidth, bool bold = false, Color? valueColor = null)
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
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(73, 80, 87),
                Location = new Point(0, 5),
                AutoSize = true
            };

            var lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", bold ? 10F : 9.5F, bold ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = valueColor ?? Color.FromArgb(30, 41, 59),
                AutoSize = false,
                Size = new Size((panelWidth - 30) / 2, 25),
                Location = new Point((panelWidth - 30) / 2, 5),
                TextAlign = ContentAlignment.MiddleRight
            };

            rowPanel.Controls.AddRange([lblLabel, lblValue]);
            panel.Controls.Add(rowPanel);

            return yPos + 30;
        }

        private static Button CreateActionButton(string text, Color backColor, int panelWidth)
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
                Cursor = Cursors.Hand
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

        private static Color GetRoleColor(string? role)
        {
            return role switch
            {
                "Admin" => Color.FromArgb(220, 53, 69),
                "Quản lý" => Color.FromArgb(255, 193, 7),
                "Thu ngân" => Color.FromArgb(23, 162, 184),
                "Phục vụ" => Color.FromArgb(40, 167, 69),
                _ => Color.Gray
            };
        }

        private static string GetShiftText(string shift)
        {
            return shift switch
            {
                "Sang" => "🌅 Ca sáng",
                "Chieu" => "☀️ Ca chiều",
                "Toi" => "🌙 Ca tối",
                "FullTime" => "⏰ Full time",
                _ => shift
            };
        }

        private static void ShowError(string message)
        {
            MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion

        #region Event Handlers
        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            var editForm = new EditNhanVienForm(_nhanVien.MaNv, _currentUserId);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                OnDataChanged?.Invoke(this, EventArgs.Empty);
                _ = LoadNhanVienDetail();
            }
        }

        private void BtnHistory_Click(object? sender, EventArgs e)
        {
            try
            {
                var historyForm = new ActivityHistoryForm(_nhanVien.MaNv, _nhanVien.TenNv);
                historyForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở lịch sử hoạt động: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnViewAttendance_Click(object? sender, EventArgs e)
        {
            try
            {
                var historyForm = new AttendanceHistoryForm(_nhanVien.MaNv, _nhanVien.TenNv);
                historyForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở lịch sử chấm công: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}