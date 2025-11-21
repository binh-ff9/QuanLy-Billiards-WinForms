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
            // Password fields with toggle
            pnlXacNhanMatKhau = new Panel();
            txtXacNhanMatKhau = new TextBox();
            btnToggleConfirm = new Button();
            lblXacNhanMatKhau = new Label();
            pnlMatKhau = new Panel();
            txtMatKhau = new TextBox();
            btnTogglePassword = new Button();
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
            pnlMatKhau.SuspendLayout();
            pnlXacNhanMatKhau.SuspendLayout();
            SuspendLayout();
            // 
            // pnlLeft
            // 
            pnlLeft.BackColor = Color.FromArgb(248, 250, 252);
            pnlLeft.Controls.Add(pnlDecoration);
            pnlLeft.Dock = DockStyle.Left;
            pnlLeft.Location = new Point(0, 0);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Size = new Size(394, 750);
            pnlLeft.TabIndex = 0;
            // 
            // pnlDecoration
            // 
            pnlDecoration.BackColor = Color.MidnightBlue;
            pnlDecoration.Controls.Add(lblDecoSubtitle);
            pnlDecoration.Controls.Add(lblDecoTitle);
            pnlDecoration.Location = new Point(21, 24);
            pnlDecoration.Name = "pnlDecoration";
            pnlDecoration.Size = new Size(340, 700);
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
            lblDecoSubtitle.Size = new Size(340, 403);
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
            pnlRight.Size = new Size(476, 750);
            pnlRight.TabIndex = 1;
            // 
            // pnlMain
            // 
            pnlMain.AutoScroll = true;
            pnlMain.Controls.Add(btnClose);
            pnlMain.Controls.Add(lblTitle);
            pnlMain.Controls.Add(btnBackToLogin);
            pnlMain.Controls.Add(btnSignup);
            pnlMain.Controls.Add(pnlXacNhanMatKhau);
            pnlMain.Controls.Add(lblXacNhanMatKhau);
            pnlMain.Controls.Add(pnlMatKhau);
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
            pnlMain.Size = new Size(476, 750);
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
            btnClose.Size = new Size(40, 46);
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
            lblTitle.Text = "ĐĂNG KÝ THÀNH VIÊN";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTenKH
            // 
            lblTenKH.AutoSize = true;
            lblTenKH.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTenKH.ForeColor = Color.FromArgb(51, 65, 85);
            lblTenKH.Location = new Point(41, 90);
            lblTenKH.Name = "lblTenKH";
            lblTenKH.Size = new Size(155, 28);
            lblTenKH.TabIndex = 2;
            lblTenKH.Text = "👤 Họ và Tên *";
            // 
            // txtTenKH
            // 
            txtTenKH.BorderStyle = BorderStyle.FixedSingle;
            txtTenKH.Font = new Font("Segoe UI", 11F);
            txtTenKH.Location = new Point(41, 120);
            txtTenKH.Name = "txtTenKH";
            txtTenKH.PlaceholderText = "Nguyễn Văn A";
            txtTenKH.Size = new Size(380, 37);
            txtTenKH.TabIndex = 0;
            // 
            // lblSDT
            // 
            lblSDT.AutoSize = true;
            lblSDT.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblSDT.ForeColor = Color.FromArgb(51, 65, 85);
            lblSDT.Location = new Point(41, 170);
            lblSDT.Name = "lblSDT";
            lblSDT.Size = new Size(187, 28);
            lblSDT.TabIndex = 4;
            lblSDT.Text = "📱 Số điện thoại *";
            // 
            // txtSDT
            // 
            txtSDT.BorderStyle = BorderStyle.FixedSingle;
            txtSDT.Font = new Font("Segoe UI", 11F);
            txtSDT.Location = new Point(41, 200);
            txtSDT.MaxLength = 11;
            txtSDT.Name = "txtSDT";
            txtSDT.PlaceholderText = "0909123456";
            txtSDT.Size = new Size(380, 37);
            txtSDT.TabIndex = 1;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblEmail.ForeColor = Color.FromArgb(51, 65, 85);
            lblEmail.Location = new Point(41, 250);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(113, 28);
            lblEmail.TabIndex = 6;
            lblEmail.Text = "📧 Email *";
            // 
            // txtEmail
            // 
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.Font = new Font("Segoe UI", 11F);
            txtEmail.Location = new Point(41, 280);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "email@example.com";
            txtEmail.Size = new Size(380, 37);
            txtEmail.TabIndex = 2;
            // 
            // lblNgaySinh
            // 
            lblNgaySinh.AutoSize = true;
            lblNgaySinh.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblNgaySinh.ForeColor = Color.FromArgb(51, 65, 85);
            lblNgaySinh.Location = new Point(41, 330);
            lblNgaySinh.Name = "lblNgaySinh";
            lblNgaySinh.Size = new Size(244, 28);
            lblNgaySinh.TabIndex = 8;
            lblNgaySinh.Text = "🎂 Ngày sinh (tùy chọn)";
            // 
            // dtpNgaySinh
            // 
            dtpNgaySinh.Font = new Font("Segoe UI", 11F);
            dtpNgaySinh.Format = DateTimePickerFormat.Short;
            dtpNgaySinh.Location = new Point(41, 360);
            dtpNgaySinh.Name = "dtpNgaySinh";
            dtpNgaySinh.Size = new Size(380, 37);
            dtpNgaySinh.TabIndex = 3;
            // 
            // lblMatKhau
            // 
            lblMatKhau.AutoSize = true;
            lblMatKhau.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblMatKhau.ForeColor = Color.FromArgb(51, 65, 85);
            lblMatKhau.Location = new Point(41, 410);
            lblMatKhau.Name = "lblMatKhau";
            lblMatKhau.Size = new Size(151, 28);
            lblMatKhau.TabIndex = 10;
            lblMatKhau.Text = "🔒 Mật khẩu *";
            // 
            // pnlMatKhau - Container for password with eye icon
            // 
            pnlMatKhau.BackColor = Color.White;
            pnlMatKhau.BorderStyle = BorderStyle.FixedSingle;
            pnlMatKhau.Controls.Add(txtMatKhau);
            pnlMatKhau.Controls.Add(btnTogglePassword);
            pnlMatKhau.Location = new Point(41, 440);
            pnlMatKhau.Name = "pnlMatKhau";
            pnlMatKhau.Size = new Size(380, 42);
            pnlMatKhau.TabIndex = 4;
            // 
            // txtMatKhau
            // 
            txtMatKhau.BorderStyle = BorderStyle.None;
            txtMatKhau.Font = new Font("Segoe UI", 11F);
            txtMatKhau.Location = new Point(8, 8);
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.PlaceholderText = "Tối thiểu 6 ký tự";
            txtMatKhau.Size = new Size(325, 30);
            txtMatKhau.TabIndex = 0;
            txtMatKhau.UseSystemPasswordChar = true;
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
            btnTogglePassword.Location = new Point(335, 2);
            btnTogglePassword.Name = "btnTogglePassword";
            btnTogglePassword.Size = new Size(40, 36);
            btnTogglePassword.TabIndex = 1;
            btnTogglePassword.TabStop = false;
            btnTogglePassword.Text = "👁";
            btnTogglePassword.UseVisualStyleBackColor = false;
            btnTogglePassword.Click += BtnTogglePassword_Click;
            // 
            // lblXacNhanMatKhau
            // 
            lblXacNhanMatKhau.AutoSize = true;
            lblXacNhanMatKhau.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblXacNhanMatKhau.ForeColor = Color.FromArgb(51, 65, 85);
            lblXacNhanMatKhau.Location = new Point(41, 495);
            lblXacNhanMatKhau.Name = "lblXacNhanMatKhau";
            lblXacNhanMatKhau.Size = new Size(243, 28);
            lblXacNhanMatKhau.TabIndex = 12;
            lblXacNhanMatKhau.Text = "🔐 Xác nhận mật khẩu *";
            // 
            // pnlXacNhanMatKhau - Container for confirm password with eye icon
            // 
            pnlXacNhanMatKhau.BackColor = Color.White;
            pnlXacNhanMatKhau.BorderStyle = BorderStyle.FixedSingle;
            pnlXacNhanMatKhau.Controls.Add(txtXacNhanMatKhau);
            pnlXacNhanMatKhau.Controls.Add(btnToggleConfirm);
            pnlXacNhanMatKhau.Location = new Point(41, 525);
            pnlXacNhanMatKhau.Name = "pnlXacNhanMatKhau";
            pnlXacNhanMatKhau.Size = new Size(380, 42);
            pnlXacNhanMatKhau.TabIndex = 5;
            // 
            // txtXacNhanMatKhau
            // 
            txtXacNhanMatKhau.BorderStyle = BorderStyle.None;
            txtXacNhanMatKhau.Font = new Font("Segoe UI", 11F);
            txtXacNhanMatKhau.Location = new Point(8, 8);
            txtXacNhanMatKhau.Name = "txtXacNhanMatKhau";
            txtXacNhanMatKhau.PlaceholderText = "Nhập lại mật khẩu";
            txtXacNhanMatKhau.Size = new Size(325, 30);
            txtXacNhanMatKhau.TabIndex = 0;
            txtXacNhanMatKhau.UseSystemPasswordChar = true;
            txtXacNhanMatKhau.KeyPress += TxtXacNhanMatKhau_KeyPress;
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
            btnToggleConfirm.Location = new Point(335, 2);
            btnToggleConfirm.Name = "btnToggleConfirm";
            btnToggleConfirm.Size = new Size(40, 36);
            btnToggleConfirm.TabIndex = 1;
            btnToggleConfirm.TabStop = false;
            btnToggleConfirm.Text = "👁";
            btnToggleConfirm.UseVisualStyleBackColor = false;
            btnToggleConfirm.Click += BtnToggleConfirm_Click;
            // 
            // btnSignup
            // 
            btnSignup.BackColor = Color.SeaGreen;
            btnSignup.Cursor = Cursors.Hand;
            btnSignup.FlatAppearance.BorderSize = 0;
            btnSignup.FlatStyle = FlatStyle.Flat;
            btnSignup.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnSignup.ForeColor = Color.White;
            btnSignup.Location = new Point(41, 590);
            btnSignup.Name = "btnSignup";
            btnSignup.Size = new Size(380, 50);
            btnSignup.TabIndex = 6;
            btnSignup.Text = "Đăng ký";
            btnSignup.UseVisualStyleBackColor = false;
            btnSignup.Click += BtnSignup_Click;
            // 
            // btnBackToLogin
            // 
            btnBackToLogin.BackColor = Color.FromArgb(241, 245, 249);
            btnBackToLogin.Cursor = Cursors.Hand;
            btnBackToLogin.FlatAppearance.BorderSize = 0;
            btnBackToLogin.FlatStyle = FlatStyle.Flat;
            btnBackToLogin.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnBackToLogin.ForeColor = Color.FromArgb(51, 65, 85);
            btnBackToLogin.Location = new Point(41, 660);
            btnBackToLogin.Name = "btnBackToLogin";
            btnBackToLogin.Size = new Size(380, 45);
            btnBackToLogin.TabIndex = 7;
            btnBackToLogin.Text = "← Đã có tài khoản? Đăng nhập";
            btnBackToLogin.UseVisualStyleBackColor = false;
            btnBackToLogin.Click += BtnBackToLogin_Click;
            // 
            // SignupForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(870, 750);
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
            pnlMatKhau.ResumeLayout(false);
            pnlMatKhau.PerformLayout();
            pnlXacNhanMatKhau.ResumeLayout(false);
            pnlXacNhanMatKhau.PerformLayout();
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
        private Panel pnlMatKhau;
        private TextBox txtMatKhau;
        private Button btnTogglePassword;
        private Label lblXacNhanMatKhau;
        private Panel pnlXacNhanMatKhau;
        private TextBox txtXacNhanMatKhau;
        private Button btnToggleConfirm;
        private Button btnSignup;
        private Button btnBackToLogin;
        private Button btnClose;
    }
}