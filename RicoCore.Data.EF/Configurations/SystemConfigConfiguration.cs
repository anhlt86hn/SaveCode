using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using RicoCore.Data.EF.Extensions;
using RicoCore.Data.Entities;
using RicoCore.Data.Entities.System;

namespace RicoCore.Data.EF.Configurations
{
    public class SystemConfigConfiguration : DbEntityConfiguration<Setting>
    {
        public override void Configure(EntityTypeBuilder<Setting> entity)
        {
            entity.Property(c => c.Id).HasMaxLength(255).IsRequired();
            // etc.
        }
    }
}
