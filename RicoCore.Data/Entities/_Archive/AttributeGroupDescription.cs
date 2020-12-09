using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComAttributeGroupDescriptions")]
    public class AttributeGroupDescription : DomainEntity<Guid>, IMultiLanguage<Guid>
    {
        public AttributeGroupDescription()
        {
        }

        public AttributeGroupDescription(Guid attributeGroupId, Guid languageId, string name)
        {
            AttributeGroupId = attributeGroupId;
            LanguageId = languageId;
            Name = name;
        }

        [Required]
        public Guid AttributeGroupId { set; get; }

        [Required]
        public Guid LanguageId { set; get; }

        [Required]
        [MaxLength(64)]
        public string Name { set; get; }
    }
}