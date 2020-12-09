using Microsoft.AspNetCore.Mvc.Rendering;
using RicoCore.Services.ECommerce.ProductCategories.Dtos;
using RicoCore.Services.ECommerce.Products.Dtos;
using RicoCore.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicoCore.Services.ECommerce.Products.Dtos
{
    public class ProductCartViewModel
    {
        public ProductViewModel Product { get; set; }       
        public List<ProductItemViewModel> ProductItems { get; set; }
        public ProductItemViewModel ProductItem { get; set; }
    }
}
