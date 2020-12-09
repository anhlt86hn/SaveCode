using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RicoCore.Services.Systems.Functions.Dtos;
using RicoCore.Data.Entities;
using RicoCore.Data.Entities.System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RicoCore.Services.Systems.Functions
{
    public interface IFunctionService : IWebServiceBase<Function, int, FunctionViewModel>
    {
        int SetNewOrder(int? parentId);
        string GetNameById(int parentId);
        bool ValidateAddUniqueCode(FunctionViewModel menuVm);
        bool ValidateUpdateUniqueCode(FunctionViewModel menuVm);
        bool ValidateAddOrder(FunctionViewModel menuVm);
        bool ValidateUpdateOrder(FunctionViewModel menuVm);
        FunctionViewModel SetValueToNewFunction(int parentId);
        IList<SelectListItem> FunctionsSelectListItem(int id = 0);

        List<Function> AllSubCategories(int id);
        Task<List<FunctionViewModel>> GetAll(string filter);

        Task<List<FunctionViewModel>> GetAllWithPermission(string userName);

        List<FunctionViewModel> GetAllWithParentId(int? parentId);

        bool CheckExistedId(int id);

        void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items);

        void ReOrder(int sourceId, int targetId);
    }
}