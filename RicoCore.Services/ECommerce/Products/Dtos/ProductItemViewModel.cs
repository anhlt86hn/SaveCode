using RicoCore.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace RicoCore.Services.ECommerce.Products.Dtos
{
    public class ProductItemViewModel
    {
        
        public int Id { set; get; }
        [Column(Order = 1)]
        public int ProductCategoryId { get; set; }
        public string ProductCategoryName { set; get; }

        [Column(Order = 2)]
        public Guid ProductId { get; set; }
        public string ProductName { set; get; }
        public string ProductCode { set; get; }

        [Column(Order = 3)]
        public int ProductColorId { set; get; }
        public string ColorName { set; get; }
        public string ColorImage { set; get; }

        public string Name { set; get; }
        public string Code { set; get; }
        public string Url { set; get; }
        public string ColorUrl { set; get; }
        public int Quantity { set; get; }

        [Required]
        [DefaultValue(0)]
        public decimal Price { set; get; }
        public decimal? PromotionPrice { get; set; }
        public decimal? SpecialPrice { get; set; }
        public decimal OriginalPrice { get; set; }
        public Status IsGood { set; get; }
        public Status HotFlag { set; get; }

        public int HotOrder { set; get; }

        public Status HomeFlag { get; set; }

        public int HomeOrder { set; get; }

        [StringLength(70)]
        public string MetaTitle { set; get; }

        [StringLength(500)]
        public string MetaKeywords { set; get; }

        [StringLength(255)]
        public string MetaDescription { set; get; }

        [StringLength(500)]
        public string Image1 { get; set; }
        [StringLength(500)]
        public string Image1Alt { get; set; }

        [StringLength(500)]
        public string Image2 { get; set; }
        [StringLength(500)]
        public string Image2Alt { get; set; }

        [StringLength(500)]
        public string Image3 { get; set; }
        [StringLength(500)]
        public string Image3Alt { get; set; }

        [StringLength(500)]
        public string Image4 { get; set; }
        [StringLength(500)]
        public string Image4Alt { get; set; }

        [StringLength(500)]
        public string Image5 { get; set; }
        [StringLength(500)]
        public string Image5Alt { get; set; }
      
        public string Content { set; get; }
        public int SortOrder { get; set; }

        public Status Status { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public DateTime? DateDeleted { get; set; }
        //public Status Status { get; set; }

        //public  ProductViewModel Product { get; set; }

        //public virtual SizeViewModel Size { get; set; }

        //public virtual ColorViewModel Color { get; set; }
    }
}