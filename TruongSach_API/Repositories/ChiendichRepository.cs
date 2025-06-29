using Microsoft.EntityFrameworkCore;
using TruongSach_API.DTOs;
using TruongSach_API.Models;

namespace TruongSach_API.Repositories
{
    public class ChiendichRepository : IChiendichRepository
    {
        private readonly TruongSachContext _context;

        public ChiendichRepository(TruongSachContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Chiendich>> GetAllAsync()
            => await _context.Chiendiches.Include(u => u.MaTruongNavigation).OrderByDescending(c => c.NgayTao).ToListAsync();

        public async Task<Chiendich> GetByIdAsync(int id)
            => await _context.Chiendiches.Include(u => u.MaTruongNavigation).FirstOrDefaultAsync( u => u.MaChienDich == id);

        public async Task<Chiendich> AddAsync(Chiendich entity)
        {
            _context.Chiendiches.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Chiendich> UpdateAsync(Chiendich entity)
        {
            _context.Chiendiches.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Chiendiches.FindAsync(id);
            if (entity == null) return false;
            _context.Chiendiches.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<int> CountDistinctNguoiUngHoAsync(int maChienDich)
        {
            return await _context.Donggops
                .Where(d => d.MaChienDich == maChienDich)
                .Select(d => d.MaNguoiDung)
                .Distinct()
                .CountAsync();

        }

        public async Task<IEnumerable<Donggop>> GetLichSuUngHoByUserAsync(int maNguoiDung)
        {
            return await _context.Donggops
                .Include(d => d.MaChienDichNavigation)
                .Where(d => d.MaNguoiDung == maNguoiDung)
                .OrderByDescending(d => d.NgayDongGop)
                .ToListAsync();
        }

        public async Task<List<NguoiUngHoDTO>> GetNguoiUngHoByChienDichIdAsync(int maChienDich)
        {
            return await _context.Donggops
                .Where(dg => dg.MaChienDich == maChienDich)
                .Include(dg => dg.MaNguoiDungNavigation)
                .Select(dg => new NguoiUngHoDTO
                {
                    MaNguoiDung = dg.MaNguoiDung,
                    HoTen = dg.MaNguoiDungNavigation.HoTen,
                    SoTien = dg.SoTien,
                    NgayDongGop = dg.NgayDongGop,
                    NoiDung = dg.NoiDung,
                    TenChienDich = dg.MaChienDichNavigation.TieuDe,
                    TenTruong = dg.MaChienDichNavigation.MaTruongNavigation.TenTruong


                })
                .ToListAsync();
        }


    }

}
