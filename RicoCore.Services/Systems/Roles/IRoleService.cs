using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RicoCore.Services.Systems.Permissions.Dtos;
using RicoCore.Services.Systems.Roles.Dtos;
using RicoCore.Data.Entities;
using RicoCore.Utilities.Dtos;
using RicoCore.Services.Systems.Announcements.Dtos;

namespace RicoCore.Services.Systems.Roles
{
    public interface IRoleService 
    {
        Task<bool> AddAsync(AnnouncementViewModel announcementVm, List<AnnouncementUserViewModel> announcementUsers, AppRoleViewModel userVm);
        Task UpdateAsync(AppRoleViewModel userVm);

        Task DeleteAsync(Guid id);
        Task<AppRoleViewModel> GetById(Guid id);
        Task<List<AppRoleViewModel>> GetAllAsync();
        PagedResult<AppRoleViewModel> GetAllPagingAsync(string keyword, int page, int pageSize);
        List<PermissionViewModel> GetListFunctionWithRole(Guid roleId);
        //IQueryable<AppRoleViewModel> GetAll();             

        void SavePermission(List<PermissionViewModel> permissions, Guid roleId);

        Task<bool> CheckPermission(string functionCode, string action, string[] roles);
        void MultiDelete(IList<string> selectedIds);
    }
}
