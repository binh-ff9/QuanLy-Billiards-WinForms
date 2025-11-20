namespace Billiard.WinForm
{
    partial class MainForm
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
            pnlUserInfo = new Panel();
            btnLogout = new Button();
            lblUserRole = new Label();
            lblUserName = new Label();
            lblUserAvatar = new Label();
            lblTitle = new Label();
            pnlStats = new Panel();
            tableLayoutStats = new TableLayoutPanel();
            pnlStatTrong = new Panel();
            lblBanTrongValue = new Label();
            lblBanTrongLabel = new Label();
            pnlStatDangChoi = new Panel();
            lblDangChoiValue = new Label();
            lblDangChoiLabel = new Label();
            pnlStatDatTruoc = new Panel();
            lblDatTruocValue = new Label();
            lblDatTruocLabel = new Label();
            pnlStatDoanhThu = new Panel();
            lblDoanhThuValue = new Label();
            lblDoanhThuLabel = new Label();
            pnlStatKhachHang = new Panel();
            lblKhachHangValue = new Label();
            lblKhachHangLabel = new Label();
            pnlSidebar = new Panel();
            btnQuanLyBan = new Button();
            btnDichVu = new Button();
            btnHoaDon = new Button();
            btnKhachHang = new Button();
            btnThongKe = new Button();
            btnNhanVien = new Button();
            btnCaiDat = new Button();
            pnlMain = new Panel();
            pnlDetail = new Panel();
            pnlHeader.SuspendLayout();
            pnlUserInfo.SuspendLayout();
            pnlStats.SuspendLayout();
            tableLayoutStats.SuspendLayout();
            pnlStatTrong.SuspendLayout();
            pnlStatDangChoi.SuspendLayout();
            pnlStatDatTruoc.SuspendLayout();
            pnlStatDoanhThu.SuspendLayout();
            pnlStatKhachHang.SuspendLayout();
            pnlSidebar.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.MidnightBlue;
            pnlHeader.Controls.Add(pnlUserInfo);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1400, 96);
            pnlHeader.TabIndex = 0;
            // 
            // pnlUserInfo
            // 
            pnlUserInfo.Controls.Add(btnLogout);
            pnlUserInfo.Controls.Add(lblUserRole);
            pnlUserInfo.Controls.Add(lblUserName);
            pnlUserInfo.Controls.Add(lblUserAvatar);
            pnlUserInfo.Dock = DockStyle.Right;
            pnlUserInfo.Location = new Point(967, 0);
            pnlUserInfo.Name = "pnlUserInfo";
            pnlUserInfo.Size = new Size(433, 96);
            pnlUserInfo.TabIndex = 1;
            // 
            // btnLogout
            // 
            btnLogout.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLogout.BackColor = Color.FromArgb(220, 53, 69);
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLogout.ForeColor = Color.White;
            btnLogout.Location = new Point(293, 25);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(120, 45);
            btnLogout.TabIndex = 3;
            btnLogout.Text = "Đăng xuất";
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += BtnLogout_Click;
            btnLogout.MouseEnter += Button_MouseEnter;
            btnLogout.MouseLeave += Button_MouseLeave;
            // 
            // lblUserRole
            // 
            lblUserRole.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblUserRole.AutoSize = true;
            lblUserRole.Font = new Font("Segoe UI", 9F);
            lblUserRole.ForeColor = Color.FromArgb(173, 181, 189);
            lblUserRole.Location = new Point(91, 50);
            lblUserRole.Name = "lblUserRole";
            lblUserRole.Size = new Size(91, 25);
            lblUserRole.TabIndex = 2;
            lblUserRole.Text = "Nhân viên";
            // 
            // lblUserName
            // 
            lblUserName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblUserName.AutoSize = true;
            lblUserName.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblUserName.ForeColor = Color.White;
            lblUserName.Location = new Point(91, 20);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(117, 30);
            lblUserName.TabIndex = 1;
            lblUserName.Text = "Nhân viên";
            // 
            // lblUserAvatar
            // 
            lblUserAvatar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblUserAvatar.BackColor = Color.FromArgb(102, 126, 234);
            lblUserAvatar.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblUserAvatar.ForeColor = Color.White;
            lblUserAvatar.Location = new Point(18, 20);
            lblUserAvatar.Name = "lblUserAvatar";
            lblUserAvatar.Size = new Size(50, 50);
            lblUserAvatar.TabIndex = 0;
            lblUserAvatar.Text = "NV";
            lblUserAvatar.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 25);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(461, 48);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "🎱 Quản Lý Quán Bi-a Pro";
            // 
            // pnlStats
            // 
            pnlStats.BackColor = Color.MidnightBlue;
            pnlStats.Controls.Add(tableLayoutStats);
            pnlStats.Dock = DockStyle.Top;
            pnlStats.Location = new Point(0, 96);
            pnlStats.Name = "pnlStats";
            pnlStats.Padding = new Padding(15);
            pnlStats.Size = new Size(1400, 161);
            pnlStats.TabIndex = 1;
            // 
            // tableLayoutStats
            // 
            tableLayoutStats.ColumnCount = 5;
            tableLayoutStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutStats.Controls.Add(pnlStatTrong, 0, 0);
            tableLayoutStats.Controls.Add(pnlStatDangChoi, 1, 0);
            tableLayoutStats.Controls.Add(pnlStatDatTruoc, 2, 0);
            tableLayoutStats.Controls.Add(pnlStatDoanhThu, 3, 0);
            tableLayoutStats.Controls.Add(pnlStatKhachHang, 4, 0);
            tableLayoutStats.Dock = DockStyle.Fill;
            tableLayoutStats.Location = new Point(15, 15);
            tableLayoutStats.Name = "tableLayoutStats";
            tableLayoutStats.RowCount = 1;
            tableLayoutStats.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutStats.Size = new Size(1370, 131);
            tableLayoutStats.TabIndex = 0;
            // 
            // pnlStatTrong
            // 
            pnlStatTrong.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlStatTrong.BackColor = Color.White;
            pnlStatTrong.BorderStyle = BorderStyle.FixedSingle;
            pnlStatTrong.Controls.Add(lblBanTrongValue);
            pnlStatTrong.Controls.Add(lblBanTrongLabel);
            pnlStatTrong.Location = new Point(3, 3);
            pnlStatTrong.Margin = new Padding(3, 3, 5, 3);
            pnlStatTrong.Name = "pnlStatTrong";
            pnlStatTrong.Size = new Size(266, 125);
            pnlStatTrong.TabIndex = 0;
            // 
            // lblBanTrongValue
            // 
            lblBanTrongValue.Dock = DockStyle.Top;
            lblBanTrongValue.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblBanTrongValue.ForeColor = Color.FromArgb(34, 139, 34);
            lblBanTrongValue.Location = new Point(0, 0);
            lblBanTrongValue.Name = "lblBanTrongValue";
            lblBanTrongValue.Padding = new Padding(10, 10, 0, 0);
            lblBanTrongValue.Size = new Size(264, 87);
            lblBanTrongValue.TabIndex = 0;
            lblBanTrongValue.Text = "0";
            // 
            // lblBanTrongLabel
            // 
            lblBanTrongLabel.Dock = DockStyle.Bottom;
            lblBanTrongLabel.Font = new Font("Segoe UI", 10F);
            lblBanTrongLabel.ForeColor = Color.FromArgb(80, 100, 90);
            lblBanTrongLabel.Location = new Point(0, 87);
            lblBanTrongLabel.Name = "lblBanTrongLabel";
            lblBanTrongLabel.Padding = new Padding(10, 0, 0, 5);
            lblBanTrongLabel.Size = new Size(264, 36);
            lblBanTrongLabel.TabIndex = 1;
            lblBanTrongLabel.Text = "Bàn Trống";
            // 
            // pnlStatDangChoi
            // 
            pnlStatDangChoi.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlStatDangChoi.BackColor = Color.White;
            pnlStatDangChoi.BorderStyle = BorderStyle.FixedSingle;
            pnlStatDangChoi.Controls.Add(lblDangChoiValue);
            pnlStatDangChoi.Controls.Add(lblDangChoiLabel);
            pnlStatDangChoi.Location = new Point(277, 3);
            pnlStatDangChoi.Margin = new Padding(3, 3, 5, 3);
            pnlStatDangChoi.Name = "pnlStatDangChoi";
            pnlStatDangChoi.Size = new Size(266, 125);
            pnlStatDangChoi.TabIndex = 1;
            // 
            // lblDangChoiValue
            // 
            lblDangChoiValue.Dock = DockStyle.Top;
            lblDangChoiValue.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblDangChoiValue.ForeColor = Color.FromArgb(218, 165, 32);
            lblDangChoiValue.Location = new Point(0, 0);
            lblDangChoiValue.Name = "lblDangChoiValue";
            lblDangChoiValue.Padding = new Padding(10, 10, 0, 0);
            lblDangChoiValue.Size = new Size(264, 87);
            lblDangChoiValue.TabIndex = 0;
            lblDangChoiValue.Text = "0";
            // 
            // lblDangChoiLabel
            // 
            lblDangChoiLabel.Dock = DockStyle.Bottom;
            lblDangChoiLabel.Font = new Font("Segoe UI", 10F);
            lblDangChoiLabel.ForeColor = Color.FromArgb(80, 100, 90);
            lblDangChoiLabel.Location = new Point(0, 87);
            lblDangChoiLabel.Name = "lblDangChoiLabel";
            lblDangChoiLabel.Padding = new Padding(10, 0, 0, 5);
            lblDangChoiLabel.Size = new Size(264, 36);
            lblDangChoiLabel.TabIndex = 1;
            lblDangChoiLabel.Text = "Đang Chơi";
            // 
            // pnlStatDatTruoc
            // 
            pnlStatDatTruoc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlStatDatTruoc.BackColor = Color.White;
            pnlStatDatTruoc.BorderStyle = BorderStyle.FixedSingle;
            pnlStatDatTruoc.Controls.Add(lblDatTruocValue);
            pnlStatDatTruoc.Controls.Add(lblDatTruocLabel);
            pnlStatDatTruoc.Location = new Point(551, 3);
            pnlStatDatTruoc.Margin = new Padding(3, 3, 5, 3);
            pnlStatDatTruoc.Name = "pnlStatDatTruoc";
            pnlStatDatTruoc.Size = new Size(266, 125);
            pnlStatDatTruoc.TabIndex = 2;
            // 
            // lblDatTruocValue
            // 
            lblDatTruocValue.Dock = DockStyle.Top;
            lblDatTruocValue.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblDatTruocValue.ForeColor = Color.FromArgb(70, 130, 180);
            lblDatTruocValue.Location = new Point(0, 0);
            lblDatTruocValue.Name = "lblDatTruocValue";
            lblDatTruocValue.Padding = new Padding(10, 10, 0, 0);
            lblDatTruocValue.Size = new Size(264, 87);
            lblDatTruocValue.TabIndex = 0;
            lblDatTruocValue.Text = "0";
            // 
            // lblDatTruocLabel
            // 
            lblDatTruocLabel.Dock = DockStyle.Bottom;
            lblDatTruocLabel.Font = new Font("Segoe UI", 10F);
            lblDatTruocLabel.ForeColor = Color.FromArgb(80, 100, 90);
            lblDatTruocLabel.Location = new Point(0, 87);
            lblDatTruocLabel.Name = "lblDatTruocLabel";
            lblDatTruocLabel.Padding = new Padding(10, 0, 0, 5);
            lblDatTruocLabel.Size = new Size(264, 36);
            lblDatTruocLabel.TabIndex = 1;
            lblDatTruocLabel.Text = "Đặt Trước";
            // 
            // pnlStatDoanhThu
            // 
            pnlStatDoanhThu.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlStatDoanhThu.BackColor = Color.White;
            pnlStatDoanhThu.BorderStyle = BorderStyle.FixedSingle;
            pnlStatDoanhThu.Controls.Add(lblDoanhThuValue);
            pnlStatDoanhThu.Controls.Add(lblDoanhThuLabel);
            pnlStatDoanhThu.Location = new Point(825, 3);
            pnlStatDoanhThu.Margin = new Padding(3, 3, 5, 3);
            pnlStatDoanhThu.Name = "pnlStatDoanhThu";
            pnlStatDoanhThu.Size = new Size(266, 125);
            pnlStatDoanhThu.TabIndex = 3;
            // 
            // lblDoanhThuValue
            // 
            lblDoanhThuValue.Dock = DockStyle.Top;
            lblDoanhThuValue.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblDoanhThuValue.ForeColor = Color.FromArgb(178, 34, 34);
            lblDoanhThuValue.Location = new Point(0, 0);
            lblDoanhThuValue.Name = "lblDoanhThuValue";
            lblDoanhThuValue.Padding = new Padding(10, 10, 0, 0);
            lblDoanhThuValue.Size = new Size(264, 87);
            lblDoanhThuValue.TabIndex = 0;
            lblDoanhThuValue.Text = "0đ";
            // 
            // lblDoanhThuLabel
            // 
            lblDoanhThuLabel.Dock = DockStyle.Bottom;
            lblDoanhThuLabel.Font = new Font("Segoe UI", 10F);
            lblDoanhThuLabel.ForeColor = Color.FromArgb(80, 100, 90);
            lblDoanhThuLabel.Location = new Point(0, 87);
            lblDoanhThuLabel.Name = "lblDoanhThuLabel";
            lblDoanhThuLabel.Padding = new Padding(10, 0, 0, 5);
            lblDoanhThuLabel.Size = new Size(264, 36);
            lblDoanhThuLabel.TabIndex = 1;
            lblDoanhThuLabel.Text = "Doanh Thu Hôm Nay";
            // 
            // pnlStatKhachHang
            // 
            pnlStatKhachHang.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlStatKhachHang.BackColor = Color.White;
            pnlStatKhachHang.BorderStyle = BorderStyle.FixedSingle;
            pnlStatKhachHang.Controls.Add(lblKhachHangValue);
            pnlStatKhachHang.Controls.Add(lblKhachHangLabel);
            pnlStatKhachHang.Location = new Point(1099, 3);
            pnlStatKhachHang.Name = "pnlStatKhachHang";
            pnlStatKhachHang.Size = new Size(268, 125);
            pnlStatKhachHang.TabIndex = 4;
            // 
            // lblKhachHangValue
            // 
            lblKhachHangValue.Dock = DockStyle.Top;
            lblKhachHangValue.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblKhachHangValue.ForeColor = Color.FromArgb(128, 0, 128);
            lblKhachHangValue.Location = new Point(0, 0);
            lblKhachHangValue.Name = "lblKhachHangValue";
            lblKhachHangValue.Padding = new Padding(10, 10, 0, 0);
            lblKhachHangValue.Size = new Size(266, 87);
            lblKhachHangValue.TabIndex = 0;
            lblKhachHangValue.Text = "0";
            // 
            // lblKhachHangLabel
            // 
            lblKhachHangLabel.Dock = DockStyle.Bottom;
            lblKhachHangLabel.Font = new Font("Segoe UI", 10F);
            lblKhachHangLabel.ForeColor = Color.FromArgb(80, 100, 90);
            lblKhachHangLabel.Location = new Point(0, 87);
            lblKhachHangLabel.Name = "lblKhachHangLabel";
            lblKhachHangLabel.Padding = new Padding(10, 0, 0, 5);
            lblKhachHangLabel.Size = new Size(266, 36);
            lblKhachHangLabel.TabIndex = 1;
            lblKhachHangLabel.Text = "Khách Hàng";
            // 
            // pnlSidebar
            // 
            pnlSidebar.BackColor = Color.MidnightBlue;
            pnlSidebar.Controls.Add(btnQuanLyBan);
            pnlSidebar.Controls.Add(btnDichVu);
            pnlSidebar.Controls.Add(btnHoaDon);
            pnlSidebar.Controls.Add(btnKhachHang);
            pnlSidebar.Controls.Add(btnThongKe);
            pnlSidebar.Controls.Add(btnNhanVien);
            pnlSidebar.Controls.Add(btnCaiDat);
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Location = new Point(0, 257);
            pnlSidebar.MinimumSize = new Size(200, 0);
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Size = new Size(250, 1253);
            pnlSidebar.TabIndex = 2;
            // 
            // btnQuanLyBan
            // 
            btnQuanLyBan.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnQuanLyBan.BackColor = Color.Transparent;
            btnQuanLyBan.Cursor = Cursors.Hand;
            btnQuanLyBan.FlatAppearance.BorderSize = 0;
            btnQuanLyBan.FlatStyle = FlatStyle.Flat;
            btnQuanLyBan.Font = new Font("Segoe UI", 11F);
            btnQuanLyBan.ForeColor = Color.White;
            btnQuanLyBan.Location = new Point(10, 10);
            btnQuanLyBan.Name = "btnQuanLyBan";
            btnQuanLyBan.Padding = new Padding(10, 0, 0, 0);
            btnQuanLyBan.Size = new Size(230, 50);
            btnQuanLyBan.TabIndex = 0;
            btnQuanLyBan.Text = "📊 Quản lý bàn";
            btnQuanLyBan.TextAlign = ContentAlignment.MiddleLeft;
            btnQuanLyBan.UseVisualStyleBackColor = false;
            btnQuanLyBan.Click += SidebarButton_Click;
            btnQuanLyBan.MouseEnter += SidebarButton_MouseEnter;
            btnQuanLyBan.MouseLeave += SidebarButton_MouseLeave;
            // 
            // btnDichVu
            // 
            btnDichVu.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnDichVu.BackColor = Color.Transparent;
            btnDichVu.Cursor = Cursors.Hand;
            btnDichVu.FlatAppearance.BorderSize = 0;
            btnDichVu.FlatStyle = FlatStyle.Flat;
            btnDichVu.Font = new Font("Segoe UI", 11F);
            btnDichVu.ForeColor = Color.White;
            btnDichVu.Location = new Point(10, 70);
            btnDichVu.Name = "btnDichVu";
            btnDichVu.Padding = new Padding(10, 0, 0, 0);
            btnDichVu.Size = new Size(230, 50);
            btnDichVu.TabIndex = 1;
            btnDichVu.Text = "🍴 Dịch vụ && Menu";
            btnDichVu.TextAlign = ContentAlignment.MiddleLeft;
            btnDichVu.UseVisualStyleBackColor = false;
            btnDichVu.Click += SidebarButton_Click;
            btnDichVu.MouseEnter += SidebarButton_MouseEnter;
            btnDichVu.MouseLeave += SidebarButton_MouseLeave;
            // 
            // btnHoaDon
            // 
            btnHoaDon.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnHoaDon.BackColor = Color.Transparent;
            btnHoaDon.Cursor = Cursors.Hand;
            btnHoaDon.FlatAppearance.BorderSize = 0;
            btnHoaDon.FlatStyle = FlatStyle.Flat;
            btnHoaDon.Font = new Font("Segoe UI", 11F);
            btnHoaDon.ForeColor = Color.White;
            btnHoaDon.Location = new Point(10, 130);
            btnHoaDon.Name = "btnHoaDon";
            btnHoaDon.Padding = new Padding(10, 0, 0, 0);
            btnHoaDon.Size = new Size(230, 50);
            btnHoaDon.TabIndex = 2;
            btnHoaDon.Text = "📄 Hóa đơn";
            btnHoaDon.TextAlign = ContentAlignment.MiddleLeft;
            btnHoaDon.UseVisualStyleBackColor = false;
            btnHoaDon.Click += SidebarButton_Click;
            btnHoaDon.MouseEnter += SidebarButton_MouseEnter;
            btnHoaDon.MouseLeave += SidebarButton_MouseLeave;
            // 
            // btnKhachHang
            // 
            btnKhachHang.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnKhachHang.BackColor = Color.Transparent;
            btnKhachHang.Cursor = Cursors.Hand;
            btnKhachHang.FlatAppearance.BorderSize = 0;
            btnKhachHang.FlatStyle = FlatStyle.Flat;
            btnKhachHang.Font = new Font("Segoe UI", 11F);
            btnKhachHang.ForeColor = Color.White;
            btnKhachHang.Location = new Point(10, 190);
            btnKhachHang.Name = "btnKhachHang";
            btnKhachHang.Padding = new Padding(10, 0, 0, 0);
            btnKhachHang.Size = new Size(230, 50);
            btnKhachHang.TabIndex = 3;
            btnKhachHang.Text = "👥 Khách hàng";
            btnKhachHang.TextAlign = ContentAlignment.MiddleLeft;
            btnKhachHang.UseVisualStyleBackColor = false;
            btnKhachHang.Click += SidebarButton_Click;
            btnKhachHang.MouseEnter += SidebarButton_MouseEnter;
            btnKhachHang.MouseLeave += SidebarButton_MouseLeave;
            // 
            // btnThongKe
            // 
            btnThongKe.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnThongKe.BackColor = Color.Transparent;
            btnThongKe.Cursor = Cursors.Hand;
            btnThongKe.FlatAppearance.BorderSize = 0;
            btnThongKe.FlatStyle = FlatStyle.Flat;
            btnThongKe.Font = new Font("Segoe UI", 11F);
            btnThongKe.ForeColor = Color.White;
            btnThongKe.Location = new Point(10, 250);
            btnThongKe.Name = "btnThongKe";
            btnThongKe.Padding = new Padding(10, 0, 0, 0);
            btnThongKe.Size = new Size(230, 50);
            btnThongKe.TabIndex = 4;
            btnThongKe.Text = "📈 Thống kê";
            btnThongKe.TextAlign = ContentAlignment.MiddleLeft;
            btnThongKe.UseVisualStyleBackColor = false;
            btnThongKe.Click += SidebarButton_Click;
            btnThongKe.MouseEnter += SidebarButton_MouseEnter;
            btnThongKe.MouseLeave += SidebarButton_MouseLeave;
            // 
            // btnNhanVien
            // 
            btnNhanVien.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnNhanVien.BackColor = Color.Transparent;
            btnNhanVien.Cursor = Cursors.Hand;
            btnNhanVien.FlatAppearance.BorderSize = 0;
            btnNhanVien.FlatStyle = FlatStyle.Flat;
            btnNhanVien.Font = new Font("Segoe UI", 11F);
            btnNhanVien.ForeColor = Color.White;
            btnNhanVien.Location = new Point(10, 310);
            btnNhanVien.Name = "btnNhanVien";
            btnNhanVien.Padding = new Padding(10, 0, 0, 0);
            btnNhanVien.Size = new Size(230, 50);
            btnNhanVien.TabIndex = 5;
            btnNhanVien.Text = "\U0001f9d1‍💻 Nhân viên";
            btnNhanVien.TextAlign = ContentAlignment.MiddleLeft;
            btnNhanVien.UseVisualStyleBackColor = false;
            btnNhanVien.Click += SidebarButton_Click;
            btnNhanVien.MouseEnter += SidebarButton_MouseEnter;
            btnNhanVien.MouseLeave += SidebarButton_MouseLeave;
            // 
            // btnCaiDat
            // 
            btnCaiDat.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnCaiDat.BackColor = Color.Transparent;
            btnCaiDat.Cursor = Cursors.Hand;
            btnCaiDat.FlatAppearance.BorderSize = 0;
            btnCaiDat.FlatStyle = FlatStyle.Flat;
            btnCaiDat.Font = new Font("Segoe UI", 11F);
            btnCaiDat.ForeColor = Color.White;
            btnCaiDat.Location = new Point(10, 1193);
            btnCaiDat.Name = "btnCaiDat";
            btnCaiDat.Padding = new Padding(10, 0, 0, 0);
            btnCaiDat.Size = new Size(230, 50);
            btnCaiDat.TabIndex = 6;
            btnCaiDat.Text = "⚙️ Cài đặt";
            btnCaiDat.TextAlign = ContentAlignment.MiddleLeft;
            btnCaiDat.UseVisualStyleBackColor = false;
            btnCaiDat.Click += SidebarButton_Click;
            btnCaiDat.MouseEnter += SidebarButton_MouseEnter;
            btnCaiDat.MouseLeave += SidebarButton_MouseLeave;
            // 
            // pnlMain
            // 
            pnlMain.BackColor = Color.White;
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(250, 257);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(1150, 1253);
            pnlMain.TabIndex = 3;
            // 
            // pnlDetail
            // 
            pnlDetail.BackColor = Color.White;
            pnlDetail.BorderStyle = BorderStyle.FixedSingle;
            pnlDetail.Dock = DockStyle.Right;
            pnlDetail.Location = new Point(800, 257);
            pnlDetail.MinimumSize = new Size(300, 0);
            pnlDetail.Name = "pnlDetail";
            pnlDetail.Size = new Size(600, 1253);
            pnlDetail.TabIndex = 4;
            pnlDetail.Visible = false;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1400, 1510);
            Controls.Add(pnlDetail);
            Controls.Add(pnlMain);
            Controls.Add(pnlSidebar);
            Controls.Add(pnlStats);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            MinimumSize = new Size(1024, 768);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản Lý Quán Bi-a Pro";
            WindowState = FormWindowState.Maximized;
            Load += MainForm_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlUserInfo.ResumeLayout(false);
            pnlUserInfo.PerformLayout();
            pnlStats.ResumeLayout(false);
            tableLayoutStats.ResumeLayout(false);
            pnlStatTrong.ResumeLayout(false);
            pnlStatDangChoi.ResumeLayout(false);
            pnlStatDatTruoc.ResumeLayout(false);
            pnlStatDoanhThu.ResumeLayout(false);
            pnlStatKhachHang.ResumeLayout(false);
            pnlSidebar.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlUserInfo;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Label lblUserRole;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblUserAvatar;
        private System.Windows.Forms.Panel pnlStats;
        private System.Windows.Forms.TableLayoutPanel tableLayoutStats;
        private System.Windows.Forms.Panel pnlStatTrong;
        private System.Windows.Forms.Label lblBanTrongValue;
        private System.Windows.Forms.Label lblBanTrongLabel;
        private System.Windows.Forms.Panel pnlStatDangChoi;
        private System.Windows.Forms.Label lblDangChoiValue;
        private System.Windows.Forms.Label lblDangChoiLabel;
        private System.Windows.Forms.Panel pnlStatDatTruoc;
        private System.Windows.Forms.Label lblDatTruocValue;
        private System.Windows.Forms.Label lblDatTruocLabel;
        private System.Windows.Forms.Panel pnlStatDoanhThu;
        private System.Windows.Forms.Label lblDoanhThuValue;
        private System.Windows.Forms.Label lblDoanhThuLabel;
        private System.Windows.Forms.Panel pnlStatKhachHang;
        private System.Windows.Forms.Label lblKhachHangValue;
        private System.Windows.Forms.Label lblKhachHangLabel;
        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Button btnQuanLyBan;
        private System.Windows.Forms.Button btnDichVu;
        private System.Windows.Forms.Button btnHoaDon;
        private System.Windows.Forms.Button btnKhachHang;
        private System.Windows.Forms.Button btnThongKe;
        private System.Windows.Forms.Button btnNhanVien;
        private System.Windows.Forms.Button btnCaiDat;
        private System.Windows.Forms.Panel pnlMain;
        public System.Windows.Forms.Panel pnlDetail;
    }
}