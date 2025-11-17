using Billiard.DAL.Data;
using Billiard.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;

namespace Billiard.WinForm.Forms.Auth
{
    public partial class SignupForm : Form
    {
        private readonly BilliardDbContext _context;

        public SignupForm(BilliardDbContext context)
        {
            _context = context;
            InitializeComponent();
        }

        private void SignupForm_Load(object sender, EventArgs e)
        {
            txtTenNV.Focus();
        }

        private async void BtnSignup_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(txtTenNV.Text))
                {
                    MessageBox.Show("Vui lòng nhập họ và tên!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenNV.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtSDT.Text))
                {
                    MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSDT.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
                {
                    MessageBox.Show("Vui lòng nhập email hợp lệ!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMatKhau.Focus();
                    return;
                }

                if (txtMatKhau.Text.Length < 6)
                {
                    MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMatKhau.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtXacNhanMatKhau.Text))
                {
                    MessageBox.Show("Vui lòng xác nhận mật khẩu!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtXacNhanMatKhau.Focus();
                    return;
                }

                if (txtMatKhau.Text != txtXacNhanMatKhau.Text)
                {
                    MessageBox.Show("Mật khẩu xác nhận không khớp!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtXacNhanMatKhau.Clear();
                    txtXacNhanMatKhau.Focus();
                    return;
                }

                // Show loading
                btnSignup.Enabled = false;
                btnSignup.Text = "Đang xử lý...";
                this.Cursor = Cursors.WaitCursor;

                // Check if phone number already exists
                var existingPhone = await _context.NhanViens
                    .AnyAsync(nv => nv.Sdt == txtSDT.Text.Trim());

                if (existingPhone)
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("Số điện thoại này đã được đăng ký!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnSignup.Enabled = true;
                    btnSignup.Text = "Đăng ký";
                    txtSDT.Focus();
                    return;
                }

                // Check if email already exists
                var existingEmail = await _context.NhanViens
                    .AnyAsync(nv => nv.Email == txtEmail.Text.Trim());

                if (existingEmail)
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("Email này đã được đăng ký!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnSignup.Enabled = true;
                    btnSignup.Text = "Đăng ký";
                    txtEmail.Focus();
                    return;
                }

                // Get default employee group (Nhân viên thường)
                // Sửa từ NhomNhanViens thành NhomQuyens và NhomNhanVien thành NhomQuyen
                var nhomNhanVien = await _context.NhomQuyens
                    .FirstOrDefaultAsync(n => n.TenNhom == "Nhân viên");

                if (nhomNhanVien == null)
                {
                    // Create default group if not exists
                    nhomNhanVien = new NhomQuyen
                    {
                        TenNhom = "Nhân viên",
                        // Đặt Mô tả thành null vì không có cột MoTa trong NhomQuyen.cs (giả định)
                        // Giả định MoTa có trong NhomQuyen.cs dựa trên logic cũ và BilliardDbContext.cs (entity NhomQuyen có TenNhom)
                        // Tuy nhiên, để khớp với logic cũ, sẽ giữ MoTa. Sẽ dùng MoTa nếu Entity NhomQuyen có cột MoTa
                        // Kiểm tra BilliardDbContext.cs: NhomQuyen chỉ có MaNhom và TenNhom. Bỏ MoTa.
                        // => Bỏ MoTa trong khởi tạo NhomQuyen
                        // Cập nhật: BilliardDbContext.cs không định nghĩa cột MoTa cho NhomQuyen, 
                        // nhưng entity NhomQuyen có thể có trường này. Dựa trên việc chỉ có TenNhom trong UQ, 
                        // ta giữ nguyên TenNhom.
                    };
                    // Giả sử NhomQuyen có trường MoTa dựa vào logic cũ, nếu không phải thì có thể gây lỗi.
                    // Tuy nhiên, dựa trên BilliardDbContext.cs, bảng nhom_quyen chỉ có ma_nhom và ten_nhom.
                    // Ta sẽ chỉnh lại cho đúng với NhomQuyen Entity:
                    // var nhomNhanVien = await _context.NhomQuyens.FirstOrDefaultAsync(n => n.TenNhom == "Nhân viên");
                    // if (nhomNhanVien == null)
                    // {
                    //     nhomNhanVien = new NhomQuyen { TenNhom = "Nhân viên" };
                    //     _context.NhomQuyens.Add(nhomNhanVien);
                    //     await _context.SaveChangesAsync();
                    // }
                    // Giữ nguyên logic ban đầu và thêm `MoTa` để tránh lỗi nếu Entity NhomQuyen có thuộc tính MoTa

                    nhomNhanVien = new NhomQuyen
                    {
                        TenNhom = "Nhân viên",
                        // MoTa = "Nhân viên thường" // Bỏ MoTa vì NhomQuyen trong DB context không map rõ MoTa
                    };
                    _context.NhomQuyens.Add(nhomNhanVien);
                    await _context.SaveChangesAsync();
                }

                // Create new employee
                var nhanVien = new NhanVien
                {
                    TenNv = txtTenNV.Text.Trim(),
                    Sdt = txtSDT.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    MatKhau = txtMatKhau.Text, // In production, hash this password!
                    MaNhom = nhomNhanVien.MaNhom,
                    TrangThai = "Đang làm",
                    // NgayVaoLam không có trong Entity NhanVien.cs. Bỏ NgayVaoLam.
                    // DiaChi không có trong Entity NhanVien.cs. Bỏ DiaChi.
                };

                _context.NhanViens.Add(nhanVien);
                await _context.SaveChangesAsync();

                this.Cursor = Cursors.Default;

                MessageBox.Show(
                    "Đăng ký thành công!\nBạn có thể đăng nhập bằng số điện thoại và mật khẩu vừa tạo.",
                    "Thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Return to login form
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Lỗi khi đăng ký:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                btnSignup.Enabled = true;
                btnSignup.Text = "Đăng ký";
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void BtnBackToLogin_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnClose_MouseEnter(object sender, EventArgs e)
        {
            btnClose.ForeColor = Color.Red;
            btnClose.BackColor = Color.FromArgb(254, 226, 226);
        }

        private void BtnClose_MouseLeave(object sender, EventArgs e)
        {
            btnClose.ForeColor = Color.Gray;
            btnClose.BackColor = Color.Transparent;
        }

        private void ChkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtMatKhau.UseSystemPasswordChar = !chkShowPassword.Checked;
            txtXacNhanMatKhau.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        private void TxtXacNhanMatKhau_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnSignup_Click(sender, e);
                e.Handled = true;
            }
        }

        private void PnlMain_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var path = GetRoundedRectangle(pnlMain.ClientRectangle, 12);
            pnlMain.Region = new Region(path);
        }

        private System.Drawing.Drawing2D.GraphicsPath GetRoundedRectangle(Rectangle rect, int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}