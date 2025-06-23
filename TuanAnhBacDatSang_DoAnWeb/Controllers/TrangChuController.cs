using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TuanAnhBacDatSang_DoAnWeb.Models;

namespace TuanAnhBacDatSang_DoAnWeb.Controllers
{
    public class TrangChuController : Controller
    {
        private readonly ILogger<TrangChuController> _logger;

        public TrangChuController(ILogger<TrangChuController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            TempData.Keep();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
