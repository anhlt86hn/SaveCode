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
    public class ProductColorService : WebServiceBase<ProductColor, int, ProductColorViewModel>, IProductColorService
    {            
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<Color> Colors;
        private readonly DbSet<ProductColor> ProductColors;
        private readonly IRepository<ProductColor, int> _productColorRepository;
        private readonly IRepository<Color, int> _colorRepository;
        private readonly IRepository<ProductCategory, int> _productCategoryRepository;
        private readonly IRepository<Product, Guid> _productRepository;

        public ProductColorService(IRepository<Color, int> colorRepository,
            IRepository<ProductColor, int> productColorRepository,
            IRepository<ProductCategory, int> productCategoryRepository,
            IRepository<Product, Guid> productRepository,
            AppDbContext context,
            IUnitOfWork unitOfWork)
            : base(productColorRepository, unitOfWork)
        {
            _colorRepository = colorRepository;
            _productColorRepository = productColorRepository;
            _productCategoryRepository = productCategoryRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            Colors = context.Set<Color>();
            ProductColors = context.Set<ProductColor>();
        }

        public bool ValidateAddSortOrder(ProductColorViewModel vm)
        {
            return _productColorRepository.GetAll().Any(x => x.SortOrder == vm.SortOrder && x.ProductId == vm.ProductId);
        }      

        public bool ValidateUpdateSortOrder(ProductColorViewModel vm)
        {
            var compare = _productColorRepository.GetAllIncluding(x => x.SortOrder == vm.SortOrder && x.ProductId == vm.ProductId);
            var result = compare.Count() > 1 ? true : false;
            return result;
        }

        public void MultiDelete(IList<string> selectedIds)
        {
            foreach (var item in selectedIds)
            {
                var color = ProductColors.Where(r => r.Id == int.Parse(item)).FirstOrDefault();
                ProductColors.Remove(color);                
            }
            _unitOfWork.Commit();
        }

        public override void Add(ProductColorViewModel productColorVm)
        {
            productColorVm.ProductName = _productRepository.GetById(productColorVm.ProductId).Name;
            productColorVm.ProductCategoryName = _productCategoryRepository.GetById(productColorVm.ProductCategoryId).Name;
            productColorVm.Name = _colorRepository.GetById(productColorVm.ColorId).Name;
            productColorVm.Url = _colorRepository.GetById(productColorVm.ColorId).Url;           
            var productColor = Mapper.Map<ProductColorViewModel, ProductColor>(productColorVm);            
            _productColorRepository.Insert(productColor);
        }

        public override void Update(ProductColorViewModel productVm)
        {
            productVm.ProductCategoryName = _productCategoryRepository.GetById(productVm.ProductCategoryId).Name;
            productVm.ProductName = _productRepository.GetById(productVm.ProductId).Name;
            productVm.Name = _colorRepository.GetById(productVm.ColorId).Name;
            var product = _productColorRepository.GetById(productVm.Id);    
            Mapper.Map(productVm, product);               
            _productColorRepository.Update(product);
        }

        public PagedResult<ProductColorViewModel> GetAllPaging(int productCategoryId, string productId, string keyword, int page, int pageSize, string sortBy)
        {
            var query = _productColorRepository.GetAll();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword)
                || x.ProductCategoryName.Contains(keyword)
                || x.ProductName.Contains(keyword));

            if (productCategoryId != 0)
                query = query.Where(x => x.ProductCategoryId == productCategoryId);

            if (!string.IsNullOrEmpty(productId) && productId != CommonConstants.DefaultGuid)
                query = query.Where(x => x.ProductId.ToString() == productId);

            //if (colorId != 0)
            //    query = query.Where(x => x.ColorId == colorId);

            switch (sortBy)
            {
                case "thu-tu-giam-dan":
                    query = query.OrderByDescending(x => x.SortOrder);
                    break;
                case "thu-tu-tang-dan":
                    query = query.OrderBy(x => x.SortOrder);
                    break;
                default:
                    query = query.OrderBy(x => x.SortOrder);
                    break;                    
            }

            int totalRow = query.Count();
            query = query.Skip((page - 1) * pageSize)
               .Take(pageSize);

            var data = query.ProjectTo<ProductColorViewModel>().ToList();
            var paginationSet = new PagedResult<ProductColorViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }
        
        public List<ProductColorViewModel> GetListProductColorByProductId(Guid productId)
        {
            return _productColorRepository.GetAllIncluding(x => x.ProductId == productId).OrderBy(x=>x.SortOrder).ProjectTo<ProductColorViewModel>().ToList();
        }

        public int SetNewProductColorOrder(Guid productId)
        {
            var order = 0;
            //var list = parentId == null
            //    ? _postCategoryRepository.GetAllIncluding(x => x.ParentId == null).OrderBy(x => x.SortOrder).Select(x => x.SortOrder).ToList()
            //    : _postCategoryRepository.GetAllIncluding(x => x.ParentId.ToString() == parentId).OrderBy(x => x.SortOrder).Select(x => x.SortOrder).ToList();
            var list = _productColorRepository.GetAllIncluding(x => x.ProductId == productId).OrderBy(x => x.SortOrder).Select(x => x.SortOrder).ToList();

            order = CommonMethods.GetOrder(list);
            return order;
        }

        public ColorViewModel GetColor(int colorId)
        {
            return Mapper.Map<Color, ColorViewModel>(_colorRepository.GetById(colorId));
        }
    }
}