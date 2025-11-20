namespace Billiard.WinForm.Forms.QLBan
{
    partial class QLBanForm
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
            pnlToolbar = new Panel();
            btnThemBan = new Button();
            btnDatBan = new Button();
            btnXemBanDat = new Button();
            btnXemSoDo = new Button();
            pnlFilters = new Panel();
            txtSearch = new TextBox();
            lblLoaiBan = new Label();
            pnlLoaiBanFilters = new FlowLayoutPanel();
            btnTypeAll = new Button();
            btnTypeLo9Bi = new Button();
            btnTypePhangCarom = new Button();
            btnTypeSnooker = new Button();
            btnTypeVIPLo = new Button();
            btnTypeVIPPhang = new Button();
            lblTrangThai = new Label();
            pnlTrangThaiFilters = new FlowLayoutPanel();
            btnStatusAll = new Button();
            btnStatusTrong = new Button();
            btnStatusDangChoi = new Button();
            btnStatusDaDat = new Button();
            lblKhuVuc = new Label();
            pnlKhuVucFilters = new FlowLayoutPanel();
            btnFilterAll = new Button();
            btnFilterTang1 = new Button();
            btnFilterTang2 = new Button();
            btnFilterVIP = new Button();
            flpBanBia = new FlowLayoutPanel();
            pnlToolbar.SuspendLayout();
            pnlFilters.SuspendLayout();
            pnlLoaiBanFilters.SuspendLayout();
            pnlTrangThaiFilters.SuspendLayout();
            pnlKhuVucFilters.SuspendLayout();
            SuspendLayout();
            // 
            // pnlToolbar
            // 
            pnlToolbar.BackColor = Color.FromArgb(248, 250, 252);
            pnlToolbar.Controls.Add(btnThemBan);
            pnlToolbar.Controls.Add(btnDatBan);
            pnlToolbar.Controls.Add(btnXemBanDat);
            pnlToolbar.Controls.Add(btnXemSoDo);
            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Location = new Point(0, 0);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Padding = new Padding(15);
            pnlToolbar.Size = new Size(1095, 70);
            pnlToolbar.TabIndex = 0;
            // 
            // btnThemBan
            // 
            btnThemBan.BackColor = Color.FromArgb(34, 197, 94);
            btnThemBan.Cursor = Cursors.Hand;
            btnThemBan.FlatAppearance.BorderSize = 0;
            btnThemBan.FlatStyle = FlatStyle.Flat;
            btnThemBan.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnThemBan.ForeColor = Color.White;
            btnThemBan.Location = new Point(676, 15);
            btnThemBan.Name = "btnThemBan";
            btnThemBan.Size = new Size(191, 40);
            btnThemBan.TabIndex = 3;
            btnThemBan.Text = "➕ Thêm bàn mới";
            btnThemBan.UseVisualStyleBackColor = false;
            btnThemBan.Click += BtnThemBan_Click;
            // 
            // btnDatBan
            // 
            btnDatBan.BackColor = Color.FromArgb(234, 179, 8);
            btnDatBan.Cursor = Cursors.Hand;
            btnDatBan.FlatAppearance.BorderSize = 0;
            btnDatBan.FlatStyle = FlatStyle.Flat;
            btnDatBan.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDatBan.ForeColor = Color.White;
            btnDatBan.Location = new Point(464, 15);
            btnDatBan.Name = "btnDatBan";
            btnDatBan.Size = new Size(191, 40);
            btnDatBan.TabIndex = 2;
            btnDatBan.Text = "📅 Đặt bàn trước";
            btnDatBan.UseVisualStyleBackColor = false;
            btnDatBan.Click += BtnDatBan_Click;
            // 
            // btnXemBanDat
            // 
            btnXemBanDat.BackColor = Color.FromArgb(59, 130, 246);
            btnXemBanDat.Cursor = Cursors.Hand;
            btnXemBanDat.FlatAppearance.BorderSize = 0;
            btnXemBanDat.FlatStyle = FlatStyle.Flat;
            btnXemBanDat.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnXemBanDat.ForeColor = Color.White;
            btnXemBanDat.Location = new Point(214, 15);
            btnXemBanDat.Name = "btnXemBanDat";
            btnXemBanDat.Size = new Size(229, 40);
            btnXemBanDat.TabIndex = 1;
            btnXemBanDat.Text = "📅 Xem bàn đặt";
            btnXemBanDat.UseVisualStyleBackColor = false;
            btnXemBanDat.Click += BtnXemBanDat_Click;
            // 
            // btnXemSoDo
            // 
            btnXemSoDo.BackColor = Color.FromArgb(100, 116, 139);
            btnXemSoDo.Cursor = Cursors.Hand;
            btnXemSoDo.FlatAppearance.BorderSize = 0;
            btnXemSoDo.FlatStyle = FlatStyle.Flat;
            btnXemSoDo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnXemSoDo.ForeColor = Color.White;
            btnXemSoDo.Location = new Point(15, 15);
            btnXemSoDo.Name = "btnXemSoDo";
            btnXemSoDo.Size = new Size(181, 40);
            btnXemSoDo.TabIndex = 0;
            btnXemSoDo.Text = "🗺️ Xem sơ đồ";
            btnXemSoDo.UseVisualStyleBackColor = false;
            btnXemSoDo.Click += BtnXemSoDo_Click;
            // 
            // pnlFilters
            // 
            pnlFilters.BackColor = Color.White;
            pnlFilters.Controls.Add(txtSearch);
            pnlFilters.Controls.Add(lblLoaiBan);
            pnlFilters.Controls.Add(pnlLoaiBanFilters);
            pnlFilters.Controls.Add(lblTrangThai);
            pnlFilters.Controls.Add(pnlTrangThaiFilters);
            pnlFilters.Controls.Add(lblKhuVuc);
            pnlFilters.Controls.Add(pnlKhuVucFilters);
            pnlFilters.Dock = DockStyle.Top;
            pnlFilters.Location = new Point(0, 70);
            pnlFilters.Name = "pnlFilters";
            pnlFilters.Padding = new Padding(15);
            pnlFilters.Size = new Size(1095, 152);
            pnlFilters.TabIndex = 1;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 11F);
            txtSearch.Location = new Point(755, 83);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "🔍 Tìm kiếm bàn...";
            txtSearch.Size = new Size(299, 37);
            txtSearch.TabIndex = 6;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            // 
            // lblLoaiBan
            // 
            lblLoaiBan.AutoSize = true;
            lblLoaiBan.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblLoaiBan.Location = new Point(15, 87);
            lblLoaiBan.Name = "lblLoaiBan";
            lblLoaiBan.Size = new Size(97, 28);
            lblLoaiBan.TabIndex = 4;
            lblLoaiBan.Text = "Loại bàn:";
            // 
            // pnlLoaiBanFilters
            // 
            pnlLoaiBanFilters.Controls.Add(btnTypeAll);
            pnlLoaiBanFilters.Controls.Add(btnTypeLo9Bi);
            pnlLoaiBanFilters.Controls.Add(btnTypePhangCarom);
            pnlLoaiBanFilters.Controls.Add(btnTypeSnooker);
            pnlLoaiBanFilters.Controls.Add(btnTypeVIPLo);
            pnlLoaiBanFilters.Controls.Add(btnTypeVIPPhang);
            pnlLoaiBanFilters.Location = new Point(115, 82);
            pnlLoaiBanFilters.Name = "pnlLoaiBanFilters";
            pnlLoaiBanFilters.Size = new Size(634, 51);
            pnlLoaiBanFilters.TabIndex = 5;
            // 
            // btnTypeAll
            // 
            btnTypeAll.BackColor = Color.FromArgb(99, 102, 241);
            btnTypeAll.Cursor = Cursors.Hand;
            btnTypeAll.FlatAppearance.BorderSize = 0;
            btnTypeAll.FlatStyle = FlatStyle.Flat;
            btnTypeAll.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTypeAll.ForeColor = Color.White;
            btnTypeAll.Location = new Point(3, 3);
            btnTypeAll.Name = "btnTypeAll";
            btnTypeAll.Size = new Size(90, 35);
            btnTypeAll.TabIndex = 0;
            btnTypeAll.Tag = "all";
            btnTypeAll.Text = "Tất cả";
            btnTypeAll.UseVisualStyleBackColor = false;
            btnTypeAll.Click += TypeFilterButton_Click;
            // 
            // btnTypeLo9Bi
            // 
            btnTypeLo9Bi.BackColor = Color.FromArgb(226, 232, 240);
            btnTypeLo9Bi.Cursor = Cursors.Hand;
            btnTypeLo9Bi.FlatAppearance.BorderSize = 0;
            btnTypeLo9Bi.FlatStyle = FlatStyle.Flat;
            btnTypeLo9Bi.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTypeLo9Bi.ForeColor = Color.FromArgb(51, 65, 85);
            btnTypeLo9Bi.Location = new Point(99, 3);
            btnTypeLo9Bi.Name = "btnTypeLo9Bi";
            btnTypeLo9Bi.Size = new Size(100, 35);
            btnTypeLo9Bi.TabIndex = 1;
            btnTypeLo9Bi.Tag = "Bàn Lỗ 9 bi";
            btnTypeLo9Bi.Text = "Lỗ 9 bi";
            btnTypeLo9Bi.UseVisualStyleBackColor = false;
            btnTypeLo9Bi.Click += TypeFilterButton_Click;
            // 
            // btnTypePhangCarom
            // 
            btnTypePhangCarom.BackColor = Color.FromArgb(226, 232, 240);
            btnTypePhangCarom.Cursor = Cursors.Hand;
            btnTypePhangCarom.FlatAppearance.BorderSize = 0;
            btnTypePhangCarom.FlatStyle = FlatStyle.Flat;
            btnTypePhangCarom.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTypePhangCarom.ForeColor = Color.FromArgb(51, 65, 85);
            btnTypePhangCarom.Location = new Point(205, 3);
            btnTypePhangCarom.Name = "btnTypePhangCarom";
            btnTypePhangCarom.Size = new Size(120, 35);
            btnTypePhangCarom.TabIndex = 2;
            btnTypePhangCarom.Tag = "Bàn Phăng Carom";
            btnTypePhangCarom.Text = "Phăng Carom";
            btnTypePhangCarom.UseVisualStyleBackColor = false;
            btnTypePhangCarom.Click += TypeFilterButton_Click;
            // 
            // btnTypeSnooker
            // 
            btnTypeSnooker.BackColor = Color.FromArgb(226, 232, 240);
            btnTypeSnooker.Cursor = Cursors.Hand;
            btnTypeSnooker.FlatAppearance.BorderSize = 0;
            btnTypeSnooker.FlatStyle = FlatStyle.Flat;
            btnTypeSnooker.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTypeSnooker.ForeColor = Color.FromArgb(51, 65, 85);
            btnTypeSnooker.Location = new Point(331, 3);
            btnTypeSnooker.Name = "btnTypeSnooker";
            btnTypeSnooker.Size = new Size(90, 35);
            btnTypeSnooker.TabIndex = 3;
            btnTypeSnooker.Tag = "Bàn Snooker";
            btnTypeSnooker.Text = "Snooker";
            btnTypeSnooker.UseVisualStyleBackColor = false;
            btnTypeSnooker.Click += TypeFilterButton_Click;
            // 
            // btnTypeVIPLo
            // 
            btnTypeVIPLo.BackColor = Color.FromArgb(226, 232, 240);
            btnTypeVIPLo.Cursor = Cursors.Hand;
            btnTypeVIPLo.FlatAppearance.BorderSize = 0;
            btnTypeVIPLo.FlatStyle = FlatStyle.Flat;
            btnTypeVIPLo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTypeVIPLo.ForeColor = Color.FromArgb(51, 65, 85);
            btnTypeVIPLo.Location = new Point(427, 3);
            btnTypeVIPLo.Name = "btnTypeVIPLo";
            btnTypeVIPLo.Size = new Size(90, 35);
            btnTypeVIPLo.TabIndex = 4;
            btnTypeVIPLo.Tag = "Bàn VIP Lỗ";
            btnTypeVIPLo.Text = "VIP Lỗ";
            btnTypeVIPLo.UseVisualStyleBackColor = false;
            btnTypeVIPLo.Click += TypeFilterButton_Click;
            // 
            // btnTypeVIPPhang
            // 
            btnTypeVIPPhang.BackColor = Color.FromArgb(226, 232, 240);
            btnTypeVIPPhang.Cursor = Cursors.Hand;
            btnTypeVIPPhang.FlatAppearance.BorderSize = 0;
            btnTypeVIPPhang.FlatStyle = FlatStyle.Flat;
            btnTypeVIPPhang.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTypeVIPPhang.ForeColor = Color.FromArgb(51, 65, 85);
            btnTypeVIPPhang.Location = new Point(523, 3);
            btnTypeVIPPhang.Name = "btnTypeVIPPhang";
            btnTypeVIPPhang.Size = new Size(100, 35);
            btnTypeVIPPhang.TabIndex = 5;
            btnTypeVIPPhang.Tag = "Bàn VIP Phăng";
            btnTypeVIPPhang.Text = "VIP Phăng";
            btnTypeVIPPhang.UseVisualStyleBackColor = false;
            btnTypeVIPPhang.Click += TypeFilterButton_Click;
            // 
            // lblTrangThai
            // 
            lblTrangThai.AutoSize = true;
            lblTrangThai.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTrangThai.Location = new Point(522, 15);
            lblTrangThai.Name = "lblTrangThai";
            lblTrangThai.Size = new Size(113, 28);
            lblTrangThai.TabIndex = 2;
            lblTrangThai.Text = "Trạng thái:";
            // 
            // pnlTrangThaiFilters
            // 
            pnlTrangThaiFilters.Controls.Add(btnStatusAll);
            pnlTrangThaiFilters.Controls.Add(btnStatusTrong);
            pnlTrangThaiFilters.Controls.Add(btnStatusDangChoi);
            pnlTrangThaiFilters.Controls.Add(btnStatusDaDat);
            pnlTrangThaiFilters.Location = new Point(641, 10);
            pnlTrangThaiFilters.Name = "pnlTrangThaiFilters";
            pnlTrangThaiFilters.Size = new Size(413, 47);
            pnlTrangThaiFilters.TabIndex = 3;
            // 
            // btnStatusAll
            // 
            btnStatusAll.BackColor = Color.FromArgb(99, 102, 241);
            btnStatusAll.Cursor = Cursors.Hand;
            btnStatusAll.FlatAppearance.BorderSize = 0;
            btnStatusAll.FlatStyle = FlatStyle.Flat;
            btnStatusAll.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnStatusAll.ForeColor = Color.White;
            btnStatusAll.Location = new Point(3, 3);
            btnStatusAll.Name = "btnStatusAll";
            btnStatusAll.Size = new Size(90, 35);
            btnStatusAll.TabIndex = 0;
            btnStatusAll.Tag = "all";
            btnStatusAll.Text = "Tất cả";
            btnStatusAll.UseVisualStyleBackColor = false;
            btnStatusAll.Click += StatusFilterButton_Click;
            // 
            // btnStatusTrong
            // 
            btnStatusTrong.BackColor = Color.FromArgb(226, 232, 240);
            btnStatusTrong.Cursor = Cursors.Hand;
            btnStatusTrong.FlatAppearance.BorderSize = 0;
            btnStatusTrong.FlatStyle = FlatStyle.Flat;
            btnStatusTrong.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnStatusTrong.ForeColor = Color.FromArgb(51, 65, 85);
            btnStatusTrong.Location = new Point(99, 3);
            btnStatusTrong.Name = "btnStatusTrong";
            btnStatusTrong.Size = new Size(90, 35);
            btnStatusTrong.TabIndex = 1;
            btnStatusTrong.Tag = "Trống";
            btnStatusTrong.Text = "Trống";
            btnStatusTrong.UseVisualStyleBackColor = false;
            btnStatusTrong.Click += StatusFilterButton_Click;
            // 
            // btnStatusDangChoi
            // 
            btnStatusDangChoi.BackColor = Color.FromArgb(226, 232, 240);
            btnStatusDangChoi.Cursor = Cursors.Hand;
            btnStatusDangChoi.FlatAppearance.BorderSize = 0;
            btnStatusDangChoi.FlatStyle = FlatStyle.Flat;
            btnStatusDangChoi.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnStatusDangChoi.ForeColor = Color.FromArgb(51, 65, 85);
            btnStatusDangChoi.Location = new Point(195, 3);
            btnStatusDangChoi.Name = "btnStatusDangChoi";
            btnStatusDangChoi.Size = new Size(100, 35);
            btnStatusDangChoi.TabIndex = 2;
            btnStatusDangChoi.Tag = "Đang chơi";
            btnStatusDangChoi.Text = "Đang chơi";
            btnStatusDangChoi.UseVisualStyleBackColor = false;
            btnStatusDangChoi.Click += StatusFilterButton_Click;
            // 
            // btnStatusDaDat
            // 
            btnStatusDaDat.BackColor = Color.FromArgb(226, 232, 240);
            btnStatusDaDat.Cursor = Cursors.Hand;
            btnStatusDaDat.FlatAppearance.BorderSize = 0;
            btnStatusDaDat.FlatStyle = FlatStyle.Flat;
            btnStatusDaDat.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnStatusDaDat.ForeColor = Color.FromArgb(51, 65, 85);
            btnStatusDaDat.Location = new Point(301, 3);
            btnStatusDaDat.Name = "btnStatusDaDat";
            btnStatusDaDat.Size = new Size(90, 35);
            btnStatusDaDat.TabIndex = 3;
            btnStatusDaDat.Tag = "Đã đặt";
            btnStatusDaDat.Text = "Đã đặt";
            btnStatusDaDat.UseVisualStyleBackColor = false;
            btnStatusDaDat.Click += StatusFilterButton_Click;
            // 
            // lblKhuVuc
            // 
            lblKhuVuc.AutoSize = true;
            lblKhuVuc.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblKhuVuc.Location = new Point(15, 15);
            lblKhuVuc.Name = "lblKhuVuc";
            lblKhuVuc.Size = new Size(94, 28);
            lblKhuVuc.TabIndex = 0;
            lblKhuVuc.Text = "Khu vực:";
            // 
            // pnlKhuVucFilters
            // 
            pnlKhuVucFilters.Controls.Add(btnFilterAll);
            pnlKhuVucFilters.Controls.Add(btnFilterTang1);
            pnlKhuVucFilters.Controls.Add(btnFilterTang2);
            pnlKhuVucFilters.Controls.Add(btnFilterVIP);
            pnlKhuVucFilters.Location = new Point(115, 10);
            pnlKhuVucFilters.Name = "pnlKhuVucFilters";
            pnlKhuVucFilters.Size = new Size(397, 47);
            pnlKhuVucFilters.TabIndex = 1;
            // 
            // btnFilterAll
            // 
            btnFilterAll.BackColor = Color.FromArgb(99, 102, 241);
            btnFilterAll.Cursor = Cursors.Hand;
            btnFilterAll.FlatAppearance.BorderSize = 0;
            btnFilterAll.FlatStyle = FlatStyle.Flat;
            btnFilterAll.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnFilterAll.ForeColor = Color.White;
            btnFilterAll.Location = new Point(3, 3);
            btnFilterAll.Name = "btnFilterAll";
            btnFilterAll.Size = new Size(90, 35);
            btnFilterAll.TabIndex = 0;
            btnFilterAll.Tag = "all";
            btnFilterAll.Text = "Tất cả";
            btnFilterAll.UseVisualStyleBackColor = false;
            btnFilterAll.Click += FilterButton_Click;
            // 
            // btnFilterTang1
            // 
            btnFilterTang1.BackColor = Color.FromArgb(226, 232, 240);
            btnFilterTang1.Cursor = Cursors.Hand;
            btnFilterTang1.FlatAppearance.BorderSize = 0;
            btnFilterTang1.FlatStyle = FlatStyle.Flat;
            btnFilterTang1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnFilterTang1.ForeColor = Color.FromArgb(51, 65, 85);
            btnFilterTang1.Location = new Point(99, 3);
            btnFilterTang1.Name = "btnFilterTang1";
            btnFilterTang1.Size = new Size(90, 35);
            btnFilterTang1.TabIndex = 1;
            btnFilterTang1.Tag = "Tầng 1";
            btnFilterTang1.Text = "Tầng 1";
            btnFilterTang1.UseVisualStyleBackColor = false;
            btnFilterTang1.Click += FilterButton_Click;
            // 
            // btnFilterTang2
            // 
            btnFilterTang2.BackColor = Color.FromArgb(226, 232, 240);
            btnFilterTang2.Cursor = Cursors.Hand;
            btnFilterTang2.FlatAppearance.BorderSize = 0;
            btnFilterTang2.FlatStyle = FlatStyle.Flat;
            btnFilterTang2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnFilterTang2.ForeColor = Color.FromArgb(51, 65, 85);
            btnFilterTang2.Location = new Point(195, 3);
            btnFilterTang2.Name = "btnFilterTang2";
            btnFilterTang2.Size = new Size(90, 35);
            btnFilterTang2.TabIndex = 2;
            btnFilterTang2.Tag = "Tầng 2";
            btnFilterTang2.Text = "Tầng 2";
            btnFilterTang2.UseVisualStyleBackColor = false;
            btnFilterTang2.Click += FilterButton_Click;
            // 
            // btnFilterVIP
            // 
            btnFilterVIP.BackColor = Color.FromArgb(226, 232, 240);
            btnFilterVIP.Cursor = Cursors.Hand;
            btnFilterVIP.FlatAppearance.BorderSize = 0;
            btnFilterVIP.FlatStyle = FlatStyle.Flat;
            btnFilterVIP.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnFilterVIP.ForeColor = Color.FromArgb(51, 65, 85);
            btnFilterVIP.Location = new Point(291, 3);
            btnFilterVIP.Name = "btnFilterVIP";
            btnFilterVIP.Size = new Size(90, 35);
            btnFilterVIP.TabIndex = 3;
            btnFilterVIP.Tag = "VIP";
            btnFilterVIP.Text = "VIP";
            btnFilterVIP.UseVisualStyleBackColor = false;
            btnFilterVIP.Click += FilterButton_Click;
            // 
            // flpBanBia
            // 
            flpBanBia.AutoScroll = true;
            flpBanBia.BackColor = Color.White;
            flpBanBia.Dock = DockStyle.Fill;
            flpBanBia.Location = new Point(0, 222);
            flpBanBia.Name = "flpBanBia";
            flpBanBia.Padding = new Padding(15);
            flpBanBia.Size = new Size(1095, 368);
            flpBanBia.TabIndex = 2;
            // 
            // QLBanForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1095, 590);
            Controls.Add(flpBanBia);
            Controls.Add(pnlFilters);
            Controls.Add(pnlToolbar);
            Font = new Font("Segoe UI", 9F);
            Name = "QLBanForm";
            Text = "Quản lý bàn";
            Load += QLBanForm_Load;
            pnlToolbar.ResumeLayout(false);
            pnlFilters.ResumeLayout(false);
            pnlFilters.PerformLayout();
            pnlLoaiBanFilters.ResumeLayout(false);
            pnlTrangThaiFilters.ResumeLayout(false);
            pnlKhuVucFilters.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.Button btnThemBan;
        private System.Windows.Forms.Button btnDatBan;
        private System.Windows.Forms.Button btnXemBanDat;
        private System.Windows.Forms.Button btnXemSoDo;
        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblLoaiBan;
        private System.Windows.Forms.FlowLayoutPanel pnlLoaiBanFilters;
        private System.Windows.Forms.Button btnTypeAll;
        private System.Windows.Forms.Button btnTypeLo9Bi;
        private System.Windows.Forms.Button btnTypePhangCarom;
        private System.Windows.Forms.Button btnTypeSnooker;
        private System.Windows.Forms.Button btnTypeVIPLo;
        private System.Windows.Forms.Button btnTypeVIPPhang;
        private System.Windows.Forms.Label lblTrangThai;
        private System.Windows.Forms.FlowLayoutPanel pnlTrangThaiFilters;
        private System.Windows.Forms.Button btnStatusAll;
        private System.Windows.Forms.Button btnStatusTrong;
        private System.Windows.Forms.Button btnStatusDangChoi;
        private System.Windows.Forms.Button btnStatusDaDat;
        private System.Windows.Forms.Label lblKhuVuc;
        private System.Windows.Forms.FlowLayoutPanel pnlKhuVucFilters;
        private System.Windows.Forms.Button btnFilterAll;
        private System.Windows.Forms.Button btnFilterTang1;
        private System.Windows.Forms.Button btnFilterTang2;
        private System.Windows.Forms.Button btnFilterVIP;
        private System.Windows.Forms.FlowLayoutPanel flpBanBia;
    }
}