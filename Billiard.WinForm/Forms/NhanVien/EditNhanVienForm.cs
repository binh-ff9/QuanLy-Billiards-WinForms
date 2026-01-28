using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Billiard.BLL.Services.NhanVienService;
using Billiard.DAL.Entities;

namespace Billiard.WinForm.Forms.NhanVien
{
    public partial class EditNhanVienForm : Form
    {
        private readonly NhanVienService _service;
        private readonly int _maNV;
        private readonly int _currentUserId;
        private DAL.Entities.NhanVien _employee;
        private List<NhomQuyen> _roles;

        public EditNhanVienForm(int maNV, int currentUserId)
        {
            InitializeComponent();
            _service = new NhanVienService();
            _maNV = maNV;
            _currentUserId = currentUserId;
        }

        private void EditNhanVienForm_Load(object sender, EventArgs e)
        {
            LoadRoles();
            LoadShifts();
            LoadStatuses();
            LoadEmployeeData();
        }

        private void LoadRoles()
        {
            _roles = _service.GetAllRoles();
            cboNhomQuyen.Items.Clear();
            foreach (var role in _roles)
                cboNhomQuyen.Items.Add(new ComboItem { Value = role.MaNhom, Text = role.TenNhom });
        }

        private void LoadShifts()
        {
            cboCaLam.Items.Clear();
            cboCaLam.Items.Add(new ComboItem { Value = "Sang", Text = "Ca sáng" });
            cboCaLam.Items.Add(new ComboItem { Value = "Chieu", Text = "Ca chiều" });
            cboCaLam.Items.Add(new ComboItem { Value = "Toi", Text = "Ca tối" });
            cboCaLam.Items.Add(new ComboItem { Value = "FullTime", Text = "Full time" });
        }

        private void LoadStatuses()
        {
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.Add(new ComboItem { Value = "DangLam", Text = "Đang làm" });
            cboTrangThai.Items.Add(new ComboItem { Value = "Nghi", Text = "Nghỉ việc" });
        }

        private void LoadEmployeeData()
        {
            try
            {
                _employee = _service.GetEmployeeById(_maNV);
                if (_employee == null)
                {
                    MessageBox.Show("Không tìm thấy nhân viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    return;
                }

                txtTenNV.Text = _employee.TenNv;
                txtSDT.Text = _employee.Sdt;
                txtEmail.Text = _employee.Email;
                txtLuongCoBan.Text = (_employee.LuongCoBan ?? 0).ToString("N0");
                txtPhuCap.Text = (_employee.PhuCap ?? 0).ToString("N0");

                // Select role
                for (int i = 0; i < cboNhomQuyen.Items.Count; i++)
                {
                    if (((ComboItem)cboNhomQuyen.Items[i]).Value == _employee.MaNhom)
                    {
                        cboNhomQuyen.SelectedIndex = i;
                        break;
                    }
                }

                // Select shift
                for (int i = 0; i < cboCaLam.Items.Count; i++)
                {
                    if (((ComboItem)cboCaLam.Items[i]).Value.ToString() == _employee.CaMacDinh)
                    {
                        cboCaLam.SelectedIndex = i;
                        break;
                    }
                }

                // Select status
                for (int i = 0; i < cboTrangThai.Items.Count; i++)
                {
                    if (((ComboItem)cboTrangThai.Items[i]).Value.ToString() == _employee.TrangThai)
                    {
                        cboTrangThai.SelectedIndex = i;
                        break;
                    }
                }

                // Show current face image
                if (!string.IsNullOrEmpty(_employee.FaceidAnh))
                {
                    try
                    {
                        picCurrentFace.Image = Image.FromFile($"asset/img/{_employee.FaceidAnh}");
                        picCurrentFace.Visible = true;
                        btnDeleteFace.Visible = true;
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInput()) return;

                _employee.TenNv = txtTenNV.Text.Trim();
                _employee.Sdt = txtSDT.Text.Trim();
                _employee.Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim();
                _employee.MaNhom = ((ComboItem)cboNhomQuyen.SelectedItem).Value;
                _employee.CaMacDinh = ((ComboItem)cboCaLam.SelectedItem).Value.ToString();
                _employee.TrangThai = ((ComboItem)cboTrangThai.SelectedItem).Value.ToString();
                _employee.LuongCoBan = ParseCurrency(txtLuongCoBan.Text);
                _employee.PhuCap = ParseCurrency(txtPhuCap.Text);

                // Update password if provided
                if (!string.IsNullOrWhiteSpace(txtMatKhauMoi.Text))
                {
                    if (txtMatKhauMoi.Text.Length < 6)
                    {
                        MessageBox.Show("Mật khẩu mới phải có ít nhất 6 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (txtMatKhauMoi.Text != txtMatKhauConfirm.Text)
                    {
                        MessageBox.Show("Mật khẩu xác nhận không khớp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    _employee.MatKhau = txtMatKhauMoi.Text;
                }

                if (_service.UpdateEmployee(_employee))
                {
                    _service.LogActivity(_currentUserId, "Cập nhật nhân viên", $"Đã cập nhật thông tin NV: {_employee.TenNv} (#{_employee.MaNv})");
                    MessageBox.Show("✅ Cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("❌ Không thể cập nhật. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra trạng thái hiện tại
                if (_employee.TrangThai == "Nghi")
                {
                    MessageBox.Show("Nhân viên này đã nghỉ việc rồi!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var result = MessageBox.Show(
                    $"⚠️ Bạn có chắc muốn cho nhân viên '{_employee.TenNv}' nghỉ việc?\n\n" +
                    "Lưu ý: Nhân viên sẽ chuyển sang trạng thái 'Nghỉ việc' và không thể đăng nhập hệ thống.",
                    "Xác nhận nghỉ việc",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2
                );

                if (result == DialogResult.Yes)
                {
                    // Gọi soft delete - chỉ đổi trạng thái
                    if (_service.DeleteEmployee(_maNV))
                    {
                        _service.LogActivity(_currentUserId, "Cho nhân viên nghỉ việc",
                            $"Đã chuyển NV '{_employee.TenNv}' (#{_employee.MaNv}) sang trạng thái nghỉ việc");

                        MessageBox.Show(
                            "✅ Đã cho nhân viên nghỉ việc thành công!\n\nTrạng thái đã được chuyển sang 'Nghỉ việc'.",
                            "Thành công",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );

                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("❌ Không thể thực hiện thao tác. Vui lòng thử lại.",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (cboNhomQuyen.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn nhóm quyền!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private decimal ParseCurrency(string text)
        {
            string cleaned = text.Replace(",", "").Replace(".", "").Replace(" ", "");
            if (decimal.TryParse(cleaned, out decimal result)) return result;
            return 0;
        }

        private void btnDeleteFace_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa Face ID?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    if (_service.DeleteFaceID(_maNV))
                    {
                        picCurrentFace.Visible = false;
                        btnDeleteFace.Visible = false;
                        MessageBox.Show("✅ Đã xóa Face ID", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"❌ Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void txtLuongCoBan_Leave(object sender, EventArgs e) => FormatCurrency(txtLuongCoBan);
        private void txtPhuCap_Leave(object sender, EventArgs e) => FormatCurrency(txtPhuCap);

        private void FormatCurrency(TextBox txt)
        {
            if (decimal.TryParse(txt.Text.Replace(",", "").Replace(".", ""), out decimal value))
                txt.Text = value.ToString("N0");
        }
    }
}