using System;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComProductDiscounts")]
    public class ProductDiscount : DomainEntity<Guid>
    {
        public Guid ProductId { set; get; }
        public Guid CustomerGroupId { set; get; }
        public int Quantity { set; get; }
        public int Priority { set; get; }
        public decimal Price { set; get; }
        public DateTime DateStart { set; get; }
        public DateTime DateEnd { set; get; }
    }
}