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

namespace RicoCore.Services.Content.Slides
{
    public class SlideService : WebServiceBase<Slide, int, SlideViewModel>, ISlideService
    {
        private readonly IRepository<Slide, int> _slideRepository;
        private readonly DbSet<Slide> Slides;
        private readonly IUnitOfWork _unitOfWork;
        public SlideService(IRepository<Slide, int> slideRepository,
            AppDbContext context,
            IUnitOfWork unitOfWork) : base(slideRepository, unitOfWork)
        {
            _slideRepository = slideRepository;
            Slides = context.Set<Slide>();
            _unitOfWork = unitOfWork;
        }

        public bool ValidateAddSlideName(SlideViewModel vm)
        {
            return _slideRepository.GetAll().Any(x => x.Name.ToLower() == vm.Name.ToLower());
        }

        public bool ValidateAddSortOrder(SlideViewModel vm, int slideGroup)
        {
            return _slideRepository.GetAll().Any(x => x.SortOrder == vm.SortOrder && (int) x.GroupAlias == slideGroup);
        }

        public bool ValidateUpdateSlideName(SlideViewModel vm)
        {
            var compare = _slideRepository.GetAllIncluding(x => x.Name.ToLower() == vm.Name.ToLower());
            var result = compare.Count() > 1 ? true : false;
            return result;
        }

        public bool ValidateUpdateSortOrder(SlideViewModel vm, int slideGroup)
        {
            var compare = _slideRepository.GetAllIncluding(x => x.SortOrder == vm.SortOrder && (int)x.GroupAlias == slideGroup);
            var result = compare.Count() > 1 ? true : false;
            return result;
        }

        public int SetNewSlideOrder(int slideGroup)
        {
            var order = 0;
            var list = _slideRepository.GetAllIncluding(x => (int) x.GroupAlias == slideGroup).OrderBy(x => x.SortOrder).Select(x => x.SortOrder).ToList();
            order = CommonMethods.GetOrder(list);
            return order;
        }

        public override void Add(SlideViewModel productVm)
        {
            var product = Mapper.Map<SlideViewModel, Slide>(productVm);
            if (!string.IsNullOrWhiteSpace(productVm.Name))
            {
                product.Name = productVm.Name.Trim();
            }
            product.Url = string.IsNullOrWhiteSpace(productVm.Url) || string.IsNullOrEmpty(productVm.Url) ? TextHelper.ToUnsignString(product.Name.ToLower().Trim()) : productVm.Url.ToLower().Trim();

            _slideRepository.Insert(product);
        }

        public override void Update(SlideViewModel productVm)
        {
            productVm.Name = productVm.Name.Trim();
            if(!string.IsNullOrEmpty(productVm.Content) || !string.IsNullOrWhiteSpace(productVm.Content))
            productVm.Content = productVm.Content.Trim();

            if(!string.IsNullOrEmpty(productVm.MainCaption) || !string.IsNullOrWhiteSpace(productVm.MainCaption))
                productVm.MainCaption = productVm.MainCaption.Trim();

            if(!string.IsNullOrEmpty(productVm.SubCaption) || !string.IsNullOrWhiteSpace(productVm.SubCaption))
                productVm.SubCaption = productVm.SubCaption.Trim();

            if(!string.IsNullOrEmpty(productVm.SmallCaption) || !string.IsNullOrWhiteSpace(productVm.SmallCaption))
            productVm.SmallCaption = productVm.SmallCaption.Trim();

            if(!string.IsNullOrEmpty(productVm.ActionCaption) || !string.IsNullOrWhiteSpace(productVm.ActionCaption))
            productVm.ActionCaption = productVm.ActionCaption.Trim();

            var product = _slideRepository.GetById(productVm.Id);

            product.Url = string.IsNullOrWhiteSpace(productVm.Url) || string.IsNullOrEmpty(productVm.Url) || product.Url == productVm.Url
                ? TextHelper.ToUnsignString(product.Name.ToLower().Trim())
                : productVm.Url.ToLower().Trim();
            
            Mapper.Map(productVm, product);
            
            _slideRepository.Update(product);
        }

        public void MultiDelete(IList<string> selectedIds)
        {
            foreach (var item in selectedIds)
            {
                var slide = Slides.Where(r => r.Id == int.Parse(item)).FirstOrDefault();
                Slides.Remove(slide);
            }
            _unitOfWork.Commit();
        }

        public PagedResult<SlideViewModel> GetAllPaging(string keyword, string position, int page, int pageSize, string sortBy)
        {
            var query = _slideRepository.GetAll();

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword));

            if (!string.IsNullOrEmpty(position))
                query = query.Where(x => (int) x.GroupAlias == int.Parse(position));

            int totalRow = query.Count();

            query = query.OrderByDescending(x => x.Id).Skip((page - 1) * pageSize)
                .Take(pageSize);

            var data = query.ProjectTo<SlideViewModel>().ToList();
            var paginationSet = new PagedResult<SlideViewModel>()
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