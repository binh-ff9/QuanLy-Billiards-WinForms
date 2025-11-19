using Billiard.BLL.Services;
using Billiard.DAL.Entities;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Billiard.BLL.Services.QLBan;

namespace Billiard.WinForm.Forms.QLBan
{
    public partial class ThemBanForm : Form
    {
        private readonly BanBiaService _banBiaService;
        private readonly LoaiBanService _loaiBanService;
        private readonly KhuVucService _khuVucService;
        private string _selectedImagePath;

        public ThemBanForm(BanBiaService banBiaService, LoaiBanService loaiBanService, KhuVucService khuVucService)
        {
            _banBiaService = banBiaService;
            _loaiBanService = loaiBanService;
            _khuVucService = khuVucService;
            InitializeComponent();
        }

        private async void ThemBanForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // DEBUG: Log bắt đầu load
                System.Diagnostics.Debug.WriteLine("=== BẮT ĐẦU LOAD FORM THÊM BÀN ===");

                // Load loại bàn
                System.Diagnostics.Debug.WriteLine("Đang load danh sách loại bàn...");
                var loaiBans = await _loaiBanService.GetAllAsync();
                System.Diagnostics.Debug.WriteLine($"Tổng số loại bàn: {loaiBans.Count}");

                var activeLoaiBans = loaiBans.Where(l => l.TrangThai == "Đang áp dụng").ToList();
                System.Diagnostics.Debug.WriteLine($"Số loại bàn đang áp dụng: {activeLoaiBans.Count}");

