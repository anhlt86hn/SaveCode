using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RicoCore.Data.Entities.System;
using RicoCore.Services.Systems.Users.Dtos;
using RicoCore.Utilities.Dtos;

namespace RicoCore.Services.Systems.Users
{
    public interface IUserService
    {
        Task<bool> AddAsync(AppUserViewModel userVm);
        Task<bool> ChangePassword(AppUserViewModel userVm, string newpass);
        Task<IdentityResult> ChangePasswordAsync(AppUserViewModel userVm, string currentPassword, string newPassword);
        Task DeleteAsync(Guid id);

        Task<List<AppUserViewModel>> GetAllAsync();

        PagedResult<AppUserViewModel> GetAllPagingAsync(string keyword, int page, int pageSize);

        Task<AppUserViewModel> GetById(Guid id);
        AppUser GetByEmail(string email);

        Task UpdateAsync(AppUserViewModel userVm);
        void MultiDelete(IList<string> selectedIds);
    }
}
