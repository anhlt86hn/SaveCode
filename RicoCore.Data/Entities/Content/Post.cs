using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.Content
{
    [Table("Posts")]
    public class Post : DomainEntity<Guid>,
        IDateTracking, IHasSeoMetaData
    {        
        public Post()
        {
        }

        public Post(int categoryId, string code, string url, string name, string description, string content, string image,
            Status status, Status homeFlag, Status hotFlag, string tags, bool orderStatus, bool hotOrderStatus, bool homeOrderStatus, 
            int hotOrder, int homeOrder, int viewed, int sortOrder, string metaTitle, string metaKeywords, string metaDescription)
        {
            CategoryId = categoryId;           
            Code = code;
            Url = url;
            Name = name;
            Description = description;
            Content = content;
            Image = image;
            Tags = tags;
            HotOrder = hotOrder;            
            HomeOrder = homeOrder;
            Viewed = viewed;           
            SortOrder = sortOrder;
            MetaTitle = metaTitle;
            MetaKeywords = metaKeywords;
            MetaDescription = metaDescription;            
            Status = status;
            HomeFlag = homeFlag;
            HotFlag = hotFlag;
            OrderStatus = orderStatus;
            HotOrderStatus = hotOrderStatus;
            HomeOrderStatus = homeOrderStatus;
        }      

        public Post(Guid id, int categoryId, string code, string url, string name, string description, string content, string image,
            Status status, Status homeFlag, Status hotFlag, string tags, bool orderStatus, bool hotOrderStatus, bool homeOrderStatus,
            int hotOrder, int homeOrder, int viewed, int sortOrder, string metaTitle, string metaKeywords, string metaDescription)
        {
            Id = id;
            CategoryId = categoryId;           
            Code = code;
            Url = url;
            Name = name;
            Description = description;
            Content = content;
            Image = image;           
            HotOrder = hotOrder;           
            HomeOrder = homeOrder;
            Viewed = viewed;
            Tags = tags;          
            SortOrder = sortOrder;
            MetaTitle = metaTitle;
            MetaKeywords = metaKeywords;
            MetaDescription = metaDescription;          
            Status = status;
            HomeFlag = homeFlag;
            HotFlag = hotFlag;
            OrderStatus = orderStatus;
            HotOrderStatus = hotOrderStatus;
            HomeOrderStatus = homeOrderStatus;
        }

        public int CategoryId { get; set; }        

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

        [StringLength(500)]
        public string Image { set; get; }
       
        public int HotOrder { set; get; }        

        public int HomeOrder { set; get; }      

        public int Viewed { set; get; }

        [StringLength(500)]
        public string Tags { get; set; }
        public Status Status { set; get; }
        public bool OrderStatus { set; get; }
        public Status HotFlag { set; get; }
        public bool HotOrderStatus { set; get; }
        public Status HomeFlag { set; get; }
        public bool HomeOrderStatus { set; get; }
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
        //public virtual PostCategory PostCategory { set; get; }

        //public virtual ICollection<Post> PostTags { set; get; }
    }
}