using System;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComOrderHistories")]
    public class OrderHistory : DomainEntity<Guid>, IDateTracking
    {
        public OrderHistory()
        {
        }

        public OrderHistory(Guid orderId, Guid orderStatusId, bool notify, string comment, DateTime dateCreated, DateTime? dateModified, DateTime? dateDeleted)
        {
            OrderId = orderId;
            OrderStatusId = orderStatusId;
            Notify = notify;
            Comment = comment;
            DateCreated = dateCreated;
            DateModified = dateModified;
            DateDeleted = dateDeleted;
        }

        public Guid OrderId { set; get; }
        public Guid OrderStatusId { set; get; }
        public bool Notify { set; get; }
        public string Comment { set; get; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}