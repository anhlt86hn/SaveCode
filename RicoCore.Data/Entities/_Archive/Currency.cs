using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComCurrencies")]
    public class Currency : DomainEntity<Guid>, ISwitchable, IDateTracking
    {
        public Currency()
        {
        }

        public Currency(string title, string code, string symbolLeft, string symbolRight, string decimalPlace, double value, Status status, DateTime dateCreated, DateTime? dateModified, DateTime? dateDeleted)
        {
            Title = title;
            Code = code;
            SymbolLeft = symbolLeft;
            SymbolRight = symbolRight;
            DecimalPlace = decimalPlace;
            Value = value;
            Status = status;
            DateCreated = dateCreated;
            DateModified = dateModified;
            DateDeleted = dateDeleted;
        }

        [MaxLength(32)]
        public string Title { set; get; }

        [MaxLength(3)]
        public string Code { set; get; }

        [MaxLength(12)]
        public string SymbolLeft { set; get; }

        [MaxLength(12)]
        public string SymbolRight { set; get; }

        [MaxLength(10)]
        public string DecimalPlace { set; get; }

        public double Value { set; get; }
        public Status Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}