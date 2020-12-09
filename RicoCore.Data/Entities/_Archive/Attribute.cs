using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComAttributes")]
    public class Attribute : DomainEntity<Guid>, ISortable
    {
        public Attribute()
        {
        }

        public Attribute(Guid attributeGroupId, int sortOrder)
        {
            AttributeGroupId = attributeGroupId;
            SortOrder = sortOrder;
        }

        [Required]
        public Guid AttributeGroupId { set; get; }

        public int SortOrder { get; set; }
    }
}