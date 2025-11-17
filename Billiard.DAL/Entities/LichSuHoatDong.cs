using System;
using System.Collections.Generic;

namespace Billiard.DAL.Entities;

public partial class LichSuHoatDong
{
    public int Id { get; set; }

    public DateTime? ThoiGian { get; set; }

    public int? MaNv { get; set; }

    public string? HanhDong { get; set; }

    public string? ChiTiet { get; set; }

    public virtual NhanVien? MaNvNavigation { get; set; }
}
