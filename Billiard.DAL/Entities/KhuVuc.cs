using System;
using System.Collections.Generic;

namespace Billiard.DAL.Entities;

public partial class KhuVuc
{
    public int MaKhuVuc { get; set; }

    public string TenKhuVuc { get; set; } = null!;

    public string? MoTa { get; set; }

    public virtual ICollection<BanBium> BanBia { get; set; } = new List<BanBium>();
}
