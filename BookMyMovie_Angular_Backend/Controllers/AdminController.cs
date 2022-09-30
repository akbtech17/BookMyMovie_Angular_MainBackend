using Microsoft.AspNetCore.Mvc;

namespace BookMyMovie_Angular_Backend.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
