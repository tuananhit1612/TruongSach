using TruongSach_API.Models;

namespace TruongSach_API.Repositories
{
    public interface INguoiDungRepository
    {
        Task<Nguoidung> GetByEmailAsync(string email);
        Task<Nguoidung> GetByIdAsync(int id);
        Task<bool> EmailExistsAsync(string email);
        Task<Nguoidung> RegisterAsync(Nguoidung user);

        Task<Nguoidung> GetByGoogleIdAsync(string googleId);

        Task UpdateAsync(Nguoidung user);

        Task<List<Nguoidung>> GetAllAsync();

        Task<bool> DoiMatKhau(int id, string oldPwd, string newPwd);

        Task<decimal> LayDiemThienNguyen(int userId);

        Task<bool> DeleteAsync(int id);
    }
}
