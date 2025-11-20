using Billiard.BLL.Services;
using Billiard.DAL.Data;
using Billiard.WinForm.Helpers;
using Billiard.WinForm.Forms.Helpers;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
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

        public LoginForm(BilliardDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            lblTitle.Text = "🎱 CHÀO MỪNG ĐẾN BIA CLUB";
            lblSubtitle.Text = "Đăng nhập để trải nghiệm dịch vụ tốt nhất";
            lblUsername.Text = "📱 Số điện thoại / Email *";
            txtUsername.PlaceholderText = "Nhập SĐT hoặc Email";
            lblSignup.Visible = true;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            txtUsername.Select();

            // Debug log
            Debug.WriteLine("=== LoginForm Loaded ===");
            Debug.WriteLine($"AuthService: {(_authService != null ? "OK" : "NULL")}");
            Debug.WriteLine($"DbContext: {(_context != null ? "OK" : "NULL")}");
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            if (_isLoggingIn) return;

            try
            {
                _isLoggingIn = true;

                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text;

                Debug.WriteLine("\n=== LOGIN ATTEMPT ===");
                Debug.WriteLine($"Username: {username}");
                Debug.WriteLine($"Password Length: {password.Length}");
                Debug.WriteLine($"Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

                // Validate inputs
                if (string.IsNullOrWhiteSpace(username))
                {
                    Debug.WriteLine("ERROR: Username is empty");
                    ShowError("Vui lòng nhập thông tin đăng nhập!", txtUsername);
                    return;
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    Debug.WriteLine("ERROR: Password is empty");
                    ShowError("Vui lòng nhập mật khẩu!", txtPassword);
                    return;
                }

                SetLoadingState(true);

                // Debug: Test database connection
                try
                {
                    bool canConnect = await _context.Database.CanConnectAsync();
                    Debug.WriteLine($"Database Connection: {(canConnect ? "SUCCESS" : "FAILED")}");
                }
                catch (Exception dbEx)
                {
                    Debug.WriteLine($"Database Connection Error: {dbEx.Message}");
                }

                
                // Use AuthService to login
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
                        result.Message + "\n\n" +
                        "🔍 Debug Info:\n" +
                        $"• Username: {username}\n" +
                        $"• Kiểm tra Console để xem chi tiết",
                        "Đăng nhập thất bại",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    txtPassword.Clear();
                    txtPassword.Focus();
                    return;
                }

                Debug.WriteLine("LOGIN SUCCESS!");

                // Check user type and open appropriate form
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
                        UserSession.Logout(); // Xóa session
                        this.Show();          // Hiện lại form đăng nhập
                        ResetForm();
                        txtUsername.Focus();
                    };

                    this.Hide(); // Ẩn form đăng nhập đi


                    //// TODO: Open CustomerMainForm when implemented
                    //Debug.WriteLine("WARNING: CustomerMainForm not implemented yet");
                    //MessageBox.Show(
                    //    "Giao diện khách hàng đang được phát triển.\n" +
                    //    "Vui lòng sử dụng tài khoản nhân viên để truy cập hệ thống.",
                    //    "Thông báo",
                    //    MessageBoxButtons.OK,
                    //    MessageBoxIcon.Information);

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
                    $"Lỗi: {ex.Message}\n\n" +
                    $"Chi tiết:\n{ex.StackTrace}\n\n" +
                    $"Kiểm tra Output Window để xem log đầy đủ",
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
            btnLogin.Text = isLoading ? "⏳ Đang đăng nhập..." : "✅ Đăng nhập";
            this.Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
        }

        private void ResetForm()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            chkRemember.Checked = false;
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
            var forgotForm = new ForgotPasswordForm(_context, _authService);
            forgotForm.ShowDialog();
        }

        private void LblSignup_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Opening SignupForm");
            var signupForm = new SignupForm(_context, _authService);
            if (signupForm.ShowDialog() == DialogResult.OK)
            {
                txtUsername.Focus();
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
                txtPassword.Focus();
            }
        }

        private void ChkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
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