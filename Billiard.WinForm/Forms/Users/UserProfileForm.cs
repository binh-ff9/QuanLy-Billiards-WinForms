using Billiard.BLL.Services;
using Billiard.BLL.Services.HoaDonServices;
using Billiard.BLL.Services.KhachHangServices;
using Billiard.BLL.Services.QLBan;
using Billiard.WinForm.Forms.Helpers;
using Billiard.WinForm.Forms.HoaDon; // Để dùng ChiTietHoaDonControl
using Billiard.WinForm.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.Users
{
    public partial class UserProfileForm : Form
    {
        private readonly KhachHangService _khService;
        private readonly DatBanService _datBanService;
        private readonly HoaDonService _hoaDonService;

        // Controls
        private TextBox txtName, txtPhone, txtEmail, txtAddress;
        private DataGridView dgvBooking, dgvInvoice;
        private Label lblRank, lblPoints;

        public UserProfileForm(KhachHangService kh, DatBanService db, HoaDonService hd)
        {
            InitializeComponent();
            _khService = kh;
            _datBanService = db;
            _hoaDonService = hd;

            SetupUI();
            this.Load += async (s, e) => await LoadAllData();
        }

        private void SetupUI()
        {
            this.Text = "Hồ sơ cá nhân";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;

            // Tạo TabControl
            var tabControl = new TabControl { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10) };

            // --- TAB 1: THÔNG TIN ---
            var tabInfo = new TabPage("Thông tin cá nhân");
            tabInfo.BackColor = Color.White;
            SetupTabInfo(tabInfo);
            tabControl.TabPages.Add(tabInfo);

            // --- TAB 2: LỊCH SỬ ĐẶT BÀN ---
            var tabBooking = new TabPage("Lịch sử đặt bàn");
            tabBooking.BackColor = Color.White;
            dgvBooking = CreateGrid();
            // Thêm cột nút Hủy
            var btnCancel = new DataGridViewButtonColumn
            {
                Name = "btnCancel",
                HeaderText = "Thao tác",
                Text = "Hủy",
                UseColumnTextForButtonValue = true
            };
            dgvBooking.Columns.Add(btnCancel);
            dgvBooking.CellClick += DgvBooking_CellClick;

            tabBooking.Controls.Add(dgvBooking);
            tabControl.TabPages.Add(tabBooking);

            // --- TAB 3: LỊCH SỬ HÓA ĐƠN ---
            var tabInvoice = new TabPage("Lịch sử hóa đơn");
            tabInvoice.BackColor = Color.White;
            dgvInvoice = CreateGrid();
            // Thêm cột Xem chi tiết
            var btnView = new DataGridViewButtonColumn
            {
                Name = "btnView",
                HeaderText = "",
                Text = "Xem chi tiết",
                UseColumnTextForButtonValue = true
            };
            dgvInvoice.Columns.Add(btnView);
            dgvInvoice.CellClick += DgvInvoice_CellClick;

            tabInvoice.Controls.Add(dgvInvoice);
            tabControl.TabPages.Add(tabInvoice);

            this.Controls.Add(tabControl);
        }

        private void SetupTabInfo(TabPage page)
        {
            int x = 50, y = 40;
            int w = 300;

            // Avatar giả
            var lblAvatar = new Label { Text = "👤", Font = new Font("Segoe UI", 50), AutoSize = true, Location = new Point(x, y) };
            page.Controls.Add(lblAvatar);

            // Hạng & Điểm
            lblRank = new Label { Text = "Hạng: ...", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.OrangeRed, Location = new Point(x + 300, y + 20), AutoSize = true };
            lblPoints = new Label { Text = "Điểm: ...", Font = new Font("Segoe UI", 10), ForeColor = Color.Gray, Location = new Point(x + 300, y + 50), AutoSize = true };
            page.Controls.Add(lblRank);
            page.Controls.Add(lblPoints);

            y += 120;

            // Inputs
            txtName = AddInput(page, "Họ và tên:", x, ref y, w);
            txtPhone = AddInput(page, "Số điện thoại:", x, ref y, w);
            txtPhone.ReadOnly = true; // SĐT thường không cho sửa để định danh
            txtEmail = AddInput(page, "Email:", x, ref y, w);

            // Button Save
            var btnSave = new Button
            {
                Text = "Cập nhật thông tin",
                Location = new Point(x, y + 20),
                Size = new Size(w, 45),
                BackColor = Color.FromArgb(99, 102, 241),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSave.Click += async (s, e) => await UpdateProfile();
            page.Controls.Add(btnSave);
        }

        private TextBox AddInput(TabPage p, string label, int x, ref int y, int w)
        {
            p.Controls.Add(new Label { Text = label, Location = new Point(x, y), AutoSize = true, ForeColor = Color.Gray });
            var txt = new TextBox { Location = new Point(x, y + 25), Width = w, Font = new Font("Segoe UI", 11), BorderStyle = BorderStyle.FixedSingle };
            p.Controls.Add(txt);
            y += 70;
            return txt;
        }

        private DataGridView CreateGrid()
        {
            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowTemplate = { Height = 40 }
            };
            return dgv;
        }

        // --- LOGIC LOADING DATA ---

        private async Task LoadAllData()
        {
            if (!UserSession.IsLoggedIn) return;
            int maKh = (int)UserSession.MaKH;

            // 1. Load Info
            var kh = await _khService.GetKhachHangDetailAsync(maKh);
            if (kh != null)
            {
                txtName.Text = kh.TenKh;
                txtPhone.Text = kh.Sdt;
                txtEmail.Text = kh.Email;
                lblPoints.Text = $"Điểm tích lũy: {kh.DiemTichLuy ?? 0}";

                // Logic hạng đơn giản
                int diem = kh.DiemTichLuy ?? 0;
                string rank = diem > 1000 ? "Vàng" : (diem > 500 ? "Bạc" : "Đồng");
                lblRank.Text = $"Hạng thành viên: {rank}";
            }

            // 2. Load Booking History
            var bookings = await _datBanService.GetByCustomerAsync(maKh);
            dgvBooking.DataSource = bookings.Select(b => new
            {
                MaDat = b.MaDat,
                Ngay = b.ThoiGianBatDau?.ToString("dd/MM/yyyy"),
                Gio = $"{b.ThoiGianBatDau:HH:mm} - {b.ThoiGianKetThuc:HH:mm}",
                Ban = b.MaBanNavigation?.TenBan ?? "Unknown",
                TrangThai = b.TrangThai
            }).ToList();

            // Ẩn cột ID
            if (dgvBooking.Columns["MaDat"] != null) dgvBooking.Columns["MaDat"].Visible = false;

            // 3. Load Invoice History
            var invoices = await _hoaDonService.GetHistoryByCustomerAsync(maKh);
            dgvInvoice.DataSource = invoices.Select(i => new
            {
                MaHD = i.MaHd,
                NgayChoi = i.ThoiGianBatDau?.ToString("dd/MM/yyyy HH:mm"),
                Ban = i.MaBanNavigation?.TenBan,
                TongTien = i.TongTien,
                TrangThai = i.TrangThai
            }).ToList();

            if (dgvInvoice.Columns["TongTien"] != null)
                dgvInvoice.Columns["TongTien"].DefaultCellStyle.Format = "N0";
        }

        // --- LOGIC CẬP NHẬT PROFILE ---
        private async Task UpdateProfile()
        {
            try
            {
                // Tạo scope mới để update
                using (var scope = Program.ServiceProvider.CreateScope())
                {
                    var sv = scope.ServiceProvider.GetRequiredService<KhachHangService>();
                    var kh = await sv.GetKhachHangDetailAsync((int)UserSession.MaKH);

                    if (kh != null)
                    {
                        kh.TenKh = txtName.Text.Trim();
                        kh.Email = txtEmail.Text.Trim();

                        await sv.UpdateAsync(kh);

                        // Cập nhật lại Session
                        UserSession.TenKH = kh.TenKh;
                        MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        // --- LOGIC HỦY ĐẶT BÀN ---
        private async void DgvBooking_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != dgvBooking.Columns["btnCancel"].Index) return;

            string status = dgvBooking.Rows[e.RowIndex].Cells["TrangThai"].Value.ToString();
            if (status != "Đang chờ")
            {
                MessageBox.Show("Chỉ có thể hủy đơn đang chờ xác nhận!", "Không thể hủy", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn muốn hủy lịch đặt này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int maDat = (int)dgvBooking.Rows[e.RowIndex].Cells["MaDat"].Value;
                await _datBanService.CancelBookingAsync(maDat);
                await LoadAllData(); // Refresh lưới
            }
        }

        // --- LOGIC XEM CHI TIẾT HÓA ĐƠN ---
        private async void DgvInvoice_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != dgvInvoice.Columns["btnView"].Index) return;

            int maHd = (int)dgvInvoice.Rows[e.RowIndex].Cells["MaHD"].Value;

            using (var scope = Program.ServiceProvider.CreateScope())
            {
                var hdSv = scope.ServiceProvider.GetRequiredService<HoaDonService>();
                var fullInfo = await hdSv.GetChiTietHoaDon(maHd);

                if (fullInfo != null)
                {
                    // Tái sử dụng UserControl ChiTietHoaDonControl
                    var detailControl = new ChiTietHoaDonControl();
                    detailControl.LoadData(fullInfo);

                    // Mở trong Form Popup
                    var popup = new PopupDetailForm(detailControl, $"Chi tiết hóa đơn #{maHd}");
                    popup.ShowDialog();
                }
            }
        }
    }
}