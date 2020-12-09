using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using RicoCore.Services.Content.Slides.Dtos;
using RicoCore.Data.Entities;
using RicoCore.Infrastructure.Interfaces;
using RicoCore.Utilities.Dtos;
using RicoCore.Data.Entities.Content;
using Microsoft.EntityFrameworkCore;
using RicoCore.Data.EF;
using RicoCore.Utilities.Helpers;
using RicoCore.Data.Entities.System;
using RicoCore.Services.Systems.Logos.Dtos;

namespace RicoCore.Services.Systems.Logos
{
    public class LogoService : WebServiceBase<Logo, int, LogoViewModel>, ILogoService
    {
        private readonly IRepository<Logo, int> _logoRepository;
        private readonly DbSet<Logo> Logos;
        private readonly IUnitOfWork _unitOfWork;
        public LogoService(IRepository<Logo, int> logoRepository,
            AppDbContext context,
            IUnitOfWork unitOfWork) : base(logoRepository, unitOfWork)
        {
            _logoRepository = logoRepository;
            Logos = context.Set<Logo>();
            _unitOfWork = unitOfWork;
        }      

        public bool ValidateAddSortOrder(LogoViewModel vm)
        {
            return _logoRepository.GetAll().Any(x => x.SortOrder == vm.SortOrder && x.SortOrder != 0);
        }       

        public bool ValidateUpdateSortOrder(LogoViewModel vm)
        {
            var compare = _logoRepository.GetAllIncluding(x => x.SortOrder == vm.SortOrder && x.SortOrder != 0);
            var result = compare.Count() > 1 ? true : false;
            return result;
        }

        public int SetNewOrder()
        {
            var order = 0;
            var list = _logoRepository.GetAllIncluding(x => x.SortOrder != 0).OrderBy(x => x.SortOrder).Select(x => x.SortOrder).ToList();
            order = CommonMethods.GetOrder(list);
            return order;
        }        

        public void MultiDelete(IList<string> selectedIds)
        {
            foreach (var item in selectedIds)
            {
                var slide = Logos.Where(r => r.Id == int.Parse(item)).FirstOrDefault();
                Logos.Remove(slide);
            }
            _unitOfWork.Commit();
        }

        public PagedResult<LogoViewModel> GetAllPaging(int page, int pageSize, string sortBy)
        {
            var query = _logoRepository.GetAll();
           
            int totalRow = query.Count();

            query = query.OrderByDescending(x => x.Id).Skip((page - 1) * pageSize)
                .Take(pageSize);

            var data = query.ProjectTo<LogoViewModel>().ToList();
            var paginationSet = new PagedResult<LogoViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }       
    }
}