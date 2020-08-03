using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatRoom.Application.Commands;
using ChatRoom.Application.Queries;
using ChatRoom.Application.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatRoomWithBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : BaseController
    {
        [HttpGet]
        [Route("getmessages/{qty?}")]
        public async Task<IEnumerable<ChatMessageViewModel>> GetMessages(int? qty) => await Mediator.Send(new GetChatMessagesQuery { MostRecentMessagesQty = qty });

        [HttpPost]
        [Route("postmessage")]
        public async Task<ChatMessageViewModel> PostMessage([FromBody] PostMessageCommand message) => await Mediator.Send(message);
    }
}
