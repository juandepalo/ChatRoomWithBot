using System;
using System.Collections.Generic;
using System.Text;

namespace ChatRoom.ChatBot.Domain
{
    public class ChatBotSettings
    {
        public string StockBotURL { get; set; }
        public string StockMsg { get; set; }
        public string StockNotFoundMsg { get; set; }
    }
}
