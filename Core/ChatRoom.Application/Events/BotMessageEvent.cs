using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatRoom.Application.Events
{
    public class BotMessageEvent : INotification
    {
        public string Message { get; set; }
        public BotMessageEvent(string message)
        {
            Message = message;
        }
    }
}
