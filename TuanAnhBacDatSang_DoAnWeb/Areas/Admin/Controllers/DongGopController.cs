using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TuanAnhBacDatSang_DoAnWeb.ViewModels;

namespace TuanAnhBacDatSang_DoAnWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DongGopController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5269/api");
        private readonly HttpClient _httpClient;

        public DongGopController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }
        public async Task<IActionResult> Index(string? TenChienDich, string? TenTruong, string? TenNguoiDung, DateTime? Ngay,
                                       string? sort = "ngaydesc", int page = 1, int per_page = 20)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Auth");
            if (HttpContext.Session.GetString("UserRole") == "User") return View("Accessdenied");

            var response = await _httpClient.GetAsync("/api/DongGop/lichsu");
            if (!response.IsSuccessStatusCode)
                return View(new List<NguoiUngHoViewModel>());

            var all = await response.Content.ReadFromJsonAsync<List<NguoiUngHoViewModel>>();

            
            if (!string.IsNullOrEmpty(TenChienDich))
                all = all.Where(x => x.TenChienDich?.Contains(TenChienDich, StringComparison.OrdinalIgnoreCase) == true).ToList();

            if (!string.IsNullOrEmpty(TenTruong))
                all = all.Where(x => x.TenTruong?.Contains(TenTruong, StringComparison.OrdinalIgnoreCase) == true).ToList();

            if (!string.IsNullOrEmpty(TenNguoiDung))
                all = all.Where(x => x.HoTen?.Contains(TenNguoiDung, StringComparison.OrdinalIgnoreCase) == true).ToList();

            if (Ngay.HasValue)
                all = all.Where(x => x.NgayDongGop?.Date == Ngay.Value.Date).ToList();

            // Sắp xếp
            all = sort switch
            {
                "tienasc" => all.OrderBy(x => x.SoTien).ToList(),
                "tiendesc" => all.OrderByDescending(x => x.SoTien).ToList(),
                "ngayasc" => all.OrderBy(x => x.NgayDongGop).ToList(),
                _ => all.OrderByDescending(x => x.NgayDongGop).ToList()
            };

            // Phân trang
            int total = all.Count;
            int totalPages = (int)Math.Ceiling((double)total / per_page);
            var paged = all.Skip((page - 1) * per_page).Take(per_page).ToList();


            ViewBag.TenChienDich = TenChienDich;
            ViewBag.TenTruong = TenTruong;
            ViewBag.TenNguoiDung = TenNguoiDung;
            ViewBag.Ngay = Ngay?.ToString("yyyy-MM-dd");

            ViewBag.Sort = sort;
            ViewBag.Page = page;
            ViewBag.PerPage = per_page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = total;

            return View(paged);
        }


    }
}
