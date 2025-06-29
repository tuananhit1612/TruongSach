namespace TuanAnhBacDatSang_DoAnWeb.Models
{
    public class HoaDon
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
        public List<ChiTietHoaDon>? ChiTiet { get; set; }
    }
}
