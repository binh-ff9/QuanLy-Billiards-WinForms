using System;
using System.Collections.Generic;

namespace Billiard.DAL.Entities;

public partial class BanBium
{
    public int MaBan { get; set; }

    public string TenBan { get; set; } = null!;

    public int MaLoai { get; set; }

    public int MaKhuVuc { get; set; }

    public string? TrangThai { get; set; }

    public DateTime? GioBatDau { get; set; }

    public int? MaKh { get; set; }

    public int? ViTriX { get; set; }

    public int? ViTriY { get; set; }

    public string? GhiChu { get; set; }

    public DateTime? NgayTao { get; set; }

    public string? HinhAnh { get; set; }

    public virtual ICollection<DatBan> DatBans { get; set; } = new List<DatBan>();

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual KhachHang? MaKhNavigation { get; set; }

    public virtual KhuVuc MaKhuVucNavigation { get; set; } = null!;

    public virtual LoaiBan MaLoaiNavigation { get; set; } = null!;
}
