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
        private int _maNv; // Mã nhân viên đang đăng nhập

        // ✅ SỬA: Constructor mặc định PHẢI khởi tạo _context
        public ucPhieuNhapXuat() : this(0)
        {
            // Gọi constructor khác với maNv = 0
            // Constructor này sẽ khởi tạo _context
        }

        // ✅ Constructor chính - LUÔN khởi tạo _context
        public ucPhieuNhapXuat(int maNv)
        {
            InitializeComponent();

            // ✅ QUAN TRỌNG: Luôn khởi tạo context
            _context = new BilliardDbContext();

            _maNv = maNv;
        }

        // Method để set mã nhân viên sau khi khởi tạo
        public void SetMaNhanVien(int maNv)
        {
            _maNv = maNv;
        }

        private void ucPhieuNhapXuat_Load(object sender, EventArgs e)
        {
            // ✅ THÊM: Kiểm tra _context trước khi sử dụng
            if (_context == null)
            {
                MessageBox.Show("Lỗi: Database context chưa được khởi tạo!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LoadNhaCungCap();
            LoadPhieuNhap();
            dtpTuNgay.Value = DateTime.Now.AddMonths(-1);
            dtpDenNgay.Value = DateTime.Now;
        }

        private void LoadNhaCungCap()
        {
            try
            {
                // ✅ THÊM: Kiểm tra _context
                if (_context == null)
                {
                    MessageBox.Show("Lỗi: Database context chưa được khởi tạo!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

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

        private void LoadPhieuNhap(int? maNcc = null, DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            try
            {
                // ✅ THÊM: Kiểm tra _context
                if (_context == null)
                {
                    MessageBox.Show("Lỗi: Database context chưa được khởi tạo!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Clear cache
                _context.ChangeTracker.Clear();

                var query = _context.PhieuNhaps
                    .AsNoTracking()
                    .AsQueryable();

                if (maNcc.HasValue)
                    query = query.Where(pn => pn.MaNcc == maNcc.Value);

                if (tuNgay.HasValue)
                    query = query.Where(pn => pn.NgayNhap >= tuNgay.Value);

                if (denNgay.HasValue)
                    query = query.Where(pn => pn.NgayNhap <= denNgay.Value.AddDays(1).AddSeconds(-1));

                // Lấy danh sách phiếu nhập
                var phieuNhaps = query
                    .OrderByDescending(pn => pn.NgayNhap)
                    .Select(pn => new
                    {
                        pn.MaPn,
                        pn.MaNv,
                        pn.MaNcc,
                        pn.NgayNhap,
                        pn.TongTien,
                        pn.GhiChu
                    })
                    .ToList();

                dgvPhieuNhap.Rows.Clear();

                // Lấy danh sách nhân viên và nhà cung cấp 1 lần để tối ưu
                var maNvs = phieuNhaps.Select(p => p.MaNv).Distinct().ToList();
                var maNccs = phieuNhaps.Select(p => p.MaNcc).Distinct().ToList();

                var nhanViens = _context.NhanViens
                    .AsNoTracking()
                    .Where(nv => maNvs.Contains(nv.MaNv))
                    .ToDictionary(nv => nv.MaNv, nv => nv.TenNv);

                var nhaCungCaps = _context.NhaCungCaps
                    .AsNoTracking()
                    .Where(ncc => maNccs.Contains(ncc.MaNcc))
                    .ToDictionary(ncc => ncc.MaNcc, ncc => ncc.TenNcc);

                foreach (var item in phieuNhaps)
                {
                    // Xử lý trường hợp không tìm thấy nhân viên hoặc nhà cung cấp
                    string tenNv = nhanViens.ContainsKey(item.MaNv)
                        ? nhanViens[item.MaNv]
                        : $"NV#{item.MaNv}";

                    string tenNcc = nhaCungCaps.ContainsKey(item.MaNcc)
                        ? nhaCungCaps[item.MaNcc]
                        : $"NCC#{item.MaNcc}";

                    dgvPhieuNhap.Rows.Add(
                        item.MaPn,
                        tenNv,
                        tenNcc,
                        item.NgayNhap.HasValue ? item.NgayNhap.Value.ToString("dd/MM/yyyy HH:mm") : "",
                        string.Format("{0:N0} đ", item.TongTien),
                        item.GhiChu
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách phiếu nhập: " + ex.Message +
                                "\n\nChi tiết: " + (ex.InnerException?.Message ?? "Không có"),
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadChiTietPhieuNhap(int maPn)
        {
            try
            {
                // ✅ THÊM: Kiểm tra _context
                if (_context == null)
                {
                    MessageBox.Show("Lỗi: Database context chưa được khởi tạo!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _context.ChangeTracker.Clear();

                var phieuNhap = _context.PhieuNhaps
                    .AsNoTracking()
                    .Where(pn => pn.MaPn == maPn)
                    .Select(pn => new
                    {
                        pn.MaPn,
                        pn.MaNv,
                        pn.MaNcc,
                        pn.NgayNhap
                    })
                    .FirstOrDefault();

                if (phieuNhap == null)
                {
                    MessageBox.Show("Không tìm thấy phiếu nhập!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy tên nhân viên và nhà cung cấp
                var tenNv = _context.NhanViens
                    .Where(nv => nv.MaNv == phieuNhap.MaNv)
                    .Select(nv => nv.TenNv)
                    .FirstOrDefault() ?? $"NV#{phieuNhap.MaNv}";

                var tenNcc = _context.NhaCungCaps
                    .Where(ncc => ncc.MaNcc == phieuNhap.MaNcc)
                    .Select(ncc => ncc.TenNcc)
                    .FirstOrDefault() ?? $"NCC#{phieuNhap.MaNcc}";

                lblThongTinPhieu.Text = $"Phiếu nhập #{phieuNhap.MaPn} - Nhà cung cấp: {tenNcc} - " +
                                       $"Nhân viên: {tenNv} - " +
                                       $"Ngày: {(phieuNhap.NgayNhap.HasValue ? phieuNhap.NgayNhap.Value.ToString("dd/MM/yyyy HH:mm") : "")}";

                var chiTiets = _context.ChiTietPhieuNhaps
                    .AsNoTracking()
                    .Where(ct => ct.MaPn == maPn)
                    .Select(ct => new
                    {
                        MaHang = ct.MaHang,
                        ct.SoLuongNhap,
                        ct.DonGiaNhap,
                        ct.ThanhTien
                    })
                    .ToList();

                // Lấy tên hàng
                var maHangs = chiTiets.Select(ct => ct.MaHang).ToList();
                var matHangs = _context.MatHangs
                    .AsNoTracking()
                    .Where(mh => maHangs.Contains(mh.MaHang))
                    .ToDictionary(mh => mh.MaHang, mh => mh.TenHang);

                dgvChiTiet.Rows.Clear();
                int stt = 1;
                foreach (var item in chiTiets)
                {
                    string tenHang = matHangs.ContainsKey(item.MaHang)
                        ? matHangs[item.MaHang]
                        : $"Hàng#{item.MaHang}";

                    dgvChiTiet.Rows.Add(
                        stt++,
                        tenHang,
                        item.SoLuongNhap,
                        string.Format("{0:N0} đ", item.DonGiaNhap),
                        string.Format("{0:N0} đ", item.ThanhTien)
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải chi tiết phiếu nhập: " + ex.Message +
                                "\n\nChi tiết: " + (ex.InnerException?.Message ?? "Không có"),
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            try
            {
                // ✅ KIỂM TRA: Nếu _maNv = 0, cảnh báo
                if (_maNv == 0)
                {
                    MessageBox.Show("Lỗi: Không xác định được nhân viên đang đăng nhập!\n" +
                                    "Vui lòng đăng xuất và đăng nhập lại.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var frmTaoPhieuNhap = new frmTaoPhieuNhap(_maNv);
                if (frmTaoPhieuNhap.ShowDialog() == DialogResult.OK)
                {
                    // Refresh danh sách sau khi tạo phiếu nhập thành công
                    LoadPhieuNhap();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở form tạo phiếu nhập: " + ex.Message +
                                "\n\nChi tiết: " + (ex.InnerException?.Message ?? "Không có"),
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTaoPhieuXuat_Click(object sender, EventArgs e)
        {
            try
            {
                // ✅ KIỂM TRA: Nếu _maNv = 0, cảnh báo
                if (_maNv == 0)
                {
                    MessageBox.Show("Lỗi: Không xác định được nhân viên đang đăng nhập!\n" +
                                    "Vui lòng đăng xuất và đăng nhập lại.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var frmTaoPhieuXuat = new frmTaoPhieuXuat(_maNv);
                if (frmTaoPhieuXuat.ShowDialog() == DialogResult.OK)
                {
                    // Không cần refresh vì phiếu xuất không lưu vào bảng PhieuNhap
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở form tạo phiếu xuất: " + ex.Message +
                                "\n\nChi tiết: " + (ex.InnerException?.Message ?? "Không có"),
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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