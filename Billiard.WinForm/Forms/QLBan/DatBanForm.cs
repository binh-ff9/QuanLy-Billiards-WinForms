using Billiard.BLL.Services.QLBan;
using Billiard.DAL.Entities;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.QLBan
{
    public partial class DatBanForm : Form
    {
        private readonly BanBiaService _banBiaService;
        private readonly DatBanService _datBanService;
        private BanBium _selectedTable;
        private int? _maKhachHang;
        private Panel _selectedCard;
        private bool _isLoading = false;
        // Biến để ngăn chặn hiển thị cảnh báo liên tục khi các DTP thay đổi
        private bool _isWarningShown = false;

        public DatBanForm(BanBiaService banBiaService, DatBanService datBanService)
        {
            _banBiaService = banBiaService;
            _datBanService = datBanService;
            InitializeComponent();
        }

        private async void DatBanForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Set default datetime
                dtpNgayDat.Value = DateTime.Now;
                dtpGioDat.Value = DateTime.Now.AddHours(1);

                // Cải tiến: Sử dụng CustomFormat để dễ chọn giờ
                dtpGioDat.Format = DateTimePickerFormat.Custom;
                dtpGioDat.CustomFormat = "HH:mm";
                dtpGioDat.ShowUpDown = true;

                // Set giờ kết thúc mặc định sau 2 giờ
                dtpGioKetThuc.Value = dtpGioDat.Value.AddHours(2);
                dtpGioKetThuc.Format = DateTimePickerFormat.Custom;
                dtpGioKetThuc.CustomFormat = "HH:mm";
                dtpGioKetThuc.ShowUpDown = true;

                await LoadAvailableTablesForReservation();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //private bool ValidateTimeRangeAndShowWarning()
        //{
        //    if (_isWarningShown) _isWarningShown = false;

        //    // 1. Tính toán thời gian đặt bàn
        //    var gioBatDau = dtpNgayDat.Value.Date
        //        .AddHours(dtpGioDat.Value.Hour)
        //        .AddMinutes(dtpGioDat.Value.Minute);

        //    var gioKetThuc = dtpNgayDat.Value.Date
        //        .AddHours(dtpGioKetThuc.Value.Hour)
        //        .AddMinutes(dtpGioKetThuc.Value.Minute);

        //    // Xử lý logic đặt qua đêm: Nếu giờ kết thúc < giờ bắt đầu, tự động cộng thêm 1 ngày
        //    if (gioKetThuc < gioBatDau)
        //    {
        //        gioKetThuc = gioKetThuc.AddDays(1);
        //    }

        //    // Khung giờ hoạt động: 8:00 sáng (ngày D) đến 2:00 sáng (ngày D+1)
        //    var thoiDiemBatDauHoatDong = dtpNgayDat.Value.Date.AddHours(8); // 8:00 AM Ngày D
        //    var thoiDiemKetThucHoatDong = dtpNgayDat.Value.Date.AddDays(1).AddHours(2); // 2:00 AM Ngày D+1

        //    // Nếu ngày đặt là ngày D, nhưng giờ đặt nằm trong khoảng 00:00 - 02:00, 
        //    // ta phải xem nó thuộc ngày làm việc của ngày D-1 (tức là 8:00 sáng D-1 đến 2:00 sáng D)
        //    if (dtpNgayDat.Value.Date == DateTime.Now.Date && gioBatDau.Hour >= 0 && gioBatDau.Hour < 2)
        //    {
        //        // Nếu người dùng chọn hôm nay, và chọn giờ 00:00 - 02:00, thì không hợp lệ vì quán đã đóng hoặc sắp đóng.
        //        // Tuy nhiên, logic này phức tạp. Tạm thời dùng: Bắt đầu phải sau hiện tại.

        //        // Nếu ngày đặt là ngày hôm sau và giờ đặt 00:00 - 02:00
        //        // Logic sẽ được đơn giản hóa bằng cách kiểm tra:
        //        // 1. Giờ kết thúc phải > Giờ bắt đầu (đã xử lý cộng ngày)
        //        if (gioKetThuc <= gioBatDau)
        //        {
        //            if (!_isWarningShown)
        //            {
        //                _isWarningShown = true;
        //                MessageBox.Show("Giờ kết thúc phải sau giờ bắt đầu (hoặc sau 00:00 sáng hôm sau)!", "Cảnh báo thời gian",
        //                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            }
        //            return false;
        //        }
        //    }


        //    // Cảnh báo 2: Thời gian đặt bàn phải sau thời gian hiện tại
        //    // Dùng AddMinutes(5) để chặn đặt ngay lập tức
        //    if (gioBatDau <= DateTime.Now.AddMinutes(5))
        //    {
        //        if (dtpNgayDat.Value.Date < DateTime.Now.Date)
        //        {
        //            if (!_isWarningShown)
        //            {
        //                _isWarningShown = true;
        //                MessageBox.Show("Không thể đặt bàn cho ngày trong quá khứ!", "Cảnh báo ngày tháng",
        //                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                dtpNgayDat.Value = DateTime.Now.Date;
        //            }
        //        }
        //        else
        //        {
        //            // Chỉ cảnh báo nếu giờ bắt đầu nằm trong vòng 5 phút so với hiện tại
        //            if (!_isWarningShown)
        //            {
        //                _isWarningShown = true;
        //                MessageBox.Show("Thời gian đặt bàn phải sau thời gian hiện tại!", "Cảnh báo thời gian",
        //                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            }
        //        }
        //        return false;
        //    }

        //    // 3. Ràng buộc khung giờ hoạt động (Đã được kiểm tra kỹ trong BtnXacNhan_Click, nhưng cần kiểm tra ở đây để lọc bàn)
        //    // Ràng buộc 3a: Giờ kết thúc không được sau 2:00 sáng của ngày làm việc
        //    // Ngày làm việc là ngày của gioBatDau.
        //    var endOfDayWork = gioBatDau.Date.AddDays(1).AddHours(2); // 2:00 AM ngày hôm sau

        //    if (gioKetThuc > endOfDayWork)
        //    {
        //        if (!_isWarningShown)
        //        {
        //            _isWarningShown = true;
        //            MessageBox.Show("Giờ kết thúc đặt bàn không được sau 2:00 sáng của ngày làm việc (Ngày bắt đầu)!",
        //                "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        }
        //        return false;
        //    }

        //    return true;
        //}

        private async void DtpDateTime_ValueChanged(object sender, EventArgs e)
        {
            // Tự động set giờ kết thúc sau 2 giờ khi GIỜ BẮT ĐẦU thay đổi
            if (sender == dtpGioDat)
            {
                // Tắt sự kiện để tránh gọi LoadAvailableTablesForReservation 2 lần
                dtpGioKetThuc.ValueChanged -= DtpDateTime_ValueChanged;
                dtpGioKetThuc.Value = dtpGioDat.Value.AddHours(2);
                dtpGioKetThuc.ValueChanged += DtpDateTime_ValueChanged;
            }
        }


        // Phương thức mới để dọn dẹp màn hình hiển thị bàn
        private void ClearTableDisplay()
        {
            if (_selectedCard != null)
            {
                _selectedCard.BackColor = Color.White;
                _selectedCard.Invalidate();
                _selectedCard = null;
                _selectedTable = null;
            }

            flpBanTrong.SuspendLayout();
            flpBanTrong.Controls.Clear();
            ShowEmptyState();
            flpBanTrong.ResumeLayout();

            lblChonBan.Text = "Nhấp vào bàn để chọn";
            lblChonBan.ForeColor = Color.FromArgb(100, 116, 139);
        }

        private async Task LoadAvailableTablesForReservation()
        {
            // Ngăn chặn thao tác thứ hai nếu thao tác trước chưa hoàn thành
            if (_isLoading)
                return;

            _isLoading = true; // Bắt đầu thao tác

            try
            {
                flpBanTrong.SuspendLayout();
                flpBanTrong.Controls.Clear();

                
                var gioBatDau = new DateTime(
                    dtpNgayDat.Value.Year,
                    dtpNgayDat.Value.Month,
                    dtpNgayDat.Value.Day,
                    dtpGioDat.Value.Hour,
                    dtpGioDat.Value.Minute,
                    0
                );

                var gioKetThuc = new DateTime(
                    dtpNgayDat.Value.Year,
                    dtpNgayDat.Value.Month,
                    dtpNgayDat.Value.Day,
                    dtpGioKetThuc.Value.Hour,
                    dtpGioKetThuc.Value.Minute,
                    0
                );

                // Xử lý đặt qua đêm
                if (gioKetThuc <= gioBatDau)
                {
                    gioKetThuc = gioKetThuc.AddDays(1);
                }

                // Xóa chọn bàn cũ khi tải lại danh sách
                if (_selectedCard != null)
                {
                    _selectedCard.BackColor = Color.White;
                    _selectedCard.Invalidate();
                    _selectedCard = null;
                    _selectedTable = null;
                }

                lblChonBan.Text = "Nhấp vào bàn để chọn";
                lblChonBan.ForeColor = Color.FromArgb(100, 116, 139);

                // Gọi Service mới để lấy bàn trống trong khoảng thời gian
                var availableTables = await _datBanService.GetAvailableTablesForReservationAsync(gioBatDau, gioKetThuc);

                // ... (Logic hiển thị bàn trống giữ nguyên) ...
                if (availableTables.Count == 0)
                {
                    ShowEmptyState();
                }
                else
                {
                    foreach (var ban in availableTables)
                    {
                        var card = CreateTableMiniCard(ban);
                        flpBanTrong.Controls.Add(card);
                    }

                    lblChonBan.Text = $"Có {availableTables.Count} bàn trống trong khung giờ đã chọn - Nhấp vào bàn để chọn";
                }

                flpBanTrong.ResumeLayout();
            }
            catch (Exception ex)
            {
                // Hiển thị lỗi nhưng đảm bảo flag được reset
                MessageBox.Show($"Lỗi khi tải danh sách bàn: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isLoading = false; // Kết thúc thao tác (dù thành công hay thất bại)
            }
        }

        private void ShowEmptyState()
        {
            var pnlEmpty = new Panel
            {
                Size = new Size(flpBanTrong.Width - 20, 200),
                BackColor = Color.White
            };

            var lblIcon = new Label
            {
                Text = "😔",
                Font = new Font("Segoe UI", 48F),
                AutoSize = true
            };
            lblIcon.Location = new Point(
                (pnlEmpty.Width - lblIcon.Width) / 2,
                40
            );

            var lblMessage = new Label
            {
                Text = "Không có bàn trống",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                AutoSize = true
            };
            lblMessage.Location = new Point(
                (pnlEmpty.Width - lblMessage.Width) / 2,
                120
            );

            pnlEmpty.Controls.AddRange(new Control[] { lblIcon, lblMessage });
            flpBanTrong.Controls.Add(pnlEmpty);
        }

        private Panel CreateTableMiniCard(BanBium ban)
        {
            var card = new Panel
            {
                Width = 140,
                Height = 180,
                Margin = new Padding(5),
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand,
                Tag = ban,
                BackColor = Color.White
            };

            // Border effect
            card.Paint += (s, e) =>
            {
                var borderColor = card == _selectedCard
                    ? Color.FromArgb(99, 102, 241)
                    : Color.FromArgb(226, 232, 240);
                var borderWidth = card == _selectedCard ? 3 : 1;

                using (var pen = new Pen(borderColor, borderWidth))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                }
            };

            // Image panel
            var pnlImage = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(140, 100),
                BackColor = Color.FromArgb(248, 250, 252)
            };

            // Load image
            if (!string.IsNullOrEmpty(ban.HinhAnh))
            {
                try
                {
                    var projectRoot = Directory.GetParent(Directory.GetParent(Directory.GetParent(
                        Directory.GetParent(Application.StartupPath).FullName).FullName).FullName).FullName;
                    var imagePath = Path.Combine(projectRoot, "Forms", "Resources", "img", "tables", ban.HinhAnh);

                    if (File.Exists(imagePath))
                    {
                        var picTable = new PictureBox
                        {
                            Size = new Size(140, 100),
                            Location = new Point(0, 0),
                            SizeMode = PictureBoxSizeMode.Zoom,
                            BackColor = Color.FromArgb(248, 250, 252)
                        };

                        using (var img = Image.FromFile(imagePath))
                        {
                            picTable.Image = new Bitmap(img);
                        }

                        pnlImage.Controls.Add(picTable);
                    }
                    else
                    {
                        AddDefaultIcon(pnlImage);
                    }
                }
                catch
                {
                    AddDefaultIcon(pnlImage);
                }
            }
            else
            {
                AddDefaultIcon(pnlImage);
            }

            // VIP badge nếu là bàn VIP
            if (ban.MaKhuVucNavigation?.TenKhuVuc == "VIP")
            {
                var lblVIP = new Label
                {
                    Text = "⭐",
                    Font = new Font("Segoe UI", 14F),
                    BackColor = Color.FromArgb(168, 85, 247),
                    ForeColor = Color.White,
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Size = new Size(30, 30),
                    Location = new Point(105, 5)
                };
                pnlImage.Controls.Add(lblVIP);
                lblVIP.BringToFront();
            }

            // Table name
            var lblName = new Label
            {
                Text = ban.TenBan,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 105),
                Size = new Size(140, 30)
            };

            // Table info
            var lblInfo = new Label
            {
                Text = $"{ban.MaKhuVucNavigation?.TenKhuVuc ?? ""}",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 135),
                Size = new Size(140, 20)
            };

            // Price
            var lblPrice = new Label
            {
                Text = $"{ban.MaLoaiNavigation?.GiaGio:N0} đ/h",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(99, 102, 241),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 155),
                Size = new Size(140, 20)
            };

            card.Controls.AddRange(new Control[] { pnlImage, lblName, lblInfo, lblPrice });

            // Click event
            EventHandler clickHandler = (s, e) => SelectTable(card, ban);
            card.Click += clickHandler;
            foreach (Control ctrl in card.Controls)
            {
                ctrl.Click += clickHandler;
                if (ctrl == pnlImage)
                {
                    foreach (Control subCtrl in ctrl.Controls)
                    {
                        subCtrl.Click += clickHandler;
                    }
                }
            }

            // Hover effect
            card.MouseEnter += (s, e) =>
            {
                if (card != _selectedCard)
                {
                    card.BackColor = Color.FromArgb(248, 250, 252);
                }
            };

            card.MouseLeave += (s, e) =>
            {
                if (card != _selectedCard)
                {
                    card.BackColor = Color.White;
                }
            };

            return card;
        }

        private void AddDefaultIcon(Panel pnlImage)
        {
            var lblIcon = new Label
            {
                Text = "🎱",
                Font = new Font("Segoe UI", 40F),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(140, 100),
                BackColor = Color.Transparent
            };
            pnlImage.Controls.Add(lblIcon);
        }

        private void SelectTable(Panel card, BanBium ban)
        {
            // Deselect previous card
            if (_selectedCard != null)
            {
                _selectedCard.BackColor = Color.White;
                _selectedCard.Invalidate(); // Trigger repaint
            }

            // Select new card
            _selectedCard = card;
            _selectedTable = ban;
            card.BackColor = Color.FromArgb(239, 246, 255);
            card.Invalidate(); // Trigger repaint

            // Update label
            lblChonBan.Text = $"✓ Đã chọn: {ban.TenBan} - {ban.MaLoaiNavigation?.TenLoai} - {ban.MaLoaiNavigation?.GiaGio:N0} đ/giờ";
            lblChonBan.ForeColor = Color.FromArgb(99, 102, 241);
        }

        private async void BtnTimKhachHang_Click(object sender, EventArgs e)
        {
            await TimKhachHangAsync();
        }
        private async void TxtSoDienThoai_TextChanged(object sender, EventArgs e)
        {
            if (txtSoDienThoai.Text.Length >= 10 && System.Text.RegularExpressions.Regex.IsMatch(txtSoDienThoai.Text, @"^0\d{9,10}$"))
            {
                await TimKhachHangAsync();
            }
        }

        private async Task TimKhachHangAsync()
        {
            try
            {
                string soDienThoai = txtSoDienThoai.Text.Trim();
                if (string.IsNullOrWhiteSpace(soDienThoai) ||
                    !System.Text.RegularExpressions.Regex.IsMatch(soDienThoai, @"^0\d{9,10}$"))
                {
                    txtTenKhach.Text = "";
                    _maKhachHang = null;
                    return;
                }

                var khachHang = await _datBanService.GetCustomerByPhoneNumberAsync(soDienThoai);

                if (khachHang != null)
                {
                    txtTenKhach.Text = khachHang.TenKh;
                    _maKhachHang = khachHang.MaKh;
                    txtTenKhach.ReadOnly = true;
                    MessageBox.Show($"Đã tìm thấy khách hàng: {khachHang.TenKh}", "Tìm kiếm thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Nếu không tìm thấy, reset MaKhachHang, cho phép nhập tên
                    txtTenKhach.Text = "";
                    txtTenKhach.ReadOnly = false;
                    _maKhachHang = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm khách hàng: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void BtnXacNhan_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Validate Bàn
                if (_selectedTable == null)
                {
                    MessageBox.Show("Vui lòng chọn bàn!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. Validate Khách hàng
                if (string.IsNullOrWhiteSpace(txtTenKhach.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên khách hàng!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenKhach.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtSoDienThoai.Text))
                {
                    MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoDienThoai.Focus();
                    return;
                }

                // Validate số điện thoại
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtSoDienThoai.Text, @"^0\d{9,10}$"))
                {
                    MessageBox.Show("Số điện thoại không hợp lệ!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoDienThoai.Focus();
                    return;
                }

                // 3. Xây dựng và Validate Thời gian
                var gioBatDau = new DateTime(
                    dtpNgayDat.Value.Year,
                    dtpNgayDat.Value.Month,
                    dtpNgayDat.Value.Day,
                    dtpGioDat.Value.Hour,
                    dtpGioDat.Value.Minute,
                    0
                );

                var gioKetThuc = new DateTime(
                    dtpNgayDat.Value.Year,
                    dtpNgayDat.Value.Month,
                    dtpNgayDat.Value.Day,
                    dtpGioKetThuc.Value.Hour,
                    dtpGioKetThuc.Value.Minute,
                    0
                );

                // Nếu giờ kết thúc nhỏ hơn giờ bắt đầu (đặt qua đêm)
                if (gioKetThuc <= gioBatDau)
                {
                    gioKetThuc = gioKetThuc.AddDays(1);
                    if (gioKetThuc <= gioBatDau)
                    {
                        MessageBox.Show("Giờ kết thúc phải sau giờ bắt đầu!", "Thông báo",
                           MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Ràng buộc 3a: Thời gian đặt bàn phải sau thời gian hiện tại (không đặt cho quá khứ)
                // Dùng AddMinutes(5) để đảm bảo không bị lỗi time-sync
                if (gioBatDau <= DateTime.Now.AddMinutes(5))
                {
                    MessageBox.Show("Thời gian đặt bàn phải sau thời gian hiện tại!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Định nghĩa khung giờ hoạt động: 8:00 sáng đến 2:00 sáng hôm sau
                var thoiDiemBatDauHoatDong = new DateTime(gioBatDau.Year, gioBatDau.Month, gioBatDau.Day, 8, 0, 0);
                var thoiDiemKetThucHoatDong = thoiDiemBatDauHoatDong.AddDays(1).AddHours(-6); // 2:00 sáng hôm sau

                // Điều chỉnh thoiDiemBatDauHoatDong nếu gioBatDau nằm trong khoảng 00:00 - 02:00
                if (gioBatDau < thoiDiemBatDauHoatDong && gioBatDau < thoiDiemKetThucHoatDong.AddDays(-1).AddHours(2))
                {
                    thoiDiemBatDauHoatDong = thoiDiemBatDauHoatDong.AddDays(-1);
                }

                // Ràng buộc 3b: Giờ bắt đầu phải nằm trong khung (8:00 - 2:00 sáng hôm sau)
                if (gioBatDau < thoiDiemBatDauHoatDong || gioBatDau >= thoiDiemKetThucHoatDong)
                {
                    MessageBox.Show("Thời gian bắt đầu đặt bàn nằm ngoài khung giờ cho phép (8:00 sáng - 2:00 sáng hôm sau)!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Ràng buộc 3c: Giờ kết thúc không được sau 2:00 sáng hôm sau
                if (gioKetThuc > thoiDiemKetThucHoatDong)
                {
                    MessageBox.Show("Giờ kết thúc đặt bàn không được sau 2:00 sáng hôm sau!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Ràng buộc 3d: Giới hạn đặt bàn mới (trước 1:00 sáng hôm sau)
                var thoiDiemGioiHanDatMoi = thoiDiemBatDauHoatDong.AddDays(1).AddHours(-7); // 1:00 sáng hôm sau

                if (gioBatDau >= thoiDiemGioiHanDatMoi)
                {
                    MessageBox.Show("Hệ thống không cho phép đặt bàn mới sau 1:00 sáng hôm sau!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 4. Kiểm tra lại tính khả dụng của bàn (Phòng trường hợp người khác vừa đặt)
                var isReserved = await _datBanService.IsTableReservedAsync(
                    _selectedTable.MaBan,
                    gioBatDau,
                    gioKetThuc
                );

                if (isReserved)
                {
                    MessageBox.Show("Bàn này đã được đặt trong khoảng thời gian này! (Vui lòng chọn bàn khác hoặc làm mới)", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    await LoadAvailableTablesForReservation(); // Tải lại danh sách
                    return;
                }

                // 5. Thực hiện Đặt bàn
                // Sửa lỗi: Gọi đúng _datBanService và truyền đủ 6 tham số
                var success = await _datBanService.ReserveTableAsync(
                    _selectedTable.MaBan,
                    _maKhachHang,
                    txtTenKhach.Text.Trim(),
                    txtSoDienThoai.Text.Trim(),
                    gioBatDau,
                    gioKetThuc,
                    txtGhiChu.Text.Trim()
                );

                if (success)
                {
                    MessageBox.Show("Đặt bàn thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không thể đặt bàn. Vui lòng thử lại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đặt bàn: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}