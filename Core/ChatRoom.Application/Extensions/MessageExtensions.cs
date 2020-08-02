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
                    queue: rabbitMQSettings.BotBrokerQueue.Name,
                    durable: rabbitMQSettings.BotBrokerQueue.Durable,
                    exclusive: rabbitMQSettings.BotBrokerQueue.Exclusive,
                    autoDelete: rabbitMQSettings.BotBrokerQueue.AutoDelete,
                    arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: rabbitMQSettings.BotBrokerQueue.Name,
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }
        }
    }
}
