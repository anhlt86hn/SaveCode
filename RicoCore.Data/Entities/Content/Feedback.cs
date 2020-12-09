using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.Content
{
    [Table("Feedbacks")]
    public class Feedback : DomainEntity<Guid>, ISwitchable, IDateTracking
    {
        public Feedback()
        {
        }

        public Feedback(Guid id, string name, string email, 
            string message, Status status)
        {
            Id = id;
            Name = name;
            Email = email;
            Message = message;
            Status = status;
        }

        [MaxLength(100)]
        [Required]
        public string Name { set; get; }

        [MaxLength(100)]
        public string Email { set; get; }

        [MaxLength(500)]
        public string Message { set; get; }

        public Status Status { set; get; }
        public DateTime DateCreated { set; get; }
        public DateTime? DateModified { set; get; }
        public DateTime? DateDeleted { set; get; }
    }
}