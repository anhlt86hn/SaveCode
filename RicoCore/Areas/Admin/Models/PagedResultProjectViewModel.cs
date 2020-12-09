using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RicoCore.Services.Content.Posts.Dtos;
using RicoCore.Utilities.Dtos;
using RicoCore.Services.Content.PostCategories.Dtos;
using RicoCore.Services.Systems.Projects.Dtos;

namespace RicoCore.Areas.Admin.Models
{
    public class PagedResultProjectViewModel : PagedResultBaseViewModel<ProjectViewModel>
    {       
        //public string CategoryName { set; get; }
        //public int CategoryId { set; get; }
        public List<SelectListItem> SortTypes { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Value = "moi-nhat",Text = "Mới nhất"},
            //new SelectListItem(){Value = "price",Text = "Price"},
            new SelectListItem(){Value = "theo-ten",Text = "Tên"},
        };
    }
}
