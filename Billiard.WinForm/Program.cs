using Billiard.BLL.Services;
using Billiard.BLL.Services.QLBan;
using Billiard.BLL.Services.HoaDonServices;
using Billiard.DAL.Data;
using Billiard.WinForm.Forms;
using Billiard.WinForm.Forms.Auth;
using Billiard.WinForm.Forms.HoaDon;
using Billiard.WinForm.Forms.QLBan;
using Billiard.WinForm.Forms.CaiDat;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows.Forms;
using Billiard.BLL.Services.VietQR;

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
            // ✅ QUAN TRỌNG: Đăng ký DbContext với TRANSIENT lifetime cho WinForms
            services.AddDbContext<BilliardDbContext>(
                options => options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorNumbersToAdd: null
                    )
                ),
                ServiceLifetime.Transient,  // ← THÊM DÒNG NÀY
                ServiceLifetime.Transient   // ← VÀ DÒNG NÀY
            );

            // ✅ Đổi tất cả Services từ Scoped → Transient
            services.AddTransient<AuthService>();
            services.AddTransient<EmailService>();
            services.AddTransient<DichVuService>();
            services.AddTransient<MatHangService>();
            services.AddTransient<AuthService>();
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
            services.AddTransient<ThanhToanService>();
            services.AddTransient<VietQRConfigForm>();
            // Register Auth Forms
            services.AddTransient<LoginForm>();
            services.AddTransient<SignupForm>();
            services.AddTransient<ForgotPasswordForm>();
            services.AddTransient<ResetPasswordForm>();

            // Register Main Form
            services.AddTransient<MainForm>();

            // Register Feature Forms
            services.AddTransient<DichVuForm>();
            services.AddTransient<DichVuEditForm>();
            services.AddTransient<QLBanForm>();
            services.AddTransient<HoaDonForm>();
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