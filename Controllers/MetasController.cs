using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Controllers
{
    public class MetasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
