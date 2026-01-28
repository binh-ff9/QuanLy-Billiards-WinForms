using Billiard.BLL.Services;
using Billiard.BLL.Services.QLBan;
using Billiard.BLL.Services.HoaDonServices;
using Billiard.DAL.Data;
using Billiard.WinForm.Forms;
using Billiard.WinForm.Forms.Auth;
using Billiard.WinForm.Forms.HoaDon;
<<<<<<< Updated upstream
=======
using Billiard.WinForm.Forms.KhachHang;
using Billiard.WinForm.Forms.NhanVien;
using Billiard.WinForm.Forms.QLBan;
>>>>>>> Stashed changes
using Billiard.WinForm.Forms.ThongKe;
using Billiard.WinForm.Forms.QLBan;
using Billiard.WinForm.Forms.CaiDat;
using Billiard.BLL.Services.KhachHangServices;
using Billiard.WinForm.Forms.KhachHang;
using Billiard.BLL.Services.VietQR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows.Forms;
using Billiard.WinForm.Forms.Users;

namespace Billiard.WinForm
{
    internal static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        public static IConfiguration Configuration { get; private set; }

        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

<<<<<<< Updated upstream
            // Load configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();
=======
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
>>>>>>> Stashed changes

            // Setup Dependency Injection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            // Run LoginForm
            Application.Run(ServiceProvider.GetRequiredService<ClientMainForm>());
        }

        private static void ConfigureServices(IServiceCollection services)
        {
<<<<<<< Updated upstream
            // DbContext - GIỮ NGUYÊN TRANSIENT
=======
            services.AddSingleton<IConfiguration>(Configuration);

            // DbContext
>>>>>>> Stashed changes
            services.AddTransient<BilliardDbContext>(provider =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<BilliardDbContext>();
                optionsBuilder.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")
                );
                return new BilliardDbContext(optionsBuilder.Options);
            });

<<<<<<< Updated upstream
            // ✅ Đổi tất cả Services từ Scoped → Transient (theo chỉ dẫn)
=======
            // Services
>>>>>>> Stashed changes
            services.AddTransient<AuthService>();
            services.AddTransient<EmailService>();
            services.AddTransient<DichVuService>();
            services.AddTransient<MatHangService>();
            services.AddTransient<ThongKeService>();
<<<<<<< Updated upstream
=======
            services.AddTransient<NhanVienService>();
>>>>>>> Stashed changes

            // HttpClient
            services.AddSingleton<HttpClient>();

            // BanBia services
            services.AddTransient<BanBiaService>();
            services.AddTransient<DatBanService>();
            services.AddTransient<LoaiBanService>();
            services.AddTransient<KhuVucService>();

            // HoaDon services
            services.AddTransient<HoaDonService>();
            services.AddTransient<VietQRService>();
            services.AddTransient<ThanhToanService>();
            services.AddTransient<VietQRConfigForm>();

            services.AddScoped<DatBanService>();

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
<<<<<<< Updated upstream
            services.AddTransient<KhachHangForm>(); // Khách hàng
            services.AddTransient<ClientMainForm>();
            services.AddTransient<DatBanDialog>();   // Đăng ký luôn các Dialog con
            services.AddTransient<UserProfileForm>();
=======
            services.AddTransient<KhachHangForm>();
            services.AddTransient<ClientMainForm>();
            services.AddTransient<DatBanDialog>();
            services.AddTransient<UserProfileForm>();

            // NhanVien Forms
            services.AddTransient<NhanVienForm>();
            services.AddTransient<AddNhanVienForm>();
            services.AddTransient<EditNhanVienForm>();

            // ✅ CaiDat Forms & UserControls
            services.AddTransient<CaiDatForm>();
            services.AddTransient<ucKiemSoatKho>();
            services.AddTransient<ucLichSuHoatDong>();
            services.AddTransient<ucPhieuNhapXuat>();
>>>>>>> Stashed changes
        }

        // ✅ THÊM: Method để tạo Scope mới (tùy chọn)
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