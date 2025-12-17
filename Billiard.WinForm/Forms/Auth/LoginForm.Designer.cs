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
            btnClose = new Button();
            lblSignup = new Label();
            lblForgotPassword = new Label();
            btnLogin = new Button();
            pnlPassword = new Panel();
            txtPassword = new TextBox();
            btnTogglePassword = new Button();
            lblPassword = new Label();
            txtUsername = new TextBox();
            lblUsername = new Label();
            lblSubtitle = new Label();
            lblTitle = new Label();
            pnlLeft.SuspendLayout();
            pnlDecoration.SuspendLayout();
            pnlRight.SuspendLayout();
            pnlMain.SuspendLayout();
            pnlPassword.SuspendLayout();
            SuspendLayout();
            // 
            // pnlLeft
            // 
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
            pnlRight.Dock = DockStyle.Fill;
            pnlRight.Location = new Point(394, 0);
            pnlRight.Name = "pnlRight";
            pnlRight.Size = new Size(456, 550);
            pnlRight.TabIndex = 1;
            // 
            // pnlMain
            // 
            pnlMain.Controls.Add(btnClose);
            pnlMain.Controls.Add(lblSignup);
            pnlMain.Controls.Add(lblForgotPassword);
            pnlMain.Controls.Add(btnLogin);
            pnlMain.Controls.Add(pnlPassword);
            pnlMain.Controls.Add(lblPassword);
            pnlMain.Controls.Add(txtUsername);
            pnlMain.Controls.Add(lblUsername);
            pnlMain.Controls.Add(lblSubtitle);
            pnlMain.Controls.Add(lblTitle);
            pnlMain.Location = new Point(21, 12);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(423, 535);
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
            btnClose.Location = new Point(385, -12);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(50, 49);
            btnClose.TabIndex = 10;
            btnClose.TabStop = false;
            btnClose.Text = "✕";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += BtnClose_Click;
            btnClose.MouseEnter += BtnClose_MouseEnter;
            btnClose.MouseLeave += BtnClose_MouseLeave;
            // 
            // lblSignup
            // 
            lblSignup.Cursor = Cursors.Hand;
            lblSignup.Font = new Font("Segoe UI", 9.5F, FontStyle.Underline);
            lblSignup.ForeColor = Color.FromArgb(99, 102, 241);
            lblSignup.Location = new Point(51, 468);
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
            lblForgotPassword.Location = new Point(51, 438);
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
            btnLogin.Location = new Point(51, 378);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(300, 50);
            btnLogin.TabIndex = 3;
            btnLogin.Text = "Đăng nhập";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += BtnLogin_Click;
            btnLogin.MouseEnter += BtnLogin_MouseEnter;
            btnLogin.MouseLeave += BtnLogin_MouseLeave;
            // 
            // pnlPassword
            // 
            pnlPassword.BackColor = Color.White;
            pnlPassword.BorderStyle = BorderStyle.FixedSingle;
            pnlPassword.Controls.Add(txtPassword);
            pnlPassword.Controls.Add(btnTogglePassword);
            pnlPassword.Location = new Point(51, 294);
            pnlPassword.Name = "pnlPassword";
            pnlPassword.Size = new Size(300, 42);
            pnlPassword.TabIndex = 1;
            // 
            // txtPassword
            // 
            txtPassword.BorderStyle = BorderStyle.None;
            txtPassword.Font = new Font("Segoe UI", 11F);
            txtPassword.Location = new Point(8, 8);
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "Nhập mật khẩu";
            txtPassword.Size = new Size(245, 30);
            txtPassword.TabIndex = 0;
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.KeyPress += TxtPassword_KeyPress;
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
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPassword.ForeColor = Color.FromArgb(51, 65, 85);
            lblPassword.Location = new Point(51, 264);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(117, 28);
            lblPassword.TabIndex = 5;
            lblPassword.Text = "Mật khẩu *";
            // 
            // txtUsername
            // 
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            txtUsername.Font = new Font("Segoe UI", 11F);
            txtUsername.Location = new Point(51, 209);
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
            lblUsername.Location = new Point(51, 179);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(226, 28);
            lblUsername.TabIndex = 2;
            lblUsername.Text = "Số điện thoại / Email *";
            // 
            // lblSubtitle
            // 
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.FromArgb(100, 116, 139);
            lblSubtitle.Location = new Point(51, 121);
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
            lblTitle.Location = new Point(10, 13);
            lblTitle.Margin = new Padding(0, 0, 3, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(383, 121);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "CHÀO MỪNG\r\nĐẾN BILLARD PRO";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
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
            pnlPassword.ResumeLayout(false);
            pnlPassword.PerformLayout();
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
        private Panel pnlPassword;
        private TextBox txtPassword;
        private Button btnTogglePassword;
        private Button btnLogin;
        private Label lblForgotPassword;
        private Label lblSignup;
        private Button btnClose;
    }
}