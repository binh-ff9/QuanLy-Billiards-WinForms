namespace Billiard.WinForm.Forms.NhanVien
{
    partial class SalaryDetailForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Khai báo các Controls chính làm biến thành viên
        private System.Windows.Forms.NumericUpDown nudBonus;
        private System.Windows.Forms.NumericUpDown nudPenalty;
        private System.Windows.Forms.TextBox txtNote; // Control này không được sử dụng trong code gốc, nhưng được khai báo
        private System.Windows.Forms.DataGridView dgvAttendance;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlSalary;
        private System.Windows.Forms.Label lblAttendance;
        private System.Windows.Forms.Panel pnlActions;

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
            this.components = new System.ComponentModel.Container();
            // Khởi tạo các controls (hoặc gọi phương thức khởi tạo)
            this.InitializeCustomControls();

            // Thuộc tính cơ bản của Form
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 750); // Đặt kích thước cố định ở đây
            this.Text = "SalaryDetailForm"; // Tên form mặc định

            // Thuộc tính tùy chỉnh đã chuyển từ .cs
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Thêm Main Panel vào Form
            this.Controls.Add(this.mainPanel);
        }

        private void InitializeCustomControls()
        {
            // Khởi tạo các controls (đã được khai báo)
            this.mainPanel = new System.Windows.Forms.Panel();
            this.pnlHeader = CreateEmployeeHeader();
            this.pnlSalary = CreateSalaryPanel();
            this.lblAttendance = new System.Windows.Forms.Label();
            this.dgvAttendance = CreateAttendanceGrid();
            this.pnlActions = CreateActionPanel();

            // Setup Main Panel
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Padding = new System.Windows.Forms.Padding(20);
            this.mainPanel.AutoScroll = true;

            int yPos = 20;

            // Header
            this.pnlHeader.Location = new System.Drawing.Point(20, yPos);
            this.mainPanel.Controls.Add(this.pnlHeader);
            yPos += 130;

            // Salary Calculation Panel
            this.pnlSalary.Location = new System.Drawing.Point(20, yPos);
            this.mainPanel.Controls.Add(this.pnlSalary);
            yPos += 240;

            // Attendance List Label
            this.lblAttendance.Text = "📋 Chi tiết chấm công";
            this.lblAttendance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblAttendance.AutoSize = true;
            this.lblAttendance.Location = new System.Drawing.Point(20, yPos);
            this.mainPanel.Controls.Add(this.lblAttendance);
            yPos += 35;

            // Attendance Grid
            this.dgvAttendance.Location = new System.Drawing.Point(20, yPos);
            this.dgvAttendance.Size = new System.Drawing.Size(820, 250);
            this.mainPanel.Controls.Add(this.dgvAttendance);
            yPos += 260;

            // Action Buttons
            this.pnlActions.Location = new System.Drawing.Point(20, yPos);
            this.mainPanel.Controls.Add(this.pnlActions);
        }

        // Chuyển các phương thức tạo Panel sang Designer.cs (để tập trung code controls)
        // Cần using System.Windows.Forms và System.Drawing
        private System.Windows.Forms.Panel CreateEmployeeHeader()
        {
            var panel = new System.Windows.Forms.Panel
            {
                Size = new System.Drawing.Size(820, 120),
                BackColor = System.Drawing.Color.FromArgb(102, 126, 234)
            };

            var pnlAvatar = new System.Windows.Forms.Panel
            {
                Size = new System.Drawing.Size(80, 80),
                Location = new System.Drawing.Point(20, 20),
                BackColor = System.Drawing.Color.FromArgb(59, 130, 246)
            };

            var lblInitial = new System.Windows.Forms.Label
            {
                // Text sẽ được gán trong LoadData
                Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                Size = new System.Drawing.Size(80, 80),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            pnlAvatar.Controls.Add(lblInitial);

            var lblName = new System.Windows.Forms.Label
            {
                // Text sẽ được gán trong LoadData
                Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                AutoSize = true,
                Location = new System.Drawing.Point(115, 25)
            };

            var lblRole = new System.Windows.Forms.Label
            {
                // Text sẽ được gán trong LoadData
                Font = new System.Drawing.Font("Segoe UI", 10F),
                ForeColor = System.Drawing.Color.FromArgb(230, 230, 255),
                AutoSize = true,
                Location = new System.Drawing.Point(115, 55)
            };

            var lblPeriod = new System.Windows.Forms.Label
            {
                // Text sẽ được gán trong LoadData
                Font = new System.Drawing.Font("Segoe UI", 10F),
                ForeColor = System.Drawing.Color.FromArgb(230, 230, 255),
                AutoSize = true,
                Location = new System.Drawing.Point(115, 80)
            };

            // Lưu trữ các labels để dễ dàng truy cập sau này
            panel.Tag = new { lblInitial, lblName, lblRole, lblPeriod };

            panel.Controls.AddRange(new System.Windows.Forms.Control[] { pnlAvatar, lblName, lblRole, lblPeriod });
            return panel;
        }

        private System.Windows.Forms.Panel CreateSalaryPanel()
        {
            var panel = new System.Windows.Forms.Panel
            {
                Size = new System.Drawing.Size(820, 230),
                BackColor = System.Drawing.Color.White
            };

            var lblTitle = new System.Windows.Forms.Label
            {
                Text = "💰 Chi tiết tính lương",
                Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold),
                AutoSize = true,
                Location = new System.Drawing.Point(15, 15)
            };
            panel.Controls.Add(lblTitle);

            int yPos = 50;

            // Các rows thông tin sẽ được thêm vào trong LoadData để tính toán
            // và gọi AddInfoRow (đã chuyển sang đây)
            yPos = AddInfoRow(panel, "Số ngày làm việc:", "0 ngày", yPos);
            yPos = AddInfoRow(panel, "Tổng giờ làm việc:", "0.00 giờ", yPos);
            yPos = AddInfoRow(panel, "Lương theo giờ:", "0đ/giờ", yPos);
            yPos = AddInfoRow(panel, "Lương cơ bản (theo giờ):", "0đ", yPos, true);
            yPos = AddInfoRow(panel, "Phụ cấp:", "0đ", yPos);


            // Bonus input
            var lblBonus = new System.Windows.Forms.Label
            {
                Text = "Thưởng:",
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                AutoSize = true,
                Location = new System.Drawing.Point(20, yPos + 5)
            };
            this.nudBonus = new System.Windows.Forms.NumericUpDown
            {
                Size = new System.Drawing.Size(200, 30),
                Location = new System.Drawing.Point(280, yPos),
                Font = new System.Drawing.Font("Segoe UI", 10F),
                Maximum = 100000000,
                ThousandsSeparator = true
            };
            panel.Controls.AddRange(new System.Windows.Forms.Control[] { lblBonus, this.nudBonus });
            yPos += 35;

            // Penalty input
            var lblPenalty = new System.Windows.Forms.Label
            {
                Text = "Phạt:",
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                AutoSize = true,
                Location = new System.Drawing.Point(20, yPos + 5)
            };
            this.nudPenalty = new System.Windows.Forms.NumericUpDown
            {
                Size = new System.Drawing.Size(200, 30),
                Location = new System.Drawing.Point(280, yPos),
                Font = new System.Drawing.Font("Segoe UI", 10F),
                Maximum = 100000000,
                ThousandsSeparator = true
            };

            panel.Controls.AddRange(new System.Windows.Forms.Control[] { lblPenalty, this.nudPenalty });

            return panel;
        }

        private int AddInfoRow(System.Windows.Forms.Panel panel, string label, string value, int yPos, bool highlight = false)
        {
            var lblLabel = new System.Windows.Forms.Label
            {
                Text = label,
                Font = new System.Drawing.Font("Segoe UI", 10F, highlight ? System.Drawing.FontStyle.Bold : System.Drawing.FontStyle.Regular),
                AutoSize = true,
                Location = new System.Drawing.Point(20, yPos)
            };

            var lblValue = new System.Windows.Forms.Label
            {
                Text = value,
                Font = new System.Drawing.Font("Segoe UI", 10F, highlight ? System.Drawing.FontStyle.Bold : System.Drawing.FontStyle.Regular),
                ForeColor = highlight ? System.Drawing.Color.FromArgb(34, 197, 94) : System.Drawing.Color.FromArgb(30, 41, 59),
                AutoSize = false,
                Size = new System.Drawing.Size(300, 25),
                Location = new System.Drawing.Point(280, yPos),
                TextAlign = System.Drawing.ContentAlignment.MiddleRight
            };

            panel.Controls.AddRange(new System.Windows.Forms.Control[] { lblLabel, lblValue });
            // Gán Tag để LoadData có thể tìm và cập nhật giá trị
            lblValue.Tag = label;
            return yPos + 30;
        }

        private System.Windows.Forms.DataGridView CreateAttendanceGrid()
        {
            var dgv = new System.Windows.Forms.DataGridView
            {
                BackgroundColor = System.Drawing.Color.White,
                BorderStyle = System.Windows.Forms.BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                Font = new System.Drawing.Font("Segoe UI", 9F),
                RowTemplate = { Height = 35 }
            };

            dgv.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Ngay", HeaderText = "Ngày", Width = 120 });
            dgv.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "GioVao", HeaderText = "Giờ vào", Width = 100 });
            dgv.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "GioRa", HeaderText = "Giờ ra", Width = 100 });
            dgv.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn
            {
                Name = "SoGio",
                HeaderText = "Số giờ",
                Width = 90,
                DefaultCellStyle = { Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, Format = "N2" }
            });
            dgv.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "TrangThai", HeaderText = "Trạng thái", Width = 120 });
            dgv.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "GhiChu", HeaderText = "Ghi chú", Width = 200 });

            dgv.ColumnHeadersDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle
            {
                BackColor = System.Drawing.Color.FromArgb(102, 126, 234),
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold),
                Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
            };

            dgv.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);

            return dgv;
        }

        private System.Windows.Forms.Panel CreateActionPanel()
        {
            var panel = new System.Windows.Forms.Panel
            {
                Size = new System.Drawing.Size(820, 60),
                BackColor = System.Drawing.Color.Transparent
            };

            var btnSave = new System.Windows.Forms.Button
            {
                Text = "💾 Lưu & Tính lương",
                Size = new System.Drawing.Size(150, 45),
                Location = new System.Drawing.Point(520, 8),
                BackColor = System.Drawing.Color.FromArgb(34, 197, 94),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Cursor = System.Windows.Forms.Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click; // Gán sự kiện

            var btnCancel = new System.Windows.Forms.Button
            {
                Text = "Hủy",
                Size = new System.Drawing.Size(120, 45),
                Location = new System.Drawing.Point(680, 8),
                BackColor = System.Drawing.Color.FromArgb(108, 117, 125),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 10F),
                Cursor = System.Windows.Forms.Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close(); // Gán sự kiện

            panel.Controls.AddRange(new System.Windows.Forms.Control[] { btnSave, btnCancel });
            return panel;
        }

        #endregion
    }
}