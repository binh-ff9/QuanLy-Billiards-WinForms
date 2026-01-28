using System;
using System.Linq;
using System.Windows.Forms;
using Billiard.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace Billiard.WinForm.Forms
{
    public partial class ucPhieuNhapXuat : UserControl
    {
        private readonly BilliardDbContext _context;
        private int selectedMaPN = 0;

        public ucPhieuNhapXuat()
        {
            InitializeComponent();
            _context = new BilliardDbContext();
        }

        private void ucPhieuNhapXuat_Load(object sender, EventArgs e)
        {
            LoadNhaCungCap();
            LoadPhieuNhap();
            dtpTuNgay.Value = DateTime.Now.AddMonths(-1);
            dtpDenNgay.Value = DateTime.Now;
        }

        private void LoadNhaCungCap()
        {
            try
            {
                var nhaCungCaps = _context.NhaCungCaps
                    .Select(ncc => new { ncc.MaNcc, ncc.TenNcc })
                    .ToList();

                cboNhaCungCap.DataSource = nhaCungCaps;
                cboNhaCungCap.DisplayMember = "TenNcc";
                cboNhaCungCap.ValueMember = "MaNcc";
                cboNhaCungCap.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách nhà cung cấp: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPhieuNhap(int? maNcc = null, DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            try
            {
                var query = _context.PhieuNhaps
                    .Include(pn => pn.MaNvNavigation)
                    .Include(pn => pn.MaNccNavigation)
                    .AsQueryable();

                if (maNcc.HasValue)
                    query = query.Where(pn => pn.MaNcc == maNcc.Value);

                if (tuNgay.HasValue)
                    query = query.Where(pn => pn.NgayNhap >= tuNgay.Value);

                if (denNgay.HasValue)
                    query = query.Where(pn => pn.NgayNhap <= denNgay.Value.AddDays(1).AddSeconds(-1));

                var phieuNhaps = query
                    .OrderByDescending(pn => pn.NgayNhap)
                    .Select(pn => new
                    {
                        pn.MaPn,
                        TenNv = pn.MaNvNavigation.TenNv,
                        TenNcc = pn.MaNccNavigation.TenNcc,
                        pn.NgayNhap,
                        pn.TongTien,
                        pn.GhiChu
                    })
                    .ToList();

                dgvPhieuNhap.Rows.Clear();
                foreach (var item in phieuNhaps)
                {
                    dgvPhieuNhap.Rows.Add(
                        item.MaPn,
                        item.TenNv,
                        item.TenNcc,
                        item.NgayNhap.HasValue ? item.NgayNhap.Value.ToString("dd/MM/yyyy HH:mm") : "",
                        item.TongTien.HasValue ? item.TongTien.Value.ToString("#,##0") + " đ" : "0 đ",
                        item.GhiChu
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách phiếu nhập: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadChiTietPhieuNhap(int maPn)
        {
            try
            {
                var phieuNhap = _context.PhieuNhaps
                    .Include(pn => pn.MaNvNavigation)
                    .Include(pn => pn.MaNccNavigation)
                    .FirstOrDefault(pn => pn.MaPn == maPn);

                if (phieuNhap == null)
                {
                    MessageBox.Show("Không tìm thấy phiếu nhập!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                lblThongTinPhieu.Text = $"Phiếu nhập #{phieuNhap.MaPn} - Nhà cung cấp: {phieuNhap.MaNccNavigation?.TenNcc} - " +
                                       $"Nhân viên: {phieuNhap.MaNvNavigation?.TenNv} - " +
                                       $"Ngày: {(phieuNhap.NgayNhap.HasValue ? phieuNhap.NgayNhap.Value.ToString("dd/MM/yyyy HH:mm") : "")}";

                var chiTiets = _context.ChiTietPhieuNhaps
                    .Include(ct => ct.MaHangNavigation)
                    .Where(ct => ct.MaPn == maPn)
                    .Select(ct => new
                    {
                        TenHang = ct.MaHangNavigation.TenHang,
                        ct.SoLuongNhap,
                        ct.DonGiaNhap,
                        ct.ThanhTien
                    })
                    .ToList();

                dgvChiTiet.Rows.Clear();
                int stt = 1;
                foreach (var item in chiTiets)
                {
                    dgvChiTiet.Rows.Add(
                        stt++,
                        item.TenHang,
                        item.SoLuongNhap,
                        item.DonGiaNhap.ToString("#,##0") + " đ",
                        item.ThanhTien.HasValue ? item.ThanhTien.Value.ToString("#,##0") + " đ" : "0 đ"
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải chi tiết phiếu nhập: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            int? maNcc = cboNhaCungCap.SelectedValue != null ? (int?)cboNhaCungCap.SelectedValue : null;
            DateTime? tuNgay = dtpTuNgay.Value.Date;
            DateTime? denNgay = dtpDenNgay.Value.Date;

            if (tuNgay > denNgay)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadPhieuNhap(maNcc, tuNgay, denNgay);
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            cboNhaCungCap.SelectedIndex = -1;
            dtpTuNgay.Value = DateTime.Now.AddMonths(-1);
            dtpDenNgay.Value = DateTime.Now;
            LoadPhieuNhap();
        }

        private void btnTaoPhieuNhap_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng tạo phiếu nhập đang được phát triển!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            // TODO: Mở form tạo phiếu nhập mới
        }

        private void btnXemChiTiet_Click(object sender, EventArgs e)
        {
            if (dgvPhieuNhap.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một phiếu nhập để xem chi tiết!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            selectedMaPN = Convert.ToInt32(dgvPhieuNhap.CurrentRow.Cells["colMaPN"].Value);
            LoadChiTietPhieuNhap(selectedMaPN);
            tabControl.SelectedTab = tabChiTietPhieu;
        }

        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabPhieuNhap;
        }
    }
}