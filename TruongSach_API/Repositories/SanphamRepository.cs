using Microsoft.EntityFrameworkCore;
using TruongSach_API.Models;

namespace TruongSach_API.Repositories
{
    public class SanphamRepository : ISanphamRepository
    {
        private readonly TruongSachContext _context;

        public SanphamRepository(TruongSachContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Sanpham>> GetAllAsync()
            => await _context.Sanphams.Include(sp => sp.MaLoaiSanPhamNavigation).ToListAsync();

        public async Task<Sanpham> GetByIdAsync(int id)
            => await _context.Sanphams.Include(sp => sp.MaLoaiSanPhamNavigation)
                                      .FirstOrDefaultAsync(sp => sp.MaSanPham == id);
        public async Task<IEnumerable<Sanpham>> GetAllByIdLSP(int maLoaiSanPham)
        {
            return await _context.Sanphams.Include(sp => sp.MaLoaiSanPhamNavigation)
                                           .Where(sp => sp.MaLoaiSanPham == maLoaiSanPham)
                                           .ToListAsync();
        }
        public async Task<Sanpham> AddAsync(Sanpham sp)
        {
            _context.Sanphams.Add(sp);
            await _context.SaveChangesAsync();
            return sp;
        }

        public async Task<Sanpham> UpdateAsync(Sanpham sp)
        {
            _context.Sanphams.Update(sp);
            await _context.SaveChangesAsync();
            return sp;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sp = await _context.Sanphams.FindAsync(id);
            if (sp == null) return false;
            _context.Sanphams.Remove(sp);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
