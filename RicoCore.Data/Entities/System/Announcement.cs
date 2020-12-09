using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.System
{
    [Table("Announcements")]
    public class Announcement : DomainEntity<Guid>, ISwitchable, IDateTracking, IHasOwner<Guid>
    {
        public Announcement()
        {           
        }

        public Announcement(string title, string content, Status status, Guid ownerId)
        {
            Title = title;
            Content = content;
            Status = status;
            OwnerId = ownerId;
        }

        [Required]
        [StringLength(250)]
        public string Title { set; get; }

        [StringLength(250)]
        public string Content { set; get; }

        public DateTime DateCreated { set; get; }
        public DateTime? DateModified { set; get; }

        public DateTime? DateDeleted { set; get; }
        public Status Status { set; get; }
        public Guid OwnerId { set; get; }
        //[ForeignKey("UserId")]
        //public virtual AppUser AppUser { get; set; }

        //public virtual ICollection<AnnouncementUser> AnnouncementUsers { get; set; }
    }
}