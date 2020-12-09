using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.System
{
    [Table("Menus")]
    public class Menu : DomainEntity<int>, ISwitchable
    {
        public Menu()
        {

        }
        public Menu(int? parentId, string url, string name, int sortOrder, Status status)
        {
            ParentId = parentId;
            Url = url;
            Name = name;
            SortOrder = sortOrder;
            Status = status;
        }
        public int? ParentId { set; get; }
        public string Url { set; get; }
        public string Name { set; get; }
        public int SortOrder { set; get; }
        public Status Status { set; get; }
    }
}