using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Security.Claims;
using HelpTechAppWeb.Configurations.Interfaces;
using HelpTechAppWeb.Models;

namespace HelpTechAppWeb.Controllers
{
    [Authorize(Roles = "TECNICO,CONSUMIDOR")]
    public class CommunicationsController
        (IBaseRequest baseRequest) : Controller
    {
        private static int _chatRoomId;
        private ClaimsPrincipal? _claimsPrincipal;

        #region Views

        [HttpGet]
        public async Task<IActionResult> Messaging
            (int chatRoomId)
        {
            var chatMember = await baseRequest.GetSingleAsync<ChatMember>
                ("chatsmembers/chat-member-by-chat-room?chatRoomId=" +
                chatRoomId, GetToken()) ?? new();

            var technical = await baseRequest
                .GetSingleAsync<Technical>
                ("informations/technical-by-id?id=" +
                chatMember.TechnicalId, GetToken()) ?? new();

            var consumer = await baseRequest
                .GetSingleAsync<Consumer>
                ("informations/consumer-by-id?id=" +
                chatMember.ConsumerId, GetToken()) ?? new();

            _chatRoomId = chatRoomId;

            ViewBag.ChatRoomId = chatRoomId;
            ViewBag.Role = GetRole();

            ViewBag.ProfileUrlTechnical = technical.ProfileUrl;
            ViewBag.FirstnameTechnical = technical.Firstname;

            ViewBag.ProfileUrlConsumer = consumer.ProfileUrl;
            ViewBag.FirstnameConsumer = consumer.Firstname;

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

            var consumers = new ConcurrentBag<Consumer>();

            await Task.WhenAll(chatsMembers.Select(async item =>
            {
                var consumer = await baseRequest.GetSingleAsync<Consumer>
                ("informations/consumer-by-id?id=" + item.ConsumerId,
                GetToken()) ?? new();

                consumers.Add(consumer);
            }));

            var result =
                from cm in chatsMembers
                join co in consumers
                on cm.ConsumerId equals co.Id
                select new
                {
                    cm.ChatRoomId,
                    co.ProfileUrl,
                    co.Firstname,
                    co.Lastname,
                };

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> ChatByChatRoom()
        {
            var chat = await baseRequest.GetAsync<Chat>
                ("chats/chats-by-chat-room?chatRoomId=" +
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