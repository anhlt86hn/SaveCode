using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RicoCore.Services.Content.Posts.Dtos;
using RicoCore.Utilities.Dtos;
using RicoCore.Services.Content.PostCategories.Dtos;

namespace RicoCore.Areas.Admin.Models
{
    public class PagedResultBaseViewModel<ViewModel> where ViewModel : class
    {
        public PagedResult<ViewModel> Data { get; set; }               
        public string SortType { set; get; }

        public int? PageSize { set; get; }
       

        public List<SelectListItem> PageSizes { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Value = "12",Text = "12"},
            new SelectListItem(){Value = "24",Text = "24"},
            new SelectListItem(){Value = "36",Text = "36"},
            new SelectListItem(){Value = "48",Text = "48"},
            new SelectListItem(){Value = "60",Text = "60"},
            new SelectListItem(){Value = "72",Text = "72"},
            new SelectListItem(){Value = "84",Text = "84"},
            new SelectListItem(){Value = "96",Text = "96"},
            new SelectListItem(){Value = "108",Text = "108"},
            new SelectListItem(){Value = "216",Text = "216"},            
        };
    }
}
