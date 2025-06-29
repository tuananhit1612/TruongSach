using Microsoft.EntityFrameworkCore;
using TruongSach_API.DTOs;
using TruongSach_API.Models;

namespace TruongSach_API.Repositories
{
    public class DongGopRepository : IDongGopRepository
    {
        private readonly TruongSachContext _context;

        public DongGopRepository(TruongSachContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Donggop>> GetAllAsync()
        {
            return await _context.Donggops
                .Include(d => d.MaNguoiDungNavigation)
                .Include(d => d.MaChienDichNavigation)
                .ToListAsync();
        }

        public async Task<Donggop> GetByIdAsync(int id)
        {
            return await _context.Donggops
                .Include(d => d.MaNguoiDungNavigation)
                .Include(d => d.MaChienDichNavigation)
                .FirstOrDefaultAsync(d => d.MaDongGop == id);
        }

        public async Task<IEnumerable<Donggop>> GetByUserIdAsync(int userId)
        {
            return await _context.Donggops
                .Where(d => d.MaNguoiDung == userId)
                .Include(d => d.MaChienDichNavigation)
                .ToListAsync();
        }

        public async Task CreateAsync(Donggop dongGop)
        {
            dongGop.NgayDongGop = DateTime.Now;
            _context.Donggops.Add(dongGop);
            await _context.SaveChangesAsync();
        }
        public async Task<List<NguoiUngHoDTO>> GetLishSuDongGopAsync()
        {
            return await _context.Donggops
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
