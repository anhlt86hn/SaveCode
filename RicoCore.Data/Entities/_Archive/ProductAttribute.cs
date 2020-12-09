using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComProductAttributes")]
    public class ProductAttribute : DomainEntity<Guid>, IMultiLanguage<Guid>
    {
        [Required]
        public Guid ProductId { set; get; }

        [Required]
        public Guid AttributeId { set; get; }

        [Required]
        public Guid LanguageId { get; set; }

        [Required]
        public string Text { set; get; }
    }
}