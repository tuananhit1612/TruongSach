using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruongSach_API.DTO;
using TruongSach_API.Models;
using TruongSach_API.Repositories;

namespace TruongSach_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly INguoiDungRepository _userRepository;
        public AuthController(INguoiDungRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(Nguoidung user)
        {
            if (await _userRepository.EmailExistsAsync(user.Email))
                return BadRequest("Email đã được sử dụng.");

            
            user.MatKhau = BCrypt.Net.BCrypt.HashPassword(user.MatKhau);
            user.MaVaiTro = 10000;

            user.NgayDangKy = DateTime.Now;
            user.TrangThai = "active";
            await _userRepository.RegisterAsync(user);

            return Ok("Đăng ký thành công.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginInfo)
        {
            var user = await _userRepository.GetByEmailAsync(loginInfo.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginInfo.MatKhau, user.MatKhau))
                return Unauthorized("Email hoặc mật khẩu không đúng.");
            return Ok(new
            {
                message = "Đăng nhập thành công",
                user = new
                {
                    user.MaNguoiDung,
                    user.HoTen,
                    user.Email,
                    TenVaiTro = user.MaVaiTroNavigation?.TenVaiTro ?? "User"
                }
            });
        }

        [HttpPost("login-google")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] GoogleLoginRequest request)
        {
            // 1. Tìm theo GoogleId
            var user = await _userRepository.GetByGoogleIdAsync(request.GoogleId);

            // 2. Nếu không có → kiểm tra email
            if (user == null && !string.IsNullOrEmpty(request.Email))
            {
                user = await _userRepository.GetByEmailAsync(request.Email);
                if (user != null)
                {
                    
                    user.GoogleId = request.GoogleId;
                    user.AvatarUrl = request.AvatarUrl;
                    await _userRepository.UpdateAsync(user);
                }
            }

            // 3. Nếu không tồn tại → đăng ký mới
            if (user == null)
            {
                user = new Nguoidung
                {
                    HoTen = request.FullName,
                    Email = request.Email,
                    GoogleId = request.GoogleId,
                    AvatarUrl = request.AvatarUrl,
                    NgayDangKy = DateTime.Now,
                    MaVaiTro = 10000,
                    TrangThai = "active"
                };

                await _userRepository.RegisterAsync(user);
            }
            user = await _userRepository.GetByGoogleIdAsync(user.GoogleId);
            return Ok(new
            {
                message = "Đăng nhập Google thành công",
                user = new
                {
                    user.MaNguoiDung,
                    user.HoTen,
                    user.Email,
                    user.AvatarUrl,
                    TenVaiTro = user.MaVaiTroNavigation.TenVaiTro ?? "User",
                    user.GoogleId
                }
            });
        }


    }
}
