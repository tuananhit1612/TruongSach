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
    public class SanPhamController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5269/api");
        private readonly HttpClient _httpClient;

        public SanPhamController()
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
            var response = await _httpClient.GetAsync("/api/SanPham");
            if (!response.IsSuccessStatusCode) return View(new List<SanPhamViewModel>());

            var data = await response.Content.ReadFromJsonAsync<List<SanPhamViewModel>>();
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
            var response = await _httpClient.GetAsync("/api/LoaiSanPham");
            if (!response.IsSuccessStatusCode) return View(new LoaiSanPhamViewModel());

            var loaisp = await response.Content.ReadFromJsonAsync<List<LoaiSanPhamViewModel>>();
            ViewBag.LoaiSanPham = new SelectList(loaisp, "MaLoaiSanPham", "TenLoaiSanPham");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(SanPhamViewModel model, IFormFile HinhAnhDaiDien)
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
            model.TrangThai = "Còn hàng";
            model.HinhAnhUpload = null;
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/SanPham", content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {errorContent}");
            ModelState.AddModelError("", $"Thêm sản phẩm thất bại: {errorContent}");
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
            var response = await _httpClient.GetAsync($"/api/SanPham/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var model = await response.Content.ReadFromJsonAsync<SanPhamViewModel>();

            var rs = await _httpClient.GetAsync("/api/LoaiSanPham");
            if (!rs.IsSuccessStatusCode) return View(new LoaiSanPhamViewModel());

            var loaisp = await rs.Content.ReadFromJsonAsync<List<LoaiSanPhamViewModel>>();
            ViewBag.LoaiSanPham = new SelectList(loaisp, "MaLoaiSanPham", "TenLoaiSanPham");

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, SanPhamViewModel model, IFormFile HinhAnhUpload)
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

            var response = await _httpClient.PutAsJsonAsync($"/api/SanPham/{id}", model);
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
            var response = await _httpClient.GetAsync($"/api/SanPham/{id}");
            if (!response.IsSuccessStatusCode) return View(new SanPhamViewModel());

            var data = await response.Content.ReadFromJsonAsync<SanPhamViewModel>();
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
            var response = await _httpClient.DeleteAsync($"/api/SanPham/{id}");
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
            var savePath = Path.Combine("wwwroot/images/sanpham", image.FileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/sanpham/" + image.FileName;
        }
    }
}
