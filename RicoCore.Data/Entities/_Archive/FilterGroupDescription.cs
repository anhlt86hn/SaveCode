using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComFilterGroupDescriptions")]
    public class FilterGroupDescription : DomainEntity<Guid>, IMultiLanguage<Guid>
    {
        [Required]
        public Guid FilterGroupId { set; get; }

        [Required]
        public Guid LanguageId { get; set; }

        [MaxLength(256)]
        public string Name { set; get; }
    }
}