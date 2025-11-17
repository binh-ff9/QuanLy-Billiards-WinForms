using System;
using System.Collections.Generic;

namespace Billiard.DAL.Entities;

public partial class VietqrWebhookLog
{
    public int Id { get; set; }

    public string? MaGiaoDich { get; set; }

    public string? Payload { get; set; }

    public string? Headers { get; set; }

    public string? IpAddress { get; set; }

    public bool? XuLyThanhCong { get; set; }

    public string? ThongBaoLoi { get; set; }

    public DateTime? ThoiGianNhan { get; set; }
}
