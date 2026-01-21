using Billiard.BLL.Services;
using Billiard.BLL.Services.HoaDonServices;
using Billiard.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.QLBan
{
    public partial class ThemDichVuForm : Form
    {
        private readonly DichVuService _dichVuService;
        private readonly HoaDonService _hoaDonService;
        private readonly int _maHoaDon;
        private List<DichVu> _allServices;
        private string _currentCategory = "all";
        private FlowLayoutPanel flpServices;
        private TextBox txtSearch;
        private Label lblSelectedCount;
        private Dictionary<int, int> _selectedServices = new Dictionary<int, int>();
        private Panel pnlBottom;

        // Debounce search
        private System.Windows.Forms.Timer _searchDebounceTimer;
        private const int SEARCH_DEBOUNCE_MS = 300;

        public ThemDichVuForm(DichVuService dichVuService, HoaDonService hoaDonService, int maHoaDon)
        {
            _dichVuService = dichVuService;
            _hoaDonService = hoaDonService;
            _maHoaDon = maHoaDon;

            InitializeComponent();
            InitializeCustomComponents();
            InitializeSearchDebounce();
            this.Load += ThemDichVuForm_Load;
        }

        private void InitializeSearchDebounce()
        {
            _searchDebounceTimer = new System.Windows.Forms.Timer
            {
                Interval = SEARCH_DEBOUNCE_MS
            };
            _searchDebounceTimer.Tick += (s, e) =>
            {
                _searchDebounceTimer.Stop();
                ApplyFiltersOptimized();
            };
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Thêm dịch vụ";
            this.Size = new Size(950, 700);
            this.MinimumSize = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = true;

            // Enable double buffering
            this.DoubleBuffered = true;

            this.Resize += (s, e) =>
            {
                if (_allServices != null && _allServices.Count > 0)
                {
                    ApplyFiltersOptimized();
                }
                UpdateBottomButtonPositions();
            };

            // Header Panel
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(99, 102, 241),
                Padding = new Padding(20, 15, 20, 15)
            };

            var lblTitle = new Label
            {
                Text = "Thêm dịch vụ",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Left,
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblTitle);

            // Search Panel
            var pnlSearch = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White,
                Padding = new Padding(20, 10, 20, 10)
            };

            txtSearch = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11F),
                PlaceholderText = "Tìm kiếm dịch vụ..."
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;
            pnlSearch.Controls.Add(txtSearch);

            // Category Panel
            var pnlCategory = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(248, 250, 252),
                Padding = new Padding(20, 10, 20, 10)
            };

            var flpCategory = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = false,
                WrapContents = true
            };

            var categories = new[]
            {
                ("all", "Tất cả"),
                ("Đồ uống", "Đồ uống"),
                ("Đồ ăn", "Đồ ăn"),
                ("Khác", "Khác")
            };

            foreach (var (value, text) in categories)
            {
                var btn = CreateCategoryButton(text, value);
                if (value == "all")
                {
                    btn.BackColor = Color.FromArgb(99, 102, 241);
                    btn.ForeColor = Color.White;
                }
                flpCategory.Controls.Add(btn);
            }

            pnlCategory.Controls.Add(flpCategory);

            // Services Panel - Enable double buffering
            flpServices = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.White,
                Padding = new Padding(15)
            };
            // Enable double buffering cho FlowLayoutPanel
            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null, flpServices, new object[] { true });

            // Bottom Panel
            pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                BackColor = Color.FromArgb(248, 250, 252),
                Padding = new Padding(20, 15, 20, 15)
            };

            lblSelectedCount = new Label
            {
                Text = "Chưa chọn dịch vụ nào",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                Location = new Point(20, 25),
                Anchor = AnchorStyles.Left | AnchorStyles.Top
            };

            var btnConfirm = new Button
            {
                Text = "Xác nhận thêm",
                Width = 180,
                Height = 40,
                BackColor = Color.FromArgb(34, 197, 94),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };
            btnConfirm.FlatAppearance.BorderSize = 0;
            btnConfirm.Click += BtnConfirm_Click;

            var btnCancel = new Button
            {
                Text = "Hủy",
                Width = 120,
                Height = 40,
                BackColor = Color.FromArgb(239, 68, 68),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            pnlBottom.Controls.AddRange(new Control[] { lblSelectedCount, btnCancel, btnConfirm });

            this.Controls.Add(flpServices);
            this.Controls.Add(pnlCategory);
            this.Controls.Add(pnlSearch);
            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlBottom);

            this.Load += (s, e) => UpdateBottomButtonPositions();
        }

        private void UpdateBottomButtonPositions()
        {
            if (pnlBottom != null)
            {
                var btnConfirm = pnlBottom.Controls.OfType<Button>().FirstOrDefault(b => b.Text.Contains("Xác nhận"));
                var btnCancel = pnlBottom.Controls.OfType<Button>().FirstOrDefault(b => b.Text.Contains("Hủy"));

                if (btnConfirm != null && btnCancel != null)
                {
                    int rightPadding = 20;
                    int topPadding = 15;
                    int panelWidth = pnlBottom.ClientSize.Width;

                    btnConfirm.Location = new Point(panelWidth - btnConfirm.Width - rightPadding, topPadding);
                    btnCancel.Location = new Point(btnConfirm.Location.X - btnCancel.Width - 10, topPadding);
                }

                if (lblSelectedCount != null)
                {
                    lblSelectedCount.Location = new Point(20, (pnlBottom.Height - lblSelectedCount.Height) / 2);
                }
            }
        }

        private Button CreateCategoryButton(string text, string value)
        {
            var btn = new Button
            {
                Text = text,
                Width = 110,
                Height = 38,
                BackColor = Color.FromArgb(226, 232, 240),
                ForeColor = Color.FromArgb(51, 65, 85),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 10, 0),
                Tag = value
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += CategoryButton_Click;

            btn.MouseEnter += (s, e) =>
            {
                if (btn.BackColor != Color.FromArgb(99, 102, 241))
                {
                    btn.BackColor = Color.FromArgb(203, 213, 225);
                }
            };
            btn.MouseLeave += (s, e) =>
            {
                if (btn.BackColor != Color.FromArgb(99, 102, 241))
                {
                    btn.BackColor = Color.FromArgb(226, 232, 240);
                }
            };

            return btn;
        }

        private async void ThemDichVuForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Hiển thị loading state
                ShowLoadingState();

                await LoadServices();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dịch vụ: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowLoadingState()
        {
            flpServices.Controls.Clear();

            var pnlLoading = new Panel
            {
                Width = flpServices.ClientSize.Width - 40,
                Height = 200,
                BackColor = Color.White,
                Margin = new Padding(20)
            };

            var lblLoading = new Label
            {
                Text = "⏳ Đang tải dịch vụ...",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true
            };
            lblLoading.Location = new Point((pnlLoading.Width - lblLoading.Width) / 2, 80);

            pnlLoading.Controls.Add(lblLoading);
            flpServices.Controls.Add(pnlLoading);
        }

        private async System.Threading.Tasks.Task LoadServices()
        {
            // Load trong background thread
            _allServices = await System.Threading.Tasks.Task.Run(() =>
            {
                return _dichVuService.GetAllDichVu()
                    .Where(d => d.TrangThai == "Còn hàng")
                    .ToList();
            });

            ApplyFiltersOptimized();
        }

        private void ApplyFiltersOptimized()
        {
            flpServices.SuspendLayout();
            flpServices.Controls.Clear();

            var filtered = _allServices.AsEnumerable();

            if (_currentCategory != "all")
            {
                filtered = filtered.Where(d => d.Loai == _currentCategory);
            }

            var searchText = txtSearch.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(searchText))
            {
                filtered = filtered.Where(d => d.TenDv.ToLower().Contains(searchText));
            }

            var services = filtered.ToList();

            if (services.Count == 0)
            {
                ShowEmptyState();
            }
            else
            {
                // Batch create cards
                var cards = new List<Panel>(services.Count);
                foreach (var service in services)
                {
                    var card = CreateServiceCard(service);
                    cards.Add(card);
                }

                // Add all at once
                flpServices.Controls.AddRange(cards.ToArray());
            }

            flpServices.ResumeLayout(false);
            flpServices.PerformLayout();
        }

        private void ShowEmptyState()
        {
            var pnlEmpty = new Panel
            {
                Width = flpServices.ClientSize.Width - 40,
                Height = 250,
                BackColor = Color.White,
                Margin = new Padding(20)
            };

            var lblIcon = new Label
            {
                Text = "🍽️",
                Font = new Font("Segoe UI", 56F),
                AutoSize = true
            };

            var lblText = new Label
            {
                Text = "Không tìm thấy dịch vụ nào",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true
            };

            var lblHint = new Label
            {
                Text = "Thử thay đổi bộ lọc hoặc từ khóa tìm kiếm",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(148, 163, 184),
                AutoSize = true
            };

            lblIcon.Location = new Point((pnlEmpty.Width - lblIcon.Width) / 2, 50);
            lblText.Location = new Point((pnlEmpty.Width - lblText.Width) / 2, 140);
            lblHint.Location = new Point((pnlEmpty.Width - lblHint.Width) / 2, 175);

            pnlEmpty.Controls.AddRange(new Control[] { lblIcon, lblText, lblHint });
            flpServices.Controls.Add(pnlEmpty);
        }

        private Panel CreateServiceCard(DichVu service)
        {
            int paddingTotal = flpServices.Padding.Left + flpServices.Padding.Right;
            int scrollBarWidth = 25;
            int availableWidth = flpServices.ClientSize.Width - paddingTotal - scrollBarWidth;

            int cardWidth = (availableWidth / 4) - 20;

            if (cardWidth < 180) cardWidth = (availableWidth / 2) - 20; 

            var card = new Panel
            {
                Width = cardWidth,
                Height = 270, 
                Margin = new Padding(10),
                BackColor = Color.White,
                Tag = service
            };

            card.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(226, 232, 240), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                }
            };

            // 1. Hình ảnh (Giảm chiều cao xuống để phù hợp tổng thể 280)
            var picImage = new PictureBox
            {
                Size = new Size(cardWidth - 16, 120),
                Location = new Point(8, 8),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(248, 250, 252)
            };

            if (!string.IsNullOrEmpty(service.HinhAnh))
            {
                LoadServiceImageAsync(picImage, service.HinhAnh);
            }

            // 2. Tên dịch vụ (Giới hạn 2 dòng, font nhỏ hơn một chút)
            var lblName = new Label
            {
                Text = service.TenDv,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(8, 130),
                Size = new Size(cardWidth - 16, 25),
                TextAlign = ContentAlignment.TopCenter,
                AutoEllipsis = true
            };

            // 3. Giá tiền
            var lblPrice = new Label
            {
                Text = $"{service.Gia:N0} đ",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 38, 38),
                Location = new Point(8, 150),
                Size = new Size(cardWidth - 16, 25),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var currentQty = _selectedServices.ContainsKey(service.MaDv) ? _selectedServices[service.MaDv] : 1;

            // 4. Bộ điều khiển số lượng (Nhỏ gọn hơn)
            int controlsY = 180;
            int centerShift = (cardWidth - (28 + 40 + 28)) / 2;

            var btnMinus = new Button
            {
                Text = "−",
                Width = 28,
                Height = 28,
                Location = new Point(centerShift, controlsY+2),
                BackColor = Color.FromArgb(241, 245, 249),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Tag = service.MaDv
            };
            btnMinus.FlatAppearance.BorderSize = 0;
            btnMinus.Click += BtnMinus_Click;

            var txtQty = new TextBox
            {
                Width = 40,
                Height = 28,
                Location = new Point(centerShift + 28, controlsY + 1),
                Text = currentQty.ToString(),
                TextAlign = HorizontalAlignment.Center,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ReadOnly = true,
                Name = $"qty_{service.MaDv}"
            };

            var btnPlus = new Button
            {
                Text = "+",
                Width = 28,
                Height = 28,
                Location = new Point(centerShift + 70, controlsY+2),
                BackColor = Color.FromArgb(241, 245, 249),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Tag = service.MaDv
            };
            btnPlus.FlatAppearance.BorderSize = 0;
            btnPlus.Click += BtnPlus_Click;

            // 5. Nút bấm (Sát dưới cùng)
            var btnAdd = new Button
            {
                Text = _selectedServices.ContainsKey(service.MaDv) ? "Đã chọn" : "Thêm",
                Size = new Size(cardWidth - 24, 32),
                Location = new Point(12, 225),
                BackColor = _selectedServices.ContainsKey(service.MaDv) ? Color.FromArgb(34, 197, 94) : Color.FromArgb(99, 102, 241),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Tag = service.MaDv
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += BtnAddService_Click;

            card.Controls.AddRange(new Control[] { picImage, lblName, lblPrice, btnMinus, txtQty, btnPlus, btnAdd });
            return card;
        }

        private async void LoadServiceImageAsync(PictureBox picImage, string hinhAnh)
        {
            try
            {
                var projectRoot = AppDomain.CurrentDomain.BaseDirectory;
                var imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", hinhAnh);

                if (System.IO.File.Exists(imagePath))
                {
                    // Load image asynchronously
                    var img = await System.Threading.Tasks.Task.Run(() =>
                    {
                        using (var original = Image.FromFile(imagePath))
                        {
                            return new Bitmap(original);
                        }
                    });

                    if (picImage != null && !picImage.IsDisposed)
                    {
                        picImage.Image = img;
                    }
                }
            }
            catch
            {
                // Ignore image loading errors
            }
        }

        private void BtnMinus_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var maDv = (int)btn.Tag;
            var card = btn.Parent as Panel;
            var txtQty = card.Controls.Find($"qty_{maDv}", false).FirstOrDefault() as TextBox;

            if (txtQty != null)
            {
                var currentQty = int.Parse(txtQty.Text);
                if (currentQty > 1)
                {
                    currentQty--;
                    txtQty.Text = currentQty.ToString();

                    var btnAdd = card.Controls.OfType<Button>().FirstOrDefault(b => b.Text.Contains("Đã chọn"));
                    if (btnAdd != null && _selectedServices.ContainsKey(maDv))
                    {
                        _selectedServices[maDv] = currentQty;
                        UpdateSelectedCount();
                    }
                }
            }
        }

        private void BtnPlus_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var maDv = (int)btn.Tag;
            var card = btn.Parent as Panel;
            var txtQty = card.Controls.Find($"qty_{maDv}", false).FirstOrDefault() as TextBox;

            if (txtQty != null)
            {
                var currentQty = int.Parse(txtQty.Text);
                if (currentQty < 99)
                {
                    currentQty++;
                    txtQty.Text = currentQty.ToString();

                    var btnAdd = card.Controls.OfType<Button>().FirstOrDefault(b => b.Text.Contains("Đã chọn"));
                    if (btnAdd != null && _selectedServices.ContainsKey(maDv))
                    {
                        _selectedServices[maDv] = currentQty;
                        UpdateSelectedCount();
                    }
                }
            }
        }

        private void BtnAddService_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var maDv = (int)btn.Tag;
            var card = btn.Parent as Panel;
            var txtQty = card.Controls.Find($"qty_{maDv}", false).FirstOrDefault() as TextBox;

            if (txtQty == null) return;

            if (_selectedServices.ContainsKey(maDv))
            {
                // Bỏ chọn
                _selectedServices.Remove(maDv);
                btn.BackColor = Color.FromArgb(99, 102, 241);
                btn.Text = "Thêm";
                txtQty.Text = "1";
            }
            else
            {
                // Thêm mới
                var qty = int.Parse(txtQty.Text);
                _selectedServices[maDv] = qty;
                btn.BackColor = Color.FromArgb(34, 197, 94);
                btn.Text = "Đã chọn";
            }

            // Căn chỉnh lại vị trí X để nút luôn bám lề phải card sau khi thay đổi kích thước
            // Khoảng cách lề phải là 15px
            btn.Location = new Point(card.Width - btn.Width - 15, btn.Location.Y);

            UpdateSelectedCount();
        }

        private void UpdateSelectedCount()
        {
            if (lblSelectedCount != null)
            {
                if (_selectedServices.Count == 0)
                {
                    lblSelectedCount.Text = "Chưa chọn dịch vụ nào";
                    lblSelectedCount.ForeColor = Color.FromArgb(100, 116, 139);
                }
                else
                {
                    var totalQty = _selectedServices.Values.Sum();
                    lblSelectedCount.Text = $"Đã chọn: {_selectedServices.Count} dịch vụ ({totalQty} món)";
                    lblSelectedCount.ForeColor = Color.FromArgb(99, 102, 241);
                }
            }
        }

        private async void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (_selectedServices.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một dịch vụ!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var totalQty = _selectedServices.Values.Sum();
            var result = MessageBox.Show(
                $"Xác nhận thêm {_selectedServices.Count} dịch vụ ({totalQty} món) vào hóa đơn?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            this.Enabled = false;

            try
            {
                var failedServices = new List<(string Name, string Reason)>();
                var successCount = 0;
                var totalItems = 0;

                foreach (var item in _selectedServices)
                {
                    try
                    {
                        var success = await _hoaDonService.AddServiceToInvoiceAsync(
                            _maHoaDon, item.Key, item.Value);

                        if (success)
                        {
                            successCount++;
                            totalItems += item.Value;
                        }
                        else
                        {
                            var service = _allServices.FirstOrDefault(s => s.MaDv == item.Key);
                            if (service != null)
                            {
                                failedServices.Add((service.TenDv, "Có thể hết hàng hoặc lỗi hệ thống"));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var service = _allServices.FirstOrDefault(s => s.MaDv == item.Key);
                        if (service != null)
                        {
                            failedServices.Add((service.TenDv, ex.Message));
                        }
                    }
                }

                this.Enabled = true;

                if (successCount > 0 && failedServices.Count == 0)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else if (successCount > 0 && failedServices.Count > 0)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    var failedList = string.Join("\n• ",
                        failedServices.Select(f => $"{f.Name} ({f.Reason})"));

                    MessageBox.Show(
                        $"Không thể thêm bất kỳ dịch vụ nào!\n\n" +
                        $"Chi tiết:\n• {failedList}",
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                this.Enabled = true;

                MessageBox.Show(
                    $"Lỗi nghiêm trọng khi thêm dịch vụ:\n\n{ex.Message}\n\n" +
                    $"Vui lòng thử lại hoặc liên hệ quản trị viên.",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void CategoryButton_Click(object sender, EventArgs e)
        {
            var clickedBtn = sender as Button;
            _currentCategory = clickedBtn.Tag.ToString();

            foreach (Control ctrl in clickedBtn.Parent.Controls)
            {
                if (ctrl is Button btn)
                {
                    if (btn == clickedBtn)
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

            ApplyFiltersOptimized();
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            // Debounce search
            _searchDebounceTimer.Stop();
            _searchDebounceTimer.Start();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _searchDebounceTimer?.Stop();
                _searchDebounceTimer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}