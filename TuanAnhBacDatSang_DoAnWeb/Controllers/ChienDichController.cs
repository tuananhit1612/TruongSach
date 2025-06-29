using Microsoft.AspNetCore.Mvc;
using TuanAnhBacDatSang_DoAnWeb.Models;
using TuanAnhBacDatSang_DoAnWeb.ViewModels;

namespace TuanAnhBacDatSang_DoAnWeb.Controllers
{
    public class ChienDichController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5269/api");
        private readonly HttpClient _httpClient;
        public ChienDichController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }
        public async Task<IActionResult> Index(string? search, string? status, int page = 1, int pageSize = 9)
        {
            List<ChienDichViewModel> allCampaigns = new();
            var userId = HttpContext.Session.GetInt32("UserId");
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/chienDich/GetAll?userId={userId}");
            if (response.IsSuccessStatusCode)
            {
                allCampaigns = await response.Content.ReadFromJsonAsync<List<ChienDichViewModel>>();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Không thể tải danh sách chiến dịch. Vui lòng thử lại sau.");
                return View(new List<ChienDichViewModel>());
            }

            // Tìm kiếm
            if (!string.IsNullOrWhiteSpace(search))
            {
                allCampaigns = allCampaigns
                    .Where(x => x.TieuDe.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Lọc theo trạng thái
            if (!string.IsNullOrWhiteSpace(status) && status != "Tất cả")
            {
                allCampaigns = allCampaigns
                    .Where(x => x.TrangThai.Equals(status, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Tổng số trang
            int totalCount = allCampaigns.Count;
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            // Phân trang
            var campaignsToShow = allCampaigns
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.Search = search;
            ViewBag.Status = status;

            return View(campaignsToShow);
        }
        public async Task<IActionResult> Details(int id)
        {
            ChienDichViewModel campaign = new();
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/chienDich/GetById/{id}");
            if (response.IsSuccessStatusCode)
            {
                campaign = await response.Content.ReadFromJsonAsync<ChienDichViewModel>();
            }
            return View(campaign);
        }

    }
}
