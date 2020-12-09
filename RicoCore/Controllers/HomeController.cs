using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RicoCore.Models;
using RicoCore.Services.Content.Posts;
using Microsoft.Extensions.Configuration;
using RicoCore.Services.Content.PostCategories;
using RicoCore.Services.ECommerce.ProductCategories;
using RicoCore.Services.ECommerce.Products;
using RicoCore.Services.Systems.Commons;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;

namespace RicoCore.Controllers
{
    public class HomeController : Controller
    {       
        private readonly ICommonService _commonService;
        private readonly IConfiguration _configuration;
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(ICommonService commonService,
            IConfiguration configuration, IStringLocalizer<HomeController> localizer)
        {         
            _configuration = configuration;
            _commonService = commonService;
            _localizer = localizer;
        }

        public IActionResult Index()
        {
                //var title = _localizer["HomeMetaTitle"];
                //var culture = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
                ViewData["BodyClass"] = "cms-index-index cms-home-page";                           
                return Redirect("/admin");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }
        
        public IActionResult Contact()
        {
            
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
