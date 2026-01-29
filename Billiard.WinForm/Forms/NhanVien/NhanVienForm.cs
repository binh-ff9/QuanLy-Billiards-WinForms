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

        private MainForm _mainForm;
        private ChiTietNhanVienControl _currentDetailControl;
        private int _currentSelectedEmployeeId = -1;
        private const int DETAIL_PANEL_WIDTH = 450;
        #endregion

        #region Constructor
        public NhanVienForm(NhanVienService nhanVienService)
        {
            InitializeComponent();
            _nhanVienService = nhanVienService;

            // [OPTIMIZED] Enable double buffering cho form để giảm flicker
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);
            UpdateStyles();

            // [OPTIMIZED] Enable double buffering cho FlowLayoutPanel
            typeof(FlowLayoutPanel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, flowLayoutEmployees, new object[] { true });
        }
        #endregion

        public void SetUserInfo(int userId, string userRole)
        {
            _currentUserId = userId;
            _currentUserRole = userRole;
            SetupPermissions();
        }

        public void SetMainForm(MainForm mainForm)
        {
            _mainForm = mainForm;
        }

        #region Form Events
        private void NhanVienForm_Load(object sender, EventArgs e)
        {
            try
            {
                // [OPTIMIZED] Suspend layout để tránh redraw nhiều lần
                SuspendLayout();

                // Ẩn detail panel khi load form
                HideDetailPanel();
                LoadEmployees();

                // [OPTIMIZED] Resume layout sau khi hoàn tất
                ResumeLayout(false);
                PerformLayout();
            }
            catch (Exception ex)
            {
                ResumeLayout(false);
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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
            // [OPTIMIZED] Suspend layout để giảm flicker
            flowLayoutEmployees.SuspendLayout();
            try
            {
                flowLayoutEmployees.Controls.Clear();

                if (_filteredEmployees == null || !_filteredEmployees.Any())
                {
                    ShowEmptyState();
                    return;
                }

                // [OPTIMIZED] Tạo tất cả cards trước khi add vào flow layout
                var cards = new List<Panel>(_filteredEmployees.Count);
                foreach (var emp in _filteredEmployees)
                {
                    var card = CreateEmployeeCard(emp);
                    cards.Add(card);
                }

                // [OPTIMIZED] Add tất cả cards một lúc
                flowLayoutEmployees.Controls.AddRange(cards.ToArray());
            }
            finally
            {
                // [OPTIMIZED] Resume layout một lần duy nhất
                flowLayoutEmployees.ResumeLayout(true);
            }
        }

        private void ShowEmptyState()
        {
            var emptyPanel = new Panel { Size = new Size(flowLayoutEmployees.Width - 40, 300), BackColor = Color.White };
            var lblTitle = new Label { Text = "Không có nhân viên nào", Font = new Font("Segoe UI", 16F, FontStyle.Bold), AutoSize = true, Location = new Point((emptyPanel.Width - 250) / 2, 140) };
            var lblText = new Label { Text = "Chưa có nhân viên trong hệ thống", Font = new Font("Segoe UI", 11F), ForeColor = Color.Gray, AutoSize = true, Location = new Point((emptyPanel.Width - 280) / 2, 180) };
            emptyPanel.Controls.AddRange(new Control[] { lblTitle, lblText });
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

            // [OPTIMIZED] Enable double buffering cho card
            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, card, new object[] { true });

            // Highlight card nếu đang được chọn
            if (_currentSelectedEmployeeId == emp.MaNv)
            {
                card.BackColor = Color.FromArgb(232, 240, 254);
            }

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

            // Nút Edit
            if (_currentUserRole == "Admin" || _currentUserRole == "Quản lý")
            {
                var btnQuickEdit = new Button
                {
                    Text = "✏️",
                    Size = new Size(35, 35),
                    Location = new Point(235, 10),
                    BackColor = Color.White,
                    ForeColor = Color.FromArgb(102, 126, 234),
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 12F),
                    Cursor = Cursors.Hand
                };
                btnQuickEdit.FlatAppearance.BorderSize = 0;
                btnQuickEdit.Click += (s, e) => { e = e ?? EventArgs.Empty; OpenEditForm(emp); };
                imgPanel.Controls.Add(btnQuickEdit);
            }

            // Info panel
            var infoPanel = new Panel { Size = new Size(280, 180), Location = new Point(0, 200), BackColor = Color.White };

            var lblName = new Label
            {
                Text = emp.TenNv,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 26, 46),
                Location = new Point(15, 15),
                Size = new Size(250, 30),
                AutoSize = false,
                AutoEllipsis = true
            };

            var lblPhone = new Label
            {
                Text = $"📱 {emp.Sdt ?? "Chưa có SĐT"}",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                Location = new Point(15, 50),
                AutoSize = true
            };

            var lblEmail = new Label
            {
                Text = $"📧 {emp.Email ?? "Chưa có email"}",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                Location = new Point(15, 75),
                Size = new Size(250, 20),
                AutoSize = false,
                AutoEllipsis = true
            };

            var lblSalary = new Label
            {
                Text = $"💰 {emp.LuongCoBan:N0}đ",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 167, 69),
                Location = new Point(15, 100),
                AutoSize = true
            };

            var lblShift = new Label
            {
                Text = GetShiftText(emp.CaMacDinh),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(102, 126, 234),
                Location = new Point(15, 125),
                AutoSize = true
            };

            infoPanel.Controls.AddRange(new Control[] { lblName, lblPhone, lblEmail, lblSalary, lblShift });
            card.Controls.AddRange(new Control[] { imgPanel, infoPanel });

            AddClickHandlerToChildren(card);

            return card;
        }

        private void AddAvatarPlaceholder(Panel imgPanel, string tenNv)
        {
            var lblAvatar = new Label
            {
                Text = !string.IsNullOrEmpty(tenNv) ? tenNv.Substring(0, 1).ToUpper() : "?",
                Font = new Font("Segoe UI", 48F, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(280, 200),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            imgPanel.Controls.Add(lblAvatar);
        }

        private static Color GetRoleColor(string role)
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

        private static string GetRoleIcon(string role)
        {
            return role switch
            {
                "Admin" => "👑",
                "Quản lý" => "📋",
                "Thu ngân" => "💰",
                "Phục vụ" => "🍽️",
                _ => "👤"
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
        #endregion

        #region Filter Events
        private void FilterStatus_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                _currentStatusFilter = btn.Tag?.ToString() ?? "all";
                UpdateFilterButtons(new[] { btnFilterAll, btnFilterActive, btnFilterInactive }, btn);
                ApplyFilters();
            }
        }

        private void FilterRole_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                _currentRoleFilter = btn.Tag?.ToString() ?? "all";
                UpdateFilterButtons(new[] { btnRoleAll, btnRoleAdmin, btnRoleManager, btnRoleCashier, btnRoleStaff }, btn);
                ApplyFilters();
            }
        }

        private void UpdateFilterButtons(Button[] buttons, Button activeButton)
        {
            foreach (var b in buttons)
            {
                if (b == activeButton)
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
        #endregion

        #region Navigation Methods
        private void ViewEmployeeDetail(DAL.Entities.NhanVien emp)
        {
            // [OPTIMIZED] Suspend layout trong khi thay đổi UI
            tableLayoutMain.SuspendLayout();
            pnlDetailContainer.SuspendLayout();

            try
            {
                if (_currentSelectedEmployeeId == emp.MaNv && pnlDetailContainer.Visible)
                {
                    _ = _currentDetailControl?.LoadNhanVienDetail();
                    return;
                }

                _currentSelectedEmployeeId = emp.MaNv;
                DisplayEmployees();

                if (_currentDetailControl != null)
                {
                    _currentDetailControl.OnDataChanged -= DetailControl_OnDataChanged;
                    _currentDetailControl.Dispose();
                    _currentDetailControl = null;
                }

                _currentDetailControl = new ChiTietNhanVienControl(_nhanVienService, emp, _currentUserId, _currentUserRole);
                _currentDetailControl.OnDataChanged += DetailControl_OnDataChanged;

                ShowDetailPanel();

                pnlDetailContainer.Controls.Clear();
                _currentDetailControl.Dock = DockStyle.Fill;
                pnlDetailContainer.Controls.Add(_currentDetailControl);
            }
            finally
            {
                // [OPTIMIZED] Resume layout
                pnlDetailContainer.ResumeLayout(true);
                tableLayoutMain.ResumeLayout(true);
            }
        }

        private void ShowDetailPanel()
        {
            tableLayoutMain.ColumnStyles[1].SizeType = SizeType.Absolute;
            tableLayoutMain.ColumnStyles[1].Width = DETAIL_PANEL_WIDTH;
            tableLayoutMain.ColumnStyles[0].SizeType = SizeType.Percent;
            tableLayoutMain.ColumnStyles[0].Width = 100F;
            pnlDetailContainer.Visible = true;
        }

        private void HideDetailPanel()
        {
            pnlDetailContainer.Visible = false;
            tableLayoutMain.ColumnStyles[1].SizeType = SizeType.Absolute;
            tableLayoutMain.ColumnStyles[1].Width = 0;
            tableLayoutMain.ColumnStyles[0].SizeType = SizeType.Percent;
            tableLayoutMain.ColumnStyles[0].Width = 100F;
            _currentSelectedEmployeeId = -1;

            if (_currentDetailControl != null)
            {
                _currentDetailControl.OnDataChanged -= DetailControl_OnDataChanged;
                _currentDetailControl.Dispose();
                _currentDetailControl = null;
            }

            DisplayEmployees();
        }

        private void DetailControl_OnDataChanged(object sender, EventArgs e)
        {
            LoadEmployees();

            if (_currentDetailControl != null && pnlDetailContainer.Visible && _currentSelectedEmployeeId > 0)
            {
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

                if (_currentDetailControl != null && pnlDetailContainer.Visible && _currentSelectedEmployeeId == emp.MaNv)
                {
                    _ = _currentDetailControl.LoadNhanVienDetail();
                }
            }
        }
        #endregion

        private void flowLayoutEmployees_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}