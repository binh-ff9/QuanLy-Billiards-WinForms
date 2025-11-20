namespace Billiard.WinForm.Forms.Users
{
    partial class DatBanDialog
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
            lblTitle = new Label();
            dtpNgay = new DateTimePicker();
            dateTimePicker2 = new DateTimePicker();
            pnlTimeSlots = new Panel();
            txtGhiChu = new TextBox();
            pnlTimeSlots.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(345, 71);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(50, 20);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "label1";
            // 
            // dtpNgay
            // 
            dtpNgay.Location = new Point(33, 38);
            dtpNgay.Name = "dtpNgay";
            dtpNgay.Size = new Size(230, 27);
            dtpNgay.TabIndex = 1;
            // 
            // dateTimePicker2
            // 
            dateTimePicker2.Location = new Point(33, 71);
            dateTimePicker2.Name = "dateTimePicker2";
            dateTimePicker2.Size = new Size(230, 27);
            dateTimePicker2.TabIndex = 2;
            // 
            // pnlTimeSlots
            // 
            pnlTimeSlots.Controls.Add(txtGhiChu);
            pnlTimeSlots.Controls.Add(dtpNgay);
            pnlTimeSlots.Controls.Add(lblTitle);
            pnlTimeSlots.Controls.Add(dateTimePicker2);
            pnlTimeSlots.Location = new Point(12, 12);
            pnlTimeSlots.Name = "pnlTimeSlots";
            pnlTimeSlots.Size = new Size(594, 124);
            pnlTimeSlots.TabIndex = 3;
            // 
            // txtGhiChu
            // 
            txtGhiChu.Location = new Point(434, 36);
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.Size = new Size(125, 27);
            txtGhiChu.TabIndex = 3;
            // 
            // DatBanDialog
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(pnlTimeSlots);
            Name = "DatBanDialog";
            Text = "DatBanDialog";
            pnlTimeSlots.ResumeLayout(false);
            pnlTimeSlots.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label lblTitle;
        private DateTimePicker dtpNgay;
        private DateTimePicker dateTimePicker2;
        private Panel pnlTimeSlots;
        private TextBox txtGhiChu;
    }
}