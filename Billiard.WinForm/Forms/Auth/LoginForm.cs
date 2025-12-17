using Billiard.BLL.Services;
using Billiard.DAL.Data;
using Billiard.WinForm.Helpers;
using Billiard.WinForm.Forms.Helpers;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Billiard.WinForm.Forms.Users;

namespace Billiard.WinForm.Forms.Auth
{
    public partial class LoginForm : Form
    {
        private readonly BilliardDbContext _context;
        private readonly AuthService _authService;
        private bool _isLoggingIn;

        // Label để hiển thị lỗi validation
        private Label lblUsernameError;
        private Label lblPasswordError;

        // Danh sách các đuôi email phổ biến
        private readonly string[] _commonEmailDomains = new[]
        {
            "@gmail.com", "@yahoo.com", "@outlook.com", "@hotmail.com",
            "@icloud.com", "@aol.com", "@protonmail.com", "@zoho.com",
            "@mail.com", "@gmx.com", "@yandex.com", "@tutanota.com"
        };

        public LoginForm(BilliardDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
            InitializeComponent();
            InitializeUI();
            InitializeValidationLabels();
        }

        private void InitializeUI()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            lblSignup.Visible = true;
        }

        private void InitializeValidationLabels()
        {
            // Tạo label thông báo lỗi cho username
            lblUsernameError = new Label
            {
                AutoSize = false,
                Size = new Size(300, 20),
                Location = new Point(51, 249), // Ngay dưới txtUsername
                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(220, 38, 38), // Red-600
                Text = "",
                Visible = false
            };
            pnlMain.Controls.Add(lblUsernameError);
            lblUsernameError.BringToFront();

            // Tạo label thông báo lỗi cho password
            lblPasswordError = new Label
            {
                AutoSize = false,
                Size = new Size(300, 20),
                Location = new Point(51, 339), // Ngay dưới pnlPassword
                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(220, 38, 38), // Red-600
                Text = "",
                Visible = false
            };
            pnlMain.Controls.Add(lblPasswordError);
            lblPasswordError.BringToFront();

            // Thêm sự kiện TextChanged để xóa lỗi khi user nhập lại
            txtUsername.TextChanged += (s, e) => ClearUsernameError();
            txtPassword.TextChanged += (s, e) => ClearPasswordError();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            txtUsername.Select();
            Debug.WriteLine("=== LoginForm Loaded ===");
            Debug.WriteLine($"AuthService: {(_authService != null ? "OK" : "NULL")}");
            Debug.WriteLine($"DbContext: {(_context != null ? "OK" : "NULL")}");
        }

        // Kiểm tra định dạng email với đuôi phổ biến
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Pattern email chuẩn
                string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                if (!Regex.IsMatch(email, pattern))
                    return false;

                // Kiểm tra có đuôi email phổ biến không
                string lowerEmail = email.ToLower();
                foreach (var domain in _commonEmailDomains)
                {
                    if (lowerEmail.EndsWith(domain))
                        return true;
                }

                // Nếu không phải đuôi phổ biến nhưng có @, vẫn chấp nhận (có thể là email doanh nghiệp)
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Lấy gợi ý đuôi email phổ biến
        private string GetEmailSuggestion(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                return "";

            string[] parts = email.Split('@');
            if (parts.Length != 2)
                return "";

            string localPart = parts[0];
            string domainPart = parts[1].ToLower();

            // Tìm domain phổ biến gần giống nhất
            foreach (var commonDomain in _commonEmailDomains)
            {
                string domain = commonDomain.Substring(1); // Bỏ @
                if (domain.StartsWith(domainPart) ||
                    IsStringsSimilar(domain, domainPart))
                {
                    return $"{localPart}{commonDomain}";
                }
            }

            return "";
        }

        // So sánh độ tương đồng của 2 chuỗi
        private bool IsStringsSimilar(string s1, string s2)
        {
            if (s1.Length < 3 || s2.Length < 3)
                return false;

            // Kiểm tra xem s2 có chứa 3 ký tự đầu của s1 không
            string prefix = s1.Substring(0, Math.Min(3, s1.Length));
            return s2.StartsWith(prefix);
        }

        // Kiểm tra định dạng số điện thoại 
        private bool IsValidPhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            // Loại bỏ khoảng trắng và dấu gạch ngang
            phone = phone.Replace(" ", "").Replace("-", "");

