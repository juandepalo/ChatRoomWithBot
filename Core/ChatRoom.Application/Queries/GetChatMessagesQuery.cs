using ChatRoom.Application.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatRoom.Application.Queries
{
    public class GetChatMessagesQuery: IRequest<IEnumerable<ChatMessageViewModel>>
    {
        public int? MostRecentMessagesQty { get; set; }
    }
}
