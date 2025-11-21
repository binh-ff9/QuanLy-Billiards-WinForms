using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Billiard.BLL.Services.NhanVienService;
using Billiard.DAL.Entities;

namespace Billiard.WinForm.Forms.NhanVien
{
    public partial class NhanVienForm : Form
    {
        #region Fields
        private readonly NhanVienService _nhanVienService;
        private List<DAL.Entities.NhanVien> _allEmployees;
        private List<DAL.Entities.NhanVien> _filteredEmployees;
        private string _currentStatusFilter = "all";
        private string _currentRoleFilter = "all";
        private int _currentUserId;
        private string _currentUserRole;

        // [MODIFIED] Thêm tham chiếu đến MainForm và loại bỏ panel detail nội bộ
        private MainForm _mainForm;
        private ChiTietNhanVienControl _currentDetailControl;
        private int _currentSelectedEmployeeId = -1; // Track employee đang được xem
        #endregion

        #region Constructor
        public NhanVienForm(NhanVienService nhanVienService)
        {
            InitializeComponent();
            _nhanVienService = nhanVienService;
            // [REMOVED] Bỏ InitializeDetailPanel()
        }
        #endregion

        // [MODIFIED] Thêm phương thức SetMainForm
        public void SetMainForm(MainForm mainForm)
        {
            _mainForm = mainForm;
        }

        // [REMOVED] Logic InitializeDetailPanel đã được xóa

        public void SetUserInfo(int userId, string userRole)
        {
            _currentUserId = userId;
            _currentUserRole = userRole;
            SetupPermissions();
        }

