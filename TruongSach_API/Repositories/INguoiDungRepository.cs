using TruongSach_API.Models;

namespace TruongSach_API.Repositories
{
    public interface INguoiDungRepository
    {
        Task<Nguoidung> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<Nguoidung> RegisterAsync(Nguoidung user);

        Task<Nguoidung> GetByGoogleIdAsync(string googleId);

        Task UpdateAsync(Nguoidung user);
    }
}
