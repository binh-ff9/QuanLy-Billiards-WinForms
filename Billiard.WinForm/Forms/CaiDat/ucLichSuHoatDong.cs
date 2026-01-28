using System;
using System.Linq;
using System.Windows.Forms;
using Billiard.DAL.Data;

namespace Billiard.WinForm.Forms
{
    public partial class ucLichSuHoatDong : UserControl
    {
        private readonly BilliardDbContext _context;

        public ucLichSuHoatDong()
        {
            InitializeComponent();
            _context = new BilliardDbContext();
        }

        private void ucLichSuHoatDong_Load(object sender, EventArgs e)
        {
            LoadNhanVien();
            LoadLichSuHoatDong();
        }

        private void LoadNhanVien()
        {
            try
            {
                var nhanViens = _context.NhanViens
                    .Where(nv => nv.TrangThai == "Đang làm")
                    .Select(nv => new { nv.MaNv, nv.TenNv })
                    .ToList();

                cboNhanVien.DataSource = nhanViens;
                cboNhanVien.DisplayMember = "TenNv";
                cboNhanVien.ValueMember = "MaNv";
                cboNhanVien.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách nhân viên: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadLichSuHoatDong(int? maNv = null, DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            try
            {
                var query = _context.LichSuHoatDongs.AsQueryable();

                if (maNv.HasValue)
                    query = query.Where(ls => ls.MaNv == maNv.Value);

                if (tuNgay.HasValue)
                    query = query.Where(ls => ls.ThoiGian >= tuNgay.Value);

                if (denNgay.HasValue)
                    query = query.Where(ls => ls.ThoiGian <= denNgay.Value.AddDays(1).AddSeconds(-1));

                var lichSu = query
                    .OrderByDescending(ls => ls.ThoiGian)
                    .Select(ls => new
                    {
                        ls.Id,
                        TenNv = ls.MaNvNavigation != null ? ls.MaNvNavigation.TenNv : "Không xác định",
                        ls.HanhDong,
                        ls.ChiTiet,
                        ls.ThoiGian
                    })
                    .ToList();

                dgvLichSu.Rows.Clear();
                int stt = 1;
                foreach (var item in lichSu)
                {
                    dgvLichSu.Rows.Add(
                        stt++,
                        item.TenNv,
                        item.HanhDong,
                        item.ChiTiet,
                        item.ThoiGian?.ToString("dd/MM/yyyy HH:mm:ss")
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải lịch sử hoạt động: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            int? maNv = cboNhanVien.SelectedValue != null ? (int?)cboNhanVien.SelectedValue : null;
            DateTime? tuNgay = dtpTuNgay.Value.Date;
            DateTime? denNgay = dtpDenNgay.Value.Date;

            if (tuNgay > denNgay)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadLichSuHoatDong(maNv, tuNgay, denNgay);
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            cboNhanVien.SelectedIndex = -1;
            dtpTuNgay.Value = DateTime.Now.AddDays(-7);
            dtpDenNgay.Value = DateTime.Now;
            LoadLichSuHoatDong();
        }
    }
}