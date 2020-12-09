using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using RicoCore.Services.Content.Pages;
using RicoCore.Services.Content.Pages.Dtos;
using RicoCore.Data.Entities;
using RicoCore.Infrastructure.Interfaces;
using RicoCore.Utilities.Dtos;
using RicoCore.Data.Entities.Content;
using RicoCore.Infrastructure.SharedKernel;
using RicoCore.Utilities.Helpers;

namespace RicoCore.Services.Content.Pages
{
    public class PageService : WebServiceBase<Page, Guid, PageViewModel>, IPageService
    {
        private readonly IRepository<Page, Guid> _pageRepository;        

        public PageService(IRepository<Page, Guid> pageRepository,
            IUnitOfWork unitOfWork)
            : base (pageRepository, unitOfWork)
        {
            _pageRepository = pageRepository;
        }

        public virtual bool HasExistsCode(string code)
        {
            return _pageRepository.GetAll().Any(x => x.UniqueCode == code);
        }

        private string GenerateCode()
        {
            var code = CommonHelper.GenerateRandomCode();
            while (HasExistsCode(code))
            {
                code = CommonHelper.GenerateRandomCode();
            }
            return code;
        }

        public override void Add(PageViewModel pageVm)
        {
            var page = Mapper.Map<PageViewModel, Page>(pageVm);
            if (string.IsNullOrWhiteSpace(pageVm.Url))
                page.Url = TextHelper.ToUnsignString(pageVm.Name.Trim());
            else
                page.Url = pageVm.Url.ToLower();
            page.UniqueCode = GenerateCode();
            var query = _pageRepository.FirstOrDefault(x => x.Url == page.Url);
            if (query != null)
            {
                page.Url = $"{query.Url}-{page.UniqueCode.ToLower()}";
            }
            _pageRepository.Insert(page);
        }

        public override void Update(PageViewModel pageVm)
        {
            var page = _pageRepository.GetById(pageVm.Id);
            var createdDate = page.DateCreated;
            if (string.IsNullOrWhiteSpace(pageVm.Url))
                page.Url = TextHelper.ToUnsignString(pageVm.Name.Trim());
            else
                page.Url = pageVm.Url.ToLower();
            var query = _pageRepository.FirstOrDefault(x => x.Url == page.Url);
            if (query != null)
            {
                page.Url = $"{query.Url}-{page.UniqueCode.ToLower()}";
            }
            Mapper.Map(pageVm, page);
            page.DateCreated = createdDate;
            _pageRepository.Update(page);
        }
        public PagedResult<PageViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            var query = _pageRepository.GetAll();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword));

            int totalRow = query.Count();
            var data = query.OrderByDescending(x => x.UniqueCode)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var paginationSet = new PagedResult<PageViewModel>()
            {
                Results = data.ProjectTo<PageViewModel>().ToList(),
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public PageViewModel GetByAlias(string alias)
        {
            return Mapper.Map<Page, PageViewModel>(_pageRepository.Single(x => x.Url == alias));
        }       
    }
}