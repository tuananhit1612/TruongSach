using System;
using System.Collections.Generic;

namespace TruongSach_API.Models;

public partial class Baiviet
{
    public int MaBaiViet { get; set; }

    public string TieuDe { get; set; } = null!;

    public string NoiDung { get; set; } = null!;

    public string HinhAnh { get; set; } = null!;

    public string? TrangThai { get; set; }

    public DateTime? NgayDang { get; set; }

    public int MaTruong { get; set; }

    public virtual ICollection<Binhluan> Binhluans { get; set; } = new List<Binhluan>();

    public virtual ICollection<Hinhanhbaiviet> Hinhanhbaiviets { get; set; } = new List<Hinhanhbaiviet>();

    public virtual ICollection<Luotthich> Luotthiches { get; set; } = new List<Luotthich>();

    public virtual Truong MaTruongNavigation { get; set; } = null!;
}
