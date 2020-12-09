using System;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComProductRecurrings")]
    public class ProductRecurring : DomainEntity<Guid>
    {
        public ProductRecurring()
        {
        }

        public ProductRecurring(Guid productId, Guid recurringId, Guid customerGroupId)
        {
            ProductId = productId;
            RecurringId = recurringId;
            CustomerGroupId = customerGroupId;
        }

        public Guid ProductId { set; get; }
        public Guid RecurringId { set; get; }
        public Guid CustomerGroupId { set; get; }
    }
}