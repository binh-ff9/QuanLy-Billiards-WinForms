using System;
using System.Collections.Generic;

namespace Billiard.DAL.Entities;

public partial class LoaiBan
{
    public int MaLoai { get; set; }

    public string TenLoai { get; set; } = null!;

    public string? MoTa { get; set; }

    public decimal GiaGio { get; set; }

    public string? TrangThai { get; set; }

    public virtual ICollection<BanBium> BanBia { get; set; } = new List<BanBium>();

    public virtual ICollection<GiaGioChoi> GiaGioChois { get; set; } = new List<GiaGioChoi>();
}
