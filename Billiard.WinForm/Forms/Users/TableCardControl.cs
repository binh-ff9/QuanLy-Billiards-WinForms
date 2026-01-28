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
            lblTrangThai.Text = ban.TrangThai;

            lblTrangThai.ForeColor = ban.TrangThai == "Trống" ? Color.Green : Color.Red;

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
            this.BackColor = Color.White;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Size = new Size(220, 350);

            this.MouseEnter += (s, e) => this.BackColor = Color.FromArgb(245, 247, 250);
            this.MouseLeave += (s, e) => this.BackColor = Color.White;

            this.BackColor = ban.TrangThai switch
            {
                "Trống" => Color.FromArgb(240, 253, 244),
                "Đang chơi" => Color.FromArgb(254, 242, 242),
                "Đã đặt" => Color.FromArgb(255, 251, 235),
                _ => Color.White
            };
            this.Paint += (s, e) =>
            {
                var borderColor = ban.TrangThai switch
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
            lblTrangThai.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTrangThai.ForeColor = Color.White;
            lblTrangThai.BackColor = ban.TrangThai switch
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
            // 1. Cập nhật màu nền
            this.BackColor = ban.TrangThai switch
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
                lblTrangThai.Text = ban.TrangThai;
                lblTrangThai.BackColor = ban.TrangThai switch
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
                        if (ban.TrangThai == "Trống")
                        {
                            ban.TrangThai = "Đã đặt";
                            UpdateUI(); // Hàm vẽ lại màu sắc Card
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi mở form đặt bàn: " + ex.Message);
            }
        }
    }
}
