using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Enums;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities
{
    [Table("Bills")]
    public class Bill : DomainEntity<Guid>, ISwitchable, IDateTracking
    {
        public Bill() { }

        public Bill(string customerName, string customerAddress, string customerMobile, string customerFacebook, string shipName, string shipAddress, string shipMobile, string customerMessage, string code, decimal? shippingFee,
            BillStatus billStatus, PaymentMethod paymentMethod, Status status, Guid? customerId)
        {
            CustomerName = customerName;
            CustomerAddress = customerAddress;
            CustomerMobile = customerMobile;
            CustomerMessage = customerMessage;
            BillStatus = billStatus;
            PaymentMethod = paymentMethod;
            Status = status;
            CustomerId = customerId;
            ShippingFee = shippingFee;
            Code = code;
            ShipName = shipName;
            ShipAddress = shipAddress;
            ShipMobile = shipMobile;
            CustomerFacebook = customerFacebook;
        }

        public Bill(Guid id, string customerName, string customerAddress, string customerMobile, string customerFacebook, string shipName, string shipAddress, string shipMobile, string customerMessage, string code, decimal? shippingFee,
           BillStatus billStatus, PaymentMethod paymentMethod, Status status, Guid? customerId)
        {
            Id = id;
            CustomerName = customerName;
            CustomerAddress = customerAddress;
            CustomerMobile = customerMobile;
            CustomerMessage = customerMessage;
            BillStatus = billStatus;
            PaymentMethod = paymentMethod;
            Status = status;
            CustomerId = customerId;
            ShippingFee = shippingFee;
            Code = code;
            ShipName = shipName;
            ShipAddress = shipAddress;
            ShipMobile = shipMobile;
            CustomerFacebook = customerFacebook;
        }
        [Required]
        [MaxLength(256)]
        public string CustomerName { set; get; }

        [Required]
        [MaxLength(256)]
        public string CustomerAddress { set; get; }

        [Required]
        [MaxLength(50)]
        public string CustomerMobile { set; get; }
        
        [MaxLength(256)]
        public string ShipName { set; get; }
       
        [MaxLength(256)]
        public string ShipAddress { set; get; }
        
        [MaxLength(50)]
        public string ShipMobile { set; get; }

        [MaxLength(1000)]
        public string CustomerMessage { set; get; }
        [StringLength(255)]
        public string Code { set; get; }
        public PaymentMethod PaymentMethod { set; get; }
        public string CustomerFacebook { set; get; }
        public BillStatus BillStatus { set; get; }
        public decimal? ShippingFee { set; get; }
        public DateTime DateCreated { set; get; }
        public DateTime? DateModified { set; get; }
        public DateTime? DateDeleted { set; get; }
        [DefaultValue(Status.Actived)]
        public Status Status { set; get; } = Status.Actived;

        public Guid? CustomerId { set; get; }

        //[ForeignKey("CustomerId")]
        //public virtual AppUser User { set; get; }

        //public virtual ICollection<BillDetail> BillDetails { set; get; }
    }
}