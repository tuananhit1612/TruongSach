namespace TuanAnhBacDatSang_DoAnWeb.DTO
{
    public class LoginApiResult
    {
        public string Message { get; set; }
        public GoogleUserDto User { get; set; }
    }

    public class GoogleUserDto
    {
        public int MaNguoiDung { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public string TenVaiTro { get; set; }
        public string GoogleId { get; set; }
    }
}
