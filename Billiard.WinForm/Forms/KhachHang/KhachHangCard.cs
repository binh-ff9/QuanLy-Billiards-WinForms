using Billiard.DAL.Entities;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.KhachHang
{
    public partial class KhachHangCard : UserControl
    {
        public Billiard.DAL.Entities.KhachHang Data { get; private set; }

        // Các màu sắc từ hình mẫu
        private Color _borderColor = Color.FromArgb(34, 197, 94); // Viền xanh lá
        private Color _bgTop = Color.FromArgb(224, 242, 254); // Xanh dương nhạt
        private Color _bgBottom = Color.White;
        private Color _badgeColor = Color.FromArgb(254, 240, 138); // Vàng nhạt (Badge)

        public KhachHangCard()
        {
            InitializeComponent();
            this.Size = new Size(300, 260); // Kích thước chuẩn card
            this.DoubleBuffered = true; // Chống nháy
            this.Cursor = Cursors.Hand;
            this.Padding = new Padding(15);
            this.BackColor = Color.Transparent;
        }

        public void SetData(Billiard.DAL.Entities.KhachHang kh)
        {
            this.Data = kh;
            this.Invalidate(); // Vẽ lại card
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var g = e.Graphics;

            // 1. Vẽ nền Gradient (Xanh nhạt -> Trắng)
            using (var path = GetRoundedPath(ClientRectangle, 15))
            using (var brush = new LinearGradientBrush(ClientRectangle, _bgTop, _bgBottom, 90F))
            using (var pen = new Pen(_borderColor, 3)) // Viền dày 3px
            {
                g.FillPath(brush, path);
                g.DrawPath(pen, path);
            }

            if (Data == null) return;

            // 2. Vẽ Badge (Hạng thành viên) - Góc phải
            DrawBadge(g, GetRankName(Data.DiemTichLuy ?? 0));

            // 3. Vẽ Avatar (Tròn) - Ở giữa
            DrawAvatar(g, Data.TenKh);

            // 4. Vẽ Tên & Thông tin
            int yPos = 110;

            // Tên KH
            var fontName = new Font("Segoe UI", 14, FontStyle.Bold);
            var szName = g.MeasureString(Data.TenKh, fontName);
            g.DrawString(Data.TenKh, fontName, Brushes.Black, 15, yPos);
            yPos += 25;

            // SĐT (Icon 📱)
            var fontInfo = new Font("Segoe UI", 10, FontStyle.Regular);
            g.DrawString($"📱 {Data.Sdt}", fontInfo, Brushes.DimGray, 15, yPos);
            yPos += 20;

            // Email (Icon ✉️)
            string email = Data.Email ?? "---";
            if (email.Length > 25) email = email.Substring(0, 22) + "..."; // Cắt bớt nếu dài
            g.DrawString($"✉️ {email}", fontInfo, Brushes.DimGray, 15, yPos);
            yPos += 30;

            // 5. Vẽ Box Thống kê (Màu trắng, bo góc dưới)
            Rectangle rectStats = new Rectangle(15, yPos, this.Width - 30, 60);
            using (var pathStats = GetRoundedPath(rectStats, 10))
            using (var brushStats = new SolidBrush(Color.FromArgb(245, 247, 250))) // Nền xám cực nhạt
            {
                g.FillPath(brushStats, pathStats);
            }

            // Nội dung thống kê
            var fontLabel = new Font("Segoe UI", 9, FontStyle.Regular);
            var fontValue = new Font("Segoe UI", 9, FontStyle.Bold);

            // Dòng 1: Điểm
            g.DrawString("Điểm tích lũy:", fontLabel, Brushes.Black, 25, yPos + 10);
            string diem = $"{Data.DiemTichLuy ?? 0}";
            var szDiem = g.MeasureString(diem, fontValue);
            g.DrawString(diem, fontValue, Brushes.OrangeRed, rectStats.Right - szDiem.Width - 10, yPos + 10);

            // Kẻ ngang mờ
            g.DrawLine(Pens.LightGray, 25, yPos + 30, rectStats.Right - 10, yPos + 30);

            // Dòng 2: Tổng chi tiêu (Giả lập tính toán)
            decimal tongTien = 0; // Bạn có thể truyền vào từ Service nếu muốn chuẩn
            g.DrawString("Tổng chi tiêu:", fontLabel, Brushes.Black, 25, yPos + 35);
            string tien = $"{tongTien:N0} đ";
            if (Data.HoaDons != null) tien = $"{Data.HoaDons.Sum(h => h.TongTien):N0} đ";

            var szTien = g.MeasureString(tien, fontValue);
            g.DrawString(tien, fontValue, Brushes.Black, rectStats.Right - szTien.Width - 10, yPos + 35);
        }

        private Color GetRankColor(string rank)
        {
            switch (rank)
            {
                case "Bạch Kim": return Color.FromArgb(6, 182, 212);   // Cyan sáng
                case "Vàng": return Color.FromArgb(234, 179, 8);       // Vàng đậm
                case "Bạc": return Color.FromArgb(100, 116, 139);      // Xám bạc
                case "Đồng": return Color.FromArgb(183, 110, 121);     // Đồng / Rose Brown
                default: return Color.FromArgb(34, 197, 94);           // Màu mặc định (nếu lỗi)
            }
        }


        private void DrawBadge(Graphics g, string rank)
        {
            // Tính toán kích thước chữ trước
            var fontBadge = new Font("Segoe UI", 8, FontStyle.Bold);
            var sz = g.MeasureString(rank, fontBadge);

            // Độ rộng Badge = Chữ + Icon (12px) + Padding (15px)
            int badgeWidth = (int)sz.Width + 27;
            Rectangle rectBadge = new Rectangle(this.Width - badgeWidth - 15, 15, badgeWidth, 24);

            // Vẽ nền Badge (Màu trắng mờ hoặc nhạt)
            using (var path = GetRoundedPath(rectBadge, 12))
            using (var brush = new SolidBrush(Color.White))
            {
                g.FillPath(brush, path);
            }

            // Lấy màu đặc trưng của hạng
            Color rankColor = GetRankColor(rank);

            // 1. Vẽ chấm tròn màu (Icon)
            using (var brush = new SolidBrush(rankColor))
            {
                // Vẽ hình tròn đường kính 8px nằm bên trái chữ
                g.FillEllipse(brush, rectBadge.X + 8, rectBadge.Y + 8, 8, 8);
            }

            // 2. Vẽ Tên hạng
            using (var brushText = new SolidBrush(Color.FromArgb(30, 41, 59))) // Màu chữ xám đen
            {
                g.DrawString(rank, fontBadge, brushText, rectBadge.X + 20, rectBadge.Y + 4);
            }
        }

        private void DrawAvatar(Graphics g, string name)
        {
            // Vẽ vòng tròn viền trắng
            int size = 60;
            int x = (this.Width - size) / 2;
            int y = 15;
            Rectangle rectAvt = new Rectangle(x, y, size, size);

            g.FillEllipse(Brushes.White, x - 2, y - 2, size + 4, size + 4); // Viền trắng

            // Vẽ nền avatar (Màu ngẫu nhiên hoặc cố định)
            using (var brush = new SolidBrush(Color.FromArgb(51, 65, 85)))
            {
                g.FillEllipse(brush, rectAvt);
            }

            // Vẽ chữ cái đầu tên
            string initial = string.IsNullOrEmpty(name) ? "?" : name.Substring(0, 1).ToUpper();
            var fontAvt = new Font("Segoe UI", 20, FontStyle.Bold);
            var sz = g.MeasureString(initial, fontAvt);
            g.DrawString(initial, fontAvt, Brushes.White, x + (size - sz.Width) / 2 + 1, y + (size - sz.Height) / 2);
        }

        // Hàm hỗ trợ vẽ bo góc
        private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            float d = radius * 2.0F;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private string GetRankName(int diem)
        {
            if (diem > 300) return "Bạch Kim";
            if (diem > 150) return "Vàng";
            if (diem > 70) return "Bạc";
            return "Đồng";
        }
    }
}