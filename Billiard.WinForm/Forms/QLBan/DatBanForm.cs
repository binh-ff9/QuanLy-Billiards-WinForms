using Billiard.BLL.Services.QLBan;
using Billiard.DAL.Entities;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.QLBan
{
    public partial class DatBanForm : Form
    {
        private readonly BanBiaService _banBiaService;
        private readonly DatBanService _datBanService;
        private BanBium _selectedTable;
        private int? _maKhachHang;

        public DatBanForm(BanBiaService banBiaService, DatBanService datBanService)
        {
            _banBiaService = banBiaService;
            _datBanService = datBanService;
            InitializeComponent();
        }

        private async void DatBanForm_Load(object sender, EventArgs e)
        {
            try
            {
                await LoadAvailableTables();

                // Set default datetime
                dtpNgayDat.Value = DateTime.Now;
                dtpGioDat.Value = DateTime.Now.AddHours(1);
                dtpGioDat.Format = DateTimePickerFormat.Time;
                dtpGioDat.ShowUpDown = true;

                // Set giờ kết thúc mặc định sau 2 giờ
                dtpGioKetThuc.Value = dtpGioDat.Value.AddHours(2);
                dtpGioKetThuc.Format = DateTimePickerFormat.Time;
                dtpGioKetThuc.ShowUpDown = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadAvailableTables()
        {
            try
            {
                var allTables = await _banBiaService.GetAllTablesAsync();
                var availableTables = allTables.Where(b => b.TrangThai == "Trống").ToList();

                cboChonBan.DataSource = availableTables;
                cboChonBan.DisplayMember = "TenBan";
                cboChonBan.ValueMember = "MaBan";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách bàn: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CboChonBan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboChonBan.SelectedItem is BanBium ban)
            {
                _selectedTable = ban;
                UpdateTableInfo(ban);
            }
        }

        private void UpdateTableInfo(BanBium ban)
        {
            lblThongTinBan.Text = $@"
Loại bàn: {ban.MaLoaiNavigation?.TenLoai ?? "N/A"}
Khu vực: {ban.MaKhuVucNavigation?.TenKhuVuc ?? "N/A"}
Giá giờ: {ban.MaLoaiNavigation?.GiaGio:N0} đ/giờ";
        }

        private void DtpGioDat_ValueChanged(object sender, EventArgs e)
        {
            // Tự động set giờ kết thúc sau 2 giờ
            dtpGioKetThuc.Value = dtpGioDat.Value.AddHours(2);
            
        }

        private void DtpGioKetThuc_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void BtnTimKhachHang_Click(object sender, EventArgs e)
        {
            // TODO: Mở form tìm kiếm khách hàng
            MessageBox.Show("Chức năng tìm khách hàng đang được phát triển", "Thông báo");
        }


        private async void BtnXacNhan_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate
                if (_selectedTable == null)
                {
                    MessageBox.Show("Vui lòng chọn bàn!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtTenKhach.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên khách hàng!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenKhach.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtSoDienThoai.Text))
                {
                    MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoDienThoai.Focus();
                    return;
                }

                // Validate số điện thoại
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtSoDienThoai.Text, @"^0\d{9,10}$"))
                {
                    MessageBox.Show("Số điện thoại không hợp lệ!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoDienThoai.Focus();
                    return;
                }

                var gioBatDau = new DateTime(
                    dtpNgayDat.Value.Year,
                    dtpNgayDat.Value.Month,
                    dtpNgayDat.Value.Day,
                    dtpGioDat.Value.Hour,
                    dtpGioDat.Value.Minute,
                    0
                );

                var gioKetThuc = new DateTime(
                    dtpNgayDat.Value.Year,
                    dtpNgayDat.Value.Month,
                    dtpNgayDat.Value.Day,
                    dtpGioKetThuc.Value.Hour,
                    dtpGioKetThuc.Value.Minute,
                    0
                );

                // Nếu giờ kết thúc nhỏ hơn giờ bắt đầu (vd: đặt từ 23h đến 1h), thì giờ kết thúc phải là ngày hôm sau.
                if (gioKetThuc <= gioBatDau)
                {
                    // Kiểm tra xem giờ kết thúc có sớm hơn giờ bắt đầu không (trường hợp đặt qua đêm)
                    if (dtpGioKetThuc.Value.Hour < dtpGioDat.Value.Hour || (dtpGioKetThuc.Value.Hour == dtpGioDat.Value.Hour && dtpGioKetThuc.Value.Minute <= dtpGioDat.Value.Minute))
                    {
                        gioKetThuc = gioKetThuc.AddDays(1);
                    }

                    if (gioKetThuc <= gioBatDau) // Kiểm tra lại sau khi điều chỉnh
                    {
                        MessageBox.Show("Giờ kết thúc phải sau giờ bắt đầu!", "Thông báo",
                           MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                if (gioBatDau <= DateTime.Now)
                {
                    MessageBox.Show("Thời gian đặt bàn phải sau thời gian hiện tại!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // --- LOGIC KIỂM TRA GIỜ ĐẶT BÀN MỚI ---
                var gioBatDauHour = gioBatDau.Hour;
                var gioBatDauMinute = gioBatDau.Minute;

                // Quy định: Chỉ cho phép đặt bàn từ 8:00 sáng đến 2:00 sáng hôm sau
                // 8:00 (H=8) -> 23:59 (H=23) cùng ngày
                // 00:00 (H=0) -> 02:00 (H=2) hôm sau

                // Trường hợp đặt bàn vào ngày hiện tại
                bool isDuringCurrentDayHours = gioBatDauHour >= 8 && gioBatDauHour <= 23;

                // Trường hợp đặt bàn vào sáng sớm hôm sau (qua đêm)
                bool isDuringNextDayEarlyHours = gioBatDauHour >= 0 && gioBatDauHour <= 2 && gioBatDau.Date > DateTime.Now.Date; // Giả định dtpNgayDat.Value là ngày hiện tại hoặc ngày hôm sau

                // Để đơn giản, ta sẽ kiểm tra tổng thể giờ và ngày:
                // Tính giờ đặt bàn theo khung 24h quy ước (08:00 -> 25:59 là 01:59 ngày hôm sau)
                int checkHour = gioBatDauHour;
                if (checkHour >= 0 && checkHour <= 7) // Giờ từ 0h đến 7h, coi như của ngày hôm trước
                {
                    checkHour += 24;
                }

                // Giờ cho phép đặt: từ 8 (8h sáng) đến 25 (1h sáng hôm sau)
                // Giờ không cho phép đặt (dừng nhận đặt): 26 (2h sáng hôm sau) trở đi
                // checkHour = 8..25 (8:00 - 1:59) -> Cho phép
                // checkHour = 26..7 (2:00 - 7:59) -> Không cho phép

                if (checkHour < 8 || checkHour >= 26 || (checkHour == 26 && gioBatDauMinute > 0)) // Không cho đặt từ 2:00 sáng hôm sau trở đi
                {
                    MessageBox.Show("Thời gian đặt bàn nằm ngoài khung giờ cho phép (8:00 sáng - 2:00 sáng hôm sau) hoặc đã quá thời gian nhận đặt (trước 1:00 sáng hôm sau)!", "Thông báo",
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Giờ kết thúc cũng phải nằm trong giới hạn 2:00 sáng hôm sau.
                // Nếu giờ kết thúc là 2:00 hoặc sau 2:00, không cho phép.
                int checkEndHour = gioKetThuc.Hour;
                if (checkEndHour >= 0 && checkEndHour <= 7)
                {
                    checkEndHour += 24;
                }

                // Nếu giờ kết thúc lớn hơn hoặc bằng 2:00 sáng hôm sau
                if (checkEndHour > 26 || (checkEndHour == 26 && gioKetThuc.Minute > 0))
                {
                    MessageBox.Show("Giờ kết thúc đặt bàn không được sau 2:00 sáng hôm sau!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // --- KẾT THÚC LOGIC KIỂM TRA GIỜ ĐẶT BÀN MỚI ---


                // Kiểm tra bàn đã được đặt trong khoảng thời gian này chưa
                var isReserved = await _datBanService.IsTableReservedAsync(
                    _selectedTable.MaBan,
                    gioBatDau,
                    gioKetThuc
                );

                if (isReserved)
                {
                    MessageBox.Show("Bàn này đã được đặt trong khoảng thời gian này!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                // Đặt bàn
                var success = await _banBiaService.ReserveTableAsync(
                    _selectedTable.MaBan,
                    _maKhachHang,
                    txtTenKhach.Text.Trim(),
                    txtSoDienThoai.Text.Trim(),
                    gioBatDau,
                    null,
                    txtGhiChu.Text.Trim()
                );

                if (success)
                {
                    MessageBox.Show("Đặt bàn thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không thể đặt bàn. Vui lòng thử lại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đặt bàn: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}