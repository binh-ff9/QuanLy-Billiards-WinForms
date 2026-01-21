using Billiard.BLL.Services.HoaDonServices;
using Billiard.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.DependencyInjection;
using Color = System.Drawing.Color;
using Control = System.Windows.Forms.Control;


namespace Billiard.WinForm.Forms.HoaDon
{
    public partial class HoaDonForm : Form
    {
        private readonly HoaDonService _hoaDonService;

        private List<dynamic> _originalData = new List<dynamic>();

        private string _currentStatusFilter = "Tất cả";

        private MainForm _mainForm;


        private int _currentPage = 1;
        private int _pageSize = 15;
        private int _totalRecords = 0;

        // UI Controls for Pagination (Created dynamically)
        private Panel pnlPagination;
        private FlowLayoutPanel flowPageNumbers;
        private Button btnFirst, btnLast, btnPrev, btnNext;

        private System.Threading.Timer _searchDebounceTimer;

        public HoaDonForm(HoaDonService hoaDonService)
        {
            InitializeComponent();
            _hoaDonService = hoaDonService;
            
            SetupDateTimePickers();
            SetupPaginationUI(); // Tạo thanh phân trang

            // Đăng ký sự kiện
            txtSearch.TextChanged += (s, e) =>
            {
                _searchDebounceTimer?.Change(System.Threading.Timeout.Infinite, 0);
                _searchDebounceTimer = new System.Threading.Timer(async state =>
                {
                    if (this.IsHandleCreated)
                    {
                        this.Invoke(new Action(async () =>
                        {
                            _currentPage = 1;
                            await LoadDataAsync();
                        }));
                    }
                }, null, 500, System.Threading.Timeout.Infinite);
            };

            dtpTuNgay.ValueChanged += async (s, e) => { _currentPage = 1; await LoadDataAsync(); };
            dtpDenNgay.ValueChanged += async (s, e) => { _currentPage = 1; await LoadDataAsync(); };

            btnTatCa.Click += async (s, e) => await SetStatusFilter("Tất cả", btnTatCa);
            btnChuaThanhToan.Click += async (s, e) => await SetStatusFilter("Đang chơi", btnChuaThanhToan);
            btnDaThanhToan.Click += async (s, e) => await SetStatusFilter("Đã thanh toán", btnDaThanhToan);

            btnXuatBaoCao.Click += btnXuatBaoCao_Click;

            pnlListHoaDon.SizeChanged += (s, e) =>
            {
                pnlListHoaDon.SuspendLayout();
                foreach (Control c in pnlListHoaDon.Controls) c.Width = pnlListHoaDon.ClientSize.Width - 25;
                pnlListHoaDon.ResumeLayout();
            };
        }

        private async void HoaDonForm_Load(object sender, EventArgs e)
        {
            HighlightButton(btnTatCa);
            await LoadDataAsync();
        }


        public void SetMainForm(MainForm main)
        {
            _mainForm = main;
        }

        private async Task LoadDataAsync()
        {
            using (var scope = Program.ServiceProvider.CreateScope())
            {
                var scopedService = scope.ServiceProvider.GetRequiredService<HoaDonService>();

                try
                {
                    this.Cursor = Cursors.WaitCursor;

                    string keyword = txtSearch.Text.Trim();
                    DateTime fromDate = dtpTuNgay.Value.Date;
                    DateTime toDate = dtpDenNgay.Value.Date.AddDays(1).AddSeconds(-1);

                    // Gọi Service phân trang
                    var result = await scopedService.GetListHoaDonPagingAsync(
                        keyword, _currentStatusFilter, fromDate, toDate, _currentPage, _pageSize
                    );

                    var listHoaDon = result.Data;
                    _totalRecords = result.TotalCount;

                    // --- [RENDER GIAO DIỆN FLOWLAYOUT] ---
                    pnlListHoaDon.SuspendLayout();
                    pnlListHoaDon.Controls.Clear();

                    if (listHoaDon.Count == 0)
                    {
                        ShowEmptyState();
                    }
                    else
                    {
                        foreach (var hd in listHoaDon)
                        {
                            // Tạo dòng mới
                            var row = new HoaDonRowControl();
                            row.SetData(hd);

                            // Chỉnh kích thước full chiều ngang
                            row.Width = pnlListHoaDon.ClientSize.Width - 25; // Trừ 25px cho thanh cuộn
                            row.Margin = new Padding(0); // Sát nhau

                            // Gán sự kiện click để xem chi tiết
                            row.Clicked += (s, e) => ShowDetailInvoice(hd.MaHd);

                            pnlListHoaDon.Controls.Add(row);
                        }
                    }

                    RenderPagination(); // Vẽ lại phân trang
                    pnlListHoaDon.ResumeLayout();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}");
                }
                finally
                {
                    if (this.IsHandleCreated) this.Invoke(new Action(() => this.Cursor = Cursors.Default));
                }
            }
        }

