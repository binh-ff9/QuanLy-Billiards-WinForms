namespace Billiard.WinForm.Forms
{
    partial class ucPhieuNhapXuat
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

        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.btnTimKiem = new System.Windows.Forms.Button();
            this.dtpDenNgay = new System.Windows.Forms.DateTimePicker();
            this.dtpTuNgay = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboNhaCungCap = new System.Windows.Forms.ComboBox();
            this.lblNhaCungCap = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPhieuNhap = new System.Windows.Forms.TabPage();
            this.panelPhieuNhapBottom = new System.Windows.Forms.Panel();
            this.btnXemChiTiet = new System.Windows.Forms.Button();
            this.btnTaoPhieuNhap = new System.Windows.Forms.Button();
            this.dgvPhieuNhap = new System.Windows.Forms.DataGridView();
            this.colMaPN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNhanVien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNhaCungCap = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNgayNhap = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTongTien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGhiChu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabChiTietPhieu = new System.Windows.Forms.TabPage();
            this.panelChiTietBottom = new System.Windows.Forms.Panel();
            this.btnQuayLai = new System.Windows.Forms.Button();
            this.dgvChiTiet = new System.Windows.Forms.DataGridView();
            this.colSTT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMatHang = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoLuong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDonGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThanhTien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelChiTietTop = new System.Windows.Forms.Panel();
            this.lblThongTinPhieu = new System.Windows.Forms.Label();
            this.panelTop.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPhieuNhap.SuspendLayout();
            this.panelPhieuNhapBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhieuNhap)).BeginInit();
            this.tabChiTietPhieu.SuspendLayout();
            this.panelChiTietBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTiet)).BeginInit();
            this.panelChiTietTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.White;
            this.panelTop.Controls.Add(this.btnLamMoi);
            this.panelTop.Controls.Add(this.btnTimKiem);
            this.panelTop.Controls.Add(this.dtpDenNgay);
            this.panelTop.Controls.Add(this.dtpTuNgay);
            this.panelTop.Controls.Add(this.label2);
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Controls.Add(this.cboNhaCungCap);
            this.panelTop.Controls.Add(this.lblNhaCungCap);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(950, 120);
            this.panelTop.TabIndex = 0;
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnLamMoi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLamMoi.FlatAppearance.BorderSize = 0;
            this.btnLamMoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLamMoi.ForeColor = System.Drawing.Color.White;
            this.btnLamMoi.Location = new System.Drawing.Point(750, 70);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(120, 35);
            this.btnLamMoi.TabIndex = 7;
            this.btnLamMoi.Text = "Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = false;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnTimKiem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTimKiem.FlatAppearance.BorderSize = 0;
            this.btnTimKiem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTimKiem.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnTimKiem.ForeColor = System.Drawing.Color.White;
            this.btnTimKiem.Location = new System.Drawing.Point(750, 30);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(120, 35);
            this.btnTimKiem.TabIndex = 6;
            this.btnTimKiem.Text = "Tìm kiếm";
            this.btnTimKiem.UseVisualStyleBackColor = false;
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click);
            // 
            // dtpDenNgay
            // 
            this.dtpDenNgay.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpDenNgay.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDenNgay.Location = new System.Drawing.Point(530, 70);
            this.dtpDenNgay.Name = "dtpDenNgay";
            this.dtpDenNgay.Size = new System.Drawing.Size(180, 25);
            this.dtpDenNgay.TabIndex = 5;
            // 
            // dtpTuNgay
            // 
            this.dtpTuNgay.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpTuNgay.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTuNgay.Location = new System.Drawing.Point(530, 30);
            this.dtpTuNgay.Name = "dtpTuNgay";
            this.dtpTuNgay.Size = new System.Drawing.Size(180, 25);
            this.dtpTuNgay.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(440, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = "Đến ngày:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(440, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "Từ ngày:";
            // 
            // cboNhaCungCap
            // 
            this.cboNhaCungCap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNhaCungCap.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboNhaCungCap.FormattingEnabled = true;
            this.cboNhaCungCap.Location = new System.Drawing.Point(150, 50);
            this.cboNhaCungCap.Name = "cboNhaCungCap";
            this.cboNhaCungCap.Size = new System.Drawing.Size(250, 25);
            this.cboNhaCungCap.TabIndex = 1;
            // 
            // lblNhaCungCap
            // 
            this.lblNhaCungCap.AutoSize = true;
            this.lblNhaCungCap.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblNhaCungCap.Location = new System.Drawing.Point(30, 55);
            this.lblNhaCungCap.Name = "lblNhaCungCap";
            this.lblNhaCungCap.Size = new System.Drawing.Size(105, 19);
            this.lblNhaCungCap.TabIndex = 0;
            this.lblNhaCungCap.Text = "Nhà cung cấp:";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPhieuNhap);
            this.tabControl.Controls.Add(this.tabChiTietPhieu);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tabControl.Location = new System.Drawing.Point(0, 120);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(950, 510);
            this.tabControl.TabIndex = 1;
            // 
            // tabPhieuNhap
            // 
            this.tabPhieuNhap.Controls.Add(this.panelPhieuNhapBottom);
            this.tabPhieuNhap.Controls.Add(this.dgvPhieuNhap);
            this.tabPhieuNhap.Location = new System.Drawing.Point(4, 26);
            this.tabPhieuNhap.Name = "tabPhieuNhap";
            this.tabPhieuNhap.Padding = new System.Windows.Forms.Padding(3);
            this.tabPhieuNhap.Size = new System.Drawing.Size(942, 480);
            this.tabPhieuNhap.TabIndex = 0;
            this.tabPhieuNhap.Text = "Danh sách phiếu nhập";
            this.tabPhieuNhap.UseVisualStyleBackColor = true;
            // 
            // panelPhieuNhapBottom
            // 
            this.panelPhieuNhapBottom.BackColor = System.Drawing.Color.White;
            this.panelPhieuNhapBottom.Controls.Add(this.btnXemChiTiet);
            this.panelPhieuNhapBottom.Controls.Add(this.btnTaoPhieuNhap);
            this.panelPhieuNhapBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelPhieuNhapBottom.Location = new System.Drawing.Point(3, 417);
            this.panelPhieuNhapBottom.Name = "panelPhieuNhapBottom";
            this.panelPhieuNhapBottom.Size = new System.Drawing.Size(936, 60);
            this.panelPhieuNhapBottom.TabIndex = 1;
            // 
            // btnXemChiTiet
            // 
            this.btnXemChiTiet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnXemChiTiet.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXemChiTiet.FlatAppearance.BorderSize = 0;
            this.btnXemChiTiet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXemChiTiet.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnXemChiTiet.ForeColor = System.Drawing.Color.White;
            this.btnXemChiTiet.Location = new System.Drawing.Point(180, 12);
            this.btnXemChiTiet.Name = "btnXemChiTiet";
            this.btnXemChiTiet.Size = new System.Drawing.Size(150, 38);
            this.btnXemChiTiet.TabIndex = 1;
            this.btnXemChiTiet.Text = "Xem chi tiết";
            this.btnXemChiTiet.UseVisualStyleBackColor = false;
            this.btnXemChiTiet.Click += new System.EventHandler(this.btnXemChiTiet_Click);
            // 
            // btnTaoPhieuNhap
            // 
            this.btnTaoPhieuNhap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnTaoPhieuNhap.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTaoPhieuNhap.FlatAppearance.BorderSize = 0;
            this.btnTaoPhieuNhap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTaoPhieuNhap.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnTaoPhieuNhap.ForeColor = System.Drawing.Color.White;
            this.btnTaoPhieuNhap.Location = new System.Drawing.Point(20, 12);
            this.btnTaoPhieuNhap.Name = "btnTaoPhieuNhap";
            this.btnTaoPhieuNhap.Size = new System.Drawing.Size(150, 38);
            this.btnTaoPhieuNhap.TabIndex = 0;
            this.btnTaoPhieuNhap.Text = "Tạo phiếu nhập";
            this.btnTaoPhieuNhap.UseVisualStyleBackColor = false;
            this.btnTaoPhieuNhap.Click += new System.EventHandler(this.btnTaoPhieuNhap_Click);
            // 
            // dgvPhieuNhap
            // 
            this.dgvPhieuNhap.AllowUserToAddRows = false;
            this.dgvPhieuNhap.AllowUserToDeleteRows = false;
            this.dgvPhieuNhap.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPhieuNhap.BackgroundColor = System.Drawing.Color.White;
            this.dgvPhieuNhap.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPhieuNhap.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaPN,
            this.colNhanVien,
            this.colNhaCungCap,
            this.colNgayNhap,
            this.colTongTien,
            this.colGhiChu});
            this.dgvPhieuNhap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPhieuNhap.Location = new System.Drawing.Point(3, 3);
            this.dgvPhieuNhap.Name = "dgvPhieuNhap";
            this.dgvPhieuNhap.ReadOnly = true;
            this.dgvPhieuNhap.RowHeadersVisible = false;
            this.dgvPhieuNhap.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPhieuNhap.Size = new System.Drawing.Size(936, 474);
            this.dgvPhieuNhap.TabIndex = 0;
            // 
            // colMaPN
            // 
            this.colMaPN.FillWeight = 60F;
            this.colMaPN.HeaderText = "Mã PN";
            this.colMaPN.Name = "colMaPN";
            this.colMaPN.ReadOnly = true;
            // 
            // colNhanVien
            // 
            this.colNhanVien.FillWeight = 120F;
            this.colNhanVien.HeaderText = "Nhân viên";
            this.colNhanVien.Name = "colNhanVien";
            this.colNhanVien.ReadOnly = true;
            // 
            // colNhaCungCap
            // 
            this.colNhaCungCap.FillWeight = 150F;
            this.colNhaCungCap.HeaderText = "Nhà cung cấp";
            this.colNhaCungCap.Name = "colNhaCungCap";
            this.colNhaCungCap.ReadOnly = true;
            // 
            // colNgayNhap
            // 
            this.colNgayNhap.FillWeight = 100F;
            this.colNgayNhap.HeaderText = "Ngày nhập";
            this.colNgayNhap.Name = "colNgayNhap";
            this.colNgayNhap.ReadOnly = true;
            // 
            // colTongTien
            // 
            this.colTongTien.FillWeight = 100F;
            this.colTongTien.HeaderText = "Tổng tiền";
            this.colTongTien.Name = "colTongTien";
            this.colTongTien.ReadOnly = true;
            // 
            // colGhiChu
            // 
            this.colGhiChu.FillWeight = 150F;
            this.colGhiChu.HeaderText = "Ghi chú";
            this.colGhiChu.Name = "colGhiChu";
            this.colGhiChu.ReadOnly = true;
            // 
            // tabChiTietPhieu
            // 
            this.tabChiTietPhieu.Controls.Add(this.panelChiTietBottom);
            this.tabChiTietPhieu.Controls.Add(this.dgvChiTiet);
            this.tabChiTietPhieu.Controls.Add(this.panelChiTietTop);
            this.tabChiTietPhieu.Location = new System.Drawing.Point(4, 26);
            this.tabChiTietPhieu.Name = "tabChiTietPhieu";
            this.tabChiTietPhieu.Padding = new System.Windows.Forms.Padding(3);
            this.tabChiTietPhieu.Size = new System.Drawing.Size(942, 480);
            this.tabChiTietPhieu.TabIndex = 1;
            this.tabChiTietPhieu.Text = "Chi tiết phiếu nhập";
            this.tabChiTietPhieu.UseVisualStyleBackColor = true;
            // 
            // panelChiTietBottom
            // 
            this.panelChiTietBottom.BackColor = System.Drawing.Color.White;
            this.panelChiTietBottom.Controls.Add(this.btnQuayLai);
            this.panelChiTietBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelChiTietBottom.Location = new System.Drawing.Point(3, 417);
            this.panelChiTietBottom.Name = "panelChiTietBottom";
            this.panelChiTietBottom.Size = new System.Drawing.Size(936, 60);
            this.panelChiTietBottom.TabIndex = 2;
            // 
            // btnQuayLai
            // 
            this.btnQuayLai.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.btnQuayLai.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnQuayLai.FlatAppearance.BorderSize = 0;
            this.btnQuayLai.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuayLai.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnQuayLai.ForeColor = System.Drawing.Color.White;
            this.btnQuayLai.Location = new System.Drawing.Point(20, 12);
            this.btnQuayLai.Name = "btnQuayLai";
            this.btnQuayLai.Size = new System.Drawing.Size(120, 38);
            this.btnQuayLai.TabIndex = 0;
            this.btnQuayLai.Text = "Quay lại";
            this.btnQuayLai.UseVisualStyleBackColor = false;
            this.btnQuayLai.Click += new System.EventHandler(this.btnQuayLai_Click);
            // 
            // dgvChiTiet
            // 
            this.dgvChiTiet.AllowUserToAddRows = false;
            this.dgvChiTiet.AllowUserToDeleteRows = false;
            this.dgvChiTiet.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvChiTiet.BackgroundColor = System.Drawing.Color.White;
            this.dgvChiTiet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChiTiet.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSTT,
            this.colMatHang,
            this.colSoLuong,
            this.colDonGia,
            this.colThanhTien});
            this.dgvChiTiet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvChiTiet.Location = new System.Drawing.Point(3, 73);
            this.dgvChiTiet.Name = "dgvChiTiet";
            this.dgvChiTiet.ReadOnly = true;
            this.dgvChiTiet.RowHeadersVisible = false;
            this.dgvChiTiet.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvChiTiet.Size = new System.Drawing.Size(936, 404);
            this.dgvChiTiet.TabIndex = 1;
            // 
            // colSTT
            // 
            this.colSTT.FillWeight = 40F;
            this.colSTT.HeaderText = "STT";
            this.colSTT.Name = "colSTT";
            this.colSTT.ReadOnly = true;
            // 
            // colMatHang
            // 
            this.colMatHang.FillWeight = 200F;
            this.colMatHang.HeaderText = "Mặt hàng";
            this.colMatHang.Name = "colMatHang";
            this.colMatHang.ReadOnly = true;
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
            // panelChiTietTop
            // 
            this.panelChiTietTop.BackColor = System.Drawing.Color.White;
            this.panelChiTietTop.Controls.Add(this.lblThongTinPhieu);
            this.panelChiTietTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelChiTietTop.Location = new System.Drawing.Point(3, 3);
            this.panelChiTietTop.Name = "panelChiTietTop";
            this.panelChiTietTop.Size = new System.Drawing.Size(936, 70);
            this.panelChiTietTop.TabIndex = 0;
            // 
            // lblThongTinPhieu
            // 
            this.lblThongTinPhieu.AutoSize = true;
            this.lblThongTinPhieu.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblThongTinPhieu.Location = new System.Drawing.Point(20, 25);
            this.lblThongTinPhieu.Name = "lblThongTinPhieu";
            this.lblThongTinPhieu.Size = new System.Drawing.Size(163, 21);
            this.lblThongTinPhieu.TabIndex = 0;
            this.lblThongTinPhieu.Text = "Thông tin phiếu nhập";
            // 
            // ucPhieuNhapXuat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panelTop);
            this.Name = "ucPhieuNhapXuat";
            this.Size = new System.Drawing.Size(950, 630);
            this.Load += new System.EventHandler(this.ucPhieuNhapXuat_Load);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPhieuNhap.ResumeLayout(false);
            this.panelPhieuNhapBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhieuNhap)).EndInit();
            this.tabChiTietPhieu.ResumeLayout(false);
            this.panelChiTietBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTiet)).EndInit();
            this.panelChiTietTop.ResumeLayout(false);
            this.panelChiTietTop.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.ComboBox cboNhaCungCap;
        private System.Windows.Forms.Label lblNhaCungCap;
        private System.Windows.Forms.DateTimePicker dtpDenNgay;
        private System.Windows.Forms.DateTimePicker dtpTuNgay;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.Button btnTimKiem;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPhieuNhap;
        private System.Windows.Forms.DataGridView dgvPhieuNhap;
        private System.Windows.Forms.TabPage tabChiTietPhieu;
        private System.Windows.Forms.Panel panelPhieuNhapBottom;
        private System.Windows.Forms.Button btnTaoPhieuNhap;
        private System.Windows.Forms.Button btnXemChiTiet;
        private System.Windows.Forms.DataGridView dgvChiTiet;
        private System.Windows.Forms.Panel panelChiTietTop;
        private System.Windows.Forms.Label lblThongTinPhieu;
        private System.Windows.Forms.Panel panelChiTietBottom;
        private System.Windows.Forms.Button btnQuayLai;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaPN;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNhanVien;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNhaCungCap;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNgayNhap;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTongTien;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGhiChu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSTT;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMatHang;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoLuong;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDonGia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThanhTien;
    }
}