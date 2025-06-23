using System;
using System.Collections.Generic;

namespace TruongSach_API.Models;

public partial class ChienDichYeuThich
{
    public int MaNguoiDung { get; set; }

    public int MaChienDich { get; set; }

    public DateTime? NgayLuu { get; set; }

    public virtual Chiendich MaChienDichNavigation { get; set; } = null!;

    public virtual Nguoidung MaNguoiDungNavigation { get; set; } = null!;
}
