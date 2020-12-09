using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using RicoCore.Areas.Admin.Models;
using RicoCore.Services.Content.PostCategories;
using RicoCore.Services.Content.PostCategories.Dtos;
using RicoCore.Services.Content.Posts;
using RicoCore.Services.Content.Posts.Dtos;
using RicoCore.Services.Content.Tags;
using RicoCore.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

namespace RicoCore.Areas.Admin.Controllers
{
    public class TagController : BaseController
    {
        private readonly IPostCategoryService _postCategoryService;
        private readonly IPostService _postService;
        private readonly ITagService _tagService;        
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        public TagController(IPostCategoryService postCategoryService, IPostService postService,
            ITagService tagService, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _postCategoryService = postCategoryService;
            _postService = postService;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _tagService = tagService;
        }

        public IActionResult Index(string keyword, int? kichcotrang, string sapxep, int trang = 1)
        {
            var tags = new PagedResultTagViewModel();
            if (kichcotrang == null)
                kichcotrang = _configuration.GetValue<int>("PageSize");

            //pageSize = 10;
            tags.PageSize = kichcotrang;
            tags.SortType = sapxep;           
            tags.Data = _tagService.GetPostTagAllPaging(keyword, trang, kichcotrang.Value, tags.SortType);           
            return View(tags);
        }
      

        #region Get Data API

     


        [HttpGet]
        public IActionResult GetById(Guid id)
        {
            var model = _postService.GetById(id);

            return new ObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetNameById(int id)
        {
            var name = _postCategoryService.GetNameById(id);
            return Json(new
            {
                name
            });
        }
        
        #endregion Get Data API

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                _postService.Delete(id);              
                _postService.Save();
                return new OkObjectResult(id);
            }
        }      

        [HttpPost]
        public IActionResult MultiDelete(ICollection<string> selectedIds)
        {
            if (selectedIds != null)
            {
                _postService.MultiDelete(selectedIds.ToList());
            }
            return Json(new { Result = true });
        }
    }
}