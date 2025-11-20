namespace Billiard.WinForm.Forms.HoaDon
{
    partial class ChiTietHoaDonControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblMaHD = new Label();
            lblGioVao = new Label();
            lblBan = new Label();
            lblGioRa = new Label();
            lblTongTien = new Label();
            dgvChiTiet = new DataGridView();
            btnInHoaDon = new Button();
            lblTrangThai = new Label();
            lblGiaGio = new Label();
            pnlThongTin = new Panel();
            lblKhachHang = new Label();
            panel1 = new Panel();
            ((System.ComponentModel.ISupportInitialize)dgvChiTiet).BeginInit();
            pnlThongTin.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // lblMaHD
            // 
            lblMaHD.AutoSize = true;
            lblMaHD.Location = new Point(18, 1);
            lblMaHD.Name = "lblMaHD";
            lblMaHD.Size = new Size(67, 20);
            lblMaHD.TabIndex = 1;
            lblMaHD.Text = "Hóa đơn";
            // 
            // lblGioVao
            // 
            lblGioVao.AutoSize = true;
            lblGioVao.Location = new Point(18, 91);
            lblGioVao.Name = "lblGioVao";
            lblGioVao.Size = new Size(34, 20);
            lblGioVao.TabIndex = 2;
            lblGioVao.Text = "Vào";
            // 
            // lblBan
            // 
            lblBan.AutoSize = true;
            lblBan.Location = new Point(18, 31);
            lblBan.Name = "lblBan";
            lblBan.Size = new Size(34, 20);
            lblBan.TabIndex = 3;
            lblBan.Text = "Bàn";
            // 
            // lblGioRa
            // 
            lblGioRa.AutoSize = true;
            lblGioRa.Location = new Point(18, 128);
            lblGioRa.Name = "lblGioRa";
            lblGioRa.Size = new Size(26, 20);
            lblGioRa.TabIndex = 4;
            lblGioRa.Text = "Ra";
            // 
            // lblTongTien
            // 
            lblTongTien.AutoSize = true;
            lblTongTien.Location = new Point(18, 21);
            lblTongTien.Name = "lblTongTien";
            lblTongTien.Size = new Size(72, 20);
            lblTongTien.TabIndex = 5;
            lblTongTien.Text = "Tổng tiền";
            // 
            // dgvChiTiet
            // 
            dgvChiTiet.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvChiTiet.Dock = DockStyle.Fill;
            dgvChiTiet.Location = new Point(0, 204);
            dgvChiTiet.Name = "dgvChiTiet";
            dgvChiTiet.RowHeadersWidth = 51;
            dgvChiTiet.Size = new Size(450, 343);
            dgvChiTiet.TabIndex = 6;
            // 
            // btnInHoaDon
            // 
            btnInHoaDon.BackColor = Color.FromArgb(59, 130, 246);
            btnInHoaDon.Cursor = Cursors.Hand;
            btnInHoaDon.FlatAppearance.BorderSize = 0;
            btnInHoaDon.FlatStyle = FlatStyle.Flat;
            btnInHoaDon.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnInHoaDon.ForeColor = Color.White;
            btnInHoaDon.Location = new Point(12, 63);
            btnInHoaDon.Margin = new Padding(3, 4, 3, 4);
            btnInHoaDon.Name = "btnInHoaDon";
            btnInHoaDon.Size = new Size(171, 37);
            btnInHoaDon.TabIndex = 7;
            btnInHoaDon.Text = "In hóa đơn";
            btnInHoaDon.UseVisualStyleBackColor = false;
            btnInHoaDon.Click += btnInHoaDon_Click;
            // 
            // lblTrangThai
            // 
            lblTrangThai.AutoSize = true;
            lblTrangThai.Location = new Point(217, 5);
            lblTrangThai.Name = "lblTrangThai";
            lblTrangThai.Size = new Size(103, 20);
            lblTrangThai.TabIndex = 8;
            lblTrangThai.Text = "Đã thanh toán";
            // 
            // lblGiaGio
            // 
            lblGiaGio.AutoSize = true;
            lblGiaGio.Location = new Point(18, 171);
            lblGiaGio.Name = "lblGiaGio";
            lblGiaGio.Size = new Size(57, 20);
            lblGiaGio.TabIndex = 9;
            lblGiaGio.Text = "Giá giờ";
            // 
            // pnlThongTin
            // 
            pnlThongTin.Controls.Add(lblTrangThai);
            pnlThongTin.Controls.Add(lblKhachHang);
            pnlThongTin.Controls.Add(lblGiaGio);
            pnlThongTin.Controls.Add(lblMaHD);
            pnlThongTin.Controls.Add(lblBan);
            pnlThongTin.Controls.Add(lblGioVao);
            pnlThongTin.Controls.Add(lblGioRa);
            pnlThongTin.Dock = DockStyle.Top;
            pnlThongTin.Location = new Point(0, 0);
            pnlThongTin.Name = "pnlThongTin";
            pnlThongTin.Size = new Size(450, 204);
            pnlThongTin.TabIndex = 10;
            // 
            // lblKhachHang
            // 
            lblKhachHang.AutoSize = true;
            lblKhachHang.Location = new Point(18, 58);
            lblKhachHang.Name = "lblKhachHang";
            lblKhachHang.Size = new Size(86, 20);
            lblKhachHang.TabIndex = 11;
            lblKhachHang.Text = "Khách hàng";
            // 
            // panel1
            // 
            panel1.Controls.Add(lblTongTien);
            panel1.Controls.Add(btnInHoaDon);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 304);
            panel1.Name = "panel1";
            panel1.Size = new Size(450, 243);
            panel1.TabIndex = 11;
            // 
            // ChiTietHoaDonControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel1);
            Controls.Add(dgvChiTiet);
            Controls.Add(pnlThongTin);
            Name = "ChiTietHoaDonControl";
            Size = new Size(450, 547);
            ((System.ComponentModel.ISupportInitialize)dgvChiTiet).EndInit();
            pnlThongTin.ResumeLayout(false);
            pnlThongTin.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label lblMaHD;
        private Label lblGioVao;
        private Label lblBan;
        private Label lblGioRa;
        private Label lblTongTien;
        private DataGridView dgvChiTiet;
        private Button btnInHoaDon;
        private Label lblTrangThai;
        private Label lblGiaGio;
        private Panel pnlThongTin;
        private Label lblKhachHang;
        private Panel panel1;
    }
}
