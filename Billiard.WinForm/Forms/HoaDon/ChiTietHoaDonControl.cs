using Billiard.DAL.Entities;
using Billiard.WinForm.Helpers; // Để dùng InvoicePrinter nếu có
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.HoaDon
{
    public partial class ChiTietHoaDonControl : UserControl
    {
        private Billiard.DAL.Entities.HoaDon _currentHoaDon;

        // Controls giao diện
        private FlowLayoutPanel pnlContainer;
        private Button btnClose;
        private DataGridView dgvChiTiet; // Grid hiển thị món ăn/dịch vụ
        private Label lblTongTien; // Label tổng tiền to

        // Sự kiện đóng
        public event EventHandler OnCloseClick;

        public ChiTietHoaDonControl()
        {
            InitializeComponent();
            this.BackColor = Color.White;
            SetupLayout();
            SetupResponsiveLayout(); 
            this.Size = new Size(550, 650);
            this.AutoSize = true;
        }

        private void SetupLayout()
        {
            this.Controls.Clear();

            // --- 0. Tổng kết (Cuộn dọc) ---
            var pnlTotalFixed = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.White,
                Padding = new Padding(20, 5, 20, 5)
            };
            // Vẽ đường kẻ trên
            pnlTotalFixed.Paint += (s, e) => e.Graphics.DrawLine(new Pen(Color.WhiteSmoke, 2), 0, 0, pnlTotalFixed.Width, 0);

            this.Controls.Add(pnlTotalFixed);

            this.Tag = pnlTotalFixed;

            // --- 1. FOOTER (Nút In hóa đơn) ---
            var pnlFooter = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                Padding = new Padding(20, 5, 20, 0),
                BackColor = Color.White
            };
            // Vẽ đường kẻ trên footer
            pnlFooter.Paint += (s, e) => e.Graphics.DrawLine(new Pen(Color.WhiteSmoke, 2), 0, 0, pnlFooter.Width, 0);

            var btnPrint = new Button
            {
                Text = "🖨️ In Hóa Đơn",
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(99, 102, 241), // Tím Indigo
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnPrint.FlatAppearance.BorderSize = 0;
            btnPrint.Click += btnInHoaDon_Click; // Gắn sự kiện in
            pnlFooter.Controls.Add(btnPrint);
            this.Controls.Add(pnlFooter);

            // --- 2. CONTAINER CHÍNH (Cuộn dọc) ---
            pnlContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(20),
                BackColor = Color.White
            };
            // Hack full width cho con
            pnlContainer.SizeChanged += (s, e) => {
                foreach (Control c in pnlContainer.Controls) c.Width = pnlContainer.ClientSize.Width - 40;
            };
            this.Controls.Add(pnlContainer);


            // --- 3. NÚT ĐÓNG (Overlay) ---
            btnClose = new Button
            {
                Text = "✕",
                Font = new Font("Segoe UI", 14, FontStyle.Regular),
                Size = new Size(40, 40),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(this.Width - 45, 5),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = Color.Gray,
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(254, 202, 202);
            btnClose.FlatAppearance.MouseDownBackColor = Color.FromArgb(239, 68, 68);
            btnClose.Click += (s, e) => OnCloseClick?.Invoke(this, EventArgs.Empty); // Kích hoạt sự kiện đóng
            this.Controls.Add(btnClose);
            btnClose.BringToFront();
        }

        private void SetupResponsiveLayout()
        {
            // Khi kích thước khung chứa thay đổi -> Gọi hàm tính toán lại
            pnlContainer.SizeChanged += (s, e) =>
            {
                pnlContainer.SuspendLayout(); // 1. Tạm dừng vẽ để tránh giật lag

                // 2. Tính toán chiều rộng thực tế khả dụng
                // ClientSize.Width là chiều rộng bên trong (không tính viền)
                // Trừ đi Padding trái phải của chính nó (đang set là 20)
                int realWidth = pnlContainer.ClientSize.Width - pnlContainer.Padding.Left - pnlContainer.Padding.Right;

                // 3. Nếu thanh cuộn dọc đang hiện, trừ thêm chiều rộng thanh cuộn để không bị che nội dung
                if (pnlContainer.VerticalScroll.Visible)
                {
                    realWidth -= SystemInformation.VerticalScrollBarWidth;
                }

                // 4. Ép tất cả các con phải rộng bằng chiều rộng thực tế này
                foreach (Control c in pnlContainer.Controls)
                {
                    // Trừ thêm 2-3px margin an toàn để không bị sát mép quá
                    c.Width = realWidth - 2;
                }

                pnlContainer.ResumeLayout(true); // 5. Vẽ lại
            };
        }


        public void LoadData(Billiard.DAL.Entities.HoaDon hd)
        {
            _currentHoaDon = hd;
            pnlContainer.Controls.Clear();

            // --- 1. HEADER (Mã HĐ + Trạng thái) ---
            var pnlHeader = new Panel { Width = 500, Height = 95, Margin = new Padding(0, 0, 0, 10) };

            var lblMaHD = new Label
            {
                Text = $"#{hd.MaHd}",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = true,
                Location = new Point(0, 10)
            };

            // Badge Trạng thái
            string statusText = hd.TrangThai ?? "Đang chơi";
            Color statusColor = (statusText == "Đã thanh toán") ? Color.FromArgb(22, 163, 74) : Color.FromArgb(220, 38, 38);
            Color statusBg = (statusText == "Đã thanh toán") ? Color.FromArgb(220, 252, 231) : Color.FromArgb(254, 226, 226);

            var lblStatus = new Label
            {
                Text = statusText.ToUpper(),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = statusColor,
                BackColor = statusBg,
                AutoSize = true,
                Padding = new Padding(8, 4, 8, 4),
                Location = new Point(0, lblMaHD.Bottom + 35)
            };

            // ✅ CẬP NHẬT: Hiển thị thời gian vào - ra - thanh toán
            var lblDateInfo = new Label
            {
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                AutoSize = true,
                TextAlign = ContentAlignment.TopRight,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            // ✅ XÂY DỰNG CHUỖI THỜI GIAN
            string timeInfo = "";

            // Giờ vào
            if (hd.ThoiGianBatDau.HasValue)
            {
                timeInfo += $"🕐 Vào:  {hd.ThoiGianBatDau.Value:HH:mm dd/MM/yyyy}\n";
            }

            // Giờ ra
            if (hd.ThoiGianKetThuc.HasValue)
            {
                timeInfo += $"🕐 Ra:   {hd.ThoiGianKetThuc.Value:HH:mm dd/MM/yyyy}\n";
            }

            // ✅ Thời gian thanh toán/in hóa đơn
            if (hd.ThoiGianThanhToan.HasValue)
            {
                timeInfo += $"🖨️ In:    {hd.ThoiGianThanhToan.Value:HH:mm dd/MM/yyyy}";
            }
            else if (hd.TrangThai == "Đã thanh toán")
            {
                // Nếu đã thanh toán nhưng chưa có thời gian in, hiển thị "Chưa in"
                timeInfo += $"🖨️ In:    Chưa in hóa đơn";
            }

            lblDateInfo.Text = timeInfo.TrimEnd();
            lblDateInfo.Location = new Point(pnlHeader.Width - 220, 15);

            pnlHeader.Controls.AddRange(new Control[] { lblMaHD, lblStatus, lblDateInfo });
            pnlContainer.Controls.Add(pnlHeader);

            // --- 2. THÔNG TIN CHÍNH (Bàn, Khách) ---
            var pnlInfo = new TableLayoutPanel
            {
                Height = 70,
                Width = 550,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0, 0, 0, 20),
                BackColor = Color.FromArgb(248, 250, 252),
                Location = new Point(pnlContainer.Location.X, 0)
            };
            pnlInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            pnlInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            // Cột trái: Bàn
            string tenBan = hd.MaBanNavigation?.TenBan ?? "Mang về";
            string giaGio = $"{hd.MaBanNavigation?.MaLoaiNavigation?.GiaGio:N0}/h";
            pnlInfo.Controls.Add(CreateInfoItem("🎱 Bàn chơi", $"{tenBan} ({giaGio})"), 0, 0);

            // Cột phải: Khách
            string tenKhach = hd.MaKhNavigation?.TenKh ?? "Khách vãng lai";
            pnlInfo.Controls.Add(CreateInfoItem("👤 Khách hàng", tenKhach), 1, 0);

            pnlContainer.Controls.Add(pnlInfo);

            // --- 3. DANH SÁCH DỊCH VỤ (GRID) ---
            var lblTitleDichVu = new Label
            {
                Text = "Chi tiết dịch vụ",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 10)
            };
            pnlContainer.Controls.Add(lblTitleDichVu);

            dgvChiTiet = new DataGridView();
            SetupBeautifulGrid(dgvChiTiet);

            // Lấy dữ liệu chi tiết
            var listMon = hd.ChiTietHoaDons.Select(ct => new
            {
                TenDichVu = ct.MaDvNavigation?.TenDv ?? "Dịch vụ",
                SoLuong = ct.SoLuong,
                DonGia = ct.MaDvNavigation?.Gia,
                ThanhTien = ct.ThanhTien
            }).ToList();

            dgvChiTiet.DataSource = listMon;
            dgvChiTiet.AutoGenerateColumns = true;

            dgvChiTiet.DataBindingComplete += (s, ev) =>
            {
                FormatGridColumns(dgvChiTiet);
            };

            dgvChiTiet.Width = pnlContainer.ClientSize.Width - 40;
            const int MAX_GRID_HEIGHT = 195;
            int rowHeight = dgvChiTiet.RowTemplate.Height;
            int headerHeight = dgvChiTiet.ColumnHeadersHeight;
            int calculatedHeight = headerHeight + (listMon.Count * rowHeight) + 2;

            dgvChiTiet.Height = Math.Min(calculatedHeight, MAX_GRID_HEIGHT);
            dgvChiTiet.ScrollBars = ScrollBars.Vertical;

            pnlContainer.Controls.Add(dgvChiTiet);

            // --- 4. TIỀN BÀN ---
            var pnlTienBan = new TableLayoutPanel
            {
                Height = 60,
                Width = pnlContainer.ClientSize.Width - 40,
                Margin = new Padding(0, 20, 0, 0),
                BackColor = Color.FromArgb(248, 250, 252),
                Padding = new Padding(15, 10, 15, 10),
                ColumnCount = 2,
                RowCount = 1
            };
            pnlTienBan.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            pnlTienBan.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));

            // Tính tiền bàn
            decimal tienBan = 0;
            string thongTinGio = "";

            if (hd.ThoiGianBatDau.HasValue && hd.ThoiGianKetThuc.HasValue)
            {
                TimeSpan thoiGianChoi = hd.ThoiGianKetThuc.Value - hd.ThoiGianBatDau.Value;
                double soGio = thoiGianChoi.TotalHours;
                decimal _giaGio = hd.MaBanNavigation?.MaLoaiNavigation?.GiaGio ?? 0;
                tienBan = (decimal)soGio * _giaGio;

                int gio = (int)soGio;
                int phut = (int)((soGio - gio) * 60);
                thongTinGio = $"{gio}h {phut}ph × {_giaGio:N0}đ/h";
            }
            else if (hd.ThoiGianBatDau.HasValue)
            {
                TimeSpan thoiGianChoi = DateTime.Now - hd.ThoiGianBatDau.Value;
                double soGio = thoiGianChoi.TotalHours;
                decimal _giaGio = hd.MaBanNavigation?.MaLoaiNavigation?.GiaGio ?? 0;
                tienBan = (decimal)soGio * _giaGio;

                int gio = (int)soGio;
                int phut = (int)((soGio - gio) * 60);
                thongTinGio = $"{gio}h {phut}ph × {_giaGio:N0}đ/h (đang chơi)";
            }

            var lblTienBanInfo = new Label
            {
                Text = $"🎱 Tiền bàn  •  {thongTinGio}",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(71, 85, 105),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true
            };

            var lblSoTienBan = new Label
            {
                Text = $"{tienBan:N0} đ",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(99, 102, 241),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight
            };

            pnlTienBan.Controls.Add(lblTienBanInfo, 0, 0);
            pnlTienBan.Controls.Add(lblSoTienBan, 1, 0);

            pnlContainer.Controls.Add(pnlTienBan);

            // --- 5. TỔNG KẾT TIỀN ---
            var pnlTotalFixed = this.Tag as Panel;
            if (pnlTotalFixed != null)
            {
                pnlTotalFixed.Controls.Clear();
                var lblTongCongTitle = new Label
                {
                    Text = "TỔNG CỘNG",
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    ForeColor = Color.Gray,
                    Location = new Point(0, 8),
                    TextAlign = ContentAlignment.MiddleRight,
                    AutoSize = true
                };

                lblTongTien = new Label
                {
                    Text = $"{hd.TongTien:N0} đ",
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    ForeColor = Color.FromArgb(220, 38, 38),
                    Dock = DockStyle.Right,
                    TextAlign = ContentAlignment.MiddleRight,
                    AutoSize = true
                };

                pnlTotalFixed.Controls.Add(lblTongCongTitle);
                pnlTotalFixed.Controls.Add(lblTongTien);
            }
        }
        private Control CreateInfoItem(string label, string value)
        {
            var pnl = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15, 10, 0, 0) };
            var lblL = new Label { Text = label, Font = new Font("Segoe UI", 9), ForeColor = Color.Gray, Dock = DockStyle.Top };
            var lblV = new Label { Text = value, Font = new Font("Segoe UI", 11, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59), Dock = DockStyle.Top, Height = 30 };
            pnl.Controls.Add(lblV);
            pnl.Controls.Add(lblL);
            return pnl;
        }

        private void SetupBeautifulGrid(DataGridView dgv)
        {
            // 1. Reset giao diện bảng
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // Chỉ kẻ ngang
            dgv.GridColor = Color.FromArgb(241, 245, 249); // Kẻ mờ

            // 2. HEADER (Quan trọng nhất để mất màu xanh mặc định)
            dgv.EnableHeadersVisualStyles = false; // [QUAN TRỌNG] Phải set false mới đổi màu được
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersHeight = 45;

            var headerStyle = dgvChiTiet.ColumnHeadersDefaultCellStyle;
            headerStyle.BackColor = Color.FromArgb(248, 250, 252); // Nền xám nhạt
            headerStyle.ForeColor = Color.FromArgb(100, 116, 139); // Chữ xám đậm
            headerStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            headerStyle.SelectionBackColor = Color.FromArgb(248, 250, 252); // Không đổi màu khi chọn header

            // 3. ROW
            dgv.RowTemplate.Height = 45;
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(51, 65, 85);
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10f);
            dgv.DefaultCellStyle.Padding = new Padding(10, 0, 0, 0); // Cách lề chữ

            // Màu khi chọn dòng (Xanh nhạt thay vì xanh đậm mặc định)
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(238, 242, 255);
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(99, 102, 241);

            // Cấu hình cột
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.RowHeadersVisible = false;
        }

        private void FormatGridColumns(DataGridView dgv)
        {
            if (dgv.Columns["TenDichVu"] != null)
            {
                dgv.Columns["TenDichVu"].HeaderText = "Tên Dịch Vụ";
                dgv.Columns["TenDichVu"].FillWeight = 39;
            }
            if (dgv.Columns["SoLuong"] != null)
            {
                dgv.Columns["SoLuong"].HeaderText = "SL";
                dgv.Columns["SoLuong"].FillWeight = 10;
                dgv.Columns["SoLuong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.Columns["SoLuong"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgv.Columns["DonGia"] != null)
            {
                dgv.Columns["DonGia"].HeaderText = "Đơn Giá";
                dgv.Columns["DonGia"].FillWeight = 24; 
                dgv.Columns["DonGia"].DefaultCellStyle.Format = "N0";
                dgv.Columns["DonGia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["DonGia"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (dgv.Columns["ThanhTien"] != null)
            {
                dgv.Columns["ThanhTien"].HeaderText = "Thành Tiền";
                dgv.Columns["ThanhTien"].FillWeight = 27;
                dgv.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
                dgv.Columns["ThanhTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["ThanhTien"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["ThanhTien"].DefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            }
        }

        private void btnInHoaDon_Click(object sender, EventArgs e)
        {
            if (_currentHoaDon == null) { MessageBox.Show("Không có dữ liệu!"); return; }
            try
            {
                var printer = new InvoicePrinter();
                printer.PrintInvoice(_currentHoaDon, "CLB BI-A PRO VIP", "123 Lê Văn Việt, Thủ Đức");
            }
            catch (Exception ex) { MessageBox.Show("Lỗi in: " + ex.Message); }
        }
    }
}