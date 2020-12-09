using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using RicoCore.Infrastructure.Enums;

namespace RicoCore.Services.Systems.Settings.Dtos
{
    public class SystemConfigViewModel
    {
        public Guid Id { set; get; }
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
