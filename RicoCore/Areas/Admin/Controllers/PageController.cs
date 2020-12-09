using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RicoCore.Services.Content.Pages;
using RicoCore.Services.Content.Pages.Dtos;
using RicoCore.Utilities.Constants;

namespace RicoCore.Areas.Admin.Controllers
{
    public class PageController : BaseController
    {
        public IPageService _pageService;

        public PageController(IPageService pageService)
        {
            _pageService = pageService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAll()
        {
            var model = _pageService.GetAll();

            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetById(Guid id)
        {
            var model = _pageService.GetById(id);

            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _pageService.GetAllPaging(keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(PageViewModel pageVm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                    return new BadRequestObjectResult(allErrors);
                }
                if (pageVm.Id.ToString() == CommonConstants.DefaultGuid)
                {
                    _pageService.Add(pageVm);
                }
                else
                {
                    _pageService.Update(pageVm);
                }
                _pageService.Save();
                return new OkObjectResult(pageVm);
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            _pageService.Delete(id);
            _pageService.Save();

            return new OkObjectResult(id);
        }
    }
}