using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComProductDescriptions")]
    public class ProductDescription : DomainEntity<Guid>, IMultiLanguage<Guid>, IHasSeoMetaData
    {
        [Required]
        public Guid ProductId { set; get; }

        [Required]
        public Guid LanguageId { get; set; }

        [MaxLength(255)]
        public string Name { set; get; }

        public string Description { set; get; }
        public string Tag { set; get; }

        [MaxLength(255)]
        public string SeoPageTitle { get; set; }

        [MaxLength(255)]
        public string SeoAlias { get; set; }

        [MaxLength(255)]
        public string SeoKeywords { get; set; }

        [MaxLength(255)]
        public string SeoDescription { get; set; }
    }
}