using ChatRoom.ChatBot.Domain;
using ChatRoom.ChatBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ChatRoom.ChatBot
{
    class Program
    {
        private static IServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            RegisterServices();

            _serviceProvider.GetService<ChatCommandReceiver>()
                            .Register();

            Console.WriteLine(" ChatBot Microservice");
            Console.WriteLine(" Press ctrl+c to exit.");
            while (true)
            {
                Console.ReadLine();
            }
        }

        private static void RegisterServices()
        {
            var collection = new ServiceCollection();
            collection.AddSingleton<ChatCommandReceiver>();
            collection.AddTransient<BotResponseSender>();
            collection.AddSingleton<BotService>();

            var configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();

            collection.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQ"));

            collection.Configure<ChatBotSettings>(configuration.GetSection("ChatBots"));

            _serviceProvider = collection.BuildServiceProvider();
        }
    }
}
