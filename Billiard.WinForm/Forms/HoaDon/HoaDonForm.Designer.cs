namespace Billiard.WinForm.Forms.HoaDon
{
    partial class HoaDonForm
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
            dataGridViewHoaDon = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dataGridViewHoaDon).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewHoaDon
            // 
            dataGridViewHoaDon.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewHoaDon.Dock = DockStyle.Bottom;
            dataGridViewHoaDon.Location = new Point(0, 32);
            dataGridViewHoaDon.Name = "dataGridViewHoaDon";
            dataGridViewHoaDon.RowHeadersWidth = 51;
            dataGridViewHoaDon.Size = new Size(800, 418);
            dataGridViewHoaDon.TabIndex = 0;
            // 
            // HoaDonForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dataGridViewHoaDon);
            Name = "HoaDonForm";
            Text = "HoaDonForm";
            Load += HoaDonForm_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewHoaDon).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridViewHoaDon;
    }
}