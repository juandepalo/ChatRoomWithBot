using AutoMapper.QueryableExtensions;
using ChatRoom.Application.Interfaces;
using ChatRoom.Application.ViewModels;
using ChatRoom.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using ChatRoom.Application.Queries;
using System.Runtime.InteropServices.WindowsRuntime;
using ChatRoom.Application.Common;

namespace ChatRoom.Application.Handlers
{
    public class GetChatMessagesHandler : RequestHandlerBase, IRequestHandler<GetChatMessagesQuery, IEnumerable<ChatMessageViewModel>>
    {
        private readonly IRepository<ChatMessage> _chatRepository;

        public GetChatMessagesHandler(IServiceProvider services, IRepository<ChatMessage> chatRepository) : base(services)
        {
            _chatRepository = chatRepository;
        }

        async Task<IEnumerable<ChatMessageViewModel>> IRequestHandler<GetChatMessagesQuery, IEnumerable<ChatMessageViewModel>>.Handle(GetChatMessagesQuery request, CancellationToken cancellationToken)
        {
            var messages = await _chatRepository.GetAll()
                .Include(c => c.ApplicationUser)
                .OrderByDescending(c => c.CreationDate)
                .Take(request.MostRecentMessagesQty ?? 50)
                .ProjectTo<ChatMessageViewModel>(Mapper.ConfigurationProvider)
                .ToListAsync();
            messages.Reverse();
            return messages;
        }
    }
}