        #region Form Events
        private void NhanVienForm_Load(object sender, EventArgs e)
        {
            try
            {
                // [MODIFIED] Đảm bảo panel detail trên MainForm bị ẩn khi load form
                _mainForm?.HideDetailPanel();
                LoadEmployees();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // [MODIFIED] Thêm OnFormClosing để ẩn detail panel khi form con đóng
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            HideDetailPanel();
            base.OnFormClosing(e);
        }
        #endregion

        #region Setup Methods
        private void SetupPermissions()
        {
            bool isAdmin = _currentUserRole == "Admin";
            bool isManager = _currentUserRole == "Quản lý" || isAdmin;
            btnAdd.Visible = isManager;
            btnSchedule.Visible = isManager;
        }
        #endregion

        #region Load Data Methods
        private void LoadEmployees()
        {
            try
            {
                _allEmployees = _nhanVienService.GetAllEmployees();
                _filteredEmployees = new List<DAL.Entities.NhanVien>(_allEmployees);
                DisplayEmployees();
            }
            catch (Exception ex)
            {
                throw new Exception($"Không thể tải danh sách nhân viên: {ex.Message}");
            }
        }

        private void DisplayEmployees()
        {
            flowLayoutEmployees.Controls.Clear();
            if (_filteredEmployees == null || !_filteredEmployees.Any())
            {
                ShowEmptyState();
                return;
            }
            foreach (var emp in _filteredEmployees)
            {
                var card = CreateEmployeeCard(emp);
                flowLayoutEmployees.Controls.Add(card);
            }
        }

        private void ShowEmptyState()
        {
            var emptyPanel = new Panel { Size = new Size(flowLayoutEmployees.Width - 40, 300), BackColor = Color.White };
            var lblIcon = new Label { Text = "👥", Font = new Font("Segoe UI", 48F), AutoSize = true, Location = new Point((emptyPanel.Width - 100) / 2, 60) };
            var lblTitle = new Label { Text = "Không có nhân viên nào", Font = new Font("Segoe UI", 16F, FontStyle.Bold), AutoSize = true, Location = new Point((emptyPanel.Width - 250) / 2, 140) };
            var lblText = new Label { Text = "Chưa có nhân viên trong hệ thống", Font = new Font("Segoe UI", 11F), ForeColor = Color.Gray, AutoSize = true, Location = new Point((emptyPanel.Width - 280) / 2, 180) };
            emptyPanel.Controls.AddRange(new Control[] { lblIcon, lblTitle, lblText });
            flowLayoutEmployees.Controls.Add(emptyPanel);
        }
        #endregion

        #region Create Employee Card
        private Panel CreateEmployeeCard(DAL.Entities.NhanVien emp)
        {
            var card = new Panel
            {
                Size = new Size(280, 380),
                BackColor = Color.White,
                Margin = new Padding(10),
                Cursor = Cursors.Hand,
                Tag = emp
            };

            // Hover effects
            card.MouseEnter += (s, e) =>
            {
                if (_currentSelectedEmployeeId != emp.MaNv)
                    card.BackColor = Color.FromArgb(248, 249, 255);
            };
            card.MouseLeave += (s, e) =>
            {
                if (_currentSelectedEmployeeId != emp.MaNv)
                    card.BackColor = Color.White;
            };

            // Click để xem detail
            card.Click += (s, e) => ViewEmployeeDetail(emp);

            // Hàm helper để thêm click handler cho tất cả children
            void AddClickHandlerToChildren(Control parent)
            {
                foreach (Control child in parent.Controls)
                {
                    // Bỏ qua buttons
                    if (child is Button) continue;

                    child.Click += (s, e) => ViewEmployeeDetail(emp);
                    child.Cursor = Cursors.Hand;

                    if (child.HasChildren)
                        AddClickHandlerToChildren(child);
                }
            }

            var imgPanel = new Panel { Size = new Size(280, 200), Location = new Point(0, 0), BackColor = Color.FromArgb(102, 126, 234) };

            // Load ảnh hoặc hiển thị placeholder
            if (!string.IsNullOrEmpty(emp.FaceidAnh))
            {
                try
                {
                    // Lỗi: đường dẫn asset/img/{emp.FaceidAnh} không khả dụng trong môi trường này. 
                    // Tạm thời hiển thị placeholder.
                    // var pic = new PictureBox
                    // {
                    //     Size = new Size(280, 200),
                    //     Image = Image.FromFile($"asset/img/{emp.FaceidAnh}"),
                    //     SizeMode = PictureBoxSizeMode.Zoom,
                    //     Location = new Point(0, 0)
                    // };
                    // pic.Click += (s, e) => ViewEmployeeDetail(emp);
                    // pic.Cursor = Cursors.Hand;
                    // imgPanel.Controls.Add(pic);
                    AddAvatarPlaceholder(imgPanel, emp.TenNv);
                }
                catch
                {
                    AddAvatarPlaceholder(imgPanel, emp.TenNv);
                }
            }
            else
            {
                AddAvatarPlaceholder(imgPanel, emp.TenNv);
            }

            // Status badge
            var statusBadge = new Label
            {
                Text = emp.TrangThai == "Đang làm" ? "Đang làm" : "Nghỉ việc",
                BackColor = emp.TrangThai == "Đang làm" ? Color.FromArgb(40, 167, 69) : Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                AutoSize = true,
                Padding = new Padding(8, 4, 8, 4),
                Location = new Point(10, 10)
            };

            // Role badge
            string roleName = emp.MaNhomNavigation?.TenNhom ?? "Nhân viên";
            var roleBadge = new Label
            {
                Text = $"{GetRoleIcon(roleName)} {roleName}",
                BackColor = GetRoleColor(roleName),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                AutoSize = true,
                Padding = new Padding(8, 4, 8, 4),
                Location = new Point(10, 170)
            };
            imgPanel.Controls.AddRange(new Control[] { statusBadge, roleBadge });

            // Nút Edit - CHỈ HIỂN THỊ CHO ADMIN/QUẢN LÝ
            if (_currentUserRole == "Admin" || _currentUserRole == "Quản lý")
            {
                var btnQuickEdit = new Button
                {
                    Text = "✏️",
                    Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                    Size = new Size(35, 35),
                    Location = new Point(238, 8),
                    BackColor = Color.FromArgb(59, 130, 246),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand,
                    Tag = emp,
                    TabStop = false // Tránh focus khi tab
                };
                btnQuickEdit.FlatAppearance.BorderSize = 0;

                // Hover effect cho nút edit
                btnQuickEdit.MouseEnter += (s, e) => btnQuickEdit.BackColor = Color.FromArgb(37, 99, 235);
                btnQuickEdit.MouseLeave += (s, e) => btnQuickEdit.BackColor = Color.FromArgb(59, 130, 246);

                // Event click cho nút Edit - NGĂN PROPAGATION
                btnQuickEdit.Click += (s, e) =>
                {
                    OpenEditForm(emp);
                };

                imgPanel.Controls.Add(btnQuickEdit);
                btnQuickEdit.BringToFront();
            }

            // Content panel
            var contentPanel = new Panel { Size = new Size(280, 180), Location = new Point(0, 200), BackColor = Color.White };
            var lblName = new Label { Text = emp.TenNv, Font = new Font("Segoe UI", 12F, FontStyle.Bold), Location = new Point(15, 15), Size = new Size(250, 25), ForeColor = Color.FromArgb(26, 26, 46) };
            var lblId = new Label { Text = $"#{emp.MaNv}", Font = new Font("Segoe UI", 9F), Location = new Point(15, 42), ForeColor = Color.Gray, AutoSize = true };
            var lblPhone = new Label { Text = $"📱 {emp.Sdt}", Font = new Font("Segoe UI", 9F), Location = new Point(15, 65), Size = new Size(250, 20) };
            var lblShift = new Label { Text = $"{GetShiftIcon(emp.CaMacDinh)} {emp.CaMacDinh}", BackColor = Color.FromArgb(233, 236, 239), Font = new Font("Segoe UI", 8F), Padding = new Padding(6, 3, 6, 3), AutoSize = true, Location = new Point(15, 95) };
            decimal total = (emp.LuongCoBan ?? 0) + (emp.PhuCap ?? 0);
            var lblSalary = new Label { Text = $"💵 {total:N0}đ", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(102, 126, 234), Location = new Point(15, 130), Size = new Size(250, 25) };

            contentPanel.Controls.AddRange(new Control[] { lblName, lblId, lblPhone, lblShift, lblSalary });
            card.Controls.AddRange(new Control[] { imgPanel, contentPanel });

            // Áp dụng click handler cho tất cả children
            AddClickHandlerToChildren(card);

            // Highlight card nếu đang được chọn
            if (_currentSelectedEmployeeId == emp.MaNv)
            {
                card.BackColor = Color.FromArgb(224, 231, 255);
            }

            return card;
        }

        private void AddAvatarPlaceholder(Panel panel, string name)
        {
            var initial = name?.Length > 0 ? name.Substring(0, 1).ToUpper() : "?";
            var lbl = new Label
            {
                Text = initial,
                Font = new Font("Segoe UI", 48F, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(280, 200),
                TextAlign = ContentAlignment.MiddleCenter
            };
            lbl.Click += (s, e) =>
            {
                if (panel.Tag is DAL.Entities.NhanVien emp)
                    ViewEmployeeDetail(emp);
            };
            lbl.Cursor = Cursors.Hand;
            panel.Controls.Add(lbl);
        }
        #endregion

        #region Helper Methods
        private string GetRoleIcon(string role) => role switch
        {
            "Admin" => "👑",
            "Quản lý" => "⭐",
            "Thu ngân" => "💰",
            "Phục vụ" => "👨‍🍳",
            _ => "👤"
        };

        private Color GetRoleColor(string role) => role switch
        {
            "Admin" => Color.FromArgb(220, 53, 69),
            "Quản lý" => Color.FromArgb(255, 193, 7),
            "Thu ngân" => Color.FromArgb(23, 162, 184),
            "Phục vụ" => Color.FromArgb(40, 167, 69),
            _ => Color.Gray
        };

        private string GetShiftIcon(string shift) => shift switch
        {
            "Sang" => "🌅",
            "Chieu" => "☀️",
            "Toi" => "🌙",
            "FullTime" => "⏰",
            _ => "⏰"
        };
        #endregion

        #region Filter Methods
        private void FilterStatus_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;
            _currentStatusFilter = btn.Tag?.ToString() ?? "all";
            UpdateStatusButtonStyles(btn);
            ApplyFilters();
        }

        private void FilterRole_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;
            _currentRoleFilter = btn.Tag?.ToString() ?? "all";
            UpdateRoleButtonStyles(btn);
            ApplyFilters();
        }

