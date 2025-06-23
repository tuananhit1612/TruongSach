using System;
using System.Collections.Generic;

namespace TruongSach_API.Models;

public partial class Vaitro
{
    public int MaVaiTro { get; set; }

    public string TenVaiTro { get; set; } = null!;

    public string MoTa { get; set; } = null!;

    public virtual ICollection<Nguoidung> Nguoidungs { get; set; } = new List<Nguoidung>();
}
