using Billiard.DAL.Data;
using Billiard.DAL.Entities;
using Billiard.BLL.Services.QLBan;
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

                // ✅ BƯỚC 1: LẤY THÔNG TIN BOOKING (nếu có)
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

                // ✅ BƯỚC 2: TÍNH THỜI GIAN KẾT THÚC ĐÚNG
                var thoiGianKetThuc = _gioHoatDongService.LayThoiGianKetThucHopLe(
                    hoaDon.ThoiGianBatDau.Value,
                    gioKetThucBooking
                );

                System.Diagnostics.Debug.WriteLine($"Thời gian kết thúc tính toán: {thoiGianKetThuc:HH:mm:ss dd/MM/yyyy}");

                // ✅ BƯỚC 3: TÍNH DURATION VÀ LÀM TRÒN LÊN PHÚT
                var duration = thoiGianKetThuc - hoaDon.ThoiGianBatDau.Value;
                var tongPhut = (int)Math.Ceiling(duration.TotalMinutes);

                System.Diagnostics.Debug.WriteLine($"Duration: {duration.TotalMinutes:F2} phút → Làm tròn: {tongPhut} phút");

                // ✅ BƯỚC 4: TÍNH TIỀN BÀN
                var giaGio = hoaDon.MaBanNavigation?.MaLoaiNavigation?.GiaGio ?? 0;
                if (giaGio == 0)
                {
                    System.Diagnostics.Debug.WriteLine($"⚠ Giá giờ = 0, kiểm tra dữ liệu bàn/loại bàn");
                }
                System.Diagnostics.Debug.WriteLine($"Giá giờ: {giaGio:N0} đ");

                var soGio = (decimal)tongPhut / 60m;
                var tienBan = Math.Round(soGio * giaGio, 2); // ✅ Làm tròn 2 chữ số thập phân
                System.Diagnostics.Debug.WriteLine($"Số giờ: {soGio:F4} ({tongPhut} phút / 60)");
                System.Diagnostics.Debug.WriteLine($"Tiền bàn (đã làm tròn): {tienBan:N2} đ");

                // ✅ BƯỚC 5: TÍNH TIỀN DỊCH VỤ
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

                // ✅ BƯỚC 6: TÍNH TỔNG
                var giamGia = hoaDon.GiamGia ?? 0;
                System.Diagnostics.Debug.WriteLine($"Giảm giá: {giamGia:N0} đ");

                var tamTinh = Math.Round(tienBan + tienDichVu - giamGia, 2); // ✅ Làm tròn
                System.Diagnostics.Debug.WriteLine($"Tạm tính (đã làm tròn): {tamTinh:N2} đ");

                var tongTien = Math.Ceiling(tamTinh / 1000m) * 1000m;
                var chenhLech = Math.Round(tongTien - tamTinh, 2); // ✅ Làm tròn
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
                    return ThanhToanResult.Fail("Không tìm thấy hóa đơn hoặc hóa đơn đã thanh toán");
                }

                if (tienKhachDua < thanhToanInfo.TongTien)
                {
                    var thieu = thanhToanInfo.TongTien - tienKhachDua;
                    return ThanhToanResult.Fail($"Tiền khách đưa không đủ! Còn thiếu {thieu:N0} đ");
                }

                var tienThua = tienKhachDua - thanhToanInfo.TongTien;

                using (var newContext = new BilliardDbContext())
                {
                    var strategy = newContext.Database.CreateExecutionStrategy();

                    return await strategy.ExecuteAsync(async () =>
                    {
                        using var transaction = await newContext.Database.BeginTransactionAsync();
                        try
                        {
                            var hoaDon = await newContext.HoaDons
                                .Include(h => h.MaBanNavigation)
                                .FirstOrDefaultAsync(h => h.MaHd == maHd);

                            if (hoaDon == null)
                            {
                                return ThanhToanResult.Fail("Không tìm thấy hóa đơn");
                            }

                            if (hoaDon.TrangThai != "Đang chơi")
                            {
                                return ThanhToanResult.Fail($"Hóa đơn đã {hoaDon.TrangThai}");
                            }

                            // ✅ LƯU THỜI GIAN THANH TOÁN NGAY KHI THANH TOÁN
                            var thoiGianThanhToan = DateTime.Now;

                            // ✅ Nếu chưa có ThoiGianKetThuc, set = thời gian thanh toán
                            if (!hoaDon.ThoiGianKetThuc.HasValue)
                            {
                                hoaDon.ThoiGianKetThuc = thoiGianThanhToan;
                            }

                            hoaDon.TienBan = Math.Round(thanhToanInfo.TienBan, 2);
                            hoaDon.TienDichVu = Math.Round(thanhToanInfo.TienDichVu, 2);
                            hoaDon.GiamGia = Math.Round(thanhToanInfo.GiamGia, 2);
                            hoaDon.TongTien = Math.Round(thanhToanInfo.TongTien, 2);
                            hoaDon.TrangThai = "Đã thanh toán";
                            hoaDon.PhuongThucThanhToan = "Tiền mặt";

                            // ✅ SET THỜI GIAN THANH TOÁN NGAY TẠI ĐÂY
                            hoaDon.ThoiGianThanhToan = thoiGianThanhToan;

                            // Cập nhật bàn
                            if (hoaDon.MaBanNavigation != null)
                            {
                                var ban = hoaDon.MaBanNavigation;
                                ban.TrangThai = "Trống";
                                ban.GioBatDau = null;
                                ban.MaKh = null;
                                ban.GhiChu = null;
                            }

                            // Lưu sổ quỹ
                            var soQuy = new SoQuy
                            {
                                LoaiPhieu = "Thu",
                                SoTien = thanhToanInfo.TongTien,
                                LyDo = $"Thanh toán tiền mặt HD{maHd:D6}",
                                MaHdLienQuan = maHd,
                                MaNv = hoaDon.MaNv ?? 1,
                                NgayLap = thoiGianThanhToan
                            };
                            newContext.SoQuies.Add(soQuy);

                            await newContext.SaveChangesAsync();
                            await transaction.CommitAsync();

                            System.Diagnostics.Debug.WriteLine($"✓✓✓ THANH TOÁN THÀNH CÔNG - Thời gian: {thoiGianThanhToan:HH:mm:ss}\n");

                            return ThanhToanResult.Success("Thanh toán tiền mặt thành công", new
                            {
                                MaHd = maHd,
                                TenBan = thanhToanInfo.TenBan,
                                TongTien = thanhToanInfo.TongTien,
                                TienKhachDua = tienKhachDua,
                                TienThua = tienThua,
                                ThoiGianThanhToan = thoiGianThanhToan
                            });
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            return ThanhToanResult.Fail($"Lỗi: {ex.Message}");
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                return ThanhToanResult.Fail($"Lỗi: {ex.Message}");
            }
        }
        /// <summary>
        /// ✅ FIXED: Thanh toán QR - Sử dụng DbContext riêng
        /// </summary>
        public async Task<ThanhToanResult> ThanhToanQR(int maHd, string maGiaoDichQR)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"\n=== THANH TOÁN QR HD{maHd} ===");

                var giaoDichQR = await _context.VietqrGiaoDiches
                    .FirstOrDefaultAsync(g => g.MaGiaoDich == maGiaoDichQR);

                if (giaoDichQR == null)
                {
                    return ThanhToanResult.Fail("Không tìm thấy giao dịch QR");
                }

                if (giaoDichQR.TrangThai != "Đã thanh toán")
                {
                    return ThanhToanResult.Fail("Giao dịch QR chưa được xác nhận thanh toán");
                }

                int hoaDonId = giaoDichQR.MaHd;
                var thanhToanInfo = await TinhToanThanhToan(hoaDonId);

                if (thanhToanInfo == null)
                {
                    return ThanhToanResult.Fail($"Không tìm thấy hóa đơn {hoaDonId}");
                }

                if (giaoDichQR.SoTien < thanhToanInfo.TongTien)
                {
                    var thieu = thanhToanInfo.TongTien - giaoDichQR.SoTien;
                    return ThanhToanResult.Fail($"Số tiền QR không đủ! Thiếu {thieu:N0} đ");
                }

                using (var newContext = new BilliardDbContext())
                {
                    var strategy = newContext.Database.CreateExecutionStrategy();

                    return await strategy.ExecuteAsync(async () =>
                    {
                        using var transaction = await newContext.Database.BeginTransactionAsync();
                        try
                        {
                            var hoaDon = await newContext.HoaDons
                                .Include(h => h.MaBanNavigation)
                                .FirstOrDefaultAsync(h => h.MaHd == hoaDonId);

                            if (hoaDon == null)
                            {
                                return ThanhToanResult.Fail("Không tìm thấy hóa đơn");
                            }

                            if (hoaDon.TrangThai != "Đang chơi")
                            {
                                return ThanhToanResult.Fail($"Hóa đơn đã {hoaDon.TrangThai}");
                            }

                            // ✅ LƯU THỜI GIAN THANH TOÁN NGAY KHI THANH TOÁN
                            var thoiGianThanhToan = DateTime.Now;

                            // ✅ Nếu chưa có ThoiGianKetThuc, set = thời gian thanh toán
                            if (!hoaDon.ThoiGianKetThuc.HasValue)
                            {
                                hoaDon.ThoiGianKetThuc = thoiGianThanhToan;
                            }

                            hoaDon.TienBan = Math.Round(thanhToanInfo.TienBan, 2);
                            hoaDon.TienDichVu = Math.Round(thanhToanInfo.TienDichVu, 2);
                            hoaDon.GiamGia = Math.Round(thanhToanInfo.GiamGia, 2);
                            hoaDon.TongTien = Math.Round(thanhToanInfo.TongTien, 2);
                            hoaDon.TrangThai = "Đã thanh toán";
                            hoaDon.PhuongThucThanhToan = "Chuyển khoản";

                            // ✅ SET THỜI GIAN THANH TOÁN NGAY TẠI ĐÂY
                            hoaDon.ThoiGianThanhToan = thoiGianThanhToan;

                            // Cập nhật bàn
                            if (hoaDon.MaBanNavigation != null)
                            {
                                var ban = hoaDon.MaBanNavigation;
                                ban.TrangThai = "Trống";
                                ban.GioBatDau = null;
                                ban.MaKh = null;
                                ban.GhiChu = null;
                            }

                            // Lưu sổ quỹ
                            var soQuy = new SoQuy
                            {
                                LoaiPhieu = "Thu",
                                SoTien = thanhToanInfo.TongTien,
                                LyDo = $"Thanh toán QR HD{hoaDonId:D6} - {maGiaoDichQR}",
                                MaHdLienQuan = hoaDonId,
                                MaNv = hoaDon.MaNv ?? 1,
                                NgayLap = thoiGianThanhToan
                            };
                            newContext.SoQuies.Add(soQuy);

                            await newContext.SaveChangesAsync();
                            await transaction.CommitAsync();

                            System.Diagnostics.Debug.WriteLine($"✓✓✓ THANH TOÁN QR THÀNH CÔNG - Thời gian: {thoiGianThanhToan:HH:mm:ss}\n");

                            return ThanhToanResult.Success("Thanh toán QR thành công", new
                            {
                                MaHd = hoaDonId,
                                TenBan = thanhToanInfo.TenBan,
                                TongTien = thanhToanInfo.TongTien,
                                MaGiaoDich = maGiaoDichQR,
                                ThoiGianThanhToan = thoiGianThanhToan
                            });
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            return ThanhToanResult.Fail($"Lỗi: {ex.Message}");
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                return ThanhToanResult.Fail($"Lỗi: {ex.Message}");
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

                // ✅ 4. CẬP NHẬT BÀN
                if (hoaDon.MaBanNavigation != null)
                {
                    var ban = hoaDon.MaBanNavigation;
                    System.Diagnostics.Debug.WriteLine($"  - Bàn {ban.TenBan}: {ban.TrangThai} → Trống");

                    ban.TrangThai = "Trống";
                    ban.GioBatDau = null;
                    ban.MaKh = null;
                    ban.GhiChu = null;
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