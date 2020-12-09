using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComProductCategoryPaths")]
    public class ProductCategoryPath : DomainEntity<Guid>
    {
        public ProductCategoryPath()
        {
        }

        public ProductCategoryPath(Guid productCategoryId, Guid path, int level)
        {
            ProductCategoryId = productCategoryId;
            Path = path;
            Level = level;
        }

        [Required]
        public Guid ProductCategoryId { set; get; }

        [Required]
        public Guid Path { set; get; }

        [Required]
        public int Level { set; get; }
    }
}