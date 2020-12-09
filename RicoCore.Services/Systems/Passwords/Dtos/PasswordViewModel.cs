using System;
using System.Collections.Generic;
using System.Text;

namespace RicoCore.Services.Systems.Passwords.Dtos
{
    public class PasswordViewModel
    {
        public int Id { set; get; }
        public string Content { set; get; }
        public int Level { set; get; }
        public int Order { set; get; }
    }
}
