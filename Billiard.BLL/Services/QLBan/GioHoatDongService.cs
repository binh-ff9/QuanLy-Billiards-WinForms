using System;

namespace Billiard.BLL.Services.QLBan
{
    /// <summary>
    /// Service quản lý giờ hoạt động với hỗ trợ booking qua đêm
    /// </summary>
    public class GioHoatDongService
    {
        private const int GIO_MO_CUA = 9;
        private const int GIO_DONG_CUA = 2;
        private const int PHUT_CANH_BAO = 5;
        private const int SO_GIO_HOAT_DONG_TOI_DA = 17;
        private const int PHUT_GIA_HAN_MIEN_PHI = 10;

        public bool KiemTraTrongGioHoatDong()
        {
            var now = DateTime.Now;
            var hour = now.Hour;
            return hour >= GIO_MO_CUA || hour < GIO_DONG_CUA;
        }

        public DateTime LayThoiDiemDongCua()
        {
            var now = DateTime.Now;
            if (now.Hour < GIO_DONG_CUA)
            {
                return new DateTime(now.Year, now.Month, now.Day, GIO_DONG_CUA, 0, 0);
            }
            return new DateTime(now.Year, now.Month, now.Day, GIO_DONG_CUA, 0, 0).AddDays(1);
        }

        public DateTime LayThoiDiemMoCua()
        {
            var now = DateTime.Now;
            if (now.Hour < GIO_MO_CUA)
            {
                return new DateTime(now.Year, now.Month, now.Day, GIO_MO_CUA, 0, 0);
            }
            return new DateTime(now.Year, now.Month, now.Day, GIO_MO_CUA, 0, 0).AddDays(1);
        }

        public DateTime LayThoiDiemDongCuaTheoBanBatDau(DateTime gioBatDau)
        {
            if (gioBatDau.Hour >= 0 && gioBatDau.Hour < GIO_DONG_CUA)
            {
                return new DateTime(gioBatDau.Year, gioBatDau.Month, gioBatDau.Day, GIO_DONG_CUA, 0, 0);
            }
            return new DateTime(gioBatDau.Year, gioBatDau.Month, gioBatDau.Day, GIO_DONG_CUA, 0, 0).AddDays(1);
        }

        public bool SapDenGioDongCua()
        {
            if (!KiemTraTrongGioHoatDong())
                return false;

            var now = DateTime.Now;
            var gioDongCua = LayThoiDiemDongCua();
            var phutConLai = (gioDongCua - now).TotalMinutes;

            return phutConLai > 0 && phutConLai <= PHUT_CANH_BAO;
        }

        public bool DaDenGioDongCua()
        {
            var now = DateTime.Now;
            var gioDongCua = LayThoiDiemDongCua();
            return now >= gioDongCua;
        }

        public int TinhSoPhutConLaiDenDongCua()
        {
            var now = DateTime.Now;
            var gioDongCua = LayThoiDiemDongCua();
            var phutConLai = (gioDongCua - now).TotalMinutes;
            return phutConLai > 0 ? (int)Math.Ceiling(phutConLai) : 0;
        }

        public bool KiemTraBanQuaGioChoPhep(DateTime gioBatDau)
        {
            var now = DateTime.Now;
            var gioDongCua = LayThoiDiemDongCuaTheoBanBatDau(gioBatDau);
            return now > gioDongCua;
        }

        /// <summary>
        /// ✅ FIXED: Lấy thời gian kết thúc hợp lệ, XÉT ĐẾN BOOKING QUA ĐÊM
        /// </summary>
        public DateTime LayThoiGianKetThucHopLe(DateTime gioBatDau, DateTime? gioKetThucBooking = null)
        {
            var now = DateTime.Now;
            var gioDongCua = LayThoiDiemDongCuaTheoBanBatDau(gioBatDau);

            // ✅ Nếu có booking, xét đến giờ kết thúc booking
            if (gioKetThucBooking.HasValue)
            {
                var gioKetThuc = gioKetThucBooking.Value;

                // ✅ XỬ LÝ BOOKING QUA ĐÊM
                // Nếu giờ kết thúc < giờ bắt đầu → booking qua đêm
                if (gioKetThuc < gioBatDau)
                {
                    gioKetThuc = gioKetThuc.AddDays(1);
                }

                // Chọn thời gian nhỏ nhất giữa: now, booking, đóng cửa
                var thoiGianKetThucToiDa = gioKetThuc < gioDongCua ? gioKetThuc : gioDongCua;
                return now < thoiGianKetThucToiDa ? now : thoiGianKetThucToiDa;
            }

            // Không có booking → chỉ giới hạn bởi giờ đóng cửa
            return now < gioDongCua ? now : gioDongCua;
        }

