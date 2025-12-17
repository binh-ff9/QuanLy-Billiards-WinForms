namespace Billiard.WinForm.Forms.ThongKe
{
    partial class ThongKeForm
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
            panelHeader = new Panel();
            btnHomNay = new Button();
            btnXemBaoCao = new Button();
            dtpDenNgay = new DateTimePicker();
            lblDen = new Label();
            dtpTuNgay = new DateTimePicker();
            lblTitle = new Label();
            panelCards = new Panel();
            cardTrungBinh = new Panel();
            lblTrungBinhSubtitle = new Label();
            lblTrungBinhValue = new Label();
            lblTrungBinhTitle = new Label();
            lblTrungBinhIcon = new Label();
            cardKhachHang = new Panel();
            lblKhachHangSubtitle = new Label();
            lblKhachHangValue = new Label();
            lblKhachHangTitle = new Label();
            lblKhachHangIcon = new Label();
            cardHoaDon = new Panel();
            lblHoaDonSubtitle = new Label();
            lblHoaDonValue = new Label();
            lblHoaDonTitle = new Label();
            lblHoaDonIcon = new Label();
            cardDoanhThu = new Panel();
            lblDoanhThuValue = new Label();
            lblDoanhThuTitle = new Label();
            lblDoanhThuIcon = new Label();
            tabControl = new TabControl();
            tabTongQuan = new TabPage();
            panelTongQuan = new Panel();
            panelDoanhThu7Ngay = new Panel();
            tabDichVu = new TabPage();
            panelDichVu = new Panel();
            tabKhachHang = new TabPage();
            dgvTopKhachHang = new DataGridView();
            colSTT = new DataGridViewTextBoxColumn();
            colTenKH = new DataGridViewTextBoxColumn();
            colSDT = new DataGridViewTextBoxColumn();
            colTongChiTieu = new DataGridViewTextBoxColumn();
            colSoLanDen = new DataGridViewTextBoxColumn();
            tabKhac = new TabPage();
            panelKhac = new Panel();
            panelSoSanh = new Panel();
            cardChenhLech = new Panel();
            lblChenhLechPercent = new Label();
            lblChenhLechValue = new Label();
            lblChenhLechTitle = new Label();
            cardKyTruoc = new Panel();
            lblKyTruocValue = new Label();
            lblKyTruocTitle = new Label();
            cardKyHienTai = new Panel();
            lblKyHienTaiPercent = new Label();
            lblKyHienTaiValue = new Label();
            lblKyHienTaiTitle = new Label();
            panelSoSanhHeader = new Panel();
            btnThang = new Button();
            btnTuan = new Button();
            btnNgay = new Button();
            lblSoSanhTitle = new Label();
            panelThangHeader = new Panel();
            cboNam = new ComboBox();
            lblDoanhThuThang = new Label();

            // Khởi tạo các Charts
            chartDoanhThu7Ngay = new System.Windows.Forms.DataVisualization.Charting.Chart();
            chartDoanhThuThang = new System.Windows.Forms.DataVisualization.Charting.Chart();
            chartKhungGio = new System.Windows.Forms.DataVisualization.Charting.Chart();
            chartPhuongThuc = new System.Windows.Forms.DataVisualization.Charting.Chart();
            chartTopDichVu = new System.Windows.Forms.DataVisualization.Charting.Chart();
            chartLoaiDichVu = new System.Windows.Forms.DataVisualization.Charting.Chart();
            chartLoaiBan = new System.Windows.Forms.DataVisualization.Charting.Chart();

            panelHeader.SuspendLayout();
            panelCards.SuspendLayout();
            cardTrungBinh.SuspendLayout();
            cardKhachHang.SuspendLayout();
            cardHoaDon.SuspendLayout();
            cardDoanhThu.SuspendLayout();
            tabControl.SuspendLayout();
            tabTongQuan.SuspendLayout();
            panelTongQuan.SuspendLayout();
            tabDichVu.SuspendLayout();
            tabKhachHang.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTopKhachHang).BeginInit();
            tabKhac.SuspendLayout();
            panelKhac.SuspendLayout();
            panelSoSanh.SuspendLayout();
            cardChenhLech.SuspendLayout();
            cardKyTruoc.SuspendLayout();
            cardKyHienTai.SuspendLayout();
            panelSoSanhHeader.SuspendLayout();
            panelThangHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chartDoanhThu7Ngay).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chartDoanhThuThang).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chartKhungGio).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chartPhuongThuc).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chartTopDichVu).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chartLoaiDichVu).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chartLoaiBan).BeginInit();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(102, 126, 234);
            panelHeader.Controls.Add(btnHomNay);
            panelHeader.Controls.Add(btnXemBaoCao);
            panelHeader.Controls.Add(dtpDenNgay);
            panelHeader.Controls.Add(lblDen);
            panelHeader.Controls.Add(dtpTuNgay);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Padding = new Padding(20, 23, 20, 23);
            panelHeader.Size = new Size(1300, 100);
            panelHeader.TabIndex = 0;
            // 
            // btnHomNay
            // 
            btnHomNay.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnHomNay.BackColor = Color.FromArgb(0, 192, 0);
            btnHomNay.FlatAppearance.BorderSize = 0;
            btnHomNay.FlatStyle = FlatStyle.Flat;
            btnHomNay.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnHomNay.ForeColor = Color.White;
            btnHomNay.Location = new Point(1180, 28);
            btnHomNay.Name = "btnHomNay";
            btnHomNay.Size = new Size(100, 40);
            btnHomNay.TabIndex = 5;
            btnHomNay.Text = "🔄 Hôm nay";
            btnHomNay.UseVisualStyleBackColor = false;
            btnHomNay.Click += btnHomNay_Click;
            // 
            // btnXemBaoCao
            // 
            btnXemBaoCao.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnXemBaoCao.BackColor = Color.White;
            btnXemBaoCao.FlatAppearance.BorderSize = 0;
            btnXemBaoCao.FlatStyle = FlatStyle.Flat;
            btnXemBaoCao.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnXemBaoCao.ForeColor = Color.FromArgb(102, 126, 234);
            btnXemBaoCao.Location = new Point(1050, 28);
            btnXemBaoCao.Name = "btnXemBaoCao";
            btnXemBaoCao.Size = new Size(120, 40);
            btnXemBaoCao.TabIndex = 4;
            btnXemBaoCao.Text = "🔍 Xem báo cáo";
            btnXemBaoCao.UseVisualStyleBackColor = false;
            btnXemBaoCao.Click += btnXemBaoCao_Click;
            // 
            // dtpDenNgay
            // 
            dtpDenNgay.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dtpDenNgay.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpDenNgay.Font = new Font("Segoe UI", 10F);
            dtpDenNgay.Format = DateTimePickerFormat.Custom;
            dtpDenNgay.Location = new Point(870, 31);
            dtpDenNgay.Name = "dtpDenNgay";
            dtpDenNgay.Size = new Size(170, 34);
            dtpDenNgay.TabIndex = 3;
            // 
            // lblDen
            // 
            lblDen.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblDen.AutoSize = true;
            lblDen.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDen.ForeColor = Color.White;
            lblDen.Location = new Point(830, 35);
            lblDen.Name = "lblDen";
            lblDen.Size = new Size(36, 23);
            lblDen.TabIndex = 2;
            lblDen.Text = "đến";
            // 
            // dtpTuNgay
            // 
            dtpTuNgay.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dtpTuNgay.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpTuNgay.Font = new Font("Segoe UI", 10F);
            dtpTuNgay.Format = DateTimePickerFormat.Custom;
            dtpTuNgay.Location = new Point(650, 31);
            dtpTuNgay.Name = "dtpTuNgay";
            dtpTuNgay.Size = new Size(170, 34);
            dtpTuNgay.TabIndex = 1;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 28);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(295, 41);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "📊 Thống kê && Báo cáo";
            // 
            // panelCards
            // 
            panelCards.Controls.Add(cardTrungBinh);
            panelCards.Controls.Add(cardKhachHang);
            panelCards.Controls.Add(cardHoaDon);
            panelCards.Controls.Add(cardDoanhThu);
            panelCards.Dock = DockStyle.Top;
            panelCards.Location = new Point(0, 100);
            panelCards.Name = "panelCards";
            panelCards.Padding = new Padding(20, 23, 20, 12);
            panelCards.Size = new Size(1300, 160);
            panelCards.TabIndex = 1;
            // 
            // cardDoanhThu
            // 
            cardDoanhThu.BackColor = Color.White;
            cardDoanhThu.BorderStyle = BorderStyle.FixedSingle;
            cardDoanhThu.Controls.Add(lblDoanhThuValue);
            cardDoanhThu.Controls.Add(lblDoanhThuTitle);
            cardDoanhThu.Controls.Add(lblDoanhThuIcon);
            cardDoanhThu.Location = new Point(20, 23);
            cardDoanhThu.Name = "cardDoanhThu";
            cardDoanhThu.Size = new Size(300, 120);
            cardDoanhThu.TabIndex = 0;
            // 
            // lblDoanhThuValue
            // 
            lblDoanhThuValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblDoanhThuValue.ForeColor = Color.FromArgb(44, 62, 80);
            lblDoanhThuValue.Location = new Point(80, 50);
            lblDoanhThuValue.Name = "lblDoanhThuValue";
            lblDoanhThuValue.Size = new Size(210, 50);
            lblDoanhThuValue.TabIndex = 2;
            lblDoanhThuValue.Text = "0 đ";
            // 
            // lblDoanhThuTitle
            // 
            lblDoanhThuTitle.AutoSize = true;
            lblDoanhThuTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDoanhThuTitle.ForeColor = Color.Gray;
            lblDoanhThuTitle.Location = new Point(80, 20);
            lblDoanhThuTitle.Name = "lblDoanhThuTitle";
            lblDoanhThuTitle.Size = new Size(119, 20);
            lblDoanhThuTitle.TabIndex = 1;
            lblDoanhThuTitle.Text = "Tổng doanh thu";
            // 
            // lblDoanhThuIcon
            // 
            lblDoanhThuIcon.Font = new Font("Segoe UI", 28F);
            lblDoanhThuIcon.Location = new Point(10, 20);
            lblDoanhThuIcon.Name = "lblDoanhThuIcon";
            lblDoanhThuIcon.Size = new Size(65, 70);
            lblDoanhThuIcon.TabIndex = 0;
            lblDoanhThuIcon.Text = "💰";
            lblDoanhThuIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cardHoaDon
            // 
            cardHoaDon.BackColor = Color.White;
            cardHoaDon.BorderStyle = BorderStyle.FixedSingle;
            cardHoaDon.Controls.Add(lblHoaDonSubtitle);
            cardHoaDon.Controls.Add(lblHoaDonValue);
            cardHoaDon.Controls.Add(lblHoaDonTitle);
            cardHoaDon.Controls.Add(lblHoaDonIcon);
            cardHoaDon.Location = new Point(340, 23);
            cardHoaDon.Name = "cardHoaDon";
            cardHoaDon.Size = new Size(300, 120);
            cardHoaDon.TabIndex = 1;
            // 
            // lblHoaDonSubtitle
            // 
            lblHoaDonSubtitle.AutoSize = true;
            lblHoaDonSubtitle.Font = new Font("Segoe UI", 8F);
            lblHoaDonSubtitle.ForeColor = Color.Gray;
            lblHoaDonSubtitle.Location = new Point(80, 85);
            lblHoaDonSubtitle.Name = "lblHoaDonSubtitle";
            lblHoaDonSubtitle.Size = new Size(134, 19);
            lblHoaDonSubtitle.TabIndex = 3;
            lblHoaDonSubtitle.Text = "hóa đơn đã thanh toán";
            // 
            // lblHoaDonValue
            // 
            lblHoaDonValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblHoaDonValue.ForeColor = Color.FromArgb(44, 62, 80);
            lblHoaDonValue.Location = new Point(80, 45);
            lblHoaDonValue.Name = "lblHoaDonValue";
            lblHoaDonValue.Size = new Size(210, 40);
            lblHoaDonValue.TabIndex = 2;
            lblHoaDonValue.Text = "0";
            // 
            // lblHoaDonTitle
            // 
            lblHoaDonTitle.AutoSize = true;
            lblHoaDonTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblHoaDonTitle.ForeColor = Color.Gray;
            lblHoaDonTitle.Location = new Point(80, 20);
            lblHoaDonTitle.Name = "lblHoaDonTitle";
            lblHoaDonTitle.Size = new Size(86, 20);
            lblHoaDonTitle.TabIndex = 1;
            lblHoaDonTitle.Text = "Số hóa đơn";
            // 
            // lblHoaDonIcon
            // 
            lblHoaDonIcon.Font = new Font("Segoe UI", 28F);
            lblHoaDonIcon.Location = new Point(10, 20);
            lblHoaDonIcon.Name = "lblHoaDonIcon";
            lblHoaDonIcon.Size = new Size(65, 70);
            lblHoaDonIcon.TabIndex = 0;
            lblHoaDonIcon.Text = "📝";
            lblHoaDonIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cardKhachHang
            // 
            cardKhachHang.BackColor = Color.White;
            cardKhachHang.BorderStyle = BorderStyle.FixedSingle;
            cardKhachHang.Controls.Add(lblKhachHangSubtitle);
            cardKhachHang.Controls.Add(lblKhachHangValue);
            cardKhachHang.Controls.Add(lblKhachHangTitle);
            cardKhachHang.Controls.Add(lblKhachHangIcon);
            cardKhachHang.Location = new Point(660, 23);
            cardKhachHang.Name = "cardKhachHang";
            cardKhachHang.Size = new Size(300, 120);
            cardKhachHang.TabIndex = 2;
            // 
            // lblKhachHangSubtitle
            // 
            lblKhachHangSubtitle.AutoSize = true;
            lblKhachHangSubtitle.Font = new Font("Segoe UI", 8F);
            lblKhachHangSubtitle.ForeColor = Color.Gray;
            lblKhachHangSubtitle.Location = new Point(80, 85);
            lblKhachHangSubtitle.Name = "lblKhachHangSubtitle";
            lblKhachHangSubtitle.Size = new Size(74, 19);
            lblKhachHangSubtitle.TabIndex = 3;
            lblKhachHangSubtitle.Text = "khách hàng";
            // 
            // lblKhachHangValue
            // 
            lblKhachHangValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblKhachHangValue.ForeColor = Color.FromArgb(44, 62, 80);
            lblKhachHangValue.Location = new Point(80, 45);
            lblKhachHangValue.Name = "lblKhachHangValue";
            lblKhachHangValue.Size = new Size(210, 40);
            lblKhachHangValue.TabIndex = 2;
            lblKhachHangValue.Text = "0";
            // 
            // lblKhachHangTitle
            // 
            lblKhachHangTitle.AutoSize = true;
            lblKhachHangTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblKhachHangTitle.ForeColor = Color.Gray;
            lblKhachHangTitle.Location = new Point(80, 20);
            lblKhachHangTitle.Name = "lblKhachHangTitle";
            lblKhachHangTitle.Size = new Size(91, 20);
            lblKhachHangTitle.TabIndex = 1;
            lblKhachHangTitle.Text = "Khách hàng";
            // 
            // lblKhachHangIcon
            // 
            lblKhachHangIcon.Font = new Font("Segoe UI", 28F);
            lblKhachHangIcon.Location = new Point(10, 20);
            lblKhachHangIcon.Name = "lblKhachHangIcon";
            lblKhachHangIcon.Size = new Size(65, 70);
            lblKhachHangIcon.TabIndex = 0;
            lblKhachHangIcon.Text = "👥";
            lblKhachHangIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cardTrungBinh
            // 
            cardTrungBinh.BackColor = Color.White;
            cardTrungBinh.BorderStyle = BorderStyle.FixedSingle;
            cardTrungBinh.Controls.Add(lblTrungBinhSubtitle);
            cardTrungBinh.Controls.Add(lblTrungBinhValue);
            cardTrungBinh.Controls.Add(lblTrungBinhTitle);
            cardTrungBinh.Controls.Add(lblTrungBinhIcon);
            cardTrungBinh.Location = new Point(980, 23);
            cardTrungBinh.Name = "cardTrungBinh";
            cardTrungBinh.Size = new Size(300, 120);
            cardTrungBinh.TabIndex = 3;
            // 
            // lblTrungBinhSubtitle
            // 
            lblTrungBinhSubtitle.AutoSize = true;
            lblTrungBinhSubtitle.Font = new Font("Segoe UI", 8F);
            lblTrungBinhSubtitle.ForeColor = Color.Gray;
            lblTrungBinhSubtitle.Location = new Point(80, 85);
            lblTrungBinhSubtitle.Name = "lblTrungBinhSubtitle";
            lblTrungBinhSubtitle.Size = new Size(126, 19);
            lblTrungBinhSubtitle.TabIndex = 3;
            lblTrungBinhSubtitle.Text = "doanh thu trung bình";
            // 
            // lblTrungBinhValue
            // 
            lblTrungBinhValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTrungBinhValue.ForeColor = Color.FromArgb(44, 62, 80);
            lblTrungBinhValue.Location = new Point(80, 45);
            lblTrungBinhValue.Name = "lblTrungBinhValue";
            lblTrungBinhValue.Size = new Size(210, 40);
            lblTrungBinhValue.TabIndex = 2;
            lblTrungBinhValue.Text = "0 đ";
            // 
            // lblTrungBinhTitle
            // 
            lblTrungBinhTitle.AutoSize = true;
            lblTrungBinhTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTrungBinhTitle.ForeColor = Color.Gray;
            lblTrungBinhTitle.Location = new Point(80, 20);
            lblTrungBinhTitle.Name = "lblTrungBinhTitle";
            lblTrungBinhTitle.Size = new Size(113, 20);
            lblTrungBinhTitle.TabIndex = 1;
            lblTrungBinhTitle.Text = "Trung bình/HĐ";
            // 
            // lblTrungBinhIcon
            // 
            lblTrungBinhIcon.Font = new Font("Segoe UI", 28F);
            lblTrungBinhIcon.Location = new Point(10, 20);
            lblTrungBinhIcon.Name = "lblTrungBinhIcon";
            lblTrungBinhIcon.Size = new Size(65, 70);
            lblTrungBinhIcon.TabIndex = 0;
            lblTrungBinhIcon.Text = "📈";
            lblTrungBinhIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabTongQuan);
            tabControl.Controls.Add(tabDichVu);
            tabControl.Controls.Add(tabKhachHang);
            tabControl.Controls.Add(tabKhac);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            tabControl.Location = new Point(0, 260);
            tabControl.Name = "tabControl";
            tabControl.Padding = new Point(10, 5);
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1300, 640);
            tabControl.TabIndex = 2;
            // 
            // tabTongQuan
            // 
            tabTongQuan.Controls.Add(panelTongQuan);
            tabTongQuan.Location = new Point(4, 37);
            tabTongQuan.Name = "tabTongQuan";
            tabTongQuan.Padding = new Padding(6, 7, 6, 7);
            tabTongQuan.Size = new Size(1292, 599);
            tabTongQuan.TabIndex = 0;
            tabTongQuan.Text = "📊 Tổng quan";
            tabTongQuan.UseVisualStyleBackColor = true;
            // 
            // panelTongQuan
            // 
            panelTongQuan.AutoScroll = true;
            panelTongQuan.Controls.Add(panelThangHeader);
            panelTongQuan.Controls.Add(chartDoanhThuThang);
            panelTongQuan.Controls.Add(chartKhungGio);
            panelTongQuan.Controls.Add(chartPhuongThuc);
            panelTongQuan.Controls.Add(panelDoanhThu7Ngay);
            panelTongQuan.Dock = DockStyle.Fill;
            panelTongQuan.Location = new Point(6, 7);
            panelTongQuan.Name = "panelTongQuan";
            panelTongQuan.Padding = new Padding(10, 12, 10, 12);
            panelTongQuan.Size = new Size(1280, 585);
            panelTongQuan.TabIndex = 0;
            // 
            // panelDoanhThu7Ngay
            // 
            panelDoanhThu7Ngay.BackColor = Color.White;
            panelDoanhThu7Ngay.BorderStyle = BorderStyle.FixedSingle;
            panelDoanhThu7Ngay.Controls.Add(chartDoanhThu7Ngay);
            panelDoanhThu7Ngay.Location = new Point(10, 12);
            panelDoanhThu7Ngay.Name = "panelDoanhThu7Ngay";
            panelDoanhThu7Ngay.Padding = new Padding(15, 18, 15, 18);
            panelDoanhThu7Ngay.Size = new Size(1240, 350);
            panelDoanhThu7Ngay.TabIndex = 0;
            // 
            // chartDoanhThu7Ngay
            // 
            chartDoanhThu7Ngay.BackColor = Color.White;
            chartDoanhThu7Ngay.Dock = DockStyle.Fill;
            chartDoanhThu7Ngay.Location = new Point(15, 18);
            chartDoanhThu7Ngay.Name = "chartDoanhThu7Ngay";
            chartDoanhThu7Ngay.Size = new Size(1208, 312);
            chartDoanhThu7Ngay.TabIndex = 0;
            // 
            // panelThangHeader
            // 
            panelThangHeader.BackColor = Color.White;
            panelThangHeader.BorderStyle = BorderStyle.FixedSingle;
            panelThangHeader.Controls.Add(cboNam);
            panelThangHeader.Controls.Add(lblDoanhThuThang);
            panelThangHeader.Location = new Point(10, 370);
            panelThangHeader.Name = "panelThangHeader";
            panelThangHeader.Padding = new Padding(15, 10, 15, 10);
            panelThangHeader.Size = new Size(620, 50);
            panelThangHeader.TabIndex = 4;
            // 
            // cboNam
            // 
            cboNam.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cboNam.DropDownStyle = ComboBoxStyle.DropDownList;
            cboNam.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            cboNam.FormattingEnabled = true;
            cboNam.Location = new Point(510, 8);
            cboNam.Name = "cboNam";
            cboNam.Size = new Size(90, 31);
            cboNam.TabIndex = 1;
            cboNam.SelectedIndexChanged += cboNam_SelectedIndexChanged;
            // 
            // lblDoanhThuThang
            // 
            lblDoanhThuThang.AutoSize = true;
            lblDoanhThuThang.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblDoanhThuThang.Location = new Point(15, 10);
            lblDoanhThuThang.Name = "lblDoanhThuThang";
            lblDoanhThuThang.Size = new Size(235, 28);
            lblDoanhThuThang.TabIndex = 0;
            lblDoanhThuThang.Text = "📅 Doanh thu theo tháng";
            // 
            // chartDoanhThuThang
            // 
            chartDoanhThuThang.BackColor = Color.White;
            chartDoanhThuThang.BorderlineColor = Color.LightGray;
            chartDoanhThuThang.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartDoanhThuThang.Location = new Point(10, 425);
            chartDoanhThuThang.Name = "chartDoanhThuThang";
            chartDoanhThuThang.Size = new Size(620, 300);
            chartDoanhThuThang.TabIndex = 1;
            // 
            // chartKhungGio
            // 
            chartKhungGio.BackColor = Color.White;
            chartKhungGio.BorderlineColor = Color.LightGray;
            chartKhungGio.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartKhungGio.Location = new Point(640, 370);
            chartKhungGio.Name = "chartKhungGio";
            chartKhungGio.Size = new Size(300, 355);
            chartKhungGio.TabIndex = 2;
            // 
            // chartPhuongThuc
            // 
            chartPhuongThuc.BackColor = Color.White;
            chartPhuongThuc.BorderlineColor = Color.LightGray;
            chartPhuongThuc.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartPhuongThuc.Location = new Point(950, 370);
            chartPhuongThuc.Name = "chartPhuongThuc";
            chartPhuongThuc.Size = new Size(300, 355);
            chartPhuongThuc.TabIndex = 3;
            // 
            // tabDichVu
            // 
            tabDichVu.Controls.Add(panelDichVu);
            tabDichVu.Location = new Point(4, 37);
            tabDichVu.Name = "tabDichVu";
            tabDichVu.Padding = new Padding(6, 7, 6, 7);
            tabDichVu.Size = new Size(1292, 599);
            tabDichVu.TabIndex = 1;
            tabDichVu.Text = "🍽️ Dịch vụ";
            tabDichVu.UseVisualStyleBackColor = true;
            // 
            // panelDichVu
            // 
            panelDichVu.AutoScroll = true;
            panelDichVu.Controls.Add(chartTopDichVu);
            panelDichVu.Controls.Add(chartLoaiDichVu);
            panelDichVu.Controls.Add(chartLoaiBan);
            panelDichVu.Dock = DockStyle.Fill;
            panelDichVu.Location = new Point(6, 7);
            panelDichVu.Name = "panelDichVu";
            panelDichVu.Padding = new Padding(10, 12, 10, 12);
            panelDichVu.Size = new Size(1280, 585);
            panelDichVu.TabIndex = 0;
            // 
            // chartTopDichVu
            // 
            chartTopDichVu.BackColor = Color.White;
            chartTopDichVu.BorderlineColor = Color.LightGray;
            chartTopDichVu.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartTopDichVu.Location = new Point(10, 12);
            chartTopDichVu.Name = "chartTopDichVu";
            chartTopDichVu.Size = new Size(620, 550);
            chartTopDichVu.TabIndex = 0;
            // 
            // chartLoaiDichVu
            // 
            chartLoaiDichVu.BackColor = Color.White;
            chartLoaiDichVu.BorderlineColor = Color.LightGray;
            chartLoaiDichVu.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartLoaiDichVu.Location = new Point(640, 12);
            chartLoaiDichVu.Name = "chartLoaiDichVu";
            chartLoaiDichVu.Size = new Size(300, 550);
            chartLoaiDichVu.TabIndex = 1;
            // 
            // chartLoaiBan
            // 
            chartLoaiBan.BackColor = Color.White;
            chartLoaiBan.BorderlineColor = Color.LightGray;
            chartLoaiBan.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartLoaiBan.Location = new Point(950, 12);
            chartLoaiBan.Name = "chartLoaiBan";
            chartLoaiBan.Size = new Size(300, 550);
            chartLoaiBan.TabIndex = 2;
            // 
            // tabKhachHang
            // 
            tabKhachHang.Controls.Add(dgvTopKhachHang);
            tabKhachHang.Location = new Point(4, 37);
            tabKhachHang.Name = "tabKhachHang";
            tabKhachHang.Size = new Size(1292, 599);
            tabKhachHang.TabIndex = 2;
            tabKhachHang.Text = "👥 Khách hàng";
            tabKhachHang.UseVisualStyleBackColor = true;
            // 
            // dgvTopKhachHang
            // 
            dgvTopKhachHang.AllowUserToAddRows = false;
            dgvTopKhachHang.AllowUserToDeleteRows = false;
            dgvTopKhachHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTopKhachHang.BackgroundColor = Color.White;
            dgvTopKhachHang.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTopKhachHang.Columns.AddRange(new DataGridViewColumn[] { colSTT, colTenKH, colSDT, colTongChiTieu, colSoLanDen });
            dgvTopKhachHang.Dock = DockStyle.Fill;
            dgvTopKhachHang.Location = new Point(0, 0);
            dgvTopKhachHang.Name = "dgvTopKhachHang";
            dgvTopKhachHang.ReadOnly = true;
            dgvTopKhachHang.RowHeadersVisible = false;
            dgvTopKhachHang.RowHeadersWidth = 62;
            dgvTopKhachHang.Size = new Size(1292, 599);
            dgvTopKhachHang.TabIndex = 0;
            // 
            // colSTT
            // 
            colSTT.FillWeight = 50F;
            colSTT.HeaderText = "#";
            colSTT.MinimumWidth = 8;
            colSTT.Name = "colSTT";
            colSTT.ReadOnly = true;
            // 
            // colTenKH
            // 
            colTenKH.HeaderText = "Tên khách hàng";
            colTenKH.MinimumWidth = 8;
            colTenKH.Name = "colTenKH";
            colTenKH.ReadOnly = true;
            // 
            // colSDT
            // 
            colSDT.HeaderText = "Số điện thoại";
            colSDT.MinimumWidth = 8;
            colSDT.Name = "colSDT";
            colSDT.ReadOnly = true;
            // 
            // colTongChiTieu
            // 
            colTongChiTieu.HeaderText = "Tổng chi tiêu";
            colTongChiTieu.MinimumWidth = 8;
            colTongChiTieu.Name = "colTongChiTieu";
            colTongChiTieu.ReadOnly = true;
            // 
            // colSoLanDen
            // 
            colSoLanDen.FillWeight = 80F;
            colSoLanDen.HeaderText = "Số lần đến";
            colSoLanDen.MinimumWidth = 8;
            colSoLanDen.Name = "colSoLanDen";
            colSoLanDen.ReadOnly = true;
            // 
            // tabKhac
            // 
            tabKhac.Controls.Add(panelKhac);
            tabKhac.Location = new Point(4, 37);
            tabKhac.Name = "tabKhac";
            tabKhac.Size = new Size(1292, 599);
            tabKhac.TabIndex = 3;
            tabKhac.Text = "📋 Khác";
            tabKhac.UseVisualStyleBackColor = true;
            // 
            // panelKhac
            // 
            panelKhac.Controls.Add(panelSoSanh);
            panelKhac.Dock = DockStyle.Fill;
            panelKhac.Location = new Point(0, 0);
            panelKhac.Name = "panelKhac";
            panelKhac.Padding = new Padding(10, 12, 10, 12);
            panelKhac.Size = new Size(1292, 599);
            panelKhac.TabIndex = 0;
            // 
            // panelSoSanh
            // 
            panelSoSanh.BackColor = Color.White;
            panelSoSanh.BorderStyle = BorderStyle.FixedSingle;
            panelSoSanh.Controls.Add(cardChenhLech);
            panelSoSanh.Controls.Add(cardKyTruoc);
            panelSoSanh.Controls.Add(cardKyHienTai);
            panelSoSanh.Controls.Add(panelSoSanhHeader);
            panelSoSanh.Location = new Point(10, 12);
            panelSoSanh.Name = "panelSoSanh";
            panelSoSanh.Padding = new Padding(20, 23, 20, 23);
            panelSoSanh.Size = new Size(1260, 300);
            panelSoSanh.TabIndex = 0;
            // 
            // panelSoSanhHeader
            // 
            panelSoSanhHeader.Controls.Add(btnThang);
            panelSoSanhHeader.Controls.Add(btnTuan);
            panelSoSanhHeader.Controls.Add(btnNgay);
            panelSoSanhHeader.Controls.Add(lblSoSanhTitle);
            panelSoSanhHeader.Dock = DockStyle.Top;
            panelSoSanhHeader.Location = new Point(20, 23);
            panelSoSanhHeader.Name = "panelSoSanhHeader";
            panelSoSanhHeader.Size = new Size(1218, 60);
            panelSoSanhHeader.TabIndex = 0;
            // 
            // lblSoSanhTitle
            // 
            lblSoSanhTitle.AutoSize = true;
            lblSoSanhTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblSoSanhTitle.Location = new Point(0, 15);
            lblSoSanhTitle.Name = "lblSoSanhTitle";
            lblSoSanhTitle.Size = new Size(250, 32);
            lblSoSanhTitle.TabIndex = 0;
            lblSoSanhTitle.Text = "📊 So sánh doanh thu";
            // 
            // btnNgay
            // 
            btnNgay.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNgay.BackColor = Color.FromArgb(102, 126, 234);
            btnNgay.FlatAppearance.BorderSize = 0;
            btnNgay.FlatStyle = FlatStyle.Flat;
            btnNgay.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnNgay.ForeColor = Color.White;
            btnNgay.Location = new Point(918, 10);
            btnNgay.Name = "btnNgay";
            btnNgay.Size = new Size(95, 40);
            btnNgay.TabIndex = 1;
            btnNgay.Text = "Hôm nay";
            btnNgay.UseVisualStyleBackColor = false;
            btnNgay.Click += btnNgay_Click;
            // 
            // btnTuan
            // 
            btnTuan.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnTuan.BackColor = Color.White;
            btnTuan.FlatAppearance.BorderColor = Color.LightGray;
            btnTuan.FlatStyle = FlatStyle.Flat;
            btnTuan.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTuan.Location = new Point(1018, 10);
            btnTuan.Name = "btnTuan";
            btnTuan.Size = new Size(95, 40);
            btnTuan.TabIndex = 2;
            btnTuan.Text = "Tuần này";
            btnTuan.UseVisualStyleBackColor = false;
            btnTuan.Click += btnTuan_Click;
            // 
            // btnThang
            // 
            btnThang.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnThang.BackColor = Color.White;
            btnThang.FlatAppearance.BorderColor = Color.LightGray;
            btnThang.FlatStyle = FlatStyle.Flat;
            btnThang.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnThang.Location = new Point(1118, 10);
            btnThang.Name = "btnThang";
            btnThang.Size = new Size(95, 40);
            btnThang.TabIndex = 3;
            btnThang.Text = "Tháng này";
            btnThang.UseVisualStyleBackColor = false;
            btnThang.Click += btnThang_Click;
            // 
            // cardKyHienTai
            // 
            cardKyHienTai.BackColor = Color.FromArgb(248, 249, 250);
            cardKyHienTai.BorderStyle = BorderStyle.FixedSingle;
            cardKyHienTai.Controls.Add(lblKyHienTaiPercent);
            cardKyHienTai.Controls.Add(lblKyHienTaiValue);
            cardKyHienTai.Controls.Add(lblKyHienTaiTitle);
            cardKyHienTai.Location = new Point(20, 100);
            cardKyHienTai.Name = "cardKyHienTai";
            cardKyHienTai.Padding = new Padding(20, 23, 20, 23);
            cardKyHienTai.Size = new Size(380, 160);
            cardKyHienTai.TabIndex = 1;
            // 
            // lblKyHienTaiTitle
            // 
            lblKyHienTaiTitle.AutoSize = true;
            lblKyHienTaiTitle.Font = new Font("Segoe UI", 10F);
            lblKyHienTaiTitle.ForeColor = Color.Gray;
            lblKyHienTaiTitle.Location = new Point(20, 23);
            lblKyHienTaiTitle.Name = "lblKyHienTaiTitle";
            lblKyHienTaiTitle.Size = new Size(75, 23);
            lblKyHienTaiTitle.TabIndex = 0;
            lblKyHienTaiTitle.Text = "Hôm nay";
            // 
            // lblKyHienTaiValue
            // 
            lblKyHienTaiValue.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblKyHienTaiValue.ForeColor = Color.FromArgb(44, 62, 80);
            lblKyHienTaiValue.Location = new Point(20, 55);
            lblKyHienTaiValue.Name = "lblKyHienTaiValue";
            lblKyHienTaiValue.Size = new Size(340, 50);
            lblKyHienTaiValue.TabIndex = 1;
            lblKyHienTaiValue.Text = "0 đ";
            // 
            // lblKyHienTaiPercent
            // 
            lblKyHienTaiPercent.AutoSize = true;
            lblKyHienTaiPercent.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblKyHienTaiPercent.ForeColor = Color.FromArgb(40, 167, 69);
            lblKyHienTaiPercent.Location = new Point(20, 115);
            lblKyHienTaiPercent.Name = "lblKyHienTaiPercent";
            lblKyHienTaiPercent.Size = new Size(0, 20);
            lblKyHienTaiPercent.TabIndex = 2;
            // 
            // cardKyTruoc
            // 
            cardKyTruoc.BackColor = Color.FromArgb(248, 249, 250);
            cardKyTruoc.BorderStyle = BorderStyle.FixedSingle;
            cardKyTruoc.Controls.Add(lblKyTruocValue);
            cardKyTruoc.Controls.Add(lblKyTruocTitle);
            cardKyTruoc.Location = new Point(420, 100);
            cardKyTruoc.Name = "cardKyTruoc";
            cardKyTruoc.Padding = new Padding(20, 23, 20, 23);
            cardKyTruoc.Size = new Size(380, 160);
            cardKyTruoc.TabIndex = 2;
            // 
            // lblKyTruocTitle
            // 
            lblKyTruocTitle.AutoSize = true;
            lblKyTruocTitle.Font = new Font("Segoe UI", 10F);
            lblKyTruocTitle.ForeColor = Color.Gray;
            lblKyTruocTitle.Location = new Point(20, 23);
            lblKyTruocTitle.Name = "lblKyTruocTitle";
            lblKyTruocTitle.Size = new Size(77, 23);
            lblKyTruocTitle.TabIndex = 0;
            lblKyTruocTitle.Text = "Hôm qua";
            // 
            // lblKyTruocValue
            // 
            lblKyTruocValue.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblKyTruocValue.ForeColor = Color.FromArgb(44, 62, 80);
            lblKyTruocValue.Location = new Point(20, 55);
            lblKyTruocValue.Name = "lblKyTruocValue";
            lblKyTruocValue.Size = new Size(340, 50);
            lblKyTruocValue.TabIndex = 1;
            lblKyTruocValue.Text = "0 đ";
            // 
            // cardChenhLech
            // 
            cardChenhLech.BackColor = Color.FromArgb(248, 249, 250);
            cardChenhLech.BorderStyle = BorderStyle.FixedSingle;
            cardChenhLech.Controls.Add(lblChenhLechPercent);
            cardChenhLech.Controls.Add(lblChenhLechValue);
            cardChenhLech.Controls.Add(lblChenhLechTitle);
            cardChenhLech.Location = new Point(820, 100);
            cardChenhLech.Name = "cardChenhLech";
            cardChenhLech.Padding = new Padding(20, 23, 20, 23);
            cardChenhLech.Size = new Size(380, 160);
            cardChenhLech.TabIndex = 3;
            // 
            // lblChenhLechTitle
            // 
            lblChenhLechTitle.AutoSize = true;
            lblChenhLechTitle.Font = new Font("Segoe UI", 10F);
            lblChenhLechTitle.ForeColor = Color.Gray;
            lblChenhLechTitle.Location = new Point(20, 23);
            lblChenhLechTitle.Name = "lblChenhLechTitle";
            lblChenhLechTitle.Size = new Size(87, 23);
            lblChenhLechTitle.TabIndex = 0;
            lblChenhLechTitle.Text = "Chênh lệch";
            // 
            // lblChenhLechValue
            // 
            lblChenhLechValue.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblChenhLechValue.ForeColor = Color.FromArgb(44, 62, 80);
            lblChenhLechValue.Location = new Point(20, 55);
            lblChenhLechValue.Name = "lblChenhLechValue";
            lblChenhLechValue.Size = new Size(340, 50);
            lblChenhLechValue.TabIndex = 1;
            lblChenhLechValue.Text = "0 đ";
            // 
            // lblChenhLechPercent
            // 
            lblChenhLechPercent.AutoSize = true;
            lblChenhLechPercent.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblChenhLechPercent.ForeColor = Color.FromArgb(40, 167, 69);
            lblChenhLechPercent.Location = new Point(20, 115);
            lblChenhLechPercent.Name = "lblChenhLechPercent";
            lblChenhLechPercent.Size = new Size(0, 20);
            lblChenhLechPercent.TabIndex = 2;
            // 
            // ThongKeForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(240, 242, 245);
            ClientSize = new Size(1300, 900);
            Controls.Add(tabControl);
            Controls.Add(panelCards);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ThongKeForm";
            Text = "Thống kê & Báo cáo";
            Load += ThongKeForm_Load;
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelCards.ResumeLayout(false);
            cardTrungBinh.ResumeLayout(false);
            cardTrungBinh.PerformLayout();
            cardKhachHang.ResumeLayout(false);
            cardKhachHang.PerformLayout();
            cardHoaDon.ResumeLayout(false);
            cardHoaDon.PerformLayout();
            cardDoanhThu.ResumeLayout(false);
            cardDoanhThu.PerformLayout();
            tabControl.ResumeLayout(false);
            tabTongQuan.ResumeLayout(false);
            panelTongQuan.ResumeLayout(false);
            panelDoanhThu7Ngay.ResumeLayout(false);
            tabDichVu.ResumeLayout(false);
            panelDichVu.ResumeLayout(false);
            tabKhachHang.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTopKhachHang).EndInit();
            tabKhac.ResumeLayout(false);
            panelKhac.ResumeLayout(false);
            panelSoSanh.ResumeLayout(false);
            cardChenhLech.ResumeLayout(false);
            cardChenhLech.PerformLayout();
            cardKyTruoc.ResumeLayout(false);
            cardKyTruoc.PerformLayout();
            cardKyHienTai.ResumeLayout(false);
            cardKyHienTai.PerformLayout();
            panelSoSanhHeader.ResumeLayout(false);
            panelSoSanhHeader.PerformLayout();
            panelThangHeader.ResumeLayout(false);
            panelThangHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)chartDoanhThu7Ngay).EndInit();
            ((System.ComponentModel.ISupportInitialize)chartDoanhThuThang).EndInit();
            ((System.ComponentModel.ISupportInitialize)chartKhungGio).EndInit();
            ((System.ComponentModel.ISupportInitialize)chartPhuongThuc).EndInit();
            ((System.ComponentModel.ISupportInitialize)chartTopDichVu).EndInit();
            ((System.ComponentModel.ISupportInitialize)chartLoaiDichVu).EndInit();
            ((System.ComponentModel.ISupportInitialize)chartLoaiBan).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelHeader;
        private Label lblTitle;
        private DateTimePicker dtpTuNgay;
        private Label lblDen;
        private DateTimePicker dtpDenNgay;
        private Button btnXemBaoCao;
        private Button btnHomNay;
        private Panel panelCards;
        private Panel cardDoanhThu;
        private Label lblDoanhThuIcon;
        private Label lblDoanhThuTitle;
        private Label lblDoanhThuValue;
        private Panel cardHoaDon;
        private Label lblHoaDonSubtitle;
        private Label lblHoaDonValue;
        private Label lblHoaDonTitle;
        private Label lblHoaDonIcon;
        private Panel cardKhachHang;
        private Label lblKhachHangSubtitle;
        private Label lblKhachHangValue;
        private Label lblKhachHangTitle;
        private Label lblKhachHangIcon;
        private Panel cardTrungBinh;
        private Label lblTrungBinhSubtitle;
        private Label lblTrungBinhValue;
        private Label lblTrungBinhTitle;
        private Label lblTrungBinhIcon;
        private TabControl tabControl;
        private TabPage tabTongQuan;
        private Panel panelTongQuan;
        private Panel panelDoanhThu7Ngay;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartDoanhThu7Ngay;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartDoanhThuThang;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartKhungGio;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartPhuongThuc;
        private Panel panelThangHeader;
        private ComboBox cboNam;
        private Label lblDoanhThuThang;
        private TabPage tabDichVu;
        private Panel panelDichVu;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartTopDichVu;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartLoaiDichVu;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartLoaiBan;
        private TabPage tabKhachHang;
        private DataGridView dgvTopKhachHang;
        private DataGridViewTextBoxColumn colSTT;
        private DataGridViewTextBoxColumn colTenKH;
        private DataGridViewTextBoxColumn colSDT;
        private DataGridViewTextBoxColumn colTongChiTieu;
        private DataGridViewTextBoxColumn colSoLanDen;
        private TabPage tabKhac;
        private Panel panelKhac;
        private Panel panelSoSanh;
        private Panel panelSoSanhHeader;
        private Label lblSoSanhTitle;
        private Button btnNgay;
        private Button btnTuan;
        private Button btnThang;
        private Panel cardKyHienTai;
        private Label lblKyHienTaiTitle;
        private Label lblKyHienTaiValue;
        private Label lblKyHienTaiPercent;
        private Panel cardKyTruoc;
        private Label lblKyTruocTitle;
        private Label lblKyTruocValue;
        private Panel cardChenhLech;
        private Label lblChenhLechTitle;
        private Label lblChenhLechValue;
        private Label lblChenhLechPercent;
    }
}