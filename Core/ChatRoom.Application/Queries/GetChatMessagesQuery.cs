using ChatRoom.Application.ViewModels;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatRoom.Application.Queries
{
    public class GetChatMessagesQuery: IRequest<IEnumerable<ChatMessageViewModel>>
    {
        public int? MostRecentMessagesQty { get; set; }
    }
    public class GetChatMessagesQueryValidator : AbstractValidator<GetChatMessagesQuery>
    {
        public GetChatMessagesQueryValidator()
        {
            RuleFor(p => p.MostRecentMessagesQty)
                .GreaterThan(0);
        }
    }
}
