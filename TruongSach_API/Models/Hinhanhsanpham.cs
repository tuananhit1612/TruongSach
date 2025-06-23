using System;
using System.Collections.Generic;

namespace TruongSach_API.Models;

public partial class Hinhanhsanpham
{
    public int MaHinhAnh { get; set; }

    public string HinhAnh { get; set; } = null!;

    public int MaSanPham { get; set; }

    public virtual Sanpham MaSanPhamNavigation { get; set; } = null!;
}
