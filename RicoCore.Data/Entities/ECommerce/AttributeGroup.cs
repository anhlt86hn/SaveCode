using System;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("AttributeGroups")]
    public class AttributeGroup : DomainEntity<Guid>, ISortable
    {
        public AttributeGroup()
        {
        }
        public AttributeGroup(int sortOrder)
        {
            SortOrder = sortOrder;
        }

        

        public int SortOrder { get; set; }
    }
}