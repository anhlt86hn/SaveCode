using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RicoCore.Services.ECommerce.ProductCategories.Dtos;
using RicoCore.Infrastructure.Enums;
using System.ComponentModel;

namespace RicoCore.Services.ECommerce.Products.Dtos
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        //[DefaultValue(0)]
        //public int CurrentIdentity { get; set; }

        [StringLength(255)]
        public string Code { set; get; }

        [StringLength(255)]
        public string Url { set; get; }

        [StringLength(255)]
        [Required]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public string Content { get; set; }

        [MaxLength(500)]
        public string Image { set; get; }

        [Required]
        [DefaultValue(0)]
        public decimal Price { set; get; }

        [DefaultValue(0)]
        public int Quantity { set; get; }

        public decimal? PromotionPrice { get; set; }

        [Required]
        public decimal OriginalPrice { get; set; }

        public Status HotFlag { set; get; }

        public int HotOrder { set; get; }

        public Status HomeFlag { get; set; }

        public int HomeOrder { set; get; }

        public int Points { set; get; }

        public int Viewed { set; get; }

        [StringLength(500)]
        public string Tags { get; set; }

        public Status Status { get; set; }

        public int SortOrder { get; set; }

        [StringLength(70)]
        public string MetaTitle { set; get; }

        [StringLength(500)]
        public string MetaKeywords { set; get; }

        [StringLength(255)]
        public string MetaDescription { set; get; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public DateTime? DateDeleted { get; set; }
      
        //public ProductCategoryViewModel ProductCategory { set; get; }

        //public ICollection<ProductTagViewModel> ProductTags { set; get; }
    }
}