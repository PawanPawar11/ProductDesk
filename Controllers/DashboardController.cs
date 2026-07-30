using Microsoft.AspNetCore.Mvc;

namespace ProductDesk.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
