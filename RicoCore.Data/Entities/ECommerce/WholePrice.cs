using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities
{
    [Table("WholePrices")]
    public class WholePrice : DomainEntity<Guid>
    {
        
        public Guid ProductId { get; set; }

        public int FromQuantity { get; set; }

        public int ToQuantity { get; set; }

        public decimal Price { get; set; }

        //[ForeignKey("ProductId")]
        //public virtual Product Product { get; set; }
    }
}
