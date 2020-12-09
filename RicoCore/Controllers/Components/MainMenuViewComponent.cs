using Microsoft.AspNetCore.Mvc;
using RicoCore.Services.Systems.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicoCore.Controllers.Components
{
    public class MainMenuViewComponent : ViewComponent
    {
        private readonly IMenuService _menuService;

        public MainMenuViewComponent(IMenuService menuService)
        {
            _menuService = menuService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = _menuService.GetAll();
            return View(list);
        }
    }
}
