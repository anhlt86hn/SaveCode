using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RicoCore.Services.Content.Posts.Dtos;
using RicoCore.Utilities.Dtos;
using RicoCore.Services.Content.PostCategories.Dtos;
using RicoCore.Services.Content.Tags.Dtos;

namespace RicoCore.Areas.Admin.Models
{
    public class PagedResultTagViewModel : PagedResultBaseViewModel<TagViewModel>
    {        
        public List<SelectListItem> SortTypes { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Value = "theo-thu-tu",Text = "Price"},
            new SelectListItem(){Value = "moi-nhat",Text = "Mới nhất"},            
            new SelectListItem(){Value = "theo-ten",Text = "Tên"},
        };
    }
}
