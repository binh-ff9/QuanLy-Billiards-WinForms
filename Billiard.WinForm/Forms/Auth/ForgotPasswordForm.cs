using Billiard.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.Auth
{
    public partial class ForgotPasswordForm : Form
    {
        private readonly BilliardDbContext _context;
        private readonly bool _isAdminMode;
        private string generatedOTP;
        private string userEmail;

        public ForgotPasswordForm(BilliardDbContext context, bool isAdminMode = false)
        {
            _context = context;
            _isAdminMode = isAdminMode;
            InitializeComponent();
            UpdateUIForMode();
        }

        private void UpdateUIForMode()
        {
            if (_isAdminMode)
            {
                lblTitle.Text = "🔑 QUÊN MẬT KHẨU ADMIN";
                lblSubtitle.Text = "Nhập email đã đăng ký (Admin/Nhân viên)";
                pnlDecoration.BackColor = Color.FromArgb(99, 102, 241);
                lblDecoTitle.ForeColor = Color.FromArgb(99, 102, 241);
                btnSendOTP.BackColor = Color.FromArgb(99, 102, 241);
            }
            else
            {
                lblTitle.Text = "🔑 QUÊN MẬT KHẨU";
                lblSubtitle.Text = "Nhập email để nhận mã xác nhận";
                pnlDecoration.BackColor = Color.FromArgb(16, 185, 129);
                lblDecoTitle.ForeColor = Color.FromArgb(16, 185, 129);
                btnSendOTP.BackColor = Color.FromArgb(16, 185, 129);
            }
        }

        private void ForgotPasswordForm_Load(object sender, EventArgs e)
        {
            txtEmail.Focus();
        }

        private async void BtnSendOTP_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    ShowError("Vui lòng nhập email!", txtEmail);
                    return;
                }

                if (!IsValidEmail(txtEmail.Text))
                {
                    ShowError("Email không hợp lệ!", txtEmail);
                    return;
                }

                SetLoadingState(true);

                // Check email based on mode
                bool emailExists;
                if (_isAdminMode)
                {
                    emailExists = await _context.NhanViens
                        .AnyAsync(nv => nv.Email == txtEmail.Text.Trim());
                }
                else
                {
                    emailExists = await _context.KhachHangs
                        .AnyAsync(kh => kh.Email == txtEmail.Text.Trim());
                }

                if (!emailExists)
                {
                    SetLoadingState(false);
                    ShowError($"Email không tồn tại trong hệ thống {(_isAdminMode ? "quản trị" : "khách hàng")}!", txtEmail);
                    return;
                }

                // Generate OTP
                generatedOTP = GenerateOTP();
                userEmail = txtEmail.Text.Trim();

                // Send OTP
                bool emailSent = await SendOTPEmail(userEmail, generatedOTP);

                SetLoadingState(false);

                if (emailSent)
                {
                    MessageBox.Show(
                        $"✅ Mã OTP đã được gửi đến:\n{userEmail}\n\n" +
                        $"Vui lòng kiểm tra hộp thư!\n" +
                        $"(Kiểm tra cả Spam nếu không thấy)",
                        "Gửi OTP thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    var resetForm = new ResetPasswordForm(_context, userEmail, generatedOTP, _isAdminMode);
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

        private async Task<bool> SendOTPEmail(string email, string otp)
        {
            try
            {
                // TODO: Cấu hình SMTP từ appsettings hoặc config
                string smtpServer = "smtp.gmail.com";
                int smtpPort = 587;
                string senderEmail = "your-email@gmail.com"; // TODO: Thay đổi
                string senderPassword = "your-app-password"; // TODO: Thay đổi

                using (SmtpClient client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(senderEmail, "Quán Bi-a Pro");
                    mail.To.Add(email);
                    mail.Subject = $"[Quán Bi-a Pro] Mã OTP khôi phục mật khẩu {(_isAdminMode ? "(Admin)" : "")}";
                    mail.Body = GetEmailTemplate(otp);
                    mail.IsBodyHtml = true;

                    await client.SendMailAsync(mail);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email error: {ex.Message}");
                return false;
            }
        }

        private string GetEmailTemplate(string otp)
        {
            string color = _isAdminMode ? "#6366F1" : "#10B981";
            string type = _isAdminMode ? "Quản trị viên" : "Khách hàng";

            return $@"
                <html>
                <body style='font-family: Arial, sans-serif; background: #f3f4f6; padding: 20px;'>
                    <div style='max-width: 600px; margin: 0 auto; background: white; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 6px rgba(0,0,0,0.1);'>
                        <div style='background: {color}; color: white; padding: 30px; text-align: center;'>
                            <h1 style='margin: 0; font-size: 28px;'>🎱 Quán Bi-a Pro</h1>
                            <p style='margin: 10px 0 0 0; font-size: 14px;'>Hệ thống quản lý chuyên nghiệp</p>
                        </div>
                        <div style='padding: 40px 30px;'>
                            <h2 style='color: #1f2937; margin-top: 0;'>Xin chào!</h2>
                            <p style='color: #4b5563; line-height: 1.6;'>
                                Bạn đã yêu cầu đặt lại mật khẩu cho tài khoản <strong>{type}</strong>.
                            </p>
                            <p style='color: #4b5563; line-height: 1.6;'>
                                Mã OTP xác nhận của bạn là:
                            </p>
                            <div style='background: #f9fafb; border: 3px dashed {color}; padding: 25px; text-align: center; border-radius: 8px; margin: 25px 0;'>
                                <div style='font-size: 48px; font-weight: bold; color: {color}; letter-spacing: 8px;'>
                                    {otp}
                                </div>
                            </div>
                            <div style='background: #fef3c7; border-left: 4px solid #f59e0b; padding: 15px; margin: 20px 0;'>
                                <p style='margin: 0; color: #92400e;'>
                                    ⚠️ <strong>Lưu ý quan trọng:</strong><br>
                                    • Mã OTP có hiệu lực trong <strong>5 phút</strong><br>
                                    • Không chia sẻ mã này với bất kỳ ai<br>
                                    • Nếu không phải bạn yêu cầu, hãy bỏ qua email này
                                </p>
                            </div>
                        </div>
                        <div style='background: #f9fafb; padding: 20px 30px; text-align: center; border-top: 1px solid #e5e7eb;'>
                            <p style='color: #6b7280; font-size: 12px; margin: 0;'>
                                © 2024 Quán Bi-a Pro. All rights reserved.<br>
                                📍 123 Đường ABC, TP.HCM | 📞 0909 123 456
                            </p>
                        </div>
                    </div>
                </body>
                </html>";
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