using Billiard.DAL.Data;
using Billiard.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billiard.BLL.Services.HoaDonServices
{
    public class HoaDonService
    {
        private readonly BilliardDbContext _context;

        public HoaDonService(BilliardDbContext context)
        {
            _context = context;
        }

        public async Task<List<HoaDon>> GetTatCaHoaDonAsync()
        {
            return await _context.HoaDons
                .Include(h=> h.MaNvNavigation)
                .Include(h => h.MaKhNavigation)
                .Include(h=> h.MaBanNavigation)
                .OrderByDescending(h => h.ThoiGianBatDau) // Sắp xếp mới nhất lên đầu
                .ToListAsync();
        }




    }
}
