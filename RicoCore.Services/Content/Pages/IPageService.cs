using System;
using System.Collections.Generic;
using RicoCore.Data.Entities.Content;
using RicoCore.Services.Content.Pages.Dtos;
using RicoCore.Utilities.Dtos;

namespace RicoCore.Services.Content.Pages
{
    public interface IPageService : IWebServiceBase<Page, Guid, PageViewModel>
    {       
        PagedResult<PageViewModel> GetAllPaging(string keyword, int page, int pageSize);

        PageViewModel GetByAlias(string alias);
        bool HasExistsCode(string code);
    }
}