using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Enums;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.Content
{
    [Table("Tags")]
    public class Tag : DomainEntity<string>, IDateTracking
    {
        public Tag()
        {

        }
        public Tag(string name, string type, int sortOrder, string metaTitle, string metaKeywords, string metaDescription)
        {
            Name = name;           
            Type = type;
            SortOrder = sortOrder;
            MetaTitle = metaTitle;
            MetaKeywords = metaKeywords;
            MetaDescription = metaDescription;
        }

        [MaxLength(255)]
        [Required]
        public string Name { set; get; }        

        [Required]
        public string Type { get; set; }
        //public TagType Type { set; get; }
        [StringLength(70)]
        public string MetaTitle { set; get; }

        [StringLength(500)]
        public string MetaKeywords { set; get; }

        [StringLength(255)]
        public string MetaDescription { set; get; }
        public int SortOrder { get; set; }
        public DateTime DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public DateTime? DateDeleted { get; set; }
    }
}