using System;
using System.Collections.Generic;

namespace Billiard.DAL.Entities;

public partial class HoaDon
{
    public int MaHd { get; set; }

    public int? MaBan { get; set; }

    public int? MaKh { get; set; }

    public int? MaNv { get; set; }

    public DateTime? ThoiGianBatDau { get; set; }

    public DateTime? ThoiGianKetThuc { get; set; }

    public int? ThoiLuongPhut { get; set; }

    public decimal? TienBan { get; set; }

    public decimal? TienDichVu { get; set; }

    public decimal? GiamGia { get; set; }

    public string? GhiChuGiamGia { get; set; }

    public decimal? TongTien { get; set; }

    public string? PhuongThucThanhToan { get; set; }

    public string? TrangThai { get; set; }

    public string? MaGiaoDichQr { get; set; }

    public string? QrCodeUrl { get; set; }

    public string? GhiChu { get; set; }

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    public virtual BanBium? MaBanNavigation { get; set; }

    public virtual KhachHang? MaKhNavigation { get; set; }

    public virtual NhanVien? MaNvNavigation { get; set; }

    public virtual ICollection<SoQuy> SoQuies { get; set; } = new List<SoQuy>();

    public virtual ICollection<VietqrGiaoDich> VietqrGiaoDiches { get; set; } = new List<VietqrGiaoDich>();
}
