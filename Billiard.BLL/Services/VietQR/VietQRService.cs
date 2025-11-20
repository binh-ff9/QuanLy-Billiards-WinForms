using Billiard.DAL.Data;
using Billiard.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Billiard.BLL.Services.VietQR
{
    public class VietQRService
    {
        private readonly BilliardDbContext _context;
        private readonly HttpClient _httpClient;

        public VietQRService(BilliardDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        /// <summary>
        /// Tạo mã QR thanh toán cho hóa đơn
        /// </summary>
        public async Task<VietqrGiaoDich> TaoMaQRThanhToan(int maHd, decimal soTien)
        {
            try
            {
                // Lấy cấu hình VietQR mặc định
                var config = await _context.VietqrConfigs
                    .FirstOrDefaultAsync(c => c.LaMacDinh == true && c.TrangThai == true);

                if (config == null)
                {
                    throw new Exception("Chưa có cấu hình VietQR. Vui lòng cấu hình trong phần Cài đặt.");
                }

                // Tạo mã giao dịch unique
                var maGiaoDich = $"HD{maHd:D6}_{DateTime.Now:yyyyMMddHHmmss}";
                var noiDung = $"Thanh toan HD{maHd:D6}";

                // Tạo URL VietQR
                var qrUrl = TaoVietQRUrl(config, soTien, noiDung, maGiaoDich);

                // Lưu thông tin giao dịch vào database
                var giaoDich = new VietqrGiaoDich
                {
                    MaHd = maHd,
                    MaGiaoDich = maGiaoDich,
                    SoTien = soTien,
                    NoiDung = noiDung,
                    BankId = config.BankId,
                    AccountNo = config.AccountNo,
                    AccountName = config.AccountName,
                    QrCodeUrl = qrUrl,
                    NgayTao = DateTime.Now,
                    NgayHetHan = DateTime.Now.AddMinutes(30), // QR có hiệu lực 30 phút
                    TrangThai = "Chờ thanh toán"
                };

                _context.VietqrGiaoDiches.Add(giaoDich);
                await _context.SaveChangesAsync();

                // Cập nhật mã giao dịch vào hóa đơn
                var hoaDon = await _context.HoaDons.FindAsync(maHd);
                if (hoaDon != null)
                {
                    hoaDon.MaGiaoDichQr = maGiaoDich;
                    hoaDon.QrCodeUrl = qrUrl;
                    await _context.SaveChangesAsync();
                }

                System.Diagnostics.Debug.WriteLine($"✓ Tạo mã QR thành công: {maGiaoDich}");
                return giaoDich;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Lỗi tạo mã QR: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tạo URL VietQR theo chuẩn
        /// </summary>
        private string TaoVietQRUrl(VietqrConfig config, decimal soTien, string noiDung, string maGiaoDich)
        {
            // Format: https://img.vietqr.io/image/{BANK_ID}-{ACCOUNT_NO}-{TEMPLATE}.png?amount={AMOUNT}&addInfo={INFO}&accountName={NAME}

            var baseUrl = "https://img.vietqr.io/image";
            var template = config.Template ?? "compact"; // compact, compact2, qr_only, print

            var url = $"{baseUrl}/{config.BankId}-{config.AccountNo}-{template}.png";

            var queryParams = new StringBuilder("?");
            queryParams.Append($"amount={soTien:F0}");
            queryParams.Append($"&addInfo={Uri.EscapeDataString(noiDung)}");
            queryParams.Append($"&accountName={Uri.EscapeDataString(config.AccountName)}");

            return url + queryParams.ToString();
        }

        /// <summary>
        /// Kiểm tra trạng thái thanh toán
        /// </summary>
        public async Task<bool> KiemTraThanhToan(string maGiaoDich)
        {
            var giaoDich = await _context.VietqrGiaoDiches
                .FirstOrDefaultAsync(g => g.MaGiaoDich == maGiaoDich);

            if (giaoDich == null) return false;

            return giaoDich.TrangThai == "Đã thanh toán";
        }

        /// <summary>
        /// Cập nhật trạng thái thanh toán (gọi từ webhook hoặc manual)
        /// </summary>
        public async Task<bool> XacNhanThanhToan(string maGiaoDich, string maGiaoDichNganHang = null)
        {
            try
            {
                var giaoDich = await _context.VietqrGiaoDiches
                    .FirstOrDefaultAsync(g => g.MaGiaoDich == maGiaoDich);

                if (giaoDich == null || giaoDich.TrangThai == "Đã thanh toán")
                    return false;

                giaoDich.TrangThai = "Đã thanh toán";
                giaoDich.ThoiGianThanhToan = DateTime.Now;
                giaoDich.MaGiaoDichNganHang = maGiaoDichNganHang;

                await _context.SaveChangesAsync();

                System.Diagnostics.Debug.WriteLine($"✓ Xác nhận thanh toán thành công: {maGiaoDich}");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Lỗi xác nhận thanh toán: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Hủy mã QR
        /// </summary>
        public async Task<bool> HuyMaQR(string maGiaoDich)
        {
            try
            {
                var giaoDich = await _context.VietqrGiaoDiches
                    .FirstOrDefaultAsync(g => g.MaGiaoDich == maGiaoDich);

                if (giaoDich == null) return false;

                giaoDich.TrangThai = "Đã hủy";
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Lấy thông tin giao dịch
        /// </summary>
        public async Task<VietqrGiaoDich> LayThongTinGiaoDich(string maGiaoDich)
        {
            return await _context.VietqrGiaoDiches
                .Include(g => g.MaHdNavigation)
                .FirstOrDefaultAsync(g => g.MaGiaoDich == maGiaoDich);
        }

        /// <summary>
        /// Download QR code as Base64 (nếu cần lưu offline)
        /// </summary>
        public async Task<string> DownloadQRCodeBase64(string qrUrl)
        {
            try
            {
                var response = await _httpClient.GetAsync(qrUrl);
                if (response.IsSuccessStatusCode)
                {
                    var bytes = await response.Content.ReadAsByteArrayAsync();
                    return Convert.ToBase64String(bytes);
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Lỗi download QR: {ex.Message}");
                return null;
            }
        }
    }
}