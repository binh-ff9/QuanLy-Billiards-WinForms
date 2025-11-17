using System;
using System.Collections.Generic;

namespace Billiard.DAL.Entities;

public partial class PhieuNhap
{
    public int MaPn { get; set; }

    public int MaNv { get; set; }

    public int MaNcc { get; set; }

    public DateTime? NgayNhap { get; set; }

    public decimal? TongTien { get; set; }

    public string? GhiChu { get; set; }

    public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();

    public virtual NhaCungCap MaNccNavigation { get; set; } = null!;

    public virtual NhanVien MaNvNavigation { get; set; } = null!;
}
