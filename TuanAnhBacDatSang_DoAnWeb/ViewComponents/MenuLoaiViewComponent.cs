using Microsoft.AspNetCore.Mvc;
using TuanAnhBacDatSang_DoAnWeb.ViewModels;

namespace TuanAnhBacDatSang_DoAnWeb.ViewComponents
{
    public class MenuLoaiViewComponent : ViewComponent
    {
        Uri baseAddress = new Uri("http://localhost:5269/api");
        private readonly HttpClient _httpClient;

        public MenuLoaiViewComponent()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<LoaiSanPhamViewModel> lsp = new();
            HttpResponseMessage response = await _httpClient.GetAsync("/api/LoaiSanPham");
            if (response.IsSuccessStatusCode)
            {
                lsp = await response.Content.ReadFromJsonAsync<List<LoaiSanPhamViewModel>>();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Không thể tải danh sách loại. Vui lòng thử lại sau.");
                return View(new List<LoaiSanPhamViewModel>());
            }
            return View(lsp);
        }
    }
}
