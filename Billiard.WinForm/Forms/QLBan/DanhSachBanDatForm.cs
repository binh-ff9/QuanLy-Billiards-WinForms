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

        private const int WarningMinutes = 30;

        public DanhSachBanDatForm(DatBanService datBanService, BanBiaService banBiaService, MainForm mainForm)
        {
            _datBanService = datBanService;
            _banBiaService = banBiaService;
            _mainForm = mainForm;
            InitializeComponent();
            this.Text = "Danh sách bàn đặt";

            dgvDatBan.CellFormatting += dgvDatBan_CellFormatting;
        }

        private async void DanhSachBanDatForm_Load(object sender, EventArgs e)
        {
            pnlFilter.Height = 10;
            await LoadAllActiveDatBanAsync();
        }

        private void btnCalendarView_Click(object sender, EventArgs e)
        {
            LichDatBanForm lichForm = new LichDatBanForm(_datBanService, _banBiaService, _mainForm);
            lichForm.ShowDialog();
            _ = LoadAllActiveDatBanAsync();
        }

        private async Task LoadAllActiveDatBanAsync()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

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
                var thoiGianKetThucCell = row.Cells["ThoiGianKetThuc"].Value;
                var trangThaiCell = row.Cells["TrangThai"].Value;

                if (thoiGianBatDauCell != null && thoiGianBatDauCell is DateTime thoiGianBatDau &&
                    thoiGianKetThucCell != null && thoiGianKetThucCell is DateTime thoiGianKetThuc)
                {
                    // Kiểm tra nếu đặt bàn đã qua (quá khứ)
                    if (thoiGianKetThuc < DateTime.Now)
                    {
                        // Màu xám cho quá khứ
                        row.DefaultCellStyle.BackColor = Color.FromArgb(229, 231, 235);
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(107, 114, 128);
                        row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(209, 213, 219);
                        row.DefaultCellStyle.SelectionForeColor = Color.FromArgb(75, 85, 99);
                        return;
                    }

                    // Đặt bàn hiện tại hoặc tương lai
                    var timeUntilStart = thoiGianBatDau - DateTime.Now;

                    if (trangThaiCell != null && trangThaiCell.ToString() == "Đang chờ")
                    {
                        if (timeUntilStart.TotalMinutes <= WarningMinutes && timeUntilStart.TotalMinutes > 0)
                        {
                            // Sắp đến giờ: Tô màu vàng
                            row.DefaultCellStyle.BackColor = Color.FromArgb(254, 249, 195);
                            row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(253, 224, 71);
                        }
                        else if (timeUntilStart.TotalMinutes <= 0)
                        {
                            // Quá giờ đặt: Tô màu hồng
                            row.DefaultCellStyle.BackColor = Color.FromArgb(254, 202, 202);
                            row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(252, 165, 165);
                        }
                        else
                        {
                            // Bình thường: Màu trắng
                            row.DefaultCellStyle.BackColor = dgvDatBan.DefaultCellStyle.BackColor;
                            row.DefaultCellStyle.SelectionBackColor = dgvDatBan.DefaultCellStyle.SelectionBackColor;
                        }
                    }
                    else if (trangThaiCell != null && trangThaiCell.ToString() == "Đã đặt")
                    {
                        // Đã xác nhận: Tô màu xanh lá
                        row.DefaultCellStyle.BackColor = Color.FromArgb(187, 247, 208);
                        row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(134, 239, 172);
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
            var thoiGianKetThuc = (DateTime)dgvDatBan.Rows[e.RowIndex].Cells["ThoiGianKetThuc"].Value;

            // Kiểm tra nếu là đặt bàn quá khứ
            bool isPast = thoiGianKetThuc < DateTime.Now;

            if (isPast)
            {
                MessageBox.Show(
                    "Không thể thao tác với đặt bàn đã kết thúc (quá khứ).",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            // ✅ FIX 3: Kiểm tra trạng thái bàn thực tế để không cho phép hủy khi đang chơi
            var datBan = await _datBanService.GetByIdAsync(maDat);
            if (datBan != null && datBan.MaBan.HasValue)
            {
                var ban = await _banBiaService.GetTableByIdAsync(datBan.MaBan.Value);

                // ✅ NẾU BÀN ĐANG CHƠI → KHÔNG CHO PHÉP HỦY
                if (dgvDatBan.Columns[e.ColumnIndex].Name == "Cancel" && ban != null && ban.TrangThai == "Đang chơi")
                {
                    MessageBox.Show(
                        $"Không thể hủy đặt bàn khi bàn đang chơi!\n\n" +
                        $"Bàn {tenBan} hiện tại đang ở trạng thái: {ban.TrangThai}\n" +
                        $"Vui lòng thanh toán hoặc kết thúc ca chơi trước.",
                        "Không thể hủy",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }
            }

            // Xử lý nút "Xác nhận" (Actions)
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
                            var datBanUpdate = await _datBanService.GetByIdAsync(maDat);
                            if (datBanUpdate != null)
                            {
                                if (datBanUpdate.MaBan.HasValue)
                                {
                                    var ban = await _banBiaService.GetTableByIdAsync(datBanUpdate.MaBan.Value);
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