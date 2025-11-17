using Billiard.DAL.Data;
using Billiard.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Billiard.BLL.Services
{
    public class BanBiaService
    {
        private readonly BilliardDbContext _context;

        public BanBiaService(BilliardDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả bàn
        public async Task<List<BanBium>> GetAllTablesAsync()
        {
            return await _context.BanBia
                .Include(b => b.MaKhuVucNavigation)
                .Include(b => b.MaLoaiNavigation)
                .Include(b => b.MaKhNavigation)
                .Where(b => b.TrangThai != "Bảo trì")
                .OrderBy(b => b.TenBan)
                .ToListAsync();
        }

        // Lọc bàn theo điều kiện
        public async Task<List<BanBium>> FilterTablesAsync(string areaFilter, string statusFilter, string typeFilter, string searchText)
        {
            var query = _context.BanBia
                .Include(b => b.MaKhuVucNavigation)
                .Include(b => b.MaLoaiNavigation)
                .Include(b => b.MaKhNavigation)
                .Where(b => b.TrangThai != "Bảo trì")
                .AsQueryable();

            // Filter by area
            if (!string.IsNullOrEmpty(areaFilter) && areaFilter != "all")
            {
                query = query.Where(b => b.MaKhuVucNavigation.TenKhuVuc == areaFilter);
            }

            // Filter by status
            if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "all")
            {
                query = query.Where(b => b.TrangThai == statusFilter);
            }

            // Filter by type
            if (!string.IsNullOrEmpty(typeFilter) && typeFilter != "all")
            {
                query = query.Where(b => b.MaLoaiNavigation.TenLoai == typeFilter);
            }

            // Filter by search text
            if (!string.IsNullOrEmpty(searchText))
            {
                searchText = searchText.ToLower();
                query = query.Where(b => b.TenBan.ToLower().Contains(searchText));
            }

            return await query.OrderBy(b => b.TenBan).ToListAsync();
        }

        // Bắt đầu chơi bàn
        public async Task<bool> StartTableAsync(int maBan, int maNv, int? maKh = null)
        {
            try
            {
                var ban = await _context.BanBia.FindAsync(maBan);
                if (ban == null || ban.TrangThai != "Trống")
                    return false;

                ban.TrangThai = "Đang chơi";
                ban.GioBatDau = DateTime.Now;
                ban.MaKh = maKh;

                // Tạo hóa đơn mới
                var hoaDon = new HoaDon
                {
                    MaBan = maBan,
                    MaNv = maNv,
                    MaKh = maKh,
                    ThoiGianBatDau = DateTime.Now,
                    TrangThai = "Đang chơi",
                    TienBan = 0,
                    TienDichVu = 0,
                    TongTien = 0
                };

                _context.HoaDons.Add(hoaDon);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        // Tạm dừng bàn
        public async Task<bool> PauseTableAsync(int maBan)
        {
            try
            {
                var ban = await _context.BanBia.FindAsync(maBan);
                if (ban == null || ban.TrangThai != "Đang chơi")
                    return false;

                ban.TrangThai = "Trống";
                ban.GioBatDau = null;
                ban.MaKh = null;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Lấy thông tin bàn theo ID
        public async Task<BanBium> GetTableByIdAsync(int maBan)
        {
            return await _context.BanBia
                .Include(b => b.MaKhuVucNavigation)
                .Include(b => b.MaLoaiNavigation)
                .Include(b => b.MaKhNavigation)
                .FirstOrDefaultAsync(b => b.MaBan == maBan);
        }

        // Lấy hóa đơn đang hoạt động của bàn
        public async Task<HoaDon> GetActiveInvoiceAsync(int maBan)
        {
            return await _context.HoaDons
                .Where(h => h.MaBan == maBan && h.TrangThai == "Đang chơi")
                .FirstOrDefaultAsync();
        }

        // Thống kê bàn
        public async Task<(int trong, int dangChoi, int daDat)> GetTableStatsAsync()
        {
            var trong = await _context.BanBia.CountAsync(b => b.TrangThai == "Trống");
            var dangChoi = await _context.BanBia.CountAsync(b => b.TrangThai == "Đang chơi");
            var daDat = await _context.BanBia.CountAsync(b => b.TrangThai == "Đã đặt");

            return (trong, dangChoi, daDat);
        }

        // Lấy danh sách khu vực
        public async Task<List<string>> GetAreasAsync()
        {
            return await _context.KhuVucs
                .Select(k => k.TenKhuVuc)
                .Distinct()
                .ToListAsync();
        }

        // Lấy danh sách loại bàn
        public async Task<List<string>> GetTableTypesAsync()
        {
            return await _context.LoaiBans
                .Select(l => l.TenLoai)
                .Distinct()
                .ToListAsync();
        }
    }
}