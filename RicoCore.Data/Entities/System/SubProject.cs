using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.System
{
    [Table("SubProjects")]
    public class SubProject : DomainEntity<int>, IDateTracking
    {
        public SubProject()
        {

        }
        public SubProject(int parentId, string url, string name, int sortOrder, string source)
        {
            ParentId = parentId;
            Url = url;
            Name = name;
            SortOrder = sortOrder;
            Source = source;
        }
        public int ParentId { set; get; }
        public string Url { set; get; }
        public string Name { set; get; }
        public int SortOrder { set; get; }
        public string Source { set; get; }
        public DateTime DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public DateTime? DateDeleted { get; set; }
    }
}