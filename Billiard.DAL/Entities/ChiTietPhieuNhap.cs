using System;
using System.Collections.Generic;

namespace Billiard.DAL.Entities;

public partial class ChiTietPhieuNhap
{
    public int Id { get; set; }

    public int MaPn { get; set; }

    public int MaHang { get; set; }

    public int SoLuongNhap { get; set; }

    public decimal DonGiaNhap { get; set; }

    public decimal? ThanhTien { get; set; }

    public virtual MatHang MaHangNavigation { get; set; } = null!;

    public virtual PhieuNhap MaPnNavigation { get; set; } = null!;
}
