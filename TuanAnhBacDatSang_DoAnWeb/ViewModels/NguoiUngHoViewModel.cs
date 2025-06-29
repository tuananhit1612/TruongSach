namespace TuanAnhBacDatSang_DoAnWeb.ViewModels
{
    public class NguoiUngHoViewModel
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
