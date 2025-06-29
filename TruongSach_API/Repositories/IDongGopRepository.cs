using TruongSach_API.DTOs;
using TruongSach_API.Models;

namespace TruongSach_API.Repositories
{
    public interface IDongGopRepository
    {
        Task<IEnumerable<Donggop>> GetAllAsync();
        Task<Donggop> GetByIdAsync(int id);
        Task<IEnumerable<Donggop>> GetByUserIdAsync(int userId);
        Task CreateAsync(Donggop dongGop);

        Task<List<NguoiUngHoDTO>> GetLishSuDongGopAsync();
    }
}
