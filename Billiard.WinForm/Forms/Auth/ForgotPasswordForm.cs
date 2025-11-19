using Billiard.BLL.Services;
using Billiard.DAL.Data;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.Auth
{
    public partial class ForgotPasswordForm : Form
    {
        private readonly BilliardDbContext _context;
        private readonly AuthService _authService;
        private readonly EmailService _emailService;
        private string generatedOTP;
        private string userEmail;

        public ForgotPasswordForm(BilliardDbContext context, AuthService authService, EmailService emailService = null)
        {
            _context = context;
            _authService = authService;
            _emailService = emailService;
            InitializeComponent();
            UpdateUI();
        }

        private void UpdateUI()
        {
            lblTitle.Text = "🔑 QUÊN MẬT KHẨU";
            lblSubtitle.Text = "Nhập email để nhận mã xác nhận";
        }

        private void ForgotPasswordForm_Load(object sender, EventArgs e)
        {
            txtEmail.Focus();
        }

        private async void BtnSendOTP_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate email
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

                SetLoadingState(true);

                // Check if email exists (auto-detect user type)
                var (exists, userType) = await _authService.CheckEmailExistsAsync(txtEmail.Text.Trim());

                if (!exists)
                {
                    SetLoadingState(false);
                    ShowError("Email không tồn tại trong hệ thống!", txtEmail);
                    return;
                }

                // Generate OTP
                generatedOTP = GenerateOTP();
                userEmail = txtEmail.Text.Trim();

                // Send OTP email
                bool emailSent = false;
                if (_emailService != null)
                {
                    emailSent = await _emailService.SendOTPEmailAsync(
                        userEmail,
                        generatedOTP,
                        userType == UserType.NhanVien
                    );
                }
                else
                {
                    // Fallback: Show OTP in message box for testing
                    MessageBox.Show(
                        $"⚠️ Email service chưa được cấu hình!\n\n" +
                        $"Mã OTP của bạn là: {generatedOTP}\n\n" +
                        $"(Chỉ dùng để test)",
                        "OTP Test Mode",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    emailSent = true;
                }

                SetLoadingState(false);

                if (emailSent)
                {
                    string userTypeText = userType == UserType.NhanVien ? "Nhân viên/Quản trị" : "Khách hàng";

                    MessageBox.Show(
                        $"✅ Mã OTP đã được gửi đến:\n{userEmail}\n\n" +
                        $"Loại tài khoản: {userTypeText}\n" +
                        $"Vui lòng kiểm tra hộp thư!\n" +
                        $"(Kiểm tra cả Spam nếu không thấy)",
                        "Gửi OTP thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    var resetForm = new ResetPasswordForm(
                        _context,
                        _authService,
                        userEmail,
                        generatedOTP,
                        userType == UserType.NhanVien
                    );
                    resetForm.ShowDialog();
                    this.Close();
                }
                else
                {
                    ShowError("Không thể gửi email!\nVui lòng kiểm tra cấu hình SMTP.", null);
                }
            }
            catch (Exception ex)
            {
                SetLoadingState(false);
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private void SetLoadingState(bool isLoading)
        {
            btnSendOTP.Enabled = !isLoading;
            btnSendOTP.Text = isLoading ? "⏳ Đang gửi..." : "📧 Gửi mã OTP";
            this.Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
        }

        private void ShowError(string message, Control focusControl)
        {
            MessageBox.Show(message, "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            focusControl?.Focus();
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TxtEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnSendOTP_Click(sender, e);
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