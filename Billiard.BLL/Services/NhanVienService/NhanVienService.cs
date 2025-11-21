using Billiard.DAL.Entities;
using Billiard.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Billiard.BLL.Services.NhanVienService
{
    public class NhanVienService
    {
        #region Get Methods
        public List<DAL.Entities.NhanVien> GetAllEmployees()
        {
            using (var context = new BilliardDbContext())
            {
                return context.NhanViens
                    .Include(n => n.MaNhomNavigation)
                    .OrderBy(n => n.TenNv)
                    .ToList();
            }
        }

        public DAL.Entities.NhanVien GetEmployeeById(int maNV)
        {
            using (var context = new BilliardDbContext())
            {
                return context.NhanViens
                    .Include(n => n.MaNhomNavigation)
                    .FirstOrDefault(n => n.MaNv == maNV);
            }
        }

        public DAL.Entities.NhanVien GetEmployeeByPhone(string phone)
        {
            using (var context = new BilliardDbContext())
            {
                return context.NhanViens
                    .Include(n => n.MaNhomNavigation)
                    .FirstOrDefault(n => n.Sdt == phone);
            }
        }

        public List<DAL.Entities.NhanVien> GetEmployeesByStatus(string status)
        {
            using (var context = new BilliardDbContext())
            {
                return context.NhanViens
                    .Include(n => n.MaNhomNavigation)
                    .Where(n => n.TrangThai == status)
                    .OrderBy(n => n.TenNv)
                    .ToList();
            }
        }

        public List<DAL.Entities.NhanVien> GetEmployeesByRole(int maNhom)
        {
            using (var context = new BilliardDbContext())
            {
                return context.NhanViens
                    .Include(n => n.MaNhomNavigation)
                    .Where(n => n.MaNhom == maNhom)
                    .OrderBy(n => n.TenNv)
                    .ToList();
            }
        }

        public List<NhomQuyen> GetAllRoles()
        {
            using (var context = new BilliardDbContext())
            {
                return context.NhomQuyens
                    .OrderBy(n => n.TenNhom)
                    .ToList();
            }
        }
        #endregion

        #region CRUD Methods
        public bool AddEmployee(DAL.Entities.NhanVien employee)
        {
            if (string.IsNullOrEmpty(employee.TenNv))
                throw new ArgumentException("Tên nhân viên không được để trống");

            if (string.IsNullOrEmpty(employee.Sdt))
                throw new ArgumentException("Số điện thoại không được để trống");

            if (string.IsNullOrEmpty(employee.MatKhau))
                throw new ArgumentException("Mật khẩu không được để trống");

            using (var context = new BilliardDbContext())
            {
                // Check phone exist
                if (context.NhanViens.Any(n => n.Sdt == employee.Sdt))
                    throw new ArgumentException("Số điện thoại đã tồn tại trong hệ thống");

                // Set default values
                employee.TrangThai ??= "DangLam";
                employee.CaMacDinh ??= "Sang";
                employee.LuongCoBan ??= 0;
                employee.PhuCap ??= 0;

                context.NhanViens.Add(employee);
                return context.SaveChanges() > 0;
            }
        }

        public bool UpdateEmployee(DAL.Entities.NhanVien employee)
        {
            if (employee.MaNv <= 0)
                throw new ArgumentException("Mã nhân viên không hợp lệ");

            if (string.IsNullOrEmpty(employee.TenNv))
                throw new ArgumentException("Tên nhân viên không được để trống");

            using (var context = new BilliardDbContext())
            {
                var existing = context.NhanViens.Find(employee.MaNv);
                if (existing == null)
                    throw new ArgumentException("Không tìm thấy nhân viên");

                // Check phone duplicate (exclude current employee)
                if (context.NhanViens.Any(n => n.Sdt == employee.Sdt && n.MaNv != employee.MaNv))
                    throw new ArgumentException("Số điện thoại đã được sử dụng bởi nhân viên khác");

                // Update properties
                existing.TenNv = employee.TenNv;
                existing.Sdt = employee.Sdt;
                existing.Email = employee.Email;
                existing.MaNhom = employee.MaNhom;
                existing.CaMacDinh = employee.CaMacDinh;
                existing.TrangThai = employee.TrangThai;
                existing.LuongCoBan = employee.LuongCoBan;
                existing.PhuCap = employee.PhuCap;
                existing.FaceidAnh = employee.FaceidAnh;
                existing.FaceidHash = employee.FaceidHash;

                // Update password only if provided
                if (!string.IsNullOrEmpty(employee.MatKhau))
                {
                    existing.MatKhau = employee.MatKhau;
                }

                return context.SaveChanges() > 0;
            }
        }

        public bool DeleteEmployee(int maNV)
        {
            using (var context = new BilliardDbContext())
            {
                var employee = context.NhanViens.Find(maNV);
                if (employee == null)
                    throw new ArgumentException("Không tìm thấy nhân viên");

                // Soft delete - change status to "Nghi"
                employee.TrangThai = "Nghi";
                return context.SaveChanges() > 0;
            }
        }

        public bool HardDeleteEmployee(int maNV)
        {
            using (var context = new BilliardDbContext())
            {
                var employee = context.NhanViens.Find(maNV);
                if (employee == null) return false;

                context.NhanViens.Remove(employee);
                return context.SaveChanges() > 0;
            }
        }
        #endregion

        #region Attendance Methods
        public ChamCong GetTodayAttendance(int maNV)
        {
            using (var context = new BilliardDbContext())
            {
                var today = DateOnly.FromDateTime(DateTime.Now);
                return context.ChamCongs
                    .FirstOrDefault(c => c.MaNv == maNV && c.Ngay == today);
            }
        }

        public List<ChamCong> GetAttendanceByMonth(int maNV, int month, int year)
        {
            using (var context = new BilliardDbContext())
            {
                return context.ChamCongs
                    .Where(c => c.MaNv == maNV && c.Ngay.Month == month && c.Ngay.Year == year)
                    .OrderBy(c => c.Ngay)
                    .ToList();
            }
        }

        public bool CheckIn(int maNV, string xacThucBang = "ThuCong", string ghiChu = null, string hinhAnhVao = null)
        {
            using (var context = new BilliardDbContext())
            {
                var today = DateOnly.FromDateTime(DateTime.Now);
                var existing = context.ChamCongs
                    .FirstOrDefault(c => c.MaNv == maNV && c.Ngay == today);

                if (existing != null && existing.GioVao.HasValue)
                    throw new InvalidOperationException("Nhân viên đã check-in hôm nay");

                var chamCong = new ChamCong
                {
                    MaNv = maNV,
                    Ngay = today,
                    GioVao = DateTime.Now,
                    XacThucBang = xacThucBang,
                    GhiChu = ghiChu,
                    HinhAnhVao = hinhAnhVao,
                    TrangThai = DetermineCheckInStatus(DateTime.Now)
                };

                context.ChamCongs.Add(chamCong);
                return context.SaveChanges() > 0;
            }
        }

        public bool CheckOut(int maNV, string ghiChu = null, string hinhAnhRa = null)
        {
            using (var context = new BilliardDbContext())
            {
                var today = DateOnly.FromDateTime(DateTime.Now);
                var attendance = context.ChamCongs
                    .FirstOrDefault(c => c.MaNv == maNV && c.Ngay == today);

                if (attendance == null || !attendance.GioVao.HasValue)
                    throw new InvalidOperationException("Nhân viên chưa check-in hôm nay");

                if (attendance.GioRa.HasValue)
                    throw new InvalidOperationException("Nhân viên đã check-out hôm nay");

                attendance.GioRa = DateTime.Now;
                attendance.HinhAnhRa = hinhAnhRa;
                if (!string.IsNullOrEmpty(ghiChu))
                    attendance.GhiChu = (attendance.GhiChu ?? "") + " | " + ghiChu;

                return context.SaveChanges() > 0;
            }
        }

        private string DetermineCheckInStatus(DateTime checkInTime)
        {
            var hour = checkInTime.Hour;
            // Assuming morning shift starts at 8:00
            if (hour > 8) return "DiTre";
            return "DungGio";
        }
        #endregion

        #region Salary Methods
        public BangLuong GetLatestSalary(int maNV)
        {
            using (var context = new BilliardDbContext())
            {
                return context.BangLuongs
                    .Where(b => b.MaNv == maNV)
                    .OrderByDescending(b => b.Nam)
                    .ThenByDescending(b => b.Thang)
                    .FirstOrDefault();
            }
        }

        public List<BangLuong> GetSalaryByYear(int maNV, int year)
        {
            using (var context = new BilliardDbContext())
            {
                return context.BangLuongs
                    .Where(b => b.MaNv == maNV && b.Nam == year)
                    .OrderBy(b => b.Thang)
                    .ToList();
            }
        }

        public bool CalculateMonthlySalary(int maNV, int month, int year)
        {
            using (var context = new BilliardDbContext())
            {
                var employee = context.NhanViens.Find(maNV);
                if (employee == null)
                    throw new ArgumentException("Không tìm thấy nhân viên");

                var attendances = context.ChamCongs
                    .Where(c => c.MaNv == maNV && c.Ngay.Month == month && c.Ngay.Year == year)
                    .ToList();

                decimal totalHours = attendances.Sum(a => a.SoGioLam ?? 0);
                int lateDays = attendances.Count(a => a.TrangThai == "DiTre");

                // Check if salary already exists
                var existingSalary = context.BangLuongs
                    .FirstOrDefault(b => b.MaNv == maNV && b.Thang == month && b.Nam == year);

                if (existingSalary != null)
                {
                    existingSalary.LuongCoBan = employee.LuongCoBan ?? 0;
                    existingSalary.PhuCap = employee.PhuCap ?? 0;
                    existingSalary.TongGio = totalHours;
                    existingSalary.Phat = lateDays * 50000;
                    existingSalary.Thuong = 0;
                    existingSalary.NgayTinh = DateTime.Now;
                    existingSalary.TongLuong = existingSalary.LuongCoBan + existingSalary.PhuCap + existingSalary.Thuong - existingSalary.Phat;
                }
                else
                {
                    var salary = new BangLuong
                    {
                        MaNv = maNV,
                        Thang = month,
                        Nam = year,
                        LuongCoBan = employee.LuongCoBan ?? 0,
                        PhuCap = employee.PhuCap ?? 0,
                        TongGio = totalHours,
                        Phat = lateDays * 50000,
                        Thuong = 0,
                        NgayTinh = DateTime.Now
                    };
                    salary.TongLuong = salary.LuongCoBan + salary.PhuCap + salary.Thuong - salary.Phat;
                    context.BangLuongs.Add(salary);
                }

                return context.SaveChanges() > 0;
            }
        }
        #endregion

        #region Statistics Methods
        public (int totalDays, decimal totalHours, int lateDays) GetMonthlyStats(int maNV, int month, int year)
        {
            using (var context = new BilliardDbContext())
            {
                var attendances = context.ChamCongs
                    .Where(c => c.MaNv == maNV && c.Ngay.Month == month && c.Ngay.Year == year)
                    .ToList();

                int totalDays = attendances.Count;
                decimal totalHours = attendances.Sum(a => a.SoGioLam ?? 0);
                int lateDays = attendances.Count(a => a.TrangThai == "DiTre");
                return (totalDays, totalHours, lateDays);
            }
        }
        #endregion

        #region Activity Log
        public List<LichSuHoatDong> GetActivityHistory(int maNV, int count = 50)
        {
            using (var context = new BilliardDbContext())
            {
                return context.LichSuHoatDongs
                    .Where(l => l.MaNv == maNV)
                    .OrderByDescending(l => l.ThoiGian)
                    .Take(count)
                    .ToList();
            }
        }

        public bool LogActivity(int maNV, string action, string detail)
        {
            using (var context = new BilliardDbContext())
            {
                var log = new LichSuHoatDong
                {
                    MaNv = maNV,
                    HanhDong = action,
                    ChiTiet = detail,
                    ThoiGian = DateTime.Now
                };
                context.LichSuHoatDongs.Add(log);
                return context.SaveChanges() > 0;
            }
        }
        #endregion

        #region Password Methods
        public bool ChangePassword(int maNV, string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword) || newPassword.Length < 6)
                throw new ArgumentException("Mật khẩu phải có ít nhất 6 ký tự");

            using (var context = new BilliardDbContext())
            {
                var employee = context.NhanViens.Find(maNV);
                if (employee == null)
                    throw new ArgumentException("Không tìm thấy nhân viên");

                employee.MatKhau = newPassword;
                return context.SaveChanges() > 0;
            }
        }

        public bool ValidatePassword(int maNV, string password)
        {
            using (var context = new BilliardDbContext())
            {
                var employee = context.NhanViens.Find(maNV);
                if (employee == null) return false;
                return employee.MatKhau == password;
            }
        }
        #endregion

        #region FaceID Methods
        public bool UpdateFaceID(int maNV, string faceImagePath, string faceHash)
        {
            using (var context = new BilliardDbContext())
            {
                var employee = context.NhanViens.Find(maNV);
                if (employee == null)
                    throw new ArgumentException("Không tìm thấy nhân viên");

                employee.FaceidAnh = faceImagePath;
                employee.FaceidHash = faceHash;
                return context.SaveChanges() > 0;
            }
        }

        public bool DeleteFaceID(int maNV)
        {
            using (var context = new BilliardDbContext())
            {
                var employee = context.NhanViens.Find(maNV);
                if (employee == null)
                    throw new ArgumentException("Không tìm thấy nhân viên");

                employee.FaceidAnh = null;
                employee.FaceidHash = null;
                return context.SaveChanges() > 0;
            }
        }

        public DAL.Entities.NhanVien GetEmployeeByFaceHash(string faceHash)
        {
            using (var context = new BilliardDbContext())
            {
                return context.NhanViens
                    .Include(n => n.MaNhomNavigation)
                    .FirstOrDefault(n => n.FaceidHash == faceHash);
            }
        }
        #endregion
    }
}