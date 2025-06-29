using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Text.Json;
using TuanAnhBacDatSang_DoAnWeb.ViewModels;

namespace TuanAnhBacDatSang_DoAnWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ChienDichController : Controller
    {

        Uri baseAddress = new Uri("http://localhost:5269/api");
        private readonly HttpClient _httpClient;
        public ChienDichController()
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

            var response = await _httpClient.GetAsync("/api/chiendich/GetAll");
            if (!response.IsSuccessStatusCode) return View(new List<ChienDichViewModel>());

            var data = await response.Content.ReadFromJsonAsync<List<ChienDichViewModel>>();
            return View(data);
        }
        public async Task<IActionResult> Create()
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
            var response = await _httpClient.GetAsync("/api/Truong/get-all");
            if (!response.IsSuccessStatusCode) return View(new TruongViewModel());

            var schools = await response.Content.ReadFromJsonAsync<List<TruongViewModel>>();
            ViewBag.Schools = new SelectList(schools, "MaTruong", "TenTruong");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ChienDichViewModel model , IFormFile HinhAnhDaiDien)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            if (HttpContext.Session.GetString("UserRole") == "User")
                return View("Accessdenied");

            if (HinhAnhDaiDien != null && HinhAnhDaiDien.Length > 0)
            {
                model.HinhAnhDaiDien = await SaveImage(HinhAnhDaiDien);
            }
            model.TrangThai = "Chưa bắt đầu";
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/chiendich/Create", content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Tạo chiến dịch thất bại");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
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
            var response = await _httpClient.GetAsync($"/api/ChienDich/GetById/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var model = await response.Content.ReadFromJsonAsync<ChienDichViewModel>();

            var rs = await _httpClient.GetAsync("/api/Truong/get-all");
            if (!rs.IsSuccessStatusCode) return View(new TruongViewModel());

            var schools = await rs.Content.ReadFromJsonAsync<List<TruongViewModel>>();
            ViewBag.Schools = new SelectList(schools, "MaTruong", "TenTruong");

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ChienDichViewModel model, IFormFile HinhAnhUpload)
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

            if (HinhAnhUpload != null && HinhAnhUpload.Length > 0)
            {
                model.HinhAnhDaiDien = await SaveImage(HinhAnhUpload);
            }
            
            
            var response = await _httpClient.PutAsJsonAsync($"/api/chiendich/Update/{id}", model);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {errorContent}");
            ModelState.AddModelError("", "Cập nhật thất bại");
            return RedirectToAction("Edit", model);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
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
            var response = await _httpClient.GetAsync($"/api/chiendich/GetById/{id}");
            if (!response.IsSuccessStatusCode) return View(new ChienDichViewModel());

            var data = await response.Content.ReadFromJsonAsync<ChienDichViewModel>();
            return View(data);
        }
        public async Task<IActionResult> Delete(int id)
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
            var response = await _httpClient.DeleteAsync($"/api/ChienDich/Delete/{id}");
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Xóa chiến dịch thất bại");
                return RedirectToAction("Index");
            }
            TempData["SuccessMessage"] = "Chiến dịch đã được xóa thành công.";
            return RedirectToAction("Index");
        }
        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images/chiendich", image.FileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/chiendich/" + image.FileName;
        }
    }
}