        public int LaySoGioHoatDongToiDa()
        {
            return SO_GIO_HOAT_DONG_TOI_DA;
        }

        // ============================================================
        // CÁC PHƯƠNG THỨC XỬ LÝ BOOKING
        // ============================================================

        /// <summary>
        /// Tính tiền cho bàn có booking với các quy tắc:
        /// - Trong 10 phút sau giờ kết thúc: tính theo giờ đã đặt
        /// - Sau 10 phút: tính tiếp thời gian thực tế
        /// - Không vượt quá giờ đóng cửa
        /// - Không vượt quá booking tiếp theo (nếu có)
        /// </summary>
        public (decimal tienBan, string ghiChu, DateTime thoiGianKetThuc, bool canChuyenBan, string lyDoChuyenBan)
            TinhTienVoiBooking(
                DateTime gioBatDau,
                DateTime? gioKetThucDat,
                DateTime? gioBookingTiepTheo,
                decimal giaGio)
        {
            var now = DateTime.Now;
            var gioDongCua = LayThoiDiemDongCuaTheoBanBatDau(gioBatDau);

            // ============================================================
            // TRƯỜNG HỢP 1: Bàn KHÔNG có booking (chơi tự do)
            // ============================================================
            if (!gioKetThucDat.HasValue)
            {
                return TinhTienBanTuDo(gioBatDau, gioDongCua, gioBookingTiepTheo, giaGio);
            }

            // ============================================================
            // TRƯỜNG HỢP 2: Bàn CÓ booking (đã đặt trước)
            // ============================================================
            var gioKetThuc = gioKetThucDat.Value;

            // ✅ FIX: Kiểm tra booking qua đêm
            if (gioKetThuc < gioBatDau)
            {
                gioKetThuc = gioKetThuc.AddDays(1);
            }

            // Case 2.1: Giờ kết thúc booking đúng bằng hoặc sau giờ đóng cửa
            if (gioKetThuc >= gioDongCua)
            {
                var thoiGianKetThucCoDinh = now < gioDongCua ? now : gioDongCua;
                var duration = thoiGianKetThucCoDinh - gioBatDau;
                var tongPhut = (int)Math.Ceiling(duration.TotalMinutes);
                var soGio = (decimal)tongPhut / 60m;
                var tienBan = soGio * giaGio;

                string ghiChu;
                if (now >= gioDongCua)
                {
                    ghiChu = $"⚠️ ĐÚNG GIỜ ĐÓNG CỬA - Tính đến {gioDongCua:HH:mm}";
                }
                else
                {
                    ghiChu = $"Booking đến giờ đóng cửa ({gioBatDau:HH:mm} - {gioDongCua:HH:mm})";
                }

                return (tienBan, ghiChu, thoiGianKetThucCoDinh, false, null);
            }

            // Case 2.2: Chưa hết giờ booking
            if (now <= gioKetThuc)
            {
                var duration = now - gioBatDau;
                var tongPhut = (int)Math.Ceiling(duration.TotalMinutes);
                var soGio = (decimal)tongPhut / 60m;
                var tienBan = soGio * giaGio;

                var phutConLai = (int)(gioKetThuc - now).TotalMinutes;
                var ghiChu = $"Booking đến {gioKetThuc:HH:mm} (còn {phutConLai}p)";

                return (tienBan, ghiChu, now, false, null);
            }

            // Case 2.3: Đã hết giờ booking
            var phutQuaGio = (int)(now - gioKetThuc).TotalMinutes;

            // Case 2.3.1: Trong 10 phút miễn phí sau khi hết giờ
            if (phutQuaGio <= PHUT_GIA_HAN_MIEN_PHI)
            {
                var duration = gioKetThuc - gioBatDau;
                var tongPhut = (int)Math.Ceiling(duration.TotalMinutes);
                var soGio = (decimal)tongPhut / 60m;
                var tienBan = soGio * giaGio;

                var ghiChu = $"Trong {PHUT_GIA_HAN_MIEN_PHI}p miễn phí - Tính theo booking ({gioBatDau:HH:mm} - {gioKetThuc:HH:mm})";

                return (tienBan, ghiChu, gioKetThuc, false, null);
            }

            // Case 2.3.2: Quá 10 phút - tính tiếp thời gian thực tế
            var thoiGianKetThucToiDa = gioDongCua;
            bool biGioiHanBoiDongCua = now >= gioDongCua;

            bool coBookingTiepTheo = gioBookingTiepTheo.HasValue && gioBookingTiepTheo.Value > gioKetThuc;
            bool biGioiHanBoiBookingTiepTheo = coBookingTiepTheo && now >= gioBookingTiepTheo.Value;

            if (coBookingTiepTheo && gioBookingTiepTheo.Value < thoiGianKetThucToiDa)
            {
                thoiGianKetThucToiDa = gioBookingTiepTheo.Value;
            }

            var thoiGianKetThucThucTe = now < thoiGianKetThucToiDa ? now : thoiGianKetThucToiDa;

            var durationThucTe = thoiGianKetThucThucTe - gioBatDau;
            var tongPhutThucTe = (int)Math.Ceiling(durationThucTe.TotalMinutes);
            var soGioThucTe = (decimal)tongPhutThucTe / 60m;
            var tienBanThucTe = soGioThucTe * giaGio;

            string ghiChuThucTe;
            bool canChuyenBan = false;
            string lyDoChuyenBan = null;

            if (biGioiHanBoiBookingTiepTheo)
            {
                canChuyenBan = true;
                lyDoChuyenBan = $"Khung giờ {gioBookingTiepTheo.Value:HH:mm} - {gioBookingTiepTheo.Value.AddHours(2):HH:mm} đã có người đặt";
                ghiChuThucTe = $"⚠️ VỰT QUÁ BOOKING TIẾP THEO - Tính đến {thoiGianKetThucThucTe:HH:mm}\n" +
                              $"Booking gốc: {gioBatDau:HH:mm} - {gioKetThuc:HH:mm}\n" +
                              $"Đã chơi thêm: {phutQuaGio}p\n" +
                              $"{lyDoChuyenBan}";
            }
            else if (biGioiHanBoiDongCua)
            {
                ghiChuThucTe = $"⚠️ ĐÃ ĐÓNG CỬA - Tính đến {thoiGianKetThucThucTe:HH:mm}\n" +
                              $"Booking gốc: {gioBatDau:HH:mm} - {gioKetThuc:HH:mm}\n" +
                              $"Đã chơi thêm: {phutQuaGio}p";
            }
            else
            {
                ghiChuThucTe = $"Quá {PHUT_GIA_HAN_MIEN_PHI}p miễn phí - Tính thời gian thực tế\n" +
                              $"Booking gốc: {gioBatDau:HH:mm} - {gioKetThuc:HH:mm}\n" +
                              $"Đã chơi thêm: {phutQuaGio}p";

                if (coBookingTiepTheo)
                {
                    var phutConLaiDenBookingTiepTheo = (int)(gioBookingTiepTheo.Value - now).TotalMinutes;
                    if (phutConLaiDenBookingTiepTheo <= 15 && phutConLaiDenBookingTiepTheo > 0)
                    {
                        ghiChuThucTe += $"\n⚠️ Còn {phutConLaiDenBookingTiepTheo}p đến booking tiếp theo";
                    }
                }
            }

            return (tienBanThucTe, ghiChuThucTe, thoiGianKetThucThucTe, canChuyenBan, lyDoChuyenBan);
        }

