using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using RicoCore.Infrastructure.Enums;

namespace RicoCore.Services.Systems.Functions.Dtos
{
    public class FunctionViewModel
    {
        public FunctionViewModel()
        {
            Categories = new List<SelectListItem>();
        }

        public int Id { set; get; }

        public string Name { set; get; }
        public string ParentName { set; get; }
   
        public string Url { set; get; }

        public int? ParentId { set; get; }        

        public string CssClass { get; set; }
        public int SortOrder { set; get; }
        public bool IsActive { set; get; }
        public Status Status { set; get; }
        public string UniqueCode { set; get; }

        public IList<SelectListItem> Categories { set; get; }
        //public List<FunctionViewModel> ChildFunctions { get; set; }
    }
}