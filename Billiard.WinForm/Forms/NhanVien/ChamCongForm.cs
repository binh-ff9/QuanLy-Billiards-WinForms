using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Billiard.BLL.Services.NhanVienService;
using Billiard.DAL.Entities;

namespace Billiard.WinForm.Forms.NhanVien
{
    public partial class ChamCongForm : Form
    {
        #region Fields
        private readonly NhanVienService _nhanVienService;
        private int _currentUserId;
        private string _currentUserRole;
        private string _currentMethod = "manual"; // manual, faceid, vangtay
        private DAL.Entities.NhanVien _recognizedEmployee;
        private ChamCong _todayAttendance;

        // Controls
        private Panel pnlMethodSelection;
        private Panel pnlManualSection;
        private Panel pnlFaceIDSection;
        private Panel pnlEmployeeInfo;
        private Panel pnlAttendanceStatus;
        private Button btnCheckIn;
        private Button btnCheckOut;
        private TextBox txtSearch;
        private RichTextBox txtGhiChu;
        #endregion

        #region Constructor
        public ChamCongForm()
        {
            InitializeComponent();
            _nhanVienService = new NhanVienService();
            InitializeCustomControls();
        }

        public void SetUserInfo(int userId, string userRole)
        {
            _currentUserId = userId;
            _currentUserRole = userRole;
        }
        #endregion

        #region Initialize UI
        private void InitializeCustomControls()
        {
            this.Text = "🕐 Chấm công nhân viên";
            this.Size = new Size(900, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 249, 250);

            // Main container
            var mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(30),
                AutoScroll = true
            };

            // Header
            var header = CreateHeader();
            mainPanel.Controls.Add(header);

            // Method Selection
            pnlMethodSelection = CreateMethodSelection();
            pnlMethodSelection.Location = new Point(30, 100);
            mainPanel.Controls.Add(pnlMethodSelection);

            // Manual Section (default visible)
            pnlManualSection = CreateManualSection();
            pnlManualSection.Location = new Point(30, 200);
            mainPanel.Controls.Add(pnlManualSection);

            // Face ID Section (hidden by default)
            pnlFaceIDSection = CreateFaceIDSection();
            pnlFaceIDSection.Location = new Point(30, 200);
            pnlFaceIDSection.Visible = false;
            mainPanel.Controls.Add(pnlFaceIDSection);

            // Employee Info
            pnlEmployeeInfo = CreateEmployeeInfoPanel();
            pnlEmployeeInfo.Location = new Point(30, 350);
            pnlEmployeeInfo.Visible = false;
            mainPanel.Controls.Add(pnlEmployeeInfo);

            // Attendance Status
            pnlAttendanceStatus = CreateAttendanceStatusPanel();
            pnlAttendanceStatus.Location = new Point(30, 470);
            pnlAttendanceStatus.Visible = false;
            mainPanel.Controls.Add(pnlAttendanceStatus);

            // Action Buttons
            var actionPanel = CreateActionButtons();
            actionPanel.Location = new Point(30, 570);
            mainPanel.Controls.Add(actionPanel);

            this.Controls.Add(mainPanel);
        }

        private Panel CreateHeader()
        {
            var header = new Panel
            {
                Size = new Size(840, 80),
                Location = new Point(0, 0),
                BackColor = Color.White
            };

            var lblTitle = new Label
            {
                Text = "🕐 Chấm công nhân viên",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 26, 46),
                AutoSize = true,
                Location = new Point(20, 15)
            };

            var lblSubtitle = new Label
            {
                Text = "💡 Chọn phương thức chấm công phù hợp",
                Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                ForeColor = Color.FromArgb(107, 114, 128),
                AutoSize = true,
                Location = new Point(20, 50)
            };

