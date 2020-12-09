using System;
using System.Collections.Generic;
using RicoCore.Data.Entities.Content;
using RicoCore.Data.Entities.System;
using RicoCore.Services.Content.Slides.Dtos;
using RicoCore.Services.Systems.Logos.Dtos;
using RicoCore.Utilities.Dtos;

namespace RicoCore.Services.Systems.Logos
{
    public interface ILogoService : IWebServiceBase<Logo, int, LogoViewModel>
    {      
        PagedResult<LogoViewModel> GetAllPaging(int page, int pageSize, string sortBy);

        void MultiDelete(IList<string> selectedIds);     
        bool ValidateAddSortOrder(LogoViewModel vm);    
        bool ValidateUpdateSortOrder(LogoViewModel vm);
        int SetNewOrder();
    }
}