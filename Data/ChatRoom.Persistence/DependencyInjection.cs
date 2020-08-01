using ChatRoom.Application.Interfaces;
using ChatRoom.Domain;
using ChatRoom.Persistence.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatRoom.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ChatRoomDbContext>(options =>
                  options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IChatRoomDbContext>(provider => provider.GetService<ChatRoomDbContext>());



            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }

        public static void UpdateDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ChatRoomDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
