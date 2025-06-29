using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TuanAnhBacDatSang_DoAnWeb.ViewModels
{
    public class SanPhamViewModel
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

        [JsonIgnore]
        [NotMapped]
        public IFormFile? HinhAnhUpload { get; set; }
        public string? TenLoaiSanPham { get; set; }
    }
}
