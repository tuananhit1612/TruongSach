using Microsoft.AspNetCore.Mvc;
using TuanAnhBacDatSang_DoAnWeb.Models;
using TuanAnhBacDatSang_DoAnWeb.ViewModels;

namespace TuanAnhBacDatSang_DoAnWeb.Controllers
{
    public class NguoiDungController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5269/api");
        private readonly HttpClient _httpClient;
        public NguoiDungController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            NguoiDungViewModel user = new NguoiDungViewModel();
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/Auth/?userId={userId}");
            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadFromJsonAsync<NguoiDungViewModel>();

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Không thể tải thông tin người dùng. Vui lòng thử lại sau.");
                return View(new NguoiDungViewModel());
            }
            return View(user);
        }
        

    }
}
