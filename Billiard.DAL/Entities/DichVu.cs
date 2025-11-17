using System;
using System.Collections.Generic;

namespace Billiard.DAL.Entities;

public partial class DichVu
{
    public int MaDv { get; set; }

    public string TenDv { get; set; } = null!;

    public string? Loai { get; set; }

    public decimal Gia { get; set; }

    public string? DonVi { get; set; }

    public int? MaHang { get; set; }

    public string? TrangThai { get; set; }

    public string? MoTa { get; set; }

    public string? HinhAnh { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    public virtual MatHang? MaHangNavigation { get; set; }
}
