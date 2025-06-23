using Microsoft.AspNetCore.Mvc;
using TruongSach_API.Models;
using TruongSach_API.Repositories;
using TruongSach_API.DTO;
using Microsoft.EntityFrameworkCore;

namespace TruongSach_API.Controllers
{
    [Route("api/chiendich/[action]")]
    [ApiController]
    public class ChienDichController : ControllerBase
    {
        private readonly IChiendichRepository _repo;
        private readonly TruongSachContext _context;

        public ChienDichController(IChiendichRepository repo, TruongSachContext context)
        {
            _context = context;
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChienDichDTO>>> GetAll([FromQuery] int? userId = null)
        {
            var campaigns = await _repo.GetAllAsync();

            List<int> dsDaYeuThich = new();

            if (userId.HasValue)
            {
                dsDaYeuThich = await _context.ChienDichYeuThiches
                    .Where(x => x.MaNguoiDung == userId)
                    .Select(x => x.MaChienDich)
                    .ToListAsync();
            }

            var result = campaigns.Select(c => new ChienDichDTO
            {
                MaChienDich = c.MaChienDich,
                TieuDe = c.TieuDe,
                MoTa = c.MoTa,
                SoTienHienTai = c.SoTienHienTai,
                MucTieuQuyenGop = c.MucTieuQuyenGop,
                HinhAnhDaiDien = c.HinhAnhDaiDien,
                TrangThai = c.TrangThai,
                NguoiDaiDien = c.NguoiDaiDien,
                NgayTao = c.NgayTao,
                MaTruong = c.MaTruong,
                TenTruong = c.MaTruongNavigation.TenTruong,
                IsLiked = dsDaYeuThich.Contains(c.MaChienDich)
            }).ToList();

            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ChienDichDTO>> GetById(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null)
                return NotFound();

            var dto = new ChienDichDTO
            {
                MaChienDich = c.MaChienDich,
                TieuDe = c.TieuDe,
                MoTa = c.MoTa,
                SoTienHienTai = c.SoTienHienTai,
                MucTieuQuyenGop = c.MucTieuQuyenGop,
                HinhAnhDaiDien = c.HinhAnhDaiDien,
                TrangThai = c.TrangThai,
                NguoiDaiDien = c.NguoiDaiDien,
                NgayTao = c.NgayTao ?? DateTime.Now,
                MaTruong = c.MaTruong,
                TenTruong = c.MaTruongNavigation.TenTruong,
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<ChienDichDTO>> Create(ChienDichDTO dto)
        {
            var campaign = new Chiendich
            {
                TieuDe = dto.TieuDe,
                MoTa = dto.MoTa,
                SoTienHienTai = dto.SoTienHienTai,
                MucTieuQuyenGop = dto.MucTieuQuyenGop,
                HinhAnhDaiDien = dto.HinhAnhDaiDien,
                TrangThai = dto.TrangThai,
                NguoiDaiDien = dto.NguoiDaiDien,
                NgayTao = dto.NgayTao ?? DateTime.Now,
                MaTruong = dto.MaTruong
            };

            var created = await _repo.AddAsync(campaign);

            dto.MaChienDich = created.MaChienDich;
            return CreatedAtAction(nameof(GetById), new { id = created.MaChienDich }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ChienDichDTO dto)
        {
            if (id != dto.MaChienDich)
                return BadRequest("ID mismatch");

            var campaign = await _repo.GetByIdAsync(id);
            if (campaign == null)
                return NotFound();

            campaign.TieuDe = dto.TieuDe;
            campaign.MoTa = dto.MoTa;
            campaign.SoTienHienTai = dto.SoTienHienTai;
            campaign.MucTieuQuyenGop = dto.MucTieuQuyenGop;
            campaign.HinhAnhDaiDien = dto.HinhAnhDaiDien;
            campaign.TrangThai = dto.TrangThai;
            campaign.NguoiDaiDien = dto.NguoiDaiDien;
            campaign.NgayTao = dto.NgayTao ?? DateTime.Now;
            campaign.MaTruong = dto.MaTruong;

            await _repo.UpdateAsync(campaign);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repo.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> LuuYeuThich(int userId, int campaignId)
        {
            var existed = await _context.ChienDichYeuThiches.FindAsync(userId, campaignId);

            if (existed != null)
            {
                _context.ChienDichYeuThiches.Remove(existed);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, liked = false });
            }

            var yeuThich = new ChienDichYeuThich
            {
                MaNguoiDung = userId,
                MaChienDich = campaignId,
                NgayLuu = DateTime.Now
            };

            _context.ChienDichYeuThiches.Add(yeuThich);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, liked = true });
        }



    }
}
