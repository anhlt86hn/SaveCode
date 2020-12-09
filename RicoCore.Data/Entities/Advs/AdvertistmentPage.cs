using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.Advs
{
    [Table("AdvertistmentPages")]
    public class AdvertistmentPage : DomainEntity<string>,IHasUniqueCode
    {
        public AdvertistmentPage()
        {

        }
        public AdvertistmentPage(string uniqueCode, string name)
        {
            UniqueCode = uniqueCode;
            Name = name;
        }

        public string UniqueCode { get; set; }
        public string Name { get; set; }
        //public virtual ICollection<AdvertistmentPosition> AdvertistmentPositions { get; set; }
    }
}