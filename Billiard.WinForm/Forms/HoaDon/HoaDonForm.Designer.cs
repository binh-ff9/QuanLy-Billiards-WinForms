namespace Billiard.WinForm.Forms.HoaDon
{
    partial class HoaDonForm
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
            dataGridViewHoaDon = new DataGridView();
            pnlFilters = new Panel();
            dtpDenNgay = new DateTimePicker();
            label1 = new Label();
            dtpTuNgay = new DateTimePicker();
            btnXuatBaoCao = new Button();
            lblThoiGian = new Label();
            txtSearch = new TextBox();
            pnlTrangThaiFilters = new FlowLayoutPanel();
            btnTatCa = new Button();
            btnChuaThanhToan = new Button();
            btnDaThanhToan = new Button();
            lblTrangThai = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridViewHoaDon).BeginInit();
            pnlFilters.SuspendLayout();
            pnlTrangThaiFilters.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridViewHoaDon
            // 
            dataGridViewHoaDon.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewHoaDon.Dock = DockStyle.Fill;
            dataGridViewHoaDon.GridColor = SystemColors.InactiveCaption;
            dataGridViewHoaDon.Location = new Point(0, 133);
            dataGridViewHoaDon.Name = "dataGridViewHoaDon";
            dataGridViewHoaDon.RowHeadersWidth = 51;
            dataGridViewHoaDon.Size = new Size(800, 317);
            dataGridViewHoaDon.TabIndex = 0;
            // 
            // pnlFilters
            // 
            pnlFilters.Controls.Add(dtpDenNgay);
            pnlFilters.Controls.Add(label1);
            pnlFilters.Controls.Add(dtpTuNgay);
            pnlFilters.Controls.Add(btnXuatBaoCao);
            pnlFilters.Controls.Add(lblThoiGian);
            pnlFilters.Controls.Add(txtSearch);
            pnlFilters.Controls.Add(pnlTrangThaiFilters);
            pnlFilters.Controls.Add(lblTrangThai);
            pnlFilters.Dock = DockStyle.Top;
            pnlFilters.Location = new Point(0, 0);
            pnlFilters.Name = "pnlFilters";
            pnlFilters.Size = new Size(800, 133);
            pnlFilters.TabIndex = 1;
            // 
            // dtpDenNgay
            // 
            dtpDenNgay.Location = new Point(327, 58);
            dtpDenNgay.Name = "dtpDenNgay";
            dtpDenNgay.Size = new Size(170, 27);
            dtpDenNgay.TabIndex = 12;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label1.Location = new Point(285, 58);
            label1.Name = "label1";
            label1.Size = new Size(40, 23);
            label1.TabIndex = 11;
            label1.Text = "đến";
            // 
            // dtpTuNgay
            // 
            dtpTuNgay.Location = new Point(109, 58);
            dtpTuNgay.Name = "dtpTuNgay";
            dtpTuNgay.Size = new Size(170, 27);
            dtpTuNgay.TabIndex = 9;
            // 
            // btnXuatBaoCao
            // 
            btnXuatBaoCao.BackColor = Color.FromArgb(34, 197, 94);
            btnXuatBaoCao.Cursor = Cursors.Hand;
            btnXuatBaoCao.FlatAppearance.BorderSize = 0;
            btnXuatBaoCao.FlatStyle = FlatStyle.Flat;
            btnXuatBaoCao.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnXuatBaoCao.ForeColor = Color.White;
            btnXuatBaoCao.Location = new Point(553, 8);
            btnXuatBaoCao.Margin = new Padding(3, 4, 3, 4);
            btnXuatBaoCao.Name = "btnXuatBaoCao";
            btnXuatBaoCao.Size = new Size(171, 53);
            btnXuatBaoCao.TabIndex = 4;
            btnXuatBaoCao.Text = "➕ Xuất báo cáo";
            btnXuatBaoCao.UseVisualStyleBackColor = false;
            btnXuatBaoCao.Click += btnXuatBaoCao_Click;
            // 
            // lblThoiGian
            // 
            lblThoiGian.AutoSize = true;
            lblThoiGian.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblThoiGian.Location = new Point(12, 58);
            lblThoiGian.Name = "lblThoiGian";
            lblThoiGian.Size = new Size(91, 23);
            lblThoiGian.TabIndex = 8;
            lblThoiGian.Text = "Thời gian:";
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 11F);
            txtSearch.Location = new Point(12, 95);
            txtSearch.Margin = new Padding(3, 4, 3, 4);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "🔍 Tìm kiếm hóa đơn...(Tên Khách hàng, mã hóa đơn, số điện thoại)";
            txtSearch.Size = new Size(632, 32);
            txtSearch.TabIndex = 7;
            // 
            // pnlTrangThaiFilters
            // 
            pnlTrangThaiFilters.Controls.Add(btnTatCa);
            pnlTrangThaiFilters.Controls.Add(btnChuaThanhToan);
            pnlTrangThaiFilters.Controls.Add(btnDaThanhToan);
            pnlTrangThaiFilters.Location = new Point(115, 4);
            pnlTrangThaiFilters.Margin = new Padding(3, 4, 3, 4);
            pnlTrangThaiFilters.Name = "pnlTrangThaiFilters";
            pnlTrangThaiFilters.Size = new Size(382, 47);
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
            btnTatCa.Size = new Size(103, 37);
            btnTatCa.TabIndex = 0;
            btnTatCa.Tag = "all";
            btnTatCa.Text = "Tất cả";
            btnTatCa.UseVisualStyleBackColor = false;
            // 
            // btnChuaThanhToan
            // 
            btnChuaThanhToan.BackColor = Color.FromArgb(226, 232, 240);
            btnChuaThanhToan.Cursor = Cursors.Hand;
            btnChuaThanhToan.FlatAppearance.BorderSize = 0;
            btnChuaThanhToan.FlatStyle = FlatStyle.Flat;
            btnChuaThanhToan.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnChuaThanhToan.ForeColor = Color.FromArgb(51, 65, 85);
            btnChuaThanhToan.Location = new Point(112, 4);
            btnChuaThanhToan.Margin = new Padding(3, 4, 3, 4);
            btnChuaThanhToan.Name = "btnChuaThanhToan";
            btnChuaThanhToan.Size = new Size(137, 37);
            btnChuaThanhToan.TabIndex = 2;
            btnChuaThanhToan.Tag = "Chưa thanh toán";
            btnChuaThanhToan.Text = "Chưa thanh toán";
            btnChuaThanhToan.UseVisualStyleBackColor = false;
            // 
            // btnDaThanhToan
            // 
            btnDaThanhToan.BackColor = Color.FromArgb(226, 232, 240);
            btnDaThanhToan.Cursor = Cursors.Hand;
            btnDaThanhToan.FlatAppearance.BorderSize = 0;
            btnDaThanhToan.FlatStyle = FlatStyle.Flat;
            btnDaThanhToan.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDaThanhToan.ForeColor = Color.FromArgb(51, 65, 85);
            btnDaThanhToan.Location = new Point(255, 4);
            btnDaThanhToan.Margin = new Padding(3, 4, 3, 4);
            btnDaThanhToan.Name = "btnDaThanhToan";
            btnDaThanhToan.Size = new Size(121, 37);
            btnDaThanhToan.TabIndex = 3;
            btnDaThanhToan.Tag = "Đã thanh toán";
            btnDaThanhToan.Text = "Đã thanh toán";
            btnDaThanhToan.UseVisualStyleBackColor = false;
            // 
            // lblTrangThai
            // 
            lblTrangThai.AutoSize = true;
            lblTrangThai.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTrangThai.Location = new Point(12, 21);
            lblTrangThai.Name = "lblTrangThai";
            lblTrangThai.Size = new Size(97, 23);
            lblTrangThai.TabIndex = 4;
            lblTrangThai.Text = "Trạng thái:";
            // 
            // HoaDonForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dataGridViewHoaDon);
            Controls.Add(pnlFilters);
            Name = "HoaDonForm";
            Text = "HoaDonForm";
            Load += HoaDonForm_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewHoaDon).EndInit();
            pnlFilters.ResumeLayout(false);
            pnlFilters.PerformLayout();
            pnlTrangThaiFilters.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridViewHoaDon;
        private Panel pnlFilters;
        private FlowLayoutPanel pnlTrangThaiFilters;
        private Button btnTatCa;
        private Button btnChuaThanhToan;
        private Button btnDaThanhToan;
        private Label lblTrangThai;
        private TextBox txtSearch;
        private Label lblThoiGian;
        private DateTimePicker dtpDenNgay;
        private Label label1;
        private DateTimePicker dtpTuNgay;
        private Button btnXuatBaoCao;
    }
}