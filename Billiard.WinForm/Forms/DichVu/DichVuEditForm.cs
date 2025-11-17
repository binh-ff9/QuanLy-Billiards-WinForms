using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Billiard.DAL.Entities;
using Billiard.BLL.Services;

namespace Billiard.WinForm.Forms
{
    public partial class DichVuEditForm : Form
    {
        private readonly DichVuService _dichVuService;
        private readonly MatHangService _matHangService;
        private int? _maDV;
        private DichVu _currentService;
        private string _selectedImagePath;

        // Form Controls
        private TextBox txtTenDV;
        private ComboBox cboLoai;
        private ComboBox cboTrangThai;
        private NumericUpDown numGia;
        private TextBox txtDonVi;
        private ComboBox cboMatHang;
        private TextBox txtMoTa;
        private PictureBox picPreview;
        private Button btnChooseImage;
        private Button btnRemoveImage;
        private Button btnSave;
        private Button btnCancel;

        // Constructor nhận DI
        public DichVuEditForm(DichVuService dichVuService, MatHangService matHangService)
        {
            _dichVuService = dichVuService;
            _matHangService = matHangService;

            InitializeComponent();
        }

        // THÊM METHOD NÀY - Quan trọng!
        public void SetServiceId(int? maDV)
        {
            _maDV = maDV;

            // Cập nhật tiêu đề form
            if (_maDV.HasValue)
            {
                this.Text = "✏️ Chỉnh sửa dịch vụ";
            }
            else
            {
                this.Text = "➕ Thêm dịch vụ mới";
            }
        }

        private void DichVuEditForm_Load(object sender, EventArgs e)
        {
            LoadMatHangs();

            if (_maDV.HasValue)
            {
                LoadServiceData();
            }
        }

        private void InitializeCustomComponents()
        {
            // Header Panel
            Panel headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(102, 126, 234)
            };

            Label lblTitle = new Label
            {
                Text = _maDV.HasValue ? "✏️ Chỉnh sửa dịch vụ" : "➕ Thêm dịch vụ mới",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 15),
                AutoSize = true
            };

            headerPanel.Controls.Add(lblTitle);
            this.Controls.Add(headerPanel);

            // Scrollable content panel
            Panel contentPanel = new Panel
            {
                Location = new Point(0, 60),
                Size = new Size(700, 630),
                AutoScroll = true,
                BackColor = Color.FromArgb(248, 249, 250)
            };

            int yPos = 20;

            // Basic Info Group
            GroupBox grpBasicInfo = CreateGroupBox("📋 Thông tin cơ bản", yPos, 580);
            yPos += 30;

            // Tên dịch vụ
            CreateLabel("Tên dịch vụ *", 20, yPos, grpBasicInfo);
            txtTenDV = new TextBox
            {
                Location = new Point(20, yPos + 25),
                Size = new Size(540, 30),
                Font = new Font("Segoe UI", 11)
            };
            grpBasicInfo.Controls.Add(txtTenDV);
            yPos += 70;

            // Loại dịch vụ và Trạng thái
            CreateLabel("Loại dịch vụ *", 20, yPos, grpBasicInfo);
            cboLoai = new ComboBox
            {
                Location = new Point(20, yPos + 25),
                Size = new Size(260, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 11)
            };
            cboLoai.Items.AddRange(new object[] { "Đồ uống", "Đồ ăn", "Khác" });
            cboLoai.SelectedIndex = 2;
            grpBasicInfo.Controls.Add(cboLoai);

            CreateLabel("Trạng thái *", 300, yPos, grpBasicInfo);
            cboTrangThai = new ComboBox
            {
                Location = new Point(300, yPos + 25),
                Size = new Size(260, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 11)
            };
            cboTrangThai.Items.AddRange(new object[] { "Còn hàng", "Hết hàng", "Ngừng kinh doanh" });
            cboTrangThai.SelectedIndex = 0;
            grpBasicInfo.Controls.Add(cboTrangThai);
            yPos += 70;

            // Giá và Đơn vị
            CreateLabel("Giá *", 20, yPos, grpBasicInfo);
            numGia = new NumericUpDown
            {
                Location = new Point(20, yPos + 25),
                Size = new Size(260, 30),
                Maximum = 9999999999,
                Minimum = 0,
                Increment = 1000,
                ThousandsSeparator = true,
                Font = new Font("Segoe UI", 11)
            };
            grpBasicInfo.Controls.Add(numGia);

