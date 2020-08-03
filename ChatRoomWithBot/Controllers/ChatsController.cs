using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChatRoom.Application.Commands;
using ChatRoom.Application.Queries;
using ChatRoom.Application.ViewModels;
using ChatRoom.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatRoomWithBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : BaseController
    {
        /// <summary>
        /// Method for getting chat messages
        /// </summary>
        /// <param name="qty">Quantity of the most recent record messages</param>
        /// <returns>The most recent messages</returns>
        [HttpGet]
        [Route("getmessages/{qty?}")]
        public async Task<IEnumerable<ChatMessageViewModel>> GetMessages(int? qty) => await Mediator.Send(new GetChatMessagesQuery { MostRecentMessagesQty = qty });

        /// <summary>
        /// Action for sending and store messages
        /// </summary>
        /// <param name="message">Model with message composition</param>
        /// <returns>Returns the posted message</returns>
        [HttpPost]
        [Route("postmessage")]
        public async Task<ChatMessageViewModel> PostMessage([FromBody] PostMessageCommand message) => await Mediator.Send(message);
    }
}
