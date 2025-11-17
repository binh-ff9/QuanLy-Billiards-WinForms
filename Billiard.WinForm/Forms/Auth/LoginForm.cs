using Billiard.DAL.Data;
using Billiard.DAL.Entities;
using Billiard.WinForm.Forms.Auth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Billiard.WinForm
{
    public partial class LoginForm : Form
    {
        private readonly BilliardDbContext _context;

        public LoginForm(BilliardDbContext context)
        {
            _context = context;
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // Focus vào textbox username
            txtUsername.Focus();
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsername.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Focus();
                    return;
                }

                // Disable button and show loading
                btnLogin.Enabled = false;
                btnLogin.Text = "Đang đăng nhập...";
                this.Cursor = Cursors.WaitCursor;

                // Query database
                var nhanVien = await _context.NhanViens
                    .Include(nv => nv.MaNhomNavigation)
                    .FirstOrDefaultAsync(nv =>
                        nv.Sdt == txtUsername.Text.Trim() &&
                        nv.MatKhau == txtPassword.Text &&
                        nv.TrangThai == "Đang làm");

                this.Cursor = Cursors.Default;

                if (nhanVien == null)
                {
                    MessageBox.Show(
                        "Số điện thoại hoặc mật khẩu không đúng!\nHoặc tài khoản đã bị vô hiệu hóa.",
                        "Đăng nhập thất bại",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    btnLogin.Enabled = true;
                    btnLogin.Text = "Đăng nhập";
                    txtPassword.Clear();
                    txtPassword.Focus();
                    return;
                }

                // Log activity
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

                // Successful login - create and show main form
                var mainForm = Program.GetService<MainForm>();
                mainForm.MaNV = nhanVien.MaNv;
                mainForm.TenNV = nhanVien.TenNv;
                mainForm.ChucVu = nhanVien.MaNhomNavigation?.TenNhom ?? "Nhân viên";

                // Show main form and hide login
                mainForm.Show();
                this.Hide();

                // When main form closes, close the application
                mainForm.FormClosed += (s, args) => Application.Exit();

                // Reset form for next login
                txtUsername.Clear();
                txtPassword.Clear();
                btnLogin.Enabled = true;
                btnLogin.Text = "Đăng nhập";
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Lỗi kết nối cơ sở dữ liệu:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                btnLogin.Enabled = true;
                btnLogin.Text = "Đăng nhập";
            }
        }

        private void TxtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnLogin_Click(sender, e);
                e.Handled = true;
            }
        }

        private void BtnLogin_MouseEnter(object sender, EventArgs e)
        {
            btnLogin.BackColor = Color.FromArgb(79, 70, 229);
        }

        private void BtnLogin_MouseLeave(object sender, EventArgs e)
        {
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
            // Open Forgot Password Form
            var forgotPasswordForm = Program.GetService<ForgotPasswordForm>();
            forgotPasswordForm.ShowDialog();
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
            // Open Signup Form
            var signupForm = Program.GetService<SignupForm>();
            var result = signupForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                // User successfully registered, can auto-focus on login form
                txtUsername.Focus();
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
            // Draw rounded corners for main panel
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
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