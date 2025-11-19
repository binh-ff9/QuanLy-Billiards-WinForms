using Billiard.BLL.Services;
using Billiard.DAL.Data;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.Auth
{
    public partial class ResetPasswordForm : Form
    {
        private readonly BilliardDbContext _context;
        private readonly AuthService _authService;
        private readonly string _email;
        private readonly string _correctOTP;
        private readonly bool _isAdminMode;
        private DateTime _otpExpiration;
        private System.Windows.Forms.Timer _countdownTimer;
        private int _remainingSeconds;

        public ResetPasswordForm(
            BilliardDbContext context,
            AuthService authService,
            string email,
            string otp,
            bool isAdminMode = false)
        {
            _context = context;
            _authService = authService;
            _email = email;
            _correctOTP = otp;
            _isAdminMode = isAdminMode;
            _otpExpiration = DateTime.Now.AddMinutes(5); // OTP expires in 5 minutes
            _remainingSeconds = 300; // 5 minutes = 300 seconds

            InitializeComponent();
            InitializeTimer();
            UpdateUIForMode();
        }

        private void UpdateUIForMode()
        {
            if (_isAdminMode)
            {
                lblTitle.Text = "🔐 ĐẶT LẠI MẬT KHẨU (Nhân viên)";
                pnlDecoration.BackColor = Color.FromArgb(99, 102, 241);
                btnResetPassword.BackColor = Color.FromArgb(99, 102, 241);
            }
            else
            {
                lblTitle.Text = "🔐 ĐẶT LẠI MẬT KHẨU";
                pnlDecoration.BackColor = Color.FromArgb(16, 185, 129);
                btnResetPassword.BackColor = Color.FromArgb(16, 185, 129);
            }
        }

        private void InitializeTimer()
        {
            _countdownTimer = new System.Windows.Forms.Timer();
            _countdownTimer.Interval = 1000; // 1 second
            _countdownTimer.Tick += CountdownTimer_Tick;
            _countdownTimer.Start();
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            _remainingSeconds--;
            UpdateCountdownDisplay();

            if (_remainingSeconds <= 0)
            {
                _countdownTimer.Stop();
                MessageBox.Show(
                    "⏰ Mã OTP đã hết hạn!\n\nVui lòng yêu cầu gửi lại mã mới.",
                    "Hết thời gian",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                this.Close();
            }
        }

        private void UpdateCountdownDisplay()
        {
            int minutes = _remainingSeconds / 60;
            int seconds = _remainingSeconds % 60;
            lblCountdown.Text = $"⏱️ Thời gian còn lại: {minutes:D2}:{seconds:D2}";

            // Change color based on remaining time
            if (_remainingSeconds <= 60)
            {
                lblCountdown.ForeColor = Color.FromArgb(239, 68, 68); // Red
            }
            else if (_remainingSeconds <= 120)
            {
                lblCountdown.ForeColor = Color.FromArgb(245, 158, 11); // Orange
            }
            else
            {
                lblCountdown.ForeColor = Color.FromArgb(100, 116, 139); // Gray
            }
        }

        private void ResetPasswordForm_Load(object sender, EventArgs e)
        {
            lblEmailDisplay.Text = $"📧 {_email}";
            UpdateCountdownDisplay();
            txtOTP.Focus();
        }

        private async void BtnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate OTP
                if (string.IsNullOrWhiteSpace(txtOTP.Text))
                {
                    ShowError("Vui lòng nhập mã OTP!", txtOTP);
                    return;
                }

                if (txtOTP.Text.Length != 6)
                {
                    ShowError("Mã OTP phải có 6 chữ số!", txtOTP);
                    return;
                }

                // Check OTP expiration
                if (DateTime.Now > _otpExpiration)
                {
                    ShowError("Mã OTP đã hết hạn!\nVui lòng yêu cầu gửi lại mã mới.", txtOTP);
                    return;
                }

                // Verify OTP
                if (txtOTP.Text.Trim() != _correctOTP)
                {
                    ShowError("❌ Mã OTP không chính xác!", txtOTP);
                    txtOTP.Clear();
                    txtOTP.Focus();
                    return;
                }

                // Validate new password
                if (string.IsNullOrWhiteSpace(txtNewPassword.Text))
                {
                    ShowError("Vui lòng nhập mật khẩu mới!", txtNewPassword);
                    return;
                }

                if (txtNewPassword.Text.Length < 6)
                {
                    ShowError("Mật khẩu phải có ít nhất 6 ký tự!", txtNewPassword);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
                {
                    ShowError("Vui lòng xác nhận mật khẩu!", txtConfirmPassword);
                    return;
                }

                if (txtNewPassword.Text != txtConfirmPassword.Text)
                {
                    ShowError("Mật khẩu xác nhận không khớp!", txtConfirmPassword);
                    txtConfirmPassword.Clear();
                    txtConfirmPassword.Focus();
                    return;
                }

                // Show loading
                SetLoadingState(true);

                // Use AuthService to reset password (auto-detect user type)
                bool success = await _authService.ResetPasswordAsync(_email, txtNewPassword.Text);

                SetLoadingState(false);

                if (success)
                {
                    _countdownTimer.Stop();

                    MessageBox.Show(
                        "✅ Đặt lại mật khẩu thành công!\n\n" +
                        "Bạn có thể đăng nhập ngay bây giờ với mật khẩu mới.",
                        "Thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    ShowError("Không tìm thấy tài khoản!", null);
                }
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
            btnResetPassword.Enabled = !isLoading;
            btnResetPassword.Text = isLoading ? "⏳ Đang xử lý..." : "✅ Đặt lại mật khẩu";
            this.Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
        }

        private void ShowError(string message, Control focusControl)
        {
            MessageBox.Show(message, "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            focusControl?.Focus();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Bạn có chắc muốn hủy?\nTiến trình sẽ không được lưu.",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _countdownTimer.Stop();
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            BtnCancel_Click(sender, e);
        }

        private void ChkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtNewPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
            txtConfirmPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        private void TxtOTP_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Only allow digits
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Enter)
            {
                txtNewPassword.Focus();
                e.Handled = true;
            }
        }

        private void TxtConfirmPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnResetPassword_Click(sender, e);
                e.Handled = true;
            }
        }

        private void LblResendOTP_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Gửi lại mã OTP mới?\n\nMã OTP hiện tại sẽ không còn hiệu lực.",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _countdownTimer.Stop();
                this.Close();

                // Reopen ForgotPasswordForm
                var forgotForm = new ForgotPasswordForm(_context, _authService);
                forgotForm.ShowDialog();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _countdownTimer?.Stop();
            _countdownTimer?.Dispose();
            base.OnFormClosing(e);
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

        private void LblResendOTP_MouseEnter(object sender, EventArgs e)
        {
            lblResendOTP.ForeColor = _isAdminMode ?
                Color.FromArgb(79, 70, 229) : Color.FromArgb(5, 150, 105);
        }

        private void LblResendOTP_MouseLeave(object sender, EventArgs e)
        {
            lblResendOTP.ForeColor = _isAdminMode ?
                Color.FromArgb(99, 102, 241) : Color.FromArgb(16, 185, 129);
        }

        private void PnlMain_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        }
        #endregion
    }
}