using Microsoft.EntityFrameworkCore;
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
            => await _context.Chiendiches.Include(u => u.MaTruongNavigation).ToListAsync();

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
    }

}
