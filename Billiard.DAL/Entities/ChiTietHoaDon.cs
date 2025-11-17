using System;
using System.Collections.Generic;

namespace Billiard.DAL.Entities;

public partial class ChiTietHoaDon
{
    public int Id { get; set; }

    public int? MaHd { get; set; }

    public int? MaDv { get; set; }

    public int? SoLuong { get; set; }

    public decimal? ThanhTien { get; set; }

    public virtual DichVu? MaDvNavigation { get; set; }

    public virtual HoaDon? MaHdNavigation { get; set; }
}
