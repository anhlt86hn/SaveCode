using AutoMapper;
using AutoMapper.QueryableExtensions;
using RicoCore.Data.Entities.Content;
using RicoCore.Data.Entities.System;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.Interfaces;
using RicoCore.Infrastructure.SharedKernel;
using RicoCore.Services.Systems.Menus.Dtos;
using RicoCore.Utilities.Dtos;
using RicoCore.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RicoCore.Services.Systems.Menus
{
    public class MenuService : WebServiceBase<Menu, int, MenuViewModel>, IMenuService
    {
        private readonly IRepository<Menu, int> _menuRepository;
        //private readonly IUnitOfWork _unitOfWork;

        public MenuService(IRepository<Menu, int> menuRepository,          
           IUnitOfWork unitOfWork) : base(menuRepository, unitOfWork)
        {
            _menuRepository = menuRepository;           
            //_unitOfWork = unitOfWork;
        }

        public MenuViewModel GetByUrl(string url)
        {
            var menu = _menuRepository.FirstOrDefault(x => x.Url == url);
            if (menu == null) throw new Exception("Không để rỗng Menu");

            var vm = Mapper.Map<Menu, MenuViewModel>(menu);
            return vm;
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
                var menu = _menuRepository.GetById(parentId);
                name = menu.Name;
            }
            return name;
        }


        public bool ValidateAddMenuName(MenuViewModel menuVm)
        {
            return _menuRepository.GetAll().Any(x => x.Name.ToLower() == menuVm.Name.ToLower());
        }
        public bool ValidateUpdateMenuName(MenuViewModel menuVm)
        {
            var compare = _menuRepository.GetAllIncluding(x => x.Name.ToLower() == menuVm.Name.ToLower());
            var result = compare.Count() > 1 ? true : false;
            return result;
        }

        public bool ValidateAddMenuOrder(MenuViewModel menuVm)
        {
            var level = menuVm.ParentId;
            if(level.HasValue && level > 0)
            {
                var list = _menuRepository.GetAllIncluding(x => x.ParentId == level && x.SortOrder == menuVm.SortOrder);
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

        public bool ValidateUpdateMenuOrder(MenuViewModel menuVm)
        {
            var level = menuVm.ParentId;
            if(level.HasValue && level > 0)
            {
                var list = _menuRepository.GetAllIncluding(x => x.ParentId == level && x.SortOrder == menuVm.SortOrder);
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

       
        public MenuViewModel SetValueToNewMenu(int parentId)
        {
            var menu = new MenuViewModel();
            if (parentId == 0)
            {
                menu.ParentId = null;
            }
            else
            {
                menu.ParentId = parentId;
            }            
            menu.ParentName = GetNameById(parentId);
            menu.SortOrder = SetNewMenuOrder(parentId);          
            return menu;
        }
      
        public int SetNewMenuOrder(int? parentId)
        {
            var order = 0;
            var list = parentId == 0
                ? _menuRepository.GetAllIncluding(x => x.ParentId == null).OrderBy(x => x.SortOrder).Select(x => x.SortOrder).ToList()
                : _menuRepository.GetAllIncluding(x => x.ParentId == parentId).OrderBy(x => x.SortOrder).Select(x => x.SortOrder).ToList();

            order = CommonMethods.GetOrder(list);
            return order;
        }

       

        #region Add

        public override void Add(MenuViewModel menuVm)
        {
            var menu = Mapper.Map<MenuViewModel, Menu>(menuVm);                      
            _menuRepository.Insert(menu);
        }

        #endregion Add

        #region Update

        public override void Update(MenuViewModel menuVm)
        {
            var menu = _menuRepository.GetById(menuVm.Id);                     
            Mapper.Map(menuVm, menu);           
            _menuRepository.Update(menu);
        }

        #endregion Update

        #region Delete

        public override void Delete(int id)
        {
            _menuRepository.Delete(id);
        }

        #endregion Delete

        #region GetById

        public override MenuViewModel GetById(int id)
        {  
            var menu = (from pcat in _menuRepository.GetAll() where pcat.Id == id select new { pcat }).FirstOrDefault();

            var parentName = menu != null && menu.pcat.ParentId != null
                    ? _menuRepository.GetAllIncluding(x => x.Id == menu.pcat.ParentId).Select(x => x.Name).FirstOrDefault()
                    : "Không có";

            //var productlist = query.Select(x => new PostViewModel()         
            //}).ToList();

            var model = new MenuViewModel()
            {
                Name = menu.pcat.Name,               
                Id = menu.pcat.Id,
                ParentId = menu.pcat.ParentId,
                ParentName = parentName,
                Url = menu.pcat.Url,              
                SortOrder = menu.pcat.SortOrder,               
                Status = menu.pcat.Status
            };

            return model;
        }  

        #endregion GetById

        #region GetAll

        public override List<MenuViewModel> GetAll()
        {
            try
            {
                //var menuList = _menuRepository.GetAll();
                return _menuRepository.GetAll().OrderBy(x => x.Id)
                //return menuList
                .ProjectTo<MenuViewModel>()
                .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<MenuViewModel> GetAll(string keyword)
        {
            if (!string.IsNullOrWhiteSpace(keyword))
                return _menuRepository.GetAll().Where(x => x.Name.Contains(keyword)
                || x.Url.Contains(keyword))
                    .OrderBy(x => x.ParentId).ProjectTo<MenuViewModel>().ToList();
            return _menuRepository.GetAll().OrderBy(x => x.Id)
                .ProjectTo<MenuViewModel>()
                .ToList();
        }

        #endregion GetAll

        #region GetAllByParentId

        public List<MenuViewModel> GetAllByParentId(int? parentId)
        {
            return _menuRepository.GetAll().Where(x => x.Status == Status.Actived && x.ParentId == parentId)
                .ProjectTo<MenuViewModel>()
                .ToList();
        }

        #endregion GetAllByParentId

        #region GetAllPaging

        public PagedResult<MenuViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            var query = _menuRepository.GetAll();
            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(x => x.Name.Contains(keyword));
            int totalRow = query.Count();
            var data = query.OrderByDescending(x => x.SortOrder)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
            var paginationSet = new PagedResult<MenuViewModel>()
            {
                Results = data.ProjectTo<MenuViewModel>().ToList(),
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize,
            };
            return paginationSet;
        }

        public PagedResult<MenuViewModel> GetAllPagingAsync(string keyword, int page, int pageSize)
        {
            var query = _menuRepository.GetAll();
            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(x => x.Name.Contains(keyword)
                || x.Url.Contains(keyword));

            int totalRow = query.Count();
            query = query.Skip((page - 1) * pageSize)
               .Take(pageSize);

            var data = query.ProjectTo<MenuViewModel>().ToList();
            var paginationSet = new PagedResult<MenuViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        #endregion GetAllPaging

        

        #region UpdateParentId

        public void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            //Update parent id for source
            var category = _menuRepository.GetById(sourceId);
            category.ParentId = targetId;
            _menuRepository.Update(category);

            //Get all sibling
            var sibling = _menuRepository.GetAll().Where(x => items.ContainsKey(x.Id));
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];
                _menuRepository.Update(child);
            }
        }

        #endregion UpdateParentId

        #region ReOrder

        public void ReOrder(int sourceId, int targetId)
        {
            var source = _menuRepository.GetById(sourceId);
            var target = _menuRepository.GetById(targetId);
            int tempOrder = source.SortOrder;

            source.SortOrder = target.SortOrder;
            target.SortOrder = tempOrder;

            _menuRepository.Update(source);
            _menuRepository.Update(target);
        }

        #endregion ReOrder

        public List<Menu> AllSubCategories(int id)
        {
            return _menuRepository.GetAllIncluding(x => x.ParentId == id).ToList();
        }
      
    }
}