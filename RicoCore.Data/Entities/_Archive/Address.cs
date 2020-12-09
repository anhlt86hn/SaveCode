using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComAddress")]
    public class Address : DomainEntity<Guid>
    {
        public Address()
        {
        }

        public Address(Guid customerId, string firstName, string lastName, string company, string address1, string address2, string city, string postCode, Guid countryId, Guid zoneId, string customField)
        {
            CustomerId = customerId;
            FirstName = firstName;
            LastName = lastName;
            Company = company;
            Address1 = address1;
            Address2 = address2;
            City = city;
            PostCode = postCode;
            CountryId = countryId;
            ZoneId = zoneId;
            CustomField = customField;
        }

        public Guid CustomerId { set; get; }

        [MaxLength(32)]
        public string FirstName { set; get; }

        [MaxLength(32)]
        public string LastName { set; get; }

        [MaxLength(40)]
        public string Company { set; get; }

        [MaxLength(128)]
        public string Address1 { set; get; }

        [MaxLength(128)]
        public string Address2 { set; get; }

        [MaxLength(128)]
        public string City { set; get; }

        [MaxLength(10)]
        public string PostCode { set; get; }

        public Guid CountryId { set; get; }
        public Guid ZoneId { set; get; }
        public string CustomField { set; get; }
    }
}