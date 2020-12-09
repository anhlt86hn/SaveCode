using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using RicoCore.Data.EF.Extensions;
using RicoCore.Data.Entities;
using RicoCore.Data.Entities.System;

namespace RicoCore.Data.EF.Configurations
{
    public class AppUserConfiguration : DbEntityConfiguration<AppUser>
    {
        public override void Configure(EntityTypeBuilder<AppUser> entity)
        {
            entity.Property(c => c.Email).IsRequired();
            entity.Property(c => c.UserName).IsRequired();
            
            // etc.            
        }
    }
}
