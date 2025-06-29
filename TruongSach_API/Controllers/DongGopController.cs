using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TruongSach_API.Models;
using TruongSach_API.DTOs;
using TruongSach_API.Repositories;

namespace TruongSach_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DongGopController : ControllerBase
    {
        private readonly IDongGopRepository _dongGopRepo;
        public DongGopController(IDongGopRepository dongGopRepo)
        {
            _dongGopRepo = dongGopRepo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _dongGopRepo.GetAllAsync();

            var result = list.Select(d => new DongGopDTO
            {
                MaDongGop = d.MaDongGop,
                SoTien = d.SoTien,
                HinhThuc = d.HinhThuc,
                NoiDung = d.NoiDung,
                TrangThai = d.TrangThai,
                NgayDongGop = d.NgayDongGop,
                MaNguoiDung = d.MaNguoiDung,
                MaChienDich = d.MaChienDich
            });

            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var d = await _dongGopRepo.GetByIdAsync(id);
            if (d == null) return NotFound();

            var dto = new DongGopDTO
            {
                MaDongGop = d.MaDongGop,
                SoTien = d.SoTien,
                HinhThuc = d.HinhThuc,
                NoiDung = d.NoiDung,
                TrangThai = d.TrangThai,
                NgayDongGop = d.NgayDongGop,
                MaNguoiDung = d.MaNguoiDung,
                MaChienDich = d.MaChienDich
            };

            return Ok(dto);
        }


        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetByUser(int id)
        {
            var list = await _dongGopRepo.GetByUserIdAsync(id);

            var result = list.Select(d => new DongGopDTO
            {
                MaDongGop = d.MaDongGop,
                SoTien = d.SoTien,
                HinhThuc = d.HinhThuc,
                NoiDung = d.NoiDung,
                TrangThai = d.TrangThai,
                NgayDongGop = d.NgayDongGop,
                MaNguoiDung = d.MaNguoiDung,
                MaChienDich = d.MaChienDich
            });

            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DongGopDTO dto)
        {
            var dongGop = new Donggop
            {
                SoTien = dto.SoTien,
                HinhThuc = dto.HinhThuc,
                NoiDung = dto.NoiDung,
                TrangThai = dto.TrangThai ?? "pending",
                NgayDongGop = dto.NgayDongGop ?? DateTime.Now,
                MaNguoiDung = dto.MaNguoiDung,
                MaChienDich = dto.MaChienDich
            };

            await _dongGopRepo.CreateAsync(dongGop);
            return Ok(new { message = "Tạo đóng góp thành công" });
        }

        [HttpGet("lichsu")]
        public async Task<IActionResult> GetLichSuDongGop()
        {
            var lichSu = await _dongGopRepo.GetLishSuDongGopAsync();
            return Ok(lichSu);

        }
    }
}
