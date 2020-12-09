using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComProductSpecials")]
    public class ProductSpecial : DomainEntity<Guid>
    {
        [Required]
        public Guid ProductId { set; get; }

        [Required]
        public Guid CustomerGroupId { set; get; }

        [Required]
        public int Priority { set; get; }

        [Required]
        public decimal Price { set; get; }

        [Required]
        public DateTime DateStart { set; get; }

        [Required]
        public DateTime DateEnd { set; get; }
    }
}