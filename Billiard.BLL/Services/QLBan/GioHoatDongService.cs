using System;

namespace Billiard.BLL.Services.QLBan
{
    /// <summary>
    /// Service quản lý giờ hoạt động của quán: 9h sáng hôm nay → 2h sáng hôm sau
    /// CRITICAL: Không cho phép bàn hoạt động quá 17 tiếng (giờ mở → giờ đóng)
    /// </summary>
    public class GioHoatDongService
    {
        // Giờ mở cửa: 9h sáng
        private const int GIO_MO_CUA = 9;

        // Giờ đóng cửa: 2h sáng hôm sau
        private const int GIO_DONG_CUA = 2;

        // Cảnh báo trước khi đóng cửa (phút)
        private const int PHUT_CANH_BAO = 5;

        // Số giờ hoạt động tối đa trong 1 ngày làm việc
        private const int SO_GIO_HOAT_DONG_TOI_DA = 17; // 9h → 2h sáng = 17 giờ

        /// <summary>
        /// Kiểm tra hiện tại có trong giờ hoạt động không
        /// </summary>
        public bool KiemTraTrongGioHoatDong()
        {
            var now = DateTime.Now;
            var hour = now.Hour;

            // Giờ hoạt động: 9h → 23h59 (cùng ngày) HOẶC 0h → 2h (ngày hôm sau)
            return hour >= GIO_MO_CUA || hour < GIO_DONG_CUA;
        }

        /// <summary>
        /// Lấy thời điểm đóng cửa gần nhất (2h sáng)
        /// </summary>
        public DateTime LayThoiDiemDongCua()
        {
            var now = DateTime.Now;

            // Nếu hiện tại < 2h sáng → đóng cửa là 2h sáng hôm nay
            if (now.Hour < GIO_DONG_CUA)
            {
                return new DateTime(now.Year, now.Month, now.Day, GIO_DONG_CUA, 0, 0);
            }

            // Nếu hiện tại >= 9h → đóng cửa là 2h sáng hôm sau
            return new DateTime(now.Year, now.Month, now.Day, GIO_DONG_CUA, 0, 0).AddDays(1);
        }

        /// <summary>
        /// Lấy thời điểm mở cửa gần nhất (9h sáng)
        /// </summary>
        public DateTime LayThoiDiemMoCua()
        {
            var now = DateTime.Now;

            // Nếu hiện tại < 9h sáng → mở cửa là 9h sáng hôm nay
            if (now.Hour < GIO_MO_CUA)
            {
                return new DateTime(now.Year, now.Month, now.Day, GIO_MO_CUA, 0, 0);
            }

            // Nếu hiện tại >= 9h → mở cửa là 9h sáng hôm sau
            return new DateTime(now.Year, now.Month, now.Day, GIO_MO_CUA, 0, 0).AddDays(1);
        }

        /// <summary>
        /// Lấy thời điểm đóng cửa dựa trên giờ bắt đầu chơi
        /// QUAN TRỌNG: Dùng để xác định giờ đóng cửa CỤ THỂ cho ca làm việc của bàn đó
        /// </summary>
        public DateTime LayThoiDiemDongCuaTheoBanBatDau(DateTime gioBatDau)
        {
            // Nếu bắt đầu trong khoảng 0h → 2h sáng
            // => Ca làm việc là từ 9h sáng HÔM TRƯỚC đến 2h sáng HÔM NAY
            if (gioBatDau.Hour >= 0 && gioBatDau.Hour < GIO_DONG_CUA)
            {
                return new DateTime(gioBatDau.Year, gioBatDau.Month, gioBatDau.Day, GIO_DONG_CUA, 0, 0);
            }

            // Nếu bắt đầu từ 9h trở đi
            // => Ca làm việc là từ 9h HÔM NAY đến 2h sáng HÔM SAU
            return new DateTime(gioBatDau.Year, gioBatDau.Month, gioBatDau.Day, GIO_DONG_CUA, 0, 0).AddDays(1);
        }

        /// <summary>
        /// Kiểm tra sắp đến giờ đóng cửa (còn <= 5 phút)
        /// </summary>
        public bool SapDenGioDongCua()
        {
            if (!KiemTraTrongGioHoatDong())
                return false;

            var now = DateTime.Now;
            var gioDongCua = LayThoiDiemDongCua();
            var phutConLai = (gioDongCua - now).TotalMinutes;

            return phutConLai > 0 && phutConLai <= PHUT_CANH_BAO;
        }

        /// <summary>
        /// Kiểm tra ĐÃ ĐẾN giờ đóng cửa
        /// </summary>
        public bool DaDenGioDongCua()
        {
            var now = DateTime.Now;
            var gioDongCua = LayThoiDiemDongCua();
            return now >= gioDongCua;
        }

        /// <summary>
        /// Tính số phút còn lại đến giờ đóng cửa
        /// </summary>
        public int TinhSoPhutConLaiDenDongCua()
        {
            var now = DateTime.Now;
            var gioDongCua = LayThoiDiemDongCua();
            var phutConLai = (gioDongCua - now).TotalMinutes;

            return phutConLai > 0 ? (int)Math.Ceiling(phutConLai) : 0;
        }

