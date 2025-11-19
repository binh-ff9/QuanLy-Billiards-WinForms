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

        // Constructor nhận DI
        public DichVuEditForm(DichVuService dichVuService, MatHangService matHangService)
        {
            _dichVuService = dichVuService;
            _matHangService = matHangService;

            InitializeComponent();
        }

        // Method để set ServiceId từ bên ngoài
        public void SetServiceId(int? maDV)
        {
            _maDV = maDV;

            // Cập nhật tiêu đề form
            if (_maDV.HasValue)
            {
                this.Text = "✏️ Chỉnh sửa dịch vụ";
                lblTitle.Text = "✏️ Chỉnh sửa dịch vụ";
                btnSave.Text = "✅ Cập nhật";
            }
            else
            {
                this.Text = "➕ Thêm dịch vụ mới";
                lblTitle.Text = "➕ Thêm dịch vụ mới";
                btnSave.Text = "✅ Thêm dịch vụ";
            }
        }

        private void DichVuEditForm_Load(object sender, EventArgs e)
        {
            // Set giá trị mặc định cho ComboBox
            if (cboLoai.SelectedIndex == -1)
                cboLoai.SelectedIndex = 2; // Mặc định chọn "Khác"

            if (cboTrangThai.SelectedIndex == -1)
                cboTrangThai.SelectedIndex = 0; // Mặc định chọn "Còn hàng"

            LoadMatHangs();

            if (_maDV.HasValue)
            {
                LoadServiceData();
            }
        }

        private void LoadMatHangs()
        {
            var matHangs = _matHangService.GetMatHangConHang();

            cboMatHang.Items.Clear();
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
            if (picPreview.Image != null)
            {
                picPreview.Image.Dispose();
                picPreview.Image = null;
            }
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

            if (cboLoai.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn loại dịch vụ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboLoai.Focus();
                return;
            }

            if (cboTrangThai.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn trạng thái!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTrangThai.Focus();
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
    }
}