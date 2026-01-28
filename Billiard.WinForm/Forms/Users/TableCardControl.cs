using Billiard.BLL.Services.QLBan;
using Billiard.DAL.Entities;
using Billiard.WinForm.Forms.Auth;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Billiard.WinForm.Forms.Users
{
    public partial class TableCardControl : UserControl
    {
        private BanBium ban;
        private BanBiaService _service;

        public TableCardControl(BanBium _ban, BanBiaService service)
        {
            InitializeComponent();
            ban = _ban;
            _service = service;
            LoadData();

            SetUpUI();

        }

        private void LoadData()
        {
            lblTenBan.Text = ban.TenBan;
            lblLoaiBan.Text = ban.MaLoaiNavigation?.TenLoai;
            lblGia.Text = (ban.MaLoaiNavigation?.GiaGio ?? 0).ToString("N0") + " đ/giờ";

            string statusHienThi = GetTrangThaiHienThi();
            lblTrangThai.Text = statusHienThi;

            lblTrangThai.ForeColor = statusHienThi == "Trống" ? Color.Green :
                                                 (statusHienThi == "Đã đặt" ? Color.FromArgb(234, 179, 8) : Color.Red);
            if (!string.IsNullOrEmpty(ban.HinhAnh))
            {
                try
                {
                    var projectRoot = Directory.GetParent(Directory.GetParent(Directory.GetParent(
                        Directory.GetParent(Application.StartupPath).FullName).FullName).FullName).FullName;
                    string imagePath = Path.Combine(projectRoot, "Forms", "Resources", "img", "tables", ban.HinhAnh);

                    if (File.Exists(imagePath))
                    {
                        using (var img = Image.FromFile(imagePath))
                        {
                            picTable.Image = new Bitmap(img);
                        }
                    }
                    else
                    {
                        picTable.Image = null;
                        AddDefaultTableIcon(pnlImage);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error loading table image: {ex.Message}");
                    AddDefaultTableIcon(pnlImage);
                }
            }
            else
            {
                AddDefaultTableIcon(pnlImage);
            }

        }
        private void SetUpUI()
        {
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Size = new Size(220, 350);

            string statusHienThi = GetTrangThaiHienThi();

            this.BackColor = statusHienThi switch
            {
                "Trống" => Color.FromArgb(240, 253, 244),
                "Đang chơi" => Color.FromArgb(254, 242, 242),
                "Đã đặt" => Color.FromArgb(255, 251, 235),
                _ => Color.White
            };

            // Sự kiện hover
            this.MouseEnter += (s, e) => this.BackColor = Color.FromArgb(245, 247, 250);
            this.MouseLeave += (s, e) => this.BackColor = statusHienThi switch
            {
                "Trống" => Color.FromArgb(240, 253, 244),
                "Đang chơi" => Color.FromArgb(254, 242, 242),
                "Đã đặt" => Color.FromArgb(255, 251, 235),
                _ => Color.White
            };

            this.Paint += (s, e) =>
            {
                var borderColor = statusHienThi switch
                {
                    "Trống" => Color.FromArgb(34, 197, 94),
                    "Đang chơi" => Color.FromArgb(239, 68, 68),
                    "Đã đặt" => Color.FromArgb(234, 179, 8),
                    _ => Color.Gray
                };

            };
            pnlImage = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(220, 140),
                BackColor = Color.FromArgb(248, 250, 252)
            };

            //lblTrangThai
            lblTrangThai.Text = statusHienThi;
            lblTrangThai.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTrangThai.ForeColor = Color.White;
            lblTrangThai.BackColor = statusHienThi switch
            {
                "Trống" => Color.FromArgb(34, 197, 94),
                "Đang chơi" => Color.FromArgb(239, 68, 68),
                "Đã đặt" => Color.FromArgb(234, 179, 8),
                _ => Color.Gray
            };
            lblTrangThai.AutoSize = false;
            lblTrangThai.TextAlign = ContentAlignment.MiddleCenter;
            lblTrangThai.Size = new Size(85, 28);

            // Tên bàn
            lblTenBan.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTenBan.ForeColor = Color.FromArgb(30, 41, 59);

            // ===== NÚT CHỈNH SỬA =====

            btnDatBan.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnDatBan.BackColor = Color.FromArgb(59, 130, 246);
            btnDatBan.ForeColor = Color.White;
            btnDatBan.FlatStyle = FlatStyle.Flat;
            btnDatBan.Cursor = Cursors.Hand;
            btnDatBan.Tag = ban;
            btnDatBan.TabStop = false; // Tránh focus khi tab

            btnDatBan.FlatAppearance.BorderSize = 0;
            btnDatBan.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            // Hover effect cho nút edit
            btnDatBan.MouseEnter += (s, e) =>
            {
                btnDatBan.BackColor = Color.FromArgb(37, 99, 235);
            };

            btnDatBan.MouseLeave += (s, e) =>
            {
                btnDatBan.BackColor = Color.FromArgb(59, 130, 246);
            };
            btnDatBan.BringToFront(); // Đảm bảo nút ở trên cùng

            // Loại bàn
            lblLoaiBan.Font = new Font("Segoe UI", 12F);
            lblLoaiBan.ForeColor = Color.FromArgb(100, 116, 139);



            lblGia.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblGia.ForeColor = Color.FromArgb(99, 102, 241);

        }

        // Hàm này dùng để vẽ lại giao diện dựa trên trạng thái hiện tại của _ban
        private void UpdateUI()
        {
            string statusHienThi = GetTrangThaiHienThi();

            // 1. Cập nhật màu nền
            this.BackColor = statusHienThi switch
            {
                "Trống" => Color.FromArgb(240, 253, 244), // Xanh nhạt
                "Đang chơi" => Color.FromArgb(254, 242, 242), // Đỏ nhạt
                "Đã đặt" => Color.FromArgb(255, 251, 235), // Vàng nhạt
                _ => Color.White
            };

            // 2. Cập nhật Label trạng thái
            // Giả sử bạn đã lưu lblStatus là biến toàn cục trong class này
            if (lblTrangThai != null)
            {
                lblTrangThai.Text = statusHienThi;
                lblTrangThai.BackColor = statusHienThi switch
                {
                    "Trống" => Color.FromArgb(34, 197, 94),
                    "Đang chơi" => Color.FromArgb(239, 68, 68),
                    "Đã đặt" => Color.FromArgb(234, 179, 8), // Màu vàng
                    _ => Color.Gray
                };
            }
            // 4. Trigger vẽ lại viền (gọi sự kiện Paint)
            this.Invalidate();
        }
        private void AddDefaultTableIcon(Panel pnlImage)
        {
            var lblIcon = new Label
            {
                Text = "🎱",
                Font = new Font("Segoe UI", 56F),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(220, 140),
                BackColor = Color.Transparent
            };
            pnlImage.Controls.Add(lblIcon);
        }
        private void btnDatBan_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra đăng nhập (Logic cũ của bạn)
            if (!UserSession.IsLoggedIn)
            {
                MessageBox.Show("Bạn cần đăng nhập để đặt bàn!");
                var login = Program.GetService<LoginForm>();
                login.ShowDialog();
                if (!UserSession.IsLoggedIn)
                {
                    return;
                }
            }

            try
            {
                // 3. Khởi tạo Service và Dialog
                var datBanService = Program.GetService<DatBanService>();

                using (var dialog = new DatBanDialog(datBanService))
                {
                    dialog.SetTableInfo(ban.MaBan, ban.TenBan);

                    // 5. Hiển thị Dialog và chờ kết quả
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        // --- XỬ LÝ KHI ĐẶT THÀNH CÔNG ---
                        // --- KHẮC PHỤC LỖI KHÔNG CẬP NHẬT UI ---

                        // Vì dialog đã lưu vào DB rồi, ta cần "giả lập" dữ liệu đó vào biến `ban` 
                        // để hàm GetTrangThaiHienThi() nhận diện được ngay lập tức.

                        if (ban.DatBans == null) ban.DatBans = new List<DatBan>();

                        // Thêm 1 đơn đặt "ảo" vào list để UI đổi màu
                        ban.DatBans.Add(new DatBan
                        {
                            ThoiGianBatDau = DateTime.Now,
                            TrangThai = "Chưa nhận"
                        });

                        // Gọi hàm này để tính toán lại màu sắc và text
                        UpdateUI();

                        MessageBox.Show("Đặt bàn thành công!", "Thông báo");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi mở form đặt bàn: " + ex.Message);
            }
        }

        private string GetTrangThaiHienThi()
        {
            // Ưu tiên 1: Nếu bàn đang có khách chơi thật -> Luôn hiện Đang chơi
            if (ban.TrangThai == "Đang chơi")
            {
                return "Đang chơi";
            }

            // Ưu tiên 2: Nếu không chơi, kiểm tra xem hôm nay có lịch đặt không
            // Điều kiện: Có đơn trong DatBans + Ngày đặt là Hôm nay + Chưa bị hủy
            if (ban.DatBans != null && ban.DatBans.Any(d =>
                d.ThoiGianBatDau.HasValue &&
                d.ThoiGianBatDau.Value.Date == DateTime.Today && // Chỉ check ngày hôm nay
                d.TrangThai != "Đã hủy" &&
                d.TrangThai != "Đã xong"))
            {
                return "Đã đặt";
            }

            // Ưu tiên 3: Nếu không rơi vào 2 trường hợp trên -> Hiện Trống
            return "Trống";
        }
    }
}
