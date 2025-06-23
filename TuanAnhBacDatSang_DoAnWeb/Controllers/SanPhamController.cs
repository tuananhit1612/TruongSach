using Microsoft.AspNetCore.Mvc;

namespace TuanAnhBacDatSang_DoAnWeb.Controllers
{
    [Route("shop/[action]")]
    public class SanPhamController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}
