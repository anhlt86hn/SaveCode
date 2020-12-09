using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RicoCore.Services.Systems.Users.Dtos;
using RicoCore.Data.Entities;
using RicoCore.Utilities.Dtos;
using RicoCore.Data.Entities.System;
using RicoCore.Infrastructure.Interfaces;
using RicoCore.Data.EF;

namespace RicoCore.Services.Systems.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<AppUser> AppUsers;

        public UserService(UserManager<AppUser> userManager,
             IUnitOfWork unitOfWork,
            AppDbContext context)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            AppUsers = context.Set<AppUser>();
        }

        public async Task<bool> AddAsync(AppUserViewModel userVm)
        {            
            var user = new AppUser()
            {
                UserName = userVm.UserName,
                Avatar = userVm.Avatar,
                Email = userVm.Email,
                FullName = userVm.FullName,
                DateCreated = DateTime.Now,
                PhoneNumber = userVm.PhoneNumber,
                Status = Infrastructure.Enums.Status.Actived
            };
            var result = await _userManager.CreateAsync(user, userVm.Password);
            if (result.Succeeded && userVm.Roles.Count > 0)
            {
                var appUser = await _userManager.FindByNameAsync(user.UserName);
                if (appUser != null)
                    await _userManager.AddToRolesAsync(appUser, userVm.Roles);
            }
            return true;
        }

        public async Task<IdentityResult> ChangePasswordAsync(AppUserViewModel userVm, string currentPassword, string newPassword)
        {
            var user = Mapper.Map<AppUserViewModel, AppUser>(userVm);
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public async Task<bool> ChangePassword(AppUserViewModel userVm, string newpass)
        {
            var user = await _userManager.FindByNameAsync(userVm.UserName);
            if (user == null) { /**/ }
            //if (model.smsCode != user.SmsCode) { /**/}

            // compute the new hash string
            var newPassword = _userManager.PasswordHasher.HashPassword(user, newpass);
            user.PasswordHash = newPassword;
            var res = await _userManager.UpdateAsync(user);

            if (res.Succeeded) {/**/}
            else { /**/}
            return true;
        }
        
        public async Task DeleteAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            await _userManager.DeleteAsync(user);
        }

        public async Task<List<AppUserViewModel>> GetAllAsync()
        {
            return await _userManager.Users.ProjectTo<AppUserViewModel>().ToListAsync();
        }

        public PagedResult<AppUserViewModel> GetAllPagingAsync(string keyword, int page, int pageSize)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.FullName.Contains(keyword)
                || x.UserName.Contains(keyword)
                || x.Email.Contains(keyword));

            int totalRow = query.Count();
            query = query.Skip((page - 1) * pageSize)
               .Take(pageSize);

            var data = query.Select(x => new AppUserViewModel()
            {
                UserName = x.UserName,
                Avatar = x.Avatar,
                BirthDay = x.BirthDay.ToString(),
                Email = x.Email,
                FullName = x.FullName,
                Id = x.Id,
                PhoneNumber = x.PhoneNumber,
                Status = x.Status,
                DateCreated = x.DateCreated
            }).ToList();
            var paginationSet = new PagedResult<AppUserViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public async Task<AppUserViewModel> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var roles = await _userManager.GetRolesAsync(user);
            var userVm = Mapper.Map<AppUser, AppUserViewModel>(user);
            userVm.Roles = roles;
            return userVm;
        }

        public AppUser GetByEmail(string email)
        {
            var query = (from u in _userManager.Users
                        where u.Email == email
                        select new AppUser()
                        {
                            Id = u.Id,
                            UserName = u.UserName,
                            Email = u.Email,
                            PhoneNumber = u.PhoneNumber,
                            FullName = u.FullName,                            
                            DateCreated = DateTime.Now,
                            Status = Infrastructure.Enums.Status.Actived
                        }).FirstOrDefault();                    
            return query;
        }

        public async Task UpdateAsync(AppUserViewModel userVm)
        {
            var user = await _userManager.FindByIdAsync(userVm.Id.ToString());
            //Remove current roles in db
            var currentRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.AddToRolesAsync(user,
                userVm.Roles.Except(currentRoles).ToArray());
            if (result.Succeeded)
            {
                string[] needRemoveRoles = currentRoles.Except(userVm.Roles).ToArray();
                await _userManager.RemoveFromRolesAsync(user, needRemoveRoles);

                //Update user detail
                user.FullName = userVm.FullName;
                user.Status = userVm.Status;
                user.Email = userVm.Email;
                user.PhoneNumber = userVm.PhoneNumber;
                await _userManager.UpdateAsync(user);
            }
        }

        public void MultiDelete(IList<string> selectedIds)
        {
            try
            {
                foreach (var item in selectedIds)
                {
                    var user = AppUsers.Where(r => r.Id == Guid.Parse(item)).FirstOrDefault();
                    AppUsers.Remove(user);
                }
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}