namespace Billiard.WinForm.Forms.HoaDon
{
    partial class ChiTietHoaDonControl
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
            lblMaHD = new Label();
            lblGioVao = new Label();
            lblBan = new Label();
            lblGioRa = new Label();
            lblTongTien = new Label();
            dgvChiTiet = new DataGridView();
            btnInHoaDon = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvChiTiet).BeginInit();
            SuspendLayout();
            // 
            // lblMaHD
            // 
            lblMaHD.AutoSize = true;
            lblMaHD.Location = new Point(9, 12);
            lblMaHD.Name = "lblMaHD";
            lblMaHD.Size = new Size(50, 20);
            lblMaHD.TabIndex = 1;
            lblMaHD.Text = "label1";
            // 
            // lblGioVao
            // 
            lblGioVao.AutoSize = true;
            lblGioVao.Location = new Point(9, 103);
            lblGioVao.Name = "lblGioVao";
            lblGioVao.Size = new Size(50, 20);
            lblGioVao.TabIndex = 2;
            lblGioVao.Text = "label1";
            // 
            // lblBan
            // 
            lblBan.AutoSize = true;
            lblBan.Location = new Point(9, 58);
            lblBan.Name = "lblBan";
            lblBan.Size = new Size(50, 20);
            lblBan.TabIndex = 3;
            lblBan.Text = "label1";
            // 
            // lblGioRa
            // 
            lblGioRa.AutoSize = true;
            lblGioRa.Location = new Point(9, 146);
            lblGioRa.Name = "lblGioRa";
            lblGioRa.Size = new Size(50, 20);
            lblGioRa.TabIndex = 4;
            lblGioRa.Text = "label1";
            // 
            // lblTongTien
            // 
            lblTongTien.AutoSize = true;
            lblTongTien.Location = new Point(9, 200);
            lblTongTien.Name = "lblTongTien";
            lblTongTien.Size = new Size(50, 20);
            lblTongTien.TabIndex = 5;
            lblTongTien.Text = "label1";
            // 
            // dgvChiTiet
            // 
            dgvChiTiet.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvChiTiet.Location = new Point(9, 271);
            dgvChiTiet.Name = "dgvChiTiet";
            dgvChiTiet.RowHeadersWidth = 51;
            dgvChiTiet.Size = new Size(294, 212);
            dgvChiTiet.TabIndex = 6;
            // 
            // btnInHoaDon
            // 
            btnInHoaDon.BackColor = Color.FromArgb(59, 130, 246);
            btnInHoaDon.Cursor = Cursors.Hand;
            btnInHoaDon.FlatAppearance.BorderSize = 0;
            btnInHoaDon.FlatStyle = FlatStyle.Flat;
            btnInHoaDon.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnInHoaDon.ForeColor = Color.White;
            btnInHoaDon.Location = new Point(9, 517);
            btnInHoaDon.Margin = new Padding(3, 4, 3, 4);
            btnInHoaDon.Name = "btnInHoaDon";
            btnInHoaDon.Size = new Size(171, 37);
            btnInHoaDon.TabIndex = 7;
            btnInHoaDon.Text = "In hóa đơn";
            btnInHoaDon.UseVisualStyleBackColor = false;
            // 
            // ChiTietHoaDonControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnInHoaDon);
            Controls.Add(dgvChiTiet);
            Controls.Add(lblTongTien);
            Controls.Add(lblGioRa);
            Controls.Add(lblBan);
            Controls.Add(lblGioVao);
            Controls.Add(lblMaHD);
            Name = "ChiTietHoaDonControl";
            Size = new Size(1135, 622);
            ((System.ComponentModel.ISupportInitialize)dgvChiTiet).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblMaHD;
        private Label lblGioVao;
        private Label lblBan;
        private Label lblGioRa;
        private Label lblTongTien;
        private DataGridView dgvChiTiet;
        private Button btnInHoaDon;
    }
}
