using Microsoft.AspNetCore.Mvc;
using RicoCore.Services.Content.PostCategories;
using RicoCore.Services.Content.Posts;
using RicoCore.Services.Systems.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicoCore.Controllers.Components
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly IPostCategoryService _postCategoryService;

        public CategoriesViewComponent(IPostCategoryService postCategoryService)
        {
            _postCategoryService = postCategoryService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = _postCategoryService.GetAll();
            return View(list);
        }
    }
}
