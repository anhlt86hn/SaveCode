using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using RicoCore.Services.Content.PostCategories;
using RicoCore.Services.Content.PostCategories.Dtos;
using RicoCore.Services.Content.Posts;
using RicoCore.Services.Content.Posts.Dtos;
using RicoCore.Services.Systems.Passwords.Dtos;
using RicoCore.Services.Systems.Passwords;
using RicoCore.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

namespace RicoCore.Areas.Admin.Controllers
{
    public class PasswordController : BaseController
    {       
        private readonly IPasswordService _passwordService;    
        private readonly IConfiguration _configuration;

        public PasswordController(IPasswordService passwordService, IConfiguration configuration)
        {            
            _passwordService = passwordService;           
            _configuration = configuration;
        }

        public IActionResult Index(int? kichcotrang, string sapxep, int trang = 1)
        {
            var passwords = new Models.PagedResultPasswordViewModel();
            if (kichcotrang == null)
                kichcotrang = _configuration.GetValue<int>("PageSize");

            //pageSize = 10;
            passwords.PageSize = kichcotrang;
            passwords.SortType = sapxep;
            passwords.Data = _passwordService.GetAllPaging(string.Empty, trang, kichcotrang.Value, sapxep);            
            return View(passwords);
        }
      

        [HttpGet]
        public IActionResult Create()
        {
            var model = new PasswordViewModel();           
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(PasswordViewModel passwordVm)
        {
            try
            {
                if (ModelState.IsValid)
                {                    
                    if (_passwordService.ValidateAddPasswordOrder(passwordVm))
                    {
                        ModelState.AddModelError("",
                            "Thứ tự đã tồn tại");
                        return View(passwordVm);
                    }
                                           
                    _passwordService.Add(passwordVm);
                    _passwordService.Save();
                    return Redirect("/Admin/Password/Index");
                }
                else
                {
                    // IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                    // return new BadRequestObjectResult(allErrors);

                    // return BadRequest(ModelState.Select(x => x.Value.Errors).FirstOrDefault(y => y.Count > 0)?.First()
                    //    .ErrorMessage);
                    IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (var item in allErrors)
                    {
                        var message = item.ErrorMessage;
                    }
                    return View(passwordVm);
                }                            
            }
            catch (Exception)
            {
                return Redirect("/Admin/Password/Index");
            }            
        }


        [HttpGet]
        public IActionResult Update(int id)
        {
            var passwordVm = _passwordService.GetById(id);                   
            return View(passwordVm);
        }

        [HttpPost]
        public IActionResult Update(PasswordViewModel passwordVm)
        {
            try
            {
                if (ModelState.IsValid)
                {                    
                    if (_passwordService.ValidateUpdatePasswordOrder(passwordVm))
                    {
                        ModelState.AddModelError("",
                            "Thứ tự đã tồn tại");
                        return View(passwordVm);
                    }
                  
                    _passwordService.Update(passwordVm);
                    _passwordService.Save();
                    return Redirect("/Admin/Password/Index");
                }
                else
                {
                    // IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                    // return new BadRequestObjectResult(allErrors);

                    // return BadRequest(ModelState.Select(x => x.Value.Errors).FirstOrDefault(y => y.Count > 0)?.First()
                    //    .ErrorMessage);
                    IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (var item in allErrors)
                    {
                        var message = item.ErrorMessage;
                    }
                    return View(passwordVm);
                }
            }
            catch (Exception)
            {
                return Redirect("/Admin/Password/Index");
            }
        }

        [HttpPost]
        public IActionResult SetNewOrder()
        {
            var order = _passwordService.SetNewOrder();
            return Json(new
            {
                order
            });
        }


        #region Get Data API

        //[HttpGet]
        //public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        //{
        //    var model = _passwordService.GetAllPaging(keyword, page, pageSize);
        //    return new OkObjectResult(model);
        //}

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _passwordService.GetById(id);

            return new ObjectResult(model);
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
                _passwordService.Delete(id);              
                _passwordService.Save();
                return new OkObjectResult(id);
            }
        }      

        [HttpPost]
        public IActionResult MultiDelete(ICollection<string> selectedIds)
        {
            if (selectedIds != null)
            {
                _passwordService.MultiDelete(selectedIds.ToList());
            }
            return Json(new { Result = true });
        }    
    }
}