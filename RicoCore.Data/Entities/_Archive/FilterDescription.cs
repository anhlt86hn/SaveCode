using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComFilterDescriptions")]
    public class FilterDescription : DomainEntity<Guid>, IMultiLanguage<Guid>
    {
        [Required]
        public Guid FilterId { set; get; }

        [Required]
        public Guid LanguageId { get; set; }

        [Required]
        public Guid FilterGroupId { set; get; }

        [MaxLength(256)]
        public string Name { set; get; }
    }
}