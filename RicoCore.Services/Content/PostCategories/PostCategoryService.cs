using AutoMapper;
using AutoMapper.QueryableExtensions;
using RicoCore.Data.Entities.ECommerce;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.Interfaces;
using RicoCore.Services.Content.PostCategories.Dtos;
using RicoCore.Utilities.Dtos;
using RicoCore.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using RicoCore.Data.Entities.Content;
using RicoCore.Services.Content.Posts.Dtos;
using RicoCore.Infrastructure.SharedKernel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RicoCore.Services.Content.PostCategories
{
    public class PostCategoryService : WebServiceBase<PostCategory, int, PostCategoryViewModel>, IPostCategoryService
    {
        private readonly IRepository<PostCategory, int> _postCategoryRepository;
        private readonly IRepository<Post, Guid> _postRepository;
        //private readonly IUnitOfWork _unitOfWork;

        public PostCategoryService(IRepository<PostCategory, int> postCategoryRepository,
           IRepository<Post, Guid> postRepository,
           IUnitOfWork unitOfWork) : base(postCategoryRepository, unitOfWork)
        {
            _postCategoryRepository = postCategoryRepository;
            _postRepository = postRepository;
            //_unitOfWork = unitOfWork;
        }
        public void SetSelectListItemCategories(PostViewModel postVm, List<PostCategoryViewModel> postCategoriesVm)
        {
            postVm.Categories = new List<SelectListItem>();
            foreach (var item in postCategoriesVm)
            {
                postVm.Categories.Add(new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = postVm.CategoryId == item.Id ? true : false
                });
            }
        }

        public PostCategoryViewModel GetByUrl(string url)
        {
            var postCategory = _postCategoryRepository.FirstOrDefault(x => x.Url == url);
            if (postCategory == null) throw new Exception("Không để rỗng PostCategory");

            var vm = Mapper.Map<PostCategory, PostCategoryViewModel>(postCategory);
            return vm;
        }
        public string GetNameById(int parentId)
        {
            var name = string.Empty;
            if(parentId == 0)
            {
                name = "Không có";
            }
            else
            {
                var postCategory = _postCategoryRepository.GetById(parentId);
                name = postCategory.Name;
            }                        
            return name;                             
        }

        public bool ValidateAddPostCategoryName(PostCategoryViewModel postCategoryVm)
        {
            return _postCategoryRepository.GetAll().Any(x => x.Name.ToLower() == postCategoryVm.Name.ToLower());
        }
        public bool ValidateUpdatePostCategoryName(PostCategoryViewModel postCategoryVm)
        {
            var compare = _postCategoryRepository.GetAllIncluding(x => x.Name.ToLower() == postCategoryVm.Name.ToLower());
            return compare.Any();
        }

        public bool ValidateAddPostCategoryOrder(PostCategoryViewModel postCategoryVm)
        {
            var compare = _postCategoryRepository.GetAllIncluding(x => x.ParentId == postCategoryVm.ParentId 
            && x.SortOrder == postCategoryVm.SortOrder && x.SortOrder != 0);
            return compare.Any();
        }
        public bool ValidateUpdatePostCategoryOrder(PostCategoryViewModel postCategoryVm)
        {
            var compare = _postCategoryRepository.GetAllIncluding(x => x.ParentId == postCategoryVm.ParentId
            && x.SortOrder == postCategoryVm.SortOrder && x.SortOrder != 0);
            return compare.Any();          
        }
        public PostCategoryViewModel SetValueToNewPostCategory(int parentId)
        {
            var productCategory = new PostCategoryViewModel();
            if (parentId == 0)
            {
                productCategory.ParentId = null;
            }
            else
            {
                productCategory.ParentId = parentId;
            }
            productCategory.ParentPostCategoryName = GetNameById(parentId);
            productCategory.SortOrder = SetNewPostCategoryOrder(parentId);
            productCategory.HomeOrder = SetNewPostCategoryHomeOrder();
            productCategory.HotOrder = SetNewPostCategoryHotOrder();
            //productCategory.Order = SetNewOrder(parentId);
            return productCategory;
        }
        public int SetNewPostCategoryOrder(int? parentId)
        {
            var order = 0;
            var list = parentId == 0
                ? _postCategoryRepository.GetAllIncluding(x => x.ParentId == null && x.SortOrder != 0).OrderBy(x => x.SortOrder).Select(x => x.SortOrder).ToList()
                : _postCategoryRepository.GetAllIncluding(x => x.ParentId == parentId && x.SortOrder != 0).OrderBy(x => x.SortOrder).Select(x => x.SortOrder).ToList();

            order = CommonMethods.GetOrder(list);
            return order;
        }

        public bool ValidateAddPostCategoryHotOrder(PostCategoryViewModel postCategoryVm)
        {
            var compare = _postCategoryRepository.GetAllIncluding(x => x.ParentId == postCategoryVm.ParentId 
            && x.HotOrder == postCategoryVm.HotOrder && x.HotOrder != 0);
            return compare.Any();
        }

        public bool ValidateUpdatePostCategoryHotOrder(PostCategoryViewModel postCategoryVm)
        {
            var compare = _postCategoryRepository.GetAllIncluding(x => x.ParentId == postCategoryVm.ParentId && x.HotOrder == postCategoryVm.HotOrder && x.HotOrder != 0);
            return compare.Any();
        }

        public int SetNewPostCategoryHotOrder()
        {
            var order = 0;
            var list = _postCategoryRepository.GetAllIncluding(x=>x.HotOrder != 0).OrderBy(x => x.HotOrder).Select(x => x.HotOrder).ToList();

            order = CommonMethods.GetOrder(list);
            return order;
        }

        public bool ValidateAddPostCategoryHomeOrder(PostCategoryViewModel postCategoryVm)
        {
            var compare = _postCategoryRepository.GetAllIncluding(x => x.ParentId == postCategoryVm.ParentId && x.HomeOrder == postCategoryVm.HomeOrder && x.HomeOrder != 0);
            return compare.Any();
        }
        public bool ValidateUpdatePostCategoryHomeOrder(PostCategoryViewModel postCategoryVm)
        {
            var compare = _postCategoryRepository.GetAllIncluding(x => x.ParentId == postCategoryVm.ParentId && x.HomeOrder == postCategoryVm.HomeOrder && x.HomeOrder != 0);
            return compare.Any();
        }
        public int SetNewPostCategoryHomeOrder()
        {
            var order = 0;
            var list = _postCategoryRepository.GetAllIncluding(x=>x.HomeOrder != 0).OrderBy(x => x.HomeOrder).Select(x => x.HomeOrder).ToList();

            order = CommonMethods.GetOrder(list);
            return order;
        }


        #region Add
        public override void Add(PostCategoryViewModel postCategoryVm)
        {
            var postCategory = Mapper.Map<PostCategoryViewModel, PostCategory>(postCategoryVm);
            if (string.IsNullOrWhiteSpace(postCategoryVm.Url) || string.IsNullOrEmpty(postCategoryVm.Url))
                postCategory.Url = TextHelper.ToUnsignString(postCategoryVm.Name.ToLower().Trim());
            else
                postCategory.Url = postCategoryVm.Url.ToLower().Trim();
            postCategory.Code = GenerateCode();
            var query = _postCategoryRepository.GetAllIncluding(x => x.Url == postCategory.Url);
            if(query.Count() > 1)
            {
                postCategory.Url = $"{postCategory.Url}-{postCategory.Code.ToLower()}";
            }            
            _postCategoryRepository.Insert(postCategory);
        }
        #endregion

        #region Update
        public override void Update(PostCategoryViewModel postCategoryVm)
        {
            var postCategory = _postCategoryRepository.GetById(postCategoryVm.Id);
            var createdDate = postCategory.DateCreated;
            Mapper.Map(postCategoryVm, postCategory);

            if (string.IsNullOrWhiteSpace(postCategoryVm.Url) || string.IsNullOrEmpty(postCategoryVm.Url))
                postCategory.Url = TextHelper.ToUnsignString(postCategoryVm.Name.ToLower().Trim());
            else
            postCategory.Url = postCategoryVm.Url.ToLower().Trim();
            var query = _postCategoryRepository.GetAllIncluding(x => x.Url == postCategory.Url);
            if (query.Count() > 1)
            {
                postCategory.Url = $"{postCategory.Url}-{postCategory.Code.ToLower()}";
            }
            
            postCategory.DateCreated = createdDate;
            _postCategoryRepository.Update(postCategory);
        }
        #endregion

        #region Delete
        public override void Delete(int id)
        {
            _postCategoryRepository.Delete(id);
        }
        #endregion

        #region GetById
        public override PostCategoryViewModel GetById(int id)
        {
            //var query = (from pc in _postCategoryRepository.GetAll()
            //             join p in _postRepository.GetAll()
            //                 on pc.Id equals p.CategoryId
            //             where pc.Id == id && p.CategoryId == id
            //             select new { p });
            
            var postCategory = (from pcat in _postCategoryRepository.GetAll() where pcat.Id == id select new { pcat }).FirstOrDefault();
           
            var parentName = postCategory != null && postCategory.pcat.ParentId != null
                    ? _postCategoryRepository.GetAllIncluding(x => x.Id == postCategory.pcat.ParentId).Select(x => x.Name).FirstOrDefault()
                    : "Không có";           

            //var productlist = query.Select(x => new PostViewModel()
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

            var model = new PostCategoryViewModel()
            {
                Name = postCategory.pcat.Name,
                Code = postCategory.pcat.Code,
                Id = postCategory.pcat.Id,
                ParentId = postCategory.pcat.ParentId,
                ParentPostCategoryName = parentName,
                Url = postCategory.pcat.Url,
                Description = postCategory.pcat.Description,
                Image = postCategory.pcat.Image,
                MetaTitle = postCategory.pcat.MetaTitle,
                MetaDescription = postCategory.pcat.MetaDescription,
                MetaKeywords = postCategory.pcat.MetaKeywords,
                HotOrder = postCategory.pcat.HotOrder,
                HotFlag = postCategory.pcat.HotFlag,
                HomeOrder = postCategory.pcat.HomeOrder,
                HomeFlag = postCategory.pcat.HomeFlag,
                SortOrder = postCategory.pcat.SortOrder,
                DateCreated = postCategory.pcat.DateCreated,
                DateModified = postCategory.pcat.DateModified,
                //Order = postCategory.pcat.Order,
                //Products = productlist,
                Status = postCategory.pcat.Status
            };

            return model;
        }

        //public override PostCategoryViewModel GetById(Guid id)
        //{
        //    return Mapper.Map<PostCategory, PostCategoryViewModel>(_postCategoryRepository.GetById(id));
        //}      

        //public PagedResult<PostCategoryViewModel> GetAllPaging(string keyword, int pageSize, int page = 1)
        //{
        //    var query = _postCategoryRepository.FindAll();
        //    if (!string.IsNullOrWhiteSpace(keyword))
        //        query = query.Where(x => x.Name.Contains(keyword));
        //    int totalRow = query.Count();
        //    var data = query.OrderByDescending(x => x.CreatedDate)
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize);
        //    var paginationSet = new PagedResult<PostCategoryViewModel>()
        //    {
        //        Results = data.ProjectTo<PostCategoryViewModel>().ToList(),
        //        CurrentPage = page,
        //        RowCount = totalRow,
        //        PageSize = pageSize,
        //    };
        //    return paginationSet;
        //}
        #endregion

        #region GetAll
        public override List<PostCategoryViewModel> GetAll()
        {
            try
            {
                //var postCategoryList = _postCategoryRepository.GetAll();                
                return _postCategoryRepository.GetAll().OrderBy(x => x.ParentId)
                //return postCategoryList
                .ProjectTo<PostCategoryViewModel>()
                .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        public List<PostCategoryViewModel> GetAll(string keyword)
        {
            if (!string.IsNullOrWhiteSpace(keyword))
                return _postCategoryRepository.GetAll().Where(x => x.Name.Contains(keyword)
                || x.Description.Contains(keyword))
                    .OrderBy(x => x.ParentId).ProjectTo<PostCategoryViewModel>().ToList();
            return _postCategoryRepository.GetAll().OrderBy(x => x.Id)
                .ProjectTo<PostCategoryViewModel>()
                .ToList();
        }
        #endregion

        #region GetCategoriesList
        public List<PostCategoryViewModel> GetCategoriesList()
        {
            try
            {
                //var postCategoryList = _postCategoryRepository.GetAll();                
                return _postCategoryRepository.GetAllIncluding(x=>x.ParentId!=null).OrderBy(x => x.ParentId)
                //return postCategoryList
                .ProjectTo<PostCategoryViewModel>()
                .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetAllByParentId
        public List<PostCategoryViewModel> GetAllByParentId(int? parentId)
        {
            return _postCategoryRepository.GetAll().Where(x => x.Status == Status.Actived && x.ParentId == parentId)
                .ProjectTo<PostCategoryViewModel>()
                .ToList();
        }
        #endregion

        #region GetAllPaging
        public PagedResult<PostCategoryViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            var query = _postCategoryRepository.GetAll();
            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(x => x.Name.Contains(keyword));
            int totalRow = query.Count();
            var data = query.OrderByDescending(x => x.SortOrder)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
            var paginationSet = new PagedResult<PostCategoryViewModel>()
            {
                Results = data.ProjectTo<PostCategoryViewModel>().ToList(),
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize,
            };
            return paginationSet;
        }

        public PagedResult<PostCategoryViewModel> GetAllPagingAsync(string keyword, int page, int pageSize)
        {
            var query = _postCategoryRepository.GetAll();
            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(x => x.Name.Contains(keyword)
                || x.Description.Contains(keyword));

            int totalRow = query.Count();
            query = query.Skip((page - 1) * pageSize)
               .Take(pageSize);

            var data = query.ProjectTo<PostCategoryViewModel>().ToList();
            var paginationSet = new PagedResult<PostCategoryViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }
        #endregion

        #region GetHomeCategories
        public List<PostCategoryViewModel> GetHomeCategories(int top)
        {
            var query = _postCategoryRepository.GetAll().Where(x => x.HomeFlag == Status.Actived)
                .OrderBy(x => x.HomeOrder).Take(top).ProjectTo<PostCategoryViewModel>();
            var categories = query.ToList();
            //foreach (var category in categories)
            //{
            //    category.Products = _postRepository
            //        .GetAll().Where(x => x.CategoryId == category.Id)
            //        .OrderByDescending(x => x.CreatedDate)
            //        .Take(5)
            //        .ProjectTo<PostViewModel>().ToList();
            //}
            return categories;
        }
        #endregion

        #region UpdateParentId
        public void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            //Update parent id for source
            var category = _postCategoryRepository.GetById(sourceId);
            category.ParentId = targetId;
            _postCategoryRepository.Update(category);

            //Get all sibling
            var sibling = _postCategoryRepository.GetAll().Where(x => items.ContainsKey(x.Id));
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];
                _postCategoryRepository.Update(child);
            }
        }
        #endregion

        #region ReOrder
        public void ReOrder(int sourceId, int targetId)
        {
            var source = _postCategoryRepository.GetById(sourceId);
            var target = _postCategoryRepository.GetById(targetId);
            int tempOrder = source.SortOrder;

            source.SortOrder = target.SortOrder;
            target.SortOrder = tempOrder;

            _postCategoryRepository.Update(source);
            _postCategoryRepository.Update(target);
        }
        #endregion

        public List<PostCategory> AllSubCategories(int id)
        {            
            return _postCategoryRepository.GetAllIncluding(x => x.ParentId == id).ToList();           
        }

        public virtual bool HasExistsCode(string code)
        {
            return _postCategoryRepository.GetAll().Any(x => x.Code == code);
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