namespace Billiard.WinForm.Forms.Auth
{
    partial class ForgotPasswordForm
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
            pnlInfo = new Panel();
            lblInfoIcon = new Label();
            lblInfoText = new Label();
            btnBack = new Button();
            btnSendOTP = new Button();
            txtEmail = new TextBox();
            lblEmail = new Label();
            lblSubtitle = new Label();
            lblTitle = new Label();
            btnClose = new Button();
            pnlLeft.SuspendLayout();
            pnlDecoration.SuspendLayout();
            pnlRight.SuspendLayout();
            pnlMain.SuspendLayout();
            pnlInfo.SuspendLayout();
            SuspendLayout();
            // 
            // pnlLeft
            // 
            pnlLeft.BackColor = Color.FromArgb(248, 250, 252);
            pnlLeft.Controls.Add(pnlDecoration);
            pnlLeft.Dock = DockStyle.Left;
            pnlLeft.Location = new Point(0, 0);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Size = new Size(400, 550);
            pnlLeft.TabIndex = 0;
            // 
            // pnlDecoration
            // 
            pnlDecoration.BackColor = Color.MidnightBlue;
            pnlDecoration.Controls.Add(lblDecoSubtitle);
            pnlDecoration.Controls.Add(lblDecoTitle);
            pnlDecoration.Location = new Point(29, 37);
            pnlDecoration.Name = "pnlDecoration";
            pnlDecoration.Size = new Size(340, 483);
            pnlDecoration.TabIndex = 0;
            // 
            // lblDecoSubtitle
            // 
            lblDecoSubtitle.BackColor = Color.MidnightBlue;
            lblDecoSubtitle.Dock = DockStyle.Bottom;
            lblDecoSubtitle.Font = new Font("Segoe UI", 11F);
            lblDecoSubtitle.ForeColor = Color.White;
            lblDecoSubtitle.Location = new Point(0, 267);
            lblDecoSubtitle.Name = "lblDecoSubtitle";
            lblDecoSubtitle.Padding = new Padding(30);
            lblDecoSubtitle.Size = new Size(340, 216);
            lblDecoSubtitle.TabIndex = 1;
            lblDecoSubtitle.Text = "📧 Quy trình khôi phục:\r\n1️⃣ Nhập email đã đăng ký\r\n2️⃣ Nhận mã OTP (6 số)\r\n3️⃣ Nhập OTP và mật khẩu mới\r\n4️⃣ Hoàn tất!\r\n⚡ Mã OTP có hiệu lực 5 phút";
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
            lblDecoTitle.Size = new Size(340, 219);
            lblDecoTitle.TabIndex = 0;
            lblDecoTitle.Text = "KHÔI PHỤC\r\nMẬT KHẨU";
            lblDecoTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlRight
            // 
            pnlRight.BackColor = Color.White;
            pnlRight.Controls.Add(pnlMain);
            pnlRight.Controls.Add(btnClose);
            pnlRight.Dock = DockStyle.Fill;
            pnlRight.Location = new Point(400, 0);
            pnlRight.Name = "pnlRight";
            pnlRight.Size = new Size(500, 550);
            pnlRight.TabIndex = 1;
            // 
            // pnlMain
            // 
            pnlMain.Controls.Add(pnlInfo);
            pnlMain.Controls.Add(btnBack);
            pnlMain.Controls.Add(btnSendOTP);
            pnlMain.Controls.Add(txtEmail);
            pnlMain.Controls.Add(lblEmail);
            pnlMain.Controls.Add(lblSubtitle);
            pnlMain.Controls.Add(lblTitle);
            pnlMain.Location = new Point(54, 37);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(400, 483);
            pnlMain.TabIndex = 0;
            pnlMain.Paint += PnlMain_Paint;
            // 
            // pnlInfo
            // 
            pnlInfo.BackColor = Color.FromArgb(254, 249, 195);
            pnlInfo.BorderStyle = BorderStyle.FixedSingle;
            pnlInfo.Controls.Add(lblInfoIcon);
            pnlInfo.Controls.Add(lblInfoText);
            pnlInfo.Location = new Point(50, 224);
            pnlInfo.Name = "pnlInfo";
            pnlInfo.Size = new Size(300, 100);
            pnlInfo.TabIndex = 6;
            // 
            // lblInfoIcon
            // 
            lblInfoIcon.Font = new Font("Segoe UI", 24F);
            lblInfoIcon.Location = new Point(10, 20);
            lblInfoIcon.Name = "lblInfoIcon";
            lblInfoIcon.Size = new Size(60, 60);
            lblInfoIcon.TabIndex = 0;
            lblInfoIcon.Text = "💡";
            lblInfoIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblInfoText
            // 
            lblInfoText.Font = new Font("Segoe UI", 9F);
            lblInfoText.ForeColor = Color.FromArgb(120, 53, 15);
            lblInfoText.Location = new Point(75, 10);
            lblInfoText.Name = "lblInfoText";
            lblInfoText.Size = new Size(210, 80);
            lblInfoText.TabIndex = 1;
            lblInfoText.Text = "Mã OTP sẽ được gửi đến email của bạn.\r\n\r\nVui lòng kiểm tra cả hộp thư Spam nếu không thấy email.";
            lblInfoText.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnBack
            // 
            btnBack.BackColor = Color.FromArgb(241, 245, 249);
            btnBack.Cursor = Cursors.Hand;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnBack.ForeColor = Color.FromArgb(51, 65, 85);
            btnBack.Location = new Point(50, 409);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(300, 45);
            btnBack.TabIndex = 2;
            btnBack.Text = "← Quay lại Đăng nhập";
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Click += BtnBack_Click;
            // 
            // btnSendOTP
            // 
            btnSendOTP.BackColor = Color.SeaGreen;
            btnSendOTP.Cursor = Cursors.Hand;
            btnSendOTP.FlatAppearance.BorderSize = 0;
            btnSendOTP.FlatStyle = FlatStyle.Flat;
            btnSendOTP.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnSendOTP.ForeColor = Color.White;
            btnSendOTP.Location = new Point(50, 344);
            btnSendOTP.Name = "btnSendOTP";
            btnSendOTP.Size = new Size(300, 50);
            btnSendOTP.TabIndex = 1;
            btnSendOTP.Text = "📧 Gửi mã OTP";
            btnSendOTP.UseVisualStyleBackColor = false;
            btnSendOTP.Click += BtnSendOTP_Click;
            // 
            // txtEmail
            // 
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.Font = new Font("Segoe UI", 12F);
            txtEmail.Location = new Point(50, 163);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "example@email.com";
            txtEmail.Size = new Size(300, 39);
            txtEmail.TabIndex = 0;
            txtEmail.KeyPress += TxtEmail_KeyPress;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblEmail.ForeColor = Color.FromArgb(51, 65, 85);
            lblEmail.Location = new Point(50, 130);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(243, 30);
            lblEmail.TabIndex = 2;
            lblEmail.Text = "📧 Email đã đăng ký *";
            // 
            // lblSubtitle
            // 
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.FromArgb(100, 116, 139);
            lblSubtitle.Location = new Point(50, 95);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(300, 50);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Nhập email để nhận mã xác nhận";
            lblSubtitle.TextAlign = ContentAlignment.TopCenter;
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.MidnightBlue;
            lblTitle.Location = new Point(3, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(394, 57);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "QUÊN MẬT KHẨU";
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
            btnClose.Location = new Point(457, 0);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(40, 64);
            btnClose.TabIndex = 5;
            btnClose.TabStop = false;
            btnClose.Text = "✕";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += BtnClose_Click;
            btnClose.MouseEnter += BtnClose_MouseEnter;
            btnClose.MouseLeave += BtnClose_MouseLeave;
            // 
            // ForgotPasswordForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 550);
            Controls.Add(pnlRight);
            Controls.Add(pnlLeft);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ForgotPasswordForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quên mật khẩu - Quán Bi-a Pro";
            Load += ForgotPasswordForm_Load;
            pnlLeft.ResumeLayout(false);
            pnlDecoration.ResumeLayout(false);
            pnlRight.ResumeLayout(false);
            pnlMain.ResumeLayout(false);
            pnlMain.PerformLayout();
            pnlInfo.ResumeLayout(false);
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
        private Label lblEmail;
        private TextBox txtEmail;
        private Button btnSendOTP;
        private Button btnBack;
        private Panel pnlInfo;
        private Label lblInfoIcon;
        private Label lblInfoText;
        private Button btnClose;
    }
}