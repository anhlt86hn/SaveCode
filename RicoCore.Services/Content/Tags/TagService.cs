using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RicoCore.Data.EF;
using RicoCore.Data.Entities.Content;
using RicoCore.Infrastructure.Interfaces;
using RicoCore.Services.Content.Posts.Dtos;
using RicoCore.Services.Content.Tags.Dtos;
using RicoCore.Utilities.Constants;
using RicoCore.Utilities.Dtos;
using RicoCore.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RicoCore.Services.Content.Tags
{
    public class TagService : WebServiceBase<Tag, string, TagViewModel>, ITagService
    {
        private readonly IRepository<Post, Guid> _postRepository;
        private readonly IRepository<Tag, string> _tagRepository;
        private readonly IRepository<PostTag, Guid> _postTagRepository;
        private readonly DbSet<Tag> Tags;
        private readonly DbSet<PostTag> PostTags;
        private readonly IUnitOfWork _unitOfWork;

        public TagService(IRepository<Post, Guid> postRepository,
            IRepository<PostTag, Guid> postTagRepository,
            IRepository<Tag, string> tagRepository,
            AppDbContext context,
            IUnitOfWork unitOfWork) : base(tagRepository, unitOfWork)
        {
            _postRepository = postRepository;
            _postTagRepository = postTagRepository;
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
            Tags = context.Set<Tag>();
            PostTags = context.Set<PostTag>();
        }


        public bool ValidateAddTagName(TagViewModel tagVm, string type)
        {                      
            return _tagRepository.GetAll().Any(x => x.Name.ToLower().Trim() == tagVm.Name.ToLower().Trim() && x.Type == type);
        }
        public bool ValidateUpdateTagName(TagViewModel tagVm, string type)
        {
            var compare = _tagRepository.GetAllIncluding(x => x.Name.ToLower() == tagVm.Name.ToLower() && x.Type == type);
            var result = compare.Count() > 1 ? true : false;
            return result;
        }

        public bool ValidateAddTagOrder(TagViewModel tagVm, string type)
        {
            return _tagRepository.GetAll().Any(x => x.SortOrder == tagVm.SortOrder && x.Type == type);
        }
        public bool ValidateUpdateTagOrder(TagViewModel tagVm, string type)
        {
            var compare = _tagRepository.GetAllIncluding(x => x.SortOrder == tagVm.SortOrder && x.Type == type);
            var result = compare.Count() > 1 ? true : false;
            return result;
        }      
        public int SetNewTagOrder(out List<int> list, string typeTag)
        {
            var order = 0;
            list = _tagRepository.GetAllIncluding(x => x.Type == typeTag).OrderBy(x => x.SortOrder).Select(x => x.SortOrder).ToList();
            order = CommonMethods.GetOrder(list);
            return order;
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
                            var check = i == 0 ? _tagRepository.FirstOrDefault(x => x.Id == tagId) : _tagRepository.FirstOrDefault(x => x.Id == tagId + "-" + (i + 1) + "");
                            if (check != null)
                            {
                                listStr.Add(check.Id);
                            }
                        }
                        if (listStr.Count() == 0)
                        {
                            listStr.Add(tagId);
                        }

                        if (listStr.Count() > 0)
                        {
                            CreateTagId(ref tagId, ref listStr, ref listDuoi);
                        }

                        //if (!_tagRepository.GetAll().Where(x => x.Id == tagId).Any())
                        //{
                        var tagOrder = SetNewTagOrder(out var lstOrder, CommonConstants.PostTag);
                        Tag tag = new Tag
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
                        //}

                        PostTag postTag = new PostTag
                        {
                            Id = Guid.NewGuid(),
                            PostId = post.Id,
                            TagId = tagId
                        };
                        _postTagRepository.Insert(postTag);
                        filteredTagList.Add(t.Trim());
                    }
                }
                var filteredTagsArray = filteredTagList.ToArray();
                var filteredTagStr = string.Join(",", filteredTagsArray);
                post.Tags = filteredTagStr;
            }
            _postRepository.Update(post);
        }

        public void CheckNewTag(out string newTagId, string oldTagId, Post post, PostViewModel postVm)
        {
            newTagId = string.Empty;
            string[] newTagsArr = postVm.Tags.Trim().Split(',');

            if (newTagsArr.Length == 0)
            {
                foreach (var newTag in newTagsArr)
                {
                    if (!string.IsNullOrWhiteSpace(newTag))
                    {
                        newTagId = TextHelper.ToUnsignString(newTag);
                        if (newTagId == oldTagId)
                            break;
                        CreateTagAndPostTag(post, postVm);
                    }
                }
            }
        }
        

        public void Add(TagViewModel tagVm, string typeTag)
        {
            var name = tagVm.Name.Trim();
            if (!string.IsNullOrWhiteSpace(name))
            {
                tagVm.Id = TextHelper.ToUnsignString(name);
                tagVm.Name = name;
                tagVm.Type = typeTag;
                tagVm.MetaTitle = !string.IsNullOrWhiteSpace(name) ? name : string.Empty;
                tagVm.MetaDescription = !string.IsNullOrWhiteSpace(name) ? name : string.Empty;
                tagVm.MetaKeywords = !string.IsNullOrWhiteSpace(name) ? name : string.Empty;
                var tagId = tagVm.Id;
                var tagName = tagVm.Name;
                var tagMetaTitle = tagVm.MetaTitle;
                var tagMetaDescription = tagVm.MetaDescription;
                var tagMetaKeywords = tagVm.MetaKeywords;

                //var listStr = new List<string>();
                //var listDuoi = new List<int>();
                var isExistTag = false;
                for (int i = 0; i < 15; i++)
                {
                    var check = i == 0 ? _tagRepository.FirstOrDefault(x => x.Id == tagId) : _tagRepository.FirstOrDefault(x => x.Id == tagId + "-" + i + "");
                    if (check != null)
                    {
                        isExistTag = true;
                    }
                }

                //if (listStr.Count() == 0)
                if (isExistTag == false)
                {
                    var tagOrder = SetNewTagOrder(out var lstOrder, typeTag);
                    var newTag = new Tag
                    {
                        Id = tagId,
                        Name = tagName,
                        //Type = Data.Enums.TagType.Product
                        Type = typeTag,
                        MetaTitle = tagMetaTitle,
                        MetaDescription = tagMetaDescription,
                        MetaKeywords = tagMetaKeywords,
                        SortOrder = tagOrder,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now
                    };
                    _tagRepository.Insert(newTag);
                    _unitOfWork.Commit();
                }
            }
            
        }

        public PagedResult<TagViewModel> GetPostTagAllPaging(string keyword, int page, int pageSize, string sort)
        {
            var query = _tagRepository.GetAllIncluding(x => x.Type == CommonConstants.PostTag);
            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(x => x.Name.Contains(keyword));

            int totalRow = query.Count();
            switch (sort)
            {
                case "oldest":
                    query = query.OrderBy(x => x.DateModified);
                    break;

                default:
                    query = query.OrderByDescending(x => x.SortOrder);
                    break;
            }
            var data = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var list = new List<TagViewModel>();
            foreach (var item in data)
            {
                var tagVm = new TagViewModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Type = item.Type,
                    MetaTitle = item.MetaTitle,
                    MetaDescription = item.MetaDescription,
                    MetaKeywords = item.MetaKeywords,                    
                    SortOrder = item.SortOrder,
                    DateCreated = item.DateCreated,
                    DateModified = item.DateModified                    
                };
                list.Add(tagVm);
            }

            var paginationSet = new PagedResult<TagViewModel>()
            {
                Results = list,
                //Results = data.ProjectTo<PostViewModel>().ToList(),
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize,
            };

            return paginationSet;
        }
        public PagedResult<TagViewModel> GetProductTagAllPaging(string keyword, int page, int pageSize, string sort)
        {
            var query = _tagRepository.GetAllIncluding(x=>x.Type == CommonConstants.ProductTag);
            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(x => x.Name.Contains(keyword));

            int totalRow = query.Count();
            switch (sort)
            {
                case "oldest":
                    query = query.OrderBy(x => x.DateModified);
                    break;

                default:
                    query = query.OrderByDescending(x => x.SortOrder);
                    break;
            }
            var data = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var list = new List<TagViewModel>();
            foreach (var item in data)
            {
                var tagVm = new TagViewModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Type = item.Type,
                    MetaTitle = item.MetaTitle,
                    MetaDescription = item.MetaDescription,
                    MetaKeywords = item.MetaKeywords,
                    SortOrder = item.SortOrder,
                    DateCreated = item.DateCreated,
                    DateModified = item.DateModified
                };
                list.Add(tagVm);
            }

            var paginationSet = new PagedResult<TagViewModel>()
            {
                Results = list,
                //Results = data.ProjectTo<PostViewModel>().ToList(),
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize,
            };

            return paginationSet;
        }
        public override TagViewModel GetById(string id)
        {
            //var query = (from pc in _postCategoryRepository.GetAll()
            //             join p in _postRepository.GetAll()
            //                 on pc.Id equals p.CategoryId
            //             where pc.Id == id && p.CategoryId == id
            //             select new { p });

            var tag = _tagRepository.FirstOrDefault(x => x.Id == id);
            if (tag == null)
                throw new Exception("Tag null");
            var model = new TagViewModel()
            {
                Id = id,
                Name = tag.Name,
                Type = tag.Type,
                MetaTitle = tag.MetaTitle,
                MetaDescription = tag.MetaDescription,
                MetaKeywords = tag.MetaKeywords,
                SortOrder = tag.SortOrder
            };
            return model;
        }

        public bool IsTagNameExists(string tagName)
        {
            var check = false;
            var tagId = TextHelper.ToUnsignString(tagName);
            var tags = _tagRepository.FirstOrDefault(t => t.Id == tagId);

            var posts = _postRepository.GetAllIncluding(x => x.Tags.Contains(tagName.Trim())).ToList();
            if(tags != null)
            {
                check = true;                
            }

            if (posts != null)
            {
                foreach (var post in posts)
                {
                    if (!string.IsNullOrWhiteSpace(post.Tags))
                    {
                        string[] arrTags = post.Tags.Split(',');
                        foreach (var t in arrTags)
                        {
                            if (t == tagName)
                            {
                                check = true;
                                break;
                            }
                        }
                    }
                }
            }
            return check;
        }

        public override void Update(TagViewModel tagVm)
        {
            //var oldTag = _tagRepository.GetById(tagId);
            //var oldTagName = oldTag.Name;
            //var dateCreated = oldTag.DateCreated;
            //_tagRepository.Delete(tagId);
                       
            var newId = TextHelper.ToUnsignString(tagVm.Name);
            var listStr = new List<string>();
            var listDuoi = new List<int>();
            for (int i = 0; i < 15; i++)
            {
                var check = i == 0 ? _tagRepository.FirstOrDefault(x => x.Id == newId) : _tagRepository.FirstOrDefault(x => x.Id == newId + "-" + (i + 1) + "");
                if (check != null)
                {
                    listStr.Add(check.Id);
                }
            }

            if (listStr.Count() > 0)
            {
                foreach (var item in listStr)
                {
                    if (newId == item)
                    {
                        int idx = item.LastIndexOf('-');
                        if (idx != -1)
                        {
                            var duoi = item.Substring(idx + 1);
                            if (int.TryParse(item, out var number))
                            {
                                listDuoi.Add(number);
                            }
                            else
                            {
                                newId = $"{newId}-1";
                            }
                        }
                    }
                }
                listDuoi.Sort();

                if (listDuoi.Count() == 0)
                    newId = newId + "-" + 1;

                if (listDuoi.Count() == 1)
                {
                    if (listDuoi[0] <= 1)
                        newId = newId + "-" + (listDuoi[0] + 1);
                    else
                        newId = newId + "-" + 1;
                }

                if (listDuoi.Count() == 2)
                {
                    var rs = listDuoi[1] - listDuoi[0] > 1 ? listDuoi[0] + 1 : listDuoi[1] + 1;
                    newId = newId + "-" + rs;
                }

                if (listDuoi.Count() > 2)
                {
                    for (int i = 0; i < listDuoi.Count(); i++)
                    {
                        if (i <= listDuoi.Count() - 2)
                        {
                            if (listDuoi[i + 1] - listDuoi[i] > 1)
                                newId = newId + "-" + (listDuoi[i] + 1);
                        }
                        if (i == listDuoi.Count() - 1)
                            newId = newId + "-" + i;
                    }
                }
            }

            var newTag = new Tag()
            {
                Id = newId,
                Name = tagVm.Name.Trim(),
                Type = CommonConstants.PostTag,
                MetaTitle = tagVm.MetaTitle.Trim(),
                MetaDescription = tagVm.MetaDescription.Trim(),
                MetaKeywords = tagVm.MetaKeywords.Trim(),
                SortOrder = tagVm.SortOrder                
            };
            _tagRepository.Insert(newTag);
            
        }       

        public override void Delete(string id)
        {
            var tag = Tags.Where(t => t.Id == id).FirstOrDefault();
            Tags.Remove(tag);
            var postTags = PostTags.Where(pt => pt.TagId == id);
            PostTags.RemoveRange(postTags);
            _unitOfWork.Commit();
        }

        public void MultiDelete(IList<string> selectedIds)
        {
            foreach (var item in selectedIds)
            {
                var tags = Tags.Where(t => t.Id == item).FirstOrDefault();
                Tags.Remove(tags);
                var postTags = PostTags.Where(pt => pt.TagId == item);
                PostTags.RemoveRange(postTags);
            }
            _unitOfWork.Commit();
        }
    }
}