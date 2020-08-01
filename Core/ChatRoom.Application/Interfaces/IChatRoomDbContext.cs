using ChatRoom.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatRoom.Application.Interfaces
{
    public interface IChatRoomDbContext
    {
        DbSet<ChatMessage> ChatMessages { get; set; }
    }
}
