using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComCustomFields")]
    public class CustomField : DomainEntity<Guid>, ISwitchable, ISortable
    {
        public CustomField()
        {
        }

        public CustomField(string type, string value, string validation, string location, Status status, int sortOrder)
        {
            Type = type;
            Value = value;
            Validation = validation;
            Location = location;
            Status = status;
            SortOrder = sortOrder;
        }
        [MaxLength(32)]
        public string Type { set; get; }
        public string Value { set; get; }
        [MaxLength(255)]
        public string Validation { set; get; }
        [MaxLength(10)]
        public string Location { set; get; }

        public Status Status { get; set; }
        public int SortOrder { get; set; }
    }
}