using System;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities
{
    [Table("ProductTags")]
    public class ProductTag : DomainEntity<Guid>
    {
        public Guid ProductId { set; get; }

        public string TagId { set; get; }
    }
}