        private (decimal tienBan, string ghiChu, DateTime thoiGianKetThuc, bool canChuyenBan, string lyDoChuyenBan)
            TinhTienBanTuDo(
                DateTime gioBatDau,
                DateTime gioDongCua,
                DateTime? gioBookingTiepTheo,
                decimal giaGio)
        {
            var now = DateTime.Now;

            var thoiGianKetThucToiDa = gioDongCua;
            bool coBookingTiepTheo = gioBookingTiepTheo.HasValue;
            bool biGioiHanBoiBooking = false;

            if (coBookingTiepTheo && gioBookingTiepTheo.Value < thoiGianKetThucToiDa)
            {
                thoiGianKetThucToiDa = gioBookingTiepTheo.Value;
                biGioiHanBoiBooking = now >= gioBookingTiepTheo.Value;
            }

            var thoiGianKetThuc = now < thoiGianKetThucToiDa ? now : thoiGianKetThucToiDa;

            var duration = thoiGianKetThuc - gioBatDau;
            var tongPhut = (int)Math.Ceiling(duration.TotalMinutes);
            var soGio = (decimal)tongPhut / 60m;
            var tienBan = soGio * giaGio;

            string ghiChu;
            bool canChuyenBan = false;
            string lyDoChuyenBan = null;

            if (biGioiHanBoiBooking)
            {
                canChuyenBan = true;
                lyDoChuyenBan = $"Khung giờ {gioBookingTiepTheo.Value:HH:mm} đã có người đặt";
                ghiChu = $"⚠️ CÓ BOOKING TIẾP THEO - Tính đến {thoiGianKetThuc:HH:mm}\n{lyDoChuyenBan}";
            }
            else if (now >= gioDongCua)
            {
                ghiChu = $"⚠️ ĐÃ ĐÓNG CỬA - Tính đến {thoiGianKetThuc:HH:mm}";
            }
            else if (coBookingTiepTheo)
            {
                var phutConLai = (int)(gioBookingTiepTheo.Value - now).TotalMinutes;
                if (phutConLai <= 15)
                {
                    ghiChu = $"Tính đến {thoiGianKetThuc:HH:mm}\n⚠️ Còn {phutConLai}p đến booking tiếp theo";
                }
                else
                {
                    ghiChu = $"Tính đến {thoiGianKetThuc:HH:mm}";
                }
            }
            else
            {
                ghiChu = $"Tính đến {thoiGianKetThuc:HH:mm}";
            }

            return (tienBan, ghiChu, thoiGianKetThuc, canChuyenBan, lyDoChuyenBan);
        }

