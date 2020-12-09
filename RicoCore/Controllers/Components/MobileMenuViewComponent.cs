using Microsoft.AspNetCore.Mvc;
using RicoCore.Services.Systems.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicoCore.Controllers.Components
{
    public class MobileMenuViewComponent : ViewComponent
    {
        private readonly IMenuService _menuService;

        public MobileMenuViewComponent(IMenuService menuService)
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
