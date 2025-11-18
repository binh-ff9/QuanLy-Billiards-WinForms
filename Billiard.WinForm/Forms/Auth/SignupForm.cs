using Billiard.DAL.Data;
using Billiard.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.Auth
{
    public partial class SignupForm : Form
    {
        private readonly BilliardDbContext _context;

        public SignupForm(BilliardDbContext context)
        {
            _context = context;
            InitializeComponent();
        }

        private void SignupForm_Load(object sender, EventArgs e)
        {
            txtTenKH.Focus();
        }

        private async void BtnSignup_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(txtTenKH.Text))
                {
                    ShowError("Vui lòng nhập họ và tên!", txtTenKH);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtSDT.Text))
                {
                    ShowError("Vui lòng nhập số điện thoại!", txtSDT);
                    return;
                }

                if (txtSDT.Text.Length < 10)
                {
                    ShowError("Số điện thoại không hợp lệ!", txtSDT);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
                {
                    ShowError("Vui lòng nhập email hợp lệ!", txtEmail);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
                {
                    ShowError("Vui lòng nhập mật khẩu!", txtMatKhau);
                    return;
                }

                if (txtMatKhau.Text.Length < 6)
                {
                    ShowError("Mật khẩu phải có ít nhất 6 ký tự!", txtMatKhau);
                    return;
                }

                if (txtMatKhau.Text != txtXacNhanMatKhau.Text)
                {
                    ShowError("Mật khẩu xác nhận không khớp!", txtXacNhanMatKhau);
                    txtXacNhanMatKhau.Clear();
                    txtXacNhanMatKhau.Focus();
                    return;
                }

                // Show loading
                SetLoadingState(true);

                // Check existing phone
                var existingPhone = await _context.KhachHangs
                    .AnyAsync(kh => kh.Sdt == txtSDT.Text.Trim());

                if (existingPhone)
                {
                    SetLoadingState(false);
                    ShowError("Số điện thoại này đã được đăng ký!", txtSDT);
                    return;
                }

                // Check existing email
                var existingEmail = await _context.KhachHangs
                    .AnyAsync(kh => kh.Email == txtEmail.Text.Trim());

                if (existingEmail)
                {
                    SetLoadingState(false);
                    ShowError("Email này đã được đăng ký!", txtEmail);
                    return;
                }

                // Create new customer
                var khachHang = new KhachHang
                {
                    TenKh = txtTenKH.Text.Trim(),
                    Sdt = txtSDT.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    MatKhau = txtMatKhau.Text, // TODO: Hash password
                    NgaySinh = dtpNgaySinh.Value.Date > DateTime.Now.Date ? null : DateOnly.FromDateTime(dtpNgaySinh.Value),
                    HangTv = "Đồng",
                    DiemTichLuy = 0,
                    TongChiTieu = 0,
                    NgayDangKy = DateTime.Now,
                    HoatDong = true
                };

                _context.KhachHangs.Add(khachHang);
                await _context.SaveChangesAsync();

                SetLoadingState(false);

                MessageBox.Show(
                    $"🎉 Chào mừng {khachHang.TenKh}!\n\n" +
                    $"Đăng ký thành công với thông tin:\n" +
                    $"📱 SĐT: {khachHang.Sdt}\n" +
                    $"📧 Email: {khachHang.Email}\n" +
                    $"🏆 Hạng: {khachHang.HangTv}\n" +
                    $"⭐ Điểm: {khachHang.DiemTichLuy}\n\n" +
                    $"Bạn có thể đăng nhập ngay bây giờ!",
                    "Đăng ký thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                SetLoadingState(false);
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetLoadingState(bool isLoading)
        {
            btnSignup.Enabled = !isLoading;
            btnSignup.Text = isLoading ? "⏳ Đang xử lý..." : "✅ Đăng ký";
            this.Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
        }

        private void ShowError(string message, Control focusControl)
        {
            MessageBox.Show(message, "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            focusControl?.Focus();
        }

        private bool IsValidEmail(string email)
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

        private void BtnBackToLogin_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ChkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtMatKhau.UseSystemPasswordChar = !chkShowPassword.Checked;
            txtXacNhanMatKhau.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        private void TxtXacNhanMatKhau_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnSignup_Click(sender, e);
                e.Handled = true;
            }
        }

        #region UI Effects
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