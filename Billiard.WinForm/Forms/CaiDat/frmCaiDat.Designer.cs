namespace Billiard.WinForm.Forms.CaiDat
{
    partial class CaiDatForm
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
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelMenu = new System.Windows.Forms.Panel();
            this.btnLichSuHoatDong = new System.Windows.Forms.Button();
            this.btnPhieuNhapXuat = new System.Windows.Forms.Button();
            this.btnKiemSoatKho = new System.Windows.Forms.Button();
            this.btnVietQR = new System.Windows.Forms.Button();
            this.panelContent = new System.Windows.Forms.Panel();
            this.panelHeader.SuspendLayout();
            this.panelMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new Size(1200, 70);
            this.panelHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(119, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "⚙️ CÀI ĐẶT";
            // 
            // panelMenu
            // 
            this.panelMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.panelMenu.Controls.Add(this.btnLichSuHoatDong);
            this.panelMenu.Controls.Add(this.btnPhieuNhapXuat);
            this.panelMenu.Controls.Add(this.btnKiemSoatKho);
            this.panelMenu.Controls.Add(this.btnVietQR);
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelMenu.Location = new System.Drawing.Point(0, 70);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new Size(1200, 60);
            this.panelMenu.TabIndex = 1;
            // 
            // btnLichSuHoatDong
            // 
            this.btnLichSuHoatDong.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.btnLichSuHoatDong.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLichSuHoatDong.FlatAppearance.BorderSize = 0;
            this.btnLichSuHoatDong.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnLichSuHoatDong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLichSuHoatDong.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnLichSuHoatDong.ForeColor = System.Drawing.Color.White;
            this.btnLichSuHoatDong.Location = new System.Drawing.Point(10, 10);
            this.btnLichSuHoatDong.Name = "btnLichSuHoatDong";
            this.btnLichSuHoatDong.Size = new Size(200, 40);
            this.btnLichSuHoatDong.TabIndex = 0;
            this.btnLichSuHoatDong.Text = "📜 Lịch Sử Hoạt Động";
            this.btnLichSuHoatDong.UseVisualStyleBackColor = false;
            this.btnLichSuHoatDong.Click += new System.EventHandler(this.btnLichSuHoatDong_Click);
            // 
            // btnPhieuNhapXuat
            // 
            this.btnPhieuNhapXuat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnPhieuNhapXuat.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPhieuNhapXuat.FlatAppearance.BorderSize = 0;
            this.btnPhieuNhapXuat.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnPhieuNhapXuat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPhieuNhapXuat.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnPhieuNhapXuat.ForeColor = System.Drawing.Color.White;
            this.btnPhieuNhapXuat.Location = new System.Drawing.Point(220, 10);
            this.btnPhieuNhapXuat.Name = "btnPhieuNhapXuat";
            this.btnPhieuNhapXuat.Size = new Size(200, 40);
            this.btnPhieuNhapXuat.TabIndex = 1;
            this.btnPhieuNhapXuat.Text = "📄 Phiếu Nhập/Xuất";
            this.btnPhieuNhapXuat.UseVisualStyleBackColor = false;
            this.btnPhieuNhapXuat.Click += new System.EventHandler(this.btnPhieuNhapXuat_Click);
            // 
            // btnKiemSoatKho
            // 
            this.btnKiemSoatKho.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnKiemSoatKho.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnKiemSoatKho.FlatAppearance.BorderSize = 0;
            this.btnKiemSoatKho.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnKiemSoatKho.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKiemSoatKho.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnKiemSoatKho.ForeColor = System.Drawing.Color.White;
            this.btnKiemSoatKho.Location = new System.Drawing.Point(430, 10);
            this.btnKiemSoatKho.Name = "btnKiemSoatKho";
            this.btnKiemSoatKho.Size = new Size(200, 40);
            this.btnKiemSoatKho.TabIndex = 2;
            this.btnKiemSoatKho.Text = "📦 Kiểm Soát Kho";
            this.btnKiemSoatKho.UseVisualStyleBackColor = false;
            this.btnKiemSoatKho.Click += new System.EventHandler(this.btnKiemSoatKho_Click);
            // 
            // btnVietQR
            // 
            this.btnVietQR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnVietQR.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVietQR.FlatAppearance.BorderSize = 0;
            this.btnVietQR.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnVietQR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVietQR.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnVietQR.ForeColor = System.Drawing.Color.White;
            this.btnVietQR.Location = new System.Drawing.Point(640, 10);
            this.btnVietQR.Name = "btnVietQR";
            this.btnVietQR.Size = new Size(200, 40);
            this.btnVietQR.TabIndex = 3;
            this.btnVietQR.Text = "💳 Thanh Toán VietQR";
            this.btnVietQR.UseVisualStyleBackColor = false;
            this.btnVietQR.Click += new System.EventHandler(this.btnVietQR_Click);
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 130);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new Size(1200, 570);
            this.panelContent.TabIndex = 2;
            // 
            // CaiDatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new Size(1200, 700);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelMenu);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CaiDatForm";
            this.Text = "Cài Đặt - Quản Lý Quán Billiards";
            this.Load += new System.EventHandler(this.CaiDatForm_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.Button btnLichSuHoatDong;
        private System.Windows.Forms.Button btnPhieuNhapXuat;
        private System.Windows.Forms.Button btnKiemSoatKho;
        private System.Windows.Forms.Button btnVietQR;
        private System.Windows.Forms.Panel panelContent;
    }
}