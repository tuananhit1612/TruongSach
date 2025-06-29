using Microsoft.EntityFrameworkCore;
using TruongSach_API.Models;

namespace TruongSach_API.Repositories
{
    public class HoaDonRepository : IHoaDonRepository
    {
        private readonly TruongSachContext _context;

        public HoaDonRepository(TruongSachContext context)
        {
            _context = context;
        }

        public async Task<Hoadon> AddAsync(Hoadon entity, List<Chitiethoadon> chiTiet)
        {
            _context.Hoadons.Add(entity);
            await _context.SaveChangesAsync();

            foreach (var ct in chiTiet)
            {
                ct.MaHoaDon = entity.MaHoaDon;
                _context.Chitiethoadons.Add(ct);
            }
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<Hoadon>> GetAllByUserIdAsync(int userId)
        {
            return await _context.Hoadons
                .Where(h => h.MaNguoiDung == userId)
                .Include(h => h.MaNguoiDungNavigation)
                .ToListAsync();
        }

        public async Task<Hoadon?> GetByIdAsync(int id)
        {
            return await _context.Hoadons
                .Include(h => h.Chitiethoadons)
                .ThenInclude(ct => ct.MaSanPhamNavigation)
                .FirstOrDefaultAsync(h => h.MaHoaDon == id);
        }
        public async Task<Hoadon?> UpdateAsync(Hoadon entity)
        {
            _context.Hoadons.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