                if (activeLoaiBans.Count == 0)
                {
                    MessageBox.Show("Không có loại bàn nào đang áp dụng!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                cboLoaiBan.DataSource = activeLoaiBans;
                cboLoaiBan.DisplayMember = "TenLoai";
                cboLoaiBan.ValueMember = "MaLoai";
                System.Diagnostics.Debug.WriteLine($"Đã bind loại bàn vào ComboBox");

                // Load khu vực
                System.Diagnostics.Debug.WriteLine("Đang load danh sách khu vực...");
                var khuVucs = await _khuVucService.GetAllAsync();
                System.Diagnostics.Debug.WriteLine($"Tổng số khu vực: {khuVucs.Count}");

                cboKhuVuc.DataSource = khuVucs;
                cboKhuVuc.DisplayMember = "TenKhuVuc";
                cboKhuVuc.ValueMember = "MaKhuVuc";
                System.Diagnostics.Debug.WriteLine($"Đã bind khu vực vào ComboBox");

                this.Cursor = Cursors.Default;
                System.Diagnostics.Debug.WriteLine("=== LOAD FORM THÀNH CÔNG ===");
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                System.Diagnostics.Debug.WriteLine($"❌ LỖI LOAD FORM: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }

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
                        System.Diagnostics.Debug.WriteLine($"Đã chọn file: {ofd.FileName}");

                        // Kiểm tra kích thước file (tối đa 5MB)
                        var fileInfo = new FileInfo(ofd.FileName);
                        System.Diagnostics.Debug.WriteLine($"Kích thước file: {fileInfo.Length / 1024} KB");

                        if (fileInfo.Length > 5 * 1024 * 1024)
                        {
                            MessageBox.Show("Kích thước file không được vượt quá 5MB!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        _selectedImagePath = ofd.FileName;

                        // Hiển thị preview
                        using (var img = Image.FromFile(_selectedImagePath))
                        {
                            picPreview.Image = new Bitmap(img);
                        }

                        btnXoaAnh.Enabled = true;
                        System.Diagnostics.Debug.WriteLine("✓ Đã load ảnh thành công");
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
            btnXoaAnh.Enabled = false;
            System.Diagnostics.Debug.WriteLine("Đã xóa ảnh");
        }

        private async void BtnLuu_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("\n=== BẮT ĐẦU LƯU BÀN MỚI ===");

            try
            {
                // Validation
                System.Diagnostics.Debug.WriteLine("1. Kiểm tra validation...");

                if (string.IsNullOrWhiteSpace(txtTenBan.Text))
                {
                    System.Diagnostics.Debug.WriteLine("❌ Tên bàn trống");
                    MessageBox.Show("Vui lòng nhập tên bàn!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenBan.Focus();
                    return;
                }
                System.Diagnostics.Debug.WriteLine($"✓ Tên bàn: {txtTenBan.Text}");

                if (cboLoaiBan.SelectedValue == null)
                {
                    System.Diagnostics.Debug.WriteLine("❌ Chưa chọn loại bàn");
                    MessageBox.Show("Vui lòng chọn loại bàn!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboLoaiBan.Focus();
                    return;
                }
                System.Diagnostics.Debug.WriteLine($"✓ Loại bàn: {cboLoaiBan.SelectedValue}");

                if (cboKhuVuc.SelectedValue == null)
                {
                    System.Diagnostics.Debug.WriteLine("❌ Chưa chọn khu vực");
                    MessageBox.Show("Vui lòng chọn khu vực!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboKhuVuc.Focus();
                    return;
                }
                System.Diagnostics.Debug.WriteLine($"✓ Khu vực: {cboKhuVuc.SelectedValue}");

                // Disable buttons during save
                btnLuu.Enabled = false;
                btnHuy.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                // Copy image to Resources folder if selected
                string imageName = null;
                if (!string.IsNullOrEmpty(_selectedImagePath))
                {
                    System.Diagnostics.Debug.WriteLine("2. Đang xử lý hình ảnh...");

                    imageName = $"table_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(_selectedImagePath)}";
                    System.Diagnostics.Debug.WriteLine($"Tên file ảnh: {imageName}");

                    // Tìm thư mục project root
                    var startupPath = Application.StartupPath;
                    System.Diagnostics.Debug.WriteLine($"Startup Path: {startupPath}");

                    var projectRoot = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(startupPath).FullName).FullName).FullName).FullName;
                    System.Diagnostics.Debug.WriteLine($"Project Root: {projectRoot}");

                    var destFolder = Path.Combine(projectRoot, "Forms", "Resources", "img", "tables");
                    System.Diagnostics.Debug.WriteLine($"Destination Folder: {destFolder}");

                    // Create directory if not exists
                    if (!Directory.Exists(destFolder))
                    {
                        System.Diagnostics.Debug.WriteLine("Tạo thư mục mới...");
                        Directory.CreateDirectory(destFolder);
                    }

                    var destPath = Path.Combine(destFolder, imageName);
                    System.Diagnostics.Debug.WriteLine($"Destination Path: {destPath}");

                    File.Copy(_selectedImagePath, destPath, true);
                    System.Diagnostics.Debug.WriteLine("✓ Đã copy ảnh thành công");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("2. Không có ảnh để xử lý");
                }

                // Create new table object
                System.Diagnostics.Debug.WriteLine("3. Tạo đối tượng BanBium mới...");

                var newBan = new BanBium
                {
                    TenBan = txtTenBan.Text.Trim(),
                    MaLoai = (int)cboLoaiBan.SelectedValue,
                    MaKhuVuc = (int)cboKhuVuc.SelectedValue,
                    TrangThai = "Trống",
                    GhiChu = string.IsNullOrWhiteSpace(txtGhiChu.Text) ? null : txtGhiChu.Text.Trim(),
                    HinhAnh = imageName,
                    GioBatDau = null,
                    MaKh = null,
                    ViTriX = 0,
                    ViTriY = 0,
                    NgayTao = DateTime.Now
                };

                System.Diagnostics.Debug.WriteLine($"Thông tin bàn:");
                System.Diagnostics.Debug.WriteLine($"  - Tên: {newBan.TenBan}");
                System.Diagnostics.Debug.WriteLine($"  - Mã loại: {newBan.MaLoai}");
                System.Diagnostics.Debug.WriteLine($"  - Mã khu vực: {newBan.MaKhuVuc}");
                System.Diagnostics.Debug.WriteLine($"  - Trạng thái: {newBan.TrangThai}");
                System.Diagnostics.Debug.WriteLine($"  - Ghi chú: {newBan.GhiChu ?? "(trống)"}");
                System.Diagnostics.Debug.WriteLine($"  - Hình ảnh: {newBan.HinhAnh ?? "(trống)"}");

                System.Diagnostics.Debug.WriteLine("4. Gọi BanBiaService.AddTableAsync...");
                var result = await _banBiaService.AddTableAsync(newBan);

                this.Cursor = Cursors.Default;

                if (result)
                {
                    System.Diagnostics.Debug.WriteLine("✓ LƯU THÀNH CÔNG!");
                    MessageBox.Show("Thêm bàn thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("❌ LƯU THẤT BẠI - Service trả về false");
                    MessageBox.Show("Không thể thêm bàn! Tên bàn có thể đã tồn tại hoặc có lỗi xảy ra.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnLuu.Enabled = true;
                    btnHuy.Enabled = true;

                    // Delete uploaded image if save failed
                    if (!string.IsNullOrEmpty(imageName))
                    {
                        System.Diagnostics.Debug.WriteLine("Xóa ảnh đã upload do lưu thất bại...");
                        var startupPath = Application.StartupPath;
                        var projectRoot = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(startupPath).FullName).FullName).FullName).FullName;
                        var failedImagePath = Path.Combine(projectRoot, "Forms", "Resources", "img", "tables", imageName);

                        if (File.Exists(failedImagePath))
                        {
                            try
                            {
                                File.Delete(failedImagePath);
                                System.Diagnostics.Debug.WriteLine("✓ Đã xóa ảnh");
                            }
                            catch (Exception delEx)
                            {
                                System.Diagnostics.Debug.WriteLine($"⚠ Không xóa được ảnh: {delEx.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                btnLuu.Enabled = true;
                btnHuy.Enabled = true;

                System.Diagnostics.Debug.WriteLine($"\n❌❌❌ LỖI NGHIÊM TRỌNG ❌❌❌");
                System.Diagnostics.Debug.WriteLine($"Message: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace:\n{ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"\nInner Exception:");
                    System.Diagnostics.Debug.WriteLine($"Message: {ex.InnerException.Message}");
                    System.Diagnostics.Debug.WriteLine($"Stack Trace:\n{ex.InnerException.StackTrace}");
                }

                MessageBox.Show($"Lỗi: {ex.Message}\n\nChi tiết: {ex.InnerException?.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            System.Diagnostics.Debug.WriteLine("=== KẾT THÚC QUÁ TRÌNH LƯU ===\n");
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("User đã hủy thêm bàn");
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Dispose image if any
            if (picPreview.Image != null)
            {
                picPreview.Image.Dispose();
                picPreview.Image = null;
            }
            base.OnFormClosing(e);
        }
    }
}