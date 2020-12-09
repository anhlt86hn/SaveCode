using System;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.Content
{
    [Table("CMS_Informations")]
    public class Information : DomainEntity<Guid>, ISortable, ISwitchable
    {
        public int Bottom { set; get; }
        public int SortOrder { get; set; }
        public Status Status { get; set; }
    }
}