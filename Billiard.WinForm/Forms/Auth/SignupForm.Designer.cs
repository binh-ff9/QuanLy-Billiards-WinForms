namespace Billiard.WinForm.Forms.Auth
{
    partial class SignupForm
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
            btnClose = new Button();
            lblTitle = new Label();
            btnBackToLogin = new Button();
            btnSignup = new Button();
            chkShowPassword = new CheckBox();
            txtXacNhanMatKhau = new TextBox();
            lblXacNhanMatKhau = new Label();
            txtMatKhau = new TextBox();
            lblMatKhau = new Label();
            dtpNgaySinh = new DateTimePicker();
            lblNgaySinh = new Label();
            txtEmail = new TextBox();
            lblEmail = new Label();
            txtSDT = new TextBox();
            lblSDT = new Label();
            txtTenKH = new TextBox();
            lblTenKH = new Label();
            pnlLeft.SuspendLayout();
            pnlDecoration.SuspendLayout();
            pnlRight.SuspendLayout();
            pnlMain.SuspendLayout();
            SuspendLayout();
            // 
            // pnlLeft
            // 
            pnlLeft.BackColor = Color.FromArgb(248, 250, 252);
            pnlLeft.Controls.Add(pnlDecoration);
            pnlLeft.Dock = DockStyle.Left;
            pnlLeft.Location = new Point(0, 0);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Size = new Size(394, 820);
            pnlLeft.TabIndex = 0;
            // 
            // pnlDecoration
            // 
            pnlDecoration.BackColor = Color.MidnightBlue;
            pnlDecoration.Controls.Add(lblDecoSubtitle);
            pnlDecoration.Controls.Add(lblDecoTitle);
            pnlDecoration.Location = new Point(21, 24);
            pnlDecoration.Name = "pnlDecoration";
            pnlDecoration.Size = new Size(340, 765);
            pnlDecoration.TabIndex = 0;
            // 
            // lblDecoSubtitle
            // 
            lblDecoSubtitle.Dock = DockStyle.Bottom;
            lblDecoSubtitle.Font = new Font("Segoe UI", 11F);
            lblDecoSubtitle.ForeColor = Color.White;
            lblDecoSubtitle.Location = new Point(0, 297);
            lblDecoSubtitle.Name = "lblDecoSubtitle";
            lblDecoSubtitle.Padding = new Padding(30);
            lblDecoSubtitle.Size = new Size(340, 468);
            lblDecoSubtitle.TabIndex = 1;
            lblDecoSubtitle.Text = "✨ Đặc quyền thành viên:\r\n\r\n🎁 Tích điểm mỗi lần chơi\r\n💎 Ưu đãi theo rank\r\n🎉 Khuyến mãi đặc biệt\r\n🎯 Đặt bàn trước online\r\n⚡ Thanh toán nhanh chóng\r\n📊 Theo dõi lịch sử chơi";
            // 
            // lblDecoTitle
            // 
            lblDecoTitle.BackColor = Color.MidnightBlue;
            lblDecoTitle.Dock = DockStyle.Top;
            lblDecoTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblDecoTitle.ForeColor = Color.White;
            lblDecoTitle.Location = new Point(0, 0);
            lblDecoTitle.Name = "lblDecoTitle";
            lblDecoTitle.Padding = new Padding(20, 60, 20, 0);
            lblDecoTitle.Size = new Size(340, 250);
            lblDecoTitle.TabIndex = 0;
            lblDecoTitle.Text = "🎱\r\nĐĂNG KÝ";
            lblDecoTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlRight
            // 
            pnlRight.BackColor = Color.White;
            pnlRight.Controls.Add(pnlMain);
            pnlRight.Dock = DockStyle.Fill;
            pnlRight.Location = new Point(394, 0);
            pnlRight.Name = "pnlRight";
            pnlRight.Size = new Size(476, 820);
            pnlRight.TabIndex = 1;
            // 
            // pnlMain
            // 
            pnlMain.AutoScroll = true;
            pnlMain.Controls.Add(btnClose);
            pnlMain.Controls.Add(lblTitle);
            pnlMain.Controls.Add(btnBackToLogin);
            pnlMain.Controls.Add(btnSignup);
            pnlMain.Controls.Add(chkShowPassword);
            pnlMain.Controls.Add(txtXacNhanMatKhau);
            pnlMain.Controls.Add(lblXacNhanMatKhau);
            pnlMain.Controls.Add(txtMatKhau);
            pnlMain.Controls.Add(lblMatKhau);
            pnlMain.Controls.Add(dtpNgaySinh);
            pnlMain.Controls.Add(lblNgaySinh);
            pnlMain.Controls.Add(txtEmail);
            pnlMain.Controls.Add(lblEmail);
            pnlMain.Controls.Add(txtSDT);
            pnlMain.Controls.Add(lblSDT);
            pnlMain.Controls.Add(txtTenKH);
            pnlMain.Controls.Add(lblTenKH);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(0, 0);
            pnlMain.Name = "pnlMain";
            pnlMain.Padding = new Padding(50, 70, 50, 30);
            pnlMain.Size = new Size(476, 820);
            pnlMain.TabIndex = 0;
            pnlMain.Paint += PnlMain_Paint;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.Transparent;
            btnClose.Cursor = Cursors.Hand;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            btnClose.ForeColor = Color.Gray;
            btnClose.Location = new Point(407, 12);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(40, 62);
            btnClose.TabIndex = 10;
            btnClose.TabStop = false;
            btnClose.Text = "✕";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += BtnClose_Click;
            btnClose.MouseEnter += BtnClose_MouseEnter;
            btnClose.MouseLeave += BtnClose_MouseLeave;
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.MidnightBlue;
            lblTitle.Location = new Point(41, 24);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(373, 50);
            lblTitle.TabIndex = 13;
            lblTitle.Text = "ĐĂNG KÝ";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnBackToLogin
            // 
            btnBackToLogin.BackColor = Color.FromArgb(241, 245, 249);
            btnBackToLogin.Cursor = Cursors.Hand;
            btnBackToLogin.FlatAppearance.BorderSize = 0;
            btnBackToLogin.FlatStyle = FlatStyle.Flat;
            btnBackToLogin.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnBackToLogin.ForeColor = Color.FromArgb(51, 65, 85);
            btnBackToLogin.Location = new Point(41, 746);
            btnBackToLogin.Name = "btnBackToLogin";
            btnBackToLogin.Size = new Size(380, 45);
            btnBackToLogin.TabIndex = 8;
            btnBackToLogin.Text = "← Đã có tài khoản? Đăng nhập";
            btnBackToLogin.UseVisualStyleBackColor = false;
            btnBackToLogin.Click += BtnBackToLogin_Click;
            // 
            // btnSignup
            // 
            btnSignup.BackColor = Color.SeaGreen;
            btnSignup.Cursor = Cursors.Hand;
            btnSignup.FlatAppearance.BorderSize = 0;
            btnSignup.FlatStyle = FlatStyle.Flat;
            btnSignup.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnSignup.ForeColor = Color.White;
            btnSignup.Location = new Point(41, 676);
            btnSignup.Name = "btnSignup";
            btnSignup.Size = new Size(380, 50);
            btnSignup.TabIndex = 7;
            btnSignup.Text = "Đăng ký";
            btnSignup.UseVisualStyleBackColor = false;
            btnSignup.Click += BtnSignup_Click;
            // 
            // chkShowPassword
            // 
            chkShowPassword.AutoSize = true;
            chkShowPassword.Font = new Font("Segoe UI", 9F);
            chkShowPassword.ForeColor = Color.FromArgb(100, 116, 139);
            chkShowPassword.Location = new Point(41, 626);
            chkShowPassword.Name = "chkShowPassword";
            chkShowPassword.Size = new Size(183, 29);
            chkShowPassword.TabIndex = 6;
            chkShowPassword.Text = "👁️ Hiện mật khẩu";
            chkShowPassword.UseVisualStyleBackColor = true;
            chkShowPassword.CheckedChanged += ChkShowPassword_CheckedChanged;
            // 
            // txtXacNhanMatKhau
            // 
            txtXacNhanMatKhau.BorderStyle = BorderStyle.FixedSingle;
            txtXacNhanMatKhau.Font = new Font("Segoe UI", 11F);
            txtXacNhanMatKhau.Location = new Point(41, 576);
            txtXacNhanMatKhau.Name = "txtXacNhanMatKhau";
            txtXacNhanMatKhau.PlaceholderText = "Nhập lại mật khẩu";
            txtXacNhanMatKhau.Size = new Size(380, 37);
            txtXacNhanMatKhau.TabIndex = 5;
            txtXacNhanMatKhau.UseSystemPasswordChar = true;
            txtXacNhanMatKhau.KeyPress += TxtXacNhanMatKhau_KeyPress;
            // 
            // lblXacNhanMatKhau
            // 
            lblXacNhanMatKhau.AutoSize = true;
            lblXacNhanMatKhau.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblXacNhanMatKhau.ForeColor = Color.FromArgb(51, 65, 85);
            lblXacNhanMatKhau.Location = new Point(41, 546);
            lblXacNhanMatKhau.Name = "lblXacNhanMatKhau";
            lblXacNhanMatKhau.Size = new Size(243, 28);
            lblXacNhanMatKhau.TabIndex = 12;
            lblXacNhanMatKhau.Text = "🔐 Xác nhận mật khẩu *";
            // 
            // txtMatKhau
            // 
            txtMatKhau.BorderStyle = BorderStyle.FixedSingle;
            txtMatKhau.Font = new Font("Segoe UI", 11F);
            txtMatKhau.Location = new Point(41, 496);
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.PlaceholderText = "Tối thiểu 6 ký tự";
            txtMatKhau.Size = new Size(380, 37);
            txtMatKhau.TabIndex = 4;
            txtMatKhau.UseSystemPasswordChar = true;
            // 
            // lblMatKhau
            // 
            lblMatKhau.AutoSize = true;
            lblMatKhau.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblMatKhau.ForeColor = Color.FromArgb(51, 65, 85);
            lblMatKhau.Location = new Point(41, 466);
            lblMatKhau.Name = "lblMatKhau";
            lblMatKhau.Size = new Size(151, 28);
            lblMatKhau.TabIndex = 10;
            lblMatKhau.Text = "🔒 Mật khẩu *";
            // 
            // dtpNgaySinh
            // 
            dtpNgaySinh.Font = new Font("Segoe UI", 11F);
            dtpNgaySinh.Format = DateTimePickerFormat.Short;
            dtpNgaySinh.Location = new Point(41, 416);
            dtpNgaySinh.Name = "dtpNgaySinh";
            dtpNgaySinh.Size = new Size(380, 37);
            dtpNgaySinh.TabIndex = 3;
            // 
            // lblNgaySinh
            // 
            lblNgaySinh.AutoSize = true;
            lblNgaySinh.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblNgaySinh.ForeColor = Color.FromArgb(51, 65, 85);
            lblNgaySinh.Location = new Point(41, 386);
            lblNgaySinh.Name = "lblNgaySinh";
            lblNgaySinh.Size = new Size(244, 28);
            lblNgaySinh.TabIndex = 8;
            lblNgaySinh.Text = "🎂 Ngày sinh (tùy chọn)";
            // 
            // txtEmail
            // 
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.Font = new Font("Segoe UI", 11F);
            txtEmail.Location = new Point(41, 336);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "email@example.com";
            txtEmail.Size = new Size(380, 37);
            txtEmail.TabIndex = 2;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblEmail.ForeColor = Color.FromArgb(51, 65, 85);
            lblEmail.Location = new Point(41, 306);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(113, 28);
            lblEmail.TabIndex = 6;
            lblEmail.Text = "📧 Email *";
            // 
            // txtSDT
            // 
            txtSDT.BorderStyle = BorderStyle.FixedSingle;
            txtSDT.Font = new Font("Segoe UI", 11F);
            txtSDT.Location = new Point(41, 256);
            txtSDT.MaxLength = 11;
            txtSDT.Name = "txtSDT";
            txtSDT.PlaceholderText = "0909123456";
            txtSDT.Size = new Size(380, 37);
            txtSDT.TabIndex = 1;
            // 
            // lblSDT
            // 
            lblSDT.AutoSize = true;
            lblSDT.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblSDT.ForeColor = Color.FromArgb(51, 65, 85);
            lblSDT.Location = new Point(41, 226);
            lblSDT.Name = "lblSDT";
            lblSDT.Size = new Size(187, 28);
            lblSDT.TabIndex = 4;
            lblSDT.Text = "📱 Số điện thoại *";
            // 
            // txtTenKH
            // 
            txtTenKH.BorderStyle = BorderStyle.FixedSingle;
            txtTenKH.Font = new Font("Segoe UI", 11F);
            txtTenKH.Location = new Point(41, 176);
            txtTenKH.Name = "txtTenKH";
            txtTenKH.PlaceholderText = "Nguyễn Văn A";
            txtTenKH.Size = new Size(380, 37);
            txtTenKH.TabIndex = 0;
            // 
            // lblTenKH
            // 
            lblTenKH.AutoSize = true;
            lblTenKH.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTenKH.ForeColor = Color.FromArgb(51, 65, 85);
            lblTenKH.Location = new Point(41, 146);
            lblTenKH.Name = "lblTenKH";
            lblTenKH.Size = new Size(155, 28);
            lblTenKH.TabIndex = 2;
            lblTenKH.Text = "👤 Họ và Tên *";
            // 
            // SignupForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(870, 820);
            Controls.Add(pnlRight);
            Controls.Add(pnlLeft);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.None;
            Name = "SignupForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng ký - Quán Bi-a Pro";
            Load += SignupForm_Load;
            pnlLeft.ResumeLayout(false);
            pnlDecoration.ResumeLayout(false);
            pnlRight.ResumeLayout(false);
            pnlMain.ResumeLayout(false);
            pnlMain.PerformLayout();
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
        private Label lblTenKH;
        private TextBox txtTenKH;
        private Label lblSDT;
        private TextBox txtSDT;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblNgaySinh;
        private DateTimePicker dtpNgaySinh;
        private Label lblMatKhau;
        private TextBox txtMatKhau;
        private Label lblXacNhanMatKhau;
        private TextBox txtXacNhanMatKhau;
        private CheckBox chkShowPassword;
        private Button btnSignup;
        private Button btnBackToLogin;
        private Button btnClose;
    }
}