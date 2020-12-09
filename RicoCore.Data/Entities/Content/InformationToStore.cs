using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.Content
{
    [Table("CMS_InformationToStore")]
    public class InformationToStore : DomainEntity<Guid>
    {
        [Required]
        public Guid InformationId { set; get; }

        [Required]
        public Guid StoreId { set; get; }
    }
}