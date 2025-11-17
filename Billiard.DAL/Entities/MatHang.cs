using System;
using System.Collections.Generic;

namespace Billiard.DAL.Entities;

public partial class MatHang
{
    public int MaHang { get; set; }

    public string TenHang { get; set; } = null!;

    public string? Loai { get; set; }

    public string? DonVi { get; set; }

    public decimal Gia { get; set; }

    public int? SoLuongTon { get; set; }

    public int? NguongCanhBao { get; set; }

    public int? MaNccDefault { get; set; }

    public DateOnly? NgayNhapGanNhat { get; set; }

    public string? TrangThai { get; set; }

    public string? MoTa { get; set; }

    public string? HinhAnh { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();

    public virtual ICollection<DichVu> DichVus { get; set; } = new List<DichVu>();

    public virtual NhaCungCap? MaNccDefaultNavigation { get; set; }
}
