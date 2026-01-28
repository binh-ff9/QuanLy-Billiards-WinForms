namespace Billiard.WinForm.Forms.NhanVien
{
    partial class EditNhanVienForm
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

            // Basic Info
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
            this.lblTrangThai = new System.Windows.Forms.Label();
            this.cboTrangThai = new System.Windows.Forms.ComboBox();

            // Salary
            this.lblSectionSalary = new System.Windows.Forms.Label();
            this.lblLuongCoBan = new System.Windows.Forms.Label();
            this.txtLuongCoBan = new System.Windows.Forms.TextBox();
            this.lblPhuCap = new System.Windows.Forms.Label();
            this.txtPhuCap = new System.Windows.Forms.TextBox();

            // Password
            this.lblSectionPassword = new System.Windows.Forms.Label();
            this.lblMatKhauMoi = new System.Windows.Forms.Label();
            this.txtMatKhauMoi = new System.Windows.Forms.TextBox();
            this.lblMatKhauConfirm = new System.Windows.Forms.Label();
            this.txtMatKhauConfirm = new System.Windows.Forms.TextBox();

            // Face ID
            this.lblSectionFace = new System.Windows.Forms.Label();
            this.picCurrentFace = new System.Windows.Forms.PictureBox();
            this.btnDeleteFace = new System.Windows.Forms.Button();

            // Buttons
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.picCurrentFace)).BeginInit();
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
            this.lblTitle.Text = "✏️ Chỉnh sửa nhân viên";

            // Section Basic
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
            this.txtTenNV.Size = new System.Drawing.Size(420, 30);
            this.txtTenNV.Font = new System.Drawing.Font("Segoe UI", 11F);

            // SDT
            this.lblSDT.Location = new System.Drawing.Point(30, 170);
            this.lblSDT.Text = "Số điện thoại *";
            this.lblSDT.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSDT.AutoSize = true;
            this.txtSDT.Location = new System.Drawing.Point(30, 195);
            this.txtSDT.Size = new System.Drawing.Size(200, 30);
            this.txtSDT.Font = new System.Drawing.Font("Segoe UI", 11F);

            // Email
            this.lblEmail.Location = new System.Drawing.Point(250, 170);
            this.lblEmail.Text = "Email";
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblEmail.AutoSize = true;
            this.txtEmail.Location = new System.Drawing.Point(250, 195);
            this.txtEmail.Size = new System.Drawing.Size(200, 30);
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 11F);

            // NhomQuyen
            this.lblNhomQuyen.Location = new System.Drawing.Point(30, 235);
            this.lblNhomQuyen.Text = "Nhóm quyền *";
            this.lblNhomQuyen.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblNhomQuyen.AutoSize = true;
            this.cboNhomQuyen.Location = new System.Drawing.Point(30, 260);
            this.cboNhomQuyen.Size = new System.Drawing.Size(130, 30);
            this.cboNhomQuyen.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cboNhomQuyen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // CaLam
            this.lblCaLam.Location = new System.Drawing.Point(180, 235);
            this.lblCaLam.Text = "Ca làm việc";
            this.lblCaLam.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCaLam.AutoSize = true;
            this.cboCaLam.Location = new System.Drawing.Point(180, 260);
            this.cboCaLam.Size = new System.Drawing.Size(130, 30);
            this.cboCaLam.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cboCaLam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // TrangThai
            this.lblTrangThai.Location = new System.Drawing.Point(330, 235);
            this.lblTrangThai.Text = "Trạng thái";
            this.lblTrangThai.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTrangThai.AutoSize = true;
            this.cboTrangThai.Location = new System.Drawing.Point(330, 260);
            this.cboTrangThai.Size = new System.Drawing.Size(120, 30);
            this.cboTrangThai.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cboTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // Section Salary
            this.lblSectionSalary.AutoSize = true;
            this.lblSectionSalary.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSectionSalary.ForeColor = System.Drawing.Color.FromArgb(102, 126, 234);
            this.lblSectionSalary.Location = new System.Drawing.Point(30, 310);
            this.lblSectionSalary.Text = "💰 Thông tin lương";

            // LuongCoBan
            this.lblLuongCoBan.Location = new System.Drawing.Point(30, 345);
            this.lblLuongCoBan.Text = "Lương cơ bản";
            this.lblLuongCoBan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblLuongCoBan.AutoSize = true;
            this.txtLuongCoBan.Location = new System.Drawing.Point(30, 370);
            this.txtLuongCoBan.Size = new System.Drawing.Size(200, 30);
            this.txtLuongCoBan.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtLuongCoBan.Leave += new System.EventHandler(this.txtLuongCoBan_Leave);

            // PhuCap
            this.lblPhuCap.Location = new System.Drawing.Point(250, 345);
            this.lblPhuCap.Text = "Phụ cấp";
            this.lblPhuCap.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblPhuCap.AutoSize = true;
            this.txtPhuCap.Location = new System.Drawing.Point(250, 370);
            this.txtPhuCap.Size = new System.Drawing.Size(200, 30);
            this.txtPhuCap.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtPhuCap.Leave += new System.EventHandler(this.txtPhuCap_Leave);

            // Section Password
            this.lblSectionPassword.AutoSize = true;
            this.lblSectionPassword.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSectionPassword.ForeColor = System.Drawing.Color.FromArgb(102, 126, 234);
            this.lblSectionPassword.Location = new System.Drawing.Point(30, 420);
            this.lblSectionPassword.Text = "🔐 Đổi mật khẩu (tùy chọn)";

            // MatKhauMoi
            this.lblMatKhauMoi.Location = new System.Drawing.Point(30, 455);
            this.lblMatKhauMoi.Text = "Mật khẩu mới";
            this.lblMatKhauMoi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMatKhauMoi.AutoSize = true;
            this.txtMatKhauMoi.Location = new System.Drawing.Point(30, 480);
            this.txtMatKhauMoi.Size = new System.Drawing.Size(200, 30);
            this.txtMatKhauMoi.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtMatKhauMoi.UseSystemPasswordChar = true;
            this.txtMatKhauMoi.PlaceholderText = "Để trống nếu không đổi";

            // MatKhauConfirm
            this.lblMatKhauConfirm.Location = new System.Drawing.Point(250, 455);
            this.lblMatKhauConfirm.Text = "Xác nhận mật khẩu";
            this.lblMatKhauConfirm.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMatKhauConfirm.AutoSize = true;
            this.txtMatKhauConfirm.Location = new System.Drawing.Point(250, 480);
            this.txtMatKhauConfirm.Size = new System.Drawing.Size(200, 30);
            this.txtMatKhauConfirm.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtMatKhauConfirm.UseSystemPasswordChar = true;

            // Section Face
            this.lblSectionFace.AutoSize = true;
            this.lblSectionFace.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSectionFace.ForeColor = System.Drawing.Color.FromArgb(102, 126, 234);
            this.lblSectionFace.Location = new System.Drawing.Point(30, 530);
            this.lblSectionFace.Text = "📸 Face ID";

            // picCurrentFace
            this.picCurrentFace.Location = new System.Drawing.Point(30, 560);
            this.picCurrentFace.Size = new System.Drawing.Size(100, 100);
            this.picCurrentFace.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picCurrentFace.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picCurrentFace.Visible = false;

            // btnDeleteFace
            this.btnDeleteFace.Location = new System.Drawing.Point(140, 600);
            this.btnDeleteFace.Size = new System.Drawing.Size(120, 35);
            this.btnDeleteFace.Text = "🗑️ Xóa Face";
            this.btnDeleteFace.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDeleteFace.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            this.btnDeleteFace.ForeColor = System.Drawing.Color.White;
            this.btnDeleteFace.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteFace.FlatAppearance.BorderSize = 0;
            this.btnDeleteFace.Visible = false;
            this.btnDeleteFace.Click += new System.EventHandler(this.btnDeleteFace_Click);

            // btnSave
            this.btnSave.Location = new System.Drawing.Point(30, 690);
            this.btnSave.Size = new System.Drawing.Size(130, 45);
            this.btnSave.Text = "💾 Cập nhật";
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(102, 126, 234);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // btnDelete
            this.btnDelete.Location = new System.Drawing.Point(170, 690);
            this.btnDelete.Size = new System.Drawing.Size(140, 45);
            this.btnDelete.Text = "🚫 Nghỉ việc";
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(320, 690);
            this.btnCancel.Size = new System.Drawing.Size(100, 45);
            this.btnCancel.Text = "❌ Hủy";
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // Add controls
            this.panelMain.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblTitle, lblSectionBasic,
                lblTenNV, txtTenNV, lblSDT, txtSDT, lblEmail, txtEmail,
                lblNhomQuyen, cboNhomQuyen, lblCaLam, cboCaLam, lblTrangThai, cboTrangThai,
                lblSectionSalary, lblLuongCoBan, txtLuongCoBan, lblPhuCap, txtPhuCap,
                lblSectionPassword, lblMatKhauMoi, txtMatKhauMoi, lblMatKhauConfirm, txtMatKhauConfirm,
                lblSectionFace, picCurrentFace, btnDeleteFace,
                btnSave, btnDelete, btnCancel
            });

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 780);
            this.Controls.Add(this.panelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditNhanVienForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chỉnh sửa nhân viên";
            this.Load += new System.EventHandler(this.EditNhanVienForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picCurrentFace)).EndInit();
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
        private System.Windows.Forms.Label lblTrangThai;
        private System.Windows.Forms.ComboBox cboTrangThai;
        private System.Windows.Forms.Label lblSectionSalary;
        private System.Windows.Forms.Label lblLuongCoBan;
        private System.Windows.Forms.TextBox txtLuongCoBan;
        private System.Windows.Forms.Label lblPhuCap;
        private System.Windows.Forms.TextBox txtPhuCap;
        private System.Windows.Forms.Label lblSectionPassword;
        private System.Windows.Forms.Label lblMatKhauMoi;
        private System.Windows.Forms.TextBox txtMatKhauMoi;
        private System.Windows.Forms.Label lblMatKhauConfirm;
        private System.Windows.Forms.TextBox txtMatKhauConfirm;
        private System.Windows.Forms.Label lblSectionFace;
        private System.Windows.Forms.PictureBox picCurrentFace;
        private System.Windows.Forms.Button btnDeleteFace;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCancel;
    }
}