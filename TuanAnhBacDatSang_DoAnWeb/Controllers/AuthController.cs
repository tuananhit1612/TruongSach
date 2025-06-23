using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using TuanAnhBacDatSang_DoAnWeb.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using TuanAnhBacDatSang_DoAnWeb.DTO;

namespace TuanAnhBacDatSang_DoAnWeb.Controllers
{
    public class AuthController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5269/api");
        private readonly HttpClient _httpClient;
        public AuthController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public IActionResult Register()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            var apiRegister = new
            {
                HoTen = user.full_name,
                Email = user.email,
                MatKhau = user.password,
                SoDienThoai = user.phone
            };

            var content = new StringContent(JsonSerializer.Serialize(apiRegister), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/auth/register", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["ToastMessage"] = "Đăng ký thành công!, vui lòng đăng nhập";
                TempData["ToastType"] = "success";
                return RedirectToAction("Login");
            }
            else
            {
                TempData["ToastMessage"] = await response.Content.ReadAsStringAsync();
                TempData["ToastType"] = "danger";
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            

            var apiLogin = new
            {
                Email = user.email,
                MatKhau = user.password
            };
            var content = new StringContent(JsonSerializer.Serialize(apiLogin), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<LoginApiResult>(json,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                    );
                if(result == null)
                {
                    TempData["ToastMessage"] = "Đăng nhập thất bại, vui lòng thử lại!";
                    TempData["ToastType"] = "danger";
                    return RedirectToAction("Login");
                }

                HttpContext.Session.SetInt32("UserId", result.User.MaNguoiDung);
                HttpContext.Session.SetString("UserName", result.User.HoTen);
                HttpContext.Session.SetString("UserEmail", result.User.Email);
                HttpContext.Session.SetString("UserRole", result.User.TenVaiTro);

                return RedirectToAction("Index", "Home");
            }
            TempData["ToastMessage"] = await response.Content.ReadAsStringAsync();
            TempData["ToastType"] = "danger";
            return View(user);
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public async Task LoginByGoogle()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("GoogleResponse")
                });
        }
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (result.Succeeded)
            {
                var claims = result.Principal.Claims.ToList();

                var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                var googleId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var avatarUrl = claims.FirstOrDefault(c => c.Type == "urn:google:picture")?.Value;

                var apiLogin = new
                {
                    Email = email,
                    FullName = name,
                    GoogleId = googleId,
                    AvatarUrl = avatarUrl ?? "https://png.pngtree.com/png-clipart/20210915/ourmid/pngtree-avatar-icon-abstract-user-login-icon-png-image_3917181.jpg"
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(apiLogin),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync("api/auth/login-google", content);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var rs = JsonSerializer.Deserialize<LoginApiResult>(json,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                    );
                    if (rs == null)
                    {
                        TempData["ToastMessage"] = "Đăng nhập thất bại, vui lòng thử lại!";
                        TempData["ToastType"] = "danger";
                        return RedirectToAction("Login");
                    }
                    HttpContext.Session.SetInt32("UserId", rs.User.MaNguoiDung);
                    HttpContext.Session.SetString("UserName", rs.User.HoTen ?? "");
                    HttpContext.Session.SetString("UserEmail", rs.User.Email ?? "");
                    HttpContext.Session.SetString("UserAvatar", rs.User.AvatarUrl ?? "");
                    HttpContext.Session.SetString("UserRole", rs.User.TenVaiTro ?? "");
                    return RedirectToAction("Index", "Home");
                }

                TempData["ToastMessage"] = await response.Content.ReadAsStringAsync();
                TempData["ToastType"] = "danger";
            }

            return RedirectToAction("Login");
        }

    }
}
