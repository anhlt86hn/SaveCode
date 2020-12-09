using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RicoCore.Services.Systems.Permissions.Dtos;
using RicoCore.Data.Entities;
using RicoCore.Infrastructure.Interfaces;
using RicoCore.Data.Entities.System;

namespace RicoCore.Services.Systems.Permissions
{
    public class PermissionService : WebServiceBase<Permission, int, PermissionViewModel>, IPermissionService
    {
        private readonly IRepository<Function, int> _functionRepository;
        private readonly IRepository<Permission, int> _permissionRepository;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        //private readonly IUnitOfWork _unitOfWork;

        public PermissionService(IRepository<Permission, int> permissionRepository,
              RoleManager<AppRole> roleManager,
              UserManager<AppUser> userManager,
            IRepository<Function, int> functionRepository, IUnitOfWork unitOfWork)
            : base(permissionRepository, unitOfWork)
        {
            _permissionRepository = permissionRepository;
            _functionRepository = functionRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            //_unitOfWork = unitOfWork;
        }      

        public void DeleteAll(int functionId)
        {
            _permissionRepository.Delete(x => x.FunctionId == functionId);
        }

        public ICollection<PermissionViewModel> GetByFunctionId(int functionId)
        {
            return _permissionRepository
                .GetAll().Where(x => x.FunctionId == functionId)
                .ProjectTo<PermissionViewModel>().ToList();
        }

        public async Task<List<PermissionViewModel>> GetByUserId(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var roles = await _userManager.GetRolesAsync(user);

            var query = (from f in _functionRepository.GetAll()
                         join p in _permissionRepository.GetAll() on f.Id equals p.FunctionId
                         join r in _roleManager.Roles on p.RoleId equals r.Id
                         where roles.Contains(r.Name)
                         select p);

            return query.ProjectTo<PermissionViewModel>().ToList();
        }    
       
    }
}