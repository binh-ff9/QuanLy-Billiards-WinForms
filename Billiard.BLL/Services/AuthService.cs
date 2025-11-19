using Billiard.DAL.Data;
using Billiard.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Billiard.BLL.Services
{
    /// <summary>
    /// Kết quả đăng nhập chung cho cả nhân viên và khách hàng
    /// </summary>
    public class LoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public UserType? UserType { get; set; }
        public NhanVien NhanVien { get; set; }
        public KhachHang KhachHang { get; set; }
    }

    public enum UserType
    {
        NhanVien,
        KhachHang
    }

    public class AuthService
    {
        private readonly BilliardDbContext _context;

        public AuthService(BilliardDbContext context)
        {
            _context = context;
        }

        #region Password Hashing
        /// <summary>
        /// Mã hóa mật khẩu bằng SHA256
        /// </summary>
        public static string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Xác thực mật khẩu
        /// </summary>
        public bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }
        #endregion

        #region Universal Login
        /// <summary>
        /// Đăng nhập thống nhất - Tự động kiểm tra cả nhân viên và khách hàng
        /// </summary>
        public async Task<LoginResult> LoginAsync(string username, string password)
        {
            try
            {
                Debug.WriteLine($"[AuthService] Login attempt for: {username}");
                Debug.WriteLine($"[AuthService] Password: {password}");

                // 1. Tìm Nhân Viên
                var nhanVien = await _context.NhanViens
                    .Include(nv => nv.MaNhomNavigation)
                    .FirstOrDefaultAsync(nv =>
                        (nv.Sdt == username || nv.Email == username) &&
                        nv.TrangThai == "Đang làm");

                if (nhanVien != null)
                {
                    Debug.WriteLine($"[AuthService] Found NhanVien: {nhanVien.TenNv}");
                    Debug.WriteLine($"[AuthService] DB Password: {nhanVien.MatKhau}");

                    // ✅ SO SÁNH PLAIN TEXT (vì DB chưa hash)
                    bool passwordMatch = nhanVien.MatKhau == password;
                    Debug.WriteLine($"[AuthService] Password match: {passwordMatch}");

                    if (passwordMatch)
                    {
                        Debug.WriteLine("[AuthService] Login SUCCESS for NhanVien");
                        return new LoginResult
                        {
                            Success = true,
                            Message = "Đăng nhập thành công!",
                            UserType = UserType.NhanVien,
                            NhanVien = nhanVien
                        };
                    }
                    else
                    {
                        Debug.WriteLine("[AuthService] Password mismatch!");
                        return new LoginResult
                        {
                            Success = false,
                            Message = "Mật khẩu không chính xác!"
                        };
                    }
                }

                // 2. Tìm Khách Hàng
                var khachHang = await _context.KhachHangs
                    .FirstOrDefaultAsync(kh =>
                        (kh.Sdt == username || kh.Email == username) &&
                        kh.HoatDong == true);

                if (khachHang != null)
                {
                    Debug.WriteLine($"[AuthService] Found KhachHang: {khachHang.TenKh}");
                    Debug.WriteLine($"[AuthService] DB Password: {khachHang.MatKhau}");

                    // ✅ SO SÁNH PLAIN TEXT
                    bool passwordMatch = khachHang.MatKhau == password;
                    Debug.WriteLine($"[AuthService] Password match: {passwordMatch}");

                    if (passwordMatch)
                    {
                        Debug.WriteLine("[AuthService] Login SUCCESS for KhachHang");

                        // Cập nhật lần đến cuối
                        khachHang.LanDenCuoi = DateTime.Now;
                        await _context.SaveChangesAsync();

                        return new LoginResult
                        {
                            Success = true,
                            Message = "Đăng nhập thành công!",
                            UserType = UserType.KhachHang,
                            KhachHang = khachHang
                        };
                    }
                    else
                    {
                        Debug.WriteLine("[AuthService] Password mismatch!");
                        return new LoginResult
                        {
                            Success = false,
                            Message = "Mật khẩu không chính xác!"
                        };
                    }
                }

                Debug.WriteLine("[AuthService] User not found");
                return new LoginResult
                {
                    Success = false,
                    Message = "Tài khoản không tồn tại hoặc đã bị khóa!"
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AuthService] Exception: {ex.Message}");
                Debug.WriteLine($"[AuthService] StackTrace: {ex.StackTrace}");
                return new LoginResult
                {
                    Success = false,
                    Message = $"Lỗi hệ thống: {ex.Message}"
                };
            }
        }
        #endregion

        #region Customer Registration (Khách hàng)
        /// <summary>
        /// Đăng ký khách hàng mới
        /// </summary>
        public async Task<(bool Success, string Message, KhachHang Customer)> RegisterCustomerAsync(
            string tenKh, string sdt, string email, string matKhau, DateOnly? ngaySinh = null)
        {
            // Kiểm tra số điện thoại đã tồn tại (cả nhân viên và khách hàng)
            var sdtExistsInNV = await _context.NhanViens.AnyAsync(nv => nv.Sdt == sdt);
            var sdtExistsInKH = await _context.KhachHangs.AnyAsync(kh => kh.Sdt == sdt);

            if (sdtExistsInNV || sdtExistsInKH)
            {
                return (false, "Số điện thoại này đã được đăng ký!", null);
            }

            // Kiểm tra email đã tồn tại (cả nhân viên và khách hàng)
            var emailExistsInNV = await _context.NhanViens.AnyAsync(nv => nv.Email == email);
            var emailExistsInKH = await _context.KhachHangs.AnyAsync(kh => kh.Email == email);

            if (emailExistsInNV || emailExistsInKH)
            {
                return (false, "Email này đã được đăng ký!", null);
            }

            // Tạo khách hàng mới
            var khachHang = new KhachHang
            {
                TenKh = tenKh,
                Sdt = sdt,
                Email = email,
                MatKhau = HashPassword(matKhau),
                NgaySinh = ngaySinh,
                HangTv = "Đồng",
                DiemTichLuy = 0,
                TongChiTieu = 0,
                NgayDangKy = DateTime.Now,
                HoatDong = true
            };

            _context.KhachHangs.Add(khachHang);
            await _context.SaveChangesAsync();

            return (true, "Đăng ký thành công!", khachHang);
        }
        #endregion

        #region Password Recovery
        /// <summary>
        /// Kiểm tra email tồn tại (cả nhân viên và khách hàng)
        /// </summary>
        public async Task<(bool Exists, UserType? UserType)> CheckEmailExistsAsync(string email)
        {
            var isNhanVien = await _context.NhanViens.AnyAsync(nv => nv.Email == email);
            if (isNhanVien)
            {
                return (true, Billiard.BLL.Services.UserType.NhanVien);
            }

            var isKhachHang = await _context.KhachHangs.AnyAsync(kh => kh.Email == email);
            if (isKhachHang)
            {
                return (true, Billiard.BLL.Services.UserType.KhachHang);
            }

            return (false, null);
        }

        /// <summary>
        /// Đặt lại mật khẩu (tự động phát hiện loại tài khoản)
        /// </summary>
        public async Task<bool> ResetPasswordAsync(string email, string newPassword)
        {
            var hashedPassword = HashPassword(newPassword);

            // Kiểm tra nhân viên
            var nhanVien = await _context.NhanViens
                .FirstOrDefaultAsync(nv => nv.Email == email);

            if (nhanVien != null)
            {
                nhanVien.MatKhau = hashedPassword;
                await _context.SaveChangesAsync();
                return true;
            }

            // Kiểm tra khách hàng
            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(kh => kh.Email == email);

            if (khachHang != null)
            {
                khachHang.MatKhau = hashedPassword;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
        #endregion

        #region Activity Logging
        /// <summary>
        /// Ghi log hoạt động (chỉ cho nhân viên)
        /// </summary>
        public async Task LogActivityAsync(int maNv, string hanhDong, string chiTiet)
        {
            try
            {
                var log = new LichSuHoatDong
                {
                    MaNv = maNv,
                    HanhDong = hanhDong,
                    ChiTiet = chiTiet,
                    ThoiGian = DateTime.Now
                };
                _context.LichSuHoatDongs.Add(log);
                await _context.SaveChangesAsync();
            }
            catch
            {
                // Log errors silently
            }
        }
        #endregion

        #region Validation
        /// <summary>
        /// Kiểm tra email hợp lệ
        /// </summary>
        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}