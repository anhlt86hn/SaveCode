using AutoMapper;
using AutoMapper.QueryableExtensions;
using RicoCore.Data.Entities.ECommerce;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.Interfaces;
using RicoCore.Infrastructure.SharedKernel;
using RicoCore.Services.ECommerce.ProductCategories.Dtos;
using RicoCore.Utilities.Dtos;
using RicoCore.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RicoCore.Services.ECommerce.ProductCategories
{
    public class ProductCategoryService : WebServiceBase<ProductCategory, int, ProductCategoryViewModel>, IProductCategoryService
    {
        private readonly IRepository<ProductCategory, int> _productCategoryRepository;
        private readonly IRepository<Product, Guid> _productRepository;
        //private readonly IUnitOfWork _unitOfWork;

        public ProductCategoryService(IRepository<ProductCategory, int> productCategoryRepository,
           IRepository<Product, Guid> productRepository,
           IUnitOfWork unitOfWork) : base(productCategoryRepository, unitOfWork)
        {
            _productCategoryRepository = productCategoryRepository;
            _productRepository = productRepository;
            //_unitOfWork = unitOfWork;
        }

        public bool ValidateAddProductCategoryName(ProductCategoryViewModel productCategoryVm)
        {
            return _productCategoryRepository.GetAll().Any(x => x.Name.ToLower() == productCategoryVm.Name.ToLower());
        }
        public bool ValidateUpdateProductCategoryName(ProductCategoryViewModel productCategoryVm)
        {
            var compare = _productCategoryRepository.GetAllIncluding(x => x.Name.ToLower() == productCategoryVm.Name.ToLower());
            var result = compare.Count() > 1 ? true : false;
            return result;            
        }

        public bool ValidateAddProductCategoryOrder(ProductCategoryViewModel productCategoryVm)
        {
            return _productCategoryRepository.GetAll().Any(x => x.SortOrder == productCategoryVm.SortOrder && x.SortOrder != 0);
        }
        public bool ValidateUpdateProductCategoryOrder(ProductCategoryViewModel productCategoryVm)
        {
            var compare = _productCategoryRepository.GetAllIncluding(x => x.SortOrder == productCategoryVm.SortOrder && x.SortOrder != 0);
            var result = compare.Count() > 1 ? true : false;
            return result;
        }
        public ProductCategoryViewModel SetValueToNewProductCategory(int parentId)
        {
            var productCategory = new ProductCategoryViewModel();
            if (parentId == 0)
            {
                productCategory.ParentId = null;
            }
            else
            {
                productCategory.ParentId = parentId;
            }
            productCategory.ParentProductCategoryName = GetNameById(parentId);
            productCategory.SortOrder = SetNewProductCategoryOrder(parentId);
            productCategory.HomeOrder = SetNewProductCategoryHomeOrder();
            productCategory.HotOrder = SetNewProductCategoryHotOrder();
            //productCategory.Order = SetNewOrder(parentId);
            return productCategory;
        }
        public int SetNewProductCategoryOrder(int? parentId)
        {
            var order = 0;
            var list = parentId == 0
                ? _productCategoryRepository.GetAllIncluding(x => x.ParentId == null & x.SortOrder != 0).OrderBy(x => x.SortOrder).Select(x => x.SortOrder).ToList()
                : _productCategoryRepository.GetAllIncluding(x => x.ParentId == parentId && x.SortOrder != 0).OrderBy(x => x.SortOrder).Select(x => x.SortOrder).ToList();

            order = CommonMethods.GetOrder(list);
            return order;
        }        

        public bool ValidateAddProductCategoryHotOrder(ProductCategoryViewModel productCategoryVm)
        {
            return _productCategoryRepository.GetAll().Any(x => x.HotOrder == productCategoryVm.HotOrder && x.HotOrder != 0);
        }
        public bool ValidateUpdateProductCategoryHotOrder(ProductCategoryViewModel productCategoryVm)
        {
            var compare = _productCategoryRepository.GetAllIncluding(x => x.HotOrder == productCategoryVm.HotOrder && x.HotOrder != 0);
            var result = compare.Count() > 1 ? true : false;
            return result;
        }
        public int SetNewProductCategoryHotOrder()
        {
            var order = 0;
            var list = _productCategoryRepository.GetAllIncluding(x=>x.HotOrder != 0).OrderBy(x => x.HotOrder).Select(x => x.HotOrder).ToList();

            order = CommonMethods.GetOrder(list);
            return order;
        }
       
        public bool ValidateAddProductCategoryHomeOrder(ProductCategoryViewModel productCategoryVm)
        {
            return _productCategoryRepository.GetAll().Any(x => x.HomeOrder == productCategoryVm.HomeOrder && x.HomeOrder != 0);
        }
        public bool ValidateUpdateProductCategoryHomeOrder(ProductCategoryViewModel productCategoryVm)
        {
            var compare = _productCategoryRepository.GetAllIncluding(x => x.HomeOrder == productCategoryVm.HomeOrder && x.HomeOrder != 0);
            var result = compare.Count() > 1 ? true : false;
            return result;
        }
        public int SetNewProductCategoryHomeOrder()
        {
            var order = 0;
            var list = _productCategoryRepository.GetAllIncluding(x=>x.HomeOrder != 0).OrderBy(x => x.HomeOrder).Select(x => x.HomeOrder).ToList();

            order = CommonMethods.GetOrder(list);
            return order;
        }


        public string GetNameById(int parentId)
        {
            var name = string.Empty;
            if (parentId == 0)
            {
                name = "Không có";
            }
            else
            {
                var postCategory = _productCategoryRepository.GetById(parentId);
                name = postCategory.Name;
            }
            return name;
        }

       

       
        

        #region Add

        public override void Add(ProductCategoryViewModel productCategoryVm)
        {
            var productCategory = Mapper.Map<ProductCategoryViewModel, ProductCategory>(productCategoryVm);
            if (string.IsNullOrWhiteSpace(productCategoryVm.Url) || string.IsNullOrEmpty(productCategoryVm.Url))
                productCategory.Url = TextHelper.ToUnsignString(productCategoryVm.Name.ToLower().Trim());
            else
                productCategory.Url = productCategoryVm.Url.ToLower().Trim();
            productCategory.Code = GenerateCode();
            var query = _productCategoryRepository.GetAllIncluding(x => x.Url == productCategory.Url);
            if (query.Count() > 1)
            {
                productCategory.Url = $"{productCategory.Url}-{productCategory.Code.ToLower()}";
            }
            _productCategoryRepository.Insert(productCategory);
        }

        #endregion Add

        #region Update

        public override void Update(ProductCategoryViewModel productCategoryVm)
        {
            var productCategory = _productCategoryRepository.GetById(productCategoryVm.Id);
            var createdDate = productCategory.DateCreated;
            Mapper.Map(productCategoryVm, productCategory);
            if (string.IsNullOrWhiteSpace(productCategoryVm.Url) || string.IsNullOrEmpty(productCategoryVm.Url))
                productCategory.Url = TextHelper.ToUnsignString(productCategoryVm.Name.ToLower().Trim());
            else
                productCategory.Url = productCategoryVm.Url.ToLower().Trim();
            var query = _productCategoryRepository.GetAllIncluding(x => x.Url == productCategory.Url);
            if (query.Count() > 1)
            {
                productCategory.Url = $"{productCategory.Url}-{productCategory.Code.ToLower()}";
            }
            
            productCategory.DateCreated = createdDate;
            _productCategoryRepository.Update(productCategory);
        }

        #endregion Update

        #region Delete

        public override void Delete(int id)
        {
            _productCategoryRepository.Delete(id);
        }

        #endregion Delete

        #region GetById

        public override ProductCategoryViewModel GetById(int id)
        {
            //var query = (from pc in _productCategoryRepository.GetAll()
            //             join p in _productRepository.GetAll()
            //                 on pc.Id equals p.CategoryId
            //             where pc.Id == id && p.CategoryId == id
            //             select new { p });

            var productCategory = (from pcat in _productCategoryRepository.GetAll() where pcat.Id == id select new { pcat }).FirstOrDefault();

            var parentName = productCategory != null && productCategory.pcat.ParentId != null
                    ? _productCategoryRepository.GetAllIncluding(x => x.Id == productCategory.pcat.ParentId).Select(x => x.Name).FirstOrDefault()
                    : "Không có";

            //var productlist = query.Select(x => new ProductViewModel()
            //{
            //    Name = x.p.Name,
            //    //Id = x.p.Id,
            //    CategoryId = x.p.CategoryId,
            //    Url = x.p.Url,
            //    Description = x.p.Description,
            //    Image = x.p.Image,
            //    Content = x.p.Content,
            //    Viewed = x.p.Viewed,
            //    Tags = x.p.Tags,
            //    //Unit = x.p.Unit,
            //    HomeFlag = x.p.HomeFlag,
            //    HotFlag = x.p.HotFlag,
            //    //Quantity = x.p.Quantity,

            //    Status = x.p.Status,
            //    MetaTitle = x.p.MetaTitle,
            //    MetaDescription = x.p.MetaDescription,
            //    MetaKeywords = x.p.MetaKeywords
            //}).ToList();

            var model = new ProductCategoryViewModel()
            {
                Name = productCategory.pcat.Name,
                Code = productCategory.pcat.Code,
                Id = productCategory.pcat.Id,
                ParentId = productCategory.pcat.ParentId,
                ParentProductCategoryName = parentName,
                Url = productCategory.pcat.Url,
                Description = productCategory.pcat.Description,
                Image = productCategory.pcat.Image,
                MetaTitle = productCategory.pcat.MetaTitle,
                MetaDescription = productCategory.pcat.MetaDescription,
                MetaKeywords = productCategory.pcat.MetaKeywords,
                HotOrder = productCategory.pcat.HotOrder,
                HotFlag = productCategory.pcat.HotFlag,
                HomeOrder = productCategory.pcat.HomeOrder,
                HomeFlag = productCategory.pcat.HomeFlag,
                SortOrder = productCategory.pcat.SortOrder,
                DateCreated = productCategory.pcat.DateCreated,
                DateModified = productCategory.pcat.DateModified,
                //Order = productCategory.pcat.Order,
                //Products = productlist,
                Status = productCategory.pcat.Status
            };

            return model;
        }

        //public override ProductCategoryViewModel GetById(Guid id)
        //{
        //    return Mapper.Map<ProductCategory, ProductCategoryViewModel>(_productCategoryRepository.GetById(id));
        //}

        //public PagedResult<ProductCategoryViewModel> GetAllPaging(string keyword, int pageSize, int page = 1)
        //{
        //    var query = _productCategoryRepository.FindAll();
        //    if (!string.IsNullOrWhiteSpace(keyword))
        //        query = query.Where(x => x.Name.Contains(keyword));
        //    int totalRow = query.Count();
        //    var data = query.OrderByDescending(x => x.CreatedDate)
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize);
        //    var paginationSet = new PagedResult<ProductCategoryViewModel>()
        //    {
        //        Results = data.ProjectTo<ProductCategoryViewModel>().ToList(),
        //        CurrentPage = page,
        //        RowCount = totalRow,
        //        PageSize = pageSize,
        //    };
        //    return paginationSet;
        //}

        #endregion GetById

        #region GetAll

        public override List<ProductCategoryViewModel> GetAll()
        {
            try
            {
                //var productCategoryList = _productCategoryRepository.GetAll();
                return _productCategoryRepository.GetAll().OrderBy(x => x.SortOrder)
                //return productCategoryList
                .ProjectTo<ProductCategoryViewModel>()
                .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ProductCategoryViewModel> GetAll(string keyword)
        {
            if (!string.IsNullOrWhiteSpace(keyword))
                return _productCategoryRepository.GetAll().Where(x => x.Name.Contains(keyword)
                || x.Description.Contains(keyword))
                    .OrderBy(x => x.ParentId).ProjectTo<ProductCategoryViewModel>().ToList();
            return _productCategoryRepository.GetAll().OrderBy(x => x.Id)
                .ProjectTo<ProductCategoryViewModel>()
                .ToList();
        }

        #endregion GetAll

        #region GetAllByParentId

        public List<ProductCategoryViewModel> GetAllByParentId(int? parentId)
        {
            return _productCategoryRepository.GetAll().Where(x => x.Status == Status.Actived && x.ParentId == parentId)
                .ProjectTo<ProductCategoryViewModel>()
                .ToList();
        }

        #endregion GetAllByParentId

        #region GetAllPaging

        public PagedResult<ProductCategoryViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            var query = _productCategoryRepository.GetAll();
            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(x => x.Name.Contains(keyword));
            int totalRow = query.Count();
            var data = query.OrderByDescending(x => x.SortOrder)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
            var paginationSet = new PagedResult<ProductCategoryViewModel>()
            {
                Results = data.ProjectTo<ProductCategoryViewModel>().ToList(),
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize,
            };
            return paginationSet;
        }

        public PagedResult<ProductCategoryViewModel> GetAllPagingAsync(string keyword, int page, int pageSize)
        {
            var query = _productCategoryRepository.GetAll();
            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(x => x.Name.Contains(keyword)
                || x.Description.Contains(keyword));

            int totalRow = query.Count();
            query = query.Skip((page - 1) * pageSize)
               .Take(pageSize);

            var data = query.ProjectTo<ProductCategoryViewModel>().ToList();
            var paginationSet = new PagedResult<ProductCategoryViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        #endregion GetAllPaging

        #region GetHomeCategories

        public List<ProductCategoryViewModel> GetHomeCategories(int top)
        {
            var query = _productCategoryRepository.GetAll().Where(x => x.HomeFlag == Status.Actived)
                .OrderBy(x => x.HomeOrder).Take(top).ProjectTo<ProductCategoryViewModel>();
            var categories = query.ToList();
            //foreach (var category in categories)
            //{
            //    category.Products = _productRepository
            //        .GetAll().Where(x => x.CategoryId == category.Id)
            //        .OrderByDescending(x => x.CreatedDate)
            //        .Take(5)
            //        .ProjectTo<ProductViewModel>().ToList();
            //}
            return categories;
        }

        public ProductCategoryViewModel GetByUrl(string url)
        {
            var query = _productCategoryRepository.GetAllIncluding(x => x.Url.Equals(url)).FirstOrDefault();
            var vm = Mapper.Map<ProductCategory, ProductCategoryViewModel>(query);
            return vm;
        }
        #endregion GetHomeCategories

        #region UpdateParentId

        public void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            //Update parent id for source
            var category = _productCategoryRepository.GetById(sourceId);
            category.ParentId = targetId;
            _productCategoryRepository.Update(category);

            //Get all sibling
            var sibling = _productCategoryRepository.GetAll().Where(x => items.ContainsKey(x.Id));
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];
                _productCategoryRepository.Update(child);
            }
        }

        #endregion UpdateParentId

        #region ReOrder

        public void ReOrder(int sourceId, int targetId)
        {
            var source = _productCategoryRepository.GetById(sourceId);
            var target = _productCategoryRepository.GetById(targetId);
            int tempOrder = source.SortOrder;

            source.SortOrder = target.SortOrder;
            target.SortOrder = tempOrder;

            _productCategoryRepository.Update(source);
            _productCategoryRepository.Update(target);
        }

        #endregion ReOrder

        public List<ProductCategory> AllSubCategories(int id)
        {
            return _productCategoryRepository.GetAllIncluding(x => x.ParentId == id).ToList();
        }

        public virtual bool HasExistsCode(string code)
        {
            return _productCategoryRepository.GetAll().Any(x => x.Code == code);
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
    }
}