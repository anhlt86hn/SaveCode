using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.System
{
    [Table("Folders")]
    public class Folder : DomainEntity<int>, IDateTracking
    {
        public Folder()
        {

        }
        public Folder(int? parentId, int subProjectId, string url, string name, string source)
        {
            ParentId = parentId;
            Url = url;
            Name = name;                   
            SubProjectId = subProjectId;
            Source = source;
        }
        public int? ParentId { set; get; }
        public int SubProjectId { set; get; }
        public string Url { set; get; }
        public string Name { set; get; }
        public string Source { set; get; }
        public DateTime DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public DateTime? DateDeleted { get; set; }
    }
}