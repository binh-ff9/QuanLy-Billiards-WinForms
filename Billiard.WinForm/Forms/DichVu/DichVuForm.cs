using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Billiard.DAL.Entities;
using Billiard.BLL.Services;

namespace Billiard.WinForm.Forms
{
    public partial class DichVuForm : Form
    {
        private readonly DichVuService _dichVuService;
        private FlowLayoutPanel flpServices;
        private TextBox txtSearch;
        private ComboBox cboFilter;
        private Button btnAdd;

        public DichVuForm(DichVuService dichVuService)
        {
            InitializeComponent();
            _dichVuService = dichVuService;
        }

        private void DichVuForm_Load(object sender, EventArgs e)
        {
            LoadServices();
        }

        private void InitializeCustomComponents()
        {
            // Header Panel
            Panel headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(102, 126, 234),
                Padding = new Padding(20)
            };

            Label lblTitle = new Label
            {
                Text = "Dịch vụ & Menu",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 25)
            };

            btnAdd = new Button
            {
                Text = "➕ Thêm dịch vụ mới",
                Size = new Size(180, 40),
                Location = new Point(1000, 20),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(102, 126, 234),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += BtnAdd_Click;

            headerPanel.Controls.AddRange(new Control[] { lblTitle, btnAdd });
            this.Controls.Add(headerPanel);

            // Filter Panel
            Panel filterPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                BackColor = Color.White,
                Padding = new Padding(20, 15, 20, 15)
            };

            Label lblFilter = new Label
            {
                Text = "Loại dịch vụ:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            cboFilter = new ComboBox
            {
                Location = new Point(120, 17),
                Size = new Size(200, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cboFilter.Items.AddRange(new object[] { "Tất cả", "Đồ uống", "Đồ ăn", "Khác" });
            cboFilter.SelectedIndex = 0;
            cboFilter.SelectedIndexChanged += CboFilter_SelectedIndexChanged;

            txtSearch = new TextBox
            {
                Location = new Point(20, 60),
                Size = new Size(1140, 35),
                Font = new Font("Segoe UI", 11),
                PlaceholderText = "🔍 Tìm kiếm dịch vụ..."
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;

            filterPanel.Controls.AddRange(new Control[] { lblFilter, cboFilter, txtSearch });
            this.Controls.Add(filterPanel);

            // Services Panel with ScrollBar
            Panel servicesContainer = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(240, 242, 245),
                Padding = new Padding(20)
            };

            flpServices = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                BackColor = Color.FromArgb(240, 242, 245)
            };

            servicesContainer.Controls.Add(flpServices);
            this.Controls.Add(servicesContainer);
        }

        private void LoadServices(string filter = "Tất cả", string search = "")
        {
            flpServices.Controls.Clear();

            var services = _dichVuService.GetAllDichVu();

            // Filter by category
            if (filter != "Tất cả")
            {
                var loaiDV = filter switch
                {
                    "Đồ uống" => "Đồ uống",
                    "Đồ ăn" => "Đồ ăn",
                    _ => "Khác"
                };
                services = services.Where(d => d.Loai == loaiDV).ToList();
            }

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                services = services.Where(d => d.TenDv.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!services.Any())
            {
                ShowEmptyState();
                return;
            }

            foreach (var service in services)
            {
                flpServices.Controls.Add(CreateServiceCard(service));
            }
        }

        private Panel CreateServiceCard(DichVu service)
        {
            Panel card = new Panel
            {
                Size = new Size(280, 380),
                BackColor = Color.White,
                Margin = new Padding(10),
                BorderStyle = BorderStyle.None
            };

            // Add shadow effect
            card.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, card.ClientRectangle,
                    Color.FromArgb(200, 200, 200), 1, ButtonBorderStyle.Solid,
                    Color.FromArgb(200, 200, 200), 1, ButtonBorderStyle.Solid,
                    Color.FromArgb(200, 200, 200), 1, ButtonBorderStyle.Solid,
                    Color.FromArgb(200, 200, 200), 1, ButtonBorderStyle.Solid);
            };

