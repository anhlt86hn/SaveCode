using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities
{
    [Table("ProductQuantities")]
    public class ProductQuantity : DomainEntity<Guid>
    {
 
        [Column(Order = 1)]
        public Guid ProductId { get; set; }

        //[Column(Order = 2)]
        //public Guid SizeId { get; set; }


        [Column(Order = 2)]
        public int ColorId { get; set; }

        public int Quantity { get; set; }

        //[ForeignKey("ProductId")]
        //public virtual Product Product { get; set; }

        //[ForeignKey("SizeId")]
        //public virtual Size Size { get; set; }

        //[ForeignKey("ColorId")]
        //public virtual Color Color { get; set; }
    }
}
