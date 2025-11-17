using System;
using System.Collections.Generic;

namespace Billiard.DAL.Entities;

public partial class DatBan
{
    public int MaDat { get; set; }

    public int? MaBan { get; set; }

    public int? MaKh { get; set; }

    public string TenKhach { get; set; } = null!;

    public string Sdt { get; set; } = null!;

    public DateTime ThoiGianDat { get; set; }

    public int? SoNguoi { get; set; }

    public string? GhiChu { get; set; }

    public string? TrangThai { get; set; }

    public DateTime? NgayTao { get; set; }

    public DateTime? ThoiGianBatDau { get; set; }

    public DateTime? ThoiGianKetThuc { get; set; }

    public virtual BanBium? MaBanNavigation { get; set; }

    public virtual KhachHang? MaKhNavigation { get; set; }
}
