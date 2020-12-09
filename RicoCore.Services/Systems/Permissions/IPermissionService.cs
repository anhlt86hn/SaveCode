using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RicoCore.Data.Entities.System;
using RicoCore.Services.Systems.Permissions.Dtos;

namespace RicoCore.Services.Systems.Permissions
{
    public interface IPermissionService : IWebServiceBase<Permission, int, PermissionViewModel>
    {
        ICollection<PermissionViewModel> GetByFunctionId(int functionId);

        Task<List<PermissionViewModel>> GetByUserId(Guid userId);      

        void DeleteAll(int functionId);       
    }
}