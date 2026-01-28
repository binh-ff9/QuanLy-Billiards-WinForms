using Billiard.DAL.Data;
using Billiard.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms
{
    public partial class frmTaoPhieuNhap : Form
    {
        private readonly BilliardDbContext _context;
        private int _maNv; // Mã nhân viên đang đăng nhập
        private decimal _tongTien = 0;

        public frmTaoPhieuNhap(int maNv)
        {
            InitializeComponent();
            _context = new BilliardDbContext();
            _maNv = maNv;
        }

        private void frmTaoPhieuNhap_Load(object sender, EventArgs e)
        {
            LoadNhaCungCap();
            LoadMatHang();
            LoadNhanVien();
            dtpNgayNhap.Value = DateTime.Now;
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

        private void LoadNhaCungCap()
        {
            try
            {
                var nhaCungCaps = _context.NhaCungCaps
                    .AsNoTracking()
                    .Select(ncc => new { ncc.MaNcc, ncc.TenNcc })
                    .ToList();

                cboNhaCungCap.DataSource = nhaCungCaps;
                cboNhaCungCap.DisplayMember = "TenNcc";
                cboNhaCungCap.ValueMember = "MaNcc";
                cboNhaCungCap.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách nhà cung cấp: " + ex.Message +
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
                    .Where(mh => mh.TrangThai != "Ngừng kinh doanh")
                    .Select(mh => new { mh.MaHang, mh.TenHang, mh.DonVi })
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
                        lblDonVi.Text = "Đơn vị: " + matHang.DonVi;
                    }
                }
            }
            catch
            {
                // Silent fail để tránh lỗi khi đang binding data
                lblDonVi.Text = "Đơn vị:";
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

            if (string.IsNullOrWhiteSpace(txtDonGia.Text) || !decimal.TryParse(txtDonGia.Text, out decimal donGia) || donGia <= 0)
            {
                MessageBox.Show("Vui lòng nhập đơn giá hợp lệ!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maHang = (int)cboMatHang.SelectedValue;
            string tenHang = cboMatHang.Text;
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
            txtDonGia.Clear();
            lblDonVi.Text = "Đơn vị:";

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
            if (cboNhaCungCap.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
                    // Tạo phiếu nhập
                    var phieuNhap = new PhieuNhap
                    {
                        MaNv = _maNv,
                        MaNcc = (int)cboNhaCungCap.SelectedValue,
                        NgayNhap = dtpNgayNhap.Value,
                        TongTien = _tongTien,
                        GhiChu = txtGhiChu.Text.Trim()
                    };

                    _context.PhieuNhaps.Add(phieuNhap);
                    _context.SaveChanges();

                    // Thêm chi tiết phiếu nhập và cập nhật kho
                    foreach (DataGridViewRow row in dgvChiTiet.Rows)
                    {
                        if (row.Tag != null)
                        {
                            var tagType = row.Tag.GetType();
                            var maHangProp = tagType.GetProperty("MaHang");
                            var soLuongProp = tagType.GetProperty("SoLuong");
                            var donGiaProp = tagType.GetProperty("DonGia");

                            int maHang = (int)maHangProp.GetValue(row.Tag);
                            int soLuong = (int)soLuongProp.GetValue(row.Tag);
                            decimal donGia = (decimal)donGiaProp.GetValue(row.Tag);

                            // Thêm chi tiết phiếu nhập
                            var chiTiet = new ChiTietPhieuNhap
                            {
                                MaPn = phieuNhap.MaPn,
                                MaHang = maHang,
                                SoLuongNhap = soLuong,
                                DonGiaNhap = donGia
                            };
                            _context.ChiTietPhieuNhaps.Add(chiTiet);

                            // Cập nhật số lượng tồn kho
                            var matHang = _context.MatHangs.FirstOrDefault(mh => mh.MaHang == maHang);
                            if (matHang != null)
                            {
                                matHang.SoLuongTon += soLuong;
                                matHang.NgayNhapGanNhat = DateOnly.FromDateTime(dtpNgayNhap.Value);

                                // Cập nhật trạng thái
                                if (matHang.SoLuongTon > matHang.NguongCanhBao)
                                    matHang.TrangThai = "Còn hàng";
                                else if (matHang.SoLuongTon > 0)
                                    matHang.TrangThai = "Sắp hết";
                                else
                                    matHang.TrangThai = "Hết hàng";
                            }
                        }
                    }

                    _context.SaveChanges();
                    transaction.Commit();

                    MessageBox.Show("Tạo phiếu nhập thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu phiếu nhập: " + ex.Message +
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

        private void txtDonGia_KeyPress(object sender, KeyPressEventArgs e)
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