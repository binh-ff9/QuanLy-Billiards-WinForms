using Billiard.DAL.Data;
using Billiard.DAL.Entities;
using Billiard.WinForm.Forms;
using Billiard.WinForm.Forms.HoaDon;
using Billiard.WinForm.Forms.QLBan;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Billiard.WinForm.Forms.Auth;
using Billiard.BLL.Services.QLBan;
namespace Billiard.WinForm
{
    public partial class MainForm : Form
    {
        private readonly BilliardDbContext _context;
        private readonly BanBiaService _banBiaService;
        private Button _activeButton;
        private Form _activeForm;
        private System.Timers.Timer _refreshTimer;

        // Session info
        public int MaNV { get; set; }
        public string TenNV { get; set; }
        public string ChucVu { get; set; }

        public MainForm(BilliardDbContext context, BanBiaService banBiaService)
        {
            InitializeComponent();
            _context = context;
            _banBiaService = banBiaService;
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Show loading indicator
                this.Cursor = Cursors.WaitCursor;

                // Load user info from session
                LoadUserInfo();

                // Load statistics
                await LoadStatistics();

                // Setup auto-refresh timer
                SetupRefreshTimer();

                // Set initial styles
                ApplyInitialStyles();

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupRefreshTimer()
        {
            _refreshTimer = new System.Timers.Timer
            {
                Interval = 30000, // 30 seconds
                AutoReset = true,
                Enabled = true
            };
            _refreshTimer.Elapsed += async (s, ev) => await LoadStatistics();
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

        private void ApplyInitialStyles()
        {
            // Set stat cards initial values
            lblBanTrongValue.Text = "0";
            lblDangChoiValue.Text = "0";
            lblDatTruocValue.Text = "0";
            lblDoanhThuValue.Text = "0đ";
            lblKhachHangValue.Text = "0";

            // Show welcome panel
            ShowWelcomePanel();

            // Hide detail panel initially
            pnlDetail.Visible = false;
        }

        private void ShowWelcomePanel()
        {
            pnlMain.Controls.Clear();

            var welcomePanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            var lblWelcome = new Label
            {
                Text = "🎱",
                Font = new Font("Segoe UI", 72F),
                AutoSize = true,
                ForeColor = Color.FromArgb(99, 102, 241)
            };
            lblWelcome.Location = new Point(
                (pnlMain.Width - lblWelcome.Width) / 2,
                (pnlMain.Height - lblWelcome.Height) / 2 - 100
            );

            var lblTitle = new Label
            {
                Text = "Chào mừng đến Quản Lý Quán Bi-a Pro",
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                AutoSize = true,
                ForeColor = Color.FromArgb(30, 41, 59)
            };
            lblTitle.Location = new Point(
                (pnlMain.Width - lblTitle.Width) / 2,
                lblWelcome.Bottom + 30
            );

            var lblDesc = new Label
            {
                Text = "Chọn menu bên trái để bắt đầu",
                Font = new Font("Segoe UI", 14F),
                AutoSize = true,
                ForeColor = Color.FromArgb(100, 116, 139)
            };
            lblDesc.Location = new Point(
                (pnlMain.Width - lblDesc.Width) / 2,
                lblTitle.Bottom + 20
            );

            welcomePanel.Controls.AddRange(new Control[] { lblWelcome, lblTitle, lblDesc });
            pnlMain.Controls.Add(welcomePanel);
        }

        private async System.Threading.Tasks.Task LoadStatistics()
        {
            try
            {
                // Get table stats using service
                var stats = await _banBiaService.GetTableStatsAsync();

                // Update UI on main thread
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        lblBanTrongValue.Text = stats.trong.ToString();
                        lblDangChoiValue.Text = stats.dangChoi.ToString();
                        lblDatTruocValue.Text = stats.daDat.ToString();
                    }));
                }
                else
                {
                    lblBanTrongValue.Text = stats.trong.ToString();
                    lblDangChoiValue.Text = stats.dangChoi.ToString();
                    lblDatTruocValue.Text = stats.daDat.ToString();
                }

                // Doanh thu hôm nay
                var today = DateTime.Today;
                var doanhThu = await _context.HoaDons
                    .Where(h => h.ThoiGianBatDau.HasValue &&
                               h.ThoiGianBatDau.Value.Date == today &&
                               h.TrangThai == "Đã thanh toán")
                    .SumAsync(h => h.TongTien) ?? 0M;

