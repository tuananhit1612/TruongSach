namespace TruongSach_API.DTOs
{
    public class HoaDonDTO
    {
        public int MaHoaDon { get; set; }
        public DateTime? NgayLap { get; set; }
        public decimal? TongTien { get; set; }
        public decimal? PhiVanChuyen { get; set; }
        public decimal? ThueVAT { get; set; }
        public string? HinhThucThanhToan { get; set; }
        public string? TrangThai { get; set; }
        public int MaNguoiDung { get; set; }
        public string? DiaChi { get; set; }
        public List<ChiTietHoaDonDTO>? ChiTiet { get; set; }
    }
}
