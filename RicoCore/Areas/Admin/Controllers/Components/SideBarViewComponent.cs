using Microsoft.AspNetCore.Mvc;
using RicoCore.Services.Systems.Functions;
using RicoCore.Services.Systems.Functions.Dtos;
using RicoCore.Utilities.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RicoCore.Areas.Admin.Components
{
    // ViewComponent
    // using Microsoft.AspNetCore.Mvc;
    public class SideBarViewComponent : ViewComponent
    {
        private readonly IFunctionService _functionService;

        public SideBarViewComponent(IFunctionService functionService)
        {
            _functionService = functionService;
        }

        // IViewComponentResult
        // using Microsoft.AspNetCore.Mvc;
        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                //var roles = ((ClaimsPrincipal)User).GetSpecificClaim("Roles");
                //List<FunctionViewModel> functions;
                //if (roles.Split(";").Contains(CommonConstants.AppRole.Admin))
                //{
                //    functions = await _functionService.GetAll(string.Empty);
                //    //functions = await _functionService.GetAllList();
                //}
                //else
                //{
                //    functions = await _functionService.GetAll(string.Empty);

                //    functions = await _functionService.GetAll(string.Empty);
                //}
                //else
                //{
                //    functions = await _functionService.GetAll(string.Empty);

                //}

                var roles = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(x => x.Type == CommonConstants.UserClaims.Roles);
                List<FunctionViewModel> functions;
                if (roles != null && roles.Value.Split(";").Contains(CommonConstants.AppRole.Admin)
                    || roles != null && roles.Value.Split(";").Contains(CommonConstants.AppRole.Rico))
                    functions = await _functionService.GetAll(string.Empty);
                else
                    functions = await _functionService.GetAllWithPermission(User.Identity.Name);

                return View(functions);
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
           
        }
    }
}