            CreateLabel("Đơn vị", 300, yPos, grpBasicInfo);
            txtDonVi = new TextBox
            {
                Location = new Point(300, yPos + 25),
                Size = new Size(260, 30),
                Text = "phần",
                Font = new Font("Segoe UI", 11)
            };
            grpBasicInfo.Controls.Add(txtDonVi);
            yPos += 70;

            // Mặt hàng liên quan
            CreateLabel("Mặt hàng liên quan", 20, yPos, grpBasicInfo);
            cboMatHang = new ComboBox
            {
                Location = new Point(20, yPos + 25),
                Size = new Size(540, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 11)
            };
            grpBasicInfo.Controls.Add(cboMatHang);
            yPos += 70;

            // Mô tả
            CreateLabel("Mô tả", 20, yPos, grpBasicInfo);
            txtMoTa = new TextBox
            {
                Location = new Point(20, yPos + 25),
                Size = new Size(540, 80),
                Multiline = true,
                Font = new Font("Segoe UI", 11),
                ScrollBars = ScrollBars.Vertical
            };
            grpBasicInfo.Controls.Add(txtMoTa);

            grpBasicInfo.Height = 430;
            contentPanel.Controls.Add(grpBasicInfo);

            // Image Group
            GroupBox grpImage = CreateGroupBox("🖼️ Hình ảnh dịch vụ", 460, 300);

            picPreview = new PictureBox
            {
                Location = new Point(20, 30),
                Size = new Size(540, 200),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };
            grpImage.Controls.Add(picPreview);

