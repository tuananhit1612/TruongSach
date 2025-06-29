using TuanAnhBacDatSang_DoAnWeb.Models;

namespace TuanAnhBacDatSang_DoAnWeb.ViewModels
{
    public class CheckoutViewModel
    {
        public GioHang GioHang { get; set; }
        public string? HoTen { get; set; }
        public string? Email { get; set; }
        public string? SoDienThoai { get; set; }
        public string? DiaChi { get; set; }
    }
}
