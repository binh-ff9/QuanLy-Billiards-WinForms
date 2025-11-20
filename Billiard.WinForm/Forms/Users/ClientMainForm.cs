using Billiard.BLL.Services.QLBan;
using Billiard.WinForm.Forms.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.Users
{
    public partial class ClientMainForm : Form
    {
        private readonly BanBiaService _banService;

        public ClientMainForm(BanBiaService banService)
        {
            InitializeComponent();
            _banService = banService;
            SetupUI();
        }

        private void SetupUI()
        {
            // Thêm nút Login/Profile trên góc phải
            if (UserSession.IsLoggedIn)
            {
                lblUser.Text = $"Xin chào, {UserSession.TenKH}";
                btnProfile.Visible = true;
                btnLogin.Visible = false;
            }
            else
            {
                lblUser.Text = "Khách vãng lai";
                btnProfile.Visible = false;
                btnLogin.Visible = true;
            }
        }

        private async void ClientMainForm_Load(object sender, EventArgs e)
        {
            await LoadTables();
        }

        private async Task LoadTables()
        {
            flowLayoutPanel1.Controls.Clear();
            var listBan = await _banService.GetAllTablesAsync();

            foreach (var ban in listBan)
            {
                // Tạo Card bàn đơn giản
                var btn = new Button
                {
                    Text = $"{ban.TenBan}\n({ban.MaLoaiNavigation.TenLoai})\n{ban.MaLoaiNavigation.GiaGio:N0}đ/h",
                    Size = new Size(150, 100),
                    BackColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                // Sự kiện: Bấm vào bàn để xem lịch
                btn.Click += (s, e) =>
                {
                    // Mở Form Đặt Bàn (Dialog)
                    using (var scope = Program.ServiceProvider.CreateScope())
                    {
                        var frm = scope.ServiceProvider.GetRequiredService<DatBanDialog>();
                        //frm.SetTableInfo(ban.MaBan, ban.TenBan); // Truyền thông tin bàn qua
                        frm.ShowDialog();
                    }
                };

                flowLayoutPanel1.Controls.Add(btn);
            }
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            // Mở form Profile
            var frm = Program.GetService<UserProfileForm>();
            frm.ShowDialog();
        }
    }
}
