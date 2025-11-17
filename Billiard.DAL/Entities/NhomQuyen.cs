using System;
using System.Collections.Generic;

namespace Billiard.DAL.Entities;

public partial class NhomQuyen
{
    public int MaNhom { get; set; }

    public string TenNhom { get; set; } = null!;

    public virtual ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();

    public virtual ICollection<ChucNang> MaCns { get; set; } = new List<ChucNang>();
}
