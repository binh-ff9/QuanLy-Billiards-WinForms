using Billiard.DAL.Data;
using Billiard.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.Auth
{
    public partial class LoginForm : Form
    {
        private readonly BilliardDbContext _context;
        private bool _isLoggingIn = false;
        private bool _isAdminMode = false;

        public LoginForm(BilliardDbContext context)
        {
            _context = context;
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            UpdateUIForMode();
        }

        private void UpdateUIForMode()
        {
            if (_isAdminMode)
            {
                lblTitle.Text = "🎱 QUẢN TRỊ HỆ THỐNG";
                lblSubtitle.Text = "Đăng nhập dành cho Admin & Nhân viên";
                lblUsername.Text = "Số điện thoại *";
                txtUsername.PlaceholderText = "Nhập số điện thoại";
                btnSwitchMode.Text = "🎮 Đăng nhập Khách hàng";
                lblSignup.Visible = false;
                pnlDecoration.BackColor = Color.FromArgb(99, 102, 241);
            }
            else
            {
                lblTitle.Text = "🎱 CHÀO MỪNG ĐẾN BIA CLUB";
                lblSubtitle.Text = "Đăng nhập để trải nghiệm dịch vụ tốt nhất";
                lblUsername.Text = "Số điện thoại / Email *";
                txtUsername.PlaceholderText = "Nhập SĐT hoặc Email";
                btnSwitchMode.Text = "👨‍💼 Đăng nhập Quản trị";
                lblSignup.Visible = true;
                pnlDecoration.BackColor = Color.FromArgb(16, 185, 129);
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            txtUsername.Select();
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            if (_isLoggingIn) return;

            try
            {
                _isLoggingIn = true;

                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    ShowError("Vui lòng nhập thông tin đăng nhập!", txtUsername);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    ShowError("Vui lòng nhập mật khẩu!", txtPassword);
                    return;
                }

                SetLoadingState(true);

                if (_isAdminMode)
                {
                    await PerformAdminLogin();
                }
                else
                {
                    await PerformCustomerLogin();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isLoggingIn = false;
                SetLoadingState(false);
            }
        }

        private async Task PerformAdminLogin()
        {
            var nhanVien = await _context.NhanViens
                .Include(nv => nv.MaNhomNavigation)
                .FirstOrDefaultAsync(nv =>
                    nv.Sdt == txtUsername.Text.Trim() &&
                    nv.MatKhau == txtPassword.Text &&
                    nv.TrangThai == "Đang làm");

            if (nhanVien == null)
            {
                MessageBox.Show(
                    "Thông tin đăng nhập không chính xác!\nHoặc tài khoản đã bị vô hiệu hóa.",
                    "Đăng nhập thất bại",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
                return;
            }

            // Log hoạt động
            await LogActivity(nhanVien.MaNv, $"Đăng nhập hệ thống từ {Environment.MachineName}");

            // Mở MainForm cho Admin/NV
            var mainForm = Program.GetService<MainForm>();
            mainForm.MaNV = nhanVien.MaNv;
            mainForm.TenNV = nhanVien.TenNv;
            mainForm.ChucVu = nhanVien.MaNhomNavigation?.TenNhom ?? "Nhân viên";

            MessageBox.Show($"Chào mừng {nhanVien.TenNv}!\nChức vụ: {mainForm.ChucVu}",
                "Đăng nhập thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

            mainForm.Show();
            mainForm.FormClosed += (s, args) => { this.Show(); ResetForm(); };
            this.Hide();
        }

        private async Task PerformCustomerLogin()
        {
            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(kh =>
                    (kh.Sdt == txtUsername.Text.Trim() || kh.Email == txtUsername.Text.Trim()) &&
                    kh.MatKhau == txtPassword.Text);

            if (khachHang == null)
            {
                MessageBox.Show(
                    "Thông tin đăng nhập không chính xác!",
                    "Đăng nhập thất bại",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
                return;
            }

            // Cập nhật thông tin khách hàng
            khachHang.LanDenCuoi = DateTime.Now;
            khachHang.HoatDong = true;
            await _context.SaveChangesAsync();

            // Mở CustomerMainForm
            MessageBox.Show($"Chào mừng {khachHang.TenKh}!\nHạng thành viên: {khachHang.HangTv}\nĐiểm tích lũy: {khachHang.DiemTichLuy}",
                "Đăng nhập thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // TODO: Mở form dành cho khách hàng
            // var customerForm = new CustomerMainForm(_context, khachHang.MaKh);
            // customerForm.Show();
            // this.Hide();
        }

        private async Task LogActivity(int maNv, string chiTiet)
        {
            try
            {
                var log = new LichSuHoatDong
                {
                    MaNv = maNv,
                    HanhDong = "Đăng nhập",
                    ChiTiet = chiTiet,
                    ThoiGian = DateTime.Now
                };
                _context.LichSuHoatDongs.Add(log);
                await _context.SaveChangesAsync();
            }
            catch { }
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
            chkRemember.Checked = false;
            txtUsername.Focus();
        }

        private void ShowError(string message, Control focusControl)
        {
            MessageBox.Show(message, "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            focusControl?.Focus();
        }

        private void BtnSwitchMode_Click(object sender, EventArgs e)
        {
            _isAdminMode = !_isAdminMode;
            UpdateUIForMode();
            ResetForm();
        }

        private void LblForgotPassword_Click(object sender, EventArgs e)
        {
            var forgotForm = new ForgotPasswordForm(_context, _isAdminMode);
            forgotForm.ShowDialog();
        }

        private void LblSignup_Click(object sender, EventArgs e)
        {
            if (_isAdminMode)
            {
                MessageBox.Show("Tài khoản quản trị chỉ được tạo bởi Admin!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var signupForm = new SignupForm(_context);
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
                Application.Exit();
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
                btnLogin.BackColor = _isAdminMode ?
                    Color.FromArgb(79, 70, 229) : Color.FromArgb(5, 150, 105);
        }

        private void BtnLogin_MouseLeave(object sender, EventArgs e)
        {
            if (btnLogin.Enabled)
                btnLogin.BackColor = _isAdminMode ?
                    Color.FromArgb(99, 102, 241) : Color.FromArgb(16, 185, 129);
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