            // - Bắt đầu bằng 0 hoặc +84
            // - Theo sau là 9 hoặc 10 số
            string pattern = @"^(0|\+84)(3|5|7|8|9)[0-9]{8}$";
            return Regex.IsMatch(phone, pattern);
        }

        private bool ValidateUsername()
        {
            string username = txtUsername.Text.Trim();

            if (string.IsNullOrWhiteSpace(username))
            {
                ShowUsernameError("Vui lòng nhập số điện thoại hoặc email!");
                return false;
            }

            // Kiểm tra xem là email hay SĐT
            bool isEmail = username.Contains("@");
            bool isValid = false;
            string errorMessage = "";

            if (isEmail)
            {
                isValid = IsValidEmail(username);
                if (!isValid)
                {
                    string suggestion = GetEmailSuggestion(username);
                    errorMessage = "Email không đúng định dạng!";

                    if (!string.IsNullOrEmpty(suggestion))
                    {
                        errorMessage += $" Ý bạn là: {suggestion}?";
                    }
                }
            }
            else
            {
                isValid = IsValidPhoneNumber(username);
                if (!isValid)
                    errorMessage = "Số điện thoại không đúng định dạng! (VD: 0912345678)";
            }

            if (!isValid)
            {
                ShowUsernameError(errorMessage);
                return false;
            }

            ClearUsernameError();
            return true;
        }

        private bool ValidatePassword()
        {
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(password))
            {
                ShowPasswordError("Vui lòng nhập mật khẩu!");
                return false;
            }

            if (password.Length < 6)
            {
                ShowPasswordError("Mật khẩu phải có ít nhất 6 ký tự!");
                return false;
            }

            if (password.Length > 50)
            {
                ShowPasswordError("Mật khẩu không được quá 50 ký tự!");
                return false;
            }

            ClearPasswordError();
            return true;
        }

        // Hiển thị lỗi validation cho username
        private void ShowUsernameError(string message)
        {
            lblUsernameError.Text = message;
            lblUsernameError.Visible = true;
            txtUsername.BackColor = Color.FromArgb(254, 242, 242); // Red-50

            // Tạo viền đỏ cho textbox
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
        }

        // Xóa thông báo lỗi username
        private void ClearUsernameError()
        {
            lblUsernameError.Visible = false;
            txtUsername.BackColor = Color.White;
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
        }

        // Hiển thị lỗi validation cho password
        private void ShowPasswordError(string message)
        {
            lblPasswordError.Text = message;
            lblPasswordError.Visible = true;
            pnlPassword.BackColor = Color.FromArgb(254, 242, 242); // Red-50
            pnlPassword.BorderStyle = BorderStyle.FixedSingle;

            // Thay đổi viền của panel thành màu đỏ
            pnlPassword.BackColor = Color.White;
            pnlPassword.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(220, 38, 38), 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, pnlPassword.Width - 1, pnlPassword.Height - 1);
                }
            };
            pnlPassword.Invalidate();
        }

        // Xóa thông báo lỗi password
        private void ClearPasswordError()
        {
            lblPasswordError.Visible = false;
            pnlPassword.BackColor = Color.White;
            pnlPassword.BorderStyle = BorderStyle.FixedSingle;

            // Xóa custom paint
            pnlPassword.Paint -= (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(220, 38, 38), 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, pnlPassword.Width - 1, pnlPassword.Height - 1);
                }
            };
            pnlPassword.Invalidate();
        }

        // Toggle password visibility
        private void BtnTogglePassword_Click(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !txtPassword.UseSystemPasswordChar;
            btnTogglePassword.Text = txtPassword.UseSystemPasswordChar ? "👁" : "🙈";
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            if (_isLoggingIn) return;

            try
            {
                _isLoggingIn = true;

                // Validate username và password trước
                bool isUsernameValid = ValidateUsername();
                bool isPasswordValid = ValidatePassword();

                if (!isUsernameValid)
                {
                    txtUsername.Focus();
                    return;
                }

                if (!isPasswordValid)
                {
                    txtPassword.Focus();
                    return;
                }

                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text;

                Debug.WriteLine("\n=== LOGIN ATTEMPT ===");
                Debug.WriteLine($"Username: {username}");
                Debug.WriteLine($"Password Length: {password.Length}");
                Debug.WriteLine($"Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

                SetLoadingState(true);

                try
                {
                    bool canConnect = await _context.Database.CanConnectAsync();
                    Debug.WriteLine($"Database Connection: {(canConnect ? "SUCCESS" : "FAILED")}");
                }
                catch (Exception dbEx)
                {
                    Debug.WriteLine($"Database Connection Error: {dbEx.Message}");
                }

                Debug.WriteLine("Calling AuthService.LoginAsync...");
                var result = await _authService.LoginAsync(username, password);

                Debug.WriteLine($"Login Result - Success: {result.Success}");
                Debug.WriteLine($"Login Result - Message: {result.Message}");
                Debug.WriteLine($"Login Result - UserType: {result.UserType}");

                SetLoadingState(false);

                if (!result.Success)
                {
                    Debug.WriteLine("LOGIN FAILED!");
                    Debug.WriteLine($"Failure Reason: {result.Message}");

                    MessageBox.Show(
                        result.Message,
                        "Đăng nhập thất bại",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    txtPassword.Clear();
                    txtPassword.Focus();
                    return;
                }

                Debug.WriteLine("LOGIN SUCCESS!");

                if (result.UserType == UserType.NhanVien)
                {
                    Debug.WriteLine("User Type: NHAN VIEN");
                    var nhanVien = result.NhanVien;
                    Debug.WriteLine($"NhanVien ID: {nhanVien.MaNv}");
                    Debug.WriteLine($"NhanVien Name: {nhanVien.TenNv}");
                    Debug.WriteLine($"NhanVien Role: {nhanVien.MaNhomNavigation?.TenNhom}");

                    var mainForm = Program.GetService<MainForm>();
                    mainForm.MaNV = nhanVien.MaNv;
                    mainForm.TenNV = nhanVien.TenNv;
                    mainForm.ChucVu = nhanVien.MaNhomNavigation?.TenNhom ?? "Nhân viên";

                    Debug.WriteLine("Opening MainForm...");
                    mainForm.Show();
                    mainForm.FormClosed += (s, args) => {
                        Debug.WriteLine("MainForm closed, showing LoginForm");
                        this.Show();
                        ResetForm();
                    };
                    this.Hide();
                    Debug.WriteLine("LoginForm hidden");
                }
                else if (result.UserType == UserType.KhachHang)
                {
                    Debug.WriteLine("User Type: KHACH HANG");
                    var khachHang = result.KhachHang;
                    Debug.WriteLine($"KhachHang ID: {khachHang.MaKh}");
                    Debug.WriteLine($"KhachHang Name: {khachHang.TenKh}");
                    Debug.WriteLine($"KhachHang Rank: {khachHang.HangTv}");

                    UserSession.MaKH = khachHang.MaKh;
                    UserSession.TenKH = khachHang.TenKh;
                    UserSession.Sdt = khachHang.Sdt;

                    var clientForm = Program.GetService<ClientMainForm>();
                    clientForm.Show();

                    MessageBox.Show(
                        $"✅ Chào mừng {khachHang.TenKh}!\n" +
                        $"🏆 Hạng thành viên: {khachHang.HangTv}\n" +
                        $"⭐ Điểm tích lũy: {khachHang.DiemTichLuy}",
                        "Đăng nhập thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    clientForm.FormClosed += (s, args) =>
                    {
                        UserSession.Logout();
                        this.Show();
                        ResetForm();
                        txtUsername.Focus();
                    };

                    this.Hide();
                    ResetForm();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\n=== EXCEPTION ===");
                Debug.WriteLine($"Message: {ex.Message}");
                Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }

                MessageBox.Show(
                    $"Lỗi: {ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                _isLoggingIn = false;
                SetLoadingState(false);
                Debug.WriteLine("=== LOGIN PROCESS END ===\n");
            }
        }

        private void SetLoadingState(bool isLoading)
        {
            btnLogin.Enabled = !isLoading;
            txtUsername.Enabled = !isLoading;
            txtPassword.Enabled = !isLoading;
            btnLogin.Text = isLoading ? "Đang đăng nhập..." : "Đăng nhập";
            this.Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
        }

        private void ResetForm()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtPassword.UseSystemPasswordChar = true;
            btnTogglePassword.Text = "👁";
            ClearUsernameError();
            ClearPasswordError();
            txtUsername.Focus();
            Debug.WriteLine("Form reset");
        }

        private void ShowError(string message, Control focusControl)
        {
            MessageBox.Show(message, "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            focusControl?.Focus();
        }

        private void LblForgotPassword_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Opening ForgotPasswordForm");
            this.Hide();

            var emailService = Program.GetService<EmailService>();
            var forgotForm = new ForgotPasswordForm(_context, _authService, emailService);

            forgotForm.FormClosed += (s, args) =>
            {
                this.Show();
                txtUsername.Focus();
            };

            forgotForm.Show();
        }

        private void LblSignup_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Opening SignupForm");
            this.Hide();
            var signupForm = new SignupForm(_context, _authService);
            var result = signupForm.ShowDialog();
            this.Show();
            txtUsername.Focus();

            if (result == DialogResult.OK)
            {
                Debug.WriteLine("Signup successful - returned to login");
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc muốn thoát?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Debug.WriteLine("Application Exit");
                Application.Exit();
            }
        }

        private void TxtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                BtnLogin_Click(sender, e);
            }
        }

        private void TxtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                if (ValidateUsername())
                {
                    txtPassword.Focus();
                }
            }
        }

        #region UI Effects
        private void BtnLogin_MouseEnter(object sender, EventArgs e)
        {
            if (btnLogin.Enabled)
                btnLogin.BackColor = Color.FromArgb(5, 150, 105);
        }

        private void BtnLogin_MouseLeave(object sender, EventArgs e)
        {
            if (btnLogin.Enabled)
                btnLogin.BackColor = Color.SeaGreen;
        }

        private void BtnClose_MouseEnter(object sender, EventArgs e)
        {
            btnClose.ForeColor = Color.Red;
            btnClose.BackColor = Color.FromArgb(254, 226, 226);
        }

        private void BtnClose_MouseLeave(object sender, EventArgs e)
        {
            btnClose.ForeColor = Color.Gray;
            btnClose.BackColor = Color.Transparent;
        }

        private void PnlMain_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        }
        #endregion
    }
}
