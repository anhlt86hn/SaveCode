using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RicoCore.Extensions;
using RicoCore.Services.Systems.Announcements.Dtos;
using RicoCore.Services.Systems.Permissions.Dtos;
using RicoCore.Services.Systems.Roles;
using RicoCore.Services.Systems.Roles.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicoCore.Areas.Admin.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;
        //private readonly IHubContext<SignalRHub> _hubContext;

        public RoleController(IRoleService roleService
            //, IHubContext<SignalRHub> hubContext
            )
        {
            _roleService = roleService;
            //_hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAll()
        {
            var model = await _roleService.GetAllAsync();
            return new ObjectResult(model);
        }

        public async Task<IActionResult> GetById(Guid Id)
        {
            var model = await _roleService.GetById(Id);
            return new ObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _roleService.GetAllPagingAsync(keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(AppRoleViewModel roleVm)
        {
            //if (!ModelState.IsValid)
            //{
            //    IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            //    return new BadRequestObjectResult(allErrors);
            //}
            //if (string.IsNullOrWhiteSpace(roleVm.Id.ToString()) || roleVm.Id == Guid.Empty)
            //if(!roleVm.Id.HasValue)
            //if (roleVm.Id == null)
            if (roleVm.Id == Guid.Empty)
            {
                var notificationId = Guid.NewGuid();
                var announcement = new AnnouncementViewModel()
                {
                    Title = "Role created",
                    DateCreated = DateTime.Now,
                    Content = $"Role {roleVm.Name} has been created",
                    Id = notificationId,
                    OwnerId = User.GetUserId()
                };
                var announcementUsers = new List<AnnouncementUserViewModel>()
                {
                    new AnnouncementUserViewModel(){AnnouncementId=notificationId, HasRead=false, UserId = User.GetUserId()}
                };
                await _roleService.AddAsync(announcement, announcementUsers, roleVm);
                //await _hubContext.Clients.All.SendAsync("ReceiveMessage", announcement);
            }
            else
            {
                await _roleService.UpdateAsync(roleVm);
            }
            return new OkObjectResult(roleVm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            await _roleService.DeleteAsync(id);
            return new OkObjectResult(id);
        }

        //[HttpPost]
        //public async Task<IActionResult> DeleteAll(string deleteAll)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return new BadRequestObjectResult(ModelState);
        //    }
        //    var model = await _roleService.DeleteAllAsync(lstRolesVm);
        //    return new OkObjectResult(model);
        //}

        [HttpPost]
        public IActionResult ListAllFunction(Guid roleId)
        {
            var functions = _roleService.GetListFunctionWithRole(roleId);
            return new OkObjectResult(functions);
        }

        [HttpPost]
        public IActionResult SavePermission(List<PermissionViewModel> listPermmission, Guid roleId)
        {
            _roleService.SavePermission(listPermmission, roleId);
            return new OkResult();
        }

        [HttpPost]
        public IActionResult MultiDelete(ICollection<string> selectedIds)
        {
            if (selectedIds != null)
            {
                _roleService.MultiDelete(selectedIds.ToList());
            }
            return Json(new { Result = true });
        }
    }
}