using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComOrderRecurringTransactions")]
    public class OrderRecurringTransaction : DomainEntity<Guid>, IDateTracking
    {
        public Guid OrderRecurringId { set; get; }

        [MaxLength(255)]
        public string Reference { set; get; }

        [MaxLength(255)]
        public string Type { set; get; }

        public decimal Amount { set; get; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}