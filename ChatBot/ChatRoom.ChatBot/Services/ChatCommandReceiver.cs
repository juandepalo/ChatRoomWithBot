using ChatRoom.ChatBot.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatRoom.ChatBot.Services
{
    public class ChatCommandReceiver
    {
        private ConnectionFactory factory { get; set; }
        private IConnection connection { get; set; }
        private IModel channel { get; set; }

        private readonly ILogger _logger;
        private readonly BotService _botService;
        private readonly RabbitMQSettings _rabbitMQSettings;

        public ChatCommandReceiver(ILogger<ChatCommandReceiver> logger, BotService botService, IOptions<RabbitMQSettings> settings)
        {
            _logger = logger;
            _rabbitMQSettings = settings.Value;

            factory = new ConnectionFactory()
            {
                HostName = _rabbitMQSettings.connection.HostName,
                UserName = _rabbitMQSettings.connection.Username,
                Password = _rabbitMQSettings.connection.Password
            };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            _botService = botService;
        }

        public void Register()
        {
            channel.QueueDeclare(
                queue: _rabbitMQSettings.BotBundleQueue.Name,
                durable: _rabbitMQSettings.BotBundleQueue.Durable,
                exclusive: _rabbitMQSettings.BotBundleQueue.Exclusive,
                autoDelete: _rabbitMQSettings.BotBundleQueue.AutoDelete,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());

                _logger.LogInformation(" [x] Received command {0}", message);

                _botService.HandleCommand(message);
            };
            channel.BasicConsume(queue: _rabbitMQSettings.BotBundleQueue.Name,
                                 autoAck: true,
                                 consumer: consumer);
        }

        public void Deregister()
        {
            connection.Close();
        }
    }
}
