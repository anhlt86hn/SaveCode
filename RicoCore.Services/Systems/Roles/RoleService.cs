﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RicoCore.Services.Systems.Permissions.Dtos;
using RicoCore.Services.Systems.Roles.Dtos;
using RicoCore.Data.Entities;
using RicoCore.Infrastructure.Interfaces;
using RicoCore.Utilities.Dtos;
using RicoCore.Data.Entities.System;
using RicoCore.Services.Systems.Announcements.Dtos;
using RicoCore.Data.Entities.ECommerce;
using RicoCore.Data.EF;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace RicoCore.Services.Systems.Roles
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IRepository<Function, int> _functionRepository;
        private readonly IRepository<Permission, int> _permissionRepository;
        private readonly IRepository<Announcement, Guid> _announRepository;
        private readonly IRepository<AnnouncementUser, Guid> _announUserRepository;        
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<AppRole> AppRoles;

        public RoleService(RoleManager<AppRole> roleManager,
            IRepository<Function, int> functionRepository,
            IRepository<Permission, int> permissionRepository,
            IRepository<Announcement, Guid> announRepository,
            IRepository<AnnouncementUser, Guid> announUserRepository,
            IUnitOfWork unitOfWork,
            AppDbContext context)
        {
            _roleManager = roleManager;
            _functionRepository = functionRepository;
            _permissionRepository = permissionRepository;
            _announRepository = announRepository;
            _unitOfWork = unitOfWork;
            _announUserRepository = announUserRepository;
            AppRoles = context.Set<AppRole>();
        }

        public async Task<bool> AddAsync(AnnouncementViewModel announcementVm,
            List<AnnouncementUserViewModel> announcementUsers,
            AppRoleViewModel roleVm)
        {
            var role = new AppRole()
            {
                //Id = Guid.NewGuid(),
                Name = roleVm.Name,
                Description = roleVm.Description
            };
            var result = await _roleManager.CreateAsync(role);
            var announcement = Mapper.Map<AnnouncementViewModel, Announcement>(announcementVm);
            _announRepository.Insert(announcement);
            foreach (var userVm in announcementUsers)
            {
                var user = Mapper.Map<AnnouncementUserViewModel, AnnouncementUser>(userVm);
                _announUserRepository.Insert(user);
            }
            _unitOfWork.Commit();
            return result.Succeeded;
        }

        public async Task UpdateAsync(AppRoleViewModel roleVm)
        {
            var role = await _roleManager.FindByIdAsync(roleVm.Id.ToString());
            role.Description = roleVm.Description;
            role.Name = roleVm.Name;
            await _roleManager.UpdateAsync(role);
        }

        public async Task DeleteAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            await _roleManager.DeleteAsync(role);
        }

        public async Task<AppRoleViewModel> GetById(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            return Mapper.Map<AppRole, AppRoleViewModel>(role);
        }

        public async Task<List<AppRoleViewModel>> GetAllAsync()
        {
            return await _roleManager.Roles.ProjectTo<AppRoleViewModel>().ToListAsync();
        }

        public PagedResult<AppRoleViewModel> GetAllPagingAsync(string keyword, int page, int pageSize)
        {
            var query = _roleManager.Roles;
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword)
                || x.Description.Contains(keyword));

            int totalRow = query.Count();
            query = query.Skip((page - 1) * pageSize)
               .Take(pageSize);

            var data = query.ProjectTo<AppRoleViewModel>().ToList();
            var paginationSet = new PagedResult<AppRoleViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public Task<bool> CheckPermission(string functionCode, string action, string[] roles)
        {
            var functions = _functionRepository.GetAll();
            var permissions = _permissionRepository.GetAll();
            var query = from f in functions
                        join p in permissions on f.Id equals p.FunctionId
                        join r in _roleManager.Roles on p.RoleId equals r.Id
                        where roles.Contains(r.Name) && f.UniqueCode == functionCode
                        && ((p.CanCreate && action == "Create")
                        || (p.CanUpdate && action == "Update")
                        || (p.CanDelete && action == "Delete")
                        || (p.CanRead && action == "Read"))
                        select p;
            return query.AnyAsync();
        }          

        //public IQueryable<AppRoleViewModel> GetAll()
        //{
        //    return _roleManager.Roles.ProjectTo<AppRoleViewModel>();
        //}
       
        public List<PermissionViewModel> GetListFunctionWithRole(Guid roleId)
        {
            var functions = _functionRepository.GetAll();
            var permissions = _permissionRepository.GetAll();

            var query = from f in functions
                        join p in permissions on f.Id equals p.FunctionId into fp
                        from p in fp.DefaultIfEmpty()
                        where p != null && p.RoleId == roleId
                        select new PermissionViewModel()
                        {
                            RoleId = roleId,
                            FunctionId = f.Id,
                            CanCreate = p != null ? p.CanCreate : false,
                            CanDelete = p != null ? p.CanDelete : false,
                            CanRead = p != null ? p.CanRead : false,
                            CanUpdate = p != null ? p.CanUpdate : false
                        };
            return query.ToList();
        }

        public void SavePermission(List<PermissionViewModel> permissionVms, Guid roleId)
        {
            var permissions = Mapper.Map<List<PermissionViewModel>, List<Permission>>(permissionVms);
            var oldPermission = _permissionRepository.GetAll().ToList();
            if (oldPermission.Count > 0)
            {
                _permissionRepository.Delete(x => x.RoleId == roleId);
            }
            foreach (var permission in permissions)
            {
                _permissionRepository.Insert(permission);
            }
            _unitOfWork.Commit();
        }

        public void MultiDelete(IList<string> selectedIds)
        {                         
                foreach (var item in selectedIds)
                {
                    var role =  AppRoles.Where(r => r.Id == Guid.Parse(item)).FirstOrDefault();
                    AppRoles.Remove(role);                    
                }
                _unitOfWork.Commit();                              
        }       
    }
}