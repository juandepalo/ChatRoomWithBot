using ChatRoom.ChatBot.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatRoom.ChatBot.Bots
{
    public interface IBotBase
    {
        string BotCommandName { get; }
        bool VerifyCommandName(string command);
        BotResponse ExecuteActions(string command);
        string BotName { get; }
    }
}
