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


namespace Billiard.WinForm.Forms.HoaDon
{
    public partial class HoaDonForm : Form
    {
        private readonly HoaDonService _hoaDonService;

        private List<dynamic> _originalData = new List<dynamic>();

        private string _currentStatusFilter = "Tất cả";

        private MainForm _mainForm;

        public HoaDonForm(HoaDonService hoaDonService)
        {
            InitializeComponent();
            _hoaDonService = hoaDonService;

            dataGridViewHoaDon.CellFormatting += DataGridViewHoaDon_CellFormatting;

            txtSearch.TextChanged += SearchInput_Changed;
            dtpTuNgay.ValueChanged += Filter_Changed;
            dtpDenNgay.ValueChanged += Filter_Changed;
            btnTatCa.Click += (s, e) => SetStatusFilter("Tất cả", btnTatCa);
            btnChuaThanhToan.Click += (s, e) => SetStatusFilter("Đang chơi", btnChuaThanhToan);
            btnDaThanhToan.Click += (s, e) => SetStatusFilter("Đã thanh toán", btnDaThanhToan);
            btnXuatBaoCao.Click += btnXuatBaoCao_Click;
        }

        private async void HoaDonForm_Load(object sender, EventArgs e)
        {
            SetupDateTimePickers();
            await LoadDataAsync();

            HighlightButton(btnTatCa);
        }


        public void SetMainForm(MainForm main)
        {
            _mainForm = main;
        }

        private async Task LoadDataAsync()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                var rawData = await _hoaDonService.GetTatCaHoaDonAsync();

                _originalData = rawData.Select(h => new
                {
                    MaHoaDon = h.MaHd,
                    Ban = h.MaBanNavigation?.TenBan ?? "N/A",
                    NhanVien = h.MaNvNavigation?.TenNv ?? "N/A",
                    KhachHang = h.MaKhNavigation?.TenKh ?? "Vãng lai",
                    SDT = h.MaKhNavigation?.Sdt ?? "",
                    NgayTao = h.ThoiGianBatDau,
                    BatDau = h.ThoiGianBatDau,
                    KetThuc = h.ThoiGianKetThuc,
                    TongTien = h.TongTien,
                    TrangThai = h.TrangThai

                }).ToList<dynamic>();

                ApplyFilters();


                ConfigureDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách hóa đơn: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        #region Filters
        private void SetStatusFilter(string status, Button activeBtn)
        {
            _currentStatusFilter = status;
            HighlightButton(activeBtn);
            ApplyFilters();
        }
        private void SetupDateTimePickers()
        {
            // Format: 19/11/2025
            dtpTuNgay.Format = DateTimePickerFormat.Custom;
            dtpTuNgay.CustomFormat = "dd/MM/yyyy";

            dtpDenNgay.Format = DateTimePickerFormat.Custom;
            dtpDenNgay.CustomFormat = "dd/MM/yyyy";

            // Set mặc định: Từ đầu tháng đến hiện tại
            var today = DateTime.Now;
            dtpTuNgay.Value = new DateTime(today.Year, 1, 1);
            dtpDenNgay.Value = today;
        }

        private void HighlightButton(Button btn)
        {
            // Reset màu các nút (Giả sử màu gốc là White, màu chọn là Blueviolet)
            btnTatCa.BackColor = Color.White;
            btnTatCa.ForeColor = Color.Black;
            btnChuaThanhToan.BackColor = Color.White;
            btnChuaThanhToan.ForeColor = Color.Black;
            btnDaThanhToan.BackColor = Color.White;
            btnDaThanhToan.ForeColor = Color.Black;

            // Set màu nút đang chọn
            btn.BackColor = Color.MediumSlateBlue; // Màu tím giống trong hình
            btn.ForeColor = Color.White;
        }
        private void SearchInput_Changed(object sender, EventArgs e) => ApplyFilters();
        private void Filter_Changed(object sender, EventArgs e) => ApplyFilters();


