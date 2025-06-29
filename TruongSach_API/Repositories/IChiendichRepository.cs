using TruongSach_API.DTOs;
using TruongSach_API.Models;

namespace TruongSach_API.Repositories
{
    public interface IChiendichRepository
    {
        Task<IEnumerable<Chiendich>> GetAllAsync();
        Task<Chiendich> GetByIdAsync(int id);
        Task<Chiendich> AddAsync(Chiendich entity);
        Task<Chiendich> UpdateAsync(Chiendich entity);
        Task<bool> DeleteAsync(int id);

        Task<int> CountDistinctNguoiUngHoAsync(int maChienDich);
        Task<IEnumerable<Donggop>> GetLichSuUngHoByUserAsync(int maNguoiDung);
        public Task<List<NguoiUngHoDTO>> GetNguoiUngHoByChienDichIdAsync(int maChienDich);


    }
}
