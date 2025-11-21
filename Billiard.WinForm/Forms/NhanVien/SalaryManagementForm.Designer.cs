namespace Billiard.WinForm.Forms.NhanVien
{
    partial class SalaryManagementForm
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            panelMain = new Panel();
            panelActions = new Panel();
            btnClose = new Button();
            dgvSalary = new DataGridView();
            colMaNV = new DataGridViewTextBoxColumn();
            colTenNV = new DataGridViewTextBoxColumn();
            colChucVu = new DataGridViewTextBoxColumn();
            colSoNgayLam = new DataGridViewTextBoxColumn();
            colTongGioLam = new DataGridViewTextBoxColumn();
            colLuongTheoGio = new DataGridViewTextBoxColumn();
            colLuongCoBan = new DataGridViewTextBoxColumn();
            colPhuCap = new DataGridViewTextBoxColumn();
            colThuong = new DataGridViewTextBoxColumn();
            colPhat = new DataGridViewTextBoxColumn();
            colTongLuong = new DataGridViewTextBoxColumn();
            colActions = new DataGridViewButtonColumn();
            panelStats = new Panel();
            cardSalary = new Panel();
            lblTotalSalary = new Label();
            lblTitleSalary = new Label();
            lblIconSalary = new Label();
            cardHours = new Panel();
            lblTotalHours = new Label();
            lblTitleHours = new Label();
            lblIconHours = new Label();
            cardEmployees = new Panel();
            lblTotalEmployees = new Label();
            lblTitleEmployees = new Label();
            lblIconEmployees = new Label();
            panelFilter = new Panel();
            btnExport = new Button();
            btnCalculateAll = new Button();
            btnRefresh = new Button();
            cboYear = new ComboBox();
            lblYear = new Label();
            cboMonth = new ComboBox();
            lblMonth = new Label();
            panelHeader = new Panel();
            lblTitle = new Label();
            panelMain.SuspendLayout();
            panelActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSalary).BeginInit();
            panelStats.SuspendLayout();
            cardSalary.SuspendLayout();
            cardHours.SuspendLayout();
            cardEmployees.SuspendLayout();
            panelFilter.SuspendLayout();
            panelHeader.SuspendLayout();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.AutoScroll = true;
            panelMain.Controls.Add(panelActions);
            panelMain.Controls.Add(dgvSalary);
            panelMain.Controls.Add(panelStats);
            panelMain.Controls.Add(panelFilter);
            panelMain.Controls.Add(panelHeader);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(0, 0);
            panelMain.Name = "panelMain";
            panelMain.Padding = new Padding(20);
            panelMain.Size = new Size(1483, 800);
            panelMain.TabIndex = 0;
            // 
            // panelActions
            // 
            panelActions.BackColor = Color.Transparent;
            panelActions.Controls.Add(btnClose);
            panelActions.Dock = DockStyle.Bottom;
            panelActions.Location = new Point(20, 720);
            panelActions.Name = "panelActions";
            panelActions.Size = new Size(1443, 60);
            panelActions.TabIndex = 4;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BackColor = Color.FromArgb(108, 117, 125);
            btnClose.Cursor = Cursors.Hand;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(1303, 8);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(120, 45);
            btnClose.TabIndex = 0;
            btnClose.Text = "Đóng";
            btnClose.UseVisualStyleBackColor = false;
            // 
            // dgvSalary
            // 
            dgvSalary.AllowUserToAddRows = false;
            dgvSalary.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 249, 250);
            dgvSalary.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvSalary.BackgroundColor = Color.White;
            dgvSalary.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(102, 126, 234);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvSalary.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvSalary.ColumnHeadersHeight = 40;
            dgvSalary.Columns.AddRange(new DataGridViewColumn[] { colMaNV, colTenNV, colChucVu, colSoNgayLam, colTongGioLam, colLuongTheoGio, colLuongCoBan, colPhuCap, colThuong, colPhat, colTongLuong, colActions });
            dgvSalary.Dock = DockStyle.Fill;
            dgvSalary.Location = new Point(20, 347);
            dgvSalary.Name = "dgvSalary";
            dgvSalary.ReadOnly = true;
            dgvSalary.RowHeadersVisible = false;
            dgvSalary.RowHeadersWidth = 62;
            dgvSalary.RowTemplate.Height = 40;
            dgvSalary.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSalary.Size = new Size(1443, 433);
            dgvSalary.TabIndex = 3;
            dgvSalary.CellContentClick += DgvSalary_CellContentClick;
            // 
            // colMaNV
            // 
            colMaNV.HeaderText = "Mã NV";
            colMaNV.MinimumWidth = 8;
            colMaNV.Name = "colMaNV";
            colMaNV.ReadOnly = true;
            colMaNV.Width = 80;
            // 
            // colTenNV
            // 
            colTenNV.HeaderText = "Tên nhân viên";
            colTenNV.MinimumWidth = 8;
            colTenNV.Name = "colTenNV";
            colTenNV.ReadOnly = true;
            colTenNV.Width = 200;
            // 
            // colChucVu
            // 
            colChucVu.HeaderText = "Chức vụ";
            colChucVu.MinimumWidth = 8;
            colChucVu.Name = "colChucVu";
            colChucVu.ReadOnly = true;
            colChucVu.Width = 120;
            // 
            // colSoNgayLam
            // 
            colSoNgayLam.HeaderText = "Số ngày";
            colSoNgayLam.MinimumWidth = 8;
            colSoNgayLam.Name = "colSoNgayLam";
            colSoNgayLam.ReadOnly = true;
            colSoNgayLam.Width = 90;
            // 
            // colTongGioLam
            // 
            colTongGioLam.HeaderText = "Tổng giờ";
            colTongGioLam.MinimumWidth = 8;
            colTongGioLam.Name = "colTongGioLam";
            colTongGioLam.ReadOnly = true;
            // 
            // colLuongTheoGio
            // 
            colLuongTheoGio.HeaderText = "Lương/giờ";
            colLuongTheoGio.MinimumWidth = 8;
            colLuongTheoGio.Name = "colLuongTheoGio";
            colLuongTheoGio.ReadOnly = true;
            colLuongTheoGio.Width = 120;
            // 
            // colLuongCoBan
            // 
            colLuongCoBan.HeaderText = "Lương cơ bản";
            colLuongCoBan.MinimumWidth = 8;
            colLuongCoBan.Name = "colLuongCoBan";
            colLuongCoBan.ReadOnly = true;
            colLuongCoBan.Width = 130;
            // 
            // colPhuCap
            // 
            colPhuCap.HeaderText = "Phụ cấp";
            colPhuCap.MinimumWidth = 8;
            colPhuCap.Name = "colPhuCap";
            colPhuCap.ReadOnly = true;
            colPhuCap.Width = 110;
            // 
            // colThuong
            // 
            colThuong.HeaderText = "Thưởng";
            colThuong.MinimumWidth = 8;
            colThuong.Name = "colThuong";
            colThuong.ReadOnly = true;
            colThuong.Width = 110;
            // 
            // colPhat
            // 
            colPhat.HeaderText = "Phạt";
            colPhat.MinimumWidth = 8;
            colPhat.Name = "colPhat";
            colPhat.ReadOnly = true;
            colPhat.Width = 110;
            // 
            // colTongLuong
            // 
            colTongLuong.HeaderText = "Tổng lương";
            colTongLuong.MinimumWidth = 8;
            colTongLuong.Name = "colTongLuong";
            colTongLuong.ReadOnly = true;
            colTongLuong.Width = 150;
            // 
            // colActions
            // 
            colActions.HeaderText = "Thao tác";
            colActions.MinimumWidth = 8;
            colActions.Name = "colActions";
            colActions.ReadOnly = true;
            colActions.Text = "Chi tiết";
            colActions.UseColumnTextForButtonValue = true;
            colActions.Width = 120;
            // 
            // panelStats
            // 
            panelStats.BackColor = Color.White;
            panelStats.Controls.Add(cardSalary);
            panelStats.Controls.Add(cardHours);
            panelStats.Controls.Add(cardEmployees);
            panelStats.Dock = DockStyle.Top;
            panelStats.Location = new Point(20, 178);
            panelStats.Name = "panelStats";
            panelStats.Padding = new Padding(20, 15, 20, 15);
            panelStats.Size = new Size(1443, 169);
            panelStats.TabIndex = 2;
            // 
            // cardSalary
            // 
            cardSalary.BackColor = Color.FromArgb(34, 197, 94);
            cardSalary.Controls.Add(lblTotalSalary);
            cardSalary.Controls.Add(lblTitleSalary);
            cardSalary.Controls.Add(lblIconSalary);
            cardSalary.Location = new Point(806, 15);
            cardSalary.Name = "cardSalary";
            cardSalary.Size = new Size(320, 96);
            cardSalary.TabIndex = 2;
            // 
            // lblTotalSalary
            // 
            lblTotalSalary.AutoSize = true;
            lblTotalSalary.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTotalSalary.ForeColor = Color.White;
            lblTotalSalary.Location = new Point(70, 35);
            lblTotalSalary.Name = "lblTotalSalary";
            lblTotalSalary.Size = new Size(58, 45);
            lblTotalSalary.TabIndex = 2;
            lblTotalSalary.Text = "0đ";
            // 
            // lblTitleSalary
            // 
            lblTitleSalary.AutoSize = true;
            lblTitleSalary.Font = new Font("Segoe UI", 9F);
            lblTitleSalary.ForeColor = Color.FromArgb(230, 230, 255);
            lblTitleSalary.Location = new Point(70, 15);
            lblTitleSalary.Name = "lblTitleSalary";
            lblTitleSalary.Size = new Size(105, 25);
            lblTitleSalary.TabIndex = 1;
            lblTitleSalary.Text = "Tổng lương";
            // 
            // lblIconSalary
            // 
            lblIconSalary.AutoSize = true;
            lblIconSalary.Font = new Font("Segoe UI", 24F);
            lblIconSalary.ForeColor = Color.White;
            lblIconSalary.Location = new Point(0, 15);
            lblIconSalary.Name = "lblIconSalary";
            lblIconSalary.Size = new Size(94, 65);
            lblIconSalary.TabIndex = 0;
            lblIconSalary.Text = "💰";
            // 
            // cardHours
            // 
            cardHours.BackColor = Color.FromArgb(245, 158, 11);
            cardHours.Controls.Add(lblTotalHours);
            cardHours.Controls.Add(lblTitleHours);
            cardHours.Controls.Add(lblIconHours);
            cardHours.Location = new Point(409, 15);
            cardHours.Name = "cardHours";
            cardHours.Size = new Size(320, 96);
            cardHours.TabIndex = 1;
            // 
            // lblTotalHours
            // 
            lblTotalHours.AutoSize = true;
            lblTotalHours.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTotalHours.ForeColor = Color.White;
            lblTotalHours.Location = new Point(70, 35);
            lblTotalHours.Name = "lblTotalHours";
            lblTotalHours.Size = new Size(57, 45);
            lblTotalHours.TabIndex = 2;
            lblTotalHours.Text = "0h";
            // 
            // lblTitleHours
            // 
            lblTitleHours.AutoSize = true;
            lblTitleHours.Font = new Font("Segoe UI", 9F);
            lblTitleHours.ForeColor = Color.FromArgb(230, 230, 255);
            lblTitleHours.Location = new Point(70, 15);
            lblTitleHours.Name = "lblTitleHours";
            lblTitleHours.Size = new Size(118, 25);
            lblTitleHours.TabIndex = 1;
            lblTitleHours.Text = "Tổng giờ làm";
            // 
            // lblIconHours
            // 
            lblIconHours.AutoSize = true;
            lblIconHours.Font = new Font("Segoe UI", 24F);
            lblIconHours.ForeColor = Color.White;
            lblIconHours.Location = new Point(0, 15);
            lblIconHours.Name = "lblIconHours";
            lblIconHours.Size = new Size(94, 65);
            lblIconHours.TabIndex = 0;
            lblIconHours.Text = "⏰";
            // 
            // cardEmployees
            // 
            cardEmployees.BackColor = Color.FromArgb(59, 130, 246);
            cardEmployees.Controls.Add(lblTotalEmployees);
            cardEmployees.Controls.Add(lblTitleEmployees);
            cardEmployees.Controls.Add(lblIconEmployees);
            cardEmployees.Location = new Point(20, 15);
            cardEmployees.Name = "cardEmployees";
            cardEmployees.Size = new Size(320, 96);
            cardEmployees.TabIndex = 0;
            // 
            // lblTotalEmployees
            // 
            lblTotalEmployees.AutoSize = true;
            lblTotalEmployees.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTotalEmployees.ForeColor = Color.White;
            lblTotalEmployees.Location = new Point(70, 35);
            lblTotalEmployees.Name = "lblTotalEmployees";
            lblTotalEmployees.Size = new Size(38, 45);
            lblTotalEmployees.TabIndex = 2;
            lblTotalEmployees.Text = "0";
            // 
            // lblTitleEmployees
            // 
            lblTitleEmployees.AutoSize = true;
            lblTitleEmployees.Font = new Font("Segoe UI", 9F);
            lblTitleEmployees.ForeColor = Color.FromArgb(230, 230, 255);
            lblTitleEmployees.Location = new Point(70, 15);
            lblTitleEmployees.Name = "lblTitleEmployees";
            lblTitleEmployees.Size = new Size(134, 25);
            lblTitleEmployees.TabIndex = 1;
            lblTitleEmployees.Text = "Tổng nhân viên";
            // 
            // lblIconEmployees
            // 
            lblIconEmployees.AutoSize = true;
            lblIconEmployees.Font = new Font("Segoe UI", 24F);
            lblIconEmployees.ForeColor = Color.White;
            lblIconEmployees.Location = new Point(-5, 15);
            lblIconEmployees.Name = "lblIconEmployees";
            lblIconEmployees.Size = new Size(94, 65);
            lblIconEmployees.TabIndex = 0;
            lblIconEmployees.Text = "👥";
            // 
            // panelFilter
            // 
            panelFilter.BackColor = Color.White;
            panelFilter.Controls.Add(btnExport);
            panelFilter.Controls.Add(btnCalculateAll);
            panelFilter.Controls.Add(btnRefresh);
            panelFilter.Controls.Add(cboYear);
            panelFilter.Controls.Add(lblYear);
            panelFilter.Controls.Add(cboMonth);
            panelFilter.Controls.Add(lblMonth);
            panelFilter.Dock = DockStyle.Top;
            panelFilter.Location = new Point(20, 102);
            panelFilter.Name = "panelFilter";
            panelFilter.Padding = new Padding(20, 15, 20, 15);
            panelFilter.Size = new Size(1443, 76);
            panelFilter.TabIndex = 1;
            // 
            // btnExport
            // 
            btnExport.BackColor = Color.FromArgb(23, 162, 184);
            btnExport.Cursor = Cursors.Hand;
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnExport.ForeColor = Color.White;
            btnExport.Location = new Point(763, 18);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(130, 40);
            btnExport.TabIndex = 6;
            btnExport.Text = "📥 Xuất Excel";
            btnExport.UseVisualStyleBackColor = false;
            btnExport.Click += BtnExport_Click;
            // 
            // btnCalculateAll
            // 
            btnCalculateAll.BackColor = Color.FromArgb(34, 197, 94);
            btnCalculateAll.Cursor = Cursors.Hand;
            btnCalculateAll.FlatAppearance.BorderSize = 0;
            btnCalculateAll.FlatStyle = FlatStyle.Flat;
            btnCalculateAll.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCalculateAll.ForeColor = Color.White;
            btnCalculateAll.Location = new Point(593, 18);
            btnCalculateAll.Name = "btnCalculateAll";
            btnCalculateAll.Size = new Size(160, 40);
            btnCalculateAll.TabIndex = 5;
            btnCalculateAll.Text = "💵 Tính lương tất cả";
            btnCalculateAll.UseVisualStyleBackColor = false;
            btnCalculateAll.Click += BtnCalculateAll_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(102, 126, 234);
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(463, 18);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(120, 40);
            btnRefresh.TabIndex = 4;
            btnRefresh.Text = "🔄 Làm mới";
            btnRefresh.UseVisualStyleBackColor = false;
            // 
            // cboYear
            // 
            cboYear.DropDownStyle = ComboBoxStyle.DropDownList;
            cboYear.Font = new Font("Segoe UI", 10F);
            cboYear.FormattingEnabled = true;
            cboYear.Location = new Point(305, 18);
            cboYear.Name = "cboYear";
            cboYear.Size = new Size(100, 36);
            cboYear.TabIndex = 3;
            // 
            // lblYear
            // 
            lblYear.AutoSize = true;
            lblYear.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblYear.Location = new Point(237, 21);
            lblYear.Name = "lblYear";
            lblYear.Size = new Size(62, 28);
            lblYear.TabIndex = 2;
            lblYear.Text = "Năm:";
            // 
            // cboMonth
            // 
            cboMonth.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMonth.Font = new Font("Segoe UI", 10F);
            cboMonth.FormattingEnabled = true;
            cboMonth.Location = new Point(131, 18);
            cboMonth.Name = "cboMonth";
            cboMonth.Size = new Size(100, 36);
            cboMonth.TabIndex = 1;
            // 
            // lblMonth
            // 
            lblMonth.AutoSize = true;
            lblMonth.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblMonth.Location = new Point(15, 21);
            lblMonth.Name = "lblMonth";
            lblMonth.Size = new Size(110, 28);
            lblMonth.TabIndex = 0;
            lblMonth.Text = "📅 Tháng:";
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.White;
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(20, 20);
            panelHeader.Name = "panelHeader";
            panelHeader.Padding = new Padding(20, 15, 20, 15);
            panelHeader.Size = new Size(1443, 82);
            panelHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(26, 26, 46);
            lblTitle.Location = new Point(20, 15);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(464, 54);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "💰 Quản lý bảng lương";
            // 
            // SalaryManagementForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 249, 250);
            ClientSize = new Size(1483, 800);
            Controls.Add(panelMain);
            Font = new Font("Segoe UI", 9F);
            Name = "SalaryManagementForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "💰 Quản lý bảng lương";
            panelMain.ResumeLayout(false);
            panelActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvSalary).EndInit();
            panelStats.ResumeLayout(false);
            cardSalary.ResumeLayout(false);
            cardSalary.PerformLayout();
            cardHours.ResumeLayout(false);
            cardHours.PerformLayout();
            cardEmployees.ResumeLayout(false);
            cardEmployees.PerformLayout();
            panelFilter.ResumeLayout(false);
            panelFilter.PerformLayout();
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelFilter;
        private System.Windows.Forms.Label lblMonth;
        private System.Windows.Forms.ComboBox cboMonth;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.ComboBox cboYear;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnCalculateAll;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Panel panelStats;
        private System.Windows.Forms.Panel cardEmployees;
        private System.Windows.Forms.Label lblIconEmployees;
        private System.Windows.Forms.Label lblTitleEmployees;
        private System.Windows.Forms.Label lblTotalEmployees;
        private System.Windows.Forms.Panel cardHours;
        private System.Windows.Forms.Label lblTotalHours;
        private System.Windows.Forms.Label lblTitleHours;
        private System.Windows.Forms.Label lblIconHours;
        private System.Windows.Forms.Panel cardSalary;
        private System.Windows.Forms.Label lblTotalSalary;
        private System.Windows.Forms.Label lblTitleSalary;
        private System.Windows.Forms.Label lblIconSalary;
        private System.Windows.Forms.DataGridView dgvSalary;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaNV;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenNV;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChucVu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoNgayLam;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTongGioLam;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLuongTheoGio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLuongCoBan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPhuCap;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThuong;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPhat;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTongLuong;
        private System.Windows.Forms.DataGridViewButtonColumn colActions;
        private System.Windows.Forms.Panel panelActions;
        private System.Windows.Forms.Button btnClose;
    }
}