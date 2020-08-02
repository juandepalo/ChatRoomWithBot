using ChatRoom.Application.Commands;
using ChatRoom.Application.Events;
using ChatRoom.Application.Extensions;
using ChatRoom.Application.Interfaces;
using ChatRoom.Application.ViewModels;
using ChatRoom.ChatBot.Domain;
using ChatRoom.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatRoom.Application.Handlers
{
    public class PostMessageHandler : RequestHandlerBase, IRequestHandler<PostMessageCommand>
    {
        private readonly IRepository<ChatMessage> _chatRepository;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RabbitMQSettings _rabbitMQSettings;

        private readonly IMediator _mediator;
        public PostMessageHandler(IServiceProvider services,
            IRepository<ChatMessage> chatRepository,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IOptions<RabbitMQSettings> settings,
            IMediator mediator) : base(services)
        {
            _chatRepository = chatRepository;
            _rabbitMQSettings = settings.Value;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
        }

        async Task<Unit> IRequestHandler<PostMessageCommand, Unit>.Handle(PostMessageCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            if (request.Message.IsBotCommand())
            {
                request.Message.SendToBotBundle(_rabbitMQSettings);
            }
            else
            {
                var message = await _chatRepository.AddAsync(new ChatMessage { ApplicationUser = user, Message = request.Message, CreationDate = DateTime.Now });

                await _mediator.Publish(new MessagePostedEvent(Mapper.Map<ChatMessageViewModel>(message)));
            }
            return Unit.Value;
        }
    }
}
