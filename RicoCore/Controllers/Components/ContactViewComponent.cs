using Microsoft.AspNetCore.Mvc;
using RicoCore.Services.Content.Contacts;
using RicoCore.Services.Content.PostCategories;
using RicoCore.Services.Content.Posts;
using RicoCore.Services.Systems.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicoCore.Controllers.Components
{
    public class ContactViewComponent : ViewComponent
    {
        private readonly IContactService _contactService;

        public ContactViewComponent(IContactService contactService)
        {
            _contactService = contactService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = _contactService.GetAll().FirstOrDefault();
            return View(list);
        }
    }
}
