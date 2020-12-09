using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComCustomerLogins")]
    public class CustomerLogin: DomainEntity<Guid>, IDateTracking
    {
        public CustomerLogin()
        {
        }

        public CustomerLogin(string email, string ip, int total, DateTime dateCreated, DateTime? dateModified, DateTime? dateDeleted)
        {
            Email = email;
            Ip = ip;
            Total = total;
            DateCreated = dateCreated;
            DateModified = dateModified;
            DateDeleted = dateDeleted;
        }

        [MaxLength(200)]
        public string Email { set; get; }
        [MaxLength(40)]
        public string Ip { set; get; }
        public int Total { set; get; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}
