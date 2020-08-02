using ChatRoom.ChatBot.Domain;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatRoom.ComService
{
    public class BotResponseReceiver
    {
        private ConnectionFactory factory { get; set; }
        private IConnection connection { get; set; }
        private IModel channel { get; set; }

        private readonly IHubContext<ChatRoomHub> _chatRoomHub;
        private readonly RabbitMQSettings _rabbitMQSettings;

        public BotResponseReceiver(IHubContext<ChatRoomHub> stockChatHub, IOptions<RabbitMQSettings> settings)
        {
            _rabbitMQSettings = settings.Value;
            _chatRoomHub = stockChatHub;

            factory = new ConnectionFactory()
            {
                HostName = _rabbitMQSettings.connection.HostName,
                UserName = _rabbitMQSettings.connection.Username,
                Password = _rabbitMQSettings.connection.Password
            };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
        }

        public void Register()
        {
            channel.QueueDeclare(
                queue: _rabbitMQSettings.BotResponseQueue.Name,
                durable: _rabbitMQSettings.BotResponseQueue.Durable,
                exclusive: _rabbitMQSettings.BotResponseQueue.Exclusive,
                autoDelete: _rabbitMQSettings.BotResponseQueue.AutoDelete,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());

                var botResponse = JsonConvert.DeserializeObject<BotResponse>(message);

                _chatRoomHub.Clients.All.SendAsync("Send", botResponse.BotName, botResponse.Message, DateTime.Now);
                Console.WriteLine(" [x] Received {0}", message);
            };
            channel.BasicConsume(queue: _rabbitMQSettings.BotResponseQueue.Name, autoAck: true, consumer: consumer);
        }

        public void Deregister()
        {
            this.connection.Close();
        }
    }
}
