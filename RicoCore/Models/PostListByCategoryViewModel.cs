using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RicoCore.Services.Content.Posts.Dtos;
using RicoCore.Utilities.Dtos;
using RicoCore.Services.Content.PostCategories.Dtos;

namespace RicoCore.Models
{
    public class PostListByCategoryViewModel
    {
        public PagedResult<PostViewModel> Data { get; set; }

        public PostCategoryViewModel Category { set; get; }

        public string SortType { set; get; }

        public int? PageSize { set; get; }

        public List<SelectListItem> SortTypes { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Value = "moi-nhat",Text = "Mới nhất"},
            //new SelectListItem(){Value = "price",Text = "Price"},
            new SelectListItem(){Value = "theo-ten",Text = "Tên"},
        };

        public List<SelectListItem> PageSizes { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Value = "10",Text = "10"},
            new SelectListItem(){Value = "20",Text = "20"},
            new SelectListItem(){Value = "30",Text = "30"},
            new SelectListItem(){Value = "40",Text = "40"},
            new SelectListItem(){Value = "50",Text = "50"},
            new SelectListItem(){Value = "60",Text = "60"},
            new SelectListItem(){Value = "70",Text = "70"},
            new SelectListItem(){Value = "80",Text = "80"},
            new SelectListItem(){Value = "90",Text = "90"},
            new SelectListItem(){Value = "100",Text = "100"},
            new SelectListItem(){Value = "200",Text = "200"},
            new SelectListItem(){Value = "300",Text = "300"},
            new SelectListItem(){Value = "500",Text = "500"},
        };
    }
}
