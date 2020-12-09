using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComOrders")]
    public class Order : DomainEntity<Guid>, IMultiLanguage<Guid>, IDateTracking
    {
        public Order()
        {
        }

        public Order(int invoiceNo, string invoicePrefix, Guid storeId, string storeName, string storeUrl, Guid customerId, Guid customerGroupId, string firstName, string lastName, string email, string telephone, string fax, string customField, string paymentFirstName, string paymentLastName, string paymentCompany, string paymentAddress1, string paymentAddress2, string paymentCity, string paymentPostCode, string paymentCountry, Guid paymentCountryId, string paymentZone, Guid paymentZoneId, string paymentAddressFormat, string paymentCustomField, string paymentMethod, string paymentCode, string shippingFirstName, string shippingLastName, string shippingCompany, string shippingAddress1, string shippingAddress2, string shippingCity, string shippingPostCode, string shippingCountry, Guid shippingCountryId, string shippingZone, Guid shippingZoneId, string shippingAddressFormat, string shippingCustomField, string shippingMethod, string shippingCode, string comment, decimal total, Guid orderStatusId, Guid affiliateId, decimal commission, Guid marketingId, string tracking, Guid languageId, Guid currencyId, string currencyCode, decimal currencyValue, string ip, string fowardedIp, string userAgent, string acceptLanguage, DateTime dateCreated, DateTime? dateModified, DateTime? dateDeleted)
        {
            InvoiceNo = invoiceNo;
            InvoicePrefix = invoicePrefix;
            StoreId = storeId;
            StoreName = storeName;
            StoreUrl = storeUrl;
            CustomerId = customerId;
            CustomerGroupId = customerGroupId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Telephone = telephone;
            Fax = fax;
            CustomField = customField;
            PaymentFirstName = paymentFirstName;
            PaymentLastName = paymentLastName;
            PaymentCompany = paymentCompany;
            PaymentAddress1 = paymentAddress1;
            PaymentAddress2 = paymentAddress2;
            PaymentCity = paymentCity;
            PaymentPostCode = paymentPostCode;
            PaymentCountry = paymentCountry;
            PaymentCountryId = paymentCountryId;
            PaymentZone = paymentZone;
            PaymentZoneId = paymentZoneId;
            PaymentAddressFormat = paymentAddressFormat;
            PaymentCustomField = paymentCustomField;
            PaymentMethod = paymentMethod;
            PaymentCode = paymentCode;
            ShippingFirstName = shippingFirstName;
            ShippingLastName = shippingLastName;
            ShippingCompany = shippingCompany;
            ShippingAddress1 = shippingAddress1;
            ShippingAddress2 = shippingAddress2;
            ShippingCity = shippingCity;
            ShippingPostCode = shippingPostCode;
            ShippingCountry = shippingCountry;
            ShippingCountryId = shippingCountryId;
            ShippingZone = shippingZone;
            ShippingZoneId = shippingZoneId;
            ShippingAddressFormat = shippingAddressFormat;
            ShippingCustomField = shippingCustomField;
            ShippingMethod = shippingMethod;
            ShippingCode = shippingCode;
            Comment = comment;
            Total = total;
            OrderStatusId = orderStatusId;
            AffiliateId = affiliateId;
            Commission = commission;
            MarketingId = marketingId;
            Tracking = tracking;
            LanguageId = languageId;
            CurrencyId = currencyId;
            CurrencyCode = currencyCode;
            CurrencyValue = currencyValue;
            Ip = ip;
            FowardedIp = fowardedIp;
            UserAgent = userAgent;
            AcceptLanguage = acceptLanguage;
            DateCreated = dateCreated;
            DateModified = dateModified;
            DateDeleted = dateDeleted;
        }

        public int InvoiceNo { set; get; }
        public string InvoicePrefix { set; get; }
        public Guid StoreId { set; get; }

        [MaxLength(64)]
        public string StoreName { set; get; }

        [MaxLength(255)]
        public string StoreUrl { set; get; }

        public Guid CustomerId { set; get; }
        public Guid CustomerGroupId { set; get; }

        [MaxLength(32)]
        public string FirstName { set; get; }

        [MaxLength(32)]
        public string LastName { set; get; }

        [MaxLength(200)]
        public string Email { set; get; }

        [MaxLength(32)]
        public string Telephone { set; get; }

        [MaxLength(32)]
        public string Fax { set; get; }

        public string CustomField { set; get; }

        [MaxLength(32)]
        public string PaymentFirstName { set; get; }

        [MaxLength(32)]
        public string PaymentLastName { set; get; }

        [MaxLength(60)]
        public string PaymentCompany { set; get; }

        [MaxLength(128)]
        public string PaymentAddress1 { set; get; }

        [MaxLength(128)]
        public string PaymentAddress2 { set; get; }

        [MaxLength(128)]
        public string PaymentCity { set; get; }

        [MaxLength(10)]
        public string PaymentPostCode { set; get; }

        [MaxLength(128)]
        public string PaymentCountry { set; get; }

        public Guid PaymentCountryId { set; get; }

        [MaxLength(128)]
        public string PaymentZone { get; set; }

        public Guid PaymentZoneId { set; get; }

        public string PaymentAddressFormat { set; get; }

        public string PaymentCustomField { set; get; }

        [MaxLength(128)]
        public string PaymentMethod { set; get; }

        [MaxLength(128)]
        public string PaymentCode { set; get; }

        [MaxLength(32)]
        public string ShippingFirstName { set; get; }

        [MaxLength(32)]
        public string ShippingLastName { set; get; }

        [MaxLength(32)]
        public string ShippingCompany { set; get; }

        [MaxLength(128)]
        public string ShippingAddress1 { set; get; }

        [MaxLength(128)]
        public string ShippingAddress2 { set; get; }

        [MaxLength(128)]
        public string ShippingCity { set; get; }

        [MaxLength(10)]
        public string ShippingPostCode { set; get; }

        [MaxLength(128)]
        public string ShippingCountry { set; get; }

        public Guid ShippingCountryId { set; get; }

        [MaxLength(128)]
        public string ShippingZone { set; get; }

        public Guid ShippingZoneId { set; get; }

        public string ShippingAddressFormat { set; get; }

        public string ShippingCustomField { set; get; }

        [MaxLength(128)]
        public string ShippingMethod { set; get; }

        [MaxLength(128)]
        public string ShippingCode { set; get; }

        public string Comment { set; get; }

        public decimal Total { set; get; }

        public Guid OrderStatusId { set; get; }

        public Guid AffiliateId { set; get; }
        public decimal Commission { set; get; }
        public Guid MarketingId { set; get; }

        [MaxLength(64)]
        public string Tracking { set; get; }

        public Guid LanguageId { set; get; }

        public Guid CurrencyId { set; get; }

        public string CurrencyCode { set; get; }

        public decimal CurrencyValue { set; get; }

        [MaxLength(40)]
        public string Ip { set; get; }

        [MaxLength(40)]
        public string FowardedIp { set; get; }

        [MaxLength(255)]
        public string UserAgent { set; get; }

        [MaxLength(255)]
        public string AcceptLanguage { set; get; }

        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}