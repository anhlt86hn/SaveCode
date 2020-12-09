using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComProductToCategories")]
    public class ProductToCategory : DomainEntity<Guid>
    {
        [Required]
        public Guid ProductId { set; get; }

        [Required]
        public Guid ProductCategoryId { set; get; }
    }
}