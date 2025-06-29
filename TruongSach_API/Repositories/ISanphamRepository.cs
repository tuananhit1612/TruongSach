using TruongSach_API.Models;

namespace TruongSach_API.Repositories
{
    public interface ISanphamRepository
    {
        Task<IEnumerable<Sanpham>> GetAllAsync();
        Task<Sanpham> GetByIdAsync(int id);
        Task<Sanpham> AddAsync(Sanpham sp);
        Task<Sanpham> UpdateAsync(Sanpham sp);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Sanpham>> GetAllByIdLSP(int maLoaiSanPham);
    }
}
