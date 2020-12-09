using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComAttributeDescriptions")]
    public class AttributeDescription : DomainEntity<Guid>, IMultiLanguage<Guid>
    {
        public AttributeDescription()
        {
        }

        public AttributeDescription(Guid attributeId, Guid languageId, string name)
        {
            AttributeId = attributeId;
            LanguageId = languageId;
            Name = name;
        }

        [Required]
        public Guid AttributeId { set; get; }

        [Required]
        public Guid LanguageId { set; get; }

        [Required]
        [MaxLength(64)]
        public string Name { set; get; }
    }
}