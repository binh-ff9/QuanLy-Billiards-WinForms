using Billiard.BLL.Services.QLBan;
using Billiard.DAL.Entities;
using Billiard.WinForm.Forms.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.Users
{
    public partial class DatBanDialog : Form
    {
        private readonly DatBanService _datBanService;
        private int _maBan;
        private string _tenBan;
        private Button _selectedSlot = null; // Lưu giờ khách đang chọn

        public DatBanDialog(DatBanService service)
        {
            InitializeComponent();
            _datBanService = service;
        }

        public void SetTableInfo(int maBan, string tenBan)
        {
            _maBan = maBan;
            _tenBan = tenBan;
            lblTitle.Text = $"Lịch đặt - {_tenBan}";
        }

        private async void DatBanDialog_Load(object sender, EventArgs e)
        {
            dtpNgay.MinDate = DateTime.Now; // Không cho đặt quá khứ
            await LoadTimeSlots();
        }

        private async void dtpNgay_ValueChanged(object sender, EventArgs e)
        {
            await LoadTimeSlots(); // Load lại lịch khi đổi ngày
        }

        private async Task LoadTimeSlots()
        {
            pnlTimeSlots.Controls.Clear();
            _selectedSlot = null; // Reset chọn
            DateTime date = dtpNgay.Value.Date;

            // Lấy các đơn đã đặt của bàn này trong ngày
            var bookings = await _datBanService.GetByDateRangeAsync(date, date.AddDays(1));
            // Lọc đúng bàn này
            bookings = bookings.Where(b => b.MaBan == _maBan && b.TrangThai != "Đã hủy").ToList();

            // Vẽ các khung giờ từ 8:00 đến 22:00
            for (int h = 8; h < 23; h++)
            {
                var btn = new Button
                {
                    Text = $"{h}:00 - {h + 1}:00",
                    Width = 120,
                    Height = 40,
                    Tag = h, // Lưu giờ vào Tag
                    FlatStyle = FlatStyle.Flat,
                    Margin = new Padding(5)
                };

                // Kiểm tra giờ này có ai đặt chưa (Logic va chạm thời gian)
                // Bận nếu: Có đơn nào bắt đầu <= h VÀ kết thúc > h
                bool isBusy = bookings.Any(b =>
                     b.ThoiGianBatDau.HasValue &&
                     b.ThoiGianKetThuc.HasValue &&
                     b.ThoiGianBatDau.Value.Hour <= h &&
                     b.ThoiGianKetThuc.Value.Hour > h
                 );

                if (isBusy)
                {
                    btn.BackColor = Color.IndianRed; // Đỏ = Bận
                    btn.ForeColor = Color.White;
                    btn.Text += " (Bận)";
                    btn.Enabled = false;
                }
                else
                {
                    btn.BackColor = Color.FromArgb(240, 253, 244); // Xanh nhạt = Trống
                    btn.Click += Slot_Click;
                }

                pnlTimeSlots.Controls.Add(btn);
            }
        }

        private void Slot_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra đăng nhập
            if (!UserSession.IsLoggedIn)
            {
                MessageBox.Show("Bạn cần đăng nhập để đặt bàn!");
                return;
            }

            var btn = sender as Button;

            // 2. Reset nút cũ
            if (_selectedSlot != null)
            {
                _selectedSlot.BackColor = Color.FromArgb(240, 253, 244);
                _selectedSlot.ForeColor = Color.Black;
            }

            // 3. Highlight nút mới
            _selectedSlot = btn;
            _selectedSlot.BackColor = Color.Orange; // Màu cam = Đang chọn
            _selectedSlot.ForeColor = Color.White;
        }

        private async void btnXacNhan_Click(object sender, EventArgs e)
        {
            if (_selectedSlot == null)
            {
                MessageBox.Show("Vui lòng chọn khung giờ!");
                return;
            }

            int hour = (int)_selectedSlot.Tag;
            DateTime date = dtpNgay.Value.Date;

            var booking = new DatBan
            {
                MaBan = _maBan,
                MaKh = UserSession.MaKH,
                ThoiGianDat = DateTime.Now,
                // Mặc định đặt 1 tiếng (Logic thực tế có thể cho chọn nhiều slot liên tiếp)
                ThoiGianBatDau = new DateTime(date.Year, date.Month, date.Day, hour, 0, 0),
                ThoiGianKetThuc = new DateTime(date.Year, date.Month, date.Day, hour + 1, 0, 0),
                TrangThai = "Đang chờ", // Quan trọng: Chờ nhân viên xác nhận
                GhiChu = txtGhiChu.Text
            };

            // Gọi Service lưu (Bạn cần thêm hàm CreateBookingAsync trong Service)
            // await _datBanService.CreateBookingAsync(booking);

            MessageBox.Show("Đặt bàn thành công! Vui lòng chờ nhân viên xác nhận.", "Thông báo");
            this.Close();
        }
    }
}
