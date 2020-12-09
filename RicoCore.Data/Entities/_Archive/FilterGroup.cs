using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComFilterGroups")]
    public class FilterGroup : DomainEntity<Guid>, ISortable
    {
        public FilterGroup()
        {
        }

        public FilterGroup(int sortOrder)
        {
            SortOrder = sortOrder;
        }

        public int SortOrder { get; set; }
    }
}
