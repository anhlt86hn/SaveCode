using System;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComCustomerOnlines")]
    public class CustomerOnline : DomainEntity<Guid>, IDateTracking
    {
        public CustomerOnline()
        {
        }

        public CustomerOnline(string ip, Guid customerId, string url, string referer)
        {
            Ip = ip;
            CustomerId = customerId;
            Url = url;
            Referer = referer;
        }

        public string Ip { set; get; }
        public Guid CustomerId { set; get; }
        public string Url { set; get; }
        public string Referer { set; get; }

        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}