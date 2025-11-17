using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Billiard.DAL.Data;
using Billiard.DAL.Entities;

namespace Billiard.BLL.Services
{
    public class DichVuService : IDisposable
    {
        private readonly BilliardDbContext _context;

        //public DichVuService()
        //{
        //    _context = new BilliardDbContext();
        //}

        public DichVuService(BilliardDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả dịch vụ
        public List<DichVu> GetAllDichVu()
        {
            return _context.DichVus
                .Include(d => d.MaHangNavigation)
                .OrderBy(d => d.Loai)
                .ThenBy(d => d.TenDv)
                .ToList();
        }

        // Lấy dịch vụ theo ID
        public DichVu GetDichVuById(int maDV)
        {
            return _context.DichVus
                .Include(d => d.MaHangNavigation)
                .FirstOrDefault(d => d.MaDv == maDV);
        }

        // Lấy dịch vụ theo loại
        public List<DichVu> GetDichVuByLoai(string loai)
        {
            return _context.DichVus
                .Where(d => d.Loai == loai)
                .OrderBy(d => d.TenDv)
                .ToList();
        }

        // Tìm kiếm dịch vụ
        public List<DichVu> SearchDichVu(string keyword)
        {
            return _context.DichVus
                .Where(d => d.TenDv.Contains(keyword))
                .OrderBy(d => d.TenDv)
                .ToList();
        }

        // Thêm dịch vụ mới
        public bool AddDichVu(DichVu dichVu)
        {
            try
            {
                _context.DichVus.Add(dichVu);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding service: {ex.Message}");
                return false;
            }
        }

        // Cập nhật dịch vụ
        public bool UpdateDichVu(DichVu dichVu)
        {
            try
            {
                _context.DichVus.Update(dichVu);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating service: {ex.Message}");
                return false;
            }
        }

        // Xóa dịch vụ
        public bool DeleteDichVu(int maDV)
        {
            try
            {
                // Kiểm tra xem dịch vụ có được sử dụng trong hóa đơn không
                var isUsed = _context.ChiTietHoaDons.Any(ct => ct.MaDv == maDV);
                if (isUsed)
                {
                    return false;
                }

                var dichVu = _context.DichVus.Find(maDV);
                if (dichVu != null)
                {
                    // Xóa file ảnh nếu có
                    if (!string.IsNullOrEmpty(dichVu.HinhAnh))
                    {
                        try
                        {
                            string imagePath = System.IO.Path.Combine(
                                AppDomain.CurrentDomain.BaseDirectory,
                                "Images",
                                dichVu.HinhAnh);

                            if (System.IO.File.Exists(imagePath))
                            {
                                System.IO.File.Delete(imagePath);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error deleting image: {ex.Message}");
                        }
                    }

                    _context.DichVus.Remove(dichVu);
                    _context.SaveChanges();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting service: {ex.Message}");
                return false;
            }
        }

        // Kiểm tra dịch vụ có được sử dụng không
        public bool IsDichVuUsed(int maDV)
        {
            return _context.ChiTietHoaDons.Any(ct => ct.MaDv == maDV);
        }

        // Lấy dịch vụ còn hàng
        public List<DichVu> GetDichVuConHang()
        {
            return _context.DichVus
                .Where(d => d.TrangThai == "Còn hàng")
                .OrderBy(d => d.TenDv)
                .ToList();
        }

        // Dispose
        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}