using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Billiard.DAL.Data;
using Billiard.DAL.Entities;
using Billiard.WinForm.Forms.Menu;
namespace Billiard.WinForm
{
    public partial class MainForm : Form
    {
        private readonly BilliardDbContext _context;
        private Button _activeButton;
        private Form _activeForm;

        // Session info
        public int MaNV { get; set; }
        public string TenNV { get; set; }
        public string ChucVu { get; set; }

        public MainForm(BilliardDbContext context)
        {
            InitializeComponent();
            _context = context;

            // Rounded corners for panels
            SetRoundedCorners();
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Load user info from session
                LoadUserInfo();

                // Load statistics
                await LoadStatistics();

                // Auto-refresh every 30 seconds
                var timer = new System.Timers.Timer { Interval = 30000 };
                timer.Elapsed += async (s, ev) => await LoadStatistics();
                timer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUserInfo()
        {
            lblUserName.Text = TenNV ?? "Nhân viên";
            lblUserRole.Text = ChucVu ?? "Nhân viên";
            lblUserAvatar.Text = GetRoleInitials(ChucVu);
        }

        private string GetRoleInitials(string role)
        {
            return role switch
            {
                "Admin" => "AD",
                "Quản lý" => "QL",
                "Thu ngân" => "TN",
                "Phục vụ" => "PV",
                _ => "NV"
            };
        }

        private async System.Threading.Tasks.Task LoadStatistics()
        {
            try
            {
                // Bàn trống
                var banTrong = await _context.BanBia
                    .CountAsync(b => b.TrangThai == "Trống");
                lblBanTrongValue.Text = banTrong.ToString();

                // Đang chơi
                var dangChoi = await _context.BanBia
                    .CountAsync(b => b.TrangThai == "Đang chơi");
                lblDangChoiValue.Text = dangChoi.ToString();

                // Đặt trước
                var datTruoc = await _context.DatBans
                    .CountAsync(d => d.TrangThai == "Đang chờ");
                lblDatTruocValue.Text = datTruoc.ToString();

                // Doanh thu hôm nay
                var today = DateTime.Today;
                var doanhThu = await _context.HoaDons
                    .Where(h => h.ThoiGianBatDau.HasValue &&
                               h.ThoiGianBatDau.Value.Date == today &&
                               h.TrangThai == "Đã thanh toán")
                    .SumAsync(h => h.TongTien);
                //lblDoanhThuValue.Text = FormatCurrency(doanhThu);

                // Tổng khách hàng
                var tongKH = await _context.KhachHangs.CountAsync();
                lblKhachHangValue.Text = tongKH.ToString();
            }
            catch (Exception ex)
            {
                // Log error but don't show to user to avoid interruption
                Console.WriteLine($"Error loading statistics: {ex.Message}");
            }
        }

        private string FormatCurrency(decimal amount)
        {
            return $"{amount:N0}đ";
        }

        #region Sidebar Navigation

        private void SidebarButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            ActivateButton(button);

            // Clear current form
            if (_activeForm != null)
            {
                _activeForm.Close();
            }

            // Open corresponding form
            switch (button.Name)
            {
                case "btnQuanLyBan":
                    OpenChildForm(new QLBanForm(_context, this));
                    break;
                case "btnDichVu":
                    //OpenChildForm(new DichVuForm(_context));
                    break;
                case "btnHoaDon":
                    //OpenChildForm(new HoaDonForm(_context));
                    break;
                case "btnKhachHang":
                    //OpenChildForm(new KhachHangForm(_context));
                    break;
                case "btnThongKe":
                    //OpenChildForm(new ThongKeForm(_context));
                    break;
                case "btnNhanVien":
                    //OpenChildForm(new NhanVienForm(_context));
                    break;
                case "btnCaiDat":
                    //OpenChildForm(new CaiDatForm(_context));
                    break;
            }
        }

        private void OpenChildForm(Form childForm)
        {
            _activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            pnlMain.Controls.Clear();
            pnlMain.Controls.Add(childForm);
            childForm.Show();
        }

        private void ActivateButton(Button button)
        {
            if (_activeButton != null)
            {
                _activeButton.BackColor = Color.Transparent;
            }

            button.BackColor = Color.FromArgb(51, 65, 85);
            _activeButton = button;
        }

        private void SidebarButton_MouseEnter(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != _activeButton)
            {
                button.BackColor = Color.FromArgb(51, 65, 85);
            }
        }

        private void SidebarButton_MouseLeave(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != _activeButton)
            {
                button.BackColor = Color.Transparent;
            }
        }

        #endregion

        #region Button Hover Effects

        private void Button_MouseEnter(object sender, EventArgs e)
        {
            var button = sender as Button;
            button.BackColor = Color.FromArgb(220, 38, 38);
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            var button = sender as Button;
            button.BackColor = Color.FromArgb(239, 68, 68);
        }

        #endregion

        #region Logout

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Bạn có chắc muốn đăng xuất?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Clear session
                MaNV = 0;
                TenNV = null;
                ChucVu = null;

                // Show login form
                var loginForm = Program.GetService<LoginForm>();
                loginForm.Show();
                this.Close();
            }
        }

        #endregion

        #region UI Enhancements

        private void SetRoundedCorners()
        {
            // Add rounded corners to stat cards
            foreach (Control ctrl in pnlStats.Controls)
            {
                if (ctrl is Panel panel)
                {
                    panel.Paint += (s, e) =>
                    {
                        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        var path = GetRoundedRectangle(panel.ClientRectangle, 8);
                        panel.Region = new Region(path);
                    };
                }
            }

            // Rounded avatar
            lblUserAvatar.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                var path = GetRoundedRectangle(lblUserAvatar.ClientRectangle, 25);
                lblUserAvatar.Region = new Region(path);
            };
        }

        private System.Drawing.Drawing2D.GraphicsPath GetRoundedRectangle(Rectangle rect, int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }

        public void UpdateDetailPanel(string title, Control content)
        {
            pnlDetail.Controls.Clear();

            var lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Location = new Point(15, 15),
                AutoSize = true
            };

            pnlDetail.Controls.Add(lblTitle);

            content.Location = new Point(15, 50);
            content.Width = pnlDetail.Width - 30;
            pnlDetail.Controls.Add(content);
        }

        #endregion
    }
}