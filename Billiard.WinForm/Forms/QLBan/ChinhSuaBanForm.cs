using Billiard.BLL.Services.QLBan;
using Billiard.DAL.Entities;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.QLBan
{
    public partial class ChinhSuaBanForm : Form
    {
        private readonly BanBiaService _banBiaService;
        private readonly LoaiBanService _loaiBanService;
        private readonly KhuVucService _khuVucService;
        private readonly BanBium _banBia;
        private string _selectedImagePath;
        private bool _imageChanged = false;

        public ChinhSuaBanForm(BanBiaService banBiaService, LoaiBanService loaiBanService,
            KhuVucService khuVucService, BanBium banBia)
        {
            _banBiaService = banBiaService;
            _loaiBanService = loaiBanService;
            _khuVucService = khuVucService;
            _banBia = banBia;
            InitializeComponent();
        }

        private async void ChinhSuaBanForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                System.Diagnostics.Debug.WriteLine("=== BẮT ĐẦU LOAD FORM CHỈNH SỬA ===");

                // Load loại bàn
                var loaiBans = await _loaiBanService.GetAllAsync();
                var activeLoaiBans = loaiBans.Where(l => l.TrangThai == "Đang áp dụng").ToList();
                System.Diagnostics.Debug.WriteLine($"Số loại bàn: {activeLoaiBans.Count}");

                cboLoaiBan.DataSource = activeLoaiBans;
                cboLoaiBan.DisplayMember = "TenLoai";
                cboLoaiBan.ValueMember = "MaLoai";

                // Load khu vực
                var khuVucs = await _khuVucService.GetAllAsync();
                System.Diagnostics.Debug.WriteLine($"Số khu vực: {khuVucs.Count}");

                cboKhuVuc.DataSource = khuVucs;
                cboKhuVuc.DisplayMember = "TenKhuVuc";
                cboKhuVuc.ValueMember = "MaKhuVuc";

                // Load thông tin bàn hiện tại
                System.Diagnostics.Debug.WriteLine($"Đang load thông tin bàn: {_banBia.TenBan}");

                txtTenBan.Text = _banBia.TenBan;
                cboLoaiBan.SelectedValue = _banBia.MaLoai;
                cboKhuVuc.SelectedValue = _banBia.MaKhuVuc;
                txtGhiChu.Text = _banBia.GhiChu ?? "";

                System.Diagnostics.Debug.WriteLine($"Tên bàn: {txtTenBan.Text}");
                System.Diagnostics.Debug.WriteLine($"Loại bàn selected: {cboLoaiBan.SelectedValue}");
                System.Diagnostics.Debug.WriteLine($"Khu vực selected: {cboKhuVuc.SelectedValue}");

                // Load hình ảnh nếu có
                if (!string.IsNullOrEmpty(_banBia.HinhAnh))
                {
                    System.Diagnostics.Debug.WriteLine($"Đang load ảnh: {_banBia.HinhAnh}");

                    // SỬA: Dùng đường dẫn giống như trong QLBanForm
                    var projectRoot = Directory.GetParent(Directory.GetParent(Directory.GetParent(
                        Directory.GetParent(Application.StartupPath).FullName).FullName).FullName).FullName;
                    var imagePath = Path.Combine(projectRoot, "Forms", "Resources", "img", "tables", _banBia.HinhAnh);

                    System.Diagnostics.Debug.WriteLine($"Image path: {imagePath}");

                    if (File.Exists(imagePath))
                    {
                        using (var img = Image.FromFile(imagePath))
                        {
                            picPreview.Image = new Bitmap(img);
                        }
                        btnXoaAnh.Enabled = true;
                        System.Diagnostics.Debug.WriteLine("✓ Đã load ảnh thành công");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("⚠ File ảnh không tồn tại");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Bàn không có ảnh");
                }

                this.Cursor = Cursors.Default;
                System.Diagnostics.Debug.WriteLine("=== LOAD FORM THÀNH CÔNG ===");
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                System.Diagnostics.Debug.WriteLine($"❌ LỖI: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");

                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnChonAnh_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                ofd.Title = "Chọn hình ảnh bàn";
                ofd.Multiselect = false;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var fileInfo = new FileInfo(ofd.FileName);
                        if (fileInfo.Length > 5 * 1024 * 1024)
                        {
                            MessageBox.Show("Kích thước file không được vượt quá 5MB!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        _selectedImagePath = ofd.FileName;
                        _imageChanged = true;

                        if (picPreview.Image != null)
                        {
                            picPreview.Image.Dispose();
                        }

                        using (var img = Image.FromFile(_selectedImagePath))
                        {
                            picPreview.Image = new Bitmap(img);
                        }

                        btnXoaAnh.Enabled = true;
                        System.Diagnostics.Debug.WriteLine("✓ Đã chọn ảnh mới");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ Lỗi load ảnh: {ex.Message}");
                        MessageBox.Show($"Không thể tải hình ảnh: {ex.Message}",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnXoaAnh_Click(object sender, EventArgs e)
        {
            if (picPreview.Image != null)
            {
                picPreview.Image.Dispose();
                picPreview.Image = null;
            }
            _selectedImagePath = null;
            _imageChanged = true;
            btnXoaAnh.Enabled = false;
            System.Diagnostics.Debug.WriteLine("Đã xóa ảnh");
        }

        private async void BtnLuu_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("\n=== BẮT ĐẦU CẬP NHẬT BÀN ===");

            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(txtTenBan.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên bàn!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenBan.Focus();
                    return;
                }

                if (cboLoaiBan.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn loại bàn!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cboKhuVuc.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn khu vực!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                btnLuu.Enabled = false;
                btnHuy.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                // Xử lý hình ảnh
                string imageName = _banBia.HinhAnh;

                if (_imageChanged)
                {
                    System.Diagnostics.Debug.WriteLine("Đang xử lý thay đổi ảnh...");

                    // Xóa ảnh cũ nếu có
                    if (!string.IsNullOrEmpty(_banBia.HinhAnh))
                    {
                        var projectRoot = Directory.GetParent(Directory.GetParent(Directory.GetParent(
                            Directory.GetParent(Application.StartupPath).FullName).FullName).FullName).FullName;
                        var oldImagePath = Path.Combine(projectRoot, "Forms", "Resources", "img", "tables", _banBia.HinhAnh);

                        if (File.Exists(oldImagePath))
                        {
                            try
                            {
                                File.Delete(oldImagePath);
                                System.Diagnostics.Debug.WriteLine("✓ Đã xóa ảnh cũ");
                            }
                            catch (Exception delEx)
                            {
                                System.Diagnostics.Debug.WriteLine($"⚠ Không xóa được ảnh cũ: {delEx.Message}");
                            }
                        }
                    }

                    // Lưu ảnh mới
                    if (!string.IsNullOrEmpty(_selectedImagePath))
                    {
                        imageName = $"table_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(_selectedImagePath)}";

                        var projectRoot = Directory.GetParent(Directory.GetParent(Directory.GetParent(
                            Directory.GetParent(Application.StartupPath).FullName).FullName).FullName).FullName;
                        var destFolder = Path.Combine(projectRoot, "Forms", "Resources", "img", "tables");

                        Directory.CreateDirectory(destFolder);
                        var destPath = Path.Combine(destFolder, imageName);

                        File.Copy(_selectedImagePath, destPath, true);
                        System.Diagnostics.Debug.WriteLine($"✓ Đã copy ảnh mới: {imageName}");
                    }
                    else
                    {
                        imageName = null;
                        System.Diagnostics.Debug.WriteLine("Xóa ảnh - set imageName = null");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Không có thay đổi ảnh");
                }

                // Cập nhật thông tin bàn
                System.Diagnostics.Debug.WriteLine("Đang cập nhật thông tin bàn...");

                _banBia.TenBan = txtTenBan.Text.Trim();
                _banBia.MaLoai = (int)cboLoaiBan.SelectedValue;
                _banBia.MaKhuVuc = (int)cboKhuVuc.SelectedValue;
                _banBia.GhiChu = string.IsNullOrWhiteSpace(txtGhiChu.Text) ? null : txtGhiChu.Text.Trim();
                _banBia.HinhAnh = imageName;

                System.Diagnostics.Debug.WriteLine($"Tên: {_banBia.TenBan}");
                System.Diagnostics.Debug.WriteLine($"Loại: {_banBia.MaLoai}");
                System.Diagnostics.Debug.WriteLine($"Khu vực: {_banBia.MaKhuVuc}");
                System.Diagnostics.Debug.WriteLine($"Ảnh: {_banBia.HinhAnh ?? "(null)"}");

                var result = await _banBiaService.UpdateTableAsync(_banBia);

                this.Cursor = Cursors.Default;

                if (result)
                {
                    System.Diagnostics.Debug.WriteLine("✓ CẬP NHẬT THÀNH CÔNG!");
                    MessageBox.Show("Cập nhật bàn thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("❌ CẬP NHẬT THẤT BẠI");
                    MessageBox.Show("Không thể cập nhật bàn!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnLuu.Enabled = true;
                    btnHuy.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                btnLuu.Enabled = true;
                btnHuy.Enabled = true;

                System.Diagnostics.Debug.WriteLine($"❌ LỖI: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");

                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            System.Diagnostics.Debug.WriteLine("=== KẾT THÚC CẬP NHẬT ===\n");
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (picPreview.Image != null)
            {
                picPreview.Image.Dispose();
                picPreview.Image = null;
            }
            base.OnFormClosing(e);
        }
    }
}