        /// <summary>
        /// ✅ FIXED: Tính tiền bàn TẠM THỜI với logic CHẶT CHẼ
        /// - Không cho phép vượt quá giờ đóng cửa
        /// - Không cho phép vượt quá số giờ hoạt động tối đa (17 tiếng)
        /// </summary>
        public (decimal tienBan, string ghiChu) TinhTienTamThoi(DateTime gioBatDau, decimal giaGio)
        {
            var now = DateTime.Now;
            var gioDongCua = LayThoiDiemDongCuaTheoBanBatDau(gioBatDau);

            // ============================================================
            // BƯỚC 1: Xác định thời gian kết thúc HỢP LỆ
            // ============================================================
            DateTime thoiGianKetThuc;
            string ghiChu;

            // Trường hợp 1: Đã quá giờ đóng cửa → BẮT BUỘC tính đến giờ đóng cửa
            if (now >= gioDongCua)
            {
                thoiGianKetThuc = gioDongCua;
                ghiChu = $"⚠️ ĐÃ ĐÓNG CỬA - Tính đến {gioDongCua:HH:mm}";
            }
            // Trường hợp 2: Chưa đến giờ đóng cửa → tính đến hiện tại
            else
            {
                thoiGianKetThuc = now;
                ghiChu = $"Tính đến {now:HH:mm}";
            }

            // ============================================================
            // BƯỚC 2: KIỂM TRA VÀ CHẶN SỐ GIỜ TỐI ĐA
            // ============================================================
            var duration = thoiGianKetThuc - gioBatDau;
            var soGioThucTe = duration.TotalHours;

            // Nếu vượt quá số giờ hoạt động tối đa (17 tiếng)
            // → Tự động cắt về giờ đóng cửa
            if (soGioThucTe > SO_GIO_HOAT_DONG_TOI_DA)
            {
                thoiGianKetThuc = gioDongCua;
                duration = thoiGianKetThuc - gioBatDau;
                ghiChu = $"⚠️ QUÁ GIỜ HOẠT ĐỘNG - Chỉ tính {SO_GIO_HOAT_DONG_TOI_DA}h (đến {gioDongCua:HH:mm})";
            }

            // ============================================================
            // BƯỚC 3: Tính tiền
            // ============================================================
            var tongPhut = (int)Math.Ceiling(duration.TotalMinutes);
            var soGio = (decimal)tongPhut / 60m;
            var tienBan = soGio * giaGio;

            return (tienBan, ghiChu);
        }

        /// <summary>
        /// ✅ NEW: Kiểm tra bàn có đang chơi quá giờ cho phép không
        /// </summary>
        public bool KiemTraBanQuaGioChoPhep(DateTime gioBatDau)
        {
            var gioDongCua = LayThoiDiemDongCuaTheoBanBatDau(gioBatDau);
            var soGioThucTe = (DateTime.Now - gioBatDau).TotalHours;

            return soGioThucTe > SO_GIO_HOAT_DONG_TOI_DA || DateTime.Now > gioDongCua;
        }

        /// <summary>
        /// ✅ NEW: Lấy thời gian kết thúc HỢP LỆ cho một bàn
        /// (Dùng để fix dữ liệu hoặc hiển thị cảnh báo)
        /// </summary>
        public DateTime LayThoiGianKetThucHopLe(DateTime gioBatDau)
        {
            var gioDongCua = LayThoiDiemDongCuaTheoBanBatDau(gioBatDau);
            var now = DateTime.Now;

            // Nếu hiện tại chưa đến giờ đóng cửa → trả về hiện tại
            if (now < gioDongCua)
            {
                var soGioHienTai = (now - gioBatDau).TotalHours;

                // Nếu đã chơi quá 17 tiếng → trả về giờ đóng cửa
                if (soGioHienTai > SO_GIO_HOAT_DONG_TOI_DA)
                {
                    return gioDongCua;
                }

                return now;
            }

            // Nếu đã quá giờ đóng cửa → trả về giờ đóng cửa
            return gioDongCua;
        }

        /// <summary>
        /// Lấy thông báo trạng thái giờ hoạt động
        /// </summary>
        public string LayThongBaoGioHoatDong()
        {
            if (!KiemTraTrongGioHoatDong())
            {
                var moCua = LayThoiDiemMoCua();
                return $"Quán đóng cửa. Mở cửa lúc {moCua:HH:mm dd/MM/yyyy}";
            }

            if (SapDenGioDongCua())
            {
                var phutConLai = TinhSoPhutConLaiDenDongCua();
                return $"⚠️ Sắp đóng cửa! Còn {phutConLai} phút";
            }

            var dongCua = LayThoiDiemDongCua();
            return $"Đang hoạt động. Đóng cửa lúc {dongCua:HH:mm}";
        }

        /// <summary>
        /// Kiểm tra một bàn có cần bắt buộc thanh toán không (đã quá giờ đóng cửa)
        /// </summary>
        public bool CanBatBuocThanhToan(DateTime? gioBatDau)
        {
            if (!gioBatDau.HasValue)
                return false;

            var gioDongCua = LayThoiDiemDongCuaTheoBanBatDau(gioBatDau.Value);
            return DateTime.Now >= gioDongCua;
        }

        /// <summary>
        /// Kiểm tra bàn có cần cảnh báo sắp đóng cửa không
        /// </summary>
        public bool CanCanhBaoSapDongCua(DateTime? gioBatDau)
        {
            if (!gioBatDau.HasValue)
                return false;

            return SapDenGioDongCua();
        }

        /// <summary>
        /// ✅ NEW: Lấy số giờ tối đa có thể chơi
        /// </summary>
        public int LaySoGioHoatDongToiDa()
        {
            return SO_GIO_HOAT_DONG_TOI_DA;
        }
    }
}