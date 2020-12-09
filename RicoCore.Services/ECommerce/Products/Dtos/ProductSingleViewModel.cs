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
    public class ProductSingleViewModel
    {
        public string Position { set; get; }
        public string ProductUrl { set; get; }         
        public ProductItemViewModel ProductItem { get; set; }
    }
}
