using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Billiard.DAL.Data;

namespace Billiard.WinForm.Forms.Auth
{
    public partial class ResetPasswordForm : Form
    {
        private readonly BilliardDbContext _context;
        private readonly string _email;
        private readonly string _correctOTP;
        private DateTime _otpExpiration;

        public ResetPasswordForm(BilliardDbContext context, string email, string otp)
        {
            _context = context;
            _email = email;
            _correctOTP = otp;
            _otpExpiration = DateTime.Now.AddMinutes(5); // OTP expires in 5 minutes
            InitializeComponent();
        }

        private void ResetPasswordForm_Load(object sender, EventArgs e)
        {
            lblEmailDisplay.Text = $"Email: {_email}";
            txtOTP.Focus();
        }

        private async void BtnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate OTP
                if (string.IsNullOrWhiteSpace(txtOTP.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã OTP!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtOTP.Focus();
                    return;
                }

                // Check OTP expiration
                if (DateTime.Now > _otpExpiration)
                {
                    MessageBox.Show("Mã OTP đã hết hạn!\nVui lòng yêu cầu gửi lại mã mới.",
                        "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Verify OTP
                if (txtOTP.Text.Trim() != _correctOTP)
                {
                    MessageBox.Show("Mã OTP không chính xác!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtOTP.Clear();
                    txtOTP.Focus();
                    return;
                }

                // Validate new password
                if (string.IsNullOrWhiteSpace(txtNewPassword.Text))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu mới!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNewPassword.Focus();
                    return;
                }

                if (txtNewPassword.Text.Length < 6)
                {
                    MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNewPassword.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
                {
                    MessageBox.Show("Vui lòng xác nhận mật khẩu!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtConfirmPassword.Focus();
                    return;
                }

                if (txtNewPassword.Text != txtConfirmPassword.Text)
                {
                    MessageBox.Show("Mật khẩu xác nhận không khớp!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtConfirmPassword.Clear();
                    txtConfirmPassword.Focus();
                    return;
                }

                // Show loading
                btnResetPassword.Enabled = false;
                btnResetPassword.Text = "Đang xử lý...";
                this.Cursor = Cursors.WaitCursor;

                // Update password in database
                var nhanVien = await _context.NhanViens
                    .FirstOrDefaultAsync(nv => nv.Email == _email);

                if (nhanVien != null)
                {
                    nhanVien.MatKhau = txtNewPassword.Text; // In production, hash this password!
                    await _context.SaveChangesAsync();

                    this.Cursor = Cursors.Default;

                    MessageBox.Show(
                        "Đặt lại mật khẩu thành công!\nBạn có thể đăng nhập với mật khẩu mới.",
                        "Thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("Không tìm thấy tài khoản!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                btnResetPassword.Enabled = true;
                btnResetPassword.Text = "Đặt lại Mật khẩu";
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                btnResetPassword.Enabled = true;
                btnResetPassword.Text = "Đặt lại Mật khẩu";
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void ChkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtNewPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
            txtConfirmPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        private void TxtConfirmPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnResetPassword_Click(sender, e);
                e.Handled = true;
            }
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
    }
}