using Microsoft.AspNetCore.Mvc;
using TuanAnhBacDatSang_DoAnWeb.ViewModels;

namespace TuanAnhBacDatSang_DoAnWeb.Controllers
{
    public class DongGopController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5269/api");
        private readonly HttpClient _httpClient;
        public DongGopController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }
        public async Task<IActionResult> Index()
        {
            List<ChienDichViewModel> allCampaigns = new();
            HttpResponseMessage response = await _httpClient.GetAsync("/api/chienDich/GetAll");
            if (response.IsSuccessStatusCode)
            {
                allCampaigns = await response.Content.ReadFromJsonAsync<List<ChienDichViewModel>>();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Không thể tải danh sách chiến dịch. Vui lòng thử lại sau.");
                return View(new List<ChienDichViewModel>());
            }
            allCampaigns = allCampaigns
                   .Where(x => x.TrangThai.Equals("Đang diễn ra", StringComparison.OrdinalIgnoreCase))
                   .ToList();
            return View(allCampaigns);
        }
        //[HttpPost]
        //public async Task<IActionResult> Donate(IFormCollection form)
        //{
        //    var donorName = form["donor_name"];
        //    var email = form["email"];
        //    var phone = form["phone"];
        //    var message = form["message"];
        //    var isAnonymous = form["anonymous"] == "1";
        //    var selectedCampaignId = form["SelectedChienDichId"];
        //    var paymentMethod = form["payment_method"];
        //    var amount = string.IsNullOrEmpty(form["custom_amount"]) ? form["amount"] : form["custom_amount"];



        //}
    }
}
