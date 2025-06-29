using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using TruongSach_API.Models;
using TuanAnhBacDatSang_DoAnWeb.Models;
using TuanAnhBacDatSang_DoAnWeb.Models.Vnpay;
using TuanAnhBacDatSang_DoAnWeb.Services.VnPay;
using TuanAnhBacDatSang_DoAnWeb.ViewModels;

namespace TuanAnhBacDatSang_DoAnWeb.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IVnPayService _vnPayService;
        Uri baseAddress = new Uri("http://localhost:5269/api");
        private readonly HttpClient _httpClient;
        public CheckoutController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> PaymentCallbackVnpay()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            if (response.Success)
            {
                var json = HttpContext.Session.GetString("VNPAY_PAYMENT_INFO");
                if (!string.IsNullOrEmpty(json))
                {
                    var info = JsonSerializer.Deserialize<PaymentInformationModel>(json);

                    if (info.LoaiGiaoDich == "Donation")
                    {
                        var model = new DongGop
                        {
                            SoTien = info.SoTien,
                            HinhThuc = "VNPAY",
                            NoiDung = info.NoiDung,
                            TrangThai = "Đã thanh toán",
                            NgayDongGop = DateTime.Now,
                            MaNguoiDung = info.MaNguoiDung,
                            MaChienDich = info.MaChienDich ?? 0
                        };
                        var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                        var rs = await _httpClient.PostAsync("api/donggop", content);


                        if (rs.IsSuccessStatusCode)
                        {
                            TempData["Success"] = "Đã ghi nhận đóng góp thành công!";
                            return View("VnpayResult", new PaymentResponseModel
                            {
                                Amount = info.SoTien,
                                TransactionId = response.TransactionId
                            });
                        }
                        else
                        {
                            TempData["Error"] = "Lỗi khi gọi API tạo đóng góp.";
                        }
                    }
                    else
                    {
                        HoaDonViewModel hd = new HoaDonViewModel();
                        HttpResponseMessage rp = await _httpClient.GetAsync($"/api/HoaDon/{info.MaDonHang}");

                        if (rp.IsSuccessStatusCode)
                        {
                            hd = await rp.Content.ReadFromJsonAsync<HoaDonViewModel>();
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Không thể lấy hóa đơn. Vui lòng thử lại sau.");
                            return Json("Lỗi không thể lấy hóa đơn");
                        }

                        hd.TrangThai = "Đã thanh toán";

                        
                        HttpResponseMessage updateResponse = await _httpClient.PutAsJsonAsync($"/api/HoaDon/{hd.MaHoaDon}", hd);


                        if (updateResponse.IsSuccessStatusCode)
                        {
                            return View("OrderCompleted",hd);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Không thể cập nhật hóa đơn.");
                            return Json(new { success = false, message = "Cập nhật hóa đơn thất bại." });
                        }
                    }                 
                }
            }
            
            return RedirectToAction("Index", "DongGop");
        }
    }
}
