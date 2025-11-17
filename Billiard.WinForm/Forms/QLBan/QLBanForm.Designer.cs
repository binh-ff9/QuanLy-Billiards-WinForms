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
            this.pnlToolbar = new System.Windows.Forms.Panel();
            this.btnXemSoDo = new System.Windows.Forms.Button();
            this.btnXemBanDat = new System.Windows.Forms.Button();
            this.btnDatBan = new System.Windows.Forms.Button();
            this.btnThemBan = new System.Windows.Forms.Button();
            this.pnlFilters = new System.Windows.Forms.Panel();
            this.lblKhuVuc = new System.Windows.Forms.Label();
            this.pnlKhuVucFilters = new System.Windows.Forms.FlowLayoutPanel();
            this.btnFilterAll = new System.Windows.Forms.Button();
            this.btnFilterTang1 = new System.Windows.Forms.Button();
            this.btnFilterTang2 = new System.Windows.Forms.Button();
            this.btnFilterVIP = new System.Windows.Forms.Button();
            this.lblTrangThai = new System.Windows.Forms.Label();
            this.pnlTrangThaiFilters = new System.Windows.Forms.FlowLayoutPanel();
            this.btnStatusAll = new System.Windows.Forms.Button();
            this.btnStatusTrong = new System.Windows.Forms.Button();
            this.btnStatusDangChoi = new System.Windows.Forms.Button();
            this.btnStatusDaDat = new System.Windows.Forms.Button();
            this.lblLoaiBan = new System.Windows.Forms.Label();
            this.pnlLoaiBanFilters = new System.Windows.Forms.FlowLayoutPanel();
            this.btnTypeAll = new System.Windows.Forms.Button();
            this.btnTypeLo9Bi = new System.Windows.Forms.Button();
            this.btnTypePhangCarom = new System.Windows.Forms.Button();
            this.btnTypeSnooker = new System.Windows.Forms.Button();
            this.btnTypeVIPLo = new System.Windows.Forms.Button();
            this.btnTypeVIPPhang = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.flpBanBia = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlToolbar.SuspendLayout();
            this.pnlFilters.SuspendLayout();
            this.pnlKhuVucFilters.SuspendLayout();
            this.pnlTrangThaiFilters.SuspendLayout();
            this.pnlLoaiBanFilters.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlToolbar
            // 
            this.pnlToolbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlToolbar.Controls.Add(this.btnThemBan);
            this.pnlToolbar.Controls.Add(this.btnDatBan);
            this.pnlToolbar.Controls.Add(this.btnXemBanDat);
            this.pnlToolbar.Controls.Add(this.btnXemSoDo);
            this.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlToolbar.Location = new System.Drawing.Point(0, 0);
            this.pnlToolbar.Name = "pnlToolbar";
            this.pnlToolbar.Padding = new System.Windows.Forms.Padding(15);
            this.pnlToolbar.Size = new System.Drawing.Size(850, 70);
            this.pnlToolbar.TabIndex = 0;
            // 
            // btnXemSoDo
            // 
            this.btnXemSoDo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.btnXemSoDo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXemSoDo.FlatAppearance.BorderSize = 0;
            this.btnXemSoDo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXemSoDo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnXemSoDo.ForeColor = System.Drawing.Color.White;
            this.btnXemSoDo.Location = new System.Drawing.Point(15, 15);
            this.btnXemSoDo.Name = "btnXemSoDo";
            this.btnXemSoDo.Size = new System.Drawing.Size(140, 40);
            this.btnXemSoDo.TabIndex = 0;
            this.btnXemSoDo.Text = "🗺️ Xem sơ đồ";
            this.btnXemSoDo.UseVisualStyleBackColor = false;
            this.btnXemSoDo.Click += new System.EventHandler(this.BtnXemSoDo_Click);
            // 
            // btnXemBanDat
            // 
            this.btnXemBanDat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.btnXemBanDat.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXemBanDat.FlatAppearance.BorderSize = 0;
            this.btnXemBanDat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXemBanDat.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnXemBanDat.ForeColor = System.Drawing.Color.White;
            this.btnXemBanDat.Location = new System.Drawing.Point(165, 15);
            this.btnXemBanDat.Name = "btnXemBanDat";
            this.btnXemBanDat.Size = new System.Drawing.Size(150, 40);
            this.btnXemBanDat.TabIndex = 1;
            this.btnXemBanDat.Text = "📅 Xem bàn đặt";
            this.btnXemBanDat.UseVisualStyleBackColor = false;
            this.btnXemBanDat.Click += new System.EventHandler(this.BtnXemBanDat_Click);
            // 
            // btnDatBan
            // 
            this.btnDatBan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(179)))), ((int)(((byte)(8)))));
            this.btnDatBan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDatBan.FlatAppearance.BorderSize = 0;
            this.btnDatBan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDatBan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDatBan.ForeColor = System.Drawing.Color.White;
            this.btnDatBan.Location = new System.Drawing.Point(325, 15);
            this.btnDatBan.Name = "btnDatBan";
            this.btnDatBan.Size = new System.Drawing.Size(150, 40);
            this.btnDatBan.TabIndex = 2;
            this.btnDatBan.Text = "📅 Đặt bàn trước";
            this.btnDatBan.UseVisualStyleBackColor = false;
            this.btnDatBan.Click += new System.EventHandler(this.BtnDatBan_Click);
            // 
            // btnThemBan
            // 
            this.btnThemBan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(197)))), ((int)(((byte)(94)))));
            this.btnThemBan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThemBan.FlatAppearance.BorderSize = 0;
            this.btnThemBan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThemBan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnThemBan.ForeColor = System.Drawing.Color.White;
            this.btnThemBan.Location = new System.Drawing.Point(485, 15);
            this.btnThemBan.Name = "btnThemBan";
            this.btnThemBan.Size = new System.Drawing.Size(150, 40);
            this.btnThemBan.TabIndex = 3;
            this.btnThemBan.Text = "➕ Thêm bàn mới";
            this.btnThemBan.UseVisualStyleBackColor = false;
            this.btnThemBan.Click += new System.EventHandler(this.BtnThemBan_Click);
            // 
            // pnlFilters
            // 
            this.pnlFilters.BackColor = System.Drawing.Color.White;
            this.pnlFilters.Controls.Add(this.txtSearch);
            this.pnlFilters.Controls.Add(this.lblLoaiBan);
            this.pnlFilters.Controls.Add(this.pnlLoaiBanFilters);
            this.pnlFilters.Controls.Add(this.lblTrangThai);
            this.pnlFilters.Controls.Add(this.pnlTrangThaiFilters);
            this.pnlFilters.Controls.Add(this.lblKhuVuc);
            this.pnlFilters.Controls.Add(this.pnlKhuVucFilters);
            this.pnlFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilters.Location = new System.Drawing.Point(0, 70);
            this.pnlFilters.Name = "pnlFilters";
            this.pnlFilters.Padding = new System.Windows.Forms.Padding(15);
            this.pnlFilters.Size = new System.Drawing.Size(850, 220);
            this.pnlFilters.TabIndex = 1;
            // 
            // lblKhuVuc
            // 
            this.lblKhuVuc.AutoSize = true;
            this.lblKhuVuc.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblKhuVuc.Location = new System.Drawing.Point(15, 15);
            this.lblKhuVuc.Name = "lblKhuVuc";
            this.lblKhuVuc.Size = new System.Drawing.Size(71, 19);
            this.lblKhuVuc.TabIndex = 0;
            this.lblKhuVuc.Text = "Khu vực:";
            // 
            // pnlKhuVucFilters
            // 
            this.pnlKhuVucFilters.Controls.Add(this.btnFilterAll);
            this.pnlKhuVucFilters.Controls.Add(this.btnFilterTang1);
            this.pnlKhuVucFilters.Controls.Add(this.btnFilterTang2);
            this.pnlKhuVucFilters.Controls.Add(this.btnFilterVIP);
            this.pnlKhuVucFilters.Location = new System.Drawing.Point(100, 10);
            this.pnlKhuVucFilters.Name = "pnlKhuVucFilters";
            this.pnlKhuVucFilters.Size = new System.Drawing.Size(700, 35);
            this.pnlKhuVucFilters.TabIndex = 1;
            // 
            // btnFilterAll
            // 
            this.btnFilterAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.btnFilterAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFilterAll.FlatAppearance.BorderSize = 0;
            this.btnFilterAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilterAll.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnFilterAll.ForeColor = System.Drawing.Color.White;
            this.btnFilterAll.Location = new System.Drawing.Point(3, 3);
            this.btnFilterAll.Name = "btnFilterAll";
            this.btnFilterAll.Size = new System.Drawing.Size(90, 28);
            this.btnFilterAll.TabIndex = 0;
            this.btnFilterAll.Tag = "all";
            this.btnFilterAll.Text = "Tất cả";
            this.btnFilterAll.UseVisualStyleBackColor = false;
            this.btnFilterAll.Click += new System.EventHandler(this.FilterButton_Click);
            // 
            // btnFilterTang1
            // 
            this.btnFilterTang1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnFilterTang1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFilterTang1.FlatAppearance.BorderSize = 0;
            this.btnFilterTang1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilterTang1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnFilterTang1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnFilterTang1.Location = new System.Drawing.Point(99, 3);
            this.btnFilterTang1.Name = "btnFilterTang1";
            this.btnFilterTang1.Size = new System.Drawing.Size(90, 28);
            this.btnFilterTang1.TabIndex = 1;
            this.btnFilterTang1.Tag = "Tầng 1";
            this.btnFilterTang1.Text = "Tầng 1";
            this.btnFilterTang1.UseVisualStyleBackColor = false;
            this.btnFilterTang1.Click += new System.EventHandler(this.FilterButton_Click);
            // 
            // btnFilterTang2
            // 
            this.btnFilterTang2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnFilterTang2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFilterTang2.FlatAppearance.BorderSize = 0;
            this.btnFilterTang2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilterTang2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnFilterTang2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnFilterTang2.Location = new System.Drawing.Point(195, 3);
            this.btnFilterTang2.Name = "btnFilterTang2";
            this.btnFilterTang2.Size = new System.Drawing.Size(90, 28);
            this.btnFilterTang2.TabIndex = 2;
            this.btnFilterTang2.Tag = "Tầng 2";
            this.btnFilterTang2.Text = "Tầng 2";
            this.btnFilterTang2.UseVisualStyleBackColor = false;
            this.btnFilterTang2.Click += new System.EventHandler(this.FilterButton_Click);
            // 
            // btnFilterVIP
            // 
            this.btnFilterVIP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnFilterVIP.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFilterVIP.FlatAppearance.BorderSize = 0;
            this.btnFilterVIP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilterVIP.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnFilterVIP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnFilterVIP.Location = new System.Drawing.Point(291, 3);
            this.btnFilterVIP.Name = "btnFilterVIP";
            this.btnFilterVIP.Size = new System.Drawing.Size(90, 28);
            this.btnFilterVIP.TabIndex = 3;
            this.btnFilterVIP.Tag = "VIP";
            this.btnFilterVIP.Text = "VIP";
            this.btnFilterVIP.UseVisualStyleBackColor = false;
            this.btnFilterVIP.Click += new System.EventHandler(this.FilterButton_Click);
            // 
            // lblTrangThai
            // 
            this.lblTrangThai.AutoSize = true;
            this.lblTrangThai.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTrangThai.Location = new System.Drawing.Point(15, 60);
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.Size = new System.Drawing.Size(82, 19);
            this.lblTrangThai.TabIndex = 2;
            this.lblTrangThai.Text = "Trạng thái:";
            // 
            // pnlTrangThaiFilters
            // 
            this.pnlTrangThaiFilters.Controls.Add(this.btnStatusAll);
            this.pnlTrangThaiFilters.Controls.Add(this.btnStatusTrong);
            this.pnlTrangThaiFilters.Controls.Add(this.btnStatusDangChoi);
            this.pnlTrangThaiFilters.Controls.Add(this.btnStatusDaDat);
            this.pnlTrangThaiFilters.Location = new System.Drawing.Point(100, 55);
            this.pnlTrangThaiFilters.Name = "pnlTrangThaiFilters";
            this.pnlTrangThaiFilters.Size = new System.Drawing.Size(700, 35);
            this.pnlTrangThaiFilters.TabIndex = 3;
            // 
            // btnStatusAll
            // 
            this.btnStatusAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.btnStatusAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStatusAll.FlatAppearance.BorderSize = 0;
            this.btnStatusAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStatusAll.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnStatusAll.ForeColor = System.Drawing.Color.White;
            this.btnStatusAll.Location = new System.Drawing.Point(3, 3);
            this.btnStatusAll.Name = "btnStatusAll";
            this.btnStatusAll.Size = new System.Drawing.Size(90, 28);
            this.btnStatusAll.TabIndex = 0;
            this.btnStatusAll.Tag = "all";
            this.btnStatusAll.Text = "Tất cả";
            this.btnStatusAll.UseVisualStyleBackColor = false;
            this.btnStatusAll.Click += new System.EventHandler(this.StatusFilterButton_Click);
            // 
            // btnStatusTrong
            // 
            this.btnStatusTrong.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnStatusTrong.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStatusTrong.FlatAppearance.BorderSize = 0;
            this.btnStatusTrong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStatusTrong.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnStatusTrong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnStatusTrong.Location = new System.Drawing.Point(99, 3);
            this.btnStatusTrong.Name = "btnStatusTrong";
            this.btnStatusTrong.Size = new System.Drawing.Size(90, 28);
            this.btnStatusTrong.TabIndex = 1;
            this.btnStatusTrong.Tag = "Trống";
            this.btnStatusTrong.Text = "Trống";
            this.btnStatusTrong.UseVisualStyleBackColor = false;
            this.btnStatusTrong.Click += new System.EventHandler(this.StatusFilterButton_Click);
            // 
            // btnStatusDangChoi
            // 
            this.btnStatusDangChoi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnStatusDangChoi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStatusDangChoi.FlatAppearance.BorderSize = 0;
            this.btnStatusDangChoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStatusDangChoi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnStatusDangChoi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnStatusDangChoi.Location = new System.Drawing.Point(195, 3);
            this.btnStatusDangChoi.Name = "btnStatusDangChoi";
            this.btnStatusDangChoi.Size = new System.Drawing.Size(100, 28);
            this.btnStatusDangChoi.TabIndex = 2;
            this.btnStatusDangChoi.Tag = "Đang chơi";
            this.btnStatusDangChoi.Text = "Đang chơi";
            this.btnStatusDangChoi.UseVisualStyleBackColor = false;
            this.btnStatusDangChoi.Click += new System.EventHandler(this.StatusFilterButton_Click);
            // 
            // btnStatusDaDat
            // 
            this.btnStatusDaDat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnStatusDaDat.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStatusDaDat.FlatAppearance.BorderSize = 0;
            this.btnStatusDaDat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStatusDaDat.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnStatusDaDat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnStatusDaDat.Location = new System.Drawing.Point(301, 3);
            this.btnStatusDaDat.Name = "btnStatusDaDat";
            this.btnStatusDaDat.Size = new System.Drawing.Size(90, 28);
            this.btnStatusDaDat.TabIndex = 3;
            this.btnStatusDaDat.Tag = "Đã đặt";
            this.btnStatusDaDat.Text = "Đã đặt";
            this.btnStatusDaDat.UseVisualStyleBackColor = false;
            this.btnStatusDaDat.Click += new System.EventHandler(this.StatusFilterButton_Click);
            // 
            // lblLoaiBan
            // 
            this.lblLoaiBan.AutoSize = true;
            this.lblLoaiBan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblLoaiBan.Location = new System.Drawing.Point(15, 105);
            this.lblLoaiBan.Name = "lblLoaiBan";
            this.lblLoaiBan.Size = new System.Drawing.Size(72, 19);
            this.lblLoaiBan.TabIndex = 4;
            this.lblLoaiBan.Text = "Loại bàn:";
            // 
            // pnlLoaiBanFilters
            // 
            this.pnlLoaiBanFilters.Controls.Add(this.btnTypeAll);
            this.pnlLoaiBanFilters.Controls.Add(this.btnTypeLo9Bi);
            this.pnlLoaiBanFilters.Controls.Add(this.btnTypePhangCarom);
            this.pnlLoaiBanFilters.Controls.Add(this.btnTypeSnooker);
            this.pnlLoaiBanFilters.Controls.Add(this.btnTypeVIPLo);
            this.pnlLoaiBanFilters.Controls.Add(this.btnTypeVIPPhang);
            this.pnlLoaiBanFilters.Location = new System.Drawing.Point(100, 100);
            this.pnlLoaiBanFilters.Name = "pnlLoaiBanFilters";
            this.pnlLoaiBanFilters.Size = new System.Drawing.Size(700, 70);
            this.pnlLoaiBanFilters.TabIndex = 5;
            // 
            // btnTypeAll
            // 
            this.btnTypeAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.btnTypeAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTypeAll.FlatAppearance.BorderSize = 0;
            this.btnTypeAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTypeAll.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnTypeAll.ForeColor = System.Drawing.Color.White;
            this.btnTypeAll.Location = new System.Drawing.Point(3, 3);
            this.btnTypeAll.Name = "btnTypeAll";
            this.btnTypeAll.Size = new System.Drawing.Size(90, 28);
            this.btnTypeAll.TabIndex = 0;
            this.btnTypeAll.Tag = "all";
            this.btnTypeAll.Text = "Tất cả";
            this.btnTypeAll.UseVisualStyleBackColor = false;
            this.btnTypeAll.Click += new System.EventHandler(this.TypeFilterButton_Click);
            // 
            // btnTypeLo9Bi
            // 
            this.btnTypeLo9Bi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnTypeLo9Bi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTypeLo9Bi.FlatAppearance.BorderSize = 0;
            this.btnTypeLo9Bi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTypeLo9Bi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnTypeLo9Bi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnTypeLo9Bi.Location = new System.Drawing.Point(99, 3);
            this.btnTypeLo9Bi.Name = "btnTypeLo9Bi";
            this.btnTypeLo9Bi.Size = new System.Drawing.Size(100, 28);
            this.btnTypeLo9Bi.TabIndex = 1;
            this.btnTypeLo9Bi.Tag = "Bàn Lỗ 9 bi";
            this.btnTypeLo9Bi.Text = "Lỗ 9 bi";
            this.btnTypeLo9Bi.UseVisualStyleBackColor = false;
            this.btnTypeLo9Bi.Click += new System.EventHandler(this.TypeFilterButton_Click);
            // 
            // btnTypePhangCarom
            // 
            this.btnTypePhangCarom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnTypePhangCarom.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTypePhangCarom.FlatAppearance.BorderSize = 0;
            this.btnTypePhangCarom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTypePhangCarom.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnTypePhangCarom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnTypePhangCarom.Location = new System.Drawing.Point(205, 3);
            this.btnTypePhangCarom.Name = "btnTypePhangCarom";
            this.btnTypePhangCarom.Size = new System.Drawing.Size(120, 28);
            this.btnTypePhangCarom.TabIndex = 2;
            this.btnTypePhangCarom.Tag = "Bàn Phăng Carom";
            this.btnTypePhangCarom.Text = "Phăng Carom";
            this.btnTypePhangCarom.UseVisualStyleBackColor = false;
            this.btnTypePhangCarom.Click += new System.EventHandler(this.TypeFilterButton_Click);
            // 
            // btnTypeSnooker
            // 
            this.btnTypeSnooker.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnTypeSnooker.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTypeSnooker.FlatAppearance.BorderSize = 0;
            this.btnTypeSnooker.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTypeSnooker.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnTypeSnooker.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnTypeSnooker.Location = new System.Drawing.Point(331, 3);
            this.btnTypeSnooker.Name = "btnTypeSnooker";
            this.btnTypeSnooker.Size = new System.Drawing.Size(90, 28);
            this.btnTypeSnooker.TabIndex = 3;
            this.btnTypeSnooker.Tag = "Bàn Snooker";
            this.btnTypeSnooker.Text = "Snooker";
            this.btnTypeSnooker.UseVisualStyleBackColor = false;
            this.btnTypeSnooker.Click += new System.EventHandler(this.TypeFilterButton_Click);
            // 
            // btnTypeVIPLo
            // 
            this.btnTypeVIPLo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnTypeVIPLo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTypeVIPLo.FlatAppearance.BorderSize = 0;
            this.btnTypeVIPLo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTypeVIPLo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnTypeVIPLo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnTypeVIPLo.Location = new System.Drawing.Point(427, 3);
            this.btnTypeVIPLo.Name = "btnTypeVIPLo";
            this.btnTypeVIPLo.Size = new System.Drawing.Size(90, 28);
            this.btnTypeVIPLo.TabIndex = 4;
            this.btnTypeVIPLo.Tag = "Bàn VIP Lỗ";
            this.btnTypeVIPLo.Text = "VIP Lỗ";
            this.btnTypeVIPLo.UseVisualStyleBackColor = false;
            this.btnTypeVIPLo.Click += new System.EventHandler(this.TypeFilterButton_Click);
            // 
            // btnTypeVIPPhang
            // 
            this.btnTypeVIPPhang.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnTypeVIPPhang.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTypeVIPPhang.FlatAppearance.BorderSize = 0;
            this.btnTypeVIPPhang.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTypeVIPPhang.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnTypeVIPPhang.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnTypeVIPPhang.Location = new System.Drawing.Point(523, 3);
            this.btnTypeVIPPhang.Name = "btnTypeVIPPhang";
            this.btnTypeVIPPhang.Size = new System.Drawing.Size(100, 28);
            this.btnTypeVIPPhang.TabIndex = 5;
            this.btnTypeVIPPhang.Tag = "Bàn VIP Phăng";
            this.btnTypeVIPPhang.Text = "VIP Phăng";
            this.btnTypeVIPPhang.UseVisualStyleBackColor = false;
            this.btnTypeVIPPhang.Click += new System.EventHandler(this.TypeFilterButton_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtSearch.Location = new System.Drawing.Point(15, 175);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PlaceholderText = "🔍 Tìm kiếm bàn...";
            this.txtSearch.Size = new System.Drawing.Size(820, 27);
            this.txtSearch.TabIndex = 6;
            this.txtSearch.TextChanged += new System.EventHandler(this.TxtSearch_TextChanged);
            // 
            // flpBanBia
            // 
            this.flpBanBia.AutoScroll = true;
            this.flpBanBia.BackColor = System.Drawing.Color.White;
            this.flpBanBia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpBanBia.Location = new System.Drawing.Point(0, 290);
            this.flpBanBia.Name = "flpBanBia";
            this.flpBanBia.Padding = new System.Windows.Forms.Padding(15);
            this.flpBanBia.Size = new System.Drawing.Size(850, 300);
            this.flpBanBia.TabIndex = 2;
            // 
            // QuanLyBanForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(850, 590);
            this.Controls.Add(this.flpBanBia);
            this.Controls.Add(this.pnlFilters);
            this.Controls.Add(this.pnlToolbar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "QLBanForm";
            this.Text = "Quản lý bàn";
            this.Load += new System.EventHandler(this.QLBanForm_Load);
            this.pnlToolbar.ResumeLayout(false);
            this.pnlFilters.ResumeLayout(false);
            this.pnlFilters.PerformLayout();
            this.pnlKhuVucFilters.ResumeLayout(false);
            this.pnlTrangThaiFilters.ResumeLayout(false);
            this.pnlLoaiBanFilters.ResumeLayout(false);
            this.ResumeLayout(false);

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