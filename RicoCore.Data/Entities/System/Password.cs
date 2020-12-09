using RicoCore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RicoCore.Data.Entities.System
{
    [Table("Passwords")]
    public class Password : DomainEntity<int>
    {
        public Password()
        {

        }
        public Password(string content, int level, int order)
        {
            Content = content;
            Level = level;
            Order = order;
        }
        public string Content { get; set; }
        public int Level { set; get; }
        public int Order { set; get; }
    }
}
