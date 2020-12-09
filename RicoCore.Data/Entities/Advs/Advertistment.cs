using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.Advs
{
    [Table("Advertistments")]
    public class Advertistment : DomainEntity<Guid>, ISwitchable, ISortable
    {
        public Advertistment()
        {

        }
        public Advertistment(string name, string description, string image, string url, Guid positionId, Status status, int sortOrder)
        {
            Name = name;
            Description = description;
            Image = image;
            Url = url;
            PositionId = positionId;
            Status = status;
            SortOrder = sortOrder;
        }

        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [StringLength(250)]
        public string Image { get; set; }

        [StringLength(250)]
        public string Url { get; set; }

        public Guid PositionId { get; set; }
        //[ForeignKey("PositionId")]
        //public virtual AdvertistmentPosition AdvertistmentPosition { get; set; }
        public Status Status { set; get; }
        public DateTime DateCreated { set; get; }
        public DateTime? DateModified { set; get; }
        public int SortOrder { set; get; }
    }
}