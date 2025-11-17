using System;
using System.Collections.Generic;

namespace Billiard.DAL.Entities;

public partial class ChucNang
{
    public string MaCn { get; set; } = null!;

    public string TenCn { get; set; } = null!;

    public string? MoTa { get; set; }

    public virtual ICollection<NhomQuyen> MaNhoms { get; set; } = new List<NhomQuyen>();
}
