using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RicoCore.Models;
using RicoCore.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicoCore.Controllers.Components
{
    public class HeaderCartViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var session = HttpContext.Session.GetString(CommonConstants.CartSession);
            var cart = new List<ShoppingCartViewModel>();
            if(session != null)
                cart = JsonConvert.DeserializeObject<List<ShoppingCartViewModel>>(session);
                return View(cart);
        }
    }
}
