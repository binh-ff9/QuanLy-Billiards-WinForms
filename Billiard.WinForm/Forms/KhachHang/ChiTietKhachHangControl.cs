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
        private bool _isDeletedUser = false;
        private Button btnDeleteAction;
        public ChiTietKhachHangControl()
        {
            InitializeComponent();
            this.BackColor = Color.White;
            SetupLayout();
        }

        private void SetupLayout()
        {
            var pnlFooter = new Panel { Dock = DockStyle.Bottom, Height = 60, Padding = new Padding(10) };

            // Dùng TableLayoutPanel để chia đôi 2 nút (Sửa và Xóa)
            var tblButtons = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
            tblButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F)); // 50%
            tblButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F)); // 50%

            // Nút Sửa (Code cũ, giữ nguyên, cho vào cột 0)
            var btnEdit = new Button { Text = "✏️ Chỉnh sửa", Dock = DockStyle.Fill, BackColor = Color.FromArgb(234, 179, 8), ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand, Margin = new Padding(0, 0, 5, 0) };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Click += (s, e) => OnEditClick?.Invoke(this, _currentMaKh);

            btnDeleteAction = new Button { Dock = DockStyle.Fill, ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand, Margin = new Padding(5, 0, 0, 0) };
            btnDeleteAction.FlatAppearance.BorderSize = 0;
            btnDeleteAction.Click += (s, e) => OnDeleteClick?.Invoke(this, _currentMaKh);

            tblButtons.Controls.Add(btnEdit, 0, 0);
            tblButtons.Controls.Add(btnDeleteAction, 1, 0);

            pnlFooter.Controls.Add(tblButtons);
            this.Controls.Add(pnlFooter);

            //pnlFooter.Controls.Add(btnEdit);
            //this.Controls.Add(pnlFooter); // Thêm Footer vào UserControl

            pnlContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(20)
            };

            // Hack full width
            pnlContainer.SizeChanged += (s, e) => {
                foreach (Control c in pnlContainer.Controls) c.Width = pnlContainer.ClientSize.Width - 40;
            };
            this.Controls.Add(pnlContainer);
            pnlContainer.BringToFront();
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
            var pnlHeader = new Panel { Height = 100, Margin = new Padding(0, 0, 0, 20) };

            // Avatar tròn (Vẽ bằng code)
            var lblAvatar = new Label
            {
                Text = GetInitials(kh.TenKh),
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(80, 80),
                Location = new Point(0, 10)
            };
            lblAvatar.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var brush = new SolidBrush(Color.FromArgb(99, 102, 241))) // Màu tím
                    e.Graphics.FillEllipse(brush, 0, 0, 79, 79);
                TextRenderer.DrawText(e.Graphics, lblAvatar.Text, lblAvatar.Font, new Rectangle(0, 0, 80, 80), Color.White, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
            };

            // Tên & SĐT
            var lblName = new Label { Text = kh.TenKh, Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59), AutoSize = true, Location = new Point(90, 20) };
            var lblPhone = new Label { Text = kh.Sdt, Font = new Font("Segoe UI", 11, FontStyle.Regular), ForeColor = Color.Gray, AutoSize = true, Location = new Point(92, 50) };

            pnlHeader.Controls.AddRange(new Control[] { lblAvatar, lblName, lblPhone });
            pnlContainer.Controls.Add(pnlHeader);


            // --- 2. THỐNG KÊ (STATS) ---
            decimal tongTien = kh.HoaDons.Sum(h => h.TongTien) ?? 0;
            int soLanDen = kh.HoaDons.Count;

            var pnlStats = new TableLayoutPanel { Height = 80, ColumnCount = 2, RowCount = 1, Margin = new Padding(0, 0, 0, 20) };
            pnlStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            pnlStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            pnlStats.Controls.Add(CreateStatBox("💰 Tổng chi tiêu", $"{tongTien:N0}đ", Color.FromArgb(22, 163, 74)), 0, 0);
            pnlStats.Controls.Add(CreateStatBox("🏆 Số lần đến", $"{soLanDen} lần", Color.FromArgb(234, 179, 8)), 1, 0);

            pnlContainer.Controls.Add(pnlStats);


            // --- 3. THÔNG TIN LIÊN HỆ ---
            var pnlInfo = new Panel { AutoSize = true, Margin = new Padding(0, 0, 0, 20) };
            AddInfoRow(pnlInfo, "Email:", kh.Email ?? "Chưa cập nhật", 0);
            AddInfoRow(pnlInfo, "Nhóm:", "Thành viên thân thiết", 60);
            pnlContainer.Controls.Add(pnlInfo);


            // --- 4. LỊCH SỬ GIAO DỊCH (5 Gần nhất) ---
            var lblHistoryTitle = new Label { Text = "Lịch sử gần đây", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.Black, AutoSize = true, Margin = new Padding(0, 0, 0, 10) };
            pnlContainer.Controls.Add(lblHistoryTitle);

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
            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(248, 250, 252) }; // Nền xám nhạt
                                                                                                      // Có thể thêm bo góc ở đây nếu muốn

            var lblVal = new Label { Text = value, Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = color, Dock = DockStyle.Bottom, TextAlign = ContentAlignment.MiddleCenter, Height = 30 };
            var lblTit = new Label { Text = title, Font = new Font("Segoe UI", 9, FontStyle.Regular), ForeColor = Color.Gray, Dock = DockStyle.Top, TextAlign = ContentAlignment.MiddleCenter, Height = 25 };

            pnl.Controls.Add(lblTit);
            pnl.Controls.Add(lblVal);
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