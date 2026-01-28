using Billiard.BLL.Services;
using Billiard.BLL.Services.HoaDonServices;
using Billiard.BLL.Services.KhachHangServices;
using Billiard.BLL.Services.NhanVienService;
using Billiard.BLL.Services.QLBan;
using Billiard.BLL.Services.VietQR;
using Billiard.DAL.Data;
using Billiard.WinForm.Forms;
using Billiard.WinForm.Forms.Auth;
using Billiard.WinForm.Forms.CaiDat;
using Billiard.WinForm.Forms.HoaDon;
using Billiard.WinForm.Forms.KhachHang;
using Billiard.WinForm.Forms.NhanVien;
using Billiard.WinForm.Forms.QLBan;
using Billiard.WinForm.Forms.ThongKe;
using Billiard.WinForm.Forms.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows.Forms;

namespace Billiard.WinForm
{
    internal static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; } = null!;
        public static IConfiguration Configuration { get; private set; } = null!;

        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                Configuration = builder.Build();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"❌ Không thể load cấu hình!\n\n" +
                    $"Lỗi: {ex.Message}\n\n" +
                    $"Vui lòng kiểm tra file appsettings.json",
                    "Lỗi Cấu Hình",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            Application.Run(ServiceProvider.GetRequiredService<User>());
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);

            // DbContext
            services.AddTransient<BilliardDbContext>(provider =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<BilliardDbContext>();
                optionsBuilder.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")
                );
                return new BilliardDbContext(optionsBuilder.Options);
            });

            // Đăng ký Factory cho DbContext
            services.AddDbContextFactory<BilliardDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // ✅ Services (Transient)
            services.AddTransient<AuthService>();
            services.AddTransient<EmailService>();
            services.AddTransient<DichVuService>();
            services.AddTransient<MatHangService>();
            services.AddTransient<ThongKeService>();
            services.AddTransient<NhanVienService>();
            services.AddTransient<System.Net.Http.HttpClient>();

            // BanBia services
            services.AddTransient<BanBiaService>();
            services.AddTransient<DatBanService>();
            services.AddTransient<GioHoatDongService>();
            services.AddTransient<LoaiBanService>();
            services.AddTransient<KhuVucService>();

            // HoaDon services
            services.AddTransient<HoaDonService>();
            services.AddTransient<VietQRService>();
            services.AddTransient<ThanhToanService>();
            services.AddTransient<VietQRConfigForm>();

            // KhachHang services
            services.AddTransient<KhachHangService>();

            // Register Auth Forms
            services.AddTransient<LoginForm>();
            services.AddTransient<SignupForm>();
            services.AddTransient<ForgotPasswordForm>();
            services.AddTransient<ResetPasswordForm>();

            // Main Forms
            services.AddTransient<MainForm>();
            services.AddTransient<DichVuForm>();
            services.AddTransient<DichVuEditForm>();
            services.AddTransient<QLBanForm>();
            services.AddTransient<HoaDonForm>();
            services.AddTransient<ThongKeForm>();
            services.AddTransient<KhachHangForm>();
            services.AddTransient<ClientMainForm>();
            services.AddTransient<User>(); // User
            services.AddTransient<DatBanDialog>();   // Đăng ký luôn các Dialog con
            services.AddTransient<UserProfileForm>();

            // NhanVien Forms
            services.AddTransient<NhanVienForm>();
            services.AddTransient<AddNhanVienForm>();
            services.AddTransient<EditNhanVienForm>();

            // CaiDat Forms & UserControls
            services.AddTransient<CaiDatForm>();
            services.AddTransient<ucKiemSoatKho>();
            services.AddTransient<ucLichSuHoatDong>();
            services.AddTransient<ucPhieuNhapXuat>();
        }

        public static IServiceScope CreateScope()
        {
            return ServiceProvider.CreateScope();
        }

        public static T GetService<T>() where T : class
        {
            return ServiceProvider.GetRequiredService<T>();
        }
    }
}