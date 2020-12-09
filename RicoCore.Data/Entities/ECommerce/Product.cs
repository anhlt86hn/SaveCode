using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("Products")]
    public class Product : DomainEntity<Guid>, ISortable, ISwitchable, IDateTracking
    {
        public Product()
        {
        }

        public Product(int categoryId, string code, string url, string name, string description, string content, string image, decimal price, 
            decimal? promotionPrice, decimal originalPrice, Status hotFlag, int hotOrder, Status homeFlag, int homeOrder, int points, int viewed, string tags, Status status, 
            int sortOrder, string metaTitle, string metaKeywords, string metaDescription)
        {
            CategoryId = categoryId;            
            Name = name;           
            Code = code;
            Description = description;
            Content = content;
            Image = image;
            Price = price;
            PromotionPrice = promotionPrice;
            OriginalPrice = originalPrice;
            HomeFlag = homeFlag;
            HotFlag = hotFlag;
            HotOrder = hotOrder;
            Tags = tags;
            Points = points;
            Viewed = viewed;
            SortOrder = sortOrder;
            HomeOrder = homeOrder;
            Status = status;
            MetaTitle = metaTitle;
            Url = url;
            MetaKeywords = metaKeywords;
            MetaDescription = metaDescription;
        }

        public int CategoryId { get; set; }

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

        //[ForeignKey("CategoryId")]
        //public virtual ProductCategory ProductCategory { set; get; }

        //public virtual ICollection<ProductTag> ProductTags { set; get; }
    }
}