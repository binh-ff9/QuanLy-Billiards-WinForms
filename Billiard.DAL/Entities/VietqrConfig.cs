using System;
using System.Collections.Generic;

namespace Billiard.DAL.Entities;

public partial class VietqrConfig
{
    public int Id { get; set; }

    public string TenCauHinh { get; set; } = null!;

    public string BankId { get; set; } = null!;

    public string AccountNo { get; set; } = null!;

    public string AccountName { get; set; } = null!;

    public string? ApiUrl { get; set; }

    public string? ClientId { get; set; }

    public string? ApiKey { get; set; }

    public string? Template { get; set; }

    public string? WebhookUrl { get; set; }

    public string? WebhookSecret { get; set; }

    public bool? TrangThai { get; set; }

    public bool? LaMacDinh { get; set; }

    public DateTime? NgayTao { get; set; }

    public DateTime? NgayCapNhat { get; set; }
}
