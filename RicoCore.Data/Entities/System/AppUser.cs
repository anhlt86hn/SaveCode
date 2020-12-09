using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.Enums;

namespace RicoCore.Data.Entities.System
{
    [Table("AppUsers")]
    /// AppUser inherit from IdentityUser:
    /// using Microsoft.AspNetCore.Identity;
    public class AppUser : IdentityUser<Guid>, IDateTracking, ISwitchable
    {
        public AppUser()
        {
        }       

        public AppUser(string fullName, string userName, 
            string email, string phoneNumber, string avatar, Status status)
        {
            //Id = id;
            FullName = fullName;
            UserName = userName;
            Email = email;
            PhoneNumber = phoneNumber;
            Avatar = avatar;
            Status = status;
        }

        public string FullName { get; set; }

        public DateTime? BirthDay { set; get; }

        //public string Address { get; set; }

        //public string Gender { get; set; }

        public decimal Balance { get; set; }

        public string Avatar { get; set; }       
        public DateTime DateCreated { set; get; }
        public DateTime? DateModified { set; get; }
        public DateTime? DateDeleted { set; get; }
        public Status Status { set; get; }
    }
}