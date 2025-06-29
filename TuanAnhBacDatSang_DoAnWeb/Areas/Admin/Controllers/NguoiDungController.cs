using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TuanAnhBacDatSang_DoAnWeb.ViewModels;

namespace TuanAnhBacDatSang_DoAnWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class NguoiDungController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly Uri baseAddress = new("http://localhost:5269/api");

        public NguoiDungController()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = baseAddress
            };
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userId == null)
                return RedirectToAction("Login", "Auth");

            if (userRole == "User")
                return View("AccessDenied");

            var response = await _httpClient.GetAsync("/api/NguoiDung");
            if (!response.IsSuccessStatusCode)
                return View(new List<NguoiDungViewModel>());

            var data = await response.Content.ReadFromJsonAsync<List<NguoiDungViewModel>>();
            return View(data);
        }

        //[HttpGet]
        //public IActionResult Create()
        //{

        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(NguoiDungViewModel model, IFormFile avatarFile)
        //{
        //    if (avatarFile != null && avatarFile.Length > 0)
        //    {
        //        model.AvatarUrl = await SaveImage(avatarFile);
        //    }

        //    var response = await _httpClient.PostAsJsonAsync("/api/NguoiDung", model);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    ModelState.AddModelError(string.Empty, "Thêm người dùng thất bại.");
        //    return View(model);
        //}

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"/api/NguoiDung/{id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();


            var data = await response.Content.ReadFromJsonAsync<NguoiDungViewModel>();

            ViewBag.VaiTro = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "10001", Text = "User" },
                new SelectListItem { Value = "10001", Text = "Admin" }
            }, "Value", "Text", data.MaVaiTro);
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NguoiDungViewModel model, IFormFile HinhAnhUpload)
        {
            if (HinhAnhUpload != null && HinhAnhUpload.Length > 0)
            {
                model.AvatarUrl = await SaveImage(HinhAnhUpload);
            }

            var response = await _httpClient.PutAsJsonAsync($"/api/NguoiDung/{id}", model);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {errorContent}");
            ModelState.AddModelError(string.Empty, "Cập nhật thất bại.");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"/api/NguoiDung/{id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var data = await response.Content.ReadFromJsonAsync<NguoiDungViewModel>();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/NguoiDung/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {errorContent}");
            TempData["Error"] = "Xóa người dùng thất bại.";
            return RedirectToAction("Index");
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            var fileName = Path.GetFileName(image.FileName);
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/nguoidung");
            Directory.CreateDirectory(folderPath);
            var savePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return $"/images/nguoidung/{fileName}";
        }
    }
}