            btnChooseImage = new Button
            {
                Text = "📷 Chọn hình ảnh",
                Location = new Point(20, 240),
                Size = new Size(260, 40),
                BackColor = Color.FromArgb(102, 126, 234),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnChooseImage.FlatAppearance.BorderSize = 0;
            btnChooseImage.Click += BtnChooseImage_Click;
            grpImage.Controls.Add(btnChooseImage);

            btnRemoveImage = new Button
            {
                Text = "🗑️ Xóa ảnh",
                Location = new Point(300, 240),
                Size = new Size(260, 40),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRemoveImage.FlatAppearance.BorderSize = 0;
            btnRemoveImage.Click += BtnRemoveImage_Click;
            grpImage.Controls.Add(btnRemoveImage);

            contentPanel.Controls.Add(grpImage);

            this.Controls.Add(contentPanel);

            // Footer Panel
            Panel footerPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                BackColor = Color.White
            };

            btnCancel = new Button
            {
                Text = "❌ Hủy",
                Location = new Point(420, 15),
                Size = new Size(120, 40),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                DialogResult = DialogResult.Cancel,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            footerPanel.Controls.Add(btnCancel);

            btnSave = new Button
            {
                Text = _maDV.HasValue ? "✅ Cập nhật" : "✅ Thêm dịch vụ",
                Location = new Point(560, 15),
                Size = new Size(120, 40),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            footerPanel.Controls.Add(btnSave);

            this.Controls.Add(footerPanel);
        }

        private GroupBox CreateGroupBox(string title, int yPos, int height)
        {
            return new GroupBox
            {
                Text = title,
                Location = new Point(20, yPos),
                Size = new Size(640, height),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80)
            };
        }

        private void CreateLabel(string text, int x, int y, Control parent)
        {
            Label label = new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80)
            };
            parent.Controls.Add(label);
        }

        private void LoadMatHangs()
        {
            var matHangs = _matHangService.GetMatHangConHang();

            cboMatHang.Items.Add(new { MaHang = 0, Display = "-- Không liên kết --" });
            foreach (var item in matHangs)
            {
                cboMatHang.Items.Add(new { item.MaHang, Display = $"{item.TenHang} (Tồn: {item.SoLuongTon})" });
            }
            cboMatHang.DisplayMember = "Display";
            cboMatHang.ValueMember = "MaHang";
            cboMatHang.SelectedIndex = 0;
        }

        private void LoadServiceData()
        {
            _currentService = _dichVuService.GetDichVuById(_maDV.Value);

            if (_currentService != null)
            {
                txtTenDV.Text = _currentService.TenDv;

                // Set loại
                cboLoai.SelectedItem = _currentService.Loai;

                // Set trạng thái
                cboTrangThai.SelectedItem = _currentService.TrangThai;

                numGia.Value = _currentService.Gia;
                txtDonVi.Text = _currentService.DonVi;
                txtMoTa.Text = _currentService.MoTa ?? "";

                // Set mặt hàng
                if (_currentService.MaHang.HasValue)
                {
                    for (int i = 0; i < cboMatHang.Items.Count; i++)
                    {
                        dynamic item = cboMatHang.Items[i];
                        if (item.MaHang == _currentService.MaHang.Value)
                        {
                            cboMatHang.SelectedIndex = i;
                            break;
                        }
                    }
                }

                // Load image
                if (!string.IsNullOrEmpty(_currentService.HinhAnh))
                {
                    string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", _currentService.HinhAnh);
                    if (File.Exists(imagePath))
                    {
                        picPreview.Image = Image.FromFile(imagePath);
                    }
                }
            }
        }

        private void BtnChooseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                ofd.Title = "Chọn hình ảnh dịch vụ";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        FileInfo fileInfo = new FileInfo(ofd.FileName);
                        if (fileInfo.Length > 5 * 1024 * 1024) // 5MB
                        {
                            MessageBox.Show("Kích thước file không được vượt quá 5MB!",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        _selectedImagePath = ofd.FileName;
                        picPreview.Image = Image.FromFile(_selectedImagePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi tải ảnh: {ex.Message}",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnRemoveImage_Click(object sender, EventArgs e)
        {
            _selectedImagePath = null;
            picPreview.Image = null;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(txtTenDV.Text))
            {
                MessageBox.Show("Vui lòng nhập tên dịch vụ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDV.Focus();
                return;
            }

            if (numGia.Value <= 0)
            {
                MessageBox.Show("Vui lòng nhập giá hợp lệ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numGia.Focus();
                return;
            }

            try
            {
                string savedImageName = null;

                // Save image if selected
                if (!string.IsNullOrEmpty(_selectedImagePath))
                {
                    savedImageName = SaveImage(_selectedImagePath);
                }

                if (_maDV.HasValue) // Update
                {
                    if (_currentService != null)
                    {
                        // Delete old image if new image is selected
                        if (savedImageName != null && !string.IsNullOrEmpty(_currentService.HinhAnh))
                        {
                            DeleteImage(_currentService.HinhAnh);
                        }

                        _currentService.TenDv = txtTenDV.Text.Trim();
                        _currentService.Loai = cboLoai.SelectedItem.ToString();
                        _currentService.TrangThai = cboTrangThai.SelectedItem.ToString();
                        _currentService.Gia = numGia.Value;
                        _currentService.DonVi = txtDonVi.Text.Trim();
                        _currentService.MoTa = txtMoTa.Text.Trim();

                        if (cboMatHang.SelectedIndex > 0)
                        {
                            dynamic selectedItem = cboMatHang.SelectedItem;
                            _currentService.MaHang = selectedItem.MaHang;
                        }
                        else
                        {
                            _currentService.MaHang = null;
                        }

                        if (savedImageName != null)
                        {
                            _currentService.HinhAnh = savedImageName;
                        }

                        _dichVuService.UpdateDichVu(_currentService);
                        MessageBox.Show("Cập nhật dịch vụ thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else // Add new
                {
                    var newService = new DichVu
                    {
                        TenDv = txtTenDV.Text.Trim(),
                        Loai = cboLoai.SelectedItem.ToString(),
                        TrangThai = cboTrangThai.SelectedItem.ToString(),
                        Gia = numGia.Value,
                        DonVi = txtDonVi.Text.Trim(),
                        MoTa = txtMoTa.Text.Trim(),
                        HinhAnh = savedImageName,
                        NgayTao = DateTime.Now
                    };

                    if (cboMatHang.SelectedIndex > 0)
                    {
                        dynamic selectedItem = cboMatHang.SelectedItem;
                        newService.MaHang = selectedItem.MaHang;
                    }

                    _dichVuService.AddDichVu(newService);

                    MessageBox.Show("Thêm dịch vụ thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string SaveImage(string sourcePath)
        {
            string fileName = $"service_{Guid.NewGuid()}{Path.GetExtension(sourcePath)}";
            string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            string destinationPath = Path.Combine(imagesFolder, fileName);
            File.Copy(sourcePath, destinationPath, true);

            return fileName;
        }

        private void DeleteImage(string imageName)
        {
            try
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", imageName);
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting image: {ex.Message}");
            }
        }

        // XÓA hoặc COMMENT phần Dispose này nếu có lỗi
        // protected override void Dispose(bool disposing)
        // {
        //     if (disposing)
        //     {
        //         picPreview?.Image?.Dispose();
        //     }
        //     base.Dispose(disposing);
        // }
    }
}