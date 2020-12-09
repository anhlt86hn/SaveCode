using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RicoCore.Data.Entities.System
{
    [Table("AppRoles")]
    /// AppRole inherit from IdentityRole:
    /// using Microsoft.AspNetCore.Identity;   
    public class AppRole : IdentityRole<Guid>
    {
        public AppRole() : base()
        {
        }

        public AppRole(string name, string description) : base(name)
        {
            Description = description;
        }

        [StringLength(250)]
        public string Description { get; set; }
    }
}