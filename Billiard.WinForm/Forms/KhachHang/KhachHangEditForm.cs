using Billiard.BLL.Services.KhachHangServices;
using Billiard.DAL.Entities;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.KhachHang
{
    public partial class KhachHangEditForm : Form
    {
        private readonly KhachHangService _service;
        private int? _maKh = null;

        // Màu sắc chủ đạo
        private Color primaryColor = Color.FromArgb(99, 102, 241); // Tím Indigo
        private Color bgColor = Color.White;
        private Color inputBorderColor = Color.FromArgb(226, 232, 240); // Xám nhạt

        public KhachHangEditForm(KhachHangService service, int? maKh = null)
        {
            InitializeComponent(); // Giữ nguyên nếu Designer có code rác, nhưng ta sẽ override UI
            _service = service;
            _maKh = maKh;

            SetupCustomUI(); // Hàm vẽ giao diện đẹp
            LoadData();      // Hàm điền dữ liệu nếu là Sửa
        }

        private void SetupCustomUI()
        {
            // 1. Cấu hình Form
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(500, 600);
            this.BackColor = bgColor;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Text = _maKh == null ? "Thêm thành viên mới" : "Cập nhật thông tin";

            // Xóa hết controls cũ nếu có
            this.Controls.Clear();

            // 2. Header (Tiêu đề to)
            var lblHeader = new Label
            {
                Text = _maKh == null ? "Thêm Khách Hàng" : "Sửa Thông Tin",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = true,
                Location = new Point(30, 25)
            };
            this.Controls.Add(lblHeader);

            var lblSub = new Label
            {
                Text = "Vui lòng điền đầy đủ thông tin bên dưới",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(32, 70)
            };
            this.Controls.Add(lblSub);

            // 3. Tạo các ô nhập liệu (Dùng hàm custom ở dưới)
            int y = 100;
            txtTenKH = CreateBeautifulInput("Họ và tên (*)", 30, ref y);
            txtSDT = CreateBeautifulInput("Số điện thoại (*)", 30, ref y);
            txtEmail = CreateBeautifulInput("Email", 30, ref y);

            // NumericUpDown (Điểm) - Custom riêng một chút
            var lblDiem = new Label { Text = "Điểm tích lũy", Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.FromArgb(71, 85, 105), Location = new Point(30, y), AutoSize = true };
            this.Controls.Add(lblDiem);

            numDiem = new NumericUpDown
            {
                Location = new Point(30, y + 25),
                Width = 420,
                Height = 35,
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle,
                Maximum = 1000000,
                Minimum = 0,
                TextAlign = HorizontalAlignment.Right
            };
            this.Controls.Add(numDiem);
            y += 80;

            // 4. Footer (Nút bấm)
            var pnlFooter = new Panel { Dock = DockStyle.Bottom, Height = 80, BackColor = Color.FromArgb(248, 250, 252) }; // Xám nhạt dưới đáy
            pnlFooter.Paint += (s, e) => e.Graphics.DrawLine(new Pen(Color.FromArgb(226, 232, 240)), 0, 0, pnlFooter.Width, 0); // Kẻ ngang

            btnHuy = new Button
            {
                Text = "Hủy bỏ",
                Size = new Size(120, 45),
                Location = new Point(210, 18),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnHuy.FlatAppearance.BorderSize = 1;
            btnHuy.FlatAppearance.BorderColor = Color.Silver;
            btnHuy.Click += (s, e) => this.Close();

            btnLuu = new Button
            {
                Text = _maKh == null ? "✨ Thêm mới" : "💾 Lưu thay đổi",
                Size = new Size(140, 45),
                Location = new Point(340, 18),
                FlatStyle = FlatStyle.Flat,
                BackColor = primaryColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLuu.FlatAppearance.BorderSize = 0;
            btnLuu.Click += btnLuu_Click;

            pnlFooter.Controls.Add(btnHuy);
            pnlFooter.Controls.Add(btnLuu);
            this.Controls.Add(pnlFooter);
        }

        // Hàm hỗ trợ tạo ô nhập liệu đẹp (Label + TextBox bọc trong Panel bo góc)
        private TextBox CreateBeautifulInput(string labelText, int x, ref int y)
        {
            // Label
            var lbl = new Label
            {
                Text = labelText,
                Font = new Font("Segoe UI", 10, FontStyle.Bold), // Label đậm
                ForeColor = Color.FromArgb(71, 85, 105), // Màu xám xanh
                Location = new Point(x, y),
                AutoSize = true
            };
            this.Controls.Add(lbl);

            // Panel chứa TextBox (để vẽ viền bo tròn)
            var pnlInput = new Panel
            {
                Location = new Point(x, y + 25),
                Size = new Size(420, 40),
                BackColor = Color.White,
                Padding = new Padding(10, 8, 10, 8) // Padding nội dung
            };
            // Vẽ viền bo tròn cho Panel
            pnlInput.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = RoundedRect(new Rectangle(0, 0, pnlInput.Width - 1, pnlInput.Height - 1), 8))
                using (var pen = new Pen(inputBorderColor, 1)) // Viền mỏng
                {
                    e.Graphics.DrawPath(pen, path);
                }
            };

            // TextBox thật (nằm trong Panel)
            var txt = new TextBox
            {
                BorderStyle = BorderStyle.None, // Bỏ viền xấu xí mặc định
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11),
                BackColor = Color.White
            };

            pnlInput.Controls.Add(txt);
            this.Controls.Add(pnlInput);

            y += 80; // Tăng Y để control sau xuống dòng
            return txt;
        }

        private GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();
            if (radius == 0) { path.AddRectangle(bounds); return path; }
            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();
            return path;
        }

        private async void LoadData()
        {
            if (_maKh != null)
            {
                var kh = await _service.GetKhachHangDetailAsync(_maKh.Value);
                if (kh != null)
                {
                    txtTenKH.Text = kh.TenKh;
                    txtSDT.Text = kh.Sdt;
                    txtEmail.Text = kh.Email;
                    numDiem.Value = kh.DiemTichLuy ?? 0;
                }
            }
            else
            {
                // Chế độ thêm mới: Ẩn điểm đi cho gọn, hoặc disable
                numDiem.Enabled = false;
            }
        }

        private async void btnLuu_Click(object sender, EventArgs e)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(txtTenKH.Text)) { MessageBox.Show("Vui lòng nhập tên!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (string.IsNullOrWhiteSpace(txtSDT.Text)) { MessageBox.Show("Vui lòng nhập SĐT!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            try
            {
                if (_maKh == null)
                {
                    // Thêm mới
                    var kh = new Billiard.DAL.Entities.KhachHang
                    {
                        TenKh = txtTenKH.Text.Trim(),
                        Sdt = txtSDT.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        DiemTichLuy = 0,
                        HoatDong = true // Mặc định Active
                    };
                    await _service.AddAsync(kh);
                }
                else
                {
                    // Sửa
                    var kh = await _service.GetKhachHangDetailAsync(_maKh.Value);
                    if (kh != null)
                    {
                        kh.TenKh = txtTenKH.Text.Trim();
                        kh.Sdt = txtSDT.Text.Trim();
                        kh.Email = txtEmail.Text.Trim();
                        kh.DiemTichLuy = (int)numDiem.Value;
                        await _service.UpdateAsync(kh);
                    }
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {

        }
    }
}