namespace Billiard.WinForm.Forms.KhachHang
{
    partial class KhachHangForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlFilters = new Panel();
            btnThungRac = new Button();
            btnThem = new Button();
            btnXuatBaoCao = new Button();
            txtSearch = new TextBox();
            pnlTrangThaiFilters = new FlowLayoutPanel();
            btnTatCa = new Button();
            btnDong = new Button();
            btnBac = new Button();
            btnVang = new Button();
            btnBachKim = new Button();
            lblTrangThai = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            pnlFilters.SuspendLayout();
            pnlTrangThaiFilters.SuspendLayout();
            SuspendLayout();
            // 
            // pnlFilters
            // 
            pnlFilters.Controls.Add(btnThungRac);
            pnlFilters.Controls.Add(btnThem);
            pnlFilters.Controls.Add(btnXuatBaoCao);
            pnlFilters.Controls.Add(txtSearch);
            pnlFilters.Controls.Add(pnlTrangThaiFilters);
            pnlFilters.Controls.Add(lblTrangThai);
            pnlFilters.Dock = DockStyle.Top;
            pnlFilters.Location = new Point(0, 0);
            pnlFilters.Name = "pnlFilters";
            pnlFilters.Size = new Size(800, 127);
            pnlFilters.TabIndex = 3;
            // 
            // btnThungRac
            // 
            btnThungRac.BackColor = Color.FromArgb(100, 116, 139);
            btnThungRac.Cursor = Cursors.Hand;
            btnThungRac.FlatAppearance.BorderSize = 0;
            btnThungRac.FlatStyle = FlatStyle.Flat;
            btnThungRac.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnThungRac.ForeColor = Color.White;
            btnThungRac.Location = new Point(609, 67);
            btnThungRac.Margin = new Padding(3, 4, 3, 4);
            btnThungRac.Name = "btnThungRac";
            btnThungRac.Size = new Size(160, 34);
            btnThungRac.TabIndex = 14;
            btnThungRac.Text = "Đã xóa";
            btnThungRac.UseVisualStyleBackColor = false;
            // 
            // btnThem
            // 
            btnThem.BackColor = Color.FromArgb(234, 179, 8);
            btnThem.Cursor = Cursors.Hand;
            btnThem.FlatAppearance.BorderSize = 0;
            btnThem.FlatStyle = FlatStyle.Flat;
            btnThem.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnThem.ForeColor = Color.White;
            btnThem.Location = new Point(414, 67);
            btnThem.Margin = new Padding(3, 4, 3, 4);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(187, 34);
            btnThem.TabIndex = 13;
            btnThem.Text = "➕ Thêm thành viên";
            btnThem.UseVisualStyleBackColor = false;
            // 
            // btnXuatBaoCao
            // 
            btnXuatBaoCao.BackColor = Color.FromArgb(34, 197, 94);
            btnXuatBaoCao.Cursor = Cursors.Hand;
            btnXuatBaoCao.FlatAppearance.BorderSize = 0;
            btnXuatBaoCao.FlatStyle = FlatStyle.Flat;
            btnXuatBaoCao.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnXuatBaoCao.ForeColor = Color.White;
            btnXuatBaoCao.Location = new Point(607, 14);
            btnXuatBaoCao.Margin = new Padding(3, 4, 3, 4);
            btnXuatBaoCao.Name = "btnXuatBaoCao";
            btnXuatBaoCao.Size = new Size(162, 31);
            btnXuatBaoCao.TabIndex = 4;
            btnXuatBaoCao.Text = "Xuất báo cáo";
            btnXuatBaoCao.UseVisualStyleBackColor = false;
            btnXuatBaoCao.Click += btnXuatBaoCao_Click;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 11F);
            txtSearch.Location = new Point(12, 67);
            txtSearch.Margin = new Padding(3, 4, 3, 4);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "🔍 Tìm kiếm thành viên...(SĐT, Tên)";
            txtSearch.Size = new Size(374, 32);
            txtSearch.TabIndex = 7;
            // 
            // pnlTrangThaiFilters
            // 
            pnlTrangThaiFilters.Controls.Add(btnTatCa);
            pnlTrangThaiFilters.Controls.Add(btnDong);
            pnlTrangThaiFilters.Controls.Add(btnBac);
            pnlTrangThaiFilters.Controls.Add(btnVang);
            pnlTrangThaiFilters.Controls.Add(btnBachKim);
            pnlTrangThaiFilters.Location = new Point(76, 12);
            pnlTrangThaiFilters.Margin = new Padding(3, 4, 3, 4);
            pnlTrangThaiFilters.Name = "pnlTrangThaiFilters";
            pnlTrangThaiFilters.Size = new Size(525, 47);
            pnlTrangThaiFilters.TabIndex = 4;
            // 
            // btnTatCa
            // 
            btnTatCa.BackColor = Color.FromArgb(99, 102, 241);
            btnTatCa.Cursor = Cursors.Hand;
            btnTatCa.FlatAppearance.BorderSize = 0;
            btnTatCa.FlatStyle = FlatStyle.Flat;
            btnTatCa.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTatCa.ForeColor = Color.White;
            btnTatCa.Location = new Point(3, 4);
            btnTatCa.Margin = new Padding(3, 4, 3, 4);
            btnTatCa.Name = "btnTatCa";
            btnTatCa.Size = new Size(87, 37);
            btnTatCa.TabIndex = 0;
            btnTatCa.Tag = "Tất cả";
            btnTatCa.Text = "Tất cả";
            btnTatCa.UseVisualStyleBackColor = false;
            // 
            // btnDong
            // 
            btnDong.BackColor = Color.FromArgb(226, 232, 240);
            btnDong.Cursor = Cursors.Hand;
            btnDong.FlatAppearance.BorderSize = 0;
            btnDong.FlatStyle = FlatStyle.Flat;
            btnDong.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDong.ForeColor = Color.FromArgb(51, 65, 85);
            btnDong.Location = new Point(96, 4);
            btnDong.Margin = new Padding(3, 4, 3, 4);
            btnDong.Name = "btnDong";
            btnDong.Size = new Size(68, 37);
            btnDong.TabIndex = 2;
            btnDong.Tag = "Đồng";
            btnDong.Text = "Đồng";
            btnDong.UseVisualStyleBackColor = false;
            // 
            // btnBac
            // 
            btnBac.BackColor = Color.FromArgb(226, 232, 240);
            btnBac.Cursor = Cursors.Hand;
            btnBac.FlatAppearance.BorderSize = 0;
            btnBac.FlatStyle = FlatStyle.Flat;
            btnBac.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnBac.ForeColor = Color.FromArgb(51, 65, 85);
            btnBac.Location = new Point(170, 4);
            btnBac.Margin = new Padding(3, 4, 3, 4);
            btnBac.Name = "btnBac";
            btnBac.Size = new Size(90, 37);
            btnBac.TabIndex = 3;
            btnBac.Tag = "Bạc";
            btnBac.Text = "Bạc";
            btnBac.UseVisualStyleBackColor = false;
            // 
            // btnVang
            // 
            btnVang.BackColor = Color.FromArgb(226, 232, 240);
            btnVang.Cursor = Cursors.Hand;
            btnVang.FlatAppearance.BorderSize = 0;
            btnVang.FlatStyle = FlatStyle.Flat;
            btnVang.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnVang.ForeColor = Color.FromArgb(51, 65, 85);
            btnVang.Location = new Point(266, 4);
            btnVang.Margin = new Padding(3, 4, 3, 4);
            btnVang.Name = "btnVang";
            btnVang.Size = new Size(117, 37);
            btnVang.TabIndex = 4;
            btnVang.Tag = "Vàng";
            btnVang.Text = "Vàng";
            btnVang.UseVisualStyleBackColor = false;
            // 
            // btnBachKim
            // 
            btnBachKim.BackColor = Color.FromArgb(226, 232, 240);
            btnBachKim.Cursor = Cursors.Hand;
            btnBachKim.FlatAppearance.BorderSize = 0;
            btnBachKim.FlatStyle = FlatStyle.Flat;
            btnBachKim.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnBachKim.ForeColor = Color.FromArgb(51, 65, 85);
            btnBachKim.Location = new Point(389, 4);
            btnBachKim.Margin = new Padding(3, 4, 3, 4);
            btnBachKim.Name = "btnBachKim";
            btnBachKim.Size = new Size(121, 37);
            btnBachKim.TabIndex = 5;
            btnBachKim.Tag = "Bạch Kim";
            btnBachKim.Text = "Bạch Kim";
            btnBachKim.UseVisualStyleBackColor = false;
            // 
            // lblTrangThai
            // 
            lblTrangThai.AutoSize = true;
            lblTrangThai.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTrangThai.Location = new Point(12, 21);
            lblTrangThai.Name = "lblTrangThai";
            lblTrangThai.Size = new Size(58, 23);
            lblTrangThai.TabIndex = 4;
            lblTrangThai.Text = "Hạng:";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.BackColor = Color.White;
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 127);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(20, 0, 0, 0);
            flowLayoutPanel1.Size = new Size(800, 323);
            flowLayoutPanel1.TabIndex = 4;
            // 
            // KhachHangForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(pnlFilters);
            Name = "KhachHangForm";
            Text = "KhachHangForm";
            pnlFilters.ResumeLayout(false);
            pnlFilters.PerformLayout();
            pnlTrangThaiFilters.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Panel pnlFilters;
        private DateTimePicker dtpDenNgay;
        private Label label1;
        private DateTimePicker dtpTuNgay;
        private Button btnXuatBaoCao;
        private Label lblThoiGian;
        private TextBox txtSearch;
        private FlowLayoutPanel pnlTrangThaiFilters;
        private Button btnTatCa;
        private Button btnChuaThanhToan;
        private Button btnDaThanhToan;
        private Label lblTrangThai;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button btnThem;
        private Button btnDong;
        private Button button1;
        private Button btnBac;
        private Button btnVang;
        private Button btnBachKim;
        private Button btnThungRac;
    }
}