using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComOrderOptions")]
    public class OrderOption : DomainEntity<Guid>
    {
        [Required]
        public Guid OrderId { set; get; }

        [Required]
        public Guid OrderProductId { set; get; }

        [Required]
        public Guid ProductOptionId { set; get; }

        [Required]
        public Guid ProductOptionValueId { set; get; }

        [MaxLength(255)]
        public string Name { set; get; }

        public string Value { set; get; }

        [MaxLength(32)]
        public string Type { set; get; }
    }
}