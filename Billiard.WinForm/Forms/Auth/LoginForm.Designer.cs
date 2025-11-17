namespace Billiard.WinForm
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
            pnlMain = new Panel();
            lblSignup = new Label();
            lblForgotPassword = new Label();
            btnLogin = new Button();
            chkRemember = new CheckBox();
            txtPassword = new TextBox();
            lblPassword = new Label();
            txtUsername = new TextBox();
            lblUsername = new Label();
            lblSubtitle = new Label();
            lblTitle = new Label();
            btnClose = new Button();
            pnlMain.SuspendLayout();
            SuspendLayout();
            // 
            // pnlMain
            // 
            pnlMain.BackColor = Color.White;
            pnlMain.Controls.Add(lblSignup);
            pnlMain.Controls.Add(lblForgotPassword);
            pnlMain.Controls.Add(btnLogin);
            pnlMain.Controls.Add(chkRemember);
            pnlMain.Controls.Add(txtPassword);
            pnlMain.Controls.Add(lblPassword);
            pnlMain.Controls.Add(txtUsername);
            pnlMain.Controls.Add(lblUsername);
            pnlMain.Controls.Add(lblSubtitle);
            pnlMain.Controls.Add(lblTitle);
            pnlMain.Location = new Point(0, -3);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(504, 654);
            pnlMain.TabIndex = 0;
            pnlMain.Paint += PnlMain_Paint;
            // 
            // lblSignup
            // 
            lblSignup.Cursor = Cursors.Hand;
            lblSignup.Font = new Font("Segoe UI", 9F);
            lblSignup.ForeColor = Color.FromArgb(99, 102, 241);
            lblSignup.Location = new Point(95, 524);
            lblSignup.Name = "lblSignup";
            lblSignup.Size = new Size(311, 37);
            lblSignup.TabIndex = 6;
            lblSignup.TabStop = false;
            lblSignup.Text = "Chưa có tài khoản? Đăng ký ngay";
            lblSignup.TextAlign = ContentAlignment.MiddleCenter;
            lblSignup.Click += LblSignup_Click;
            lblSignup.MouseEnter += LblSignup_MouseEnter;
            lblSignup.MouseLeave += LblSignup_MouseLeave;
            // 
            // lblForgotPassword
            // 
            lblForgotPassword.Cursor = Cursors.Hand;
            lblForgotPassword.Font = new Font("Segoe UI", 9F, FontStyle.Underline);
            lblForgotPassword.ForeColor = Color.FromArgb(99, 102, 241);
            lblForgotPassword.Location = new Point(95, 494);
            lblForgotPassword.Name = "lblForgotPassword";
            lblForgotPassword.Size = new Size(311, 30);
            lblForgotPassword.TabIndex = 5;
            lblForgotPassword.TabStop = false;
            lblForgotPassword.Text = "Quên mật khẩu?";
            lblForgotPassword.TextAlign = ContentAlignment.MiddleCenter;
            lblForgotPassword.Click += LblForgotPassword_Click;
            lblForgotPassword.MouseEnter += LblForgotPassword_MouseEnter;
            lblForgotPassword.MouseLeave += LblForgotPassword_MouseLeave;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(99, 102, 241);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(95, 434);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(300, 45);
            btnLogin.TabIndex = 3;
            btnLogin.Text = "Đăng nhập";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += BtnLogin_Click;
            btnLogin.MouseEnter += BtnLogin_MouseEnter;
            btnLogin.MouseLeave += BtnLogin_MouseLeave;
            // 
            // chkRemember
            // 
            chkRemember.AutoSize = true;
            chkRemember.Font = new Font("Segoe UI", 9F);
            chkRemember.ForeColor = Color.FromArgb(71, 85, 105);
            chkRemember.Location = new Point(95, 394);
            chkRemember.Name = "chkRemember";
            chkRemember.Size = new Size(191, 29);
            chkRemember.TabIndex = 2;
            chkRemember.TabStop = false;
            chkRemember.Text = "Ghi nhớ đăng nhập";
            chkRemember.UseVisualStyleBackColor = true;
            // 
            // txtPassword
            // 
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Font = new Font("Segoe UI", 11F);
            txtPassword.Location = new Point(95, 349);
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
            lblPassword.Location = new Point(95, 324);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(117, 28);
            lblPassword.TabIndex = 4;
            lblPassword.Text = "Mật khẩu *";
            // 
            // txtUsername
            // 
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            txtUsername.Font = new Font("Segoe UI", 11F);
            txtUsername.Location = new Point(95, 279);
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "Nhập số điện thoại";
            txtUsername.Size = new Size(300, 37);
            txtUsername.TabIndex = 0;
            txtUsername.KeyPress += TxtUsername_KeyPress;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblUsername.ForeColor = Color.FromArgb(51, 65, 85);
            lblUsername.Location = new Point(95, 254);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(153, 28);
            lblUsername.TabIndex = 2;
            lblUsername.Text = "Số điện thoại *";
            // 
            // lblSubtitle
            // 
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.FromArgb(100, 116, 139);
            lblSubtitle.Location = new Point(95, 179);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(300, 51);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Đăng nhập vào hệ thống";
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(99, 102, 241);
            lblTitle.Location = new Point(50, 30);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(388, 117);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "🎱 Quản Lý Quán Bi-a Pro";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.Transparent;
            btnClose.Cursor = Cursors.Hand;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnClose.ForeColor = Color.Gray;
            btnClose.Location = new Point(455, 10);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(35, 35);
            btnClose.TabIndex = 4;
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
            BackColor = Color.FromArgb(248, 250, 252);
            ClientSize = new Size(500, 650);
            Controls.Add(btnClose);
            Controls.Add(pnlMain);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.None;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng nhập";
            Load += LoginForm_Load;
            pnlMain.ResumeLayout(false);
            pnlMain.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.CheckBox chkRemember;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblForgotPassword;
        private System.Windows.Forms.Label lblSignup;
        private System.Windows.Forms.Button btnClose;
    }
}