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

        // Controls
        private DateTimePicker dtpDate;
        private FlowLayoutPanel pnlTimeSlots;
        private TextBox txtGhiChu;

        private Button _selectedSlot = null;

        public DatBanDialog(DatBanService service)
        {
            InitializeComponent();
            _datBanService = service;
            SetupUI();
        }

        public void SetTableInfo(int maBan, string tenBan)
        {
            _maBan = maBan;
            _tenBan = tenBan;
            this.Text = $"Đặt bàn: {_tenBan}";
        }

        private void SetupUI()
        {
            this.Size = new Size(500, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // 1. Chọn ngày
            var lblDate = new Label { Text = "Chọn ngày:", Location = new Point(20, 20), AutoSize = true };
            dtpDate = new DateTimePicker { Location = new Point(100, 15), Width = 200, Format = DateTimePickerFormat.Short, MinDate = DateTime.Now };
            dtpDate.ValueChanged += async (s, e) => await LoadSlots();

            // 2. Panel chứa các nút giờ
            var lblTime = new Label { Text = "Chọn khung giờ:", Location = new Point(20, 60), AutoSize = true };
            pnlTimeSlots = new FlowLayoutPanel { Location = new Point(20, 90), Size = new Size(440, 300), AutoScroll = true, BorderStyle = BorderStyle.FixedSingle };

            // 3. Ghi chú
            var lblNote = new Label { Text = "Ghi chú:", Location = new Point(20, 410), AutoSize = true };
            txtGhiChu = new TextBox { Location = new Point(20, 435), Size = new Size(440, 60), Multiline = true };

            // 4. Nút Xác nhận
            var btnConfirm = new Button
            {
                Text = "XÁC NHẬN ĐẶT BÀN",
                Location = new Point(20, 510),
                Size = new Size(440, 40),
                BackColor = Color.FromArgb(99, 102, 241),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnConfirm.Click += BtnConfirm_Click;

            this.Controls.AddRange(new Control[] { lblDate, dtpDate, lblTime, pnlTimeSlots, lblNote, txtGhiChu, btnConfirm });

            this.Load += async (s, e) => await LoadSlots();
        }

        private async Task LoadSlots()
        {
            pnlTimeSlots.Controls.Clear();
            _selectedSlot = null;

            DateTime date = dtpDate.Value.Date;

            // Lấy các đơn đã đặt trong ngày
            var bookedList = await _datBanService.GetByDateRangeAsync(date, date.AddDays(1));
            // Lọc đúng bàn này và chưa hủy
            var bookingsOfTable = bookedList.Where(b => b.MaBan == _maBan && b.TrangThai != "Đã hủy").ToList();

            // Tạo slot từ 9h đến 22h
            for (int h = 9; h < 22; h++)
            {
                var btn = new Button
                {
                    Text = $"{h}:00 - {h + 1}:00",
                    Width = 100,
                    Height = 40,
                    Tag = h,
                    Margin = new Padding(5)
                };

                // Kiểm tra trùng giờ
                // Logic: Có đơn nào bắt đầu trước khi slot kết thúc VÀ kết thúc sau khi slot bắt đầu
                DateTime slotStart = date.AddHours(h);
                DateTime slotEnd = date.AddHours(h + 1);

                bool isBusy = bookingsOfTable.Any(b => b.ThoiGianBatDau < slotEnd && b.ThoiGianKetThuc > slotStart);

                if (isBusy)
                {
                    btn.BackColor = Color.LightCoral; // Đỏ nhạt
                    btn.Text += "\n(Bận)";
                    btn.Enabled = false;
                }
                else
                {
                    btn.BackColor = Color.White;
                    btn.Click += Slot_Click;
                }

                pnlTimeSlots.Controls.Add(btn);
            }
        }

        private void Slot_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;

            // Reset nút cũ
            if (_selectedSlot != null) _selectedSlot.BackColor = Color.White;

            // Highlight nút mới
            _selectedSlot = btn;
            _selectedSlot.BackColor = Color.LightGreen;
        }

        private async void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (_selectedSlot == null)
            {
                MessageBox.Show("Vui lòng chọn khung giờ!");
                return;
            }

            int hour = (int)_selectedSlot.Tag;
            DateTime date = dtpDate.Value.Date;

            var booking = new DatBan
            {
                MaBan = _maBan,
                MaKh = UserSession.MaKH > 0 ? UserSession.MaKH : (int?)null,
                TenKhach = string.IsNullOrEmpty(UserSession.TenKH) ? "Khách vãng lai" : UserSession.TenKH,
                Sdt = UserSession.Sdt,       // Dự phòng
                ThoiGianDat = DateTime.Now,
                ThoiGianBatDau = date.AddHours(hour),
                ThoiGianKetThuc = date.AddHours(hour + 1), // Mặc định 1 tiếng
                TrangThai = "Đang chờ",
                GhiChu = txtGhiChu.Text.Trim()
            };

            // Kiểm tra lại lần cuối ở Server (tránh 2 người đặt cùng lúc)
            bool isConflict = await _datBanService.IsTableReservedAsync(_maBan, booking.ThoiGianBatDau ?? DateTime.Now, booking.ThoiGianKetThuc ?? DateTime.Now);

            if (isConflict)
            {
                MessageBox.Show("Rất tiếc, khung giờ này vừa có người đặt mất rồi!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                await LoadSlots(); // Load lại
                return;
            }

            // Lưu
             await _datBanService.AddAsync(booking); 


            // Sau khi lưu thành công:
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
