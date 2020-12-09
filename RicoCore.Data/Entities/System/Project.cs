using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.System
{
    [Table("Projects")]
    public class Project : DomainEntity<int>, ISwitchable, IDateTracking
    {
        public Project()
        {

        }
        public Project(string url, string name, int sortOrder, Status status, string source, bool isActive)
        {           
            Url = url;
            Name = name;
            SortOrder = sortOrder;
            Status = status;
            Source = source;
            IsActive = isActive;
        }       
        public string Url { set; get; }
        public string Name { set; get; }
        public int SortOrder { set; get; }
        public bool IsActive { set; get; }
        public Status Status { set; get; }
        public string Source { set; get; }
        public DateTime DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public DateTime? DateDeleted { get; set; }

    }
}