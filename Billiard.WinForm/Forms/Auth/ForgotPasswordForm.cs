using Billiard.DAL.Data;
using Billiard.WinForm.Forms.Auth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.Auth
{
    public partial class ForgotPasswordForm : Form
    {
        private readonly BilliardDbContext _context;
        private string generatedOTP;
        private string userEmail;

        public ForgotPasswordForm(BilliardDbContext context)
        {
            _context = context;
            InitializeComponent();
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
                    MessageBox.Show("Vui lòng nhập email!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return;
                }

                if (!IsValidEmail(txtEmail.Text))
                {
                    MessageBox.Show("Email không hợp lệ!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return;
                }

                // Show loading
                btnSendOTP.Enabled = false;
                btnSendOTP.Text = "Đang gửi...";
                this.Cursor = Cursors.WaitCursor;

                // Check if email exists
                var nhanVien = await _context.NhanViens
                    .FirstOrDefaultAsync(nv => nv.Email == txtEmail.Text.Trim());

                if (nhanVien == null)
                {
                    MessageBox.Show("Email không tồn tại trong hệ thống!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    btnSendOTP.Enabled = true;
                    btnSendOTP.Text = "Gửi mã xác nhận";
                    this.Cursor = Cursors.Default;
                    return;
                }

                // Generate OTP
                generatedOTP = GenerateOTP();
                userEmail = txtEmail.Text.Trim();

                // Send OTP via email
                bool emailSent = await SendOTPEmail(userEmail, generatedOTP);

                this.Cursor = Cursors.Default;

                if (emailSent)
                {
                    MessageBox.Show(
                        $"Mã OTP đã được gửi đến email {userEmail}\nVui lòng kiểm tra hộp thư của bạn!",
                        "Thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    // Open ResetPassword form
                    var resetForm = new ResetPasswordForm(_context, userEmail, generatedOTP);
                    resetForm.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                        "Không thể gửi email. Vui lòng kiểm tra cấu hình email!",
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    btnSendOTP.Enabled = true;
                    btnSendOTP.Text = "Gửi mã xác nhận";
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                btnSendOTP.Enabled = true;
                btnSendOTP.Text = "Gửi mã xác nhận";
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
                // Cấu hình SMTP - Thay đổi theo email server của bạn
                string smtpServer = "smtp.gmail.com"; // Hoặc smtp của bạn
                int smtpPort = 587;
                string senderEmail = "your-email@gmail.com"; // Email gửi
                string senderPassword = "your-app-password"; // App password

                using (SmtpClient client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(senderEmail, "Quản Lý Quán Bi-a Pro");
                    mailMessage.To.Add(email);
                    mailMessage.Subject = "Mã OTP xác nhận đặt lại mật khẩu";
                    mailMessage.Body = $@"
                        <html>
                        <body style='font-family: Arial, sans-serif;'>
                            <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                                <h2 style='color: #667eea;'>🎱 Quản Lý Quán Bi-a Pro</h2>
                                <p>Xin chào,</p>
                                <p>Bạn đã yêu cầu đặt lại mật khẩu. Mã OTP của bạn là:</p>
                                <div style='background: #f0f0f0; padding: 20px; text-align: center; font-size: 32px; font-weight: bold; color: #667eea; margin: 20px 0;'>
                                    {otp}
                                </div>
                                <p>Mã OTP có hiệu lực trong 5 phút.</p>
                                <p>Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này.</p>
                                <hr style='margin: 20px 0;'>
                                <p style='color: #666; font-size: 12px;'>
                                    © 2025 Quản Lý Quán Bi-a Pro. All rights reserved.
                                </p>
                            </div>
                        </body>
                        </html>
                    ";
                    mailMessage.IsBodyHtml = true;

                    await client.SendMailAsync(mailMessage);
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Email error: {ex.Message}");
                return false;
            }
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

        private void BtnBack_Click(object sender, EventArgs e)
        {
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

        private void TxtEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnSendOTP_Click(sender, e);
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