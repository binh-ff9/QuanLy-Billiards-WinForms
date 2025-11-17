using System;
using System.Collections.Generic;
using System.Linq;
using Billiard.DAL.Data;
using Billiard.DAL.Entities;

namespace Billiard.BLL.Services
{
    public class MatHangService : IDisposable
    {
        private readonly BilliardDbContext _context;

        public MatHangService()
        {
            _context = new BilliardDbContext();
        }

        //public MatHangService(BilliardDbContext context)
        //{
        //    _context = context;
        //}

        // Lấy tất cả mặt hàng
        public List<MatHang> GetAllMatHang()
        {
            return _context.MatHangs
                .OrderBy(m => m.TenHang)
                .ToList();
        }

        // Lấy mặt hàng còn hàng (số lượng tồn > 0)
        public List<MatHang> GetMatHangConHang()
        {
            return _context.MatHangs
                .Where(m => m.SoLuongTon > 0)
                .OrderBy(m => m.TenHang)
                .ToList();
        }

        // Lấy mặt hàng theo ID
        public MatHang GetMatHangById(int maHang)
        {
            return _context.MatHangs.Find(maHang);
        }

        // Thêm mặt hàng mới
        public bool AddMatHang(MatHang matHang)
        {
            try
            {
                _context.MatHangs.Add(matHang);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding MatHang: {ex.Message}");
                return false;
            }
        }

        // Cập nhật mặt hàng
        public bool UpdateMatHang(MatHang matHang)
        {
            try
            {
                _context.MatHangs.Update(matHang);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating MatHang: {ex.Message}");
                return false;
            }
        }

        // Xóa mặt hàng
        public bool DeleteMatHang(int maHang)
        {
            try
            {
                var matHang = _context.MatHangs.Find(maHang);
                if (matHang != null)
                {
                    _context.MatHangs.Remove(matHang);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting MatHang: {ex.Message}");
                return false;
            }
        }

        // Dispose
        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}