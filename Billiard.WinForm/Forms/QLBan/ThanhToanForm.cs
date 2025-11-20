using Billiard.BLL.Services.HoaDonServices;
using Billiard.BLL.Services.HoaDonServices;
using Billiard.BLL.Services.VietQR;
using Billiard.DAL.Entities;
using System;
using System.Drawing;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;
namespace Billiard.WinForm.Forms.QLBan
{
    public partial class ThanhToanForm : Form
    {
        private readonly ThanhToanService _thanhToanService;
        private readonly VietQRService _vietQRService;
        private readonly int _maHd;
        private ThanhToanInfo _thanhToanInfo;
        private VietqrGiaoDich _qrGiaoDich;
        private Timer _qrCheckTimer;
        private CancellationTokenSource _cts;

        // UI Controls
        private Panel pnlMain;
        private Panel pnlLeft;
        private Panel pnlRight;
        private Label lblTitle;
        private GroupBox grpThongTin;
        private GroupBox grpPhuongThuc;
        private Button btnTienMat;
        private Button btnQRCode;
        private Button btnQuetThe;
        private Panel pnlPaymentDetail;

        public ThanhToanForm(ThanhToanService thanhToanService, VietQRService vietQRService, int maHd)
        {
            _thanhToanService = thanhToanService;
            _vietQRService = vietQRService;
            _maHd = maHd;

            InitializeComponent();
            InitializeCustomUI();
        }

        private void InitializeCustomUI()
        {
            // Form settings
            this.Text = "Thanh toán";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Main panel
            pnlMain = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20)
            };
            this.Controls.Add(pnlMain);

            // Title
            lblTitle = new Label
            {
                Text = "💰 Thanh toán hóa đơn",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(20, 20),
                AutoSize = true
            };
            pnlMain.Controls.Add(lblTitle);

            // Left panel - Thông tin
            pnlLeft = new Panel
            {
                Location = new Point(20, 70),
                Size = new Size(420, 460),
                BackColor = Color.FromArgb(248, 250, 252)
            };
            pnlMain.Controls.Add(pnlLeft);

            grpThongTin = new GroupBox
            {
                Text = "📋 Thông tin thanh toán",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(400, 440),
                ForeColor = Color.FromArgb(30, 41, 59)
            };
            pnlLeft.Controls.Add(grpThongTin);

            // Right panel - Phương thức
            pnlRight = new Panel
            {
                Location = new Point(460, 70),
                Size = new Size(400, 460),
                BackColor = Color.White
            };
            pnlMain.Controls.Add(pnlRight);

            grpPhuongThuc = new GroupBox
            {
                Text = "💳 Chọn phương thức thanh toán",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(0, 0),
                Size = new Size(400, 150),
                ForeColor = Color.FromArgb(30, 41, 59)
            };
            pnlRight.Controls.Add(grpPhuongThuc);

            // Payment method buttons
            btnTienMat = CreatePaymentButton("💵 Tiền mặt", 20, 30, Color.FromArgb(34, 197, 94));
            btnTienMat.Click += BtnTienMat_Click;
            grpPhuongThuc.Controls.Add(btnTienMat);

            btnQRCode = CreatePaymentButton("📱 Mã QR", 200, 30, Color.FromArgb(59, 130, 246));
            btnQRCode.Click += BtnQRCode_Click;
            grpPhuongThuc.Controls.Add(btnQRCode);

            btnQuetThe = CreatePaymentButton("💳 Quẹt thẻ", 20, 80, Color.FromArgb(168, 85, 247));
            btnQuetThe.Click += BtnQuetThe_Click;
            btnQuetThe.Enabled = false; // Đang phát triển
            grpPhuongThuc.Controls.Add(btnQuetThe);

            var lblDevNote = new Label
            {
                Text = "* Quẹt thẻ đang phát triển",
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = Color.Gray,
                Location = new Point(200, 95),
                AutoSize = true
            };
            grpPhuongThuc.Controls.Add(lblDevNote);

