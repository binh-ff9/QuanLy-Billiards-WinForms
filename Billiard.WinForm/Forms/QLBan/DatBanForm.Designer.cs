namespace Billiard.WinForm.Forms.QLBan
{
    partial class DatBanForm
    {
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            lblTitle = new Label();
            pnlContent = new Panel();
            groupBoxGhiChu = new GroupBox();
            txtGhiChu = new TextBox();
            groupBoxThoiGian = new GroupBox();
            dtpGioKetThuc = new DateTimePicker();
            lblGioKetThuc = new Label();
            dtpGioDat = new DateTimePicker();
            lblGioDat = new Label();
            dtpNgayDat = new DateTimePicker();
            lblNgayDat = new Label();
            groupBoxKhach = new GroupBox();
            btnTimKhachHang = new Button();
            txtSoDienThoai = new TextBox();
            lblSoDienThoai = new Label();
            txtTenKhach = new TextBox();
            lblTenKhach = new Label();
            groupBoxBan = new GroupBox();
            flpBanTrong = new FlowLayoutPanel();
            lblChonBan = new Label();
            pnlButtons = new Panel();
            btnHuy = new Button();
            btnXacNhan = new Button();
            pnlHeader.SuspendLayout();
            pnlContent.SuspendLayout();
            groupBoxGhiChu.SuspendLayout();
            groupBoxThoiGian.SuspendLayout();
            groupBoxKhach.SuspendLayout();
            groupBoxBan.SuspendLayout();
            pnlButtons.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(99, 102, 241);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(20);
            pnlHeader.Size = new Size(900, 70);
            pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 18);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(315, 48);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "📅 Đặt bàn trước";
            // 
            // pnlContent
            // 
            pnlContent.AutoScroll = true;
            pnlContent.BackColor = Color.White;
            pnlContent.Controls.Add(groupBoxGhiChu);
            pnlContent.Controls.Add(groupBoxThoiGian);
            pnlContent.Controls.Add(groupBoxKhach);
            pnlContent.Controls.Add(groupBoxBan);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(0, 70);
            pnlContent.Name = "pnlContent";
            pnlContent.Padding = new Padding(20);
            pnlContent.Size = new Size(900, 883);
            pnlContent.TabIndex = 1;
            // 
            // groupBoxGhiChu
            // 
            groupBoxGhiChu.Controls.Add(txtGhiChu);
            groupBoxGhiChu.Dock = DockStyle.Top;
            groupBoxGhiChu.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            groupBoxGhiChu.Location = new Point(20, 739);
            groupBoxGhiChu.Name = "groupBoxGhiChu";
            groupBoxGhiChu.Padding = new Padding(15);
            groupBoxGhiChu.Size = new Size(860, 120);
            groupBoxGhiChu.TabIndex = 3;
            groupBoxGhiChu.TabStop = false;
            groupBoxGhiChu.Text = "Ghi chú";
            // 
            // txtGhiChu
            // 
            txtGhiChu.Dock = DockStyle.Fill;
            txtGhiChu.Font = new Font("Segoe UI", 11F);
            txtGhiChu.Location = new Point(15, 45);
            txtGhiChu.Multiline = true;
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.PlaceholderText = "Nhập ghi chú (không bắt buộc)";
            txtGhiChu.Size = new Size(830, 60);
            txtGhiChu.TabIndex = 0;
            // 
            // groupBoxThoiGian
            // 
            groupBoxThoiGian.Controls.Add(dtpGioKetThuc);
            groupBoxThoiGian.Controls.Add(lblGioKetThuc);
            groupBoxThoiGian.Controls.Add(dtpGioDat);
            groupBoxThoiGian.Controls.Add(lblGioDat);
            groupBoxThoiGian.Controls.Add(dtpNgayDat);
            groupBoxThoiGian.Controls.Add(lblNgayDat);
            groupBoxThoiGian.Dock = DockStyle.Top;
            groupBoxThoiGian.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            groupBoxThoiGian.Location = new Point(20, 515);
            groupBoxThoiGian.Name = "groupBoxThoiGian";
            groupBoxThoiGian.Padding = new Padding(15);
            groupBoxThoiGian.Size = new Size(860, 224);
            groupBoxThoiGian.TabIndex = 2;
            groupBoxThoiGian.TabStop = false;
            groupBoxThoiGian.Text = "Thời gian đặt bàn";
            // 
            // dtpGioKetThuc
            // 
            dtpGioKetThuc.Font = new Font("Segoe UI", 11F);
            dtpGioKetThuc.Location = new Point(175, 169);
            dtpGioKetThuc.Name = "dtpGioKetThuc";
            dtpGioKetThuc.Size = new Size(382, 37);
            dtpGioKetThuc.TabIndex = 5;
            dtpGioKetThuc.ValueChanged += DtpGioKetThuc_ValueChanged;
            // 
            // lblGioKetThuc
            // 
            lblGioKetThuc.AutoSize = true;
            lblGioKetThuc.Font = new Font("Segoe UI", 11F);
            lblGioKetThuc.Location = new Point(20, 172);
            lblGioKetThuc.Name = "lblGioKetThuc";
            lblGioKetThuc.Size = new Size(134, 30);
            lblGioKetThuc.TabIndex = 4;
            lblGioKetThuc.Text = "Giờ kết thúc:";
            // 
            // dtpGioDat
            // 
            dtpGioDat.Font = new Font("Segoe UI", 11F);
            dtpGioDat.Location = new Point(175, 113);
            dtpGioDat.Name = "dtpGioDat";
            dtpGioDat.Size = new Size(382, 37);
            dtpGioDat.TabIndex = 3;
            dtpGioDat.ValueChanged += DtpGioDat_ValueChanged;
            // 
            // lblGioDat
            // 
            lblGioDat.AutoSize = true;
            lblGioDat.Font = new Font("Segoe UI", 11F);
            lblGioDat.Location = new Point(18, 113);
            lblGioDat.Name = "lblGioDat";
            lblGioDat.Size = new Size(88, 30);
            lblGioDat.TabIndex = 2;
            lblGioDat.Text = "Giờ đặt:";
            // 
            // dtpNgayDat
            // 
            dtpNgayDat.Font = new Font("Segoe UI", 11F);
            dtpNgayDat.Format = DateTimePickerFormat.Short;
            dtpNgayDat.Location = new Point(130, 45);
            dtpNgayDat.Name = "dtpNgayDat";
            dtpNgayDat.Size = new Size(508, 37);
            dtpNgayDat.TabIndex = 1;
            // 
            // lblNgayDat
            // 
            lblNgayDat.AutoSize = true;
            lblNgayDat.Font = new Font("Segoe UI", 11F);
            lblNgayDat.Location = new Point(18, 48);
            lblNgayDat.Name = "lblNgayDat";
            lblNgayDat.Size = new Size(106, 30);
            lblNgayDat.TabIndex = 0;
            lblNgayDat.Text = "Ngày đặt:";
            // 
            // groupBoxKhach
            // 
            groupBoxKhach.Controls.Add(btnTimKhachHang);
            groupBoxKhach.Controls.Add(txtSoDienThoai);
            groupBoxKhach.Controls.Add(lblSoDienThoai);
            groupBoxKhach.Controls.Add(txtTenKhach);
            groupBoxKhach.Controls.Add(lblTenKhach);
            groupBoxKhach.Dock = DockStyle.Top;
            groupBoxKhach.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            groupBoxKhach.Location = new Point(20, 384);
            groupBoxKhach.Name = "groupBoxKhach";
            groupBoxKhach.Padding = new Padding(15);
            groupBoxKhach.Size = new Size(860, 131);
            groupBoxKhach.TabIndex = 1;
            groupBoxKhach.TabStop = false;
            groupBoxKhach.Text = "Thông tin khách hàng";
            // 
            // btnTimKhachHang
            // 
            btnTimKhachHang.BackColor = Color.FromArgb(99, 102, 241);
            btnTimKhachHang.Cursor = Cursors.Hand;
            btnTimKhachHang.FlatAppearance.BorderSize = 0;
            btnTimKhachHang.FlatStyle = FlatStyle.Flat;
            btnTimKhachHang.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnTimKhachHang.ForeColor = Color.White;
            btnTimKhachHang.Location = new Point(478, 35);
            btnTimKhachHang.Name = "btnTimKhachHang";
            btnTimKhachHang.Size = new Size(160, 35);
            btnTimKhachHang.TabIndex = 6;
            btnTimKhachHang.Text = "🔍 Tìm khách hàng";
            btnTimKhachHang.UseVisualStyleBackColor = false;
            btnTimKhachHang.Click += BtnTimKhachHang_Click;
            // 
            // txtSoDienThoai
            // 
            txtSoDienThoai.Font = new Font("Segoe UI", 11F);
            txtSoDienThoai.Location = new Point(160, 78);
            txtSoDienThoai.Name = "txtSoDienThoai";
            txtSoDienThoai.PlaceholderText = "Nhập số điện thoại";
            txtSoDienThoai.Size = new Size(478, 37);
            txtSoDienThoai.TabIndex = 3;
            // 
            // lblSoDienThoai
            // 
            lblSoDienThoai.AutoSize = true;
            lblSoDienThoai.Font = new Font("Segoe UI", 11F);
            lblSoDienThoai.Location = new Point(18, 81);
            lblSoDienThoai.Name = "lblSoDienThoai";
            lblSoDienThoai.Size = new Size(145, 30);
            lblSoDienThoai.TabIndex = 2;
            lblSoDienThoai.Text = "Số điện thoại:";
            // 
            // txtTenKhach
            // 
            txtTenKhach.Font = new Font("Segoe UI", 11F);
            txtTenKhach.Location = new Point(160, 36);
            txtTenKhach.Name = "txtTenKhach";
            txtTenKhach.PlaceholderText = "Nhập tên khách hàng";
            txtTenKhach.Size = new Size(300, 37);
            txtTenKhach.TabIndex = 1;
            // 
            // lblTenKhach
            // 
            lblTenKhach.AutoSize = true;
            lblTenKhach.Font = new Font("Segoe UI", 11F);
            lblTenKhach.Location = new Point(18, 39);
            lblTenKhach.Name = "lblTenKhach";
            lblTenKhach.Size = new Size(168, 30);
            lblTenKhach.TabIndex = 0;
            lblTenKhach.Text = "Tên khách hàng:";
            // 
            // groupBoxBan
            // 
            groupBoxBan.Controls.Add(flpBanTrong);
            groupBoxBan.Controls.Add(lblChonBan);
            groupBoxBan.Dock = DockStyle.Top;
            groupBoxBan.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            groupBoxBan.Location = new Point(20, 20);
            groupBoxBan.Name = "groupBoxBan";
            groupBoxBan.Padding = new Padding(15);
            groupBoxBan.Size = new Size(860, 364);
            groupBoxBan.TabIndex = 0;
            groupBoxBan.TabStop = false;
            groupBoxBan.Text = "Chọn bàn trống";
            // 
            // flpBanTrong
            // 
            flpBanTrong.AutoScroll = true;
            flpBanTrong.BackColor = Color.FromArgb(248, 250, 252);
            flpBanTrong.Dock = DockStyle.Fill;
            flpBanTrong.Location = new Point(15, 75);
            flpBanTrong.Name = "flpBanTrong";
            flpBanTrong.Padding = new Padding(5);
            flpBanTrong.Size = new Size(830, 274);
            flpBanTrong.TabIndex = 1;
            // 
            // lblChonBan
            // 
            lblChonBan.Dock = DockStyle.Top;
            lblChonBan.Font = new Font("Segoe UI", 10F);
            lblChonBan.ForeColor = Color.FromArgb(100, 116, 139);
            lblChonBan.Location = new Point(15, 45);
            lblChonBan.Name = "lblChonBan";
            lblChonBan.Size = new Size(830, 30);
            lblChonBan.TabIndex = 0;
            lblChonBan.Text = "Nhấp vào bàn để chọn";
            lblChonBan.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlButtons
            // 
            pnlButtons.BackColor = Color.FromArgb(248, 250, 252);
            pnlButtons.Controls.Add(btnHuy);
            pnlButtons.Controls.Add(btnXacNhan);
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.Location = new Point(0, 953);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Padding = new Padding(20);
            pnlButtons.Size = new Size(900, 80);
            pnlButtons.TabIndex = 2;
            // 
            // btnHuy
            // 
            btnHuy.BackColor = Color.FromArgb(226, 232, 240);
            btnHuy.Cursor = Cursors.Hand;
            btnHuy.FlatAppearance.BorderSize = 0;
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnHuy.ForeColor = Color.FromArgb(51, 65, 85);
            btnHuy.Location = new Point(560, 20);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(150, 45);
            btnHuy.TabIndex = 1;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = false;
            btnHuy.Click += BtnHuy_Click;
            // 
            // btnXacNhan
            // 
            btnXacNhan.BackColor = Color.FromArgb(34, 197, 94);
            btnXacNhan.Cursor = Cursors.Hand;
            btnXacNhan.FlatAppearance.BorderSize = 0;
            btnXacNhan.FlatStyle = FlatStyle.Flat;
            btnXacNhan.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnXacNhan.ForeColor = Color.White;
            btnXacNhan.Location = new Point(730, 20);
            btnXacNhan.Name = "btnXacNhan";
            btnXacNhan.Size = new Size(150, 45);
            btnXacNhan.TabIndex = 0;
            btnXacNhan.Text = "✓ Xác nhận đặt";
            btnXacNhan.UseVisualStyleBackColor = false;
            btnXacNhan.Click += BtnXacNhan_Click;
            // 
            // DatBanForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 1033);
            Controls.Add(pnlContent);
            Controls.Add(pnlButtons);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DatBanForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Đặt bàn trước";
            Load += DatBanForm_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlContent.ResumeLayout(false);
            groupBoxGhiChu.ResumeLayout(false);
            groupBoxGhiChu.PerformLayout();
            groupBoxThoiGian.ResumeLayout(false);
            groupBoxThoiGian.PerformLayout();
            groupBoxKhach.ResumeLayout(false);
            groupBoxKhach.PerformLayout();
            groupBoxBan.ResumeLayout(false);
            pnlButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.GroupBox groupBoxBan;
        private System.Windows.Forms.FlowLayoutPanel flpBanTrong;
        private System.Windows.Forms.Label lblChonBan;
        private System.Windows.Forms.GroupBox groupBoxKhach;
        private System.Windows.Forms.TextBox txtTenKhach;
        private System.Windows.Forms.Label lblTenKhach;
        private System.Windows.Forms.TextBox txtSoDienThoai;
        private System.Windows.Forms.Label lblSoDienThoai;
        private System.Windows.Forms.Button btnTimKhachHang;
        private System.Windows.Forms.GroupBox groupBoxThoiGian;
        private System.Windows.Forms.DateTimePicker dtpNgayDat;
        private System.Windows.Forms.Label lblNgayDat;
        private System.Windows.Forms.DateTimePicker dtpGioKetThuc;
        private System.Windows.Forms.Label lblGioKetThuc;
        private System.Windows.Forms.DateTimePicker dtpGioDat;
        private System.Windows.Forms.Label lblGioDat;
        private System.Windows.Forms.GroupBox groupBoxGhiChu;
        private System.Windows.Forms.TextBox txtGhiChu;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.Button btnXacNhan;
    }
}