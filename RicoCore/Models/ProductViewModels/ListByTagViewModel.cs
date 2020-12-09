using Microsoft.AspNetCore.Mvc.Rendering;
using RicoCore.Services.ECommerce.ProductCategories.Dtos;
using RicoCore.Services.ECommerce.Products.Dtos;
using RicoCore.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicoCore.Models.ProductViewModels
{
    public class ListByTagViewModel
    {       
        public string TagName { set; get; }
        public string TagUrl { set; get; }
        public string MetaTitle { set; get; }
        public string MetaKeywords { set; get; }
        public string MetaDescription { set; get; }        
        public PagedResult<ProductUnitViewModel> Data { get; set; }
        public string SortType { set; get; }
        public int? PageSize { set; get; }
        public int? CategoryId { set; get; }
        public List<SelectListItem> DdlCategory { set; get; }
        public List<SelectListItem> SortTypes { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Value = "moi-nhat", Text = "Sắp xếp theo thứ tự: Mới nhất"},
            new SelectListItem(){Value = "gia", Text = "Sắp xếp theo Giá"},
            new SelectListItem(){Value = "ten", Text = "Sắp xếp theo Tên"},
        };
        public List<SelectListItem> PageSizes { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Value = "12",Text = "Số bản ghi trong 1 trang: 12 bản ghi"},
            new SelectListItem(){Value = "24",Text = "24 bản ghi"},
            new SelectListItem(){Value = "48",Text = "48 bản ghi"},
        };            
    }
}
