namespace Billiard.WinForm.Forms.NhanVien
{
    partial class AddNhanVienForm
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.panelMain = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();

            // Basic Info Section
            this.lblSectionBasic = new System.Windows.Forms.Label();
            this.lblTenNV = new System.Windows.Forms.Label();
            this.txtTenNV = new System.Windows.Forms.TextBox();
            this.lblSDT = new System.Windows.Forms.Label();
            this.txtSDT = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblNhomQuyen = new System.Windows.Forms.Label();
            this.cboNhomQuyen = new System.Windows.Forms.ComboBox();
            this.lblCaLam = new System.Windows.Forms.Label();
            this.cboCaLam = new System.Windows.Forms.ComboBox();

            // Salary Section
            this.lblSectionSalary = new System.Windows.Forms.Label();
            this.lblLuongCoBan = new System.Windows.Forms.Label();
            this.txtLuongCoBan = new System.Windows.Forms.TextBox();
            this.lblPhuCap = new System.Windows.Forms.Label();
            this.txtPhuCap = new System.Windows.Forms.TextBox();

            // Password Section
            this.lblSectionPassword = new System.Windows.Forms.Label();
            this.lblMatKhau = new System.Windows.Forms.Label();
            this.txtMatKhau = new System.Windows.Forms.TextBox();
            this.lblMatKhauConfirm = new System.Windows.Forms.Label();
            this.txtMatKhauConfirm = new System.Windows.Forms.TextBox();

            // Buttons
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCaptureFace = new System.Windows.Forms.Button();

            this.panelMain.SuspendLayout();
            this.SuspendLayout();

            // panelMain
            this.panelMain.AutoScroll = true;
            this.panelMain.BackColor = System.Drawing.Color.White;
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Padding = new System.Windows.Forms.Padding(30);

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(26, 26, 46);
            this.lblTitle.Location = new System.Drawing.Point(30, 20);
            this.lblTitle.Text = "➕ Thêm nhân viên mới";

            // Section Basic Info
            this.lblSectionBasic.AutoSize = true;
            this.lblSectionBasic.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSectionBasic.ForeColor = System.Drawing.Color.FromArgb(102, 126, 234);
            this.lblSectionBasic.Location = new System.Drawing.Point(30, 70);
            this.lblSectionBasic.Text = "👤 Thông tin cơ bản";

            // TenNV
            this.lblTenNV.Location = new System.Drawing.Point(30, 105);
            this.lblTenNV.Text = "Họ và tên *";
            this.lblTenNV.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTenNV.AutoSize = true;
            this.txtTenNV.Location = new System.Drawing.Point(30, 130);
            this.txtTenNV.Size = new System.Drawing.Size(400, 30);
            this.txtTenNV.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtTenNV.PlaceholderText = "Nhập họ và tên nhân viên";

            // SDT
            this.lblSDT.Location = new System.Drawing.Point(30, 170);
            this.lblSDT.Text = "Số điện thoại *";
            this.lblSDT.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSDT.AutoSize = true;
            this.txtSDT.Location = new System.Drawing.Point(30, 195);
            this.txtSDT.Size = new System.Drawing.Size(190, 30);
            this.txtSDT.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtSDT.PlaceholderText = "0987654321";
            this.txtSDT.MaxLength = 11;

            // Email
            this.lblEmail.Location = new System.Drawing.Point(240, 170);
            this.lblEmail.Text = "Email";
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblEmail.AutoSize = true;
            this.txtEmail.Location = new System.Drawing.Point(240, 195);
            this.txtEmail.Size = new System.Drawing.Size(190, 30);
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtEmail.PlaceholderText = "email@example.com";

            // NhomQuyen
            this.lblNhomQuyen.Location = new System.Drawing.Point(30, 235);
            this.lblNhomQuyen.Text = "Nhóm quyền *";
            this.lblNhomQuyen.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblNhomQuyen.AutoSize = true;
            this.cboNhomQuyen.Location = new System.Drawing.Point(30, 260);
            this.cboNhomQuyen.Size = new System.Drawing.Size(190, 30);
            this.cboNhomQuyen.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cboNhomQuyen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // CaLam
            this.lblCaLam.Location = new System.Drawing.Point(240, 235);
            this.lblCaLam.Text = "Ca làm việc";
            this.lblCaLam.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCaLam.AutoSize = true;
            this.cboCaLam.Location = new System.Drawing.Point(240, 260);
            this.cboCaLam.Size = new System.Drawing.Size(190, 30);
            this.cboCaLam.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cboCaLam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // Section Salary
            this.lblSectionSalary.AutoSize = true;
            this.lblSectionSalary.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSectionSalary.ForeColor = System.Drawing.Color.FromArgb(102, 126, 234);
            this.lblSectionSalary.Location = new System.Drawing.Point(30, 310);
            this.lblSectionSalary.Text = "💰 Thông tin lương";

            // LuongCoBan
            this.lblLuongCoBan.Location = new System.Drawing.Point(30, 345);
            this.lblLuongCoBan.Text = "Lương cơ bản *";
            this.lblLuongCoBan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblLuongCoBan.AutoSize = true;
            this.txtLuongCoBan.Location = new System.Drawing.Point(30, 370);
            this.txtLuongCoBan.Size = new System.Drawing.Size(190, 30);
            this.txtLuongCoBan.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtLuongCoBan.Text = "0";
            this.txtLuongCoBan.Leave += new System.EventHandler(this.txtLuongCoBan_Leave);

