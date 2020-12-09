using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RicoCore.Services.Systems.Functions;
using RicoCore.Services.Systems.Functions.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicoCore.Areas.Admin.Controllers
{
    public class FunctionController : BaseController
    {
        private readonly IFunctionService _functionService;

        public FunctionController(IFunctionService functionService)
        {
            _functionService = functionService;
        }

        public IActionResult Index()
        {
            return View();
        }    

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var model = await _functionService.GetAll(string.Empty);
            var parentFunctions = model.Where(c => c.ParentId == null);
            var items = new List<FunctionViewModel>();
            foreach (var function in parentFunctions)
            {
                //add the parent category to the item list
                items.Add(function);
                //now get all its children (separate Category in case you need recursion)
                GetByParentId(model.ToList(), function, items);
            }
            return new ObjectResult(items);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _functionService.GetById(id);

            return new ObjectResult(model);
        }
        public IActionResult PrepareSetNewFunction(int parentId)
        {
            var model = _functionService.SetValueToNewFunction(parentId);
            return Json(new
            {
                parentId = model.ParentId,
                parentName = model.ParentName,
                sortOrder = model.SortOrder
            });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new FunctionViewModel();
            var functions = _functionService.GetAll();
            model.Categories = _functionService.FunctionsSelectListItem();
            model.Status = Infrastructure.Enums.Status.Actived;
            model.IsActive = true;
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(FunctionViewModel functionVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var categories = _functionService.GetAll();
                    functionVm.Categories = _functionService.FunctionsSelectListItem(id: functionVm.Id);

                    var errorByProductItemName = "Mã chức năng đã tồn tại";
                    if (_functionService.ValidateAddUniqueCode(functionVm))
                    {
                        ModelState.AddModelError("",
                           errorByProductItemName);
                        return View(functionVm);
                    }                        

                    if (_functionService.ValidateAddOrder(functionVm))
                    {
                        ModelState.AddModelError("",
                           "Thứ tự đã tồn tại");
                        return View(functionVm);
                    }
                    if (functionVm.ParentId == 0) functionVm.ParentId = null;
                    _functionService.Add(functionVm);
                    _functionService.Save();
                    return Redirect("/admin/function/index");
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
                    return View(functionVm);
                }
            }
            catch (Exception)
            {
                return Redirect("/admin/function/index");
            }
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var model = _functionService.GetById(id);           
            model.Categories = _functionService.FunctionsSelectListItem(id: id);                      
            return View(model);
        }

        [HttpPost]
        public IActionResult Update(FunctionViewModel functionVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var categories = _functionService.GetAll();
                    functionVm.Categories = _functionService.FunctionsSelectListItem(id: functionVm.Id);

                    var errorByProductItemName = "Mã chức năng đã tồn tại";
                    if (_functionService.ValidateUpdateUniqueCode(functionVm))
                    {
                        ModelState.AddModelError("",
                           errorByProductItemName);
                        return View(functionVm);
                    }

                    if (_functionService.ValidateUpdateOrder(functionVm))
                    {
                        ModelState.AddModelError("",
                           "Thứ tự đã tồn tại");
                        return View(functionVm);
                    }
                    if (functionVm.ParentId == 0) functionVm.ParentId = null;
                    _functionService.Update(functionVm);
                    _functionService.Save();
                    return Redirect("/admin/function/index");
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
                    return View(functionVm);
                }
            }
            catch (Exception)
            {
                return Redirect("/admin/function/index");
            }
        }
        public IActionResult SaveEntity(FunctionViewModel functionVm)
        {
            try
            {
                if (functionVm.Id == 0)
                {
                    if (ModelState.IsValid)
                    {
                        var errorByProductItemName = "Mã chức năng đã tồn tại";
                        if (_functionService.ValidateAddUniqueCode(functionVm))
                            ModelState.AddModelError("",
                               errorByProductItemName);

                        if (_functionService.ValidateAddOrder(functionVm))
                            ModelState.AddModelError("",
                                "Thứ tự đã tồn tại");
                        _functionService.Add(functionVm);
                    }
                    else
                    {
                        //IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                        //foreach (var item in allErrors)
                        //{
                        //    var message = item.ErrorMessage;
                        //}
                        return BadRequest(ModelState.Select(x => x.Value.Errors).FirstOrDefault(y => y.Count > 0)?.First()
                           .ErrorMessage);
                    }                                          
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        var errorByProductItemName = "Mã chức năng đã tồn tại";
                        if (_functionService.ValidateUpdateUniqueCode(functionVm))
                            ModelState.AddModelError("",
                               errorByProductItemName);

                        if (_functionService.ValidateUpdateOrder(functionVm))
                            ModelState.AddModelError("",
                                "Thứ tự đã tồn tại");
                        _functionService.Update(functionVm);
                    }
                    else
                    {
                        return BadRequest(ModelState.Select(x => x.Value.Errors).FirstOrDefault(y => y.Count > 0)?.First()
                            .ErrorMessage);
                    }   
   
                }

                //if (!ModelState.IsValid)
                //{
                //        IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                //        return new BadRequestObjectResult(allErrors);
                //}

                _functionService.Save();
                return new OkObjectResult(functionVm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public IActionResult UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                if (sourceId == targetId)
                {
                    return new BadRequestResult();
                }
                else
                {
                    _functionService.UpdateParentId(sourceId, targetId, items);
                    _functionService.Save();
                    return new OkResult();
                }
            }
        }

        [HttpPost]
        public IActionResult ReOrder(int sourceId, int targetId)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                if (sourceId == targetId)
                {
                    return new BadRequestResult();
                }
                else
                {
                    _functionService.ReOrder(sourceId, targetId);
                    _functionService.Save();
                    return new OkObjectResult(sourceId);
                }
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            }
            else
            {
                _functionService.Delete(id);
                _functionService.Save();
                return new OkObjectResult(id);
            }
        }

        #region Private Functions

        private void GetByParentId(IEnumerable<FunctionViewModel> allCats,
            FunctionViewModel parent, IList<FunctionViewModel> items)
        {
            var categoryEntities = allCats as FunctionViewModel[] ?? allCats.ToArray();
            var subCats = categoryEntities.Where(c => c.ParentId == parent.Id);
            foreach (var cat in subCats)
            {
                //add this category
                items.Add(cat);
                //recursive call in case your have a hierarchy more than 1 level deep
                GetByParentId(categoryEntities, cat, items);
            }
        }

        #endregion Private Functions
    }
}