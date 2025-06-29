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
        public async Task<Nguoidung> GetByIdAsync(int id)
        {
            return await _context.Nguoidungs.Include(u => u.MaVaiTroNavigation).FirstOrDefaultAsync(u => u.MaNguoiDung == id);
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

        public async Task<List<Nguoidung>> GetAllAsync()
        {
            return await _context.Nguoidungs.ToListAsync();
        }

        public async Task<bool> DoiMatKhau(int id, string oldPwd, string newPwd)
        {
            var nd = _context.Nguoidungs.FirstOrDefault(x => x.MaNguoiDung == id);
            if (nd == null || nd.MatKhau != oldPwd)
                return false;

            nd.MatKhau = newPwd;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<decimal> LayDiemThienNguyen(int userId)
        {
            return await _context.Nguoidungs
            .Where(u => u.MaNguoiDung == userId)
            .Select(u => u.DiemThuong ?? 0)
            .FirstOrDefaultAsync();
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Nguoidungs.FindAsync(id);
            if (user == null)
                return false;

            _context.Nguoidungs.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
