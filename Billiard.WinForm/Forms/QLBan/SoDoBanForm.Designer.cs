namespace Billiard.WinForm.Forms.QLBan
{
    partial class SoDoBanForm
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
            lblTitle = new Label();
            btnClose = new Button();
            pnlControls = new Panel();
            pnlFloorTabs = new FlowLayoutPanel();
            btnFloorTang1 = new Button();
            btnFloorTang2 = new Button();
            btnFloorVIP = new Button();
            pnlEditControls = new Panel();
            btnEdit = new Button();
            btnSave = new Button();
            btnCancel = new Button();
            pnlBody = new Panel();
            pnlCanvas = new Panel();
            pnlLegend = new Panel();
            lblLegendTitle = new Label();
            pnlLegendItems = new FlowLayoutPanel();
            pnlLegendTrong = new Panel();
            lblLegendTrong = new Label();
            pnlLegendDangChoi = new Panel();
            lblLegendDangChoi = new Label();
            pnlLegendDaDat = new Panel();
            lblLegendDaDat = new Label();
            pnlInstructions = new Panel();
            lblInstructionsTitle = new Label();
            lblInstructions = new Label();
            pnlHeader.SuspendLayout();
            pnlControls.SuspendLayout();
            pnlFloorTabs.SuspendLayout();
            pnlEditControls.SuspendLayout();
            pnlBody.SuspendLayout();
            pnlLegend.SuspendLayout();
            pnlLegendItems.SuspendLayout();
            pnlLegendTrong.SuspendLayout();
            pnlLegendDangChoi.SuspendLayout();
            pnlLegendDaDat.SuspendLayout();
            pnlInstructions.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(99, 102, 241);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(btnClose);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1200, 70);
            pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 17);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(251, 48);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "🗺️ Sơ đồ bàn";
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BackColor = Color.Transparent;
            btnClose.Cursor = Cursors.Hand;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(1140, 10);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(50, 50);
            btnClose.TabIndex = 1;
            btnClose.Text = "✕";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += BtnClose_Click;
            // 
            // pnlControls
            // 
            pnlControls.BackColor = Color.White;
            pnlControls.BorderStyle = BorderStyle.FixedSingle;
            pnlControls.Controls.Add(pnlFloorTabs);
            pnlControls.Controls.Add(pnlEditControls);
            pnlControls.Dock = DockStyle.Top;
            pnlControls.Location = new Point(0, 70);
            pnlControls.Name = "pnlControls";
            pnlControls.Padding = new Padding(15);
            pnlControls.Size = new Size(1200, 80);
            pnlControls.TabIndex = 1;
            // 
            // pnlFloorTabs
            // 
            pnlFloorTabs.Controls.Add(btnFloorTang1);
            pnlFloorTabs.Controls.Add(btnFloorTang2);
            pnlFloorTabs.Controls.Add(btnFloorVIP);
            pnlFloorTabs.Dock = DockStyle.Left;
            pnlFloorTabs.Location = new Point(15, 15);
            pnlFloorTabs.Name = "pnlFloorTabs";
            pnlFloorTabs.Size = new Size(400, 48);
            pnlFloorTabs.TabIndex = 0;
            // 
            // btnFloorTang1
            // 
            btnFloorTang1.BackColor = Color.FromArgb(99, 102, 241);
            btnFloorTang1.Cursor = Cursors.Hand;
            btnFloorTang1.FlatAppearance.BorderSize = 0;
            btnFloorTang1.FlatStyle = FlatStyle.Flat;
            btnFloorTang1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnFloorTang1.ForeColor = Color.White;
            btnFloorTang1.Location = new Point(3, 3);
            btnFloorTang1.Name = "btnFloorTang1";
            btnFloorTang1.Size = new Size(120, 40);
            btnFloorTang1.TabIndex = 0;
            btnFloorTang1.Tag = "Tầng 1";
            btnFloorTang1.Text = "Tầng 1";
            btnFloorTang1.UseVisualStyleBackColor = false;
            btnFloorTang1.Click += FloorTab_Click;
            // 
            // btnFloorTang2
            // 
            btnFloorTang2.BackColor = Color.FromArgb(226, 232, 240);
            btnFloorTang2.Cursor = Cursors.Hand;
            btnFloorTang2.FlatAppearance.BorderSize = 0;
            btnFloorTang2.FlatStyle = FlatStyle.Flat;
            btnFloorTang2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnFloorTang2.ForeColor = Color.FromArgb(51, 65, 85);
            btnFloorTang2.Location = new Point(129, 3);
            btnFloorTang2.Name = "btnFloorTang2";
            btnFloorTang2.Size = new Size(120, 40);
            btnFloorTang2.TabIndex = 1;
            btnFloorTang2.Tag = "Tầng 2";
            btnFloorTang2.Text = "Tầng 2";
            btnFloorTang2.UseVisualStyleBackColor = false;
            btnFloorTang2.Click += FloorTab_Click;
            // 
            // btnFloorVIP
            // 
            btnFloorVIP.BackColor = Color.FromArgb(226, 232, 240);
            btnFloorVIP.Cursor = Cursors.Hand;
            btnFloorVIP.FlatAppearance.BorderSize = 0;
            btnFloorVIP.FlatStyle = FlatStyle.Flat;
            btnFloorVIP.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnFloorVIP.ForeColor = Color.FromArgb(51, 65, 85);
            btnFloorVIP.Location = new Point(255, 3);
            btnFloorVIP.Name = "btnFloorVIP";
            btnFloorVIP.Size = new Size(120, 40);
            btnFloorVIP.TabIndex = 2;
            btnFloorVIP.Tag = "VIP";
            btnFloorVIP.Text = "VIP";
            btnFloorVIP.UseVisualStyleBackColor = false;
            btnFloorVIP.Click += FloorTab_Click;
            // 
            // pnlEditControls
            // 
            pnlEditControls.Controls.Add(btnEdit);
            pnlEditControls.Controls.Add(btnSave);
            pnlEditControls.Controls.Add(btnCancel);
            pnlEditControls.Dock = DockStyle.Right;
            pnlEditControls.Location = new Point(785, 15);
            pnlEditControls.Name = "pnlEditControls";
            pnlEditControls.Size = new Size(398, 48);
            pnlEditControls.TabIndex = 1;
            // 
            // btnEdit
            // 
            btnEdit.BackColor = Color.FromArgb(59, 130, 246);
            btnEdit.Cursor = Cursors.Hand;
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnEdit.ForeColor = Color.White;
            btnEdit.Location = new Point(3, 3);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(170, 40);
            btnEdit.TabIndex = 0;
            btnEdit.Text = "✏️ Chỉnh sửa";
            btnEdit.UseVisualStyleBackColor = false;
            btnEdit.Click += BtnEdit_Click;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(34, 197, 94);
            btnSave.Cursor = Cursors.Hand;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(3, 3);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(170, 40);
            btnSave.TabIndex = 1;
            btnSave.Text = "💾 Lưu thay đổi";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Visible = false;
            btnSave.Click += BtnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(107, 114, 128);
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(179, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(120, 40);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "❌ Hủy";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Visible = false;
            btnCancel.Click += BtnCancel_Click;
            // 
            // pnlBody
            // 
            pnlBody.BackColor = Color.FromArgb(248, 250, 252);
            pnlBody.Controls.Add(pnlCanvas);
            pnlBody.Controls.Add(pnlLegend);
            pnlBody.Controls.Add(pnlInstructions);
            pnlBody.Dock = DockStyle.Fill;
            pnlBody.Location = new Point(0, 150);
            pnlBody.Name = "pnlBody";
            pnlBody.Padding = new Padding(20);
            pnlBody.Size = new Size(1200, 550);
            pnlBody.TabIndex = 2;
            // 
            // pnlCanvas
            // 
            pnlCanvas.AutoScroll = true;
            pnlCanvas.BackColor = Color.White;
            pnlCanvas.BorderStyle = BorderStyle.FixedSingle;
            pnlCanvas.Dock = DockStyle.Fill;
            pnlCanvas.Location = new Point(20, 20);
            pnlCanvas.Name = "pnlCanvas";
            pnlCanvas.Size = new Size(860, 510);
            pnlCanvas.TabIndex = 0;
            // 
            // pnlLegend
            // 
            pnlLegend.BackColor = Color.White;
            pnlLegend.BorderStyle = BorderStyle.FixedSingle;
            pnlLegend.Controls.Add(lblLegendTitle);
            pnlLegend.Controls.Add(pnlLegendItems);
            pnlLegend.Dock = DockStyle.Right;
            pnlLegend.Location = new Point(880, 20);
            pnlLegend.Name = "pnlLegend";
            pnlLegend.Padding = new Padding(15);
            pnlLegend.Size = new Size(300, 510);
            pnlLegend.TabIndex = 1;
            // 
            // lblLegendTitle
            // 
            lblLegendTitle.AutoSize = true;
            lblLegendTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblLegendTitle.Location = new Point(15, 15);
            lblLegendTitle.Name = "lblLegendTitle";
            lblLegendTitle.Size = new Size(107, 32);
            lblLegendTitle.TabIndex = 0;
            lblLegendTitle.Text = "Chú giải";
            // 
            // pnlLegendItems
            // 
            pnlLegendItems.Controls.Add(pnlLegendTrong);
            pnlLegendItems.Controls.Add(pnlLegendDangChoi);
            pnlLegendItems.Controls.Add(pnlLegendDaDat);
            pnlLegendItems.FlowDirection = FlowDirection.TopDown;
            pnlLegendItems.Location = new Point(15, 60);
            pnlLegendItems.Name = "pnlLegendItems";
            pnlLegendItems.Size = new Size(270, 200);
            pnlLegendItems.TabIndex = 1;
            // 
            // pnlLegendTrong
            // 
            pnlLegendTrong.Controls.Add(lblLegendTrong);
            pnlLegendTrong.Location = new Point(3, 3);
            pnlLegendTrong.Name = "pnlLegendTrong";
            pnlLegendTrong.Size = new Size(260, 40);
            pnlLegendTrong.TabIndex = 0;
            // 
            // lblLegendTrong
            // 
            lblLegendTrong.AutoSize = true;
            lblLegendTrong.Font = new Font("Segoe UI", 10F);
            lblLegendTrong.Location = new Point(40, 8);
            lblLegendTrong.Name = "lblLegendTrong";
            lblLegendTrong.Size = new Size(62, 28);
            lblLegendTrong.TabIndex = 0;
            lblLegendTrong.Text = "Trống";
            // 
            // pnlLegendDangChoi
            // 
            pnlLegendDangChoi.Controls.Add(lblLegendDangChoi);
            pnlLegendDangChoi.Location = new Point(3, 49);
            pnlLegendDangChoi.Name = "pnlLegendDangChoi";
            pnlLegendDangChoi.Size = new Size(260, 40);
            pnlLegendDangChoi.TabIndex = 1;
            // 
            // lblLegendDangChoi
            // 
            lblLegendDangChoi.AutoSize = true;
            lblLegendDangChoi.Font = new Font("Segoe UI", 10F);
            lblLegendDangChoi.Location = new Point(40, 8);
            lblLegendDangChoi.Name = "lblLegendDangChoi";
            lblLegendDangChoi.Size = new Size(101, 28);
            lblLegendDangChoi.TabIndex = 0;
            lblLegendDangChoi.Text = "Đang chơi";
            // 
            // pnlLegendDaDat
            // 
            pnlLegendDaDat.Controls.Add(lblLegendDaDat);
            pnlLegendDaDat.Location = new Point(3, 95);
            pnlLegendDaDat.Name = "pnlLegendDaDat";
            pnlLegendDaDat.Size = new Size(260, 40);
            pnlLegendDaDat.TabIndex = 2;
            // 
            // lblLegendDaDat
            // 
            lblLegendDaDat.AutoSize = true;
            lblLegendDaDat.Font = new Font("Segoe UI", 10F);
            lblLegendDaDat.Location = new Point(40, 8);
            lblLegendDaDat.Name = "lblLegendDaDat";
            lblLegendDaDat.Size = new Size(70, 28);
            lblLegendDaDat.TabIndex = 0;
            lblLegendDaDat.Text = "Đã đặt";
            // 
            // pnlInstructions
            // 
            pnlInstructions.BackColor = Color.FromArgb(254, 249, 195);
            pnlInstructions.BorderStyle = BorderStyle.FixedSingle;
            pnlInstructions.Controls.Add(lblInstructionsTitle);
            pnlInstructions.Controls.Add(lblInstructions);
            pnlInstructions.Location = new Point(900, 280);
            pnlInstructions.Name = "pnlInstructions";
            pnlInstructions.Padding = new Padding(15);
            pnlInstructions.Size = new Size(260, 230);
            pnlInstructions.TabIndex = 2;
            pnlInstructions.Visible = false;
            // 
            // lblInstructionsTitle
            // 
            lblInstructionsTitle.AutoSize = true;
            lblInstructionsTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblInstructionsTitle.Location = new Point(15, 15);
            lblInstructionsTitle.Name = "lblInstructionsTitle";
            lblInstructionsTitle.Size = new Size(174, 30);
            lblInstructionsTitle.TabIndex = 0;
            lblInstructionsTitle.Text = "💡 Hướng dẫn:";
            // 
            // lblInstructions
            // 
            lblInstructions.Font = new Font("Segoe UI", 9F);
            lblInstructions.Location = new Point(15, 55);
            lblInstructions.Name = "lblInstructions";
            lblInstructions.Size = new Size(228, 160);
            lblInstructions.TabIndex = 1;
            lblInstructions.Text = "• Kéo thả bàn để di chuyển vị trí\r\n\r\n• Click vào bàn để xem chi tiết\r\n\r\n• Nhấn \"Lưu thay đổi\" để cập nhật";
            // 
            // SoDoBanForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 700);
            Controls.Add(pnlBody);
            Controls.Add(pnlControls);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.None;
            Name = "SoDoBanForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sơ đồ bàn";
            Load += SoDoBanForm_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlControls.ResumeLayout(false);
            pnlFloorTabs.ResumeLayout(false);
            pnlEditControls.ResumeLayout(false);
            pnlBody.ResumeLayout(false);
            pnlLegend.ResumeLayout(false);
            pnlLegend.PerformLayout();
            pnlLegendItems.ResumeLayout(false);
            pnlLegendTrong.ResumeLayout(false);
            pnlLegendTrong.PerformLayout();
            pnlLegendDangChoi.ResumeLayout(false);
            pnlLegendDangChoi.PerformLayout();
            pnlLegendDaDat.ResumeLayout(false);
            pnlLegendDaDat.PerformLayout();
            pnlInstructions.ResumeLayout(false);
            pnlInstructions.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblTitle;
        private Button btnClose;
        private Panel pnlControls;
        private FlowLayoutPanel pnlFloorTabs;
        private Button btnFloorTang1;
        private Button btnFloorTang2;
        private Button btnFloorVIP;
        private Panel pnlEditControls;
        private Button btnEdit;
        private Button btnSave;
        private Button btnCancel;
        private Panel pnlBody;
        private Panel pnlCanvas;
        private Panel pnlLegend;
        private Label lblLegendTitle;
        private FlowLayoutPanel pnlLegendItems;
        private Panel pnlLegendTrong;
        private Label lblLegendTrong;
        private Panel pnlLegendDangChoi;
        private Label lblLegendDangChoi;
        private Panel pnlLegendDaDat;
        private Label lblLegendDaDat;
        private Panel pnlInstructions;
        private Label lblInstructionsTitle;
        private Label lblInstructions;
    }
}