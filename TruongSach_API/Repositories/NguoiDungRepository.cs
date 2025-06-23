using Microsoft.EntityFrameworkCore;
using TruongSach_API.Models;

namespace TruongSach_API.Repositories
{
    public class NguoiDungRepository : INguoiDungRepository
    {
        private readonly TruongSachContext _context;
        public NguoiDungRepository(TruongSachContext context)
        {
            _context = context;
        }
        public async Task<Nguoidung> GetByGoogleIdAsync(string googleId)
        {
            return await _context.Nguoidungs.Include(u => u.MaVaiTroNavigation).FirstOrDefaultAsync(u => u.GoogleId == googleId);
        }
        public async Task<Nguoidung> GetByEmailAsync(string email)
        {
            return await _context.Nguoidungs.Include(u => u.MaVaiTroNavigation).FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Nguoidungs.AnyAsync(u => u.Email == email);
        }
        public async Task<Nguoidung> RegisterAsync(Nguoidung user)
        {
            _context.Nguoidungs.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task UpdateAsync(Nguoidung user)
        {
            _context.Nguoidungs.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
