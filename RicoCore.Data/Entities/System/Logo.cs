using System;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.System
{
    [Table("Logos")]
    public class Logo : DomainEntity<int>, IDateTracking
    {
        public Logo(string image, string imageAlt, string size, string favicon, int sortOrder, bool status)
        {
            Image = image;
            ImageAlt = imageAlt;
            Size = size;
            Favicon = favicon;
            Status = status;
            SortOrder = sortOrder;
        }
        public string Image { set; get; }
        public string ImageAlt { set; get; }
        public string Size { set; get; }
        public string Favicon { set; get; }
        public bool Status { set; get; }
        public int SortOrder { set; get; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public DateTime? DateDeleted { get; set; }
    }
}