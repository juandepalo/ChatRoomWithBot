using ChatRoom.ChatBot.Domain.Bots;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatRoom.ChatBot.Domain
{
    public class ChatBotBundle
    {
        private List<IBotBase> _bots { get; }

        public ChatBotBundle()
        {
            _bots = new List<IBotBase>();
        }

        public void Register(IBotBase bot)
        {
            _bots.Add(bot);
        }

        public IBotBase GetBotForCommand(string command)
        {
            foreach (var bot in _bots)
            {
                if (bot.VerifyCommandName(command))
                {
                    return bot;
                }
            }

            return new NotFoundBot();
        }
    }
}
