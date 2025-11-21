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
        private DateTimePicker dtpTime; // Ô chọn giờ phút chi tiết
        private NumericUpDown nudHour;   // Chọn giờ
        private NumericUpDown nudMinute; // Chọn phút
        private FlowLayoutPanel pnlTimeSlots;
        private TextBox txtGhiChu;
        private Button _selectedSlot = null;

        private const int START_HOUR = 8;
        private const int END_HOUR = 27; // 24 + 3 = 3h sáng hôm sau

        private bool _isUpdatingFromCode = false;

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
            // 1. Cấu hình Form
            this.Size = new Size(600, 750); // Tăng chiều rộng một chút cho thoải mái
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Text = string.IsNullOrEmpty(_tenBan) ? "Đặt bàn" : $"Đặt bàn: {_tenBan}";

            int padding = 25;
            int currentY = 25;

            // 2. DÒNG 1: CHỌN NGÀY & GIỜ BẮT ĐẦU
            // ------------------------------------------------------------
            var lblDate = new Label { Text = "📅 Ngày:", Location = new Point(padding, currentY + 3), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) };

            dtpDate = new DateTimePicker
            {
                Location = new Point(lblDate.Right + 5, currentY),
                Width = 110,
                Format = DateTimePickerFormat.Short,
                MinDate = DateTime.Now,
                Font = new Font("Segoe UI", 10)
            };
            dtpDate.ValueChanged += async (s, e) => await LoadSlots();

            var lblTimeDetail = new Label { Text = "🕒 Bắt đầu:", Location = new Point(dtpDate.Right + 15, currentY + 3), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) };

            dtpTime = new DateTimePicker
            {
                Location = new Point(lblTimeDetail.Right + 5, currentY),
                Width = 70,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "HH:mm",
                ShowUpDown = true,
                Font = new Font("Segoe UI", 10)
            };
            dtpTime.ValueChanged += (s, e) =>
            {
                if (_isUpdatingFromCode) return;

                if (_selectedSlot != null)
                {
                    _selectedSlot.BackColor = Color.White;
                    _selectedSlot.ForeColor = Color.Black;
                    _selectedSlot = null;
                }
            };

            this.Controls.AddRange(new Control[] { lblDate, dtpDate, lblTimeDetail, dtpTime });

            currentY += 50; // Xuống dòng 2

            // 3. DÒNG 2: CHỌN THỜI LƯỢNG (GIỜ + PHÚT)
            // ------------------------------------------------------------
            var lblDuration = new Label { Text = "⏳ Thời gian chơi:", Location = new Point(padding, currentY + 3), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) };

            // Ô nhập Giờ
            nudHour = new NumericUpDown
            {
                Location = new Point(lblDuration.Right + 55, currentY),
                Width = 50,
                Minimum = 0,
                Maximum = 24,
                Value = 1, // Mặc định 1 giờ
                Font = new Font("Segoe UI", 10),
                TextAlign = HorizontalAlignment.Center
            };
            var lblH = new Label { Text = "giờ", Location = new Point(nudHour.Right + 2, currentY + 3), AutoSize = true, Font = new Font("Segoe UI", 10) };

            // Ô nhập Phút
            nudMinute = new NumericUpDown
            {
                Location = new Point(lblH.Right + 10, currentY),
                Width = 50,
                Minimum = 0,
                Maximum = 59,
                Value = 0, // Mặc định 0 phút
                Increment = 15, // Bấm lên xuống sẽ nhảy 15 phút một (tiện lợi)
                Font = new Font("Segoe UI", 10),
                TextAlign = HorizontalAlignment.Center
            };
            var lblM = new Label { Text = "phút", Location = new Point(nudMinute.Right + 2, currentY + 3), AutoSize = true, Font = new Font("Segoe UI", 10) };

            // Reset chọn slot khi đổi thời gian chơi
            EventHandler resetSlot = (s, e) => { _selectedSlot = null; LoadSlots(); };
            nudHour.ValueChanged += resetSlot;
            nudMinute.ValueChanged += resetSlot;

            this.Controls.AddRange(new Control[] { lblDuration, nudHour, lblH, nudMinute, lblM });

            currentY += 50; // Xuống dòng tiếp theo

            CreateLegendItem(padding, currentY, Color.White, "Trống", true);
            CreateLegendItem(padding + 100, currentY, Color.FromArgb(254, 226, 226), "Bận", false);
            CreateLegendItem(padding + 200, currentY, Color.FromArgb(99, 102, 241), "Đang chọn", false);
            currentY += 40;

            var lblTimeGrid = new Label { Text = "Hoặc chọn nhanh khung giờ chẵn:", Location = new Point(padding, currentY), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            this.Controls.Add(lblTimeGrid);
            currentY += 30;

            pnlTimeSlots = new FlowLayoutPanel
            {
                Location = new Point(padding, currentY),
                Size = new Size(530, 300), // Tăng chiều rộng cho khớp Form
                AutoScroll = true,
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(248, 250, 252)
            };
            this.Controls.Add(pnlTimeSlots);
            currentY += 310;

            var lblNote = new Label { Text = "📝 Ghi chú:", Location = new Point(padding, currentY), AutoSize = true, Font = new Font("Segoe UI", 10) };
            this.Controls.Add(lblNote);
            currentY += 25;

            txtGhiChu = new TextBox { Location = new Point(padding, currentY), Size = new Size(530, 50), Multiline = true, BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 10) };
            this.Controls.Add(txtGhiChu);
            currentY += 70;

            var btnConfirm = new Button
            {
                Text = "XÁC NHẬN ĐẶT BÀN",
                Location = new Point(padding, this.ClientSize.Height - 70),
                Size = new Size(530, 50),
                BackColor = Color.FromArgb(99, 102, 241),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnConfirm.FlatAppearance.BorderSize = 0;
            btnConfirm.Click += BtnConfirm_Click;
            this.Controls.Add(btnConfirm);

            this.Load += async (s, e) => await LoadSlots();
        }

        private async Task LoadSlots()
        {
            pnlTimeSlots.Controls.Clear();
            pnlTimeSlots.SuspendLayout(); // Tối ưu hiệu năng vẽ
            _selectedSlot = null;

            DateTime dateBase = dtpDate.Value.Date;

            // Lấy dữ liệu đặt bàn trong 2 ngày (để cover trường hợp qua đêm)
            var bookedList = await _datBanService.GetByDateRangeAsync(dateBase, dateBase.AddDays(2));
            var bookingsOfTable = bookedList.Where(b => b.MaBan == _maBan && b.TrangThai != "Đã hủy").ToList();

            // Vòng lặp từ 8h sáng -> 27h (tức 3h sáng hôm sau)
            for (int i = START_HOUR; i < END_HOUR; i++)
            {
                // Tính toán giờ thực tế (0-23h)
                int displayHour = i % 24;

                // Tính toán ngày thực tế (Nếu i >= 24 tức là đã sang ngày hôm sau)
                DateTime slotDate = (i >= 24) ? dateBase.AddDays(1) : dateBase;

                DateTime slotStart = new DateTime(slotDate.Year, slotDate.Month, slotDate.Day, displayHour, 0, 0);
                DateTime slotEnd = slotStart.AddHours(1); // Slot cơ bản là 1 tiếng để hiển thị

                var btn = new Button
                {
                    Text = $"{displayHour}:00", // Chỉ hiện giờ bắt đầu cho gọn
                    Width = 90,
                    Height = 45,
                    Tag = slotStart, // Lưu DateTime thực tế vào Tag
                    Margin = new Padding(5),
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10)
                };
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = Color.LightGray;

                // Kiểm tra slot này có bị ai đặt chưa
                bool isBusy = bookingsOfTable.Any(b =>
                    b.ThoiGianBatDau < slotEnd && b.ThoiGianKetThuc > slotStart
                );

                if (isBusy)
                {
                    btn.BackColor = Color.FromArgb(254, 226, 226); // Đỏ nhạt
                    btn.ForeColor = Color.Red;
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

            pnlTimeSlots.ResumeLayout();
        }

        private void Slot_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;

            // 1. Reset nút cũ
            if (_selectedSlot != null)
            {
                _selectedSlot.BackColor = Color.White;
                _selectedSlot.ForeColor = Color.Black;
            }

            // 2. Highlight nút mới
            _selectedSlot = btn;
            _selectedSlot.BackColor = Color.FromArgb(99, 102, 241); // Màu tím
            _selectedSlot.ForeColor = Color.White;

            // 3. Cập nhật dtpTime (SỬA LỖI TẠI ĐÂY)
            if (btn.Tag is DateTime slotTime) // Phải ép kiểu sang DateTime
            {
                _isUpdatingFromCode = true; // Bật cờ: "Đừng reset nút của tao"

                // Chỉ lấy giờ phút từ nút bấm, ngày giữ nguyên theo dtpDate
                // (Phòng hờ trường hợp qua đêm ngày mai)
                dtpTime.Value = slotTime;

                _isUpdatingFromCode = false; // Tắt cờ
            }
        }

        private async void BtnConfirm_Click(object sender, EventArgs e)
        {
            // 1. Lấy ngày giờ bắt đầu
            DateTime selectedDate = dtpDate.Value.Date;
            DateTime selectedTime = dtpTime.Value;

            DateTime startTime = new DateTime(
                selectedDate.Year, selectedDate.Month, selectedDate.Day,
                selectedTime.Hour, selectedTime.Minute, 0);

            // Xử lý qua đêm (nếu < 8h sáng thì là hôm sau)
            if (startTime.Hour < 8) startTime = startTime.AddDays(1);

            if (startTime < DateTime.Now)
            {
                MessageBox.Show("Không thể đặt lùi thời gian!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (startTime < DateTime.Now.AddMinutes(30))
            {
                MessageBox.Show("Vui lòng đặt trước ít nhất 30 phút để quán kịp chuẩn bị!",
                    "Quy định đặt bàn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // 2. TÍNH GIỜ KẾT THÚC (MỚI)
            int hours = (int)nudHour.Value;
            int minutes = (int)nudMinute.Value;

            // Validate: Không cho đặt 0 giờ 0 phút
            if (hours == 0 && minutes == 0)
            {
                MessageBox.Show("Vui lòng nhập thời gian chơi (tối thiểu 15 phút)!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime endTime = startTime.AddHours(hours).AddMinutes(minutes);

            // 3. KIỂM TRA TRÙNG LỊCH (Giữ nguyên)
            bool isBusy = await _datBanService.IsTableReservedAsync(_maBan, startTime, endTime);

            if (isBusy)
            {
                MessageBox.Show($"Khoảng thời gian từ {startTime:HH:mm} đến {endTime:HH:mm} đã bị vướng lịch.\nVui lòng chọn giờ khác.",
                    "Trùng lịch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                await LoadSlots();
                return;
            }

            // 4. Lưu Booking (Giữ nguyên logic cũ, chỉ thay endTime)
            var booking = new DatBan
            {
                MaBan = _maBan,
                MaKh = UserSession.MaKH > 0 ? UserSession.MaKH : (int?)null,
                TenKhach = UserSession.TenKH,
                Sdt = UserSession.Sdt,
                ThoiGianDat = DateTime.Now,

                ThoiGianBatDau = startTime,
                ThoiGianKetThuc = endTime, // Đã tính chính xác từng phút

                TrangThai = "Đang chờ",
                GhiChu = txtGhiChu.Text.Trim()
            };

            try
            {
                await _datBanService.AddAsync(booking);
                MessageBox.Show("Đặt bàn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                MessageBox.Show("Lỗi: " + innerMessage);
            }
        }

        private void CreateLegendItem(int x, int y, Color color, string text, bool hasBorder)
        {
            // Tạo ô màu
            var pnlColor = new Panel
            {
                Location = new Point(x, y + 3), // Căn giữa dòng với chữ
                Size = new Size(20, 20),
                BackColor = color,
                BorderStyle = hasBorder ? BorderStyle.FixedSingle : BorderStyle.None // Nếu màu trắng thì cần viền
            };

            // Tạo chữ
            var lblText = new Label
            {
                Text = text,
                Location = new Point(x + 25, y + 3),
                AutoSize = true,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray
            };

            this.Controls.Add(pnlColor);
            this.Controls.Add(lblText);
        }
    }
}
