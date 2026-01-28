using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Billiard.DAL.Data;

namespace Billiard.WinForm.Forms
{
    public partial class ucKiemSoatKho : UserControl
    {
        private readonly BilliardDbContext _context;

        public ucKiemSoatKho()
        {
            InitializeComponent();
            _context = new BilliardDbContext();
        }

        private void ucKiemSoatKho_Load(object sender, EventArgs e)
        {
            // Khởi tạo combobox loại hàng
            cboLoaiHang.Items.Clear();
            cboLoaiHang.Items.AddRange(new string[] { "Tất cả", "Đồ ăn", "Đồ uống", "Khác" });
            cboLoaiHang.SelectedIndex = 0; // Tất cả

            // Khởi tạo combobox trạng thái
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.AddRange(new string[] { "Tất cả", "Còn hàng", "Sắp hết", "Hết hàng" });
            cboTrangThai.SelectedIndex = 0; // Tất cả

            LoadMatHang();
            LoadThongKe();
        }

        private void LoadThongKe()
        {
            try
            {
                var matHangs = _context.MatHangs.ToList();

                lblTongMHValue.Text = matHangs.Count.ToString();
                lblConHangValue.Text = matHangs.Count(mh => mh.TrangThai == "Còn hàng").ToString();

                // Sắp hết: số lượng tồn <= ngưỡng cảnh báo và > 0
                lblSapHetValue.Text = matHangs.Count(mh =>
                    mh.SoLuongTon <= mh.NguongCanhBao && mh.SoLuongTon > 0).ToString();

                // Hết hàng: số lượng tồn = 0
                lblHetHangValue.Text = matHangs.Count(mh => mh.SoLuongTon == 0).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thống kê: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMatHang(string timKiem = null, string loai = null, string trangThai = null)
        {
            try
            {
                var query = _context.MatHangs.AsQueryable();

                // Tìm kiếm theo tên
                if (!string.IsNullOrWhiteSpace(timKiem))
                    query = query.Where(mh => mh.TenHang.Contains(timKiem));

                // Lọc theo loại - FIX: Kiểm tra chính xác
                if (!string.IsNullOrWhiteSpace(loai) && loai != "Tất cả")
                {
                    query = query.Where(mh => mh.Loai == loai);
                }

                // Lọc theo trạng thái
                if (!string.IsNullOrWhiteSpace(trangThai) && trangThai != "Tất cả")
                {
                    if (trangThai == "Sắp hết")
                        query = query.Where(mh => mh.SoLuongTon <= mh.NguongCanhBao && mh.SoLuongTon > 0);
                    else if (trangThai == "Hết hàng")
                        query = query.Where(mh => mh.SoLuongTon == 0);
                    else if (trangThai == "Còn hàng")
                        query = query.Where(mh => mh.SoLuongTon > mh.NguongCanhBao);
                }

                var matHangs = query
                    .OrderBy(mh => mh.TenHang)
                    .Select(mh => new
                    {
                        mh.MaHang,
                        mh.TenHang,
                        mh.Loai,
                        mh.DonVi,
                        mh.SoLuongTon,
                        mh.NguongCanhBao,
                        mh.Gia,
                        mh.TrangThai,
                        mh.NgayNhapGanNhat
                    })
                    .ToList();

                dgvMatHang.Rows.Clear();
                foreach (var item in matHangs)
                {
                    int rowIndex = dgvMatHang.Rows.Add(
                        item.MaHang,
                        item.TenHang,
                        item.Loai,
                        item.DonVi,
                        item.SoLuongTon,
                        item.NguongCanhBao,
                        item.Gia.ToString("#,##0") + " đ",
                        item.TrangThai,
                        item.NgayNhapGanNhat.HasValue ? item.NgayNhapGanNhat.Value.ToString("dd/MM/yyyy") : ""
                    );

                    // Highlight màu theo trạng thái
                    if (item.SoLuongTon == 0)
                    {
                        dgvMatHang.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 230, 230);
                        dgvMatHang.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.FromArgb(192, 57, 43);
                    }
                    else if (item.SoLuongTon <= item.NguongCanhBao)
                    {
                        dgvMatHang.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);
                        dgvMatHang.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.FromArgb(211, 84, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách mặt hàng: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string timKiem = txtTimKiem.Text.Trim();
            string loai = cboLoaiHang.SelectedItem?.ToString();
            string trangThai = cboTrangThai.SelectedItem?.ToString();

            LoadMatHang(timKiem, loai, trangThai);
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            cboLoaiHang.SelectedIndex = 0;
            cboTrangThai.SelectedIndex = 0;
            LoadMatHang();
            LoadThongKe();
        }
    }
}