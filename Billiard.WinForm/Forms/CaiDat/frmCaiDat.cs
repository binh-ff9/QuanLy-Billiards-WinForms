using Billiard.DAL.Entities;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.CaiDat
{
    public partial class CaiDatForm : Form
    {
        private Button currentButton;
        private MainForm _mainForm;

        public CaiDatForm()
        {
            InitializeComponent();
        }

        public void SetMainForm(MainForm mainForm)
        {
            _mainForm = mainForm;
        }

        private void CaiDatForm_Load(object sender, EventArgs e)
        {
            // Load mặc định form Lịch Sử Hoạt Động
            LoadUserControl(new ucLichSuHoatDong());
            HighlightButton(btnLichSuHoatDong);
        }

        private void btnLichSuHoatDong_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ucLichSuHoatDong());
            HighlightButton(btnLichSuHoatDong);
        }

        private void btnPhieuNhapXuat_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ucPhieuNhapXuat());
            HighlightButton(btnPhieuNhapXuat);
        }

        private void btnKiemSoatKho_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ucKiemSoatKho());
            HighlightButton(btnKiemSoatKho);
        }

        private void btnVietQR_Click(object sender, EventArgs e)
        {
            try
            {
                var vietQRConfigForm = Program.GetService<VietQRConfigForm>();
                vietQRConfigForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở cấu hình VietQR: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUserControl(UserControl uc)
        {
            panelContent.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            panelContent.Controls.Add(uc);
        }

        private void HighlightButton(Button btn)
        {
            // Reset màu tất cả buttons
            foreach (Control ctrl in panelMenu.Controls)
            {
                if (ctrl is Button button && button != btnVietQR)
                {
                    button.BackColor = Color.FromArgb(52, 73, 94);
                }
            }

            // Highlight button được chọn (trừ VietQR vì nó mở dialog)
            if (btn != btnVietQR)
            {
                currentButton = btn;
                currentButton.BackColor = Color.FromArgb(41, 128, 185);
            }
        }
    }
}