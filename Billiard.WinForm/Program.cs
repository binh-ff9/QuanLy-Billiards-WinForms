using Billiard.BLL.Services;
using Billiard.BLL.Services.QLBan;
using Billiard.BLL.Services.HoaDonServices;
using Billiard.DAL.Data;
using Billiard.WinForm.Forms;
using Billiard.WinForm.Forms.Auth;
using Billiard.WinForm.Forms.HoaDon;
using Billiard.WinForm.Forms.ThongKe;
using Billiard.WinForm.Forms.QLBan;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows.Forms;
using Billiard.BLL.Services.HoaDonServices;
using Billiard.BLL.Services.KhachHangServices;
using Billiard.WinForm.Forms.KhachHang;
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

            // Load configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();

            // Setup Dependency Injection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            // Run LoginForm
            Application.Run(ServiceProvider.GetRequiredService<LoginForm>());
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // ===== THAY ĐỔI: DbContext sang TRANSIENT =====
            services.AddTransient<BilliardDbContext>(provider =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<BilliardDbContext>();
                optionsBuilder.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorNumbersToAdd: null
                    )
                );
                return new BilliardDbContext(optionsBuilder.Options);
            });

            // Register BLL Services
            services.AddTransient<AuthService>();
            services.AddTransient<EmailService>();
            services.AddTransient<DichVuService>();
            services.AddTransient<MatHangService>();
            services.AddTransient<ThongKeService>();
            services.AddTransient<BanBiaService>();
            services.AddTransient<HoaDonService>();

            // Register Forms


            // BanBia services
            services.AddScoped<BanBiaService>();

            // HoaDon services
            services.AddScoped<HoaDonService>();

            // KhachHang services
            services.AddScoped<KhachHangService>();

            services.AddScoped<DatBanService>();
            // Register Auth Forms

            services.AddTransient<LoginForm>();
            services.AddTransient<SignupForm>();
            services.AddTransient<ForgotPasswordForm>();
            services.AddTransient<ResetPasswordForm>();
            services.AddTransient<MainForm>();
            services.AddTransient<DichVuForm>();
            services.AddTransient<DichVuEditForm>();
            services.AddTransient<QLBanForm>();
            services.AddTransient<HoaDonForm>();
            services.AddTransient<ThongKeForm>();
            services.AddTransient<KhachHangForm>(); // Khách hàng
            services.AddTransient<ClientMainForm>();
            services.AddTransient<DatBanDialog>();   // Đăng ký luôn các Dialog con
            services.AddTransient<UserProfileForm>();

        }

        public static T GetService<T>() where T : class
        {
            return ServiceProvider.GetRequiredService<T>();
        }
    }
}