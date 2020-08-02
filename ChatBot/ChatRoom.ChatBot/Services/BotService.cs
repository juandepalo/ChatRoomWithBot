using ChatRoom.ChatBot.Bots;
using ChatRoom.ChatBot.Domain;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatRoom.ChatBot.Services
{
    public class BotService
    {
        private readonly ChatBotBundle _botBundle;
        private readonly BotResponseSender _botResponseSender;
        private readonly ChatBotSettings _chatBotSettings;

        public BotService(BotResponseSender botResponseSender, IOptions<ChatBotSettings> settings)
        {
            _chatBotSettings = settings.Value;

            _botBundle = new ChatBotBundle();
            RegisterBotsToBundle();

            _botResponseSender = botResponseSender;
        }

        public void HandleCommand(string command)
        {
            var bot = _botBundle.GetBotForCommand(command);
            var botMessage = bot.ExecuteActions(command);

            _botResponseSender.SendToSignalR(botMessage);
        }

        private void RegisterBotsToBundle()
        {
            _botBundle.Register(new StockBot(_chatBotSettings.StockBotURL, _chatBotSettings.StockMsg, _chatBotSettings.StockNotFoundMsg));
        }
    }
}
