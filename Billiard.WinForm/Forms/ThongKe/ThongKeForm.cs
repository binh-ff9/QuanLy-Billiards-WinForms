using Billiard.BLL.Services;

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using WinFormsChart = System.Windows.Forms.DataVisualization.Charting.Chart;
using WinFormsChartArea = System.Windows.Forms.DataVisualization.Charting.ChartArea;
using WinFormsLegend = System.Windows.Forms.DataVisualization.Charting.Legend;
using WinFormsSeries = System.Windows.Forms.DataVisualization.Charting.Series;

namespace Billiard.WinForm.Forms.ThongKe
{
    public partial class ThongKeForm : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private string currentSoSanhType = "ngay";

        public ThongKeForm(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
        }

        private async void ThongKeForm_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeDefaultValues();
                await LoadAllData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}\n\nStack: {ex.StackTrace}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeDefaultValues()
        {
            var now = DateTime.Now;
            dtpTuNgay.Value = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            dtpDenNgay.Value = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);

            cboNam.Items.Clear();
            for (int year = DateTime.Now.Year; year >= DateTime.Now.Year - 5; year--)
            {
                cboNam.Items.Add(year);
            }
            cboNam.SelectedIndex = 0;

            StyleDataGridView();
        }

        private void StyleDataGridView()
        {
            dgvTopKhachHang.EnableHeadersVisualStyles = false;
            dgvTopKhachHang.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(102, 126, 234);
            dgvTopKhachHang.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvTopKhachHang.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvTopKhachHang.ColumnHeadersHeight = 40;
            dgvTopKhachHang.RowTemplate.Height = 35;
            dgvTopKhachHang.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
        }

        private ThongKeService CreateService()
        {
            return _serviceProvider.GetRequiredService<ThongKeService>();
        }

        private async Task LoadAllData()
        {
            await LoadTongQuan();
            await LoadDoanhThu7Ngay();
            await LoadDoanhThuThang();
            await LoadKhungGio();
            await LoadPhuongThuc();

            // Load tab hiện tại
            if (tabControl.SelectedTab == tabDichVu)
            {
                await LoadTopDichVu();
                await LoadLoaiDichVu();
                await LoadLoaiBan();
            }
            else if (tabControl.SelectedTab == tabKhachHang)
            {
                await LoadTopKhachHang();
            }
            else if (tabControl.SelectedTab == tabKhac)
            {
                await LoadSoSanh();
            }
        }

        #region Button Events
        private async void btnXemBaoCao_Click(object sender, EventArgs e)
        {
            await XemBaoCaoAsync();
        }

        private async Task XemBaoCaoAsync()
        {
            if (dtpTuNgay.Value > dtpDenNgay.Value)
            {
                MessageBox.Show("Ngày bắt đầu phải nhỏ hơn ngày kết thúc!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            await LoadTongQuan();
            await LoadKhungGio();
            await LoadPhuongThuc();

            if (tabControl.SelectedTab == tabDichVu)
            {
                await LoadTopDichVu();
                await LoadLoaiDichVu();
                await LoadLoaiBan();
            }
            else if (tabControl.SelectedTab == tabKhachHang)
            {
                await LoadTopKhachHang();
            }
        }

        private async void btnHomNay_Click(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            dtpTuNgay.Value = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            dtpDenNgay.Value = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
            await XemBaoCaoAsync();
        }

        private async void cboNam_SelectedIndexChanged(object sender, EventArgs e)
        {
            await LoadDoanhThuThang();
        }

        private async void btnNgay_Click(object sender, EventArgs e)
        {
            SetSoSanhButtonActive(btnNgay);
            currentSoSanhType = "ngay";
            await LoadSoSanh();
        }

        private async void btnTuan_Click(object sender, EventArgs e)
        {
            SetSoSanhButtonActive(btnTuan);
            currentSoSanhType = "tuan";
            await LoadSoSanh();
        }

        private async void btnThang_Click(object sender, EventArgs e)
        {
            SetSoSanhButtonActive(btnThang);
            currentSoSanhType = "thang";
            await LoadSoSanh();
        }

        private void SetSoSanhButtonActive(Button activeButton)
        {
            foreach (Control ctrl in panelSoSanhHeader.Controls)
            {
                if (ctrl is Button btn && ctrl != activeButton)
                {
                    btn.BackColor = Color.White;
                    btn.ForeColor = Color.Black;
                }
            }
            activeButton.BackColor = Color.FromArgb(102, 126, 234);
            activeButton.ForeColor = Color.White;
        }
        #endregion

        #region Load Data Methods
        private async Task LoadTongQuan()
        {
            try
            {
                using var service = CreateService();
                var data = await service.GetTongQuanAsync(dtpTuNgay.Value, dtpDenNgay.Value);

                lblDoanhThuValue.Text = FormatCurrency(data.TongDoanhThu);
                lblHoaDonValue.Text = data.SoHoaDon.ToString("N0");
                lblKhachHangValue.Text = data.SoKhachHang.ToString("N0");
                lblTrungBinhValue.Text = FormatCurrency(data.DoanhThuTrungBinh);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải tổng quan: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadDoanhThu7Ngay()
        {
            try
            {
                using var service = CreateService();
                var data = await service.GetDoanhThu7NgayAsync();

                var chartData = data.Select(d => Tuple.Create(
                    d.Ngay.ToString("dd/MM"),
                    d.DoanhThu
                )).ToList();

                UpdateBarChart(chartDoanhThu7Ngay, "📈 Doanh thu 7 ngày gần nhất",
                    chartData, Color.FromArgb(102, 126, 234));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải doanh thu 7 ngày: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadDoanhThuThang()
        {
            try
            {
                if (cboNam.SelectedItem == null) return;

                int nam = Convert.ToInt32(cboNam.SelectedItem);
                using var service = CreateService();
                var data = await service.GetDoanhThuTheoThangAsync(nam);

                var chartData = data.Select(d => Tuple.Create(
                    $"T{d.Thang}",
                    d.DoanhThu
                )).ToList();

                UpdateLineChart(chartDoanhThuThang, $"📅 Doanh thu theo tháng năm {nam}",
                    chartData, Color.FromArgb(102, 126, 234));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải doanh thu tháng: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadKhungGio()
        {
            try
            {
                using var service = CreateService();
                var data = await service.GetDoanhThuTheoKhungGioAsync(dtpTuNgay.Value, dtpDenNgay.Value);

                var chartData = data.Select(d => Tuple.Create(d.KhungGio, d.DoanhThu)).ToList();

                UpdateDoughnutChart(chartKhungGio, "🕐 Doanh thu theo khung giờ", chartData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải khung giờ: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadPhuongThuc()
        {
            try
            {
                using var service = CreateService();
                var data = await service.GetPhuongThucThanhToanAsync(dtpTuNgay.Value, dtpDenNgay.Value);

                var chartData = data.Select(d => Tuple.Create(d.PhuongThuc, d.TongTien)).ToList();

                UpdatePieChart(chartPhuongThuc, "💳 Phương thức thanh toán", chartData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải phương thức: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadTopDichVu()
        {
            try
            {
                using var service = CreateService();
                var data = await service.GetTopDichVuAsync(dtpTuNgay.Value, dtpDenNgay.Value);

                var chartData = data.Select(d => Tuple.Create(d.TenDichVu, (decimal)d.SoLuong)).ToList();

                UpdateHorizontalBarChart(chartTopDichVu, "🔥 Top 10 dịch vụ bán chạy nhất",
                    chartData, Color.FromArgb(102, 126, 234));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải top dịch vụ: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadLoaiDichVu()
        {
            try
            {
                using var service = CreateService();
                var data = await service.GetDoanhThuTheoLoaiDichVuAsync(dtpTuNgay.Value, dtpDenNgay.Value);

                var chartData = data.Select(d => Tuple.Create(
                    d.Loai == "DoUong" ? "🍹 Đồ uống" :
                    d.Loai == "DoAn" ? "🍔 Đồ ăn" : "📦 Khác",
                    d.DoanhThu
                )).ToList();

                UpdateDoughnutChart(chartLoaiDichVu, "🍽️ Doanh thu theo loại dịch vụ", chartData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải loại dịch vụ: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadLoaiBan()
        {
            try
            {
                using var service = CreateService();
                var data = await service.GetDoanhThuTheoLoaiBanAsync(dtpTuNgay.Value, dtpDenNgay.Value);

                var chartData = data.Select(d => Tuple.Create(d.LoaiBan, d.DoanhThu)).ToList();

                UpdatePieChart(chartLoaiBan, "🎱 Doanh thu theo loại bàn", chartData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải loại bàn: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadTopKhachHang()
        {
            try
            {
                using var service = CreateService();
                var data = await service.GetTopKhachHangAsync(dtpTuNgay.Value, dtpDenNgay.Value);

                dgvTopKhachHang.Rows.Clear();
                int index = 1;
                foreach (var item in data)
                {
                    dgvTopKhachHang.Rows.Add(
                        index++,
                        item.TenKhachHang,
                        item.SoDienThoai,
                        FormatCurrency(item.TongChiTieu),
                        $"{item.SoLanDen} lần"
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải top khách hàng: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadSoSanh()
        {
            try
            {
                using var service = CreateService();
                var data = await service.GetSoSanhDoanhThuAsync(currentSoSanhType);

                var chenhLech = Math.Abs(data.DoanhThuHienTai - data.DoanhThuTruoc);

                lblKyHienTaiTitle.Text = data.TieuDeHienTai;
                lblKyHienTaiValue.Text = FormatCurrency(data.DoanhThuHienTai);
                lblKyHienTaiPercent.Text = $"{(data.TangTruong >= 0 ? "📈 +" : "📉 ")}{data.TangTruong:F1}%";
                lblKyHienTaiPercent.ForeColor = data.TangTruong >= 0 ?
                    Color.FromArgb(40, 167, 69) : Color.FromArgb(220, 53, 69);

                lblKyTruocTitle.Text = data.TieuDeTruoc;
                lblKyTruocValue.Text = FormatCurrency(data.DoanhThuTruoc);

                lblChenhLechValue.Text = FormatCurrency(chenhLech);
                lblChenhLechPercent.Text = $"{(data.TangTruong >= 0 ? "+" : "")}{data.TangTruong:F1}%";
                lblChenhLechPercent.ForeColor = data.TangTruong >= 0 ?
                    Color.FromArgb(40, 167, 69) : Color.FromArgb(220, 53, 69);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải so sánh: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Chart Helper Methods
        private void UpdateBarChart(WinFormsChart chart, string title, List<Tuple<string, decimal>> data, Color color)
        {
            if (chart == null) return;

            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.Titles.Clear();

            var titleObj = chart.Titles.Add(title);
            titleObj.Font = new Font("Segoe UI", 12F, FontStyle.Bold);

            var chartArea = new WinFormsChartArea();
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.LabelStyle.Format = "#,##0";
            chart.ChartAreas.Add(chartArea);

            var series = new WinFormsSeries
            {
                ChartType = SeriesChartType.Column,
                Color = color,
                BorderWidth = 2
            };

            foreach (var item in data)
            {
                series.Points.AddXY(item.Item1, item.Item2);
            }

            chart.Series.Add(series);
        }

        private void UpdateLineChart(WinFormsChart chart, string title, List<Tuple<string, decimal>> data, Color color)
        {
            if (chart == null) return;

            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.Titles.Clear();

            var titleObj = chart.Titles.Add(title);
            titleObj.Font = new Font("Segoe UI", 12F, FontStyle.Bold);

            var chartArea = new WinFormsChartArea();
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.LabelStyle.Format = "#,##0";
            chart.ChartAreas.Add(chartArea);

            var series = new WinFormsSeries
            {
                ChartType = SeriesChartType.Line,
                Color = color,
                BorderWidth = 3,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 8,
                MarkerColor = color
            };

            foreach (var item in data)
            {
                series.Points.AddXY(item.Item1, item.Item2);
            }

            chart.Series.Add(series);
        }

        private void UpdateHorizontalBarChart(WinFormsChart chart, string title, List<Tuple<string, decimal>> data, Color color)
        {
            if (chart == null) return;

            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.Titles.Clear();

            var titleObj = chart.Titles.Add(title);
            titleObj.Font = new Font("Segoe UI", 12F, FontStyle.Bold);

            var chartArea = new WinFormsChartArea();
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisX.LabelStyle.Format = "#,##0";
            chart.ChartAreas.Add(chartArea);

            var series = new WinFormsSeries
            {
                ChartType = SeriesChartType.Bar,
                Color = color,
                BorderWidth = 2
            };

            foreach (var item in data)
            {
                series.Points.AddXY(item.Item1, item.Item2);
            }

            chart.Series.Add(series);
        }

        private void UpdateDoughnutChart(WinFormsChart chart, string title, List<Tuple<string, decimal>> data)
        {
            if (chart == null) return;

            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.Titles.Clear();
            chart.Legends.Clear();

            var titleObj = chart.Titles.Add(title);
            titleObj.Font = new Font("Segoe UI", 12F, FontStyle.Bold);

            var chartArea = new WinFormsChartArea();
            chart.ChartAreas.Add(chartArea);

            var legend = new WinFormsLegend
            {
                Docking = Docking.Bottom,
                Font = new Font("Segoe UI", 9F)
            };
            chart.Legends.Add(legend);

            var series = new WinFormsSeries
            {
                ChartType = SeriesChartType.Doughnut,
                IsValueShownAsLabel = false
            };

            var colors = new[]
            {
                Color.FromArgb(102, 126, 234),
                Color.FromArgb(118, 75, 162),
                Color.FromArgb(255, 193, 7),
                Color.FromArgb(220, 53, 69)
            };

            int colorIndex = 0;
            decimal total = data.Sum(d => d.Item2);

            foreach (var item in data)
            {
                var point = series.Points.Add((double)item.Item2);
                point.Color = colors[colorIndex % colors.Length];
                point.LegendText = item.Item1;
                point.Label = total > 0 ? $"{(item.Item2 / total * 100):F1}%" : "0%";
                colorIndex++;
            }

            chart.Series.Add(series);
        }

        private void UpdatePieChart(WinFormsChart chart, string title, List<Tuple<string, decimal>> data)
        {
            if (chart == null) return;

            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.Titles.Clear();
            chart.Legends.Clear();

            var titleObj = chart.Titles.Add(title);
            titleObj.Font = new Font("Segoe UI", 12F, FontStyle.Bold);

            var chartArea = new WinFormsChartArea();
            chart.ChartAreas.Add(chartArea);

            var legend = new WinFormsLegend
            {
                Docking = Docking.Bottom,
                Font = new Font("Segoe UI", 9F)
            };
            chart.Legends.Add(legend);

            var series = new WinFormsSeries
            {
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = false
            };

            var colors = new[]
            {
                Color.FromArgb(40, 167, 69),
                Color.FromArgb(23, 162, 184),
                Color.FromArgb(255, 193, 7),
                Color.FromArgb(220, 53, 69),
                Color.FromArgb(102, 126, 234)
            };

            int colorIndex = 0;
            decimal total = data.Sum(d => d.Item2);

            foreach (var item in data)
            {
                var point = series.Points.Add((double)item.Item2);
                point.Color = colors[colorIndex % colors.Length];
                point.LegendText = item.Item1;
                point.Label = total > 0 ? $"{(item.Item2 / total * 100):F1}%" : "0%";
                colorIndex++;
            }

            chart.Series.Add(series);
        }
        #endregion

        #region Helper Methods
        private string FormatCurrency(decimal value)
        {
            return value.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " đ";
        }
        #endregion

        #region Tab Events
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
        }

        private async void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (tabControl.SelectedTab == tabDichVu)
                {
                    await LoadTopDichVu();
                    await LoadLoaiDichVu();
                    await LoadLoaiBan();
                }
                else if (tabControl.SelectedTab == tabKhachHang)
                {
                    await LoadTopKhachHang();
                }
                else if (tabControl.SelectedTab == tabKhac)
                {
                    await LoadSoSanh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chuyển tab: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        private void panelDoanhThu7Ngay_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}