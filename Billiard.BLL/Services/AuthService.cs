using Billiard.DAL.Data;
using Billiard.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Billiard.BLL.Services
{
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
        public static string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }
        #endregion

        #region Validation
        /// <summary>
        /// Kiểm tra định dạng số điện thoại Việt Nam
        /// Hỗ trợ: 10 số bắt đầu bằng 0 (03, 05, 07, 08, 09)
        /// Hoặc có mã quốc gia +84
        /// </summary>
        public (bool IsValid, string Message) ValidatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return (false, "Số điện thoại không được để trống!");

            // Loại bỏ khoảng trắng, dấu gạch ngang
            string cleaned = phoneNumber.Trim().Replace(" ", "").Replace("-", "").Replace(".", "");

            // Pattern cho SĐT Việt Nam:
            // - Bắt đầu bằng 0: 03, 05, 07, 08, 09 + 8 số (tổng 10 số)
            // - Bắt đầu bằng +84: +84 + 9 số (bỏ số 0 đầu)
            // - Bắt đầu bằng 84: 84 + 9 số
            string pattern = @"^(0[3|5|7|8|9][0-9]{8}|\+84[3|5|7|8|9][0-9]{8}|84[3|5|7|8|9][0-9]{8})$";

            if (!Regex.IsMatch(cleaned, pattern))
            {
                return (false, "Số điện thoại không hợp lệ! Vui lòng nhập SĐT 10 số (VD: 0912345678)");
            }

            return (true, "Số điện thoại hợp lệ");
        }

        /// <summary>
        /// Chuẩn hóa số điện thoại về dạng 10 số bắt đầu bằng 0
        /// </summary>
        public string NormalizePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return phoneNumber;

            string cleaned = phoneNumber.Trim().Replace(" ", "").Replace("-", "").Replace(".", "");

            // Chuyển +84 hoặc 84 về dạng 0
            if (cleaned.StartsWith("+84"))
                cleaned = "0" + cleaned.Substring(3);
            else if (cleaned.StartsWith("84") && cleaned.Length == 11)
                cleaned = "0" + cleaned.Substring(2);

            return cleaned;
        }

        /// <summary>
        /// Kiểm tra ngày sinh hợp lệ
        /// - Không được là ngày trong tương lai
        /// - Tuổi từ 10 đến 120
        /// </summary>
        public (bool IsValid, string Message) ValidateDateOfBirth(DateOnly? dateOfBirth)
        {
            if (dateOfBirth == null)
                return (true, "Ngày sinh không bắt buộc"); // Cho phép null

            var today = DateOnly.FromDateTime(DateTime.Now);
            var dob = dateOfBirth.Value;

            // Kiểm tra ngày sinh không được trong tương lai
            if (dob > today)
                return (false, "Ngày sinh không được là ngày trong tương lai!");

            // Tính tuổi
            int age = today.Year - dob.Year;
            if (today < dob.AddYears(age))
                age--;

            // Kiểm tra tuổi hợp lệ (10-120 tuổi)
            if (age < 16)
                return (false, "Khách hàng phải từ 10 tuổi trở lên!");

            if (age > 120)
                return (false, "Ngày sinh không hợp lệ! Vui lòng kiểm tra lại.");

            return (true, "Ngày sinh hợp lệ");
        }

        /// <summary>
        /// Kiểm tra ngày sinh từ DateTime
        /// </summary>
        public (bool IsValid, string Message) ValidateDateOfBirth(DateTime? dateOfBirth)
        {
            if (dateOfBirth == null)
                return (true, "Ngày sinh không bắt buộc");

            return ValidateDateOfBirth(DateOnly.FromDateTime(dateOfBirth.Value));
        }

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch { return false; }
        }
        #endregion

        #region Universal Login
        public async Task<LoginResult> LoginAsync(string username, string password)
        {
            try
            {
                Debug.WriteLine($"[AuthService] Login attempt for: {username}");

                // Chuẩn hóa nếu username là số điện thoại
                string normalizedUsername = username;
                var phoneValidation = ValidatePhoneNumber(username);
                if (phoneValidation.IsValid)
                {
                    normalizedUsername = NormalizePhoneNumber(username);
                }

                string hashedInputPassword = HashPassword(password);
                Debug.WriteLine($"[AuthService] Hashed input password: {hashedInputPassword}");

                // 1. Tìm Nhân Viên
                var nhanVien = await _context.NhanViens
                    .Include(nv => nv.MaNhomNavigation)
                    .FirstOrDefaultAsync(nv =>
                        (nv.Sdt == normalizedUsername || nv.Email == username) &&
                        nv.TrangThai == "Đang làm");

                if (nhanVien != null)
                {
                    Debug.WriteLine($"[AuthService] Found NhanVien: {nhanVien.TenNv}");

                    bool passwordMatch = nhanVien.MatKhau == password ||
                                         nhanVien.MatKhau == hashedInputPassword;

                    if (passwordMatch)
                    {
                        if (nhanVien.MatKhau == password && nhanVien.MatKhau != hashedInputPassword)
                        {
                            nhanVien.MatKhau = hashedInputPassword;
                            await _context.SaveChangesAsync();
                        }

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
                        (kh.Sdt == normalizedUsername || kh.Email == username) &&
                        kh.HoatDong == true);

                if (khachHang != null)
                {
                    Debug.WriteLine($"[AuthService] Found KhachHang: {khachHang.TenKh}");

                    bool passwordMatch = khachHang.MatKhau == password ||
                                         khachHang.MatKhau == hashedInputPassword;

                    if (passwordMatch)
                    {
                        if (khachHang.MatKhau == password && khachHang.MatKhau != hashedInputPassword)
                        {
                            khachHang.MatKhau = hashedInputPassword;
                        }

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
                        return new LoginResult
                        {
                            Success = false,
                            Message = "Mật khẩu không chính xác!"
                        };
                    }
                }

                return new LoginResult
                {
                    Success = false,
                    Message = "Tài khoản không tồn tại hoặc đã bị khóa!"
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AuthService] Exception: {ex.Message}");
                return new LoginResult
                {
                    Success = false,
                    Message = $"Lỗi hệ thống: {ex.Message}"
                };
            }
        }
        #endregion
        public bool HasVietnameseSigns(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;

            // Pattern chứa các ký tự tiếng Việt có dấu phổ biến
            string pattern = @"[áàảãạâấầẩẫậăắằẳẵặéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵđÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴĐ]";
            return Regex.IsMatch(text, pattern);
        }
        #region Customer Registration
        public async Task<(bool Success, string Message, KhachHang Customer)> RegisterCustomerAsync(
            string tenKh, string sdt, string email, string matKhau, DateOnly? ngaySinh = null)
        {
            // ✅ Validate số điện thoại
            var phoneValidation = ValidatePhoneNumber(sdt);
            if (!phoneValidation.IsValid)
                return (false, phoneValidation.Message, null);

            // Chuẩn hóa SĐT
            sdt = NormalizePhoneNumber(sdt);

            // ✅ Validate ngày sinh
            var dobValidation = ValidateDateOfBirth(ngaySinh);
            if (!dobValidation.IsValid)
                return (false, dobValidation.Message, null);

            // ✅ Validate email
            if (!IsValidEmail(email))
                return (false, "Email không hợp lệ!", null);

            // Kiểm tra SĐT đã tồn tại
            var sdtExistsInNV = await _context.NhanViens.AnyAsync(nv => nv.Sdt == sdt);
            var sdtExistsInKH = await _context.KhachHangs.AnyAsync(kh => kh.Sdt == sdt);

            if (sdtExistsInNV || sdtExistsInKH)
                return (false, "Số điện thoại này đã được đăng ký!", null);

            // Kiểm tra Email đã tồn tại
            var emailExistsInNV = await _context.NhanViens.AnyAsync(nv => nv.Email == email);
            var emailExistsInKH = await _context.KhachHangs.AnyAsync(kh => kh.Email == email);

            if (emailExistsInNV || emailExistsInKH)
                return (false, "Email này đã được đăng ký!", null);

            if (HasVietnameseSigns(matKhau))
                return (false, "Mật khẩu không được chứa ký tự có dấu!", null);

            if (matKhau.Contains(" "))
                return (false, "Mật khẩu không được chứa khoảng trắng!", null);
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
        public async Task<(bool Exists, UserType? UserType)> CheckEmailExistsAsync(string email)
        {
            var isNhanVien = await _context.NhanViens.AnyAsync(nv => nv.Email == email);
            if (isNhanVien)
                return (true, Billiard.BLL.Services.UserType.NhanVien);

            var isKhachHang = await _context.KhachHangs.AnyAsync(kh => kh.Email == email);
            if (isKhachHang)
                return (true, Billiard.BLL.Services.UserType.KhachHang);

            return (false, null);
        }

        public async Task<bool> ResetPasswordAsync(string email, string newPassword)
        {
            var hashedPassword = HashPassword(newPassword);

            var nhanVien = await _context.NhanViens
                .FirstOrDefaultAsync(nv => nv.Email == email);

            if (nhanVien != null)
            {
                nhanVien.MatKhau = hashedPassword;
                await _context.SaveChangesAsync();
                return true;
            }

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
            catch { }
        }
        #endregion
    }
}