        private void UpdateStatusButtonStyles(Button activeBtn)
        {
            var statusBtns = new[] { btnFilterAll, btnFilterActive, btnFilterInactive };
            foreach (var b in statusBtns)
            {
                if (b == activeBtn)
                {
                    b.BackColor = Color.FromArgb(102, 126, 234);
                    b.ForeColor = Color.White;
                    b.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }
                else
                {
                    b.BackColor = Color.FromArgb(233, 236, 239);
                    b.ForeColor = Color.Black;
                    b.Font = new Font("Segoe UI", 9F);
                }
            }
        }

        private void UpdateRoleButtonStyles(Button activeBtn)
        {
            var roleBtns = new[] { btnRoleAll, btnRoleAdmin, btnRoleManager, btnRoleCashier, btnRoleStaff };
            foreach (var b in roleBtns)
            {
                if (b == activeBtn)
                {
                    b.BackColor = Color.FromArgb(102, 126, 234);
                    b.ForeColor = Color.White;
                    b.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }
                else
                {
                    b.BackColor = Color.FromArgb(233, 236, 239);
                    b.ForeColor = Color.Black;
                    b.Font = new Font("Segoe UI", 9F);
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) => ApplyFilters();

        private void ApplyFilters()
        {
            _filteredEmployees = new List<DAL.Entities.NhanVien>(_allEmployees);
            if (_currentStatusFilter != "all")
                _filteredEmployees = _filteredEmployees.Where(x => x.TrangThai == _currentStatusFilter).ToList();
            if (_currentRoleFilter != "all")
                _filteredEmployees = _filteredEmployees.Where(x => x.MaNhomNavigation?.TenNhom == _currentRoleFilter).ToList();
            string searchText = txtSearch.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(searchText))
                _filteredEmployees = _filteredEmployees.Where(x => x.TenNv.ToLower().Contains(searchText) || (x.Sdt?.Contains(searchText) ?? false)).ToList();
            DisplayEmployees();
        }
        #endregion

        #region Button Click Events
        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addForm = new AddNhanVienForm(_currentUserId);
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                LoadEmployees();
                // Ẩn detail panel khi thêm mới
                HideDetailPanel();
            }
        }

