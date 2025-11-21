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
        private Panel pnlBottom; // Thêm biến cho pnlBottom để dễ truy cập

        public ThemDichVuForm(DichVuService dichVuService, HoaDonService hoaDonService, int maHoaDon)
        {
            _dichVuService = dichVuService;
            _hoaDonService = hoaDonService;
            _maHoaDon = maHoaDon;

            InitializeComponent();
            InitializeCustomComponents();
            this.Load += ThemDichVuForm_Load;
        }

        private void InitializeCustomComponents()
        {
            // Form settings - Responsive
            this.Text = "Thêm dịch vụ";
            this.Size = new Size(950, 700);
            this.MinimumSize = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = true;

            // Handle resize to recalculate card layout and button positions
            this.Resize += (s, e) =>
            {
                if (_allServices != null && _allServices.Count > 0)
                {
                    ApplyFilters();
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
                Text = "📋 Thêm dịch vụ",
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
                PlaceholderText = "🔍 Tìm kiếm dịch vụ..."
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

            // Services Panel
            flpServices = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.White,
                Padding = new Padding(15)
            };

            // Bottom Panel - Responsive buttons
            pnlBottom = new Panel // Gán giá trị cho biến pnlBottom
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                BackColor = Color.FromArgb(248, 250, 252),
                Padding = new Padding(20, 15, 20, 15)
            };

            // Label hiển thị số dịch vụ đã chọn
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
                Text = "✅ Xác nhận thêm",
                Width = 180,
                Height = 40,
                BackColor = Color.FromArgb(34, 197, 94),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };
            // btnConfirm.Location được set trong UpdateBottomButtonPositions
            btnConfirm.FlatAppearance.BorderSize = 0;
            btnConfirm.Click += BtnConfirm_Click;

            var btnCancel = new Button
            {
                Text = "❌ Hủy",
                Width = 120,
                Height = 40,
                BackColor = Color.FromArgb(239, 68, 68),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };
            // btnCancel.Location được set trong UpdateBottomButtonPositions
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            pnlBottom.Controls.AddRange(new Control[] { lblSelectedCount, btnCancel, btnConfirm });

            // Add all panels to form
            this.Controls.Add(flpServices);
            this.Controls.Add(pnlCategory);
            this.Controls.Add(pnlSearch);
            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlBottom);

            // Cần gọi một lần khi form load xong để căn chỉnh vị trí ban đầu
            this.Load += (s, e) => UpdateBottomButtonPositions();
        }

        // Phương thức riêng để cập nhật vị trí các nút ở dưới cùng
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

                    // Confirm button (căn phải)
                    btnConfirm.Location = new Point(panelWidth - btnConfirm.Width - rightPadding, topPadding);

                    // Cancel button (căn phải, bên trái confirm)
                    btnCancel.Location = new Point(btnConfirm.Location.X - btnCancel.Width - 10, topPadding);
                }

                // Căn chỉnh label số lượng
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

            // Hover effect
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
                this.Cursor = Cursors.WaitCursor;
                await LoadServices();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Lỗi khi tải dịch vụ: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async System.Threading.Tasks.Task LoadServices()
        {
            // Đảm bảo _dichVuService không trả về null, nếu có lỗi DB, nó sẽ ném ra exception và được catch
            _allServices = _dichVuService.GetAllDichVu()
                .Where(d => d.TrangThai == "Còn hàng")
                .ToList();

            ApplyFilters();
        }

        private void ApplyFilters()
        {
            flpServices.SuspendLayout();
            flpServices.Controls.Clear();

            var filtered = _allServices.AsEnumerable();

            // Category filter
            if (_currentCategory != "all")
            {
                filtered = filtered.Where(d => d.Loai == _currentCategory);
            }

            // Search filter
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
                foreach (var service in services)
                {
                    var card = CreateServiceCard(service);
                    flpServices.Controls.Add(card);
                }
            }

            flpServices.ResumeLayout();
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

            // Center positioning
            lblIcon.Location = new Point(
                (pnlEmpty.Width - lblIcon.Width) / 2,
                50
            );

            lblText.Location = new Point(
                (pnlEmpty.Width - lblText.Width) / 2,
                140
            );

            lblHint.Location = new Point(
                (pnlEmpty.Width - lblHint.Width) / 2,
                175
            );

            pnlEmpty.Controls.AddRange(new Control[] { lblIcon, lblText, lblHint });
            flpServices.Controls.Add(pnlEmpty);
        }

        private Panel CreateServiceCard(DichVu service)
        {
            // Đảm bảo cardWidth được tính toán lại khi form resize
            int cardWidth = (flpServices.ClientSize.Width / 2) - 50; // Cố gắng hiển thị 2 cột, trừ padding
            if (cardWidth < 300) cardWidth = flpServices.ClientSize.Width - 30; // Chuyển sang 1 cột nếu quá hẹp

            var card = new Panel
            {
                Width = cardWidth,
                Height = 120,
                Margin = new Padding(10),
                BackColor = Color.FromArgb(248, 250, 252),
                Tag = service
            };

            card.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(226, 232, 240), 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                }
            };

            // Image
            var picImage = new PictureBox
            {
                Size = new Size(100, 100),
                Location = new Point(10, 10),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White
            };

            if (!string.IsNullOrEmpty(service.HinhAnh))
            {
                try
                {
                    // Lỗi tiềm ẩn: đường dẫn hình ảnh có thể sai
                    var projectRoot = AppDomain.CurrentDomain.BaseDirectory; // Dùng BaseDirectory để dễ dàng hơn trong môi trường runtime
                    var imagePath = System.IO.Path.Combine(projectRoot, "Images", "services", service.HinhAnh); // Giả định thư mục Images/services là nơi chứa ảnh

                    if (System.IO.File.Exists(imagePath))
                    {
                        using (var img = Image.FromFile(imagePath))
                        {
                            picImage.Image = new Bitmap(img);
                        }
                    }
                }
                catch { }
            }

            // Service Info - Tính toán vị trí động
            int infoStartX = 120;
            int availableWidth = cardWidth - infoStartX - 20; // 20px padding bên phải

            var lblName = new Label
            {
                Text = service.TenDv,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(infoStartX, 12),
                AutoSize = false,
                Size = new Size(availableWidth, 40),
                AutoEllipsis = true
            };

            var lblPrice = new Label
            {
                Text = $"{service.Gia:N0} đ / {service.DonVi}", // Thêm đơn vị tính
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 38, 38),
                Location = new Point(infoStartX, 50),
                AutoSize = true
            };

            // Quantity Controls - Responsive positioning
            // Khởi tạo số lượng từ _selectedServices nếu đã chọn, nếu chưa thì là 1
            var currentQty = _selectedServices.ContainsKey(service.MaDv) ? _selectedServices[service.MaDv] : 1;

            var btnMinus = new Button
            {
                Text = "−",
                Width = 32,
                Height = 32,
                Location = new Point(infoStartX, 78),
                BackColor = Color.FromArgb(226, 232, 240),
                ForeColor = Color.FromArgb(51, 65, 85),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Tag = service.MaDv
            };
            btnMinus.FlatAppearance.BorderSize = 0;
            btnMinus.Click += BtnMinus_Click;

            var txtQty = new TextBox
            {
                Width = 45,
                Height = 32,
                Location = new Point(infoStartX + 37, 78),
                Text = currentQty.ToString(),
                TextAlign = HorizontalAlignment.Center,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ReadOnly = true,
                Name = $"qty_{service.MaDv}"
            };

            var btnPlus = new Button
            {
                Text = "+",
                Width = 32,
                Height = 32,
                Location = new Point(infoStartX + 87, 78),
                BackColor = Color.FromArgb(226, 232, 240),
                ForeColor = Color.FromArgb(51, 65, 85),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Tag = service.MaDv
            };
            btnPlus.FlatAppearance.BorderSize = 0;
            btnPlus.Click += BtnPlus_Click;

            // Nút Thêm - Căn phải
            var btnAddWidth = Math.Min(80, availableWidth - 130);
            var btnAdd = new Button
            {
                Text = _selectedServices.ContainsKey(service.MaDv) ? "✓ Đã chọn" : "Thêm",
                Width = btnAddWidth,
                Height = 32,
                Location = new Point(cardWidth - btnAddWidth - 15, 78),
                BackColor = _selectedServices.ContainsKey(service.MaDv) ? Color.FromArgb(34, 197, 94) : Color.FromArgb(99, 102, 241),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Tag = service.MaDv
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += BtnAddService_Click;

            card.Controls.AddRange(new Control[]
            {
                picImage, lblName, lblPrice,
                btnMinus, txtQty, btnPlus, btnAdd
            });

            return card;
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

                    // Cập nhật lại số lượng trong selectedServices nếu nút 'Đã chọn'
                    var btnAdd = card.Controls.OfType<Button>().FirstOrDefault(b => b.Text.Contains("Đã chọn"));
                    if (btnAdd != null)
                    {
                        if (_selectedServices.ContainsKey(maDv))
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

                    // Cập nhật lại số lượng trong selectedServices nếu nút 'Đã chọn'
                    var btnAdd = card.Controls.OfType<Button>().FirstOrDefault(b => b.Text.Contains("Đã chọn"));
                    if (btnAdd != null)
                    {
                        if (_selectedServices.ContainsKey(maDv))
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
            var service = card.Tag as DichVu;

            if (txtQty != null && service != null)
            {
                var qty = int.Parse(txtQty.Text);

                // Nếu đã tồn tại, cộng dồn
                if (_selectedServices.ContainsKey(maDv))
                    _selectedServices[maDv] += qty;
                else
                    _selectedServices[maDv] = qty;

                // Cập nhật label hiển thị số dịch vụ đã chọn
                UpdateSelectedCount();

                // Visual feedback
                btn.BackColor = Color.FromArgb(34, 197, 94);
                btn.Text = "✓ Đã chọn"; // Thay đổi thành 'Đã chọn' để chỉ trạng thái thêm vào giỏ hàng tạm

                // Không cần delay nữa, chỉ cần thay đổi text và màu để biểu thị là đã thêm vào giỏ hàng
                // Sau đó nếu người dùng thay đổi số lượng, nút sẽ vẫn là 'Đã chọn' và số lượng được cập nhật trong _selectedServices (xử lý ở Plus/Minus)
            }
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
                    lblSelectedCount.Text = $"🛒 Đã chọn: {_selectedServices.Count} dịch vụ ({totalQty} món)";
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

            var result = MessageBox.Show(
                $"Xác nhận thêm {_selectedServices.Count} dịch vụ vào hóa đơn?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;

                    var failedServices = new List<string>();
                    var successCount = 0;

                    // Lặp qua danh sách dịch vụ đã chọn
                    foreach (var item in _selectedServices)
                    {
                        var success = await _hoaDonService.AddServiceToInvoiceAsync(
                            _maHoaDon, item.Key, item.Value);

                        if (success)
                            successCount++;
                        else
                        {
                            var service = _allServices.FirstOrDefault(s => s.MaDv == item.Key);
                            if (service != null)
                                failedServices.Add(service.TenDv);
                        }
                    }

                    this.Cursor = Cursors.Default;

                    if (successCount > 0 && failedServices.Count == 0)
                    {
                        MessageBox.Show(
                            $"Đã thêm thành công {successCount} dịch vụ!",
                            "Thành công",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        this.DialogResult = DialogResult.OK;
                    }
                    else if (successCount > 0 && failedServices.Count > 0)
                    {
                        MessageBox.Show(
                            $"Đã thêm {successCount} dịch vụ thành công.\n" +
                            $"Không thể thêm (có thể do hết hàng): {string.Join(", ", failedServices)}",
                            "Thông báo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        this.DialogResult = DialogResult.OK; // Vẫn cho phép thoát nếu có món được thêm
                    }
                    else
                    {
                        MessageBox.Show(
                            $"Không thể thêm bất kỳ dịch vụ nào (có thể do hết hàng hoặc lỗi hệ thống).",
                            "Lỗi",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show($"Lỗi khi thêm dịch vụ: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CategoryButton_Click(object sender, EventArgs e)
        {
            var clickedBtn = sender as Button;
            _currentCategory = clickedBtn.Tag.ToString();

            // Update button styles
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

            ApplyFilters();
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }
    }
}