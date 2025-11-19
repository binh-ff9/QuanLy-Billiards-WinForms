namespace Billiard.WinForm.Forms.QLBan
{
    partial class DanhSachBanDatForm
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            pnlHeader = new Panel();
            lblTitle = new Label();
            dgvDatBan = new DataGridView();
            MaDat = new DataGridViewTextBoxColumn();
            TenKhach = new DataGridViewTextBoxColumn();
            Sdt = new DataGridViewTextBoxColumn();
            TenBan = new DataGridViewTextBoxColumn();
            LoaiBan = new DataGridViewTextBoxColumn();
            KhuVuc = new DataGridViewTextBoxColumn();
            ThoiGianBatDau = new DataGridViewTextBoxColumn();
            ThoiGianKetThuc = new DataGridViewTextBoxColumn();
            TrangThai = new DataGridViewTextBoxColumn();
            NgayTao = new DataGridViewTextBoxColumn();
            GhiChu = new DataGridViewTextBoxColumn();
            Actions = new DataGridViewButtonColumn();
            Cancel = new DataGridViewButtonColumn();
            pnlFilter = new Panel();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDatBan).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(99, 102, 241);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(10);
            pnlHeader.Size = new Size(1473, 60);
            pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(10, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(350, 45);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "📅 Danh sách bàn đặt";
            // 
            // dgvDatBan
            // 
            dgvDatBan.AllowUserToAddRows = false;
            dgvDatBan.AllowUserToDeleteRows = false;
            dgvDatBan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDatBan.BackgroundColor = Color.White;
            dgvDatBan.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDatBan.Columns.AddRange(new DataGridViewColumn[] { MaDat, TenKhach, Sdt, TenBan, LoaiBan, KhuVuc, ThoiGianBatDau, ThoiGianKetThuc, TrangThai, NgayTao, GhiChu, Actions, Cancel });
            dgvDatBan.Dock = DockStyle.Fill;
            dgvDatBan.Location = new Point(0, 70);
            dgvDatBan.MultiSelect = false;
            dgvDatBan.Name = "dgvDatBan";
            dgvDatBan.ReadOnly = true;
            dgvDatBan.RowHeadersWidth = 62;
            dgvDatBan.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDatBan.Size = new Size(1473, 730);
            dgvDatBan.TabIndex = 1;
            dgvDatBan.CellContentClick += dgvDatBan_CellContentClick;
            // 
            // MaDat
            // 
            MaDat.DataPropertyName = "MaDat";
            MaDat.HeaderText = "Mã Đặt";
            MaDat.MinimumWidth = 8;
            MaDat.Name = "MaDat";
            MaDat.ReadOnly = true;
            MaDat.Visible = false;
            // 
            // TenKhach
            // 
            TenKhach.DataPropertyName = "TenKhach";
            TenKhach.HeaderText = "Tên Khách";
            TenKhach.MinimumWidth = 8;
            TenKhach.Name = "TenKhach";
            TenKhach.ReadOnly = true;
            // 
            // Sdt
            // 
            Sdt.DataPropertyName = "Sdt";
            Sdt.HeaderText = "SĐT";
            Sdt.MinimumWidth = 8;
            Sdt.Name = "Sdt";
            Sdt.ReadOnly = true;
            // 
            // TenBan
            // 
            TenBan.DataPropertyName = "TenBan";
            TenBan.HeaderText = "Bàn";
            TenBan.MinimumWidth = 8;
            TenBan.Name = "TenBan";
            TenBan.ReadOnly = true;
            // 
            // LoaiBan
            // 
            LoaiBan.DataPropertyName = "LoaiBan";
            LoaiBan.HeaderText = "Loại bàn";
            LoaiBan.MinimumWidth = 8;
            LoaiBan.Name = "LoaiBan";
            LoaiBan.ReadOnly = true;
            // 
            // KhuVuc
            // 
            KhuVuc.DataPropertyName = "KhuVuc";
            KhuVuc.HeaderText = "Khu vực";
            KhuVuc.MinimumWidth = 8;
            KhuVuc.Name = "KhuVuc";
            KhuVuc.ReadOnly = true;
            // 
            // ThoiGianBatDau
            // 
            ThoiGianBatDau.DataPropertyName = "ThoiGianBatDau";
            dataGridViewCellStyle1.Format = "HH:mm dd/MM";
            ThoiGianBatDau.DefaultCellStyle = dataGridViewCellStyle1;
            ThoiGianBatDau.HeaderText = "Bắt đầu";
            ThoiGianBatDau.MinimumWidth = 8;
            ThoiGianBatDau.Name = "ThoiGianBatDau";
            ThoiGianBatDau.ReadOnly = true;
            // 
            // ThoiGianKetThuc
            // 
            ThoiGianKetThuc.DataPropertyName = "ThoiGianKetThuc";
            dataGridViewCellStyle2.Format = "HH:mm dd/MM";
            ThoiGianKetThuc.DefaultCellStyle = dataGridViewCellStyle2;
            ThoiGianKetThuc.HeaderText = "Kết thúc";
            ThoiGianKetThuc.MinimumWidth = 8;
            ThoiGianKetThuc.Name = "ThoiGianKetThuc";
            ThoiGianKetThuc.ReadOnly = true;
            // 
            // TrangThai
            // 
            TrangThai.DataPropertyName = "TrangThai";
            TrangThai.HeaderText = "Trạng thái";
            TrangThai.MinimumWidth = 8;
            TrangThai.Name = "TrangThai";
            TrangThai.ReadOnly = true;
            // 
            // NgayTao
            // 
            NgayTao.DataPropertyName = "NgayTao";
            NgayTao.HeaderText = "Ngày tạo";
            NgayTao.MinimumWidth = 8;
            NgayTao.Name = "NgayTao";
            NgayTao.ReadOnly = true;
            NgayTao.Visible = false;
            // 
            // GhiChu
            // 
            GhiChu.DataPropertyName = "GhiChu";
            GhiChu.HeaderText = "Ghi chú";
            GhiChu.MinimumWidth = 8;
            GhiChu.Name = "GhiChu";
            GhiChu.ReadOnly = true;
            // 
            // Actions
            // 
            Actions.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Actions.FlatStyle = FlatStyle.Flat;
            Actions.HeaderText = "Thao tác";
            Actions.MinimumWidth = 8;
            Actions.Name = "Actions";
            Actions.ReadOnly = true;
            Actions.Text = "✓ Xác nhận";
            Actions.UseColumnTextForButtonValue = true;
            // 
            // Cancel
            // 
            Cancel.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Cancel.FlatStyle = FlatStyle.Flat;
            Cancel.HeaderText = "Hủy";
            Cancel.MinimumWidth = 8;
            Cancel.Name = "Cancel";
            Cancel.ReadOnly = true;
            Cancel.Text = "✕ Hủy đặt";
            Cancel.UseColumnTextForButtonValue = true;
            Cancel.Width = 150;
            // 
            // pnlFilter
            // 
            pnlFilter.BackColor = Color.FromArgb(248, 250, 252);
            pnlFilter.Dock = DockStyle.Top;
            pnlFilter.Location = new Point(0, 60);
            pnlFilter.Name = "pnlFilter";
            pnlFilter.Size = new Size(1473, 10);
            pnlFilter.TabIndex = 2;
            // 
            // DanhSachBanDatForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1473, 800);
            Controls.Add(dgvDatBan);
            Controls.Add(pnlFilter);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 9F);
            Name = "DanhSachBanDatForm";
            Text = "Danh sách bàn đặt";
            Load += DanhSachBanDatForm_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDatBan).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DataGridView dgvDatBan;
        private System.Windows.Forms.Panel pnlFilter;
        private DataGridViewTextBoxColumn MaDat;
        private DataGridViewTextBoxColumn TenKhach;
        private DataGridViewTextBoxColumn Sdt;
        private DataGridViewTextBoxColumn TenBan;
        private DataGridViewTextBoxColumn LoaiBan;
        private DataGridViewTextBoxColumn KhuVuc;
        private DataGridViewTextBoxColumn ThoiGianBatDau;
        private DataGridViewTextBoxColumn ThoiGianKetThuc;
        private DataGridViewTextBoxColumn TrangThai;
        private DataGridViewTextBoxColumn NgayTao;
        private DataGridViewTextBoxColumn GhiChu;
        private DataGridViewButtonColumn Actions;
        private DataGridViewButtonColumn Cancel;
    }
}