            // Payment detail panel
            pnlPaymentDetail = new Panel
            {
                Location = new Point(0, 160),
                Size = new Size(400, 300),
                BackColor = Color.White,
                AutoScroll = true
            };
            pnlRight.Controls.Add(pnlPaymentDetail);
        }

        private Button CreatePaymentButton(string text, int x, int y, Color backColor)
        {
            var btn = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(160, 40),
                BackColor = backColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;

            var hoverColor = Color.FromArgb(
                Math.Max(0, backColor.R - 20),
                Math.Max(0, backColor.G - 20),
                Math.Max(0, backColor.B - 20)
            );

            btn.MouseEnter += (s, e) => btn.BackColor = hoverColor;
            btn.MouseLeave += (s, e) => btn.BackColor = backColor;

            return btn;
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            await LoadThanhToanInfo();
        }

        private async Task LoadThanhToanInfo()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                _thanhToanInfo = await _thanhToanService.TinhToanThanhToan(_maHd);
                if (_thanhToanInfo == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin hóa đơn!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                DisplayThanhToanInfo();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void DisplayThanhToanInfo()
        {
            grpThongTin.Controls.Clear();

            int yPos = 30;

            yPos = AddInfoRow(grpThongTin, "🎯 Bàn:", _thanhToanInfo.TenBan, yPos);
            yPos = AddInfoRow(grpThongTin, "👤 Khách:", _thanhToanInfo.TenKhach, yPos);
            yPos = AddInfoRow(grpThongTin, "🕐 Bắt đầu:",
                _thanhToanInfo.ThoiGianBatDau.ToString("HH:mm dd/MM/yyyy"), yPos);

            var hours = _thanhToanInfo.ThoiLuongPhut / 60;
            var minutes = _thanhToanInfo.ThoiLuongPhut % 60;
            yPos = AddInfoRow(grpThongTin, "⏱️ Thời gian:",
                $"{hours} giờ {minutes} phút", yPos, true, Color.FromArgb(220, 38, 38));

            yPos += 20;
            yPos = AddInfoRow(grpThongTin, "💰 Giá giờ:",
                $"{_thanhToanInfo.GiaGio:N0} đ", yPos);
            yPos = AddInfoRow(grpThongTin, "🎱 Tiền bàn:",
                $"{_thanhToanInfo.TienBan:N0} đ", yPos);
            yPos = AddInfoRow(grpThongTin, "🍴 Dịch vụ:",
                $"{_thanhToanInfo.TienDichVu:N0} đ", yPos);

            if (_thanhToanInfo.GiamGia > 0)
            {
                yPos = AddInfoRow(grpThongTin, "🎁 Giảm giá:",
                    $"-{_thanhToanInfo.GiamGia:N0} đ", yPos, false, Color.FromArgb(34, 197, 94));
            }

            if (_thanhToanInfo.ChenhLech > 0)
            {
                yPos = AddInfoRow(grpThongTin, "Tạm tính:",
                    $"{_thanhToanInfo.TamTinh:N0} đ", yPos, false, Color.Gray);
            }

            // Separator
            var separator = new Panel
            {
                Location = new Point(20, yPos),
                Size = new Size(360, 2),
                BackColor = Color.FromArgb(226, 232, 240)
            };
            grpThongTin.Controls.Add(separator);
            yPos += 20;

            // Total
            var lblTotalLabel = new Label
            {
                Text = "TỔNG CỘNG:",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(20, yPos),
                AutoSize = true
            };
            grpThongTin.Controls.Add(lblTotalLabel);

            var lblTotal = new Label
            {
                Text = $"{_thanhToanInfo.TongTien:N0} đ",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 38, 38),
                AutoSize = false,
                Size = new Size(200, 35),
                Location = new Point(180, yPos - 5),
                TextAlign = ContentAlignment.MiddleRight
            };
            grpThongTin.Controls.Add(lblTotal);
        }

        private int AddInfoRow(GroupBox grp, string label, string value, int yPos,
            bool bold = false, Color? valueColor = null)
        {
            var lblLabel = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(20, yPos),
                AutoSize = true
            };
            grp.Controls.Add(lblLabel);

            var lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 9.5F, bold ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = valueColor ?? Color.FromArgb(30, 41, 59),
                AutoSize = false,
                Size = new Size(200, 25),
                Location = new Point(180, yPos - 2),
                TextAlign = ContentAlignment.MiddleRight
            };
            grp.Controls.Add(lblValue);

            return yPos + 30;
        }

        #region Payment Methods

        private void BtnTienMat_Click(object sender, EventArgs e)
        {
            ShowTienMatPanel();
        }

        private async void BtnQRCode_Click(object sender, EventArgs e)
        {
            await ShowQRCodePanel();
        }

        private void BtnQuetThe_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng quẹt thẻ đang được phát triển.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region Tiền mặt Panel

        private void ShowTienMatPanel()
        {
            pnlPaymentDetail.Controls.Clear();

            var pnlTienMat = new Panel
            {
                Location = new Point(10, 10),
                Size = new Size(380, 280),
                BackColor = Color.FromArgb(240, 253, 244)
            };

            var lblTitle = new Label
            {
                Text = "💵 Thanh toán tiền mặt",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(15, 15),
                AutoSize = true
            };
            pnlTienMat.Controls.Add(lblTitle);

            var lblTongTien = new Label
            {
                Text = $"Tổng tiền: {_thanhToanInfo.TongTien:N0} đ",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 38, 38),
                Location = new Point(15, 55),
                AutoSize = true
            };
            pnlTienMat.Controls.Add(lblTongTien);

            var lblKhachDua = new Label
            {
                Text = "Tiền khách đưa:",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(15, 95),
                AutoSize = true
            };
            pnlTienMat.Controls.Add(lblKhachDua);

            var lblTienThua = new Label
            {
                Text = "Tiền thừa: 0 đ",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(34, 197, 94),
                Location = new Point(15, 165),
                AutoSize = true
            };
            pnlTienMat.Controls.Add(lblTienThua);

            var txtTienKhachDua = new TextBox
            {
                Font = new Font("Segoe UI", 12F),
                Location = new Point(15, 120),
                Size = new Size(350, 35),
                Text = _thanhToanInfo.TongTien.ToString("N0")
            };
            txtTienKhachDua.TextChanged += (s, e) => UpdateTienThua(txtTienKhachDua, lblTienThua);
            pnlTienMat.Controls.Add(txtTienKhachDua);

            var btnXacNhan = new Button
            {
                Text = "✓ Xác nhận thanh toán",
                Location = new Point(15, 210),
                Size = new Size(350, 45),
                BackColor = Color.FromArgb(34, 197, 94),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnXacNhan.FlatAppearance.BorderSize = 0;
            btnXacNhan.Click += async (s, e) => await XacNhanThanhToanTienMat(txtTienKhachDua.Text);
            pnlTienMat.Controls.Add(btnXacNhan);

            pnlPaymentDetail.Controls.Add(pnlTienMat);
        }
        private void UpdateTienThua(TextBox txtKhachDua, Label lblTienThua)
        {
            try
            {
                var tienKhachDua = decimal.Parse(txtKhachDua.Text.Replace(",", "").Replace(".", ""));
                var tienThua = tienKhachDua - _thanhToanInfo.TongTien;

                lblTienThua.Text = tienThua >= 0
                    ? $"Tiền thừa: {tienThua:N0} đ"
                    : $"Còn thiếu: {Math.Abs(tienThua):N0} đ";

                lblTienThua.ForeColor = tienThua >= 0
                    ? Color.FromArgb(34, 197, 94)
                    : Color.FromArgb(220, 38, 38);
            }
            catch
            {
                lblTienThua.Text = "Nhập số tiền hợp lệ";
                lblTienThua.ForeColor = Color.Gray;
            }
        }

        private async Task XacNhanThanhToanTienMat(string tienKhachDuaText)
        {
            try
            {
                var tienKhachDua = decimal.Parse(tienKhachDuaText.Replace(",", "").Replace(".", ""));

                if (tienKhachDua < _thanhToanInfo.TongTien)
                {
                    MessageBox.Show("Tiền khách đưa không đủ!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                var result = await _thanhToanService.ThanhToanTienMat(_maHd, tienKhachDua);

                this.Cursor = Cursors.Default;

                if (result.IsSuccess)
                {
                    var tienThua = tienKhachDua - _thanhToanInfo.TongTien;
                    MessageBox.Show(
                        $"✓ Thanh toán thành công!\n\n" +
                        $"Tổng tiền: {_thanhToanInfo.TongTien:N0} đ\n" +
                        $"Khách đưa: {tienKhachDua:N0} đ\n" +
                        $"Tiền thừa: {tienThua:N0} đ",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(result.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region QR Code Panel

        private async Task ShowQRCodePanel()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                pnlPaymentDetail.Controls.Clear();

                // Tạo mã QR
                _qrGiaoDich = await _vietQRService.TaoMaQRThanhToan(_maHd, _thanhToanInfo.TongTien);

                var pnlQR = new Panel
                {
                    Location = new Point(10, 10),
                    Size = new Size(380, 280),
                    BackColor = Color.FromArgb(239, 246, 255),
                    AutoScroll = true
                };

                var lblTitle = new Label
                {
                    Text = "📱 Quét mã QR để thanh toán",
                    Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(30, 41, 59),
                    Location = new Point(15, 15),
                    Size = new Size(350, 25),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                pnlQR.Controls.Add(lblTitle);

                // QR Code Image
                var picQR = new PictureBox
                {
                    Location = new Point(90, 50),
                    Size = new Size(200, 200),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BorderStyle = BorderStyle.FixedSingle
                };

                // Load QR image từ URL
                await LoadQRImage(picQR, _qrGiaoDich.QrCodeUrl);
                pnlQR.Controls.Add(picQR);

                var lblAmount = new Label
                {
                    Text = $"Số tiền: {_thanhToanInfo.TongTien:N0} đ",
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(220, 38, 38),
                    Location = new Point(15, 260),
                    Size = new Size(350, 25),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                pnlQR.Controls.Add(lblAmount);

                var lblMaGD = new Label
                {
                    Text = $"Mã GD: {_qrGiaoDich.MaGiaoDich}",
                    Font = new Font("Segoe UI", 8F),
                    ForeColor = Color.Gray,
                    Location = new Point(15, 285),
                    Size = new Size(350, 20),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                pnlQR.Controls.Add(lblMaGD);

                var lblStatus = new Label
                {
                    Name = "lblQRStatus",
                    Text = "⏳ Đang chờ thanh toán...",
                    Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                    ForeColor = Color.FromArgb(234, 179, 8),
                    Location = new Point(15, 310),
                    Size = new Size(350, 25),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                pnlQR.Controls.Add(lblStatus);

                var btnXacNhan = new Button
                {
                    Text = "✓ Đã thanh toán - Xác nhận",
                    Location = new Point(15, 345),
                    Size = new Size(350, 40),
                    BackColor = Color.FromArgb(34, 197, 94),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btnXacNhan.FlatAppearance.BorderSize = 0;
                btnXacNhan.Click += async (s, e) => await XacNhanThanhToanQR();
                pnlQR.Controls.Add(btnXacNhan);

                var btnHuy = new Button
                {
                    Text = "✕ Hủy",
                    Location = new Point(15, 395),
                    Size = new Size(350, 35),
                    BackColor = Color.FromArgb(148, 163, 184),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 9F),
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btnHuy.FlatAppearance.BorderSize = 0;
                btnHuy.Click += (s, e) => pnlPaymentDetail.Controls.Clear();
                pnlQR.Controls.Add(btnHuy);

                pnlPaymentDetail.Controls.Add(pnlQR);
                this.Cursor = Cursors.Default;

                // Bắt đầu kiểm tra trạng thái thanh toán
                StartQRStatusCheck(lblStatus);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Lỗi tạo mã QR: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadQRImage(PictureBox picBox, string url)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var imageBytes = await httpClient.GetByteArrayAsync(url);
                    using (var ms = new System.IO.MemoryStream(imageBytes))
                    {
                        picBox.Image = Image.FromStream(ms);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi load QR image: {ex.Message}");

                // Hiển thị placeholder nếu không load được
                picBox.BackColor = Color.FromArgb(226, 232, 240);
                var lblError = new Label
                {
                    Text = "⚠️\nKhông tải được mã QR\nVui lòng thử lại",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = Color.Gray
                };
                picBox.Controls.Add(lblError);
            }
        }

        private void StartQRStatusCheck(Label lblStatus)
        {
            _cts = new CancellationTokenSource();

            _qrCheckTimer = new Timer();
            _qrCheckTimer.Interval = 3000; // Check mỗi 3 giây
            _qrCheckTimer.Tick += async (s, e) =>
            {
                if (_cts.Token.IsCancellationRequested)
                {
                    _qrCheckTimer?.Stop();
                    return;
                }

                var isPaid = await _vietQRService.KiemTraThanhToan(_qrGiaoDich.MaGiaoDich);
                if (isPaid)
                {
                    _qrCheckTimer?.Stop();
                    lblStatus.Text = "✓ Đã thanh toán thành công!";
                    lblStatus.ForeColor = Color.FromArgb(34, 197, 94);

                    MessageBox.Show("Đã nhận được thanh toán!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    await XacNhanThanhToanQR();
                }
            };
            _qrCheckTimer.Start();
        }

        private async Task XacNhanThanhToanQR()
        {
            try
            {
                // Xác nhận thanh toán thủ công (nếu chưa tự động)
                await _vietQRService.XacNhanThanhToan(_qrGiaoDich.MaGiaoDich);

                this.Cursor = Cursors.WaitCursor;

                var result = await _thanhToanService.ThanhToanQR(_maHd, _qrGiaoDich.MaGiaoDich);

                this.Cursor = Cursors.Default;

                if (result.IsSuccess)
                {
                    MessageBox.Show("✓ Thanh toán QR thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(result.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _cts?.Cancel();
            _qrCheckTimer?.Stop();
            _qrCheckTimer?.Dispose();
            base.OnFormClosing(e);
        }
    }
}