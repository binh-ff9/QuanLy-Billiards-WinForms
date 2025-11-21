using Billiard.DAL.Data;
using Billiard.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Billiard.BLL.Services.QLBan
{
    public class BanBiaService
    {
        private readonly BilliardDbContext _context;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        public BanBiaService(BilliardDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả bàn
        public async Task<List<BanBium>> GetAllTablesAsync()
        {
            return await _context.BanBia
                .Include(b => b.MaKhuVucNavigation)
                .Include(b => b.MaLoaiNavigation)
                .Include(b => b.MaKhNavigation)
                .Where(b => b.TrangThai != "Bảo trì")
                .OrderBy(b => b.TenBan)
                .ToListAsync();
        }

        // Lọc bàn theo điều kiện
        public async Task<List<BanBium>> FilterTablesAsync(string areaFilter, string statusFilter, string typeFilter, string searchText)
        {
            var query = _context.BanBia
                .Include(b => b.MaKhuVucNavigation)
                .Include(b => b.MaLoaiNavigation)
                .Include(b => b.MaKhNavigation)
                .Where(b => b.TrangThai != "Bảo trì")
                .AsQueryable();

            if (!string.IsNullOrEmpty(areaFilter) && areaFilter != "all")
                query = query.Where(b => b.MaKhuVucNavigation.TenKhuVuc == areaFilter);

            if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "all")
                query = query.Where(b => b.TrangThai == statusFilter);

            if (!string.IsNullOrEmpty(typeFilter) && typeFilter != "all")
                query = query.Where(b => b.MaLoaiNavigation.TenLoai == typeFilter);

            if (!string.IsNullOrEmpty(searchText))
            {
                searchText = searchText.ToLower();
                query = query.Where(b => b.TenBan.ToLower().Contains(searchText));
            }

            return await query.OrderBy(b => b.TenBan).ToListAsync();
        }

        // Bắt đầu chơi bàn
        public async Task<bool> StartTableAsync(int maBan, int maNv, int? maKh = null)
        {
            await _semaphore.WaitAsync();
            try
            {
                // Sử dụng Execution Strategy
                var strategy = _context.Database.CreateExecutionStrategy();

                return await strategy.ExecuteAsync(async () =>
                {
                    using var transaction = await _context.Database.BeginTransactionAsync();
                    try
                    {
                        var ban = await _context.BanBia
                            .Include(b => b.MaKhuVucNavigation)
                            .Include(b => b.MaLoaiNavigation)
                            .FirstOrDefaultAsync(b => b.MaBan == maBan);

                        if (ban == null)
                        {
                            System.Diagnostics.Debug.WriteLine($"❌ Không tìm thấy bàn {maBan}");
                            return false;
                        }

                        // Cho phép bắt đầu cả bàn "Trống" VÀ "Đã đặt"
                        if (ban.TrangThai != "Trống" && ban.TrangThai != "Đã đặt")
                        {
                            System.Diagnostics.Debug.WriteLine($"❌ Bàn {ban.TenBan} có trạng thái: {ban.TrangThai}");
                            return false;
                        }

                        // Kiểm tra hóa đơn đang hoạt động
                        var existingInvoice = await _context.HoaDons
                            .FirstOrDefaultAsync(h => h.MaBan == maBan && h.TrangThai == "Đang chơi");

                        if (existingInvoice != null)
                        {
                            System.Diagnostics.Debug.WriteLine($"❌ Bàn {ban.TenBan} đã có hóa đơn: HD{existingInvoice.MaHd}");
                            return false;
                        }

                        // Nếu là bàn "Đã đặt", xử lý đơn đặt bàn
                        if (ban.TrangThai == "Đã đặt")
                        {
                            var datBan = await _context.DatBans
                                .Where(d => d.MaBan == maBan && (d.TrangThai == "Đang chờ" || d.TrangThai == "Đã đặt"))
                                .OrderBy(d => d.ThoiGianBatDau)
                                .FirstOrDefaultAsync();

                            if (datBan != null)
                            {
                                if (!maKh.HasValue && datBan.MaKh.HasValue)
                                {
                                    maKh = datBan.MaKh;
                                }
                                datBan.TrangThai = "Đã xác nhận";
                            }
                        }

                        // Cập nhật trạng thái bàn
                        ban.TrangThai = "Đang chơi";
                        ban.GioBatDau = DateTime.Now;
                        ban.MaKh = maKh;

                        // Tạo hóa đơn mới
                        var hoaDon = new HoaDon
                        {
                            MaBan = maBan,
                            MaNv = maNv,
                            MaKh = maKh,
                            ThoiGianBatDau = DateTime.Now,
                            TrangThai = "Đang chơi",
                            TienBan = 0,
                            TienDichVu = 0,
                            GiamGia = 0,
                            TongTien = 0
                        };

                        _context.HoaDons.Add(hoaDon);

                        System.Diagnostics.Debug.WriteLine($"✓ Bắt đầu bàn {ban.TenBan}");

                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        return true;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        System.Diagnostics.Debug.WriteLine($"❌ Inner Exception: {ex.Message}");
                        throw;
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ StartTableAsync Exception: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Inner: {ex.InnerException?.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack: {ex.StackTrace}");
                return false;
            }
        }

        // Tạm dừng/Hủy bàn
        public async Task<bool> PauseTableAsync(int maBan)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var ban = await _context.BanBia.FindAsync(maBan);
                if (ban == null || ban.TrangThai != "Đang chơi")
                    return false;

                ban.TrangThai = "Trống";
                ban.GioBatDau = null;
                ban.MaKh = null;

                // Update hóa đơn
                var hoaDon = await _context.HoaDons
                    .Where(h => h.MaBan == maBan && h.TrangThai == "Đang chơi")
                    .FirstOrDefaultAsync();

                if (hoaDon != null)
                {
                    hoaDon.TrangThai = "Đã hủy";
                    hoaDon.ThoiGianKetThuc = DateTime.Now;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        // Thêm bàn mới
        public async Task<bool> AddTableAsync(BanBium ban)
        {
            System.Diagnostics.Debug.WriteLine("\n=== BanBiaService.AddTableAsync ===");

            try
            {
                System.Diagnostics.Debug.WriteLine("1. Kiểm tra dữ liệu đầu vào...");

                if (ban == null)
                {
                    System.Diagnostics.Debug.WriteLine("❌ Đối tượng ban = null");
                    return false;
                }

                System.Diagnostics.Debug.WriteLine($"Thông tin bàn nhận được:");
                System.Diagnostics.Debug.WriteLine($"  - TenBan: {ban.TenBan}");
                System.Diagnostics.Debug.WriteLine($"  - MaLoai: {ban.MaLoai}");
                System.Diagnostics.Debug.WriteLine($"  - MaKhuVuc: {ban.MaKhuVuc}");
                System.Diagnostics.Debug.WriteLine($"  - TrangThai: {ban.TrangThai}");
                System.Diagnostics.Debug.WriteLine($"  - GhiChu: {ban.GhiChu ?? "(null)"}");
                System.Diagnostics.Debug.WriteLine($"  - HinhAnh: {ban.HinhAnh ?? "(null)"}");
                System.Diagnostics.Debug.WriteLine($"  - ViTriX: {ban.ViTriX}");
                System.Diagnostics.Debug.WriteLine($"  - ViTriY: {ban.ViTriY}");

                // Kiểm tra tên bàn đã tồn tại chưa
                System.Diagnostics.Debug.WriteLine("2. Kiểm tra tên bàn đã tồn tại...");
                var existingTable = await _context.BanBia
                    .FirstOrDefaultAsync(b => b.TenBan == ban.TenBan);

                if (existingTable != null)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ Tên bàn '{ban.TenBan}' đã tồn tại (MaBan: {existingTable.MaBan})");
                    return false;
                }
                System.Diagnostics.Debug.WriteLine("✓ Tên bàn chưa tồn tại");

                // Kiểm tra MaLoai có tồn tại không
                System.Diagnostics.Debug.WriteLine("3. Kiểm tra MaLoai...");
                var loaiBan = await _context.LoaiBans.FindAsync(ban.MaLoai);
                if (loaiBan == null)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ MaLoai {ban.MaLoai} không tồn tại trong database");
                    return false;
                }
                System.Diagnostics.Debug.WriteLine($"✓ MaLoai hợp lệ: {loaiBan.TenLoai}");

                // Kiểm tra MaKhuVuc có tồn tại không
                System.Diagnostics.Debug.WriteLine("4. Kiểm tra MaKhuVuc...");
                var khuVuc = await _context.KhuVucs.FindAsync(ban.MaKhuVuc);
                if (khuVuc == null)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ MaKhuVuc {ban.MaKhuVuc} không tồn tại trong database");
                    return false;
                }
                System.Diagnostics.Debug.WriteLine($"✓ MaKhuVuc hợp lệ: {khuVuc.TenKhuVuc}");

                // Set giá trị mặc định
                System.Diagnostics.Debug.WriteLine("5. Set giá trị mặc định...");
                ban.NgayTao = DateTime.Now;
                ban.TrangThai = ban.TrangThai ?? "Trống";
                System.Diagnostics.Debug.WriteLine($"  - NgayTao: {ban.NgayTao}");
                System.Diagnostics.Debug.WriteLine($"  - TrangThai: {ban.TrangThai}");

                System.Diagnostics.Debug.WriteLine("6. Thêm vào DbContext...");
                _context.BanBia.Add(ban);
                System.Diagnostics.Debug.WriteLine("✓ Đã Add vào DbContext");

                System.Diagnostics.Debug.WriteLine("7. Gọi SaveChangesAsync...");
                var savedCount = await _context.SaveChangesAsync();
                System.Diagnostics.Debug.WriteLine($"✓ SaveChanges hoàn tất. Số bản ghi đã lưu: {savedCount}");

                if (savedCount > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"✓✓✓ THÀNH CÔNG! MaBan mới: {ban.MaBan}");
                    return true;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("⚠ SaveChanges không lưu bản ghi nào");
                    return false;
                }
            }
            catch (DbUpdateException dbEx)
            {
                System.Diagnostics.Debug.WriteLine($"\n❌ DbUpdateException:");
                System.Diagnostics.Debug.WriteLine($"Message: {dbEx.Message}");

                if (dbEx.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner Exception: {dbEx.InnerException.Message}");

                    if (dbEx.InnerException.InnerException != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Inner Inner Exception: {dbEx.InnerException.InnerException.Message}");
                    }
                }

                System.Diagnostics.Debug.WriteLine($"Stack Trace:\n{dbEx.StackTrace}");
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"\n❌ Exception:");
                System.Diagnostics.Debug.WriteLine($"Type: {ex.GetType().Name}");
                System.Diagnostics.Debug.WriteLine($"Message: {ex.Message}");

                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }

                System.Diagnostics.Debug.WriteLine($"Stack Trace:\n{ex.StackTrace}");
                return false;
            }
        }

        // Cập nhật bàn
        public async Task<bool> UpdateTableAsync(BanBium ban)
        {
            try
            {
                var existingBan = await _context.BanBia.FindAsync(ban.MaBan);
                if (existingBan == null)
                    return false;

                // Update các thuộc tính (trừ NgayTao)
                existingBan.TenBan = ban.TenBan;
                existingBan.MaLoai = ban.MaLoai;
                existingBan.MaKhuVuc = ban.MaKhuVuc;
                existingBan.TrangThai = ban.TrangThai;
                existingBan.ViTriX = ban.ViTriX;
                existingBan.ViTriY = ban.ViTriY;
                existingBan.GhiChu = ban.GhiChu;
                existingBan.HinhAnh = ban.HinhAnh;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Xóa bàn
        public async Task<bool> DeleteTableAsync(int maBan)
        {
            try
            {
                var ban = await _context.BanBia.FindAsync(maBan);
                if (ban == null || ban.TrangThai != "Trống")
                    return false;

                // Check if table has history
                var hasHistory = await _context.HoaDons.AnyAsync(h => h.MaBan == maBan);
                if (hasHistory)
                {
                    // Soft delete - mark as maintenance
                    ban.TrangThai = "Bảo trì";
                }
                else
                {
                    // Hard delete
                    _context.BanBia.Remove(ban);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Lấy thông tin bàn theo ID
        public async Task<BanBium> GetTableByIdAsync(int maBan)
        {
            return await _context.BanBia
                .Include(b => b.MaKhuVucNavigation)
                .Include(b => b.MaLoaiNavigation)
                .Include(b => b.MaKhNavigation)
                .FirstOrDefaultAsync(b => b.MaBan == maBan);
        }

        // Lấy hóa đơn đang hoạt động của bàn
        public async Task<HoaDon> GetActiveInvoiceAsync(int maBan)
        {
            return await _context.HoaDons
                .Include(h => h.MaKhNavigation)
                .Include(h => h.MaBanNavigation)
                .ThenInclude(b => b.MaLoaiNavigation)
                .Include(h => h.MaNvNavigation)
                .Where(h => h.MaBan == maBan && h.TrangThai == "Đang chơi")
                .FirstOrDefaultAsync();
        }

        // Lấy chi tiết hóa đơn
        public async Task<List<ChiTietHoaDon>> GetInvoiceDetailsAsync(int maHd)
        {
            return await _context.ChiTietHoaDons
                .Include(ct => ct.MaDvNavigation)
                .Where(ct => ct.MaHd == maHd)
                .ToListAsync();
        }

        // Thống kê bàn
        public async Task<(int trong, int dangChoi, int daDat)> GetTableStatsAsync()
        {
            var trong = await _context.BanBia.CountAsync(b => b.TrangThai == "Trống");
            var dangChoi = await _context.BanBia.CountAsync(b => b.TrangThai == "Đang chơi");
            var daDat = await _context.BanBia.CountAsync(b => b.TrangThai == "Đã đặt");

            return (trong, dangChoi, daDat);
        }

        // Đặt bàn
        public async Task<bool> ReserveTableAsync(int maBan, int? maKh, string tenKhach, string sdt, DateTime thoiGianDat, int? soNguoi, string ghiChu)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var ban = await _context.BanBia.FindAsync(maBan);
                if (ban == null || ban.TrangThai != "Trống")
                    return false;

                ban.TrangThai = "Đã đặt";
                ban.MaKh = maKh;
                ban.GhiChu = ghiChu;

                var datBan = new DatBan
                {
                    MaBan = maBan,
                    MaKh = maKh,
                    TenKhach = tenKhach,
                    Sdt = sdt,
                    ThoiGianDat = thoiGianDat,
                    SoNguoi = soNguoi,
                    GhiChu = ghiChu,
                    TrangThai = "Đang chờ",
                    NgayTao = DateTime.Now
                };

                _context.DatBans.Add(datBan);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        // Hủy đặt bàn
        public async Task<bool> CancelReservationAsync(int maDat)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    System.Diagnostics.Debug.WriteLine($"\n=== CancelReservationAsync ===");
                    System.Diagnostics.Debug.WriteLine($"MaDat: {maDat}");

                    var datBan = await _context.DatBans
                        .Include(d => d.MaBanNavigation)
                        .FirstOrDefaultAsync(d => d.MaDat == maDat);

                    if (datBan == null)
                    {
                        System.Diagnostics.Debug.WriteLine("❌ Không tìm thấy đơn đặt bàn");
                        return false;
                    }

                    System.Diagnostics.Debug.WriteLine($"Đơn đặt: {datBan.TenKhach}");

                    // Cập nhật trạng thái bàn về Trống
                    if (datBan.MaBanNavigation != null)
                    {
                        var ban = datBan.MaBanNavigation;
                        System.Diagnostics.Debug.WriteLine($"Bàn: {ban.TenBan} - Trạng thái cũ: {ban.TrangThai}");

                        ban.TrangThai = "Trống";
                        ban.MaKh = null;
                        ban.GhiChu = null;
                        ban.GioBatDau = null;

                        System.Diagnostics.Debug.WriteLine($"✓ Cập nhật bàn -> Trống");
                    }

                    // Cập nhật trạng thái đơn đặt thành "Đã hủy"
                    datBan.TrangThai = "Đã hủy";
                    System.Diagnostics.Debug.WriteLine($"✓ Cập nhật đơn đặt -> Đã hủy");

                    await _context.SaveChangesAsync();
                    System.Diagnostics.Debug.WriteLine("✓ SaveChanges thành công");

                    await transaction.CommitAsync();
                    System.Diagnostics.Debug.WriteLine("✓✓✓ HOÀN TẤT HỦY ĐẶT BÀN");

                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    System.Diagnostics.Debug.WriteLine($"❌ Exception: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Inner: {ex.InnerException?.Message}");
                    throw;
                }
            });
        }

        // Xác nhận đặt bàn và bắt đầu chơi
        public async Task<bool> ConfirmReservationAsync(int maDat, int maNv)
        {
            // Sử dụng Execution Strategy để tránh lỗi transaction
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    System.Diagnostics.Debug.WriteLine($"\n=== ConfirmReservationAsync ===");
                    System.Diagnostics.Debug.WriteLine($"MaDat: {maDat}, MaNV: {maNv}");

                    // 1. Tìm đơn đặt bàn
                    var datBan = await _context.DatBans
                        .Include(d => d.MaBanNavigation)
                        .FirstOrDefaultAsync(d => d.MaDat == maDat);

                    if (datBan == null)
                    {
                        System.Diagnostics.Debug.WriteLine("❌ Không tìm thấy đơn đặt bàn");
                        return false;
                    }

                    if (datBan.MaBanNavigation == null)
                    {
                        System.Diagnostics.Debug.WriteLine("❌ Không tìm thấy thông tin bàn");
                        return false;
                    }

                    System.Diagnostics.Debug.WriteLine($"Đơn đặt: {datBan.TenKhach} - Bàn: {datBan.MaBanNavigation.TenBan}");
                    System.Diagnostics.Debug.WriteLine($"Trạng thái đơn: {datBan.TrangThai}");
                    System.Diagnostics.Debug.WriteLine($"Trạng thái bàn: {datBan.MaBanNavigation.TrangThai}");

                    // 2. Kiểm tra trạng thái đơn đặt
                    if (datBan.TrangThai != "Đang chờ" && datBan.TrangThai != "Đã đặt")
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ Trạng thái đơn không hợp lệ: {datBan.TrangThai}");
                        return false;
                    }

                    // 3. Kiểm tra trạng thái bàn
                    var ban = datBan.MaBanNavigation;
                    if (ban.TrangThai != "Đã đặt" && ban.TrangThai != "Trống")
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ Bàn có trạng thái không hợp lệ: {ban.TrangThai}");
                        return false;
                    }

                    // 4. Kiểm tra xem bàn có hóa đơn đang hoạt động không
                    var existingInvoice = await _context.HoaDons
                        .FirstOrDefaultAsync(h => h.MaBan == ban.MaBan && h.TrangThai == "Đang chơi");

                    if (existingInvoice != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ Bàn đã có hóa đơn đang hoạt động: HD{existingInvoice.MaHd}");
                        return false;
                    }

                    // 5. Cập nhật trạng thái bàn
                    ban.TrangThai = "Đang chơi";
                    ban.GioBatDau = DateTime.Now;
                    ban.MaKh = datBan.MaKh; // Gán khách hàng từ đơn đặt
                    System.Diagnostics.Debug.WriteLine($"✓ Cập nhật bàn: {ban.TenBan} -> Đang chơi");

                    // 6. Tạo hóa đơn mới
                    var hoaDon = new HoaDon
                    {
                        MaBan = datBan.MaBan,
                        MaNv = maNv,
                        MaKh = datBan.MaKh,
                        ThoiGianBatDau = DateTime.Now,
                        TrangThai = "Đang chơi",
                        TienBan = 0,
                        TienDichVu = 0,
                        GiamGia = 0,
                        TongTien = 0
                    };

                    _context.HoaDons.Add(hoaDon);
                    System.Diagnostics.Debug.WriteLine($"✓ Tạo hóa đơn mới cho bàn {ban.TenBan}");

                    // 7. Cập nhật trạng thái đơn đặt thành "Đã xác nhận"
                    datBan.TrangThai = "Đã xác nhận";
                    System.Diagnostics.Debug.WriteLine($"✓ Cập nhật trạng thái đơn đặt -> Đã xác nhận");

                    // 8. Lưu thay đổi
                    await _context.SaveChangesAsync();
                    System.Diagnostics.Debug.WriteLine("✓ SaveChanges thành công");

                    await transaction.CommitAsync();
                    System.Diagnostics.Debug.WriteLine("✓✓✓ HOÀN TẤT XÁC NHẬN ĐẶT BÀN");

                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    System.Diagnostics.Debug.WriteLine($"❌ Exception trong ConfirmReservationAsync:");
                    System.Diagnostics.Debug.WriteLine($"Message: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Inner: {ex.InnerException?.Message}");
                    System.Diagnostics.Debug.WriteLine($"Stack: {ex.StackTrace}");
                    throw;
                }
            });
        }
    }
}