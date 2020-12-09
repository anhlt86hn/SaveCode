using System;
using System.Collections.Generic;
using RicoCore.Data.Entities.Content;
using RicoCore.Services.Content.Slides.Dtos;
using RicoCore.Utilities.Dtos;

namespace RicoCore.Services.Content.Slides
{
    public interface ISlideService : IWebServiceBase<Slide, int, SlideViewModel>
    {      
        PagedResult<SlideViewModel> GetAllPaging(string keyword, string position, int page, int pageSize, string sortBy);

        void MultiDelete(IList<string> selectedIds);
        bool ValidateAddSlideName(SlideViewModel vm);
        bool ValidateAddSortOrder(SlideViewModel vm, int slideGroup);

        bool ValidateUpdateSlideName(SlideViewModel vm);
        bool ValidateUpdateSortOrder(SlideViewModel vm, int slideGroup);
        int SetNewSlideOrder(int slideGroup);
    }
}