            // Image
            PictureBox picImage = new PictureBox
            {
                Location = new Point(10, 10),
                Size = new Size(260, 180),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(248, 249, 250)
            };

            if (!string.IsNullOrEmpty(service.HinhAnh))
            {
                try
                {
                    string imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", service.HinhAnh);
                    if (System.IO.File.Exists(imagePath))
                    {
                        picImage.Image = Image.FromFile(imagePath);
                    }
                }
                catch { }
            }

            // Service Name
            Label lblName = new Label
            {
                Text = service.TenDv,
                Location = new Point(10, 200),
                Size = new Size(260, 50),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                TextAlign = ContentAlignment.TopLeft
            };

            // Service Code
            Label lblCode = new Label
            {
                Text = $"Mã: {service.MaDv}",
                Location = new Point(10, 255),
                Size = new Size(260, 20),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(108, 117, 125)
            };

            // Price
            Label lblPrice = new Label
            {
                Text = $"{service.Gia:N0} đ",
                Location = new Point(10, 280),
                Size = new Size(260, 30),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 167, 69)
            };

            // Edit Button
            Button btnEdit = new Button
            {
                Text = "✏️ Sửa",
                Location = new Point(10, 320),
                Size = new Size(120, 40),
                BackColor = Color.FromArgb(102, 126, 234),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Click += (s, e) => EditService(service.MaDv);

            // Delete Button
            Button btnDelete = new Button
            {
                Text = "🗑️ Xóa",
                Location = new Point(140, 320),
                Size = new Size(120, 40),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += (s, e) => DeleteService(service.MaDv, service.TenDv);

            card.Controls.AddRange(new Control[] { picImage, lblName, lblCode, lblPrice, btnEdit, btnDelete });

            return card;
        }

        private void ShowEmptyState()
        {
            Panel emptyPanel = new Panel
            {
                Size = new Size(400, 300),
                Location = new Point((flpServices.Width - 400) / 2, 100),
                BackColor = Color.White
            };

            Label lblIcon = new Label
            {
                Text = "📦",
                Font = new Font("Segoe UI", 48),
                Location = new Point(176, 50),
                AutoSize = true
            };

            Label lblTitle = new Label
            {
                Text = "Chưa có dịch vụ",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(120, 150),
                AutoSize = true,
                ForeColor = Color.FromArgb(44, 62, 80)
            };

            Label lblText = new Label
            {
                Text = "Thêm dịch vụ đầu tiên vào menu",
                Font = new Font("Segoe UI", 11),
                Location = new Point(90, 190),
                AutoSize = true,
                ForeColor = Color.FromArgb(108, 117, 125)
            };

            emptyPanel.Controls.AddRange(new Control[] { lblIcon, lblTitle, lblText });
            flpServices.Controls.Add(emptyPanel);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var form = Program.GetService<DichVuEditForm>();
            form.SetServiceId(null); // null = thêm mới
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadServices(cboFilter.SelectedItem.ToString(), txtSearch.Text);
            }
        }

        private void EditService(int maDV)
        {
            var form = Program.GetService<DichVuEditForm>();
            form.SetServiceId(maDV); // truyền maDV để edit
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadServices(cboFilter.SelectedItem.ToString(), txtSearch.Text);
            }
        }

        private void DeleteService(int maDV, string tenDV)
        {
            var result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa dịch vụ \"{tenDV}\"?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var success = _dichVuService.DeleteDichVu(maDV);
                    if (success)
                    {
                        MessageBox.Show("Xóa dịch vụ thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadServices(cboFilter.SelectedItem.ToString(), txtSearch.Text);
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa! Dịch vụ đã được sử dụng trong hóa đơn.",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CboFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadServices(cboFilter.SelectedItem.ToString(), txtSearch.Text);
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadServices(cboFilter.SelectedItem.ToString(), txtSearch.Text);
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        // Dispose managed resources
        //        _dichVuService?.Dispose();

        //        // Dispose components nếu có
        //        components?.Dispose();
        //    }

        //    // Gọi base.Dispose cuối cùng
        //    base.Dispose(disposing);
        //}
    }
}