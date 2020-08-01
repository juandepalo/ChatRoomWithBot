using System;
using System.Collections.Generic;
using System.Text;

namespace ChatRoom.Application.ViewModels
{
    public class ChatMessageViewModel
    {
        public string NickName { get; set; }
        public string Message { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
