using ChatRoom.Application.ViewModels;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatRoom.Application.Commands
{
    public class PostMessageCommand : IRequest<ChatMessageViewModel>
    {
        public string Message { get; set; }
    }
    public class PostMessageCommandValidator : AbstractValidator<PostMessageCommand>
    {
        public PostMessageCommandValidator()
        {
            RuleFor(p => p.Message)
                .NotEmpty();
        }
    }
}
