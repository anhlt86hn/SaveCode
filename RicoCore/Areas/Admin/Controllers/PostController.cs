using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using RicoCore.Areas.Admin.Models;
using RicoCore.Infrastructure.Enums;
using RicoCore.Services.Content.PostCategories;
using RicoCore.Services.Content.PostCategories.Dtos;
using RicoCore.Services.Content.Posts;
using RicoCore.Services.Content.Posts.Dtos;
using RicoCore.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

namespace RicoCore.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class PostController : BaseController
    {
        private readonly IPostCategoryService _postCategoryService;
        private readonly IPostService _postService;
        private readonly IHostingEnvironment _hostingEnvironment;     
        private readonly IConfiguration _configuration;

        public PostController(IPostCategoryService postCategoryService, IPostService postService, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _postCategoryService = postCategoryService;
            _postService = postService;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(string url)
        {
            var post = _postService.GetByUrl(url);
            return View(post);
        }

        public IActionResult ViewBy(string tag, string keyword, int? kichcotrang, string sapxep, int trang = 1)
        {
            var model = new PagedResultPostViewModel();
            if (kichcotrang == null)
                kichcotrang = _configuration.GetValue<int>("PageSize");

            //pageSize = 10;
            model.PageSize = kichcotrang;
            model.SortType = sapxep;
            model.Data = _postService.GetPostsByTagPaging(tag, keyword, trang, kichcotrang.Value, model.SortType);            
            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var model = new PostViewModel();            
            var categories = _postCategoryService.GetCategoriesList();
            _postCategoryService.SetSelectListItemCategories(model, categories);
            model.Status = Status.Actived;
            model.OrderStatus = true;
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(PostViewModel postVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var categories = _postCategoryService.GetCategoriesList();
                    _postCategoryService.SetSelectListItemCategories(postVm, categories);

                    var errorByPostItemName = "Tên bài viết đã tồn tại";
                    if (_postService.ValidateAddPostName(postVm))
                    {
                        ModelState.AddModelError("",
                          errorByPostItemName);
                        return View(postVm);
                    }
                       

                    if (_postService.ValidateAddPostOrder(postVm))
                    {
                        ModelState.AddModelError("",
                            "Thứ tự đã tồn tại");
                        return View(postVm);
                    }
                        

                    if (_postService.ValidateAddPostHotOrder(postVm))
                    {
                        ModelState.AddModelError("",
                            "Thứ tự khu vực HOT đã tồn tại");
                        return View(postVm);
                    }
                        
                    if (_postService.ValidateAddPostHomeOrder(postVm))
                    {
                        ModelState.AddModelError("",
                            "Thứ tự trang chủ đã tồn tại");
                        return View(postVm);
                    }
                                    
                    _postService.Add(postVm);
                    _postService.Save();
                    return Redirect("/Admin/Post/Index");
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
                    return View(postVm);
                }                            
            }
            catch (Exception)
            {
                return Redirect("/Admin/Post/Index");
            }            
        }


        [HttpGet]
        public IActionResult Update(Guid id)
        {
            var postVm = _postService.GetById(id);
            var categories = _postCategoryService.GetAll();
            _postCategoryService.SetSelectListItemCategories(postVm, categories);
            return View(postVm);
        }

        [HttpPost]
        public IActionResult Update(PostViewModel postVm, string oldTags)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var categories = _postCategoryService.GetAll();
                    _postCategoryService.SetSelectListItemCategories(postVm, categories);

                    var errorByPostItemName = "Tên bài viết đã tồn tại";
                    if (_postService.ValidateUpdatePostName(postVm))
                    {
                        ModelState.AddModelError("",
                          errorByPostItemName);
                        return View(postVm);
                    }


                    if (_postService.ValidateUpdatePostOrder(postVm))
                    {
                        ModelState.AddModelError("",
                            "Thứ tự đã tồn tại");
                        return View(postVm);
                    }


                    if (_postService.ValidateUpdatePostHotOrder(postVm))
                    {
                        ModelState.AddModelError("",
                            "Thứ tự khu vực HOT đã tồn tại");
                        return View(postVm);
                    }

                    if (_postService.ValidateUpdatePostHomeOrder(postVm))
                    {
                        ModelState.AddModelError("",
                            "Thứ tự trang chủ đã tồn tại");
                        return View(postVm);
                    }                   
                    _postService.Update(postVm, oldTags);
                    _postService.Save();
                    return Redirect("/Admin/Post/Index");
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
                    return View(postVm);
                }
            }
            catch (Exception)
            {
                return Redirect("/Admin/Post/Index");
            }
        }

        [HttpPost]
        public IActionResult SetNewHomeOrder()
        {
            var homeOrder = _postService.SetNewPostHomeOrder();
            return Json(new
            {
                homeOrder
            });
        }

        [HttpPost]
        public IActionResult SetNewHotOrder()
        {
            var hotOrder = _postService.SetNewPostHotOrder();
            return Json(new
            {
                hotOrder
            });
        }



        #region Get Data API

        [HttpGet]
        public IActionResult GetAllPaging(int? categoryId, string keyword, int page, int pageSize, string sort)
        {
            var model = _postService.GetAllPaging(categoryId, keyword, page, pageSize, sort);
            return new OkObjectResult(model);
        }


        public IActionResult PrepareSetNewPost(int postCategoryId)
        {
            var model = _postService.SetValueToNewPost(postCategoryId);
            return Json(new
            {
                postCategoryId = model.CategoryId,               
                order = model.SortOrder,             
            });
        }
        public IActionResult SetNewPostHotOrder()
        {
            var model = _postService.SetNewPostHotOrder();
            return Json(new
            {               
                order = model
            });
        }

        public IActionResult SetNewPostHomeOrder()
        {
            var model = _postService.SetNewPostHomeOrder();
            return Json(new
            {
                order = model
            });
        }



        [HttpPost]
        public IActionResult SaveEntity(PostViewModel postVm, string oldTags)
        {
            try
            {
                if (postVm.Id.ToString() == CommonConstants.DefaultGuid)
                {
                    var errorByPostItemName = "Tên bài viết đã tồn tại";
                    if (_postService.ValidateAddPostName(postVm))
                        ModelState.AddModelError("",
                           errorByPostItemName);

                    if (_postService.ValidateAddPostOrder(postVm))
                        ModelState.AddModelError("",
                            "Thứ tự đã tồn tại");

                    if (_postService.ValidateAddPostHotOrder(postVm))
                        ModelState.AddModelError("",
                            "Thứ tự khu vực HOT đã tồn tại");

                    if (_postService.ValidateAddPostHomeOrder(postVm))
                        ModelState.AddModelError("",
                            "Thứ tự trang chủ đã tồn tại");

                    if (!ModelState.IsValid)
                        return BadRequest(ModelState.Select(x => x.Value.Errors).FirstOrDefault(y => y.Count > 0)?.First()
                            .ErrorMessage);
                    _postService.Add(postVm);
                }
                else
                {
                    var errorByPostItemName = "Tên bài viết đã tồn tại";
                    if (_postService.ValidateUpdatePostName(postVm))
                        ModelState.AddModelError("",
                           errorByPostItemName);

                    if (_postService.ValidateUpdatePostOrder(postVm))
                        ModelState.AddModelError("",
                            "Thứ tự đã tồn tại");

                    if (_postService.ValidateUpdatePostHotOrder(postVm))
                        ModelState.AddModelError("",
                            "Thứ tự khu vực HOT đã tồn tại");

                    if (_postService.ValidateUpdatePostHomeOrder(postVm))
                        ModelState.AddModelError("",
                            "Thứ tự trang chủ đã tồn tại");

                    if (!ModelState.IsValid)
                        return BadRequest(ModelState.Select(x => x.Value.Errors).FirstOrDefault(y => y.Count > 0)?.First()
                            .ErrorMessage);
                    _postService.Update(postVm, oldTags);
                }

                //if (!ModelState.IsValid)
                //{
                //        IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                //        return new BadRequestObjectResult(allErrors);
                //}

                _postService.Save();
                return new OkObjectResult(postVm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


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
        public IActionResult SaveImages(Guid postId, string[] images)
        {
            _postService.AddImages(postId, images);
            _postService.Save();
            return new OkObjectResult(images);
        }

        [HttpGet]
        public IActionResult GetImages(Guid postId)
        {
            var images = _postService.GetImages(postId);
            return new OkObjectResult(images);
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

        [HttpPost]
        public IActionResult ImportExcel(IList<IFormFile> files, int categoryId)
        {
            if (files != null && files.Count > 0)
            {
                var file = files[0];
                var filename = ContentDispositionHeaderValue
                                   .Parse(file.ContentDisposition)
                                   .FileName
                                   .Trim('"');

                string folder = _hostingEnvironment.WebRootPath + $@"\uploaded\excels\posts";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, filename);

                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                _postService.ImportExcel(filePath, categoryId);
                _postService.Save();
                return new OkObjectResult(filePath);
            }
            return new NoContentResult();
        }

        [HttpPost]
        public IActionResult ExportExcel(int categoryId)
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string sFileName = $"Post_{DateTime.Now:yyyyMMddhhmmss}.xlsx";
            string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(directory, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            var posts = _postService.GetAllToExport(categoryId);
            using (ExcelPackage package = new ExcelPackage(file))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Posts");
                worksheet.Cells["A1"].LoadFromCollection(posts, true, TableStyles.Light1);
                worksheet.Cells.AutoFitColumns();
                package.Save(); //Save the workbook.
            }
            return new OkObjectResult(fileUrl);
        }
    }
}