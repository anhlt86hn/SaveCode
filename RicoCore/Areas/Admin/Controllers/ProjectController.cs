using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using RicoCore.Areas.Admin.Models;
using RicoCore.Services.Systems.Functions;
using RicoCore.Services.Systems.Functions.Dtos;
using RicoCore.Services.Systems.Projects;
using RicoCore.Services.Systems.Projects.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicoCore.Areas.Admin.Controllers
{
    public class ProjectController : BaseController
    {
        private readonly IProjectService _projectService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        public ProjectController(IProjectService projectService, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _projectService = projectService;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }
      

        public IActionResult Index(string keyword = null, int? kichcotrang = null, string sapxep = null, int trang = 1)
        {
            var projects = new PagedResultProjectViewModel();
            if (kichcotrang == null)
                kichcotrang = _configuration.GetValue<int>("PageSize");

            //pageSize = 10;
            projects.PageSize = kichcotrang;
            projects.SortType = sapxep;
            projects.Data = _projectService.GetAllPaging(keyword, trang, kichcotrang.Value, projects.SortType);
            return View(projects);
        }     

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _projectService.GetById(id);

            return new ObjectResult(model);
        }
        

        [HttpGet]
        public IActionResult Create()
        {
            var model = new ProjectViewModel();       
            model.Status = Infrastructure.Enums.Status.Actived;
            model.IsActive = true;
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(ProjectViewModel projectVm)
        {
            try
            {
                if (ModelState.IsValid)
                {                    
                    var errorByProjectName = "Tên dự án đã tồn tại";
                    if (_projectService.ValidateAddProjectName(projectVm))
                    {
                        ModelState.AddModelError("",
                           errorByProjectName);
                        return View(projectVm);
                    }                        

                    if (_projectService.ValidateAddProjectOrder(projectVm))
                    {
                        ModelState.AddModelError("",
                           "Thứ tự đã tồn tại");
                        return View(projectVm);
                    }                    
                    _projectService.Add(projectVm);
                    _projectService.Save();
                    return Redirect("/admin/project/index");
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
                    return View(projectVm);
                }
            }
            catch (Exception)
            {
                return Redirect("/admin/project/index");
            }
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var model = _projectService.GetById(id);                                           
            return View(model);
        }

        [HttpPost]
        public IActionResult Update(ProjectViewModel projectVm)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var errorByProjectName = "Tên dự án đã tồn tại";
                    if (_projectService.ValidateUpdateProjectName(projectVm))
                    {
                        ModelState.AddModelError("",
                           errorByProjectName);
                        return View(projectVm);
                    }

                    if (_projectService.ValidateUpdateProjectOrder(projectVm))
                    {
                        ModelState.AddModelError("",
                           "Thứ tự đã tồn tại");
                        return View(projectVm);
                    }
                    _projectService.Update(projectVm);
                    _projectService.Save();
                    return Redirect("/admin/project/index");
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
                    return View(projectVm);
                }
            }
            catch (Exception)
            {
                return Redirect("/admin/project/index");
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
                _projectService.Delete(id);
                _projectService.Save();
                return new OkObjectResult(id);
            }
        }

       
    }
}