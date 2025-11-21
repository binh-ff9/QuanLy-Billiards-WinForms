namespace Billiard.WinForm.Forms.NhanVien
{
    partial class NhanVienForm
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
            panelHeader = new Panel();
            lblTitle = new Label();
            btnAdd = new Button();
            btnAttendance = new Button();
            btnSchedule = new Button();
            panelFilter = new Panel();
            lblFilterStatus = new Label();
            btnFilterAll = new Button();
            btnFilterActive = new Button();
            btnFilterInactive = new Button();
            lblFilterRole = new Label();
            btnRoleAll = new Button();
            btnRoleAdmin = new Button();
            btnRoleManager = new Button();
            btnRoleCashier = new Button();
            btnRoleStaff = new Button();
            txtSearch = new TextBox();
            panelContent = new Panel();
            flowLayoutEmployees = new FlowLayoutPanel();
            btnSalary = new Button();
            panelHeader.SuspendLayout();
            panelFilter.SuspendLayout();
            panelContent.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.White;
            panelHeader.Controls.Add(btnSalary);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Controls.Add(btnAdd);
            panelHeader.Controls.Add(btnAttendance);
            panelHeader.Controls.Add(btnSchedule);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Padding = new Padding(20, 15, 20, 15);
            panelHeader.Size = new Size(1385, 80);
            panelHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(26, 26, 46);
            lblTitle.Location = new Point(20, 22);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(385, 48);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "👔 Quản lý nhân viên";
            // 
            // btnAdd
            // 
            btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAdd.BackColor = Color.FromArgb(102, 126, 234);
            btnAdd.Cursor = Cursors.Hand;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAdd.ForeColor = Color.White;
            btnAdd.Location = new Point(721, 22);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(150, 42);
            btnAdd.TabIndex = 1;
            btnAdd.Text = "➕ Thêm NV";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnAttendance
            // 
            btnAttendance.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAttendance.BackColor = Color.FromArgb(40, 167, 69);
            btnAttendance.Cursor = Cursors.Hand;
            btnAttendance.FlatAppearance.BorderSize = 0;
            btnAttendance.FlatStyle = FlatStyle.Flat;
            btnAttendance.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAttendance.ForeColor = Color.White;
            btnAttendance.Location = new Point(561, 22);
            btnAttendance.Name = "btnAttendance";
            btnAttendance.Size = new Size(150, 42);
            btnAttendance.TabIndex = 2;
            btnAttendance.Text = "Chấm công";
            btnAttendance.UseVisualStyleBackColor = false;
            btnAttendance.Click += btnAttendance_Click;
            // 
            // btnSchedule
            // 
            btnSchedule.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSchedule.BackColor = Color.FromArgb(255, 193, 7);
            btnSchedule.Cursor = Cursors.Hand;
            btnSchedule.FlatAppearance.BorderSize = 0;
            btnSchedule.FlatStyle = FlatStyle.Flat;
            btnSchedule.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSchedule.ForeColor = Color.Black;
            btnSchedule.Location = new Point(401, 22);
            btnSchedule.Name = "btnSchedule";
            btnSchedule.Size = new Size(150, 42);
            btnSchedule.TabIndex = 3;
            btnSchedule.Text = "📅 Ca làm";
            btnSchedule.UseVisualStyleBackColor = false;
            btnSchedule.Click += btnSchedule_Click;
            // 
            // panelFilter
            // 
            panelFilter.BackColor = Color.White;
            panelFilter.Controls.Add(lblFilterStatus);
            panelFilter.Controls.Add(btnFilterAll);
            panelFilter.Controls.Add(btnFilterActive);
            panelFilter.Controls.Add(btnFilterInactive);
            panelFilter.Controls.Add(lblFilterRole);
            panelFilter.Controls.Add(btnRoleAll);
            panelFilter.Controls.Add(btnRoleAdmin);
            panelFilter.Controls.Add(btnRoleManager);
            panelFilter.Controls.Add(btnRoleCashier);
            panelFilter.Controls.Add(btnRoleStaff);
            panelFilter.Controls.Add(txtSearch);
            panelFilter.Dock = DockStyle.Top;
            panelFilter.Location = new Point(0, 80);
            panelFilter.Name = "panelFilter";
            panelFilter.Padding = new Padding(20, 15, 20, 15);
            panelFilter.Size = new Size(1385, 150);
            panelFilter.TabIndex = 1;
            // 
            // lblFilterStatus
            // 
            lblFilterStatus.AutoSize = true;
            lblFilterStatus.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblFilterStatus.Location = new Point(40, 8);
            lblFilterStatus.Name = "lblFilterStatus";
            lblFilterStatus.Size = new Size(113, 28);
            lblFilterStatus.TabIndex = 0;
            lblFilterStatus.Text = "Trạng thái:";
            // 
            // btnFilterAll
            // 
            btnFilterAll.BackColor = Color.FromArgb(102, 126, 234);
            btnFilterAll.Cursor = Cursors.Hand;
            btnFilterAll.FlatAppearance.BorderSize = 0;
            btnFilterAll.FlatStyle = FlatStyle.Flat;
            btnFilterAll.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnFilterAll.ForeColor = Color.White;
            btnFilterAll.Location = new Point(183, 6);
            btnFilterAll.Name = "btnFilterAll";
            btnFilterAll.Size = new Size(100, 35);
            btnFilterAll.TabIndex = 1;
            btnFilterAll.Tag = "all";
            btnFilterAll.Text = "Tất cả";
            btnFilterAll.UseVisualStyleBackColor = false;
            btnFilterAll.Click += FilterStatus_Click;
            // 
            // btnFilterActive
            // 
            btnFilterActive.BackColor = Color.FromArgb(233, 236, 239);
            btnFilterActive.Cursor = Cursors.Hand;
            btnFilterActive.FlatAppearance.BorderSize = 0;
            btnFilterActive.FlatStyle = FlatStyle.Flat;
            btnFilterActive.Font = new Font("Segoe UI", 9F);
            btnFilterActive.Location = new Point(293, 6);
            btnFilterActive.Name = "btnFilterActive";
            btnFilterActive.Size = new Size(100, 35);
            btnFilterActive.TabIndex = 2;
            btnFilterActive.Tag = "DangLam";
            btnFilterActive.Text = "Đang làm";
            btnFilterActive.UseVisualStyleBackColor = false;
            btnFilterActive.Click += FilterStatus_Click;
            // 
            // btnFilterInactive
            // 
            btnFilterInactive.BackColor = Color.FromArgb(233, 236, 239);
            btnFilterInactive.Cursor = Cursors.Hand;
            btnFilterInactive.FlatAppearance.BorderSize = 0;
            btnFilterInactive.FlatStyle = FlatStyle.Flat;
            btnFilterInactive.Font = new Font("Segoe UI", 9F);
            btnFilterInactive.Location = new Point(403, 6);
            btnFilterInactive.Name = "btnFilterInactive";
            btnFilterInactive.Size = new Size(100, 35);
            btnFilterInactive.TabIndex = 3;
            btnFilterInactive.Tag = "Nghi";
            btnFilterInactive.Text = "Nghỉ việc";
            btnFilterInactive.UseVisualStyleBackColor = false;
            btnFilterInactive.Click += FilterStatus_Click;
            // 
            // lblFilterRole
            // 
            lblFilterRole.AutoSize = true;
            lblFilterRole.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblFilterRole.Location = new Point(40, 53);
            lblFilterRole.Name = "lblFilterRole";
            lblFilterRole.Size = new Size(139, 28);
            lblFilterRole.TabIndex = 4;
            lblFilterRole.Text = "Nhóm quyền:";
            // 
            // btnRoleAll
            // 
            btnRoleAll.BackColor = Color.FromArgb(102, 126, 234);
            btnRoleAll.Cursor = Cursors.Hand;
            btnRoleAll.FlatAppearance.BorderSize = 0;
            btnRoleAll.FlatStyle = FlatStyle.Flat;
            btnRoleAll.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRoleAll.ForeColor = Color.White;
            btnRoleAll.Location = new Point(183, 51);
            btnRoleAll.Name = "btnRoleAll";
            btnRoleAll.Size = new Size(100, 35);
            btnRoleAll.TabIndex = 5;
            btnRoleAll.Tag = "all";
            btnRoleAll.Text = "Tất cả";
            btnRoleAll.UseVisualStyleBackColor = false;
            btnRoleAll.Click += FilterRole_Click;
            // 
            // btnRoleAdmin
            // 
            btnRoleAdmin.BackColor = Color.FromArgb(233, 236, 239);
            btnRoleAdmin.Cursor = Cursors.Hand;
            btnRoleAdmin.FlatAppearance.BorderSize = 0;
            btnRoleAdmin.FlatStyle = FlatStyle.Flat;
            btnRoleAdmin.Font = new Font("Segoe UI", 9F);
            btnRoleAdmin.Location = new Point(293, 51);
            btnRoleAdmin.Name = "btnRoleAdmin";
            btnRoleAdmin.Size = new Size(100, 35);
            btnRoleAdmin.TabIndex = 6;
            btnRoleAdmin.Tag = "Admin";
            btnRoleAdmin.Text = "Admin";
            btnRoleAdmin.UseVisualStyleBackColor = false;
            btnRoleAdmin.Click += FilterRole_Click;
            // 
            // btnRoleManager
            // 
            btnRoleManager.BackColor = Color.FromArgb(233, 236, 239);
            btnRoleManager.Cursor = Cursors.Hand;
            btnRoleManager.FlatAppearance.BorderSize = 0;
            btnRoleManager.FlatStyle = FlatStyle.Flat;
            btnRoleManager.Font = new Font("Segoe UI", 9F);
            btnRoleManager.Location = new Point(403, 51);
            btnRoleManager.Name = "btnRoleManager";
            btnRoleManager.Size = new Size(100, 35);
            btnRoleManager.TabIndex = 7;
            btnRoleManager.Tag = "Quản lý";
            btnRoleManager.Text = "Quản lý";
            btnRoleManager.UseVisualStyleBackColor = false;
            btnRoleManager.Click += FilterRole_Click;
            // 
            // btnRoleCashier
            // 
            btnRoleCashier.BackColor = Color.FromArgb(233, 236, 239);
            btnRoleCashier.Cursor = Cursors.Hand;
            btnRoleCashier.FlatAppearance.BorderSize = 0;
            btnRoleCashier.FlatStyle = FlatStyle.Flat;
            btnRoleCashier.Font = new Font("Segoe UI", 9F);
            btnRoleCashier.Location = new Point(513, 51);
            btnRoleCashier.Name = "btnRoleCashier";
            btnRoleCashier.Size = new Size(100, 35);
            btnRoleCashier.TabIndex = 8;
            btnRoleCashier.Tag = "Thu ngân";
            btnRoleCashier.Text = "Thu ngân";
            btnRoleCashier.UseVisualStyleBackColor = false;
            btnRoleCashier.Click += FilterRole_Click;
            // 
            // btnRoleStaff
            // 
            btnRoleStaff.BackColor = Color.FromArgb(233, 236, 239);
            btnRoleStaff.Cursor = Cursors.Hand;
            btnRoleStaff.FlatAppearance.BorderSize = 0;
            btnRoleStaff.FlatStyle = FlatStyle.Flat;
            btnRoleStaff.Font = new Font("Segoe UI", 9F);
            btnRoleStaff.Location = new Point(623, 51);
            btnRoleStaff.Name = "btnRoleStaff";
            btnRoleStaff.Size = new Size(100, 35);
            btnRoleStaff.TabIndex = 9;
            btnRoleStaff.Tag = "Phục vụ";
            btnRoleStaff.Text = "Phục vụ";
            btnRoleStaff.UseVisualStyleBackColor = false;
            btnRoleStaff.Click += FilterRole_Click;
            // 
            // txtSearch
            // 
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSearch.Font = new Font("Segoe UI", 11F);
            txtSearch.Location = new Point(20, 105);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "🔍 Tìm kiếm theo tên, số điện thoại...";
            txtSearch.Size = new Size(570, 37);
            txtSearch.TabIndex = 10;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // panelContent
            // 
            panelContent.AutoScroll = true;
            panelContent.BackColor = Color.FromArgb(248, 249, 250);
            panelContent.Controls.Add(flowLayoutEmployees);
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(0, 230);
            panelContent.Name = "panelContent";
            panelContent.Padding = new Padding(20);
            panelContent.Size = new Size(1385, 520);
            panelContent.TabIndex = 2;
            // 
            // flowLayoutEmployees
            // 
            flowLayoutEmployees.AutoScroll = true;
            flowLayoutEmployees.Dock = DockStyle.Fill;
            flowLayoutEmployees.Location = new Point(20, 20);
            flowLayoutEmployees.Name = "flowLayoutEmployees";
            flowLayoutEmployees.Padding = new Padding(10);
            flowLayoutEmployees.Size = new Size(1345, 480);
            flowLayoutEmployees.TabIndex = 0;
            // 
            // btnSalary
            // 
            btnSalary.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSalary.BackColor = Color.FromArgb(0, 192, 192);
            btnSalary.Cursor = Cursors.Hand;
            btnSalary.FlatAppearance.BorderSize = 0;
            btnSalary.FlatStyle = FlatStyle.Flat;
            btnSalary.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSalary.ForeColor = Color.Black;
            btnSalary.Location = new Point(245, 20);
            btnSalary.Name = "btnSalary";
            btnSalary.Size = new Size(150, 42);
            btnSalary.TabIndex = 4;
            btnSalary.Text = "Lương";
            btnSalary.UseVisualStyleBackColor = false;
            btnSalary.Click += btnSalary_Click;
            // 
            // NhanVienForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1385, 750);
            Controls.Add(panelContent);
            Controls.Add(panelFilter);
            Controls.Add(panelHeader);
            Font = new Font("Segoe UI", 9F);
            Name = "NhanVienForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản lý nhân viên";
            Load += NhanVienForm_Load;
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelFilter.ResumeLayout(false);
            panelFilter.PerformLayout();
            panelContent.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnAttendance;
        private System.Windows.Forms.Button btnSchedule;
        private System.Windows.Forms.Panel panelFilter;
        private System.Windows.Forms.Label lblFilterStatus;
        private System.Windows.Forms.Button btnFilterAll;
        private System.Windows.Forms.Button btnFilterActive;
        private System.Windows.Forms.Button btnFilterInactive;
        private System.Windows.Forms.Label lblFilterRole;
        private System.Windows.Forms.Button btnRoleAll;
        private System.Windows.Forms.Button btnRoleAdmin;
        private System.Windows.Forms.Button btnRoleManager;
        private System.Windows.Forms.Button btnRoleCashier;
        private System.Windows.Forms.Button btnRoleStaff;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutEmployees;
        private Button btnSalary;
    }
}