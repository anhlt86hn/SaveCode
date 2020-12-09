using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComCouponProducts")]
    public class CouponProduct : DomainEntity<Guid>
    {
        public CouponProduct()
        {
        }

        public CouponProduct(Guid couponId, Guid productId)
        {
            CouponId = couponId;
            ProductId = productId;
        }

        [Required]
        public Guid CouponId { set; get; }

        [Required]
        public Guid ProductId { set; get; }
    }
}