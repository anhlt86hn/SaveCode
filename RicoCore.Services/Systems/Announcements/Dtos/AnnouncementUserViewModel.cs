using RicoCore.Services.Systems.Users.Dtos;
using System;
using System.ComponentModel.DataAnnotations;

namespace RicoCore.Services.Systems.Announcements.Dtos
{
    public class AnnouncementUserViewModel
    {
        public Guid AnnouncementId { get; set; }
     
        public Guid UserId { get; set; }

        public bool? HasRead { get; set; }

        //public virtual AppUserViewModel AppUser { get; set; }

        //public virtual AnnouncementViewModel Announcement { get; set; }
    }
}