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
        private readonly GioHoatDongService _gioHoatDongService;
        public BanBiaService(BilliardDbContext context, GioHoatDongService gioHoatDongService)
        {
            _context = context;
            _gioHoatDongService = gioHoatDongService;
        }

        // Lấy tất cả bàn
        public async Task<List<BanBium>> GetAllTablesAsync()
        {
            var allTables = await _context.BanBia
                .AsNoTracking()
                .Include(b => b.MaKhuVucNavigation)
                .Include(b => b.MaLoaiNavigation)
                .Include(b => b.MaKhNavigation)
                .Where(b => b.TrangThai != "Bảo trì")
                .ToListAsync();

            return allTables
                .OrderByDescending(b => b.GhiChu?.Contains("URGENT_PAYMENT") ?? false)
                .ThenBy(b => b.TenBan)
                .ToList();
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

            var results = await query.ToListAsync();

            return results
                .OrderByDescending(b => b.GhiChu?.Contains("URGENT_PAYMENT") ?? false)
                .ThenBy(b => b.TenBan)
                .ToList();
        }
        public async Task<List<BanBium>> KiemTraBanDenGioDongCua()
        {
            var gioDongCua = _gioHoatDongService.LayThoiDiemDongCua();
            if (DateTime.Now < gioDongCua)
                return new List<BanBium>();

            var banDangChoi = await _context.BanBia
                .Include(b => b.MaKhuVucNavigation)
                .Include(b => b.MaLoaiNavigation)
                .Include(b => b.MaKhNavigation)
                .Where(b => b.TrangThai == "Đang chơi"
                    && b.GioBatDau.HasValue
                    && b.GioBatDau.Value < gioDongCua)
                .ToListAsync();

            return banDangChoi;
        }
        public async Task<(decimal tienTamTinh, string ghiChu)> TinhTienTamThoiBan(int maBan)
        {
            var ban = await _context.BanBia
                .Include(b => b.MaLoaiNavigation)
                .FirstOrDefaultAsync(b => b.MaBan == maBan);

            if (ban == null || !ban.GioBatDau.HasValue)
                return (0, "Không tìm thấy thông tin bàn");

            var giaGio = ban.MaLoaiNavigation?.GiaGio ?? 0;

            var ketQua = _gioHoatDongService.TinhTienTamThoi(ban.GioBatDau.Value, giaGio);

            if (_gioHoatDongService.KiemTraBanQuaGioChoPhep(ban.GioBatDau.Value))
            {
                var gioDongCua = _gioHoatDongService.LayThoiDiemDongCuaTheoBanBatDau(ban.GioBatDau.Value);
                var soGioToiDa = _gioHoatDongService.LaySoGioHoatDongToiDa();

                ketQua.ghiChu += $"\n⛔ BÀN ĐÃ QUÁ {soGioToiDa}H - Vui lòng thanh toán NGAY!";
            }

            return (ketQua.tienBan, ketQua.ghiChu);
        }
        public async Task<List<BanBium>> KiemTraBanQuaGioChoPhep()
        {
            var banDangChoi = await _context.BanBia
                .Include(b => b.MaKhuVucNavigation)
                .Include(b => b.MaLoaiNavigation)
                .Include(b => b.MaKhNavigation)
                .Where(b => b.TrangThai == "Đang chơi" && b.GioBatDau.HasValue)
                .ToListAsync();

            var banQuaGio = new List<BanBium>();

            foreach (var ban in banDangChoi)
            {
                if (_gioHoatDongService.KiemTraBanQuaGioChoPhep(ban.GioBatDau.Value))
                {
                    banQuaGio.Add(ban);
                }
            }

            return banQuaGio;
        }
        public async Task<(decimal tienBan, string ghiChu, DateTime thoiGianKetThuc)> TinhTienChinhXacBan(int maBan)
        {
            var ban = await _context.BanBia
                .Include(b => b.MaLoaiNavigation)
                .FirstOrDefaultAsync(b => b.MaBan == maBan);

            if (ban == null || !ban.GioBatDau.HasValue)
                return (0, "Không tìm thấy thông tin bàn", DateTime.Now);

            var giaGio = ban.MaLoaiNavigation?.GiaGio ?? 0;
            var gioBatDau = ban.GioBatDau.Value;

            var thoiGianKetThuc = _gioHoatDongService.LayThoiGianKetThucHopLe(gioBatDau);

            var duration = thoiGianKetThuc - gioBatDau;
            var tongPhut = (int)Math.Ceiling(duration.TotalMinutes);
            var soGio = (decimal)tongPhut / 60m;
            var tienBan = soGio * giaGio;

            var gioDongCua = _gioHoatDongService.LayThoiDiemDongCuaTheoBanBatDau(gioBatDau);
            string ghiChu;

            if (_gioHoatDongService.KiemTraBanQuaGioChoPhep(gioBatDau))
            {
                var soGioToiDa = _gioHoatDongService.LaySoGioHoatDongToiDa();
                ghiChu = $"⚠️ ĐÃ QUÁ {soGioToiDa}H - Tính từ {gioBatDau:HH:mm} đến {thoiGianKetThuc:HH:mm}";
            }
            else if (DateTime.Now >= gioDongCua)
            {
                ghiChu = $"⚠️ ĐÃ ĐÓNG CỬA - Tính đến {thoiGianKetThuc:HH:mm}";
            }
            else
            {
                ghiChu = $"Từ {gioBatDau:HH:mm} đến {thoiGianKetThuc:HH:mm}";
            }

            return (tienBan, ghiChu, thoiGianKetThuc);
        }
        public async Task<bool> DanhDauBanCanThanhToan(int maBan, bool canThanhToan)
        {
            try
            {
                var ban = await _context.BanBia.FindAsync(maBan);
                if (ban == null) return false;

                if (canThanhToan)
                {
                    if (!ban.GhiChu?.Contains("URGENT_PAYMENT") ?? true)
                    {
                        ban.GhiChu = (ban.GhiChu ?? "") + " URGENT_PAYMENT";
                    }
                }
                else
                {
                    if (ban.GhiChu?.Contains("URGENT_PAYMENT") ?? false)
                    {
                        ban.GhiChu = ban.GhiChu.Replace("URGENT_PAYMENT", "").Trim();
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<(List<BanBium> banCanThanhToan, bool isDongCua)> KiemTraVaXuLyGioDongCua()
        {
            var gioDongCua = _gioHoatDongService.LayThoiDiemDongCua();
            var isDongCua = DateTime.Now >= gioDongCua;

            if (!isDongCua)
                return (new List<BanBium>(), false);

            // Lấy danh sách bàn đang chơi và đã quá giờ đóng cửa
            var banDangChoi = await _context.BanBia
                .Include(b => b.MaKhuVucNavigation)
                .Include(b => b.MaLoaiNavigation)
                .Include(b => b.MaKhNavigation)
                .Where(b => b.TrangThai == "Đang chơi"
                    && b.GioBatDau.HasValue
                    && b.GioBatDau.Value < gioDongCua)
                .ToListAsync();

            // Đánh dấu tất cả các bàn này cần thanh toán khẩn cấp
            foreach (var ban in banDangChoi)
            {
                await DanhDauBanCanThanhToan(ban.MaBan, true);
            }

            return (banDangChoi, true);
        }
        private void DetachAllEntities()
        {
            foreach (var entry in _context.ChangeTracker.Entries().ToList())
            {
                entry.State = EntityState.Detached;
            }
        }
        public async Task<(bool isSuccess, string message, bool needConfirmation)> StartTableAsync(
        int maBan,
        int maNv,
        int? maKh = null,
        bool skipWarning = false)
        {
            // Kiểm tra giờ đóng cửa
            if (!skipWarning && _gioHoatDongService.SapDenGioDongCua())
            {
                var phutConLai = _gioHoatDongService.TinhSoPhutConLaiDenDongCua();
                return (false,
                    $"⚠️ Sắp đến giờ đóng cửa!\nChỉ còn {phutConLai} phút nữa.\n\nBạn có chắc muốn bắt đầu chơi?",
                    true);
            }

            await _semaphore.WaitAsync();
            try
            {
                var strategy = _context.Database.CreateExecutionStrategy();

                return await strategy.ExecuteAsync(async () =>
                {
                    using var transaction = await _context.Database.BeginTransactionAsync();
                    try
                    {
                        System.Diagnostics.Debug.WriteLine($"\n=== StartTableAsync - Bàn {maBan} ===");

                        // ✅ 1. LẤY THÔNG TIN BÀN (với AsNoTracking)
                        var ban = await _context.BanBia
                            .AsNoTracking()
                            .Include(b => b.MaKhuVucNavigation)
                            .Include(b => b.MaLoaiNavigation)
                            .FirstOrDefaultAsync(b => b.MaBan == maBan);

                        if (ban == null)
                        {
                            System.Diagnostics.Debug.WriteLine($"❌ Không tìm thấy bàn {maBan}");
                            return (false, "Không tìm thấy bàn", false);
                        }

                        System.Diagnostics.Debug.WriteLine($"Bàn: {ban.TenBan} - Trạng thái: {ban.TrangThai}");

                        // ✅ 2. KIỂM TRA TRẠNG THÁI BÀN
                        if (ban.TrangThai != "Trống" && ban.TrangThai != "Đã đặt")
                        {
                            System.Diagnostics.Debug.WriteLine($"❌ Bàn có trạng thái không hợp lệ: {ban.TrangThai}");
                            return (false, $"Bàn có trạng thái: {ban.TrangThai}", false);
                        }

                        // ✅ 3. KIỂM TRA HÓA ĐƠN HIỆN TẠI (QUAN TRỌNG)
                        var existingInvoice = await _context.HoaDons
                            .AsNoTracking() // ✅ Không tracking để tránh conflict
                            .Where(h => h.MaBan == maBan)
                            .OrderByDescending(h => h.MaHd)
                            .FirstOrDefaultAsync();

                        if (existingInvoice != null)
                        {
                            System.Diagnostics.Debug.WriteLine($"Tìm thấy hóa đơn: HD{existingInvoice.MaHd} - Trạng thái: {existingInvoice.TrangThai}");

                            // ✅ CHỈ CHẶN nếu đang chơi
                            if (existingInvoice.TrangThai == "Đang chơi")
                            {
                                System.Diagnostics.Debug.WriteLine($"❌ Bàn đang có hóa đơn đang chơi: HD{existingInvoice.MaHd}");
                                return (false, "Bàn đã có hóa đơn đang hoạt động", false);
                            }

                            // ✅ Nếu đã thanh toán hoặc đã hủy -> OK, cho phép tạo mới
                            System.Diagnostics.Debug.WriteLine($"✓ Hóa đơn cũ HD{existingInvoice.MaHd} đã {existingInvoice.TrangThai}, cho phép tạo mới");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("✓ Bàn chưa có hóa đơn nào");
                        }

                        // ✅ 4. XỬ LÝ ĐẶT BÀN (nếu có)
                        if (ban.TrangThai == "Đã đặt")
                        {
                            System.Diagnostics.Debug.WriteLine("Bàn đang được đặt, tìm thông tin đặt bàn...");

                            var datBan = await _context.DatBans
                                .Where(d => d.MaBan == maBan && (d.TrangThai == "Đang chờ" || d.TrangThai == "Đã đặt"))
                                .OrderBy(d => d.ThoiGianBatDau)
                                .FirstOrDefaultAsync();

                            if (datBan != null)
                            {
                                System.Diagnostics.Debug.WriteLine($"✓ Tìm thấy đơn đặt: {datBan.TenKhach}");

                                if (!maKh.HasValue && datBan.MaKh.HasValue)
                                {
                                    maKh = datBan.MaKh;
                                    System.Diagnostics.Debug.WriteLine($"✓ Lấy MaKH từ đơn đặt: {maKh}");
                                }

                                datBan.TrangThai = "Đã xác nhận";
                                System.Diagnostics.Debug.WriteLine("✓ Cập nhật trạng thái đơn đặt -> Đã xác nhận");
                            }
                        }

                        // ✅ 5. LẤY LẠI BÀN ĐỂ UPDATE (với tracking)
                        var banToUpdate = await _context.BanBia.FindAsync(maBan);
                        if (banToUpdate == null)
                        {
                            System.Diagnostics.Debug.WriteLine($"❌ Không tìm thấy bàn để update");
                            return (false, "Không thể cập nhật bàn", false);
                        }

                        // ✅ 6. CẬP NHẬT BÀN
                        banToUpdate.TrangThai = "Đang chơi";
                        banToUpdate.GioBatDau = DateTime.Now;
                        banToUpdate.MaKh = maKh;
                        System.Diagnostics.Debug.WriteLine($"✓ Cập nhật bàn: Trống/Đã đặt → Đang chơi");

                        // ✅ 7. TẠO HÓA ĐƠN MỚI
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
                        System.Diagnostics.Debug.WriteLine($"✓ Tạo hóa đơn mới");

                        // ✅ 8. LƯU THAY ĐỔI
                        var savedCount = await _context.SaveChangesAsync();
                        System.Diagnostics.Debug.WriteLine($"✓ Đã lưu {savedCount} thay đổi");

                        await transaction.CommitAsync();
                        System.Diagnostics.Debug.WriteLine($"✓✓✓ HOÀN TẤT - HD{hoaDon.MaHd}\n");

                        // ✅ 9. DETACH SAU KHI COMMIT
                        DetachAllEntities();

                        return (true, "Đã bắt đầu chơi thành công", false);
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        System.Diagnostics.Debug.WriteLine($"❌ Exception: {ex.Message}");
                        System.Diagnostics.Debug.WriteLine($"Stack: {ex.StackTrace}");
                        return (false, $"Lỗi: {ex.Message}", false);
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Outer Exception: {ex.Message}");
                return (false, $"Lỗi: {ex.Message}", false);
            }
            finally
            {
                _semaphore.Release();
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

                // Remove urgent flag
                await DanhDauBanCanThanhToan(maBan, false);

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
        public async Task<Dictionary<int, int>> GetReservationCountsForDateAsync(DateTime date)
        {
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1).AddSeconds(-1);

            var counts = await _context.DatBans
                .Where(d => (d.TrangThai == "Đang chờ" || d.TrangThai == "Đã đặt" || d.TrangThai == "Đã xác nhận")
                    && d.ThoiGianBatDau.HasValue
                    && d.ThoiGianBatDau >= startOfDay
                    && d.ThoiGianBatDau <= endOfDay)
                .GroupBy(d => d.MaBan ?? 0)
                .Select(g => new { MaBan = g.Key, Count = g.Count() })
                .ToListAsync();

            return counts.ToDictionary(x => x.MaBan, x => x.Count);
        }
        public async Task<BanBium> GetTableByIdAsync(int maBan)
        {
            return await _context.BanBia
                .Include(b => b.MaKhuVucNavigation)
                .Include(b => b.MaLoaiNavigation)
                .Include(b => b.MaKhNavigation)
                .FirstOrDefaultAsync(b => b.MaBan == maBan);
        }

        public async Task<HoaDon> GetActiveInvoiceAsync(int maBan)
        {
            return await _context.HoaDons
                .Include(h => h.ChiTietHoaDons)
                    .ThenInclude(ct => ct.MaDvNavigation)
                .FirstOrDefaultAsync(h => h.MaBan == maBan && h.TrangThai == "Đang chơi");
        }

        // Lấy chi tiết hóa đơn
        public async Task<List<ChiTietHoaDon>> GetInvoiceDetailsAsync(int maHd)
        {
            return await _context.ChiTietHoaDons
                .AsNoTracking()
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
        public async Task<bool> HoldReservationAsync(int maDat, int additionalMinutes = 15)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    System.Diagnostics.Debug.WriteLine($"\n=== HoldReservationAsync ===");
                    System.Diagnostics.Debug.WriteLine($"MaDat: {maDat}, Thêm {additionalMinutes} phút");

                    var datBan = await _context.DatBans
                        .Include(d => d.MaBanNavigation)
                        .FirstOrDefaultAsync(d => d.MaDat == maDat);

                    if (datBan == null)
                    {
                        System.Diagnostics.Debug.WriteLine("❌ Không tìm thấy đơn đặt bàn");
                        return false;
                    }

                    if (!datBan.ThoiGianBatDau.HasValue)
                    {
                        System.Diagnostics.Debug.WriteLine("❌ Đơn đặt không có thời gian bắt đầu");
                        return false;
                    }

                    // Gia hạn thời gian bắt đầu
                    var oldStartTime = datBan.ThoiGianBatDau.Value;
                    var newStartTime = oldStartTime.AddMinutes(additionalMinutes);

                    System.Diagnostics.Debug.WriteLine($"Thời gian cũ: {oldStartTime:HH:mm dd/MM/yyyy}");
                    System.Diagnostics.Debug.WriteLine($"Thời gian mới: {newStartTime:HH:mm dd/MM/yyyy}");

                    // Cập nhật thời gian
                    datBan.ThoiGianBatDau = newStartTime;
                    if (datBan.ThoiGianKetThuc.HasValue)
                    {
                        datBan.ThoiGianKetThuc = datBan.ThoiGianKetThuc.Value.AddMinutes(additionalMinutes);
                    }

                    // Thêm ghi chú
                    var holdNote = $"[Giữ bàn +{additionalMinutes}p lúc {DateTime.Now:HH:mm dd/MM}]";
                    datBan.GhiChu = string.IsNullOrEmpty(datBan.GhiChu)
                        ? holdNote
                        : datBan.GhiChu + " " + holdNote;

                    System.Diagnostics.Debug.WriteLine($"✓ Đã gia hạn đơn đặt");

                    await _context.SaveChangesAsync();
                    System.Diagnostics.Debug.WriteLine("✓ SaveChanges thành công");

                    await transaction.CommitAsync();
                    System.Diagnostics.Debug.WriteLine("✓✓✓ HOÀN TẤT GIỮ BÀN");

                    DetachAllEntities();

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

                    datBan.TrangThai = "Đã hủy";
                    System.Diagnostics.Debug.WriteLine($"✓ Cập nhật đơn đặt -> Đã hủy");

                    await _context.SaveChangesAsync();
                    System.Diagnostics.Debug.WriteLine("✓ SaveChanges thành công");

                    await transaction.CommitAsync();
                    System.Diagnostics.Debug.WriteLine("✓✓✓ HOÀN TẤT HỦY ĐẶT BÀN");

                    DetachAllEntities();

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
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    System.Diagnostics.Debug.WriteLine($"\n=== ConfirmReservationAsync ===");
                    System.Diagnostics.Debug.WriteLine($"MaDat: {maDat}, MaNV: {maNv}");

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

                    if (datBan.TrangThai != "Đang chờ" && datBan.TrangThai != "Đã đặt")
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ Trạng thái đơn không hợp lệ: {datBan.TrangThai}");
                        return false;
                    }

                    var ban = datBan.MaBanNavigation;
                    if (ban.TrangThai != "Đã đặt" && ban.TrangThai != "Trống")
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ Bàn có trạng thái không hợp lệ: {ban.TrangThai}");
                        return false;
                    }

                    var existingInvoice = await _context.HoaDons
                        .FirstOrDefaultAsync(h => h.MaBan == ban.MaBan && h.TrangThai == "Đang chơi");

                    if (existingInvoice != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ Bàn đã có hóa đơn đang hoạt động: HD{existingInvoice.MaHd}");
                        return false;
                    }

                    ban.TrangThai = "Đang chơi";
                    ban.GioBatDau = DateTime.Now;
                    ban.MaKh = datBan.MaKh;
                    System.Diagnostics.Debug.WriteLine($"✓ Cập nhật bàn: {ban.TenBan} -> Đang chơi");

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

                    datBan.TrangThai = "Đã xác nhận";
                    System.Diagnostics.Debug.WriteLine($"✓ Cập nhật trạng thái đơn đặt -> Đã xác nhận");

                    await _context.SaveChangesAsync();
                    System.Diagnostics.Debug.WriteLine("✓ SaveChanges thành công");

                    await transaction.CommitAsync();
                    System.Diagnostics.Debug.WriteLine("✓✓✓ HOÀN TẤT XÁC NHẬN ĐẶT BÀN");

                    DetachAllEntities();

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