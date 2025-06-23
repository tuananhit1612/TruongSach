using System;
using System.Collections.Generic;

namespace TruongSach_API.Models;

public partial class Nguoidung
{
    public int MaNguoiDung { get; set; }

    public string HoTen { get; set; } = null!;

    public string? Email { get; set; }

    public string? MatKhau { get; set; }

    public string? SoDienThoai { get; set; }

    public DateTime? NgayDangKy { get; set; }

    public string? TrangThai { get; set; }

    public decimal? DiemThuong { get; set; }

    public int MaVaiTro { get; set; }

    public string? GoogleId { get; set; }

    public string? AvatarUrl { get; set; }

    public virtual ICollection<ChienDichYeuThich> ChienDichYeuThiches { get; set; } = new List<ChienDichYeuThich>();

    public virtual ICollection<Donggop> Donggops { get; set; } = new List<Donggop>();

    public virtual ICollection<Hoadon> Hoadons { get; set; } = new List<Hoadon>();

    public virtual Vaitro MaVaiTroNavigation { get; set; } = null!;
}
