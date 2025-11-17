using Billiard.DAL.Data;
using Billiard.DAL.Entities;
using Billiard.WinForm.Forms.Auth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Billiard.WinForm
{
    public partial class LoginForm : Form
    {
        private readonly BilliardDbContext _context;
        private bool _isLoggingIn = false;

        public LoginForm(BilliardDbContext context)
        {
            _context = context;
            InitializeComponent();

            // Initialize UI
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Set form start position
            this.StartPosition = FormStartPosition.CenterScreen;

            // Set focus to username textbox after form is shown
            this.Shown += (s, e) => txtUsername.Focus();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // Additional initialization if needed
            txtUsername.Select();
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            // Prevent multiple simultaneous login attempts
            if (_isLoggingIn) return;

            try
            {
                _isLoggingIn = true;

                // Validate input
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    ShowError("Vui lòng nhập số điện thoại!", txtUsername);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    ShowError("Vui lòng nhập mật khẩu!", txtPassword);
                    return;
                }

                // Show loading state
                SetLoadingState(true);

                // Perform login
                await PerformLogin();
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Lỗi kết nối cơ sở dữ liệu:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isLoggingIn = false;
                SetLoadingState(false);
            }
        }

        private async Task PerformLogin()
        {
            try
            {
                // Query database
                var nhanVien = await _context.NhanViens
                    .Include(nv => nv.MaNhomNavigation)
                    .FirstOrDefaultAsync(nv =>
                        nv.Sdt == txtUsername.Text.Trim() &&
                        nv.MatKhau == txtPassword.Text &&
                        nv.TrangThai == "Đang làm");

                if (nhanVien == null)
                {
                    MessageBox.Show(
                        "Số điện thoại hoặc mật khẩu không đúng!\nHoặc tài khoản đã bị vô hiệu hóa.",
                        "Đăng nhập thất bại",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    txtPassword.Clear();
                    txtPassword.Focus();
                    return;
                }

                // Log activity
                await LogActivity(nhanVien);

                // Create main form
                var mainForm = Program.GetService<MainForm>();
                if (mainForm == null)
                {
                    throw new Exception("Không thể khởi tạo MainForm");
                }

                // Set session data
                mainForm.MaNV = nhanVien.MaNv;
                mainForm.TenNV = nhanVien.TenNv;
                mainForm.ChucVu = nhanVien.MaNhomNavigation?.TenNhom ?? "Nhân viên";

                // Show main form
                mainForm.Show();

                // Setup form closed event
                mainForm.FormClosed += (s, args) =>
                {
                    // Show login form again when main form closes
                    this.Show();
                    ResetForm();
                };
                this.Hide();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi trong quá trình đăng nhập: {ex.Message}", ex);
            }
        }

        private async Task LogActivity(NhanVien nhanVien)
        {
            try
            {
                var logEntry = new LichSuHoatDong
                {
                    MaNv = nhanVien.MaNv,
                    HanhDong = "Đăng nhập hệ thống",
                    ChiTiet = $"Đăng nhập từ máy {Environment.MachineName}",
                    ThoiGian = DateTime.Now
                };
                _context.LichSuHoatDongs.Add(logEntry);
                await _context.SaveChangesAsync();
            }
            catch
            {
                // Ignore logging errors
            }
        }

        private void SetLoadingState(bool isLoading)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => SetLoadingState(isLoading)));
                return;
            }

            btnLogin.Enabled = !isLoading;
            txtUsername.Enabled = !isLoading;
            txtPassword.Enabled = !isLoading;

            if (isLoading)
            {
                btnLogin.Text = "Đang đăng nhập...";
                this.Cursor = Cursors.WaitCursor;
            }
            else
            {
                btnLogin.Text = "Đăng nhập";
                this.Cursor = Cursors.Default;
            }
        }

        private void ResetForm()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            chkRemember.Checked = false;
            txtUsername.Focus();
        }

        private void ShowError(string message, Control focusControl)
        {
            MessageBox.Show(message, "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            focusControl?.Focus();
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

        #region UI Events

        private void BtnLogin_MouseEnter(object sender, EventArgs e)
        {
            if (btnLogin.Enabled)
                btnLogin.BackColor = Color.FromArgb(79, 70, 229);
        }

        private void BtnLogin_MouseLeave(object sender, EventArgs e)
        {
            if (btnLogin.Enabled)
                btnLogin.BackColor = Color.FromArgb(99, 102, 241);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Bạn có chắc muốn thoát ứng dụng?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
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

        private void LblForgotPassword_Click(object sender, EventArgs e)
        {
            try
            {
                var forgotPasswordForm = Program.GetService<ForgotPasswordForm>();
                forgotPasswordForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LblForgotPassword_MouseEnter(object sender, EventArgs e)
        {
            lblForgotPassword.ForeColor = Color.FromArgb(79, 70, 229);
        }

        private void LblForgotPassword_MouseLeave(object sender, EventArgs e)
        {
            lblForgotPassword.ForeColor = Color.FromArgb(99, 102, 241);
        }

        private void LblSignup_Click(object sender, EventArgs e)
        {
            try
            {
                var signupForm = Program.GetService<SignupForm>();
                var result = signupForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    txtUsername.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LblSignup_MouseEnter(object sender, EventArgs e)
        {
            lblSignup.ForeColor = Color.FromArgb(79, 70, 229);
        }

        private void LblSignup_MouseLeave(object sender, EventArgs e)
        {
            lblSignup.ForeColor = Color.FromArgb(99, 102, 241);
        }

        private void PnlMain_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var path = GetRoundedRectangle(pnlMain.ClientRectangle, 12);
            pnlMain.Region = new Region(path);
        }

        private System.Drawing.Drawing2D.GraphicsPath GetRoundedRectangle(Rectangle rect, int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }

        #endregion

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && this.Visible)
            {
                var result = MessageBox.Show(
                    "Bạn có chắc muốn thoát ứng dụng?",
                    "Xác nhận thoát",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    Application.Exit();
                }
            }
            base.OnFormClosing(e);
        }
    }
}