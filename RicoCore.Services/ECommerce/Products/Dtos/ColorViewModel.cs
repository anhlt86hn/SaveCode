using RicoCore.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RicoCore.Services.ECommerce.Products.Dtos
{
    public class ColorViewModel
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(250)]
        public string EnglishName { get; set; }

        [StringLength(250)]
        public string Code { get; set; }
        public string Url { set; get; }
        public int SortOrder { set; get; }
        public Status Status { set; get; }
        [StringLength(70)]
        public string MetaTitle { set; get; }

        [StringLength(500)]
        public string MetaKeywords { set; get; }

        [StringLength(255)]
        public string MetaDescription { set; get; }
    }
}