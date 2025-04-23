using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MultiShop.WebUI.Settings;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TestController : Controller
    {
        private readonly IOptions<ServiceApiSettings> _serviceApiSettings;

        public TestController(IOptions<ServiceApiSettings> serviceApiSettings)
        {
            _serviceApiSettings = serviceApiSettings;
        }

        public IActionResult Index()
        {
            ViewBag.SignalRUrl = _serviceApiSettings.Value.SignalRUrl;
            return View();
        }
    }
}
