using System;
using System.Collections.Generic;

namespace Billiard.DAL.Entities;

public partial class KhachHang
{
    public int MaKh { get; set; }

    public string TenKh { get; set; } = null!;

    public string? Sdt { get; set; }

    public string? MatKhau { get; set; }

    public string? Email { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public string? HangTv { get; set; }

    public int? DiemTichLuy { get; set; }

    public decimal? TongChiTieu { get; set; }

    public DateTime? NgayDangKy { get; set; }

    public DateTime? LanDenCuoi { get; set; }

    public bool? HoatDong { get; set; }

    public string? Avatar { get; set; }

    public virtual ICollection<BanBium> BanBia { get; set; } = new List<BanBium>();

    public virtual ICollection<DatBan> DatBans { get; set; } = new List<DatBan>();

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
}
