using AutoMapper;
using ChatRoom.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatRoom.Application.Common
{
    public class RequestHandlerBase
    {
        protected IChatRoomDbContext ChatRoomContext;
        protected readonly IMapper Mapper;

        public RequestHandlerBase(IServiceProvider services)
        {

            var configuration = services.GetService<IConfiguration>();
            ChatRoomContext = services.GetService<IChatRoomDbContext>();


            Mapper = services.GetService<IMapper>();
        }
    }
}
