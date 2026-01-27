namespace Billiard.WinForm.Forms.QLBan
{
    partial class LichDatBanForm
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
            pnlHeader = new Panel();
            btnToday = new Button();
            btnSelectDate = new Button();
            btnNextWeek = new Button();
            btnPrevWeek = new Button();
            lblWeekRange = new Label();
            lblTitle = new Label();
            pnlLegend = new Panel();
            lblLegendQuaKhu = new Label();
            pnlLegendQuaKhu = new Panel();
            lblLegendQuaGio = new Label();
            pnlLegendQuaGio = new Panel();
            lblLegendSapDen = new Label();
            pnlLegendSapDen = new Panel();
            lblLegendDaDat = new Label();
            pnlLegendDaDat = new Panel();
            lblLegendDangCho = new Label();
            pnlLegendDangCho = new Panel();
            lblLegendTitle = new Label();
            pnlCalendar = new Panel();
            pnlHeader.SuspendLayout();
            pnlLegend.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(99, 102, 241);
            pnlHeader.Controls.Add(btnToday);
            pnlHeader.Controls.Add(btnSelectDate);
            pnlHeader.Controls.Add(btnNextWeek);
            pnlHeader.Controls.Add(btnPrevWeek);
            pnlHeader.Controls.Add(lblWeekRange);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(20);
            pnlHeader.Size = new Size(1600, 80);
            pnlHeader.TabIndex = 0;
            // 
            // btnToday
            // 
            btnToday.BackColor = Color.FromArgb(34, 197, 94);
            btnToday.FlatStyle = FlatStyle.Flat;
            btnToday.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnToday.ForeColor = Color.White;
            btnToday.Location = new Point(1400, 16);
            btnToday.Name = "btnToday";
            btnToday.Size = new Size(180, 50);
            btnToday.TabIndex = 5;
            btnToday.Text = "🏠 Hôm nay";
            btnToday.UseVisualStyleBackColor = false;
            btnToday.Click += btnToday_Click;
            // 
            // btnSelectDate
            // 
            btnSelectDate.BackColor = Color.FromArgb(255, 255, 255);
            btnSelectDate.FlatStyle = FlatStyle.Flat;
            btnSelectDate.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSelectDate.ForeColor = Color.FromArgb(99, 102, 241);
            btnSelectDate.Location = new Point(1200, 16);
            btnSelectDate.Name = "btnSelectDate";
            btnSelectDate.Size = new Size(180, 50);
            btnSelectDate.TabIndex = 4;
            btnSelectDate.Text = "📆 Chọn ngày";
            btnSelectDate.UseVisualStyleBackColor = false;
            btnSelectDate.Click += btnSelectDate_Click;
            // 
            // btnNextWeek
            // 
            btnNextWeek.BackColor = Color.White;
            btnNextWeek.FlatStyle = FlatStyle.Flat;
            btnNextWeek.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnNextWeek.ForeColor = Color.FromArgb(99, 102, 241);
            btnNextWeek.Location = new Point(1017, 16);
            btnNextWeek.Name = "btnNextWeek";
            btnNextWeek.Size = new Size(110, 50);
            btnNextWeek.TabIndex = 3;
            btnNextWeek.Text = "Sau ▶";
            btnNextWeek.UseVisualStyleBackColor = false;
            btnNextWeek.Click += btnNextWeek_Click;
            // 
            // btnPrevWeek
            // 
            btnPrevWeek.BackColor = Color.White;
            btnPrevWeek.FlatStyle = FlatStyle.Flat;
            btnPrevWeek.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnPrevWeek.ForeColor = Color.FromArgb(99, 102, 241);
            btnPrevWeek.Location = new Point(346, 16);
            btnPrevWeek.Name = "btnPrevWeek";
            btnPrevWeek.Size = new Size(110, 50);
            btnPrevWeek.TabIndex = 2;
            btnPrevWeek.Text = "◀ Trước";
            btnPrevWeek.UseVisualStyleBackColor = false;
            btnPrevWeek.Click += btnPrevWeek_Click;
            // 
            // lblWeekRange
            // 
            lblWeekRange.AutoSize = true;
            lblWeekRange.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblWeekRange.ForeColor = Color.White;
            lblWeekRange.Location = new Point(476, 26);
            lblWeekRange.Name = "lblWeekRange";
            lblWeekRange.Size = new Size(537, 36);
            lblWeekRange.TabIndex = 1;
            lblWeekRange.Text = "Thứ 2, 27/01/2026 - Chủ nhật, 02/02/2026";
            lblWeekRange.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 17);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(286, 48);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "📅 Lịch đặt bàn";
            // 
            // pnlLegend
            // 
            pnlLegend.BackColor = Color.FromArgb(248, 250, 252);
            pnlLegend.Controls.Add(lblLegendQuaKhu);
            pnlLegend.Controls.Add(pnlLegendQuaKhu);
            pnlLegend.Controls.Add(lblLegendQuaGio);
            pnlLegend.Controls.Add(pnlLegendQuaGio);
            pnlLegend.Controls.Add(lblLegendSapDen);
            pnlLegend.Controls.Add(pnlLegendSapDen);
            pnlLegend.Controls.Add(lblLegendDaDat);
            pnlLegend.Controls.Add(pnlLegendDaDat);
            pnlLegend.Controls.Add(lblLegendDangCho);
            pnlLegend.Controls.Add(pnlLegendDangCho);
            pnlLegend.Controls.Add(lblLegendTitle);
            pnlLegend.Dock = DockStyle.Top;
            pnlLegend.Location = new Point(0, 80);
            pnlLegend.Name = "pnlLegend";
            pnlLegend.Padding = new Padding(20, 10, 20, 10);
            pnlLegend.Size = new Size(1600, 48);
            pnlLegend.TabIndex = 1;
            // 
            // lblLegendQuaKhu
            // 
            lblLegendQuaKhu.AutoSize = true;
            lblLegendQuaKhu.Font = new Font("Segoe UI", 9F);
            lblLegendQuaKhu.Location = new Point(775, 12);
            lblLegendQuaKhu.Name = "lblLegendQuaKhu";
            lblLegendQuaKhu.Size = new Size(80, 25);
            lblLegendQuaKhu.TabIndex = 10;
            lblLegendQuaKhu.Text = "Quá khứ";
            // 
            // pnlLegendQuaKhu
            // 
            pnlLegendQuaKhu.BackColor = Color.FromArgb(229, 231, 235);
            pnlLegendQuaKhu.BorderStyle = BorderStyle.FixedSingle;
            pnlLegendQuaKhu.Location = new Point(740, 12);
            pnlLegendQuaKhu.Name = "pnlLegendQuaKhu";
            pnlLegendQuaKhu.Size = new Size(30, 25);
            pnlLegendQuaKhu.TabIndex = 9;
            // 
            // lblLegendQuaGio
            // 
            lblLegendQuaGio.AutoSize = true;
            lblLegendQuaGio.Font = new Font("Segoe UI", 9F);
            lblLegendQuaGio.Location = new Point(645, 12);
            lblLegendQuaGio.Name = "lblLegendQuaGio";
            lblLegendQuaGio.Size = new Size(76, 25);
            lblLegendQuaGio.TabIndex = 8;
            lblLegendQuaGio.Text = "Quá giờ";
            // 
            // pnlLegendQuaGio
            // 
            pnlLegendQuaGio.BackColor = Color.FromArgb(254, 202, 202);
            pnlLegendQuaGio.BorderStyle = BorderStyle.FixedSingle;
            pnlLegendQuaGio.Location = new Point(610, 12);
            pnlLegendQuaGio.Name = "pnlLegendQuaGio";
            pnlLegendQuaGio.Size = new Size(30, 25);
            pnlLegendQuaGio.TabIndex = 7;
            // 
            // lblLegendSapDen
            // 
            lblLegendSapDen.AutoSize = true;
            lblLegendSapDen.Font = new Font("Segoe UI", 9F);
            lblLegendSapDen.Location = new Point(435, 12);
            lblLegendSapDen.Name = "lblLegendSapDen";
            lblLegendSapDen.Size = new Size(171, 25);
            lblLegendSapDen.TabIndex = 6;
            lblLegendSapDen.Text = "Sắp đến (< 30 phút)";
            // 
            // pnlLegendSapDen
            // 
            pnlLegendSapDen.BackColor = Color.FromArgb(254, 249, 195);
            pnlLegendSapDen.BorderStyle = BorderStyle.FixedSingle;
            pnlLegendSapDen.Location = new Point(400, 12);
            pnlLegendSapDen.Name = "pnlLegendSapDen";
            pnlLegendSapDen.Size = new Size(30, 25);
            pnlLegendSapDen.TabIndex = 5;
            // 
            // lblLegendDaDat
            // 
            lblLegendDaDat.AutoSize = true;
            lblLegendDaDat.Font = new Font("Segoe UI", 9F);
            lblLegendDaDat.Location = new Point(315, 12);
            lblLegendDaDat.Name = "lblLegendDaDat";
            lblLegendDaDat.Size = new Size(65, 25);
            lblLegendDaDat.TabIndex = 4;
            lblLegendDaDat.Text = "Đã đặt";
            // 
            // pnlLegendDaDat
            // 
            pnlLegendDaDat.BackColor = Color.FromArgb(187, 247, 208);
            pnlLegendDaDat.BorderStyle = BorderStyle.FixedSingle;
            pnlLegendDaDat.Location = new Point(280, 12);
            pnlLegendDaDat.Name = "pnlLegendDaDat";
            pnlLegendDaDat.Size = new Size(30, 25);
            pnlLegendDaDat.TabIndex = 3;
            // 
            // lblLegendDangCho
            // 
            lblLegendDangCho.AutoSize = true;
            lblLegendDangCho.Font = new Font("Segoe UI", 9F);
            lblLegendDangCho.Location = new Point(175, 12);
            lblLegendDangCho.Name = "lblLegendDangCho";
            lblLegendDangCho.Size = new Size(89, 25);
            lblLegendDangCho.TabIndex = 2;
            lblLegendDangCho.Text = "Đang chờ";
            // 
            // pnlLegendDangCho
            // 
            pnlLegendDangCho.BackColor = Color.White;
            pnlLegendDangCho.BorderStyle = BorderStyle.FixedSingle;
            pnlLegendDangCho.Location = new Point(140, 12);
            pnlLegendDangCho.Name = "pnlLegendDangCho";
            pnlLegendDangCho.Size = new Size(30, 25);
            pnlLegendDangCho.TabIndex = 1;
            // 
            // lblLegendTitle
            // 
            lblLegendTitle.AutoSize = true;
            lblLegendTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblLegendTitle.Location = new Point(20, 9);
            lblLegendTitle.Name = "lblLegendTitle";
            lblLegendTitle.Size = new Size(107, 28);
            lblLegendTitle.TabIndex = 0;
            lblLegendTitle.Text = "Chú thích:";
            // 
            // pnlCalendar
            // 
            pnlCalendar.AutoScroll = true;
            pnlCalendar.BackColor = Color.White;
            pnlCalendar.Dock = DockStyle.Fill;
            pnlCalendar.Location = new Point(0, 128);
            pnlCalendar.Name = "pnlCalendar";
            pnlCalendar.Padding = new Padding(10);
            pnlCalendar.Size = new Size(1600, 772);
            pnlCalendar.TabIndex = 2;
            // 
            // LichDatBanForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1600, 900);
            Controls.Add(pnlCalendar);
            Controls.Add(pnlLegend);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 9F);
            Name = "LichDatBanForm";
            Text = "Lịch đặt bàn";
            Load += LichDatBanForm_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlLegend.ResumeLayout(false);
            pnlLegend.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblWeekRange;
        private Button btnPrevWeek;
        private Button btnNextWeek;
        private Button btnSelectDate;
        private Button btnToday;
        private Panel pnlLegend;
        private Label lblLegendTitle;
        private Panel pnlLegendDangCho;
        private Label lblLegendDangCho;
        private Panel pnlLegendDaDat;
        private Label lblLegendDaDat;
        private Panel pnlLegendSapDen;
        private Label lblLegendSapDen;
        private Panel pnlLegendQuaGio;
        private Label lblLegendQuaGio;
        private Panel pnlLegendQuaKhu;
        private Label lblLegendQuaKhu;
        private Panel pnlCalendar;
    }
}