        private void ShowEmptyState()
        {
            Label lbl = new Label
            {
                Text = "Không tìm thấy hóa đơn nào 🔍",
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 100,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new System.Drawing.Font("Segoe UI", 12),
                ForeColor = System.Drawing.Color.Gray
            };
            pnlListHoaDon.Controls.Add(lbl);
        }

        #region Right Panel Container
        private async void ShowDetailInvoice(int maHd)
        {
            // Lấy dữ liệu chi tiết từ DB
            using (var scope = Program.ServiceProvider.CreateScope())
            {
                var svc = scope.ServiceProvider.GetRequiredService<HoaDonService>();
                var fullInfo = await svc.GetChiTietHoaDon(maHd);

                if (fullInfo != null && _mainForm != null)
                {
                    // A. TẠO MỚI (Dynamic) - Không dùng cái có sẵn
                    var detailControl = new ChiTietHoaDonControl();

                    // B. Đổ dữ liệu vào
                    detailControl.LoadData(fullInfo);

                    // C. Dặn dò: "Khi nào nút X ở trong ông bị bấm, hãy nhờ MainForm ẩn đi"
                    detailControl.OnCloseClick += (s, e) => HideDetailPanel();

                    // D. Gọi MainForm: "Hiện cái này lên giúp tôi"
                    ShowRightPanel(detailControl, 550);
                }
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

        #region Filters
        private async Task SetStatusFilter(string status, Button activeBtn)
        {
            _currentStatusFilter = status;
            HighlightButton(activeBtn);
            _currentPage = 1;
            await LoadDataAsync();
        }
        public void FilterBySdt(string sdt)
        {
            txtSearch.Text = sdt;
            txtSearch.Focus();
            txtSearch.SelectAll();
        }
        private void SetupDateTimePickers()
        {
            dtpTuNgay.Format = DateTimePickerFormat.Custom; 
            dtpTuNgay.CustomFormat = "dd/MM/yyyy";

            dtpDenNgay.Format = DateTimePickerFormat.Custom; 
            dtpDenNgay.CustomFormat = "dd/MM/yyyy";

            dtpTuNgay.Value = new DateTime(DateTime.Now.Year, 1, 1);
            dtpDenNgay.Value = DateTime.Now;
        }

        private void HighlightButton(Button btn)
        {
            btnTatCa.BackColor = Color.White; btnTatCa.ForeColor = Color.Black;
            btnChuaThanhToan.BackColor = Color.White; btnChuaThanhToan.ForeColor = Color.Black;
            btnDaThanhToan.BackColor = Color.White; btnDaThanhToan.ForeColor = Color.Black;
            btn.BackColor = Color.MediumSlateBlue; btn.ForeColor = Color.White;
        }


        private void SearchInput_Changed(object sender, EventArgs e) => LoadDataAsync();
        private void Filter_Changed(object sender, EventArgs e) => LoadDataAsync();


        
        #endregion
        #region Phân trang
        private void SetupPaginationUI()
        {
            pnlPagination = new Panel { Dock = DockStyle.Bottom, Height = 60, BackColor = Color.White, Padding = new Padding(10) };

            var table = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 1 };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            var centerFlow = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight, WrapContents = false };
            flowPageNumbers = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight, WrapContents = false };

