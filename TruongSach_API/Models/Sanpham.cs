using System;
using System.Collections.Generic;

namespace TruongSach_API.Models;

public partial class Sanpham
{
    public int MaSanPham { get; set; }

    public string TenSanPham { get; set; } = null!;

    public string? MoTa { get; set; }

    public int SoLuongTon { get; set; }

    public decimal GiaBan { get; set; }

    public string? TrangThai { get; set; }

    public string? HinhAnhDaiDien { get; set; }

    public int MaLoaiSanPham { get; set; }

    public virtual ICollection<Chitiethoadon> Chitiethoadons { get; set; } = new List<Chitiethoadon>();

    public virtual ICollection<Hinhanhsanpham> Hinhanhsanphams { get; set; } = new List<Hinhanhsanpham>();

    public virtual Loaisanpham MaLoaiSanPhamNavigation { get; set; } = null!;
}
