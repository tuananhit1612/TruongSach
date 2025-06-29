using TruongSach_API.Models;

namespace TruongSach_API.Repositories
{
    public interface ILoaisanphamRepository
    {
        Task<IEnumerable<Loaisanpham>> GetAllAsync();
        Task<Loaisanpham> GetByIdAsync(int id);
        Task<Loaisanpham> AddAsync(Loaisanpham sp);
        Task<Loaisanpham> UpdateAsync(Loaisanpham sp);
        Task<bool> DeleteAsync(int id);
    }
}
