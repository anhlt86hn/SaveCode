using System;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComFilters")]
    public class Filter : DomainEntity<Guid>, ISortable
    {
        public Filter()
        {
        }

        public Filter(Guid filterGroupId, int sortOrder)
        {
            FilterGroupId = filterGroupId;
            SortOrder = sortOrder;
        }

        public Guid FilterGroupId { set; get; }
        public int SortOrder { get; set; }
    }
}