using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TruongSach_API.DTOs;
using TruongSach_API.Models;
using TruongSach_API.Repositories;

namespace TruongSach_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiSanPhamController : ControllerBase
    {
        private readonly ILoaisanphamRepository _loaisanphamRepository;

        public LoaiSanPhamController(ILoaisanphamRepository loaisanphamRepository)
        {
            _loaisanphamRepository = loaisanphamRepository;
        }

        [HttpGet]
         public async Task<ActionResult<IEnumerable<LoaiSanPhamDTO>>> GetAll()
        {
            var list = await _loaisanphamRepository.GetAllAsync();

            var result = list.Select(x => new LoaiSanPhamDTO
            {
                MaLoaiSanPham = x.MaLoaiSanPham,
                TenLoaiSanPham = x.TenLoaiSanPham,
                TongSoLuong = x.TongSoLuong
            });

            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<LoaiSanPhamDTO>> GetById(int id)
        {
            var lsp = await _loaisanphamRepository.GetByIdAsync(id);
            if (lsp == null) return NotFound();

            return Ok(new LoaiSanPhamDTO
            {
                MaLoaiSanPham = lsp.MaLoaiSanPham,
                TenLoaiSanPham = lsp.TenLoaiSanPham,
                TongSoLuong = lsp.TongSoLuong
            });
        }

        [HttpPost]
        public async Task<ActionResult<LoaiSanPhamDTO>> Create(LoaiSanPhamDTO loaiSanPhamdto)
        {
            var lsp = new Loaisanpham
            {
                TenLoaiSanPham = loaiSanPhamdto.TenLoaiSanPham,
                TongSoLuong = loaiSanPhamdto.TongSoLuong
            };
            var created = await _loaisanphamRepository.AddAsync(lsp);
            loaiSanPhamdto.MaLoaiSanPham = created.MaLoaiSanPham;
            return CreatedAtAction(nameof(GetById), new { id = created.MaLoaiSanPham }, loaiSanPhamdto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, LoaiSanPhamDTO loaiSanPhamDTO)
        {
            if (id != loaiSanPhamDTO.MaLoaiSanPham) return BadRequest();

            var existing = await _loaisanphamRepository.GetByIdAsync(id);

            if (existing == null) return NotFound();

            existing.TenLoaiSanPham = loaiSanPhamDTO.TenLoaiSanPham;
            existing.TongSoLuong = loaiSanPhamDTO.TongSoLuong;

            await _loaisanphamRepository.UpdateAsync(existing);

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _loaisanphamRepository.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
