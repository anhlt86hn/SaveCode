using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RicoCore.Infrastructure.Enums;

namespace RicoCore.Services.Systems.Menus.Dtos
{
    public class MenuViewModel
    {
        public int Id { set; get; }
        public int? ParentId { set; get; }
        public string ParentName { set; get; }
        public string Url { set; get; }
        public string Name { set; get; }
        public int SortOrder { set; get; }
        public Status Status { set; get; }
    }
}