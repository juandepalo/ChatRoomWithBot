using ChatRoom.Application.Interfaces;
using ChatRoom.Domain;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;

namespace ChatRoom.Persistence
{
    public class ChatRoomDbContext : ApiAuthorizationDbContext<ApplicationUser>, IChatRoomDbContext
    {
        public ChatRoomDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
        //public ChatRoomDbContext(DbContextOptions<ChatRoomDbContext> options) : base(options)
        //{
        //}

        public DbSet<ChatMessage> ChatMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ChatMessage>();
        }
    }
}
