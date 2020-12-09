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
    public class LanguageConfiguration : DbEntityConfiguration<Language>
    {
        public override void Configure(EntityTypeBuilder<Language> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasMaxLength(255)
                .IsUnicode(false)
                //.HasColumnType("varchar(255)")
                .IsRequired();
            // etc.
        }
    }
}
