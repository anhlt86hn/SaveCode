using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.Content
{
    [Table("PostImages")]
    public class PostImage : DomainEntity<Guid>,ISortable
    {
        public PostImage() { }
        public Guid PostId { get; set; }

        [StringLength(255)]
        public string Path { get; set; }

        [StringLength(255)]
        public string Caption { get; set; }

        public int SortOrder { get; set; }
    }
}