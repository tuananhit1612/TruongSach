using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TruongSach_API.DTOs;
using TruongSach_API.Models;
using TruongSach_API.Repositories;

namespace TruongSach_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class SanPhamController : ControllerBase
    {
        private readonly ISanphamRepository _sanphamRepository;
        public SanPhamController(ISanphamRepository sanphamRepository)
        {
            _sanphamRepository = sanphamRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SanPhamDTO>>> GetAll()
        {
            var list = await _sanphamRepository.GetAllAsync();

            var result = list.Select(sp => new SanPhamDTO
            {
                MaSanPham = sp.MaSanPham,
                TenSanPham = sp.TenSanPham,
                MoTa = sp.MoTa,
                SoLuongTon = sp.SoLuongTon,
                GiaBan = sp.GiaBan,
                TrangThai = sp.TrangThai,
                NgayDang = sp.NgayDang,
                HinhAnhDaiDien = sp.HinhAnhDaiDien,
                MaLoaiSanPham = sp.MaLoaiSanPham,
                TenLoaiSanPham = sp.MaLoaiSanPhamNavigation?.TenLoaiSanPham
            });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SanPhamDTO>> GetById(int id)
        {
            var sp = await _sanphamRepository.GetByIdAsync(id);
            if (sp == null)
            {
                return NotFound();
            }
            var result = new SanPhamDTO
            {
                MaSanPham = sp.MaSanPham,
                TenSanPham = sp.TenSanPham,
                MoTa = sp.MoTa,
                SoLuongTon = sp.SoLuongTon,
                GiaBan = sp.GiaBan,
                TrangThai = sp.TrangThai,
                NgayDang = sp.NgayDang,
                HinhAnhDaiDien = sp.HinhAnhDaiDien,
                MaLoaiSanPham = sp.MaLoaiSanPham,
                TenLoaiSanPham = sp.MaLoaiSanPhamNavigation?.TenLoaiSanPham
            };
            return Ok(result);
        }
        [HttpGet("by-category/{maLoaiSanPham}")]
        public async Task<ActionResult<IEnumerable<SanPhamDTO>>> GetByCategory(int maLoaiSanPham)
        {
            var list = await _sanphamRepository.GetAllByIdLSP(maLoaiSanPham);
            if (list == null || !list.Any())
            {
                return NotFound();
            }
            var result = list.Select(sp => new SanPhamDTO
            {
                MaSanPham = sp.MaSanPham,
                TenSanPham = sp.TenSanPham,
                MoTa = sp.MoTa,
                SoLuongTon = sp.SoLuongTon,
                GiaBan = sp.GiaBan,
                TrangThai = sp.TrangThai,
                NgayDang = sp.NgayDang,
                HinhAnhDaiDien = sp.HinhAnhDaiDien,
                MaLoaiSanPham = sp.MaLoaiSanPham,
                TenLoaiSanPham = sp.MaLoaiSanPhamNavigation?.TenLoaiSanPham
            });
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<SanPhamDTO>> Create(SanPhamDTO sanphamdto)
        {
            var sp = new Sanpham
            {
                TenSanPham = sanphamdto.TenSanPham,
                MoTa = sanphamdto.MoTa,
                SoLuongTon = sanphamdto.SoLuongTon,
                GiaBan = sanphamdto.GiaBan,
                TrangThai = sanphamdto.TrangThai ?? "Còn hàng",
                HinhAnhDaiDien = sanphamdto.HinhAnhDaiDien,
                MaLoaiSanPham = sanphamdto.MaLoaiSanPham,
                NgayDang = sanphamdto.NgayDang ?? DateTime.Now
            };
            var created = await _sanphamRepository.AddAsync(sp);
            sanphamdto.MaSanPham = created.MaSanPham;
            return CreatedAtAction(nameof(GetById), new { id = created.MaSanPham }, sanphamdto);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SanPhamDTO sanPhamDTO)
        {
            if (id != sanPhamDTO.MaSanPham) return BadRequest();

            var existing = await _sanphamRepository.GetByIdAsync(id);

            if (existing == null) return NotFound();

            existing.TenSanPham = sanPhamDTO.TenSanPham;
            existing.MoTa = sanPhamDTO.MoTa;
            existing.SoLuongTon = sanPhamDTO.SoLuongTon;
            existing.GiaBan = sanPhamDTO.GiaBan;
            existing.TrangThai = sanPhamDTO.TrangThai;
            existing.HinhAnhDaiDien = sanPhamDTO.HinhAnhDaiDien;
            existing.MaLoaiSanPham = sanPhamDTO.MaLoaiSanPham;

            await _sanphamRepository.UpdateAsync(existing);

            return NoContent();

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _sanphamRepository.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
