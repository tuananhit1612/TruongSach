using System;
using System.Collections.Generic;

namespace TruongSach_API.Models;

public partial class Chiendich
{
    public int MaChienDich { get; set; }

    public string TieuDe { get; set; } = null!;

    public string MoTa { get; set; } = null!;

    public decimal SoTienHienTai { get; set; }

    public decimal MucTieuQuyenGop { get; set; }

    public string HinhAnhDaiDien { get; set; } = null!;

    public string? TrangThai { get; set; }

    public string? NguoiDaiDien { get; set; }

    public DateTime? NgayTao { get; set; }

    public int MaTruong { get; set; }

    public virtual ICollection<Donggop> Donggops { get; set; } = new List<Donggop>();

    public virtual ICollection<Hinhanhchiendich> Hinhanhchiendiches { get; set; } = new List<Hinhanhchiendich>();

    public virtual Truong MaTruongNavigation { get; set; } = null!;
}
