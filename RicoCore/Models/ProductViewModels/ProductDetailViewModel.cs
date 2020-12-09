using Microsoft.AspNetCore.Mvc.Rendering;
using RicoCore.Services.Content.Tags.Dtos;
using RicoCore.Services.ECommerce.ProductCategories.Dtos;
using RicoCore.Services.ECommerce.Products.Dtos;
using System.Collections.Generic;

namespace RicoCore.Models.ProductViewModels
{
    public class ProductDetailViewModel
    {
        public List<ProductItemViewModel> Items { get; set; }
        public ProductItemViewModel CurrentItem { get; set; }
    }
}