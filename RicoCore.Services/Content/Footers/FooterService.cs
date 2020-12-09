using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using RicoCore.Services.Content.Contacts.Dtos;
using RicoCore.Data.Entities;

using RicoCore.Infrastructure.Interfaces;
using RicoCore.Utilities.Dtos;
using RicoCore.Data.Entities.Content;
using RicoCore.Services.Content.Footers.Dtos;
using RicoCore.Services.Content.Footers;

namespace RicoCore.Services.Content.Contacts
{
    public class FooterService : WebServiceBase<Footer, string, FooterViewModel>, IFooterService
    {
        private readonly IRepository<Footer, string> _footerRepository;       

        public FooterService(IRepository<Footer, string> footerRepository,
            IUnitOfWork unitOfWork)
            : base(footerRepository, unitOfWork)
        {
            _footerRepository = footerRepository;           
        }

       
        public PagedResult<FooterViewModel> GetAllPaging(int page, int pageSize)
        {
            var query = _footerRepository.GetAll();           

            int totalRow = query.Count();
            var data = query.OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var paginationSet = new PagedResult<FooterViewModel>()
            {
                Results = data.ProjectTo<FooterViewModel>().ToList(),
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }       
    }
}