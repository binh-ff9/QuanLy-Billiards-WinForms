using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Billiard.BLL.Services.NhanVienService;
using Billiard.DAL.Entities;

namespace Billiard.WinForm.Forms.NhanVien
{
    // Dùng partial class
    public partial class SalaryDetailForm : Form
    {
        private readonly NhanVienService _nhanVienService;
        private readonly DAL.Entities.NhanVien _employee;
        private readonly int _month;
        private readonly int _year;

        // Khai báo controls đã được chuyển sang Designer.cs (Đã xóa ở đây)
        // private NumericUpDown nudBonus;
        // ...

        // Khai báo các control mà CreateEmployeeHeader() tạo ra để dễ truy cập
        private Label _lblInitial;
        private Label _lblName;
        private Label _lblRole;
        private Label _lblPeriod;

        public SalaryDetailForm(NhanVienService service, DAL.Entities.NhanVien employee, int month, int year)
        {
            _nhanVienService = service;
            _employee = employee;
            _month = month;
            _year = year;

            InitializeComponent();

            // Gán các Label từ Panel Header sau khi đã InitializeComponent
            var headerTags = pnlHeader.Tag as dynamic;
            _lblInitial = headerTags.lblInitial;
            _lblName = headerTags.lblName;
            _lblRole = headerTags.lblRole;
            _lblPeriod = headerTags.lblPeriod;

            // Cập nhật text của form và header ngay lập tức
            this.Text = $"Chi tiết lương - {_employee.TenNv}";
            _lblInitial.Text = _employee.TenNv?.Substring(0, 1).ToUpper() ?? "?";
            _lblName.Text = _employee.TenNv;
            _lblRole.Text = $"👔 {_employee.MaNhomNavigation?.TenNhom ?? "N/A"} • Ca {_employee.CaMacDinh}";
            _lblPeriod.Text = $"📅 Tháng {_month}/{_year}";

            LoadData();
        }

        // Phương thức InitializeCustomControls, CreateEmployeeHeader, CreateSalaryPanel, 
        // AddInfoRow, CreateAttendanceGrid, CreateActionPanel đã được chuyển sang Designer.cs

        private void LoadData()
        {
            try
            {
                dgvAttendance.Rows.Clear();

                var attendances = _nhanVienService.GetAttendanceByMonth(_employee.MaNv, _month, _year);

                // 1. Cập nhật Attendance Grid
                foreach (var att in attendances.OrderBy(a => a.Ngay))
                {
                    string statusText = att.TrangThai == "DiTre" ? "⚠️ Đi trễ" : "✅ Đúng giờ";

                    dgvAttendance.Rows.Add(
                        att.Ngay.ToString("dd/MM/yyyy (ddd)"),
                        att.GioVao?.ToString("HH:mm") ?? "-",
                        att.GioRa?.ToString("HH:mm") ?? "-",
                        att.SoGioLam ?? 0,
                        statusText,
                        att.GhiChu ?? ""
                    );

                    // Color row if late
                    if (att.TrangThai == "DiTre")
                    {
                        dgvAttendance.Rows[dgvAttendance.Rows.Count - 1].DefaultCellStyle.BackColor = Color.FromArgb(254, 243, 199);
                    }
                }

                // 2. Cập nhật Salary Panel Info
                int workDays = attendances.Count;
                decimal totalHours = attendances.Sum(a => a.SoGioLam ?? 0);
                // Đảm bảo LuongCoBan không null trước khi chia
                decimal baseSalary = _employee.LuongCoBan ?? 0;
                decimal hourlyRate = baseSalary / (baseSalary > 0 ? 176m : 1m);
                decimal baseSalaryByHour = totalHours * hourlyRate;
                decimal phuCap = _employee.PhuCap ?? 0;

                // Cập nhật giá trị Label trong pnlSalary
                foreach (Control control in pnlSalary.Controls)
                {
                    if (control is Label lbl && lbl.Tag != null)
                    {
                        switch (lbl.Tag.ToString())
                        {
                            case "Số ngày làm việc:":
                                lbl.Text = $"{workDays} ngày";
                                break;
                            case "Tổng giờ làm việc:":
                                lbl.Text = $"{totalHours:N2} giờ";
                                break;
                            case "Lương theo giờ:":
                                lbl.Text = $"{hourlyRate:N0}đ/giờ";
                                break;
                            case "Lương cơ bản (theo giờ):":
                                lbl.Text = $"{baseSalaryByHour:N0}đ";
                                break;
                            case "Phụ cấp:":
                                lbl.Text = $"{phuCap:N0}đ";
                                break;
                        }
                    }
                }

                // Auto-calculate penalty for late days
                int lateDays = attendances.Count(a => a.TrangThai == "DiTre");
                nudPenalty.Value = lateDays * 50000;

                // Load existing salary data if available (sau khi auto-calc)
                var salary = _nhanVienService.GetLatestSalary(_employee.MaNv);
                if (salary != null && salary.Thang == _month && salary.Nam == _year)
                {
                    nudBonus.Value = salary.Thuong ?? 0;
                    // Chỉ ghi đè Phat nếu Phat đã được lưu
                    if (salary.Phat.HasValue && salary.Phat > 0)
                    {
                        nudPenalty.Value = salary.Phat.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Calculate salary (this part remains the same)
                _nhanVienService.CalculateMonthlySalary(_employee.MaNv, _month, _year);

                // Update bonus and penalty
                using (var context = new Billiard.DAL.Data.BilliardDbContext())
                {
                    var salary = context.BangLuongs
                        .FirstOrDefault(b => b.MaNv == _employee.MaNv && b.Thang == _month && b.Nam == _year);

                    if (salary != null)
                    {
                        salary.Thuong = nudBonus.Value;
                        salary.Phat = nudPenalty.Value;
                        salary.TongLuong = (salary.LuongCoBan ?? 0) + (salary.PhuCap ?? 0) + (salary.Thuong ?? 0) - (salary.Phat ?? 0);
                        salary.NgayTinh = DateTime.Now;
                        context.SaveChanges();
                    }
                }

                MessageBox.Show(
                    "✅ Đã lưu bảng lương thành công!",
                    "Thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}