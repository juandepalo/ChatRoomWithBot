using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ChatRoom.Application.Common;
using ChatRoom.Application.Events;
using ChatRoom.Application.Extensions;
using ChatRoom.Application.Interfaces;
using ChatRoom.Application.ViewModels;
using ChatRoom.ChatBot.Domain;
using ChatRoom.Domain;
using ChatRoom.Application.Commands;
using Microsoft.Extensions.Logging;

namespace ChatRoom.Application.Handlers
{
    public class PostMessageHandler : RequestHandlerBase, IRequestHandler<PostMessageCommand, ChatMessageViewModel>
    {
        private readonly IRepository<ChatMessage> _chatRepository;

        private readonly ILogger _logger;
        private readonly IUserAccessor _httpContextAccessor;
        private readonly RabbitMQSettings _rabbitMQSettings;

        private readonly IMediator _mediator;
        public PostMessageHandler(IServiceProvider services,
            ILogger<PostMessageHandler> logger,
            IRepository<ChatMessage> chatRepository,
            IUserAccessor httpContextAccessor,
            IOptions<RabbitMQSettings> settings,
            IMediator mediator) : base(services)
        {
            _logger = logger;
            _chatRepository = chatRepository;
            _rabbitMQSettings = settings.Value;
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
        }

        async Task<ChatMessageViewModel> IRequestHandler<PostMessageCommand, ChatMessageViewModel>.Handle(PostMessageCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.User.FindFirstValue(ClaimTypes.NameIdentifier);

            ChatMessageViewModel message;
            if (request.Message.IsBotCommand())
            {
                request.Message.SendToBotBundle(_rabbitMQSettings);
                _logger.LogInformation(" [x] Sent to RabbitMQ: {0}", request.Message);

                message = new ChatMessageViewModel()
                {
                    Message = request.Message,
                    CreationDate = DateTime.Now
                };
            }
            else
            {
                message = Mapper.Map<ChatMessageViewModel>(await _chatRepository.AddAsync(new ChatMessage { ApplicationUserId = userId, Message = request.Message, CreationDate = DateTime.Now }));

                await _mediator.Publish(new MessagePostedEvent(message));
            }
            return message;
        }
    }
}
