using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Services.Advs.Dtos
{
    public class AdvertistmentPageViewModel 
    {
        public string Id { set; get; }
        public string UniqueCode { get; set; }
        public string Name { get; set; }
        //public virtual ICollection<AdvertistmentPosition> AdvertistmentPositions { get; set; }
    }
}