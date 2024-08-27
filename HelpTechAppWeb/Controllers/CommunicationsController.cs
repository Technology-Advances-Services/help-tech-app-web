using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using HelpTechAppWeb.Configurations.Interfaces;
using HelpTechAppWeb.Models;

namespace HelpTechAppWeb.Controllers
{
    [Authorize(Roles = "TECNICO,CONSUMIDOR")]
    public class CommunicationsController
        (IBaseRequest baseRequest) : Controller
    {
        private int _chatRoomId = 0;
        private ClaimsPrincipal? _claimsPrincipal;

        #region Views

        [HttpGet]
        public async Task<IActionResult> Messaging
            (int chatRoomId)
        {
            var chatMember = await baseRequest.GetSingleAsync<ChatMember>
                ("chatsmembers/chats-members-by-chat-room?chatRoomId=" +
                chatRoomId, GetToken()) ?? new();

            var consumer = await baseRequest
                .GetSingleAsync<Consumer>
                ("informations/consumer-by-id/consumerId=" +
                chatMember.ConsumerId, GetToken()) ?? new();

            var technical = await baseRequest
                .GetSingleAsync<Consumer>
                ("informations/consumer-by-id/consumerId=" +
                chatMember.ConsumerId, GetToken()) ?? new();

            this._chatRoomId = chatRoomId;

            ViewBag.ChatRoomId = chatRoomId;
            ViewBag.Role = GetRole();
            ViewBag.ProfileUrlTechnical = technical.ProfileUrl;
            ViewBag.ProfileUrlConsumer = consumer.ProfileUrl;

            return View();
        }

        #endregion

        #region Json

        [HttpGet]
        public async Task<IActionResult> ChatsMembersByTechnical()
        {
            var chatsMembers = await baseRequest.GetAsync<ChatMember>
                ("chatsmembers/chats-members-by-technical?technicalId=" +
                GetPersonId(), GetToken());

            var consumers = new List<Consumer>();

            Parallel.ForEach(chatsMembers, async j =>
            {
                var consumerId = j.ConsumerId;

                var consumer = await baseRequest
                    .GetSingleAsync<Consumer>
                    ("informations/consumer-by-id/consumerId=" +
                    consumerId, GetToken()) ?? new();

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

            queryAsync.Start();

            return Content(JsonConvert.SerializeObject
                (await queryAsync), "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> ChatByChatRoom()
        {
            var chat = await baseRequest.GetAsync<Chat>
                ("chats/chat-by-chat-room?chatRoomId=" +
                _chatRoomId, GetToken());

            return Content(JsonConvert.SerializeObject
                (chat), "application/json");
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

        private string GetPersonId()
        {
            _claimsPrincipal = HttpContext.User;

            return _claimsPrincipal
                .FindFirst(ClaimTypes.Name)?
                .Value.ToString() ?? string.Empty;
        }

        private string GetRole()
        {
            _claimsPrincipal = HttpContext.User;

            return _claimsPrincipal
                .FindFirst(ClaimTypes.Role)?
                .Value.ToString() ?? string.Empty;
        }

        #endregion
    }
}