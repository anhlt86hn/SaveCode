using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComCustomerSearchs")]
    public class CustomerSearch : DomainEntity<Guid>, IDateTracking, IMultiLanguage<Guid>
    {
        public CustomerSearch()
        {
        }

        public CustomerSearch(Guid storeId, Guid languageId, Guid customerId, string keyword, Guid productCategoryId, bool subCategory, string description, int products, string ip, DateTime dateCreated, DateTime? dateModified, DateTime? dateDeleted)
        {
            StoreId = storeId;
            LanguageId = languageId;
            CustomerId = customerId;
            Keyword = keyword;
            ProductCategoryId = productCategoryId;
            SubCategory = subCategory;
            Description = description;
            Products = products;
            Ip = ip;
            DateCreated = dateCreated;
            DateModified = dateModified;
            DateDeleted = dateDeleted;
        }

        public Guid StoreId { set; get; }
        public Guid LanguageId { set; get; }
        public Guid CustomerId { set; get; }

        [MaxLength(256)]
        public string Keyword { set; get; }

        public Guid ProductCategoryId { set; get; }
        public bool SubCategory { set; get; }
        public string Description { set; get; }
        public int Products { set; get; }
        [MaxLength(40)]
        public string Ip { set; get; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}