using Billiard.BLL.Services;
using Billiard.DAL.Data;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.Auth
{
    public partial class SignupForm : Form
    {
        private readonly BilliardDbContext _context;
        private readonly AuthService _authService;

        public SignupForm(BilliardDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
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

                if (string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    ShowError("Vui lòng nhập email!", txtEmail);
                    return;
                }

                if (!_authService.IsValidEmail(txtEmail.Text))
                {
                    ShowError("Email không hợp lệ!", txtEmail);
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

                // Use AuthService to register customer
                var ngaySinh = dtpNgaySinh.Value.Date > DateTime.Now.Date
                    ? (DateOnly?)null
                    : DateOnly.FromDateTime(dtpNgaySinh.Value);

                var (success, message, customer) = await _authService.RegisterCustomerAsync(
                    txtTenKH.Text.Trim(),
                    txtSDT.Text.Trim(),
                    txtEmail.Text.Trim(),
                    txtMatKhau.Text,
                    ngaySinh
                );

                SetLoadingState(false);

                if (!success)
                {
                    ShowError(message, null);
                    return;
                }

                MessageBox.Show(
                    $"🎉 Chào mừng {customer.TenKh}!\n\n" +
                    $"Đăng ký thành công với thông tin:\n" +
                    $"📱 SĐT: {customer.Sdt}\n" +
                    $"📧 Email: {customer.Email}\n" +
                    $"🏆 Hạng: {customer.HangTv}\n" +
                    $"⭐ Điểm: {customer.DiemTichLuy}\n\n" +
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