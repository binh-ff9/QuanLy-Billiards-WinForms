namespace Billiard.WinForm.Forms.Users
{
    partial class TableCardControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            picTable = new PictureBox();
            lblTenBan = new Label();
            lblLoaiBan = new Label();
            lblTrangThai = new Label();
            lblGia = new Label();
            btnDatBan = new Button();
            pnlImage = new Panel();
            ((System.ComponentModel.ISupportInitialize)picTable).BeginInit();
            pnlImage.SuspendLayout();
            SuspendLayout();
            // 
            // picTable
            // 
            picTable.Dock = DockStyle.Fill;
            picTable.Location = new Point(0, 0);
            picTable.Name = "picTable";
            picTable.Size = new Size(220, 140);
            picTable.SizeMode = PictureBoxSizeMode.Zoom;
            picTable.TabIndex = 0;
            picTable.TabStop = false;
            // 
            // lblTenBan
            // 
            lblTenBan.AutoSize = true;
            lblTenBan.Location = new Point(44, 152);
            lblTenBan.Name = "lblTenBan";
            lblTenBan.Size = new Size(30, 20);
            lblTenBan.TabIndex = 1;
            lblTenBan.Text = "tên";
            // 
            // lblLoaiBan
            // 
            lblLoaiBan.AutoSize = true;
            lblLoaiBan.Location = new Point(44, 215);
            lblLoaiBan.Name = "lblLoaiBan";
            lblLoaiBan.Size = new Size(34, 20);
            lblLoaiBan.TabIndex = 2;
            lblLoaiBan.Text = "loại";
            // 
            // lblTrangThai
            // 
            lblTrangThai.AutoSize = true;
            lblTrangThai.Location = new Point(44, 181);
            lblTrangThai.Name = "lblTrangThai";
            lblTrangThai.Size = new Size(47, 20);
            lblTrangThai.TabIndex = 4;
            lblTrangThai.Text = "status";
            // 
            // lblGia
            // 
            lblGia.AutoSize = true;
            lblGia.Location = new Point(44, 251);
            lblGia.Name = "lblGia";
            lblGia.Size = new Size(30, 20);
            lblGia.TabIndex = 5;
            lblGia.Text = "giá";
            // 
            // btnDatBan
            // 
            btnDatBan.Location = new Point(32, 293);
            btnDatBan.Name = "btnDatBan";
            btnDatBan.Size = new Size(116, 37);
            btnDatBan.TabIndex = 6;
            btnDatBan.Text = "Đặt bàn";
            btnDatBan.UseVisualStyleBackColor = true;
            btnDatBan.Click += btnDatBan_Click;
            // 
            // pnlImage
            // 
            pnlImage.Controls.Add(picTable);
            pnlImage.Location = new Point(0, 0);
            pnlImage.Name = "pnlImage";
            pnlImage.Size = new Size(220, 140);
            pnlImage.TabIndex = 7;
            // 
            // TableCardControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlImage);
            Controls.Add(btnDatBan);
            Controls.Add(lblGia);
            Controls.Add(lblTrangThai);
            Controls.Add(lblLoaiBan);
            Controls.Add(lblTenBan);
            Name = "TableCardControl";
            Size = new Size(223, 384);
            ((System.ComponentModel.ISupportInitialize)picTable).EndInit();
            pnlImage.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picTable;
        private Label lblTenBan;
        private Label lblLoaiBan;
        private Label lblTrangThai;
        private Label lblGia;
        private Button btnDatBan;
        private Panel pnlImage;
    }
}
