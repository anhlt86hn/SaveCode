using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComProductCategories")]
    public class ProductCategory : DomainEntity<Guid>, ISwitchable, ISortable, IDateTracking
    {
        public ProductCategory()
        {
        }

        public ProductCategory(Guid id, string description, Guid? parentId, int? homeOrder, string image, bool? homeFlag,
            int sortOrder, Status status)
        {
            Id = id;
            Description = description;
            ParentId = parentId;
            HomeOrder = homeOrder;
            Image = image;
            HomeFlag = homeFlag;
            SortOrder = sortOrder;
            Status = status;
        }

        [DefaultValue(0)]
        public int CurrentIdentity { get; set; }

        [MaxLength(500)]
        public string Description { set; get; }

        public Guid? ParentId { set; get; }

        public int? HomeOrder { set; get; }

        [MaxLength(256)]
        public string Image { set; get; }

        public bool? HomeFlag { set; get; }

        public DateTime DateCreated { set; get; }
        public int SortOrder { set; get; }
        public Status Status { set; get; }
        public DateTime? DateDeleted { set; get; }
        public DateTime? DateModified { set; get; }
    }
}