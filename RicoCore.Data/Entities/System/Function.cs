using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.System
{
    [Table("Functions")]
    public class Function : DomainEntity<int>, ISortable,IHasUniqueCode
    {
        public Function()
        {
        }

        public Function(int id, string uniqueCode, string name, int? parentId, int sortOrder, bool isActive, Status status, string url, string cssClass)
        {
            Id = id;
            UniqueCode = uniqueCode;
            Name = name;
            Url = url;
            ParentId = parentId;
            CssClass = cssClass;
            SortOrder = sortOrder;
            IsActive = isActive;
            Status = status;
        }

        public Function(string uniqueCode, string name, int? parentId, int sortOrder, bool isActive, Status status, string url, string cssClass)
        {
            UniqueCode = uniqueCode;
            Name = name;
            Url = url;
            ParentId = parentId;
            CssClass = cssClass;
            SortOrder = sortOrder;
            IsActive = isActive;
            Status = status;
        }
        public string UniqueCode { set; get; }
        public string Name { set; get; }

        public string Url { set; get; }

        public int? ParentId { set; get; }

        public string CssClass { get; set; }
        public int SortOrder { set; get; }
        public bool IsActive { set; get; }
        public Status Status { set; get; }
    }
}