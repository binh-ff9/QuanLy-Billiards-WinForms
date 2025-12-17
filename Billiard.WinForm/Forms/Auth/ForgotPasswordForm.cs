using Billiard.BLL.Services;
using Billiard.DAL.Data;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
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

        // Label để hiển thị lỗi validation
        private Label lblEmailError;

        // Danh sách các đuôi email phổ biến
        private readonly string[] _commonEmailDomains = new[]
        {
            "@gmail.com", "@yahoo.com", "@outlook.com", "@hotmail.com",
            "@icloud.com", "@aol.com", "@protonmail.com", "@zoho.com",
            "@mail.com", "@gmx.com", "@yandex.com", "@tutanota.com"
        };

        public ForgotPasswordForm(BilliardDbContext context, AuthService authService, EmailService emailService = null)
        {
            _context = context;
            _authService = authService;
            _emailService = emailService;
            InitializeComponent();
            UpdateUI();
            InitializeValidationLabels();
        }

        private void UpdateUI()
        {
            lblTitle.Text = "QUÊN MẬT KHẨU";
            lblSubtitle.Text = "Nhập email để nhận mã xác nhận";
        }

        private void InitializeValidationLabels()
        {
            // Tạo label thông báo lỗi cho email
            lblEmailError = new Label
            {
                AutoSize = false,
                Size = new Size(300, 20),
                Location = new Point(50, 207), // Ngay dưới txtEmail
                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(220, 38, 38), // Red-600
                Text = "",
                Visible = false
            };
            pnlMain.Controls.Add(lblEmailError);
            lblEmailError.BringToFront();

            // Thêm sự kiện TextChanged để xóa lỗi khi user nhập lại
            txtEmail.TextChanged += (s, e) => ClearEmailError();
        }

        private void ForgotPasswordForm_Load(object sender, EventArgs e)
        {
            txtEmail.Focus();
        }

        // Kiểm tra định dạng email với đuôi phổ biến
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Pattern email chuẩn
                string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                if (!Regex.IsMatch(email, pattern))
                    return false;

                // Kiểm tra có đuôi email phổ biến không
                string lowerEmail = email.ToLower();
                foreach (var domain in _commonEmailDomains)
                {
                    if (lowerEmail.EndsWith(domain))
                        return true;
                }

                // Nếu không phải đuôi phổ biến nhưng có @, vẫn chấp nhận (có thể là email doanh nghiệp)
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Lấy gợi ý đuôi email phổ biến
        private string GetEmailSuggestion(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                return "";

            string[] parts = email.Split('@');
            if (parts.Length != 2)
                return "";

            string localPart = parts[0];
            string domainPart = parts[1].ToLower();

            // Tìm domain phổ biến gần giống nhất
            foreach (var commonDomain in _commonEmailDomains)
            {
                string domain = commonDomain.Substring(1); // Bỏ @
                if (domain.StartsWith(domainPart) ||
                    IsStringsSimilar(domain, domainPart))
                {
                    return $"{localPart}{commonDomain}";
                }
            }

            return "";
        }

        // So sánh độ tương đồng của 2 chuỗi
        private bool IsStringsSimilar(string s1, string s2)
        {
            if (s1.Length < 3 || s2.Length < 3)
                return false;

            // Kiểm tra xem s2 có chứa 3 ký tự đầu của s1 không
            string prefix = s1.Substring(0, Math.Min(3, s1.Length));
            return s2.StartsWith(prefix);
        }

        private bool ValidateEmail()
        {
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(email))
            {
                ShowEmailError("Vui lòng nhập email!");
                return false;
            }

            bool isValid = IsValidEmail(email);
            if (!isValid)
            {
                string suggestion = GetEmailSuggestion(email);
                string errorMessage = "Email không đúng định dạng!";

                if (!string.IsNullOrEmpty(suggestion))
                {
                    errorMessage += $" Ý bạn là: {suggestion}?";
                }

                ShowEmailError(errorMessage);
                return false;
            }

            ClearEmailError();
            return true;
        }

        // Hiển thị lỗi validation cho email
        private void ShowEmailError(string message)
        {
            lblEmailError.Text = message;
            lblEmailError.Visible = true;
            txtEmail.BackColor = Color.FromArgb(254, 242, 242); // Red-50
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
        }

        // Xóa thông báo lỗi email
        private void ClearEmailError()
        {
            lblEmailError.Visible = false;
            txtEmail.BackColor = Color.White;
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
        }

        private async void BtnSendOTP_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate email trước
                if (!ValidateEmail())
                {
                    txtEmail.Focus();
                    return;
                }

                SetLoadingState(true);

                // Check if email exists (auto-detect user type)
                var (exists, userType) = await _authService.CheckEmailExistsAsync(txtEmail.Text.Trim());

                if (!exists)
                {
                    SetLoadingState(false);
                    ShowEmailError("Email không tồn tại trong hệ thống!");
                    txtEmail.Focus();
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
                        $"Email service chưa được cấu hình!\n\n" +
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
                        $"Mã OTP đã được gửi đến:\n{userEmail}\n\n" +
                        $"Loại tài khoản: {userTypeText}\n" +
                        $"Vui lòng kiểm tra hộp thư!\n" + "",
                        "Gửi OTP thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    // Đóng form hiện tại trước khi mở form mới
                    this.Hide();

                    var resetForm = new ResetPasswordForm(
                        _context,
                        _authService,
                        userEmail,
                        generatedOTP,
                        userType == UserType.NhanVien
                    );

                    // Nếu user đóng ResetPasswordForm, đóng luôn ForgotPasswordForm
                    if (resetForm.ShowDialog() == DialogResult.Cancel)
                    {
                        this.Close();
                    }
                    else
                    {
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show(
                        "Không thể gửi email!\nVui lòng kiểm tra cấu hình SMTP.",
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

        private string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private void SetLoadingState(bool isLoading)
        {
            btnSendOTP.Enabled = !isLoading;
            txtEmail.Enabled = !isLoading;
            btnBack.Enabled = !isLoading;
            btnSendOTP.Text = isLoading ? "Đang gửi..." : "Gửi mã OTP";
            this.Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
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
                e.Handled = true;
                BtnSendOTP_Click(sender, e);
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