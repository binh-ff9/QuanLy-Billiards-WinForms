using Billiard.DAL.Data;
using Billiard.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Billiard.BLL.Services.HoaDonServices
{
    public class ThanhToanService
    {
        private readonly BilliardDbContext _context;

        public ThanhToanService(BilliardDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Tính toán chi tiết thanh toán
        /// </summary>
        public async Task<ThanhToanInfo> TinhToanThanhToan(int maHd)
        {
            var hoaDon = await _context.HoaDons
                .Include(h => h.MaBanNavigation)
                    .ThenInclude(b => b.MaLoaiNavigation)
                .Include(h => h.MaKhNavigation)
                .FirstOrDefaultAsync(h => h.MaHd == maHd);

            if (hoaDon == null || hoaDon.TrangThai != "Đang chơi")
                return null;

            var duration = DateTime.Now - hoaDon.ThoiGianBatDau.Value;
            var soGio = (decimal)duration.TotalMinutes / 60;
            var giaGio = hoaDon.MaBanNavigation.MaLoaiNavigation.GiaGio;
            var tienBan = soGio * giaGio;
            var tienDichVu = hoaDon.TienDichVu ?? 0;
            var giamGia = hoaDon.GiamGia ?? 0;
            var tamTinh = tienBan + tienDichVu - giamGia;
            var tongTien = Math.Ceiling(tamTinh / 1000) * 1000; // Làm tròn lên nghìn

            return new ThanhToanInfo
            {
                MaHd = maHd,
                TenBan = hoaDon.MaBanNavigation.TenBan,
                TenKhach = hoaDon.MaKhNavigation?.TenKh ?? "Khách lẻ",
                ThoiGianBatDau = hoaDon.ThoiGianBatDau.Value,
                ThoiLuongPhut = (int)duration.TotalMinutes,
                GiaGio = giaGio,
                TienBan = tienBan,
                TienDichVu = tienDichVu,
                GiamGia = giamGia,
                TamTinh = tamTinh,
                TongTien = tongTien,
                ChenhLech = tongTien - tamTinh
            };
        }

        /// <summary>
        /// Thanh toán tiền mặt
        /// </summary>
        public async Task<ThanhToanResult> ThanhToanTienMat(int maHd, decimal tienKhachDua)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var thanhToanInfo = await TinhToanThanhToan(maHd);
                if (thanhToanInfo == null)
                    return ThanhToanResult.Fail("Không tìm thấy hóa đơn hoặc hóa đơn đã thanh toán");

                if (tienKhachDua < thanhToanInfo.TongTien)
                    return ThanhToanResult.Fail("Tiền khách đưa không đủ");

                var tienThua = tienKhachDua - thanhToanInfo.TongTien;

                // Cập nhật hóa đơn
                var success = await CapNhatHoaDonThanhToan(maHd, thanhToanInfo, "Tiền mặt");
                if (!success)
                    return ThanhToanResult.Fail("Lỗi cập nhật hóa đơn");

                // Lưu vào sổ quỹ
                await LuuSoQuy(maHd, thanhToanInfo.TongTien, "Thu", $"Thanh toán tiền mặt HD{maHd:D6}");

                await transaction.CommitAsync();

                return ThanhToanResult.Success("Thanh toán tiền mặt thành công", new
                {
                    TongTien = thanhToanInfo.TongTien,
                    TienKhachDua = tienKhachDua,
                    TienThua = tienThua
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ThanhToanResult.Fail($"Lỗi: {ex.Message}");
            }
        }

        /// <summary>
        /// Thanh toán QR (chờ xác nhận)
        /// </summary>
        public async Task<ThanhToanResult> ThanhToanQR(int maHd, string maGiaoDichQR)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var thanhToanInfo = await TinhToanThanhToan(maHd);
                if (thanhToanInfo == null)
                    return ThanhToanResult.Fail("Không tìm thấy hóa đơn");

                // Kiểm tra giao dịch QR đã được xác nhận chưa
                var giaoDichQR = await _context.VietqrGiaoDiches
                    .FirstOrDefaultAsync(g => g.MaGiaoDich == maGiaoDichQR
                        && g.TrangThai == "Đã thanh toán");

                if (giaoDichQR == null)
                    return ThanhToanResult.Fail("Giao dịch QR chưa được xác nhận thanh toán");

                // Cập nhật hóa đơn
                var success = await CapNhatHoaDonThanhToan(maHd, thanhToanInfo, "Chuyển khoản");
                if (!success)
                    return ThanhToanResult.Fail("Lỗi cập nhật hóa đơn");

                // Lưu sổ quỹ
                await LuuSoQuy(maHd, thanhToanInfo.TongTien, "Thu",
                    $"Thanh toán QR HD{maHd:D6} - {maGiaoDichQR}");

                await transaction.CommitAsync();

                return ThanhToanResult.Success("Thanh toán QR thành công");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ThanhToanResult.Fail($"Lỗi: {ex.Message}");
            }
        }

        /// <summary>
        /// Thanh toán thẻ (đang phát triển)
        /// </summary>
        public async Task<ThanhToanResult> ThanhToanThe(int maHd, string soThe, string maGiaoDichThe)
        {
            return ThanhToanResult.Fail("Chức năng thanh toán thẻ đang được phát triển");
        }

        /// <summary>
        /// Cập nhật hóa đơn sau khi thanh toán
        /// </summary>
        private async Task<bool> CapNhatHoaDonThanhToan(int maHd, ThanhToanInfo info, string phuongThuc)
        {
            try
            {
                var hoaDon = await _context.HoaDons
                    .Include(h => h.MaBanNavigation)
                    .FirstOrDefaultAsync(h => h.MaHd == maHd);

                if (hoaDon == null) return false;

                // Update hóa đơn
                hoaDon.ThoiGianKetThuc = DateTime.Now;
                hoaDon.TienBan = info.TienBan;
                hoaDon.TongTien = info.TongTien;
                hoaDon.TrangThai = "Đã thanh toán";
                hoaDon.PhuongThucThanhToan = phuongThuc;

                // Update bàn
                var ban = hoaDon.MaBanNavigation;
                ban.TrangThai = "Trống";
                ban.GioBatDau = null;
                ban.MaKh = null;
                ban.GhiChu = null;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Lưu vào sổ quỹ
        /// </summary>
        private async Task LuuSoQuy(int maHd, decimal soTien, string loaiPhieu, string lyDo)
        {
            var hoaDon = await _context.HoaDons.FindAsync(maHd);

            var soQuy = new SoQuy
            {
                LoaiPhieu = loaiPhieu,
                SoTien = soTien,
                LyDo = lyDo,
                MaHdLienQuan = maHd,
                MaNv = hoaDon.MaNv ?? 1, // Lấy từ hóa đơn
                NgayLap = DateTime.Now
            };

            _context.SoQuies.Add(soQuy);
            await _context.SaveChangesAsync();
        }
    }

    #region Helper Classes

    public class ThanhToanInfo
    {
        public int MaHd { get; set; }
        public string TenBan { get; set; }
        public string TenKhach { get; set; }
        public DateTime ThoiGianBatDau { get; set; }
        public int ThoiLuongPhut { get; set; }
        public decimal GiaGio { get; set; }
        public decimal TienBan { get; set; }
        public decimal TienDichVu { get; set; }
        public decimal GiamGia { get; set; }
        public decimal TamTinh { get; set; }
        public decimal TongTien { get; set; }
        public decimal ChenhLech { get; set; }
    }

    public class ThanhToanResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public static ThanhToanResult Success(string message, object data = null)
        {
            return new ThanhToanResult
            {
                IsSuccess = true,
                Message = message,
                Data = data
            };
        }

        public static ThanhToanResult Fail(string message)
        {
            return new ThanhToanResult
            {
                IsSuccess = false,
                Message = message
            };
        }
    }

    #endregion
}