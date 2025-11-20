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
            panelDoanhThu7Ngay = new Panel();
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
            panelHeader.Margin = new Padding(4, 3, 4, 3);
            panelHeader.Name = "panelHeader";
            panelHeader.Padding = new Padding(23);
            panelHeader.Size = new Size(1517, 92);
            panelHeader.TabIndex = 0;
            // 
            // btnHomNay
            // 
            btnHomNay.BackColor = Color.FromArgb(255, 255, 51);
            btnHomNay.FlatAppearance.BorderSize = 0;
            btnHomNay.FlatStyle = FlatStyle.Flat;
            btnHomNay.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnHomNay.ForeColor = Color.White;
            btnHomNay.Location = new Point(1343, 27);
            btnHomNay.Margin = new Padding(4, 3, 4, 3);
            btnHomNay.Name = "btnHomNay";
            btnHomNay.Size = new Size(117, 40);
            btnHomNay.TabIndex = 5;
            btnHomNay.Text = "🔄 Hôm nay";
            btnHomNay.UseVisualStyleBackColor = false;
            btnHomNay.Click += btnHomNay_Click;
            // 
            // btnXemBaoCao
            // 
            btnXemBaoCao.BackColor = Color.White;
            btnXemBaoCao.FlatAppearance.BorderSize = 0;
            btnXemBaoCao.FlatStyle = FlatStyle.Flat;
            btnXemBaoCao.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnXemBaoCao.ForeColor = Color.FromArgb(102, 126, 234);
            btnXemBaoCao.Location = new Point(1196, 27);
            btnXemBaoCao.Margin = new Padding(4, 3, 4, 3);
            btnXemBaoCao.Name = "btnXemBaoCao";
            btnXemBaoCao.Size = new Size(140, 40);
            btnXemBaoCao.TabIndex = 4;
            btnXemBaoCao.Text = "🔍 Xem báo cáo";
            btnXemBaoCao.UseVisualStyleBackColor = false;
            btnXemBaoCao.Click += btnXemBaoCao_Click;
            // 
            // dtpDenNgay
            // 
            dtpDenNgay.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpDenNgay.Font = new Font("Segoe UI", 10F);
            dtpDenNgay.Format = DateTimePickerFormat.Custom;
            dtpDenNgay.Location = new Point(974, 31);
            dtpDenNgay.Margin = new Padding(4, 3, 4, 3);
            dtpDenNgay.Name = "dtpDenNgay";
            dtpDenNgay.Size = new Size(209, 25);
            dtpDenNgay.TabIndex = 3;
            // 
            // lblDen
            // 
            lblDen.AutoSize = true;
            lblDen.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDen.ForeColor = Color.White;
            lblDen.Location = new Point(945, 58);
            lblDen.Margin = new Padding(4, 0, 4, 0);
            lblDen.Name = "lblDen";
            lblDen.Size = new Size(34, 19);
            lblDen.TabIndex = 2;
            lblDen.Text = "đến";
            // 
            // dtpTuNgay
            // 
            dtpTuNgay.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpTuNgay.Font = new Font("Segoe UI", 10F);
            dtpTuNgay.Format = DateTimePickerFormat.Custom;
            dtpTuNgay.Location = new Point(700, 31);
            dtpTuNgay.Margin = new Padding(4, 3, 4, 3);
            dtpTuNgay.Name = "dtpTuNgay";
            dtpTuNgay.Size = new Size(209, 25);
            dtpTuNgay.TabIndex = 1;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(47, 52);
            lblTitle.Margin = new Padding(4, 0, 4, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(265, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "📊 Thống kê & Báo cáo";
            // 
            // panelCards
            // 
            panelCards.Controls.Add(cardTrungBinh);
            panelCards.Controls.Add(cardKhachHang);
            panelCards.Controls.Add(cardHoaDon);
            panelCards.Controls.Add(cardDoanhThu);
            panelCards.Dock = DockStyle.Top;
            panelCards.Location = new Point(0, 92);
            panelCards.Margin = new Padding(4, 3, 4, 3);
            panelCards.Name = "panelCards";
            panelCards.Padding = new Padding(23, 23, 23, 12);
            panelCards.Size = new Size(1517, 162);
            panelCards.TabIndex = 1;
            // 
            // cardTrungBinh
            // 
            cardTrungBinh.BackColor = Color.White;
            cardTrungBinh.BorderStyle = BorderStyle.FixedSingle;
            cardTrungBinh.Controls.Add(lblTrungBinhSubtitle);
            cardTrungBinh.Controls.Add(lblTrungBinhValue);
            cardTrungBinh.Controls.Add(lblTrungBinhTitle);
            cardTrungBinh.Controls.Add(lblTrungBinhIcon);
            cardTrungBinh.Location = new Point(1038, 23);
            cardTrungBinh.Margin = new Padding(4, 3, 4, 3);
            cardTrungBinh.Name = "cardTrungBinh";
            cardTrungBinh.Size = new Size(326, 115);
            cardTrungBinh.TabIndex = 3;
            // 
            // lblTrungBinhSubtitle
            // 
            lblTrungBinhSubtitle.AutoSize = true;
            lblTrungBinhSubtitle.Font = new Font("Segoe UI", 8F);
            lblTrungBinhSubtitle.ForeColor = Color.Gray;
            lblTrungBinhSubtitle.Location = new Point(105, 81);
            lblTrungBinhSubtitle.Margin = new Padding(4, 0, 4, 0);
            lblTrungBinhSubtitle.Name = "lblTrungBinhSubtitle";
            lblTrungBinhSubtitle.Size = new Size(121, 13);
            lblTrungBinhSubtitle.TabIndex = 3;
            lblTrungBinhSubtitle.Text = "doanh thu trung bình";
            // 
            // lblTrungBinhValue
            // 
            lblTrungBinhValue.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTrungBinhValue.ForeColor = Color.FromArgb(44, 62, 80);
            lblTrungBinhValue.Location = new Point(105, 46);
            lblTrungBinhValue.Margin = new Padding(4, 0, 4, 0);
            lblTrungBinhValue.Name = "lblTrungBinhValue";
            lblTrungBinhValue.Size = new Size(210, 35);
            lblTrungBinhValue.TabIndex = 2;
            lblTrungBinhValue.Text = "0 đ";
            // 
            // lblTrungBinhTitle
            // 
            lblTrungBinhTitle.AutoSize = true;
            lblTrungBinhTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTrungBinhTitle.ForeColor = Color.Gray;
            lblTrungBinhTitle.Location = new Point(105, 23);
            lblTrungBinhTitle.Margin = new Padding(4, 0, 4, 0);
            lblTrungBinhTitle.Name = "lblTrungBinhTitle";
            lblTrungBinhTitle.Size = new Size(89, 15);
            lblTrungBinhTitle.TabIndex = 1;
            lblTrungBinhTitle.Text = "Trung bình/HĐ";
            // 
            // lblTrungBinhIcon
            // 
            lblTrungBinhIcon.Font = new Font("Segoe UI", 32F);
            lblTrungBinhIcon.Location = new Point(12, 23);
            lblTrungBinhIcon.Margin = new Padding(4, 0, 4, 0);
            lblTrungBinhIcon.Name = "lblTrungBinhIcon";
            lblTrungBinhIcon.Size = new Size(82, 69);
            lblTrungBinhIcon.TabIndex = 0;
            lblTrungBinhIcon.Text = "📈";
            lblTrungBinhIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cardKhachHang
            // 
            cardKhachHang.BackColor = Color.White;
            cardKhachHang.BorderStyle = BorderStyle.FixedSingle;
            cardKhachHang.Controls.Add(lblKhachHangSubtitle);
            cardKhachHang.Controls.Add(lblKhachHangValue);
            cardKhachHang.Controls.Add(lblKhachHangTitle);
            cardKhachHang.Controls.Add(lblKhachHangIcon);
            cardKhachHang.Location = new Point(700, 23);
            cardKhachHang.Margin = new Padding(4, 3, 4, 3);
            cardKhachHang.Name = "cardKhachHang";
            cardKhachHang.Size = new Size(326, 115);
            cardKhachHang.TabIndex = 2;
            // 
            // lblKhachHangSubtitle
            // 
            lblKhachHangSubtitle.AutoSize = true;
            lblKhachHangSubtitle.Font = new Font("Segoe UI", 8F);
            lblKhachHangSubtitle.ForeColor = Color.Gray;
            lblKhachHangSubtitle.Location = new Point(105, 81);
            lblKhachHangSubtitle.Margin = new Padding(4, 0, 4, 0);
            lblKhachHangSubtitle.Name = "lblKhachHangSubtitle";
            lblKhachHangSubtitle.Size = new Size(68, 13);
            lblKhachHangSubtitle.TabIndex = 3;
            lblKhachHangSubtitle.Text = "khách hàng";
            // 
            // lblKhachHangValue
            // 
            lblKhachHangValue.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblKhachHangValue.ForeColor = Color.FromArgb(44, 62, 80);
            lblKhachHangValue.Location = new Point(105, 46);
            lblKhachHangValue.Margin = new Padding(4, 0, 4, 0);
            lblKhachHangValue.Name = "lblKhachHangValue";
            lblKhachHangValue.Size = new Size(210, 35);
            lblKhachHangValue.TabIndex = 2;
            lblKhachHangValue.Text = "0";
            // 
            // lblKhachHangTitle
            // 
            lblKhachHangTitle.AutoSize = true;
            lblKhachHangTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblKhachHangTitle.ForeColor = Color.Gray;
            lblKhachHangTitle.Location = new Point(105, 23);
            lblKhachHangTitle.Margin = new Padding(4, 0, 4, 0);
            lblKhachHangTitle.Name = "lblKhachHangTitle";
            lblKhachHangTitle.Size = new Size(71, 15);
            lblKhachHangTitle.TabIndex = 1;
            lblKhachHangTitle.Text = "Khách hàng";
            // 
            // lblKhachHangIcon
            // 
            lblKhachHangIcon.Font = new Font("Segoe UI", 32F);
            lblKhachHangIcon.Location = new Point(12, 23);
            lblKhachHangIcon.Margin = new Padding(4, 0, 4, 0);
            lblKhachHangIcon.Name = "lblKhachHangIcon";
            lblKhachHangIcon.Size = new Size(82, 69);
            lblKhachHangIcon.TabIndex = 0;
            lblKhachHangIcon.Text = "👥";
            lblKhachHangIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cardHoaDon
            // 
            cardHoaDon.BackColor = Color.White;
            cardHoaDon.BorderStyle = BorderStyle.FixedSingle;
            cardHoaDon.Controls.Add(lblHoaDonSubtitle);
            cardHoaDon.Controls.Add(lblHoaDonValue);
            cardHoaDon.Controls.Add(lblHoaDonTitle);
            cardHoaDon.Controls.Add(lblHoaDonIcon);
            cardHoaDon.Location = new Point(362, 23);
            cardHoaDon.Margin = new Padding(4, 3, 4, 3);
            cardHoaDon.Name = "cardHoaDon";
            cardHoaDon.Size = new Size(326, 115);
            cardHoaDon.TabIndex = 1;
            // 
            // lblHoaDonSubtitle
            // 
            lblHoaDonSubtitle.AutoSize = true;
            lblHoaDonSubtitle.Font = new Font("Segoe UI", 8F);
            lblHoaDonSubtitle.ForeColor = Color.Gray;
            lblHoaDonSubtitle.Location = new Point(105, 81);
            lblHoaDonSubtitle.Margin = new Padding(4, 0, 4, 0);
            lblHoaDonSubtitle.Name = "lblHoaDonSubtitle";
            lblHoaDonSubtitle.Size = new Size(128, 13);
            lblHoaDonSubtitle.TabIndex = 3;
            lblHoaDonSubtitle.Text = "hóa đơn đã thanh toán";
            // 
            // lblHoaDonValue
            // 
            lblHoaDonValue.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblHoaDonValue.ForeColor = Color.FromArgb(44, 62, 80);
            lblHoaDonValue.Location = new Point(105, 46);
            lblHoaDonValue.Margin = new Padding(4, 0, 4, 0);
            lblHoaDonValue.Name = "lblHoaDonValue";
            lblHoaDonValue.Size = new Size(210, 35);
            lblHoaDonValue.TabIndex = 2;
            lblHoaDonValue.Text = "0";
            // 
            // lblHoaDonTitle
            // 
            lblHoaDonTitle.AutoSize = true;
            lblHoaDonTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblHoaDonTitle.ForeColor = Color.Gray;
            lblHoaDonTitle.Location = new Point(105, 23);
            lblHoaDonTitle.Margin = new Padding(4, 0, 4, 0);
            lblHoaDonTitle.Name = "lblHoaDonTitle";
            lblHoaDonTitle.Size = new Size(70, 15);
            lblHoaDonTitle.TabIndex = 1;
            lblHoaDonTitle.Text = "Số hóa đơn";
            // 
            // lblHoaDonIcon
            // 
            lblHoaDonIcon.Font = new Font("Segoe UI", 32F);
            lblHoaDonIcon.Location = new Point(12, 23);
            lblHoaDonIcon.Margin = new Padding(4, 0, 4, 0);
            lblHoaDonIcon.Name = "lblHoaDonIcon";
            lblHoaDonIcon.Size = new Size(82, 69);
            lblHoaDonIcon.TabIndex = 0;
            lblHoaDonIcon.Text = "📝";
            lblHoaDonIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cardDoanhThu
            // 
            cardDoanhThu.BackColor = Color.White;
            cardDoanhThu.BorderStyle = BorderStyle.FixedSingle;
            cardDoanhThu.Controls.Add(lblDoanhThuValue);
            cardDoanhThu.Controls.Add(lblDoanhThuTitle);
            cardDoanhThu.Controls.Add(lblDoanhThuIcon);
            cardDoanhThu.Location = new Point(23, 23);
            cardDoanhThu.Margin = new Padding(4, 3, 4, 3);
            cardDoanhThu.Name = "cardDoanhThu";
            cardDoanhThu.Size = new Size(326, 115);
            cardDoanhThu.TabIndex = 0;
            // 
            // lblDoanhThuValue
            // 
            lblDoanhThuValue.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblDoanhThuValue.ForeColor = Color.FromArgb(44, 62, 80);
            lblDoanhThuValue.Location = new Point(105, 46);
            lblDoanhThuValue.Margin = new Padding(4, 0, 4, 0);
            lblDoanhThuValue.Name = "lblDoanhThuValue";
            lblDoanhThuValue.Size = new Size(210, 46);
            lblDoanhThuValue.TabIndex = 2;
            lblDoanhThuValue.Text = "0 đ";
            // 
            // lblDoanhThuTitle
            // 
            lblDoanhThuTitle.AutoSize = true;
            lblDoanhThuTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDoanhThuTitle.ForeColor = Color.Gray;
            lblDoanhThuTitle.Location = new Point(105, 23);
            lblDoanhThuTitle.Margin = new Padding(4, 0, 4, 0);
            lblDoanhThuTitle.Name = "lblDoanhThuTitle";
            lblDoanhThuTitle.Size = new Size(94, 15);
            lblDoanhThuTitle.TabIndex = 1;
            lblDoanhThuTitle.Text = "Tổng doanh thu";
            // 
            // lblDoanhThuIcon
            // 
            lblDoanhThuIcon.Font = new Font("Segoe UI", 32F);
            lblDoanhThuIcon.Location = new Point(12, 23);
            lblDoanhThuIcon.Margin = new Padding(4, 0, 4, 0);
            lblDoanhThuIcon.Name = "lblDoanhThuIcon";
            lblDoanhThuIcon.Size = new Size(82, 69);
            lblDoanhThuIcon.TabIndex = 0;
            lblDoanhThuIcon.Text = "💰";
            lblDoanhThuIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabTongQuan);
            tabControl.Controls.Add(tabDichVu);
            tabControl.Controls.Add(tabKhachHang);
            tabControl.Controls.Add(tabKhac);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            tabControl.Location = new Point(0, 254);
            tabControl.Margin = new Padding(4, 3, 4, 3);
            tabControl.Name = "tabControl";
            tabControl.Padding = new Point(10, 5);
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1517, 669);
            tabControl.TabIndex = 2;
            // 
            // tabTongQuan
            // 
            tabTongQuan.Controls.Add(panelTongQuan);
            tabTongQuan.Location = new Point(4, 30);
            tabTongQuan.Margin = new Padding(4, 3, 4, 3);
            tabTongQuan.Name = "tabTongQuan";
            tabTongQuan.Padding = new Padding(4, 3, 4, 3);
            tabTongQuan.Size = new Size(1509, 635);
            tabTongQuan.TabIndex = 0;
            tabTongQuan.Text = "📊 Tổng quan";
            tabTongQuan.UseVisualStyleBackColor = true;
            // 
            // panelTongQuan
            // 
            panelTongQuan.AutoScroll = true;
            panelTongQuan.Controls.Add(panelDoanhThu7Ngay);
            panelTongQuan.Dock = DockStyle.Fill;
            panelTongQuan.Location = new Point(4, 3);
            panelTongQuan.Margin = new Padding(4, 3, 4, 3);
            panelTongQuan.Name = "panelTongQuan";
            panelTongQuan.Padding = new Padding(12);
            panelTongQuan.Size = new Size(1501, 629);
            panelTongQuan.TabIndex = 0;
            // 
            // tabDichVu
            // 
            tabDichVu.Controls.Add(panelDichVu);
            tabDichVu.Location = new Point(4, 30);
            tabDichVu.Margin = new Padding(4, 3, 4, 3);
            tabDichVu.Name = "tabDichVu";
            tabDichVu.Padding = new Padding(4, 3, 4, 3);
            tabDichVu.Size = new Size(1509, 635);
            tabDichVu.TabIndex = 1;
            tabDichVu.Text = "🍽️ Dịch vụ";
            tabDichVu.UseVisualStyleBackColor = true;
            // 
            // panelDichVu
            // 
            panelDichVu.AutoScroll = true;
            panelDichVu.Dock = DockStyle.Fill;
            panelDichVu.Location = new Point(4, 3);
            panelDichVu.Margin = new Padding(4, 3, 4, 3);
            panelDichVu.Name = "panelDichVu";
            panelDichVu.Padding = new Padding(12);
            panelDichVu.Size = new Size(1501, 629);
            panelDichVu.TabIndex = 0;
            // 
            // tabKhachHang
            // 
            tabKhachHang.Controls.Add(dgvTopKhachHang);
            tabKhachHang.Location = new Point(4, 30);
            tabKhachHang.Margin = new Padding(4, 3, 4, 3);
            tabKhachHang.Name = "tabKhachHang";
            tabKhachHang.Size = new Size(1509, 635);
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
            dgvTopKhachHang.Margin = new Padding(4, 3, 4, 3);
            dgvTopKhachHang.Name = "dgvTopKhachHang";
            dgvTopKhachHang.ReadOnly = true;
            dgvTopKhachHang.RowHeadersVisible = false;
            dgvTopKhachHang.Size = new Size(1509, 635);
            dgvTopKhachHang.TabIndex = 0;
            // 
            // colSTT
            // 
            colSTT.FillWeight = 50F;
            colSTT.HeaderText = "#";
            colSTT.Name = "colSTT";
            colSTT.ReadOnly = true;
            // 
            // colTenKH
            // 
            colTenKH.HeaderText = "Tên khách hàng";
            colTenKH.Name = "colTenKH";
            colTenKH.ReadOnly = true;
            // 
            // colSDT
            // 
            colSDT.HeaderText = "Số điện thoại";
            colSDT.Name = "colSDT";
            colSDT.ReadOnly = true;
            // 
            // colTongChiTieu
            // 
            colTongChiTieu.HeaderText = "Tổng chi tiêu";
            colTongChiTieu.Name = "colTongChiTieu";
            colTongChiTieu.ReadOnly = true;
            // 
            // colSoLanDen
            // 
            colSoLanDen.FillWeight = 80F;
            colSoLanDen.HeaderText = "Số lần đến";
            colSoLanDen.Name = "colSoLanDen";
            colSoLanDen.ReadOnly = true;
            // 
            // tabKhac
            // 
            tabKhac.Controls.Add(panelKhac);
            tabKhac.Location = new Point(4, 30);
            tabKhac.Margin = new Padding(4, 3, 4, 3);
            tabKhac.Name = "tabKhac";
            tabKhac.Size = new Size(1509, 635);
            tabKhac.TabIndex = 3;
            tabKhac.Text = "📋 Khác";
            tabKhac.UseVisualStyleBackColor = true;
            // 
            // panelKhac
            // 
            panelKhac.Controls.Add(panelSoSanh);
            panelKhac.Dock = DockStyle.Fill;
            panelKhac.Location = new Point(0, 0);
            panelKhac.Margin = new Padding(4, 3, 4, 3);
            panelKhac.Name = "panelKhac";
            panelKhac.Padding = new Padding(12);
            panelKhac.Size = new Size(1509, 635);
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
            panelSoSanh.Location = new Point(12, 12);
            panelSoSanh.Margin = new Padding(4, 3, 4, 3);
            panelSoSanh.Name = "panelSoSanh";
            panelSoSanh.Padding = new Padding(23);
            panelSoSanh.Size = new Size(1353, 288);
            panelSoSanh.TabIndex = 0;
            // 
            // cardChenhLech
            // 
            cardChenhLech.BackColor = Color.FromArgb(248, 249, 250);
            cardChenhLech.BorderStyle = BorderStyle.FixedSingle;
            cardChenhLech.Controls.Add(lblChenhLechPercent);
            cardChenhLech.Controls.Add(lblChenhLechValue);
            cardChenhLech.Controls.Add(lblChenhLechTitle);
            cardChenhLech.Location = new Point(887, 104);
            cardChenhLech.Margin = new Padding(4, 3, 4, 3);
            cardChenhLech.Name = "cardChenhLech";
            cardChenhLech.Padding = new Padding(23);
            cardChenhLech.Size = new Size(408, 150);
            cardChenhLech.TabIndex = 3;
            // 
            // lblChenhLechPercent
            // 
            lblChenhLechPercent.AutoSize = true;
            lblChenhLechPercent.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblChenhLechPercent.ForeColor = Color.FromArgb(40, 167, 69);
            lblChenhLechPercent.Location = new Point(47, 127);
            lblChenhLechPercent.Margin = new Padding(4, 0, 4, 0);
            lblChenhLechPercent.Name = "lblChenhLechPercent";
            lblChenhLechPercent.Size = new Size(0, 15);
            lblChenhLechPercent.TabIndex = 2;
            // 
            // lblChenhLechValue
            // 
            lblChenhLechValue.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblChenhLechValue.ForeColor = Color.FromArgb(44, 62, 80);
            lblChenhLechValue.Location = new Point(23, 52);
            lblChenhLechValue.Margin = new Padding(4, 0, 4, 0);
            lblChenhLechValue.Name = "lblChenhLechValue";
            lblChenhLechValue.Size = new Size(350, 46);
            lblChenhLechValue.TabIndex = 1;
            lblChenhLechValue.Text = "0 đ";
            // 
            // lblChenhLechTitle
            // 
            lblChenhLechTitle.AutoSize = true;
            lblChenhLechTitle.Font = new Font("Segoe UI", 10F);
            lblChenhLechTitle.ForeColor = Color.Gray;
            lblChenhLechTitle.Location = new Point(47, 46);
            lblChenhLechTitle.Margin = new Padding(4, 0, 4, 0);
            lblChenhLechTitle.Name = "lblChenhLechTitle";
            lblChenhLechTitle.Size = new Size(77, 19);
            lblChenhLechTitle.TabIndex = 0;
            lblChenhLechTitle.Text = "Chênh lệch";
            // 
            // cardKyTruoc
            // 
            cardKyTruoc.BackColor = Color.FromArgb(248, 249, 250);
            cardKyTruoc.BorderStyle = BorderStyle.FixedSingle;
            cardKyTruoc.Controls.Add(lblKyTruocValue);
            cardKyTruoc.Controls.Add(lblKyTruocTitle);
            cardKyTruoc.Location = new Point(455, 104);
            cardKyTruoc.Margin = new Padding(4, 3, 4, 3);
            cardKyTruoc.Name = "cardKyTruoc";
            cardKyTruoc.Padding = new Padding(23);
            cardKyTruoc.Size = new Size(408, 150);
            cardKyTruoc.TabIndex = 2;
            // 
            // lblKyTruocValue
            // 
            lblKyTruocValue.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblKyTruocValue.ForeColor = Color.FromArgb(44, 62, 80);
            lblKyTruocValue.Location = new Point(23, 52);
            lblKyTruocValue.Margin = new Padding(4, 0, 4, 0);
            lblKyTruocValue.Name = "lblKyTruocValue";
            lblKyTruocValue.Size = new Size(350, 46);
            lblKyTruocValue.TabIndex = 1;
            lblKyTruocValue.Text = "0 đ";
            // 
            // lblKyTruocTitle
            // 
            lblKyTruocTitle.AutoSize = true;
            lblKyTruocTitle.Font = new Font("Segoe UI", 10F);
            lblKyTruocTitle.ForeColor = Color.Gray;
            lblKyTruocTitle.Location = new Point(47, 46);
            lblKyTruocTitle.Margin = new Padding(4, 0, 4, 0);
            lblKyTruocTitle.Name = "lblKyTruocTitle";
            lblKyTruocTitle.Size = new Size(66, 19);
            lblKyTruocTitle.TabIndex = 0;
            lblKyTruocTitle.Text = "Hôm qua";
            // 
            // cardKyHienTai
            // 
            cardKyHienTai.BackColor = Color.FromArgb(248, 249, 250);
            cardKyHienTai.BorderStyle = BorderStyle.FixedSingle;
            cardKyHienTai.Controls.Add(lblKyHienTaiPercent);
            cardKyHienTai.Controls.Add(lblKyHienTaiValue);
            cardKyHienTai.Controls.Add(lblKyHienTaiTitle);
            cardKyHienTai.Location = new Point(23, 104);
            cardKyHienTai.Margin = new Padding(4, 3, 4, 3);
            cardKyHienTai.Name = "cardKyHienTai";
            cardKyHienTai.Padding = new Padding(23);
            cardKyHienTai.Size = new Size(408, 150);
            cardKyHienTai.TabIndex = 1;
            // 
            // lblKyHienTaiPercent
            // 
            lblKyHienTaiPercent.AutoSize = true;
            lblKyHienTaiPercent.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblKyHienTaiPercent.ForeColor = Color.FromArgb(40, 167, 69);
            lblKyHienTaiPercent.Location = new Point(47, 127);
            lblKyHienTaiPercent.Margin = new Padding(4, 0, 4, 0);
            lblKyHienTaiPercent.Name = "lblKyHienTaiPercent";
            lblKyHienTaiPercent.Size = new Size(0, 15);
            lblKyHienTaiPercent.TabIndex = 2;
            // 
            // lblKyHienTaiValue
            // 
            lblKyHienTaiValue.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblKyHienTaiValue.ForeColor = Color.FromArgb(44, 62, 80);
            lblKyHienTaiValue.Location = new Point(23, 52);
            lblKyHienTaiValue.Margin = new Padding(4, 0, 4, 0);
            lblKyHienTaiValue.Name = "lblKyHienTaiValue";
            lblKyHienTaiValue.Size = new Size(350, 46);
            lblKyHienTaiValue.TabIndex = 1;
            lblKyHienTaiValue.Text = "0 đ";
            // 
            // lblKyHienTaiTitle
            // 
            lblKyHienTaiTitle.AutoSize = true;
            lblKyHienTaiTitle.Font = new Font("Segoe UI", 10F);
            lblKyHienTaiTitle.ForeColor = Color.Gray;
            lblKyHienTaiTitle.Location = new Point(47, 46);
            lblKyHienTaiTitle.Margin = new Padding(4, 0, 4, 0);
            lblKyHienTaiTitle.Name = "lblKyHienTaiTitle";
            lblKyHienTaiTitle.Size = new Size(65, 19);
            lblKyHienTaiTitle.TabIndex = 0;
            lblKyHienTaiTitle.Text = "Hôm nay";
            // 
            // panelSoSanhHeader
            // 
            panelSoSanhHeader.Controls.Add(btnThang);
            panelSoSanhHeader.Controls.Add(btnTuan);
            panelSoSanhHeader.Controls.Add(btnNgay);
            panelSoSanhHeader.Controls.Add(lblSoSanhTitle);
            panelSoSanhHeader.Dock = DockStyle.Top;
            panelSoSanhHeader.Location = new Point(23, 23);
            panelSoSanhHeader.Margin = new Padding(4, 3, 4, 3);
            panelSoSanhHeader.Name = "panelSoSanhHeader";
            panelSoSanhHeader.Size = new Size(1305, 58);
            panelSoSanhHeader.TabIndex = 0;
            // 
            // btnThang
            // 
            btnThang.BackColor = Color.White;
            btnThang.FlatAppearance.BorderColor = Color.LightGray;
            btnThang.FlatStyle = FlatStyle.Flat;
            btnThang.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnThang.Location = new Point(1146, 9);
            btnThang.Margin = new Padding(4, 3, 4, 3);
            btnThang.Name = "btnThang";
            btnThang.Size = new Size(117, 40);
            btnThang.TabIndex = 3;
            btnThang.Text = "Tháng này";
            btnThang.UseVisualStyleBackColor = false;
            btnThang.Click += btnThang_Click;
            // 
            // btnTuan
            // 
            btnTuan.BackColor = Color.White;
            btnTuan.FlatAppearance.BorderColor = Color.LightGray;
            btnTuan.FlatStyle = FlatStyle.Flat;
            btnTuan.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTuan.Location = new Point(1011, 9);
            btnTuan.Margin = new Padding(4, 3, 4, 3);
            btnTuan.Name = "btnTuan";
            btnTuan.Size = new Size(117, 40);
            btnTuan.TabIndex = 2;
            btnTuan.Text = "Tuần này";
            btnTuan.UseVisualStyleBackColor = false;
            btnTuan.Click += btnTuan_Click;
            // 
            // btnNgay
            // 
            btnNgay.BackColor = Color.FromArgb(102, 126, 234);
            btnNgay.FlatAppearance.BorderSize = 0;
            btnNgay.FlatStyle = FlatStyle.Flat;
            btnNgay.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnNgay.ForeColor = Color.White;
            btnNgay.Location = new Point(869, 9);
            btnNgay.Margin = new Padding(4, 3, 4, 3);
            btnNgay.Name = "btnNgay";
            btnNgay.Size = new Size(117, 40);
            btnNgay.TabIndex = 1;
            btnNgay.Text = "Hôm nay";
            btnNgay.UseVisualStyleBackColor = false;
            btnNgay.Click += btnNgay_Click;
            // 
            // lblSoSanhTitle
            // 
            lblSoSanhTitle.AutoSize = true;
            lblSoSanhTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblSoSanhTitle.Location = new Point(0, 14);
            lblSoSanhTitle.Margin = new Padding(4, 0, 4, 0);
            lblSoSanhTitle.Name = "lblSoSanhTitle";
            lblSoSanhTitle.Size = new Size(205, 25);
            lblSoSanhTitle.TabIndex = 0;
            lblSoSanhTitle.Text = "📊 So sánh doanh thu";
            // 
            // panelThangHeader
            // 
            panelThangHeader.BackColor = Color.White;
            panelThangHeader.BorderStyle = BorderStyle.FixedSingle;
            panelThangHeader.Controls.Add(cboNam);
            panelThangHeader.Controls.Add(lblDoanhThuThang);
            panelThangHeader.Location = new Point(10, 340);
            panelThangHeader.Name = "panelThangHeader";
            panelThangHeader.Padding = new Padding(15, 10, 15, 10);
            panelThangHeader.Size = new Size(1150, 50);
            panelThangHeader.TabIndex = 4;
            // 
            // cboNam
            // 
            cboNam.DropDownStyle = ComboBoxStyle.DropDownList;
            cboNam.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            cboNam.FormattingEnabled = true;
            cboNam.Location = new Point(1020, 10);
            cboNam.Name = "cboNam";
            cboNam.Size = new Size(100, 27);
            cboNam.TabIndex = 1;
            cboNam.SelectedIndexChanged += cboNam_SelectedIndexChanged;
            // 
            // lblDoanhThuThang
            // 
            lblDoanhThuThang.AutoSize = true;
            lblDoanhThuThang.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblDoanhThuThang.Location = new Point(30, 22);
            lblDoanhThuThang.Name = "lblDoanhThuThang";
            lblDoanhThuThang.Size = new Size(206, 21);
            lblDoanhThuThang.TabIndex = 0;
            lblDoanhThuThang.Text = "📅 Doanh thu theo tháng";
            // 
            // panelDoanhThu7Ngay
            // 
            panelDoanhThu7Ngay.BackColor = Color.White;
            panelDoanhThu7Ngay.BorderStyle = BorderStyle.FixedSingle;
            panelDoanhThu7Ngay.Location = new Point(12, 12);
            panelDoanhThu7Ngay.Margin = new Padding(4, 3, 4, 3);
            panelDoanhThu7Ngay.Name = "panelDoanhThu7Ngay";
            panelDoanhThu7Ngay.Padding = new Padding(18, 17, 18, 17);
            panelDoanhThu7Ngay.Size = new Size(1341, 369);
            panelDoanhThu7Ngay.TabIndex = 0;
            panelDoanhThu7Ngay.Paint += panelDoanhThu7Ngay_Paint;
            // 
            // ThongKeForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(240, 242, 245);
            ClientSize = new Size(1517, 923);
            Controls.Add(tabControl);
            Controls.Add(panelCards);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 3, 4, 3);
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
            tabDichVu.ResumeLayout(false);
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
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DateTimePicker dtpTuNgay;
        private System.Windows.Forms.Label lblDen;
        private System.Windows.Forms.DateTimePicker dtpDenNgay;
        private System.Windows.Forms.Button btnXemBaoCao;
        private System.Windows.Forms.Button btnHomNay;
        private System.Windows.Forms.Panel panelCards;
        private System.Windows.Forms.Panel cardDoanhThu;
        private System.Windows.Forms.Label lblDoanhThuIcon;
        private System.Windows.Forms.Label lblDoanhThuTitle;
        private System.Windows.Forms.Label lblDoanhThuValue;
        private System.Windows.Forms.Panel cardHoaDon;
        private System.Windows.Forms.Label lblHoaDonSubtitle;
        private System.Windows.Forms.Label lblHoaDonValue;
        private System.Windows.Forms.Label lblHoaDonTitle;
        private System.Windows.Forms.Label lblHoaDonIcon;
        private System.Windows.Forms.Panel cardKhachHang;
        private System.Windows.Forms.Label lblKhachHangSubtitle;
        private System.Windows.Forms.Label lblKhachHangValue;
        private System.Windows.Forms.Label lblKhachHangTitle;
        private System.Windows.Forms.Label lblKhachHangIcon;
        private System.Windows.Forms.Panel cardTrungBinh;
        private System.Windows.Forms.Label lblTrungBinhSubtitle;
        private System.Windows.Forms.Label lblTrungBinhValue;
        private System.Windows.Forms.Label lblTrungBinhTitle;
        private System.Windows.Forms.Label lblTrungBinhIcon;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabTongQuan;
        private System.Windows.Forms.Panel panelTongQuan;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartDoanhThu7Ngay;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartDoanhThuThang;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartKhungGio;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartPhuongThuc;
        private System.Windows.Forms.Panel panelThangHeader;
        private System.Windows.Forms.ComboBox cboNam;
        private System.Windows.Forms.Label lblDoanhThuThang;
        private System.Windows.Forms.TabPage tabDichVu;
        private System.Windows.Forms.Panel panelDichVu;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartTopDichVu;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartLoaiDichVu;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartLoaiBan;
        private System.Windows.Forms.TabPage tabKhachHang;
        private System.Windows.Forms.DataGridView dgvTopKhachHang;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSTT;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenKH;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSDT;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTongChiTieu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoLanDen;
        private System.Windows.Forms.TabPage tabKhac;
        private System.Windows.Forms.Panel panelKhac;
        private System.Windows.Forms.Panel panelSoSanh;
        private System.Windows.Forms.Panel panelSoSanhHeader;
        private System.Windows.Forms.Label lblSoSanhTitle;
        private System.Windows.Forms.Button btnNgay;
        private System.Windows.Forms.Button btnTuan;
        private System.Windows.Forms.Button btnThang;
        private System.Windows.Forms.Panel cardKyHienTai;
        private System.Windows.Forms.Label lblKyHienTaiTitle;
        private System.Windows.Forms.Label lblKyHienTaiValue;
        private System.Windows.Forms.Label lblKyHienTaiPercent;
        private System.Windows.Forms.Panel cardKyTruoc;
        private System.Windows.Forms.Label lblKyTruocTitle;
        private System.Windows.Forms.Label lblKyTruocValue;
        private System.Windows.Forms.Panel cardChenhLech;
        private System.Windows.Forms.Label lblChenhLechTitle;
        private System.Windows.Forms.Label lblChenhLechValue;
        private System.Windows.Forms.Label lblChenhLechPercent;
        private Panel panelDoanhThu7Ngay;
    }
}