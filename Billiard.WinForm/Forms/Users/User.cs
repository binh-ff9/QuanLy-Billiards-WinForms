using Billiard.BLL.Services;
using Billiard.BLL.Services.QLBan;
using Billiard.DAL.Data;
using Billiard.DAL.Entities;
using Billiard.WinForm.Forms.Auth;
using Billiard.WinForm.Forms.Helpers;
using Billiard.WinForm.Forms.QLBan;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace Billiard.WinForm.Forms.Users
{
    public partial class User : Form
    {
        private Timer posterTimer;

        private ContextMenuStrip accountMenu;


        private readonly BanBiaService _banBiaService;

        private List<BanBium> _allTables;
        private string _currentAreaFilter = "all";
        private string _currentStatusFilter = "all";
        private string _currentTypeFilter = "all";

        public User(BanBiaService banBiaService)
        {
            InitializeComponent();
            WireUpFilterButtons();
            flpTables.AutoScroll = true;          // BẮT BUỘC
            flpTables.WrapContents = true;        // Cho xuống dòng
            flpTables.FlowDirection = FlowDirection.LeftToRight;
            flpTables.AutoSize = false;
            flpTables.Dock = DockStyle.Top;   // hoặc Fill tùy layout
            flpTables.Height = 500;

            flpTables.Margin = new Padding(0);
            flpTables.Padding = new Padding(20);


            _banBiaService = banBiaService;

            SetUpUI();
            LoadTablesAsync();
            AddBanner();
            StartPosterAutoScroll();
            TaoMenu();
            StyleNavButtons();

        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            UpdateAuthButtonUI();
        }
        #region Load Data

        private async Task LoadTablesAsync()
        {
            _allTables = await _banBiaService.GetAllTablesAsync(); // LƯU LẠI DATA GỐC
            ApplyFilters(); // GỌI FILTER SAU KHI LOAD
        }
        #endregion

        #region Filter
        // Filter Data
        private void ApplyFilters()
        {
            if (_allTables == null) return;

            flpTables.SuspendLayout();
            flpTables.Controls.Clear();

            var filteredTables = _allTables.AsEnumerable();

            // Lọc khu vực
            if (_currentAreaFilter != "all")
                filteredTables = filteredTables.Where(b =>
                    b.MaKhuVucNavigation?.TenKhuVuc == _currentAreaFilter);

            // Lọc trạng thái
            if (_currentStatusFilter != "all")
                filteredTables = filteredTables.Where(b =>
                    b.TrangThai == _currentStatusFilter);

            // Lọc loại bàn
            if (_currentTypeFilter != "all")
                filteredTables = filteredTables.Where(b =>
                    b.MaLoaiNavigation?.TenLoai == _currentTypeFilter);

            // Tìm kiếm tên bàn
            string search = txtSearch.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(search))
                filteredTables = filteredTables.Where(b =>
                    b.TenBan.ToLower().Contains(search));

            foreach (var ban in filteredTables)
            {
                var card = new TableCardControl(ban, _banBiaService);
                card.Margin = new Padding(15);
                flpTables.Controls.Add(card);
            }

            flpTables.ResumeLayout();
        }

        private void FilterButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            _currentAreaFilter = button.Tag.ToString();

            foreach (Control ctrl in pnlKhuVucFilters.Controls)
            {
                if (ctrl is Button btn)
                {
                    if (btn == button)
                    {
                        btn.BackColor = Color.FromArgb(99, 102, 241);
                        btn.ForeColor = Color.White;
                    }
                    else
                    {
                        btn.BackColor = Color.FromArgb(226, 232, 240);
                        btn.ForeColor = Color.FromArgb(51, 65, 85);
                    }
                }
            }

            ApplyFilters();
        }

        private void StatusFilterButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            _currentStatusFilter = button.Tag.ToString();

            foreach (Control ctrl in pnlTrangThaiFilters.Controls)
            {
                if (ctrl is Button btn)
                {
                    if (btn == button)
                    {
                        btn.BackColor = Color.FromArgb(99, 102, 241);
                        btn.ForeColor = Color.White;
                    }
                    else
                    {
                        btn.BackColor = Color.FromArgb(226, 232, 240);
                        btn.ForeColor = Color.FromArgb(51, 65, 85);
                    }
                }
            }

            ApplyFilters();
        }

        private void TypeFilterButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            _currentTypeFilter = button.Tag.ToString();

            foreach (Control ctrl in pnlLoaiBanFilters.Controls)
            {
                if (ctrl is Button btn)
                {
                    if (btn == button)
                    {
                        btn.BackColor = Color.FromArgb(99, 102, 241);
                        btn.ForeColor = Color.White;
                    }
                    else
                    {
                        btn.BackColor = Color.FromArgb(226, 232, 240);
                        btn.ForeColor = Color.FromArgb(51, 65, 85);
                    }
                }
            }

            ApplyFilters();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void WireUpFilterButtons()
        {
            // KHU VỰC
            foreach (Button btn in pnlKhuVucFilters.Controls.OfType<Button>())
            {
                btn.Click += FilterButton_Click;
            }

            // TRẠNG THÁI
            foreach (Button btn in pnlTrangThaiFilters.Controls.OfType<Button>())
            {
                btn.Click += StatusFilterButton_Click;
            }

            // LOẠI BÀN
            foreach (Button btn in pnlLoaiBanFilters.Controls.OfType<Button>())
            {
                btn.Click += TypeFilterButton_Click;
            }
        }


        #endregion

        public void SetUpUI()
        {
            // Size
            pnlContent.Resize += (s, e) => ResizeSections();

            // Render Bàn
            for (int i = 1; i <= 30; i++)
            {
                Button table = new Button();
                table.Text = "Bàn " + i;
                table.Size = new Size(140, 90);
                table.Margin = new Padding(10);
                flpTables.Controls.Add(table);
            }

            flpPoster.AutoScroll = true; // ❗ QUAN TRỌNG
            flpPoster.WrapContents = false;
            flpPoster.FlowDirection = FlowDirection.LeftToRight;
            flpPoster.Padding = new Padding(0);
            flpPoster.Margin = new Padding(0);




        }
        private void ResizeSections()
        {
            int width = pnlContent.ClientSize.Width;

            if (pnlContent.VerticalScroll.Visible)
                width -= SystemInformation.VerticalScrollBarWidth;

            width -= pnlContent.Padding.Horizontal;

            flpPoster.Width = width;
            pnlFilter.Width = width;
            flpTables.Width = width;
            pnlFooter.Width = width;
        }


        #region Slider Cho pnlPoster
        // Thêm ảnh

        public void AddBanner()
        {
            flpPoster.Controls.Clear();

            string imgFolder = Path.Combine(Application.StartupPath, "Images/Banner");
            if (!Directory.Exists(imgFolder)) return;

            var files = Directory.GetFiles(imgFolder)
                .Where(f => f.EndsWith(".jpg") || f.EndsWith(".png"))
                .ToList();

            int bannerHeight = flpPoster.Height - 20;

            foreach (var file in files)
            {
                PictureBox pic = new PictureBox();
                pic.Size = new Size(350, bannerHeight);
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                pic.Margin = new Padding(10, 10, 0, 10);
                pic.Image = Image.FromFile(file);

                flpPoster.Controls.Add(pic);
            }

            // Nhân đôi để scroll vô hạn
            int count = flpPoster.Controls.Count;
            for (int i = 0; i < count; i++)
            {
                PictureBox clone = new PictureBox();
                clone.Size = flpPoster.Controls[i].Size;
                clone.SizeMode = PictureBoxSizeMode.StretchImage;
                clone.Margin = flpPoster.Controls[i].Margin;
                clone.Image = ((PictureBox)flpPoster.Controls[i]).Image;

                flpPoster.Controls.Add(clone);
            }
        }

        // Auto Scroll mượt
        private void StartPosterAutoScroll()
        {
            posterTimer = new Timer();
            posterTimer.Interval = 25; // đừng quá thấp kẻo giật
            posterTimer.Tick += PosterTimer_Tick;
            posterTimer.Start();

            flpPoster.MouseEnter += (s, e) => posterTimer.Stop();
            flpPoster.MouseLeave += (s, e) => posterTimer.Start();
        }


        // Logic scroll
        private void PosterTimer_Tick(object sender, EventArgs e)
        {
            int maxScroll = flpPoster.DisplayRectangle.Width - flpPoster.ClientSize.Width;
            int newX = -flpPoster.AutoScrollPosition.X + 2;

            if (newX >= maxScroll / 2)
                newX = 0;

            flpPoster.AutoScrollPosition = new Point(newX, 0);
        }




        #endregion
        #region Button Navigationn

        // Button Order
        private void btnDatMon_Click(object sender, EventArgs e)
        {
            FrmDatMon f = new FrmDatMon();
            f.ShowDialog();
        }

        // Button Support
        private async void btnHoTro_Click(object sender, EventArgs e)
        {
            btnHoTro.Enabled = false;

            //using (HttpClient client = new HttpClient())
            //{
            //    var response = await client.PostAsync("https://yourserver/api/support", null);

            //    if (response.IsSuccessStatusCode)
            //        MessageBox.Show("Yêu cầu hỗ trợ đã gửi. Nhân viên sẽ tới sớm!");
            //    else
            //        MessageBox.Show("Gửi yêu cầu thất bại!");
            //}

            btnHoTro.Enabled = true;

        }
        // Button Login
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (UserSession.IsLoggedIn)
            {
                accountMenu.Show(btnLogin, 0, btnLogin.Height);
                return;
            }

            var loginForm = Program.GetService<LoginForm>();
            var result = loginForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                // ===== ADMIN / NHÂN VIÊN =====
                if (loginForm.LoggedUserType == UserType.NhanVien)
                {
                    var nv = loginForm.LoggedNhanVien;

                    var mainForm = Program.GetService<MainForm>();
                    mainForm.MaNV = nv.MaNv;
                    mainForm.TenNV = nv.TenNv;
                    mainForm.ChucVu = nv.MaNhomNavigation?.TenNhom ?? "Nhân viên";

                    this.Hide();                 // 🔥 ẨN USER FORM
                    mainForm.ShowDialog();       // 🔥 CHỜ ADMIN DÙNG XONG
                    this.Show();                 // 🔥 HIỆN LẠI KHI ADMIN THOÁT

                    UpdateAuthButtonUI();
                }
                // ===== KHÁCH HÀNG =====
                else
                {
                    UpdateAuthButtonUI();
                    _ = LoadTablesAsync();
                }
            }
        }


        #region Context Menu cho btnLogin và Function
        public void TaoMenu()
        {
            accountMenu = new ContextMenuStrip();
            accountMenu.Font = new Font("Segoe UI", 10);

            // Hồ sơ
            var itemProfile = new ToolStripMenuItem(" Hồ sơ cá nhân");
            itemProfile.Image = SystemIcons.Information.ToBitmap();
            itemProfile.Click += (s, e) =>
            {
                var profile = Program.GetService<UserProfileForm>();
                profile.ShowDialog();

                if (profile.CoThayDoiDuLieu)
                {
                    // Nếu có hủy/đặt gì đó, tải lại toàn bộ danh sách bàn ở màn hình chính
                    LoadTablesAsync();
                }
            };

            // Đăng xuất
            var itemLogout = new ToolStripMenuItem(" Đăng xuất");
            itemLogout.Image = SystemIcons.Error.ToBitmap();
            itemLogout.Click += (s, e) => LogoutUser();

            accountMenu.Items.Add(itemProfile);
            accountMenu.Items.Add(new ToolStripSeparator());
            accountMenu.Items.Add(itemLogout);
        }

        // Đăng xuất
        private void LogoutUser()
        {
            var confirm = MessageBox.Show(
                "Bạn có chắc muốn đăng xuất?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                UserSession.Logout();
                UserSession.HangTV = null;   // ✅ XÓA HẠNG
                UpdateAuthButtonUI();

                MessageBox.Show("Đã đăng xuất thành công!", "Thông báo");
            }
        }



        #endregion


        #endregion
        private void User_Load(object sender, EventArgs e)
        {
            UpdateAuthButtonUI();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            // Gọi hàm cập nhật giao diện Auth (nút Đăng nhập/Đăng xuất)
            UpdateAuthButtonUI();
        }

        #region UI

        private void UpdateAuthButtonUI()
        {
            if (UserSession.IsLoggedIn)
            {
                string rankIcon = GetRankIcon(UserSession.HangTV);
                btnLogin.Text = $"{rankIcon} {UserSession.TenKH}";
            }
            else
            {
                btnLogin.Text = "Đăng nhập / Đăng ký";
            }
        }
        private string GetRankIcon(string rank)
        {
            if (string.IsNullOrEmpty(rank)) return "👤";

            rank = rank.ToLower();

            if (rank.Contains("vàng")) return "🥇";
            if (rank.Contains("bạc")) return "🥈";
            if (rank.Contains("đồng")) return "🥉";
            if (rank.Contains("bạch kim")) return "💎";

            return "👤";
        }

        private void StyleNavButtons()
        {
            Color primaryColor = Color.FromArgb(79, 70, 229); // Màu tím indigo
            Color hoverColor = Color.FromArgb(67, 56, 202);   // Màu tím đậm hơn

            ApplyModernStyle(btnSoDoBan, "Sơ đồ bàn", hoverColor, primaryColor);
            //ApplyModernStyle(btnDatMon, "", hoverColor, primaryColor);
            ApplyModernStyle(btnHoTro, "🆘 Hỗ trợ", hoverColor, primaryColor);
            ApplyModernStyle(btnGacha, "Hotline: 0336608117", hoverColor, primaryColor);
            ApplyModernStyle(btnLogin, "Đăng nhập", hoverColor, primaryColor);
        }
        // Style Button
        private void ApplyModernStyle(Button btn, string text, Color hoverColor, Color backColor)
        {
            btn.Text = text;
            btn.BackColor = Color.FromArgb(79, 70, 229);
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            //Cursor = Cursors.Hand;

            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = hoverColor;
            };

            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = backColor;
            };
        }



        #endregion


        #region Event


        #endregion



        private void btnSoDoBan_Click(object sender, EventArgs e)
        {
            // 1. Lấy instance của SoDoBanForm từ ServiceProvider
            var mapForm = Program.GetService<SoDoBanForm>();

            // 2. Bật chế độ khách hàng (Ẩn nút sửa/lưu)
            mapForm.EnableClientMode();

            // 3. Đăng ký sự kiện: Khi khách chọn bàn trên Map
            mapForm.OnTableSelected += (s, tableSelected) =>
            {
                // tableSelected chính là đối tượng BanBium khách vừa click

                // Option A: Mở luôn form đặt bàn cho bàn đó
                MoFormDatBan(tableSelected);

                // Option B: Hoặc chỉ cuộn danh sách bên ngoài tới bàn đó (nếu muốn)
                // ScrollToTable(tableSelected.MaBan);
            };

            // 4. Hiển thị Form
            mapForm.ShowDialog();
        }
        private void MoFormDatBan(BanBium ban)
        {
            // Check đăng nhập
            if (!UserSession.IsLoggedIn)
            {
                MessageBox.Show("Vui lòng đăng nhập để đặt bàn!");
                var login = Program.GetService<LoginForm>();
                if (login.ShowDialog() != DialogResult.OK) return;
            }

            // Logic mở form đặt bàn (giống trong TableCardControl)
            try
            {
                var datBanService = Program.GetService<DatBanService>();
                using (var dialog = new DatBanDialog(datBanService))
                {
                    dialog.SetTableInfo(ban.MaBan, ban.TenBan);
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        // Refresh lại list bàn ở form User nếu cần
                        LoadTablesAsync();
                        MessageBox.Show("Đặt bàn thành công!", "Thông báo");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}
