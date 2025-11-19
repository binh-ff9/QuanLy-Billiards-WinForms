using Billiard.DAL.Entities;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.HoaDon
{
    public partial class ChiTietHoaDonControl : UserControl
    {
        public ChiTietHoaDonControl()
        {
            InitializeComponent();

            // Gọi hàm cài đặt giao diện ngay khi khởi tạo
            SetupBeautifulInterface();
        }

        // Hàm cấu hình giao diện tổng thể
        private void SetupBeautifulInterface()
        {
            this.BackColor = Color.White;
            this.Padding = new Padding(20); // Thêm khoảng cách lề cho thoáng

            // Style cho các Label (Nếu bạn đã kéo thả trong Designer)
            StyleLabel(lblMaHD, 16, FontStyle.Bold, Color.FromArgb(30, 41, 59)); // Màu xanh đen đậm
            StyleLabel(lblBan, 11, FontStyle.Regular, Color.FromArgb(71, 85, 105));
            StyleLabel(lblGioVao, 10, FontStyle.Regular, Color.FromArgb(100, 116, 139)); // Màu xám
            StyleLabel(lblGioRa, 10, FontStyle.Regular, Color.FromArgb(100, 116, 139));

            // Style đặc biệt cho Tổng tiền
            StyleLabel(lblTongTien, 18, FontStyle.Bold, Color.FromArgb(220, 38, 38)); // Màu đỏ nổi bật

            ConfigureGrid();
        }

        private void StyleLabel(Label lbl, float size, FontStyle style, Color color)
        {
            if (lbl != null)
            {
                lbl.Font = new Font("Segoe UI", size, style);
                lbl.ForeColor = color;
            }
        }

        private void ConfigureGrid()
        {
            // 1. Cài đặt chung
            dgvChiTiet.BackgroundColor = Color.White;
            dgvChiTiet.BorderStyle = BorderStyle.None;
            dgvChiTiet.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // Chỉ hiện đường kẻ ngang
            dgvChiTiet.GridColor = Color.FromArgb(241, 245, 249); // Màu đường kẻ rất nhạt

            dgvChiTiet.ReadOnly = true;
            dgvChiTiet.RowHeadersVisible = false;
            dgvChiTiet.AllowUserToAddRows = false;
            dgvChiTiet.AllowUserToResizeRows = false;
            dgvChiTiet.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // 2. Header (Tiêu đề cột)
            dgvChiTiet.EnableHeadersVisualStyles = false; // Bắt buộc để chỉnh màu Header
            dgvChiTiet.ColumnHeadersHeight = 45; // Header cao hơn cho thoáng
            dgvChiTiet.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            var headerStyle = dgvChiTiet.ColumnHeadersDefaultCellStyle;
            headerStyle.BackColor = Color.FromArgb(248, 250, 252); // Nền xám nhạt
            headerStyle.ForeColor = Color.FromArgb(100, 116, 139); // Chữ xám đậm
            headerStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            headerStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // 3. Rows (Dòng dữ liệu)
            dgvChiTiet.RowTemplate.Height = 40; // Dòng cao hơn

            var rowStyle = dgvChiTiet.DefaultCellStyle;
            rowStyle.BackColor = Color.White;
            rowStyle.ForeColor = Color.FromArgb(51, 65, 85);
            rowStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            rowStyle.SelectionBackColor = Color.FromArgb(224, 231, 255); // Màu tím nhạt khi chọn
            rowStyle.SelectionForeColor = Color.FromArgb(30, 58, 138);   // Chữ xanh đậm khi chọn
            rowStyle.Padding = new Padding(5, 0, 5, 0); // Đệm nội dung bên trong ô
        }

        public void LoadData(Billiard.DAL.Entities.HoaDon hd)
        {
            // 1. Hiển thị thông tin chung
            lblMaHD.Text = $"HÓA ĐƠN #{hd.MaHd}";
            lblBan.Text = $"Bàn: {hd.MaBanNavigation?.TenBan ?? "Mang về"}";

            // Thêm icon hoặc ký tự đặc biệt cho đẹp
            lblGioVao.Text = $"🕒 Vào: {hd.ThoiGianBatDau?.ToString("HH:mm dd/MM/yyyy")}";
            // 2. Hiển thị danh sách món
            var listMon = hd.ChiTietHoaDons.Select(ct => new
            {
                TenDichVu = ct.MaDvNavigation?.TenDv ?? "Dịch vụ",
                SoLuong = ct.SoLuong,
                DonGia = ct.ThanhTien,
                ThanhTien = ct.ThanhTien
            }).ToList();

            dgvChiTiet.DataSource = listMon;

            // 3. Format cột sau khi gán dữ liệu (QUAN TRỌNG)
            FormatGridColumns();

            // 4. Tổng tiền
            lblTongTien.Text = $"{hd.TongTien:N0} đ";

        }
        private void FormatGridColumns()
        {
            if (dgvChiTiet.Columns["TenDichVu"] != null)
            {
                dgvChiTiet.Columns["TenDichVu"].HeaderText = "Tên Dịch Vụ";
                dgvChiTiet.Columns["TenDichVu"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }

            if (dgvChiTiet.Columns["SoLuong"] != null)
            {
                dgvChiTiet.Columns["SoLuong"].HeaderText = "SL";
                dgvChiTiet.Columns["SoLuong"].FillWeight = 30; // Cột nhỏ thôi
                dgvChiTiet.Columns["SoLuong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvChiTiet.Columns["DonGia"] != null)
            {
                dgvChiTiet.Columns["DonGia"].HeaderText = "Đơn Giá";
                dgvChiTiet.Columns["DonGia"].DefaultCellStyle.Format = "N0"; // 100,000
                dgvChiTiet.Columns["DonGia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (dgvChiTiet.Columns["ThanhTien"] != null)
            {
                dgvChiTiet.Columns["ThanhTien"].HeaderText = "Thành Tiền";
                dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
                dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold); // In đậm cột tiền
            }
        }
    }
}
