using Billiard.BLL.Services.QLBan;
using Billiard.DAL.Data;
using Billiard.DAL.Entities;
using Emgu.CV.Ocl;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Billiard.BLL.Services.HoaDonServices
{
    public class ThanhToanService
    {
        private readonly BilliardDbContext _context;
        private readonly GioHoatDongService _gioHoatDongService;

        public ThanhToanService(BilliardDbContext context)
        {
            _context = context;
            _gioHoatDongService = new GioHoatDongService();
        }

        /// <summary>
        /// ✅ FIXED: Tính toán chi tiết thanh toán - ĐÚNG THEO BOOKING VÀ GIỜ ĐÓNG CỬA
        /// </summary>
        public async Task<ThanhToanInfo> TinhToanThanhToan(int maHd)
        {
            try
            {
                var hoaDon = await _context.HoaDons
                    .AsNoTracking()
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

                System.Diagnostics.Debug.WriteLine($"\n=== TÍNH TOÁN THANH TOÁN HD{maHd} ===");
                System.Diagnostics.Debug.WriteLine($"Thời gian bắt đầu: {hoaDon.ThoiGianBatDau.Value:HH:mm:ss dd/MM/yyyy}");

                // Lấy thông tin booking (nếu có)
                DateTime? gioKetThucBooking = null;
                try
                {
                    var datBan = await _context.DatBans
                        .FirstOrDefaultAsync(d =>
                            d.MaBan == hoaDon.MaBan &&
                            d.TrangThai == "Đã xác nhận" &&
                            d.ThoiGianKetThuc.HasValue);

                    if (datBan != null)
                    {
                        gioKetThucBooking = datBan.ThoiGianKetThuc.Value;
                        System.Diagnostics.Debug.WriteLine($"✓ Có booking - Giờ kết thúc: {gioKetThucBooking.Value:HH:mm}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"ℹ Không có booking cho bàn này");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"⚠ Lỗi khi lấy thông tin booking: {ex.Message}");
                }

                // Tính thời gian kết thúc
                var thoiGianKetThuc = _gioHoatDongService.LayThoiGianKetThucHopLe(
                    hoaDon.ThoiGianBatDau.Value,
                    gioKetThucBooking
                );

                System.Diagnostics.Debug.WriteLine($"Thời gian kết thúc tính toán: {thoiGianKetThuc:HH:mm:ss dd/MM/yyyy}");

                // Tính duration và làm tròn
                var duration = thoiGianKetThuc - hoaDon.ThoiGianBatDau.Value;
                var tongPhut = (int)Math.Ceiling(duration.TotalMinutes);

                System.Diagnostics.Debug.WriteLine($"Duration: {duration.TotalMinutes:F2} phút → Làm tròn: {tongPhut} phút");

                // Tính tiền bàn
                var giaGio = hoaDon.MaBanNavigation?.MaLoaiNavigation?.GiaGio ?? 0;
                if (giaGio == 0)
                {
                    System.Diagnostics.Debug.WriteLine($"⚠ Giá giờ = 0, kiểm tra dữ liệu bàn/loại bàn");
                }
                System.Diagnostics.Debug.WriteLine($"Giá giờ: {giaGio:N0} đ");

                var soGio = (decimal)tongPhut / 60m;
                var tienBan = Math.Round(soGio * giaGio, 2);
                System.Diagnostics.Debug.WriteLine($"Số giờ: {soGio:F4} ({tongPhut} phút / 60)");
                System.Diagnostics.Debug.WriteLine($"Tiền bàn (đã làm tròn): {tienBan:N2} đ");

                // Tính tiền dịch vụ
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

                // Tính tổng
                var giamGia = hoaDon.GiamGia ?? 0;
                System.Diagnostics.Debug.WriteLine($"Giảm giá: {giamGia:N0} đ");

                var tamTinh = Math.Round(tienBan + tienDichVu - giamGia, 2);
                System.Diagnostics.Debug.WriteLine($"Tạm tính (đã làm tròn): {tamTinh:N2} đ");

                var tongTien = Math.Ceiling(tamTinh / 1000m) * 1000m;
                var chenhLech = Math.Round(tongTien - tamTinh, 2);
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
        /// ✅ FIXED: Thanh toán tiền mặt - Sử dụng DbContext riêng để tránh tracking conflict
        /// </summary>
        public async Task<ThanhToanResult> ThanhToanTienMat(int maHd, decimal tienKhachDua)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"\n=== THANH TOÁN TIỀN MẶT HD{maHd} ===");

                var thanhToanInfo = await TinhToanThanhToan(maHd);
                if (thanhToanInfo == null)
                {
                    return ThanhToanResult.Fail("Không thể tính toán thanh toán");
                }

                if (tienKhachDua < thanhToanInfo.TongTien)
                {
                    return ThanhToanResult.Fail($"Tiền khách đưa không đủ! Cần {thanhToanInfo.TongTien:N0} đ");
                }

                var tienThua = tienKhachDua - thanhToanInfo.TongTien;
                System.Diagnostics.Debug.WriteLine($"Tiền khách đưa: {tienKhachDua:N0} đ");
                System.Diagnostics.Debug.WriteLine($"Tiền thừa: {tienThua:N0} đ");

                var success = await XuLyThanhToan(maHd, thanhToanInfo, "Tiền mặt");
                if (!success)
                {
                    return ThanhToanResult.Fail("Lỗi khi xử lý thanh toán");
                }

                await LuuSoQuy(maHd, thanhToanInfo.TongTien, "Phiếu thu",
                    $"Thu tiền bàn HD{maHd} - Tiền mặt");

                System.Diagnostics.Debug.WriteLine("✓✓✓ THANH TOÁN THÀNH CÔNG!\n");

                return ThanhToanResult.Success("Thanh toán thành công!", new
                {
                    ThanhToanInfo = thanhToanInfo,
                    TienKhachDua = tienKhachDua,
                    TienThua = tienThua
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Exception: {ex.Message}");
                return ThanhToanResult.Fail($"Lỗi: {ex.Message}");
            }
        }
        /// <summary>
        /// ✅ FIXED: Thanh toán QR - Sử dụng DbContext riêng
        /// </summary>
        public async Task<ThanhToanResult> ThanhToanQR(int maHd)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"\n=== THANH TOÁN CHUYỂN KHOẢN HD{maHd} ===");

                var thanhToanInfo = await TinhToanThanhToan(maHd);
                if (thanhToanInfo == null)
                {
                    return ThanhToanResult.Fail("Không thể tính toán thanh toán");
                }

                var success = await XuLyThanhToan(maHd, thanhToanInfo, "Chuyển khoản");
                if (!success)
                {
                    return ThanhToanResult.Fail("Lỗi khi xử lý thanh toán");
                }

                await LuuSoQuy(maHd, thanhToanInfo.TongTien, "Phiếu thu",
                    $"Thu tiền bàn HD{maHd} - Chuyển khoản");

                System.Diagnostics.Debug.WriteLine("✓✓✓ THANH TOÁN THÀNH CÔNG!\n");

                return ThanhToanResult.Success("Thanh toán thành công!", new
                {
                    ThanhToanInfo = thanhToanInfo
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Exception: {ex.Message}");
                return ThanhToanResult.Fail($"Lỗi: {ex.Message}");
            }
        }
        private async Task<bool> XuLyThanhToan(int maHd, ThanhToanInfo info, string phuongThuc)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"\n=== XỬ LÝ THANH TOÁN ===");
                System.Diagnostics.Debug.WriteLine($"HD{maHd} - Phương thức: {phuongThuc}");

                // ✅ 1. LẤY HÓA ĐƠN
                var hoaDon = await _context.HoaDons
                    .Include(h => h.MaBanNavigation)
                    .FirstOrDefaultAsync(h => h.MaHd == maHd);

                if (hoaDon == null)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ Không tìm thấy hóa đơn HD{maHd}");
                    return false;
                }

                System.Diagnostics.Debug.WriteLine($"✓ Tìm thấy hóa đơn HD{maHd}");

                // ✅ 2. CẬP NHẬT ĐƠN ĐẶT HIỆN TẠI VỀ "ĐÃ HOÀN THÀNH"
                var currentDatBan = await _context.DatBans
                    .FirstOrDefaultAsync(d => d.MaBan == hoaDon.MaBan && d.TrangThai == "Đã xác nhận");

                if (currentDatBan != null)
                {
                    currentDatBan.TrangThai = "Đã hoàn thành";
                    _context.Entry(currentDatBan).State = EntityState.Modified;
                    System.Diagnostics.Debug.WriteLine($"✓ Cập nhật đơn đặt {currentDatBan.MaDat} → 'Đã hoàn thành'");
                }

                // ✅ 3. CẬP NHẬT HÓA ĐƠN
                hoaDon.ThoiGianKetThuc = DateTime.Now;
                hoaDon.TienBan = info.TienBan;
                hoaDon.TienDichVu = info.TienDichVu;
                hoaDon.GiamGia = info.GiamGia;
                hoaDon.TongTien = info.TongTien;
                hoaDon.TrangThai = "Đã thanh toán";
                hoaDon.PhuongThucThanhToan = phuongThuc;

                System.Diagnostics.Debug.WriteLine($"  - Thời gian kết thúc: {hoaDon.ThoiGianKetThuc:HH:mm:ss}");
                System.Diagnostics.Debug.WriteLine($"  - Tiền bàn: {info.TienBan:N0} đ");
                System.Diagnostics.Debug.WriteLine($"  - Tiền dịch vụ: {info.TienDichVu:N0} đ");
                System.Diagnostics.Debug.WriteLine($"  - Giảm giá: {info.GiamGia:N0} đ");
                System.Diagnostics.Debug.WriteLine($"  - Tổng tiền: {info.TongTien:N0} đ");
                System.Diagnostics.Debug.WriteLine($"  - Phương thức: {phuongThuc}");
                System.Diagnostics.Debug.WriteLine($"  - Trạng thái mới: {hoaDon.TrangThai}");

                // ✅ 4. LUÔN LUÔN TRẢ BÀN VỀ TRỐNG - KHÔNG KIỂM TRA ĐƠN ĐẶT TIẾP THEO
                if (hoaDon.MaBanNavigation != null)
                {
                    var ban = hoaDon.MaBanNavigation;
                    System.Diagnostics.Debug.WriteLine($"\n🔄 Xử lý bàn {ban.TenBan}:");
                    System.Diagnostics.Debug.WriteLine($"  - Trạng thái cũ: {ban.TrangThai}");

                    // ✅ LOGIC MỚI: LUÔN TRẢ VỀ TRỐNG
                    ban.TrangThai = "Trống";
                    ban.GioBatDau = null;
                    ban.MaKh = null;
                    ban.GhiChu = null;

                    System.Diagnostics.Debug.WriteLine($"  - Trạng thái mới: {ban.TrangThai}");
                    System.Diagnostics.Debug.WriteLine($"  ✓ Đã reset tất cả thông tin bàn về trạng thái ban đầu");
                    System.Diagnostics.Debug.WriteLine($"\n💡 LƯU Ý: Nếu có đơn đặt tiếp theo, hệ thống sẽ tự động");
                    System.Diagnostics.Debug.WriteLine($"         cập nhật trạng thái bàn khi đến gần giờ đặt");
                }

                // ✅ 5. MARK ENTITIES AS MODIFIED
                _context.Entry(hoaDon).State = EntityState.Modified;
                if (hoaDon.MaBanNavigation != null)
                {
                    _context.Entry(hoaDon.MaBanNavigation).State = EntityState.Modified;
                }

                System.Diagnostics.Debug.WriteLine($"\n✓ Đã đánh dấu entities là Modified");

                // ✅ 6. LƯU THAY ĐỔI
                var savedCount = await _context.SaveChangesAsync();
                System.Diagnostics.Debug.WriteLine($"✓ Đã lưu {savedCount} thay đổi vào DB");
                System.Diagnostics.Debug.WriteLine($"✅ HOÀN TẤT XỬ LÝ THANH TOÁN");

                return true;
            }
            catch (DbUpdateException dbEx)
            {
                System.Diagnostics.Debug.WriteLine($"❌ DbUpdateException: {dbEx.Message}");
                if (dbEx.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"   Inner: {dbEx.InnerException.Message}");
                }
                System.Diagnostics.Debug.WriteLine($"   Stack: {dbEx.StackTrace}");
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Exception: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"   Inner: {ex.InnerException?.Message}");
                System.Diagnostics.Debug.WriteLine($"   Stack: {ex.StackTrace}");
                return false;
            }
        }
        public async Task<bool> CapNhatThoiGianThanhToan(int maHd, DateTime? thoiGianThanhToan = null)
        {
            try
            {
                using (var context = new BilliardDbContext())
                {
                    var hoaDon = await context.HoaDons.FindAsync(maHd);

                    if (hoaDon == null)
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ Không tìm thấy hóa đơn {maHd}");
                        return false;
                    }

                    // ✅ Sử dụng thời gian truyền vào hoặc thời gian hiện tại
                    hoaDon.ThoiGianThanhToan = thoiGianThanhToan ?? DateTime.Now;

                    await context.SaveChangesAsync();

                    System.Diagnostics.Debug.WriteLine($"✓ Cập nhật thời gian thanh toán HD{maHd}: {hoaDon.ThoiGianThanhToan:HH:mm:ss dd/MM/yyyy}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Lỗi cập nhật thời gian thanh toán: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> CapNhatThoiGianInHoaDon(int maHd)
        {
            try
            {
                using (var context = new BilliardDbContext())
                {
                    var hoaDon = await context.HoaDons.FindAsync(maHd);

                    if (hoaDon == null || hoaDon.TrangThai != "Đã thanh toán")
                    {
                        return false;
                    }

                    hoaDon.ThoiGianThanhToan = DateTime.Now;
                    await context.SaveChangesAsync();

                    System.Diagnostics.Debug.WriteLine($"✓ Cập nhật thời gian in hóa đơn HD{maHd}: {DateTime.Now:HH:mm:ss}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Lỗi cập nhật thời gian in: {ex.Message}");
                return false;
            }
        }
        /// <summary>
        /// ✅ CRITICAL FIX: Cập nhật hóa đơn - KHÔNG tự tính lại, dùng ThanhToanInfo đã truyền vào
        /// </summary>
        private async Task<bool> CapNhatHoaDonThanhToan(int maHd, ThanhToanInfo info, string phuongThuc)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"\n=== CẬP NHẬT HÓA ĐƠN HD{maHd} ===");

                // ✅ 1. DETACH tất cả entities đang tracking để tránh conflict
                var trackedEntities = _context.ChangeTracker.Entries()
                    .Where(e => e.State != EntityState.Detached)
                    .ToList();

                foreach (var entry in trackedEntities)
                {
                    entry.State = EntityState.Detached;
                }

                System.Diagnostics.Debug.WriteLine($"✓ Đã detach {trackedEntities.Count} entities");

                // ✅ 2. LẤY LẠI HÓA ĐƠN TỪ DB (fresh query, không tracking cũ)
                var hoaDon = await _context.HoaDons
                    .Include(h => h.MaBanNavigation)
                    .FirstOrDefaultAsync(h => h.MaHd == maHd);

                if (hoaDon == null)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ Không tìm thấy hóa đơn {maHd}");
                    return false;
                }

                System.Diagnostics.Debug.WriteLine($"✓ Tìm thấy hóa đơn - Trạng thái hiện tại: {hoaDon.TrangThai}");

                if (hoaDon.TrangThai != "Đang chơi")
                {
                    System.Diagnostics.Debug.WriteLine($"❌ Hóa đơn không ở trạng thái 'Đang chơi': {hoaDon.TrangThai}");
                    return false;
                }

                System.Diagnostics.Debug.WriteLine($"✓ Bắt đầu cập nhật hóa đơn...");

                // ✅ 3. CẬP NHẬT HÓA ĐƠN - SỬ DỤNG DỮ LIỆU TỪ ThanhToanInfo
                hoaDon.ThoiGianKetThuc = DateTime.Now;
                hoaDon.TienBan = info.TienBan;
                hoaDon.TienDichVu = info.TienDichVu;
                hoaDon.GiamGia = info.GiamGia;
                hoaDon.TongTien = info.TongTien;
                hoaDon.TrangThai = "Đã thanh toán";
                hoaDon.PhuongThucThanhToan = phuongThuc;

                System.Diagnostics.Debug.WriteLine($"  - Thời gian kết thúc: {hoaDon.ThoiGianKetThuc:HH:mm:ss}");
                System.Diagnostics.Debug.WriteLine($"  - Tiền bàn: {info.TienBan:N0} đ");
                System.Diagnostics.Debug.WriteLine($"  - Tiền dịch vụ: {info.TienDichVu:N0} đ");
                System.Diagnostics.Debug.WriteLine($"  - Giảm giá: {info.GiamGia:N0} đ");
                System.Diagnostics.Debug.WriteLine($"  - Tổng tiền: {info.TongTien:N0} đ");
                System.Diagnostics.Debug.WriteLine($"  - Phương thức: {phuongThuc}");
                System.Diagnostics.Debug.WriteLine($"  - Trạng thái mới: {hoaDon.TrangThai}");

                // ✅ 4. CẬP NHẬT BÀN - KIỂM TRA ĐƠN ĐẶT TIẾP THEO
                if (hoaDon.MaBanNavigation != null)
                {
                    var ban = hoaDon.MaBanNavigation;
                    System.Diagnostics.Debug.WriteLine($"  - Bàn {ban.TenBan}: {ban.TrangThai} → Kiểm tra đơn đặt tiếp theo");

                    // ✅ TÌM ĐƠN ĐẶT TIẾP THEO (sử dụng cùng DbContext)
                    var now = DateTime.Now;
                    var nextReservation = await _context.DatBans
                        .Include(d => d.MaBanNavigation)
                        .Include(d => d.MaKhNavigation)
                        .Where(d => d.MaBan == ban.MaBan
                            && (d.TrangThai == "Đang chờ" || d.TrangThai == "Đã đặt")
                            && d.ThoiGianBatDau.HasValue
                            && d.ThoiGianBatDau.Value >= now)
                        .OrderBy(d => d.ThoiGianBatDau)
                        .FirstOrDefaultAsync();

                    if (nextReservation != null)
                    {
                        // ✅ CÓ ĐƠN ĐẶT TIẾP THEO - CHUYỂN BÀN SANG "ĐÃ ĐẶT"
                        System.Diagnostics.Debug.WriteLine($"  ✓ Tìm thấy đơn đặt tiếp theo:");
                        System.Diagnostics.Debug.WriteLine($"    - Mã đơn: {nextReservation.MaDat}");
                        System.Diagnostics.Debug.WriteLine($"    - Khách: {nextReservation.TenKhach}");
                        System.Diagnostics.Debug.WriteLine($"    - Thời gian: {nextReservation.ThoiGianBatDau:HH:mm} - {nextReservation.ThoiGianKetThuc:HH:mm}");

                        // Cập nhật trạng thái bàn
                        ban.TrangThai = "Đã đặt";
                        ban.MaKh = nextReservation.MaKh;
                        ban.GhiChu = nextReservation.GhiChu;
                        ban.GioBatDau = null; // Reset giờ bắt đầu, chờ xác nhận

                        // ✅ QUAN TRỌNG: Cập nhật trạng thái đơn đặt thành "Đã đặt" (nếu đang là "Đang chờ")
                        if (nextReservation.TrangThai == "Đang chờ")
                        {
                            nextReservation.TrangThai = "Đã đặt";
                            _context.Entry(nextReservation).State = EntityState.Modified;
                            System.Diagnostics.Debug.WriteLine($"    - Đã cập nhật trạng thái đơn đặt: 'Đang chờ' → 'Đã đặt'");
                        }

                        System.Diagnostics.Debug.WriteLine($"  ✓ Đã chuyển bàn {ban.TenBan} sang 'Đã đặt' cho ca tiếp theo");
                    }
                    else
                    {
                        // ✅ KHÔNG CÓ ĐƠN ĐẶT TIẾP THEO - TRẢ VỀ TRỐNG
                        System.Diagnostics.Debug.WriteLine($"  - Không có đơn đặt tiếp theo, bàn {ban.TenBan} về 'Trống'");
                        ban.TrangThai = "Trống";
                        ban.GioBatDau = null;
                        ban.MaKh = null;
                        ban.GhiChu = null;
                    }
                }

                // ✅ 5. MARK ENTITIES AS MODIFIED
                _context.Entry(hoaDon).State = EntityState.Modified;
                if (hoaDon.MaBanNavigation != null)
                {
                    _context.Entry(hoaDon.MaBanNavigation).State = EntityState.Modified;
                }

                System.Diagnostics.Debug.WriteLine($"✓ Đã đánh dấu entities là Modified");

                // ✅ 6. LƯU THAY ĐỔI (KHÔNG commit transaction ở đây)
                var savedCount = await _context.SaveChangesAsync();
                System.Diagnostics.Debug.WriteLine($"✓ Đã lưu {savedCount} thay đổi vào DB");

                return true;
            }
            catch (DbUpdateException dbEx)
            {
                System.Diagnostics.Debug.WriteLine($"❌ DbUpdateException: {dbEx.Message}");
                if (dbEx.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"   Inner: {dbEx.InnerException.Message}");
                }
                System.Diagnostics.Debug.WriteLine($"   Stack: {dbEx.StackTrace}");
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Exception: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"   Inner: {ex.InnerException?.Message}");
                System.Diagnostics.Debug.WriteLine($"   Stack: {ex.StackTrace}");
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