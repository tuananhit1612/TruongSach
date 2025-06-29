using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using TuanAnhBacDatSang_DoAnWeb.ViewModels;

namespace TuanAnhBacDatSang_DoAnWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DashboardController : Controller
    {

        Uri baseAddress = new Uri("http://localhost:5269/api");
        private readonly HttpClient _httpClient;
        public DashboardController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            if (HttpContext.Session.GetString("UserRole") == "User")
            {
                return View("Accessdenied");
            }

            DashboardViewModel dashboard = new DashboardViewModel();
            HttpResponseMessage response = await _httpClient.GetAsync("/api/Dashboard/admin");
            if (response.IsSuccessStatusCode)
            {
                dashboard = await response.Content.ReadFromJsonAsync<DashboardViewModel>();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Không thể tải thông tin Dashboard. Vui lòng thử lại sau.");
                return View(new DashboardViewModel());
            }

            return View(dashboard);
        }
    }
}
