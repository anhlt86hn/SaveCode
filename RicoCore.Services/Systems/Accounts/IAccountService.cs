using RicoCore.Data.Entities.Content;
using RicoCore.Data.Entities.System;
using RicoCore.Services.Content.Posts.Dtos;
using RicoCore.Services.Content.Tags.Dtos;
using RicoCore.Services.Systems.Accounts.Dtos;
using RicoCore.Utilities.Dtos;
using System;
using System.Collections.Generic;

namespace RicoCore.Services.Systems.Accounts
{
    public interface IAccountService : IWebServiceBase<RicoCore.Data.Entities.System.Account, int, AccountViewModel>
    {
        bool ValidateAddAccountOrder(AccountViewModel accountVm);
        bool ValidateUpdateAccountOrder(AccountViewModel accountVm);
        //AccountViewModel SetValueToNewPost(int postCategoryId);
        int SetNewOrder();
        PagedResult<AccountViewModel> GetAllPaging(string keyword, int page, int pageSize, string sortBy);
        void MultiDelete(IList<string> selectedIds);
        void ImportExcel(string filePath);
        List<AccountExportViewModel> GetAllToExport();
    }
}