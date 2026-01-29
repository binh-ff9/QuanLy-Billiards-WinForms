namespace Billiard.WinForm.Forms.NhanVien
{
    partial class ScheduleForm
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
            pnlHeader = new Panel();
            btnExport = new Button();
            btnRefresh = new Button();
            btnToday = new Button();
            pnlWeekNav = new Panel();
            btnNextWeek = new Button();
            lblWeekDisplay = new Label();
            btnPrevWeek = new Button();
            lblSubtitle = new Label();
            pnlCalendar = new Panel();
            pnlHeader.SuspendLayout();
            pnlWeekNav.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(30, 41, 59);
            pnlHeader.Controls.Add(btnExport);
            pnlHeader.Controls.Add(btnRefresh);
            pnlHeader.Controls.Add(btnToday);
            pnlHeader.Controls.Add(pnlWeekNav);
            pnlHeader.Controls.Add(lblSubtitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(30, 20, 30, 20);
            pnlHeader.Size = new Size(1628, 140);
            pnlHeader.TabIndex = 0;
            // 
            // btnExport
            // 
            btnExport.BackColor = Color.FromArgb(168, 85, 247);
            btnExport.Cursor = Cursors.Hand;
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.FlatAppearance.MouseOverBackColor = Color.FromArgb(147, 51, 234);
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnExport.ForeColor = Color.White;
            btnExport.Location = new Point(799, 64);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(130, 45);
            btnExport.TabIndex = 5;
            btnExport.Text = "Xuất Excel";
            btnExport.UseVisualStyleBackColor = false;
            btnExport.Click += BtnExport_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(34, 197, 94);
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatAppearance.MouseOverBackColor = Color.FromArgb(22, 163, 74);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(658, 64);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(120, 45);
            btnRefresh.TabIndex = 4;
            btnRefresh.Text = "Làm mới";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += BtnRefresh_Click;
            // 
            // btnToday
            // 
            btnToday.BackColor = Color.FromArgb(14, 165, 233);
            btnToday.Cursor = Cursors.Hand;
            btnToday.FlatAppearance.BorderSize = 0;
            btnToday.FlatAppearance.MouseOverBackColor = Color.FromArgb(2, 132, 199);
            btnToday.FlatStyle = FlatStyle.Flat;
            btnToday.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnToday.ForeColor = Color.White;
            btnToday.Location = new Point(513, 64);
            btnToday.Name = "btnToday";
            btnToday.Size = new Size(120, 45);
            btnToday.TabIndex = 3;
            btnToday.Text = "Hôm nay";
            btnToday.UseVisualStyleBackColor = false;
            btnToday.Click += BtnToday_Click;
            // 
            // pnlWeekNav
            // 
            pnlWeekNav.BackColor = Color.Transparent;
            pnlWeekNav.Controls.Add(btnNextWeek);
            pnlWeekNav.Controls.Add(lblWeekDisplay);
            pnlWeekNav.Controls.Add(btnPrevWeek);
            pnlWeekNav.Location = new Point(30, 64);
            pnlWeekNav.Name = "pnlWeekNav";
            pnlWeekNav.Size = new Size(465, 45);
            pnlWeekNav.TabIndex = 2;
            // 
            // btnNextWeek
            // 
            btnNextWeek.BackColor = Color.FromArgb(71, 85, 105);
            btnNextWeek.Cursor = Cursors.Hand;
            btnNextWeek.FlatAppearance.BorderSize = 0;
            btnNextWeek.FlatAppearance.MouseOverBackColor = Color.FromArgb(100, 116, 139);
            btnNextWeek.FlatStyle = FlatStyle.Flat;
            btnNextWeek.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnNextWeek.ForeColor = Color.White;
            btnNextWeek.Location = new Point(337, 4);
            btnNextWeek.Name = "btnNextWeek";
            btnNextWeek.Size = new Size(140, 34);
            btnNextWeek.TabIndex = 2;
            btnNextWeek.Text = "Tuần sau ▶";
            btnNextWeek.UseVisualStyleBackColor = false;
            btnNextWeek.Click += BtnNextWeek_Click;
            // 
            // lblWeekDisplay
            // 
            lblWeekDisplay.BackColor = Color.FromArgb(51, 65, 85);
            lblWeekDisplay.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblWeekDisplay.ForeColor = Color.White;
            lblWeekDisplay.Location = new Point(123, 4);
            lblWeekDisplay.Name = "lblWeekDisplay";
            lblWeekDisplay.Size = new Size(210, 34);
            lblWeekDisplay.TabIndex = 1;
            lblWeekDisplay.Text = "📅 01/01 - 07/01/2025";
            lblWeekDisplay.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnPrevWeek
            // 
            btnPrevWeek.BackColor = Color.FromArgb(71, 85, 105);
            btnPrevWeek.Cursor = Cursors.Hand;
            btnPrevWeek.FlatAppearance.BorderSize = 0;
            btnPrevWeek.FlatAppearance.MouseOverBackColor = Color.FromArgb(100, 116, 139);
            btnPrevWeek.FlatStyle = FlatStyle.Flat;
            btnPrevWeek.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnPrevWeek.ForeColor = Color.White;
            btnPrevWeek.Location = new Point(0, 4);
            btnPrevWeek.Name = "btnPrevWeek";
            btnPrevWeek.Size = new Size(120, 34);
            btnPrevWeek.TabIndex = 0;
            btnPrevWeek.Text = "◀ Tuần trước";
            btnPrevWeek.UseVisualStyleBackColor = false;
            btnPrevWeek.Click += BtnPrevWeek_Click;
            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 11F);
            lblSubtitle.ForeColor = Color.FromArgb(148, 163, 184);
            lblSubtitle.Location = new Point(12, 20);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(811, 30);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Xếp lịch làm việc cho nhân viên theo tuần • Part-time & Full-time • 8h sáng - 2h sáng";
            // 
            // pnlCalendar
            // 
            pnlCalendar.AutoScroll = true;
            pnlCalendar.BackColor = Color.FromArgb(248, 250, 252);
            pnlCalendar.Dock = DockStyle.Fill;
            pnlCalendar.Location = new Point(0, 140);
            pnlCalendar.Name = "pnlCalendar";
            pnlCalendar.Padding = new Padding(30, 20, 30, 20);
            pnlCalendar.Size = new Size(1628, 824);
            pnlCalendar.TabIndex = 1;
            // 
            // ScheduleForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1628, 964);
            Controls.Add(pnlCalendar);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(1400, 800);
            Name = "ScheduleForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản lý lịch làm việc";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlWeekNav.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Panel pnlWeekNav;
        private System.Windows.Forms.Button btnPrevWeek;
        private System.Windows.Forms.Label lblWeekDisplay;
        private System.Windows.Forms.Button btnNextWeek;
        private System.Windows.Forms.Button btnToday;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Panel pnlCalendar;
    }
}