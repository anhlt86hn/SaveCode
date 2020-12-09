using RicoCore.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RicoCore.Services.ECommerce.Products.Dtos
{
    public class ProductColorViewModel
    {
        public int Id { set; get; }
        public Guid ProductId { set; get; }
        public int ColorId { set; get; }

        public string ColorImage { get; set; }
        public string ProductName { set; get; }
        public int ProductCategoryId { set; get; }
        public string ProductCategoryName { set; get; }
        public string Name { set; get; }
        public string Url { set; get; }
        public int SortOrder { set; get; }
        public Status Status { set; get; }
      
    }
}