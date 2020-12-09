using RicoCore.Data.Enums;
using RicoCore.Services.Content.Footers.Dtos;
using RicoCore.Services.Content.Slides.Dtos;
using RicoCore.Services.Systems.Settings.Dtos;
using System.Collections.Generic;

namespace RicoCore.Services.Systems.Commons
{
    public interface ICommonService
    {
        FooterViewModel GetFooter();

        SystemConfigViewModel GetSystemConfig(string code);

        List<SlideViewModel> GetSlides(SlideGroup groupAlias);
    }
}