namespace Billiard.WinForm.Forms.Users
{
    partial class User
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
            main = new Panel();
            layoutRoot = new TableLayoutPanel();
            pnlNav = new Panel();
            btnLogin = new Button();
            btnSoDoBan = new Button();
            btnHoTro = new Button();
            btnGacha = new Button();
            pnlContent = new Panel();
            flpMain = new FlowLayoutPanel();
            flpPoster = new FlowLayoutPanel();
            pnlFilter = new Panel();
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
            flpTables = new FlowLayoutPanel();
            pnlFooter = new Panel();
            main.SuspendLayout();
            layoutRoot.SuspendLayout();
            pnlNav.SuspendLayout();
            pnlContent.SuspendLayout();
            flpMain.SuspendLayout();
            pnlFilter.SuspendLayout();
            pnlLoaiBanFilters.SuspendLayout();
            pnlTrangThaiFilters.SuspendLayout();
            pnlKhuVucFilters.SuspendLayout();
            SuspendLayout();
            // 
            // main
            // 
            main.Controls.Add(layoutRoot);
            main.Dock = DockStyle.Fill;
            main.Location = new Point(0, 0);
            main.Name = "main";
            main.Size = new Size(907, 743);
            main.TabIndex = 0;
            // 
            // layoutRoot
            // 
            layoutRoot.ColumnCount = 1;
            layoutRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layoutRoot.Controls.Add(pnlNav, 0, 0);
            layoutRoot.Controls.Add(pnlContent, 0, 1);
            layoutRoot.Dock = DockStyle.Fill;
            layoutRoot.Location = new Point(0, 0);
            layoutRoot.Name = "layoutRoot";
            layoutRoot.RowCount = 2;
            layoutRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            layoutRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layoutRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            layoutRoot.Size = new Size(907, 743);
            layoutRoot.TabIndex = 0;
            // 
            // pnlNav
            // 
            pnlNav.BackColor = Color.FromArgb(73, 77, 126);
            pnlNav.Controls.Add(btnLogin);
            pnlNav.Controls.Add(btnSoDoBan);
            pnlNav.Controls.Add(btnHoTro);
            pnlNav.Controls.Add(btnGacha);
            pnlNav.Dock = DockStyle.Top;
            pnlNav.Location = new Point(3, 3);
            pnlNav.Name = "pnlNav";
            pnlNav.Size = new Size(901, 64);
            pnlNav.TabIndex = 0;
            // 
            // btnLogin
            // 
            btnLogin.AutoSize = true;
            btnLogin.BackColor = Color.FromArgb(198, 159, 165);
            btnLogin.Location = new Point(528, 16);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(158, 33);
            btnLogin.TabIndex = 4;
            btnLogin.Text = "Đăng nhập/Đăng ký";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // btnSoDoBan
            // 
            btnSoDoBan.BackColor = Color.FromArgb(198, 159, 165);
            btnSoDoBan.Location = new Point(9, 16);
            btnSoDoBan.Name = "btnSoDoBan";
            btnSoDoBan.Size = new Size(134, 33);
            btnSoDoBan.TabIndex = 3;
            btnSoDoBan.Text = "Sơ đồ bàn";
            btnSoDoBan.UseVisualStyleBackColor = false;
            btnSoDoBan.Click += btnSoDoBan_Click;
            // 
            // btnHoTro
            // 
            btnHoTro.BackColor = Color.FromArgb(198, 159, 165);
            btnHoTro.Location = new Point(149, 16);
            btnHoTro.Name = "btnHoTro";
            btnHoTro.Size = new Size(134, 33);
            btnHoTro.TabIndex = 1;
            btnHoTro.Text = "Hỗ trợ";
            btnHoTro.UseVisualStyleBackColor = false;
            btnHoTro.Click += btnHoTro_Click;
            // 
            // btnGacha
            // 
            btnGacha.BackColor = Color.FromArgb(198, 159, 165);
            btnGacha.Location = new Point(289, 16);
            btnGacha.Name = "btnGacha";
            btnGacha.Size = new Size(234, 33);
            btnGacha.TabIndex = 0;
            btnGacha.Text = "Gacha";
            btnGacha.UseVisualStyleBackColor = false;
            // 
            // pnlContent
            // 
            pnlContent.Controls.Add(flpMain);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(3, 73);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(901, 667);
            pnlContent.TabIndex = 1;
            // 
            // flpMain
            // 
            flpMain.AutoSize = true;
            flpMain.BackColor = SystemColors.ActiveCaption;
            flpMain.Controls.Add(flpPoster);
            flpMain.Controls.Add(pnlFilter);
            flpMain.Controls.Add(flpTables);
            flpMain.Controls.Add(pnlFooter);
            flpMain.Dock = DockStyle.Top;
            flpMain.FlowDirection = FlowDirection.TopDown;
            flpMain.Location = new Point(0, 0);
            flpMain.Name = "flpMain";
            flpMain.Size = new Size(901, 546);
            flpMain.TabIndex = 0;
            flpMain.WrapContents = false;
            // 
            // flpPoster
            // 
            flpPoster.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            flpPoster.BackColor = Color.FromArgb(242, 211, 171);
            flpPoster.Location = new Point(0, 0);
            flpPoster.Margin = new Padding(0);
            flpPoster.Name = "flpPoster";
            flpPoster.Size = new Size(902, 275);
            flpPoster.TabIndex = 1;
            flpPoster.WrapContents = false;
            // 
            // pnlFilter
            // 
            pnlFilter.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            pnlFilter.BackColor = Color.FromArgb(251, 245, 239);
            pnlFilter.Controls.Add(txtSearch);
            pnlFilter.Controls.Add(lblLoaiBan);
            pnlFilter.Controls.Add(pnlLoaiBanFilters);
            pnlFilter.Controls.Add(lblTrangThai);
            pnlFilter.Controls.Add(pnlTrangThaiFilters);
            pnlFilter.Controls.Add(lblKhuVuc);
            pnlFilter.Controls.Add(pnlKhuVucFilters);
            pnlFilter.Location = new Point(3, 278);
            pnlFilter.Name = "pnlFilter";
            pnlFilter.Size = new Size(896, 153);
            pnlFilter.TabIndex = 2;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 11F);
            txtSearch.Location = new Point(632, 85);
            txtSearch.Margin = new Padding(2);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "🔍 Tìm kiếm bàn...";
            txtSearch.Size = new Size(240, 32);
            txtSearch.TabIndex = 13;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // lblLoaiBan
            // 
            lblLoaiBan.AutoSize = true;
            lblLoaiBan.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblLoaiBan.Location = new Point(32, 89);
            lblLoaiBan.Margin = new Padding(2, 0, 2, 0);
            lblLoaiBan.Name = "lblLoaiBan";
            lblLoaiBan.Size = new Size(83, 23);
            lblLoaiBan.TabIndex = 11;
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
            pnlLoaiBanFilters.Location = new Point(112, 85);
            pnlLoaiBanFilters.Margin = new Padding(2);
            pnlLoaiBanFilters.Name = "pnlLoaiBanFilters";
            pnlLoaiBanFilters.Size = new Size(516, 41);
            pnlLoaiBanFilters.TabIndex = 12;
            // 
            // btnTypeAll
            // 
            btnTypeAll.BackColor = Color.FromArgb(99, 102, 241);
            btnTypeAll.Cursor = Cursors.Hand;
            btnTypeAll.FlatAppearance.BorderSize = 0;
            btnTypeAll.FlatStyle = FlatStyle.Flat;
            btnTypeAll.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTypeAll.ForeColor = Color.White;
            btnTypeAll.Location = new Point(2, 2);
            btnTypeAll.Margin = new Padding(2);
            btnTypeAll.Name = "btnTypeAll";
            btnTypeAll.Size = new Size(72, 28);
            btnTypeAll.TabIndex = 0;
            btnTypeAll.Tag = "all";
            btnTypeAll.Text = "Tất cả";
            btnTypeAll.UseVisualStyleBackColor = false;
            // 
            // btnTypeLo9Bi
            // 
            btnTypeLo9Bi.BackColor = Color.FromArgb(226, 232, 240);
            btnTypeLo9Bi.Cursor = Cursors.Hand;
            btnTypeLo9Bi.FlatAppearance.BorderSize = 0;
            btnTypeLo9Bi.FlatStyle = FlatStyle.Flat;
            btnTypeLo9Bi.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTypeLo9Bi.ForeColor = Color.FromArgb(51, 65, 85);
            btnTypeLo9Bi.Location = new Point(78, 2);
            btnTypeLo9Bi.Margin = new Padding(2);
            btnTypeLo9Bi.Name = "btnTypeLo9Bi";
            btnTypeLo9Bi.Size = new Size(80, 28);
            btnTypeLo9Bi.TabIndex = 1;
            btnTypeLo9Bi.Tag = "Bàn Lỗ 9 bi";
            btnTypeLo9Bi.Text = "Lỗ 9 bi";
            btnTypeLo9Bi.UseVisualStyleBackColor = false;
            // 
            // btnTypePhangCarom
            // 
            btnTypePhangCarom.BackColor = Color.FromArgb(226, 232, 240);
            btnTypePhangCarom.Cursor = Cursors.Hand;
            btnTypePhangCarom.FlatAppearance.BorderSize = 0;
            btnTypePhangCarom.FlatStyle = FlatStyle.Flat;
            btnTypePhangCarom.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTypePhangCarom.ForeColor = Color.FromArgb(51, 65, 85);
            btnTypePhangCarom.Location = new Point(162, 2);
            btnTypePhangCarom.Margin = new Padding(2);
            btnTypePhangCarom.Name = "btnTypePhangCarom";
            btnTypePhangCarom.Size = new Size(96, 28);
            btnTypePhangCarom.TabIndex = 2;
            btnTypePhangCarom.Tag = "Bàn Phăng Carom";
            btnTypePhangCarom.Text = "Phăng Carom";
            btnTypePhangCarom.UseVisualStyleBackColor = false;
            // 
            // btnTypeSnooker
            // 
            btnTypeSnooker.BackColor = Color.FromArgb(226, 232, 240);
            btnTypeSnooker.Cursor = Cursors.Hand;
            btnTypeSnooker.FlatAppearance.BorderSize = 0;
            btnTypeSnooker.FlatStyle = FlatStyle.Flat;
            btnTypeSnooker.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTypeSnooker.ForeColor = Color.FromArgb(51, 65, 85);
            btnTypeSnooker.Location = new Point(262, 2);
            btnTypeSnooker.Margin = new Padding(2);
            btnTypeSnooker.Name = "btnTypeSnooker";
            btnTypeSnooker.Size = new Size(74, 28);
            btnTypeSnooker.TabIndex = 3;
            btnTypeSnooker.Tag = "Bàn Snooker";
            btnTypeSnooker.Text = "Snooker";
            btnTypeSnooker.UseVisualStyleBackColor = false;
            // 
            // btnTypeVIPLo
            // 
            btnTypeVIPLo.BackColor = Color.FromArgb(226, 232, 240);
            btnTypeVIPLo.Cursor = Cursors.Hand;
            btnTypeVIPLo.FlatAppearance.BorderSize = 0;
            btnTypeVIPLo.FlatStyle = FlatStyle.Flat;
            btnTypeVIPLo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTypeVIPLo.ForeColor = Color.FromArgb(51, 65, 85);
            btnTypeVIPLo.Location = new Point(340, 2);
            btnTypeVIPLo.Margin = new Padding(2);
            btnTypeVIPLo.Name = "btnTypeVIPLo";
            btnTypeVIPLo.Size = new Size(72, 28);
            btnTypeVIPLo.TabIndex = 4;
            btnTypeVIPLo.Tag = "Bàn VIP Lỗ";
            btnTypeVIPLo.Text = "VIP Lỗ";
            btnTypeVIPLo.UseVisualStyleBackColor = false;
            // 
            // btnTypeVIPPhang
            // 
            btnTypeVIPPhang.BackColor = Color.FromArgb(226, 232, 240);
            btnTypeVIPPhang.Cursor = Cursors.Hand;
            btnTypeVIPPhang.FlatAppearance.BorderSize = 0;
            btnTypeVIPPhang.FlatStyle = FlatStyle.Flat;
            btnTypeVIPPhang.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTypeVIPPhang.ForeColor = Color.FromArgb(51, 65, 85);
            btnTypeVIPPhang.Location = new Point(416, 2);
            btnTypeVIPPhang.Margin = new Padding(2);
            btnTypeVIPPhang.Name = "btnTypeVIPPhang";
            btnTypeVIPPhang.Size = new Size(80, 28);
            btnTypeVIPPhang.TabIndex = 5;
            btnTypeVIPPhang.Tag = "Bàn VIP Phăng";
            btnTypeVIPPhang.Text = "VIP Phăng";
            btnTypeVIPPhang.UseVisualStyleBackColor = false;
            // 
            // lblTrangThai
            // 
            lblTrangThai.AutoSize = true;
            lblTrangThai.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTrangThai.Location = new Point(438, 31);
            lblTrangThai.Margin = new Padding(2, 0, 2, 0);
            lblTrangThai.Name = "lblTrangThai";
            lblTrangThai.Size = new Size(97, 23);
            lblTrangThai.TabIndex = 9;
            lblTrangThai.Text = "Trạng thái:";
            // 
            // pnlTrangThaiFilters
            // 
            pnlTrangThaiFilters.Controls.Add(btnStatusAll);
            pnlTrangThaiFilters.Controls.Add(btnStatusTrong);
            pnlTrangThaiFilters.Controls.Add(btnStatusDangChoi);
            pnlTrangThaiFilters.Controls.Add(btnStatusDaDat);
            pnlTrangThaiFilters.Location = new Point(533, 27);
            pnlTrangThaiFilters.Margin = new Padding(2);
            pnlTrangThaiFilters.Name = "pnlTrangThaiFilters";
            pnlTrangThaiFilters.Size = new Size(352, 38);
            pnlTrangThaiFilters.TabIndex = 10;
            // 
            // btnStatusAll
            // 
            btnStatusAll.BackColor = Color.FromArgb(99, 102, 241);
            btnStatusAll.Cursor = Cursors.Hand;
            btnStatusAll.FlatAppearance.BorderSize = 0;
            btnStatusAll.FlatStyle = FlatStyle.Flat;
            btnStatusAll.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnStatusAll.ForeColor = Color.White;
            btnStatusAll.Location = new Point(2, 2);
            btnStatusAll.Margin = new Padding(2);
            btnStatusAll.Name = "btnStatusAll";
            btnStatusAll.Size = new Size(72, 28);
            btnStatusAll.TabIndex = 0;
            btnStatusAll.Tag = "all";
            btnStatusAll.Text = "Tất cả";
            btnStatusAll.UseVisualStyleBackColor = false;
            // 
            // btnStatusTrong
            // 
            btnStatusTrong.BackColor = Color.FromArgb(226, 232, 240);
            btnStatusTrong.Cursor = Cursors.Hand;
            btnStatusTrong.FlatAppearance.BorderSize = 0;
            btnStatusTrong.FlatStyle = FlatStyle.Flat;
            btnStatusTrong.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnStatusTrong.ForeColor = Color.FromArgb(51, 65, 85);
            btnStatusTrong.Location = new Point(78, 2);
            btnStatusTrong.Margin = new Padding(2);
            btnStatusTrong.Name = "btnStatusTrong";
            btnStatusTrong.Size = new Size(72, 28);
            btnStatusTrong.TabIndex = 1;
            btnStatusTrong.Tag = "Trống";
            btnStatusTrong.Text = "Trống";
            btnStatusTrong.UseVisualStyleBackColor = false;
            // 
            // btnStatusDangChoi
            // 
            btnStatusDangChoi.BackColor = Color.FromArgb(226, 232, 240);
            btnStatusDangChoi.Cursor = Cursors.Hand;
            btnStatusDangChoi.FlatAppearance.BorderSize = 0;
            btnStatusDangChoi.FlatStyle = FlatStyle.Flat;
            btnStatusDangChoi.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnStatusDangChoi.ForeColor = Color.FromArgb(51, 65, 85);
            btnStatusDangChoi.Location = new Point(154, 2);
            btnStatusDangChoi.Margin = new Padding(2);
            btnStatusDangChoi.Name = "btnStatusDangChoi";
            btnStatusDangChoi.Size = new Size(102, 28);
            btnStatusDangChoi.TabIndex = 2;
            btnStatusDangChoi.Tag = "Đang chơi";
            btnStatusDangChoi.Text = "Đang chơi";
            btnStatusDangChoi.UseVisualStyleBackColor = false;
            // 
            // btnStatusDaDat
            // 
            btnStatusDaDat.BackColor = Color.FromArgb(226, 232, 240);
            btnStatusDaDat.Cursor = Cursors.Hand;
            btnStatusDaDat.FlatAppearance.BorderSize = 0;
            btnStatusDaDat.FlatStyle = FlatStyle.Flat;
            btnStatusDaDat.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnStatusDaDat.ForeColor = Color.FromArgb(51, 65, 85);
            btnStatusDaDat.Location = new Point(260, 2);
            btnStatusDaDat.Margin = new Padding(2);
            btnStatusDaDat.Name = "btnStatusDaDat";
            btnStatusDaDat.Size = new Size(72, 28);
            btnStatusDaDat.TabIndex = 3;
            btnStatusDaDat.Tag = "Đã đặt";
            btnStatusDaDat.Text = "Đã đặt";
            btnStatusDaDat.UseVisualStyleBackColor = false;
            // 
            // lblKhuVuc
            // 
            lblKhuVuc.AutoSize = true;
            lblKhuVuc.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblKhuVuc.Location = new Point(32, 31);
            lblKhuVuc.Margin = new Padding(2, 0, 2, 0);
            lblKhuVuc.Name = "lblKhuVuc";
            lblKhuVuc.Size = new Size(79, 23);
            lblKhuVuc.TabIndex = 7;
            lblKhuVuc.Text = "Khu vực:";
            // 
            // pnlKhuVucFilters
            // 
            pnlKhuVucFilters.Controls.Add(btnFilterAll);
            pnlKhuVucFilters.Controls.Add(btnFilterTang1);
            pnlKhuVucFilters.Controls.Add(btnFilterTang2);
            pnlKhuVucFilters.Controls.Add(btnFilterVIP);
            pnlKhuVucFilters.Location = new Point(112, 27);
            pnlKhuVucFilters.Margin = new Padding(2);
            pnlKhuVucFilters.Name = "pnlKhuVucFilters";
            pnlKhuVucFilters.Size = new Size(318, 38);
            pnlKhuVucFilters.TabIndex = 8;
            // 
            // btnFilterAll
            // 
            btnFilterAll.BackColor = Color.FromArgb(99, 102, 241);
            btnFilterAll.Cursor = Cursors.Hand;
            btnFilterAll.FlatAppearance.BorderSize = 0;
            btnFilterAll.FlatStyle = FlatStyle.Flat;
            btnFilterAll.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnFilterAll.ForeColor = Color.White;
            btnFilterAll.Location = new Point(2, 2);
            btnFilterAll.Margin = new Padding(2);
            btnFilterAll.Name = "btnFilterAll";
            btnFilterAll.Size = new Size(72, 28);
            btnFilterAll.TabIndex = 0;
            btnFilterAll.Tag = "all";
            btnFilterAll.Text = "Tất cả";
            btnFilterAll.UseVisualStyleBackColor = false;
            // 
            // btnFilterTang1
            // 
            btnFilterTang1.BackColor = Color.FromArgb(226, 232, 240);
            btnFilterTang1.Cursor = Cursors.Hand;
            btnFilterTang1.FlatAppearance.BorderSize = 0;
            btnFilterTang1.FlatStyle = FlatStyle.Flat;
            btnFilterTang1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnFilterTang1.ForeColor = Color.FromArgb(51, 65, 85);
            btnFilterTang1.Location = new Point(78, 2);
            btnFilterTang1.Margin = new Padding(2);
            btnFilterTang1.Name = "btnFilterTang1";
            btnFilterTang1.Size = new Size(72, 28);
            btnFilterTang1.TabIndex = 1;
            btnFilterTang1.Tag = "Tầng 1";
            btnFilterTang1.Text = "Tầng 1";
            btnFilterTang1.UseVisualStyleBackColor = false;
            // 
            // btnFilterTang2
            // 
            btnFilterTang2.BackColor = Color.FromArgb(226, 232, 240);
            btnFilterTang2.Cursor = Cursors.Hand;
            btnFilterTang2.FlatAppearance.BorderSize = 0;
            btnFilterTang2.FlatStyle = FlatStyle.Flat;
            btnFilterTang2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnFilterTang2.ForeColor = Color.FromArgb(51, 65, 85);
            btnFilterTang2.Location = new Point(154, 2);
            btnFilterTang2.Margin = new Padding(2);
            btnFilterTang2.Name = "btnFilterTang2";
            btnFilterTang2.Size = new Size(72, 28);
            btnFilterTang2.TabIndex = 2;
            btnFilterTang2.Tag = "Tầng 2";
            btnFilterTang2.Text = "Tầng 2";
            btnFilterTang2.UseVisualStyleBackColor = false;
            // 
            // btnFilterVIP
            // 
            btnFilterVIP.BackColor = Color.FromArgb(226, 232, 240);
            btnFilterVIP.Cursor = Cursors.Hand;
            btnFilterVIP.FlatAppearance.BorderSize = 0;
            btnFilterVIP.FlatStyle = FlatStyle.Flat;
            btnFilterVIP.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnFilterVIP.ForeColor = Color.FromArgb(51, 65, 85);
            btnFilterVIP.Location = new Point(230, 2);
            btnFilterVIP.Margin = new Padding(2);
            btnFilterVIP.Name = "btnFilterVIP";
            btnFilterVIP.Size = new Size(72, 28);
            btnFilterVIP.TabIndex = 3;
            btnFilterVIP.Tag = "VIP";
            btnFilterVIP.Text = "VIP";
            btnFilterVIP.UseVisualStyleBackColor = false;
            // 
            // flpTables
            // 
            flpTables.Anchor = AnchorStyles.Top;
            flpTables.AutoScroll = true;
            flpTables.AutoSize = true;
            flpTables.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpTables.Location = new Point(451, 437);
            flpTables.Name = "flpTables";
            flpTables.Size = new Size(0, 0);
            flpTables.TabIndex = 1;
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.FromArgb(73, 77, 126);
            pnlFooter.Location = new Point(3, 443);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Size = new Size(896, 100);
            pnlFooter.TabIndex = 1;
            // 
            // User
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(907, 743);
            Controls.Add(main);
            Name = "User";
            Text = "User";
            Load += User_Load;
            main.ResumeLayout(false);
            layoutRoot.ResumeLayout(false);
            pnlNav.ResumeLayout(false);
            pnlNav.PerformLayout();
            pnlContent.ResumeLayout(false);
            pnlContent.PerformLayout();
            flpMain.ResumeLayout(false);
            flpMain.PerformLayout();
            pnlFilter.ResumeLayout(false);
            pnlFilter.PerformLayout();
            pnlLoaiBanFilters.ResumeLayout(false);
            pnlTrangThaiFilters.ResumeLayout(false);
            pnlKhuVucFilters.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel main;
        private Panel pnlNav;
        private Panel pnlContent;
        private FlowLayoutPanel flpMain;
        private FlowLayoutPanel flpPoster;
        private Panel pnlFilter;
        private FlowLayoutPanel flpTables;
        private Panel pnlFooter;
        private TableLayoutPanel layoutRoot;
        private Button btnLogin;
        private Button btnSoDoBan;
        private Button btnHoTro;
        private Button btnGacha;
        private TextBox txtSearch;
        private Label lblLoaiBan;
        private FlowLayoutPanel pnlLoaiBanFilters;
        private Button btnTypeAll;
        private Button btnTypeLo9Bi;
        private Button btnTypePhangCarom;
        private Button btnTypeSnooker;
        private Button btnTypeVIPLo;
        private Button btnTypeVIPPhang;
        private Label lblTrangThai;
        private FlowLayoutPanel pnlTrangThaiFilters;
        private Button btnStatusAll;
        private Button btnStatusTrong;
        private Button btnStatusDangChoi;
        private Button btnStatusDaDat;
        private Label lblKhuVuc;
        private FlowLayoutPanel pnlKhuVucFilters;
        private Button btnFilterAll;
        private Button btnFilterTang1;
        private Button btnFilterTang2;
        private Button btnFilterVIP;
    }
}