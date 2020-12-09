using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComCustomers")]
    public class Customer : DomainEntity<Guid>, ISwitchable, IDateTracking, IMultiLanguage<Guid>
    {
        public Customer()
        {
        }

        public Customer(Guid customerGroupId, Guid storeId, Guid languageId, string firstName, string lastName, string email, string telephone, string fax, string password, string salt, string cart, string wishlist, bool newsLetter, Guid addressId, string customField, string ip, bool safe, string token, string code, Status status, DateTime dateCreated, DateTime? dateModified, DateTime? dateDeleted)
        {
            CustomerGroupId = customerGroupId;
            StoreId = storeId;
            LanguageId = languageId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Telephone = telephone;
            Fax = fax;
            Password = password;
            Salt = salt;
            Cart = cart;
            Wishlist = wishlist;
            NewsLetter = newsLetter;
            AddressId = addressId;
            CustomField = customField;
            IP = ip;
            Safe = safe;
            Token = token;
            Code = code;
            Status = status;
            DateCreated = dateCreated;
            DateModified = dateModified;
            DateDeleted = dateDeleted;
        }

        public Guid CustomerGroupId { set; get; }
        public Guid StoreId { set; get; }
        public Guid LanguageId { set; get; }

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

        [MaxLength(100)]
        public string Password { set; get; }

        [MaxLength(10)]
        public string Salt { set; get; }

        public string Cart { set; get; }
        public string Wishlist { set; get; }
        public bool NewsLetter { set; get; }
        public Guid AddressId { set; get; }
        public string CustomField { set; get; }
        public string IP { set; get; }
        public bool Safe { set; get; }
        public string Token { set; get; }

        [MaxLength(40)]
        public string Code { set; get; }

        public Status Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}