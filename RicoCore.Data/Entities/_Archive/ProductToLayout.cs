using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComProductToLayouts")]
    public class ProductToLayout : DomainEntity<Guid>
    {
        [Required]
        public Guid ProductId { set; get; }

        [Required]
        public Guid StoreId { set; get; }

        [Required]
        public Guid LayoutId { set; get; }
    }
}