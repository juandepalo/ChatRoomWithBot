using System;
using System.Collections.Generic;
using System.Text;

namespace ChatRoom.ChatBot.Domain
{
    public class RabbitMQSettings
    {
        public Connection connection { get; set; }
        public Queue BotBundleQueue { get; set; }
        public Queue BotResponseQueue { get; set; }

        public class Connection
        {
            public string HostName { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class Queue
        {
            public string Name { get; set; }
            public bool Durable { get; set; }
            public bool Exclusive { get; set; }
            public bool AutoDelete { get; set; }
        }
    }
}
