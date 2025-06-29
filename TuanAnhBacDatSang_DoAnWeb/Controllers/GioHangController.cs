using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text;
using System.Text.Json;
using TuanAnhBacDatSang_DoAnWeb.Extensions;
using TuanAnhBacDatSang_DoAnWeb.Models;
using TuanAnhBacDatSang_DoAnWeb.Models.Vnpay;
using TuanAnhBacDatSang_DoAnWeb.ViewModels;

namespace TuanAnhBacDatSang_DoAnWeb.Controllers
{
    public class GioHangController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5269/api");
        private readonly HttpClient _httpClient;

        public GioHangController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<GioHang>("GioHang") ?? new GioHang();
            return View(cart);
        }

        public IActionResult TangSoLuong(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<GioHang>("GioHang") ?? new GioHang();

            var item = cart.Items.FirstOrDefault(i => i.MaSanPham == productId);
            if (item != null)
            {
                item.SoLuong++;
            }
            HttpContext.Session.SetObjectAsJson("GioHang", cart);

            return RedirectToAction("Index");
        }

        public IActionResult XoaSanPham(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<GioHang>("GioHang") ?? new GioHang();

            var item = cart.Items.FirstOrDefault(i => i.MaSanPham == productId);
            if (item != null)
            {
                cart.Items.Remove(item);
            }
            HttpContext.Session.SetObjectAsJson("GioHang", cart);

            return RedirectToAction("Index");
        }
        public IActionResult XoaTatCaSanPham()
        {
            var cart = new GioHang();
            
            HttpContext.Session.SetObjectAsJson("GioHang", cart);

            return RedirectToAction("Index");
        }

        public IActionResult GiamSoLuong(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<GioHang>("GioHang") ?? new GioHang();

            var item = cart.Items.FirstOrDefault(i => i.MaSanPham == productId);
            if (item != null && item.SoLuong > 1)
            {
                item.SoLuong--;
            }
            else if (item != null && item.SoLuong == 1)
            {
                cart.Items.Remove(item);
            }

            HttpContext.Session.SetObjectAsJson("GioHang", cart);

            return RedirectToAction("Index");
        }
        public async Task<ActionResult> ThemSanPham([FromBody] SanPhamTrongGioHang sp, bool giohang = false)
        {

            var cart = HttpContext.Session.GetObjectFromJson<GioHang>("GioHang") ??
            new GioHang();
            cart.AddItem(sp);
            HttpContext.Session.SetObjectAsJson("GioHang", cart);

            if (giohang)
            {
                return Ok(new { redirect = Url.Action("Index", "GioHang") });
            }

            return Ok(new { success = true, count = cart.Items.Count});
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var cart = HttpContext.Session.GetObjectFromJson<GioHang>("GioHang") ?? new GioHang();

            NguoiDungViewModel user = new ();
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/auth/?userId={userId}");

            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadFromJsonAsync<NguoiDungViewModel>();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Không thể tải danh sách chiến dịch. Vui lòng thử lại sau.");
                return View(new CheckoutViewModel { GioHang = cart });
            }

            var viewModel = new CheckoutViewModel
            {
                GioHang = cart,
                HoTen = user?.HoTen,
                Email = user?.Email,
                SoDienThoai = user?.SoDienThoai
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(string shipping_address, string notes, string payment_method)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Auth");

            var gioHang = HttpContext.Session.GetObjectFromJson<GioHang>("GioHang");
            if (gioHang == null || !gioHang.Items.Any())
                return RedirectToAction("Index", "GioHang");

            var hoaDon = new HoaDon
            {
                NgayLap = DateTime.Now,
                TongTien = gioHang.Items.Sum(x => x.ThanhTien),
                PhiVanChuyen = 0,
                ThueVAT = 0,
                HinhThucThanhToan = payment_method,
                TrangThai = payment_method == "cod" ? "Chờ Nhận Hàng" : "Chờ Thanh Toán",
                MaNguoiDung = userId.Value,
                DiaChi = shipping_address,
                ChiTiet = gioHang.Items.Select(x => new ChiTietHoaDon
                {
                    MaSanPham = x.MaSanPham,
                    SoLuong = x.SoLuong
                }).ToList()
            };


            var content = new StringContent(JsonSerializer.Serialize(hoaDon), Encoding.UTF8, "application/json");
            var rs = await _httpClient.PostAsync("api/HoaDon", content);

            if (rs.IsSuccessStatusCode)
            {
                var json = await rs.Content.ReadAsStringAsync();

                var maHoaDon = JsonSerializer.Deserialize<int>(json);


                if (maHoaDon > 0)
                {
                    var SoTien = gioHang.Items.Sum(x => x.ThanhTien);
                    var ChiTiet = hoaDon.ChiTiet.Select(c => new ChiTietHoaDonViewModel
                    {
                        MaSanPham = c.MaSanPham,
                        SoLuong = c.SoLuong,
                        TenSanPham = gioHang.Items.FirstOrDefault(i => i.MaSanPham == c.MaSanPham)?.TenSanPham,
                        DonGia = gioHang.Items.FirstOrDefault(i => i.MaSanPham == c.MaSanPham)?.GiaBan ?? 0
                    }).ToList();

                    HttpContext.Session.Remove("GioHang");
                    if (payment_method == "cod")
                    {
                        return View("~/Views/Checkout/OrderCompleted.cshtml", new HoaDonViewModel
                        {
                            MaHoaDon = maHoaDon,
                            NgayLap = hoaDon.NgayLap,
                            TongTien = SoTien,
                            PhiVanChuyen = hoaDon.PhiVanChuyen,
                            ThueVAT = hoaDon.ThueVAT,
                            HinhThucThanhToan = hoaDon.HinhThucThanhToan,
                            TrangThai = hoaDon.TrangThai,
                            DiaChi = hoaDon.DiaChi,
                            ChiTiet = ChiTiet

                        });
                    }
                    else
                    {
                        return RedirectToAction("CreatePaymentUrlVnpayForOrder", "ThanhToan", new PaymentInformationModel
                        {
                            MaDonHang = maHoaDon,
                            SoTien = (int)SoTien,
                            MaNguoiDung = userId.Value,
                            NoiDung = "Thanh toán hóa đơn mua hàng",
                            LoaiGiaoDich = "Order"
                        });
                    }
                }
            }
            else
            {
                TempData["Error"] = "Lỗi khi gọi API tạo hóa đơn.";
            }
            return RedirectToAction("Checkout","GioHang");
        }

    }
}
