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
                    .ThenInclude(b => b!.MaKhuVucNavigation)
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b!.MaLoaiNavigation)
                .Include(d => d.MaKhNavigation)
                .Where(d => d.TrangThai == "Đang chờ" || d.TrangThai == "Đã xác nhận" || d.TrangThai == "Đã đặt")
                .OrderBy(d => d.ThoiGianBatDau)
                .ToListAsync();
        }

        public async Task<DatBan?> GetByIdAsync(int maDat)
        {
            return await _context.DatBans
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b!.MaKhuVucNavigation)
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b!.MaLoaiNavigation)
                .Include(d => d.MaKhNavigation)
                .FirstOrDefaultAsync(d => d.MaDat == maDat);
        }

        public async Task<List<DatBan>> GetByTableAsync(int maBan)
        {
            return await _context.DatBans
                .Include(d => d.MaKhNavigation)
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b!.MaKhuVucNavigation)
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b!.MaLoaiNavigation)
                .Where(d => d.MaBan == maBan &&
                    (d.TrangThai == "Đang chờ" || d.TrangThai == "Đã xác nhận" || d.TrangThai == "Đã đặt"))
                .OrderBy(d => d.ThoiGianDat)
                .ToListAsync();
        }

        public async Task<List<DatBan>> GetByCustomerAsync(int maKh)
        {
            return await _context.DatBans
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b!.MaKhuVucNavigation)
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b!.MaLoaiNavigation)
                .Where(d => d.MaKh == maKh && d.TrangThai == "Đang chờ")
                .OrderBy(d => d.ThoiGianDat)
                .ToListAsync();
        }

        public async Task<List<DatBan>> GetByDateRangeAsync(DateTime tuNgay, DateTime denNgay)
        {
            return await _context.DatBans
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b!.MaKhuVucNavigation)
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b!.MaLoaiNavigation)
                .Include(d => d.MaKhNavigation)
                .Where(d => d.ThoiGianDat >= tuNgay && d.ThoiGianDat <= denNgay)
                .OrderBy(d => d.ThoiGianDat)
                .ToListAsync();
        }

        /// <summary>
        /// ✨ CẢI TIẾN: Kiểm tra xem bàn có bị trùng lịch trong khoảng thời gian không
        /// </summary>
        public async Task<bool> IsTableReservedAsync(int maBan, DateTime thoiGianBatDau, DateTime thoiGianKetThuc, int? excludeMaDat = null)
        {
            var existingReservations = await _context.DatBans
                .Where(d => d.MaBan == maBan
                    && (d.TrangThai == "Đang chờ" || d.TrangThai == "Đã xác nhận" || d.TrangThai == "Đã đặt")
                    && d.ThoiGianKetThuc.HasValue)
                .ToListAsync();

            if (excludeMaDat.HasValue)
            {
                existingReservations = existingReservations.Where(d => d.MaDat != excludeMaDat.Value).ToList();
            }

            foreach (var reservation in existingReservations)
            {
                var existingStart = reservation.ThoiGianBatDau ?? DateTime.MinValue;
                var existingEnd = reservation.ThoiGianKetThuc!.Value;

                bool isTrungLap = thoiGianBatDau < existingEnd && thoiGianKetThuc > existingStart;

                if (isTrungLap)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ TRÙNG LẶP PHÁT HIỆN:");
                    System.Diagnostics.Debug.WriteLine($"   Đơn đặt hiện tại: {existingStart:HH:mm dd/MM} - {existingEnd:HH:mm dd/MM}");
                    System.Diagnostics.Debug.WriteLine($"   Đơn đặt mới: {thoiGianBatDau:HH:mm dd/MM} - {thoiGianKetThuc:HH:mm dd/MM}");
                    return true;
                }
            }

            System.Diagnostics.Debug.WriteLine($"✅ KHÔNG TRÙNG LẶP - Bàn {maBan} có thể đặt từ {thoiGianBatDau:HH:mm} đến {thoiGianKetThuc:HH:mm}");
            return false;
        }

        /// <summary>
        /// ✨ MỚI: Lấy danh sách các khung giờ đã đặt của một bàn
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

        public async Task<KhachHang?> GetCustomerByPhoneNumberAsync(string sdt)
        {
            return await _context.KhachHangs
                .FirstOrDefaultAsync(k => k.Sdt == sdt);
        }

        /// <summary>
        /// ✅ LOGIC ĐÚNG: Khi khách đặt bàn → Bàn VẪN TRỐNG, chờ xác nhận
        /// </summary>
        public async Task<bool> ReserveTableAsync(
            int maBan,
            int? maKhachHang,
            string tenKhach,
            string sdt,
            DateTime thoiGianBatDau,
            DateTime thoiGianKetThuc,
            string? ghiChu)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // ✅ KIỂM TRA TRÙNG LẶP TRƯỚC KHI ĐẶT
                var isReserved = await IsTableReservedAsync(maBan, thoiGianBatDau, thoiGianKetThuc);
                if (isReserved)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ KHÔNG THỂ ĐẶT - Bàn đã có lịch đặt trùng");
                    return false;
                }

                // Tìm hoặc tạo khách hàng
                var khachHang = await _context.KhachHangs.FirstOrDefaultAsync(k => k.Sdt == sdt);
                if (khachHang == null)
                {
                    khachHang = new KhachHang
                    {
                        TenKh = tenKhach,
                        Sdt = sdt
                        // ❌ KHÔNG CÓ NgayTao và TrangThai trong entity - đã xóa
                    };
                    _context.KhachHangs.Add(khachHang);
                    await _context.SaveChangesAsync();
                }

                // Tạo đơn đặt bàn
                var datBan = new DatBan
                {
                    MaBan = maBan,
                    MaKh = khachHang.MaKh,
                    TenKhach = tenKhach,
                    Sdt = sdt,
                    ThoiGianDat = DateTime.Now,
                    ThoiGianBatDau = thoiGianBatDau,
                    ThoiGianKetThuc = thoiGianKetThuc,
                    GhiChu = ghiChu,
                    TrangThai = "Đang chờ" // ✅ MẶC ĐỊNH: ĐANG CHỜ XÁC NHẬN
                };

                _context.DatBans.Add(datBan);
                await _context.SaveChangesAsync();

                System.Diagnostics.Debug.WriteLine($"✅ ĐẶT BÀN THÀNH CÔNG - Bàn vẫn TRỐNG, chờ xác nhận");

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                System.Diagnostics.Debug.WriteLine($"❌ LỖI ĐẶT BÀN: {ex.Message}");
                return false;
            }
        }

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

                // ✅ XÁC NHẬN ĐƠN ĐẶT
                datBan.TrangThai = "Đã xác nhận";

                // ✅ CHUYỂN BÀN SANG "ĐÃ ĐẶT"
                if (datBan.MaBanNavigation != null)
                {
                    datBan.MaBanNavigation.TrangThai = "Đã đặt";
                    System.Diagnostics.Debug.WriteLine($"✅ Đã chuyển bàn {datBan.MaBanNavigation.TenBan} sang trạng thái ĐÃ ĐẶT");
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                System.Diagnostics.Debug.WriteLine($"❌ LỖI XÁC NHẬN: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CancelReservationAsync(int maDat)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var datBan = await _context.DatBans
                    .Include(d => d.MaBanNavigation)
                    .FirstOrDefaultAsync(d => d.MaDat == maDat);

                if (datBan == null)
                    return false;

                // ✅ HỦY ĐƠN ĐẶT
                datBan.TrangThai = "Đã hủy";

                // ✅ NẾU BÀN ĐANG Ở TRẠNG THÁI "ĐÃ ĐẶT" → CHUYỂN VỀ "TRỐNG"
                if (datBan.MaBanNavigation != null && datBan.MaBanNavigation.TrangThai == "Đã đặt")
                {
                    datBan.MaBanNavigation.TrangThai = "Trống";
                    System.Diagnostics.Debug.WriteLine($"✅ Đã chuyển bàn {datBan.MaBanNavigation.TenBan} về trạng thái TRỐNG");
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                System.Diagnostics.Debug.WriteLine($"❌ LỖI HỦY ĐƠN: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// ✨ MỚI: Lấy danh sách đơn đặt sắp đến giờ hoặc quá giờ
        /// </summary>
        public async Task<List<DatBan>> GetReservationsNearStartTimeAsync()
        {
            var now = DateTime.Now;
            var fifteenMinutesAgo = now.AddMinutes(-15);
            var tenMinutesLater = now.AddMinutes(10);

            System.Diagnostics.Debug.WriteLine($"🔍 Tìm đơn đặt từ {fifteenMinutesAgo:HH:mm} đến {tenMinutesLater:HH:mm}");

            var reservations = await _context.DatBans
                .Include(d => d.MaBanNavigation)
                    .ThenInclude(b => b!.MaKhuVucNavigation)
                .Include(d => d.MaKhNavigation)
                .Where(d => (d.TrangThai == "Đang chờ" || d.TrangThai == "Đã xác nhận")
                    && d.ThoiGianBatDau.HasValue
                    && d.ThoiGianBatDau.Value >= fifteenMinutesAgo
                    && d.ThoiGianBatDau.Value <= tenMinutesLater)
                .OrderBy(d => d.ThoiGianBatDau)
                .ToListAsync();

            System.Diagnostics.Debug.WriteLine($"   Tìm thấy {reservations.Count} đơn đặt");

            foreach (var r in reservations)
            {
                if (r.ThoiGianBatDau.HasValue)
                {
                    var diff = (r.ThoiGianBatDau.Value - now).TotalMinutes;
                    System.Diagnostics.Debug.WriteLine(
                        $"   - Đơn #{r.MaDat}: {r.MaBanNavigation?.TenBan} " +
                        $"lúc {r.ThoiGianBatDau:HH:mm} " +
                        $"({(diff >= 0 ? $"còn {diff:F0} phút" : $"quá {Math.Abs(diff):F0} phút")})");
                }
            }

            return reservations;
        }

        /// <summary>
        /// ✅ MỚI: Tự động hủy đơn đặt bàn khi quá thời gian chờ
        /// </summary>
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

        /// <summary>
        /// ✅ MỚI: Giữ đặt bàn (xác nhận khách sẽ đến)
        /// </summary>
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
        /// ✨ CẢI TIẾN QUAN TRỌNG: Lấy danh sách bàn khả dụng cho đặt bàn
        /// KHÔNG CHỈ LẤY BÀN TRỐNG mà còn kiểm tra xem bàn có đơn đặt trùng giờ hay không
        /// </summary>
        public async Task<List<BanBium>> GetAvailableTablesForReservationAsync(DateTime thoiGianBatDau, DateTime thoiGianKetThuc)
        {
            System.Diagnostics.Debug.WriteLine($"\n╔═══════════════════════════════════════════════════════════╗");
            System.Diagnostics.Debug.WriteLine($"║  TÌM BÀN KHẢ DỤNG CHO ĐẶT BÀN");
            System.Diagnostics.Debug.WriteLine($"╠═══════════════════════════════════════════════════════════╣");
            System.Diagnostics.Debug.WriteLine($"║  Khung giờ: {thoiGianBatDau:HH:mm dd/MM} → {thoiGianKetThuc:HH:mm dd/MM}");
            System.Diagnostics.Debug.WriteLine($"╚═══════════════════════════════════════════════════════════╝\n");

            // ═══════════════════════════════════════════════════════════
            // BƯỚC 1: LẤY TẤT CẢ CÁC BÀN (không lọc trạng thái)
            // ═══════════════════════════════════════════════════════════
            var allTables = await _context.BanBia
                .Include(b => b.MaKhuVucNavigation)
                .Include(b => b.MaLoaiNavigation)
                .ToListAsync();

            System.Diagnostics.Debug.WriteLine($"📊 Tổng số bàn trong hệ thống: {allTables.Count}");

            // ═══════════════════════════════════════════════════════════
            // BƯỚC 2: LẤY TẤT CẢ CÁC ĐƠN ĐẶT ĐANG HOẠT ĐỘNG
            // ═══════════════════════════════════════════════════════════
            var activeReservations = await _context.DatBans
                .Where(d => (d.TrangThai == "Đang chờ" || d.TrangThai == "Đã xác nhận" || d.TrangThai == "Đã đặt")
                    && d.ThoiGianKetThuc.HasValue)
                .ToListAsync();

            System.Diagnostics.Debug.WriteLine($"📋 Tổng số đơn đặt đang hoạt động: {activeReservations.Count}");

            // ═══════════════════════════════════════════════════════════
            // BƯỚC 3: LỌC BÀN - CHỈ GIỮ LẠI NHỮNG BÀN KHÔNG TRÙNG GIỜ
            // ═══════════════════════════════════════════════════════════
            var availableTables = new List<BanBium>();

            foreach (var table in allTables)
            {
                // Lấy tất cả đơn đặt của bàn này
                var tableReservations = activeReservations
                    .Where(r => r.MaBan == table.MaBan)
                    .ToList();

                System.Diagnostics.Debug.WriteLine($"\n🔍 Kiểm tra bàn: {table.TenBan} (ID: {table.MaBan})");
                System.Diagnostics.Debug.WriteLine($"   Trạng thái hiện tại: {table.TrangThai}");
                System.Diagnostics.Debug.WriteLine($"   Số đơn đặt: {tableReservations.Count}");

                // Kiểm tra xem có đơn đặt nào trùng với khung giờ cần đặt không
                bool hasConflict = false;

                foreach (var reservation in tableReservations)
                {
                    var existingStart = reservation.ThoiGianBatDau ?? DateTime.MinValue;
                    var existingEnd = reservation.ThoiGianKetThuc!.Value;

                    // ✅ LOGIC KIỂM TRA TRÙNG LẶP
                    bool isOverlap = thoiGianBatDau < existingEnd && thoiGianKetThuc > existingStart;

                    System.Diagnostics.Debug.WriteLine($"   • Đơn #{reservation.MaDat}: {existingStart:HH:mm} - {existingEnd:HH:mm}");
                    System.Diagnostics.Debug.WriteLine($"     Trạng thái: {reservation.TrangThai}");
                    System.Diagnostics.Debug.WriteLine($"     Trùng lặp? {(isOverlap ? "❌ CÓ" : "✅ KHÔNG")}");

                    if (isOverlap)
                    {
                        hasConflict = true;
                        break;
                    }
                }

                // ═══════════════════════════════════════════════════════
                // ĐIỀU KIỆN LỌC:
                // 1. Nếu bàn đang "Đang chơi" → KHÔNG THỂ ĐẶT
                // 2. Nếu có đơn đặt trùng giờ → KHÔNG THỂ ĐẶT
                // 3. Còn lại → CÓ THỂ ĐẶT
                // ═══════════════════════════════════════════════════════
                if (table.TrangThai == "Đang chơi")
                {
                    System.Diagnostics.Debug.WriteLine($"   ❌ LOẠI BỎ: Bàn đang có khách chơi");
                    continue;
                }

                if (hasConflict)
                {
                    System.Diagnostics.Debug.WriteLine($"   ❌ LOẠI BỎ: Có đơn đặt trùng giờ");
                    continue;
                }

                System.Diagnostics.Debug.WriteLine($"   ✅ KHẢ DỤNG: Bàn có thể đặt");
                availableTables.Add(table);
            }

            // ═══════════════════════════════════════════════════════════
            // KẾT QUẢ
            // ═══════════════════════════════════════════════════════════
            System.Diagnostics.Debug.WriteLine($"\n╔═══════════════════════════════════════════════════════════╗");
            System.Diagnostics.Debug.WriteLine($"║  KẾT QUẢ TÌM KIẾM");
            System.Diagnostics.Debug.WriteLine($"╠═══════════════════════════════════════════════════════════╣");
            System.Diagnostics.Debug.WriteLine($"║  ✅ Số bàn khả dụng: {availableTables.Count}/{allTables.Count}");
            System.Diagnostics.Debug.WriteLine($"╚═══════════════════════════════════════════════════════════╝\n");

            return availableTables.OrderBy(b => b.TenBan).ToList();
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

                bool startChanged = existing.ThoiGianBatDau != datBan.ThoiGianBatDau;
                bool endChanged = existing.ThoiGianKetThuc != datBan.ThoiGianKetThuc;

                if (startChanged || endChanged)
                {
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

                    var newStart = datBan.ThoiGianBatDau.Value;
                    var newEnd = datBan.ThoiGianKetThuc ?? newStart.AddHours(2);

                    var isReserved = await IsTableReservedAsync(
                        datBan.MaBan.Value,
                        newStart,
                        newEnd,
                        datBan.MaDat
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

        public async Task<DatBan?> GetNextReservationForTableAsync(int maBan)
        {
            try
            {
                var now = DateTime.Now;

                System.Diagnostics.Debug.WriteLine($"\n=== TÌM ĐƠN ĐẶT TIẾP THEO ===");
                System.Diagnostics.Debug.WriteLine($"Bàn: {maBan}");
                System.Diagnostics.Debug.WriteLine($"Thời gian hiện tại: {now:HH:mm:ss dd/MM/yyyy}");

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