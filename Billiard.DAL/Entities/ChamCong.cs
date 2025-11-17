using System;
using System.Collections.Generic;

namespace Billiard.DAL.Entities;

public partial class ChamCong
{
    public int Id { get; set; }

    public int MaNv { get; set; }

    public DateOnly Ngay { get; set; }

    public DateTime? GioVao { get; set; }

    public DateTime? GioRa { get; set; }

    public string? HinhAnhVao { get; set; }

    public string? HinhAnhRa { get; set; }

    public string? XacThucBang { get; set; }

    public decimal? SoGioLam { get; set; }

    public string? TrangThai { get; set; }

    public string? GhiChu { get; set; }

    public virtual NhanVien MaNvNavigation { get; set; } = null!;
}
