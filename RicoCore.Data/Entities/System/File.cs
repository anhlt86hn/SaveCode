using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.System
{
    [Table("Files")]
    public class File : DomainEntity<int>, IDateTracking
    {
        public File()
        {

        }
        public File(int? folderId, int subProjectId, string url, string name, string content, string source)
        {
            FolderId = folderId;
            Url = url;
            Name = name;                      
            SubProjectId = subProjectId;
            Content = content;
            Source = source;
        }
        public int? FolderId { set; get; }
        public int SubProjectId { set; get; }
        public string Url { set; get; }
        public string Name { set; get; }
        public string Content { set; get; }
        public string Source { set; get; }
        public DateTime DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public DateTime? DateDeleted { get; set; }
    }
}