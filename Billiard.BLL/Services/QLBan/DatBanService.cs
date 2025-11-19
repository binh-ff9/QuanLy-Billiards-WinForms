using Billiard.DAL.Data;
using Billiard.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Billiard.BLL.Services.QLBan
{
    public class DatBanService
    {
        private readonly BilliardDbContext _context;

        public DatBanService(BilliardDbContext context)
        {
            _context = context;
        }

        public async Task<List<DatBan>> GetAllActiveAsync()
        {
            return await _context.DatBans
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b.MaKhuVucNavigation)
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b.MaLoaiNavigation)
                .Include(d => d.MaKhNavigation)
                .Where(d => d.TrangThai == "Đang chờ" || d.TrangThai == "Đã đặt") // CẬP NHẬT: Lấy cả "Đã đặt"
                .OrderBy(d => d.ThoiGianBatDau)
                .ToListAsync();
        }

        // Lấy đặt bàn theo ID
        public async Task<DatBan> GetByIdAsync(int maDat)
        {
            return await _context.DatBans
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b.MaKhuVucNavigation)
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b.MaLoaiNavigation)
                .Include(d => d.MaKhNavigation)
                .FirstOrDefaultAsync(d => d.MaDat == maDat);
        }

        // Lấy đặt bàn theo bàn
        public async Task<List<DatBan>> GetByTableAsync(int maBan)
        {
            return await _context.DatBans
                .Include(d => d.MaKhNavigation)
                .Where(d => d.MaBan == maBan && d.TrangThai == "Đang chờ")
                .OrderBy(d => d.ThoiGianDat)
                .ToListAsync();
        }

        // Lấy đặt bàn theo khách hàng
        public async Task<List<DatBan>> GetByCustomerAsync(int maKh)
        {
            return await _context.DatBans
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b.MaKhuVucNavigation)
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b.MaLoaiNavigation)
                .Where(d => d.MaKh == maKh && d.TrangThai == "Đang chờ")
                .OrderBy(d => d.ThoiGianDat)
                .ToListAsync();
        }

        // Lấy đặt bàn theo khoảng thời gian
        public async Task<List<DatBan>> GetByDateRangeAsync(DateTime tuNgay, DateTime denNgay)
        {
            return await _context.DatBans
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b.MaKhuVucNavigation)
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b.MaLoaiNavigation)
                .Include(d => d.MaKhNavigation)
                .Where(d => d.ThoiGianDat >= tuNgay && d.ThoiGianDat <= denNgay)
                .OrderBy(d => d.ThoiGianDat)
                .ToListAsync();
        }

        // Kiểm tra bàn đã được đặt trong khoảng thời gian
        public async Task<bool> IsTableReservedAsync(int maBan, DateTime thoiGianBatDau, DateTime thoiGianKetThuc)
        {
            return await _context.DatBans
                .AnyAsync(d => d.MaBan == maBan
                    && d.TrangThai == "Đang chờ"
                    && d.ThoiGianBatDau < thoiGianKetThuc
                    && d.ThoiGianKetThuc > thoiGianBatDau);
        }
        public async Task<KhachHang> GetCustomerByPhoneNumberAsync(string sdt)
        {
            return await _context.KhachHangs
                .FirstOrDefaultAsync(k => k.Sdt == sdt);
        }
        public async Task<bool> ReserveTableAsync(
            int maBan,
            int? maKhachHang,
            string tenKhach,
            string sdt,
            DateTime thoiGianBatDau,
            DateTime thoiGianKetThuc, // Thêm giờ kết thúc (Đã có trong file gốc, nhưng xác nhận lại)
            string ghiChu
        )
        {
            try
            {
                // 1. Nếu maKhachHang là null và chưa có khách hàng, tạo khách hàng mới tạm thời
                int maKh = maKhachHang ?? 0;
                if (!maKhachHang.HasValue)
                {
                    // Kiểm tra lại lần cuối xem có khách hàng này chưa
                    var existingCustomer = await GetCustomerByPhoneNumberAsync(sdt);
                    if (existingCustomer != null)
                    {
                        maKh = existingCustomer.MaKh;
                    }
                    else
                    {
                        // Tạo khách hàng mới (vãng lai/tạm thời)
                        var newCustomer = new KhachHang
                        {
                            TenKh = tenKhach,
                            Sdt = sdt,
                            NgayDangKy = DateTime.Now,
                            // Các trường khác sẽ là giá trị mặc định (HoatDong = false, HangTv = "Đồng", ...)
                        };
                        _context.KhachHangs.Add(newCustomer);
                        await _context.SaveChangesAsync(); // Lưu để lấy MaKh
                        maKh = newCustomer.MaKh;
                    }
                }

                // 2. Tạo đối tượng đặt bàn mới
                var datBan = new DatBan
                {
                    MaBan = maBan,
                    MaKh = maKh,
                    TenKhach = tenKhach, // Tên khách hàng (cho dù là khách TV hay vãng lai)
                    Sdt = sdt,
                    ThoiGianBatDau = thoiGianBatDau,
                    ThoiGianKetThuc = thoiGianKetThuc, // Sử dụng giờ kết thúc
                    GhiChu = ghiChu,
                    ThoiGianDat = DateTime.Now,
                    TrangThai = "Đang chờ", // Mặc định là Đang chờ
                    SoNguoi = 1 // Giả sử mặc định là 1 người, cần thêm trường này vào Form nếu muốn nhập
                };

                _context.DatBans.Add(datBan);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (tùy chọn)
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public async Task<List<BanBium>> GetAvailableTablesForReservationAsync(DateTime thoiGianBatDau, DateTime thoiGianKetThuc)
        {
            // 1. Lấy danh sách MaBan đã được đặt và đang "Đang chờ" trong khoảng thời gian
            // Điều kiện xung đột: (d.ThoiGianBatDau < thoiGianKetThuc) VÀ (d.ThoiGianKetThuc > thoiGianBatDau)
            var reservedTableIds = await _context.DatBans
                .Where(d => d.TrangThai == "Đang chờ"
                    && d.ThoiGianBatDau < thoiGianKetThuc
                    && d.ThoiGianKetThuc > thoiGianBatDau)
                .Select(d => d.MaBan)
                .Distinct()
                .ToListAsync();

            // 2. Lấy danh sách BanBia có trạng thái "Trống" VÀ không nằm trong danh sách MaBan đã đặt
            var availableTables = await _context.BanBia
                .Include(b => b.MaKhuVucNavigation)
                .Include(b => b.MaLoaiNavigation)
                .Where(b => b.TrangThai == "Trống" // Trạng thái phải là Trống (hoặc không phải Đang chơi/Bảo trì/Đã đặt)
                    && !reservedTableIds.Contains(b.MaBan))
                .OrderBy(b => b.TenBan)
                .ToListAsync();

            return availableTables;
        }
        // Cập nhật trạng thái đặt bàn
        public async Task<bool> UpdateStatusAsync(int maDat, string trangThai)
        {
            try
            {
                var datBan = await _context.DatBans.FindAsync(maDat);
                if (datBan == null)
                    return false;

                datBan.TrangThai = trangThai;
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        // Cập nhật thông tin đặt bàn
        public async Task<bool> UpdateReservationAsync(DatBan datBan)
        {
            try
            {
                var existing = await _context.DatBans.FindAsync(datBan.MaDat);
                if (existing == null)
                    return false;

                existing.TenKhach = datBan.TenKhach;
                existing.Sdt = datBan.Sdt;
                existing.ThoiGianDat = datBan.ThoiGianDat;
                existing.ThoiGianBatDau = datBan.ThoiGianBatDau;
                existing.ThoiGianKetThuc = datBan.ThoiGianKetThuc;
                existing.SoNguoi = datBan.SoNguoi;
                existing.GhiChu = datBan.GhiChu;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Thống kê đặt bàn
        public async Task<(int dangCho, int daXacNhan, int daHuy)> GetReservationStatsAsync()
        {
            var dangCho = await _context.DatBans.CountAsync(d => d.TrangThai == "Đang chờ");
            var daXacNhan = await _context.DatBans.CountAsync(d => d.TrangThai == "Đã xác nhận");
            var daHuy = await _context.DatBans.CountAsync(d => d.TrangThai == "Đã hủy");

            return (dangCho, daXacNhan, daHuy);
        }
    }
}