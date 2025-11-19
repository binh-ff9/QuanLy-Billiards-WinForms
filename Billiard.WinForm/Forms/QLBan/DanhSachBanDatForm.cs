using Billiard.BLL.Services.QLBan;
using Billiard.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.QLBan
{
    public partial class DanhSachBanDatForm : Form
    {
        private readonly DatBanService _datBanService;
        private readonly BanBiaService _banBiaService;
        private readonly MainForm _mainForm;

        // Định nghĩa thời gian cảnh báo (ví dụ: 30 phút)
        private const int WarningMinutes = 30;

        public DanhSachBanDatForm(DatBanService datBanService, BanBiaService banBiaService, MainForm mainForm)
        {
            _datBanService = datBanService;
            _banBiaService = banBiaService;
            _mainForm = mainForm;
            InitializeComponent();
            this.Text = "Danh sách bàn đặt";

            // Thêm sự kiện CellFormatting cho việc định dạng màu
            dgvDatBan.CellFormatting += dgvDatBan_CellFormatting;
        }

        private async void DanhSachBanDatForm_Load(object sender, EventArgs e)
        {
            pnlFilter.Height = 10;
            await LoadAllActiveDatBanAsync();
        }

        private async Task LoadAllActiveDatBanAsync()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // Lấy tất cả đặt bàn đang chờ và đã đặt, sắp xếp theo ThoiGianBatDau
                var datBans = await _datBanService.GetAllActiveAsync();
                var activeDatBans = datBans.OrderBy(d => d.ThoiGianBatDau).ToList();

                var displayList = activeDatBans.Select(d => new
                {
                    MaDat = d.MaDat,
                    TenKhach = d.TenKhach,
                    Sdt = d.Sdt,
                    TenBan = d.MaBanNavigation?.TenBan ?? "N/A",
                    LoaiBan = d.MaBanNavigation?.MaLoaiNavigation?.TenLoai ?? "N/A",
                    KhuVuc = d.MaBanNavigation?.MaKhuVucNavigation?.TenKhuVuc ?? "N/A",
                    ThoiGianBatDau = d.ThoiGianBatDau,
                    ThoiGianKetThuc = d.ThoiGianKetThuc,
                    TrangThai = d.TrangThai ?? "N/A",
                    GhiChu = d.GhiChu ?? "",
                    NgayTao = d.NgayTao ?? DateTime.MinValue
                }).ToList();

                dgvDatBan.DataSource = displayList;
                dgvDatBan.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách đặt bàn: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void dgvDatBan_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvDatBan.Rows[e.RowIndex].DataBoundItem != null)
            {
                var row = dgvDatBan.Rows[e.RowIndex];
                var thoiGianBatDauCell = row.Cells["ThoiGianBatDau"].Value;
                var trangThaiCell = row.Cells["TrangThai"].Value;

                if (thoiGianBatDauCell != null && thoiGianBatDauCell is DateTime thoiGianBatDau)
                {
                    var timeUntilStart = thoiGianBatDau - DateTime.Now;

                    // Chỉ tô màu cho các đơn "Đang chờ"
                    if (trangThaiCell != null && trangThaiCell.ToString() == "Đang chờ")
                    {
                        if (timeUntilStart.TotalMinutes <= WarningMinutes && timeUntilStart.TotalMinutes > 0)
                        {
                            // Sắp đến giờ: Tô màu cảnh báo (vàng nhạt)
                            row.DefaultCellStyle.BackColor = Color.LightYellow;
                            row.DefaultCellStyle.SelectionBackColor = Color.Yellow;
                        }
                        else if (timeUntilStart.TotalMinutes <= 0)
                        {
                            // Quá giờ đặt: Tô màu khẩn cấp (hồng nhạt)
                            row.DefaultCellStyle.BackColor = Color.LightPink;
                            row.DefaultCellStyle.SelectionBackColor = Color.Pink;
                        }
                        else
                        {
                            // Trở về màu mặc định
                            row.DefaultCellStyle.BackColor = dgvDatBan.DefaultCellStyle.BackColor;
                            row.DefaultCellStyle.SelectionBackColor = dgvDatBan.DefaultCellStyle.SelectionBackColor;
                        }
                    }
                    else if (trangThaiCell != null && trangThaiCell.ToString() == "Đã đặt")
                    {
                        // Tô màu xanh nhạt cho các đơn đã xác nhận
                        row.DefaultCellStyle.BackColor = Color.LightGreen;
                        row.DefaultCellStyle.SelectionBackColor = Color.Green;
                    }
                }
            }
        }

        private async void dgvDatBan_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvDatBan.CurrentRow == null) return;

            var maDat = (int)dgvDatBan.Rows[e.RowIndex].Cells["MaDat"].Value;
            var tenBan = dgvDatBan.Rows[e.RowIndex].Cells["TenBan"].Value?.ToString() ?? "N/A";
            var trangThai = dgvDatBan.Rows[e.RowIndex].Cells["TrangThai"].Value?.ToString() ?? "";

            // Xử lý nút "Xác nhận" (Actions) - CHỈ cho trạng thái "Đang chờ"
            if (dgvDatBan.Columns[e.ColumnIndex].Name == "Actions")
            {
                if (trangThai == "Đang chờ")
                {
                    var result = MessageBox.Show(
                        $"Xác nhận đặt bàn {tenBan}?\n\nBàn sẽ chuyển sang trạng thái 'Đã đặt' và sẵn sàng cho khách.",
                        "Xác nhận đặt bàn",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        var success = await _datBanService.UpdateStatusAsync(maDat, "Đã đặt");

                        if (success)
                        {
                            // Cập nhật trạng thái bàn thành "Đã đặt"
                            var datBan = await _datBanService.GetByIdAsync(maDat);
                            if (datBan != null)
                            {
                                if (datBan.MaBan.HasValue)
                                {
                                    var ban = await _banBiaService.GetTableByIdAsync(datBan.MaBan.Value);
                                    if (ban != null)
                                    {
                                        ban.TrangThai = "Đã đặt";
                                        await _banBiaService.UpdateTableAsync(ban);
                                    }
                                }
                            }

                            MessageBox.Show(
                                $"Đã xác nhận đặt bàn {tenBan}!\nBàn đã chuyển sang trạng thái 'Đã đặt'.",
                                "Thành công",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            await LoadAllActiveDatBanAsync();
                        }
                        else
                        {
                            MessageBox.Show(
                                "Xác nhận thất bại!",
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }
                }
                else if (trangThai == "Đã đặt")
                {
                    // Nút này sẽ BẮT ĐẦU CHƠI cho các đơn đã xác nhận
                    var result = MessageBox.Show(
                        $"Bắt đầu chơi bàn {tenBan}?",
                        "Bắt đầu chơi",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        int maNv = _mainForm?.MaNV ?? 1;
                        var success = await _banBiaService.ConfirmReservationAsync(maDat, maNv);

                        if (success)
                        {
                            MessageBox.Show(
                                $"Đã bắt đầu chơi bàn {tenBan}!",
                                "Thành công",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            await LoadAllActiveDatBanAsync();
                        }
                        else
                        {
                            MessageBox.Show(
                                "Bắt đầu chơi thất bại! (Bàn có thể đang bận hoặc lỗi hệ thống)",
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }
                }
            }
            // Xử lý nút "Hủy đặt" (Cancel)
            else if (dgvDatBan.Columns[e.ColumnIndex].Name == "Cancel")
            {
                var result = MessageBox.Show(
                    $"Bạn có chắc muốn HỦY đặt bàn {tenBan}?",
                    "Xác nhận hủy",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    var success = await _banBiaService.CancelReservationAsync(maDat);

                    if (success)
                    {
                        await _datBanService.UpdateStatusAsync(maDat, "Đã hủy");

                        MessageBox.Show(
                            $"Đã hủy đặt bàn {tenBan}!",
                            "Thành công",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        await LoadAllActiveDatBanAsync();
                    }
                    else
                    {
                        MessageBox.Show(
                            "Hủy đặt bàn thất bại!",
                            "Lỗi",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}