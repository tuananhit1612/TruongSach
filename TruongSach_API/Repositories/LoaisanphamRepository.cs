using Microsoft.EntityFrameworkCore;
using TruongSach_API.Models;

namespace TruongSach_API.Repositories
{
    public class LoaisanphamRepository : ILoaisanphamRepository
    {
        private readonly TruongSachContext _context;

        public LoaisanphamRepository(TruongSachContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Loaisanpham>> GetAllAsync()
        {
            return await _context.Loaisanphams.ToListAsync();
        }

        public async Task<Loaisanpham> GetByIdAsync(int id)
        {
            return await _context.Loaisanphams.FindAsync(id);
        }

        public async Task<Loaisanpham> AddAsync(Loaisanpham sp)
        {
            _context.Loaisanphams.Add(sp);
            await _context.SaveChangesAsync();
            return sp;
        }

        public async Task<Loaisanpham> UpdateAsync(Loaisanpham sp)
        {
            _context.Loaisanphams.Update(sp);
            await _context.SaveChangesAsync();
            return sp;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var lsp = await _context.Loaisanphams.FindAsync(id);
            if (lsp == null) return false;

            _context.Loaisanphams.Remove(lsp);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
