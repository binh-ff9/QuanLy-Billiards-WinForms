using Billiard.DAL.Data;
using Billiard.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Billiard.BLL.Services.HoaDonServices
{
    public class HoaDonService
    {
        private readonly BilliardDbContext _context;

        public HoaDonService(BilliardDbContext context)
        {
            _context = context;
        }

        #region Xem danh sách và chi tiết (cho HoaDonForm)

        /// <summary>
        /// Lấy tất cả hóa đơn để hiển thị trong grid
        /// </summary>
        public async Task<List<HoaDon>> GetTatCaHoaDonAsync()
        {
            return await _context.HoaDons
                .Include(h => h.MaNvNavigation)
                .Include(h => h.MaKhNavigation)
                .Include(h => h.MaBanNavigation)
                .OrderByDescending(h => h.ThoiGianBatDau)
                .ToListAsync();
        }
        public async Task<HoaDon> GetChiTietHoaDon(int maHoaDon)
        {
            return await _context.HoaDons
                .Include(h => h.MaBanNavigation)
                .Include(h=>h.MaBanNavigation.MaLoaiNavigation)
                .Include(h => h.MaNvNavigation)
                .Include(h => h.MaKhNavigation)
                .Include(h => h.ChiTietHoaDons)
                    .ThenInclude(ct => ct.MaDvNavigation)
                .FirstOrDefaultAsync(h => h.MaHd == maHoaDon);
        }
        public async Task<List<HoaDon>> GetInvoicesByStatusAsync(
            string trangThai,
            DateTime? tuNgay = null,
            DateTime? denNgay = null)
        {
            var query = _context.HoaDons
                .Include(h => h.MaBanNavigation)
                .Include(h => h.MaKhNavigation)
                .Include(h => h.MaNvNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(trangThai))
                query = query.Where(h => h.TrangThai == trangThai);

            if (tuNgay.HasValue)
                query = query.Where(h => h.ThoiGianBatDau >= tuNgay.Value);

            if (denNgay.HasValue)
                query = query.Where(h => h.ThoiGianBatDau <= denNgay.Value);

            return await query
                .OrderByDescending(h => h.ThoiGianBatDau)
                .ToListAsync();
        }

        public async Task<(List<HoaDon> Data, int TotalCount)> GetListHoaDonPagingAsync(
            string keyword,
            string status,
            DateTime fromDate,
            DateTime toDate,
            int pageIndex,
            int pageSize)
        {
            // 1. Khởi tạo truy vấn & Eager Loading (Include các bảng liên quan để hiển thị tên)
            var query = _context.HoaDons
                .Include(h => h.MaBanNavigation) // Để lấy Tên Bàn
                .Include(h => h.MaKhNavigation)  // Để lấy Tên Khách, SĐT
                .Include(h => h.MaNvNavigation)  // Để lấy Tên Nhân Viên
                .AsNoTracking()                  // Tối ưu hiệu năng (không theo dõi thay đổi vì chỉ để xem)
                .AsQueryable();

            // 2. Lọc theo Ngày (Bắt buộc)
            // Logic: Thời gian bắt đầu nằm trong khoảng Từ ngày -> Đến ngày
            query = query.Where(h => h.ThoiGianBatDau >= fromDate && h.ThoiGianBatDau <= toDate);

            // 3. Lọc theo Trạng thái
            if (!string.IsNullOrEmpty(status) && status != "Tất cả")
            {
                query = query.Where(h => h.TrangThai == status);
            }

            // 4. Tìm kiếm theo Từ khóa (Mã HĐ, Tên KH, SĐT)
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower(); // Chuyển về chữ thường để tìm không phân biệt hoa thường

                // Lưu ý: Cần xử lý null cho MaKhNavigation (trường hợp khách vãng lai)
                query = query.Where(h =>
                    h.MaHd.ToString().Contains(keyword) || // Tìm theo Mã HĐ
                    (h.MaKhNavigation != null && (h.MaKhNavigation.TenKh.ToLower().Contains(keyword) || h.MaKhNavigation.Sdt.Contains(keyword))) // Tìm theo Tên/SĐT
                );
            }

            // 5. Đếm tổng số bản ghi (Quan trọng để tính số trang)
            // Phải đếm TRƯỚC khi phân trang
            int totalCount = await query.CountAsync();

            // 6. Phân trang & Sắp xếp
            var data = await query
                .OrderByDescending(h => h.ThoiGianBatDau) // Hóa đơn mới nhất lên đầu
                .Skip((pageIndex - 1) * pageSize)         // Bỏ qua các dòng của trang trước
                .Take(pageSize)                           // Lấy số dòng của trang hiện tại
                .ToListAsync();

            // 7. Trả về kết quả (Tuple)
            return (data, totalCount);
        }



        #endregion

        #region Thao tác với hóa đơn đang chơi (cho QLBanForm)

        /// <summary>
        /// Lấy hóa đơn theo ID (bao gồm thông tin bàn, khu vực, loại bàn)
        /// </summary>
        public async Task<HoaDon> GetInvoiceByIdAsync(int maHd)
        {
            return await _context.HoaDons
                .Include(h => h.MaBanNavigation)
                    .ThenInclude(b => b.MaLoaiNavigation)
                .Include(h => h.MaBanNavigation)
                    .ThenInclude(b => b.MaKhuVucNavigation)
                .Include(h => h.MaKhNavigation)
                .Include(h => h.MaNvNavigation)
                .Include(h => h.ChiTietHoaDons)
                    .ThenInclude(ct => ct.MaDvNavigation)
                .FirstOrDefaultAsync(h => h.MaHd == maHd);
        }

        /// <summary>
        /// Thêm dịch vụ vào hóa đơn đang chơi
        /// </summary>
        public async Task<bool> AddServiceToInvoiceAsync(int maHd, int maDv, int soLuong)
        {
            return await Task.Run(() =>
            {
                using (var context = new BilliardDbContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            // Kiểm tra dịch vụ đã tồn tại trong hóa đơn chưa
                            var existing = context.ChiTietHoaDons
                                .FirstOrDefault(ct => ct.MaHd == maHd && ct.MaDv == maDv);

                            if (existing != null)
                            {
                                // Nếu đã tồn tại, cộng thêm số lượng
                                existing.SoLuong += soLuong;

                                // Cập nhật thành tiền
                                var dichVu = context.DichVus.Find(maDv);
                                if (dichVu != null)
                                {
                                    existing.ThanhTien = existing.SoLuong * dichVu.Gia;
                                }
                            }
                            else
                            {
                                // Nếu chưa có, thêm mới
                                var dichVu = context.DichVus.Find(maDv);
                                if (dichVu == null) return false;

                                var chiTiet = new ChiTietHoaDon
                                {
                                    MaHd = maHd,
                                    MaDv = maDv,
                                    SoLuong = soLuong,
                                    ThanhTien = soLuong * dichVu.Gia
                                };

                                context.ChiTietHoaDons.Add(chiTiet);
                            }

                            context.SaveChanges();
                            transaction.Commit();

                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                            return false;
                        }
                    }
                }
            });
        }

        /// <summary>
        /// Xóa dịch vụ khỏi hóa đơn đang chơi
        /// </summary>
        public async Task<bool> RemoveServiceFromInvoiceAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var chiTiet = await _context.ChiTietHoaDons.FindAsync(id);
                if (chiTiet == null)
                    return false;

                var hoaDon = await _context.HoaDons.FindAsync(chiTiet.MaHd);
                if (hoaDon == null || hoaDon.TrangThai != "Đang chơi")
                    return false;

                _context.ChiTietHoaDons.Remove(chiTiet);

                // Cập nhật tổng tiền dịch vụ
                var tongTienDv = await _context.ChiTietHoaDons
                    .Where(ct => ct.MaHd == chiTiet.MaHd && ct.Id != id)
                    .SumAsync(ct => ct.ThanhTien);

                hoaDon.TienDichVu = tongTienDv;

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

        /// <summary>
        /// Cập nhật giảm giá cho hóa đơn
        /// </summary>
        public async Task<bool> UpdateDiscountAsync(int maHd, decimal giamGia, string ghiChuGiamGia)
        {
            try
            {
                var hoaDon = await _context.HoaDons.FindAsync(maHd);
                if (hoaDon == null || hoaDon.TrangThai != "Đang chơi")
                    return false;

                hoaDon.GiamGia = giamGia;
                hoaDon.GhiChuGiamGia = ghiChuGiamGia;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Thanh toán hóa đơn - Kết thúc chơi và trả bàn
        /// </summary>
        public async Task<bool> PayInvoiceAsync(int maHd, string phuongThucThanhToan, decimal tienKhachDua = 0)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var hoaDon = await _context.HoaDons
                    .Include(h => h.MaBanNavigation)
                        .ThenInclude(b => b.MaLoaiNavigation)
                    .FirstOrDefaultAsync(h => h.MaHd == maHd);

                if (hoaDon == null || hoaDon.TrangThai != "Đang chơi")
                    return false;

                // Tính tiền bàn
                var duration = DateTime.Now - hoaDon.ThoiGianBatDau.Value;
                var soGio = (decimal)duration.TotalMinutes / 60;
                var giaGio = hoaDon.MaBanNavigation.MaLoaiNavigation.GiaGio;
                var tienBan = soGio * giaGio;

                // Tính tổng tiền
                var tongTien = tienBan + (hoaDon.TienDichVu ?? 0) - (hoaDon.GiamGia ?? 0);

                // Làm tròn lên nghìn
                tongTien = Math.Ceiling(tongTien / 1000) * 1000;

                // Update hóa đơn
                hoaDon.ThoiGianKetThuc = DateTime.Now;
                hoaDon.TienBan = tienBan;
                hoaDon.TongTien = tongTien;
                hoaDon.TrangThai = "Đã thanh toán";
                hoaDon.PhuongThucThanhToan = phuongThucThanhToan;

                // Update bàn
                var ban = hoaDon.MaBanNavigation;
                ban.TrangThai = "Trống";
                ban.GioBatDau = null;
                ban.MaKh = null;
                ban.GhiChu = null;

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

        #endregion

        #region Lịch sử hóa đơn cho User
        public async Task<List<HoaDon>> GetHistoryByCustomerAsync(int maKh)
        {
            return await _context.HoaDons
                .Include(h => h.MaBanNavigation)
                .Include(h => h.ChiTietHoaDons) // Để tính tổng tiền hoặc xem chi tiết sau này
                    .ThenInclude(ct => ct.MaDvNavigation)
                .Where(h => h.MaKh == maKh && h.TrangThai == "Đã thanh toán") // Chỉ lấy đơn đã xong
                .OrderByDescending(h => h.ThoiGianBatDau)
                .ToListAsync();
        }



        #endregion
    }
}