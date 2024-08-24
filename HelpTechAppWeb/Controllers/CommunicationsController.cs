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
            (int chatRoomId, string role)
        {
            ViewBag.ChatRoomId = chatRoomId;
            ViewBag.Role = role;

            return View();
        }

        #endregion

        #region Json



        #endregion
    }
}