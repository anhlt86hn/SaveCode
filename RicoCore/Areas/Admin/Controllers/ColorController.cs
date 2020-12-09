using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using RicoCore.Models;
using RicoCore.Models.ProductViewModels;
using RicoCore.Services.Content.PostCategories;
using RicoCore.Services.Content.PostCategories.Dtos;
using RicoCore.Services.Content.Posts;
using RicoCore.Services.Content.Posts.Dtos;
using RicoCore.Services.ECommerce.ProductCategories;
using RicoCore.Services.ECommerce.ProductCategories.Dtos;
using RicoCore.Services.ECommerce.Products;
using RicoCore.Services.ECommerce.Products.Dtos;
using RicoCore.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

namespace RicoCore.Areas.Admin.Controllers
{
    public class ColorController : BaseController
    {
        private readonly IColorService _colorService;       
        private readonly IHostingEnvironment _hostingEnvironment;

        public ColorController(IColorService colorService, IHostingEnvironment hostingEnvironment)
        {
            _colorService = colorService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
       
        [HttpPost]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var sortBy = string.Empty;
            //var productCategoryId = int.Parse(productCategoryIdStr);           
            var model = _colorService.GetAllPaging(keyword, page, pageSize, sortBy);
            return new OkObjectResult(model);
        }     

        #region Get Data API

        public IActionResult GetAll()
        {
            try
            {
                var model = _colorService.GetAll();
                return new OkObjectResult(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }            
        }
    
        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _colorService.GetById(id);
            return new ObjectResult(model);
        }


        [HttpPost]
        public IActionResult SetOrder()
        {           
            var order = _colorService.SetNewColorOrder();
            return Json(new
            {
                order
            });
        }
        [HttpPost]
        public IActionResult SaveEntity(ColorViewModel productItemVm)
        {
            try
            {
                if (productItemVm.Id == 0)
                {                    
                    if (string.IsNullOrEmpty(productItemVm.Code))
                        productItemVm.Code = string.Empty;

                    if (string.IsNullOrEmpty(productItemVm.Url))
                        productItemVm.Url = string.Empty;

                    var errorByColorName = "Tên màu đã tồn tại";
                    if (_colorService.ValidateAddColorName(productItemVm))
                        ModelState.AddModelError("",
                           errorByColorName);

                    var errorBySortOrder = "Thứ tự đã tồn tại";
                    if (_colorService.ValidateAddSortOrder(productItemVm))
                        ModelState.AddModelError("",
                           errorBySortOrder);
                    
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState.Select(x => x.Value.Errors).FirstOrDefault(y => y.Count > 0)?.First()
                            .ErrorMessage);
                    _colorService.Add(productItemVm);
                }
                else
                {                  

                    var errorByColorName = "Tên màu đã tồn tại";
                    if (_colorService.ValidateUpdateColorName(productItemVm))
                        ModelState.AddModelError("",
                           errorByColorName);

                    var errorBySortOrder = "Thứ tự đã tồn tại";
                    if (_colorService.ValidateUpdateSortOrder(productItemVm))
                        ModelState.AddModelError("",
                           errorBySortOrder);

                    if (!ModelState.IsValid)
                    return BadRequest(ModelState.Select(x => x.Value.Errors).FirstOrDefault(y => y.Count > 0)?.First()
                        .ErrorMessage);

                    _colorService.Update(productItemVm);
                }

                //if (!ModelState.IsValid)
                //{
                //    IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                //    return new BadRequestObjectResult(allErrors);
                //}

                _colorService.Save();
                return new OkObjectResult(productItemVm);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      

        #endregion Get Data API

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                _colorService.Delete(id);              
                _colorService.Save();
                return new OkObjectResult(id);
            }
        }

     

        [HttpPost]
        public IActionResult MultiDelete(ICollection<string> selectedIds)
        {
            try
            {
                if (selectedIds != null)
                {
                    _colorService.MultiDelete(selectedIds.ToList());
                    _colorService.Save();
                }
                return new OkResult();
            }
            catch (Exception ex)
            {
                throw ex;               
            }
          
        }               
    }
}