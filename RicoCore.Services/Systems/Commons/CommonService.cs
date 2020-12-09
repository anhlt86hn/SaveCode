using AutoMapper;
using AutoMapper.QueryableExtensions;
using RicoCore.Data.Entities.Content;
using RicoCore.Data.Entities.System;
using RicoCore.Data.Enums;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.Interfaces;
using RicoCore.Services.Content.Footers.Dtos;
using RicoCore.Services.Content.Slides.Dtos;
using RicoCore.Services.Systems.Settings.Dtos;
using RicoCore.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RicoCore.Services.Systems.Commons
{
    public class CommonService : ICommonService
    {
        private readonly IRepository<Footer, string> _footerRepository;
        private readonly IRepository<Setting, Guid> _systemConfigRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Slide, int> _slideRepository;

        public CommonService(IRepository<Footer, string> footerRepository,
            IRepository<Setting, Guid> systemConfigRepository,
            IUnitOfWork unitOfWork,
            IRepository<Slide, int> slideRepository)
        {
            _footerRepository = footerRepository;
            _unitOfWork = unitOfWork;
            _systemConfigRepository = systemConfigRepository;
            _slideRepository = slideRepository;
        }

        public FooterViewModel GetFooter()
        {
            return Mapper.Map<Footer, FooterViewModel>(_footerRepository.Single(x => x.Id ==
            CommonConstants.DefaultFooterId));
        }

        public List<SlideViewModel> GetSlides(SlideGroup groupAlias)
        {
            return _slideRepository.GetAll().Where(x => x.Status == Status.Actived && x.GroupAlias == groupAlias).OrderBy(x => x.SortOrder)
                .ProjectTo<SlideViewModel>().ToList();
        }

        public SystemConfigViewModel GetSystemConfig(string code)
        {
            return Mapper.Map<Setting, SystemConfigViewModel>(_systemConfigRepository.Single(x => x.UniqueCode == code));
        }
    }
}