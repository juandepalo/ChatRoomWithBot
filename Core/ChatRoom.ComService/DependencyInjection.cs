using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatRoom.ComService
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddComService(this IServiceCollection services)
        {

            services.AddSignalR();

            services.AddSingleton<BotResponseReceiver>();

            return services;
        }
    }
}
