using System;
using System.Collections.Generic;
using System.Text;

namespace RicoCore.Services.ECommerce.Products.Dtos
{
    public class WholePriceViewModel
    {
        public Guid ProductId { get; set; }

        public int FromQuantity { get; set; }

        public int ToQuantity { get; set; }

        public decimal Price { get; set; }
    }
}
