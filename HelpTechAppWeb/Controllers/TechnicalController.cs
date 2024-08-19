using Microsoft.AspNetCore.Mvc;

namespace HelpTechAppWeb.Controllers
{
    public class TechnicalController : Controller
    {
        public IActionResult InterfaceTechnical()
        {
            return View();
        }
    }
}