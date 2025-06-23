using System;
using System.Collections.Generic;

namespace TruongSach_API.Models;

public partial class Chitiethoadon
{
    public int MaSanPham { get; set; }

    public int MaHoaDon { get; set; }

    public int SoLuong { get; set; }

    public virtual Hoadon MaHoaDonNavigation { get; set; } = null!;

    public virtual Sanpham MaSanPhamNavigation { get; set; } = null!;
}
