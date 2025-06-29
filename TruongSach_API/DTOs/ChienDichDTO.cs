using System.ComponentModel.DataAnnotations.Schema;

namespace TruongSach_API.DTOs
{
    public class ChienDichDTO
    {
        public int MaChienDich { get; set; }
        public string TieuDe { get; set; }
        public string MoTa { get; set; }
        public decimal? SoTienHienTai { get; set; }
        public decimal MucTieuQuyenGop { get; set; }
        public string? HinhAnhDaiDien { get; set; }
        public string TrangThai { get; set; }
        public string? NguoiDaiDien { get; set; }
        public DateTime? NgayTao { get; set; }
        public int MaTruong { get; set; }
        public string? TenTruong { get; set; }

        public int? SoNguoiUngHo { get; set; }
        public bool? IsLiked { get; set; }


        public List<NguoiUngHoDTO> DanhSachNguoiUngHo { get; set; } = new();

    }
}
