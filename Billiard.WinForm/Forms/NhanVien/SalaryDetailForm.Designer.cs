namespace Billiard.WinForm.Forms.NhanVien
{
    partial class SalaryDetailForm
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
            btnCancel = new Button();
            btnSave = new Button();
            panelAttendance = new Panel();
            dgvAttendance = new DataGridView();
            colNgay = new DataGridViewTextBoxColumn();
            colGioVao = new DataGridViewTextBoxColumn();
            colGioRa = new DataGridViewTextBoxColumn();
            colSoGio = new DataGridViewTextBoxColumn();
            colTrangThai = new DataGridViewTextBoxColumn();
            lblAttendanceTitle = new Label();
            panelSalaryDetail = new Panel();
            txtGhiChu = new TextBox();
            lblGhiChu = new Label();
            numTongLuong = new NumericUpDown();
            lblTongLuong = new Label();
            numPhat = new NumericUpDown();
            lblPhat = new Label();
            numThuong = new NumericUpDown();
            lblThuong = new Label();
            numPhuCap = new NumericUpDown();
            lblPhuCap = new Label();
            numLuongCoBan = new NumericUpDown();
            lblLuongCoBan = new Label();
            txtTongGio = new TextBox();
            lblTongGio = new Label();
            txtSoNgay = new TextBox();
            lblSoNgay = new Label();
            lblSalaryDetailTitle = new Label();
            panelEmployee = new Panel();
            txtChucVu = new TextBox();
            lblChucVu = new Label();
            txtTenNV = new TextBox();
            lblTenNV = new Label();
            txtMaNV = new TextBox();
            lblMaNV = new Label();
            lblEmployeeTitle = new Label();
            panelMain.SuspendLayout();
            panelActions.SuspendLayout();
            panelAttendance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAttendance).BeginInit();
            panelSalaryDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numTongLuong).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPhat).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numThuong).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPhuCap).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numLuongCoBan).BeginInit();
            panelEmployee.SuspendLayout();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.AutoScroll = true;
            panelMain.Controls.Add(panelActions);
            panelMain.Controls.Add(panelAttendance);
            panelMain.Controls.Add(panelSalaryDetail);
            panelMain.Controls.Add(panelEmployee);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(0, 0);
            panelMain.Name = "panelMain";
            panelMain.Padding = new Padding(20);
            panelMain.Size = new Size(1000, 700);
            panelMain.TabIndex = 0;
            // 
            // panelActions
            // 
            panelActions.Controls.Add(btnCancel);
            panelActions.Controls.Add(btnSave);
            panelActions.Dock = DockStyle.Bottom;
            panelActions.Location = new Point(20, 620);
            panelActions.Name = "panelActions";
            panelActions.Size = new Size(960, 60);
            panelActions.TabIndex = 4;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancel.BackColor = Color.FromArgb(108, 117, 125);
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(820, 8);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(120, 45);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Đóng";
            btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSave.BackColor = Color.FromArgb(34, 197, 94);
            btnSave.Cursor = Cursors.Hand;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(680, 8);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(130, 45);
            btnSave.TabIndex = 0;
            btnSave.Text = "💾 Lưu";
            btnSave.UseVisualStyleBackColor = false;
            // 
            // panelAttendance
            // 
            panelAttendance.BackColor = Color.White;
            panelAttendance.Controls.Add(dgvAttendance);
            panelAttendance.Controls.Add(lblAttendanceTitle);
            panelAttendance.Dock = DockStyle.Fill;
            panelAttendance.Location = new Point(20, 292);
            panelAttendance.Name = "panelAttendance";
            panelAttendance.Padding = new Padding(20, 5, 20, 20);
            panelAttendance.Size = new Size(960, 388);
            panelAttendance.TabIndex = 3;
            // 
            // dgvAttendance
            // 
            dgvAttendance.AllowUserToAddRows = false;
            dgvAttendance.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 249, 250);
            dgvAttendance.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvAttendance.BackgroundColor = Color.White;
            dgvAttendance.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(102, 126, 234);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvAttendance.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvAttendance.ColumnHeadersHeight = 40;
            dgvAttendance.Columns.AddRange(new DataGridViewColumn[] { colNgay, colGioVao, colGioRa, colSoGio, colTrangThai });
            dgvAttendance.Dock = DockStyle.Fill;
            dgvAttendance.EnableHeadersVisualStyles = false;
            dgvAttendance.Location = new Point(20, 40);
            dgvAttendance.Name = "dgvAttendance";
            dgvAttendance.ReadOnly = true;
            dgvAttendance.RowHeadersVisible = false;
            dgvAttendance.RowHeadersWidth = 62;
            dgvAttendance.RowTemplate.Height = 35;
            dgvAttendance.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAttendance.Size = new Size(920, 328);
            dgvAttendance.TabIndex = 1;
            // 
            // colNgay
            // 
            colNgay.HeaderText = "Ngày";
            colNgay.MinimumWidth = 8;
            colNgay.Name = "colNgay";
            colNgay.ReadOnly = true;
            colNgay.Width = 120;
            // 
            // colGioVao
            // 
            colGioVao.HeaderText = "Giờ vào";
            colGioVao.MinimumWidth = 8;
            colGioVao.Name = "colGioVao";
            colGioVao.ReadOnly = true;
            colGioVao.Width = 150;
            // 
            // colGioRa
            // 
            colGioRa.HeaderText = "Giờ ra";
            colGioRa.MinimumWidth = 8;
            colGioRa.Name = "colGioRa";
            colGioRa.ReadOnly = true;
            colGioRa.Width = 150;
            // 
            // colSoGio
            // 
            colSoGio.HeaderText = "Số giờ";
            colSoGio.MinimumWidth = 8;
            colSoGio.Name = "colSoGio";
            colSoGio.ReadOnly = true;
            // 
            // colTrangThai
            // 
            colTrangThai.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colTrangThai.HeaderText = "Trạng thái";
            colTrangThai.MinimumWidth = 8;
            colTrangThai.Name = "colTrangThai";
            colTrangThai.ReadOnly = true;
            // 
            // lblAttendanceTitle
            // 
            lblAttendanceTitle.Dock = DockStyle.Top;
            lblAttendanceTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblAttendanceTitle.ForeColor = Color.FromArgb(26, 26, 46);
            lblAttendanceTitle.Location = new Point(20, 5);
            lblAttendanceTitle.Name = "lblAttendanceTitle";
            lblAttendanceTitle.Size = new Size(920, 35);
            lblAttendanceTitle.TabIndex = 0;
            lblAttendanceTitle.Text = "📋 Chi tiết chấm công";
            // 
            // panelSalaryDetail
            // 
            panelSalaryDetail.BackColor = Color.White;
            panelSalaryDetail.Controls.Add(txtGhiChu);
            panelSalaryDetail.Controls.Add(lblGhiChu);
            panelSalaryDetail.Controls.Add(numTongLuong);
            panelSalaryDetail.Controls.Add(lblTongLuong);
            panelSalaryDetail.Controls.Add(numPhat);
            panelSalaryDetail.Controls.Add(lblPhat);
            panelSalaryDetail.Controls.Add(numThuong);
            panelSalaryDetail.Controls.Add(lblThuong);
            panelSalaryDetail.Controls.Add(numPhuCap);
            panelSalaryDetail.Controls.Add(lblPhuCap);
            panelSalaryDetail.Controls.Add(numLuongCoBan);
            panelSalaryDetail.Controls.Add(lblLuongCoBan);
            panelSalaryDetail.Controls.Add(txtTongGio);
            panelSalaryDetail.Controls.Add(lblTongGio);
            panelSalaryDetail.Controls.Add(txtSoNgay);
            panelSalaryDetail.Controls.Add(lblSoNgay);
            panelSalaryDetail.Controls.Add(lblSalaryDetailTitle);
            panelSalaryDetail.Dock = DockStyle.Top;
            panelSalaryDetail.Location = new Point(20, 109);
            panelSalaryDetail.Name = "panelSalaryDetail";
            panelSalaryDetail.Padding = new Padding(20, 5, 20, 20);
            panelSalaryDetail.Size = new Size(960, 183);
            panelSalaryDetail.TabIndex = 2;
            // 
            // txtGhiChu
            // 
            txtGhiChu.Font = new Font("Segoe UI", 10F);
            txtGhiChu.Location = new Point(677, 89);
            txtGhiChu.Multiline = true;
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.Size = new Size(263, 70);
            txtGhiChu.TabIndex = 16;
            // 
            // lblGhiChu
            // 
            lblGhiChu.AutoSize = true;
            lblGhiChu.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblGhiChu.Location = new Point(677, 51);
            lblGhiChu.Name = "lblGhiChu";
            lblGhiChu.Size = new Size(89, 28);
            lblGhiChu.TabIndex = 15;
            lblGhiChu.Text = "Ghi chú:";
            // 
            // numTongLuong
            // 
            numTongLuong.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            numTongLuong.Location = new Point(630, 8);
            numTongLuong.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
            numTongLuong.Name = "numTongLuong";
            numTongLuong.ReadOnly = true;
            numTongLuong.Size = new Size(310, 34);
            numTongLuong.TabIndex = 14;
            numTongLuong.ThousandsSeparator = true;
            // 
            // lblTongLuong
            // 
            lblTongLuong.AutoSize = true;
            lblTongLuong.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTongLuong.ForeColor = Color.FromArgb(34, 197, 94);
            lblTongLuong.Location = new Point(497, 10);
            lblTongLuong.Name = "lblTongLuong";
            lblTongLuong.Size = new Size(127, 28);
            lblTongLuong.TabIndex = 13;
            lblTongLuong.Text = "Tổng lương:";
            // 
            // numPhat
            // 
            numPhat.Font = new Font("Segoe UI", 10F);
            numPhat.Location = new Point(434, 144);
            numPhat.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            numPhat.Name = "numPhat";
            numPhat.Size = new Size(230, 34);
            numPhat.TabIndex = 12;
            numPhat.ThousandsSeparator = true;
            // 
            // lblPhat
            // 
            lblPhat.AutoSize = true;
            lblPhat.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPhat.Location = new Point(361, 144);
            lblPhat.Name = "lblPhat";
            lblPhat.Size = new Size(60, 28);
            lblPhat.TabIndex = 11;
            lblPhat.Text = "Phạt:";
            // 
            // numThuong
            // 
            numThuong.Font = new Font("Segoe UI", 10F);
            numThuong.Location = new Point(434, 104);
            numThuong.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            numThuong.Name = "numThuong";
            numThuong.Size = new Size(230, 34);
            numThuong.TabIndex = 10;
            numThuong.ThousandsSeparator = true;
            // 
            // lblThuong
            // 
            lblThuong.AutoSize = true;
            lblThuong.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblThuong.Location = new Point(337, 106);
            lblThuong.Name = "lblThuong";
            lblThuong.Size = new Size(91, 28);
            lblThuong.TabIndex = 9;
            lblThuong.Text = "Thưởng:";
            // 
            // numPhuCap
            // 
            numPhuCap.Font = new Font("Segoe UI", 10F);
            numPhuCap.Location = new Point(434, 64);
            numPhuCap.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            numPhuCap.Name = "numPhuCap";
            numPhuCap.ReadOnly = true;
            numPhuCap.Size = new Size(230, 34);
            numPhuCap.TabIndex = 8;
            numPhuCap.ThousandsSeparator = true;
            // 
            // lblPhuCap
            // 
            lblPhuCap.AutoSize = true;
            lblPhuCap.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPhuCap.Location = new Point(336, 66);
            lblPhuCap.Name = "lblPhuCap";
            lblPhuCap.Size = new Size(92, 28);
            lblPhuCap.TabIndex = 7;
            lblPhuCap.Text = "Phụ cấp:";
            // 
            // numLuongCoBan
            // 
            numLuongCoBan.Font = new Font("Segoe UI", 10F);
            numLuongCoBan.Location = new Point(110, 124);
            numLuongCoBan.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
            numLuongCoBan.Name = "numLuongCoBan";
            numLuongCoBan.ReadOnly = true;
            numLuongCoBan.Size = new Size(180, 34);
            numLuongCoBan.TabIndex = 6;
            numLuongCoBan.ThousandsSeparator = true;
            // 
            // lblLuongCoBan
            // 
            lblLuongCoBan.AutoSize = true;
            lblLuongCoBan.Font = new Font("Segoe UI", 8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLuongCoBan.Location = new Point(20, 132);
            lblLuongCoBan.Name = "lblLuongCoBan";
            lblLuongCoBan.Size = new Size(84, 21);
            lblLuongCoBan.TabIndex = 5;
            lblLuongCoBan.Text = "Lương \\h:";
            // 
            // txtTongGio
            // 
            txtTongGio.Font = new Font("Segoe UI", 10F);
            txtTongGio.Location = new Point(108, 80);
            txtTongGio.Name = "txtTongGio";
            txtTongGio.ReadOnly = true;
            txtTongGio.Size = new Size(180, 34);
            txtTongGio.TabIndex = 4;
            // 
            // lblTongGio
            // 
            lblTongGio.AutoSize = true;
            lblTongGio.Font = new Font("Segoe UI", 8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTongGio.Location = new Point(20, 89);
            lblTongGio.Name = "lblTongGio";
            lblTongGio.Size = new Size(82, 21);
            lblTongGio.TabIndex = 3;
            lblTongGio.Text = "Tổng giờ:";
            // 
            // txtSoNgay
            // 
            txtSoNgay.Font = new Font("Segoe UI", 10F);
            txtSoNgay.Location = new Point(134, 33);
            txtSoNgay.Name = "txtSoNgay";
            txtSoNgay.ReadOnly = true;
            txtSoNgay.Size = new Size(180, 34);
            txtSoNgay.TabIndex = 2;
            // 
            // lblSoNgay
            // 
            lblSoNgay.AutoSize = true;
            lblSoNgay.Font = new Font("Segoe UI", 8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSoNgay.Location = new Point(20, 42);
            lblSoNgay.Name = "lblSoNgay";
            lblSoNgay.Size = new Size(108, 21);
            lblSoNgay.TabIndex = 1;
            lblSoNgay.Text = "Số ngày làm:";
            // 
            // lblSalaryDetailTitle
            // 
            lblSalaryDetailTitle.Dock = DockStyle.Top;
            lblSalaryDetailTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblSalaryDetailTitle.ForeColor = Color.FromArgb(26, 26, 46);
            lblSalaryDetailTitle.Location = new Point(20, 5);
            lblSalaryDetailTitle.Name = "lblSalaryDetailTitle";
            lblSalaryDetailTitle.Size = new Size(920, 37);
            lblSalaryDetailTitle.TabIndex = 0;
            lblSalaryDetailTitle.Text = "💰 Chi tiết lương";
            // 
            // panelEmployee
            // 
            panelEmployee.BackColor = Color.White;
            panelEmployee.Controls.Add(txtChucVu);
            panelEmployee.Controls.Add(lblChucVu);
            panelEmployee.Controls.Add(txtTenNV);
            panelEmployee.Controls.Add(lblTenNV);
            panelEmployee.Controls.Add(txtMaNV);
            panelEmployee.Controls.Add(lblMaNV);
            panelEmployee.Controls.Add(lblEmployeeTitle);
            panelEmployee.Dock = DockStyle.Top;
            panelEmployee.Location = new Point(20, 20);
            panelEmployee.Name = "panelEmployee";
            panelEmployee.Padding = new Padding(20, 5, 20, 20);
            panelEmployee.Size = new Size(960, 89);
            panelEmployee.TabIndex = 1;
            // 
            // txtChucVu
            // 
            txtChucVu.Font = new Font("Segoe UI", 10F);
            txtChucVu.Location = new Point(677, 38);
            txtChucVu.Name = "txtChucVu";
            txtChucVu.ReadOnly = true;
            txtChucVu.Size = new Size(260, 34);
            txtChucVu.TabIndex = 6;
            // 
            // lblChucVu
            // 
            lblChucVu.AutoSize = true;
            lblChucVu.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblChucVu.Location = new Point(575, 41);
            lblChucVu.Name = "lblChucVu";
            lblChucVu.Size = new Size(93, 28);
            lblChucVu.TabIndex = 5;
            lblChucVu.Text = "Chức vụ:";
            // 
            // txtTenNV
            // 
            txtTenNV.Font = new Font("Segoe UI", 10F);
            txtTenNV.Location = new Point(236, 38);
            txtTenNV.Name = "txtTenNV";
            txtTenNV.ReadOnly = true;
            txtTenNV.Size = new Size(325, 34);
            txtTenNV.TabIndex = 4;
            // 
            // lblTenNV
            // 
            lblTenNV.AutoSize = true;
            lblTenNV.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTenNV.Location = new Point(180, 41);
            lblTenNV.Name = "lblTenNV";
            lblTenNV.Size = new Size(50, 28);
            lblTenNV.TabIndex = 3;
            lblTenNV.Text = "Tên:";
            // 
            // txtMaNV
            // 
            txtMaNV.Font = new Font("Segoe UI", 10F);
            txtMaNV.Location = new Point(73, 38);
            txtMaNV.Name = "txtMaNV";
            txtMaNV.ReadOnly = true;
            txtMaNV.Size = new Size(85, 34);
            txtMaNV.TabIndex = 2;
            // 
            // lblMaNV
            // 
            lblMaNV.AutoSize = true;
            lblMaNV.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblMaNV.Location = new Point(20, 41);
            lblMaNV.Name = "lblMaNV";
            lblMaNV.Size = new Size(47, 28);
            lblMaNV.TabIndex = 1;
            lblMaNV.Text = "Mã:";
            // 
            // lblEmployeeTitle
            // 
            lblEmployeeTitle.Dock = DockStyle.Top;
            lblEmployeeTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblEmployeeTitle.ForeColor = Color.FromArgb(26, 26, 46);
            lblEmployeeTitle.Location = new Point(20, 5);
            lblEmployeeTitle.Name = "lblEmployeeTitle";
            lblEmployeeTitle.Size = new Size(920, 36);
            lblEmployeeTitle.TabIndex = 0;
            lblEmployeeTitle.Text = "👤 Thông tin nhân viên";
            // 
            // SalaryDetailForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 249, 250);
            ClientSize = new Size(1000, 700);
            Controls.Add(panelMain);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SalaryDetailForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "📝 Chi tiết bảng lương";
            panelMain.ResumeLayout(false);
            panelActions.ResumeLayout(false);
            panelAttendance.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAttendance).EndInit();
            panelSalaryDetail.ResumeLayout(false);
            panelSalaryDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numTongLuong).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPhat).EndInit();
            ((System.ComponentModel.ISupportInitialize)numThuong).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPhuCap).EndInit();
            ((System.ComponentModel.ISupportInitialize)numLuongCoBan).EndInit();
            panelEmployee.ResumeLayout(false);
            panelEmployee.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelMain;
        private Panel panelEmployee;
        private Label lblEmployeeTitle;
        private TextBox txtMaNV;
        private Label lblMaNV;
        private TextBox txtTenNV;
        private Label lblTenNV;
        private TextBox txtChucVu;
        private Label lblChucVu;
        private Panel panelSalaryDetail;
        private Label lblSalaryDetailTitle;
        private TextBox txtSoNgay;
        private Label lblSoNgay;
        private TextBox txtTongGio;
        private Label lblTongGio;
        private NumericUpDown numLuongCoBan;
        private Label lblLuongCoBan;
        private NumericUpDown numPhuCap;
        private Label lblPhuCap;
        private NumericUpDown numThuong;
        private Label lblThuong;
        private NumericUpDown numPhat;
        private Label lblPhat;
        private NumericUpDown numTongLuong;
        private Label lblTongLuong;
        private TextBox txtGhiChu;
        private Label lblGhiChu;
        private Panel panelAttendance;
        private Label lblAttendanceTitle;
        private DataGridView dgvAttendance;
        private DataGridViewTextBoxColumn colNgay;
        private DataGridViewTextBoxColumn colGioVao;
        private DataGridViewTextBoxColumn colGioRa;
        private DataGridViewTextBoxColumn colSoGio;
        private DataGridViewTextBoxColumn colTrangThai;
        private Panel panelActions;
        private Button btnCancel;
        private Button btnSave;
    }
}