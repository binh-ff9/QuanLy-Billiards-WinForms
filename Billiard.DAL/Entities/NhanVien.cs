using System;
using System.Collections.Generic;

namespace Billiard.DAL.Entities;

public partial class NhanVien
{
    public int MaNv { get; set; }

    public string TenNv { get; set; } = null!;

    public string? MaVanTay { get; set; }

    public string? FaceidHash { get; set; }

    public string? FaceidAnh { get; set; }

    public int MaNhom { get; set; }

    public string? Sdt { get; set; }

    public string? Email { get; set; }

    public decimal? LuongCoBan { get; set; }

    public decimal? PhuCap { get; set; }

    public string? CaMacDinh { get; set; }

    public string? TrangThai { get; set; }

    public string MatKhau { get; set; } = null!;

    public virtual ICollection<BangLuong> BangLuongs { get; set; } = new List<BangLuong>();

    public virtual ICollection<ChamCong> ChamCongs { get; set; } = new List<ChamCong>();

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual ICollection<LichSuHoatDong> LichSuHoatDongs { get; set; } = new List<LichSuHoatDong>();

    public virtual NhomQuyen MaNhomNavigation { get; set; } = null!;

    public virtual ICollection<PhieuNhap> PhieuNhaps { get; set; } = new List<PhieuNhap>();

    public virtual ICollection<SoQuy> SoQuies { get; set; } = new List<SoQuy>();
}
