using Microsoft.AspNetCore.Mvc;
using TuanAnhBacDatSang_DoAnWeb.ViewModels;

namespace TuanAnhBacDatSang_DoAnWeb.Controllers
{
    public class DonHangController : Controller
    {

        Uri baseAddress = new Uri("http://localhost:5269/api");
        private readonly HttpClient _httpClient;
        public DonHangController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var userId = HttpContext.Session.GetInt32("UserId");

            List<HoaDonViewModel> hoaDon = new List<HoaDonViewModel>();
            HttpResponseMessage response =  await _httpClient.GetAsync($"/api/HoaDon/user/{userId}");
            if (response.IsSuccessStatusCode)
            {
                hoaDon = await response.Content.ReadFromJsonAsync<List<HoaDonViewModel>>();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Không thể tải thông tin hóa đơn. Vui lòng thử lại sau.");
                return View(new HoaDonViewModel());
            }
            return View(hoaDon);
        }
        public async Task<IActionResult> Details(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            HoaDonViewModel hoaDon = new HoaDonViewModel();
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/HoaDon/{id}");
            if (response.IsSuccessStatusCode)
            {
                hoaDon = await response.Content.ReadFromJsonAsync<HoaDonViewModel>();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Không thể tải thông tin hóa đơn. Vui lòng thử lại sau.");
                return View(new HoaDonViewModel());
            }
            return View(hoaDon);
        }
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            HoaDonViewModel hoaDon = new HoaDonViewModel();
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/HoaDon/{id}");
            if (response.IsSuccessStatusCode)
            {
                hoaDon = await response.Content.ReadFromJsonAsync<HoaDonViewModel>();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Không thể tải thông tin hóa đơn. Vui lòng thử lại sau.");
                return View(new HoaDonViewModel());
            }
            return View(hoaDon);
        }
    }
}
