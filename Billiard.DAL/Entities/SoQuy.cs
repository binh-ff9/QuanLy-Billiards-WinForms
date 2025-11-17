using System;
using System.Collections.Generic;

namespace Billiard.DAL.Entities;

public partial class SoQuy
{
    public int MaPhieu { get; set; }

    public int MaNv { get; set; }

    public DateTime? NgayLap { get; set; }

    public string LoaiPhieu { get; set; } = null!;

    public decimal SoTien { get; set; }

    public string LyDo { get; set; } = null!;

    public int? MaHdLienQuan { get; set; }

    public virtual HoaDon? MaHdLienQuanNavigation { get; set; }

    public virtual NhanVien MaNvNavigation { get; set; } = null!;
}
