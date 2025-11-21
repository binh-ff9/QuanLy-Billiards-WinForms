using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Billiard.BLL.Services.NhanVienService;
using Billiard.DAL.Entities;

namespace Billiard.WinForm.Forms.NhanVien
{
    public partial class AddNhanVienForm : Form
    {
        private readonly NhanVienService _service;
        private readonly int _currentUserId;
        private string _capturedFaceImagePath;
        private List<NhomQuyen> _roles;

        public AddNhanVienForm(int currentUserId)
        {
            InitializeComponent();
            _service = new NhanVienService();
            _currentUserId = currentUserId;
        }

        private void AddNhanVienForm_Load(object sender, EventArgs e)
        {
            LoadRoles();
            LoadShifts();
        }

        private void LoadRoles()
        {
            try
            {
                _roles = _service.GetAllRoles();
                cboNhomQuyen.Items.Clear();
                cboNhomQuyen.Items.Add(new ComboItem { Value = 0, Text = "-- Chọn nhóm quyền --" });
                foreach (var role in _roles)
                    cboNhomQuyen.Items.Add(new ComboItem { Value = role.MaNhom, Text = role.TenNhom });
                cboNhomQuyen.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải nhóm quyền: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadShifts()
        {
            cboCaLam.Items.Clear();
            cboCaLam.Items.Add(new ComboItem { Value = "Sang", Text = "Ca sáng" });
            cboCaLam.Items.Add(new ComboItem { Value = "Chieu", Text = "Ca chiều" });
            cboCaLam.Items.Add(new ComboItem { Value = "Toi", Text = "Ca tối" });
            cboCaLam.Items.Add(new ComboItem { Value = "FullTime", Text = "Full time" });
            cboCaLam.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInput()) return;

                var employee = new DAL.Entities.NhanVien
                {
                    TenNv = txtTenNV.Text.Trim(),
                    Sdt = txtSDT.Text.Trim(),
                    Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
                    MaNhom = ((ComboItem)cboNhomQuyen.SelectedItem).Value,
                    CaMacDinh = ((ComboItem)cboCaLam.SelectedItem).Value.ToString(),
                    LuongCoBan = ParseCurrency(txtLuongCoBan.Text),
                    PhuCap = ParseCurrency(txtPhuCap.Text),
                    MatKhau = txtMatKhau.Text,
                    TrangThai = "DangLam",
                    FaceidAnh = _capturedFaceImagePath
                };

                if (_service.AddEmployee(employee))
                {
                    _service.LogActivity(_currentUserId, "Thêm nhân viên", $"Đã thêm nhân viên: {employee.TenNv}");
                    MessageBox.Show("✅ Thêm nhân viên thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("❌ Không thể thêm nhân viên. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTenNV.Text))
            {
                MessageBox.Show("Vui lòng nhập họ và tên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNV.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSDT.Text) || txtSDT.Text.Length < 10)
            {
                MessageBox.Show("Số điện thoại không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return false;
            }

            if (cboNhomQuyen.SelectedIndex <= 0)
            {
                MessageBox.Show("Vui lòng chọn nhóm quyền!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboNhomQuyen.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMatKhau.Text) || txtMatKhau.Text.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return false;
            }

            if (txtMatKhau.Text != txtMatKhauConfirm.Text)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhauConfirm.Focus();
                return false;
            }

            return true;
        }

        private decimal ParseCurrency(string text)
        {
            string cleaned = text.Replace(",", "").Replace(".", "").Replace(" ", "");
            if (decimal.TryParse(cleaned, out decimal result))
                return result;
            return 0;
        }

        private void btnCaptureFace_Click(object sender, EventArgs e)
        {
            MessageBox.Show("⚠️ Chức năng Face ID đang được phát triển", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void txtLuongCoBan_Leave(object sender, EventArgs e)
        {
            FormatCurrency(txtLuongCoBan);
        }

        private void txtPhuCap_Leave(object sender, EventArgs e)
        {
            FormatCurrency(txtPhuCap);
        }

        private void FormatCurrency(TextBox txt)
        {
            if (decimal.TryParse(txt.Text.Replace(",", "").Replace(".", ""), out decimal value))
                txt.Text = value.ToString("N0");
        }
    }

    public class ComboItem
    {
        public dynamic Value { get; set; }
        public string Text { get; set; }
        public override string ToString() => Text;
    }
}