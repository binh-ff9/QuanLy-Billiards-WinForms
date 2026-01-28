namespace Billiard.WinForm.Forms
{
    partial class frmTaoPhieuNhap
    {
        private System.ComponentModel.IContainer components = null;



        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.groupBoxChiTiet = new System.Windows.Forms.GroupBox();
            this.dgvChiTiet = new System.Windows.Forms.DataGridView();
            this.colMaHang = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenHang = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoLuong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDonGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThanhTien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelChiTietBottom = new System.Windows.Forms.Panel();
            this.lblTongTienValue = new System.Windows.Forms.Label();
            this.lblTongTien = new System.Windows.Forms.Label();
            this.groupBoxThemMatHang = new System.Windows.Forms.GroupBox();
            this.btnXoaMatHang = new System.Windows.Forms.Button();
            this.btnThemMatHang = new System.Windows.Forms.Button();
            this.lblDonVi = new System.Windows.Forms.Label();
            this.txtDonGia = new System.Windows.Forms.TextBox();
            this.lblDonGia = new System.Windows.Forms.Label();
            this.txtSoLuong = new System.Windows.Forms.TextBox();
            this.lblSoLuong = new System.Windows.Forms.Label();
            this.cboMatHang = new System.Windows.Forms.ComboBox();
            this.lblMatHang = new System.Windows.Forms.Label();
            this.groupBoxThongTin = new System.Windows.Forms.GroupBox();
            this.txtGhiChu = new System.Windows.Forms.TextBox();
            this.lblGhiChu = new System.Windows.Forms.Label();
            this.dtpNgayNhap = new System.Windows.Forms.DateTimePicker();
            this.lblNgayNhap = new System.Windows.Forms.Label();
            this.txtNhanVien = new System.Windows.Forms.TextBox();
            this.lblNhanVien = new System.Windows.Forms.Label();
            this.cboNhaCungCap = new System.Windows.Forms.ComboBox();
            this.lblNhaCungCap = new System.Windows.Forms.Label();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.btnHuy = new System.Windows.Forms.Button();
            this.btnLuu = new System.Windows.Forms.Button();
            this.panelTop.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.groupBoxChiTiet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTiet)).BeginInit();
            this.panelChiTietBottom.SuspendLayout();
            this.groupBoxThemMatHang.SuspendLayout();
            this.groupBoxThongTin.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1000, 60);
            this.panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(217, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "TẠO PHIẾU NHẬP";
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.White;
            this.panelMain.Controls.Add(this.groupBoxChiTiet);
            this.panelMain.Controls.Add(this.groupBoxThemMatHang);
            this.panelMain.Controls.Add(this.groupBoxThongTin);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 60);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(20);
            this.panelMain.Size = new System.Drawing.Size(1000, 590);
            this.panelMain.TabIndex = 1;
            // 
            // groupBoxChiTiet
            // 
            this.groupBoxChiTiet.Controls.Add(this.dgvChiTiet);
            this.groupBoxChiTiet.Controls.Add(this.panelChiTietBottom);
            this.groupBoxChiTiet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxChiTiet.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxChiTiet.Location = new System.Drawing.Point(20, 350);
            this.groupBoxChiTiet.Name = "groupBoxChiTiet";
            this.groupBoxChiTiet.Padding = new System.Windows.Forms.Padding(10);
            this.groupBoxChiTiet.Size = new System.Drawing.Size(960, 220);
            this.groupBoxChiTiet.TabIndex = 2;
            this.groupBoxChiTiet.TabStop = false;
            this.groupBoxChiTiet.Text = "Chi tiết phiếu nhập";
            // 
            // dgvChiTiet
            // 
            this.dgvChiTiet.AllowUserToAddRows = false;
            this.dgvChiTiet.AllowUserToDeleteRows = false;
            this.dgvChiTiet.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvChiTiet.BackgroundColor = System.Drawing.Color.White;
            this.dgvChiTiet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChiTiet.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaHang,
            this.colTenHang,
            this.colSoLuong,
            this.colDonGia,
            this.colThanhTien});
            this.dgvChiTiet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvChiTiet.Location = new System.Drawing.Point(10, 26);
            this.dgvChiTiet.Name = "dgvChiTiet";
            this.dgvChiTiet.ReadOnly = true;
            this.dgvChiTiet.RowHeadersVisible = false;
            this.dgvChiTiet.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvChiTiet.Size = new System.Drawing.Size(940, 134);
            this.dgvChiTiet.TabIndex = 0;
            // 
            // colMaHang
            // 
            this.colMaHang.HeaderText = "Mã hàng";
            this.colMaHang.Name = "colMaHang";
            this.colMaHang.ReadOnly = true;
            this.colMaHang.Visible = false;
            // 
            // colTenHang
            // 
            this.colTenHang.FillWeight = 200F;
            this.colTenHang.HeaderText = "Tên hàng";
            this.colTenHang.Name = "colTenHang";
            this.colTenHang.ReadOnly = true;
            // 
            // colSoLuong
            // 
            this.colSoLuong.FillWeight = 80F;
            this.colSoLuong.HeaderText = "Số lượng";
            this.colSoLuong.Name = "colSoLuong";
            this.colSoLuong.ReadOnly = true;
            // 
            // colDonGia
            // 
            this.colDonGia.FillWeight = 100F;
            this.colDonGia.HeaderText = "Đơn giá";
            this.colDonGia.Name = "colDonGia";
            this.colDonGia.ReadOnly = true;
            // 
            // colThanhTien
            // 
            this.colThanhTien.FillWeight = 120F;
            this.colThanhTien.HeaderText = "Thành tiền";
            this.colThanhTien.Name = "colThanhTien";
            this.colThanhTien.ReadOnly = true;
            // 
            // panelChiTietBottom
            // 
            this.panelChiTietBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.panelChiTietBottom.Controls.Add(this.lblTongTienValue);
            this.panelChiTietBottom.Controls.Add(this.lblTongTien);
            this.panelChiTietBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelChiTietBottom.Location = new System.Drawing.Point(10, 160);
            this.panelChiTietBottom.Name = "panelChiTietBottom";
            this.panelChiTietBottom.Size = new System.Drawing.Size(940, 50);
            this.panelChiTietBottom.TabIndex = 1;
            // 
            // lblTongTienValue
            // 
            this.lblTongTienValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTongTienValue.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTongTienValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.lblTongTienValue.Location = new System.Drawing.Point(640, 12);
            this.lblTongTienValue.Name = "lblTongTienValue";
            this.lblTongTienValue.Size = new System.Drawing.Size(280, 25);
            this.lblTongTienValue.TabIndex = 1;
            this.lblTongTienValue.Text = "0 đ";
            this.lblTongTienValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTongTien
            // 
            this.lblTongTien.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTongTien.AutoSize = true;
            this.lblTongTien.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTongTien.Location = new System.Drawing.Point(530, 15);
            this.lblTongTien.Name = "lblTongTien";
            this.lblTongTien.Size = new System.Drawing.Size(91, 21);
            this.lblTongTien.TabIndex = 0;
            this.lblTongTien.Text = "Tổng tiền:";
            // 
            // groupBoxThemMatHang
            // 
            this.groupBoxThemMatHang.Controls.Add(this.btnXoaMatHang);
            this.groupBoxThemMatHang.Controls.Add(this.btnThemMatHang);
            this.groupBoxThemMatHang.Controls.Add(this.lblDonVi);
            this.groupBoxThemMatHang.Controls.Add(this.txtDonGia);
            this.groupBoxThemMatHang.Controls.Add(this.lblDonGia);
            this.groupBoxThemMatHang.Controls.Add(this.txtSoLuong);
            this.groupBoxThemMatHang.Controls.Add(this.lblSoLuong);
            this.groupBoxThemMatHang.Controls.Add(this.cboMatHang);
            this.groupBoxThemMatHang.Controls.Add(this.lblMatHang);
            this.groupBoxThemMatHang.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxThemMatHang.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxThemMatHang.Location = new System.Drawing.Point(20, 190);
            this.groupBoxThemMatHang.Name = "groupBoxThemMatHang";
            this.groupBoxThemMatHang.Padding = new System.Windows.Forms.Padding(10);
            this.groupBoxThemMatHang.Size = new System.Drawing.Size(960, 160);
            this.groupBoxThemMatHang.TabIndex = 1;
            this.groupBoxThemMatHang.TabStop = false;
            this.groupBoxThemMatHang.Text = "Thêm mặt hàng";
            // 
            // btnXoaMatHang
            // 
            this.btnXoaMatHang.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnXoaMatHang.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXoaMatHang.FlatAppearance.BorderSize = 0;
            this.btnXoaMatHang.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoaMatHang.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnXoaMatHang.ForeColor = System.Drawing.Color.White;
            this.btnXoaMatHang.Location = new System.Drawing.Point(820, 105);
            this.btnXoaMatHang.Name = "btnXoaMatHang";
            this.btnXoaMatHang.Size = new System.Drawing.Size(120, 38);
            this.btnXoaMatHang.TabIndex = 8;
            this.btnXoaMatHang.Text = "Xóa";
            this.btnXoaMatHang.UseVisualStyleBackColor = false;
            this.btnXoaMatHang.Click += new System.EventHandler(this.btnXoaMatHang_Click);
            // 
            // btnThemMatHang
            // 
            this.btnThemMatHang.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnThemMatHang.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThemMatHang.FlatAppearance.BorderSize = 0;
            this.btnThemMatHang.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThemMatHang.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnThemMatHang.ForeColor = System.Drawing.Color.White;
            this.btnThemMatHang.Location = new System.Drawing.Point(680, 105);
            this.btnThemMatHang.Name = "btnThemMatHang";
            this.btnThemMatHang.Size = new System.Drawing.Size(120, 38);
            this.btnThemMatHang.TabIndex = 7;
            this.btnThemMatHang.Text = "Thêm";
            this.btnThemMatHang.UseVisualStyleBackColor = false;
            this.btnThemMatHang.Click += new System.EventHandler(this.btnThemMatHang_Click);
            // 
            // lblDonVi
            // 
            this.lblDonVi.AutoSize = true;
            this.lblDonVi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblDonVi.ForeColor = System.Drawing.Color.Gray;
            this.lblDonVi.Location = new System.Drawing.Point(520, 70);
            this.lblDonVi.Name = "lblDonVi";
            this.lblDonVi.Size = new System.Drawing.Size(50, 15);
            this.lblDonVi.TabIndex = 6;
            this.lblDonVi.Text = "Đơn vị: ";
            // 
            // txtDonGia
            // 
            this.txtDonGia.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtDonGia.Location = new System.Drawing.Point(680, 65);
            this.txtDonGia.Name = "txtDonGia";
            this.txtDonGia.Size = new System.Drawing.Size(260, 25);
            this.txtDonGia.TabIndex = 5;
            this.txtDonGia.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDonGia_KeyPress);
            // 
            // lblDonGia
            // 
            this.lblDonGia.AutoSize = true;
            this.lblDonGia.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDonGia.Location = new System.Drawing.Point(680, 40);
            this.lblDonGia.Name = "lblDonGia";
            this.lblDonGia.Size = new System.Drawing.Size(99, 19);
            this.lblDonGia.TabIndex = 4;
            this.lblDonGia.Text = "Đơn giá nhập:";
            // 
            // txtSoLuong
            // 
            this.txtSoLuong.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSoLuong.Location = new System.Drawing.Point(520, 35);
            this.txtSoLuong.Name = "txtSoLuong";
            this.txtSoLuong.Size = new System.Drawing.Size(120, 25);
            this.txtSoLuong.TabIndex = 3;
            this.txtSoLuong.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSoLuong_KeyPress);
            // 
            // lblSoLuong
            // 
            this.lblSoLuong.AutoSize = true;
            this.lblSoLuong.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSoLuong.Location = new System.Drawing.Point(440, 40);
            this.lblSoLuong.Name = "lblSoLuong";
            this.lblSoLuong.Size = new System.Drawing.Size(72, 19);
            this.lblSoLuong.TabIndex = 2;
            this.lblSoLuong.Text = "Số lượng:";
            // 
            // cboMatHang
            // 
            this.cboMatHang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMatHang.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboMatHang.FormattingEnabled = true;
            this.cboMatHang.Location = new System.Drawing.Point(20, 60);
            this.cboMatHang.Name = "cboMatHang";
            this.cboMatHang.Size = new System.Drawing.Size(380, 25);
            this.cboMatHang.TabIndex = 1;
            this.cboMatHang.SelectedIndexChanged += new System.EventHandler(this.cboMatHang_SelectedIndexChanged);
            // 
            // lblMatHang
            // 
            this.lblMatHang.AutoSize = true;
            this.lblMatHang.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblMatHang.Location = new System.Drawing.Point(20, 35);
            this.lblMatHang.Name = "lblMatHang";
            this.lblMatHang.Size = new System.Drawing.Size(72, 19);
            this.lblMatHang.TabIndex = 0;
            this.lblMatHang.Text = "Mặt hàng:";
            // 
            // groupBoxThongTin
            // 
            this.groupBoxThongTin.Controls.Add(this.txtGhiChu);
            this.groupBoxThongTin.Controls.Add(this.lblGhiChu);
            this.groupBoxThongTin.Controls.Add(this.dtpNgayNhap);
            this.groupBoxThongTin.Controls.Add(this.lblNgayNhap);
            this.groupBoxThongTin.Controls.Add(this.txtNhanVien);
            this.groupBoxThongTin.Controls.Add(this.lblNhanVien);
            this.groupBoxThongTin.Controls.Add(this.cboNhaCungCap);
            this.groupBoxThongTin.Controls.Add(this.lblNhaCungCap);
            this.groupBoxThongTin.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxThongTin.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxThongTin.Location = new System.Drawing.Point(20, 20);
            this.groupBoxThongTin.Name = "groupBoxThongTin";
            this.groupBoxThongTin.Padding = new System.Windows.Forms.Padding(10);
            this.groupBoxThongTin.Size = new System.Drawing.Size(960, 170);
            this.groupBoxThongTin.TabIndex = 0;
            this.groupBoxThongTin.TabStop = false;
            this.groupBoxThongTin.Text = "Thông tin phiếu nhập";
            // 
            // txtGhiChu
            // 
            this.txtGhiChu.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtGhiChu.Location = new System.Drawing.Point(520, 60);
            this.txtGhiChu.Multiline = true;
            this.txtGhiChu.Name = "txtGhiChu";
            this.txtGhiChu.Size = new System.Drawing.Size(420, 90);
            this.txtGhiChu.TabIndex = 7;
            // 
            // lblGhiChu
            // 
            this.lblGhiChu.AutoSize = true;
            this.lblGhiChu.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblGhiChu.Location = new System.Drawing.Point(520, 35);
            this.lblGhiChu.Name = "lblGhiChu";
            this.lblGhiChu.Size = new System.Drawing.Size(60, 19);
            this.lblGhiChu.TabIndex = 6;
            this.lblGhiChu.Text = "Ghi chú:";
            // 
            // dtpNgayNhap
            // 
            this.dtpNgayNhap.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpNgayNhap.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpNgayNhap.Location = new System.Drawing.Point(140, 120);
            this.dtpNgayNhap.Name = "dtpNgayNhap";
            this.dtpNgayNhap.Size = new System.Drawing.Size(320, 25);
            this.dtpNgayNhap.TabIndex = 5;
            // 
            // lblNgayNhap
            // 
            this.lblNgayNhap.AutoSize = true;
            this.lblNgayNhap.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblNgayNhap.Location = new System.Drawing.Point(20, 125);
            this.lblNgayNhap.Name = "lblNgayNhap";
            this.lblNgayNhap.Size = new System.Drawing.Size(82, 19);
            this.lblNgayNhap.TabIndex = 4;
            this.lblNgayNhap.Text = "Ngày nhập:";
            // 
            // txtNhanVien
            // 
            this.txtNhanVien.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtNhanVien.Location = new System.Drawing.Point(140, 80);
            this.txtNhanVien.Name = "txtNhanVien";
            this.txtNhanVien.ReadOnly = true;
            this.txtNhanVien.Size = new System.Drawing.Size(320, 25);
            this.txtNhanVien.TabIndex = 3;
            // 
            // lblNhanVien
            // 
            this.lblNhanVien.AutoSize = true;
            this.lblNhanVien.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblNhanVien.Location = new System.Drawing.Point(20, 85);
            this.lblNhanVien.Name = "lblNhanVien";
            this.lblNhanVien.Size = new System.Drawing.Size(78, 19);
            this.lblNhanVien.TabIndex = 2;
            this.lblNhanVien.Text = "Nhân viên:";
            // 
            // cboNhaCungCap
            // 
            this.cboNhaCungCap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNhaCungCap.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboNhaCungCap.FormattingEnabled = true;
            this.cboNhaCungCap.Location = new System.Drawing.Point(140, 40);
            this.cboNhaCungCap.Name = "cboNhaCungCap";
            this.cboNhaCungCap.Size = new System.Drawing.Size(320, 25);
            this.cboNhaCungCap.TabIndex = 1;
            // 
            // lblNhaCungCap
            // 
            this.lblNhaCungCap.AutoSize = true;
            this.lblNhaCungCap.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblNhaCungCap.Location = new System.Drawing.Point(20, 45);
            this.lblNhaCungCap.Name = "lblNhaCungCap";
            this.lblNhaCungCap.Size = new System.Drawing.Size(101, 19);
            this.lblNhaCungCap.TabIndex = 0;
            this.lblNhaCungCap.Text = "Nhà cung cấp:";
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.White;
            this.panelBottom.Controls.Add(this.btnHuy);
            this.panelBottom.Controls.Add(this.btnLuu);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 650);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1000, 70);
            this.panelBottom.TabIndex = 2;
            // 
            // btnHuy
            // 
            this.btnHuy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHuy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.btnHuy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHuy.FlatAppearance.BorderSize = 0;
            this.btnHuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnHuy.ForeColor = System.Drawing.Color.White;
            this.btnHuy.Location = new System.Drawing.Point(840, 17);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(130, 40);
            this.btnHuy.TabIndex = 1;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.UseVisualStyleBackColor = false;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // btnLuu
            // 
            this.btnLuu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLuu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnLuu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLuu.FlatAppearance.BorderSize = 0;
            this.btnLuu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.Location = new System.Drawing.Point(690, 17);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(130, 40);
            this.btnLuu.TabIndex = 0;
            this.btnLuu.Text = "Lưu";
            this.btnLuu.UseVisualStyleBackColor = false;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // frmTaoPhieuNhap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 720);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTaoPhieuNhap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tạo phiếu nhập";
            this.Load += new System.EventHandler(this.frmTaoPhieuNhap_Load);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.groupBoxChiTiet.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTiet)).EndInit();
            this.panelChiTietBottom.ResumeLayout(false);
            this.panelChiTietBottom.PerformLayout();
            this.groupBoxThemMatHang.ResumeLayout(false);
            this.groupBoxThemMatHang.PerformLayout();
            this.groupBoxThongTin.ResumeLayout(false);
            this.groupBoxThongTin.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.GroupBox groupBoxThongTin;
        private System.Windows.Forms.ComboBox cboNhaCungCap;
        private System.Windows.Forms.Label lblNhaCungCap;
        private System.Windows.Forms.TextBox txtNhanVien;
        private System.Windows.Forms.Label lblNhanVien;
        private System.Windows.Forms.DateTimePicker dtpNgayNhap;
        private System.Windows.Forms.Label lblNgayNhap;
        private System.Windows.Forms.TextBox txtGhiChu;
        private System.Windows.Forms.Label lblGhiChu;
        private System.Windows.Forms.GroupBox groupBoxThemMatHang;
        private System.Windows.Forms.ComboBox cboMatHang;
        private System.Windows.Forms.Label lblMatHang;
        private System.Windows.Forms.TextBox txtSoLuong;
        private System.Windows.Forms.Label lblSoLuong;
        private System.Windows.Forms.TextBox txtDonGia;
        private System.Windows.Forms.Label lblDonGia;
        private System.Windows.Forms.Label lblDonVi;
        private System.Windows.Forms.Button btnThemMatHang;
        private System.Windows.Forms.Button btnXoaMatHang;
        private System.Windows.Forms.GroupBox groupBoxChiTiet;
        private System.Windows.Forms.DataGridView dgvChiTiet;
        private System.Windows.Forms.Panel panelChiTietBottom;
        private System.Windows.Forms.Label lblTongTien;
        private System.Windows.Forms.Label lblTongTienValue;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaHang;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenHang;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoLuong;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDonGia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThanhTien;
    }
}