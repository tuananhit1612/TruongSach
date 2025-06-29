namespace TruongSach_API.DTOs
{
    public class DongGopDTO
    {
        public int MaDongGop { get; set; }
        public decimal SoTien { get; set; }
        public string HinhThuc { get; set; }
        public string? NoiDung { get; set; }
        public string? TrangThai { get; set; }
        public DateTime? NgayDongGop { get; set; }
        public int MaNguoiDung { get; set; }
        public int MaChienDich { get; set; }              
    }
}
