using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComOrderVouchers")]
    public class OrderVoucher : DomainEntity<Guid>
    {
        public Guid OrderId { set; get; }
        public Guid VoucherId { set; get; }

        [MaxLength(255)]
        public string Description { set; get; }

        [MaxLength(10)]
        public string Code { set; get; }

        [MaxLength(64)]
        public string FromName { set; get; }

        [MaxLength(200)]
        public string FromEmail { set; get; }

        [MaxLength(64)]
        public string ToName { set; get; }

        [MaxLength(200)]
        public string ToEmail { set; get; }

        public Guid VoucherThemeId { set; get; }

        public string Message { set; get; }
        public decimal Amount { set; get; }
    }
}