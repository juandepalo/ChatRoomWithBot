
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using ChatRoom.ChatBot.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace ChatRoom.ComService
{
    public static class DependencyInjection
    {
        private const string AspNetCoreEnvironment = "ASPNETCORE_ENVIRONMENT";
        private static BotResponseReceiver _listener { get; set; }
        public static IServiceCollection AddComService(this IServiceCollection services)
        {

            services.AddSignalR();

            services.AddSingleton<BotResponseReceiver>();

            //var basePath = Directory.GetCurrentDirectory() + string.Format("{0}..{0}ChatRoomWithBot", Path.DirectorySeparatorChar);

            //var configuration = new ConfigurationBuilder()
            //    .SetBasePath(basePath)
            //    .AddJsonFile("appsettings.json")
            //    .AddJsonFile($"appsettings.Local.json", optional: true)
            //    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable(AspNetCoreEnvironment)}.json", optional: true)
            //    .AddEnvironmentVariables()
            //                .Build();

            //services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQ"));

            return services;
        }

        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
        {
            _listener = app.ApplicationServices.GetService<BotResponseReceiver>();

            var lifetime = app.ApplicationServices.GetService<IApplicationLifetime>();

            lifetime.ApplicationStarted.Register(OnStarted);

            lifetime.ApplicationStopping.Register(OnStopping);

            return app;
        }

        private static void OnStarted()
        {
            _listener.Register();
        }

        private static void OnStopping()
        {
            _listener.Deregister();
        }
    }
}
