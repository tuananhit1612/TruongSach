namespace TruongSach_API.DTOs
{
    public class NguoiUngHoDTO
    {
        public int MaNguoiDung { get; set; }
        public string HoTen { get; set; }
        public decimal SoTien { get; set; }

        public string? NoiDung { get; set; }

        public string? TenChienDich { get; set; }

        public string? TenTruong { get; set; }
        public DateTime? NgayDongGop { get; set; }
    }
}
