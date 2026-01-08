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

        public event EventHandler<string> OnSwitchToHoaDonTab;

        private int _currentPage = 1;
        private int _pageSize = 8;
        private int _totalRecords = 0;

        private Panel pnlPagination;
        private FlowLayoutPanel flowPageNumbers; // Panel chứa các nút số (1, 2, 3...)
        private Button btnFirst; // Nút Về đầu <<
        private Button btnLast;  // Nút Về cuối >>
        private Button btnPrev;  // Nút Trước <
        private Button btnNext;  // Nút Sau >


        public KhachHangForm(KhachHangService khService)
        {
            InitializeComponent();
            _khService = khService;

            // Cấu hình giao diện ban đầu
            SetupUI();

            // Đăng ký sự kiện
            this.Load += async (s, e) => await LoadDataAsync();

            txtSearch.TextChanged += async (s, e) =>
            {
                _currentPage = 1;
                await LoadDataAsync();
            };

            if (btnXuatBaoCao != null) btnXuatBaoCao.Click += btnXuatBaoCao_Click;

            AssignFilterEvents();
        }

        // pnlDetail
        public void SetMainForm(MainForm main)
        {
            _mainForm = main;
        }

        #region SETUP UI
        private void SetupUI()
        {
            // Cấu hình FlowLayoutPanel đẹp bằng code
            // Bạn không cần chỉnh tay trong Designer nữa
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.BackColor = Color.FromArgb(241, 245, 249); // Màu nền xám nhạt hiện đại
            flowLayoutPanel1.Padding = new Padding(20); // Cách lề xung quanh
            flowLayoutPanel1.Dock = DockStyle.Fill;

            SetupPaginationUI();
        }

        private void SetupPaginationUI()
        {
            // 1. Panel tổng chứa thanh phân trang
            pnlPagination = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60, // Cao hơn chút cho thoáng
                BackColor = Color.White,
                Padding = new Padding(10) // Cách lề
            };

            // 2. FlowLayout ở giữa để chứa các nút số
            flowPageNumbers = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                Anchor = AnchorStyles.None, // Căn giữa
                BackColor = Color.Transparent
            };
            // Hack căn giữa FlowLayout trong Panel: Ta sẽ tính toán vị trí sau hoặc dùng TableLayout
            // Nhưng đơn giản nhất là đặt nó vào giữa Form thủ công một chút ở sự kiện Resize

            // Tạo các nút điều hướng (Style đẹp)
            btnFirst = CreateNavButton("«", async () => await GoToPage(1));
            btnPrev = CreateNavButton("❮", async () => await GoToPage(_currentPage - 1));
            btnNext = CreateNavButton("❯", async () => await GoToPage(_currentPage + 1));

            // Lưu ý: Nút Last sẽ cần tính toán tổng trang mới biết được, ta để placeholder
            btnLast = CreateNavButton("»", async () =>
            {
                int totalPages = (int)Math.Ceiling((double)_totalRecords / _pageSize);
                await GoToPage(totalPages);
            });

            // Thêm vào Panel (Dùng TableLayoutPanel để căn giữa cho chuẩn)
            var containerTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Color.Transparent
            };
            containerTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); // Khoảng trống trái
            containerTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));    // Nội dung chính
            containerTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); // Khoảng trống phải

            // Panel chứa tất cả nút: [<<] [<] [1] [2] ... [>] [>>]
            var centerPanel = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };

            centerPanel.Controls.Add(btnFirst);
            centerPanel.Controls.Add(btnPrev);
            centerPanel.Controls.Add(flowPageNumbers); // Các nút số sẽ được add vào đây
            centerPanel.Controls.Add(btnNext);
            centerPanel.Controls.Add(btnLast);

            containerTable.Controls.Add(centerPanel, 1, 0); // Add vào cột giữa

            pnlPagination.Controls.Add(containerTable);

            this.Controls.Add(pnlPagination);
            pnlPagination.BringToFront();
        }

        // Hàm hỗ trợ tạo nút điều hướng nhanh
        private Button CreateNavButton(string text, Action onClick)
        {
            var btn = new Button
            {
                Text = text,
                Size = new Size(45, 36), // Vuông vắn
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(64, 64, 64),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(2) // Cách nhau 2px
            };
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224); // Viền xám nhạt
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(243, 244, 246); // Hover màu xám nhẹ

            btn.Click += (s, e) => onClick();
            return btn;
        }

        // Hàm chuyển trang
        private async Task GoToPage(int page)
        {
            int totalPages = (int)Math.Ceiling((double)_totalRecords / _pageSize);
            if (totalPages == 0) totalPages = 1;

            if (page < 1 || page > totalPages) return;
            if (page == _currentPage) return;

            _currentPage = page;
            await LoadDataAsync();
        }
        private void RenderPagination()
        {
            flowPageNumbers.SuspendLayout();
            flowPageNumbers.Controls.Clear();

            int totalPages = (int)Math.Ceiling((double)_totalRecords / _pageSize);
            if (totalPages == 0) totalPages = 1;

            // Cập nhật trạng thái nút điều hướng
            btnFirst.Enabled = btnPrev.Enabled = (_currentPage > 1);
            btnNext.Enabled = btnLast.Enabled = (_currentPage < totalPages);

            // Đổi màu nút disable cho đẹp
            StyleDisabledButton(btnFirst);
            StyleDisabledButton(btnPrev);
            StyleDisabledButton(btnNext);
            StyleDisabledButton(btnLast);


            // --- THUẬT TOÁN HIỂN THỊ TRANG ---
            // Luôn hiện trang 1 và trang cuối.
            // Hiện các trang xung quanh trang hiện tại (delta = 2)

            int delta = 2; // Số lượng trang hiển thị bên cạnh trang hiện tại
            int left = _currentPage - delta;
            int right = _currentPage + delta;

            // Danh sách các trang sẽ hiển thị
            var range = new System.Collections.Generic.List<int>();
            var rangeWithDots = new System.Collections.Generic.List<object>(); // int hoặc string "..."

            for (int i = 1; i <= totalPages; i++)
            {
                if (i == 1 || i == totalPages || (i >= left && i <= right))
                {
                    range.Add(i);
                }
            }

            // Thêm dấu "..."
            int? prev = null;
            foreach (var i in range)
            {
                if (prev != null)
                {
                    if (i - prev == 2)
                    {
                        rangeWithDots.Add(prev + 1); // Nếu cách nhau 1 số thì hiện luôn số đó (vd: 1 [2] 3)
                    }
                    else if (i - prev > 1)
                    {
                        rangeWithDots.Add("..."); // Nếu cách xa thì hiện ...
                    }
                }
                rangeWithDots.Add(i);
                prev = i;
            }

            // --- VẼ NÚT ---
            foreach (var item in rangeWithDots)
            {
                if (item is string)
                {
                    // Vẽ label "..."
                    var lbl = new Label
                    {
                        Text = "...",
                        AutoSize = false,
                        Size = new Size(30, 36),
                        TextAlign = ContentAlignment.BottomCenter,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        ForeColor = Color.Gray,
                        Margin = new Padding(0, 0, 0, 5) // Căn chỉnh cho khớp button
                    };
                    flowPageNumbers.Controls.Add(lbl);
                }
                else
                {
                    // Vẽ nút số
                    int pageNum = (int)item;
                    var btn = new Button
                    {
                        Text = pageNum.ToString(),
                        Size = new Size(45, 36),
                        FlatStyle = FlatStyle.Flat,
                        Cursor = Cursors.Hand,
                        Font = new Font("Segoe UI", 10, FontStyle.Regular),
                        Margin = new Padding(2)
                    };

                    if (pageNum == _currentPage)
                    {
                        // Style cho trang hiện tại (Màu xanh, chữ trắng)
                        btn.BackColor = Color.FromArgb(99, 102, 241); // Xanh tím Indigo
                        btn.ForeColor = Color.White;
                        btn.FlatAppearance.BorderSize = 0;
                    }
                    else
                    {
                        // Style cho trang khác (Màu trắng, chữ đen)
                        btn.BackColor = Color.White;
                        btn.ForeColor = Color.Black;
                        btn.FlatAppearance.BorderSize = 1;
                        btn.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
                        btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(243, 244, 246);

                        // Sự kiện click
                        btn.Click += async (s, e) => await GoToPage(pageNum);
                    }

                    flowPageNumbers.Controls.Add(btn);
                }
            }

            flowPageNumbers.ResumeLayout();
        }

        private void StyleDisabledButton(Button btn)
        {
            if (btn.Enabled)
            {
                btn.BackColor = Color.White;
                btn.ForeColor = Color.FromArgb(64, 64, 64);
                btn.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
            }
            else
            {
                btn.BackColor = Color.FromArgb(248, 250, 252); // Xám rất nhạt
                btn.ForeColor = Color.LightGray; // Chữ mờ
                btn.FlatAppearance.BorderColor = Color.FromArgb(241, 245, 249);
            }
        }


        #endregion

        private async Task LoadDataAsync()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                flowLayoutPanel1.SuspendLayout();
                flowLayoutPanel1.Controls.Clear();

                string keyword = txtSearch.Text.Trim();

                // 1. GỌI SERVICE (Giả sử bạn đã sửa Service trả về Tuple như Bước 1)
                // Nếu Service chưa sửa, bạn phải sửa Service trước nhé!
                var result = await _khService.GetListKhachHangPagingAsync(
                    keyword,
                    _currentRankFilter,
                    _isShowDeletedMode,
                    _currentPage,
                    _pageSize
                );

                var listKH = result.Data;      // Danh sách khách hàng trang hiện tại
                _totalRecords = result.TotalCount; // Tổng số bản ghi tìm thấy

                RenderPagination();
                // 2. CẬP NHẬT UI PHÂN TRANG
                //UpdatePaginationControls();

                if (listKH == null || listKH.Count == 0)
                {
                    ShowEmptyState();
                }
                else
                {
                    foreach (var kh in listKH)
                    {
                        var card = new KhachHangCard();
                        card.SetData(kh);
                        card.Margin = new Padding(0, 0, 20, 20);

                        // Gán sự kiện click (Giữ nguyên code cũ)
                        card.Click += (s, e) => ShowDetail(kh.MaKh);
                        foreach (Control child in card.Controls)
                            child.Click += (s, e) => ShowDetail(kh.MaKh);

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

        #region Right Panel Container

        // Xem Chi Tiết Khi click on Card KhachHang
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

                        detailControl.Dock = DockStyle.Fill;

                        detailControl.LoadData(detail);

                        detailControl.OnEditClick += (s, id) =>
                        {
                            EditKhachHang(id);
                        };
                        detailControl.OnCloseClick += (s, id) =>
                        {
                            HideDetailPanel();
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
                                MessageBox.Show("Thao tác thành công!");
                            }
                        };
                        detailControl.OnRequestViewHistory += (s, sdt) =>
                        {
                            HideDetailPanel();
                            OnSwitchToHoaDonTab?.Invoke(this, sdt);
                        };
                        ShowRightPanel(detailControl,375);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xem chi tiết: " + ex.Message);
            }
        }


        private void ShowRightPanel(UserControl userControl, int width = 350)
        {
            pnlRightContainer.Width = width;

            pnlRightContainer.Controls.Clear();

            pnlRightContainer.Controls.Add(userControl);

            pnlRightContainer.Visible = true;
        }
        
        private void HideDetailPanel()
        {
            pnlRightContainer.Visible = false;
            pnlRightContainer.Controls.Clear();
        }


        #endregion
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
                    _currentPage = 1;
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

        #region Button Function
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
                //MessageBox.Show("Lỗi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        #endregion
    }
}