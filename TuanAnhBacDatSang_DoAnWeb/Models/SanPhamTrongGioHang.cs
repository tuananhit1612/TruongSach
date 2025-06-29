namespace TuanAnhBacDatSang_DoAnWeb.Models
{
    public class SanPhamTrongGioHang
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public decimal GiaBan { get; set; }
        public int SoLuong { get; set; }

        public string HinhAnh { get; set; }
        
        public decimal ThanhTien => GiaBan * SoLuong;

    }
}
