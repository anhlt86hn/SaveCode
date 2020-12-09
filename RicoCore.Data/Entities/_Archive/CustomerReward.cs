using System;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComCustomerRewards")]
    public class CustomerReward : DomainEntity<Guid>, IDateTracking
    {
        public CustomerReward()
        {
        }

        public CustomerReward(Guid customerId, Guid orderId, string description, int points, DateTime dateCreated, DateTime? dateModified, DateTime? dateDeleted)
        {
            CustomerId = customerId;
            OrderId = orderId;
            Description = description;
            Points = points;
            DateCreated = dateCreated;
            DateModified = dateModified;
            DateDeleted = dateDeleted;
        }

        public Guid CustomerId { set; get; }
        public Guid OrderId { set; get; }
        public string Description { set; get; }
        public int Points { set; get; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}