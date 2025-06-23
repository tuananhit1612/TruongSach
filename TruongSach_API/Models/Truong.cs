using System;
using System.Collections.Generic;

namespace TruongSach_API.Models;

public partial class Truong
{
    public int MaTruong { get; set; }

    public string? TenTruong { get; set; }

    public string? DiaChi { get; set; }

    public string? KhuVuc { get; set; }

    public string? GhiChu { get; set; }

    public string? ViDo { get; set; }

    public string? KinhDo { get; set; }

    public virtual ICollection<Baiviet> Baiviets { get; set; } = new List<Baiviet>();

    public virtual ICollection<Chiendich> Chiendiches { get; set; } = new List<Chiendich>();
}
