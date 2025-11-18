namespace Billiard.WinForm.Forms.Auth
{
    partial class LoginForm
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
            lblSignup = new Label();
            lblForgotPassword = new Label();
            btnLogin = new Button();
            chkShowPassword = new CheckBox();
            chkRemember = new CheckBox();
            txtPassword = new TextBox();
            lblPassword = new Label();
            txtUsername = new TextBox();
            lblUsername = new Label();
            lblSubtitle = new Label();
            lblTitle = new Label();
            btnClose = new Button();
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
            pnlLeft.Size = new Size(394, 550);
            pnlLeft.TabIndex = 0;
            // 
            // pnlDecoration
            // 
            pnlDecoration.BackColor = Color.MidnightBlue;
            pnlDecoration.Controls.Add(lblDecoSubtitle);
            pnlDecoration.Controls.Add(lblDecoTitle);
            pnlDecoration.Location = new Point(22, 25);
            pnlDecoration.Name = "pnlDecoration";
            pnlDecoration.Size = new Size(340, 500);
            pnlDecoration.TabIndex = 0;
            // 
            // lblDecoSubtitle
            // 
            lblDecoSubtitle.Dock = DockStyle.Bottom;
            lblDecoSubtitle.Font = new Font("Segoe UI", 11F);
            lblDecoSubtitle.ForeColor = Color.White;
            lblDecoSubtitle.Location = new Point(0, 350);
            lblDecoSubtitle.Name = "lblDecoSubtitle";
            lblDecoSubtitle.Padding = new Padding(30, 0, 30, 40);
            lblDecoSubtitle.Size = new Size(340, 150);
            lblDecoSubtitle.TabIndex = 1;
            lblDecoSubtitle.Text = "Hệ thống quản lý quán Billiard\r\nChuyên nghiệp & Hiện đại\r\n\r\n📍 Địa chỉ: 123 Đường ABC, TP.HCM\r\n📞 Hotline: 0909 123 456";
            lblDecoSubtitle.TextAlign = ContentAlignment.TopCenter;
            // 
            // lblDecoTitle
            // 
            lblDecoTitle.Dock = DockStyle.Top;
            lblDecoTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDecoTitle.ForeColor = Color.White;
            lblDecoTitle.Location = new Point(0, 0);
            lblDecoTitle.Name = "lblDecoTitle";
            lblDecoTitle.Padding = new Padding(20, 40, 20, 0);
            lblDecoTitle.Size = new Size(340, 257);
            lblDecoTitle.TabIndex = 0;
            lblDecoTitle.Text = "🎱\r\nBILLARD PRO";
            lblDecoTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlRight
            // 
            pnlRight.BackColor = Color.White;
            pnlRight.Controls.Add(pnlMain);
            pnlRight.Controls.Add(btnClose);
            pnlRight.Dock = DockStyle.Fill;
            pnlRight.Location = new Point(394, 0);
            pnlRight.Name = "pnlRight";
            pnlRight.Size = new Size(456, 550);
            pnlRight.TabIndex = 1;
            // 
            // pnlMain
            // 
            pnlMain.Controls.Add(lblSignup);
            pnlMain.Controls.Add(lblForgotPassword);
            pnlMain.Controls.Add(btnLogin);
            pnlMain.Controls.Add(chkShowPassword);
            pnlMain.Controls.Add(chkRemember);
            pnlMain.Controls.Add(txtPassword);
            pnlMain.Controls.Add(lblPassword);
            pnlMain.Controls.Add(txtUsername);
            pnlMain.Controls.Add(lblUsername);
            pnlMain.Controls.Add(lblSubtitle);
            pnlMain.Controls.Add(lblTitle);
            pnlMain.Location = new Point(21, 25);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(400, 500);
            pnlMain.TabIndex = 0;
            pnlMain.Paint += PnlMain_Paint;
            // 
            // lblSignup
            // 
            lblSignup.Cursor = Cursors.Hand;
            lblSignup.Font = new Font("Segoe UI", 9.5F, FontStyle.Underline);
            lblSignup.ForeColor = Color.FromArgb(99, 102, 241);
            lblSignup.Location = new Point(50, 465);
            lblSignup.Name = "lblSignup";
            lblSignup.Size = new Size(300, 25);
            lblSignup.TabIndex = 7;
            lblSignup.Text = "Chưa có tài khoản? Đăng ký ngay →";
            lblSignup.TextAlign = ContentAlignment.MiddleCenter;
            lblSignup.Click += LblSignup_Click;
            // 
            // lblForgotPassword
            // 
            lblForgotPassword.Cursor = Cursors.Hand;
            lblForgotPassword.Font = new Font("Segoe UI", 9F, FontStyle.Underline);
            lblForgotPassword.ForeColor = Color.FromArgb(99, 102, 241);
            lblForgotPassword.Location = new Point(50, 435);
            lblForgotPassword.Name = "lblForgotPassword";
            lblForgotPassword.Size = new Size(300, 25);
            lblForgotPassword.TabIndex = 6;
            lblForgotPassword.Text = "Quên mật khẩu?";
            lblForgotPassword.TextAlign = ContentAlignment.MiddleCenter;
            lblForgotPassword.Click += LblForgotPassword_Click;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.SeaGreen;
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(50, 375);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(300, 50);
            btnLogin.TabIndex = 4;
            btnLogin.Text = "Đăng nhập";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += BtnLogin_Click;
            btnLogin.MouseEnter += BtnLogin_MouseEnter;
            btnLogin.MouseLeave += BtnLogin_MouseLeave;
            // 
            // chkShowPassword
            // 
            chkShowPassword.AutoSize = true;
            chkShowPassword.Font = new Font("Segoe UI", 9F);
            chkShowPassword.ForeColor = Color.FromArgb(100, 116, 139);
            chkShowPassword.Location = new Point(200, 340);
            chkShowPassword.Name = "chkShowPassword";
            chkShowPassword.Size = new Size(183, 29);
            chkShowPassword.TabIndex = 3;
            chkShowPassword.TabStop = false;
            chkShowPassword.Text = "👁️ Hiện mật khẩu";
            chkShowPassword.UseVisualStyleBackColor = true;
            chkShowPassword.CheckedChanged += ChkShowPassword_CheckedChanged;
            // 
            // chkRemember
            // 
            chkRemember.AutoSize = true;
            chkRemember.Font = new Font("Segoe UI", 9F);
            chkRemember.ForeColor = Color.FromArgb(100, 116, 139);
            chkRemember.Location = new Point(50, 340);
            chkRemember.Name = "chkRemember";
            chkRemember.Size = new Size(156, 29);
            chkRemember.TabIndex = 2;
            chkRemember.TabStop = false;
            chkRemember.Text = "💾 Ghi nhớ tôi";
            chkRemember.UseVisualStyleBackColor = true;
            // 
            // txtPassword
            // 
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Font = new Font("Segoe UI", 11F);
            txtPassword.Location = new Point(50, 300);
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "Nhập mật khẩu";
            txtPassword.Size = new Size(300, 37);
            txtPassword.TabIndex = 1;
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.KeyPress += TxtPassword_KeyPress;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPassword.ForeColor = Color.FromArgb(51, 65, 85);
            lblPassword.Location = new Point(50, 270);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(151, 28);
            lblPassword.TabIndex = 5;
            lblPassword.Text = "🔒 Mật khẩu *";
            // 
            // txtUsername
            // 
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            txtUsername.Font = new Font("Segoe UI", 11F);
            txtUsername.Location = new Point(50, 220);
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "Nhập SĐT hoặc Email";
            txtUsername.Size = new Size(300, 37);
            txtUsername.TabIndex = 0;
            txtUsername.KeyPress += TxtUsername_KeyPress;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblUsername.ForeColor = Color.FromArgb(51, 65, 85);
            lblUsername.Location = new Point(50, 190);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(260, 28);
            lblUsername.TabIndex = 2;
            lblUsername.Text = "📱 Số điện thoại / Email *";
            // 
            // lblSubtitle
            // 
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.FromArgb(100, 116, 139);
            lblSubtitle.Location = new Point(50, 130);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(300, 50);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Đăng nhập để trải nghiệm dịch vụ tốt nhất";
            lblSubtitle.TextAlign = ContentAlignment.TopCenter;
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.MidnightBlue;
            lblTitle.Location = new Point(10, 18);
            lblTitle.Margin = new Padding(0, 0, 3, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(383, 112);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "CHÀO MỪNG\r\nĐẾN BILLARD PRO";
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
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(850, 550);
            Controls.Add(pnlRight);
            Controls.Add(pnlLeft);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.None;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng nhập - Quán Bi-a Pro";
            Load += LoginForm_Load;
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
        private Label lblSubtitle;
        private Label lblUsername;
        private TextBox txtUsername;
        private Label lblPassword;
        private TextBox txtPassword;
        private CheckBox chkRemember;
        private CheckBox chkShowPassword;
        private Button btnLogin;
        private Label lblForgotPassword;
        private Label lblSignup;
        private Button btnClose;
    }
}