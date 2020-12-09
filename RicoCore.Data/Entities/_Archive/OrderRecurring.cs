using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComOrderRecurrings")]
    public class OrderRecurring : DomainEntity<Guid>, ISwitchable, IDateTracking
    {
        public OrderRecurring()
        {
        }

        public OrderRecurring(Guid orderId, string reference, Guid productId, string productName, int productQuantity, Guid recurringId, string recurringName, string recurringDescription, string recurringFrequency, int recurringCycle, int recurringDuration, decimal recurringPrice, int trial, string trialFrequency, int trialCycle, int trialDuration, decimal trialPrice, Status status, DateTime dateCreated, DateTime? dateModified, DateTime? dateDeleted)
        {
            OrderId = orderId;
            Reference = reference;
            ProductId = productId;
            ProductName = productName;
            ProductQuantity = productQuantity;
            RecurringId = recurringId;
            RecurringName = recurringName;
            RecurringDescription = recurringDescription;
            RecurringFrequency = recurringFrequency;
            RecurringCycle = recurringCycle;
            RecurringDuration = recurringDuration;
            RecurringPrice = recurringPrice;
            Trial = trial;
            TrialFrequency = trialFrequency;
            TrialCycle = trialCycle;
            TrialDuration = trialDuration;
            TrialPrice = trialPrice;
            Status = status;
            DateCreated = dateCreated;
            DateModified = dateModified;
            DateDeleted = dateDeleted;
        }

        public Guid OrderId { set; get; }

        [MaxLength(255)]
        public string Reference { set; get; }

        public Guid ProductId { set; get; }

        [MaxLength(255)]
        public string ProductName { set; get; }

        public int ProductQuantity { set; get; }

        public Guid RecurringId { set; get; }

        [MaxLength(255)]
        public string RecurringName { set; get; }

        [MaxLength(255)]
        public string RecurringDescription { set; get; }

        [MaxLength(25)]
        public string RecurringFrequency { set; get; }

        public int RecurringCycle { set; get; }

        public int RecurringDuration { set; get; }
        public decimal RecurringPrice { set; get; }
        public int Trial { set; get; }
        public string TrialFrequency { set; get; }
        public int TrialCycle { set; get; }
        public int TrialDuration { set; get; }
        public decimal TrialPrice { set; get; }

        public Status Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}