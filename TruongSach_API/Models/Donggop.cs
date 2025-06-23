using System;
using System.Collections.Generic;

namespace TruongSach_API.Models;

public partial class Donggop
{
    public int MaDongGop { get; set; }

    public string SoTien { get; set; } = null!;

    public string HinhThuc { get; set; } = null!;

    public string? NoiDung { get; set; }

    public int MaNguoiDung { get; set; }

    public int MaChienDich { get; set; }

    public virtual Chiendich MaChienDichNavigation { get; set; } = null!;

    public virtual Nguoidung MaNguoiDungNavigation { get; set; } = null!;
}
