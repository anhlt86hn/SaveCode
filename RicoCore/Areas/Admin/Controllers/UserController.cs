using AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using RicoCore.Extensions;
using RicoCore.Infrastructure.Enums;
using RicoCore.Services.Systems.Announcements.Dtos;
using RicoCore.Services.Systems.Users;
using RicoCore.Services.Systems.Users.Dtos;
using RicoCore.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicoCore.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        //private readonly IHubContext<SignalRHub> _hubContext;

        public UserController(IUserService userService, IAuthorizationService authorizationService
            //, IHubContext<SignalRHub> hubContext
            )
        {
            _userService = userService;
            _authorizationService = authorizationService;
            //_hubContext = hubContext;
        }

        //[ClaimRequirement("", "CanReadResource")]
        public async Task<IActionResult> Index()
        {
            var result = await _authorizationService.AuthorizeAsync(User, "USER", Operations.Read);
            if (result.Succeeded == false)
            {
                return new RedirectResult("/Admin/Login/Index");
                //return new RedirectResult("/Admin/Login/AccessDeny");
            }
            return View();
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult GetAll()
        {
            var model = _userService.GetAllAsync();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new OkObjectResult(allErrors);
            }
            else
            {
                var model = await _userService.GetById(id);
                return new OkObjectResult(model);
            }
        }

        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _userService.GetAllPagingAsync(keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        //public IActionResult _ChangePassword()
        //{            
        //    return View();
        //}

        [HttpPost]
        public async Task<IActionResult> ChangePassword(AppUserViewModel userVm)
        {
            var newpass = userVm.Password;
            var model = await _userService.ChangePassword(userVm, newpass);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(AppUserViewModel userVm)
        {
            try
            {
               

                //if (!ModelState.IsValid)
                //{
                //    IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                //    return new OkObjectResult(allErrors);
                //}

                if (userVm.Id.ToString() == CommonConstants.DefaultGuid)
                {
                    if (!string.IsNullOrEmpty(userVm.Email))
                    {
                        var cust2 = _userService.GetByEmail(userVm.Email);
                        if (cust2 != null)
                            ModelState.AddModelError("",
                            "Email đã được dùng để đăng ký!!!");
                    }
                    else
                    {
                        ModelState.AddModelError("",
                           "Phải nhập Email, không được để trống!!!");
                    }

                    if (!ModelState.IsValid)
                    {
                        // ModelError
                        // using Microsoft.AspNetCore.Mvc.ModelBinding;
                        IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                        return new BadRequestObjectResult(allErrors);
                    }

                    //if (!ModelState.IsValid)
                    //    return BadRequest(ModelState.Select(x => x.Value.Errors).FirstOrDefault(y => y.Count > 0)?.First()
                    //        .ErrorMessage);

                    var announcement = new AnnouncementViewModel()
                    {
                        Content = $"Tài khoản {userVm.UserName} đã được tạo",
                        DateCreated = DateTime.Now,
                        Status = Status.Actived,
                        Title = "Tài khoản đã được tạo",
                        OwnerId = User.GetUserId(),
                        Id = Guid.NewGuid()
                    };
                    await _userService.AddAsync(userVm);
                    //await _hubContext.Clients.All.SendAsync("ReceiveMessage", announcement);
                }
                else
                {
                    await _userService.UpdateAsync(userVm);
                }
                return new OkObjectResult(userVm);
            }
            catch (Exception ex)
            {
                throw ex;
            }          
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                await _userService.DeleteAsync(id);
                return new OkObjectResult(id);
            }
        }

        [HttpPost]
        public IActionResult MultiDelete(ICollection<string> selectedIds)
        {
            if (selectedIds != null)
            {
                _userService.MultiDelete(selectedIds.ToList());
            }
            return Json(new { Result = true });
        }
    }
}