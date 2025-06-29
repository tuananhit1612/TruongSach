namespace TuanAnhBacDatSang_DoAnWeb.ViewModels
{
    public class NguoiDungViewModel
    {
        public int MaNguoiDung { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string? MatKhau { get; set; }
        public string? SoDienThoai { get; set; }
        public DateTime? NgayDangKy { get; set; }
        public string? TrangThai { get; set; }
        public decimal? DiemThuong { get; set; } = 0;
        public int MaVaiTro { get; set; }

        public string? TenVaiTro { get; set; } = "User";

        public string? AvatarUrl { get; set; } = "/images/nguoidung/avatar.png";
    }
}
