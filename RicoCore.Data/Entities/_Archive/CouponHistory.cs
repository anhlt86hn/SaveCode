using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComCouponHistorys")]
    public class CouponHistory : DomainEntity<Guid>, IDateTracking
    {
        public CouponHistory()
        {
        }

        public CouponHistory(Guid couponId, Guid orderId, Guid customerId, decimal amount, DateTime dateCreated, DateTime? dateModified, DateTime? dateDeleted)
        {
            CouponId = couponId;
            OrderId = orderId;
            CustomerId = customerId;
            Amount = amount;
            DateCreated = dateCreated;
            DateModified = dateModified;
            DateDeleted = dateDeleted;
        }

        [Required]
        public Guid CouponId { set; get; }

        [Required]
        public Guid OrderId { set; get; }

        [Required]
        public Guid CustomerId { set; get; }

        public decimal Amount { set; get; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}