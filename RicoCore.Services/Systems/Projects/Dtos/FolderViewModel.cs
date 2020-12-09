using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RicoCore.Infrastructure.Enums;

namespace RicoCore.Services.Systems.Projects.Dtos
{
    public class FolderViewModel
    {
        public int Id { set; get; }
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