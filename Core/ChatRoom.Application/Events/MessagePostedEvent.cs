using ChatRoom.Application.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatRoom.Application.Events
{
   public class MessagePostedEvent : INotification
    {
        public string NickName { get; set; }
        public string Message { get; set; }
        public DateTime CreationDate { get; set; }
        public MessagePostedEvent(ChatMessageViewModel message)
        {
            NickName = message.NickName;
            Message = message.Message;
            CreationDate = message.CreationDate;
        }
    }
}
