using RicoCore.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace RicoCore.Services.Content.Tags.Dtos
{
    public class TagViewModel
    {
        public string Id { set; get; }

        public string Name { set; get; }         
        public string Type { set; get; }
        //public TagType Type { set; get; }
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
    }
}