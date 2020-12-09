using System;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.Content
{
    [Table("PostTags")]
    public class PostTag : DomainEntity<Guid>
    {
        public Guid PostId { set; get; }

        public string TagId { set; get; }

        //[ForeignKey("PostId")]
        //public virtual Post Post { set; get; }

        //[ForeignKey("TagId")]
        //public virtual Tag Tag { set; get; }
    }
}