using Billiard.BLL.Services;
using Billiard.DAL.Data;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
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

        // Label để hiển thị lỗi validation
        private Label lblOTPError;
        private Label lblNewPasswordError;
        private Label lblConfirmPasswordError;

        public ResetPasswordForm(
            BilliardDbContext context,
            AuthService authService,
            string email,
            string otp,
            bool isAdminMode = false)
        {
            InitializeComponent();

            _context = context;
            _authService = authService;
            _email = email;
            _correctOTP = otp;
            _isAdminMode = isAdminMode;
            _otpExpiration = DateTime.Now.AddMinutes(5);
            _remainingSeconds = 300;

            UpdateUIForMode();
            AdjustControlPositions();
            InitializeValidationLabels();
            InitializeTimer();
        }

        private void UpdateUIForMode()
        {
            if (_isAdminMode)
            {
                lblTitle.Text = "ĐẶT LẠI MẬT KHẨU (Nhân viên)";
            }
            else
            {
                lblTitle.Text = "ĐẶT LẠI MẬT KHẨU";
            }
        }

        private void AdjustControlPositions()
        {
            // Điều chỉnh vị trí các control để có đủ khoảng trống cho error messages
            // Mỗi error label cần khoảng 22px

            // OTP section - giữ nguyên
            lblOTP.Location = new Point(50, 131);
            pnlOTPInput.Location = new Point(50, 161);
            // Error label sẽ ở: 161 + 32 + 3 = 196

            // New Password section - dịch xuống 22px
            lblNewPassword.Location = new Point(50, 221);
            pnlNewPassword.Location = new Point(50, 251);
            // Error label sẽ ở: 251 + 42 + 3 = 296

            // Confirm Password section - dịch xuống 44px (22*2)
            lblConfirmPassword.Location = new Point(50, 320);
            pnlConfirmPassword.Location = new Point(50, 350);
            // Error label sẽ ở: 350 + 42 + 3 = 395

            // Buttons section - dịch xuống 66px (22*3)
            btnResetPassword.Location = new Point(50, 417);
            btnCancel.Location = new Point(50, 478);

            // Footer section - dịch xuống 66px
            lblCountdown.Location = new Point(47, 530);
            lblResendOTP.Location = new Point(256, 530);
            lblBackToLogin.Location = new Point(50, 562);
        }

        private void InitializeValidationLabels()
        {
            // Label lỗi cho OTP - ngay dưới pnlOTPInput
            lblOTPError = new Label
            {
                AutoSize = false,
                Size = new Size(300, 22),
                Location = new Point(50, 196),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(220, 38, 38),
                Text = "",
                Visible = false
            };
            pnlMain.Controls.Add(lblOTPError);
            lblOTPError.BringToFront();

            // Label lỗi cho New Password - ngay dưới pnlNewPassword
            lblNewPasswordError = new Label
            {
                AutoSize = false,
                Size = new Size(300, 22),
                Location = new Point(50, 296),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(220, 38, 38),
                Text = "",
                Visible = false
            };
            pnlMain.Controls.Add(lblNewPasswordError);
            lblNewPasswordError.BringToFront();

            // Label lỗi cho Confirm Password - ngay dưới pnlConfirmPassword
            lblConfirmPasswordError = new Label
            {
                AutoSize = false,
                Size = new Size(300, 22),
                Location = new Point(50, 395),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(220, 38, 38),
                Text = "",
                Visible = false
            };
            pnlMain.Controls.Add(lblConfirmPasswordError);
            lblConfirmPasswordError.BringToFront();

            // Thêm sự kiện TextChanged để xóa lỗi khi user nhập lại
            txtOTP.TextChanged += (s, e) => ClearOTPError();
            txtNewPassword.TextChanged += (s, e) => {
                ClearNewPasswordError();
                // Validate realtime confirm password nếu đã nhập
                if (!string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
                {
                    ValidateConfirmPasswordRealtime();
                }
            };
            txtConfirmPassword.TextChanged += (s, e) => {
                ValidateConfirmPasswordRealtime();
            };
        }

        private void InitializeTimer()
        {
            _countdownTimer = new System.Windows.Forms.Timer();
            _countdownTimer.Interval = 1000;
            _countdownTimer.Tick += CountdownTimer_Tick;
            _countdownTimer.Start();
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                _remainingSeconds--;
                UpdateCountdownDisplay();

                if (_remainingSeconds <= 0)
                {
                    _countdownTimer.Stop();
                    MessageBox.Show(
                        "Mã OTP đã hết hạn!\n\nVui lòng yêu cầu gửi lại mã mới.",
                        "Hết thời gian",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Timer error: {ex.Message}");
            }
        }

        private void UpdateCountdownDisplay()
        {
            try
            {
                int mins = _remainingSeconds / 60;
                int secs = _remainingSeconds % 60;
                string timeText = $"Thời gian còn lại: {mins:D2}:{secs:D2}";

                if (lblCountdown != null && !lblCountdown.IsDisposed)
                {
                    if (lblCountdown.InvokeRequired)
                    {
                        lblCountdown.Invoke(new Action(() => {
                            lblCountdown.Text = timeText;
                        }));
                    }
                    else
                    {
                        lblCountdown.Text = timeText;
                    }
                }

                if (_remainingSeconds <= 60)
                    lblCountdown.ForeColor = Color.FromArgb(239, 68, 68);
                else if (_remainingSeconds <= 120)
                    lblCountdown.ForeColor = Color.FromArgb(245, 158, 11);
                else
                    lblCountdown.ForeColor = Color.FromArgb(100, 116, 139);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Update display error: {ex.Message}");
            }
        }

        private void ResetPasswordForm_Load(object sender, EventArgs e)
        {
            lblEmailDisplay.Text = $"{_email}";
            UpdateCountdownDisplay();
            txtOTP.Focus();
        }

        // Validation methods - giống LoginForm
        private bool ValidateOTP()
        {
            string otp = txtOTP.Text.Trim();

            if (string.IsNullOrWhiteSpace(otp))
            {
                ShowOTPError("Vui lòng nhập mã OTP!");
                return false;
            }

            if (otp.Length != 6)
            {
                ShowOTPError("Mã OTP phải có đúng 6 chữ số!");
                return false;
            }

            if (!otp.All(char.IsDigit))
            {
                ShowOTPError("Mã OTP chỉ được chứa số!");
                return false;
            }

            if (DateTime.Now > _otpExpiration)
            {
                ShowOTPError("Mã OTP đã hết hạn! Vui lòng yêu cầu gửi lại.");
                return false;
            }

            if (otp != _correctOTP)
            {
                ShowOTPError("Mã OTP không chính xác! Vui lòng kiểm tra lại.");
                return false;
            }

            ClearOTPError();
            return true;
        }

        private bool ValidateNewPassword()
        {
            string password = txtNewPassword.Text;

            if (string.IsNullOrWhiteSpace(password))
            {
                ShowNewPasswordError("Vui lòng nhập mật khẩu mới!");
                return false;
            }

            if (password.Length < 8)
            {
                ShowNewPasswordError("Mật khẩu phải có ít nhất 8 ký tự!");
                return false;
            }

            if (password.Length > 50)
            {
                ShowNewPasswordError("Mật khẩu không được quá 50 ký tự!");
                return false;
            }

            // Kiểm tra có chữ cái
            if (!password.Any(char.IsLetter))
            {
                ShowNewPasswordError("Mật khẩu phải có ít nhất 1 chữ cái!");
                return false;
            }

            // Kiểm tra có số
            if (!password.Any(char.IsDigit))
            {
                ShowNewPasswordError("Mật khẩu phải có ít nhất 1 chữ số!");
                return false;
            }

            // Kiểm tra có ký tự đặc biệt
            string specialChars = "!@#$%^&*()_+-=[]{}|;:',.<>?/~`";
            if (!password.Any(c => specialChars.Contains(c)))
            {
                ShowNewPasswordError("Mật khẩu phải có ít nhất 1 ký tự đặc biệt (!@#$%^&*...)");
                return false;
            }

            ClearNewPasswordError();

            // Kiểm tra lại confirm password nếu đã nhập
            if (!string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                ValidateConfirmPasswordRealtime();
            }

            return true;
        }

        private bool ValidateConfirmPassword()
        {
            string confirmPassword = txtConfirmPassword.Text;

            if (string.IsNullOrWhiteSpace(confirmPassword))
            {
                ShowConfirmPasswordError("Vui lòng xác nhận mật khẩu!");
                return false;
            }

            if (confirmPassword != txtNewPassword.Text)
            {
                ShowConfirmPasswordError("Mật khẩu xác nhận không khớp!");
                return false;
            }

            ClearConfirmPasswordError();
            return true;
        }

        // Validate realtime khi nhập confirm password
        private void ValidateConfirmPasswordRealtime()
        {
            string confirmPassword = txtConfirmPassword.Text;
            string newPassword = txtNewPassword.Text;

            // Chỉ validate nếu đã nhập confirm password
            if (string.IsNullOrWhiteSpace(confirmPassword))
            {
                ClearConfirmPasswordError();
                return;
            }

            // Kiểm tra khớp
            if (confirmPassword != newPassword)
            {
                ShowConfirmPasswordError("Mật khẩu xác nhận không khớp!");
            }
            else
            {
                ClearConfirmPasswordError();
            }
        }

        // Show error methods - giống LoginForm
        private void ShowOTPError(string message)
        {
            lblOTPError.Text = message;
            lblOTPError.Visible = true;
            pnlOTPInput.BackColor = Color.FromArgb(254, 242, 242);

            // Tạo viền đỏ cho panel
            pnlOTPInput.Paint += PnlOTPInput_Paint;
            pnlOTPInput.Invalidate();
        }

        private void PnlOTPInput_Paint(object sender, PaintEventArgs e)
        {
            if (lblOTPError.Visible)
            {
                using (Pen pen = new Pen(Color.FromArgb(220, 38, 38), 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, pnlOTPInput.Width - 1, pnlOTPInput.Height - 1);
                }
            }
        }

        private void ShowNewPasswordError(string message)
        {
            lblNewPasswordError.Text = message;
            lblNewPasswordError.Visible = true;
            pnlNewPassword.BackColor = Color.FromArgb(254, 242, 242);

            // Tạo viền đỏ cho panel
            pnlNewPassword.Paint += PnlNewPassword_Paint;
            pnlNewPassword.Invalidate();
        }

        private void PnlNewPassword_Paint(object sender, PaintEventArgs e)
        {
            if (lblNewPasswordError.Visible)
            {
                using (Pen pen = new Pen(Color.FromArgb(220, 38, 38), 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, pnlNewPassword.Width - 1, pnlNewPassword.Height - 1);
                }
            }
        }

        private void ShowConfirmPasswordError(string message)
        {
            lblConfirmPasswordError.Text = message;
            lblConfirmPasswordError.Visible = true;
            pnlConfirmPassword.BackColor = Color.FromArgb(254, 242, 242);

            // Tạo viền đỏ cho panel
            pnlConfirmPassword.Paint += PnlConfirmPassword_Paint;
            pnlConfirmPassword.Invalidate();
        }

        private void PnlConfirmPassword_Paint(object sender, PaintEventArgs e)
        {
            if (lblConfirmPasswordError.Visible)
            {
                using (Pen pen = new Pen(Color.FromArgb(220, 38, 38), 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, pnlConfirmPassword.Width - 1, pnlConfirmPassword.Height - 1);
                }
            }
        }

        // Clear error methods - giống LoginForm
        private void ClearOTPError()
        {
            lblOTPError.Visible = false;
            pnlOTPInput.BackColor = Color.FromArgb(240, 253, 244);

            // Xóa custom paint
            pnlOTPInput.Paint -= PnlOTPInput_Paint;
            pnlOTPInput.Invalidate();
        }

        private void ClearNewPasswordError()
        {
            lblNewPasswordError.Visible = false;
            pnlNewPassword.BackColor = Color.White;

            // Xóa custom paint
            pnlNewPassword.Paint -= PnlNewPassword_Paint;
            pnlNewPassword.Invalidate();
        }

        private void ClearConfirmPasswordError()
        {
            lblConfirmPasswordError.Visible = false;
            pnlConfirmPassword.BackColor = Color.White;

            // Xóa custom paint
            pnlConfirmPassword.Paint -= PnlConfirmPassword_Paint;
            pnlConfirmPassword.Invalidate();
        }

        // Toggle password visibility
        private void BtnTogglePassword_Click(object sender, EventArgs e)
        {
            txtNewPassword.UseSystemPasswordChar = !txtNewPassword.UseSystemPasswordChar;
            btnTogglePassword.Text = txtNewPassword.UseSystemPasswordChar ? "👁" : "🙈";
        }

        private void BtnToggleConfirm_Click(object sender, EventArgs e)
        {
            txtConfirmPassword.UseSystemPasswordChar = !txtConfirmPassword.UseSystemPasswordChar;
            btnToggleConfirm.Text = txtConfirmPassword.UseSystemPasswordChar ? "👁" : "🙈";
        }

        private async void BtnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate từng trường theo thứ tự - giống LoginForm
                bool isOTPValid = ValidateOTP();
                bool isNewPasswordValid = ValidateNewPassword();
                bool isConfirmPasswordValid = ValidateConfirmPassword();

                if (!isOTPValid)
                {
                    txtOTP.Focus();
                    return;
                }

                if (!isNewPasswordValid)
                {
                    txtNewPassword.Focus();
                    return;
                }

                if (!isConfirmPasswordValid)
                {
                    txtConfirmPassword.Focus();
                    return;
                }

                SetLoadingState(true);

                bool success = await _authService.ResetPasswordAsync(_email, txtNewPassword.Text);

                SetLoadingState(false);

                if (success)
                {
                    _countdownTimer.Stop();

                    MessageBox.Show(
                        "Đặt lại mật khẩu thành công!\n\n" +
                        "Bạn có thể đăng nhập ngay bây giờ với mật khẩu mới.",
                        "Thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                        "Không tìm thấy tài khoản hoặc có lỗi xảy ra!",
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
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
            txtOTP.Enabled = !isLoading;
            txtNewPassword.Enabled = !isLoading;
            txtConfirmPassword.Enabled = !isLoading;
            btnCancel.Enabled = !isLoading;
            btnResetPassword.Text = isLoading ? "Đang xử lý..." : "Đặt lại mật khẩu";
            this.Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
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

        private void LblBackToLogin_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Quay lại trang đăng nhập?\n\nTiến trình đặt lại mật khẩu sẽ bị hủy.",
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

        // KeyPress events - giống LoginForm
        private void TxtOTP_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép nhập số và phím Back
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Enter)
            {
                e.Handled = true;
                return;
            }

            // Xử lý khi nhấn Enter
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                if (ValidateOTP())
                {
                    txtNewPassword.Focus();
                }
            }
        }

        private void TxtNewPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                if (ValidateNewPassword())
                {
                    txtConfirmPassword.Focus();
                }
            }
        }

        private void TxtConfirmPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                // Validate trước khi submit
                if (ValidateConfirmPassword())
                {
                    BtnResetPassword_Click(sender, e);
                }
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

                var emailService = Program.GetService<EmailService>();
                var forgotForm = new ForgotPasswordForm(_context, _authService, emailService);
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

        private void LblBackToLogin_MouseEnter(object sender, EventArgs e)
        {
            lblBackToLogin.ForeColor = Color.FromArgb(79, 70, 229);
            lblBackToLogin.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold | FontStyle.Underline);
        }

        private void LblBackToLogin_MouseLeave(object sender, EventArgs e)
        {
            lblBackToLogin.ForeColor = Color.FromArgb(99, 102, 241);
            lblBackToLogin.Font = new Font("Segoe UI", 9.5F, FontStyle.Underline);
        }

        private void PnlMain_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        }
        #endregion
    }
}