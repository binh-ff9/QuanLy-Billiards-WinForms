using System;
using System.Linq;
using System.Windows.Forms;
using Billiard.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace Billiard.WinForm.Forms
{
    public partial class frmTaoPhieuXuat : Form
    {
        private readonly BilliardDbContext _context;
        private int _maNv; // Mã nhân viên đang đăng nhập
        private decimal _tongTien = 0;

        public frmTaoPhieuXuat(int maNv)
        {
            InitializeComponent();
            _context = new BilliardDbContext();
            _maNv = maNv;
        }

        private void frmTaoPhieuXuat_Load(object sender, EventArgs e)
        {
            LoadMatHang();
            LoadNhanVien();
            dtpNgayXuat.Value = DateTime.Now;
            UpdateTongTien();
        }

        private void LoadNhanVien()
        {
            try
            {
                var nhanVien = _context.NhanViens
                    .AsNoTracking()
                    .FirstOrDefault(nv => nv.MaNv == _maNv);

                if (nhanVien != null)
                {
                    txtNhanVien.Text = nhanVien.TenNv;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thông tin nhân viên: " + ex.Message +
                                "\n\nChi tiết: " + (ex.InnerException?.Message ?? "Không có"),
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMatHang()
        {
            try
            {
                var matHangs = _context.MatHangs
                    .AsNoTracking()
                    .Where(mh => mh.SoLuongTon > 0 && mh.TrangThai != "Ngừng kinh doanh")
                    .Select(mh => new { mh.MaHang, mh.TenHang, mh.DonVi, mh.SoLuongTon })
                    .ToList();

                cboMatHang.DataSource = matHangs;
                cboMatHang.DisplayMember = "TenHang";
                cboMatHang.ValueMember = "MaHang";
                cboMatHang.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách mặt hàng: " + ex.Message +
                                "\n\nChi tiết: " + (ex.InnerException?.Message ?? "Không có"),
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboMatHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboMatHang.SelectedValue != null && int.TryParse(cboMatHang.SelectedValue.ToString(), out int maHang))
                {
                    var matHang = _context.MatHangs
                        .AsNoTracking()
                        .FirstOrDefault(mh => mh.MaHang == maHang);

                    if (matHang != null)
                    {
                        lblDonVi.Text = $"Đơn vị: {matHang.DonVi}";
                        lblTonKho.Text = $"Tồn kho: {matHang.SoLuongTon}";
                    }
                }
            }
            catch
            {
                // Silent fail để tránh lỗi khi đang binding data
                lblDonVi.Text = "Đơn vị:";
                lblTonKho.Text = "Tồn kho:";
            }
        }

        private void btnThemMatHang_Click(object sender, EventArgs e)
        {
            if (cboMatHang.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn mặt hàng!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSoLuong.Text) || !int.TryParse(txtSoLuong.Text, out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Vui lòng nhập số lượng hợp lệ!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maHang = (int)cboMatHang.SelectedValue;
            var matHang = _context.MatHangs
                .AsNoTracking()
                .FirstOrDefault(mh => mh.MaHang == maHang);

            if (matHang == null)
            {
                MessageBox.Show("Không tìm thấy mặt hàng!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kiểm tra số lượng tồn kho
            if (soLuong > matHang.SoLuongTon)
            {
                MessageBox.Show($"Số lượng xuất vượt quá tồn kho! Tồn kho hiện tại: {matHang.SoLuongTon}", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tenHang = matHang.TenHang;
            decimal donGia = matHang.Gia;
            decimal thanhTien = soLuong * donGia;

            // Kiểm tra xem mặt hàng đã có trong danh sách chưa
            foreach (DataGridViewRow row in dgvChiTiet.Rows)
            {
                if (row.Cells["colMaHang"].Value != null && (int)row.Cells["colMaHang"].Value == maHang)
                {
                    MessageBox.Show("Mặt hàng này đã có trong danh sách!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // Thêm vào DataGridView
            int rowIndex = dgvChiTiet.Rows.Add(
                maHang,
                tenHang,
                soLuong,
                string.Format("{0:N0} đ", donGia),
                string.Format("{0:N0} đ", thanhTien)
            );

            // Lưu giá trị thực để tính toán
            dgvChiTiet.Rows[rowIndex].Tag = new { MaHang = maHang, SoLuong = soLuong, DonGia = donGia, ThanhTien = thanhTien };

            // Reset controls
            cboMatHang.SelectedIndex = -1;
            txtSoLuong.Clear();
            lblDonVi.Text = "Đơn vị:";
            lblTonKho.Text = "Tồn kho:";

            UpdateTongTien();
        }

        private void btnXoaMatHang_Click(object sender, EventArgs e)
        {
            if (dgvChiTiet.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn mặt hàng cần xóa!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa mặt hàng này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dgvChiTiet.Rows.RemoveAt(dgvChiTiet.CurrentRow.Index);
                UpdateTongTien();
            }
        }

        private void UpdateTongTien()
        {
            _tongTien = 0;
            foreach (DataGridViewRow row in dgvChiTiet.Rows)
            {
                if (row.Tag != null)
                {
                    var tagType = row.Tag.GetType();
                    var thanhTienProp = tagType.GetProperty("ThanhTien");
                    if (thanhTienProp != null)
                    {
                        _tongTien += (decimal)thanhTienProp.GetValue(row.Tag);
                    }
                }
            }
            lblTongTienValue.Text = string.Format("{0:N0} đ", _tongTien);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Validate
            if (dgvChiTiet.Rows.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm ít nhất một mặt hàng!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    // Cập nhật kho (xuất hàng)
                    // Lưu ý: Không tạo phiếu xuất trong database, chỉ cập nhật số lượng tồn kho
                    foreach (DataGridViewRow row in dgvChiTiet.Rows)
                    {
                        if (row.Tag != null)
                        {
                            var tagType = row.Tag.GetType();
                            var maHangProp = tagType.GetProperty("MaHang");
                            var soLuongProp = tagType.GetProperty("SoLuong");

                            int maHang = (int)maHangProp.GetValue(row.Tag);
                            int soLuong = (int)soLuongProp.GetValue(row.Tag);

                            // Cập nhật số lượng tồn kho
                            var matHang = _context.MatHangs.FirstOrDefault(mh => mh.MaHang == maHang);
                            if (matHang != null)
                            {
                                matHang.SoLuongTon -= soLuong;

                                // Cập nhật trạng thái
                                if (matHang.SoLuongTon == 0)
                                    matHang.TrangThai = "Hết hàng";
                                else if (matHang.SoLuongTon <= matHang.NguongCanhBao)
                                    matHang.TrangThai = "Sắp hết";
                                else
                                    matHang.TrangThai = "Còn hàng";
                            }
                        }
                    }

                    _context.SaveChanges();
                    transaction.Commit();

                    MessageBox.Show("Xuất hàng thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất hàng: " + ex.Message +
                                "\n\nChi tiết: " + (ex.InnerException?.Message ?? "Không có"),
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            if (dgvChiTiet.Rows.Count > 0)
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn hủy? Dữ liệu chưa lưu sẽ bị mất!", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void txtSoLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép nhập số
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}