using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComOrderProducts")]
    public class OrderProduct : DomainEntity<Guid>
    {
        [Required]
        public Guid OrderId { set; get; }

        [Required]
        public Guid ProductId { set; get; }

        [MaxLength(255)]
        [Required]
        public string Name { set; get; }

        [MaxLength(64)]
        public string Model { set; get; }

        public int Quantity { set; get; }
        public decimal Price { set; get; }
        public decimal Total { set; get; }
        public decimal Tax { set; get; }
        public int Reward { set; get; }
    }
}