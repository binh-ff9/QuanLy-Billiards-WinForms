namespace Billiard.WinForm.Forms.Auth
{
    partial class ResetPasswordForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlLeft = new Panel();
            pnlDecoration = new Panel();
            lblDecoSubtitle = new Label();
            lblDecoTitle = new Label();
            pnlRight = new Panel();
            pnlMain = new Panel();
            lblResendOTP = new Label();
            lblCountdown = new Label();
            btnCancel = new Button();
            btnResetPassword = new Button();
            // Password fields with toggle
            pnlConfirmPassword = new Panel();
            txtConfirmPassword = new TextBox();
            btnToggleConfirm = new Button();
            lblConfirmPassword = new Label();
            pnlNewPassword = new Panel();
            txtNewPassword = new TextBox();
            btnTogglePassword = new Button();
            lblNewPassword = new Label();
            pnlOTPInput = new Panel();
            txtOTP = new TextBox();
            lblOTP = new Label();
            lblEmailDisplay = new Label();
            lblSubtitle = new Label();
            lblTitle = new Label();
            btnClose = new Button();
            pnlLeft.SuspendLayout();
            pnlDecoration.SuspendLayout();
            pnlRight.SuspendLayout();
            pnlMain.SuspendLayout();
            pnlOTPInput.SuspendLayout();
            pnlNewPassword.SuspendLayout();
            pnlConfirmPassword.SuspendLayout();
            SuspendLayout();
            // 
            // pnlLeft
            // 
            pnlLeft.BackColor = Color.FromArgb(248, 250, 252);
            pnlLeft.Controls.Add(pnlDecoration);
            pnlLeft.Dock = DockStyle.Left;
            pnlLeft.Location = new Point(0, 0);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Size = new Size(382, 620);
            pnlLeft.TabIndex = 0;
            // 
            // pnlDecoration
            // 
            pnlDecoration.BackColor = Color.MidnightBlue;
            pnlDecoration.Controls.Add(lblDecoSubtitle);
            pnlDecoration.Controls.Add(lblDecoTitle);
            pnlDecoration.Location = new Point(22, 22);
            pnlDecoration.Name = "pnlDecoration";
            pnlDecoration.Size = new Size(340, 573);
            pnlDecoration.TabIndex = 0;
            // 
            // lblDecoSubtitle
            // 
            lblDecoSubtitle.Dock = DockStyle.Bottom;
            lblDecoSubtitle.Font = new Font("Segoe UI", 11F);
            lblDecoSubtitle.ForeColor = Color.White;
            lblDecoSubtitle.Location = new Point(0, 313);
            lblDecoSubtitle.Name = "lblDecoSubtitle";
            lblDecoSubtitle.Padding = new Padding(30);
            lblDecoSubtitle.Size = new Size(340, 260);
            lblDecoSubtitle.TabIndex = 1;
            lblDecoSubtitle.Text = "🔒 Bảo mật mật khẩu:\r\n\r\n✅ Tối thiểu 6 ký tự\r\n✅ Kết hợp chữ và số\r\n✅ Không dùng thông tin cá nhân\r\n✅ Không chia sẻ với người khác\r\n✅ Thay đổi định kỳ\r\n\r\n💡 Mật khẩu mạnh = Tài khoản an toàn!";
            // 
            // lblDecoTitle
            // 
            lblDecoTitle.BackColor = Color.MidnightBlue;
            lblDecoTitle.Dock = DockStyle.Top;
            lblDecoTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblDecoTitle.ForeColor = Color.White;
            lblDecoTitle.Location = new Point(0, 0);
            lblDecoTitle.Name = "lblDecoTitle";
            lblDecoTitle.Padding = new Padding(20, 40, 20, 0);
            lblDecoTitle.Size = new Size(340, 200);
            lblDecoTitle.TabIndex = 0;
            lblDecoTitle.Text = "\r\nBẢO MẬT";
            lblDecoTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlRight
            // 
            pnlRight.BackColor = Color.White;
            pnlRight.Controls.Add(pnlMain);
            pnlRight.Controls.Add(btnClose);
            pnlRight.Dock = DockStyle.Fill;
            pnlRight.Location = new Point(382, 0);
            pnlRight.Name = "pnlRight";
            pnlRight.Size = new Size(468, 620);
            pnlRight.TabIndex = 1;
            // 
            // pnlMain
            // 
            pnlMain.Controls.Add(lblResendOTP);
            pnlMain.Controls.Add(lblCountdown);
            pnlMain.Controls.Add(btnCancel);
            pnlMain.Controls.Add(btnResetPassword);
            pnlMain.Controls.Add(pnlConfirmPassword);
            pnlMain.Controls.Add(lblConfirmPassword);
            pnlMain.Controls.Add(pnlNewPassword);
            pnlMain.Controls.Add(lblNewPassword);
            pnlMain.Controls.Add(pnlOTPInput);
            pnlMain.Controls.Add(lblOTP);
            pnlMain.Controls.Add(lblEmailDisplay);
            pnlMain.Controls.Add(lblSubtitle);
            pnlMain.Controls.Add(lblTitle);
            pnlMain.Location = new Point(22, 22);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(400, 573);
            pnlMain.TabIndex = 0;
            pnlMain.Paint += PnlMain_Paint;
            // 
            // lblResendOTP
            // 
            lblResendOTP.Cursor = Cursors.Hand;
            lblResendOTP.Font = new Font("Segoe UI", 9F, FontStyle.Underline);
            lblResendOTP.ForeColor = Color.FromArgb(16, 185, 129);
            lblResendOTP.Location = new Point(250, 543);
            lblResendOTP.Name = "lblResendOTP";
            lblResendOTP.Size = new Size(100, 25);
            lblResendOTP.TabIndex = 6;
            lblResendOTP.Text = "🔄 Gửi lại OTP";
            lblResendOTP.TextAlign = ContentAlignment.MiddleRight;
            lblResendOTP.Click += LblResendOTP_Click;
            lblResendOTP.MouseEnter += LblResendOTP_MouseEnter;
            lblResendOTP.MouseLeave += LblResendOTP_MouseLeave;
            // 
            // lblCountdown
            // 
            lblCountdown.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCountdown.ForeColor = Color.FromArgb(100, 116, 139);
            lblCountdown.Location = new Point(50, 543);
            lblCountdown.Name = "lblCountdown";
            lblCountdown.Size = new Size(200, 25);
            lblCountdown.TabIndex = 11;
            lblCountdown.Text = "⏱️ Thời gian còn lại: 05:00";
            lblCountdown.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.Firebrick;
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(50, 490);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(300, 45);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Hủy";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += BtnCancel_Click;
            // 
            // btnResetPassword
            // 
            btnResetPassword.BackColor = Color.SeaGreen;
            btnResetPassword.Cursor = Cursors.Hand;
            btnResetPassword.FlatAppearance.BorderSize = 0;
            btnResetPassword.FlatStyle = FlatStyle.Flat;
            btnResetPassword.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnResetPassword.ForeColor = Color.White;
            btnResetPassword.Location = new Point(50, 425);
            btnResetPassword.Name = "btnResetPassword";
            btnResetPassword.Size = new Size(300, 50);
            btnResetPassword.TabIndex = 4;
            btnResetPassword.Text = "Đặt lại mật khẩu";
            btnResetPassword.UseVisualStyleBackColor = false;
            btnResetPassword.Click += BtnResetPassword_Click;
            // 
            // lblConfirmPassword
            // 
            lblConfirmPassword.AutoSize = true;
            lblConfirmPassword.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblConfirmPassword.ForeColor = Color.FromArgb(51, 65, 85);
            lblConfirmPassword.Location = new Point(50, 320);
            lblConfirmPassword.Name = "lblConfirmPassword";
            lblConfirmPassword.Size = new Size(243, 28);
            lblConfirmPassword.TabIndex = 7;
            lblConfirmPassword.Text = "🔐 Xác nhận mật khẩu *";
            // 
            // pnlConfirmPassword - Container for confirm password with eye icon
            // 
            pnlConfirmPassword.BackColor = Color.White;
            pnlConfirmPassword.BorderStyle = BorderStyle.FixedSingle;
            pnlConfirmPassword.Controls.Add(txtConfirmPassword);
            pnlConfirmPassword.Controls.Add(btnToggleConfirm);
            pnlConfirmPassword.Location = new Point(50, 350);
            pnlConfirmPassword.Name = "pnlConfirmPassword";
            pnlConfirmPassword.Size = new Size(300, 42);
            pnlConfirmPassword.TabIndex = 3;
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.BorderStyle = BorderStyle.None;
            txtConfirmPassword.Font = new Font("Segoe UI", 11F);
            txtConfirmPassword.Location = new Point(8, 8);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.PlaceholderText = "Nhập lại mật khẩu mới";
            txtConfirmPassword.Size = new Size(245, 30);
            txtConfirmPassword.TabIndex = 0;
            txtConfirmPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.KeyPress += TxtConfirmPassword_KeyPress;
            // 
            // btnToggleConfirm
            // 
            btnToggleConfirm.BackColor = Color.Transparent;
            btnToggleConfirm.Cursor = Cursors.Hand;
            btnToggleConfirm.FlatAppearance.BorderSize = 0;
            btnToggleConfirm.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 240, 240);
            btnToggleConfirm.FlatStyle = FlatStyle.Flat;
            btnToggleConfirm.Font = new Font("Segoe UI", 11F);
            btnToggleConfirm.ForeColor = Color.Gray;
            btnToggleConfirm.Location = new Point(255, 2);
            btnToggleConfirm.Name = "btnToggleConfirm";
            btnToggleConfirm.Size = new Size(40, 36);
            btnToggleConfirm.TabIndex = 1;
            btnToggleConfirm.TabStop = false;
            btnToggleConfirm.Text = "👁";
            btnToggleConfirm.UseVisualStyleBackColor = false;
            btnToggleConfirm.Click += BtnToggleConfirm_Click;
            // 
            // lblNewPassword
            // 
            lblNewPassword.AutoSize = true;
            lblNewPassword.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblNewPassword.ForeColor = Color.FromArgb(51, 65, 85);
            lblNewPassword.Location = new Point(50, 240);
            lblNewPassword.Name = "lblNewPassword";
            lblNewPassword.Size = new Size(194, 28);
            lblNewPassword.TabIndex = 5;
            lblNewPassword.Text = "🔒 Mật khẩu mới *";
            // 
            // pnlNewPassword - Container for new password with eye icon
            // 
            pnlNewPassword.BackColor = Color.White;
            pnlNewPassword.BorderStyle = BorderStyle.FixedSingle;
            pnlNewPassword.Controls.Add(txtNewPassword);
            pnlNewPassword.Controls.Add(btnTogglePassword);
            pnlNewPassword.Location = new Point(50, 270);
            pnlNewPassword.Name = "pnlNewPassword";
            pnlNewPassword.Size = new Size(300, 42);
            pnlNewPassword.TabIndex = 2;
            // 
            // txtNewPassword
            // 
            txtNewPassword.BorderStyle = BorderStyle.None;
            txtNewPassword.Font = new Font("Segoe UI", 11F);
            txtNewPassword.Location = new Point(8, 8);
            txtNewPassword.Name = "txtNewPassword";
            txtNewPassword.PlaceholderText = "Nhập mật khẩu mới";
            txtNewPassword.Size = new Size(245, 30);
            txtNewPassword.TabIndex = 0;
            txtNewPassword.UseSystemPasswordChar = true;
            // 
            // btnTogglePassword
            // 
            btnTogglePassword.BackColor = Color.Transparent;
            btnTogglePassword.Cursor = Cursors.Hand;
            btnTogglePassword.FlatAppearance.BorderSize = 0;
            btnTogglePassword.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 240, 240);
            btnTogglePassword.FlatStyle = FlatStyle.Flat;
            btnTogglePassword.Font = new Font("Segoe UI", 11F);
            btnTogglePassword.ForeColor = Color.Gray;
            btnTogglePassword.Location = new Point(255, 2);
            btnTogglePassword.Name = "btnTogglePassword";
            btnTogglePassword.Size = new Size(40, 36);
            btnTogglePassword.TabIndex = 1;
            btnTogglePassword.TabStop = false;
            btnTogglePassword.Text = "👁";
            btnTogglePassword.UseVisualStyleBackColor = false;
            btnTogglePassword.Click += BtnTogglePassword_Click;
            // 
            // pnlOTPInput
            // 
            pnlOTPInput.BackColor = Color.FromArgb(240, 253, 244);
            pnlOTPInput.BorderStyle = BorderStyle.FixedSingle;
            pnlOTPInput.Controls.Add(txtOTP);
            pnlOTPInput.Location = new Point(50, 165);
            pnlOTPInput.Name = "pnlOTPInput";
            pnlOTPInput.Padding = new Padding(10);
            pnlOTPInput.Size = new Size(300, 60);
            pnlOTPInput.TabIndex = 1;
            // 
            // txtOTP
            // 
            txtOTP.BackColor = Color.FromArgb(240, 253, 244);
            txtOTP.BorderStyle = BorderStyle.None;
            txtOTP.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            txtOTP.Location = new Point(12, 0);
            txtOTP.MaxLength = 6;
            txtOTP.Name = "txtOTP";
            txtOTP.PlaceholderText = "000000";
            txtOTP.Size = new Size(270, 54);
            txtOTP.TabIndex = 0;
            txtOTP.TextAlign = HorizontalAlignment.Center;
            txtOTP.KeyPress += TxtOTP_KeyPress;
            // 
            // lblOTP
            // 
            lblOTP.AutoSize = true;
            lblOTP.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblOTP.ForeColor = Color.FromArgb(51, 65, 85);
            lblOTP.Location = new Point(50, 135);
            lblOTP.Name = "lblOTP";
            lblOTP.Size = new Size(235, 28);
            lblOTP.TabIndex = 3;
            lblOTP.Text = "🔢 Mã OTP (6 chữ số) *";
            // 
            // lblEmailDisplay
            // 
            lblEmailDisplay.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblEmailDisplay.ForeColor = Color.FromArgb(16, 185, 129);
            lblEmailDisplay.Location = new Point(50, 100);
            lblEmailDisplay.Name = "lblEmailDisplay";
            lblEmailDisplay.Size = new Size(300, 25);
            lblEmailDisplay.TabIndex = 2;
            lblEmailDisplay.Text = "📧 example@email.com";
            lblEmailDisplay.TextAlign = ContentAlignment.TopCenter;
            // 
            // lblSubtitle
            // 
            lblSubtitle.Font = new Font("Segoe UI", 9.5F);
            lblSubtitle.ForeColor = Color.FromArgb(100, 116, 139);
            lblSubtitle.Location = new Point(50, 70);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(300, 25);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Nhập mã OTP và mật khẩu mới";
            lblSubtitle.TextAlign = ContentAlignment.TopCenter;
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.MidnightBlue;
            lblTitle.Location = new Point(3, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(392, 50);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "ĐẶT LẠI MẬT KHẨU";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.Transparent;
            btnClose.Cursor = Cursors.Hand;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            btnClose.ForeColor = Color.Gray;
            btnClose.Location = new Point(420, 3);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(40, 46);
            btnClose.TabIndex = 10;
            btnClose.TabStop = false;
            btnClose.Text = "✕";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += BtnClose_Click;
            btnClose.MouseEnter += BtnClose_MouseEnter;
            btnClose.MouseLeave += BtnClose_MouseLeave;
            // 
            // ResetPasswordForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(850, 620);
            Controls.Add(pnlRight);
            Controls.Add(pnlLeft);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ResetPasswordForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đặt lại mật khẩu - Quán Bi-a Pro";
            Load += ResetPasswordForm_Load;
            pnlLeft.ResumeLayout(false);
            pnlDecoration.ResumeLayout(false);
            pnlRight.ResumeLayout(false);
            pnlMain.ResumeLayout(false);
            pnlMain.PerformLayout();
            pnlOTPInput.ResumeLayout(false);
            pnlOTPInput.PerformLayout();
            pnlNewPassword.ResumeLayout(false);
            pnlNewPassword.PerformLayout();
            pnlConfirmPassword.ResumeLayout(false);
            pnlConfirmPassword.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlLeft;
        private Panel pnlDecoration;
        private Label lblDecoTitle;
        private Label lblDecoSubtitle;
        private Panel pnlRight;
        private Panel pnlMain;
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblEmailDisplay;
        private Label lblOTP;
        private Panel pnlOTPInput;
        private TextBox txtOTP;
        private Label lblNewPassword;
        private Panel pnlNewPassword;
        private TextBox txtNewPassword;
        private Button btnTogglePassword;
        private Label lblConfirmPassword;
        private Panel pnlConfirmPassword;
        private TextBox txtConfirmPassword;
        private Button btnToggleConfirm;
        private Button btnResetPassword;
        private Button btnCancel;
        private Label lblCountdown;
        private Label lblResendOTP;
        private Button btnClose;
    }
}