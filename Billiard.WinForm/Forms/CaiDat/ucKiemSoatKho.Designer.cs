namespace Billiard.WinForm.Forms
{
    partial class ucKiemSoatKho
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
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboLoaiHang = new System.Windows.Forms.ComboBox();
            this.lblLoaiHang = new System.Windows.Forms.Label();
            this.cboTrangThai = new System.Windows.Forms.ComboBox();
            this.lblTrangThai = new System.Windows.Forms.Label();
            this.panelStats = new System.Windows.Forms.Panel();
            this.panelTongMH = new System.Windows.Forms.Panel();
            this.lblTongMHValue = new System.Windows.Forms.Label();
            this.lblTongMH = new System.Windows.Forms.Label();
            this.panelConHang = new System.Windows.Forms.Panel();
            this.lblConHangValue = new System.Windows.Forms.Label();
            this.lblConHang = new System.Windows.Forms.Label();
            this.panelSapHet = new System.Windows.Forms.Panel();
            this.lblSapHetValue = new System.Windows.Forms.Label();
            this.lblSapHet = new System.Windows.Forms.Label();
            this.panelHetHang = new System.Windows.Forms.Panel();
            this.lblHetHangValue = new System.Windows.Forms.Label();
            this.lblHetHang = new System.Windows.Forms.Label();
            this.dgvMatHang = new System.Windows.Forms.DataGridView();
            this.colMaHang = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenHang = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLoai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDonVi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoLuongTon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNguongCanhBao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrangThai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNgayNhapGanNhat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelTop.SuspendLayout();
            this.panelStats.SuspendLayout();
            this.panelTongMH.SuspendLayout();
            this.panelConHang.SuspendLayout();
            this.panelSapHet.SuspendLayout();
            this.panelHetHang.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMatHang)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.White;
            this.panelTop.Controls.Add(this.cboTrangThai);
            this.panelTop.Controls.Add(this.lblTrangThai);
            this.panelTop.Controls.Add(this.btnLamMoi);
            this.panelTop.Controls.Add(this.btnTimKiem);
            this.panelTop.Controls.Add(this.txtTimKiem);
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Controls.Add(this.cboLoaiHang);
            this.panelTop.Controls.Add(this.lblLoaiHang);
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
            this.btnLamMoi.Location = new System.Drawing.Point(810, 65);
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
            this.btnTimKiem.Location = new System.Drawing.Point(810, 20);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(120, 35);
            this.btnTimKiem.TabIndex = 6;
            this.btnTimKiem.Text = "Tìm kiếm";
            this.btnTimKiem.UseVisualStyleBackColor = false;
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click);
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTimKiem.Location = new System.Drawing.Point(130, 25);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.Size = new System.Drawing.Size(250, 25);
            this.txtTimKiem.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(30, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "Tìm kiếm:";
            // 
            // cboLoaiHang
            // 
            this.cboLoaiHang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLoaiHang.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboLoaiHang.FormattingEnabled = true;
            this.cboLoaiHang.Items.AddRange(new object[] {
            "Tất cả",
            "Đồ uống",
            "Thức ăn",
            "Dụng cụ",
            "Khác"});
            this.cboLoaiHang.Location = new System.Drawing.Point(130, 70);
            this.cboLoaiHang.Name = "cboLoaiHang";
            this.cboLoaiHang.Size = new System.Drawing.Size(180, 25);
            this.cboLoaiHang.TabIndex = 1;
            // 
            // lblLoaiHang
            // 
            this.lblLoaiHang.AutoSize = true;
            this.lblLoaiHang.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblLoaiHang.Location = new System.Drawing.Point(30, 75);
            this.lblLoaiHang.Name = "lblLoaiHang";
            this.lblLoaiHang.Size = new System.Drawing.Size(78, 19);
            this.lblLoaiHang.TabIndex = 0;
            this.lblLoaiHang.Text = "Loại hàng:";
            // 
            // cboTrangThai
            // 
            this.cboTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTrangThai.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboTrangThai.FormattingEnabled = true;
            this.cboTrangThai.Items.AddRange(new object[] {
            "Tất cả",
            "Còn hàng",
            "Sắp hết",
            "Hết hàng"});
            this.cboTrangThai.Location = new System.Drawing.Point(510, 70);
            this.cboTrangThai.Name = "cboTrangThai";
            this.cboTrangThai.Size = new System.Drawing.Size(180, 25);
            this.cboTrangThai.TabIndex = 9;
            // 
            // lblTrangThai
            // 
            this.lblTrangThai.AutoSize = true;
            this.lblTrangThai.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTrangThai.Location = new System.Drawing.Point(410, 75);
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.Size = new System.Drawing.Size(80, 19);
            this.lblTrangThai.TabIndex = 8;
            this.lblTrangThai.Text = "Trạng thái:";
            // 
            // panelStats
            // 
            this.panelStats.BackColor = System.Drawing.Color.White;
            this.panelStats.Controls.Add(this.panelHetHang);
            this.panelStats.Controls.Add(this.panelSapHet);
            this.panelStats.Controls.Add(this.panelConHang);
            this.panelStats.Controls.Add(this.panelTongMH);
            this.panelStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelStats.Location = new System.Drawing.Point(0, 120);
            this.panelStats.Name = "panelStats";
            this.panelStats.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.panelStats.Size = new System.Drawing.Size(950, 100);
            this.panelStats.TabIndex = 1;
            // 
            // panelTongMH
            // 
            this.panelTongMH.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.panelTongMH.Controls.Add(this.lblTongMHValue);
            this.panelTongMH.Controls.Add(this.lblTongMH);
            this.panelTongMH.Location = new System.Drawing.Point(30, 15);
            this.panelTongMH.Name = "panelTongMH";
            this.panelTongMH.Size = new System.Drawing.Size(200, 70);
            this.panelTongMH.TabIndex = 0;
            // 
            // lblTongMHValue
            // 
            this.lblTongMHValue.AutoSize = true;
            this.lblTongMHValue.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTongMHValue.ForeColor = System.Drawing.Color.White;
            this.lblTongMHValue.Location = new System.Drawing.Point(15, 30);
            this.lblTongMHValue.Name = "lblTongMHValue";
            this.lblTongMHValue.Size = new System.Drawing.Size(28, 32);
            this.lblTongMHValue.TabIndex = 1;
            this.lblTongMHValue.Text = "0";
            // 
            // lblTongMH
            // 
            this.lblTongMH.AutoSize = true;
            this.lblTongMH.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTongMH.ForeColor = System.Drawing.Color.White;
            this.lblTongMH.Location = new System.Drawing.Point(15, 8);
            this.lblTongMH.Name = "lblTongMH";
            this.lblTongMH.Size = new System.Drawing.Size(117, 19);
            this.lblTongMH.TabIndex = 0;
            this.lblTongMH.Text = "Tổng mặt hàng";
            // 
            // panelConHang
            // 
            this.panelConHang.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.panelConHang.Controls.Add(this.lblConHangValue);
            this.panelConHang.Controls.Add(this.lblConHang);
            this.panelConHang.Location = new System.Drawing.Point(260, 15);
            this.panelConHang.Name = "panelConHang";
            this.panelConHang.Size = new System.Drawing.Size(200, 70);
            this.panelConHang.TabIndex = 1;
            // 
            // lblConHangValue
            // 
            this.lblConHangValue.AutoSize = true;
            this.lblConHangValue.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblConHangValue.ForeColor = System.Drawing.Color.White;
            this.lblConHangValue.Location = new System.Drawing.Point(15, 30);
            this.lblConHangValue.Name = "lblConHangValue";
            this.lblConHangValue.Size = new System.Drawing.Size(28, 32);
            this.lblConHangValue.TabIndex = 1;
            this.lblConHangValue.Text = "0";
            // 
            // lblConHang
            // 
            this.lblConHang.AutoSize = true;
            this.lblConHang.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblConHang.ForeColor = System.Drawing.Color.White;
            this.lblConHang.Location = new System.Drawing.Point(15, 8);
            this.lblConHang.Name = "lblConHang";
            this.lblConHang.Size = new System.Drawing.Size(73, 19);
            this.lblConHang.TabIndex = 0;
            this.lblConHang.Text = "Còn hàng";
            // 
            // panelSapHet
            // 
            this.panelSapHet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(196)))), ((int)(((byte)(15)))));
            this.panelSapHet.Controls.Add(this.lblSapHetValue);
            this.panelSapHet.Controls.Add(this.lblSapHet);
            this.panelSapHet.Location = new System.Drawing.Point(490, 15);
            this.panelSapHet.Name = "panelSapHet";
            this.panelSapHet.Size = new System.Drawing.Size(200, 70);
            this.panelSapHet.TabIndex = 2;
            // 
            // lblSapHetValue
            // 
            this.lblSapHetValue.AutoSize = true;
            this.lblSapHetValue.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblSapHetValue.ForeColor = System.Drawing.Color.White;
            this.lblSapHetValue.Location = new System.Drawing.Point(15, 30);
            this.lblSapHetValue.Name = "lblSapHetValue";
            this.lblSapHetValue.Size = new System.Drawing.Size(28, 32);
            this.lblSapHetValue.TabIndex = 1;
            this.lblSapHetValue.Text = "0";
            // 
            // lblSapHet
            // 
            this.lblSapHet.AutoSize = true;
            this.lblSapHet.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSapHet.ForeColor = System.Drawing.Color.White;
            this.lblSapHet.Location = new System.Drawing.Point(15, 8);
            this.lblSapHet.Name = "lblSapHet";
            this.lblSapHet.Size = new System.Drawing.Size(61, 19);
            this.lblSapHet.TabIndex = 0;
            this.lblSapHet.Text = "Sắp hết";
            // 
            // panelHetHang
            // 
            this.panelHetHang.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.panelHetHang.Controls.Add(this.lblHetHangValue);
            this.panelHetHang.Controls.Add(this.lblHetHang);
            this.panelHetHang.Location = new System.Drawing.Point(720, 15);
            this.panelHetHang.Name = "panelHetHang";
            this.panelHetHang.Size = new System.Drawing.Size(200, 70);
            this.panelHetHang.TabIndex = 3;
            // 
            // lblHetHangValue
            // 
            this.lblHetHangValue.AutoSize = true;
            this.lblHetHangValue.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblHetHangValue.ForeColor = System.Drawing.Color.White;
            this.lblHetHangValue.Location = new System.Drawing.Point(15, 30);
            this.lblHetHangValue.Name = "lblHetHangValue";
            this.lblHetHangValue.Size = new System.Drawing.Size(28, 32);
            this.lblHetHangValue.TabIndex = 1;
            this.lblHetHangValue.Text = "0";
            // 
            // lblHetHang
            // 
            this.lblHetHang.AutoSize = true;
            this.lblHetHang.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblHetHang.ForeColor = System.Drawing.Color.White;
            this.lblHetHang.Location = new System.Drawing.Point(15, 8);
            this.lblHetHang.Name = "lblHetHang";
            this.lblHetHang.Size = new System.Drawing.Size(71, 19);
            this.lblHetHang.TabIndex = 0;
            this.lblHetHang.Text = "Hết hàng";
            // 
            // dgvMatHang
            // 
            this.dgvMatHang.AllowUserToAddRows = false;
            this.dgvMatHang.AllowUserToDeleteRows = false;
            this.dgvMatHang.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMatHang.BackgroundColor = System.Drawing.Color.White;
            this.dgvMatHang.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMatHang.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaHang,
            this.colTenHang,
            this.colLoai,
            this.colDonVi,
            this.colSoLuongTon,
            this.colNguongCanhBao,
            this.colGia,
            this.colTrangThai,
            this.colNgayNhapGanNhat});
            this.dgvMatHang.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMatHang.Location = new System.Drawing.Point(0, 220);
            this.dgvMatHang.Name = "dgvMatHang";
            this.dgvMatHang.ReadOnly = true;
            this.dgvMatHang.RowHeadersVisible = false;
            this.dgvMatHang.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMatHang.Size = new System.Drawing.Size(950, 410);
            this.dgvMatHang.TabIndex = 2;
            // 
            // colMaHang
            // 
            this.colMaHang.FillWeight = 60F;
            this.colMaHang.HeaderText = "Mã";
            this.colMaHang.Name = "colMaHang";
            this.colMaHang.ReadOnly = true;
            // 
            // colTenHang
            // 
            this.colTenHang.FillWeight = 150F;
            this.colTenHang.HeaderText = "Tên mặt hàng";
            this.colTenHang.Name = "colTenHang";
            this.colTenHang.ReadOnly = true;
            // 
            // colLoai
            // 
            this.colLoai.FillWeight = 80F;
            this.colLoai.HeaderText = "Loại";
            this.colLoai.Name = "colLoai";
            this.colLoai.ReadOnly = true;
            // 
            // colDonVi
            // 
            this.colDonVi.FillWeight = 60F;
            this.colDonVi.HeaderText = "Đơn vị";
            this.colDonVi.Name = "colDonVi";
            this.colDonVi.ReadOnly = true;
            // 
            // colSoLuongTon
            // 
            this.colSoLuongTon.FillWeight = 80F;
            this.colSoLuongTon.HeaderText = "SL tồn";
            this.colSoLuongTon.Name = "colSoLuongTon";
            this.colSoLuongTon.ReadOnly = true;
            // 
            // colNguongCanhBao
            // 
            this.colNguongCanhBao.FillWeight = 80F;
            this.colNguongCanhBao.HeaderText = "Ngưỡng CB";
            this.colNguongCanhBao.Name = "colNguongCanhBao";
            this.colNguongCanhBao.ReadOnly = true;
            // 
            // colGia
            // 
            this.colGia.FillWeight = 90F;
            this.colGia.HeaderText = "Giá";
            this.colGia.Name = "colGia";
            this.colGia.ReadOnly = true;
            // 
            // colTrangThai
            // 
            this.colTrangThai.FillWeight = 80F;
            this.colTrangThai.HeaderText = "Trạng thái";
            this.colTrangThai.Name = "colTrangThai";
            this.colTrangThai.ReadOnly = true;
            // 
            // colNgayNhapGanNhat
            // 
            this.colNgayNhapGanNhat.FillWeight = 100F;
            this.colNgayNhapGanNhat.HeaderText = "Nhập gần nhất";
            this.colNgayNhapGanNhat.Name = "colNgayNhapGanNhat";
            this.colNgayNhapGanNhat.ReadOnly = true;
            // 
            // ucKiemSoatKho
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvMatHang);
            this.Controls.Add(this.panelStats);
            this.Controls.Add(this.panelTop);
            this.Name = "ucKiemSoatKho";
            this.Size = new System.Drawing.Size(950, 630);
            this.Load += new System.EventHandler(this.ucKiemSoatKho_Load);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelStats.ResumeLayout(false);
            this.panelTongMH.ResumeLayout(false);
            this.panelTongMH.PerformLayout();
            this.panelConHang.ResumeLayout(false);
            this.panelConHang.PerformLayout();
            this.panelSapHet.ResumeLayout(false);
            this.panelSapHet.PerformLayout();
            this.panelHetHang.ResumeLayout(false);
            this.panelHetHang.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMatHang)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.ComboBox cboLoaiHang;
        private System.Windows.Forms.Label lblLoaiHang;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.Button btnTimKiem;
        private System.Windows.Forms.ComboBox cboTrangThai;
        private System.Windows.Forms.Label lblTrangThai;
        private System.Windows.Forms.Panel panelStats;
        private System.Windows.Forms.Panel panelTongMH;
        private System.Windows.Forms.Label lblTongMHValue;
        private System.Windows.Forms.Label lblTongMH;
        private System.Windows.Forms.Panel panelConHang;
        private System.Windows.Forms.Label lblConHangValue;
        private System.Windows.Forms.Label lblConHang;
        private System.Windows.Forms.Panel panelSapHet;
        private System.Windows.Forms.Label lblSapHetValue;
        private System.Windows.Forms.Label lblSapHet;
        private System.Windows.Forms.Panel panelHetHang;
        private System.Windows.Forms.Label lblHetHangValue;
        private System.Windows.Forms.Label lblHetHang;
        private System.Windows.Forms.DataGridView dgvMatHang;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaHang;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenHang;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLoai;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDonVi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoLuongTon;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNguongCanhBao;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrangThai;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNgayNhapGanNhat;
    }
}