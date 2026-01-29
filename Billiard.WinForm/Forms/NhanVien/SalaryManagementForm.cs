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
    public partial class SalaryManagementForm : Form
    {
        #region Fields
        private readonly NhanVienService _nhanVienService;
        private List<DAL.Entities.NhanVien> _allEmployees;
        private List<SalaryViewModel> _allSalaryData;
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
            // Initialize empty lists to prevent null reference
            _allEmployees = new List<DAL.Entities.NhanVien>();
            _allSalaryData = new List<SalaryViewModel>();

            // Setup Month ComboBox
            for (int i = 1; i <= 12; i++)
                cboMonth.Items.Add(i);
            cboMonth.SelectedItem = _selectedMonth;
            cboMonth.SelectedIndexChanged += CboMonth_SelectedIndexChanged;

            // Setup Year ComboBox
            int currentYear = DateTime.Now.Year;
            for (int i = currentYear - 5; i <= currentYear; i++)
                cboYear.Items.Add(i);
            cboYear.SelectedItem = _selectedYear;
            cboYear.SelectedIndexChanged += CboYear_SelectedIndexChanged;

            // Setup Nhom (Department) ComboBox
            LoadNhomComboBox();

            // Setup Status ComboBox
            cboStatus.Items.Add("-- Tất cả trạng thái --");
            cboStatus.Items.Add("Đã tính lương");
            cboStatus.Items.Add("Chưa tính lương");
            cboStatus.SelectedIndex = 0;

            // Setup Buttons
            btnRefresh.Click += BtnRefresh_Click;
            btnCalculateAll.Click += BtnCalculateAll_Click;
            btnExport.Click += BtnExport_Click;

            // Format DataGridView columns
            FormatDataGridView();

            // Load initial data
            LoadData();
        }

        private void LoadNhomComboBox()
        {
            try
            {
                cboNhom.Items.Clear();
                cboNhom.Items.Add("-- Tất cả phòng ban --");

                var roles = _nhanVienService?.GetAllRoles();
                if (roles != null && roles.Count > 0)
                {
                    foreach (var role in roles)
                    {
                        if (role != null)
                        {
                            cboNhom.Items.Add(new ComboBoxItem
                            {
                                Value = role.MaNhom,
                                Text = role.TenNhom ?? "N/A"
                            });
                        }
                    }
                }

                cboNhom.DisplayMember = "Text";
                cboNhom.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                // Fallback to basic setup
                cboNhom.Items.Clear();
                cboNhom.Items.Add("-- Tất cả phòng ban --");
                cboNhom.SelectedIndex = 0;

                System.Diagnostics.Debug.WriteLine($"Lỗi LoadNhomComboBox: {ex.Message}");
            }
        }

        private void FormatDataGridView()
        {
            try
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

                colTrangThai.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi FormatDataGridView: {ex.Message}");
            }
        }
        #endregion

        #region Load Data
        private void LoadData()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                // Initialize lists if null
                if (_allEmployees == null)
                    _allEmployees = new List<DAL.Entities.NhanVien>();
                if (_allSalaryData == null)
                    _allSalaryData = new List<SalaryViewModel>();

                // Clear existing data
                _allEmployees.Clear();
                _allSalaryData.Clear();

                // Load all active employees - try both formats
                try
                {
                    var employees = _nhanVienService?.GetEmployeesByStatus("DangLam");
                    if (employees != null && employees.Count > 0)
                    {
                        _allEmployees = employees;
                    }
                    else
                    {
                        // Try alternative status
                        employees = _nhanVienService?.GetEmployeesByStatus("Đang làm");
                        if (employees != null)
                        {
                            _allEmployees = employees;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Lỗi load employees: {ex.Message}");
                }

                // Calculate salary for each employee
                if (_allEmployees != null)
                {
                    foreach (var emp in _allEmployees)
                    {
                        try
                        {
                            if (emp != null)
                            {
                                var salaryData = CalculateEmployeeSalary(emp);
                                if (salaryData != null)
                                {
                                    _allSalaryData.Add(salaryData);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Lỗi tính lương cho NV {emp?.MaNv}: {ex.Message}");
                        }
                    }
                }

                // Apply filters
                ApplyFilters();

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;

                // Initialize empty lists if error
                if (_allEmployees == null)
                    _allEmployees = new List<DAL.Entities.NhanVien>();
                if (_allSalaryData == null)
                    _allSalaryData = new List<SalaryViewModel>();

                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}\n\nVui lòng kiểm tra:\n" +
                    "1. Kết nối database\n" +
                    "2. Bảng NhanVien có dữ liệu\n" +
                    "3. Trường TrangThai = 'DangLam' hoặc 'Đang làm'",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Update UI with empty data
                UpdateDataGridView(new List<SalaryViewModel>());
            }
        }

        private SalaryViewModel CalculateEmployeeSalary(DAL.Entities.NhanVien emp)
        {
            if (emp == null)
                return null;

            try
            {
                // Get attendance data for the month
                var attendances = _nhanVienService?.GetAttendanceByMonth(emp.MaNv, _selectedMonth, _selectedYear);
                if (attendances == null)
                    attendances = new List<ChamCong>();

                int workDays = attendances.Count;
                decimal totalWorkHours = attendances.Sum(a => a.SoGioLam ?? 0);

                // Calculate hourly rate (assuming 176 standard hours per month = 22 days * 8 hours)
                decimal baseSalary = emp.LuongCoBan ?? 0;
                decimal hourlyRate = baseSalary > 0 ? baseSalary / 176m : 0;

                // Calculate salary based on actual hours worked
                decimal salaryByHour = totalWorkHours * hourlyRate;
                decimal allowance = emp.PhuCap ?? 0;

                // Get or calculate monthly salary from BangLuong
                var salaryRecord = _nhanVienService?.GetLatestSalary(emp.MaNv);
                decimal bonus = 0;
                decimal penalty = 0;
                bool isCalculated = false;

                if (salaryRecord != null && salaryRecord.Thang == _selectedMonth && salaryRecord.Nam == _selectedYear)
                {
                    // Salary already calculated
                    bonus = salaryRecord.Thuong ?? 0;
                    penalty = salaryRecord.Phat ?? 0;
                    isCalculated = true;
                }
                else
                {
                    // Calculate penalty for late days
                    int lateDays = attendances.Count(a => a.TrangThai == "DiTre");
                    penalty = lateDays * 50000;
                    bonus = 0;
                }

                decimal totalSalary = salaryByHour + allowance + bonus - penalty;

                return new SalaryViewModel
                {
                    MaNV = emp.MaNv,
                    TenNV = emp.TenNv ?? "N/A",
                    ChucVu = emp.MaNhomNavigation?.TenNhom ?? "N/A",
                    MaNhom = emp.MaNhom,
                    SoNgayLam = workDays,
                    TongGioLam = totalWorkHours,
                    LuongTheoGio = hourlyRate,
                    LuongCoBan = salaryByHour,
                    PhuCap = allowance,
                    Thuong = bonus,
                    Phat = penalty,
                    TongLuong = totalSalary,
                    TrangThai = isCalculated ? "Đã tính lương" : "Chưa tính lương"
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi CalculateEmployeeSalary: {ex.Message}");
                return null;
            }
        }

        private void ApplyFilters()
        {
            try
            {
                if (_allSalaryData == null)
                {
                    _allSalaryData = new List<SalaryViewModel>();
                    UpdateDataGridView(new List<SalaryViewModel>());
                    return;
                }

                var filteredData = _allSalaryData.AsEnumerable();

                // Filter by Nhom (Department)
                if (cboNhom.SelectedIndex > 0 && cboNhom.SelectedItem is ComboBoxItem nhomItem)
                {
                    filteredData = filteredData.Where(s => s.MaNhom == nhomItem.Value);
                }

                // Filter by Status
                if (cboStatus.SelectedIndex > 0)
                {
                    string selectedStatus = cboStatus.SelectedItem?.ToString();
                    if (!string.IsNullOrEmpty(selectedStatus))
                    {
                        filteredData = filteredData.Where(s => s.TrangThai == selectedStatus);
                    }
                }

                // Filter by Search Text
                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    string searchText = txtSearch.Text.ToLower();
                    filteredData = filteredData.Where(s =>
                        (s.TenNV ?? "").ToLower().Contains(searchText) ||
                        s.MaNV.ToString().Contains(searchText));
                }

                // Update DataGridView
                UpdateDataGridView(filteredData?.ToList() ?? new List<SalaryViewModel>());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi ApplyFilters: {ex.Message}");
                UpdateDataGridView(new List<SalaryViewModel>());
            }
        }

        private void UpdateDataGridView(List<SalaryViewModel> data)
        {
            try
            {
                dgvSalary.Rows.Clear();

                if (data == null)
                    data = new List<SalaryViewModel>();

                decimal totalSalary = 0;
                decimal totalHours = 0;
                int totalEmployees = data.Count;

                foreach (var item in data)
                {
                    if (item == null) continue;

                    int rowIndex = dgvSalary.Rows.Add(
                        item.MaNV,
                        item.TenNV ?? "N/A",
                        item.ChucVu ?? "N/A",
                        item.SoNgayLam,
                        item.TongGioLam,
                        item.LuongTheoGio,
                        item.LuongCoBan,
                        item.PhuCap,
                        item.Thuong,
                        item.Phat,
                        item.TongLuong,
                        item.TrangThai ?? "N/A"
                    );

                    // Color code status
                    var statusCell = dgvSalary.Rows[rowIndex].Cells["colTrangThai"];
                    if (item.TrangThai == "Đã tính lương")
                    {
                        statusCell.Style.BackColor = Color.FromArgb(220, 252, 231);
                        statusCell.Style.ForeColor = Color.FromArgb(21, 128, 61);
                    }
                    else
                    {
                        statusCell.Style.BackColor = Color.FromArgb(254, 243, 199);
                        statusCell.Style.ForeColor = Color.FromArgb(161, 98, 7);
                    }

                    totalSalary += item.TongLuong;
                    totalHours += item.TongGioLam;
                }

                // Update statistics
                lblTotalEmployees.Text = totalEmployees.ToString();
                lblTotalHours.Text = $"{totalHours:N2}h";
                lblTotalSalary.Text = $"{totalSalary:N0}đ";
                lblAvgSalary.Text = totalEmployees > 0 ? $"{(totalSalary / totalEmployees):N0}đ" : "0đ";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi UpdateDataGridView: {ex.Message}");
                MessageBox.Show($"Lỗi cập nhật giao diện: {ex.Message}", "Lỗi",
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

        private void CboNhom_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void CboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void BtnCalculateAll_Click(object sender, EventArgs e)
        {
            if (_allEmployees == null || _allEmployees.Count == 0)
            {
                MessageBox.Show("Không có nhân viên nào để tính lương!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Xác nhận tính lương cho {_allEmployees.Count} nhân viên trong tháng {_selectedMonth}/{_selectedYear}?\n\n" +
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
                List<string> errors = new List<string>();

                foreach (var emp in _allEmployees)
                {
                    try
                    {
                        if (emp != null)
                        {
                            _nhanVienService.CalculateMonthlySalary(emp.MaNv, _selectedMonth, _selectedYear);
                            successCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        if (errors.Count < 5)
                        {
                            errors.Add($"{emp?.TenNv ?? "N/A"}: {ex.Message}");
                        }
                    }
                }

                Cursor = Cursors.Default;

                string message = $"✅ Hoàn thành!\n\n" +
                    $"Thành công: {successCount}\n" +
                    $"Lỗi: {errorCount}";

                if (errors.Count > 0)
                {
                    message += "\n\nMột số lỗi:\n" + string.Join("\n", errors);
                    if (errorCount > 5)
                        message += $"\n... và {errorCount - 5} lỗi khác";
                }

                MessageBox.Show(message, "Kết quả",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

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
            if (_allSalaryData == null || _allSalaryData.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

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
                        ExportToExcel(sfd.FileName);
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

            try
            {
                int maNV = Convert.ToInt32(dgvSalary.Rows[e.RowIndex].Cells["colMaNV"].Value);
                ShowSalaryDetail(maNV);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Helper Methods
        private void ShowSalaryDetail(int maNV)
        {
            try
            {
                var employee = _allEmployees?.FirstOrDefault(e => e.MaNv == maNV);
                if (employee == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin nhân viên!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var detailForm = new SalaryDetailForm(_nhanVienService, employee, _selectedMonth, _selectedYear);
                if (detailForm.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi mở form chi tiết: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToExcel(string filePath)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add($"Luong thang {_selectedMonth}-{_selectedYear}");

                    // Title
                    worksheet.Cell(1, 1).Value = "BẢNG LƯƠNG NHÂN VIÊN";
                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Cell(1, 1).Style.Font.FontSize = 16;
                    worksheet.Range(1, 1, 1, 12).Merge();
                    worksheet.Range(1, 1, 1, 12).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // Period
                    worksheet.Cell(2, 1).Value = $"Tháng {_selectedMonth} năm {_selectedYear}";
                    worksheet.Cell(2, 1).Style.Font.Bold = true;
                    worksheet.Range(2, 1, 2, 12).Merge();
                    worksheet.Range(2, 1, 2, 12).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // Headers
                    int headerRow = 4;
                    worksheet.Cell(headerRow, 1).Value = "Mã NV";
                    worksheet.Cell(headerRow, 2).Value = "Tên nhân viên";
                    worksheet.Cell(headerRow, 3).Value = "Chức vụ";
                    worksheet.Cell(headerRow, 4).Value = "Số ngày";
                    worksheet.Cell(headerRow, 5).Value = "Tổng giờ";
                    worksheet.Cell(headerRow, 6).Value = "Lương/giờ";
                    worksheet.Cell(headerRow, 7).Value = "Lương theo giờ";
                    worksheet.Cell(headerRow, 8).Value = "Phụ cấp";
                    worksheet.Cell(headerRow, 9).Value = "Thưởng";
                    worksheet.Cell(headerRow, 10).Value = "Phạt";
                    worksheet.Cell(headerRow, 11).Value = "Tổng lương";
                    worksheet.Cell(headerRow, 12).Value = "Trạng thái";

                    // Style headers
                    var headerRange = worksheet.Range(headerRow, 1, headerRow, 12);
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Fill.BackgroundColor = XLColor.FromArgb(102, 126, 234);
                    headerRange.Style.Font.FontColor = XLColor.White;
                    headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // Data
                    int currentRow = headerRow + 1;
                    decimal totalSalary = 0;

                    if (_allSalaryData != null)
                    {
                        foreach (var item in _allSalaryData)
                        {
                            if (item == null) continue;

                            worksheet.Cell(currentRow, 1).Value = item.MaNV;
                            worksheet.Cell(currentRow, 2).Value = item.TenNV ?? "N/A";
                            worksheet.Cell(currentRow, 3).Value = item.ChucVu ?? "N/A";
                            worksheet.Cell(currentRow, 4).Value = item.SoNgayLam;
                            worksheet.Cell(currentRow, 5).Value = item.TongGioLam;
                            worksheet.Cell(currentRow, 6).Value = item.LuongTheoGio;
                            worksheet.Cell(currentRow, 7).Value = item.LuongCoBan;
                            worksheet.Cell(currentRow, 8).Value = item.PhuCap;
                            worksheet.Cell(currentRow, 9).Value = item.Thuong;
                            worksheet.Cell(currentRow, 10).Value = item.Phat;
                            worksheet.Cell(currentRow, 11).Value = item.TongLuong;
                            worksheet.Cell(currentRow, 12).Value = item.TrangThai ?? "N/A";

                            // Format numbers
                            worksheet.Cell(currentRow, 5).Style.NumberFormat.Format = "#,##0.00";
                            worksheet.Cell(currentRow, 6).Style.NumberFormat.Format = "#,##0";
                            worksheet.Cell(currentRow, 7).Style.NumberFormat.Format = "#,##0";
                            worksheet.Cell(currentRow, 8).Style.NumberFormat.Format = "#,##0";
                            worksheet.Cell(currentRow, 9).Style.NumberFormat.Format = "#,##0";
                            worksheet.Cell(currentRow, 10).Style.NumberFormat.Format = "#,##0";
                            worksheet.Cell(currentRow, 11).Style.NumberFormat.Format = "#,##0";

                            totalSalary += item.TongLuong;
                            currentRow++;
                        }
                    }

                    // Total row
                    worksheet.Cell(currentRow, 10).Value = "TỔNG CỘNG:";
                    worksheet.Cell(currentRow, 10).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, 11).Value = totalSalary;
                    worksheet.Cell(currentRow, 11).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, 11).Style.NumberFormat.Format = "#,##0";

                    // Auto-fit columns
                    worksheet.Columns().AdjustToContents();

                    // Borders
                    var dataRange = worksheet.Range(headerRow, 1, currentRow, 12);
                    dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    workbook.SaveAs(filePath);
                }

                Cursor = Cursors.Default;

                MessageBox.Show("Xuất Excel thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Open file
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show($"Lỗi xuất Excel: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }

    #region Helper Classes
    internal class SalaryViewModel
    {
        public int MaNV { get; set; }
        public string TenNV { get; set; }
        public string ChucVu { get; set; }
        public int? MaNhom { get; set; }
        public int SoNgayLam { get; set; }
        public decimal TongGioLam { get; set; }
        public decimal LuongTheoGio { get; set; }
        public decimal LuongCoBan { get; set; }
        public decimal PhuCap { get; set; }
        public decimal Thuong { get; set; }
        public decimal Phat { get; set; }
        public decimal TongLuong { get; set; }
        public string TrangThai { get; set; }
    }

    internal class ComboBoxItem
    {
        public int? Value { get; set; }
        public string Text { get; set; }

        public override string ToString()
        {
            return Text ?? "";
        }
    }
    #endregion
}