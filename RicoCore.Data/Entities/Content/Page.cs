using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.Content
{
    [Table("Pages")]
    public class Page : DomainEntity<Guid>, ISwitchable ,IHasUniqueCode, IDateTracking
    {
        public Page()
        {
        }

        public Page(Guid id, string name, string url, string uniqueCode,
            string content, Status status, string metaTitle, string metaDescription, string metaKeywords)
        {
            Id = id;
            Name = name;
            UniqueCode = uniqueCode;
            Content = content;
            Status = status;
            MetaTitle = metaTitle;
            MetaDescription = metaDescription;
            MetaKeywords = metaKeywords;
            Url = url;
        }


        [Required]
        [MaxLength(256)]
        public string Name { set; get; }

        [Required]
        [MaxLength(256)]
        public string Url { get; set; }

        [Required]
        [MaxLength(70)]
        public string MetaTitle { get; set; }

        [Required]
        [MaxLength(256)]
        public string MetaKeywords { get; set; }

        [Required]
        [MaxLength(256)]
        public string MetaDescription { get; set; }

        public string Content { set; get; }

        public Status Status { set; get; }
      
        public string UniqueCode { set; get; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public DateTime? DateDeleted { get; set; }
    }
}