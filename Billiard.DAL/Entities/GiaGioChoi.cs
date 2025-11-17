using System;
using System.Collections.Generic;

namespace Billiard.DAL.Entities;

public partial class GiaGioChoi
{
    public int Id { get; set; }

    public string KhungGio { get; set; } = null!;

    public TimeOnly GioBatDau { get; set; }

    public TimeOnly GioKetThuc { get; set; }

    public int? MaLoai { get; set; }

    public decimal Gia { get; set; }

    public DateOnly? ApDungTuNgay { get; set; }

    public string? TrangThai { get; set; }

    public virtual LoaiBan? MaLoaiNavigation { get; set; }
}
