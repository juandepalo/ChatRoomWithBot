using ChatRoom.Application.Interfaces;
using ChatRoom.Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace ChatRoom.Persistence
{
    public class ChatRoomDbContext : DbContext, IChatRoomDbContext
    {
        public ChatRoomDbContext(DbContextOptions<ChatRoomDbContext> options) : base(options)
        {
        }

        public DbSet<ChatMessage> ChatMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ChatMessage>();
        }
    }
}
