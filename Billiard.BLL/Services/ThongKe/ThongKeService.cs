using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Billiard.DAL.Data;
using Billiard.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Billiard.BLL.Services
{
    public class ThongKeService : IDisposable
    {
        private readonly BilliardDbContext _context;

        public ThongKeService(BilliardDbContext context)
        {
            _context = context;
        }

        #region Tổng quan
        public async Task<TongQuanDto> GetTongQuanAsync(DateTime tuNgay, DateTime denNgay)
        {
            // ===== SỬA: Thêm điều kiện kiểm tra ThoiGianKetThuc không null =====
            var hoaDons = await _context.HoaDons
                .Where(h => h.TrangThai == "Đã thanh toán" // Sửa thành "Đã thanh toán"
                    && h.ThoiGianKetThuc.HasValue
                    && h.ThoiGianKetThuc.Value >= tuNgay
                    && h.ThoiGianKetThuc.Value <= denNgay)
                .ToListAsync();

            var tongDoanhThu = hoaDons.Sum(h => h.TongTien ?? 0);
            var soHoaDon = hoaDons.Count;
            var soKhachHang = hoaDons.Where(h => h.MaKh != null)
                .Select(h => h.MaKh).Distinct().Count();
            var doanhThuTrungBinh = soHoaDon > 0 ? tongDoanhThu / soHoaDon : 0;

            return new TongQuanDto
            {
                TongDoanhThu = tongDoanhThu,
                SoHoaDon = soHoaDon,
                SoKhachHang = soKhachHang,
                DoanhThuTrungBinh = doanhThuTrungBinh
            };
        }
        #endregion

        #region Doanh thu theo ngày
        public async Task<List<DoanhThuTheoNgayDto>> GetDoanhThu7NgayAsync()
        {
            var endDate = DateTime.Today.AddDays(1).AddSeconds(-1);
            var startDate = DateTime.Today.AddDays(-6);

            var data = await _context.HoaDons
                .Where(h => h.TrangThai == "Đã thanh toán" // Sửa
                    && h.ThoiGianKetThuc.HasValue
                    && h.ThoiGianKetThuc.Value >= startDate
                    && h.ThoiGianKetThuc.Value <= endDate)
                .GroupBy(h => h.ThoiGianKetThuc.Value.Date)
                .Select(g => new DoanhThuTheoNgayDto
                {
                    Ngay = g.Key,
                    DoanhThu = g.Sum(h => h.TongTien ?? 0),
                    SoHoaDon = g.Count()
                })
                .OrderBy(x => x.Ngay)
                .ToListAsync();

            // Fill missing days
            var result = new List<DoanhThuTheoNgayDto>();
            for (int i = 0; i < 7; i++)
            {
                var date = startDate.AddDays(i);
                var item = data.FirstOrDefault(d => d.Ngay == date.Date);
                result.Add(new DoanhThuTheoNgayDto
                {
                    Ngay = date,
                    DoanhThu = item?.DoanhThu ?? 0,
                    SoHoaDon = item?.SoHoaDon ?? 0
                });
            }

            return result;
        }
        #endregion

        #region Doanh thu theo tháng
        public async Task<List<DoanhThuTheoThangDto>> GetDoanhThuTheoThangAsync(int nam)
        {
            var startDate = new DateTime(nam, 1, 1);
            var endDate = new DateTime(nam, 12, 31, 23, 59, 59);

            var data = await _context.HoaDons
                .Where(h => h.TrangThai == "Đã thanh toán" // Sửa
                    && h.ThoiGianKetThuc.HasValue
                    && h.ThoiGianKetThuc.Value >= startDate
                    && h.ThoiGianKetThuc.Value <= endDate)
                .GroupBy(h => h.ThoiGianKetThuc.Value.Month)
                .Select(g => new DoanhThuTheoThangDto
                {
                    Thang = g.Key,
                    DoanhThu = g.Sum(h => h.TongTien ?? 0),
                    SoHoaDon = g.Count()
                })
                .OrderBy(x => x.Thang)
                .ToListAsync();

            // Fill missing months
            var result = new List<DoanhThuTheoThangDto>();
            for (int i = 1; i <= 12; i++)
            {
                var item = data.FirstOrDefault(d => d.Thang == i);
                result.Add(new DoanhThuTheoThangDto
                {
                    Thang = i,
                    DoanhThu = item?.DoanhThu ?? 0,
                    SoHoaDon = item?.SoHoaDon ?? 0
                });
            }

            return result;
        }
        #endregion

        #region Doanh thu theo khung giờ
        public async Task<List<DoanhThuTheoKhungGioDto>> GetDoanhThuTheoKhungGioAsync(DateTime tuNgay, DateTime denNgay)
        {
            var hoaDons = await _context.HoaDons
                .Where(h => h.TrangThai == "Đã thanh toán" // Sửa
                    && h.ThoiGianKetThuc.HasValue
                    && h.ThoiGianKetThuc.Value >= tuNgay
                    && h.ThoiGianKetThuc.Value <= denNgay)
                .ToListAsync();

            var data = hoaDons
                .GroupBy(h =>
                {
                    var hour = h.ThoiGianKetThuc.Value.Hour;
                    if (hour >= 6 && hour < 12) return "Sáng (6h-12h)";
                    if (hour >= 12 && hour < 17) return "Chiều (12h-17h)";
                    if (hour >= 17 && hour < 22) return "Tối (17h-22h)";
                    return "Khuya (22h-6h)";
                })
                .Select(g => new DoanhThuTheoKhungGioDto
                {
                    KhungGio = g.Key,
                    DoanhThu = g.Sum(h => h.TongTien ?? 0),
                    SoHoaDon = g.Count()
                })
                .OrderBy(x => x.KhungGio)
                .ToList();

            return data;
        }
        #endregion

        #region Phương thức thanh toán
        public async Task<List<PhuongThucThanhToanDto>> GetPhuongThucThanhToanAsync(DateTime tuNgay, DateTime denNgay)
        {
            var data = await _context.HoaDons
                .Where(h => h.TrangThai == "Đã thanh toán" // Sửa
                    && h.ThoiGianKetThuc.HasValue
                    && h.ThoiGianKetThuc.Value >= tuNgay
                    && h.ThoiGianKetThuc.Value <= denNgay)
                .GroupBy(h => h.PhuongThucThanhToan)
                .Select(g => new PhuongThucThanhToanDto
                {
                    PhuongThuc = g.Key ?? "Khác",
                    TongTien = g.Sum(h => h.TongTien ?? 0),
                    SoLuong = g.Count()
                })
                .ToListAsync();

            return data;
        }
        #endregion

        #region Top dịch vụ
        public async Task<List<TopDichVuDto>> GetTopDichVuAsync(DateTime tuNgay, DateTime denNgay, int top = 10)
        {
            var data = await _context.ChiTietHoaDons
                .Include(ct => ct.MaHdNavigation)
                .Include(ct => ct.MaDvNavigation)
                .Where(ct => ct.MaHdNavigation.TrangThai == "Đã thanh toán" // Sửa
                    && ct.MaHdNavigation.ThoiGianKetThuc.HasValue
                    && ct.MaHdNavigation.ThoiGianKetThuc.Value >= tuNgay
                    && ct.MaHdNavigation.ThoiGianKetThuc.Value <= denNgay)
                .GroupBy(ct => new { ct.MaDvNavigation.MaDv, ct.MaDvNavigation.TenDv })
                .Select(g => new TopDichVuDto
                {
                    TenDichVu = g.Key.TenDv ?? "N/A",
                    SoLuong = g.Sum(ct => ct.SoLuong ?? 0),
                    DoanhThu = g.Sum(ct => ct.ThanhTien ?? 0)
                })
                .OrderByDescending(x => x.SoLuong)
                .Take(top)
                .ToListAsync();

            return data;
        }
        #endregion

        #region Doanh thu theo loại dịch vụ
        public async Task<List<DoanhThuTheoLoaiDichVuDto>> GetDoanhThuTheoLoaiDichVuAsync(DateTime tuNgay, DateTime denNgay)
        {
            var data = await _context.ChiTietHoaDons
                .Include(ct => ct.MaHdNavigation)
                .Include(ct => ct.MaDvNavigation)
                .Where(ct => ct.MaHdNavigation.TrangThai == "Đã thanh toán"
                    && ct.MaHdNavigation.ThoiGianKetThuc.HasValue
                    && ct.MaHdNavigation.ThoiGianKetThuc.Value >= tuNgay
                    && ct.MaHdNavigation.ThoiGianKetThuc.Value <= denNgay
                    && ct.MaDvNavigation != null) // Thêm điều kiện này
                .ToListAsync();

            var result = data
                .GroupBy(ct => ct.MaDvNavigation.Loai?.Trim() ?? "Khác")
                .Select(g => new DoanhThuTheoLoaiDichVuDto
                {
                    Loai = g.Key,
                    DoanhThu = g.Sum(ct => ct.ThanhTien ?? 0),
                    SoLuong = g.Sum(ct => ct.SoLuong ?? 0)
                })
                .ToList();

            return result;
        }
        #endregion

        #region Doanh thu theo loại bàn
        public async Task<List<DoanhThuTheoLoaiBanDto>> GetDoanhThuTheoLoaiBanAsync(DateTime tuNgay, DateTime denNgay)
        {
            var data = await _context.HoaDons
                .Include(h => h.MaBanNavigation)
                .ThenInclude(b => b.MaLoaiNavigation)
                .Where(h => h.TrangThai == "Đã thanh toán" // Sửa
                    && h.ThoiGianKetThuc.HasValue
                    && h.ThoiGianKetThuc.Value >= tuNgay
                    && h.ThoiGianKetThuc.Value <= denNgay
                    && h.MaBanNavigation != null)
                .GroupBy(h => h.MaBanNavigation.MaLoaiNavigation.TenLoai)
                .Select(g => new DoanhThuTheoLoaiBanDto
                {
                    LoaiBan = g.Key ?? "Khác",
                    DoanhThu = g.Sum(h => h.TongTien ?? 0),
                    SoHoaDon = g.Count()
                })
                .OrderByDescending(x => x.DoanhThu)
                .ToListAsync();

            return data;
        }
        #endregion

        #region Top khách hàng
        public async Task<List<TopKhachHangDto>> GetTopKhachHangAsync(DateTime tuNgay, DateTime denNgay, int top = 10)
        {
            var data = await _context.HoaDons
                .Include(h => h.MaKhNavigation)
                .Where(h => h.TrangThai == "Đã thanh toán" // Sửa
                    && h.ThoiGianKetThuc.HasValue
                    && h.ThoiGianKetThuc.Value >= tuNgay
                    && h.ThoiGianKetThuc.Value <= denNgay
                    && h.MaKh != null)
                .GroupBy(h => new { h.MaKhNavigation.MaKh, h.MaKhNavigation.TenKh, h.MaKhNavigation.Sdt })
                .Select(g => new TopKhachHangDto
                {
                    TenKhachHang = g.Key.TenKh ?? "N/A",
                    SoDienThoai = g.Key.Sdt ?? "N/A",
                    TongChiTieu = g.Sum(h => h.TongTien ?? 0),
                    SoLanDen = g.Count()
                })
                .OrderByDescending(x => x.TongChiTieu)
                .Take(top)
                .ToListAsync();

            return data;
        }
        #endregion

        #region So sánh doanh thu
        public async Task<SoSanhDoanhThuDto> GetSoSanhDoanhThuAsync(string loai)
        {
            var now = DateTime.Now;
            DateTime startCurrent, endCurrent, startPrevious, endPrevious;
            string currentTitle, previousTitle;

            if (loai == "ngay")
            {
                startCurrent = DateTime.Today;
                endCurrent = DateTime.Today.AddDays(1).AddSeconds(-1);
                startPrevious = DateTime.Today.AddDays(-1);
                endPrevious = DateTime.Today.AddSeconds(-1);
                currentTitle = "Hôm nay";
                previousTitle = "Hôm qua";
            }
            else if (loai == "tuan")
            {
                var dayOfWeek = (int)now.DayOfWeek;
                startCurrent = now.Date.AddDays(-(dayOfWeek == 0 ? 6 : dayOfWeek - 1));
                endCurrent = startCurrent.AddDays(7).AddSeconds(-1);
                startPrevious = startCurrent.AddDays(-7);
                endPrevious = startCurrent.AddSeconds(-1);
                currentTitle = "Tuần này";
                previousTitle = "Tuần trước";
            }
            else // thang
            {
                startCurrent = new DateTime(now.Year, now.Month, 1);
                endCurrent = startCurrent.AddMonths(1).AddSeconds(-1);
                startPrevious = startCurrent.AddMonths(-1);
                endPrevious = startCurrent.AddSeconds(-1);
                currentTitle = "Tháng này";
                previousTitle = "Tháng trước";
            }

            var doanhThuHienTai = await _context.HoaDons
                .Where(h => h.TrangThai == "Đã thanh toán" // Sửa
                    && h.ThoiGianKetThuc.HasValue
                    && h.ThoiGianKetThuc.Value >= startCurrent
                    && h.ThoiGianKetThuc.Value <= endCurrent)
                .SumAsync(h => h.TongTien ?? 0);

            var doanhThuTruoc = await _context.HoaDons
                .Where(h => h.TrangThai == "Đã thanh toán" // Sửa
                    && h.ThoiGianKetThuc.HasValue
                    && h.ThoiGianKetThuc.Value >= startPrevious
                    && h.ThoiGianKetThuc.Value <= endPrevious)
                .SumAsync(h => h.TongTien ?? 0);

            var tangTruong = doanhThuTruoc > 0
                ? ((doanhThuHienTai - doanhThuTruoc) / doanhThuTruoc) * 100
                : 0;

            return new SoSanhDoanhThuDto
            {
                DoanhThuHienTai = doanhThuHienTai,
                DoanhThuTruoc = doanhThuTruoc,
                TangTruong = tangTruong,
                TieuDeHienTai = currentTitle,
                TieuDeTruoc = previousTitle
            };
        }
        #endregion

        public void Dispose()
        {
            _context?.Dispose();
        }
    }

    #region DTOs
    public class TongQuanDto
    {
        public decimal TongDoanhThu { get; set; }
        public int SoHoaDon { get; set; }
        public int SoKhachHang { get; set; }
        public decimal DoanhThuTrungBinh { get; set; }
    }

    public class DoanhThuTheoNgayDto
    {
        public DateTime Ngay { get; set; }
        public decimal DoanhThu { get; set; }
        public int SoHoaDon { get; set; }
    }

    public class DoanhThuTheoThangDto
    {
        public int Thang { get; set; }
        public decimal DoanhThu { get; set; }
        public int SoHoaDon { get; set; }
    }

    public class DoanhThuTheoKhungGioDto
    {
        public string KhungGio { get; set; }
        public decimal DoanhThu { get; set; }
        public int SoHoaDon { get; set; }
    }

    public class PhuongThucThanhToanDto
    {
        public string PhuongThuc { get; set; }
        public decimal TongTien { get; set; }
        public int SoLuong { get; set; }
    }

    public class TopDichVuDto
    {
        public string TenDichVu { get; set; }
        public int SoLuong { get; set; }
        public decimal DoanhThu { get; set; }
    }

    public class DoanhThuTheoLoaiDichVuDto
    {
        public string Loai { get; set; }
        public decimal DoanhThu { get; set; }
        public int SoLuong { get; set; }
    }

    public class DoanhThuTheoLoaiBanDto
    {
        public string LoaiBan { get; set; }
        public decimal DoanhThu { get; set; }
        public int SoHoaDon { get; set; }
    }

    public class TopKhachHangDto
    {
        public string TenKhachHang { get; set; }
        public string SoDienThoai { get; set; }
        public decimal TongChiTieu { get; set; }
        public int SoLanDen { get; set; }
    }

    public class SoSanhDoanhThuDto
    {
        public decimal DoanhThuHienTai { get; set; }
        public decimal DoanhThuTruoc { get; set; }
        public decimal TangTruong { get; set; }
        public string TieuDeHienTai { get; set; }
        public string TieuDeTruoc { get; set; }
    }
    #endregion
}