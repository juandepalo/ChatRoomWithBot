using System;
using System.ComponentModel.DataAnnotations;

namespace ChatRoom.Domain
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string Message { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
