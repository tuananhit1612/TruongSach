using System;
using System.Collections.Generic;

namespace TruongSach_API.Models;

public partial class Donggop
{
    public int MaDongGop { get; set; }

    public decimal SoTien { get; set; }

    public string HinhThuc { get; set; } = null!;

    public string? NoiDung { get; set; }

    public int MaNguoiDung { get; set; }

    public int MaChienDich { get; set; }

    public DateTime? NgayDongGop { get; set; }

    public string? TrangThai { get; set; }

    public virtual Chiendich MaChienDichNavigation { get; set; } = null!;

    public virtual Nguoidung MaNguoiDungNavigation { get; set; } = null!;
}
