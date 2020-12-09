using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.System
{
    [Table("AnnouncementUsers")]
    public class AnnouncementUser : DomainEntity<Guid>
    {
        public AnnouncementUser()
        {
        }

        public AnnouncementUser(Guid announcementId, Guid userId, bool? hasRead)
        {
            AnnouncementId = announcementId;
            UserId = userId;
            HasRead = hasRead;
        }

        [Required]
        public Guid AnnouncementId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public bool? HasRead { get; set; }

        //[ForeignKey("AnnouncementId")]
        //public virtual Announcement Announcement { get; set; }
    }
}