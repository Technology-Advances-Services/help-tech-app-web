using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using HelpTechAppWeb.Configurations.Interfaces;
using HelpTechAppWeb.Models;

namespace HelpTechAppWeb.Controllers
{
    [Route("communications/")]
    [Authorize(Roles = "TECNICO,CONSUMIDOR")]
    public class CommunicationsController
        (IBaseRequest baseRequest) : Controller
    {
        private ClaimsPrincipal? _claimsPrincipal;

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

        [Route("chats-members-by-technical")]
        [HttpGet]
        public async Task<IActionResult> ChatsMembersByTechnical()
        {
            var chatsMembers = await baseRequest.GetAsync<ChatMember>
                ("chatsmembers/chats-members-by-technical?technicalId=" +
                GetTechnicalId(), GetToken());

            var consumers = new List<Consumer>();

            Parallel.ForEach(chatsMembers, async j =>
            {
                var consumerId = j.ConsumerId;

                var consumer = await baseRequest
                    .GetSingleAsync<Consumer>
                    ("informations/consumer-by-id/consumerId=" +
                    consumerId, GetToken());

                if (consumer is null)
                    return;

                lock (consumers)
                    consumers.Add(consumer);
            });

            Task<dynamic> queryAsync = new(() =>
            {
                return
                (from cm in chatsMembers
                 join co in consumers
                 on cm.ConsumerId equals co.Id
                 select new
                 {
                     cm.ChatRoomId,
                     co.ProfileUrl,
                     co.Firstname,
                     co.Lastname,
                 });
            });

            return Content(JsonConvert.SerializeObject
                (queryAsync), "application/json");
        }

        #endregion

        #region Cookies

        private string GetToken()
        {
            _claimsPrincipal = HttpContext.User;

            return _claimsPrincipal
                .FindFirst(ClaimTypes.Hash)?
                .Value.ToString() ?? string.Empty;
        }

        private string GetTechnicalId()
        {
            _claimsPrincipal = HttpContext.User;

            return _claimsPrincipal
                .FindFirst(ClaimTypes.Name)?
                .Value.ToString() ?? string.Empty;
        }

        #endregion
    }
}