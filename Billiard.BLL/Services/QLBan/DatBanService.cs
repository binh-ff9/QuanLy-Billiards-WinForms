using Billiard.DAL.Data;
using Billiard.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Billiard.BLL.Services.QLBan
{
    public class DatBanService
    {
        private readonly BilliardDbContext _context;

        public DatBanService(BilliardDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả đặt bàn đang chờ
        public async Task<List<DatBan>> GetAllActiveAsync()
        {
            return await _context.DatBans
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b.MaKhuVucNavigation)
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b.MaLoaiNavigation)
                .Include(d => d.MaKhNavigation)
                .Where(d => d.TrangThai == "Đang chờ")
                .OrderBy(d => d.ThoiGianDat)
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