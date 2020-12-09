using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RicoCore.Services.ECommerce.Products.Dtos;
using RicoCore.Infrastructure.Enums;
using System.ComponentModel;

namespace RicoCore.Services.Content.PostCategories.Dtos
{
    public class PostCategoryViewModel
    {
        public int Id { get; set; }

        public int? ParentId { set; get; }

        public string ParentPostCategoryName { set; get; }        

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
        //public int Order { set; get; }

        [StringLength(70)]
        public string MetaTitle { get; set; }

        [StringLength(500)]
        public string MetaKeywords { get; set; }

        [StringLength(255)]
        public string MetaDescription { get; set; }

        public DateTime DateCreated { set; get; }

        public DateTime? DateDeleted { set; get; }

        public DateTime? DateModified { set; get; }

        //public List<PostViewModel> Posts { set; get; }
    }
}