        private void ApplyFilters()
        {
            if (_originalData == null || !_originalData.Any()) return;

            var keyword = txtSearch.Text.ToLower().Trim();
            var fromDate = dtpTuNgay.Value.Date; // Lấy 00:00:00
            var toDate = dtpDenNgay.Value.Date.AddDays(1).AddSeconds(-1); // Lấy 23:59:59

            var filteredList = _originalData.Where(item =>
            {
                // A. Lọc theo Trạng thái
                bool matchStatus = _currentStatusFilter == "Tất cả" || item.TrangThai == _currentStatusFilter;

                // B. Lọc theo Ngày (Dựa trên NgayTao/BatDau)
                bool matchDate = false;
                if (item.NgayTao != null)
                {
                    DateTime date = (DateTime)item.NgayTao;
                    matchDate = date >= fromDate && date <= toDate;
                }

                // C. Lọc theo Từ khóa (Tên KH, Mã HĐ, SĐT)
                bool matchSearch = true;
                if (!string.IsNullOrEmpty(keyword))
                {
                    string tenKH = item.KhachHang.ToString().ToLower();
                    string sdt = item.SDT.ToString().ToLower();
                    string maHD = item.MaHoaDon.ToString();

                    matchSearch = tenKH.Contains(keyword) ||
                                  sdt.Contains(keyword) ||
                                  maHD.Contains(keyword);
                }

                return matchStatus && matchDate && matchSearch;
            }).ToList();

            // Đổ dữ liệu đã lọc vào Grid
            dataGridViewHoaDon.DataSource = filteredList;

            // Ẩn các cột phụ không cần thiết (như SDT, NgayTao nếu không muốn hiện)
            if (dataGridViewHoaDon.Columns["SDT"] != null) dataGridViewHoaDon.Columns["SDT"].Visible = false;
            if (dataGridViewHoaDon.Columns["NgayTao"] != null) dataGridViewHoaDon.Columns["NgayTao"].Visible = false;

            ConfigureDataGridView();

        }

        #endregion

        // Cấu hình bảng
        #region CSS bảng
        private void ConfigureDataGridView()
        {
            // --- 1. Cài đặt tổng quan ---
            {
                dataGridViewHoaDon.ReadOnly = true;
                dataGridViewHoaDon.AllowUserToAddRows = false;
                dataGridViewHoaDon.AllowUserToDeleteRows = false;
                dataGridViewHoaDon.AllowUserToResizeRows = false;
                dataGridViewHoaDon.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridViewHoaDon.MultiSelect = false;
                dataGridViewHoaDon.RowHeadersVisible = false; // Ẩn cột header bên trái
                dataGridViewHoaDon.BackgroundColor = Color.White;
                dataGridViewHoaDon.BorderStyle = BorderStyle.None;
            }
            // --- 2. Tùy chỉnh Header ---
            dataGridViewHoaDon.EnableHeadersVisualStyles = false; // Cho phép tùy chỉnh màu header
            dataGridViewHoaDon.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249); // Màu nền xám nhạt
            dataGridViewHoaDon.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(30, 41, 59); // Màu chữ
            dataGridViewHoaDon.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dataGridViewHoaDon.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewHoaDon.ColumnHeadersHeight = 40; // Tăng chiều cao header

            // --- 3. Tùy chỉnh các dòng ---
            dataGridViewHoaDon.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            dataGridViewHoaDon.DefaultCellStyle.SelectionBackColor = Color.FromArgb(99, 102, 241); // Màu tím khi chọn
            dataGridViewHoaDon.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridViewHoaDon.RowTemplate.Height = 35; // Chiều cao mỗi dòng

            // Thêm "Zebra Stripes" (màu xen kẽ)
            dataGridViewHoaDon.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            dataGridViewHoaDon.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(99, 102, 241);
            dataGridViewHoaDon.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;


            // --- 4. Tùy chỉnh từng cột (Quan trọng) ---

