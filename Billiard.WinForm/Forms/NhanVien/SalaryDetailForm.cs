using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Billiard.BLL.Services.NhanVienService;
using Billiard.DAL.Entities;

namespace Billiard.WinForm.Forms.NhanVien
{
    public partial class SalaryDetailForm : Form
    {
        #region Fields
        private readonly NhanVienService _nhanVienService;
        private readonly DAL.Entities.NhanVien _employee;
        private readonly int _month;
        private readonly int _year;
        #endregion

        #region Constructor
        public SalaryDetailForm(NhanVienService nhanVienService, DAL.Entities.NhanVien employee, int month, int year)
        {
            _nhanVienService = nhanVienService;
            _employee = employee;
            _month = month;
            _year = year;

            InitializeComponent();
            InitializeCustomSettings();
            LoadData();
        }
        #endregion

        #region Initialize
        private void InitializeCustomSettings()
        {
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            // Setup value changed events
            numThuong.ValueChanged += CalculateTotalSalary;
            numPhat.ValueChanged += CalculateTotalSalary;

            // Format DataGridView
            FormatDataGridView();
        }

        private void FormatDataGridView()
        {
            colSoGio.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colSoGio.DefaultCellStyle.Format = "N2";
            colTrangThai.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
        #endregion

        #region Load Data
        private void LoadData()
        {
            try
            {
                // Load employee info
                txtMaNV.Text = _employee.MaNv.ToString();
                txtTenNV.Text = _employee.TenNv;
                txtChucVu.Text = _employee.MaNhomNavigation?.TenNhom ?? "N/A";

                // Load attendance data
                var attendances = _nhanVienService.GetAttendanceByMonth(_employee.MaNv, _month, _year);

                int workDays = attendances.Count;
                decimal totalHours = attendances.Sum(a => a.SoGioLam ?? 0);

                txtSoNgay.Text = workDays.ToString();
                txtTongGio.Text = $"{totalHours:N2} giờ";

                // Calculate salary
                decimal baseSalary = _employee.LuongCoBan ?? 0;
                decimal hourlyRate = baseSalary / 176m;
                decimal salaryByHour = totalHours * hourlyRate;

                numLuongCoBan.Value = salaryByHour;
                numPhuCap.Value = _employee.PhuCap ?? 0;

                // Load existing salary record if available
                var salaryRecord = _nhanVienService.GetLatestSalary(_employee.MaNv);
                if (salaryRecord != null && salaryRecord.Thang == _month && salaryRecord.Nam == _year)
                {
                    numThuong.Value = salaryRecord.Thuong ?? 0;
                    numPhat.Value = salaryRecord.Phat ?? 0;
                }
                else
                {
                    // Calculate penalty for late days
                    int lateDays = attendances.Count(a => a.TrangThai == "DiTre");
                    numPhat.Value = lateDays * 50000;
                    numThuong.Value = 0;
                }

                // Calculate total
                CalculateTotalSalary(null, null);

                // Load attendance details
                LoadAttendanceDetails(attendances);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAttendanceDetails(System.Collections.Generic.List<ChamCong> attendances)
        {
            dgvAttendance.Rows.Clear();

            foreach (var attendance in attendances.OrderBy(a => a.Ngay))
            {
                string gioVao = attendance.GioVao?.ToString("HH:mm:ss") ?? "N/A";
                string gioRa = attendance.GioRa?.ToString("HH:mm:ss") ?? "Chưa checkout";
                decimal soGio = attendance.SoGioLam ?? 0;
                string trangThai = attendance.TrangThai ?? "N/A";

                int rowIndex = dgvAttendance.Rows.Add(
                    attendance.Ngay.ToString("dd/MM/yyyy"),
                    gioVao,
                    gioRa,
                    soGio,
                    trangThai
                );

                // Color code status
                var statusCell = dgvAttendance.Rows[rowIndex].Cells["colTrangThai"];
                switch (trangThai)
                {
                    case "DungGio":
                        statusCell.Style.BackColor = Color.FromArgb(220, 252, 231);
                        statusCell.Style.ForeColor = Color.FromArgb(21, 128, 61);
                        statusCell.Value = "Đúng giờ";
                        break;
                    case "DiTre":
                        statusCell.Style.BackColor = Color.FromArgb(254, 226, 226);
                        statusCell.Style.ForeColor = Color.FromArgb(185, 28, 28);
                        statusCell.Value = "Đi trễ";
                        break;
                    default:
                        statusCell.Style.BackColor = Color.FromArgb(254, 243, 199);
                        statusCell.Style.ForeColor = Color.FromArgb(161, 98, 7);
                        break;
                }
            }
        }
        #endregion

        #region Event Handlers
        private void CalculateTotalSalary(object sender, EventArgs e)
        {
            decimal total = numLuongCoBan.Value + numPhuCap.Value + numThuong.Value - numPhat.Value;
            numTongLuong.Value = total;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                $"Xác nhận lưu thông tin lương cho nhân viên {_employee.TenNv}?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                Cursor = Cursors.WaitCursor;

                // Update or create salary record
                using (var context = new Billiard.DAL.Data.BilliardDbContext())
                {
                    var existingSalary = context.BangLuongs
                        .FirstOrDefault(b => b.MaNv == _employee.MaNv && b.Thang == _month && b.Nam == _year);

                    var attendances = _nhanVienService.GetAttendanceByMonth(_employee.MaNv, _month, _year);
                    decimal totalHours = attendances.Sum(a => a.SoGioLam ?? 0);

                    if (existingSalary != null)
                    {
                        // Update existing
                        existingSalary.LuongCoBan = _employee.LuongCoBan ?? 0;
                        existingSalary.PhuCap = numPhuCap.Value;
                        existingSalary.Thuong = numThuong.Value;
                        existingSalary.Phat = numPhat.Value;
                        existingSalary.TongGio = totalHours;
                        existingSalary.TongLuong = numTongLuong.Value;
                        existingSalary.NgayTinh = DateTime.Now;
                    }
                    else
                    {
                        // Create new
                        var newSalary = new BangLuong
                        {
                            MaNv = _employee.MaNv,
                            Thang = _month,
                            Nam = _year,
                            LuongCoBan = _employee.LuongCoBan ?? 0,
                            PhuCap = numPhuCap.Value,
                            Thuong = numThuong.Value,
                            Phat = numPhat.Value,
                            TongGio = totalHours,
                            TongLuong = numTongLuong.Value,
                            NgayTinh = DateTime.Now
                        };
                        context.BangLuongs.Add(newSalary);
                    }

                    context.SaveChanges();
                }

                Cursor = Cursors.Default;

                MessageBox.Show("Lưu thông tin lương thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}