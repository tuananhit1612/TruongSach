using System;
using System.Collections.Generic;

namespace TruongSach_API.Models;

public partial class Hinhanhchiendich
{
    public int MaHinhAnh { get; set; }

    public string? HinhAnh { get; set; }

    public int MaChienDich { get; set; }

    public virtual Chiendich MaChienDichNavigation { get; set; } = null!;
}
