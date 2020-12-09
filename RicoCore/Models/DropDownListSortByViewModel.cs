using Microsoft.AspNetCore.Mvc.Rendering;
using RicoCore.Services.ECommerce.ProductCategories.Dtos;
using RicoCore.Services.ECommerce.Products.Dtos;
using RicoCore.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicoCore.Models
{
    public class DropDownListSortByViewModel
    {              
        public string SortType { set; get; }      
        public List<SelectListItem> SortTypes { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Value = "thu-tu-giam-dan", Text = "Sắp xếp theo thứ tự giảm dần"},
            new SelectListItem(){Value = "thu-tu-tang-dan", Text = "Sắp xếp theo thứ tự tăng dần"},
            new SelectListItem(){Value = "ngay-tao-giam-dan", Text = "Sắp xếp theo ngày tạo giảm dần"},
            new SelectListItem(){Value = "ngay-tao-tang-dan", Text = "Sắp xếp theo ngày tạo tăng dần"},
            new SelectListItem(){Value = "ngay-cap-nhat-giam-dan", Text = "Sắp xếp theo ngày cập nhật giảm dần"},
            new SelectListItem(){Value = "ngay-cap-nhat-tang-dan", Text = "Sắp xếp theo ngày cập nhật tăng dần"},
            new SelectListItem(){Value = "so-luong-giam-dan", Text = "Sắp xếp theo số lượng giảm dần"},
            new SelectListItem(){Value = "so-luong-tang-dan", Text = "Sắp xếp theo số lượng tăng dần"},
            //new SelectListItem(){Value = "gia", Text = "Giá"},
            //new SelectListItem(){Value = "ten", Text = "Tên"},
        };                  
    }
}
