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
        private static int _chatRoomId;
        private ClaimsPrincipal? _claimsPrincipal;

        #region Views

        [HttpGet]
        public async Task<IActionResult> Messaging
            (int chatRoomId)
        {
            var chatMember = await baseRequest.GetSingleAsync<dynamic>
                ("chatsmembers/chat-member-by-chat-room?chatRoomId=" +
                chatRoomId, GetToken());

            _chatRoomId = chatRoomId;

            ViewBag.ChatRoomId = chatRoomId;
            ViewBag.Role = GetRole();

            ViewBag.ProfileUrlTechnical = chatMember!.technical.profileUrl;
            ViewBag.FirstnameTechnical = chatMember!.technical.firstname;

            ViewBag.ProfileUrlConsumer = chatMember!.consumer.profileUrl;
            ViewBag.FirstnameConsumer = chatMember!.consumer.firstname;

            return View();
        }

        #endregion

        #region Json

        [HttpGet]
        public async Task<IActionResult> ChatsMembersByTechnical()
        {
            var chatsMembers = await baseRequest.GetAsync<dynamic>
                ("chatsmembers/chats-members-by-technical?technicalId=" +
                GetPersonId(), GetToken());

            var result =
                from cm in chatsMembers
                select new
                {
                    cm.chatRoomId,
                    cm.consumer.profileUrl,
                    cm.consumer.firstname,
                    cm.consumer.lastname
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

        [HttpPost]
        public async Task<IActionResult> SendMessage
            (Chat chat)
        {
            var result = await baseRequest.PostAsync
                ("chats/send-message", GetToken(),
                new Chat(chat.ChatRoomId, GetPersonId(),
                DateTime.Now, chat.Sender, chat.Message));

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> GetChatRoomId(string personId)
        {
            dynamic? chats;
            dynamic chatRoomId = 0;

            if (GetRole() == "TECNICO")
            {
                chats = await baseRequest.GetSingleAsync<dynamic>
                    ("chatsmembers/chats-members-by-technical?technicalId=" + GetPersonId(),
                    GetToken());

                foreach (var item in chats!)
                    if (item.consumerId == personId)
                        chatRoomId = item.chatRoomId;
            }
            else if (GetRole() == "CONSUMIDOR")
            {
                chats = await baseRequest.GetSingleAsync<dynamic>
                    ("chatsmembers/chats-members-by-consumer?consumerId=" + GetPersonId(),
                    GetToken());

                foreach (var item in chats!)
                    if (item.technicalId == personId)
                        chatRoomId = item.chatRoomId;
            }

            return Content(JsonConvert.SerializeObject
                (chatRoomId), "application/json");
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