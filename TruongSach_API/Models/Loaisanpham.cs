using System;
using System.Collections.Generic;

namespace TruongSach_API.Models;

public partial class Loaisanpham
{
    public int MaLoaiSanPham { get; set; }

    public string TenLoaiSanPham { get; set; } = null!;

    public int TongSoLuong { get; set; }

    public virtual ICollection<Sanpham> Sanphams { get; set; } = new List<Sanpham>();
}
