using Billiard.DAL.Data;
using Billiard.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Billiard.BLL.Services.QLBan
{
    public class DatBanService
    {
        private readonly BilliardDbContext _context;

        public DatBanService(BilliardDbContext context)
        {
            _context = context;
        }

        public async Task<List<DatBan>> GetAllActiveAsync()
        {
            return await _context.DatBans
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b.MaKhuVucNavigation)
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b.MaLoaiNavigation)
                .Include(d => d.MaKhNavigation)
                .Where(d => d.TrangThai == "Đang chờ" || d.TrangThai == "Đã xác nhận" || d.TrangThai == "Đã đặt")
                .OrderBy(d => d.ThoiGianBatDau)
                .ToListAsync();
        }

        public async Task<DatBan> GetByIdAsync(int maDat)
        {
            return await _context.DatBans
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b.MaKhuVucNavigation)
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b.MaLoaiNavigation)
                .Include(d => d.MaKhNavigation)
                .FirstOrDefaultAsync(d => d.MaDat == maDat);
        }

        public async Task<List<DatBan>> GetByTableAsync(int maBan)
        {
            return await _context.DatBans
                .Include(d => d.MaKhNavigation)
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b.MaKhuVucNavigation)
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b.MaLoaiNavigation)
                .Where(d => d.MaBan == maBan &&
                    (d.TrangThai == "Đang chờ" || d.TrangThai == "Đã xác nhận" || d.TrangThai == "Đã đặt"))
                .OrderBy(d => d.ThoiGianDat)
                .ToListAsync();
        }

        public async Task<List<DatBan>> GetByCustomerAsync(int maKh)
        {
            return await _context.DatBans
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b.MaKhuVucNavigation)
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b.MaLoaiNavigation)
                .Where(d => d.MaKh == maKh && d.TrangThai == "Đang chờ")
                .OrderBy(d => d.ThoiGianDat)
                .ToListAsync();
        }

        public async Task<List<DatBan>> GetByDateRangeAsync(DateTime tuNgay, DateTime denNgay)
        {
            return await _context.DatBans
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b.MaKhuVucNavigation)
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b.MaLoaiNavigation)
                .Include(d => d.MaKhNavigation)
                .Where(d => d.ThoiGianDat >= tuNgay && d.ThoiGianDat <= denNgay)
                .OrderBy(d => d.ThoiGianDat)
                .ToListAsync();
        }

        /// <summary>
        /// ✨ CẢI TIẾN: Kiểm tra xem bàn có bị trùng lịch trong khoảng thời gian không
        /// Logic: Hai khoảng thời gian KHÔNG TRÙNG nhau khi:
        /// - Khoảng A kết thúc TRƯỚC khi khoảng B bắt đầu
        /// - Khoảng B kết thúc TRƯỚC khi khoảng A bắt đầu
        /// 
        /// Ví dụ:
        /// - Khách A đặt: 10:00 - 12:00
        /// - Khách B đặt: 12:10 - 14:00 → OK (cách nhau 10 phút)
        /// - Khách C đặt: 11:00 - 13:00 → KHÔNG OK (trùng từ 11:00-12:00)
        /// </summary>
        public async Task<bool> IsTableReservedAsync(int maBan, DateTime thoiGianBatDau, DateTime thoiGianKetThuc, int? excludeMaDat = null)
        {
            // Lấy tất cả các đơn đặt bàn đang hoạt động của bàn này
            var existingReservations = await _context.DatBans
                .Where(d => d.MaBan == maBan
                    && (d.TrangThai == "Đang chờ" || d.TrangThai == "Đã xác nhận" || d.TrangThai == "Đã đặt")
                    && d.ThoiGianKetThuc.HasValue)
                .ToListAsync();

            // Nếu đang cập nhật đơn đặt, loại bỏ chính nó khỏi danh sách kiểm tra
            if (excludeMaDat.HasValue)
            {
                existingReservations = existingReservations.Where(d => d.MaDat != excludeMaDat.Value).ToList();
            }

            // Kiểm tra từng đơn đặt xem có trùng không
            foreach (var reservation in existingReservations)
            {
                var existingStart = reservation.ThoiGianBatDau;
                var existingEnd = reservation.ThoiGianKetThuc.Value;

                // ✅ LOGIC KIỂM TRA TRÙNG LẶP:
                // Hai khoảng thời gian TRÙNG nhau nếu:
                // - Thời gian bắt đầu mới < Thời gian kết thúc cũ
                // - Thời gian kết thúc mới > Thời gian bắt đầu cũ

                bool isTrungLap = thoiGianBatDau < existingEnd && thoiGianKetThuc > existingStart;

                if (isTrungLap)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ TRÙNG LẶP PHÁT HIỆN:");
                    System.Diagnostics.Debug.WriteLine($"   Đơn đặt hiện tại: {existingStart:HH:mm dd/MM} - {existingEnd:HH:mm dd/MM}");
                    System.Diagnostics.Debug.WriteLine($"   Đơn đặt mới: {thoiGianBatDau:HH:mm dd/MM} - {thoiGianKetThuc:HH:mm dd/MM}");
                    return true; // Có trùng lặp
                }
            }

            System.Diagnostics.Debug.WriteLine($"✅ KHÔNG TRÙNG LẶP - Bàn {maBan} có thể đặt từ {thoiGianBatDau:HH:mm} đến {thoiGianKetThuc:HH:mm}");
            return false; // Không có trùng lặp
        }

        /// <summary>
        /// ✨ MỚI: Lấy danh sách các khung giờ đã đặt của một bàn
        /// Giúp hiển thị cho người dùng biết bàn đã được đặt vào khung giờ nào
        /// </summary>
        public async Task<List<(DateTime BatDau, DateTime KetThuc, string? TenKhach)>> GetReservedTimeSlotsAsync(int maBan, DateTime ngay)
        {
            var startOfDay = ngay.Date;
            var endOfDay = startOfDay.AddDays(1).AddSeconds(-1);

            var reservations = await _context.DatBans
                .Where(d => d.MaBan == maBan
                    && (d.TrangThai == "Đang chờ" || d.TrangThai == "Đã xác nhận" || d.TrangThai == "Đã đặt")
                    && d.ThoiGianBatDau >= startOfDay
                    && d.ThoiGianBatDau <= endOfDay
                    && d.ThoiGianKetThuc.HasValue)
                .OrderBy(d => d.ThoiGianBatDau)
                .Select(d => new
                {
                    BatDau = d.ThoiGianBatDau ?? startOfDay,
                    KetThuc = d.ThoiGianKetThuc!.Value,
                    d.TenKhach
                })
                .ToListAsync();

            return reservations
                .Select(r => (r.BatDau, r.KetThuc, r.TenKhach))
                .ToList();
        }
        public async Task<KhachHang> GetCustomerByPhoneNumberAsync(string sdt)
        {
            return await _context.KhachHangs
                .FirstOrDefaultAsync(k => k.Sdt == sdt);
        }

        // ✅ LOGIC ĐÚNG: Khi khách đặt bàn → Bàn VẪN TRỐNG, chờ xác nhận
        public async Task<bool> ReserveTableAsync(
            int maBan,
            int? maKhachHang,
            string tenKhach,
            string sdt,
            DateTime thoiGianBatDau,
            DateTime thoiGianKetThuc,
            string ghiChu
        )
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // ✅ KIỂM TRA TRÙNG LẶP TRƯỚC KHI ĐẶT
                var isReserved = await IsTableReservedAsync(maBan, thoiGianBatDau, thoiGianKetThuc);
                if (isReserved)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ Bàn {maBan} đã bị đặt trong khung giờ này!");
                    return false;
                }

                // 1. Xử lý khách hàng
                int maKh = maKhachHang ?? 0;
                if (!maKhachHang.HasValue)
                {
                    var existingCustomer = await GetCustomerByPhoneNumberAsync(sdt);
                    if (existingCustomer != null)
                    {
                        maKh = existingCustomer.MaKh;
                    }
                    else
                    {
                        var newCustomer = new KhachHang
                        {
                            TenKh = tenKhach,
                            Sdt = sdt,
                            NgayDangKy = DateTime.Now,
                        };
                        _context.KhachHangs.Add(newCustomer);
                        await _context.SaveChangesAsync();
                        maKh = newCustomer.MaKh;
                    }
                }

                // 2. Tạo đơn đặt bàn - TRẠNG THÁI = "Đang chờ"
                var datBan = new DatBan
                {
                    MaBan = maBan,
                    MaKh = maKh,
                    TenKhach = tenKhach,
                    Sdt = sdt,
                    ThoiGianBatDau = thoiGianBatDau,
                    ThoiGianKetThuc = thoiGianKetThuc,
                    GhiChu = ghiChu,
                    ThoiGianDat = DateTime.Now,
                    TrangThai = "Đang chờ",
                    SoNguoi = 1
                };

                _context.DatBans.Add(datBan);

                // 3. ✅ QUAN TRỌNG: BÀN VẪN GIỮ TRẠNG THÁI "Trống" - KHÔNG ĐỔI
                var ban = await _context.BanBia.FindAsync(maBan);
                if (ban != null)
                {
                    System.Diagnostics.Debug.WriteLine($"✓ Tạo đơn đặt cho bàn {ban.TenBan} - Bàn vẫn trống, chờ xác nhận");
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                System.Diagnostics.Debug.WriteLine($"✓✓✓ ĐẶT BÀN THÀNH CÔNG - DatBan: Đang chờ, BanBia: Trống (chờ xác nhận)");
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                System.Diagnostics.Debug.WriteLine($"❌ Lỗi đặt bàn: {ex.Message}");
                return false;
            }
        }

        // ✅ THÊM HÀM XÁC NHẬN ĐƠN ĐẶT (cửa hàng xác nhận)
        public async Task<bool> ConfirmReservationAsync(int maDat)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var datBan = await _context.DatBans
                    .Include(d => d.MaBanNavigation)
                    .FirstOrDefaultAsync(d => d.MaDat == maDat);

                if (datBan == null || datBan.TrangThai != "Đang chờ")
                    return false;

                // Cập nhật đơn đặt
                datBan.TrangThai = "Đã xác nhận";

                // ✅ BÂY GIỜ MỚI CẬP NHẬT BÀN SANG "Đã đặt"
                var ban = datBan.MaBanNavigation;
                if (ban != null)
                {
                    ban.TrangThai = "Đã đặt";
                    ban.MaKh = datBan.MaKh;
                    ban.GhiChu = datBan.GhiChu;

                    System.Diagnostics.Debug.WriteLine($"✓ Xác nhận đơn đặt - Bàn {ban.TenBan} chuyển sang 'Đã đặt'");
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                System.Diagnostics.Debug.WriteLine($"❌ Lỗi xác nhận: {ex.Message}");
                return false;
            }
        }

        public async Task<List<DatBan>> GetReservationsNearStartTimeAsync()
        {
            var now = DateTime.Now;
            var fiveMinutesAgo = now.AddMinutes(-5);  // Quá giờ 5 phút
            var fiveMinutesLater = now.AddMinutes(5); // Sắp đến 5 phút

            System.Diagnostics.Debug.WriteLine($"🔍 Tìm đơn đặt từ {fiveMinutesAgo:HH:mm} đến {fiveMinutesLater:HH:mm}");

            var reservations = await _context.DatBans
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b.MaKhuVucNavigation)
                .Include(d => d.MaKhNavigation)
                .Where(d => (d.TrangThai == "Đang chờ" || d.TrangThai == "Đã xác nhận")
                    && d.ThoiGianBatDau.HasValue
                    && d.ThoiGianBatDau.Value >= fiveMinutesAgo
                    && d.ThoiGianBatDau.Value <= fiveMinutesLater)
                .OrderBy(d => d.ThoiGianBatDau)
                .ToListAsync();

            System.Diagnostics.Debug.WriteLine($"   Tìm thấy {reservations.Count} đơn đặt");

            foreach (var r in reservations)
            {
                var diff = (r.ThoiGianBatDau.Value - now).TotalMinutes;
                System.Diagnostics.Debug.WriteLine(
                    $"   - Đơn #{r.MaDat}: {r.MaBanNavigation?.TenBan} " +
                    $"lúc {r.ThoiGianBatDau:HH:mm} " +
                    $"({(diff >= 0 ? $"còn {diff:F0} phút" : $"quá {Math.Abs(diff):F0} phút")})");
            }

            return reservations;
        }

        // ✅ MỚI: Tự động hủy đơn đặt bàn khi quá thời gian chờ
        public async Task<bool> AutoCancelExpiredReservationAsync(int maDat)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var datBan = await _context.DatBans
                    .Include(d => d.MaBanNavigation)
                    .FirstOrDefaultAsync(d => d.MaDat == maDat);

                if (datBan == null)
                    return false;

                // Cập nhật trạng thái đơn đặt
                datBan.TrangThai = "Đã hủy";
                datBan.GhiChu = (datBan.GhiChu ?? "") + " [Tự động hủy - Quá thời gian chờ]";

                // Cập nhật trạng thái bàn về trống
                var ban = datBan.MaBanNavigation;
                if (ban != null && ban.TrangThai == "Đã đặt")
                {
                    ban.TrangThai = "Trống";
                    ban.MaKh = null;
                    ban.GhiChu = null;

                    System.Diagnostics.Debug.WriteLine($"✓ Tự động hủy đơn đặt - Bàn {ban.TenBan} chuyển về 'Trống'");
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                System.Diagnostics.Debug.WriteLine($"❌ Lỗi tự động hủy: {ex.Message}");
                return false;
            }
        }

        // ✅ MỚI: Giữ đặt bàn (xác nhận khách sẽ đến)
        public async Task<bool> KeepReservationAsync(int maDat)
        {
            try
            {
                var datBan = await _context.DatBans
                    .Include(d => d.MaBanNavigation)
                    .FirstOrDefaultAsync(d => d.MaDat == maDat);

                if (datBan == null)
                    return false;

                // Gia hạn thời gian bắt đầu thêm 15 phút
                datBan.ThoiGianBatDau = DateTime.Now.AddMinutes(15);
                datBan.GhiChu = (datBan.GhiChu ?? "") + $" [Gia hạn lúc {DateTime.Now:HH:mm}]";

                await _context.SaveChangesAsync();

                System.Diagnostics.Debug.WriteLine($"✓ Giữ đơn đặt - Gia hạn thêm 15 phút");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Lỗi giữ đơn đặt: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// ✨ CẢI TIẾN: Lấy danh sách bàn khả dụng cho đặt bàn
        /// Bàn được coi là khả dụng nếu:
        /// 1. Trạng thái = "Trống"
        /// 2. KHÔNG có đơn đặt nào trùng khung giờ
        /// </summary>
        public async Task<List<BanBium>> GetAvailableTablesForReservationAsync(DateTime thoiGianBatDau, DateTime thoiGianKetThuc)
        {
            // Lấy tất cả các đơn đặt bàn đang hoạt động trong khung giờ
            var reservedTableIds = await _context.DatBans
                .Where(d => (d.TrangThai == "Đang chờ" || d.TrangThai == "Đã xác nhận" || d.TrangThai == "Đã đặt")
                    && d.ThoiGianKetThuc.HasValue
                    && d.ThoiGianBatDau < thoiGianKetThuc
                    && d.ThoiGianKetThuc.Value > thoiGianBatDau)
                .Select(d => d.MaBan)
                .Distinct()
                .ToListAsync();

            // Lấy các bàn trống và không có đơn đặt trùng giờ
            var availableTables = await _context.BanBia
                .Include(b => b.MaKhuVucNavigation)
                .Include(b => b.MaLoaiNavigation)
                .Where(b => b.TrangThai == "Trống"
                    && !reservedTableIds.Contains(b.MaBan))
                .OrderBy(b => b.TenBan)
                .ToListAsync();

            System.Diagnostics.Debug.WriteLine($"✅ Tìm thấy {availableTables.Count} bàn khả dụng từ {thoiGianBatDau:HH:mm} đến {thoiGianKetThuc:HH:mm}");

            return availableTables;
        }

        public async Task<bool> UpdateStatusAsync(int maDat, string trangThai)
        {
            try
            {
                var datBan = await _context.DatBans.FindAsync(maDat);
                if (datBan == null)
                    return false;

                datBan.TrangThai = trangThai;
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// ✨ CẢI TIẾN: Cập nhật đơn đặt bàn với kiểm tra trùng lặp
        /// </summary>
        public async Task<bool> UpdateReservationAsync(DatBan datBan)
        {
            try
            {
                var existing = await _context.DatBans.FindAsync(datBan.MaDat);
                if (existing == null)
                    return false;

                // Only perform overlap check if times changed
                bool startChanged = existing.ThoiGianBatDau != datBan.ThoiGianBatDau;
                bool endChanged = existing.ThoiGianKetThuc != datBan.ThoiGianKetThuc;

                if (startChanged || endChanged)
                {
                    // Ensure MaBan and ThoiGianBatDau are present
                    if (!datBan.MaBan.HasValue)
                    {
                        System.Diagnostics.Debug.WriteLine("❌ Cannot check overlap: MaBan is null");
                        return false;
                    }

                    if (!datBan.ThoiGianBatDau.HasValue)
                    {
                        System.Diagnostics.Debug.WriteLine("❌ Cannot check overlap: ThoiGianBatDau is null");
                        return false;
                    }

                    // Provide a default end time if missing (preserve previous behavior of using a fallback)
                    var newStart = datBan.ThoiGianBatDau.Value;
                    var newEnd = datBan.ThoiGianKetThuc ?? newStart.AddHours(2);

                    var isReserved = await IsTableReservedAsync(
                        datBan.MaBan.Value,
                        newStart,
                        newEnd,
                        datBan.MaDat // exclude itself
                    );

                    if (isReserved)
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ Không thể cập nhật - Bàn đã có đơn đặt khác trong khung giờ này!");
                        return false;
                    }
                }

                existing.TenKhach = datBan.TenKhach;
                existing.Sdt = datBan.Sdt;
                existing.ThoiGianDat = datBan.ThoiGianDat;
                existing.ThoiGianBatDau = datBan.ThoiGianBatDau;
                existing.ThoiGianKetThuc = datBan.ThoiGianKetThuc;
                existing.SoNguoi = datBan.SoNguoi;
                existing.GhiChu = datBan.GhiChu;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<(int dangCho, int daXacNhan, int daHoanThanh, int daHuy)> GetReservationStatsAsync()
        {
            var dangCho = await _context.DatBans.CountAsync(d => d.TrangThai == "Đang chờ");
            var daXacNhan = await _context.DatBans.CountAsync(d => d.TrangThai == "Đã xác nhận" || d.TrangThai == "Đã đặt");
            var daHoanThanh = await _context.DatBans.CountAsync(d => d.TrangThai == "Đã hoàn thành");
            var daHuy = await _context.DatBans.CountAsync(d => d.TrangThai == "Đã hủy");

            return (dangCho, daXacNhan, daHoanThanh, daHuy);
        }

        public async Task AddAsync(DatBan booking)
        {
            _context.DatBans.Add(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CancelBookingAsync(int maDat)
        {
            var booking = await _context.DatBans.FindAsync(maDat);
            if (booking == null) return false;

            if (booking.TrangThai == "Đang chờ" || booking.TrangThai == "Đã xác nhận" || booking.TrangThai == "Đã đặt")
            {
                booking.TrangThai = "Đã hủy";
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<DatBan> GetNextReservationForTableAsync(int maBan)
        {
            try
            {
                var now = DateTime.Now;

                System.Diagnostics.Debug.WriteLine($"\n=== TÌM ĐƠN ĐẶT TIẾP THEO ===");
                System.Diagnostics.Debug.WriteLine($"Bàn: {maBan}");
                System.Diagnostics.Debug.WriteLine($"Thời gian hiện tại: {now:HH:mm:ss dd/MM/yyyy}");

                // Lấy đơn đặt tiếp theo có thời gian bắt đầu >= hiện tại
                // Chỉ lấy đơn ở trạng thái "Đang chờ" hoặc "Đã đặt"
                var nextReservation = await _context.DatBans
                    .Include(d => d.MaBanNavigation)
                    .Include(d => d.MaKhNavigation)
                    .Where(d => d.MaBan == maBan
                        && (d.TrangThai == "Đang chờ" || d.TrangThai == "Đã đặt")
                        && d.ThoiGianBatDau.HasValue
                        && d.ThoiGianBatDau.Value >= now)
                    .OrderBy(d => d.ThoiGianBatDau)
                    .FirstOrDefaultAsync();

                if (nextReservation != null)
                {
                    System.Diagnostics.Debug.WriteLine($"✓ Tìm thấy đơn đặt tiếp theo:");
                    System.Diagnostics.Debug.WriteLine($"  - Mã đơn: {nextReservation.MaDat}");
                    System.Diagnostics.Debug.WriteLine($"  - Khách: {nextReservation.TenKhach}");
                    System.Diagnostics.Debug.WriteLine($"  - Thời gian: {nextReservation.ThoiGianBatDau:HH:mm} - {nextReservation.ThoiGianKetThuc:HH:mm}");
                    System.Diagnostics.Debug.WriteLine($"  - Trạng thái: {nextReservation.TrangThai}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"ℹ Không có đơn đặt tiếp theo cho bàn {maBan}");
                }

                return nextReservation;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Lỗi khi tìm đơn đặt tiếp theo: {ex.Message}");
                return null;
            }
        }
    }
}