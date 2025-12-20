using Billiard.BLL.Services.QLBan;
using Billiard.BLL.Services.HoaDonServices;
using Billiard.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.QLBan
{
    public partial class QLBanForm : Form
    {
        private readonly BanBiaService _banBiaService;
        private MainForm _mainForm;
        private List<BanBium> _allTables;
        private string _currentAreaFilter = "all";
        private string _currentStatusFilter = "all";
        private string _currentTypeFilter = "all";
        private System.Windows.Forms.Timer _refreshTimer;

        // Panel chi tiết bàn
        private Panel pnlDetailContainer;
        private BanChiTietControl _chiTietControl;
        private bool _isDetailVisible = false;

        // Thêm biến để quản lý kích thước động
        private const int DETAIL_PANEL_WIDTH = 430;
        private const int CARD_WIDTH = 250;

        public QLBanForm(BanBiaService banBiaService)
        {
            _banBiaService = banBiaService;
            InitializeComponent();
            InitializeRefreshTimer();
            InitializeDetailPanel();

            this.AutoScroll = false;
            this.AutoSize = false;
        }

        private void InitializeDetailPanel()
        {
            // Tạo panel container
            pnlDetailContainer = new Panel
            {
                Width = DETAIL_PANEL_WIDTH,
                BackColor = Color.White,
                Visible = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom
            };

            // Nút đóng
            var btnClose = new Button
            {
                Text = "✕",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(239, 68, 68),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(40, 40),
                Location = new Point(DETAIL_PANEL_WIDTH - 50, 10),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 38, 38);
            btnClose.Click += (s, e) => HideDetailPanel();

            var toolTip = new ToolTip();
            toolTip.SetToolTip(btnClose, "Đóng chi tiết bàn (ESC)");

            // Content panel - BanChiTietControl sẽ được add vào đây
            var pnlContent = new Panel
            {
                Location = new Point(0, 55),
                Width = DETAIL_PANEL_WIDTH,
                AutoScroll = true,
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };
            pnlContent.Name = "pnlDetailContent";

            pnlDetailContainer.Controls.Add(pnlContent);
            pnlDetailContainer.Controls.Add(btnClose);

            // Loại bỏ shadow effect - chỉ giữ lại border đơn giản
            pnlDetailContainer.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(226, 232, 240), 2))
                {
                    e.Graphics.DrawLine(pen, 0, 0, 0, pnlDetailContainer.Height);
                }
            };

            this.Controls.Add(pnlDetailContainer);
            PositionDetailPanel();
            pnlDetailContainer.BringToFront();

            this.Resize += (s, e) =>
            {
                if (_isDetailVisible)
                {
                    PositionDetailPanel();
                }
            };

            // ESC để đóng
            this.KeyPreview = true;
            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape && _isDetailVisible)
                {
                    HideDetailPanel();
                    e.Handled = true;
                }
            };
        }
        private void PositionDetailPanel()
        {
            if (pnlDetailContainer == null) return;

            var targetX = this.ClientSize.Width - DETAIL_PANEL_WIDTH;
            var targetY = 0; // Bắt đầu từ đầu form
            var targetHeight = this.ClientSize.Height; // Toàn bộ chiều cao form

            pnlDetailContainer.Location = new Point(targetX, targetY);
            pnlDetailContainer.Height = targetHeight;

            // Cập nhật lại height cho pnlContent
            var pnlContent = pnlDetailContainer.Controls.Find("pnlDetailContent", false).FirstOrDefault() as Panel;
            if (pnlContent != null)
            {
                pnlContent.Location = new Point(0, 55); // Đẩy xuống dưới nút đóng
                pnlContent.Height = targetHeight - 55;
            }
        }

        private void InitializeRefreshTimer()
        {
            _refreshTimer = new System.Windows.Forms.Timer();
            _refreshTimer.Interval = 30000;
            _refreshTimer.Tick += async (s, e) => await LoadBanBia();
        }

        private void SetupPermissions()
        {
            if (_mainForm == null) return;

            var chucVu = _mainForm.ChucVu;
            bool isAdmin = chucVu == "Admin";
            bool isQuanLy = chucVu == "Quản lý" || isAdmin;
            bool isThuNgan = chucVu == "Thu ngân" || isQuanLy;

            btnXemSoDo.Visible = true;
            btnXemBanDat.Visible = isThuNgan;
            btnDatBan.Visible = isThuNgan;
            btnThemBan.Visible = isQuanLy;
        }

        public void SetMainForm(MainForm mainForm)
        {
            _mainForm = mainForm;
            SetupPermissions();
        }

        private async void QLBanForm_Load(object sender, EventArgs e)
        {
            try
            {
                _refreshTimer.Start();
                await LoadBanBia();
                this.PerformLayout();
                this.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadBanBia()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                flpBanBia.Controls.Clear();
                flpBanBia.SuspendLayout();

                _allTables = await _banBiaService.GetAllTablesAsync();
                ApplyFilters();

                flpBanBia.ResumeLayout();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Lỗi tải danh sách bàn: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilters()
        {
            flpBanBia.SuspendLayout();
            flpBanBia.Controls.Clear();

            var filteredTables = _allTables.AsEnumerable();

            if (_currentAreaFilter != "all")
            {
                filteredTables = filteredTables.Where(b =>
                    b.MaKhuVucNavigation?.TenKhuVuc == _currentAreaFilter);
            }

            if (_currentStatusFilter != "all")
            {
                filteredTables = filteredTables.Where(b => b.TrangThai == _currentStatusFilter);
            }

            if (_currentTypeFilter != "all")
            {
                filteredTables = filteredTables.Where(b =>
                    b.MaLoaiNavigation?.TenLoai == _currentTypeFilter);
            }

            var searchText = txtSearch.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(searchText))
            {
                filteredTables = filteredTables.Where(b =>
                    b.TenBan.ToLower().Contains(searchText));
            }

            var tables = filteredTables.ToList();

            if (tables.Count == 0)
            {
                ShowEmptyState();
            }
            else
            {
                foreach (var ban in tables)
                {
                    var card = CreateTableCard(ban);
                    flpBanBia.Controls.Add(card);
                }
            }

            flpBanBia.ResumeLayout();
            flpBanBia.PerformLayout();
        }

        private void ShowEmptyState()
        {
            var pnlEmpty = new Panel
            {
                Size = new Size(flpBanBia.Width - 40, 300),
                BackColor = Color.White
            };

            var lblIcon = new Label
            {
                Text = "🎱",
                Font = new Font("Segoe UI", 48F),
                AutoSize = true
            };
            lblIcon.Location = new Point(
                (pnlEmpty.Width - lblIcon.Width) / 2,
                80
            );

            var lblTitle = new Label
            {
                Text = "Không tìm thấy bàn nào",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                AutoSize = true
            };
            lblTitle.Location = new Point(
                (pnlEmpty.Width - lblTitle.Width) / 2,
                160
            );

            var lblDesc = new Label
            {
                Text = "Thử thay đổi bộ lọc hoặc tìm kiếm khác",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.Gray,
                AutoSize = true
            };
            lblDesc.Location = new Point(
                (pnlEmpty.Width - lblDesc.Width) / 2,
                195
            );

            pnlEmpty.Controls.AddRange(new Control[] { lblIcon, lblTitle, lblDesc });
            flpBanBia.Controls.Add(pnlEmpty);
        }

        private Panel CreateTableCard(BanBium ban)
        {
            var card = new Panel
            {
                Width = CARD_WIDTH,
                Height = 280,
                Margin = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand,
                Tag = ban
            };

            card.BackColor = ban.TrangThai switch
            {
                "Trống" => Color.FromArgb(240, 253, 244),
                "Đang chơi" => Color.FromArgb(254, 242, 242),
                "Đã đặt" => Color.FromArgb(255, 251, 235),
                _ => Color.White
            };

            card.Paint += (s, e) =>
            {
                var borderColor = ban.TrangThai switch
                {
                    "Trống" => Color.FromArgb(34, 197, 94),
                    "Đang chơi" => Color.FromArgb(239, 68, 68),
                    "Đã đặt" => Color.FromArgb(234, 179, 8),
                    _ => Color.Gray
                };

                using (var pen = new Pen(borderColor, 3))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                }
            };

            var pnlImage = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(CARD_WIDTH, 140),
                BackColor = Color.FromArgb(248, 250, 252)
            };

            if (!string.IsNullOrEmpty(ban.HinhAnh))
            {
                try
                {
                    var projectRoot = Directory.GetParent(Directory.GetParent(Directory.GetParent(
                        Directory.GetParent(Application.StartupPath).FullName).FullName).FullName).FullName;
                    var imagePath = Path.Combine(projectRoot, "Forms", "Resources", "img", "tables", ban.HinhAnh);

                    if (File.Exists(imagePath))
                    {
                        var picTable = new PictureBox
                        {
                            Size = new Size(CARD_WIDTH, 140),
                            Location = new Point(0, 0),
                            SizeMode = PictureBoxSizeMode.Zoom,
                            BackColor = Color.FromArgb(248, 250, 252)
                        };

                        using (var img = Image.FromFile(imagePath))
                        {
                            picTable.Image = new Bitmap(img);
                        }

                        pnlImage.Controls.Add(picTable);
                    }
                    else
                    {
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

            var lblStatus = new Label
            {
                Text = ban.TrangThai,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ban.TrangThai switch
                {
                    "Trống" => Color.FromArgb(34, 197, 94),
                    "Đang chơi" => Color.FromArgb(239, 68, 68),
                    "Đã đặt" => Color.FromArgb(234, 179, 8),
                    _ => Color.Gray
                },
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(85, 28),
                Location = new Point(10, 10)
            };
            pnlImage.Controls.Add(lblStatus);

            if (ban.MaKhuVucNavigation?.TenKhuVuc == "VIP")
            {
                var lblVIP = new Label
                {
                    Text = "⭐ VIP",
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(168, 85, 247),
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Size = new Size(75, 28),
                    Location = new Point(CARD_WIDTH - 85, 10)
                };
                pnlImage.Controls.Add(lblVIP);
            }

            var btnEdit = new Button
            {
                Text = "✏️",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Size = new Size(35, 35),
                Location = new Point(CARD_WIDTH - 45, 95),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Tag = ban,
                TabStop = false
            };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);

            btnEdit.Click += async (s, e) =>
            {
                await ChinhSuaBan(ban);
            };

            pnlImage.Controls.Add(btnEdit);
            btnEdit.BringToFront();

            var lblName = new Label
            {
                Text = ban.TenBan,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 150),
                Size = new Size(CARD_WIDTH, 32)
            };

            var lblInfo = new Label
            {
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 185),
                Size = new Size(CARD_WIDTH, 22)
            };

            if (ban.TrangThai == "Đang chơi" && ban.GioBatDau.HasValue)
            {
                var duration = DateTime.Now - ban.GioBatDau.Value;
                lblInfo.Text = $"⏱️ {(int)duration.TotalHours}h {duration.Minutes}m";
                lblInfo.ForeColor = Color.FromArgb(239, 68, 68);
                lblInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

                var lblCustomer = new Label
                {
                    Text = $"👤 {ban.MaKhNavigation?.TenKh ?? "Khách lẻ"}",
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = Color.FromArgb(71, 85, 105),
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(0, 210),
                    Size = new Size(CARD_WIDTH, 22)
                };
                card.Controls.Add(lblCustomer);
            }
            else if (ban.TrangThai == "Đã đặt")
            {
                lblInfo.Text = $"👤 {ban.MaKhNavigation?.TenKh ?? "Khách đặt"}";
            }
            else
            {
                lblInfo.Text = $"📍 {ban.MaKhuVucNavigation?.TenKhuVuc ?? "Khu vực"}";
            }

            var lblPrice = new Label
            {
                Text = $"{ban.MaLoaiNavigation?.GiaGio:N0} đ/giờ",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(99, 102, 241),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 235),
                Size = new Size(CARD_WIDTH, 28)
            };

            card.Controls.AddRange(new Control[] { pnlImage, lblName, lblInfo, lblPrice });

            EventHandler clickHandler = (s, e) => ShowTableDetail(ban);
            card.Click += clickHandler;

            foreach (Control ctrl in card.Controls)
            {
                if (ctrl == pnlImage)
                {
                    foreach (Control subCtrl in ctrl.Controls)
                    {
                        if (subCtrl != btnEdit)
                        {
                            subCtrl.Click += clickHandler;
                        }
                    }
                }
                else
                {
                    ctrl.Click += clickHandler;
                }
            }

            card.MouseEnter += (s, e) =>
            {
                card.BorderStyle = BorderStyle.Fixed3D;
                var currentColor = card.BackColor;
                card.BackColor = Color.FromArgb(
                    Math.Max(0, currentColor.R - 10),
                    Math.Max(0, currentColor.G - 10),
                    Math.Max(0, currentColor.B - 10)
                );
            };

            card.MouseLeave += (s, e) =>
            {
                card.BorderStyle = BorderStyle.FixedSingle;
                card.BackColor = ban.TrangThai switch
                {
                    "Trống" => Color.FromArgb(240, 253, 244),
                    "Đang chơi" => Color.FromArgb(254, 242, 242),
                    "Đã đặt" => Color.FromArgb(255, 251, 235),
                    _ => Color.White
                };
            };

            return card;
        }

        private async Task ChinhSuaBan(BanBium ban)
        {
            try
            {
                var loaiBanService = Program.GetService<LoaiBanService>();
                var khuVucService = Program.GetService<KhuVucService>();

                using (var chinhSuaForm = new ChinhSuaBanForm(_banBiaService, loaiBanService, khuVucService, ban))
                {
                    var result = chinhSuaForm.ShowDialog(this);

                    if (result == DialogResult.OK)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        await LoadBanBia();
                        this.Cursor = Cursors.Default;

                        MessageBox.Show("Đã cập nhật bàn thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Lỗi khi mở form chỉnh sửa: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddDefaultTableIcon(Panel pnlImage)
        {
            var lblIcon = new Label
            {
                Text = "🎱",
                Font = new Font("Segoe UI", 56F),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(CARD_WIDTH, 140),
                BackColor = Color.Transparent
            };
            pnlImage.Controls.Add(lblIcon);
        }

        private async void ShowTableDetail(BanBium ban)
        {
            if (_mainForm == null) return;

            try
            {
                var hoaDonService = Program.GetService<HoaDonService>();

                var pnlContent = pnlDetailContainer.Controls.Find("pnlDetailContent", false).FirstOrDefault() as Panel;
                if (pnlContent == null) return;

                pnlContent.Controls.Clear();

                // Tạo BanChiTietControl mới
                _chiTietControl = new BanChiTietControl(
                    _banBiaService,
                    hoaDonService,
                    ban,
                    _mainForm.MaNV);

                _chiTietControl.Dock = DockStyle.Fill;
                _chiTietControl.BackColor = Color.White;

                _chiTietControl.OnDataChanged += async (s, e) =>
                {
                    await LoadBanBia();
                };

                // Add trực tiếp vào pnlContent
                pnlContent.Controls.Add(_chiTietControl);

                ShowDetailPanel();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị chi tiết bàn: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowDetailPanel()
        {
            if (_isDetailVisible) return;

            _isDetailVisible = true;

            flpBanBia.Padding = new Padding(15, 15, DETAIL_PANEL_WIDTH + 25, 15);

            pnlDetailContainer.Visible = true;
            pnlDetailContainer.Width = DETAIL_PANEL_WIDTH;
            PositionDetailPanel();
            pnlDetailContainer.BringToFront();

            // Animation - bắt đầu từ đầu form
            var startX = this.ClientSize.Width;
            var targetX = this.ClientSize.Width - DETAIL_PANEL_WIDTH;
            pnlDetailContainer.Location = new Point(startX, 0); // Y = 0 để bắt đầu từ đầu

            var timer = new System.Windows.Forms.Timer { Interval = 8 };
            var step = 0;
            var totalSteps = 15;

            timer.Tick += (s, e) =>
            {
                step++;
                var progress = (double)step / totalSteps;
                var easedProgress = 1 - Math.Pow(1 - progress, 3);

                var newX = startX + (int)((targetX - startX) * easedProgress);
                pnlDetailContainer.Location = new Point(newX, 0); // Y = 0
                pnlDetailContainer.Height = this.ClientSize.Height; // Toàn bộ chiều cao

                // Cập nhật lại height của pnlContent
                var pnlContent = pnlDetailContainer.Controls.Find("pnlDetailContent", false).FirstOrDefault() as Panel;
                if (pnlContent != null)
                {
                    pnlContent.Height = pnlDetailContainer.Height - 55;
                }

                if (step >= totalSteps)
                {
                    pnlDetailContainer.Location = new Point(targetX, 0); // Y = 0
                    timer.Stop();
                    timer.Dispose();
                }
            };
            timer.Start();
        }

        private void HideDetailPanel()
        {
            if (!_isDetailVisible) return;

            var timer = new System.Windows.Forms.Timer { Interval = 8 };
            var targetX = this.ClientSize.Width;
            var startX = pnlDetailContainer.Location.X;
            var step = 0;
            var totalSteps = 15;

            timer.Tick += (s, e) =>
            {
                step++;
                var progress = (double)step / totalSteps;
                var easedProgress = Math.Pow(progress, 2);

                var newX = startX + (int)((targetX - startX) * easedProgress);
                pnlDetailContainer.Location = new Point(newX, 0); // Y = 0

                if (step >= totalSteps)
                {
                    pnlDetailContainer.Visible = false;
                    _isDetailVisible = false;
                    timer.Stop();
                    timer.Dispose();

                    flpBanBia.Padding = new Padding(15);

                    var pnlContent = pnlDetailContainer.Controls.Find("pnlDetailContent", false).FirstOrDefault() as Panel;
                    if (pnlContent != null)
                    {
                        pnlContent.Controls.Clear();
                    }
                    _chiTietControl = null;
                }
            };
            timer.Start();
        }

        #region Filter Events

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

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        #endregion

        #region Toolbar Button Events

        private void BtnXemSoDo_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Chức năng 'Xem sơ đồ bàn' đang trong quá trình phát triển.\nVui lòng quay lại sau!",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
        private void BtnXemBanDat_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Chức năng 'Xem bàn đặt' đang trong quá trình phát triển.\nVui lòng quay lại sau!",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private async void BtnDatBan_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Chức năng 'Đặt bàn trước' đang trong quá trình phát triển.\nVui lòng quay lại sau!",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private async void BtnThemBan_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Chức năng 'Thêm bàn mới' đang trong quá trình phát triển.\nVui lòng quay lại sau!",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        #endregion

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _refreshTimer?.Stop();
            _refreshTimer?.Dispose();
            base.OnFormClosing(e);
        }
    }
}