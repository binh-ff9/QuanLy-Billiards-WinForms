using Billiard.BLL.Services.QLBan;
using Billiard.WinForm.Forms.Helpers;
using Microsoft.Extensions.DependencyInjection;
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
    public partial class ClientMainForm : Form
    {
        private readonly BanBiaService _banService;
        private FlowLayoutPanel flpBan;
        public ClientMainForm(BanBiaService banService)
        {
            InitializeComponent();
            _banService = banService;
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Đặt bàn Bi-a Online";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;
            // 1. Header Panel
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White,
                Padding = new Padding(20, 0, 20, 0)
            };

            var lblWelcome = new Label
            {
                Text = $"👋 Xin chào, {UserSession.TenKH ?? "Khách hàng"}",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = true,
                Location = new Point(20, 18)
            };

            var btnProfile = new Button
            {
                Text = "👤 Hồ sơ & Lịch sử",
                Font = new Font("Segoe UI", 10),
                Size = new Size(250, 35),
                Location = new Point(this.Width - 320, 12),
                BackColor = Color.FromArgb(241, 245, 249),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnProfile.Click += (s, e) => OpenProfile();

            var btnLogout = new Button
            {
                Text = "Đăng xuất",
                Font = new Font("Segoe UI", 10),
                Size = new Size(100, 35),
                Location = new Point(this.Width - 150, 12),
                BackColor = Color.MistyRose,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogout.Click += (s, e) => this.Close(); // Đóng form để về Login

            pnlHeader.Controls.AddRange(new Control[] { lblWelcome, btnProfile, btnLogout });
            this.Controls.Add(pnlHeader);

            // 2. Danh sách bàn (FlowLayout)
            flpBan = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20)
            };
            this.Controls.Add(flpBan);
            pnlHeader.BringToFront(); // Đảm bảo header nằm trên

            // Load dữ liệu
            this.Load += async (s, e) => await LoadTableList();
        }
        private async Task LoadTableList()
        {
            flpBan.Controls.Clear();
            var listBan = await _banService.GetAllTablesAsync();

            foreach (var ban in listBan)
            {
                // Tạo Card cho từng bàn
                var card = new Panel
                {
                    Size = new Size(200, 150),
                    BackColor = Color.White,
                    Margin = new Padding(10),
                    Cursor = Cursors.Hand
                };
                // Hiệu ứng viền đơn giản
                card.Paint += (s, e) => ControlPaint.DrawBorder(e.Graphics, card.ClientRectangle, Color.LightGray, ButtonBorderStyle.Solid);

                var lblName = new Label
                {
                    Text = ban.TenBan,
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    ForeColor = Color.DarkSlateBlue,
                    Dock = DockStyle.Top,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Height = 50
                };

                var lblType = new Label
                {
                    Text = $"{ban.MaLoaiNavigation?.TenLoai}\n{ban.MaLoaiNavigation?.GiaGio:N0} đ/h",
                    Font = new Font("Segoe UI", 10),
                    ForeColor = Color.Gray,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                var btnBook = new Label // Dùng Label làm nút cho đẹp
                {
                    Text = "📅 ĐẶT NGAY",
                    Dock = DockStyle.Bottom,
                    Height = 40,
                    BackColor = Color.FromArgb(34, 197, 94), // Xanh lá
                    ForeColor = Color.White,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                };

                // Sự kiện click (Gán cho cả Card và các thành phần con)
                EventHandler clickEvent = (s, e) => OpenBookingDialog(ban.MaBan, ban.TenBan);
                card.Click += clickEvent;
                lblName.Click += clickEvent;
                lblType.Click += clickEvent;
                btnBook.Click += clickEvent;

                card.Controls.Add(lblType);
                card.Controls.Add(lblName);
                card.Controls.Add(btnBook);

                flpBan.Controls.Add(card);
            }
        }

        private void OpenBookingDialog(int maBan, string tenBan)
        {
            // Tạo Scope mới để mở Dialog đặt bàn
            using (var scope = Program.ServiceProvider.CreateScope())
            {
                // Lấy form từ DI (để nó tự inject Service vào)
                var frm = scope.ServiceProvider.GetRequiredService<DatBanDialog>();
                frm.SetTableInfo(maBan, tenBan);

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    // Đặt thành công thì làm gì đó (ví dụ hiện thông báo)
                }
            }
        }
        private void OpenProfile()
        {
            using (var scope = Program.ServiceProvider.CreateScope())
            {
                var frm = scope.ServiceProvider.GetRequiredService<UserProfileForm>();
                frm.ShowDialog();
            }
        }
        private void btnLogout_Click(object sender, EventArgs e) // Gán sự kiện này cho nút Đăng xuất
        {
            var result = MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // 1. Xóa sạch thông tin người dùng
                UserSession.Logout();

                // 2. Đóng form này lại
                // (LoginForm đang đứng đợi sự kiện đóng của form này để hiện lên lại)
                this.Close();
            }
        }
    }
}
