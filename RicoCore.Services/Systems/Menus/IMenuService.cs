using System;
using RicoCore.Services.Content.PostCategories.Dtos;
using RicoCore.Data.Entities.Content;
using RicoCore.Utilities.Dtos;
using System.Collections.Generic;
using RicoCore.Data.Entities.System;
using RicoCore.Services.Systems.Menus.Dtos;

namespace RicoCore.Services.Systems.Menus
{
    public interface IMenuService : IWebServiceBase<Menu, int, MenuViewModel>
    {
        //void Add(ProductCategoryViewModel productCategoryVm);
        //void Update(ProductCategoryViewModel productCategoryVm);
        //void Delete(Guid id);
        //ProductCategoryViewModel GetById(Guid id);
        //List<MenuViewModel> GetAll();

        bool ValidateAddMenuName(MenuViewModel menuVm);
        bool ValidateUpdateMenuName(MenuViewModel menuVm);

        bool ValidateAddMenuOrder(MenuViewModel menuVm);
        bool ValidateUpdateMenuOrder(MenuViewModel menuVm);
        MenuViewModel SetValueToNewMenu(int parentId);
        int SetNewMenuOrder(int? parentId);

        string GetNameById(int parentId);        
        

        List<MenuViewModel> GetAll(string keyword);       

        List<MenuViewModel> GetAllByParentId(int? parentId);

        PagedResult<MenuViewModel> GetAllPaging(string keyword, int page, int pageSize);
        PagedResult<MenuViewModel> GetAllPagingAsync(string keyword, int page, int pageSize);
        
        void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items);

        void ReOrder(int sourceId, int targetId);
        List<Menu> AllSubCategories(int id);
       
        MenuViewModel GetByUrl(string url);
    }
}