            // Đặt lại tên Header
            if (dataGridViewHoaDon.Columns["MaHoaDon"] != null)
            {
                dataGridViewHoaDon.Columns["MaHoaDon"].HeaderText = "Mã HĐ";
                dataGridViewHoaDon.Columns["MaHoaDon"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewHoaDon.Columns["MaHoaDon"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            if (dataGridViewHoaDon.Columns["Ban"] != null)
            {
                dataGridViewHoaDon.Columns["Ban"].HeaderText = "Bàn";
                dataGridViewHoaDon.Columns["Ban"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            if (dataGridViewHoaDon.Columns["NhanVien"] != null)
            {
                dataGridViewHoaDon.Columns["NhanVien"].HeaderText = "Nhân viên";
                dataGridViewHoaDon.Columns["NhanVien"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            if (dataGridViewHoaDon.Columns["KhachHang"] != null)
            {
                dataGridViewHoaDon.Columns["KhachHang"].HeaderText = "Khách hàng";
                dataGridViewHoaDon.Columns["KhachHang"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            if (dataGridViewHoaDon.Columns["BatDau"] != null)
            {
                dataGridViewHoaDon.Columns["BatDau"].HeaderText = "Bắt đầu";
                dataGridViewHoaDon.Columns["BatDau"].DefaultCellStyle.Format = "dd/MM/yy HH:mm";
                dataGridViewHoaDon.Columns["BatDau"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            if (dataGridViewHoaDon.Columns["KetThuc"] != null)
            {
                dataGridViewHoaDon.Columns["KetThuc"].HeaderText = "Kết thúc";
                dataGridViewHoaDon.Columns["KetThuc"].DefaultCellStyle.Format = "dd/MM/yy HH:mm";
                dataGridViewHoaDon.Columns["KetThuc"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            if (dataGridViewHoaDon.Columns["TongTien"] != null)
            {
                dataGridViewHoaDon.Columns["TongTien"].HeaderText = "Tổng tiền";
                dataGridViewHoaDon.Columns["TongTien"].DefaultCellStyle.Format = "N0"; // Định dạng số
                dataGridViewHoaDon.Columns["TongTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; // Căn phải
                dataGridViewHoaDon.Columns["TongTien"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            if (dataGridViewHoaDon.Columns["TrangThai"] != null)
            {
                dataGridViewHoaDon.Columns["TrangThai"].HeaderText = "Trạng thái";
                dataGridViewHoaDon.Columns["TrangThai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewHoaDon.Columns["TrangThai"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        private void DataGridViewHoaDon_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridViewHoaDon.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value != null)
            {
                string status = e.Value.ToString();

                // 2. Xử lý logic màu sắc
                if (status == "Đang chơi") // Hoặc trạng thái chưa thanh toán của bạn
                {
                    e.CellStyle.ForeColor = Color.FromArgb(220, 38, 38); // Màu Đỏ (Red)
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold); // In đậm cho chú ý
                }
                else if (status == "Đã thanh toán")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(22, 163, 74); // Màu Xanh lá (Green)
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                }
                // Các trạng thái khác (nếu có) sẽ dùng màu mặc định
            }
        }
        #endregion

        private void btnXuatBaoCao_Click(object sender, EventArgs e)
        {
            if (dataGridViewHoaDon.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("BaoCaoHoaDon");

                            // Cách 1: Xuất thô từ DataGridView (Nhanh)
                            // worksheet.Cell(1, 1).Value = "Báo Cáo Hóa Đơn";

                            // Cách 2: Lấy từ DataSource để chuẩn dữ liệu hơn (Khuyên dùng)
                            var data = dataGridViewHoaDon.DataSource as System.Collections.IList;

                            // Tạo Header
                            worksheet.Cell(1, 1).Value = "Mã HĐ";
                            worksheet.Cell(1, 2).Value = "Bàn";
                            worksheet.Cell(1, 3).Value = "Khách hàng";
                            worksheet.Cell(1, 4).Value = "Tổng tiền";
                            worksheet.Cell(1, 5).Value = "Trạng thái";
                            worksheet.Cell(1, 6).Value = "Thời gian";

                            // Style Header
                            var headerRange = worksheet.Range("A1:F1");
                            headerRange.Style.Font.Bold = true;
                            headerRange.Style.Fill.BackgroundColor = XLColor.CornflowerBlue;
                            headerRange.Style.Font.FontColor = XLColor.White;

                            // Đổ dữ liệu
                            int row = 2;
                            foreach (dynamic item in data)
                            {
                                worksheet.Cell(row, 1).Value = item.MaHoaDon;
                                worksheet.Cell(row, 2).Value = item.Ban;
                                worksheet.Cell(row, 3).Value = item.KhachHang;
                                worksheet.Cell(row, 4).Value = item.TongTien;
                                worksheet.Cell(row, 5).Value = item.TrangThai;
                                worksheet.Cell(row, 6).Value = item.BatDau;
                                row++;
                            }

                            // Tự động chỉnh độ rộng cột
                            worksheet.Columns().AdjustToContents();

                            workbook.SaveAs(sfd.FileName);
                        }
                        MessageBox.Show("Xuất báo cáo thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async void dataGridViewHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                if (dataGridViewHoaDon.Rows[e.RowIndex].Cells["MaHoaDon"].Value == null) return;

                int maHd = Convert.ToInt32(dataGridViewHoaDon.Rows[e.RowIndex].Cells["MaHoaDon"].Value);

                var fullInfo = await _hoaDonService.GetChiTietHoaDon(maHd);

                if (fullInfo != null && _mainForm != null)
                {
                    var detailControl = new ChiTietHoaDonControl();
                    detailControl.LoadData(fullInfo);

                   
                    _mainForm.UpdateDetailPanel("Chi Tiết hóa đơn", detailControl,450);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Loiox xem chi tietse: " + ex.Message);
            }
         }           
    }
}
