using System;
using System.Collections.Generic;

namespace Billiard.DAL.Entities;

public partial class BangLuong
{
    public int MaLuong { get; set; }

    public int MaNv { get; set; }

    public int Thang { get; set; }

    public int Nam { get; set; }

    public decimal? TongGio { get; set; }

    public decimal? LuongCoBan { get; set; }

    public decimal? PhuCap { get; set; }

    public decimal? Thuong { get; set; }

    public decimal? Phat { get; set; }

    public decimal? TongLuong { get; set; }

    public DateTime? NgayTinh { get; set; }

    public virtual NhanVien MaNvNavigation { get; set; } = null!;
}
