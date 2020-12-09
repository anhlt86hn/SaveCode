using System;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComCustomerWishlists")]
    public class CustomerWishlist : DomainEntity<Guid>, IDateTracking
    {
        public CustomerWishlist()
        {
        }

        public CustomerWishlist(Guid customerId, Guid productId, DateTime dateCreated, DateTime? dateModified, DateTime? dateDeleted)
        {
            CustomerId = customerId;
            ProductId = productId;
            DateCreated = dateCreated;
            DateModified = dateModified;
            DateDeleted = dateDeleted;
        }

        public Guid CustomerId { set; get; }
        public Guid ProductId { set; get; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}