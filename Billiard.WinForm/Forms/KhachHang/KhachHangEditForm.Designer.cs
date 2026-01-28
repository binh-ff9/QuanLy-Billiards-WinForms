namespace Billiard.WinForm.Forms.KhachHang
{
    partial class KhachHangEditForm
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
            txtTenKH = new TextBox();
            txtSDT = new TextBox();
            txtEmail = new TextBox();
            numDiem = new NumericUpDown();
            btnLuu = new Button();
            btnHuy = new Button();
            picAvatar = new PictureBox();
            btnChonAnh = new Button();
            btnXoaAnh = new Button();
            ((System.ComponentModel.ISupportInitialize)numDiem).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picAvatar).BeginInit();
            SuspendLayout();
            // 
            // txtTenKH
            // 
            txtTenKH.Location = new Point(72, 48);
            txtTenKH.Name = "txtTenKH";
            txtTenKH.Size = new Size(238, 27);
            txtTenKH.TabIndex = 0;
            // 
            // txtSDT
            // 
            txtSDT.Location = new Point(72, 108);
            txtSDT.Name = "txtSDT";
            txtSDT.Size = new Size(238, 27);
            txtSDT.TabIndex = 1;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(72, 169);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(238, 27);
            txtEmail.TabIndex = 2;
            // 
            // numDiem
            // 
            numDiem.Location = new Point(72, 235);
            numDiem.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numDiem.Name = "numDiem";
            numDiem.Size = new Size(238, 27);
            numDiem.TabIndex = 4;
            // 
            // btnLuu
            // 
            btnLuu.Location = new Point(72, 286);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new Size(94, 29);
            btnLuu.TabIndex = 5;
            btnLuu.Text = "button1";
            btnLuu.UseVisualStyleBackColor = true;
            btnLuu.Click += btnLuu_Click;
            // 
            // btnHuy
            // 
            btnHuy.Location = new Point(216, 286);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(94, 29);
            btnHuy.TabIndex = 6;
            btnHuy.Text = "button2";
            btnHuy.UseVisualStyleBackColor = true;
            btnHuy.Click += btnHuy_Click;
            // 
            // picAvatar
            // 
            picAvatar.Location = new Point(359, 48);
            picAvatar.Name = "picAvatar";
            picAvatar.Size = new Size(140, 78);
            picAvatar.TabIndex = 7;
            picAvatar.TabStop = false;
            // 
            // btnChonAnh
            // 
            btnChonAnh.Location = new Point(359, 148);
            btnChonAnh.Name = "btnChonAnh";
            btnChonAnh.Size = new Size(94, 29);
            btnChonAnh.TabIndex = 8;
            btnChonAnh.Text = "button1";
            btnChonAnh.UseVisualStyleBackColor = true;
            // 
            // btnXoaAnh
            // 
            btnXoaAnh.Location = new Point(459, 148);
            btnXoaAnh.Name = "btnXoaAnh";
            btnXoaAnh.Size = new Size(94, 29);
            btnXoaAnh.TabIndex = 9;
            btnXoaAnh.Text = "button2";
            btnXoaAnh.UseVisualStyleBackColor = true;
            // 
            // KhachHangEditForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnXoaAnh);
            Controls.Add(btnChonAnh);
            Controls.Add(picAvatar);
            Controls.Add(btnHuy);
            Controls.Add(btnLuu);
            Controls.Add(numDiem);
            Controls.Add(txtEmail);
            Controls.Add(txtSDT);
            Controls.Add(txtTenKH);
            Name = "KhachHangEditForm";
            Text = "KhachHangEditForm";
            ((System.ComponentModel.ISupportInitialize)numDiem).EndInit();
            ((System.ComponentModel.ISupportInitialize)picAvatar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtTenKH;
        private TextBox txtSDT;
        private TextBox txtEmail;
        private NumericUpDown numDiem;
        private Button btnLuu;
        private Button btnHuy;
        private PictureBox picAvatar;
        private Button btnChonAnh;
        private Button btnXoaAnh;
    }
}