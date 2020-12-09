using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities
{
    [Table("Sizes")]
    public class Size : DomainEntity<Guid>
    {
        public Size() { }
        [StringLength(250)]
        public string Name
        {
            get; set;
        }
    }
}
