using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using System.Text.Json;
using TuanAnhBacDatSang_DoAnWeb.Models.Momo;
using TuanAnhBacDatSang_DoAnWeb.Services.Momo;
using TuanAnhBacDatSang_DoAnWeb.Services.VnPay;

var builder = WebApplication.CreateBuilder(args);

// Momo payment service
builder.Services.Configure<MomoOptionModel>(builder.Configuration.GetSection("MomoAPI"));
builder.Services.AddScoped<IMomoService, MomoService>();

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IVnPayService, VnPayService>();
builder.Services.AddHttpContextAccessor();
// lỗi gì lạ lắm
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});


// Google Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
    options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;
    options.Scope.Add("profile");
    options.Scope.Add("email");
    options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
    options.SaveTokens = true;
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});



var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller=TrangChu}/{action=Index}/{id?}");
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=TrangChu}/{action=Index}/{id?}");
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TrangChu}/{action=Index}/{id?}");

app.Run();
