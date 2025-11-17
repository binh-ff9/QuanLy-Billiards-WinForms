using System;
using System.Collections.Generic;

namespace Billiard.DAL.Entities;

public partial class VietqrGiaoDich
{
    public int Id { get; set; }

    public int MaHd { get; set; }

    public string MaGiaoDich { get; set; } = null!;

    public string? QrCodeUrl { get; set; }

    public string? QrCodeBase64 { get; set; }

    public decimal SoTien { get; set; }

    public string? NoiDung { get; set; }

    public string? BankId { get; set; }

    public string? AccountNo { get; set; }

    public string? AccountName { get; set; }

    public string? TrangThai { get; set; }

    public string? MaGiaoDichNganHang { get; set; }

    public DateTime? ThoiGianThanhToan { get; set; }

    public string? NoiDungThucTe { get; set; }

    public DateTime? NgayTao { get; set; }

    public DateTime? NgayHetHan { get; set; }

    public virtual HoaDon MaHdNavigation { get; set; } = null!;
}
