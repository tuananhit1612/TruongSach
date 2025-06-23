using System;
using System.Collections.Generic;

namespace TruongSach_API.Models;

public partial class Hoadon
{
    public int MaHoaDon { get; set; }

    public DateTime? NgayLap { get; set; }

    public decimal? TongTien { get; set; }

    public string? HinhThucThanhToan { get; set; }

    public string? TrangThai { get; set; }

    public int MaNguoiDung { get; set; }

    public virtual ICollection<Chitiethoadon> Chitiethoadons { get; set; } = new List<Chitiethoadon>();

    public virtual Nguoidung MaNguoiDungNavigation { get; set; } = null!;
}
