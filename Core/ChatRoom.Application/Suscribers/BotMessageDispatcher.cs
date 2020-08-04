using ChatRoom.Application.Events;
using ChatRoom.Application.Extensions;
using ChatRoom.ChatBot.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatRoom.Application.Suscribers
{
    public class BotMessageDispatcher : INotificationHandler<BotMessageEvent>
    {
        private readonly RabbitMQSettings _rabbitMQSettings;
        private readonly ILogger _logger;

        public BotMessageDispatcher(ILogger<BotMessageDispatcher> logger,
            IOptions<RabbitMQSettings> settings)
        {
            _logger = logger;
            _rabbitMQSettings = settings.Value;
        }

        Task INotificationHandler<BotMessageEvent>.Handle(BotMessageEvent notification, CancellationToken cancellationToken)
        {
            notification.Message.SendToBotBundle(_rabbitMQSettings);
            _logger.LogInformation(" [x] Sent to RabbitMQ: {0}", notification.Message);
            return Task.FromResult(0);
        }
    }
}
