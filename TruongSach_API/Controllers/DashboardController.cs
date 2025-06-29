using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruongSach_API.DTOs;
using TruongSach_API.Models;

namespace TruongSach_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class DashboardController : ControllerBase
    {
        private readonly TruongSachContext _context;
        public DashboardController(TruongSachContext context)
        {
            _context = context;
        }
        [HttpGet("admin")]
        public async Task<ActionResult<DashboardAdminDTO>> GetAdminDashboard()
        {
            var dto = new DashboardAdminDTO
            {
                TotalCampaigns = await _context.Chiendiches.CountAsync(),
                ActiveCampaigns = await _context.Chiendiches.CountAsync(c => c.TrangThai == "Đang diễn ra"),
                TotalDonations = await _context.Donggops.SumAsync(d => (decimal?)d.SoTien) ?? 0,
                DonationCount = await _context.Donggops.CountAsync(),
                TotalRevenue = await _context.Hoadons.SumAsync(h => (decimal?)h.TongTien) ?? 0,
                OrderCount = await _context.Hoadons.CountAsync(),
                TotalProducts = await _context.Sanphams.CountAsync(),
                ActiveProducts = await _context.Sanphams.CountAsync(p => p.TrangThai == "Còn Hàng")
            };

            return Ok(dto);
        }
        [HttpGet("donation-stats")]
        public IActionResult GetDonationStats()
        {
            var data = _context.Donggops
                .Where(d => d.NgayDongGop != null)
                .ToList()
                .GroupBy(d => d.NgayDongGop.Value.Date)
                .Select(g => new
                {
                    Date = g.Key.ToString("yyyy-MM-dd"),
                    Total = g.Sum(x => x.SoTien)
                })
                .OrderBy(g => g.Date)
                .ToList();

            return Ok(data);
        }


        [HttpGet("order-revenue-stats")]
        public IActionResult GetOrderRevenueStats()
        {
            var data = _context.Hoadons
                .Where(h => h.NgayLap != null)
                .ToList()
                .GroupBy(h => h.NgayLap.Value.Date)
                .Select(g => new
                {
                    Date = g.Key.ToString("yyyy-MM-dd"),
                    Total = g.Sum(x => x.TongTien ?? 0)
                })
                .OrderBy(g => g.Date)
                .ToList();

            return Ok(data);
        }




    }
}