            // PhuCap
            this.lblPhuCap.Location = new System.Drawing.Point(240, 345);
            this.lblPhuCap.Text = "Phụ cấp";
            this.lblPhuCap.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblPhuCap.AutoSize = true;
            this.txtPhuCap.Location = new System.Drawing.Point(240, 370);
            this.txtPhuCap.Size = new System.Drawing.Size(190, 30);
            this.txtPhuCap.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtPhuCap.Text = "0";
            this.txtPhuCap.Leave += new System.EventHandler(this.txtPhuCap_Leave);

            // Section Password
            this.lblSectionPassword.AutoSize = true;
            this.lblSectionPassword.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSectionPassword.ForeColor = System.Drawing.Color.FromArgb(102, 126, 234);
            this.lblSectionPassword.Location = new System.Drawing.Point(30, 420);
            this.lblSectionPassword.Text = "🔐 Mật khẩu";

            // MatKhau
            this.lblMatKhau.Location = new System.Drawing.Point(30, 455);
            this.lblMatKhau.Text = "Mật khẩu *";
            this.lblMatKhau.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMatKhau.AutoSize = true;
            this.txtMatKhau.Location = new System.Drawing.Point(30, 480);
            this.txtMatKhau.Size = new System.Drawing.Size(190, 30);
            this.txtMatKhau.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtMatKhau.UseSystemPasswordChar = true;
            this.txtMatKhau.PlaceholderText = "Tối thiểu 6 ký tự";

            // MatKhauConfirm
            this.lblMatKhauConfirm.Location = new System.Drawing.Point(240, 455);
            this.lblMatKhauConfirm.Text = "Xác nhận mật khẩu *";
            this.lblMatKhauConfirm.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMatKhauConfirm.AutoSize = true;
            this.txtMatKhauConfirm.Location = new System.Drawing.Point(240, 480);
            this.txtMatKhauConfirm.Size = new System.Drawing.Size(190, 30);
            this.txtMatKhauConfirm.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtMatKhauConfirm.UseSystemPasswordChar = true;
            this.txtMatKhauConfirm.PlaceholderText = "Nhập lại mật khẩu";

            // btnCaptureFace
            this.btnCaptureFace.Location = new System.Drawing.Point(30, 530);
            this.btnCaptureFace.Size = new System.Drawing.Size(180, 40);
            this.btnCaptureFace.Text = "📸 Chụp Face ID";
            this.btnCaptureFace.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCaptureFace.BackColor = System.Drawing.Color.FromArgb(23, 162, 184);
            this.btnCaptureFace.ForeColor = System.Drawing.Color.White;
            this.btnCaptureFace.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCaptureFace.FlatAppearance.BorderSize = 0;
            this.btnCaptureFace.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCaptureFace.Click += new System.EventHandler(this.btnCaptureFace_Click);

            // btnSave
            this.btnSave.Location = new System.Drawing.Point(30, 590);
            this.btnSave.Size = new System.Drawing.Size(180, 45);
            this.btnSave.Text = "✅ Thêm nhân viên";
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(102, 126, 234);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(230, 590);
            this.btnCancel.Size = new System.Drawing.Size(120, 45);
            this.btnCancel.Text = "❌ Hủy";
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // Add controls to panelMain
            this.panelMain.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblTitle, lblSectionBasic,
                lblTenNV, txtTenNV, lblSDT, txtSDT, lblEmail, txtEmail,
                lblNhomQuyen, cboNhomQuyen, lblCaLam, cboCaLam,
                lblSectionSalary, lblLuongCoBan, txtLuongCoBan, lblPhuCap, txtPhuCap,
                lblSectionPassword, lblMatKhau, txtMatKhau, lblMatKhauConfirm, txtMatKhauConfirm,
                btnCaptureFace, btnSave, btnCancel
            });

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 680);
            this.Controls.Add(this.panelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddNhanVienForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thêm nhân viên mới";
            this.Load += new System.EventHandler(this.AddNhanVienForm_Load);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.ResumeLayout(false);
        }
        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSectionBasic;
        private System.Windows.Forms.Label lblTenNV;
        private System.Windows.Forms.TextBox txtTenNV;
        private System.Windows.Forms.Label lblSDT;
        private System.Windows.Forms.TextBox txtSDT;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblNhomQuyen;
        private System.Windows.Forms.ComboBox cboNhomQuyen;
        private System.Windows.Forms.Label lblCaLam;
        private System.Windows.Forms.ComboBox cboCaLam;
        private System.Windows.Forms.Label lblSectionSalary;
        private System.Windows.Forms.Label lblLuongCoBan;
        private System.Windows.Forms.TextBox txtLuongCoBan;
        private System.Windows.Forms.Label lblPhuCap;
        private System.Windows.Forms.TextBox txtPhuCap;
        private System.Windows.Forms.Label lblSectionPassword;
        private System.Windows.Forms.Label lblMatKhau;
        private System.Windows.Forms.TextBox txtMatKhau;
        private System.Windows.Forms.Label lblMatKhauConfirm;
        private System.Windows.Forms.TextBox txtMatKhauConfirm;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCaptureFace;
    }
}