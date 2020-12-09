using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.Content
{
    [Table("CMS_InformationDescriptions")]
    public class InformationDescription : DomainEntity<Guid>, IMultiLanguage<Guid>, IHasSeoMetaData
    {
        [Required]
        public Guid InformationId { set; get; }

        [Required]
        public Guid LanguageId { get; set; }

        [Required]
        [MaxLength(256)]
        public string Title { set; get; }

        public string Description { set; get; }

        [MaxLength(256)]
        public string SeoPageTitle { get; set; }

        [MaxLength(256)]
        public string SeoAlias { get; set; }

        [MaxLength(256)]
        public string SeoKeywords { get; set; }

        [MaxLength(256)]
        public string SeoDescription { get; set; }
    }
}