using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Billiard.BLL.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Gửi email OTP
        /// </summary>
        public async Task<bool> SendOTPEmailAsync(string recipientEmail, string otp, bool isAdminMode = false)
        {
            try
            {
                var smtpSettings = GetSmtpSettings();

                using (var client = new SmtpClient(smtpSettings.Server, smtpSettings.Port))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(
                        smtpSettings.SenderEmail,
                        smtpSettings.SenderPassword
                    );

                    var mail = new MailMessage
                    {
                        From = new MailAddress(smtpSettings.SenderEmail, "Quán Bi-a Pro"),
                        Subject = $"[Quán Bi-a Pro] Mã OTP khôi phục mật khẩu {(isAdminMode ? "(Admin)" : "")}",
                        Body = GetOTPEmailTemplate(otp, isAdminMode),
                        IsBodyHtml = true
                    };

                    mail.To.Add(recipientEmail);

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

        /// <summary>
        /// Lấy cấu hình SMTP từ appsettings.json
        /// </summary>
        private SmtpSettings GetSmtpSettings()
        {
            return new SmtpSettings
            {
                Server = _configuration["EmailSettings:SmtpServer"] ?? "smtp.gmail.com",
                Port = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587"),
                SenderEmail = _configuration["EmailSettings:SenderEmail"] ?? "your-email@gmail.com",
                SenderPassword = _configuration["EmailSettings:SenderPassword"] ?? "your-app-password"
            };
        }

        /// <summary>
        /// Template HTML cho email OTP
        /// </summary>
        private string GetOTPEmailTemplate(string otp, bool isAdminMode)
        {
            string color = isAdminMode ? "#6366F1" : "#10B981";
            string type = isAdminMode ? "Quản trị viên" : "Khách hàng";

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

        private class SmtpSettings
        {
            public string Server { get; set; }
            public int Port { get; set; }
            public string SenderEmail { get; set; }
            public string SenderPassword { get; set; }
        }
    }
}