using Microsoft.AspNetCore.Mvc;

namespace HelpTechAppWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}