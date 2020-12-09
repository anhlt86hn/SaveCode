using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using RicoCore.Data.EF.Extensions;
using RicoCore.Data.Entities;
using RicoCore.Data.Entities.Content;

namespace RicoCore.Data.EF.Configurations
{
    public class ContactDetailConfiguration : DbEntityConfiguration<ContactDetail>
    {
        public override void Configure(EntityTypeBuilder<ContactDetail> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasMaxLength(255).IsRequired();
            // etc.
        }
    }
}
