using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.HoaDon
{
    public partial class HoaDonRowControl : UserControl
    {
        public Billiard.DAL.Entities.HoaDon Data { get; private set; }

        // --- CẤU HÌNH MÀU SẮC ---     
        private Color _bgNormal = Color.White;
        private Color _bgHover = Color.FromArgb(241, 245, 249); // Xám cực nhạt khi di chuột
        private Color _textPrimary = Color.FromArgb(30, 41, 59);   // Đen xám
        private Color _textSecondary = Color.FromArgb(100, 116, 139); // Xám nhạt

        // Màu trạng thái
        private Color _badgeGreenBg = Color.FromArgb(220, 252, 231);
        private Color _badgeGreenText = Color.FromArgb(22, 163, 74);
        private Color _badgeRedBg = Color.FromArgb(254, 226, 226);
        private Color _badgeRedText = Color.FromArgb(220, 38, 38);

        public event EventHandler Clicked; // Sự kiện click để form cha bắt
        public HoaDonRowControl()
        {
            InitializeComponent();

            this.Size = new Size(800, 75); // Chiều cao cố định 75px
            this.BackColor = _bgNormal;
            this.DoubleBuffered = true; // Chống nháy hình
            this.Cursor = Cursors.Hand; // Con trỏ bàn tay

            // Hiệu ứng Hover
            this.MouseEnter += (s, e) => { this.BackColor = _bgHover; Invalidate(); };
            this.MouseLeave += (s, e) => { this.BackColor = _bgNormal; Invalidate(); };

            this.Click += (s, e) => Clicked?.Invoke(this, EventArgs.Empty);
        }


        public void SetData(Billiard.DAL.Entities.HoaDon hd)
        {
            this.Data = hd;
            this.Invalidate();
        }
        // --- PHẦN VẼ GIAO DIỆN (QUAN TRỌNG NHẤT) ---
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            var g = e.Graphics;

            // 1. Vẽ đường kẻ ngăn cách dưới cùng
            using (var pen = new Pen(Color.FromArgb(226, 232, 240), 1))
            {
                g.DrawLine(pen, 15, this.Height - 1, this.Width - 15, this.Height - 1);
            }

            if (Data == null) return;

            // --- CỘT 1: ICON & MÃ HÓA ĐƠN ---
            // Vẽ Icon Hóa đơn (Hình tròn xám + ký hiệu)
            Rectangle rectIcon = new Rectangle(20, 15, 45, 45);
            using (var brushIcon = new SolidBrush(Color.FromArgb(241, 245, 249)))
                g.FillEllipse(brushIcon, rectIcon);

            // Vẽ chữ "#" hoặc icon trong hình tròn
            var fontIcon = new Font("Segoe UI", 14, FontStyle.Bold);
            TextRenderer.DrawText(g, "#", fontIcon, rectIcon, Color.Gray, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

            // Vẽ Mã HĐ ngay dưới hoặc cạnh đó
            // Ở đây mình vẽ Mã HĐ to bên cạnh icon
            var fontId = new Font("Segoe UI", 11, FontStyle.Bold);
            g.DrawString($"#{Data.MaHd}", fontId, new SolidBrush(Color.Gray), 75, 25);


            // --- CỘT 2: THÔNG TIN BÀN & KHÁCH ---
            // Tên Bàn (To, Đậm)
            var fontBan = new Font("Segoe UI", 13, FontStyle.Bold);
            string tenBan = Data.MaBanNavigation?.TenBan ?? "Mang về";
            g.DrawString(tenBan, fontBan, new SolidBrush(_textPrimary), 160, 15);

            // Thông tin phụ (Khách hàng + Giờ)
            var fontSub = new Font("Segoe UI", 10, FontStyle.Regular);
            string tenKhach = Data.MaKhNavigation?.TenKh ?? "Khách lẻ";
            string gioChoi = Data.ThoiGianBatDau?.ToString("HH:mm dd/MM") ?? "--:--";

            // Vẽ dòng phụ: 👤 Nguyễn Văn A   🕒 10:30 20/10
            string subText = $"👤 {tenKhach}    🕒 {gioChoi}";
            g.DrawString(subText, fontSub, new SolidBrush(_textSecondary), 160, 42);


            // --- CỘT 3: TỔNG TIỀN (Căn phải, cách lề phải 150px để nhường chỗ cho Badge) ---
            var fontPrice = new Font("Segoe UI", 13, FontStyle.Bold);
            string priceText = $"{Data.TongTien:N0}đ";
            var sizePrice = g.MeasureString(priceText, fontPrice);

            int xPrice = this.Width - 180 - (int)sizePrice.Width; // Cách lề phải 180px
            g.DrawString(priceText, fontPrice, new SolidBrush(_textPrimary), xPrice, 25);


            // --- CỘT 4: TRẠNG THÁI (Badge màu bo góc ngoài cùng bên phải) ---
            string status = Data.TrangThai ?? "---";
            bool isPaid = status == "Đã thanh toán";

            // Màu nền và màu chữ badge
            Color bgBadge = isPaid ? _badgeGreenBg : _badgeRedBg;
            Color textBadge = isPaid ? _badgeGreenText : _badgeRedText;

            var fontBadge = new Font("Segoe UI", 9, FontStyle.Bold);
            var sizeBadge = g.MeasureString(status, fontBadge);

            // Tính toán khung hình chữ nhật cho Badge
            int badgeW = (int)sizeBadge.Width + 20; // Padding ngang
            int badgeH = 26;
            int xBadge = this.Width - badgeW - 20; // Cách lề phải 20px
            int yBadge = 24; // Căn giữa theo chiều dọc (75/2 - 26/2)

            Rectangle rectBadge = new Rectangle(xBadge, yBadge, badgeW, badgeH);

            // Vẽ nền Badge bo góc
            using (var pathBadge = GetRoundedPath(rectBadge, 10))
            using (var brushBadge = new SolidBrush(bgBadge))
            {
                g.FillPath(brushBadge, pathBadge);
            }

            // Vẽ chữ Status
            TextRenderer.DrawText(g, status, fontBadge, rectBadge, textBadge, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        // Hàm hỗ trợ vẽ bo góc (Copy y chang từ KhachHangCard)
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

    }
}
