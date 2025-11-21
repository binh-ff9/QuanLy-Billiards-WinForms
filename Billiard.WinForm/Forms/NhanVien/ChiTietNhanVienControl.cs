using Billiard.BLL.Services.NhanVienService;
using Billiard.DAL.Entities;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.NhanVien
{
    public partial class ChiTietNhanVienControl : UserControl
    {
        private readonly NhanVienService _nhanVienService;
        private DAL.Entities.NhanVien _nhanVien;
        private readonly int _currentUserId;
        private readonly string _currentUserRole;
        private Panel pnlContent;
        private bool _isLoading = false;

        public event EventHandler OnDataChanged;
        public event EventHandler OnEditClicked;
        public event EventHandler OnDeleted;

        public ChiTietNhanVienControl(NhanVienService nhanVienService, DAL.Entities.NhanVien nhanVien, int currentUserId, string currentUserRole)
        {
            _nhanVienService = nhanVienService;
            _nhanVien = nhanVien;
            _currentUserId = currentUserId;
            _currentUserRole = currentUserRole;

            InitializeComponent();
            InitializeLayout();
        }

        private void InitializeLayout()
        {
            pnlContent = new Panel
            {
                AutoScroll = true,
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20),
                AutoSize = false
            };

            this.Controls.Add(pnlContent);
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
                this.Cursor = Cursors.WaitCursor;
                pnlContent.Controls.Clear();

                // Reload data
                _nhanVien = _nhanVienService.GetEmployeeById(_nhanVien.MaNv);
                if (_nhanVien == null)
                {
                    ShowError("Không tìm thấy thông tin nhân viên");
                    return;
                }

                int yPos = 0;
                int panelWidth = pnlContent.ClientSize.Width - 40;

                // Header
                yPos = AddHeader(yPos, panelWidth);

                // Thông tin cơ bản
                yPos = AddBasicInfo(yPos, panelWidth);

                // Thông tin lương
                yPos = AddSalaryInfo(yPos, panelWidth);

                // Chấm công tháng này
                yPos = await AddAttendanceInfo(yPos, panelWidth);

                // Bảng lương gần nhất
                yPos = AddLatestSalary(yPos, panelWidth);

                // Action buttons
                if (_currentUserRole == "Admin" || _currentUserRole == "Quản lý")
                {
                    AddActionButtons(yPos, panelWidth);
                }

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
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
                Size = new Size(panelWidth, 120),
                BackColor = Color.White
            };

            // Avatar placeholder
            var pnlAvatar = new Panel
            {
                Location = new Point((panelWidth - 80) / 2, 10),
                Size = new Size(80, 80),
                BackColor = GetRoleColor(_nhanVien.MaNhomNavigation?.TenNhom ?? "")
            };

            var lblInitial = new Label
            {
                Text = _nhanVien.TenNv?.Length > 0 ? _nhanVien.TenNv.Substring(0, 1).ToUpper() : "?",
                Font = new Font("Segoe UI", 32F, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(80, 80),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Load ảnh nếu có
            if (!string.IsNullOrEmpty(_nhanVien.FaceidAnh))
            {
                try
                {
                    var pic = new PictureBox
                    {
                        Size = new Size(80, 80),
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
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 26, 46),
                Location = new Point(0, 95),
                Size = new Size(panelWidth, 30),
                TextAlign = ContentAlignment.TopCenter,
                AutoSize = false
            };

            pnlHeader.Controls.AddRange(new Control[] { pnlAvatar, lblName });

            // Nhóm quyền badge (bên dưới tên)
            var lblRole = new Label
            {
                Text = _nhanVien.MaNhomNavigation?.TenNhom ?? "Nhân viên",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(0, 120),
                Size = new Size(panelWidth, 20),
                TextAlign = ContentAlignment.TopCenter,
                AutoSize = false
            };
            pnlHeader.Controls.Add(lblRole);
            pnlHeader.Height = 145;

            // Trạng thái badge (góc phải)
            var lblStatus = new Label
            {
                Text = _nhanVien.TrangThai == "DangLam" ? "Đang làm việc" : "Nghỉ việc",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = _nhanVien.TrangThai == "DangLam" ? Color.FromArgb(40, 167, 69) : Color.FromArgb(220, 53, 69),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(100, 24),
                Location = new Point(panelWidth - 100, 10)
            };
            pnlHeader.Controls.Add(lblStatus);

            pnlContent.Controls.Add(pnlHeader);

            // Separator
            var separator = new Panel
            {
                Location = new Point(0, yPos + 150),
                Size = new Size(panelWidth, 1),
                BackColor = Color.FromArgb(233, 236, 239)
            };
            pnlContent.Controls.Add(separator);

            return yPos + 165;
        }
        #endregion

        #region Basic Info
        private int AddBasicInfo(int yPos, int panelWidth)
        {
            var pnlBasic = CreateCard(panelWidth, Color.FromArgb(248, 249, 250));
            pnlBasic.Location = new Point(0, yPos);

            int cardYPos = 15;

            // Title
            var lblTitle = new Label
            {
                Text = "📋 Thông tin cơ bản",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 26, 46),
                Location = new Point(15, cardYPos),
                AutoSize = true
            };
            pnlBasic.Controls.Add(lblTitle);
            cardYPos += 35;

            // Info rows
            cardYPos = AddInfoRow(pnlBasic, "Số điện thoại:", _nhanVien.Sdt ?? "-", cardYPos, panelWidth);
            cardYPos = AddInfoRow(pnlBasic, "Email:", _nhanVien.Email ?? "Chưa cập nhật", cardYPos, panelWidth);
            cardYPos = AddInfoRow(pnlBasic, "Ca làm việc:", GetShiftText(_nhanVien.CaMacDinh), cardYPos, panelWidth);

            pnlBasic.Height = cardYPos + 15;
            pnlContent.Controls.Add(pnlBasic);

            return yPos + cardYPos + 30;
        }
        #endregion

        #region Salary Info
        private int AddSalaryInfo(int yPos, int panelWidth)
        {
            var pnlSalary = CreateCard(panelWidth, Color.FromArgb(254, 249, 195));
            pnlSalary.Location = new Point(0, yPos);

            int cardYPos = 15;

            // Title
            var lblTitle = new Label
            {
                Text = "💰 Thông tin lương",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 26, 46),
                Location = new Point(15, cardYPos),
                AutoSize = true
            };
            pnlSalary.Controls.Add(lblTitle);
            cardYPos += 35;

            // Salary rows
            cardYPos = AddInfoRow(pnlSalary, "Lương cơ bản:", $"{(_nhanVien.LuongCoBan ?? 0):N0}đ", cardYPos, panelWidth, false, Color.FromArgb(40, 167, 69));
            cardYPos = AddInfoRow(pnlSalary, "Phụ cấp:", $"{(_nhanVien.PhuCap ?? 0):N0}đ", cardYPos, panelWidth, false, Color.FromArgb(255, 193, 7));

            // Separator
            var separator = new Panel
            {
                Location = new Point(15, cardYPos),
                Size = new Size(panelWidth - 30, 1),
                BackColor = Color.FromArgb(226, 232, 240)
            };
            pnlSalary.Controls.Add(separator);
            cardYPos += 10;

            // Total
            var tongLuong = (_nhanVien.LuongCoBan ?? 0) + (_nhanVien.PhuCap ?? 0);
            cardYPos = AddInfoRow(pnlSalary, "Tổng:", $"{tongLuong:N0}đ", cardYPos, panelWidth, true, Color.FromArgb(220, 38, 38));

            pnlSalary.Height = cardYPos + 15;
            pnlContent.Controls.Add(pnlSalary);

            return yPos + cardYPos + 30;
        }
        #endregion

        #region Attendance Info
        private async Task<int> AddAttendanceInfo(int yPos, int panelWidth)
        {
            var pnlAttendance = CreateCard(panelWidth, Color.FromArgb(240, 253, 244));
            pnlAttendance.Location = new Point(0, yPos);

            int cardYPos = 15;

            // Title
            var lblTitle = new Label
            {
                Text = $"📊 Chấm công tháng {DateTime.Now.Month}/{DateTime.Now.Year}",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 26, 46),
                Location = new Point(15, cardYPos),
                AutoSize = true
            };
            pnlAttendance.Controls.Add(lblTitle);
            cardYPos += 35;

            // Get stats
            var stats = _nhanVienService.GetMonthlyStats(_nhanVien.MaNv, DateTime.Now.Month, DateTime.Now.Year);

            cardYPos = AddInfoRow(pnlAttendance, "Số ngày làm:", $"{stats.totalDays} ngày", cardYPos, panelWidth);
            cardYPos = AddInfoRow(pnlAttendance, "Tổng giờ làm:", $"{stats.totalHours:F2} giờ", cardYPos, panelWidth, false, Color.FromArgb(102, 126, 234));
            cardYPos = AddInfoRow(pnlAttendance, "Số lần đi trễ:", $"{stats.lateDays} lần", cardYPos, panelWidth, false, Color.FromArgb(220, 53, 69));

            // View detail button
            var btnViewAttendance = new Button
            {
                Text = "📅 Xem chi tiết chấm công",
                Location = new Point(15, cardYPos + 10),
                Size = new Size(panelWidth - 30, 35),
                BackColor = Color.FromArgb(233, 236, 239),
                ForeColor = Color.FromArgb(30, 41, 59),
                Font = new Font("Segoe UI", 9F),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnViewAttendance.FlatAppearance.BorderSize = 0;
            btnViewAttendance.Click += BtnViewAttendance_Click;
            pnlAttendance.Controls.Add(btnViewAttendance);
            cardYPos += 50;

            pnlAttendance.Height = cardYPos + 15;
            pnlContent.Controls.Add(pnlAttendance);

            return yPos + cardYPos + 30;
        }
        #endregion

        #region Latest Salary
        private int AddLatestSalary(int yPos, int panelWidth)
        {
            var bangLuong = _nhanVienService.GetLatestSalary(_nhanVien.MaNv);
            if (bangLuong == null) return yPos;

            var pnlBangLuong = CreateCard(panelWidth, Color.FromArgb(248, 250, 252));
            pnlBangLuong.Location = new Point(0, yPos);

            int cardYPos = 15;

            // Title
            var lblTitle = new Label
            {
                Text = $"💵 Bảng lương {bangLuong.Thang}/{bangLuong.Nam}",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 26, 46),
                Location = new Point(15, cardYPos),
                AutoSize = true
            };
            pnlBangLuong.Controls.Add(lblTitle);
            cardYPos += 35;

            // Salary details
            cardYPos = AddInfoRow(pnlBangLuong, "Lương cơ bản:", $"{bangLuong.LuongCoBan:N0}đ", cardYPos, panelWidth);
            cardYPos = AddInfoRow(pnlBangLuong, "Phụ cấp:", $"{bangLuong.PhuCap:N0}đ", cardYPos, panelWidth);
            cardYPos = AddInfoRow(pnlBangLuong, "Thưởng:", $"+{bangLuong.Thuong:N0}đ", cardYPos, panelWidth, false, Color.FromArgb(40, 167, 69));
            cardYPos = AddInfoRow(pnlBangLuong, "Phạt:", $"-{bangLuong.Phat:N0}đ", cardYPos, panelWidth, false, Color.FromArgb(220, 53, 69));

            // Separator
            var separator = new Panel
            {
                Location = new Point(15, cardYPos + 5),
                Size = new Size(panelWidth - 30, 2),
                BackColor = Color.FromArgb(222, 226, 230)
            };
            pnlBangLuong.Controls.Add(separator);
            cardYPos += 15;

            // Total
            cardYPos = AddInfoRow(pnlBangLuong, "Tổng lương:", $"{bangLuong.TongLuong:N0}đ", cardYPos, panelWidth, true, Color.FromArgb(220, 38, 38));

            pnlBangLuong.Height = cardYPos + 15;
            pnlContent.Controls.Add(pnlBangLuong);

            return yPos + cardYPos + 30;
        }
        #endregion

        #region Action Buttons
        private void AddActionButtons(int yPos, int panelWidth)
        {
            yPos += 20;

            // Edit button
            var btnEdit = CreateActionButton("✏️ Chỉnh sửa", Color.FromArgb(102, 126, 234), panelWidth);
            btnEdit.Location = new Point(0, yPos);
            btnEdit.Click += BtnEdit_Click;
            pnlContent.Controls.Add(btnEdit);
            yPos += 55;

            // History button
            var btnHistory = CreateActionButton("📜 Xem lịch sử hoạt động", Color.FromArgb(108, 117, 125), panelWidth);
            btnHistory.Location = new Point(0, yPos);
            btnHistory.Click += BtnHistory_Click;
            pnlContent.Controls.Add(btnHistory);
        }
        #endregion

        #region Helper Methods
        private Panel CreateCard(int width, Color bgColor)
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
                using (var pen = new Pen(Color.FromArgb(226, 232, 240), 1))
                {
                    e.Graphics.DrawRectangle(pen, rect);
                }
            };

            return card;
        }

        private int AddInfoRow(Panel panel, string label, string value, int yPos, int panelWidth, bool bold = false, Color? valueColor = null)
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

            rowPanel.Controls.AddRange(new Control[] { lblLabel, lblValue });
            panel.Controls.Add(rowPanel);

            return yPos + 30;
        }

        private Button CreateActionButton(string text, Color backColor, int panelWidth)
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

        private Color GetRoleColor(string role)
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

        private string GetShiftText(string shift)
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

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion

        #region Event Handlers
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            var editForm = new EditNhanVienForm(_nhanVien.MaNv, _currentUserId);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                OnDataChanged?.Invoke(this, EventArgs.Empty);
                _ = LoadNhanVienDetail();
            }
        }

        private void BtnHistory_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Xem lịch sử hoạt động của {_nhanVien.TenNv}\n(Chức năng đang phát triển)",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnViewAttendance_Click(object sender, EventArgs e)
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