                // Tổng khách hàng
                var tongKH = await _context.KhachHangs.CountAsync();

                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        lblDoanhThuValue.Text = FormatCurrency(doanhThu);
                        lblKhachHangValue.Text = tongKH.ToString();
                    }));
                }
                else
                {
                    lblDoanhThuValue.Text = FormatCurrency(doanhThu);
                    lblKhachHangValue.Text = tongKH.ToString();
                }
            }
            catch (Exception ex)
            {
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

            // Activate button visual
            ActivateButton(button);

            // Close and dispose current form
            if (_activeForm != null)
            {
                _activeForm.Close();
                _activeForm.Dispose();
                _activeForm = null;
            }

            // Hide detail panel
            pnlDetail.Visible = false;

            // Open corresponding form
            try
            {
                switch (button.Name)
                {
                    case "btnQuanLyBan":
                        var qlBanForm = Program.GetService<QLBanForm>();
                        qlBanForm.SetMainForm(this);
                        OpenChildForm(qlBanForm);
                        break;
                    case "btnDichVu":
                        OpenChildForm(Program.GetService<DichVuForm>());
                        break;
                    case "btnHoaDon":
                        var hoaDonForm = Program.GetService<HoaDonForm>();
                        hoaDonForm.SetMainForm(this);
                        OpenChildForm(hoaDonForm);
                        break;
                    case "btnKhachHang":
                        ShowComingSoon("Khách hàng");
                        break;
                    case "btnThongKe":
                        ShowComingSoon("Thống kê");
                        break;
                    case "btnNhanVien":
                        ShowComingSoon("Nhân viên");
                        break;
                    case "btnCaiDat":
                        ShowComingSoon("Cài đặt");
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở form: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ShowWelcomePanel();
            }
        }

        private void ShowComingSoon(string feature)
        {
            pnlMain.Controls.Clear();

            var comingSoonPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            var lblIcon = new Label
            {
                Text = "🚧",
                Font = new Font("Segoe UI", 72F),
                AutoSize = true
            };
            lblIcon.Location = new Point(
                (pnlMain.Width - lblIcon.Width) / 2,
                (pnlMain.Height - lblIcon.Height) / 2 - 80
            );

            var lblTitle = new Label
            {
                Text = $"Chức năng {feature}",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                AutoSize = true,
                ForeColor = Color.FromArgb(30, 41, 59)
            };
            lblTitle.Location = new Point(
                (pnlMain.Width - lblTitle.Width) / 2,
                lblIcon.Bottom + 20
            );

            var lblDesc = new Label
            {
                Text = "Đang trong quá trình phát triển",
                Font = new Font("Segoe UI", 12F),
                AutoSize = true,
                ForeColor = Color.FromArgb(100, 116, 139)
            };
            lblDesc.Location = new Point(
                (pnlMain.Width - lblDesc.Width) / 2,
                lblTitle.Bottom + 15
            );

            comingSoonPanel.Controls.AddRange(new Control[] { lblIcon, lblTitle, lblDesc });
            pnlMain.Controls.Add(comingSoonPanel);
        }

        private void OpenChildForm(Form childForm)
        {
            if (childForm == null) return;

            // Clear main panel
            pnlMain.Controls.Clear();

            // Store reference
            _activeForm = childForm;

            // Configure child form
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            // Add to panel
            pnlMain.Controls.Add(childForm);
            pnlMain.Tag = childForm;

            // Show and bring to front
            childForm.Show();
            childForm.BringToFront();
        }

        private void ActivateButton(Button button)
        {
            // Reset all buttons
            foreach (Control ctrl in pnlSidebar.Controls)
            {
                if (ctrl is Button btn && btn != button)
                {
                    btn.BackColor = Color.Transparent;
                    btn.ForeColor = Color.White;
                }
            }

            // Activate clicked button
            button.BackColor = Color.FromArgb(51, 65, 85);
            button.ForeColor = Color.White;
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
                // Stop timer
                _refreshTimer?.Stop();
                _refreshTimer?.Dispose();

                // Close active form
                if (_activeForm != null)
                {
                    _activeForm.Close();
                    _activeForm.Dispose();
                    _activeForm = null;
                }

                // Clear session
                MaNV = 0;
                TenNV = null;
                ChucVu = null;

                // Show login form
                try
                {
                    var loginForm = Program.GetService<LoginForm>();
                    loginForm.Show();
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi đăng xuất: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion

        #region Detail Panel Management

        public void UpdateDetailPanel(string title, Control content, int width = 450)
        {
            pnlDetail.Width = width;

            // Clear existing controls except title label
            pnlDetail.Controls.Clear();

            // Add title
            var lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(15, 15),
                AutoSize = true
            };
            pnlDetail.Controls.Add(lblTitle);

            // Add separator
            var separator = new Panel
            {
                Location = new Point(15, 50),
                Size = new Size(pnlDetail.Width - 30, 2),
                BackColor = Color.FromArgb(226, 232, 240)
            };
            pnlDetail.Controls.Add(separator);

            // Add content
            if (content != null)
            {
                content.Location = new Point(15, 65);
                content.Size = new Size(pnlDetail.Width - 30, pnlDetail.Height - 80);
                pnlDetail.Controls.Add(content);
            }

            // Show panel
            pnlDetail.Visible = true;
            pnlDetail.BringToFront();
        }

        public void HideDetailPanel()
        {
            pnlDetail.Visible = false;
        }

        #endregion

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Stop timer
            _refreshTimer?.Stop();
            _refreshTimer?.Dispose();

            // Close active form
            if (_activeForm != null)
            {
                _activeForm.Close();
                _activeForm.Dispose();
            }

            base.OnFormClosing(e);
        }
    }
}