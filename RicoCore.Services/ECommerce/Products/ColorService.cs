using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using RicoCore.Data.EF;
using RicoCore.Data.Entities;
using RicoCore.Data.Entities.Content;
using RicoCore.Data.Entities.ECommerce;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.Interfaces;
using RicoCore.Infrastructure.SharedKernel;
using RicoCore.Services.Content.Tags.Dtos;
using RicoCore.Services.ECommerce.ProductCategories.Dtos;
using RicoCore.Services.ECommerce.Products.Dtos;
using RicoCore.Utilities.Constants;
using RicoCore.Utilities.Dtos;
using RicoCore.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RicoCore.Services.ECommerce.Products
{
    public class ColorService : WebServiceBase<Color, int, ColorViewModel>, IColorService
    {      
        private readonly IRepository<Color, int> _colorRepository;
        private readonly IRepository<ProductColor, int> _productColorRepository;
        private readonly IRepository<ProductItem, int> _productItemRepository;

        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<Color> Colors;

        public ColorService(IRepository<Color, int> colorRepository,
            IRepository<ProductColor, int> productColorRepository,
            IRepository<ProductItem, int> productItemRepository,
            AppDbContext context,
            IUnitOfWork unitOfWork)
            : base(colorRepository, unitOfWork)
        {
            _colorRepository = colorRepository;            
            _unitOfWork = unitOfWork;
            Colors = context.Set<Color>();
            _productColorRepository = productColorRepository;
            _productItemRepository = productItemRepository;
        }
        private string GenerateProductCode()
        {
            var code = CommonHelper.GenerateRandomCode();
            //while (HasExistsColorUrl(code))
            //{
            //    code = CommonHelper.GenerateRandomCode();
            //}
            return code;
        }
        //public virtual bool HasExistsColorUrl(string url)
        //{
        //    return _colorRepository.GetAll().Any(x => x.Url == url);
        //}

            public bool ValidateAddColorName(ColorViewModel vm)
            {
                return _colorRepository.GetAll().Any(x => x.Name.ToLower() == vm.Name.ToLower());
            }

        public bool ValidateAddSortOrder(ColorViewModel vm)
        {
            return _colorRepository.GetAll().Any(x => x.SortOrder == vm.SortOrder);
        }

        public bool ValidateUpdateColorName(ColorViewModel vm)
        {
            var compare = _colorRepository.GetAllIncluding(x => x.Name.ToLower() == vm.Name.ToLower());           
            var result = compare.Count() > 1 ? true : false;
            return result ;
        }

        public bool ValidateUpdateSortOrder(ColorViewModel vm)
        {
            var compare = _colorRepository.GetAllIncluding(x => x.SortOrder == vm.SortOrder);
            var result = compare.Count() > 1 ? true : false;
            return result;
        }

        public int SetNewColorOrder()
        {
            var order = 0;
            var list = _colorRepository.GetAll().OrderBy(x => x.SortOrder).Select(x => x.SortOrder).ToList();
            order = CommonMethods.GetOrder(list);
            return order;
        }

        public override void Add(ColorViewModel productVm)
        {
            var product = Mapper.Map<ColorViewModel, Color>(productVm);
            if (!string.IsNullOrWhiteSpace(productVm.Name))
            {
                product.Name = productVm.Name.Trim();
            }
            product.Url = string.IsNullOrWhiteSpace(productVm.Url) ? TextHelper.ToUnsignString(product.Name.ToLower()) : productVm.Url.ToLower();
                                               
            _colorRepository.Insert(product);
        }

        public override void Update(ColorViewModel productVm)
        {
            var product = _colorRepository.GetById(productVm.Id);             
            Mapper.Map(productVm, product);
            
            if (!string.IsNullOrWhiteSpace(productVm.Name))
            {
                product.Name = productVm.Name.Trim();
            }       

            if (string.IsNullOrWhiteSpace(productVm.Url) || product.Url == productVm.Url)
            {
                product.Url = TextHelper.ToUnsignString(product.Name.ToLower());
            }
            
            _colorRepository.Update(product);
        }

        public void MultiDelete(IList<string> selectedIds)
        {
            foreach (var item in selectedIds)
            {
                var color = Colors.Where(r => r.Id == int.Parse(item)).FirstOrDefault();
                Colors.Remove(color);                
            }
            _unitOfWork.Commit();
        }

        public PagedResult<ColorViewModel> GetAllPaging(string keyword, int page, int pageSize, string sortBy)
        {
            var query = _colorRepository.GetAll();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword)
                || x.EnglishName.Contains(keyword)
                || x.Code.Contains(keyword));

            int totalRow = query.Count();
            query = query.Skip((page - 1) * pageSize)
               .Take(pageSize);

            var data = query.OrderBy(x => x.SortOrder).ProjectTo<ColorViewModel>().ToList();
            var paginationSet = new PagedResult<ColorViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public ColorViewModel GetByUrl(string url)
        {
            return _colorRepository.GetAllIncluding(x => x.Url == url).ProjectTo<ColorViewModel>().FirstOrDefault();
        }
    }
}