using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Billiard.BLL.Services.NhanVienService;
using Billiard.DAL.Entities;
using ClosedXML.Excel;
using System.IO;

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
        private Button btnExportExcel;
        private TextBox txtSearch;
        private RichTextBox txtGhiChu;

        // Modern color scheme
        private readonly Color PrimaryColor = Color.FromArgb(79, 70, 229); // Indigo
        private readonly Color SecondaryColor = Color.FromArgb(99, 102, 241); // Light Indigo
        private readonly Color AccentColor = Color.FromArgb(236, 72, 153); // Pink
        private readonly Color SuccessColor = Color.FromArgb(16, 185, 129); // Green
        private readonly Color WarningColor = Color.FromArgb(245, 158, 11); // Orange
        private readonly Color DangerColor = Color.FromArgb(239, 68, 68); // Red
        private readonly Color BackgroundColor = Color.FromArgb(249, 250, 251); // Light Gray
        private readonly Color CardBackground = Color.White;
        private readonly Color TextPrimary = Color.FromArgb(17, 24, 39); // Dark Gray
        private readonly Color TextSecondary = Color.FromArgb(107, 114, 128); // Medium Gray
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
            this.Text = "Chấm Công Nhân Viên";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = BackgroundColor;
            this.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);

            // Main container with better spacing
            var mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(40, 30, 40, 30),
                AutoScroll = true,
                BackColor = BackgroundColor
            };

            // Header with gradient background
            var header = CreateModernHeader();
            mainPanel.Controls.Add(header);

            // Two-column layout
            var leftColumn = new Panel
            {
                Location = new Point(40, 140),
                Size = new Size(520, 580),
                BackColor = Color.Transparent
            };

            var rightColumn = new Panel
            {
                Location = new Point(580, 140),
                Size = new Size(520, 580),
                BackColor = Color.Transparent
            };

            // Left column - Method selection and search
            pnlMethodSelection = CreateModernMethodSelection();
            pnlMethodSelection.Location = new Point(0, 0);
            leftColumn.Controls.Add(pnlMethodSelection);

            pnlManualSection = CreateModernManualSection();
            pnlManualSection.Location = new Point(0, 150);
            leftColumn.Controls.Add(pnlManualSection);

            pnlFaceIDSection = CreateModernFaceIDSection();
            pnlFaceIDSection.Location = new Point(0, 150);
            pnlFaceIDSection.Visible = false;
            leftColumn.Controls.Add(pnlFaceIDSection);

            // Right column - Employee info and actions
            pnlEmployeeInfo = CreateModernEmployeeInfoPanel();
            pnlEmployeeInfo.Location = new Point(0, 0);
            pnlEmployeeInfo.Visible = false;
            rightColumn.Controls.Add(pnlEmployeeInfo);

            pnlAttendanceStatus = CreateModernAttendanceStatusPanel();
            pnlAttendanceStatus.Location = new Point(0, 330);
            pnlAttendanceStatus.Visible = false;
            rightColumn.Controls.Add(pnlAttendanceStatus);

            var actionPanel = CreateModernActionButtons();
            actionPanel.Location = new Point(0, 450);
            rightColumn.Controls.Add(actionPanel);

            mainPanel.Controls.Add(leftColumn);
            mainPanel.Controls.Add(rightColumn);

            this.Controls.Add(mainPanel);
        }

        private Panel CreateModernHeader()
        {
            var header = new Panel
            {
                Size = new Size(1120, 100),
                Location = new Point(0, 0)
            };

            // Gradient background
            header.Paint += (s, e) =>
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    header.ClientRectangle,
                    PrimaryColor,
                    SecondaryColor,
                    LinearGradientMode.Horizontal))
                {
                    e.Graphics.FillRectangle(brush, header.ClientRectangle);
                }
            };

            var lblTitle = new Label
            {
                Text = "⏰ Chấm Công Nhân Viên",
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(30, 20),
                BackColor = Color.Transparent
            };

            var lblSubtitle = new Label
            {
                Text = "Hệ thống chấm công thông minh - Chính xác & Tiện lợi",
                Font = new Font("Segoe UI", 11F, FontStyle.Regular),
                ForeColor = Color.FromArgb(224, 231, 255),
                AutoSize = true,
                Location = new Point(30, 60),
                BackColor = Color.Transparent
            };

            var lblDate = new Label
            {
                Text = DateTime.Now.ToString("dddd, dd/MM/yyyy"),
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                ForeColor = Color.FromArgb(224, 231, 255),
                AutoSize = true,
                Location = new Point(950, 35),
                BackColor = Color.Transparent
            };

            header.Controls.AddRange(new Control[] { lblTitle, lblSubtitle, lblDate });
            return header;
        }

        private Panel CreateModernMethodSelection()
        {
            var panel = CreateCard(520, 130);

            var lblTitle = new Label
            {
                Text = "📋 Phương Thức Chấm Công",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = TextPrimary,
                AutoSize = true,
                Location = new Point(25, 20)
            };

            var btnManual = CreateMethodButton("✍️ Nhập Thủ Công", 25, 60, true);
            btnManual.Tag = "manual";
            btnManual.Click += MethodButton_Click;

            var btnFaceID = CreateMethodButton("👤 Nhận Diện Khuôn Mặt", 275, 60, false);
            btnFaceID.Tag = "faceid";
            btnFaceID.Click += MethodButton_Click;

            panel.Controls.AddRange(new Control[] { lblTitle, btnManual, btnFaceID });
            return panel;
        }

        private Button CreateMethodButton(string text, int x, int y, bool isActive)
        {
            var btn = new Button
            {
                Text = text,
                Size = new Size(220, 50),
                Location = new Point(x, y),
                BackColor = isActive ? PrimaryColor : Color.FromArgb(243, 244, 246),
                ForeColor = isActive ? Color.White : TextSecondary,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = isActive ? SecondaryColor : Color.FromArgb(229, 231, 235);

            // Rounded corners
            btn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btn.Width, btn.Height, 10, 10));

            return btn;
        }

        private Panel CreateModernManualSection()
        {
            var panel = CreateCard(520, 200);

            var lblTitle = new Label
            {
                Text = "🔍 Tìm Kiếm Nhân Viên",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = TextPrimary,
                AutoSize = true,
                Location = new Point(25, 20)
            };

            txtSearch = new TextBox
            {
                Size = new Size(470, 40),
                Location = new Point(25, 60),
                Font = new Font("Segoe UI", 11F),
                PlaceholderText = "Nhập mã NV, số điện thoại hoặc tên nhân viên...",
                BorderStyle = BorderStyle.None
            };

            // Custom border for textbox
            var txtBorder = new Panel
            {
                Location = new Point(25, 60),
                Size = new Size(470, 42),
                BackColor = Color.FromArgb(229, 231, 235)
            };
            txtBorder.Controls.Add(txtSearch);
            txtSearch.Location = new Point(15, 10);
            txtSearch.Size = new Size(440, 22);
            txtSearch.TextChanged += TxtSearch_TextChanged;

            var btnSearch = new Button
            {
                Text = "🔍 Tìm Kiếm",
                Size = new Size(140, 45),
                Location = new Point(25, 120),
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.FlatAppearance.MouseOverBackColor = SecondaryColor;
            btnSearch.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnSearch.Width, btnSearch.Height, 8, 8));
            btnSearch.Click += BtnSearch_Click;

            var lblHint = new Label
            {
                Text = "💡 Nhấn Enter để tìm kiếm nhanh",
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                ForeColor = TextSecondary,
                AutoSize = true,
                Location = new Point(180, 133)
            };

            panel.Controls.AddRange(new Control[] { lblTitle, txtBorder, btnSearch, lblHint });
            return panel;
        }

        private Panel CreateModernFaceIDSection()
        {
            var panel = CreateCard(520, 280);

            var lblTitle = new Label
            {
                Text = "👤 Nhận Diện Khuôn Mặt",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = TextPrimary,
                AutoSize = true,
                Location = new Point(25, 20)
            };

            var picCamera = new PictureBox
            {
                Size = new Size(300, 200),
                Location = new Point(110, 60),
                BackColor = Color.FromArgb(17, 24, 39),
                BorderStyle = BorderStyle.None
            };

            var lblCameraText = new Label
            {
                Text = "📷\n\nĐặt khuôn mặt vào khung hình",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(300, 200),
                Location = new Point(110, 60),
                BackColor = Color.Transparent
            };

            panel.Controls.AddRange(new Control[] { lblTitle, picCamera, lblCameraText });
            return panel;
        }

        private Panel CreateModernEmployeeInfoPanel()
        {
            var panel = CreateCard(520, 310);

            var lblTitle = new Label
            {
                Text = "👤 Thông Tin Nhân Viên",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = TextPrimary,
                AutoSize = true,
                Location = new Point(25, 20)
            };

            // Avatar circle
            var pnlAvatar = new Panel
            {
                Size = new Size(100, 100),
                Location = new Point(210, 60),
                BackColor = PrimaryColor
            };
            pnlAvatar.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (SolidBrush brush = new SolidBrush(PrimaryColor))
                {
                    e.Graphics.FillEllipse(brush, 0, 0, 100, 100);
                }
            };

            var lblAvatar = new Label
            {
                Text = "NV",
                Font = new Font("Segoe UI", 28F, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(100, 100),
                BackColor = Color.Transparent,
                Location = new Point(0, 25)
            };
            pnlAvatar.Controls.Add(lblAvatar);

            var lblEmployeeName = new Label
            {
                Text = "Tên nhân viên",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = TextPrimary,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = true,
                Location = new Point(160, 175)
            };

            var lblEmployeeInfo = new Label
            {
                Text = "Mã NV: --- | SĐT: ---",
                Font = new Font("Segoe UI", 10F),
                ForeColor = TextSecondary,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = true,
                Location = new Point(180, 205)
            };

            var lblNoteTitle = new Label
            {
                Text = "📝 Ghi Chú",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = TextPrimary,
                AutoSize = true,
                Location = new Point(25, 235)
            };

            txtGhiChu = new RichTextBox
            {
                Size = new Size(470, 50),
                Location = new Point(25, 260),
                Font = new Font("Segoe UI", 10F),
                BorderStyle = BorderStyle.FixedSingle
            };

            panel.Controls.AddRange(new Control[] { lblTitle, pnlAvatar, lblEmployeeName,
                lblEmployeeInfo, lblNoteTitle, txtGhiChu });
            return panel;
        }

        private Panel CreateModernAttendanceStatusPanel()
        {
            var panel = CreateCard(520, 100);

            var lblTitle = new Label
            {
                Text = "📊 Trạng Thái Hôm Nay",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = TextPrimary,
                AutoSize = true,
                Location = new Point(25, 20)
            };

            panel.Controls.Add(lblTitle);
            return panel;
        }

        private Panel CreateModernActionButtons()
        {
            var panel = new Panel
            {
                Size = new Size(520, 80),
                BackColor = Color.Transparent
            };

            btnCheckIn = new Button
            {
                Text = "✅ CHECK IN",
                Size = new Size(250, 60),
                Location = new Point(0, 0),
                BackColor = SuccessColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnCheckIn.FlatAppearance.BorderSize = 0;
            btnCheckIn.FlatAppearance.MouseOverBackColor = Color.FromArgb(5, 150, 105);
            btnCheckIn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnCheckIn.Width, btnCheckIn.Height, 10, 10));
            btnCheckIn.Click += BtnCheckIn_Click;

            btnCheckOut = new Button
            {
                Text = "🚪 CHECK OUT",
                Size = new Size(250, 60),
                Location = new Point(270, 0),
                BackColor = DangerColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnCheckOut.FlatAppearance.BorderSize = 0;
            btnCheckOut.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 38, 38);
            btnCheckOut.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnCheckOut.Width, btnCheckOut.Height, 10, 10));
            btnCheckOut.Click += BtnCheckOut_Click;

            panel.Controls.AddRange(new Control[] { btnCheckIn, btnCheckOut });
            return panel;
        }

        private Panel CreateCard(int width, int height)
        {
            var panel = new Panel
            {
                Size = new Size(width, height),
                BackColor = CardBackground
            };

            // Shadow effect
            panel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen shadowPen = new Pen(Color.FromArgb(30, 0, 0, 0), 1))
                {
                    e.Graphics.DrawRectangle(shadowPen, 1, 1, width - 3, height - 3);
                }
            };

            return panel;
        }

        // Win32 API for rounded corners
        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);
        #endregion

        #region Event Handlers
        private void MethodButton_Click(object sender, EventArgs e)
        {
            var clickedButton = sender as Button;
            _currentMethod = clickedButton.Tag.ToString();

            // Reset all method buttons
            foreach (Control control in pnlMethodSelection.Controls)
            {
                if (control is Button btn && btn.Tag != null)
                {
                    if (btn.Tag.ToString() == _currentMethod)
                    {
                        btn.BackColor = PrimaryColor;
                        btn.ForeColor = Color.White;
                    }
                    else
                    {
                        btn.BackColor = Color.FromArgb(243, 244, 246);
                        btn.ForeColor = TextSecondary;
                    }
                }
            }

            // Show/hide sections
            if (_currentMethod == "manual")
            {
                pnlManualSection.Visible = true;
                pnlFaceIDSection.Visible = false;
            }
            else if (_currentMethod == "faceid")
            {
                pnlManualSection.Visible = false;
                pnlFaceIDSection.Visible = true;
                // TODO: Start camera/face recognition
            }

            ResetEmployeeData();
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            // Auto search when typing (debounce can be added)
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                MessageBox.Show("⚠️ Vui lòng nhập thông tin tìm kiếm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var employee = _nhanVienService.SearchEmployee(searchText);

                if (employee == null)
                {
                    MessageBox.Show($"❌ Không tìm thấy nhân viên với thông tin: {searchText}",
                        "Không tìm thấy", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetEmployeeData();
                    return;
                }

                LoadEmployeeData(employee);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi khi tìm kiếm: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCheckIn_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                $"Xác nhận CHECK IN cho nhân viên:\n\n{_recognizedEmployee.TenNv}\nMã NV: {_recognizedEmployee.MaNv}",
                "Xác nhận Check In",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                SubmitAttendance(true);
            }
        }

        private void BtnCheckOut_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                $"Xác nhận CHECK OUT cho nhân viên:\n\n{_recognizedEmployee.TenNv}\nMã NV: {_recognizedEmployee.MaNv}",
                "Xác nhận Check Out",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                SubmitAttendance(false);
            }
        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            using (var dialog = new ExportAttendanceDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    ExportToExcel(dialog.SelectedMonth, dialog.SelectedYear);
                }
            }
        }
        #endregion

        #region Business Logic
        private void LoadEmployeeData(DAL.Entities.NhanVien employee)
        {
            _recognizedEmployee = employee;

            // Update employee info display
            var lblAvatar = pnlEmployeeInfo.Controls.OfType<Panel>().FirstOrDefault()?.Controls.OfType<Label>().FirstOrDefault();
            if (lblAvatar != null)
            {
                lblAvatar.Text = GetInitials(employee.TenNv);
            }

            var lblName = pnlEmployeeInfo.Controls.OfType<Label>().FirstOrDefault(l => l.Font.Size == 14F);
            if (lblName != null)
            {
                lblName.Text = employee.TenNv;
                lblName.Location = new Point((520 - lblName.PreferredWidth) / 2, 175);
            }

            var lblInfo = pnlEmployeeInfo.Controls.OfType<Label>().FirstOrDefault(l => l.Text.Contains("Mã NV"));
            if (lblInfo != null)
            {
                lblInfo.Text = $"Mã NV: {employee.MaNv} | SĐT: {employee.Sdt ?? "N/A"}";
                lblInfo.Location = new Point((520 - lblInfo.PreferredWidth) / 2, 205);
            }

            pnlEmployeeInfo.Visible = true;

            // Load today's attendance
            _todayAttendance = _nhanVienService.GetTodayAttendance(employee.MaNv);
            UpdateAttendanceStatus();

            pnlAttendanceStatus.Visible = true;
        }

        private void UpdateAttendanceStatus()
        {
            // Clear existing status
            var existingStatus = pnlAttendanceStatus.Controls.OfType<Panel>().Where(p => p.Location.Y > 50).ToList();
            foreach (var panel in existingStatus)
            {
                pnlAttendanceStatus.Controls.Remove(panel);
            }

            if (_todayAttendance == null)
            {
                // Chưa check-in
                var statusPanel = new Panel
                {
                    Size = new Size(470, 50),
                    Location = new Point(25, 60),
                    BackColor = Color.FromArgb(254, 243, 199)
                };

                var lblStatus = new Label
                {
                    Text = "⏰ Chưa chấm công hôm nay",
                    Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(180, 83, 9),
                    Location = new Point(15, 15),
                    AutoSize = true
                };

                statusPanel.Controls.Add(lblStatus);
                pnlAttendanceStatus.Controls.Add(statusPanel);

                btnCheckIn.Enabled = true;
                btnCheckOut.Enabled = false;
            }
            else if (_todayAttendance.GioRa == null)
            {
                // Đã check-in, chưa check-out
                var statusPanel = new Panel
                {
                    Size = new Size(470, 50),
                    Location = new Point(25, 60),
                    BackColor = Color.FromArgb(209, 250, 229)
                };

                var gioVao = _todayAttendance.GioVao.Value.ToString("HH:mm");
                var workingHours = (DateTime.Now - _todayAttendance.GioVao.Value).TotalHours;

                var lblStatus = new Label
                {
                    Text = $"✅ Check-in: {gioVao} • Làm việc: {workingHours:F1}h",
                    Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(6, 95, 70),
                    Location = new Point(15, 15),
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
                    Size = new Size(470, 50),
                    Location = new Point(25, 60),
                    BackColor = Color.FromArgb(220, 252, 231)
                };

                var gioVao = _todayAttendance.GioVao.Value.ToString("HH:mm");
                var gioRa = _todayAttendance.GioRa.Value.ToString("HH:mm");
                var soGio = _todayAttendance.SoGioLam ?? 0;

                var lblStatus = new Label
                {
                    Text = $"✅ Hoàn thành: {gioVao} → {gioRa} ({soGio:F1}h)",
                    Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(5, 122, 85),
                    Location = new Point(15, 15),
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
                        $"✅ {actionType} thành công!\n\nThời gian: {now}\nNhân viên: {_recognizedEmployee.TenNv}",
                        "Thành Công",
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

        private void ExportToExcel(int month, int year)
        {
            try
            {
                var data = _nhanVienService.GetAttendanceReport(month, year);

                if (data == null || data.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu chấm công trong tháng này!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add($"Cham_Cong_{month}_{year}");

                    // Headers
                    worksheet.Cell(1, 1).Value = "Mã NV";
                    worksheet.Cell(1, 2).Value = "Tên nhân viên";
                    worksheet.Cell(1, 3).Value = "Ngày";
                    worksheet.Cell(1, 4).Value = "Giờ vào";
                    worksheet.Cell(1, 5).Value = "Giờ ra";
                    worksheet.Cell(1, 6).Value = "Số giờ làm";
                    worksheet.Cell(1, 7).Value = "Ghi chú";

                    // Data
                    int row = 2;
                    foreach (var item in data)
                    {
                        // item is ChamCong (entity) — use its properties and the navigation property for employee name
                        worksheet.Cell(row, 1).Value = item.MaNv;
                        worksheet.Cell(row, 2).Value = item.MaNvNavigation?.TenNv ?? string.Empty;

                        // ChamCong.Ngay is DateOnly; guard nullability if necessary
                        try
                        {
                            worksheet.Cell(row, 3).Value = item.Ngay.ToString("dd/MM/yyyy");
                        }
                        catch
                        {
                            worksheet.Cell(row, 3).Value = string.Empty;
                        }

                        worksheet.Cell(row, 4).Value = item.GioVao?.ToString("HH:mm") ?? string.Empty;
                        worksheet.Cell(row, 5).Value = item.GioRa?.ToString("HH:mm") ?? string.Empty;
                        worksheet.Cell(row, 6).Value = item.SoGioLam ?? 0;
                        worksheet.Cell(row, 7).Value = item.GhiChu ?? string.Empty;
                        row++;
                    }

                    // Auto-fit columns
                    worksheet.Columns().AdjustToContents();

                    // Save dialog
                    using (var sfd = new SaveFileDialog())
                    {
                        sfd.Filter = "Excel Files|*.xlsx";
                        sfd.FileName = $"BaoCaoChamCong_{month}_{year}.xlsx";

                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            workbook.SaveAs(sfd.FileName);
                            MessageBox.Show("✅ Xuất báo cáo thành công!", "Thành công",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi khi xuất Excel: {ex.Message}", "Lỗi",
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

    #region Export Dialog
    public class ExportAttendanceDialog : Form
    {
        public int SelectedMonth { get; private set; }
        public int SelectedYear { get; private set; }

        private ComboBox cboMonth;
        private ComboBox cboYear;

        public ExportAttendanceDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Chọn Tháng Xuất Báo Cáo";
            this.Size = new Size(400, 250);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(249, 250, 251);

            var lblTitle = new Label
            {
                Text = "📊 Xuất Báo Cáo Chấm Công",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(79, 70, 229),
                AutoSize = true,
                Location = new Point(30, 20)
            };

            var lblMonth = new Label
            {
                Text = "Tháng:",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(40, 70),
                AutoSize = true
            };

            cboMonth = new ComboBox
            {
                Location = new Point(130, 68),
                Width = 220,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };
            for (int i = 1; i <= 12; i++)
            {
                cboMonth.Items.Add($"Tháng {i}");
            }
            cboMonth.SelectedIndex = DateTime.Now.Month - 1;

            var lblYear = new Label
            {
                Text = "Năm:",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(40, 110),
                AutoSize = true
            };

            cboYear = new ComboBox
            {
                Location = new Point(130, 108),
                Width = 220,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };
            int currentYear = DateTime.Now.Year;
            for (int i = currentYear - 5; i <= currentYear + 1; i++)
            {
                cboYear.Items.Add(i);
            }
            cboYear.SelectedItem = currentYear;

            var btnOK = new Button
            {
                Text = "✅ Xuất Excel",
                Location = new Point(90, 160),
                Size = new Size(120, 40),
                BackColor = Color.FromArgb(16, 185, 129),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.OK
            };
            btnOK.FlatAppearance.BorderSize = 0;
            btnOK.Click += (s, e) =>
            {
                SelectedMonth = cboMonth.SelectedIndex + 1;
                SelectedYear = (int)cboYear.SelectedItem;
            };

            var btnCancel = new Button
            {
                Text = "❌ Hủy",
                Location = new Point(230, 160),
                Size = new Size(120, 40),
                BackColor = Color.FromArgb(239, 68, 68),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.Cancel
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            this.Controls.AddRange(new Control[] { lblTitle, lblMonth, cboMonth, lblYear, cboYear, btnOK, btnCancel });
            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
        }
    }
    #endregion
}