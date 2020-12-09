using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComProductToStores")]
    public class ProductToStore : DomainEntity<Guid>
    {
        [Required]
        public Guid ProductId { set; get; }

        [Required]
        public Guid StoreId { set; get; }
    }
}