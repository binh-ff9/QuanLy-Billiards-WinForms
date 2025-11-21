using Billiard.BLL.Services;
using Billiard.BLL.Services.KhachHangServices;
using Billiard.DAL.Entities; // Để dùng Entity KhachHang
using ClosedXML.Excel;
using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.Extensions.DependencyInjection; // Để tạo Scope mới
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.KhachHang
{
    public partial class KhachHangForm : Form
    {
        private readonly KhachHangService _khService;
        private MainForm _mainForm;

        private string _currentRankFilter = "Tất cả"; // Filters ở đây

        private bool _isShowDeletedMode = false;

        public KhachHangForm(KhachHangService khService)
        {
            InitializeComponent();
            _khService = khService;

            // Cấu hình giao diện ban đầu
            SetupUI();

            // Đăng ký sự kiện
            this.Load += async (s, e) => await LoadDataAsync();

            txtSearch.TextChanged += async (s, e) => await LoadDataAsync();


            if (btnXuatBaoCao != null) btnXuatBaoCao.Click += btnXuatBaoCao_Click;

            AssignFilterEvents();
        }

        // pnlDetail
        public void SetMainForm(MainForm main)
        {
            _mainForm = main;
        }

        private void SetupUI()
        {
            // Cấu hình FlowLayoutPanel đẹp bằng code
            // Bạn không cần chỉnh tay trong Designer nữa
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.BackColor = Color.FromArgb(241, 245, 249); // Màu nền xám nhạt hiện đại
            flowLayoutPanel1.Padding = new Padding(20); // Cách lề xung quanh
            flowLayoutPanel1.Dock = DockStyle.Fill;
        }

        private async Task LoadDataAsync()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // Tạm dừng vẽ giao diện để load nhanh hơn
                flowLayoutPanel1.SuspendLayout();
                flowLayoutPanel1.Controls.Clear();

                // 1. Lấy dữ liệu từ Service
                string keyword = txtSearch.Text.Trim();
                var listKH = await _khService.GetListKhachHangAsync(keyword, _currentRankFilter, _isShowDeletedMode);

                if (listKH == null || listKH.Count == 0)
                {
                    ShowEmptyState(); // Hiện thông báo nếu không có dữ liệu
                }
                else
                {
                    // 2. Tạo Card cho từng khách hàng
                    foreach (var kh in listKH)
                    {
                        // Tạo UserControl Card
                        var card = new KhachHangCard();
                        card.SetData(kh);
                        card.Margin = new Padding(0, 0, 20, 20); // Khoảng cách giữa các thẻ

                        // Xử lý sự kiện Click vào Card -> Hiện chi tiết
                        card.Click += (s, e) => ShowDetail(kh.MaKh);

                        // Mẹo: Khi click vào các thành phần con trong card (Label, Panel...)
                        // thì sự kiện cũng phải nổ. Code này gán đệ quy click cho mọi con.
                        foreach (Control child in card.Controls)
                        {
                            child.Click += (s, e) => ShowDetail(kh.MaKh);
                        }

                        flowLayoutPanel1.Controls.Add(card);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Tiếp tục vẽ và phục hồi con trỏ chuột
                flowLayoutPanel1.ResumeLayout();
                this.Cursor = Cursors.Default;
            }
        }

        private void ShowEmptyState()
        {
            Label lblEmpty = new Label
            {
                Text = "Không tìm thấy khách hàng nào 😢",
                Font = new Font("Segoe UI", 14, FontStyle.Regular),
                ForeColor = Color.Gray,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 100,
                Width = flowLayoutPanel1.Width - 40
            };
            flowLayoutPanel1.Controls.Add(lblEmpty);
        }

        // Xem Chi Tiết
        private async void ShowDetail(int maKh)
        {
            try
            {
                using (var scope = Program.ServiceProvider.CreateScope())
                {
                    var tempService = scope.ServiceProvider.GetRequiredService<KhachHangService>();
                    var detail = await tempService.GetKhachHangDetailAsync(maKh);

                    if (detail != null && _mainForm != null)
                    {
                        var detailControl = new ChiTietKhachHangControl();
                        detailControl.LoadData(detail);

                        detailControl.OnEditClick += (s, id) =>
                        {
                            // Gọi hàm sửa (Hàm này mình viết ở dưới)
                            EditKhachHang(id);
                        };

                        detailControl.OnDeleteClick += async (s, id) =>
                        {
                            string actionName = _isShowDeletedMode ? "Khôi phục" : "Xóa";
                            var confirm = MessageBox.Show($"Bạn có chắc muốn {actionName} khách hàng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                            if (confirm == DialogResult.Yes)
                            {
                                using (var scope2 = Program.ServiceProvider.CreateScope())
                                {
                                    var svc = scope2.ServiceProvider.GetRequiredService<KhachHangService>();

                                    // Gọi hàm đổi trạng thái
                                    // Nếu đang xem list Xóa (_isShowDeletedMode = true) -> Cần set Active = true (Khôi phục)
                                    // Nếu đang xem list Active (_isShowDeletedMode = false) -> Cần set Active = false (Xóa)
                                    await svc.ToggleStatusAsync(id, _isShowDeletedMode);
                                }

                                // Tải lại danh sách và đóng panel
                                await LoadDataAsync();
                                _mainForm.HideDetailPanel();

                                MessageBox.Show("Thao tác thành công!");
                            }
                        };
                        // Gọi MainForm mở Panel phải (Rộng 500px cho đẹp)
                        _mainForm.UpdateDetailPanel("Thông tin khách hàng", detailControl, 500);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xem chi tiết: " + ex.Message);
            }
        }

        private async void EditKhachHang(int maKh)
        {
            using (var scope = Program.ServiceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<KhachHangService>();

                var frm = new KhachHangEditForm(service, maKh);

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    await LoadDataAsync();

                    ShowDetail(maKh);

                    MessageBox.Show("Đã cập nhật thông tin!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        #region Filters
        private void AssignFilterEvents()
        {
            // Danh sách các nút (Đảm bảo tên đúng với Designer)
            Button[] filterBtns = { btnTatCa, btnDong, btnBac, btnVang, btnBachKim };

            foreach (var btn in filterBtns)
            {
                btn.Click += (s, e) =>
                {
                    if (btn.Tag != null)
                    {
                        _currentRankFilter = btn.Tag.ToString();
                    }
                    else
                    {
                        _currentRankFilter = btn.Text.Trim();
                    }

                    // -----------------------------------

                    // 2. Đổi màu nút (UI) - Giữ nguyên code của bạn
                    foreach (var b in filterBtns)
                    {
                        b.BackColor = Color.FromArgb(226, 232, 240);
                        b.ForeColor = Color.Black;
                    }
                    btn.BackColor = Color.FromArgb(99, 102, 241);
                    btn.ForeColor = Color.White;

                    // 3. Tải lại dữ liệu
                    LoadDataAsync();
                };
            }
        }


        #endregion

        #region CRUD
        private async void btnThem_Click_1(object sender, EventArgs e)
        {
            using (var scrope = Program.ServiceProvider.CreateScope())
            {
                var service = scrope.ServiceProvider.GetRequiredService<KhachHangService>();

                var frm = new KhachHangEditForm(service, null);

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    await LoadDataAsync();
                }
            }
        }

        #endregion

        private async void btnXuatBaoCao_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Lấy dữ liệu hiện tại (đang hiển thị trên màn hình)
                string keyword = txtSearch.Text.Trim();
                var dataToExport = await _khService.GetListKhachHangAsync(keyword, _currentRankFilter);

                if (dataToExport.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. Chọn nơi lưu file
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx", FileName = $"DanhSachKhachHang_{DateTime.Now:ddMMyyyy}.xlsx" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        // 3. Tạo Excel bằng ClosedXML
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("KhachHang");

                            // Header
                            worksheet.Cell(1, 1).Value = "Mã KH";
                            worksheet.Cell(1, 2).Value = "Họ tên";
                            worksheet.Cell(1, 3).Value = "Số điện thoại";
                            worksheet.Cell(1, 4).Value = "Email";
                            worksheet.Cell(1, 5).Value = "Điểm tích lũy";
                            worksheet.Cell(1, 6).Value = "Hạng";
                            worksheet.Cell(1, 7).Value = "Tổng chi tiêu";

                            // Style Header
                            var headerRange = worksheet.Range("A1:G1");
                            headerRange.Style.Font.Bold = true;
                            headerRange.Style.Fill.BackgroundColor = XLColor.CornflowerBlue;
                            headerRange.Style.Font.FontColor = XLColor.White;
                            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                            // Đổ dữ liệu
                            int row = 2;
                            foreach (var kh in dataToExport)
                            {
                                worksheet.Cell(row, 1).Value = kh.MaKh;
                                worksheet.Cell(row, 2).Value = kh.TenKh;
                                worksheet.Cell(row, 3).Value = kh.Sdt;
                                worksheet.Cell(row, 4).Value = kh.Email;
                                worksheet.Cell(row, 5).Value = kh.DiemTichLuy ?? 0;

                                // Tính lại hạng để xuất
                                worksheet.Cell(row, 6).Value = GetRankName(kh.DiemTichLuy ?? 0);

                                // Tính tổng tiền
                                decimal tongTien = kh.HoaDons?.Sum(h => h.TongTien) ?? 0;
                                worksheet.Cell(row, 7).Value = tongTien;
                                worksheet.Cell(row, 7).Style.NumberFormat.Format = "#,##0"; // Format tiền tệ

                                row++;
                            }

                            // AutoFit cột
                            worksheet.Columns().AdjustToContents();

                            workbook.SaveAs(sfd.FileName);
                            MessageBox.Show("Xuất báo cáo thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // GET :: RankNAme
        private string GetRankName(int diem)
        {
            if (diem > 300) return "Bạch Kim";
            if (diem > 150) return "Vàng";
            if (diem > 70) return "Bạc";
            return "Đồng";
        }

        private void BtnDaXoa_Click(object sender, EventArgs e)
        {
            // 1. Đảo ngược trạng thái
            _isShowDeletedMode = !_isShowDeletedMode;

            // 2. Đổi Text và Màu của nút "Đã xóa" để người dùng biết
            if (_isShowDeletedMode)
            {
                btnDaXoa.Text = "⬅️ Quay lại";
                btnDaXoa.BackColor = Color.Gray;
            }
            else
            {
                btnDaXoa.Text = "🗑️ Đã xóa";
                btnDaXoa.BackColor = Color.FromArgb(51, 65, 85); // Màu gốc
            }

            // 3. Tải lại dữ liệu theo chế độ mới
            LoadDataAsync();

            // 4. Đóng panel chi tiết cũ đi (vì ID cũ có thể không còn trong list mới)
            if (_mainForm != null) _mainForm.HideDetailPanel();
        }
    }
}