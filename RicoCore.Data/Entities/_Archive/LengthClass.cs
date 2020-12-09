using System;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComLengthClass")]
    public class LengthClass : DomainEntity<Guid>
    {
        public decimal Value { set; get; }
    }
}