        public bool DangTrongThoiGianGiaHanMienPhi(DateTime? gioKetThucDat)
        {
            if (!gioKetThucDat.HasValue)
                return false;

            var now = DateTime.Now;
            if (now <= gioKetThucDat.Value)
                return false;

            var phutQuaGio = (now - gioKetThucDat.Value).TotalMinutes;
            return phutQuaGio <= PHUT_GIA_HAN_MIEN_PHI;
        }

        public (bool canCanhBao, string thongBao) KiemTraCanhBaoChuyenBan(
            DateTime? gioKetThucDat,
            DateTime? gioBookingTiepTheo)
        {
            if (!gioBookingTiepTheo.HasValue)
                return (false, null);

            var now = DateTime.Now;

            if (gioKetThucDat.HasValue)
            {
                var phutQuaGio = (now - gioKetThucDat.Value).TotalMinutes;
                if (phutQuaGio > PHUT_GIA_HAN_MIEN_PHI)
                {
                    var phutConLai = (int)(gioBookingTiepTheo.Value - now).TotalMinutes;
                    if (phutConLai <= 15 && phutConLai > 0)
                    {
                        return (true, $"⚠️ Còn {phutConLai} phút đến booking tiếp theo!\nVui lòng chuẩn bị chuyển bàn hoặc thanh toán.");
                    }

                    if (phutConLai <= 0)
                    {
                        return (true, $"🚨 ĐÃ ĐẾN GIỜ BOOKING TIẾP THEO!\nVui lòng CHUYỂN BÀN hoặc THANH TOÁN NGAY!");
                    }
                }
            }
            else
            {
                var phutConLai = (int)(gioBookingTiepTheo.Value - now).TotalMinutes;
                if (phutConLai <= 10 && phutConLai > 0)
                {
                    return (true, $"⚠️ Còn {phutConLai} phút đến booking tiếp theo!\nVui lòng chuẩn bị thanh toán hoặc chuyển bàn.");
                }

                if (phutConLai <= 0)
                {
                    return (true, $"🚨 ĐÃ ĐẾN GIỜ BOOKING TIẾP THEO!\nVui lòng THANH TOÁN NGAY hoặc CHUYỂN BÀN!");
                }
            }

            return (false, null);
        }

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

        public bool CanBatBuocThanhToan(DateTime? gioBatDau)
        {
            if (!gioBatDau.HasValue)
                return false;

            var gioDongCua = LayThoiDiemDongCuaTheoBanBatDau(gioBatDau.Value);
            return DateTime.Now >= gioDongCua;
        }

        public bool CanCanhBaoSapDongCua(DateTime? gioBatDau)
        {
            if (!gioBatDau.HasValue)
                return false;

            return SapDenGioDongCua();
        }

        /// <summary>
        /// ✅ DEPRECATED: Phương thức cũ, giữ lại để tương thích
        /// Nên dùng TinhTienVoiBooking() thay thế
        /// </summary>
        public (decimal tienBan, string ghiChu) TinhTienTamThoi(DateTime gioBatDau, decimal giaGio)
        {
            var (tienBan, ghiChu, _, _, _) = TinhTienVoiBooking(gioBatDau, null, null, giaGio);
            return (tienBan, ghiChu);
        }
    }
}