using Billiard.DAL.Data;
using Billiard.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Billiard.BLL.Services.NhanVienService
{
    public class LichLamViecService
    {
        #region Get Methods
        /// <summary>
        /// Lấy lịch làm việc theo tuần
        /// </summary>
        public List<LichLamViec> GetScheduleByWeek(DateTime weekStart)
        {
            using (var context = new BilliardDbContext())
            {
                var startDate = DateOnly.FromDateTime(weekStart);
                var endDate = DateOnly.FromDateTime(weekStart.AddDays(7));

                return context.LichLamViecs
                    .Include(l => l.NhanVien)
                    .ThenInclude(n => n.MaNhomNavigation)
                    .Where(l => l.Ngay >= startDate && l.Ngay < endDate)
                    .OrderBy(l => l.Ngay)
                    .ThenBy(l => l.GioBatDau)
                    .ToList();
            }
        }

        /// <summary>
        /// Lấy lịch làm việc theo ngày
        /// </summary>
        public List<LichLamViec> GetScheduleByDate(DateOnly date)
        {
            using (var context = new BilliardDbContext())
            {
                return context.LichLamViecs
                    .Include(l => l.NhanVien)
                    .Where(l => l.Ngay == date)
                    .OrderBy(l => l.GioBatDau)
                    .ToList();
            }
        }

        /// <summary>
        /// Lấy lịch làm việc theo nhân viên trong tháng
        /// </summary>
        public List<LichLamViec> GetScheduleByEmployee(int maNv, int month, int year)
        {
            using (var context = new BilliardDbContext())
            {
                return context.LichLamViecs
                    .Include(l => l.NhanVien)
                    .Where(l => l.MaNv == maNv && l.Ngay.Month == month && l.Ngay.Year == year)
                    .OrderBy(l => l.Ngay)
                    .ThenBy(l => l.GioBatDau)
                    .ToList();
            }
        }

        /// <summary>
        /// Lấy lịch theo ngày và giờ cụ thể
        /// </summary>
        public List<LichLamViec> GetScheduleByDateAndHour(DateOnly date, int hour)
        {
            using (var context = new BilliardDbContext())
            {
                return context.LichLamViecs
                    .Include(l => l.NhanVien)
                    .Where(l => l.Ngay == date
                             && l.GioBatDau.Hour <= hour
                             && l.GioKetThuc.Hour > hour)
                    .ToList();
            }
        }

        /// <summary>
        /// Lấy lịch theo ID
        /// </summary>
        public LichLamViec GetById(int id)
        {
            using (var context = new BilliardDbContext())
            {
                return context.LichLamViecs
                    .Include(l => l.NhanVien)
                    .FirstOrDefault(l => l.Id == id);
            }
        }

        /// <summary>
        /// Lấy nhân viên đang làm việc tại thời điểm hiện tại
        /// </summary>
        public List<LichLamViec> GetCurrentWorkingEmployees()
        {
            using (var context = new BilliardDbContext())
            {
                var today = DateOnly.FromDateTime(DateTime.Now);
                var currentHour = DateTime.Now.Hour;

                return context.LichLamViecs
                    .Include(l => l.NhanVien)
                    .Where(l => l.Ngay == today
                             && l.GioBatDau.Hour <= currentHour
                             && l.GioKetThuc.Hour > currentHour)
                    .ToList();
            }
        }
        #endregion

        #region CRUD Methods
        /// <summary>
        /// Thêm lịch làm việc mới
        /// </summary>
        public bool AddSchedule(LichLamViec schedule)
        {
            if (schedule.MaNv <= 0)
                throw new ArgumentException("Mã nhân viên không hợp lệ");

            if (schedule.GioBatDau >= schedule.GioKetThuc)
                throw new ArgumentException("Giờ bắt đầu phải nhỏ hơn giờ kết thúc");

            using (var context = new BilliardDbContext())
            {
                // Kiểm tra trùng lịch
                var exists = context.LichLamViecs.Any(l =>
                    l.MaNv == schedule.MaNv &&
                    l.Ngay == schedule.Ngay &&
                    ((l.GioBatDau <= schedule.GioBatDau && l.GioKetThuc > schedule.GioBatDau) ||
                     (l.GioBatDau < schedule.GioKetThuc && l.GioKetThuc >= schedule.GioKetThuc) ||
                     (l.GioBatDau >= schedule.GioBatDau && l.GioKetThuc <= schedule.GioKetThuc)));

                if (exists)
                    throw new ArgumentException("Nhân viên đã có lịch trong khung giờ này");

                schedule.NgayTao = DateTime.Now;
                context.LichLamViecs.Add(schedule);
                return context.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// Thêm nhiều lịch cùng lúc
        /// </summary>
        public bool AddMultipleSchedules(List<LichLamViec> schedules)
        {
            using (var context = new BilliardDbContext())
            {
                foreach (var schedule in schedules)
                {
                    schedule.NgayTao = DateTime.Now;
                }
                context.LichLamViecs.AddRange(schedules);
                return context.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// Cập nhật lịch làm việc
        /// </summary>
        public bool UpdateSchedule(LichLamViec schedule)
        {
            using (var context = new BilliardDbContext())
            {
                var existing = context.LichLamViecs.Find(schedule.Id);
                if (existing == null)
                    throw new ArgumentException("Không tìm thấy lịch làm việc");

                existing.MaNv = schedule.MaNv;
                existing.Ngay = schedule.Ngay;
                existing.GioBatDau = schedule.GioBatDau;
                existing.GioKetThuc = schedule.GioKetThuc;
                existing.Ca = schedule.Ca;
                existing.TrangThai = schedule.TrangThai;
                existing.GhiChu = schedule.GhiChu;
                existing.NgayCapNhat = DateTime.Now;
                existing.NguoiCapNhat = schedule.NguoiCapNhat;

                return context.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// Xóa lịch làm việc theo ID
        /// </summary>
        public bool DeleteSchedule(int id)
        {
            using (var context = new BilliardDbContext())
            {
                var schedule = context.LichLamViecs.Find(id);
                if (schedule == null)
                    return false;

                context.LichLamViecs.Remove(schedule);
                return context.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// Xóa lịch theo ngày và khung giờ
        /// </summary>
        public bool DeleteSchedulesByDateAndTime(DateOnly date, TimeOnly startTime, TimeOnly endTime)
        {
            using (var context = new BilliardDbContext())
            {
                var schedules = context.LichLamViecs
                    .Where(l => l.Ngay == date && l.GioBatDau == startTime && l.GioKetThuc == endTime)
                    .ToList();

                if (!schedules.Any())
                    return false;

                context.LichLamViecs.RemoveRange(schedules);
                return context.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// Lưu lịch cho một khung giờ cụ thể (xóa cũ, thêm mới)
        /// </summary>
        public bool SaveScheduleForTimeSlot(DateOnly date, TimeOnly startTime, TimeOnly endTime,
            string shift, List<int> employeeIds, int createdBy)
        {
            using (var context = new BilliardDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // Xóa lịch cũ cho khung giờ này
                        var oldSchedules = context.LichLamViecs
                            .Where(l => l.Ngay == date && l.GioBatDau == startTime && l.GioKetThuc == endTime)
                            .ToList();

                        context.LichLamViecs.RemoveRange(oldSchedules);

                        // Thêm lịch mới cho từng nhân viên được chọn
                        foreach (var empId in employeeIds)
                        {
                            var newSchedule = new LichLamViec
                            {
                                MaNv = empId,
                                Ngay = date,
                                GioBatDau = startTime,
                                GioKetThuc = endTime,
                                Ca = shift,
                                TrangThai = "DaXepLich",
                                NgayTao = DateTime.Now,
                                NguoiTao = createdBy
                            };
                            context.LichLamViecs.Add(newSchedule);
                        }

                        var result = context.SaveChanges();
                        transaction.Commit();

                        return result > 0 || oldSchedules.Any(); // Trả về true nếu có thay đổi
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Xóa tất cả lịch của một nhân viên trong ngày
        /// </summary>
        public bool DeleteEmployeeScheduleForDate(int maNv, DateOnly date)
        {
            using (var context = new BilliardDbContext())
            {
                var schedules = context.LichLamViecs
                    .Where(l => l.MaNv == maNv && l.Ngay == date)
                    .ToList();

                if (!schedules.Any())
                    return false;

                context.LichLamViecs.RemoveRange(schedules);
                return context.SaveChanges() > 0;
            }
        }
        #endregion

        #region Statistics
        /// <summary>
        /// Thống kê ca làm và giờ làm trong tháng của nhân viên
        /// </summary>
        public (int totalShifts, decimal totalHours) GetMonthlyStats(int maNv, int month, int year)
        {
            using (var context = new BilliardDbContext())
            {
                var schedules = context.LichLamViecs
                    .Where(l => l.MaNv == maNv && l.Ngay.Month == month && l.Ngay.Year == year)
                    .ToList();

                int totalShifts = schedules.Count;
                decimal totalHours = schedules.Sum(s =>
                    (decimal)(s.GioKetThuc.Hour - s.GioBatDau.Hour) +
                    (s.GioKetThuc.Minute - s.GioBatDau.Minute) / 60m);

                return (totalShifts, totalHours);
            }
        }

        /// <summary>
        /// Đếm số ca theo nhân viên trong khoảng thời gian
        /// </summary>
        public Dictionary<string, int> GetShiftCountByEmployee(DateOnly startDate, DateOnly endDate)
        {
            using (var context = new BilliardDbContext())
            {
                return context.LichLamViecs
                    .Include(l => l.NhanVien)
                    .Where(l => l.Ngay >= startDate && l.Ngay <= endDate)
                    .GroupBy(l => l.NhanVien.TenNv)
                    .ToDictionary(g => g.Key, g => g.Count());
            }
        }

        /// <summary>
        /// Thống kê số nhân viên làm việc theo ngày trong tuần
        /// </summary>
        public Dictionary<DateOnly, int> GetEmployeeCountByDateInWeek(DateTime weekStart)
        {
            using (var context = new BilliardDbContext())
            {
                var startDate = DateOnly.FromDateTime(weekStart);
                var endDate = DateOnly.FromDateTime(weekStart.AddDays(7));

                return context.LichLamViecs
                    .Where(l => l.Ngay >= startDate && l.Ngay < endDate)
                    .GroupBy(l => l.Ngay)
                    .ToDictionary(g => g.Key, g => g.Select(x => x.MaNv).Distinct().Count());
            }
        }

        /// <summary>
        /// Kiểm tra nhân viên có lịch làm việc trong khung giờ không
        /// </summary>
        public bool HasConflictingSchedule(int maNv, DateOnly date, TimeOnly startTime, TimeOnly endTime, int? excludeId = null)
        {
            using (var context = new BilliardDbContext())
            {
                var query = context.LichLamViecs
                    .Where(l => l.MaNv == maNv && l.Ngay == date);

                if (excludeId.HasValue)
                    query = query.Where(l => l.Id != excludeId.Value);

                return query.Any(l =>
                    (l.GioBatDau <= startTime && l.GioKetThuc > startTime) ||
                    (l.GioBatDau < endTime && l.GioKetThuc >= endTime) ||
                    (l.GioBatDau >= startTime && l.GioKetThuc <= endTime));
            }
        }
        #endregion
    }
}