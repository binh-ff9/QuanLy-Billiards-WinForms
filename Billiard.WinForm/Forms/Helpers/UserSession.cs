namespace Billiard.WinForm.Forms.Helpers
{
    public static class UserSession
    {
        public static int? MaKH { get; set; } = 0;
        public static string TenKH { get; set; }
        public static string Sdt { get; set; }

        // Kiểm tra đã đăng nhập chưa
        public static bool IsLoggedIn => MaKH > 0;

        // Xóa session khi đăng xuất
        public static void Logout()
        {
            MaKH = 0;
            TenKH = null;
            Sdt = null;
        }
    }
}