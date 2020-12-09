using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RicoCore.Services.Content.Posts.Dtos;
using RicoCore.Utilities.Dtos;
using RicoCore.Services.Content.PostCategories.Dtos;
using RicoCore.Services.Content.Slides.Dtos;
using RicoCore.Services.ECommerce.Products.Dtos;
using RicoCore.Services.ECommerce.ProductCategories.Dtos;

namespace RicoCore.Models
{
    public class HomeViewModel
    {
        public List<PostViewModel> LastestPosts { set; get; }
        public List<SlideViewModel> HomeSlides { set; get; }

        public List<ProductSingleViewModel> HotProducts { set; get; }
        public List<ProductSingleViewModel> NewestProducts { set; get; }
        public List<ProductSingleViewModel> NicestProducts { set; get; }
        public List<ProductSingleViewModel> SpecialPriceItems { set; get; }
        public List<ProductItemViewModel> TopSellProducts { set; get; }
        public List<ProductCategoryViewModel> HomeCategories { set; get; }
        public string MetaTitle { set; get; }
        public string MetaDescription { set; get; }
        public string MetaKeywords { set; get; }

    }
}
