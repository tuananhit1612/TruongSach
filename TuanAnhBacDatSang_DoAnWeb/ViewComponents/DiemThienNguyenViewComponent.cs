using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TuanAnhBacDatSang_DoAnWeb.ViewComponents
{
    public class DiemThienNguyenViewComponent : ViewComponent
    {
        Uri baseAddress = new Uri("http://localhost:5269/api");
        private readonly HttpClient _httpClient;

        public DiemThienNguyenViewComponent()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            decimal diem = 0;
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return View(diem);
            }

            HttpResponseMessage respon = await _httpClient.GetAsync($"/api/Auth/diemThieNguyen/?userId={userId}");
            if (respon.IsSuccessStatusCode)
            {
                diem = await respon.Content.ReadFromJsonAsync<decimal>();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Không thể tải điểm thiện nguyện. Vui lòng thử lại sau.");
                return View(diem);
            }

            return View(diem);
        }
    }

}
