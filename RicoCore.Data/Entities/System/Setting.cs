using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.System
{
    [Table("Settings")]
    public class Setting : DomainEntity<Guid>, ISwitchable,IHasUniqueCode
    {
        public Setting()
        {
        }

        public Setting(string name, string url, string textValue, int? integerValue, bool? booleanValue, DateTime? dateValue, decimal? decimalValue, Status status, string uniqueCode)
        {
            Name = name;
            Url = url;
            TextValue = textValue;
            IntegerValue = integerValue;
            BooleanValue = booleanValue;
            DateValue = dateValue;
            DecimalValue = decimalValue;
            Status = status;
            UniqueCode = uniqueCode;
        }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }
        public string Url { set; get; }
        public string TextValue { get; set; }

        public int? IntegerValue { get; set; }

        public bool? BooleanValue { get; set; }

        public DateTime? DateValue { get; set; }

        public decimal? DecimalValue { get; set; }

        public Status Status { get; set; }

        public string UniqueCode { get; set; }
    }
}