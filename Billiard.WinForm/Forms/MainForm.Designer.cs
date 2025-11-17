namespace Billiard.WinForm
{
    partial class MainForm
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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pnlUserInfo = new System.Windows.Forms.Panel();
            this.btnLogout = new System.Windows.Forms.Button();
            this.lblUserRole = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblUserAvatar = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();

            this.pnlStats = new System.Windows.Forms.Panel();
            this.pnlStatTrong = new System.Windows.Forms.Panel();
            this.lblBanTrongValue = new System.Windows.Forms.Label();
            this.lblBanTrongLabel = new System.Windows.Forms.Label();

            this.pnlStatDangChoi = new System.Windows.Forms.Panel();
            this.lblDangChoiValue = new System.Windows.Forms.Label();
            this.lblDangChoiLabel = new System.Windows.Forms.Label();

            this.pnlStatDatTruoc = new System.Windows.Forms.Panel();
            this.lblDatTruocValue = new System.Windows.Forms.Label();
            this.lblDatTruocLabel = new System.Windows.Forms.Label();

            this.pnlStatDoanhThu = new System.Windows.Forms.Panel();
            this.lblDoanhThuValue = new System.Windows.Forms.Label();
            this.lblDoanhThuLabel = new System.Windows.Forms.Label();

            this.pnlStatKhachHang = new System.Windows.Forms.Panel();
            this.lblKhachHangValue = new System.Windows.Forms.Label();
            this.lblKhachHangLabel = new System.Windows.Forms.Label();

            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.btnQuanLyBan = new System.Windows.Forms.Button();
            this.btnDichVu = new System.Windows.Forms.Button();
            this.btnHoaDon = new System.Windows.Forms.Button();
            this.btnKhachHang = new System.Windows.Forms.Button();
            this.btnThongKe = new System.Windows.Forms.Button();
            this.btnNhanVien = new System.Windows.Forms.Button();
            this.btnCaiDat = new System.Windows.Forms.Button();

            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlDetail = new System.Windows.Forms.Panel();
            this.lblDetailTitle = new System.Windows.Forms.Label();

            this.pnlHeader.SuspendLayout();
            this.pnlUserInfo.SuspendLayout();
            this.pnlStats.SuspendLayout();
            this.pnlSidebar.SuspendLayout();
            this.pnlDetail.SuspendLayout();
            this.SuspendLayout();

            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(28, 37, 54);
            this.pnlHeader.Controls.Add(this.pnlUserInfo);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1400, 80);
            this.pnlHeader.TabIndex = 0;

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 25);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(350, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "🎱 Quản Lý Quán Bi-a Pro";

            // 
            // pnlUserInfo
            // 
            this.pnlUserInfo.Controls.Add(this.btnLogout);
            this.pnlUserInfo.Controls.Add(this.lblUserRole);
            this.pnlUserInfo.Controls.Add(this.lblUserName);
            this.pnlUserInfo.Controls.Add(this.lblUserAvatar);
            this.pnlUserInfo.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlUserInfo.Location = new System.Drawing.Point(1000, 0);
            this.pnlUserInfo.Name = "pnlUserInfo";
            this.pnlUserInfo.Size = new System.Drawing.Size(400, 80);
            this.pnlUserInfo.TabIndex = 1;

            // 
            // lblUserAvatar
            // 
            this.lblUserAvatar.BackColor = System.Drawing.Color.FromArgb(99, 102, 241);
            this.lblUserAvatar.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblUserAvatar.ForeColor = System.Drawing.Color.White;
            this.lblUserAvatar.Location = new System.Drawing.Point(20, 15);
            this.lblUserAvatar.Name = "lblUserAvatar";
            this.lblUserAvatar.Size = new System.Drawing.Size(50, 50);
            this.lblUserAvatar.TabIndex = 0;
            this.lblUserAvatar.Text = "NV";
            this.lblUserAvatar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblUserName.ForeColor = System.Drawing.Color.White;
            this.lblUserName.Location = new System.Drawing.Point(80, 20);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(90, 20);
            this.lblUserName.TabIndex = 1;
            this.lblUserName.Text = "Nhân viên";

            // 
            // lblUserRole
            // 
            this.lblUserRole.AutoSize = true;
            this.lblUserRole.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblUserRole.ForeColor = System.Drawing.Color.FromArgb(156, 163, 175);
            this.lblUserRole.Location = new System.Drawing.Point(80, 42);
            this.lblUserRole.Name = "lblUserRole";
            this.lblUserRole.Size = new System.Drawing.Size(70, 15);
            this.lblUserRole.TabIndex = 2;
            this.lblUserRole.Text = "Nhân viên";

            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(260, 20);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(120, 40);
            this.btnLogout.TabIndex = 3;
            this.btnLogout.Text = "Đăng xuất";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.BtnLogout_Click);
            this.btnLogout.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.btnLogout.MouseLeave += new System.EventHandler(this.Button_MouseLeave);

            // 
            // pnlStats
            // 
            this.pnlStats.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.pnlStats.Controls.Add(this.pnlStatTrong);
            this.pnlStats.Controls.Add(this.pnlStatDangChoi);
            this.pnlStats.Controls.Add(this.pnlStatDatTruoc);
            this.pnlStats.Controls.Add(this.pnlStatDoanhThu);
            this.pnlStats.Controls.Add(this.pnlStatKhachHang);
            this.pnlStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStats.Location = new System.Drawing.Point(0, 80);
            this.pnlStats.Name = "pnlStats";
            this.pnlStats.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
            this.pnlStats.Size = new System.Drawing.Size(1400, 130);
            this.pnlStats.TabIndex = 1;

            // Create stat cards
            CreateStatCard(this.pnlStatTrong, this.lblBanTrongValue, this.lblBanTrongLabel, "Bàn Trống", 20);
            CreateStatCard(this.pnlStatDangChoi, this.lblDangChoiValue, this.lblDangChoiLabel, "Đang Chơi", 280);
            CreateStatCard(this.pnlStatDatTruoc, this.lblDatTruocValue, this.lblDatTruocLabel, "Đặt Trước", 540);
            CreateStatCard(this.pnlStatDoanhThu, this.lblDoanhThuValue, this.lblDoanhThuLabel, "Doanh Thu Hôm Nay", 800);
            CreateStatCard(this.pnlStatKhachHang, this.lblKhachHangValue, this.lblKhachHangLabel, "Khách Hàng", 1060);

            // 
            // pnlSidebar
            // 
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.pnlSidebar.Controls.Add(this.btnQuanLyBan);
            this.pnlSidebar.Controls.Add(this.btnDichVu);
            this.pnlSidebar.Controls.Add(this.btnHoaDon);
            this.pnlSidebar.Controls.Add(this.btnKhachHang);
            this.pnlSidebar.Controls.Add(this.btnThongKe);
            this.pnlSidebar.Controls.Add(this.btnNhanVien);
            this.pnlSidebar.Controls.Add(this.btnCaiDat);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 210);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(250, 590);
            this.pnlSidebar.TabIndex = 2;

            // Create sidebar buttons
            CreateSidebarButton(this.btnQuanLyBan, "📊 Quản lý bàn", 10);
            CreateSidebarButton(this.btnDichVu, "🍴 Dịch vụ & Menu", 70);
            CreateSidebarButton(this.btnHoaDon, "📄 Hóa đơn", 130);
            CreateSidebarButton(this.btnKhachHang, "👥 Khách hàng", 190);
            CreateSidebarButton(this.btnThongKe, "📈 Thống kê", 250);
            CreateSidebarButton(this.btnNhanVien, "👨‍💼 Nhân viên", 310);
            CreateSidebarButton(this.btnCaiDat, "⚙️ Cài đặt", 370);

            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.White;
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(250, 210);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(20);
            this.pnlMain.Size = new System.Drawing.Size(850, 590);
            this.pnlMain.TabIndex = 3;

            // 
            // pnlDetail
            // 
            this.pnlDetail.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.pnlDetail.Controls.Add(this.lblDetailTitle);
            this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlDetail.Location = new System.Drawing.Point(1100, 210);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Padding = new System.Windows.Forms.Padding(15);
            this.pnlDetail.Size = new System.Drawing.Size(300, 590);
            this.pnlDetail.TabIndex = 4;

            // 
            // lblDetailTitle
            // 
            this.lblDetailTitle.AutoSize = true;
            this.lblDetailTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblDetailTitle.Location = new System.Drawing.Point(15, 15);
            this.lblDetailTitle.Name = "lblDetailTitle";
            this.lblDetailTitle.Size = new System.Drawing.Size(70, 21);
            this.lblDetailTitle.TabIndex = 0;
            this.lblDetailTitle.Text = "Chi tiết";

            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 800);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlDetail);
            this.Controls.Add(this.pnlSidebar);
            this.Controls.Add(this.pnlStats);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản Lý Quán Bi-a Pro";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlUserInfo.ResumeLayout(false);
            this.pnlUserInfo.PerformLayout();
            this.pnlStats.ResumeLayout(false);
            this.pnlSidebar.ResumeLayout(false);
            this.pnlDetail.ResumeLayout(false);
            this.pnlDetail.PerformLayout();
            this.ResumeLayout(false);
        }

        private void CreateStatCard(Panel panel, Label valueLabel, Label textLabel, string labelText, int x)
        {
            panel.BackColor = System.Drawing.Color.White;
            panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel.Location = new System.Drawing.Point(x, 15);
            panel.Name = $"pnlStat{labelText.Replace(" ", "")}";
            panel.Size = new System.Drawing.Size(240, 100);
            panel.TabIndex = 0;

            valueLabel.AutoSize = false;
            valueLabel.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            valueLabel.ForeColor = System.Drawing.Color.FromArgb(99, 102, 241);
            valueLabel.Location = new System.Drawing.Point(15, 15);
            valueLabel.Name = $"lbl{labelText.Replace(" ", "")}Value";
            valueLabel.Size = new System.Drawing.Size(210, 40);
            valueLabel.TabIndex = 0;
            valueLabel.Text = "0";

            textLabel.AutoSize = false;
            textLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            textLabel.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            textLabel.Location = new System.Drawing.Point(15, 60);
            textLabel.Name = $"lbl{labelText.Replace(" ", "")}Label";
            textLabel.Size = new System.Drawing.Size(210, 25);
            textLabel.TabIndex = 1;
            textLabel.Text = labelText;

            panel.Controls.Add(valueLabel);
            panel.Controls.Add(textLabel);
            this.pnlStats.Controls.Add(panel);
        }

        private void CreateSidebarButton(Button btn, string text, int y)
        {
            btn.BackColor = System.Drawing.Color.Transparent;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn.Font = new System.Drawing.Font("Segoe UI", 11F);
            btn.ForeColor = System.Drawing.Color.White;
            btn.Location = new System.Drawing.Point(10, y);
            btn.Name = $"btn{text.Substring(2).Replace(" ", "")}";
            btn.Size = new System.Drawing.Size(230, 50);
            btn.TabIndex = 0;
            btn.Text = text;
            btn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btn.UseVisualStyleBackColor = false;
            btn.Click += new System.EventHandler(this.SidebarButton_Click);
            btn.MouseEnter += new System.EventHandler(this.SidebarButton_MouseEnter);
            btn.MouseLeave += new System.EventHandler(this.SidebarButton_MouseLeave);
            this.pnlSidebar.Controls.Add(btn);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlUserInfo;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Label lblUserRole;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblUserAvatar;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlStats;
        private System.Windows.Forms.Panel pnlStatTrong;
        private System.Windows.Forms.Label lblBanTrongValue;
        private System.Windows.Forms.Label lblBanTrongLabel;
        private System.Windows.Forms.Panel pnlStatDangChoi;
        private System.Windows.Forms.Label lblDangChoiValue;
        private System.Windows.Forms.Label lblDangChoiLabel;
        private System.Windows.Forms.Panel pnlStatDatTruoc;
        private System.Windows.Forms.Label lblDatTruocValue;
        private System.Windows.Forms.Label lblDatTruocLabel;
        private System.Windows.Forms.Panel pnlStatDoanhThu;
        private System.Windows.Forms.Label lblDoanhThuValue;
        private System.Windows.Forms.Label lblDoanhThuLabel;
        private System.Windows.Forms.Panel pnlStatKhachHang;
        private System.Windows.Forms.Label lblKhachHangValue;
        private System.Windows.Forms.Label lblKhachHangLabel;
        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Button btnQuanLyBan;
        private System.Windows.Forms.Button btnDichVu;
        private System.Windows.Forms.Button btnHoaDon;
        private System.Windows.Forms.Button btnKhachHang;
        private System.Windows.Forms.Button btnThongKe;
        private System.Windows.Forms.Button btnNhanVien;
        private System.Windows.Forms.Button btnCaiDat;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlDetail;
        private System.Windows.Forms.Label lblDetailTitle;
    }
}