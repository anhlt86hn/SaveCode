using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComOptions")]
    public class Option : DomainEntity<Guid>, ISortable
    {
        public Option()
        {
        }

        public Option(string type, int sortOrder)
        {
            Type = type;
            SortOrder = sortOrder;
        }

        [MaxLength(32)]
        [Required]
        public string Type { set; get; }

        public int SortOrder { get; set; }
    }
}