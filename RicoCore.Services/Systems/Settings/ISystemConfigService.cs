using RicoCore.Data.Entities.System;
using RicoCore.Services.Systems.Settings.Dtos;
using RicoCore.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RicoCore.Services.Systems.Settings
{
    public interface ISystemConfigService : IWebServiceBase<Setting, Guid, SystemConfigViewModel>
    {
        PagedResult<SystemConfigViewModel> GetAllPaging(string keyword, int page, int pageSize);
        void MultiDelete(IList<string> selectedIds);
    }
}
