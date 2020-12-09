using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using RicoCore.Data.EF;
using RicoCore.Data.Entities.Content;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.Interfaces;
using RicoCore.Infrastructure.SharedKernel;
using RicoCore.Services.Content.Posts.Dtos;
using RicoCore.Services.Content.Tags.Dtos;
using RicoCore.Utilities.Constants;
using RicoCore.Utilities.Dtos;
using RicoCore.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RicoCore.Services.Content.Posts
{
    public class PostService : WebServiceBase<Post, Guid, PostViewModel>, IPostService
    {
        private readonly IRepository<Post, Guid> _postRepository;
        private readonly IRepository<Tag, string> _tagRepository;
        private readonly IRepository<PostTag, Guid> _postTagRepository;
        private readonly IRepository<PostImage, Guid> _postImageRepository;
        private readonly IRepository<PostCategory, int> _postCategoryRepository;
        private readonly DbSet<Tag> Tags;
        private readonly DbSet<Post> Posts;
        private readonly DbSet<PostTag> PostTags;
        private readonly IUnitOfWork _unitOfWork;

        public PostService(IRepository<Post, Guid> postRepository,
            IRepository<PostTag, Guid> postTagRepository,
            IRepository<Tag, string> tagRepository,
            IRepository<PostCategory, int> postCategoryRepository,
            IRepository<PostImage, Guid> postImageRepository,
            AppDbContext context,
            IUnitOfWork unitOfWork) : base(postRepository, unitOfWork)
        {
            _postRepository = postRepository;
            _postTagRepository = postTagRepository;
            _tagRepository = tagRepository;
            _postCategoryRepository = postCategoryRepository;
            _postImageRepository = postImageRepository;
            _unitOfWork = unitOfWork;
            Posts = context.Set<Post>();
            PostTags = context.Set<PostTag>();
            Tags = context.Set<Tag>();
        }      
      
        public bool ValidateAddPostName(PostViewModel postVm)
        {
            return _postRepository.GetAll().Any(x => x.Name.ToLower() == postVm.Name.ToLower());
        }
        public bool ValidateUpdatePostName(PostViewModel postVm)
        {
            var compare = _postRepository.GetAllIncluding(x => x.Name.ToLower() == postVm.Name.ToLower() && x.CategoryId == postVm.CategoryId);            
            return compare.Any();
        }

        public bool ValidateAddPostOrder(PostViewModel postVm)
        {
            var compare = 
                _postRepository.GetAllIncluding(x => x.SortOrder == postVm.SortOrder && x.CategoryId == postVm.CategoryId && x.SortOrder != 0);
            return compare.Any();
        }

        public bool ValidateUpdatePostOrder(PostViewModel postVm)
        {
            var compare = _postRepository.GetAllIncluding(x => x.SortOrder == postVm.SortOrder && x.CategoryId == postVm.CategoryId && x.SortOrder != 0);            
            return compare.Any();
        }

        public PostViewModel SetValueToNewPost(int postCategoryId)
        {
            var post = new PostViewModel();

            var postCategory = _postCategoryRepository.GetById(postCategoryId);
            if (postCategory == null)
                throw new Exception("Post Category is null");
           
            post.CategoryId = postCategoryId;
            post.CategoryName = postCategory.Name;
            post.SortOrder = SetNewPostOrder(postCategoryId);            
            return post;
        }
        public int SetNewPostOrder(int categoryId)
        {
            var order = 0;
            var list = _postRepository.GetAllIncluding(x => x.CategoryId == categoryId && x.SortOrder != 0).OrderBy(x => x.SortOrder).Select(x => x.SortOrder).ToList();
            order = CommonMethods.GetOrder(list);
            return order;
        }

        public bool ValidateAddPostHotOrder(PostViewModel postVm)
        {
            var compare = _postRepository.GetAllIncluding(x => x.CategoryId == postVm.CategoryId && x.HotOrder == postVm.HotOrder && x.HotOrder != 0);
            return compare.Any();
        }

        public bool ValidateUpdatePostHotOrder(PostViewModel postVm)
        {
            var compare = _postRepository.GetAllIncluding(x => x.CategoryId == postVm.CategoryId && x.HotOrder == postVm.HotOrder && x.HotOrder != 0);            
            return compare.Any();
        }
        public int SetNewPostHotOrder()
        {
            var order = 0;
            var list = _postRepository.GetAllIncluding(x=>x.HotOrder != 0).OrderBy(x => x.HotOrder).Select(x => x.HotOrder).ToList();
            order = CommonMethods.GetOrder(list);
            return order;
        }

        public bool ValidateAddPostHomeOrder(PostViewModel postVm)
        {
            var compare = _postRepository.GetAllIncluding(x => x.CategoryId == postVm.CategoryId && x.HomeOrder == postVm.HomeOrder && x.HomeOrder != 0);
            return compare.Any();
        }

        public bool ValidateUpdatePostHomeOrder(PostViewModel postVm)
        {
            var compare = _postRepository.GetAllIncluding(x => x.CategoryId == postVm.CategoryId && x.HomeOrder == postVm.HomeOrder && x.HomeOrder != 0);            
            return compare.Any();
        }

        public int SetNewPostHomeOrder()
        {
            var order = 0;
            var list = _postRepository.GetAllIncluding(x => x.HomeOrder != 0).OrderBy(x => x.HomeOrder).Select(x => x.HomeOrder).ToList();
            order = CommonMethods.GetOrder(list);
            return order;
        }

        public PagedResult<PostViewModel> GetPostsByTopic(int categoryId, string keyword, int page, int pageSize, string sortBy)
        {          
            var query = _postRepository.GetAllIncluding(x => x.CategoryId == categoryId);
            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(x => x.MetaKeywords.ToLower().Contains(keyword.ToLower()));
            int totalRow = query.Count();
            switch (sortBy)
            {
                case "theo-ten":
                    query = query.OrderBy(x => x.Name);
                    break;

                case "moi-nhat":
                    query = query.OrderBy(x => x.DateModified);
                    break;

                default:
                    query = query.OrderByDescending(x => x.DateModified);
                    break;
            }           
            var data = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
            var list = new List<PostViewModel>();
            foreach (var item in data)
            {
                var postVm = new PostViewModel()
                {
                    Id = item.Id,
                    CategoryId = item.CategoryId,                   
                    Code = item.Code,
                    Url = item.Url,
                    Name = item.Name,
                    Content = item.Content,
                    Description = item.Description,
                    MetaTitle = item.MetaTitle,
                    MetaDescription = item.MetaDescription,
                    MetaKeywords = item.MetaKeywords,
                    DateCreated = item.DateCreated,
                    DateModified = item.DateModified,
                    DateDeleted = item.DateDeleted,                    
                    HomeOrder = item.HomeOrder,                   
                    HotOrder = item.HotOrder,                  
                    SortOrder = item.SortOrder,
                    Image = item.Image,
                    Tags = item.Tags,
                    Viewed = item.Viewed,
                    HomeFlag = item.HomeFlag,
                    Status = item.Status,
                    HotFlag = item.HotFlag
                };
                list.Add(postVm);
            }
            var paginationSet = new PagedResult<PostViewModel>()
            {
                Results = list,
                //Results = data.ProjectTo<PostViewModel>().ToList(),
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize,
            };
            return paginationSet;
        }

       
        public PostViewModel GetByUrl(string url)
        {
            var post = _postRepository.FirstOrDefault(x => x.Url == url);
            if (post == null) throw new Exception("Null");
            var vm = Mapper.Map<Post, PostViewModel>(post);
            return vm;
        }

        public PagedResult<PostViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize)
        {
            try
            {
                var query = _postRepository.GetAllIncluding(x => x.Status == Status.Actived);
                if (!string.IsNullOrEmpty(keyword))
                    query = query.Where(x => x.Name.Contains(keyword));
                if (categoryId.HasValue)
                    query = query.Where(x => x.CategoryId == categoryId.Value);

                int totalRow = query.Count();

                query = query.OrderByDescending(x => x.DateCreated)
                    .Skip((page - 1) * pageSize).Take(pageSize);

                //var data = query.ProjectTo<PostViewModel>().ToList();
                var data = new List<PostViewModel>();
                foreach (var item in query)
                {
                    var postVm = new PostViewModel()
                    {
                        Id = item.Id,
                        CategoryId = item.CategoryId,
                        Code = item.Code,
                        Url = item.Url,
                        Name = item.Name,
                        Content = item.Content,
                        Description = item.Description,
                        MetaTitle = item.MetaTitle,
                        MetaDescription = item.MetaDescription,
                        MetaKeywords = item.MetaKeywords,
                        DateCreated = item.DateCreated,
                        DateModified = item.DateModified,
                        DateDeleted = item.DateDeleted,                       
                        HomeOrder = item.HomeOrder,                        
                        HotOrder = item.HotOrder,                       
                        SortOrder = item.SortOrder,
                        Image = item.Image,
                        Tags = item.Tags,
                        Viewed = item.Viewed
                    };
                    var postCategory = _postCategoryRepository.GetById(postVm.CategoryId);

                    postVm.CategoryName = postCategory == null
                        ? "Danh mục cấp 1"
                        : postCategory.Name;
                    data.Add(postVm);
                }

                var paginationSet = new PagedResult<PostViewModel>()
                {
                    Results = data,
                    CurrentPage = page,
                    RowCount = totalRow,
                    PageSize = pageSize
                };
                return paginationSet;
            }
            catch (Exception ex)
            {

                throw ex;
            }           
        }


        public List<PostViewModel> GetAllByCategoryId(int categoryId)
        {
           return _postRepository.GetAllIncluding(x => x.CategoryId == categoryId).ProjectTo<PostViewModel>().ToList();
        }
        public List<PostViewModel> GetPostsByTagName(string tagName)
        {
            var posts = _postRepository.GetAllIncluding(x => x.Tags.Contains(tagName));
            var list = (from p in posts
                        select new PostViewModel
                        {
                            CategoryId = p.CategoryId,
                            CategoryName = tagName,
                            Code = p.Code,
                            Content = p.Content,
                            DateCreated = p.DateCreated,
                            DateDeleted = p.DateDeleted,
                            DateModified = p.DateModified,
                            Description = p.Description,
                            HomeFlag = p.HomeFlag,
                            HomeOrder = p.HomeOrder,
                            HotFlag = p.HotFlag,
                            HotOrder = p.HotOrder,
                            Id = p.Id,
                            Image = p.Image,
                            MetaDescription = p.MetaDescription,
                            MetaKeywords = p.MetaKeywords,
                            MetaTitle = p.MetaTitle,
                            Name = p.Name,
                            SortOrder = p.SortOrder,
                            Status = p.Status,
                            Tags = p.Tags,
                            Url = p.Url,
                            Viewed = p.Viewed
                        }).ToList();
            return list;
        }
        public void CreateTagId(ref string tagId, ref List<string> listStr, ref List<int> listDuoi)
        {
            foreach (var item in listStr)
            {
                int idx = item.LastIndexOf('-');
                if (idx != -1)
                {
                    var duoi = item.Substring(idx + 1);
                    if (int.TryParse(item, out var number))
                    {
                        listDuoi.Add(number);
                    }
                }
            }
            listDuoi.Sort();

            if (listDuoi.Count() == 0)
                tagId = tagId + "-" + 1;

            if (listDuoi.Count() == 1)
            {
                if (listDuoi[0] <= 1)
                    tagId = tagId + "-" + (listDuoi[0] + 1);
                else
                    tagId = tagId + "-" + 1;
            }

            if (listDuoi.Count() == 2)
            {
                var rs = listDuoi[1] - listDuoi[0] > 1 ? listDuoi[0] + 1 : listDuoi[1] + 1;
                tagId = tagId + "-" + rs;
            }

            if (listDuoi.Count() > 2)
            {
                for (int i = 0; i < listDuoi.Count(); i++)
                {
                    if (i <= listDuoi.Count() - 2)
                    {
                        if (listDuoi[i + 1] - listDuoi[i] > 1)
                        {
                            tagId = tagId + "-" + (listDuoi[i] + 1);
                            break;
                        }
                    }
                    if (i == listDuoi.Count() - 1)
                    {
                        tagId = tagId + "-" + i;
                        break;
                    }
                }
            }
        }

        public void CreateTagAndPostTag(Post post, PostViewModel postVm)
        {
            var createdDdate = post.DateCreated;

            Mapper.Map(postVm, post);
            if (string.IsNullOrWhiteSpace(postVm.Url))
            {
                post.Url = TextHelper.ToUnsignString(postVm.Name);
            }
            else
            {
                post.Url = postVm.Url.ToLower();
            }
            var query = _postRepository.FirstOrDefault(x => x.Url == post.Url);
            if (query != null)
            {
                post.Url = $"{query.Url}-{post.Code.ToLower()}";
            }
            post.DateCreated = createdDdate;

            if (!string.IsNullOrWhiteSpace(post.Tags))
            {
                string[] tags = post.Tags.Split(',');
                var filteredTagList = new List<string>();
                foreach (string t in tags)
                {
                    if (!string.IsNullOrWhiteSpace(t))
                    {
                        var tagId = TextHelper.ToUnsignString(t);

                        var listStr = new List<string>();
                        var listDuoi = new List<int>();
                        for (int i = 0; i < 15; i++)
                        {
                            var check = i == 0 ? _tagRepository.FirstOrDefault(x => x.Id == tagId) : _tagRepository.FirstOrDefault(x => x.Id == tagId + "-" + i + "");
                            if (check != null)
                            {
                                listStr.Add(check.Id);
                            }
                        }

                        if (listStr.Count() > 0)
                        {
                            CreateTagId(ref tagId, ref listStr, ref listDuoi);
                        }

                        if (!_tagRepository.GetAll().Where(x => x.Id == tagId).Any())
                        {
                            var tagOrder = SetNewTagOrder(out var lstOrder);
                            var tag = new Tag
                            {
                                Id = tagId,
                                Name = t.Trim(),
                                //Type = Data.Enums.TagType.Product
                                Type = CommonConstants.PostTag,
                                MetaTitle = t.Trim(),
                                MetaDescription = t.Trim(),
                                MetaKeywords = t.Trim(),
                                SortOrder = tagOrder,
                                DateCreated = DateTime.Now,
                                DateModified = DateTime.Now
                            };
                            _tagRepository.Insert(tag);
                            _unitOfWork.Commit();
                       

                            var postTag = new PostTag
                            {
                                Id = Guid.NewGuid(),
                                PostId = post.Id,
                                TagId = tagId
                            };

                        _postTagRepository.Insert(postTag);
                        _unitOfWork.Commit();
                        filteredTagList.Add(t.Trim());
                        }
                    }
                }
                var filteredTagsArray = filteredTagList.ToArray();
                var filteredTagStr = string.Join(",", filteredTagsArray);
                post.Tags = filteredTagStr;
            }
            _postRepository.Update(post);
        }
        
        public int SetNewTagOrder(out List<int> list)
        {
            var order = 0;
            //list = _tagRepository.GetAll().Select(x => x.SortOrder).ToList();
            list = Tags.OrderBy(x=>x.SortOrder).Select(x => x.SortOrder).ToList();

            if (list.Count() == 0)
                order = 1;

            if (list.Count() == 1)
            {
                if (list[0] <= 1)
                    order = list[0] + 1;
                else
                    order = 1;
            }

            if (list.Count() == 2)
            {
                var temp = 0;
                var max = Math.Max(list[list.Count() - 1], list[list.Count() - 2]);
                var min = Math.Min(list[list.Count() - 1], list[list.Count() - 2]);
                if (min > 1)
                {
                    temp = 1;
                }
                else
                {
                    if (list[list.Count() - 1] == max)
                    {
                        temp = list[list.Count() - 1] - list[list.Count() - 2] > 1 ? list[list.Count() - 2] + 1 : list[list.Count() - 1] + 1;
                    }
                    if (list[list.Count() - 2] == max)
                    {
                        temp = list[list.Count() - 2] - list[list.Count() - 1] > 1 ? list[list.Count() - 1] + 1 : list[list.Count() - 2] + 1;
                    }
                }                               
                order = temp;
            }

            if (list.Count() > 2)
            {                
                for (int i = 0; i < list.Count(); i++)
                {
                    if (i <= list.Count() - 2)
                    {
                        if (list[i + 1] - list[i] > 1)
                        {
                            order = list[i] + 1;
                            break;
                        }
                            
                    }
                    if (i == list.Count() - 1)
                    {
                        order = list[list.Count() - 1] + 1;
                        break;
                    }                        
                }
            }
            return order;
        }
        
        public override void Add(PostViewModel postVm)
        {
            postVm.Status = postVm.OrderStatus == true ? Status.Actived : Status.InActived;
            postVm.HotFlag = postVm.HotOrderStatus == true ? Status.Actived : Status.InActived;
            postVm.HomeFlag = postVm.HomeOrderStatus == true ? Status.Actived : Status.InActived;
            var post = Mapper.Map<PostViewModel, Post>(postVm);
            if (!string.IsNullOrWhiteSpace(postVm.Name))
            {
                post.Name = postVm.Name.Trim();
            }
            var getUrlFromName = TextHelper.ToUnsignString(post.Name.ToLower());
            if (string.IsNullOrWhiteSpace(postVm.Url) || postVm.Url.ToLower().Trim() == getUrlFromName)
                post.Url = getUrlFromName;
            if(!string.IsNullOrWhiteSpace(postVm.Url) && postVm.Url.ToLower().Trim() != getUrlFromName)
                post.Url = postVm.Url.ToLower().Trim();
            post.Tags = !string.IsNullOrWhiteSpace(post.Tags) ? post.Tags.Trim() : string.Empty;
            post.Code = GenerateCode();
            var query = _postRepository.FirstOrDefault(x => x.Url == post.Url);
            if (query != null)
            {
                post.Url = $"{query.Url}-{post.Code.ToLower()}";
            }           
            post.Id = Guid.NewGuid();
            if (!string.IsNullOrWhiteSpace(post.Tags))
            {
                var tags = post.Tags.Split(',');
                var filteredTagList = new List<string>();                
                foreach (string t in tags)
                {
                    if (!string.IsNullOrWhiteSpace(t))
                    {
                        var tagId = TextHelper.ToUnsignString(t);

                        var listStr = new List<string>();                        
                        for (int i = 0; i < 15; i++)
                        {
                            var check = i == 0 ? _tagRepository.FirstOrDefault(x => x.Id == tagId) : _tagRepository.FirstOrDefault(x => x.Id == tagId + "-" + i + "");
                            if (check != null)
                            {
                                var tagIdIsNotNull = check.Id;
                                PostTag postTag = new PostTag
                                {
                                    Id = Guid.NewGuid(),
                                    PostId = post.Id,
                                    TagId = tagIdIsNotNull
                                };
                                _postTagRepository.Insert(postTag);
                                listStr.Add(tagIdIsNotNull);
                                filteredTagList.Add(t.Trim());
                                _unitOfWork.Commit();
                            }
                        }
                        if (listStr.Count() == 0)
                        {                            
                            var tagOrder = SetNewTagOrder(out var lstOrder);
                            var tag = new Tag
                            {
                                Id = tagId,
                                Name = t.Trim(),
                                //Type = Data.Enums.TagType.Product
                                Type = CommonConstants.PostTag,
                                MetaTitle = t.Trim(),
                                MetaDescription = t.Trim(),
                                MetaKeywords = t.Trim(),
                                SortOrder = tagOrder,
                                DateCreated = DateTime.Now,
                                DateModified = DateTime.Now
                            };
                            _tagRepository.Insert(tag);                            

                            var postTag = new PostTag
                            {
                                Id = Guid.NewGuid(),
                                PostId = post.Id,
                                TagId = tagId
                            };
                            _postTagRepository.Insert(postTag);
                            filteredTagList.Add(t.Trim());

                            _unitOfWork.Commit();
                        }                                                                      
                    }                                        
                }
                var filteredTagsArray = filteredTagList.ToArray();
                var tagsStr = string.Join(",", filteredTagsArray);
                post.Tags = tagsStr;
            }            
            _postRepository.Insert(post);
        }

        public PagedResult<PostViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize, string sort)
        {
            var query = _postRepository.GetAll();
            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(x => x.Name.Contains(keyword));

            if (categoryId.HasValue)
                query = query.Where(x => x.CategoryId == categoryId);

            int totalRow = query.Count();
            //switch (sort)
            //{
            //    case "oldest":
            //        query = query.OrderBy(x => x.DateModified);
            //        break;

            //    default:
            //        query = query.OrderByDescending(x => x.SortOrder);
            //        break;
            //}
            query = query.OrderByDescending(x => x.DateModified);
            var data = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var list = new List<PostViewModel>();
            foreach (var item in data)
            {
                var postVm = new PostViewModel()
                {
                    Id = item.Id,
                    CategoryId = item.CategoryId,
                    Code = item.Code,
                    Url = item.Url,
                    Name = item.Name,
                    Content = item.Content,
                    Description = item.Description,
                    MetaTitle = item.MetaTitle,
                    MetaDescription = item.MetaDescription,
                    MetaKeywords = item.MetaKeywords,
                    DateCreated = item.DateCreated,
                    DateModified = item.DateModified,
                    DateDeleted = item.DateDeleted,                   
                    HomeOrder = item.HomeOrder,                   
                    HotOrder = item.HotOrder,                    
                    SortOrder = item.SortOrder,
                    Image = item.Image,
                    Tags = item.Tags,
                    Viewed = item.Viewed,
                    HomeFlag = item.HomeFlag,
                    Status = item.Status,
                    HotFlag = item.HotFlag
                };
                var postCategory = _postCategoryRepository.GetById(postVm.CategoryId);
                
                postVm.CategoryName = postCategory == null
                    ? "Danh mục cấp 1"
                    : postCategory.Name;
                list.Add(postVm);
            }

            var paginationSet = new PagedResult<PostViewModel>()
            {
                Results = list,
                //Results = data.ProjectTo<PostViewModel>().ToList(),
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize,
            };

            return paginationSet;
        }

        public PagedResult<PostViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            var query = _postRepository.GetAll();
            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(x => x.Name.Contains(keyword));            

            int totalRow = query.Count();
            //switch (sort)
            //{
            //    case "oldest":
            //        query = query.OrderBy(x => x.DateModified);
            //        break;

            //    default:
            //        query = query.OrderByDescending(x => x.SortOrder);
            //        break;
            //}
            query = query.OrderByDescending(x => x.DateModified);
            var data = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var list = new List<PostViewModel>();
            foreach (var item in data)
            {
                var postVm = new PostViewModel()
                {
                    Id = item.Id,
                    CategoryId = item.CategoryId,
                    Code = item.Code,
                    Url = item.Url,
                    Name = item.Name,
                    Content = item.Content,
                    Description = item.Description,
                    MetaTitle = item.MetaTitle,
                    MetaDescription = item.MetaDescription,
                    MetaKeywords = item.MetaKeywords,
                    DateCreated = item.DateCreated,
                    DateModified = item.DateModified,
                    DateDeleted = item.DateDeleted,                   
                    HomeOrder = item.HomeOrder,                   
                    HotOrder = item.HotOrder,                   
                    SortOrder = item.SortOrder,
                    Image = item.Image,
                    Tags = item.Tags,
                    Viewed = item.Viewed,
                    HomeFlag = item.HomeFlag,
                    Status = item.Status,
                    HotFlag = item.HotFlag
                };
                var postCategory = _postCategoryRepository.GetById(postVm.CategoryId);

                postVm.CategoryName = postCategory == null
                    ? "Danh mục cấp 1"
                    : postCategory.Name;
                list.Add(postVm);
            }

            var paginationSet = new PagedResult<PostViewModel>()
            {
                Results = list,
                //Results = data.ProjectTo<PostViewModel>().ToList(),
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize,
            };

            return paginationSet;
        }

        public override PostViewModel GetById(Guid id)
        {
            //var query = (from pc in _postCategoryRepository.GetAll()
            //             join p in _postRepository.GetAll()
            //                 on pc.Id equals p.CategoryId
            //             where pc.Id == id && p.CategoryId == id
            //             select new { p });

            var post = (from pcat in _postRepository.GetAll() where pcat.Id == id select new { pcat }).FirstOrDefault();

            var categoryName = _postCategoryRepository.GetAllIncluding(x => x.Id == post.pcat.CategoryId).Select(x => x.Name).FirstOrDefault();

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

            var model = new PostViewModel()
            {
                Name = post.pcat.Name,
                Code = post.pcat.Code,
                Id = post.pcat.Id,
                CategoryId = post.pcat.CategoryId,
                CategoryName = categoryName,
                Url = post.pcat.Url,
                Tags = post.pcat.Tags,
                Description = post.pcat.Description,
                Content = post.pcat.Content,
                Image = post.pcat.Image,
                MetaTitle = post.pcat.MetaTitle,
                MetaDescription = post.pcat.MetaDescription,
                MetaKeywords = post.pcat.MetaKeywords,
                HotOrder = post.pcat.HotOrder,
                //HotFlag = post.pcat.HotFlag,
                HomeOrder = post.pcat.HomeOrder,
                //HomeFlag = post.pcat.HomeFlag,
                SortOrder = post.pcat.SortOrder,
                DateCreated = post.pcat.DateCreated,
                DateModified = post.pcat.DateModified,
                //Products = productlist,
                //Status = post.pcat.Status,
                HomeFlag = post.pcat.HomeFlag,
                Status = post.pcat.Status,
                HotFlag = post.pcat.HotFlag,
                OrderStatus = post.pcat.Status == Status.Actived ? true : false,
                HotOrderStatus = post.pcat.HotFlag == Status.Actived ? true : false,
                HomeOrderStatus = post.pcat.HomeFlag == Status.Actived ? true : false,
            };

            return model;
        }           
        
        public void Update(PostViewModel postVm, string oldTags)
        {
            try
            {
                var post = _postRepository.GetById(postVm.Id);
                var createdDate = post.DateCreated;

                //var postTemp = Posts.FirstOrDefault(x => x.Id == postVm.Id);
                //var createdDate = postTemp.DateCreated;

                postVm.Status = postVm.OrderStatus == true ? Status.Actived : Status.InActived;
                postVm.HotFlag = postVm.HotOrderStatus == true ? Status.Actived : Status.InActived;
                postVm.HomeFlag = postVm.HomeOrderStatus == true ? Status.Actived : Status.InActived;

                Mapper.Map(postVm, post);
                post.DateCreated = createdDate;
                var postId = post.Id;
                post.Tags = !string.IsNullOrWhiteSpace(postVm.Tags) ? postVm.Tags.Trim() : string.Empty;
                var newTags = post.Tags;

                if (!string.IsNullOrWhiteSpace(postVm.Name))
                {
                    post.Name = postVm.Name.Trim();
                }
                var getUrlFromName = TextHelper.ToUnsignString(post.Name.ToLower());
                if (string.IsNullOrWhiteSpace(postVm.Code))
                {
                    postVm.Code = GenerateCode();
                }
                post.Code = postVm.Code;
                if(string.IsNullOrWhiteSpace(postVm.Url) || postVm.Url.ToLower().Trim() == getUrlFromName )
                post.Url = TextHelper.ToUnsignString(post.Name.ToLower().Trim());

                if (!string.IsNullOrWhiteSpace(postVm.Url) && postVm.Url.ToLower().Trim() != getUrlFromName)
                    post.Url = postVm.Url.ToLower().Trim();

                    var query = _postRepository.GetAllIncluding(x => x.Url == post.Url).ToList();
                if (query.Count() > 1)
                {
                    post.Url = $"{query[0].Url}-{post.Code.ToLower()}";
                }
                
                string[] oldTagsArr = !string.IsNullOrWhiteSpace(oldTags) ? oldTags.Trim().Split(',') : new string[] { };
                string[] newTagsArr = !string.IsNullOrWhiteSpace(newTags) ? newTags.Split(",") : new string[] { };
                var lstTag = new List<string>();
                var newTagsStr = "";

                if (oldTagsArr.Length != 0)
                {
                    foreach (var oldTag in oldTagsArr)
                    {
                        if (!string.IsNullOrWhiteSpace(oldTag))
                        {
                            var oldTagId = TextHelper.ToUnsignString(oldTag);
                            var pt = _postTagRepository.FirstOrDefault(x => x.TagId == oldTagId);
                            if (pt != null)
                            {
                                _postTagRepository.Delete(pt);
                                _unitOfWork.Commit();
                            }
                        }
                    }
                }

                if (newTagsArr.Length != 0)
                {
                    foreach (var newTag in newTagsArr)
                    {
                        if (!string.IsNullOrWhiteSpace(newTag))
                        {
                            var newTagId = TextHelper.ToUnsignString(newTag);
                            var isExistTag = false;
                            var listStr = new List<string>();
                            for (int i = 0; i < 15; i++)
                            {
                                var check = i == 0 ? _tagRepository.FirstOrDefault(x => x.Id == newTagId) : _tagRepository.FirstOrDefault(x => x.Id == newTagId + "-" + i + "");
                                if (check != null)
                                {
                                    var tagIdIsNotNull = check.Id;
                                    //listStr.Add(tagIdIsNotNull);
                                    isExistTag = true;
                                    var postTag = new PostTag
                                    {
                                        Id = Guid.NewGuid(),
                                        PostId = post.Id,
                                        TagId = tagIdIsNotNull
                                    };
                                    _postTagRepository.Insert(postTag);
                                    _unitOfWork.Commit();
                                }
                            }
                            //if (listStr.Count() == 0)
                            if (isExistTag == false)
                            {
                                var tagOrder = SetNewTagOrder(out var lstOrder);
                                var tag = new Tag
                                {
                                    Id = newTagId,
                                    Name = newTag.Trim(),
                                    //Type = Data.Enums.TagType.Product
                                    Type = CommonConstants.PostTag,
                                    MetaTitle = newTag.Trim(),
                                    MetaDescription = newTag.Trim(),
                                    MetaKeywords = newTag.Trim(),
                                    SortOrder = tagOrder,
                                    DateCreated = DateTime.Now,
                                    DateModified = DateTime.Now
                                };
                                _tagRepository.Insert(tag);

                                var postTag = new PostTag
                                {
                                    Id = Guid.NewGuid(),
                                    PostId = post.Id,
                                    TagId = newTagId
                                };
                                _postTagRepository.Insert(postTag);
                                _unitOfWork.Commit();
                            }
                            lstTag.Add(newTag);
                        }
                    }
                    newTagsStr = string.Join(",", lstTag);
                    post.Tags = newTagsStr;
                }
                // ko the tracked
                _postRepository.Update(post);
            }
            catch (Exception ex) 
            {
                throw ex;
            }
            
        }

        public List<PostViewModel> GetLastest(int top)
        {
            return _postRepository.GetAll().Where(x => x.Status == Status.Actived).OrderByDescending(x => x.DateCreated)
                .Take(top).ProjectTo<PostViewModel>().ToList();
        }

        public List<PostViewModel> GetHotPost(int top)
        {
            return _postRepository.GetAll().Where(x => x.Status == Status.Actived && x.HotFlag == Status.Actived)
                .OrderByDescending(x => x.HotOrder)
                .Take(top)
                .ProjectTo<PostViewModel>()
                .ToList();
        }

        public List<PostViewModel> GetListPaging(int page, int pageSize, string sort, out int totalRow)
        {
            var query = _postRepository.GetAll().Where(x => x.Status == Status.Actived);

            switch (sort)
            {
                case "popular":
                    query = query.OrderByDescending(x => x.Viewed);
                    break;

                default:
                    query = query.OrderByDescending(x => x.DateModified);
                    break;
            }

            totalRow = query.Count();

            return query.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<PostViewModel>().ToList();
        }

        public List<string> GetListByName(string name)
        {
            return _postRepository.GetAll().Where(x => x.Status == Status.Actived
            && x.Name.Contains(name)).Select(y => y.Name).ToList();
        }

        public List<PostViewModel> Search(string keyword, int page, int pageSize, string sort, out int totalRow)
        {
            var query = _postRepository.GetAll().Where(x => x.Status == Status.Actived
            && x.Name.Contains(keyword));

            switch (sort)
            {
                case "popular":
                    query = query.OrderByDescending(x => x.Viewed);
                    break;

                default:
                    query = query.OrderByDescending(x => x.DateModified);
                    break;
            }

            totalRow = query.Count();

            return query.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<PostViewModel>()
                .ToList();
        }

        public List<PostViewModel> GetReatedBlogs(Guid id, int top)
        {
            return _postRepository.GetAll().Where(x => x.Status == Status.Actived
                && x.Id != id)
            .OrderByDescending(x => x.DateModified)
            .Take(top)
            .ProjectTo<PostViewModel>()
            .ToList();
        }

        public List<TagViewModel> GetListTagById(Guid id)
        {
            return _postTagRepository.GetAll().Where(x => x.PostId == id)
                .Select(y => y.TagId)
                .ProjectTo<TagViewModel>()
                .ToList();
            //throw new NotImplementedException();
        }

        public void IncreaseView(Guid id)
        {
            var post = _postRepository.GetById(id);
            if (post.Viewed.ToString() != null)
                post.Viewed += 1;
            else
                post.Viewed = 1;
        }

        public List<PostViewModel> GetListByTag(string tagId, int page, int pageSize, out int totalRow)
        {
            var query = from p in _postRepository.GetAll()
                        join pt in _postTagRepository.GetAll()
                        on p.Id equals pt.PostId
                        where pt.TagId == tagId && p.Status == Status.Actived
                        orderby p.DateModified descending
                        select p;

            totalRow = query.Count();

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var model = query.ProjectTo<PostViewModel>();
            return model.ToList();
        }

        public TagViewModel GetTag(string tagId)
        {
            return Mapper.Map<Tag, TagViewModel>(_tagRepository.FirstOrDefault(x => x.Id == tagId));
        }

        public List<PostViewModel> GetList(string keyword)
        {
            var query = !string.IsNullOrWhiteSpace(keyword) ?
                _postRepository.GetAll().Where(x => x.Name.Contains(keyword)).ProjectTo<PostViewModel>()
                : _postRepository.GetAll().ProjectTo<PostViewModel>();
            return query.ToList();
        }

        public List<TagViewModel> GetListTag(string searchText)
        {
            //return _tagRepository.GetAll().Where(x => x.Type == Data.Enums.TagType.Content
            return _tagRepository.GetAll().Where(x => x.Type == CommonConstants.PostTag
            && searchText.Contains(x.Name)).ProjectTo<TagViewModel>().ToList();
        }

       

        private string GenerateCode()
        {
            var code = CommonHelper.Generate6CharsRandomCode();
            while (HasExistsCode(code))
            {
                code = CommonHelper.Generate6CharsRandomCode();
            }
            return code;
        }

        /// <summary>
        /// Check exist code
        /// </summary>
        /// <param name="postCode"></param>
        /// <returns></returns>
        public virtual bool HasExistsCode(string code)
        {
            return _postRepository.GetAll().Any(x => x.Code == code);
        }

        public List<PostImageViewModel> GetImages(Guid postId)
        {
            return _postImageRepository.GetAllIncluding(x => x.PostId == postId)
                .ProjectTo<PostImageViewModel>().ToList();
        }

        public void AddImages(Guid postId, string[] images)
        {
            _postImageRepository.DeleteMultiple(_postImageRepository.GetAllIncluding(x => x.PostId == postId).ToList());
            foreach (var image in images)
            {
                _postImageRepository.Insert(new PostImage()
                {
                    Path = image,
                    PostId = postId,
                    Caption = string.Empty
                });
            }
        }

        public override void Delete(Guid id)
        {
            _postRepository.Delete(id);
            var lstPostTag = _postTagRepository.GetAllIncluding(x => x.PostId == id).ToList();
            PostTags.RemoveRange(lstPostTag);
        }

        public void MultiDelete(IList<string> selectedIds)
        {
            foreach (var item in selectedIds)
            {
                var post = _postRepository.FirstOrDefault(x => x.Id == Guid.Parse(item));
                var postId = post.Id;
                _postRepository.Delete(postId);

                //var post = Posts.Where(r => r.Id == Guid.Parse(item)).FirstOrDefault();
                //Posts.Remove(post);
                
                var lstPostTag = _postTagRepository.GetAllIncluding(x => x.PostId == postId).ToList();
                PostTags.RemoveRange(lstPostTag);
            }
            _unitOfWork.Commit();
        }

        public IList<string> GetIds(int categoryId)
        {
            var list = _postRepository.GetAllIncluding(x => x.CategoryId == categoryId).Select(x => x.Id);
            var lstIds = new List<string>();
            foreach (var item in list)
            {
                lstIds.Add(item.ToString());
            }
            return lstIds;
        }

        public void MultiDeleteByCategoryId(int id)
        {
            var list = _postRepository.GetAllIncluding(x => x.CategoryId == id).ToList();
            if (list != null)
                _postRepository.DeleteMultiple(list);
            _unitOfWork.Commit();
        }

        public void ImportExcel(string filePath, int categoryId)
        {
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];               
                for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                {
                    bool.TryParse(workSheet.Cells[i, 5].Value.ToString(), out var orderStatus);
                    int.TryParse(workSheet.Cells[i, 6].Value.ToString(), out var order);
                    bool.TryParse(workSheet.Cells[i, 7].Value.ToString(), out var hotOrderStatus);
                    int.TryParse(workSheet.Cells[i, 8].Value.ToString(), out var hotOrder);
                    bool.TryParse(workSheet.Cells[i, 9].Value.ToString(), out var homeOrderStatus);
                    int.TryParse(workSheet.Cells[i, 10].Value.ToString(), out var homeOrder);
                    int.TryParse(workSheet.Cells[i, 17].Value.ToString(), out var view);
                    DateTime.TryParse(workSheet.Cells[i, 18].Value.ToString(), out var createDate);
                    DateTime.TryParse(workSheet.Cells[i, 19].Value.ToString(), out var modifyDate);
                    DateTime.TryParse(workSheet.Cells[i, 20].Value.ToString(), out var deleteDate);

                    Post post = new Post {
                    CategoryId = categoryId,
                    Name = workSheet.Cells[i, 1].Value.ToString(),
                    Code = workSheet.Cells[i, 2].Value.ToString(),
                    Url = workSheet.Cells[i, 3].Value.ToString(),
                    Content = workSheet.Cells[i, 4].Value.ToString(),
                    Status = orderStatus == true ? Status.Actived : Status.InActived,
                    SortOrder = order,
                    HotFlag = hotOrderStatus == true ? Status.Actived : Status.InActived,
                    HotOrder = hotOrder,
                    HomeFlag = homeOrderStatus == true ? Status.Actived : Status.InActived,
                    HomeOrder = homeOrder,
                    Tags = workSheet.Cells[i, 11].Value.ToString(),
                    MetaTitle = workSheet.Cells[i, 12].Value.ToString(),
                    MetaDescription = workSheet.Cells[i, 13].Value.ToString(),
                    MetaKeywords = workSheet.Cells[i, 14].Value.ToString(),
                    Description = workSheet.Cells[i, 15].Value.ToString(),
                    Image = workSheet.Cells[i, 16].Value.ToString(),
                    Viewed = view,
                    DateCreated = createDate,
                    DateModified = modifyDate,
                    DateDeleted = deleteDate
                    };                   
                    _postRepository.Insert(post);
                }
            }
        }

        public List<PostExportViewModel> GetAllToExport(int? categoryId)
        {            
            var list = categoryId.HasValue
                ? from a in _postRepository.GetAllIncluding(x => x.CategoryId == categoryId)
                  select new PostExportViewModel
                  {
                    CategoryId = a.CategoryId, Code = a.Code, Content = a.Content, Description = a.Description, HomeFlag = a.HomeFlag, HomeOrder = a.HomeOrder, HotFlag = a.HotFlag,
                    HotOrder = a.HotOrder, Image = a.Image, MetaDescription = a.MetaDescription, MetaKeywords = a.MetaKeywords, MetaTitle = a.MetaTitle, Name = a.Name,
                    SortOrder = a.SortOrder, Status = a.Status, Tags = a.Tags, Url = a.Url, Viewed = a.Viewed
                }
                : from a in _postRepository.GetAll()
                  select new PostExportViewModel
                  {
                      CategoryId = a.CategoryId,
                      Code = a.Code,
                      Content = a.Content,
                      Description = a.Description,
                      HomeFlag = a.HomeFlag,
                      HomeOrder = a.HomeOrder,
                      HotFlag = a.HotFlag,
                      HotOrder = a.HotOrder,
                      Image = a.Image,
                      MetaDescription = a.MetaDescription,
                      MetaKeywords = a.MetaKeywords,
                      MetaTitle = a.MetaTitle,
                      Name = a.Name,
                      SortOrder = a.SortOrder,
                      Status = a.Status,
                      Tags = a.Tags,
                      Url = a.Url,
                      Viewed = a.Viewed
                  };
            //var result =  list.Select(x => new { x.Domain, x.Level, x.HiddenPassword, x.Note, x.Order, x.Password, x.PasswordId, x.Phone, x.SecurityEmail, x.Url, x.UserName }).ToList();
            return list.ToList();
        }
        public List<PostExportViewModel> GetAllToExport()
        {          
            var list = from a in _postRepository.GetAll()
                       select new PostExportViewModel
                       {
                           CategoryId = a.CategoryId,
                           Code = a.Code,
                           Content = a.Content,
                           Description = a.Description,
                           HomeFlag = a.HomeFlag,
                           HomeOrder = a.HomeOrder,
                           HotFlag = a.HotFlag,
                           HotOrder = a.HotOrder,
                           Image = a.Image,
                           MetaDescription = a.MetaDescription,
                           MetaKeywords = a.MetaKeywords,
                           MetaTitle = a.MetaTitle,
                           Name = a.Name,
                           SortOrder = a.SortOrder,
                           Status = a.Status,
                           Tags = a.Tags,
                           Url = a.Url,
                           Viewed = a.Viewed
                       };                
            //var result =  list.Select(x => new { x.Domain, x.Level, x.HiddenPassword, x.Note, x.Order, x.Password, x.PasswordId, x.Phone, x.SecurityEmail, x.Url, x.UserName }).ToList();
            return list.ToList();
        }

        public PagedResult<PostViewModel> GetPostsByTagPaging(string tag, string keyword, int page, int pageSize, string sort)
        {
            try
            {
                var query = _postRepository.GetAllIncluding(x => x.Tags.Contains(tag));
                if (!string.IsNullOrEmpty(keyword))
                    query = query.Where(x => x.Name.Contains(keyword));                

                int totalRow = query.Count();

                query = query.OrderByDescending(x => x.DateCreated)
                    .Skip((page - 1) * pageSize).Take(pageSize);

                //var data = query.ProjectTo<PostViewModel>().ToList();
                var data = query.ProjectTo<PostViewModel>().ToList();          

                var paginationSet = new PagedResult<PostViewModel>()
                {
                    Results = data,
                    CurrentPage = page,
                    RowCount = totalRow,
                    PageSize = pageSize
                };
                return paginationSet;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}