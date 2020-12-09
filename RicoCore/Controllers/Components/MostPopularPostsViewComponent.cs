using Microsoft.AspNetCore.Mvc;
using RicoCore.Services.Content.Posts;
using RicoCore.Services.Systems.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicoCore.Controllers.Components
{
    public class MostPopularPostsViewComponent : ViewComponent
    {
        private readonly IPostService _postService;

        public MostPopularPostsViewComponent(IPostService postService)
        {
            _postService = postService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = _postService.GetLastest(5);
            return View(list);
        }
    }
}
