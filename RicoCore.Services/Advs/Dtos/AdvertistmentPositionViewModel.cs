using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Services.Advs.Dtos
{
    public class AdvertistmentPositionViewModel
    {
        [StringLength(20)]
        public Guid PageId { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        //[ForeignKey("PageId")]
        //public virtual AdvertistmentPage AdvertistmentPage { get; set; }

        //public virtual ICollection<Advertistment> Advertistments { get; set; }
    }
}