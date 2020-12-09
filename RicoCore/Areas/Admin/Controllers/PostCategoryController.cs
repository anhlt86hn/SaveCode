using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RicoCore.Services.Content.PostCategories;
using RicoCore.Services.Content.PostCategories.Dtos;
using RicoCore.Services.Content.Posts;
using RicoCore.Utilities.Helpers;

namespace RicoCore.Areas.Admin.Controllers
{
    public class PostCategoryController : BaseController
    {
        IPostCategoryService _postCategoryService;
        IPostService _postService;
        public PostCategoryController(IPostCategoryService postCategoryService, IPostService postService)
        {
            _postCategoryService = postCategoryService;
            _postService = postService;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region Get Data API
        [HttpGet]
        public IActionResult GetAll()
        {
            //var model = new PostCategoryListViewModel();
            //model.PostCategories =_postCategoryService.GetAll();
            //model.SetOrder = _postCategoryService.SetNewOrder();
            var model = _postCategoryService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _postCategoryService.GetCategoriesList();
            return new OkObjectResult(categories);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _postCategoryService.GetById(id);

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


        public IActionResult PrepareSetNewPostCategory(int parentId)
        {
            var model = _postCategoryService.SetValueToNewPostCategory(parentId);
            return Json(new
            {
                parentId = model.ParentId,
                parentName = model.ParentPostCategoryName,
                sortOrder = model.SortOrder,
                homeOrder = model.HomeOrder,
                hotOrder = model.HotOrder
                //order = model.Order
            });
        }       
        [HttpPost]
        public IActionResult SaveEntity(PostCategoryViewModel postCategoryVm)
        {
            try
            {
                if (postCategoryVm.Id == 0)
                {
                    var errorByPostItemName = "Tên danh mục bài viết đã tồn tại";
                    if (_postCategoryService.ValidateAddPostCategoryName(postCategoryVm))
                        ModelState.AddModelError("",
                           errorByPostItemName);

                    if (_postCategoryService.ValidateAddPostCategoryOrder(postCategoryVm))
                        ModelState.AddModelError("",
                            "Thứ tự đã tồn tại");

                    if (_postCategoryService.ValidateAddPostCategoryHomeOrder(postCategoryVm))
                        ModelState.AddModelError("",
                            "Thứ tự trang chủ đã tồn tại");

                    if (_postCategoryService.ValidateAddPostCategoryHotOrder(postCategoryVm))
                        ModelState.AddModelError("",
                            "Thứ tự khu vực HOT đã tồn tại");

                    if (!ModelState.IsValid)
                        return BadRequest(ModelState.Select(x => x.Value.Errors).FirstOrDefault(y => y.Count > 0)?.First()
                            .ErrorMessage);
                    _postCategoryService.Add(postCategoryVm);
                }
                else
                {
                    var errorByPostItemName = "Tên danh mục bài viết đã tồn tại";
                    if (_postCategoryService.ValidateUpdatePostCategoryName(postCategoryVm))
                        ModelState.AddModelError("",
                           errorByPostItemName);

                    if (_postCategoryService.ValidateUpdatePostCategoryOrder(postCategoryVm))
                        ModelState.AddModelError("",
                            "Thứ tự đã tồn tại");

                    if (_postCategoryService.ValidateUpdatePostCategoryHomeOrder(postCategoryVm))
                        ModelState.AddModelError("",
                            "Thứ tự trang chủ đã tồn tại");

                    if (_postCategoryService.ValidateUpdatePostCategoryHotOrder(postCategoryVm))
                        ModelState.AddModelError("",
                            "Thứ tự khu vực HOT đã tồn tại");

                    if (!ModelState.IsValid)
                        return BadRequest(ModelState.Select(x => x.Value.Errors).FirstOrDefault(y => y.Count > 0)?.First()
                            .ErrorMessage);
                    _postCategoryService.Update(postCategoryVm);
                }

                //if (!ModelState.IsValid)
                //{
                //        IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                //        return new BadRequestObjectResult(allErrors);
                //}

                _postCategoryService.Save();
                return new OkObjectResult(postCategoryVm);
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
                    _postCategoryService.UpdateParentId(sourceId, targetId, items);
                    _postCategoryService.Save();
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
                    _postCategoryService.ReOrder(sourceId, targetId);
                    _postCategoryService.Save();
                    return new OkResult();
                }
            }
        }
        #endregion

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {                
                var subListL2 = _postCategoryService.AllSubCategories(id);
                if (subListL2 != null)
                {
                    foreach (var itemL2 in subListL2)
                    {
                        var subListL3 = _postCategoryService.AllSubCategories(itemL2.Id);
                        if (subListL3 != null)
                        {
                            foreach (var itemL3 in subListL3)
                            {
                                var subListL4 = _postCategoryService.AllSubCategories(itemL3.Id);
                                if (subListL4 != null)
                                {
                                    foreach (var itemL4 in subListL4)
                                    {
                                        var subListL5 = _postCategoryService.AllSubCategories(itemL4.Id);
                                        if (subListL5 != null)
                                        {
                                            foreach (var itemL5 in subListL5)
                                            {
                                                var subListL6 = _postCategoryService.AllSubCategories(itemL5.Id);
                                                if (subListL6 != null)
                                                {
                                                    foreach (var itemL6 in subListL6)
                                                    {
                                                        var lstIdL6 = _postService.GetIds(itemL6.Id);
                                                        _postService.MultiDelete(lstIdL6);
                                                        _postCategoryService.Delete(itemL6.Id);
                                                        _postService.Save();
                                                    }
                                                }
                                                var lstIdL5 = _postService.GetIds(itemL5.Id);
                                                _postService.MultiDelete(lstIdL5);
                                                _postCategoryService.Delete(itemL5.Id);
                                                _postService.Save();
                                            }
                                        }
                                    var lstIdL4 = _postService.GetIds(itemL4.Id);
                                    _postService.MultiDelete(lstIdL4);
                                    _postCategoryService.Delete(itemL4.Id);
                                    _postService.Save();
                                    }    
                                }
                                var lstIdL3 = _postService.GetIds(itemL3.Id);
                                _postService.MultiDelete(lstIdL3);
                                _postCategoryService.Delete(itemL3.Id);
                                _postService.Save();
                            }                            
                        }
                        var lstIdL2 = _postService.GetIds(itemL2.Id);
                        _postService.MultiDelete(lstIdL2);
                        _postCategoryService.Delete(itemL2.Id);
                        _postService.Save();
                    }                    
                }                
                var lstIdL1 = _postService.GetIds(id);
                _postService.MultiDelete(lstIdL1);
                _postService.Save();               
                _postCategoryService.Delete(id);
                _postCategoryService.Save();                
                return new OkObjectResult(id);
            }
        }
    }
}