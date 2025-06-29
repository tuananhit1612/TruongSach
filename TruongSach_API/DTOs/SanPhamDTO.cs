namespace TruongSach_API.DTOs
{
    public class SanPhamDTO
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string MoTa { get; set; }
        public int SoLuongTon { get; set; }
        public decimal GiaBan { get; set; }
        public string TrangThai { get; set; }
        public DateTime? NgayDang { get; set; }
        public string? HinhAnhDaiDien { get; set; }
        public int MaLoaiSanPham { get; set; }

        public string? TenLoaiSanPham { get; set; }
    }
}
