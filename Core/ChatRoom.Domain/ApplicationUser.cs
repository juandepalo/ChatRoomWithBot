using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatRoom.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string NickName { get; set; }
    }
}
