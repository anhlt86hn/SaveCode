using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComCoupons")]
    public class Coupon : DomainEntity<Guid>, ISwitchable, IDateTracking
    {
        public Coupon()
        {
        }

        public Coupon(string type, decimal discount, bool logged, bool shipping, decimal total, DateTime dateStart, DateTime dateEnd, int usesTotal, int usesCustomer, Status status, DateTime dateCreated, DateTime? dateModified, DateTime? dateDeleted)
        {
            Type = type;
            Discount = discount;
            Logged = logged;
            Shipping = shipping;
            Total = total;
            DateStart = dateStart;
            DateEnd = dateEnd;
            UsesTotal = usesTotal;
            UsesCustomer = usesCustomer;
            Status = status;
            DateCreated = dateCreated;
            DateModified = dateModified;
            DateDeleted = dateDeleted;
        }

        [MaxLength(20)]
        public string Type { set; get; }
        public decimal Discount { set; get; }
        public bool Logged { set; get; }
        public bool Shipping { set; get; }
        public decimal Total { set; get; }
        public DateTime DateStart { set; get; }
        public DateTime DateEnd { set; get; }
        public int UsesTotal { set; get; }
        public int UsesCustomer { set; get; }
        public Status Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}