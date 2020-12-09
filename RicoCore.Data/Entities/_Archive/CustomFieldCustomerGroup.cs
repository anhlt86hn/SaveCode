using System;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComCustomFieldCustomerGroups")]
    public class CustomFieldCustomerGroup : DomainEntity<Guid>
    {
        public Guid CustomFieldId { set; get; }
        public Guid CustomerGroupId { set; get; }
        public bool Required { set; get; }
    }
}