using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComProductCategoryDescriptions")]
    public class ProductCategoryDescription : DomainEntity<Guid>, IHasSeoMetaData, IMultiLanguage<Guid>
    {
        public ProductCategoryDescription()
        {
        }

        public ProductCategoryDescription(Guid productCategoryId, Guid languageId, string name, string description, string seoPageTitle, string seoAlias, string seoKeywords, string seoDescription)
        {
            ProductCategoryId = productCategoryId;
            LanguageId = languageId;
            Name = name;
            Description = description;
            SeoPageTitle = seoPageTitle;
            SeoAlias = seoAlias;
            SeoKeywords = seoKeywords;
            SeoDescription = seoDescription;
        }

        [Required]
        public Guid ProductCategoryId { set; get; }

        [Required]
        public Guid LanguageId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { set; get; }

        public string Description { set; get; }

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