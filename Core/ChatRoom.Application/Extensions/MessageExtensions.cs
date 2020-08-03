using ChatRoom.ChatBot.Domain;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatRoom.Application.Extensions
{
    public static class MessageExtensions
    {
        public static bool IsBotCommand(this string message)
        {
            return message.StartsWith('/');
        }
        public static void SendToBotBundle(this string message, RabbitMQSettings rabbitMQSettings)
        {
            var factory = new ConnectionFactory()
            {
                HostName = rabbitMQSettings.connection.HostName,
                UserName = rabbitMQSettings.connection.Username,
                Password = rabbitMQSettings.connection.Password
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: rabbitMQSettings.BotBundleQueue.Name,
                    durable: rabbitMQSettings.BotBundleQueue.Durable,
                    exclusive: rabbitMQSettings.BotBundleQueue.Exclusive,
                    autoDelete: rabbitMQSettings.BotBundleQueue.AutoDelete,
                    arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: rabbitMQSettings.BotBundleQueue.Name,
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
