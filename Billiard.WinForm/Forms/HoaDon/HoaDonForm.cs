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
using System.Drawing;

namespace Billiard.WinForm.Forms.HoaDon
{
    public partial class HoaDonForm : Form
    {
        private readonly HoaDonService _hoaDonService;
        public HoaDonForm(HoaDonService hoaDonService)
        {
            InitializeComponent();
            _hoaDonService = hoaDonService;
        }

        private async void HoaDonForm_Load(object sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                var danhSachHoaDon = await _hoaDonService.GetTatCaHoaDonAsync();

                var displayData = danhSachHoaDon.Select(h => new
                {
                    h.MaHd,
                    Ban = h.MaBanNavigation?.TenBan ?? "N/A",
                    MaNvNavigation = h.MaNvNavigation?.TenNv ?? "N/A",
                    MaKhNavigation = h.MaKhNavigation?.TenKh ?? "Vãng lai",
                    BatDau = h.ThoiGianBatDau,
                    KetThuc = h.ThoiGianKetThuc,
                    TongTien = h.TongTien,
                    TrangThai = h.TrangThai

                }).ToList();

                dataGridViewHoaDon.DataSource = displayData;

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


        // Cấu hình bảng
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
    }
}
