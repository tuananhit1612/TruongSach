using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruongSach_API.DTOs;
using TruongSach_API.Models;

namespace TruongSach_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TruongController : ControllerBase
    {
        private readonly TruongSachContext _context;

        public TruongController(TruongSachContext context)
        {
            _context = context;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<TruongDTO>>> GetAll()
        {
            var schools = await _context.Truongs
                .Select(t => new TruongDTO
                {
                    MaTruong = t.MaTruong,
                    TenTruong = t.TenTruong
                })
                .ToListAsync();
            return Ok(schools);
        }
    }
}
