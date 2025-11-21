using Billiard.DAL.Data;
using Billiard.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Billiard.BLL.Services.KhachHangServices
{
    public class KhachHangService
    {
        private readonly BilliardDbContext _context;

        public KhachHangService(BilliardDbContext context)
        {
            _context = context;
        }

        // GET :: DANH SÁCH
        public async Task<List<KhachHang>> GetListKhachHangAsync(string keyword = "", string rank = "Tất cả", bool isDeleted = false)
        {
            var query = _context.KhachHangs.AsQueryable();

            if (isDeleted)
            {
                query = query.Where(k => k.HoatDong == false);
            }
            else
            {
                query = query.Where(k => k.HoatDong == true || k.HoatDong == null);
            }

            // Lọc tên
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower();
                query = query.Where(k => k.TenKh.ToLower().Contains(keyword) ||
                                         k.Sdt.Contains(keyword) ||
                                         k.Email.Contains(keyword));
            }

            // Lọc rank
            if (rank != "Tất cả")
            {
                switch (rank)
                {
                    case "Bạch Kim": 
                        query = query.Where(k => k.DiemTichLuy > 300);
                        break;
                    case "Vàng": 
                        query = query.Where(k => k.DiemTichLuy > 150 && k.DiemTichLuy <= 300);
                        break;
                    case "Bạc": 
                        query = query.Where(k => k.DiemTichLuy > 70 && k.DiemTichLuy <= 150);
                        break;
                    case "Đồng": 
                        query = query.Where(k => k.DiemTichLuy <= 70);
                        break;
                }

            }
           
            // Sắp xếp tên A-Z
            return await query.Include(k => k.HoaDons)
                                      .AsNoTracking()
                                      .OrderByDescending(k => k.DiemTichLuy) // Người điểm cao xếp trước
                                    .ToListAsync();   
        }

        public async Task<KhachHang> GetKhachHangDetailAsync(int maKh)
        {
            return await _context.KhachHangs
                .Include(k => k.HoaDons) // Load lịch sử hóa đơn
                    .ThenInclude(h => h.MaBanNavigation) // Để hiện tên bàn đã chơi
                .FirstOrDefaultAsync(k => k.MaKh == maKh);
        }

        // 3. Thêm / Sửa / Xóa (Cơ bản)
        public async Task AddAsync(KhachHang kh)
        { 
            _context.KhachHangs.Add(kh); await _context.SaveChangesAsync(); 
        }

        public async Task UpdateAsync(KhachHang kh) 
        { 
            _context.KhachHangs.Update(kh); await _context.SaveChangesAsync(); 
        }

        public async Task ToggleStatusAsync(int maKh, bool isActive)
        {
            var kh = await _context.KhachHangs.FindAsync(maKh);
            if (kh != null)
            {
                kh.HoatDong = isActive; // true = khôi phục, false = xóa mềm
                await _context.SaveChangesAsync();
            }
        }

    }
}