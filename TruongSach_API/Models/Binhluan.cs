using System;
using System.Collections.Generic;

namespace TruongSach_API.Models;

public partial class Binhluan
{
    public int MaBinhLuan { get; set; }

    public string? NoiDung { get; set; }

    public DateTime? ThoiGian { get; set; }

    public int MaBaiViet { get; set; }

    public virtual Baiviet MaBaiVietNavigation { get; set; } = null!;
}
