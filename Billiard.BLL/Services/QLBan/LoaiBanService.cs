using Billiard.DAL.Data;
using Billiard.DAL.Entities;
using Microsoft.EntityFrameworkCore; // Thêm dòng này
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billiard.BLL.Services.QLBan
{
    public class LoaiBanService
    {
        private readonly BilliardDbContext _context;

        public LoaiBanService(BilliardDbContext context)
        {
            _context = context;
        }

        public async Task<List<LoaiBan>> GetAllAsync()
        {
            return await _context.LoaiBans
                .OrderBy(l => l.TenLoai)
                .ToListAsync();
        }

        public async Task<LoaiBan> GetByIdAsync(int maLoai)
        {
            // FindAsync không cần ToListAsync, nhưng cần Microsoft.EntityFrameworkCore cho các phương thức khác như ToListAsync, Include, Where...
            return await _context.LoaiBans.FindAsync(maLoai);
        }
    }
}