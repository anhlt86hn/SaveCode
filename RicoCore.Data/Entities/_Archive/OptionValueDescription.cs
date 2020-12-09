using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComOptionValueDescriptions")]
    public class OptionValueDescription : DomainEntity<Guid>, IMultiLanguage<Guid>
    {
        [Required]
        public Guid OptionValueId { set; get; }

        [Required]
        public Guid OptionId { set; get; }

        [Required]
        public Guid LanguageId { get; set; }

        [MaxLength(128)]
        public string Name { set; get; }
    }
}