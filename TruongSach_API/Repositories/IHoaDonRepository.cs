using TruongSach_API.Models;

namespace TruongSach_API.Repositories
{
    public interface IHoaDonRepository
    {
        Task<Hoadon> AddAsync(Hoadon entity, List<Chitiethoadon> chiTiet);
        Task<IEnumerable<Hoadon>> GetAllByUserIdAsync(int userId);
        Task<Hoadon?> GetByIdAsync(int id);
        Task<Hoadon?> UpdateAsync(Hoadon entity);
    }
}
