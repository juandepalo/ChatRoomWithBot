

namespace ChatRoom.ChatBot.Domain.Bots
{
    public interface IBotBase
    {
        string BotCommandName { get; }
        bool VerifyCommandName(string command);
        BotResponse ExecuteActions(string command);
        string BotName { get; }
    }
}