            header.Controls.AddRange(new Control[] { lblTitle, lblSubtitle });
            return header;
        }

        private Panel CreateMethodSelection()
        {
            var panel = new Panel
            {
                Size = new Size(840, 80),
                BackColor = Color.White
            };

            var lblTitle = new Label
            {
                Text = "📋 Phương thức chấm công",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 15)
            };

            var btnManual = new Button
            {
                Text = "✍️ Thủ công",
                Size = new Size(150, 45),
                Location = new Point(20, 45),
                BackColor = Color.FromArgb(102, 126, 234),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Tag = "manual"
            };
            btnManual.FlatAppearance.BorderSize = 0;
            btnManual.Click += MethodButton_Click;

            var btnFaceID = new Button
            {
                Text = "📸 Face ID",
                Size = new Size(150, 45),
                Location = new Point(180, 45),
                BackColor = Color.FromArgb(233, 236, 239),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                Cursor = Cursors.Hand,
                Tag = "faceid"
            };
            btnFaceID.FlatAppearance.BorderSize = 0;
            btnFaceID.Click += MethodButton_Click;

            var btnVanTay = new Button
            {
                Text = "👆 Vân tay",
                Size = new Size(150, 45),
                Location = new Point(340, 45),
                BackColor = Color.FromArgb(233, 236, 239),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                Cursor = Cursors.Hand,
                Tag = "vangtay",
                Enabled = false
            };
            btnVanTay.FlatAppearance.BorderSize = 0;

            panel.Controls.AddRange(new Control[] { lblTitle, btnManual, btnFaceID, btnVanTay });
            return panel;
        }

        private Panel CreateManualSection()
        {
            var panel = new Panel
            {
                Size = new Size(840, 130),
                BackColor = Color.White
            };

            var lblTitle = new Label
            {
                Text = "🔍 Tìm kiếm nhân viên",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 15)
            };

            txtSearch = new TextBox
            {
                Size = new Size(400, 35),
                Location = new Point(20, 45),
                Font = new Font("Segoe UI", 11F),
                PlaceholderText = "Nhập mã NV, số điện thoại hoặc tên nhân viên..."
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;

            var btnSearch = new Button
            {
                Text = "🔍 Tìm",
                Size = new Size(100, 35),
                Location = new Point(430, 45),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += BtnSearch_Click;

            var lblHelp = new Label
            {
                Text = "💡 Nhập ít nhất 3 ký tự để tìm kiếm",
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(20, 90)
            };

            panel.Controls.AddRange(new Control[] { lblTitle, txtSearch, btnSearch, lblHelp });
            return panel;
        }

        private Panel CreateFaceIDSection()
        {
            var panel = new Panel
            {
                Size = new Size(840, 130),
                BackColor = Color.White
            };

            var lblTitle = new Label
            {
                Text = "📸 Nhận diện khuôn mặt",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 15)
            };

            var lblStatus = new Label
            {
                Text = "⚠️ Chức năng Face ID đang được phát triển",
                Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                ForeColor = Color.FromArgb(220, 53, 69),
                AutoSize = true,
                Location = new Point(20, 50)
            };

            var lblInfo = new Label
            {
                Text = "Vui lòng sử dụng phương thức chấm công thủ công",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(20, 80)
            };

            panel.Controls.AddRange(new Control[] { lblTitle, lblStatus, lblInfo });
            return panel;
        }

        private Panel CreateEmployeeInfoPanel()
        {
            var panel = new Panel
            {
                Size = new Size(840, 100),
                BackColor = Color.White
            };

            // Content will be populated when employee is found
            return panel;
        }

        private Panel CreateAttendanceStatusPanel()
        {
            var panel = new Panel
            {
                Size = new Size(840, 80),
                BackColor = Color.White
            };

            // Content will be populated based on attendance status
            return panel;
        }

        private Panel CreateActionButtons()
        {
            var panel = new Panel
            {
                Size = new Size(840, 100),
                BackColor = Color.White
            };

            var lblTitle = new Label
            {
                Text = "📝 Ghi chú (tùy chọn)",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 15)
            };

            txtGhiChu = new RichTextBox
            {
                Size = new Size(400, 60),
                Location = new Point(20, 40),
                Font = new Font("Segoe UI", 9F)
            };

            btnCheckIn = new Button
            {
                Text = "⏰ Check-in",
                Size = new Size(180, 50),
                Location = new Point(450, 30),
                BackColor = Color.FromArgb(34, 197, 94),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnCheckIn.FlatAppearance.BorderSize = 0;
            btnCheckIn.Click += BtnCheckIn_Click;

            btnCheckOut = new Button
            {
                Text = "🚪 Check-out",
                Size = new Size(180, 50),
                Location = new Point(640, 30),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnCheckOut.FlatAppearance.BorderSize = 0;
            btnCheckOut.Click += BtnCheckOut_Click;

            panel.Controls.AddRange(new Control[] { lblTitle, txtGhiChu, btnCheckIn, btnCheckOut });
            return panel;
        }
        #endregion

        #region Event Handlers
        private void MethodButton_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;

            _currentMethod = btn.Tag?.ToString() ?? "manual";

            // Update button styles
            foreach (Control ctrl in pnlMethodSelection.Controls)
            {
                if (ctrl is Button methodBtn && methodBtn.Tag != null)
                {
                    if (methodBtn.Tag.ToString() == _currentMethod)
                    {
                        methodBtn.BackColor = Color.FromArgb(102, 126, 234);
                        methodBtn.ForeColor = Color.White;
                        methodBtn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                    }
                    else
                    {
                        methodBtn.BackColor = Color.FromArgb(233, 236, 239);
                        methodBtn.ForeColor = Color.Black;
                        methodBtn.Font = new Font("Segoe UI", 10F);
                    }
                }
            }

            // Show/hide sections
            pnlManualSection.Visible = _currentMethod == "manual";
            pnlFaceIDSection.Visible = _currentMethod == "faceid";

            // Reset data
            ResetEmployeeData();
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            // Auto-search if input is numeric and has enough digits
            string text = txtSearch.Text.Trim();

            // Check if it's a valid employee ID (numeric) or phone number
            if (text.Length >= 3 && (int.TryParse(text, out _) || text.All(char.IsDigit)))
            {
                // Delay search to avoid too many queries
                System.Threading.Tasks.Task.Delay(500).ContinueWith(_ =>
                {
                    if (txtSearch.Text.Trim() == text) // Still the same text
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            SearchEmployee(text);
                        });
                    }
                });
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                MessageBox.Show("⚠️ Vui lòng nhập từ khóa tìm kiếm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSearch.Focus();
                return;
            }

            SearchEmployee(searchText);
        }

        private void BtnCheckIn_Click(object sender, EventArgs e)
        {
            if (_recognizedEmployee == null)
            {
                MessageBox.Show("❌ Vui lòng tìm nhân viên trước!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SubmitAttendance(true);
        }

        private void BtnCheckOut_Click(object sender, EventArgs e)
        {
            if (_recognizedEmployee == null)
            {
                MessageBox.Show("❌ Vui lòng tìm nhân viên trước!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SubmitAttendance(false);
        }
        #endregion

        #region Business Logic
        private void SearchEmployee(string searchText)
        {
            try
            {
                DAL.Entities.NhanVien employee = null;

                // Try to parse as employee ID
                if (int.TryParse(searchText, out int maNV))
                {
                    employee = _nhanVienService.GetEmployeeById(maNV);
                }

                // If not found, try phone number
                if (employee == null)
                {
                    employee = _nhanVienService.GetEmployeeByPhone(searchText);
                }

                // If still not found, try name search
                if (employee == null)
                {
                    var allEmployees = _nhanVienService.GetAllEmployees();
                    employee = allEmployees.FirstOrDefault(e =>
                        e.TenNv.Contains(searchText, StringComparison.OrdinalIgnoreCase));
                }

                if (employee == null)
                {
                    MessageBox.Show("❌ Không tìm thấy nhân viên!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetEmployeeData();
                    return;
                }

                if (employee.TrangThai != "DangLam")
                {
                    MessageBox.Show("⚠️ Nhân viên này đã nghỉ việc!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ResetEmployeeData();
                    return;
                }

                // Load employee data
                LoadEmployeeData(employee);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi khi tìm kiếm: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadEmployeeData(DAL.Entities.NhanVien employee)
        {
            _recognizedEmployee = employee;
            _todayAttendance = _nhanVienService.GetTodayAttendance(employee.MaNv);

            // Update employee info panel
            UpdateEmployeeInfoPanel();

            // Update attendance status panel
            UpdateAttendanceStatusPanel();

            // Show panels
            pnlEmployeeInfo.Visible = true;
            pnlAttendanceStatus.Visible = true;
        }

        private void UpdateEmployeeInfoPanel()
        {
            pnlEmployeeInfo.Controls.Clear();

            if (_recognizedEmployee == null) return;

            var lblTitle = new Label
            {
                Text = "👤 Thông tin nhân viên",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 15)
            };

            var avatar = new Panel
            {
                Size = new Size(60, 60),
                Location = new Point(20, 45),
                BackColor = Color.FromArgb(102, 126, 234)
            };

            var lblInitial = new Label
            {
                Text = GetInitials(_recognizedEmployee.TenNv),
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(60, 60),
                TextAlign = ContentAlignment.MiddleCenter
            };
            avatar.Controls.Add(lblInitial);

            var lblName = new Label
            {
                Text = _recognizedEmployee.TenNv,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 26, 46),
                AutoSize = true,
                Location = new Point(90, 50)
            };

            var lblRole = new Label
            {
                Text = $"👔 {_recognizedEmployee.MaNhomNavigation?.TenNhom ?? "Nhân viên"} • Ca {_recognizedEmployee.CaMacDinh}",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(90, 75)
            };

            var lblPhone = new Label
            {
                Text = $"📱 {_recognizedEmployee.Sdt}",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(300, 75)
            };

            pnlEmployeeInfo.Controls.AddRange(new Control[]
            {
                lblTitle, avatar, lblName, lblRole, lblPhone
            });
        }

        private void UpdateAttendanceStatusPanel()
        {
            pnlAttendanceStatus.Controls.Clear();

            if (_recognizedEmployee == null) return;

            var lblTitle = new Label
            {
                Text = "📊 Trạng thái chấm công hôm nay",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 15)
            };

            pnlAttendanceStatus.Controls.Add(lblTitle);

            if (_todayAttendance == null || !_todayAttendance.GioVao.HasValue)
            {
                // Chưa check-in
                var statusPanel = new Panel
                {
                    Size = new Size(800, 40),
                    Location = new Point(20, 45),
                    BackColor = Color.FromArgb(255, 243, 205)
                };

                var lblStatus = new Label
                {
                    Text = "⏰ Chưa chấm công hôm nay - Sẵn sàng Check-in",
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(133, 100, 4),
                    Location = new Point(15, 10),
                    AutoSize = true
                };

                statusPanel.Controls.Add(lblStatus);
                pnlAttendanceStatus.Controls.Add(statusPanel);

                btnCheckIn.Enabled = true;
                btnCheckOut.Enabled = false;
            }
            else if (_todayAttendance.GioVao.HasValue && !_todayAttendance.GioRa.HasValue)
            {
                // Đã check-in, chưa check-out
                var statusPanel = new Panel
                {
                    Size = new Size(800, 40),
                    Location = new Point(20, 45),
                    BackColor = Color.FromArgb(212, 237, 218)
                };

                var gioVao = _todayAttendance.GioVao.Value.ToString("HH:mm");
                var workingHours = (DateTime.Now - _todayAttendance.GioVao.Value).TotalHours;

                var lblStatus = new Label
                {
                    Text = $"✅ Đã Check-in lúc {gioVao} • Đang làm việc: {workingHours:F1}h",
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(21, 87, 36),
                    Location = new Point(15, 10),
                    AutoSize = true
                };

                statusPanel.Controls.Add(lblStatus);
                pnlAttendanceStatus.Controls.Add(statusPanel);

                btnCheckIn.Enabled = false;
                btnCheckOut.Enabled = true;
            }
            else
            {
                // Đã hoàn thành
                var statusPanel = new Panel
                {
                    Size = new Size(800, 40),
                    Location = new Point(20, 45),
                    BackColor = Color.FromArgb(209, 231, 221)
                };

                var gioVao = _todayAttendance.GioVao.Value.ToString("HH:mm");
                var gioRa = _todayAttendance.GioRa.Value.ToString("HH:mm");
                var soGio = _todayAttendance.SoGioLam ?? 0;

                var lblStatus = new Label
                {
                    Text = $"✅ Đã hoàn thành: {gioVao} → {gioRa} • Tổng: {soGio:F1}h",
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(16, 109, 74),
                    Location = new Point(15, 10),
                    AutoSize = true
                };

                statusPanel.Controls.Add(lblStatus);
                pnlAttendanceStatus.Controls.Add(statusPanel);

                btnCheckIn.Enabled = false;
                btnCheckOut.Enabled = false;
            }
        }

        private void SubmitAttendance(bool isCheckIn)
        {
            try
            {
                string ghiChu = txtGhiChu.Text.Trim();

                bool success;

                if (isCheckIn)
                {
                    success = _nhanVienService.CheckIn(
                        _recognizedEmployee.MaNv,
                        _currentMethod == "faceid" ? "FaceID" : "ThuCong",
                        ghiChu
                    );
                }
                else
                {
                    success = _nhanVienService.CheckOut(
                        _recognizedEmployee.MaNv,
                        ghiChu
                    );
                }

                if (success)
                {
                    var actionType = isCheckIn ? "Check-in" : "Check-out";
                    var now = DateTime.Now.ToString("HH:mm");

                    MessageBox.Show(
                        $"✅ {actionType} thành công lúc {now}!\n\nNhân viên: {_recognizedEmployee.TenNv}",
                        "Thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    // Log activity
                    _nhanVienService.LogActivity(
                        _recognizedEmployee.MaNv,
                        $"{actionType} thành công",
                        $"{actionType} lúc {now} - Phương thức: {(_currentMethod == "faceid" ? "FaceID" : "Thủ công")}"
                    );

                    // Reload data
                    LoadEmployeeData(_recognizedEmployee);

                    // Clear note
                    txtGhiChu.Clear();
                }
                else
                {
                    MessageBox.Show("❌ Không thể chấm công. Vui lòng thử lại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetEmployeeData()
        {
            _recognizedEmployee = null;
            _todayAttendance = null;

            pnlEmployeeInfo.Visible = false;
            pnlAttendanceStatus.Visible = false;

            btnCheckIn.Enabled = false;
            btnCheckOut.Enabled = false;

            txtGhiChu.Clear();
        }

        private string GetInitials(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "?";

            var parts = name.Trim().Split(' ');
            if (parts.Length >= 2)
            {
                return (parts[0][0].ToString() + parts[^1][0].ToString()).ToUpper();
            }

            return name.Substring(0, Math.Min(2, name.Length)).ToUpper();
        }
        #endregion
    }
}