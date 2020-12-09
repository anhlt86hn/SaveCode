using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComProductCategoryFilters")]
    public class ProductCategoryFilter : DomainEntity<Guid>
    {
        public ProductCategoryFilter()
        {
        }

        public ProductCategoryFilter(Guid productCategoryId, Guid filterId)
        {
            ProductCategoryId = productCategoryId;
            FilterId = filterId;
        }

        [Required]
        public Guid ProductCategoryId { set; get; }

        [Required]
        public Guid FilterId { set; get; }
    }
}