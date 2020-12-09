using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RicoCore.Services.ECommerce.Products.Dtos
{
    public class ProductQuantityViewModel
    {
        public Guid Id { set; get; }
        [Column(Order = 1)]
        public Guid ProductId { get; set; }

        //[Column(Order = 2)]
        //public Guid SizeId { get; set; }


        [Column(Order = 2)]
        public int ColorId { get; set; }

        public int Quantity { get; set; }

        //public  ProductViewModel Product { get; set; }

        //public virtual SizeViewModel Size { get; set; }

        //public virtual ColorViewModel Color { get; set; }
    }
}