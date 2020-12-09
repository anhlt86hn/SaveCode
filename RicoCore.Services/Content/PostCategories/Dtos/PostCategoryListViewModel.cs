using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RicoCore.Services.ECommerce.Products.Dtos;
using RicoCore.Infrastructure.Enums;
using System.ComponentModel;

namespace RicoCore.Services.Content.PostCategories.Dtos
{
    public class PostCategoryListViewModel
    {
        public int SetOrder { get; set; }
       
        public List<PostCategoryViewModel> PostCategories { set; get; }
    }
}