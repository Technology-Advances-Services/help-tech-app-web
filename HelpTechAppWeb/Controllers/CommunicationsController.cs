using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HelpTechAppWeb.Configurations.Interfaces;

namespace HelpTechAppWeb.Controllers
{
    [Route("communications/")]
    [Authorize(Roles = "TECNICO,CONSUMIDOR")]
    public class CommunicationsController
        (IBaseRequest baseRequest) : Controller
    {
        #region Views

        [Route("messaging")]
        [HttpGet]
        public IActionResult Messaging
            (int personId, string role)
        {
            ViewBag.ChatRoomId = 0;
            ViewBag.Role = role;
            ViewBag.PersonId = personId;

            return View();
        }

        #endregion

        #region Json



        #endregion
    }
}