using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using RicoCore.Data.EF.Extensions;
using RicoCore.Data.Entities;
using RicoCore.Data.Entities.Content;

namespace RicoCore.Data.EF.Configurations
{
    public class PostCategoryConfiguration : DbEntityConfiguration<PostCategory>
    {
        public override void Configure(EntityTypeBuilder<PostCategory> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasMaxLength(255).IsRequired();
            //entity.Property(c => c.CurrentIdentity).ValueGeneratedOnAdd();
            // etc.
        }
    }
}
