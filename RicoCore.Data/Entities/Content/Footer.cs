using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.Content
{
    [Table("Footers")]
    public class Footer : DomainEntity<string>, ISwitchable
    {
        public Footer()
        {

        }
        public Footer(string content, Status status)
        {
            Content = content;
            Status = status;
        }

        [Required]
        public string Content { set; get; }

        public Status Status { set; get; }
    }
}