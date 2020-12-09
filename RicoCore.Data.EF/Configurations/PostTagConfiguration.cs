using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicoCore.Data.EF.Extensions;
using RicoCore.Data.Entities;
using RicoCore.Data.Entities.Content;

namespace RicoCore.Data.EF.Configurations
{
    public class PostTagConfiguration : DbEntityConfiguration<PostTag>
    {
        public override void Configure(EntityTypeBuilder<PostTag> entity)
        {
            entity.Property(c => c.TagId).HasMaxLength(255).IsRequired()
            .HasColumnType("varchar(255)");
            // etc.
        }
    }
}