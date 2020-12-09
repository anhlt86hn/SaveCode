using Microsoft.AspNetCore.Mvc.Rendering;
using RicoCore.Services.Content.Tags.Dtos;
using RicoCore.Services.ECommerce.ProductCategories.Dtos;
using RicoCore.Services.ECommerce.Products.Dtos;
using System.Collections.Generic;

namespace RicoCore.Models.ProductViewModels
{
    public class DetailViewModel
    {
        public ProductDetailViewModel ProductDetail { get; set; }
        public string ProductUrl { set; get; }
        public string CategoryName { set; get; }
        public string CategoryUrl { set; get; }       
        //public ProductViewModel Product { get; set; }
        //public bool Available { set; get; }

        //public List<ItemViewModel> ProductItems { set; get; }
        public List<ProductUnitViewModel> RelatedProducts { get; set; }

        //public ProductCategoryViewModel Category { get; set; }

        //public List<ProductImageViewModel> ProductImages { set; get; }

        //public List<ProductUnitViewModel> UpsellProducts { get; set; }

        public List<ProductUnitViewModel> LastestProducts { get; set; }

        //public List<TagViewModel> Tags { set; get; }

        //public List<SelectListItem> Colors { set; get; }

        //public List<SelectListItem> Sizes { set; get; }
    }
}