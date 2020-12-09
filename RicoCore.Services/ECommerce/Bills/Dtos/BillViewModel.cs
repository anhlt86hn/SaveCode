using RicoCore.Data.Enums;
using RicoCore.Infrastructure.Enums;
using RicoCore.Services.Systems.Users.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RicoCore.Services.ECommerce.Bills.Dtos
{
    public class BillViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string CustomerName { set; get; }

        [Required]
        [MaxLength(256)]
        public string CustomerAddress { set; get; }

        [Required]
        [MaxLength(50)]
        public string CustomerMobile { set; get; }
        public string CustomerFacebook { set; get; }
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

        public BillStatus BillStatus { set; get; }

        public DateTime DateCreated { set; get; }

        public DateTime DateModified { set; get; }
        public decimal? ShippingFee { set; get; }
        public Status Status { set; get; }

        public Guid? CustomerId { set; get; }

        //public AppUserViewModel User { set; get; }

        public List<BillDetailViewModel> BillDetails { set; get; }
    }
}