        private void btnAttendance_Click(object sender, EventArgs e)
        {
            try
            {
                var chamCongForm = new ChamCongForm();
                chamCongForm.SetUserInfo(_currentUserId, _currentUserRole);
                chamCongForm.ShowDialog(this);

                // Reload data nếu cần
                LoadEmployees();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở form chấm công: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSchedule_Click(object sender, EventArgs e)
        {
            try
            {
                var scheduleForm = new ScheduleForm(_nhanVienService);
                scheduleForm.SetUserInfo(_currentUserId, _currentUserRole);
                scheduleForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở form lịch làm việc: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Navigation Methods
        private void ViewEmployeeDetail(DAL.Entities.NhanVien emp)
        {
            if (_mainForm == null) return;

            // [MODIFIED] KIỂM TRA: Nếu đang xem cùng nhân viên thì chỉ reload detail
            if (_currentSelectedEmployeeId == emp.MaNv && _mainForm.pnlDetail.Visible)
            {
                // Reload data trong control hiện tại
                _ = _currentDetailControl?.LoadNhanVienDetail();
                return;
            }

            // Cập nhật ID nhân viên đang được chọn
            _currentSelectedEmployeeId = emp.MaNv;

            // Refresh lại display để highlight card được chọn
            DisplayEmployees();

            // Xử lý control cũ
            if (_currentDetailControl != null)
            {
                _currentDetailControl.OnDataChanged -= DetailControl_OnDataChanged;
                _currentDetailControl.Dispose();
                _currentDetailControl = null;
            }

            // Tạo control mới
            _currentDetailControl = new ChiTietNhanVienControl(_nhanVienService, emp, _currentUserId, _currentUserRole);

            // Subscribe events
            _currentDetailControl.OnDataChanged += DetailControl_OnDataChanged;

            // [MODIFIED] Gọi MainForm để hiển thị control trong pnlDetail
            _mainForm.UpdateDetailPanel($"Chi tiết nhân viên: {emp.TenNv}", _currentDetailControl, 450);
        }

        private void HideDetailPanel()
        {
            // [MODIFIED] Gọi MainForm để ẩn pnlDetail
            _mainForm?.HideDetailPanel();

            _currentSelectedEmployeeId = -1;

            if (_currentDetailControl != null)
            {
                _currentDetailControl.OnDataChanged -= DetailControl_OnDataChanged;
                _currentDetailControl.Dispose();
                _currentDetailControl = null;
            }

            // Refresh để bỏ highlight
            DisplayEmployees();
        }

        private void DetailControl_OnDataChanged(object sender, EventArgs e)
        {
            // Reload danh sách nhân viên
            LoadEmployees();

            // Reload detail nếu panel đang hiển thị
            if (_currentDetailControl != null && _mainForm.pnlDetail.Visible && _currentSelectedEmployeeId > 0)
            {
                // Lấy lại thông tin nhân viên mới nhất
                var updatedEmp = _allEmployees.FirstOrDefault(x => x.MaNv == _currentSelectedEmployeeId);
                if (updatedEmp != null)
                {
                    _ = _currentDetailControl.LoadNhanVienDetail();
                }
            }
        }

        private void OpenEditForm(DAL.Entities.NhanVien emp)
        {
            var editForm = new EditNhanVienForm(emp.MaNv, _currentUserId);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadEmployees();

                // Reload detail nếu đang hiển thị nhân viên này
                if (_currentDetailControl != null && _mainForm.pnlDetail.Visible && _currentSelectedEmployeeId == emp.MaNv)
                {
                    _ = _currentDetailControl.LoadNhanVienDetail();
                }
            }
        }
        #endregion

        private void flowLayoutEmployees_Paint(object sender, PaintEventArgs e)
        {
        }

        private void btnSalary_Click(object sender, EventArgs e)
        {
            try
            {
                var salaryForm = new SalaryManagementForm(_nhanVienService);
                salaryForm.SetUserInfo(_currentUserId, _currentUserRole);
                salaryForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở form bảng lương: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}