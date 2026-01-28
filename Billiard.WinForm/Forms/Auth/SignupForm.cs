using Billiard.BLL.Services;
using Billiard.DAL.Data;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.Auth
{
    public partial class SignupForm : Form
    {
        private readonly BilliardDbContext _context;
        private readonly AuthService _authService;

        // Label để hiển thị lỗi validation
        private Label lblTenKHError;
        private Label lblSDTError;
        private Label lblEmailError;
        private Label lblNgaySinhError;
        private Label lblMatKhauError;
        private Label lblXacNhanMatKhauError;

        // Danh sách các đuôi email phổ biến
        private readonly string[] _commonEmailDomains = new[]
        {
            "@gmail.com", "@yahoo.com", "@outlook.com", "@hotmail.com",
            "@icloud.com", "@aol.com", "@protonmail.com", "@zoho.com",
            "@mail.com", "@gmx.com", "@yandex.com", "@tutanota.com"
        };

        public SignupForm(BilliardDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
            InitializeComponent();
            AdjustControlPositions();
            InitializeValidationLabels();
        }

        private void AdjustControlPositions()
        {
            // Điều chỉnh vị trí các control với khoảng cách nhỏ hơn cho error messages
            // Mỗi error label chỉ cần khoảng 18px thay vì 22px

            // Title - giữ nguyên
            lblTitle.Location = new Point(41, 24);

            // Họ và Tên section
            lblTenKH.Location = new Point(41, 90);
            txtTenKH.Location = new Point(41, 120);
            // Error label sẽ ở: 120 + 37 + 2 = 159

            // Số điện thoại section - dịch xuống 18px
            lblSDT.Location = new Point(41, 179);
            txtSDT.Location = new Point(41, 209);
            // Error label sẽ ở: 209 + 37 + 2 = 248

            // Email section - dịch xuống 36px (18*2)
            lblEmail.Location = new Point(41, 268);
            txtEmail.Location = new Point(41, 298);
            // Error label sẽ ở: 298 + 37 + 2 = 337

            // Ngày sinh section - dịch xuống 54px (18*3)
            lblNgaySinh.Location = new Point(41, 357);
            dtpNgaySinh.Location = new Point(41, 387);
            // Error label sẽ ở: 387 + 37 + 2 = 426

            // Mật khẩu section - dịch xuống 72px (18*4)
            lblMatKhau.Location = new Point(41, 446);
            pnlMatKhau.Location = new Point(41, 476);
            // Error label sẽ ở: 476 + 42 + 2 = 520

            // Xác nhận mật khẩu section - dịch xuống 90px (18*5)
            lblXacNhanMatKhau.Location = new Point(41, 540);
            pnlXacNhanMatKhau.Location = new Point(41, 570);
            // Error label sẽ ở: 570 + 42 + 2 = 614

            // Buttons section - dịch xuống 108px (18*6)
            btnSignup.Location = new Point(41, 634);
            btnBackToLogin.Location = new Point(41, 694);
        }

        private void InitializeValidationLabels()
        {
            // Label lỗi cho Họ và Tên
            lblTenKHError = new Label
            {
                AutoSize = false,
                Size = new Size(380, 18),
                Location = new Point(41, 159),
                Font = new Font("Segoe UI", 8F, FontStyle.Regular),
                ForeColor = Color.FromArgb(220, 38, 38),
                Text = "",
                Visible = false
            };
            pnlMain.Controls.Add(lblTenKHError);
            lblTenKHError.BringToFront();

            // Label lỗi cho Số điện thoại
            lblSDTError = new Label
            {
                AutoSize = false,
                Size = new Size(380, 18),
                Location = new Point(41, 248),
                Font = new Font("Segoe UI", 8F, FontStyle.Regular),
                ForeColor = Color.FromArgb(220, 38, 38),
                Text = "",
                Visible = false
            };
            pnlMain.Controls.Add(lblSDTError);
            lblSDTError.BringToFront();

            // Label lỗi cho Email
            lblEmailError = new Label
            {
                AutoSize = false,
                Size = new Size(380, 18),
                Location = new Point(41, 337),
                Font = new Font("Segoe UI", 8F, FontStyle.Regular),
                ForeColor = Color.FromArgb(220, 38, 38),
                Text = "",
                Visible = false
            };
            pnlMain.Controls.Add(lblEmailError);
            lblEmailError.BringToFront();

            // Label lỗi cho Ngày sinh
            lblNgaySinhError = new Label
            {
                AutoSize = false,
                Size = new Size(380, 18),
                Location = new Point(41, 426),
                Font = new Font("Segoe UI", 8F, FontStyle.Regular),
                ForeColor = Color.FromArgb(220, 38, 38),
                Text = "",
                Visible = false
            };
            pnlMain.Controls.Add(lblNgaySinhError);
            lblNgaySinhError.BringToFront();

            // Label lỗi cho Mật khẩu
            lblMatKhauError = new Label
            {
                AutoSize = false,
                Size = new Size(380, 18),
                Location = new Point(41, 520),
                Font = new Font("Segoe UI", 8F, FontStyle.Regular),
                ForeColor = Color.FromArgb(220, 38, 38),
                Text = "",
                Visible = false
            };
            pnlMain.Controls.Add(lblMatKhauError);
            lblMatKhauError.BringToFront();

            // Label lỗi cho Xác nhận mật khẩu
            lblXacNhanMatKhauError = new Label
            {
                AutoSize = false,
                Size = new Size(380, 18),
                Location = new Point(41, 614),
                Font = new Font("Segoe UI", 8F, FontStyle.Regular),
                ForeColor = Color.FromArgb(220, 38, 38),
                Text = "",
                Visible = false
            };
            pnlMain.Controls.Add(lblXacNhanMatKhauError);
            lblXacNhanMatKhauError.BringToFront();

            // Thêm sự kiện TextChanged để xóa lỗi khi user nhập lại
            txtTenKH.TextChanged += (s, e) => ClearTenKHError();
            txtSDT.TextChanged += (s, e) => ClearSDTError();
            txtEmail.TextChanged += (s, e) => ClearEmailError();
            dtpNgaySinh.ValueChanged += (s, e) => ClearNgaySinhError();
            txtMatKhau.TextChanged += (s, e) =>
            {
                ClearMatKhauError();
                // Validate realtime confirm password nếu đã nhập
                if (!string.IsNullOrWhiteSpace(txtXacNhanMatKhau.Text))
                {
                    ValidateXacNhanMatKhauRealtime();
                }
            };
            txtXacNhanMatKhau.TextChanged += (s, e) => ValidateXacNhanMatKhauRealtime();

            // Thêm KeyPress events cho validation khi nhấn Enter
            txtTenKH.KeyPress += TxtTenKH_KeyPress;
            txtSDT.KeyPress += TxtSDT_KeyPress;
            txtEmail.KeyPress += TxtEmail_KeyPress;
            dtpNgaySinh.KeyPress += DtpNgaySinh_KeyPress;
            txtMatKhau.KeyPress += TxtMatKhau_KeyPress;
            txtXacNhanMatKhau.KeyPress += TxtXacNhanMatKhau_KeyPress;
        }

        private void SignupForm_Load(object sender, EventArgs e)
        {
            txtTenKH.Focus();
        }

        #region Validation Methods

        private bool ValidateTenKH()
        {
            string tenKH = txtTenKH.Text.Trim();

            if (string.IsNullOrWhiteSpace(tenKH))
            {
                ShowTenKHError("Vui lòng nhập họ và tên!");
                return false;
            }

            if (tenKH.Length < 2)
            {
                ShowTenKHError("Họ và tên phải có ít nhất 2 ký tự!");
                return false;
            }

            if (tenKH.Length > 100)
            {
                ShowTenKHError("Họ và tên không được quá 100 ký tự!");
                return false;
            }

            // Kiểm tra chỉ chứa chữ cái, khoảng trắng và dấu tiếng Việt
            if (!Regex.IsMatch(tenKH, @"^[a-zA-ZÀ-ỹ\s]+$"))
            {
                ShowTenKHError("Họ và tên chỉ được chứa chữ cái!");
                return false;
            }

            ClearTenKHError();
            return true;
        }

        private bool ValidateSDT()
        {
            string sdt = txtSDT.Text.Trim();

            if (string.IsNullOrWhiteSpace(sdt))
            {
                ShowSDTError("Vui lòng nhập số điện thoại!");
                return false;
            }

            // Loại bỏ khoảng trắng và dấu gạch ngang
            sdt = sdt.Replace(" ", "").Replace("-", "");

            if (sdt.Length < 10)
            {
                ShowSDTError("Số điện thoại phải có ít nhất 10 số!");
                return false;
            }

            // Kiểm tra định dạng số điện thoại Việt Nam
            string pattern = @"^(0|\+84)(3|5|7|8|9)[0-9]{8}$";
            if (!Regex.IsMatch(sdt, pattern))
            {
                ShowSDTError("Số điện thoại không đúng định dạng!");
                return false;
            }

            ClearSDTError();
            return true;
        }

        private bool ValidateEmail()
        {
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(email))
            {
                ShowEmailError("Vui lòng nhập email!");
                return false;
            }

            // Pattern email chuẩn
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(email, pattern))
            {
                ShowEmailError("Email không đúng định dạng!");
                return false;
            }

            // Kiểm tra đuôi email phổ biến và đưa ra gợi ý nếu sai
            string lowerEmail = email.ToLower();
            bool hasCommonDomain = false;
            foreach (var domain in _commonEmailDomains)
            {
                if (lowerEmail.EndsWith(domain))
                {
                    hasCommonDomain = true;
                    break;
                }
            }

            if (!hasCommonDomain && email.Contains("@"))
            {
                string suggestion = GetEmailSuggestion(email);
                if (!string.IsNullOrEmpty(suggestion))
                {
                    ShowEmailError($"Email không phổ biến!");
                    return false;
                }
            }

            ClearEmailError();
            return true;
        }

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
                if (domain.StartsWith(domainPart) || IsStringsSimilar(domain, domainPart))
                {
                    return $"{localPart}{commonDomain}";
                }
            }

            return "";
        }

        private bool IsStringsSimilar(string s1, string s2)
        {
            if (s1.Length < 3 || s2.Length < 3)
                return false;

            string prefix = s1.Substring(0, Math.Min(3, s1.Length));
            return s2.StartsWith(prefix);
        }

        private bool ValidateNgaySinh()
        {
            DateTime ngaySinh = dtpNgaySinh.Value.Date;
            DateTime today = DateTime.Now.Date;

            // Nếu chọn ngày trong tương lai
            if (ngaySinh > today)
            {
                ShowNgaySinhError("Ngày sinh không được ở tương lai!");
                return false;
            }

            // Kiểm tra tuổi tối thiểu (ví dụ: 13 tuổi)
            int age = today.Year - ngaySinh.Year;
            if (ngaySinh.Date > today.AddYears(-age)) age--;

            if (age < 16)
            {
                ShowNgaySinhError("Bạn phải từ 16 tuổi trở lên!");
                return false;
            }
            // Kiểm tra tuổi tối đa hợp lý (ví dụ: 120 tuổi)
            if (age > 120)
            {
                ShowNgaySinhError("Ngày sinh không hợp lệ!");
                return false;
            }
            ClearNgaySinhError();
            return true;
        }

        private bool ValidateMatKhau()
        {
            string matKhau = txtMatKhau.Text;

            if (string.IsNullOrWhiteSpace(matKhau))
            {
                ShowMatKhauError("Vui lòng nhập mật khẩu!");
                return false;
            }

            if (matKhau.Length < 6)
            {
                ShowMatKhauError("Mật khẩu phải có ít nhất 6 ký tự!");
                return false;
            }

            if (matKhau.Length > 50)
            {
                ShowMatKhauError("Mật khẩu không được quá 50 ký tự!");
                return false;
            }

            ClearMatKhauError();

            // Kiểm tra lại confirm password nếu đã nhập
            if (!string.IsNullOrWhiteSpace(txtXacNhanMatKhau.Text))
            {
                ValidateXacNhanMatKhauRealtime();
            }

            return true;
        }

        private bool ValidateXacNhanMatKhau()
        {
            string xacNhanMatKhau = txtXacNhanMatKhau.Text;

            if (string.IsNullOrWhiteSpace(xacNhanMatKhau))
            {
                ShowXacNhanMatKhauError("Vui lòng xác nhận mật khẩu!");
                return false;
            }

            if (xacNhanMatKhau != txtMatKhau.Text)
            {
                ShowXacNhanMatKhauError("Mật khẩu xác nhận không khớp!");
                return false;
            }

            ClearXacNhanMatKhauError();
            return true;
        }

        // Validate realtime khi nhập confirm password
        private void ValidateXacNhanMatKhauRealtime()
        {
            string xacNhanMatKhau = txtXacNhanMatKhau.Text;
            string matKhau = txtMatKhau.Text;

            if (string.IsNullOrWhiteSpace(xacNhanMatKhau))
            {
                ClearXacNhanMatKhauError();
                return;
            }

            if (xacNhanMatKhau != matKhau)
            {
                ShowXacNhanMatKhauError("Mật khẩu xác nhận không khớp!");
            }
            else
            {
                ClearXacNhanMatKhauError();
            }
        }

        #endregion

        #region Show Error Methods

        private void ShowTenKHError(string message)
        {
            lblTenKHError.Text = message;
            lblTenKHError.Visible = true;
            txtTenKH.BackColor = Color.FromArgb(254, 242, 242);
        }

        private void ShowSDTError(string message)
        {
            lblSDTError.Text = message;
            lblSDTError.Visible = true;
            txtSDT.BackColor = Color.FromArgb(254, 242, 242);
        }

        private void ShowEmailError(string message)
        {
            lblEmailError.Text = message;
            lblEmailError.Visible = true;
            txtEmail.BackColor = Color.FromArgb(254, 242, 242);
        }

        private void ShowNgaySinhError(string message)
        {
            lblNgaySinhError.Text = message;
            lblNgaySinhError.Visible = true;
            dtpNgaySinh.BackColor = Color.FromArgb(254, 242, 242);
        }

        private void ShowMatKhauError(string message)
        {
            lblMatKhauError.Text = message;
            lblMatKhauError.Visible = true;
            pnlMatKhau.BackColor = Color.FromArgb(254, 242, 242);

            pnlMatKhau.Paint += PnlMatKhau_Paint;
            pnlMatKhau.Invalidate();
        }

        private void PnlMatKhau_Paint(object sender, PaintEventArgs e)
        {
            if (lblMatKhauError.Visible)
            {
                using (Pen pen = new Pen(Color.FromArgb(220, 38, 38), 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, pnlMatKhau.Width - 1, pnlMatKhau.Height - 1);
                }
            }
        }

        private void ShowXacNhanMatKhauError(string message)
        {
            lblXacNhanMatKhauError.Text = message;
            lblXacNhanMatKhauError.Visible = true;
            pnlXacNhanMatKhau.BackColor = Color.FromArgb(254, 242, 242);

            pnlXacNhanMatKhau.Paint += PnlXacNhanMatKhau_Paint;
            pnlXacNhanMatKhau.Invalidate();
        }

        private void PnlXacNhanMatKhau_Paint(object sender, PaintEventArgs e)
        {
            if (lblXacNhanMatKhauError.Visible)
            {
                using (Pen pen = new Pen(Color.FromArgb(220, 38, 38), 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, pnlXacNhanMatKhau.Width - 1, pnlXacNhanMatKhau.Height - 1);
                }
            }
        }

        #endregion

        #region Clear Error Methods

        private void ClearTenKHError()
        {
            lblTenKHError.Visible = false;
            txtTenKH.BackColor = Color.White;
        }

        private void ClearSDTError()
        {
            lblSDTError.Visible = false;
            txtSDT.BackColor = Color.White;
        }

        private void ClearEmailError()
        {
            lblEmailError.Visible = false;
            txtEmail.BackColor = Color.White;
        }

        private void ClearNgaySinhError()
        {
            lblNgaySinhError.Visible = false;
            dtpNgaySinh.BackColor = Color.White;
        }

        private void ClearMatKhauError()
        {
            lblMatKhauError.Visible = false;
            pnlMatKhau.BackColor = Color.White;

            pnlMatKhau.Paint -= PnlMatKhau_Paint;
            pnlMatKhau.Invalidate();
        }

        private void ClearXacNhanMatKhauError()
        {
            lblXacNhanMatKhauError.Visible = false;
            pnlXacNhanMatKhau.BackColor = Color.White;

            pnlXacNhanMatKhau.Paint -= PnlXacNhanMatKhau_Paint;
            pnlXacNhanMatKhau.Invalidate();
        }

        #endregion

        #region Button Click Events

        // Toggle password visibility
        private void BtnTogglePassword_Click(object sender, EventArgs e)
        {
            txtMatKhau.UseSystemPasswordChar = !txtMatKhau.UseSystemPasswordChar;
            btnTogglePassword.Text = txtMatKhau.UseSystemPasswordChar ? "👁" : "🙈";
        }

        // Toggle confirm password visibility
        private void BtnToggleConfirm_Click(object sender, EventArgs e)
        {
            txtXacNhanMatKhau.UseSystemPasswordChar = !txtXacNhanMatKhau.UseSystemPasswordChar;
            btnToggleConfirm.Text = txtXacNhanMatKhau.UseSystemPasswordChar ? "👁" : "🙈";
        }

        private async void BtnSignup_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate từng trường theo thứ tự
                bool isTenKHValid = ValidateTenKH();
                bool isSDTValid = ValidateSDT();
                bool isEmailValid = ValidateEmail();
                bool isNgaySinhValid = ValidateNgaySinh();
                bool isMatKhauValid = ValidateMatKhau();
                bool isXacNhanMatKhauValid = ValidateXacNhanMatKhau();

                // Focus vào trường đầu tiên bị lỗi
                if (!isTenKHValid)
                {
                    txtTenKH.Focus();
                    return;
                }

                if (!isSDTValid)
                {
                    txtSDT.Focus();
                    return;
                }

                if (!isEmailValid)
                {
                    txtEmail.Focus();
                    return;
                }

                if (!isNgaySinhValid)
                {
                    dtpNgaySinh.Focus();
                    return;
                }

                if (!isMatKhauValid)
                {
                    txtMatKhau.Focus();
                    return;
                }

                if (!isXacNhanMatKhauValid)
                {
                    txtXacNhanMatKhau.Focus();
                    return;
                }

                SetLoadingState(true);

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
                    MessageBox.Show(
                        message,
                        "Đăng ký thất bại",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show(
                    $"Chào mừng {customer.TenKh}!\n\n" +
                    $"Đăng ký thành công với thông tin:\n" +
                    $"SĐT: {customer.Sdt}\n" +
                    $"Email: {customer.Email}\n" +
                    $"Hạng: {customer.HangTv}\n" +
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
            txtTenKH.Enabled = !isLoading;
            txtSDT.Enabled = !isLoading;
            txtEmail.Enabled = !isLoading;
            dtpNgaySinh.Enabled = !isLoading;
            txtMatKhau.Enabled = !isLoading;
            txtXacNhanMatKhau.Enabled = !isLoading;
            btnBackToLogin.Enabled = !isLoading;
            btnSignup.Text = isLoading ? "Đang xử lý..." : "Đăng ký";
            this.Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
        }

        private void BtnBackToLogin_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Hide();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region KeyPress Events

        // KeyPress events - validate khi nhấn Enter, chuyển ô nếu valid
        private void TxtTenKH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                if (ValidateTenKH())
                {
                    txtSDT.Focus();
                }
            }
        }

        private void TxtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                if (ValidateSDT())
                {
                    txtEmail.Focus();
                }
            }
        }

        private void TxtEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                if (ValidateEmail())
                {
                    dtpNgaySinh.Focus();
                }
            }
        }

        private void DtpNgaySinh_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                if (ValidateNgaySinh())
                {
                    txtMatKhau.Focus();
                }
            }
        }

        private void TxtMatKhau_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                if (ValidateMatKhau())
                {
                    txtXacNhanMatKhau.Focus();
                }
            }
        }

        private void TxtXacNhanMatKhau_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                if (ValidateXacNhanMatKhau())
                {
                    BtnSignup_Click(sender, e);
                }
            }
        }

        #endregion

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