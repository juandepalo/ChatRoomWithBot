using System;
using System.ComponentModel.DataAnnotations;

namespace ChatRoom.Domain
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
