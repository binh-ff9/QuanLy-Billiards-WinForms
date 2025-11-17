using System;
using System.Collections.Generic;

namespace Billiard.DAL.Entities;

public partial class VBaoCaoVietQr
{
    public int Id { get; set; }

    public string MaGiaoDich { get; set; } = null!;

    public int MaHd { get; set; }

    public int? MaBan { get; set; }

    public string? TenBan { get; set; }

    public string? TenKh { get; set; }

    public decimal SoTien { get; set; }

    public string? NoiDung { get; set; }

    public string? TrangThai { get; set; }

    public DateTime? NgayTao { get; set; }

    public DateTime? NgayHetHan { get; set; }

    public DateTime? ThoiGianThanhToan { get; set; }

    public string? MaGiaoDichNganHang { get; set; }

    public int? ThoiGianThanhToanGiay { get; set; }
}
