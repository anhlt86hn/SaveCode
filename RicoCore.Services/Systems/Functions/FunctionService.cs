using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RicoCore.Services.Systems.Functions.Dtos;
using RicoCore.Data.Entities;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.Interfaces;
using RicoCore.Data.Entities.System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RicoCore.Services.Systems.Functions
{
    public class FunctionService : WebServiceBase<Function, int, FunctionViewModel>, IFunctionService
    {
        private readonly IRepository<Function, int> _functionRepository;
        private readonly IRepository<Permission, int> _permissionRepository;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        //private readonly IMapper _mapper;

        public FunctionService(IMapper mapper,
             RoleManager<AppRole> roleManager,
              UserManager<AppUser> userManager,
             IRepository<Permission, int> permissionRepository,
            IRepository<Function, int> functionRepository,
            IUnitOfWork unitOfWork) : base(functionRepository, unitOfWork)
        {
            _functionRepository = functionRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _permissionRepository = permissionRepository;
            //_mapper = mapper;
        }

        public string GetNameById(int parentId)
        {
            var name = string.Empty;
            if (parentId == 0)
            {
                name = "Không có";
            }
            else
            {
                var menu = _functionRepository.GetById(parentId);
                name = menu.Name;
            }
            return name;
        }

        public int SetNewOrder(int? parentId)
        {
            var order = 0;
            var list = parentId == 0
                ? _functionRepository.GetAllIncluding(x => x.ParentId == null).OrderBy(x => x.SortOrder).Select(x => x.SortOrder).ToList()
                : _functionRepository.GetAllIncluding(x => x.ParentId == parentId).OrderBy(x => x.SortOrder).Select(x => x.SortOrder).ToList();

            order = CommonMethods.GetOrder(list);
            return order;
        }

        public bool ValidateAddUniqueCode(FunctionViewModel functionVm)
        {
            return _functionRepository.GetAll().Any(x => x.UniqueCode.ToLower() == functionVm.UniqueCode.ToLower());
        }
        public bool ValidateUpdateUniqueCode(FunctionViewModel functionVm)
        {
            var compare = _functionRepository.GetAllIncluding(x => x.UniqueCode.ToLower() == functionVm.UniqueCode.ToLower());
            var result = compare.Count() > 1 ? true : false;
            return result;
        }

        public bool ValidateAddOrder(FunctionViewModel functionVm)
        {
            var level = functionVm.ParentId;
            if (level.HasValue && level > 0)
            {
                var list = _functionRepository.GetAllIncluding(x => x.ParentId == level && x.SortOrder == functionVm.SortOrder);
                if (list.Count() > 0)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public bool ValidateUpdateOrder(FunctionViewModel functionVm)
        {
            var level = functionVm.ParentId;
            if (level.HasValue && level > 0)
            {
                var list = _functionRepository.GetAllIncluding(x => x.ParentId == level && x.SortOrder == functionVm.SortOrder);
                if (list.Count() > 1)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }


        public FunctionViewModel SetValueToNewFunction(int parentId)
        {
            var function = new FunctionViewModel
            {
                ParentId = parentId,
                ParentName = GetNameById(parentId),
                SortOrder = SetNewOrder(parentId)
            };
            return function;
        }
      
        public IList<SelectListItem> FunctionsSelectListItem(int id = 0)
        {
            var functions = _functionRepository.GetAll();
            var function = _functionRepository.GetById(id);
            var functionParentId = function == null ? 0 : function.ParentId;
            var list = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Text = "Cấp 1",
                    Value = "0",
                    Selected = functionParentId == null ? true : false
                }
            };

            var parents = id == 0 ? _functionRepository.GetAllIncluding(x => x.ParentId == null) : _functionRepository.GetAllIncluding(x => x.ParentId == null && x.Id != id);            
            foreach (var parent in parents)
            {
                list.Add(new SelectListItem()
                {
                    Value = parent.Id.ToString(),
                    Text = $"{parent.Name}",
                    Selected = parent.Id == functionParentId ? true : false
                });

                //var children = id == 0 ? functions.Where(x => x.ParentId == parent.Id) : functions.Where(x => x.ParentId == parent.Id && x.Id != id);

                //foreach (var child in children)
                //{
                //    list.Add(new SelectListItem()
                //    {
                //        Value = child.Id.ToString(),
                //        Text = $"----------------{child.Name}",
                //        Selected = child.Id == functionParentId ? true : false
                //    });
                //}                
            }
            return list;
        }


        #region GetById

        public override FunctionViewModel GetById(int id)
        {
            var function = (from pcat in _functionRepository.GetAll() where pcat.Id == id select new { pcat }).FirstOrDefault();

            var parentName = function != null && function.pcat.ParentId != null
                    ? _functionRepository.GetAllIncluding(x => x.Id == function.pcat.ParentId).Select(x => x.Name).FirstOrDefault()
                    : "Không có";

            //var productlist = query.Select(x => new PostViewModel()         
            //}).ToList();

            var model = new FunctionViewModel()
            {
                Name = function.pcat.Name,
                Id = function.pcat.Id,
                ParentId = function.pcat.ParentId,
                ParentName = parentName,
                Url = function.pcat.Url,
                SortOrder = function.pcat.SortOrder,
                IsActive = function.pcat.IsActive,
                Status = function.pcat.Status,
                CssClass = function.pcat.CssClass,
                UniqueCode = function.pcat.UniqueCode
            };

            return model;
        }

        #endregion GetById

        #region Add

        public override void Add(FunctionViewModel functionVm)
        {
            var function = Mapper.Map<FunctionViewModel, Function>(functionVm);
            _functionRepository.Insert(function);
        }

        #endregion Add

        #region Update

        public override void Update(FunctionViewModel functionVm)
        {
            functionVm.Status = functionVm.IsActive ? Status.Actived : Status.InActived;
            var function = _functionRepository.GetById(functionVm.Id);
            Mapper.Map(functionVm, function);
            _functionRepository.Update(function);
        }
        #endregion

        public List<Function> AllSubCategories(int id)
        {
            return _functionRepository.GetAllIncluding(x => x.ParentId == id).ToList();
        }

        public bool CheckExistedId(int id)
        {
            return _functionRepository.GetById(id) != null;
        }

        public Task<List<FunctionViewModel>> GetAll(string filter)
        {
            var query = _functionRepository.GetAll();
            if (!string.IsNullOrEmpty(filter))
                query = query.Where(x => x.Name.Contains(filter));           
            return query.OrderBy(x => x.ParentId).ProjectTo<FunctionViewModel>().ToListAsync();           
        }

        public List<FunctionViewModel> GetAllWithParentId(int? parentId)
        {
            return _functionRepository.GetAll()
                .Where(x => x.ParentId == parentId)
                .ProjectTo<FunctionViewModel>().ToList();
        }

        public async Task<List<FunctionViewModel>> GetAllWithPermission(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            var query = (from f in _functionRepository.GetAll()
                         join p in _permissionRepository.GetAll() on f.Id equals p.FunctionId
                         join r in _roleManager.Roles on p.RoleId equals r.Id
                         where roles.Contains(r.Name)
                         select f);

            var parentIds = query.Select(x => x.ParentId).Distinct();

            query = query.Union(_functionRepository.GetAll().Where(f => parentIds.Contains(f.Id)));

            return await query.OrderBy(x => x.ParentId).ProjectTo<FunctionViewModel>().ToListAsync();
        }

        public void ReOrder(int sourceId, int targetId)
        {
            var source = _functionRepository.GetById(sourceId);
            var target = _functionRepository.GetById(targetId);
            int tempOrder = source.SortOrder;

            source.SortOrder = target.SortOrder;
            target.SortOrder = tempOrder;

            _functionRepository.Update(source);
            _functionRepository.Update(target);
        }

        public void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            //Update parent id for source
            var category = _functionRepository.GetById(sourceId);
            category.ParentId = targetId;
            _functionRepository.Update(category);

            //Get all sibling
            var sibling = _functionRepository.GetAll().Where(x => items.ContainsKey(x.Id));
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];
                _functionRepository.Update(child);
            }
        }
    }
}