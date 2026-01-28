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
            colTrangThai = new DataGridViewTextBoxColumn();
            colActions = new DataGridViewButtonColumn();
            panelStats = new Panel();
            cardAvgSalary = new Panel();
            lblAvgSalary = new Label();
            lblTitleAvgSalary = new Label();
            lblIconAvgSalary = new Label();
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
            txtSearch = new TextBox();
            lblSearch = new Label();
            cboStatus = new ComboBox();
            lblStatus = new Label();
            cboNhom = new ComboBox();
            lblNhom = new Label();
            btnExport = new Button();
            btnCalculateAll = new Button();
            btnRefresh = new Button();
            cboYear = new ComboBox();
            lblYear = new Label();
            cboMonth = new ComboBox();
            lblMonth = new Label();
            panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSalary).BeginInit();
            panelStats.SuspendLayout();
            cardAvgSalary.SuspendLayout();
            cardSalary.SuspendLayout();
            cardHours.SuspendLayout();
            cardEmployees.SuspendLayout();
            panelFilter.SuspendLayout();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.AutoScroll = true;
            panelMain.Controls.Add(dgvSalary);
            panelMain.Controls.Add(panelStats);
            panelMain.Controls.Add(panelFilter);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(0, 0);
            panelMain.Name = "panelMain";
            panelMain.Padding = new Padding(20);
            panelMain.Size = new Size(1628, 900);
            panelMain.TabIndex = 0;
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
            dgvSalary.ColumnHeadersHeight = 70;
            dgvSalary.Columns.AddRange(new DataGridViewColumn[] { colMaNV, colTenNV, colChucVu, colSoNgayLam, colTongGioLam, colLuongTheoGio, colLuongCoBan, colPhuCap, colThuong, colPhat, colTongLuong, colTrangThai, colActions });
            dgvSalary.Dock = DockStyle.Fill;
            dgvSalary.EnableHeadersVisualStyles = false;
            dgvSalary.Location = new Point(20, 331);
            dgvSalary.Name = "dgvSalary";
            dgvSalary.ReadOnly = true;
            dgvSalary.RowHeadersVisible = false;
            dgvSalary.RowHeadersWidth = 70;
            dgvSalary.RowTemplate.Height = 50;
            dgvSalary.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSalary.Size = new Size(1588, 549);
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
            colTenNV.Width = 180;
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
            colSoNgayLam.Width = 85;
            // 
            // colTongGioLam
            // 
            colTongGioLam.HeaderText = "Tổng giờ";
            colTongGioLam.MinimumWidth = 8;
            colTongGioLam.Name = "colTongGioLam";
            colTongGioLam.ReadOnly = true;
            colTongGioLam.Width = 150;
            // 
            // colLuongTheoGio
            // 
            colLuongTheoGio.HeaderText = "Lương/giờ";
            colLuongTheoGio.MinimumWidth = 8;
            colLuongTheoGio.Name = "colLuongTheoGio";
            colLuongTheoGio.ReadOnly = true;
            colLuongTheoGio.Width = 110;
            // 
            // colLuongCoBan
            // 
            colLuongCoBan.HeaderText = "Lương theo giờ";
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
            colTongLuong.Width = 140;
            // 
            // colTrangThai
            // 
            colTrangThai.HeaderText = "Trạng thái";
            colTrangThai.MinimumWidth = 8;
            colTrangThai.Name = "colTrangThai";
            colTrangThai.ReadOnly = true;
            colTrangThai.Width = 110;
            // 
            // colActions
            // 
            colActions.HeaderText = "Thao tác";
            colActions.MinimumWidth = 8;
            colActions.Name = "colActions";
            colActions.ReadOnly = true;
            colActions.Text = "Chi tiết";
            colActions.UseColumnTextForButtonValue = true;
            colActions.Width = 150;
            // 
            // panelStats
            // 
            panelStats.BackColor = Color.Transparent;
            panelStats.Controls.Add(cardAvgSalary);
            panelStats.Controls.Add(cardSalary);
            panelStats.Controls.Add(cardHours);
            panelStats.Controls.Add(cardEmployees);
            panelStats.Dock = DockStyle.Top;
            panelStats.Location = new Point(20, 186);
            panelStats.Name = "panelStats";
            panelStats.Padding = new Padding(0, 10, 0, 10);
            panelStats.Size = new Size(1588, 145);
            panelStats.TabIndex = 2;
            // 
            // cardAvgSalary
            // 
            cardAvgSalary.BackColor = Color.White;
            cardAvgSalary.Controls.Add(lblAvgSalary);
            cardAvgSalary.Controls.Add(lblTitleAvgSalary);
            cardAvgSalary.Controls.Add(lblIconAvgSalary);
            cardAvgSalary.Dock = DockStyle.Left;
            cardAvgSalary.Location = new Point(1188, 10);
            cardAvgSalary.Name = "cardAvgSalary";
            cardAvgSalary.Padding = new Padding(20, 15, 20, 15);
            cardAvgSalary.Size = new Size(396, 125);
            cardAvgSalary.TabIndex = 3;
            // 
            // lblAvgSalary
            // 
            lblAvgSalary.Dock = DockStyle.Bottom;
            lblAvgSalary.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblAvgSalary.ForeColor = Color.FromArgb(234, 179, 8);
            lblAvgSalary.Location = new Point(90, 70);
            lblAvgSalary.Name = "lblAvgSalary";
            lblAvgSalary.Size = new Size(286, 40);
            lblAvgSalary.TabIndex = 2;
            lblAvgSalary.Text = "0đ";
            lblAvgSalary.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTitleAvgSalary
            // 
            lblTitleAvgSalary.Dock = DockStyle.Top;
            lblTitleAvgSalary.Font = new Font("Segoe UI", 10F);
            lblTitleAvgSalary.ForeColor = Color.FromArgb(100, 116, 139);
            lblTitleAvgSalary.Location = new Point(90, 15);
            lblTitleAvgSalary.Name = "lblTitleAvgSalary";
            lblTitleAvgSalary.Size = new Size(286, 28);
            lblTitleAvgSalary.TabIndex = 1;
            lblTitleAvgSalary.Text = "Lương TB/người";
            // 
            // lblIconAvgSalary
            // 
            lblIconAvgSalary.Dock = DockStyle.Left;
            lblIconAvgSalary.Font = new Font("Segoe UI", 24F);
            lblIconAvgSalary.Location = new Point(20, 15);
            lblIconAvgSalary.Name = "lblIconAvgSalary";
            lblIconAvgSalary.Size = new Size(70, 95);
            lblIconAvgSalary.TabIndex = 0;
            lblIconAvgSalary.Text = "📊";
            lblIconAvgSalary.TextAlign = ContentAlignment.TopCenter;
            // 
            // cardSalary
            // 
            cardSalary.BackColor = Color.White;
            cardSalary.Controls.Add(lblTotalSalary);
            cardSalary.Controls.Add(lblTitleSalary);
            cardSalary.Controls.Add(lblIconSalary);
            cardSalary.Dock = DockStyle.Left;
            cardSalary.Location = new Point(792, 10);
            cardSalary.Name = "cardSalary";
            cardSalary.Padding = new Padding(20, 15, 20, 15);
            cardSalary.Size = new Size(396, 125);
            cardSalary.TabIndex = 2;
            // 
            // lblTotalSalary
            // 
            lblTotalSalary.Dock = DockStyle.Bottom;
            lblTotalSalary.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTotalSalary.ForeColor = Color.FromArgb(34, 197, 94);
            lblTotalSalary.Location = new Point(90, 70);
            lblTotalSalary.Name = "lblTotalSalary";
            lblTotalSalary.Size = new Size(286, 40);
            lblTotalSalary.TabIndex = 2;
            lblTotalSalary.Text = "0đ";
            lblTotalSalary.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTitleSalary
            // 
            lblTitleSalary.Dock = DockStyle.Top;
            lblTitleSalary.Font = new Font("Segoe UI", 10F);
            lblTitleSalary.ForeColor = Color.FromArgb(100, 116, 139);
            lblTitleSalary.Location = new Point(90, 15);
            lblTitleSalary.Name = "lblTitleSalary";
            lblTitleSalary.Size = new Size(286, 28);
            lblTitleSalary.TabIndex = 1;
            lblTitleSalary.Text = "Tổng lương tháng";
            // 
            // lblIconSalary
            // 
            lblIconSalary.Dock = DockStyle.Left;
            lblIconSalary.Font = new Font("Segoe UI", 24F);
            lblIconSalary.Location = new Point(20, 15);
            lblIconSalary.Name = "lblIconSalary";
            lblIconSalary.Size = new Size(70, 95);
            lblIconSalary.TabIndex = 0;
            lblIconSalary.Text = "💰";
            lblIconSalary.TextAlign = ContentAlignment.TopCenter;
            // 
            // cardHours
            // 
            cardHours.BackColor = Color.White;
            cardHours.Controls.Add(lblTotalHours);
            cardHours.Controls.Add(lblTitleHours);
            cardHours.Controls.Add(lblIconHours);
            cardHours.Dock = DockStyle.Left;
            cardHours.Location = new Point(396, 10);
            cardHours.Name = "cardHours";
            cardHours.Padding = new Padding(20, 15, 20, 15);
            cardHours.Size = new Size(396, 125);
            cardHours.TabIndex = 1;
            // 
            // lblTotalHours
            // 
            lblTotalHours.Dock = DockStyle.Bottom;
            lblTotalHours.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTotalHours.ForeColor = Color.FromArgb(59, 130, 246);
            lblTotalHours.Location = new Point(90, 70);
            lblTotalHours.Name = "lblTotalHours";
            lblTotalHours.Size = new Size(286, 40);
            lblTotalHours.TabIndex = 2;
            lblTotalHours.Text = "0h";
            lblTotalHours.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTitleHours
            // 
            lblTitleHours.Dock = DockStyle.Top;
            lblTitleHours.Font = new Font("Segoe UI", 10F);
            lblTitleHours.ForeColor = Color.FromArgb(100, 116, 139);
            lblTitleHours.Location = new Point(90, 15);
            lblTitleHours.Name = "lblTitleHours";
            lblTitleHours.Size = new Size(286, 28);
            lblTitleHours.TabIndex = 1;
            lblTitleHours.Text = "Tổng giờ làm";
            // 
            // lblIconHours
            // 
            lblIconHours.Dock = DockStyle.Left;
            lblIconHours.Font = new Font("Segoe UI", 24F);
            lblIconHours.Location = new Point(20, 15);
            lblIconHours.Name = "lblIconHours";
            lblIconHours.Size = new Size(70, 95);
            lblIconHours.TabIndex = 0;
            lblIconHours.Text = "⏰";
            lblIconHours.TextAlign = ContentAlignment.TopCenter;
            // 
            // cardEmployees
            // 
            cardEmployees.BackColor = Color.White;
            cardEmployees.Controls.Add(lblTotalEmployees);
            cardEmployees.Controls.Add(lblTitleEmployees);
            cardEmployees.Controls.Add(lblIconEmployees);
            cardEmployees.Dock = DockStyle.Left;
            cardEmployees.Location = new Point(0, 10);
            cardEmployees.Name = "cardEmployees";
            cardEmployees.Padding = new Padding(20, 15, 20, 15);
            cardEmployees.Size = new Size(396, 125);
            cardEmployees.TabIndex = 0;
            // 
            // lblTotalEmployees
            // 
            lblTotalEmployees.Dock = DockStyle.Bottom;
            lblTotalEmployees.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTotalEmployees.ForeColor = Color.FromArgb(139, 92, 246);
            lblTotalEmployees.Location = new Point(90, 70);
            lblTotalEmployees.Name = "lblTotalEmployees";
            lblTotalEmployees.Size = new Size(286, 40);
            lblTotalEmployees.TabIndex = 2;
            lblTotalEmployees.Text = "0";
            lblTotalEmployees.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTitleEmployees
            // 
            lblTitleEmployees.Dock = DockStyle.Top;
            lblTitleEmployees.Font = new Font("Segoe UI", 10F);
            lblTitleEmployees.ForeColor = Color.FromArgb(100, 116, 139);
            lblTitleEmployees.Location = new Point(90, 15);
            lblTitleEmployees.Name = "lblTitleEmployees";
            lblTitleEmployees.Size = new Size(286, 28);
            lblTitleEmployees.TabIndex = 1;
            lblTitleEmployees.Text = "Số nhân viên";
            // 
            // lblIconEmployees
            // 
            lblIconEmployees.Dock = DockStyle.Left;
            lblIconEmployees.Font = new Font("Segoe UI", 24F);
            lblIconEmployees.Location = new Point(20, 15);
            lblIconEmployees.Name = "lblIconEmployees";
            lblIconEmployees.Size = new Size(70, 95);
            lblIconEmployees.TabIndex = 0;
            lblIconEmployees.Text = "👥";
            lblIconEmployees.TextAlign = ContentAlignment.TopCenter;
            // 
            // panelFilter
            // 
            panelFilter.BackColor = Color.White;
            panelFilter.Controls.Add(txtSearch);
            panelFilter.Controls.Add(lblSearch);
            panelFilter.Controls.Add(cboStatus);
            panelFilter.Controls.Add(lblStatus);
            panelFilter.Controls.Add(cboNhom);
            panelFilter.Controls.Add(lblNhom);
            panelFilter.Controls.Add(btnExport);
            panelFilter.Controls.Add(btnCalculateAll);
            panelFilter.Controls.Add(btnRefresh);
            panelFilter.Controls.Add(cboYear);
            panelFilter.Controls.Add(lblYear);
            panelFilter.Controls.Add(cboMonth);
            panelFilter.Controls.Add(lblMonth);
            panelFilter.Dock = DockStyle.Top;
            panelFilter.Location = new Point(20, 20);
            panelFilter.Name = "panelFilter";
            panelFilter.Padding = new Padding(15, 15, 15, 20);
            panelFilter.Size = new Size(1588, 166);
            panelFilter.TabIndex = 1;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.Location = new Point(685, 18);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Tìm theo tên hoặc mã NV...";
            txtSearch.Size = new Size(250, 34);
            txtSearch.TabIndex = 12;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblSearch.Location = new Point(585, 21);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(105, 28);
            lblSearch.TabIndex = 11;
            lblSearch.Text = "Tìm kiếm:";
            // 
            // cboStatus
            // 
            cboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cboStatus.Font = new Font("Segoe UI", 10F);
            cboStatus.FormattingEnabled = true;
            cboStatus.Location = new Point(381, 18);
            cboStatus.Name = "cboStatus";
            cboStatus.Size = new Size(180, 36);
            cboStatus.TabIndex = 10;
            cboStatus.SelectedIndexChanged += CboStatus_SelectedIndexChanged;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblStatus.Location = new Point(257, 24);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(113, 28);
            lblStatus.TabIndex = 9;
            lblStatus.Text = "Trạng thái:";
            // 
            // cboNhom
            // 
            cboNhom.DropDownStyle = ComboBoxStyle.DropDownList;
            cboNhom.Font = new Font("Segoe UI", 10F);
            cboNhom.FormattingEnabled = true;
            cboNhom.Location = new Point(381, 59);
            cboNhom.Name = "cboNhom";
            cboNhom.Size = new Size(180, 36);
            cboNhom.TabIndex = 8;
            cboNhom.SelectedIndexChanged += CboNhom_SelectedIndexChanged;
            // 
            // lblNhom
            // 
            lblNhom.AutoSize = true;
            lblNhom.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblNhom.Location = new Point(257, 67);
            lblNhom.Name = "lblNhom";
            lblNhom.Size = new Size(118, 28);
            lblNhom.TabIndex = 7;
            lblNhom.Text = "Phòng ban:";
            // 
            // btnExport
            // 
            btnExport.BackColor = Color.FromArgb(34, 197, 94);
            btnExport.Cursor = Cursors.Hand;
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnExport.ForeColor = Color.White;
            btnExport.Location = new Point(353, 110);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(160, 45);
            btnExport.TabIndex = 6;
            btnExport.Text = "📥 Xuất Excel";
            btnExport.UseVisualStyleBackColor = false;
            // 
            // btnCalculateAll
            // 
            btnCalculateAll.BackColor = Color.FromArgb(102, 126, 234);
            btnCalculateAll.Cursor = Cursors.Hand;
            btnCalculateAll.FlatAppearance.BorderSize = 0;
            btnCalculateAll.FlatStyle = FlatStyle.Flat;
            btnCalculateAll.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCalculateAll.ForeColor = Color.White;
            btnCalculateAll.Location = new Point(175, 110);
            btnCalculateAll.Name = "btnCalculateAll";
            btnCalculateAll.Size = new Size(172, 45);
            btnCalculateAll.TabIndex = 5;
            btnCalculateAll.Text = "💵 Tính lương tất cả";
            btnCalculateAll.UseVisualStyleBackColor = false;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(59, 130, 246);
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(15, 110);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(154, 45);
            btnRefresh.TabIndex = 4;
            btnRefresh.Text = "🔄 Làm mới";
            btnRefresh.UseVisualStyleBackColor = false;
            // 
            // cboYear
            // 
            cboYear.DropDownStyle = ComboBoxStyle.DropDownList;
            cboYear.Font = new Font("Segoe UI", 10F);
            cboYear.FormattingEnabled = true;
            cboYear.Location = new Point(131, 61);
            cboYear.Name = "cboYear";
            cboYear.Size = new Size(100, 36);
            cboYear.TabIndex = 3;
            // 
            // lblYear
            // 
            lblYear.AutoSize = true;
            lblYear.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblYear.Location = new Point(15, 64);
            lblYear.Name = "lblYear";
            lblYear.Size = new Size(96, 28);
            lblYear.TabIndex = 2;
            lblYear.Text = "📅 Năm:";
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
            // SalaryManagementForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 249, 250);
            ClientSize = new Size(1628, 900);
            Controls.Add(panelMain);
            Font = new Font("Segoe UI", 9F);
            Name = "SalaryManagementForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "💰 Quản lý bảng lương";
            panelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvSalary).EndInit();
            panelStats.ResumeLayout(false);
            cardAvgSalary.ResumeLayout(false);
            cardSalary.ResumeLayout(false);
            cardHours.ResumeLayout(false);
            cardEmployees.ResumeLayout(false);
            panelFilter.ResumeLayout(false);
            panelFilter.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelFilter;
        private System.Windows.Forms.Label lblMonth;
        private System.Windows.Forms.ComboBox cboMonth;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.ComboBox cboYear;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnCalculateAll;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label lblNhom;
        private System.Windows.Forms.ComboBox cboNhom;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
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
        private System.Windows.Forms.Panel cardAvgSalary;
        private System.Windows.Forms.Label lblAvgSalary;
        private System.Windows.Forms.Label lblTitleAvgSalary;
        private System.Windows.Forms.Label lblIconAvgSalary;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrangThai;
        private System.Windows.Forms.DataGridViewButtonColumn colActions;
    }
}