            btnFirst = CreateNavButton("«", async () => await GoToPage(1));
            btnPrev = CreateNavButton("❮", async () => await GoToPage(_currentPage - 1));
            btnNext = CreateNavButton("❯", async () => await GoToPage(_currentPage + 1));
            btnLast = CreateNavButton("»", async () => {
                int total = (int)Math.Ceiling((double)_totalRecords / _pageSize);
                await GoToPage(total);
            });

            centerFlow.Controls.AddRange(new Control[] { btnFirst, btnPrev, flowPageNumbers, btnNext, btnLast });
            table.Controls.Add(centerFlow, 1, 0);
            pnlPagination.Controls.Add(table);

            this.Controls.Add(pnlPagination);
            pnlPagination.BringToFront();

            pnlListHoaDon.BringToFront();
        }
        private void RenderPagination()
        {
            flowPageNumbers.SuspendLayout();
            flowPageNumbers.Controls.Clear();
            int totalPages = (int)Math.Ceiling((double)_totalRecords / _pageSize);
            if (totalPages == 0) totalPages = 1;

            btnFirst.Enabled = btnPrev.Enabled = (_currentPage > 1);
            btnNext.Enabled = btnLast.Enabled = (_currentPage < totalPages);
            StyleDisabledButton(btnFirst); StyleDisabledButton(btnPrev);
            StyleDisabledButton(btnNext); StyleDisabledButton(btnLast);

            int delta = 2;
            int left = _currentPage - delta, right = _currentPage + delta;

            // Logic vẽ số trang (rút gọn)
            for (int i = 1; i <= totalPages; i++)
            {
                if (i == 1 || i == totalPages || (i >= left && i <= right))
                {
                    var btn = new Button
                    {
                        Text = i.ToString(),
                        Size = new Size(40, 36),
                        FlatStyle = FlatStyle.Flat,
                        Margin = new Padding(2),
                        Cursor = Cursors.Hand
                    };

                    if (i == _currentPage)
                    {
                        btn.BackColor = Color.FromArgb(99, 102, 241);
                        btn.ForeColor = Color.White;
                        btn.FlatAppearance.BorderSize = 0;
                    }
                    else
                    {
                        btn.BackColor = Color.White;

                        // --- [SỬA QUAN TRỌNG TẠI ĐÂY] ---
                        int pageNum = i; // Tạo biến tạm để lưu giá trị i hiện tại
                        btn.Click += async (s, e) => await GoToPage(pageNum); // Dùng biến tạm pageNum
                                                                              // --------------------------------
                    }
                    flowPageNumbers.Controls.Add(btn);
                }
                else if ((i == left - 1) || (i == right + 1))
                {
                    flowPageNumbers.Controls.Add(new Label
                    {
                        Text = "...",
                        AutoSize = false,
                        Size = new Size(30, 36),
                        TextAlign = ContentAlignment.BottomCenter
                    });
                }
            }
            flowPageNumbers.ResumeLayout();
        }

        private async Task GoToPage(int page)
        {
            int total = (int)Math.Ceiling((double)_totalRecords / _pageSize);
            if (page < 1 || page > (total == 0 ? 1 : total) || page == _currentPage) return;
            _currentPage = page;
            await LoadDataAsync();
        }

        private Button CreateNavButton(string text, Action onClick)
        {
            var btn = new Button { Text = text, Size = new Size(40, 36), FlatStyle = FlatStyle.Flat, BackColor = Color.White, Cursor = Cursors.Hand };
            btn.Click += (s, e) => onClick();
            return btn;
        }

        private void StyleDisabledButton(Button btn)
        {
            btn.BackColor = btn.Enabled ? Color.White : Color.FromArgb(248, 250, 252);
            btn.ForeColor = btn.Enabled ? Color.Black : Color.LightGray;
        }
        #endregion

        private async void btnXuatBaoCao_Click(object sender, EventArgs e)
        {
            // Tạo SaveFileDialog để người dùng chọn nơi lưu
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Workbook|*.xlsx";
            sfd.FileName = $"BaoCaoHoaDon_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor; // Hiển thị con trỏ quay tròn

                    using (var scope = Program.ServiceProvider.CreateScope())
                    {
                        var svc = scope.ServiceProvider.GetRequiredService<HoaDonService>();

                        // 1. LẤY DỮ LIỆU (QUAN TRỌNG)
                        // Ta truyền PageSize = int.MaxValue để lấy TOÀN BỘ dữ liệu thay vì chỉ 1 trang
                        string keyword = txtSearch.Text.Trim();
                        DateTime fromDate = dtpTuNgay.Value.Date;
                        DateTime toDate = dtpDenNgay.Value.Date.AddDays(1).AddSeconds(-1);

                        var result = await svc.GetListHoaDonPagingAsync(
                            keyword, _currentStatusFilter, fromDate, toDate, 1, int.MaxValue
                        );

                        var listData = result.Data;

                        if (listData == null || listData.Count == 0)
                        {
                            MessageBox.Show("Không có dữ liệu nào để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        // 2. TẠO FILE EXCEL BẰNG CLOSEDXML
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("Danh Sách Hóa Đơn");

                            // --- A. TẠO HEADER ---
                            worksheet.Cell(1, 1).Value = "Mã HĐ";
                            worksheet.Cell(1, 2).Value = "Bàn";
                            worksheet.Cell(1, 3).Value = "Khách Hàng";
                            worksheet.Cell(1, 4).Value = "Giờ Vào";
                            worksheet.Cell(1, 5).Value = "Giờ Ra";
                            worksheet.Cell(1, 6).Value = "Tổng Tiền";
                            worksheet.Cell(1, 7).Value = "Trạng Thái";
                            worksheet.Cell(1, 8).Value = "Ghi Chú";

                            // Style cho Header (Nền xanh, Chữ trắng, In đậm)
                            var headerRange = worksheet.Range("A1:H1");
                            headerRange.Style.Font.Bold = true;
                            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#4F46E5"); // Màu xanh Indigo
                            headerRange.Style.Font.FontColor = XLColor.White;
                            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                            // --- B. ĐỔ DỮ LIỆU ---
                            int row = 2;
                            foreach (var item in listData)
                            {
                                worksheet.Cell(row, 1).Value = item.MaHd;
                                worksheet.Cell(row, 2).Value = item.MaBanNavigation.TenBan ?? "Mang về";
                                worksheet.Cell(row, 3).Value = item.MaKhNavigation?.TenKh ?? "Khách lẻ";

                                // Xử lý ngày tháng (để Excel hiểu là Date)
                                if (item.ThoiGianBatDau != null) worksheet.Cell(row, 4).Value = item.ThoiGianBatDau;
                                if (item.ThoiGianKetThuc != null) worksheet.Cell(row, 5).Value = item.ThoiGianKetThuc;

                                worksheet.Cell(row, 6).Value = item.TongTien;
                                worksheet.Cell(row, 7).Value = item.TrangThai;
                                worksheet.Cell(row, 8).Value = item.GhiChu;

                                // Style màu sắc cho trạng thái (Optional)
                                if (item.TrangThai == "Đã thanh toán")
                                    worksheet.Cell(row, 7).Style.Font.FontColor = XLColor.Green;
                                else
                                    worksheet.Cell(row, 7).Style.Font.FontColor = XLColor.Red;

                                row++;
                            }

                            // --- C. FORMAT CỘT ---
                            var dataRange = worksheet.Range(2, 1, row - 1, 8);

                            // Format cột tiền tệ (VNĐ)
                            worksheet.Column(6).Style.NumberFormat.Format = "#,##0";

                            // Format cột ngày giờ
                            worksheet.Column(4).Style.DateFormat.Format = "dd/MM/yyyy HH:mm";
                            worksheet.Column(5).Style.DateFormat.Format = "dd/MM/yyyy HH:mm";

                            // Kẻ khung viền cho toàn bộ bảng
                            dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                            // Tự động căn chỉnh độ rộng cột theo nội dung
                            worksheet.Columns().AdjustToContents();

                            // 3. LƯU FILE
                            workbook.SaveAs(sfd.FileName);
                        }
                    }

                    // Mở file ngay sau khi lưu xong
                    if (MessageBox.Show("Xuất báo cáo thành công! Bạn có muốn mở file ngay không?", "Thành công", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo() { FileName = sfd.FileName, UseShellExecute = true });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Có lỗi xảy ra khi xuất Excel: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

    }
}
