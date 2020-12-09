using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("ProductCategories")]
    public class ProductCategory : DomainEntity<int>, ISwitchable, ISortable, IDateTracking, IHasSeoMetaData
    {
        public ProductCategory()
        {

        }

        public ProductCategory(int? parentId, string code, string url, string name, string description, string image, Status hotFlag, int hotOrder,
         Status homeFlag, int homeOrder, Status status, int sortOrder, string metaTitle, string metaKeywords, string metaDescription)
        {
            ParentId = parentId;
            Name = name;
            Description = description;
            Code = code;
            HomeOrder = homeOrder;
            Image = image;
            HomeFlag = homeFlag;
            HotOrder = hotOrder;
            HotFlag = hotFlag;
            SortOrder = sortOrder;
            Status = status;
            MetaTitle = metaTitle;
            Url = url;
            MetaKeywords = metaKeywords;
            MetaDescription = metaDescription;
        }

        public int? ParentId { set; get; }

        [StringLength(255)]
        public string Code { set; get; }

        [StringLength(255)]
        public string Url { get; set; }

        [StringLength(255)]
        [Required]
        public string Name { set; get; }

        [StringLength(1000)]
        public string Description { set; get; }

        [StringLength(500)]
        public string Image { set; get; }

        public Status HotFlag { set; get; }

        [DefaultValue(0)]
        public int HotOrder { set; get; }

        public Status HomeFlag { set; get; }

        [DefaultValue(0)]
        public int HomeOrder { set; get; }

        public Status Status { set; get; }

        [DefaultValue(0)]
        public int SortOrder { set; get; }
        //public int Order { get; set; }
        [StringLength(70)]
        public string MetaTitle { get; set; }

        [StringLength(500)]
        public string MetaKeywords { get; set; }

        [StringLength(255)]
        public string MetaDescription { get; set; }

        public DateTime DateCreated { set; get; }

        public DateTime? DateDeleted { set; get; }

        public DateTime? DateModified { set; get; }

        //public virtual ICollection<Product> Products { set; get; }
    }
}