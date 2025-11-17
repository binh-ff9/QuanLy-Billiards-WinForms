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
            pnlMain = new Panel();
            btnCancel = new Button();
            btnResetPassword = new Button();
            chkShowPassword = new CheckBox();
            txtConfirmPassword = new TextBox();
            lblConfirmPassword = new Label();
            txtNewPassword = new TextBox();
            lblNewPassword = new Label();
            txtOTP = new TextBox();
            lblOTP = new Label();
            lblEmailDisplay = new Label();
            lblSubtitle = new Label();
            lblTitle = new Label();
            btnClose = new Button();
            pnlMain.SuspendLayout();
            SuspendLayout();
            // 
            // pnlMain
            // 
            pnlMain.BackColor = Color.White;
            pnlMain.Controls.Add(btnCancel);
            pnlMain.Controls.Add(btnResetPassword);
            pnlMain.Controls.Add(chkShowPassword);
            pnlMain.Controls.Add(txtConfirmPassword);
            pnlMain.Controls.Add(lblConfirmPassword);
            pnlMain.Controls.Add(txtNewPassword);
            pnlMain.Controls.Add(lblNewPassword);
            pnlMain.Controls.Add(txtOTP);
            pnlMain.Controls.Add(lblOTP);
            pnlMain.Controls.Add(lblEmailDisplay);
            pnlMain.Controls.Add(lblSubtitle);
            pnlMain.Controls.Add(lblTitle);
            pnlMain.Location = new Point(2, -3);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(501, 633);
            pnlMain.TabIndex = 0;
            pnlMain.Paint += PnlMain_Paint;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(108, 117, 125);
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(87, 522);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(300, 40);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Hủy";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += BtnCancel_Click;
            // 
            // btnResetPassword
            // 
            btnResetPassword.BackColor = Color.FromArgb(99, 102, 241);
            btnResetPassword.Cursor = Cursors.Hand;
            btnResetPassword.FlatAppearance.BorderSize = 0;
            btnResetPassword.FlatStyle = FlatStyle.Flat;
            btnResetPassword.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnResetPassword.ForeColor = Color.White;
            btnResetPassword.Location = new Point(87, 462);
            btnResetPassword.Name = "btnResetPassword";
            btnResetPassword.Size = new Size(300, 45);
            btnResetPassword.TabIndex = 4;
            btnResetPassword.Text = "Đặt lại Mật khẩu";
            btnResetPassword.UseVisualStyleBackColor = false;
            btnResetPassword.Click += BtnResetPassword_Click;
            // 
            // chkShowPassword
            // 
            chkShowPassword.AutoSize = true;
            chkShowPassword.Font = new Font("Segoe UI", 9F);
            chkShowPassword.ForeColor = Color.FromArgb(71, 85, 105);
            chkShowPassword.Location = new Point(87, 422);
            chkShowPassword.Name = "chkShowPassword";
            chkShowPassword.Size = new Size(178, 29);
            chkShowPassword.TabIndex = 3;
            chkShowPassword.Text = "Hiển thị mật khẩu";
            chkShowPassword.UseVisualStyleBackColor = true;
            chkShowPassword.CheckedChanged += ChkShowPassword_CheckedChanged;
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.BorderStyle = BorderStyle.FixedSingle;
            txtConfirmPassword.Font = new Font("Segoe UI", 11F);
            txtConfirmPassword.Location = new Point(87, 377);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.PlaceholderText = "Nhập lại mật khẩu mới";
            txtConfirmPassword.Size = new Size(300, 37);
            txtConfirmPassword.TabIndex = 2;
            txtConfirmPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.KeyPress += TxtConfirmPassword_KeyPress;
            // 
            // lblConfirmPassword
            // 
            lblConfirmPassword.AutoSize = true;
            lblConfirmPassword.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblConfirmPassword.ForeColor = Color.FromArgb(51, 65, 85);
            lblConfirmPassword.Location = new Point(87, 352);
            lblConfirmPassword.Name = "lblConfirmPassword";
            lblConfirmPassword.Size = new Size(253, 28);
            lblConfirmPassword.TabIndex = 7;
            lblConfirmPassword.Text = "Xác nhận Mật khẩu mới *";
            // 
            // txtNewPassword
            // 
            txtNewPassword.BorderStyle = BorderStyle.FixedSingle;
            txtNewPassword.Font = new Font("Segoe UI", 11F);
            txtNewPassword.Location = new Point(87, 307);
            txtNewPassword.Name = "txtNewPassword";
            txtNewPassword.PlaceholderText = "Nhập mật khẩu mới";
            txtNewPassword.Size = new Size(300, 37);
            txtNewPassword.TabIndex = 1;
            txtNewPassword.UseSystemPasswordChar = true;
            // 
            // lblNewPassword
            // 
            lblNewPassword.AutoSize = true;
            lblNewPassword.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblNewPassword.ForeColor = Color.FromArgb(51, 65, 85);
            lblNewPassword.Location = new Point(87, 282);
            lblNewPassword.Name = "lblNewPassword";
            lblNewPassword.Size = new Size(160, 28);
            lblNewPassword.TabIndex = 5;
            lblNewPassword.Text = "Mật khẩu mới *";
            // 
            // txtOTP
            // 
            txtOTP.BorderStyle = BorderStyle.FixedSingle;
            txtOTP.Font = new Font("Segoe UI", 11F);
            txtOTP.Location = new Point(87, 237);
            txtOTP.MaxLength = 6;
            txtOTP.Name = "txtOTP";
            txtOTP.PlaceholderText = "Nhập mã OTP (6 chữ số)";
            txtOTP.Size = new Size(300, 37);
            txtOTP.TabIndex = 0;
            // 
            // lblOTP
            // 
            lblOTP.AutoSize = true;
            lblOTP.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblOTP.ForeColor = Color.FromArgb(51, 65, 85);
            lblOTP.Location = new Point(87, 212);
            lblOTP.Name = "lblOTP";
            lblOTP.Size = new Size(206, 28);
            lblOTP.TabIndex = 3;
            lblOTP.Text = "Mã xác nhận (OTP) *";
            // 
            // lblEmailDisplay
            // 
            lblEmailDisplay.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblEmailDisplay.ForeColor = Color.FromArgb(99, 102, 241);
            lblEmailDisplay.Location = new Point(87, 177);
            lblEmailDisplay.Name = "lblEmailDisplay";
            lblEmailDisplay.Size = new Size(300, 20);
            lblEmailDisplay.TabIndex = 2;
            lblEmailDisplay.Text = "Email: example@email.com";
            lblEmailDisplay.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSubtitle
            // 
            lblSubtitle.Font = new Font("Segoe UI", 9.5F);
            lblSubtitle.ForeColor = Color.FromArgb(100, 116, 139);
            lblSubtitle.Location = new Point(87, 137);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(300, 40);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Nhập mã OTP và mật khẩu mới";
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(99, 102, 241);
            lblTitle.Location = new Point(50, 25);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(383, 84);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "🔒 Đặt lại Mật Khẩu";
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
            btnClose.TabIndex = 6;
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
            BackColor = Color.FromArgb(248, 250, 252);
            ClientSize = new Size(500, 630);
            Controls.Add(btnClose);
            Controls.Add(pnlMain);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ResetPasswordForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đặt lại mật khẩu";
            Load += ResetPasswordForm_Load;
            pnlMain.ResumeLayout(false);
            pnlMain.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblEmailDisplay;
        private System.Windows.Forms.Label lblOTP;
        private System.Windows.Forms.TextBox txtOTP;
        private System.Windows.Forms.Label lblNewPassword;
        private System.Windows.Forms.TextBox txtNewPassword;
        private System.Windows.Forms.Label lblConfirmPassword;
        private System.Windows.Forms.TextBox txtConfirmPassword;
        private System.Windows.Forms.CheckBox chkShowPassword;
        private System.Windows.Forms.Button btnResetPassword;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnClose;
    }
}