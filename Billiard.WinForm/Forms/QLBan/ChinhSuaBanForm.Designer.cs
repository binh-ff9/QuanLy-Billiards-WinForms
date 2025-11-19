namespace Billiard.WinForm.Forms.QLBan
{
    partial class ChinhSuaBanForm
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
            pnlHeader = new Panel();
            lblTitle = new Label();
            pnlMain = new Panel();
            lblTenBan = new Label();
            txtTenBan = new TextBox();
            lblLoaiBan = new Label();
            cboLoaiBan = new ComboBox();
            lblKhuVuc = new Label();
            cboKhuVuc = new ComboBox();
            lblGhiChu = new Label();
            txtGhiChu = new TextBox();
            lblHinhAnh = new Label();
            picPreview = new PictureBox();
            btnChonAnh = new Button();
            btnXoaAnh = new Button();
            pnlFooter = new Panel();
            btnHuy = new Button();
            btnLuu = new Button();
            pnlHeader.SuspendLayout();
            pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picPreview).BeginInit();
            pnlFooter.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(99, 102, 241);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(20, 10, 20, 10);
            pnlHeader.Size = new Size(600, 70);
            pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 18);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(320, 48);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "✏️ Chỉnh sửa bàn";
            // 
            // pnlMain
            // 
            pnlMain.AutoScroll = true;
            pnlMain.BackColor = Color.White;
            pnlMain.Controls.Add(lblTenBan);
            pnlMain.Controls.Add(txtTenBan);
            pnlMain.Controls.Add(lblLoaiBan);
            pnlMain.Controls.Add(cboLoaiBan);
            pnlMain.Controls.Add(lblKhuVuc);
            pnlMain.Controls.Add(cboKhuVuc);
            pnlMain.Controls.Add(lblGhiChu);
            pnlMain.Controls.Add(txtGhiChu);
            pnlMain.Controls.Add(lblHinhAnh);
            pnlMain.Controls.Add(picPreview);
            pnlMain.Controls.Add(btnChonAnh);
            pnlMain.Controls.Add(btnXoaAnh);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(0, 70);
            pnlMain.Name = "pnlMain";
            pnlMain.Padding = new Padding(20);
            pnlMain.Size = new Size(600, 510);
            pnlMain.TabIndex = 1;
            // 
            // lblTenBan
            // 
            lblTenBan.AutoSize = true;
            lblTenBan.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTenBan.ForeColor = Color.FromArgb(30, 41, 59);
            lblTenBan.Location = new Point(20, 20);
            lblTenBan.Name = "lblTenBan";
            lblTenBan.Size = new Size(115, 28);
            lblTenBan.TabIndex = 0;
            lblTenBan.Text = "Tên bàn (*)";
            // 
            // txtTenBan
            // 
            txtTenBan.BorderStyle = BorderStyle.FixedSingle;
            txtTenBan.Font = new Font("Segoe UI", 11F);
            txtTenBan.Location = new Point(20, 45);
            txtTenBan.MaxLength = 50;
            txtTenBan.Name = "txtTenBan";
            txtTenBan.Size = new Size(540, 37);
            txtTenBan.TabIndex = 1;
            // 
            // lblLoaiBan
            // 
            lblLoaiBan.AutoSize = true;
            lblLoaiBan.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblLoaiBan.ForeColor = Color.FromArgb(30, 41, 59);
            lblLoaiBan.Location = new Point(20, 90);
            lblLoaiBan.Name = "lblLoaiBan";
            lblLoaiBan.Size = new Size(121, 28);
            lblLoaiBan.TabIndex = 2;
            lblLoaiBan.Text = "Loại bàn (*)";
            // 
            // cboLoaiBan
            // 
            cboLoaiBan.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLoaiBan.Font = new Font("Segoe UI", 11F);
            cboLoaiBan.FormattingEnabled = true;
            cboLoaiBan.Location = new Point(20, 115);
            cboLoaiBan.Name = "cboLoaiBan";
            cboLoaiBan.Size = new Size(260, 38);
            cboLoaiBan.TabIndex = 3;
            // 
            // lblKhuVuc
            // 
            lblKhuVuc.AutoSize = true;
            lblKhuVuc.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblKhuVuc.ForeColor = Color.FromArgb(30, 41, 59);
            lblKhuVuc.Location = new Point(300, 90);
            lblKhuVuc.Name = "lblKhuVuc";
            lblKhuVuc.Size = new Size(118, 28);
            lblKhuVuc.TabIndex = 4;
            lblKhuVuc.Text = "Khu vực (*)";
            // 
            // cboKhuVuc
            // 
            cboKhuVuc.DropDownStyle = ComboBoxStyle.DropDownList;
            cboKhuVuc.Font = new Font("Segoe UI", 11F);
            cboKhuVuc.FormattingEnabled = true;
            cboKhuVuc.Location = new Point(300, 115);
            cboKhuVuc.Name = "cboKhuVuc";
            cboKhuVuc.Size = new Size(260, 38);
            cboKhuVuc.TabIndex = 5;
            // 
            // lblGhiChu
            // 
            lblGhiChu.AutoSize = true;
            lblGhiChu.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblGhiChu.ForeColor = Color.FromArgb(30, 41, 59);
            lblGhiChu.Location = new Point(20, 160);
            lblGhiChu.Name = "lblGhiChu";
            lblGhiChu.Size = new Size(84, 28);
            lblGhiChu.TabIndex = 6;
            lblGhiChu.Text = "Ghi chú";
            // 
            // txtGhiChu
            // 
            txtGhiChu.BorderStyle = BorderStyle.FixedSingle;
            txtGhiChu.Font = new Font("Segoe UI", 11F);
            txtGhiChu.Location = new Point(20, 185);
            txtGhiChu.MaxLength = 200;
            txtGhiChu.Multiline = true;
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.ScrollBars = ScrollBars.Vertical;
            txtGhiChu.Size = new Size(540, 80);
            txtGhiChu.TabIndex = 7;
            // 
            // lblHinhAnh
            // 
            lblHinhAnh.AutoSize = true;
            lblHinhAnh.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblHinhAnh.ForeColor = Color.FromArgb(30, 41, 59);
            lblHinhAnh.Location = new Point(20, 285);
            lblHinhAnh.Name = "lblHinhAnh";
            lblHinhAnh.Size = new Size(139, 28);
            lblHinhAnh.TabIndex = 8;
            lblHinhAnh.Text = "Hình ảnh bàn";
            // 
            // picPreview
            // 
            picPreview.BackColor = Color.FromArgb(248, 250, 252);
            picPreview.BorderStyle = BorderStyle.FixedSingle;
            picPreview.Location = new Point(20, 310);
            picPreview.Name = "picPreview";
            picPreview.Size = new Size(250, 180);
            picPreview.SizeMode = PictureBoxSizeMode.Zoom;
            picPreview.TabIndex = 9;
            picPreview.TabStop = false;
            // 
            // btnChonAnh
            // 
            btnChonAnh.BackColor = Color.FromArgb(100, 116, 139);
            btnChonAnh.Cursor = Cursors.Hand;
            btnChonAnh.FlatAppearance.BorderSize = 0;
            btnChonAnh.FlatStyle = FlatStyle.Flat;
            btnChonAnh.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnChonAnh.ForeColor = Color.White;
            btnChonAnh.Location = new Point(290, 310);
            btnChonAnh.Name = "btnChonAnh";
            btnChonAnh.Size = new Size(270, 45);
            btnChonAnh.TabIndex = 10;
            btnChonAnh.Text = "📷 Chọn hình ảnh";
            btnChonAnh.UseVisualStyleBackColor = false;
            btnChonAnh.Click += BtnChonAnh_Click;
            // 
            // btnXoaAnh
            // 
            btnXoaAnh.BackColor = Color.FromArgb(239, 68, 68);
            btnXoaAnh.Cursor = Cursors.Hand;
            btnXoaAnh.Enabled = false;
            btnXoaAnh.FlatAppearance.BorderSize = 0;
            btnXoaAnh.FlatStyle = FlatStyle.Flat;
            btnXoaAnh.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnXoaAnh.ForeColor = Color.White;
            btnXoaAnh.Location = new Point(290, 370);
            btnXoaAnh.Name = "btnXoaAnh";
            btnXoaAnh.Size = new Size(270, 45);
            btnXoaAnh.TabIndex = 11;
            btnXoaAnh.Text = "🗑️ Xóa ảnh";
            btnXoaAnh.UseVisualStyleBackColor = false;
            btnXoaAnh.Click += BtnXoaAnh_Click;
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.FromArgb(248, 250, 252);
            pnlFooter.Controls.Add(btnHuy);
            pnlFooter.Controls.Add(btnLuu);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(0, 580);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Padding = new Padding(20, 15, 20, 15);
            pnlFooter.Size = new Size(600, 70);
            pnlFooter.TabIndex = 2;
            // 
            // btnHuy
            // 
            btnHuy.BackColor = Color.FromArgb(100, 116, 139);
            btnHuy.Cursor = Cursors.Hand;
            btnHuy.DialogResult = DialogResult.Cancel;
            btnHuy.FlatAppearance.BorderSize = 0;
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnHuy.ForeColor = Color.White;
            btnHuy.Location = new Point(340, 15);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(120, 40);
            btnHuy.TabIndex = 0;
            btnHuy.Text = "❌ Hủy";
            btnHuy.UseVisualStyleBackColor = false;
            btnHuy.Click += BtnHuy_Click;
            // 
            // btnLuu
            // 
            btnLuu.BackColor = Color.FromArgb(34, 197, 94);
            btnLuu.Cursor = Cursors.Hand;
            btnLuu.FlatAppearance.BorderSize = 0;
            btnLuu.FlatStyle = FlatStyle.Flat;
            btnLuu.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnLuu.ForeColor = Color.White;
            btnLuu.Location = new Point(470, 15);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new Size(120, 40);
            btnLuu.TabIndex = 1;
            btnLuu.Text = "💾 Cập nhật";
            btnLuu.UseVisualStyleBackColor = false;
            btnLuu.Click += BtnLuu_Click;
            // 
            // ChinhSuaBanForm
            // 
            AcceptButton = btnLuu;
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            CancelButton = btnHuy;
            ClientSize = new Size(600, 650);
            Controls.Add(pnlMain);
            Controls.Add(pnlFooter);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ChinhSuaBanForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Chỉnh sửa bàn";
            Load += ChinhSuaBanForm_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlMain.ResumeLayout(false);
            pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picPreview).EndInit();
            pnlFooter.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label lblTenBan;
        private System.Windows.Forms.TextBox txtTenBan;
        private System.Windows.Forms.Label lblLoaiBan;
        private System.Windows.Forms.ComboBox cboLoaiBan;
        private System.Windows.Forms.Label lblKhuVuc;
        private System.Windows.Forms.ComboBox cboKhuVuc;
        private System.Windows.Forms.Label lblGhiChu;
        private System.Windows.Forms.TextBox txtGhiChu;
        private System.Windows.Forms.Label lblHinhAnh;
        private System.Windows.Forms.PictureBox picPreview;
        private System.Windows.Forms.Button btnChonAnh;
        private System.Windows.Forms.Button btnXoaAnh;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.Button btnLuu;
    }
}