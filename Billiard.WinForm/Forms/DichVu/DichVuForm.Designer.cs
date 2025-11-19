namespace Billiard.WinForm.Forms
{
    partial class DichVuForm
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
            headerPanel = new Panel();
            btnAdd = new Button();
            lblTitle = new Label();
            filterPanel = new Panel();
            txtSearch = new TextBox();
            cboFilter = new ComboBox();
            lblFilter = new Label();
            servicesContainer = new Panel();
            flpServices = new FlowLayoutPanel();
            headerPanel.SuspendLayout();
            filterPanel.SuspendLayout();
            servicesContainer.SuspendLayout();
            SuspendLayout();
            // 
            // headerPanel
            // 
            headerPanel.BackColor = Color.FromArgb(102, 126, 234);
            headerPanel.Controls.Add(btnAdd);
            headerPanel.Controls.Add(lblTitle);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Location = new Point(0, 0);
            headerPanel.Name = "headerPanel";
            headerPanel.Padding = new Padding(20);
            headerPanel.Size = new Size(1200, 80);
            headerPanel.TabIndex = 0;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.White;
            btnAdd.Cursor = Cursors.Hand;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAdd.ForeColor = Color.FromArgb(102, 126, 234);
            btnAdd.Location = new Point(1000, 20);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(180, 40);
            btnAdd.TabIndex = 1;
            btnAdd.Text = "➕ Thêm dịch vụ mới";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += BtnAdd_Click;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 25);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(180, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Dịch vụ & Menu";
            // 
            // filterPanel
            // 
            filterPanel.BackColor = Color.White;
            filterPanel.Controls.Add(txtSearch);
            filterPanel.Controls.Add(cboFilter);
            filterPanel.Controls.Add(lblFilter);
            filterPanel.Dock = DockStyle.Top;
            filterPanel.Location = new Point(0, 80);
            filterPanel.Name = "filterPanel";
            filterPanel.Padding = new Padding(20, 15, 20, 15);
            filterPanel.Size = new Size(1200, 120);
            filterPanel.TabIndex = 1;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 11F);
            txtSearch.Location = new Point(20, 60);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "🔍 Tìm kiếm dịch vụ...";
            txtSearch.Size = new Size(1140, 27);
            txtSearch.TabIndex = 2;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            // 
            // cboFilter
            // 
            cboFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboFilter.Font = new Font("Segoe UI", 10F);
            cboFilter.FormattingEnabled = true;
            cboFilter.Items.AddRange(new object[] { "Tất cả", "Đồ uống", "Đồ ăn", "Khác" });
            cboFilter.Location = new Point(120, 17);
            cboFilter.Name = "cboFilter";
            cboFilter.Size = new Size(200, 25);
            cboFilter.TabIndex = 1;
            cboFilter.SelectedIndexChanged += CboFilter_SelectedIndexChanged;
            // 
            // lblFilter
            // 
            lblFilter.AutoSize = true;
            lblFilter.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblFilter.Location = new Point(20, 20);
            lblFilter.Name = "lblFilter";
            lblFilter.Size = new Size(93, 19);
            lblFilter.TabIndex = 0;
            lblFilter.Text = "Loại dịch vụ:";
            // 
            // servicesContainer
            // 
            servicesContainer.AutoScroll = true;
            servicesContainer.BackColor = Color.FromArgb(240, 242, 245);
            servicesContainer.Controls.Add(flpServices);
            servicesContainer.Dock = DockStyle.Fill;
            servicesContainer.Location = new Point(0, 200);
            servicesContainer.Name = "servicesContainer";
            servicesContainer.Padding = new Padding(20);
            servicesContainer.Size = new Size(1200, 550);
            servicesContainer.TabIndex = 2;
            // 
            // flpServices
            // 
            flpServices.AutoScroll = true;
            flpServices.BackColor = Color.FromArgb(240, 242, 245);
            flpServices.Dock = DockStyle.Fill;
            flpServices.Location = new Point(20, 20);
            flpServices.Name = "flpServices";
            flpServices.Size = new Size(1160, 510);
            flpServices.TabIndex = 0;
            flpServices.Paint += flpServices_Paint;
            // 
            // DichVuForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 750);
            Controls.Add(servicesContainer);
            Controls.Add(filterPanel);
            Controls.Add(headerPanel);
            Name = "DichVuForm";
            Text = "Quản lý dịch vụ";
            Load += DichVuForm_Load;
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            filterPanel.ResumeLayout(false);
            filterPanel.PerformLayout();
            servicesContainer.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel filterPanel;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ComboBox cboFilter;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.Panel servicesContainer;
        private System.Windows.Forms.FlowLayoutPanel flpServices;
    }
}