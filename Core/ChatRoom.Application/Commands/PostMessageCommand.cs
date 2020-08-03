using ChatRoom.Application.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatRoom.Application.Commands
{
    public class PostMessageCommand : IRequest<ChatMessageViewModel>
    {
        public string Message { get; set; }
    }
}
