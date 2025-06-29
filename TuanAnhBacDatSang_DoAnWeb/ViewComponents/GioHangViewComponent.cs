using Microsoft.AspNetCore.Mvc;
using TuanAnhBacDatSang_DoAnWeb.Extensions;
using TuanAnhBacDatSang_DoAnWeb.Models;

namespace TuanAnhBacDatSang_DoAnWeb.ViewComponents
{
    public class GioHangViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GioHangViewComponent(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IViewComponentResult Invoke()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            var cart = session?.GetObjectFromJson<GioHang>("GioHang") ?? new GioHang();
            return View(cart);
        }
    }
}
