using System;
using RicoCore.Services.Content.PostCategories.Dtos;
using RicoCore.Data.Entities.Content;
using RicoCore.Utilities.Dtos;
using System.Collections.Generic;
using RicoCore.Services.Content.Posts.Dtos;

namespace RicoCore.Services.Content.PostCategories
{
    public interface IPostCategoryService : IWebServiceBase<PostCategory, int, PostCategoryViewModel>
    {
        //void Add(ProductCategoryViewModel productCategoryVm);
        //void Update(ProductCategoryViewModel productCategoryVm);
        //void Delete(Guid id);
        //ProductCategoryViewModel GetById(Guid id);
        //List<PostCategoryViewModel> GetAll();
        void SetSelectListItemCategories(PostViewModel postVm, List<PostCategoryViewModel> postCategoriesVm);
        bool ValidateAddPostCategoryName(PostCategoryViewModel postCategoryVm);
        bool ValidateUpdatePostCategoryName(PostCategoryViewModel postCategoryVm);

        bool ValidateAddPostCategoryOrder(PostCategoryViewModel productCategoryVm);
        bool ValidateUpdatePostCategoryOrder(PostCategoryViewModel productCategoryVm);
        PostCategoryViewModel SetValueToNewPostCategory(int parentId);
        int SetNewPostCategoryOrder(int? parentId);

        bool ValidateAddPostCategoryHotOrder(PostCategoryViewModel productCategoryVm);
        bool ValidateUpdatePostCategoryHotOrder(PostCategoryViewModel productCategoryVm);
        int SetNewPostCategoryHotOrder();

        bool ValidateAddPostCategoryHomeOrder(PostCategoryViewModel productCategoryVm);
        bool ValidateUpdatePostCategoryHomeOrder(PostCategoryViewModel productCategoryVm);
        int SetNewPostCategoryHomeOrder();


        string GetNameById(int parentId);        
        

        List<PostCategoryViewModel> GetAll(string keyword);
        List<PostCategoryViewModel> GetCategoriesList();

        List<PostCategoryViewModel> GetAllByParentId(int? parentId);

        PagedResult<PostCategoryViewModel> GetAllPaging(string keyword, int page, int pageSize);
        PagedResult<PostCategoryViewModel> GetAllPagingAsync(string keyword, int page, int pageSize);
  
        List<PostCategoryViewModel> GetHomeCategories(int top);

        void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items);

        void ReOrder(int sourceId, int targetId);
        List<PostCategory> AllSubCategories(int id);
        bool HasExistsCode(string code);
        PostCategoryViewModel GetByUrl(string url);
    }
}