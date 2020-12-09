using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RicoCore.Infrastructure.Enums;
using RicoCore.Services.Systems.Users.Dtos;

namespace RicoCore.Services.Systems.Announcements.Dtos
{
    public class AnnouncementViewModel
    {
        //public AnnouncementViewModel()
        //{
        //    AnnouncementUsers = new List<AnnouncementUserViewModel>();
        //}

        public Guid Id { set; get; }

        [StringLength(250)]
        public string Title { set; get; }

        [StringLength(250)]
        public string Content { set; get; }

        public DateTime DateCreated { set; get; }
        public DateTime? DateModified { set; get; }

        public DateTime? DateDeleted { set; get; }
        public Status Status { set; get; }
        public Guid OwnerId { set; get; }
        //public virtual ICollection<AnnouncementUserViewModel> AnnouncementUsers { get; set; }
    }
}