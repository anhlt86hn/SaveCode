using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComCustomerActivities")]
    public class CustomerActivity : DomainEntity<Guid>, IDateTracking
    {
        public CustomerActivity()
        {
        }

        public CustomerActivity(Guid customerId, string key, string data, string ip, DateTime dateCreated, DateTime? dateModified, DateTime? dateDeleted)
        {
            CustomerId = customerId;
            Key = key;
            Data = data;
            Ip = ip;
            DateCreated = dateCreated;
            DateModified = dateModified;
            DateDeleted = dateDeleted;
        }

        [Required]
        public Guid CustomerId { set; get; }

        [MaxLength(64)]
        public string Key { set; get; }

        public string Data { set; get; }

        [MaxLength(40)]
        public string Ip { set; get; }

        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}