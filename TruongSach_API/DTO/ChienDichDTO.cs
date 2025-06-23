namespace TruongSach_API.DTO
{
    public class ChienDichDTO
    {
        public int MaChienDich { get; set; }
        public string TieuDe { get; set; }
        public string MoTa { get; set; }
        public decimal SoTienHienTai { get; set; }
        public decimal MucTieuQuyenGop { get; set; }
        public string HinhAnhDaiDien { get; set; }
        public string TrangThai { get; set; }
        public string NguoiDaiDien { get; set; }
        public DateTime? NgayTao { get; set; }
        public int MaTruong { get; set; }
    }
}
