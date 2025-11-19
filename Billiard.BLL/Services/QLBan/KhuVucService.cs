using Billiard.DAL.Data;
using Billiard.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billiard.BLL.Services.QLBan
{
    public class KhuVucService
    {
        private readonly BilliardDbContext _context;

        public KhuVucService(BilliardDbContext context)
        {
            _context = context;
        }

        public async Task<List<KhuVuc>> GetAllAsync()
        {
            return await _context.KhuVucs
                .OrderBy(k => k.TenKhuVuc)
                .ToListAsync();
        }

        public async Task<KhuVuc> GetByIdAsync(int maKhuVuc)
        {
            return await _context.KhuVucs.FindAsync(maKhuVuc);
        }
    }
}
