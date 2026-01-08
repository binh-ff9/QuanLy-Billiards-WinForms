using Billiard.DAL.Entities;
using System.Drawing;
using System.Drawing.Drawing2D; // Để vẽ bo tròn
using System.Linq;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.KhachHang
{
    public partial class ChiTietKhachHangControl : UserControl
    {
        private FlowLayoutPanel pnlContainer;
        private int _currentMaKh; // Lưu ID khách hàng đang xem

        public event EventHandler<int> OnEditClick;
        public event EventHandler<int> OnDeleteClick;
        public event EventHandler OnCloseClick;


        private bool _isDeletedUser = false;

        private Button btnDeleteAction;
        private Button btnClose;

        public ChiTietKhachHangControl()
        {
            InitializeComponent();
            this.BackColor = Color.White;
            SetupLayout();
        }

        private void SetupLayout()
        {
            // --- 1. PHẦN CHÂN TRANG (Giữ nguyên như cũ) ---
            var pnlFooter = new Panel { Dock = DockStyle.Bottom, Height = 60, Padding = new Padding(10) };

            var tblButtons = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
            tblButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            var btnEdit = new Button { Text = "✏️ Chỉnh sửa", Dock = DockStyle.Fill, BackColor = Color.FromArgb(234, 179, 8), ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand, Margin = new Padding(0, 0, 5, 0) };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Click += (s, e) => OnEditClick?.Invoke(this, _currentMaKh);

            btnDeleteAction = new Button { Dock = DockStyle.Fill, ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand, Margin = new Padding(5, 0, 0, 0) };
            btnDeleteAction.FlatAppearance.BorderSize = 0;
            btnDeleteAction.Click += (s, e) => OnDeleteClick?.Invoke(this, _currentMaKh);

            tblButtons.Controls.Add(btnEdit, 0, 0);
            tblButtons.Controls.Add(btnDeleteAction, 1, 0);
            pnlFooter.Controls.Add(tblButtons);
            this.Controls.Add(pnlFooter); // Add footer trước


            // --- 2. PHẦN NỘI DUNG CHÍNH (Dock Fill) ---
            pnlContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(20) // Padding đều 4 phía
            };

            // Hack full width
            pnlContainer.SizeChanged += (s, e) => {
                foreach (Control c in pnlContainer.Controls) c.Width = pnlContainer.ClientSize.Width - 40;
            };

            this.Controls.Add(pnlContainer);


            // --- 3. NÚT ĐÓNG (Nằm đè lên trên - Overlay) ---
            btnClose = new Button
            {
                Text = "✕",
                Font = new Font("Segoe UI", 14, FontStyle.Regular), // Font to hơn chút cho dễ bấm
                Size = new Size(40, 40),
                // [QUAN TRỌNG] Neo vào góc trên phải, nhưng không Dock
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(this.Width - 45, 5), // Vị trí cố định ban đầu
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent, // Nền trong suốt hoặc White tùy bạn
                ForeColor = Color.Gray,
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(254, 202, 202);
            btnClose.FlatAppearance.MouseDownBackColor = Color.FromArgb(239, 68, 68);

            btnClose.Click += (s, e) => OnCloseClick?.Invoke(this, EventArgs.Empty);

            this.Controls.Add(btnClose); // Add nút Close sau cùng

            // [CỰC KỲ QUAN TRỌNG] Lệnh này bắt buộc phải có để nút X nổi lên trên FlowLayout
            btnClose.BringToFront();
        }

        public void LoadData(Billiard.DAL.Entities.KhachHang kh)
        {
            _currentMaKh = kh.MaKh; // Lưu ID lại để dùng khi bấm nút Sửa
            _isDeletedUser = !(kh.HoatDong ?? true); // Kiểm tra xem đang hoạt động hay xóa

            if (_isDeletedUser)
            {
                // Đang bị xóa -> Hiện nút KHÔI PHỤC (Màu xanh)
                btnDeleteAction.Text = "♻️ Khôi phục";
                btnDeleteAction.BackColor = Color.FromArgb(34, 197, 94); // Green
            }
            else
            {
                // Đang hoạt động -> Hiện nút XÓA (Màu đỏ)
                btnDeleteAction.Text = "🗑️ Xóa bỏ";
                btnDeleteAction.BackColor = Color.FromArgb(239, 68, 68); // Red
            }


            pnlContainer.Controls.Clear();

            // --- 1. AVATAR & NAME HEADER ---
            var pnlHeader = new Panel { Height = 120, Margin = new Padding(0, 0, 0, 20) };

            // Avatar tròn (Vẽ bằng code)
            var lblAvatar = new Label
            {
                Text = GetInitials(kh.TenKh),
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(80, 80),
                Location = new Point(0, 15)
            };
            lblAvatar.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var brush = new SolidBrush(Color.FromArgb(99, 102, 241))) // Màu tím
                    e.Graphics.FillEllipse(brush, 0, 0, 79, 79);
                TextRenderer.DrawText(e.Graphics, lblAvatar.Text, lblAvatar.Font, new Rectangle(0, 0, 80, 80), Color.White, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
            };

            // Tên & SĐT
            var lblName = new Label 
            { 
                Text = kh.TenKh, 
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59), 
                AutoSize = true, 
                Location = new Point(90, 10)
            };
            var lblPhone = new Label 
            { 
                Text = kh.Sdt, Font = new Font("Segoe UI", 11, FontStyle.Regular), 
                ForeColor = Color.Gray, 
                AutoSize = true, 
                Location = new Point(92, 45) 
            };
            var lblEmail = new Label
            {
                Text = string.IsNullOrEmpty(kh.Email) ? "Chưa cập nhật Email" : kh.Email,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(100, 116, 139), // Màu xám xanh nhẹ
                AutoSize = true,
                Location = new Point(92, 70)
            };


            pnlHeader.Controls.AddRange(new Control[] { lblAvatar, lblName, lblPhone, lblEmail });
            pnlContainer.Controls.Add(pnlHeader);


            // --- 2. THỐNG KÊ (STATS) ---
            decimal tongTien = kh.HoaDons.Sum(h => h.TongTien) ?? 0;
            int soLanDen = kh.HoaDons.Count;

            var pnlStats = new TableLayoutPanel 
            { 
                Height = 80, 
                ColumnCount = 2, 
                RowCount = 1, 
                Margin = new Padding(0, 0, 0, 20) ,

                Width = pnlContainer.ClientSize.Width - 40,
            };
            pnlStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            pnlStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
         
            tongTien = 1222;
            pnlStats.Controls.Add(CreateStatBox("💰 Tổng chi tiêu", $"{tongTien:N0}đ", Color.FromArgb(22, 163, 74)), 0, 0);
            pnlStats.Controls.Add(CreateStatBox("🏆 Số lần đến", $"{soLanDen} lần", Color.FromArgb(234, 179, 8)), 1, 0);

            pnlContainer.Controls.Add(pnlStats);

            // --- 3. LỊCH SỬ GIAO DỊCH (5 Gần nhất) ---

            var pnlHistoryHeader = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0, 0, 0, 10),
                Width = pnlContainer.Width // Đảm bảo rộng bằng container
            };

            var lblHistoryTitle = new Label 
            { 
                Text = "Lịch sử gần đây", 
                Font = new Font("Segoe UI", 12, FontStyle.Bold), 
                ForeColor = Color.Black, 
                AutoSize = true, 
                Margin = new Padding(0, 5, 10, 0)
            };

            var btnViewAll = new Button
            {
                Text = "Xem tất cả", // Bỏ dấu > cho gọn, hoặc để lại tùy bạn
                Font = new Font("Segoe UI", 9, FontStyle.Bold), // Font nhỏ hơn title chút cho tinh tế
                ForeColor = Color.White, // Chữ trắng
                BackColor = Color.FromArgb(59, 130, 246), // Nền xanh dương hiện đại
                Cursor = Cursors.Hand,
                AutoSize = true, // Tự co giãn theo chữ
                AutoSizeMode = AutoSizeMode.GrowAndShrink, // Co vừa khít nội dung
                FlatStyle = FlatStyle.Flat, // Bỏ hiệu ứng 3D cũ kỹ
                Padding = new Padding(10, 5, 10, 5), // Tạo khoảng cách giữa chữ và viền nút
                Margin = new Padding(0, 4, 0, 0) // Căn chỉnh lề trên để thẳng hàng với tiêu đề "Lịch sử..."
            };

            // Xóa viền đen mặc định của nút
            btnViewAll.FlatAppearance.BorderSize = 0;
            // Hiệu ứng khi di chuột vào (đậm hơn chút)
            btnViewAll.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);


            btnViewAll.Click += (s, e) =>
            { // Ý tưởng là ấn vào sẽ lấy id khach hang do rồi chuyển sang tab hóa đơn tự điền số điện thoại và lọc tất cả


                //var historyForm = new LichSuGiaoDichForm(_currentMaKh);
                //historyForm.ShowDialog(); // ShowDialog để hiện dạng popup, người dùng phải tắt form này mới quay lại được
            };

            pnlHistoryHeader.Controls.Add(lblHistoryTitle);
            pnlHistoryHeader.Controls.Add(btnViewAll);
            pnlContainer.Controls.Add(pnlHistoryHeader);

            var recentInvoices = kh.HoaDons.OrderByDescending(h => h.ThoiGianBatDau).Take(5).ToList();
            if (recentInvoices.Count > 0)
            {
                foreach (var hd in recentInvoices)
                {
                    pnlContainer.Controls.Add(CreateHistoryRow(hd));
                }
            }
            else
            {
                var lblEmpty = new Label { Text = "Chưa có giao dịch nào", ForeColor = Color.Gray, AutoSize = true };
                pnlContainer.Controls.Add(lblEmpty);
            }
        }

        // --- CÁC HÀM HỖ TRỢ VẼ GIAO DIỆN ---

        private Panel CreateStatBox(string title, string value, Color color)
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(248, 250, 252),
                Margin = new Padding(5)
            }; // Nền xám nhạt
                                                                                                      // Có thể thêm bo góc ở đây nếu muốn

            var lblVal = new Label { Text = value, Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = color, Dock = DockStyle.Bottom, TextAlign = ContentAlignment.MiddleCenter, Height = 30 };
            var lblTit = new Label { Text = title, Font = new Font("Segoe UI", 9, FontStyle.Regular), ForeColor = Color.Gray, Dock = DockStyle.Top, TextAlign = ContentAlignment.MiddleCenter, Height = 25 };

            pnl.Controls.Add(lblVal);
            pnl.Controls.Add(lblTit);

            return pnl;
        }

        private void AddInfoRow(Panel pnl, string label, string value, int y)
        {
            var lblL = new Label { Text = label, ForeColor = Color.Gray, Location = new Point(0, y), AutoSize = true };
            var lblV = new Label { Text = value, ForeColor = Color.Black, Location = new Point(100, y), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            pnl.Controls.Add(lblL);
            pnl.Controls.Add(lblV);
        }

        private Panel CreateHistoryRow(Billiard.DAL.Entities.HoaDon hd)
        {
            var pnl = new Panel { Height = 50, BackColor = Color.White, Margin = new Padding(0, 0, 0, 5) };
            // Kẻ dưới
            pnl.Paint += (s, e) => e.Graphics.DrawLine(Pens.WhiteSmoke, 0, 49, pnl.Width, 49);

            var date = hd.ThoiGianBatDau?.ToString("dd/MM/yyyy") ?? "";
            var time = hd.ThoiGianBatDau?.ToString("HH:mm") ?? "";

            var lblTime = new Label { Text = $"{date}\n{time}", Font = new Font("Segoe UI", 8), ForeColor = Color.Gray, AutoSize = true, Location = new Point(0, 8) };
            var lblBan = new Label { Text = hd.MaBanNavigation?.TenBan ?? "Bàn ?", Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59), Location = new Point(80, 12), AutoSize = true };
            var lblTien = new Label { Text = $"+{hd.TongTien:N0}đ", Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Green, Dock = DockStyle.Right, TextAlign = ContentAlignment.MiddleRight, AutoSize = false, Width = 100 };

            pnl.Controls.AddRange(new Control[] { lblTime, lblBan, lblTien });
            return pnl;
        }

        private string GetInitials(string name)
        {
            if (string.IsNullOrEmpty(name)) return "KH";
            var parts = name.Split(' ');
            if (parts.Length == 1) return parts[0].Substring(0, 1).ToUpper();
            return (parts[0].Substring(0, 1) + parts[parts.Length - 1].Substring(0, 1)).ToUpper();
        }
    }
}