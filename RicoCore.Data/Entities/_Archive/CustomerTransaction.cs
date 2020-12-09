using System;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComCustomerTransactions")]
    public class CustomerTransaction : DomainEntity<Guid>, IDateTracking
    {
        public CustomerTransaction()
        {
        }

        public CustomerTransaction(Guid customerId, Guid orderId, string description, decimal amount, DateTime dateCreated, DateTime? dateModified, DateTime? dateDeleted)
        {
            CustomerId = customerId;
            OrderId = orderId;
            Description = description;
            Amount = amount;
            DateCreated = dateCreated;
            DateModified = dateModified;
            DateDeleted = dateDeleted;
        }

        public Guid CustomerId { set; get; }
        public Guid OrderId { set; get; }
        public string Description { set; get; }
        public decimal Amount { set; get; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}