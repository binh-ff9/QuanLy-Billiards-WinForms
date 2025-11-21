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
        /// Tính toán chi tiết thanh toán - ĐÚNG THEO HÓA ĐƠN
        /// </summary>
        public async Task<ThanhToanInfo> TinhToanThanhToan(int maHd)
        {
            try
            {
                var hoaDon = await _context.HoaDons
                    .Include(h => h.MaBanNavigation)
                        .ThenInclude(b => b.MaLoaiNavigation)
                    .Include(h => h.MaKhNavigation)
                    .Include(h => h.ChiTietHoaDons)
                        .ThenInclude(ct => ct.MaDvNavigation)
                    .FirstOrDefaultAsync(h => h.MaHd == maHd);

                if (hoaDon == null)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ Không tìm thấy hóa đơn {maHd}");
                    return null;
                }

                if (hoaDon.TrangThai != "Đang chơi")
                {
                    System.Diagnostics.Debug.WriteLine($"❌ Hóa đơn {maHd} không ở trạng thái 'Đang chơi' (Trạng thái: {hoaDon.TrangThai})");
                    return null;
                }

                if (!hoaDon.ThoiGianBatDau.HasValue)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ Hóa đơn {maHd} không có thời gian bắt đầu");
                    return null;
                }

                // 1. Tính thời gian chơi
                var thoiGianKetThuc = DateTime.Now;
                var duration = thoiGianKetThuc - hoaDon.ThoiGianBatDau.Value;
                var tongPhut = (int)Math.Ceiling(duration.TotalMinutes);

                System.Diagnostics.Debug.WriteLine($"\n=== TÍNH TOÁN THANH TOÁN HD{maHd} ===");
                System.Diagnostics.Debug.WriteLine($"Thời gian bắt đầu: {hoaDon.ThoiGianBatDau.Value:HH:mm:ss dd/MM/yyyy}");
                System.Diagnostics.Debug.WriteLine($"Thời gian kết thúc: {thoiGianKetThuc:HH:mm:ss dd/MM/yyyy}");
                System.Diagnostics.Debug.WriteLine($"Tổng thời gian: {tongPhut} phút ({duration.TotalMinutes:F2} phút thực tế)");

                // 2. Lấy giá giờ
                var giaGio = hoaDon.MaBanNavigation?.MaLoaiNavigation?.GiaGio ?? 0;
                if (giaGio == 0)
                {
                    System.Diagnostics.Debug.WriteLine($"⚠ Giá giờ = 0, kiểm tra dữ liệu bàn/loại bàn");
                }
                System.Diagnostics.Debug.WriteLine($"Giá giờ: {giaGio:N0} đ");

                // 3. Tính tiền bàn
                var soGio = (decimal)tongPhut / 60m;
                var tienBan = soGio * giaGio;
                System.Diagnostics.Debug.WriteLine($"Số giờ: {soGio:F4} ({tongPhut} phút / 60)");
                System.Diagnostics.Debug.WriteLine($"Tiền bàn (chưa làm tròn): {tienBan:N2} đ");

                // 4. Tính tiền dịch vụ
                decimal tienDichVu = 0;
                if (hoaDon.ChiTietHoaDons != null && hoaDon.ChiTietHoaDons.Any())
                {
                    System.Diagnostics.Debug.WriteLine($"\nDịch vụ đã order:");
                    foreach (var ct in hoaDon.ChiTietHoaDons)
                    {
                        var giaDv = ct.MaDvNavigation?.Gia ?? 0;
                        var thanhTien = (ct.SoLuong * giaDv) ?? 0;
                        tienDichVu += thanhTien;

                        System.Diagnostics.Debug.WriteLine($"  - {ct.MaDvNavigation?.TenDv ?? "N/A"}: {ct.SoLuong} x {giaDv:N0} = {thanhTien:N0} đ");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"\nKhông có dịch vụ");
                }
                System.Diagnostics.Debug.WriteLine($"Tổng tiền dịch vụ: {tienDichVu:N0} đ");

                // 5. Lấy giảm giá
                var giamGia = hoaDon.GiamGia ?? 0;
                System.Diagnostics.Debug.WriteLine($"Giảm giá: {giamGia:N0} đ");

                // 6. Tính tạm tính
                var tamTinh = tienBan + tienDichVu - giamGia;
                System.Diagnostics.Debug.WriteLine($"Tạm tính (chưa làm tròn): {tamTinh:N2} đ");

                // 7. Làm tròn lên nghìn
                var tongTien = Math.Ceiling(tamTinh / 1000m) * 1000m;
                var chenhLech = tongTien - tamTinh;
                System.Diagnostics.Debug.WriteLine($"Làm tròn lên nghìn: {tongTien:N0} đ");
                System.Diagnostics.Debug.WriteLine($"Chênh lệch làm tròn: {chenhLech:N2} đ");

                var thanhToanInfo = new ThanhToanInfo
                {
                    MaHd = maHd,
                    TenBan = hoaDon.MaBanNavigation?.TenBan ?? "N/A",
                    TenKhach = hoaDon.MaKhNavigation?.TenKh ?? "Khách lẻ",
                    ThoiGianBatDau = hoaDon.ThoiGianBatDau.Value,
                    ThoiLuongPhut = tongPhut,
                    GiaGio = giaGio,
                    TienBan = tienBan,
                    TienDichVu = tienDichVu,
                    GiamGia = giamGia,
                    TamTinh = tamTinh,
                    TongTien = tongTien,
                    ChenhLech = chenhLech
                };

                System.Diagnostics.Debug.WriteLine($"✓ Tính toán hoàn tất: TỔNG = {tongTien:N0} đ\n");
                return thanhToanInfo;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Exception trong TinhToanThanhToan: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack: {ex.StackTrace}");
                return null;
            }
        }

        /// <summary>
        /// Thanh toán tiền mặt
        /// </summary>
        public async Task<ThanhToanResult> ThanhToanTienMat(int maHd, decimal tienKhachDua)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    System.Diagnostics.Debug.WriteLine($"\n=== THANH TOÁN TIỀN MẶT HD{maHd} ===");

                    var thanhToanInfo = await TinhToanThanhToan(maHd);
                    if (thanhToanInfo == null)
                        return ThanhToanResult.Fail("Không tìm thấy hóa đơn hoặc hóa đơn đã thanh toán");

                    System.Diagnostics.Debug.WriteLine($"Tổng tiền: {thanhToanInfo.TongTien:N0} đ");
                    System.Diagnostics.Debug.WriteLine($"Tiền khách đưa: {tienKhachDua:N0} đ");

                    if (tienKhachDua < thanhToanInfo.TongTien)
                    {
                        var thieu = thanhToanInfo.TongTien - tienKhachDua;
                        System.Diagnostics.Debug.WriteLine($"❌ Tiền không đủ, còn thiếu: {thieu:N0} đ");
                        return ThanhToanResult.Fail($"Tiền khách đưa không đủ! Còn thiếu {thieu:N0} đ");
                    }

                    var tienThua = tienKhachDua - thanhToanInfo.TongTien;
                    System.Diagnostics.Debug.WriteLine($"Tiền thừa: {tienThua:N0} đ");

                    var success = await CapNhatHoaDonThanhToan(maHd, thanhToanInfo, "Tiền mặt");
                    if (!success)
                        return ThanhToanResult.Fail("Lỗi cập nhật hóa đơn");

                    await LuuSoQuy(maHd, thanhToanInfo.TongTien, "Thu",
                        $"Thanh toán tiền mặt HD{maHd:D6}");

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    System.Diagnostics.Debug.WriteLine($"✓✓✓ THANH TOÁN THÀNH CÔNG\n");

                    return ThanhToanResult.Success("Thanh toán tiền mặt thành công", new
                    {
                        MaHd = maHd,
                        TenBan = thanhToanInfo.TenBan,
                        TongTien = thanhToanInfo.TongTien,
                        TienKhachDua = tienKhachDua,
                        TienThua = tienThua,
                        ThoiGianThanhToan = DateTime.Now
                    });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    System.Diagnostics.Debug.WriteLine($"❌ Exception: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Stack: {ex.StackTrace}");
                    return ThanhToanResult.Fail($"Lỗi: {ex.Message}");
                }
            });
        }

        /// <summary>
        /// Thanh toán QR - FIXED VERSION
        /// </summary>
        public async Task<ThanhToanResult> ThanhToanQR(int maHd, string maGiaoDichQR)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    System.Diagnostics.Debug.WriteLine($"\n=== THANH TOÁN QR HD{maHd} ===");
                    System.Diagnostics.Debug.WriteLine($"Mã giao dịch QR: {maGiaoDichQR}");

                    // 1. Kiểm tra giao dịch QR TRƯỚC
                    var giaoDichQR = await _context.VietqrGiaoDiches
                        .FirstOrDefaultAsync(g => g.MaGiaoDich == maGiaoDichQR);

                    if (giaoDichQR == null)
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ Không tìm thấy giao dịch QR: {maGiaoDichQR}");
                        return ThanhToanResult.Fail("Không tìm thấy giao dịch QR");
                    }

                    System.Diagnostics.Debug.WriteLine($"✓ Tìm thấy giao dịch QR");
                    System.Diagnostics.Debug.WriteLine($"  - Trạng thái: {giaoDichQR.TrangThai}");
                    System.Diagnostics.Debug.WriteLine($"  - Số tiền: {giaoDichQR.SoTien:N0} đ");
                    System.Diagnostics.Debug.WriteLine($"  - Mã HD liên kết: {giaoDichQR.MaHd}");

                    // 2. Kiểm tra trạng thái giao dịch
                    if (giaoDichQR.TrangThai != "Đã thanh toán")
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ Giao dịch QR chưa xác nhận thanh toán");
                        return ThanhToanResult.Fail("Giao dịch QR chưa được xác nhận thanh toán");
                    }

                    // 3. Lấy thông tin hóa đơn từ giao dịch QR
                    int hoaDonId = giaoDichQR.MaHd;
                    System.Diagnostics.Debug.WriteLine($"Sử dụng mã HD từ giao dịch QR: {hoaDonId}");

                    // 4. Tính toán lại để đảm bảo số liệu chính xác
                    var thanhToanInfo = await TinhToanThanhToan(hoaDonId);
                    if (thanhToanInfo == null)
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ Không tìm thấy hoặc không thể tính toán hóa đơn {hoaDonId}");
                        return ThanhToanResult.Fail($"Không tìm thấy hóa đơn {hoaDonId} hoặc hóa đơn đã thanh toán");
                    }

                    System.Diagnostics.Debug.WriteLine($"✓ Tính toán hóa đơn thành công");
                    System.Diagnostics.Debug.WriteLine($"  - Tổng tiền: {thanhToanInfo.TongTien:N0} đ");

                    // 5. Kiểm tra số tiền khớp (cho phép QR >= Tổng tiền)
                    if (giaoDichQR.SoTien < thanhToanInfo.TongTien)
                    {
                        var thieu = thanhToanInfo.TongTien - giaoDichQR.SoTien;
                        System.Diagnostics.Debug.WriteLine($"❌ Số tiền QR không đủ. Thiếu: {thieu:N0} đ");
                        return ThanhToanResult.Fail($"Số tiền QR không đủ! Cần {thanhToanInfo.TongTien:N0} đ, chỉ có {giaoDichQR.SoTien:N0} đ");
                    }

                    if (giaoDichQR.SoTien != thanhToanInfo.TongTien)
                    {
                        System.Diagnostics.Debug.WriteLine($"⚠ Số tiền QR ({giaoDichQR.SoTien:N0}) khác tổng tiền HD ({thanhToanInfo.TongTien:N0})");
                        System.Diagnostics.Debug.WriteLine($"  → Vẫn chấp nhận vì QR >= Tổng tiền");
                    }

                    // 6. Cập nhật hóa đơn
                    var success = await CapNhatHoaDonThanhToan(hoaDonId, thanhToanInfo, "Chuyển khoản");
                    if (!success)
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ Lỗi cập nhật hóa đơn");
                        return ThanhToanResult.Fail("Lỗi cập nhật hóa đơn");
                    }

                    // 7. Lưu sổ quỹ
                    await LuuSoQuy(hoaDonId, thanhToanInfo.TongTien, "Thu",
                        $"Thanh toán QR HD{hoaDonId:D6} - {maGiaoDichQR}");

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    System.Diagnostics.Debug.WriteLine($"✓✓✓ THANH TOÁN QR THÀNH CÔNG\n");

                    return ThanhToanResult.Success("Thanh toán QR thành công", new
                    {
                        MaHd = hoaDonId,
                        TenBan = thanhToanInfo.TenBan,
                        TongTien = thanhToanInfo.TongTien,
                        MaGiaoDich = maGiaoDichQR,
                        ThoiGianThanhToan = DateTime.Now
                    });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    System.Diagnostics.Debug.WriteLine($"❌ Exception trong ThanhToanQR: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Stack: {ex.StackTrace}");
                    return ThanhToanResult.Fail($"Lỗi: {ex.Message}");
                }
            });
        }

        /// <summary>
        /// Thanh toán thẻ (đang phát triển)
        /// </summary>
        public async Task<ThanhToanResult> ThanhToanThe(int maHd, string soThe, string maGiaoDichThe)
        {
            return ThanhToanResult.Fail("Chức năng thanh toán thẻ đang được phát triển");
        }

        /// <summary>
        /// Cập nhật hóa đơn sau khi thanh toán - ĐÚNG THEO TÍNH TOÁN
        /// </summary>
        private async Task<bool> CapNhatHoaDonThanhToan(int maHd, ThanhToanInfo info, string phuongThuc)
        {
            try
            {
                var hoaDon = await _context.HoaDons
                    .Include(h => h.MaBanNavigation)
                    .FirstOrDefaultAsync(h => h.MaHd == maHd);

                if (hoaDon == null)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ Không tìm thấy hóa đơn {maHd}");
                    return false;
                }

                System.Diagnostics.Debug.WriteLine($"\nCập nhật hóa đơn HD{maHd}:");

                hoaDon.ThoiGianKetThuc = DateTime.Now;
                hoaDon.TienBan = info.TienBan;
                hoaDon.TienDichVu = info.TienDichVu;
                hoaDon.GiamGia = info.GiamGia;
                hoaDon.TongTien = info.TongTien;
                hoaDon.TrangThai = "Đã thanh toán";
                hoaDon.PhuongThucThanhToan = phuongThuc;

                System.Diagnostics.Debug.WriteLine($"  - Tiền bàn: {info.TienBan:N0} đ");
                System.Diagnostics.Debug.WriteLine($"  - Tiền dịch vụ: {info.TienDichVu:N0} đ");
                System.Diagnostics.Debug.WriteLine($"  - Giảm giá: {info.GiamGia:N0} đ");
                System.Diagnostics.Debug.WriteLine($"  - Tổng tiền: {info.TongTien:N0} đ");
                System.Diagnostics.Debug.WriteLine($"  - Phương thức: {phuongThuc}");

                if (hoaDon.MaBanNavigation != null)
                {
                    var ban = hoaDon.MaBanNavigation;
                    ban.TrangThai = "Trống";
                    ban.GioBatDau = null;
                    ban.MaKh = null;
                    ban.GhiChu = null;
                    System.Diagnostics.Debug.WriteLine($"  - Bàn {ban.TenBan} → Trống");
                }

                await _context.SaveChangesAsync();
                System.Diagnostics.Debug.WriteLine($"✓ Cập nhật hóa đơn thành công");

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Lỗi cập nhật hóa đơn: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Lưu vào sổ quỹ
        /// </summary>
        private async Task LuuSoQuy(int maHd, decimal soTien, string loaiPhieu, string lyDo)
        {
            try
            {
                var hoaDon = await _context.HoaDons.FindAsync(maHd);

                var soQuy = new SoQuy
                {
                    LoaiPhieu = loaiPhieu,
                    SoTien = soTien,
                    LyDo = lyDo,
                    MaHdLienQuan = maHd,
                    MaNv = hoaDon?.MaNv ?? 1,
                    NgayLap = DateTime.Now
                };

                _context.SoQuies.Add(soQuy);
                await _context.SaveChangesAsync();

                System.Diagnostics.Debug.WriteLine($"✓ Lưu sổ quỹ: {loaiPhieu} {soTien:N0} đ");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"⚠ Lỗi lưu sổ quỹ: {ex.Message}");
            }
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