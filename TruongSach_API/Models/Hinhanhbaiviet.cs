using System;
using System.Collections.Generic;

namespace TruongSach_API.Models;

public partial class Hinhanhbaiviet
{
    public int MaHinhAnh { get; set; }

    public string? HinhAnh { get; set; }

    public int MaBaiViet { get; set; }

    public virtual Baiviet MaBaiVietNavigation { get; set; } = null!;
}
