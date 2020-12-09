using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComOrderShipments")]
    public class OrderShipment : DomainEntity<Guid>, IDateTracking
    {
        public Guid OrderId { set; get; }

        public Guid ShippingCourierId { set; get; }

        [MaxLength(255)]
        public string TrackingNumber { set; get; }

        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}