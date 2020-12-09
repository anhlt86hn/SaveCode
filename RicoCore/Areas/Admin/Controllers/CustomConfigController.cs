using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using RicoCore.Services.Content.PostCategories;
using RicoCore.Services.Content.PostCategories.Dtos;
using RicoCore.Services.Content.Posts;
using RicoCore.Services.Content.Posts.Dtos;
using RicoCore.Services.Systems.Settings;
using RicoCore.Services.Systems.Settings.Dtos;
using RicoCore.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

namespace RicoCore.Areas.Admin.Controllers
{
    public class CustomConfigController : BaseController
    {
        private readonly ISystemConfigService _systemConfigService;        

        public CustomConfigController(ISystemConfigService systemConfigService)
        {
            _systemConfigService = systemConfigService;
        }

        public IActionResult Index()
        {
            return View();
        }       

        #region Get Data API

        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _systemConfigService.GetAllPaging(keyword, page, pageSize);
            return new OkObjectResult(model);
        }

       

        [HttpGet]
        public IActionResult GetById(Guid id)
        {
            var model = _systemConfigService.GetById(id);

            return new ObjectResult(model);
        }
       

        [HttpPost]
        public IActionResult SaveEntity(SystemConfigViewModel systemConfigVm)
        {
            try
            {
                if (systemConfigVm.Id.ToString() == CommonConstants.DefaultGuid)
                {
                    //if (!_systemConfigService.CheckOrderAddPost(systemConfigVm))
                    //    ModelState.AddModelError("",
                    //        "Thứ tự đã tồn tại");

                    //if (!_systemConfigService.CheckHomeOrderAddUpdatePost(systemConfigVm))
                    //    ModelState.AddModelError("",
                    //        "Thứ tự trang chủ đã tồn tại");

                    //if (!_systemConfigService.CheckHotOrderAddUpdatePost(systemConfigVm))
                    //    ModelState.AddModelError("",
                    //        "Thứ tự khu vực HOT đã tồn tại");

                    //if (_systemConfigService.CheckOrderAddPost(systemConfigVm)
                    //    && _systemConfigService.CheckHomeOrderAddUpdatePost(systemConfigVm)
                    //    && _systemConfigService.CheckHotOrderAddUpdatePost(systemConfigVm))
                    //    _systemConfigService.Add(systemConfigVm);
                    _systemConfigService.Add(systemConfigVm);
                }
                else
                {
                    //if (!_systemConfigService.CheckOrderUpdatePost(systemConfigVm))
                    //    ModelState.AddModelError("",
                    //        "Thứ tự đã tồn tại");

                    //if (!_systemConfigService.CheckHomeOrderAddUpdatePost(systemConfigVm))
                    //    ModelState.AddModelError("",
                    //        "Thứ tự trang chủ đã tồn tại");

                    //if (!_systemConfigService.CheckHotOrderAddUpdatePost(systemConfigVm))
                    //    ModelState.AddModelError("",
                    //        "Thứ tự khu vực HOT đã tồn tại");

                    //if (_systemConfigService.CheckOrderUpdatePost(systemConfigVm)
                    //    && _systemConfigService.CheckHomeOrderAddUpdatePost(systemConfigVm)
                    //    && _systemConfigService.CheckHotOrderAddUpdatePost(systemConfigVm))
                    //    _systemConfigService.Update(systemConfigVm);
                    _systemConfigService.Update(systemConfigVm);
                }

                //if (!ModelState.IsValid)
                //{
                //        IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                //        return new BadRequestObjectResult(allErrors);
                //}

                _systemConfigService.Save();
                return new OkObjectResult(systemConfigVm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                _systemConfigService.Delete(id);              
                _systemConfigService.Save();
                return new OkObjectResult(id);
            }
        }


        [HttpPost]
        public IActionResult MultiDelete(ICollection<string> selectedIds)
        {
            if (selectedIds != null)
            {
                _systemConfigService.MultiDelete(selectedIds.ToList());
            }
            return Json(new { Result = true });
        }        
    }
}