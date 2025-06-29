using Microsoft.AspNetCore.Mvc;
using TruongSach_API.DTOs;
using TruongSach_API.Models;
using TruongSach_API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace TruongSach_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NguoiDungController : ControllerBase
    {
        private readonly INguoiDungRepository _nguoiDungRepository;

        public NguoiDungController(INguoiDungRepository nguoiDungRepository)
        {
            _nguoiDungRepository = nguoiDungRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _nguoiDungRepository.GetAllAsync();

            var result = users.Select(u => new NguoiDungDTO
            {
                MaNguoiDung = u.MaNguoiDung,
                HoTen = u.HoTen,
                Email = u.Email,
                SoDienThoai = u.SoDienThoai,
                NgayDangKy = u.NgayDangKy,
                TrangThai = u.TrangThai,
                DiemThuong = u.DiemThuong ?? 0,
                MaVaiTro = u.MaVaiTro,
                TenVaiTro = u.MaVaiTroNavigation?.TenVaiTro ?? "User",
                AvatarUrl = u.AvatarUrl
            }).ToList();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var u = await _nguoiDungRepository.GetByIdAsync(id);
            if (u == null)
                return NotFound("Người dùng không tồn tại.");

            var dto = new NguoiDungDTO
            {
                MaNguoiDung = u.MaNguoiDung,
                HoTen = u.HoTen,
                Email = u.Email,
                SoDienThoai = u.SoDienThoai,
                NgayDangKy = u.NgayDangKy,
                TrangThai = u.TrangThai,
                DiemThuong = u.DiemThuong ?? 0,
                MaVaiTro = u.MaVaiTro,
                TenVaiTro = u.MaVaiTroNavigation?.TenVaiTro ?? "User",
                AvatarUrl = u.AvatarUrl
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NguoiDungDTO dto)
        {
            if (await _nguoiDungRepository.EmailExistsAsync(dto.Email))
                return BadRequest("Email đã tồn tại.");

            var user = new Nguoidung
            {
                HoTen = dto.HoTen,
                Email = dto.Email,
                MatKhau = BCrypt.Net.BCrypt.HashPassword(dto.MatKhau),
                SoDienThoai = dto.SoDienThoai,
                NgayDangKy = DateTime.Now,
                TrangThai = dto.TrangThai ?? "active",
                DiemThuong = dto.DiemThuong ?? 0,
                MaVaiTro = dto.MaVaiTro,
                AvatarUrl = dto.AvatarUrl
            };

            var createdUser = await _nguoiDungRepository.RegisterAsync(user);
            dto.MaNguoiDung = createdUser.MaNguoiDung;

            return CreatedAtAction(nameof(GetById), new { id = dto.MaNguoiDung }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] NguoiDungDTO dto)
        {
            var existingUser = await _nguoiDungRepository.GetByIdAsync(id);
            if (existingUser == null)
                return NotFound("Người dùng không tồn tại.");

            existingUser.HoTen = dto.HoTen;
            existingUser.SoDienThoai = dto.SoDienThoai ?? "0123456789";
            existingUser.TrangThai = dto.TrangThai;
            existingUser.DiemThuong = dto.DiemThuong;
            existingUser.MaVaiTro = dto.MaVaiTro;
            existingUser.AvatarUrl = dto.AvatarUrl;

            await _nguoiDungRepository.UpdateAsync(existingUser);

            return Ok("Cập nhật người dùng thành công.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _nguoiDungRepository.DeleteAsync(id);
            if (!deleted)
                return NotFound("Người dùng không tồn tại.");

            return Ok("Xóa người dùng thành công.");
        }
    }
}
