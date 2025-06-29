using Microsoft.AspNetCore.Mvc;
using TuanAnhBacDatSang_DoAnWeb.ViewModels;

namespace TuanAnhBacDatSang_DoAnWeb.Controllers
{
    
    public class SanPhamController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5269/api");
        private readonly HttpClient _httpClient;
        public SanPhamController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }
        public async Task<IActionResult> Index(string? sort = "name", int per_page = 12, int page = 1,
                                       int? category = null, decimal? min_price = null, decimal? max_price = null)
        {
            List<SanPhamViewModel> allSanPham = new();
            HttpResponseMessage response = await _httpClient.GetAsync("/api/SanPham");
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Không thể tải danh sách sản phẩm.");
                return View(new List<SanPhamViewModel>());
            }

            allSanPham = await response.Content.ReadFromJsonAsync<List<SanPhamViewModel>>();

            // Lọc theo danh mục
            if (category.HasValue)
                allSanPham = allSanPham.Where(s => s.MaLoaiSanPham == category).ToList();

            // Lọc theo khoảng giá
            if (min_price.HasValue)
                allSanPham = allSanPham.Where(s => s.GiaBan >= min_price.Value).ToList();

            if (max_price.HasValue && max_price > 0)
                allSanPham = allSanPham.Where(s => s.GiaBan <= max_price.Value).ToList();

            // Sắp xếp
            allSanPham = sort switch
            {
                "price_asc" => allSanPham.OrderBy(s => s.GiaBan).ToList(),
                "price_desc" => allSanPham.OrderByDescending(s => s.GiaBan).ToList(),
                "newest" => allSanPham.OrderByDescending(s => s.NgayDang).ToList(),
                "oldest" => allSanPham.OrderBy(s => s.NgayDang).ToList(),
                _ => allSanPham.OrderBy(s => s.TenSanPham).ToList()
            };

            // Phân trang
            int totalItems = allSanPham.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / per_page);

            var sanPhamPaged = allSanPham
                .Skip((page - 1) * per_page)
                .Take(per_page)
                .ToList();

            // Truyền lại để giữ giá trị trên view
            ViewBag.Sort = sort;
            ViewBag.PerPage = per_page;
            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;
            ViewBag.Category = category;
            ViewBag.MinPrice = min_price;
            ViewBag.MaxPrice = max_price;

            return View(sanPhamPaged);
        }


        public async Task<IActionResult> Details(int id)
        {
            SanPhamViewModel sanPham = new();
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/SanPham/{id}");

            if (response.IsSuccessStatusCode)
            {
                sanPham = await response.Content.ReadFromJsonAsync<SanPhamViewModel>();
            }

            return View(sanPham);
        }
        public async Task<IActionResult> SanPhamLienQuan(int maLoaiSanPham, int excludeId)
        {
            List<SanPhamViewModel> allSanPham = new();
            HttpResponseMessage response = await _httpClient.GetAsync("/api/SanPham");

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Không thể tải danh sách sản phẩm.");
                return View(new List<SanPhamViewModel>());
            }

            var sanPham = await response.Content.ReadFromJsonAsync<List<SanPhamViewModel>>();

            var list = sanPham
                .Where(sp => sp.MaLoaiSanPham == maLoaiSanPham && sp.MaSanPham != excludeId)
                .OrderByDescending(sp => sp.NgayDang)
                .Take(4)
                .Select(sp => new
                {
                    sp.MaSanPham,
                    sp.TenSanPham,
                    sp.GiaBan,
                    sp.HinhAnhDaiDien
                });

            return Ok(list);
        }

    }
}
