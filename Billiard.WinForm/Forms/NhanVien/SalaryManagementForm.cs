using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Billiard.BLL.Services.NhanVienService;
using Billiard.DAL.Entities;

namespace Billiard.WinForm.Forms.NhanVien
{
    public partial class SalaryManagementForm : Form
    {
        #region Fields
        private readonly NhanVienService _nhanVienService;
        private List<DAL.Entities.NhanVien> _allEmployees;
        private int _selectedMonth;
        private int _selectedYear;
        private int _currentUserId;
        private string _currentUserRole;
        #endregion

        #region Constructor
        public SalaryManagementForm(NhanVienService nhanVienService)
        {
            _nhanVienService = nhanVienService;
            _selectedMonth = DateTime.Now.Month;
            _selectedYear = DateTime.Now.Year;

            InitializeComponent();
            InitializeCustomSettings();
        }

        public void SetUserInfo(int userId, string userRole)
        {
            _currentUserId = userId;
            _currentUserRole = userRole;
        }
        #endregion

        #region Initialize
        private void InitializeCustomSettings()
        {
            // Setup Month ComboBox
            for (int i = 1; i <= 12; i++)
                cboMonth.Items.Add(i);
            cboMonth.SelectedItem = _selectedMonth;
            cboMonth.SelectedIndexChanged += CboMonth_SelectedIndexChanged;

            // Setup Year ComboBox
            int currentYear = DateTime.Now.Year;
            for (int i = currentYear - 2; i <= currentYear; i++)
                cboYear.Items.Add(i);
            cboYear.SelectedItem = _selectedYear;
            cboYear.SelectedIndexChanged += CboYear_SelectedIndexChanged;

            // Setup Buttons
            btnRefresh.Click += BtnRefresh_Click;
            btnCalculateAll.Click += BtnCalculateAll_Click;
            btnExport.Click += BtnExport_Click;
            btnClose.Click += (s, e) => this.Close();

            // Format DataGridView columns
            FormatDataGridView();

            // Load initial data
            LoadData();
        }

        private void FormatDataGridView()
        {
            // Format numeric columns
            colSoNgayLam.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            colTongGioLam.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colTongGioLam.DefaultCellStyle.Format = "N2";

            colLuongTheoGio.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colLuongTheoGio.DefaultCellStyle.Format = "N0";

            colLuongCoBan.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colLuongCoBan.DefaultCellStyle.Format = "N0";

            colPhuCap.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colPhuCap.DefaultCellStyle.Format = "N0";

            colThuong.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colThuong.DefaultCellStyle.Format = "N0";

            colPhat.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colPhat.DefaultCellStyle.Format = "N0";

            colTongLuong.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colTongLuong.DefaultCellStyle.Format = "N0";
            colTongLuong.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            colTongLuong.DefaultCellStyle.ForeColor = Color.FromArgb(34, 197, 94);
        }
        #endregion

        #region Load Data
        private void LoadData()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                dgvSalary.Rows.Clear();

                // Load all active employees
                _allEmployees = _nhanVienService.GetEmployeesByStatus("DangLam");

                decimal totalSalary = 0;
                decimal totalHours = 0;
                int totalEmployees = 0;

                foreach (var emp in _allEmployees)
                {
                    // Get attendance data for the month
                    var attendances = _nhanVienService.GetAttendanceByMonth(emp.MaNv, _selectedMonth, _selectedYear);

                    int workDays = attendances.Count;
                    decimal totalWorkHours = attendances.Sum(a => a.SoGioLam ?? 0);

                    // Calculate hourly rate (assuming 176 standard hours per month)
                    decimal hourlyRate = (emp.LuongCoBan ?? 0) / 176m;

                    // Calculate salary based on actual hours worked
                    decimal baseSalaryByHour = totalWorkHours * hourlyRate;
                    decimal allowance = emp.PhuCap ?? 0;

                    // Get or calculate monthly salary
                    var salary = _nhanVienService.GetLatestSalary(emp.MaNv);
                    decimal bonus = 0;
                    decimal penalty = 0;

                    if (salary != null && salary.Thang == _selectedMonth && salary.Nam == _selectedYear)
                    {
                        bonus = salary.Thuong ?? 0;
                        penalty = salary.Phat ?? 0;
                    }
                    else
                    {
                        // Calculate penalty for late days
                        int lateDays = attendances.Count(a => a.TrangThai == "DiTre");
                        penalty = lateDays * 50000;
                    }

                    decimal total = baseSalaryByHour + allowance + bonus - penalty;

                    dgvSalary.Rows.Add(
                        emp.MaNv,
                        emp.TenNv,
                        emp.MaNhomNavigation?.TenNhom ?? "N/A",
                        workDays,
                        totalWorkHours,
                        hourlyRate,
                        baseSalaryByHour,
                        allowance,
                        bonus,
                        penalty,
                        total
                    );

                    totalSalary += total;
                    totalHours += totalWorkHours;
                    totalEmployees++;
                }

                // Update statistics
                lblTotalEmployees.Text = totalEmployees.ToString();
                lblTotalHours.Text = $"{totalHours:N2}h";
                lblTotalSalary.Text = $"{totalSalary:N0}đ";

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Event Handlers
        private void CboMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMonth.SelectedItem != null)
            {
                _selectedMonth = (int)cboMonth.SelectedItem;
                LoadData();
            }
        }

        private void CboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboYear.SelectedItem != null)
            {
                _selectedYear = (int)cboYear.SelectedItem;
                LoadData();
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void BtnCalculateAll_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                $"Xác nhận tính lương cho tất cả nhân viên trong tháng {_selectedMonth}/{_selectedYear}?\n\n" +
                "Hành động này sẽ cập nhật bảng lương trong cơ sở dữ liệu.",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                Cursor = Cursors.WaitCursor;
                int successCount = 0;
                int errorCount = 0;

                foreach (var emp in _allEmployees)
                {
                    try
                    {
                        _nhanVienService.CalculateMonthlySalary(emp.MaNv, _selectedMonth, _selectedYear);
                        successCount++;
                    }
                    catch
                    {
                        errorCount++;
                    }
                }

                Cursor = Cursors.Default;

                MessageBox.Show(
                    $"✅ Hoàn thành!\n\n" +
                    $"Thành công: {successCount}\n" +
                    $"Lỗi: {errorCount}",
                    "Kết quả",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadData();
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                using (var sfd = new SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    FileName = $"BangLuong_{_selectedMonth}_{_selectedYear}.xlsx"
                })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        // Export logic here (requires ClosedXML or similar library)
                        MessageBox.Show("Chức năng xuất Excel đang được phát triển",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất file: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvSalary_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != dgvSalary.Columns["colActions"].Index)
                return;

            int maNV = Convert.ToInt32(dgvSalary.Rows[e.RowIndex].Cells["colMaNV"].Value);
            ShowSalaryDetail(maNV);
        }

        private void ShowSalaryDetail(int maNV)
        {
            var employee = _allEmployees.FirstOrDefault(e => e.MaNv == maNV);
            if (employee == null) return;

            var detailForm = new SalaryDetailForm(_nhanVienService, employee, _selectedMonth, _selectedYear);
            if (detailForm.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }
        #endregion
    }
}