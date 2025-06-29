using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using TuanAnhBacDatSang_DoAnWeb.Models;
using TuanAnhBacDatSang_DoAnWeb.Models.Vnpay;
using TuanAnhBacDatSang_DoAnWeb.Services.Momo;
using TuanAnhBacDatSang_DoAnWeb.Services.VnPay;
using TuanAnhBacDatSang_DoAnWeb.ViewModels;

namespace TuanAnhBacDatSang_DoAnWeb.Controllers
{
    public class ThanhToanController : Controller
    {
        private IMomoService _momoService;
        private readonly IVnPayService _vnPayService;

        Uri baseAddress = new Uri("http://localhost:5269/api");
        private readonly HttpClient _httpClient;

        public ThanhToanController(IMomoService momoService, IVnPayService vnPayService)
        {
            _momoService = momoService;
            _vnPayService = vnPayService;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreatePaymentUrlMomo(IFormCollection form)
        {
            var amount = string.IsNullOrEmpty(form["custom_amount"]) ? form["amount"].ToString() : form["custom_amount"].ToString();

            var model = new OrderInfo
            {
                FullName = form["FullName"],
                //Email = form["Email"],
                //Phone = form["Phone"],
                OrderInfomation = form["OrderInfomation"],
                Amount = int.TryParse(amount, out var result) ? result : 0
            };

            var response = await _momoService.CreatePaymentAsync(model);
            return Redirect(response.PayUrl);
        }


        [HttpGet]
        public IActionResult PaymentCallBack()
        {
            var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
            return View(response);
        }

        public async Task<IActionResult> CreatePayment(IFormCollection form)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var donationType = form["donation_type"].ToString(); // "money" or "points"
            var amount = string.IsNullOrEmpty(form["custom_amount"]) ? form["amount"].ToString() : form["custom_amount"].ToString();
            var userId = int.TryParse(form["userId"], out var user) ? user : 0;
            var campaignId = int.TryParse(form["campaignId"], out var campaign) ? campaign : 0;
            var message = form["NoiDung"].ToString();
            var isAnonymous = form["anonymous"] == "1";

            if (donationType != "points")
            {
                var model = new PaymentInformationModel
                {
                    MaChienDich = campaignId,
                    SoTien = int.TryParse(amount, out var money) ? money : 0,
                    MaNguoiDung = userId,
                    NoiDung = message,
                    LoaiGiaoDich = "Donation"
                };

                var url = _vnPayService.CreatePaymentUrl(model, HttpContext);
                HttpContext.Session.SetString("VNPAY_PAYMENT_INFO", JsonSerializer.Serialize(model));

                if (string.IsNullOrEmpty(url))
                {
                    TempData["Error"] = "Không thể tạo liên kết thanh toán.";
                    return View("Error");
                }

                return Redirect(url);
            }
            else
            {
                var pointAmount = int.TryParse(form["point_amount"], out var points) ? points : 0;

                if (pointAmount < 10000)
                {
                    TempData["Error"] = "Số điểm tối thiểu là 10.000.";
                    return View("Error");
                }

                var model = new DongGopViewModel
                {
                    SoTien = pointAmount,
                    HinhThuc = "DIEM",
                    NoiDung = message,
                    TrangThai = "Đã thanh toán",
                    NgayDongGop = DateTime.Now,
                    MaNguoiDung = userId,
                    MaChienDich = campaignId
                };

                var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                var rs = await _httpClient.PostAsync("api/donggop", content);

                if (rs.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Đã ghi nhận đóng góp điểm thành công!";
                }
                else
                {
                    TempData["Error"] = "Có lỗi xảy ra khi gửi dữ liệu đến API.";
                }

                return View("ThanhToanDiem", model);
            }
        }

        public IActionResult CreatePaymentUrlVnpayForOrder(PaymentInformationModel model)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);
            HttpContext.Session.SetString("VNPAY_PAYMENT_INFO", JsonSerializer.Serialize(model));

            if (string.IsNullOrEmpty(url))
            {
                return BadRequest("Failed to create payment URL.");
            }

            return Redirect(url);
        }
        [HttpGet]
        public IActionResult PaymentCallbackVnpay()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            return Json(response);
        }


    }
}
