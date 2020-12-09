using RicoCore.Data.Entities.Content;
using RicoCore.Services.Content.Posts.Dtos;
using RicoCore.Services.Content.Tags.Dtos;
using RicoCore.Utilities.Dtos;
using System;
using System.Collections.Generic;

namespace RicoCore.Services.Content.Posts
{
    public interface IPostService : IWebServiceBase<Post, Guid, PostViewModel>
    {       
        bool ValidateAddPostName(PostViewModel postVm);
        bool ValidateUpdatePostName(PostViewModel postVm);

        bool ValidateAddPostOrder(PostViewModel postVm);
        bool ValidateUpdatePostOrder(PostViewModel postVm);
        PostViewModel SetValueToNewPost(int postCategoryId);
        int SetNewPostOrder(int categoryId);

        bool ValidateAddPostHotOrder(PostViewModel postVm);
        bool ValidateUpdatePostHotOrder(PostViewModel postVm);
        int SetNewPostHotOrder();

        bool ValidateAddPostHomeOrder(PostViewModel postVm);
        bool ValidateUpdatePostHomeOrder(PostViewModel postVm);
        int SetNewPostHomeOrder();


        void CreateTagId(ref string tagId, ref List<string> listStr, ref List<int> listDuoi);
        void CreateTagAndPostTag(Post post, PostViewModel postVm);        
        int SetNewTagOrder(out List<int> list);
        List<PostViewModel> GetPostsByTagName(string tagName);

        void Update(PostViewModel postVm, string oldTags);        
        PagedResult<PostViewModel> GetAllPaging(int? categoryId, string keyword, int pageSize, int page, string sort);
        PostViewModel GetByUrl(string url);
        PagedResult<PostViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize);

        PagedResult<PostViewModel> GetAllPaging(string keyword, int page, int pageSize);
        PagedResult<PostViewModel> GetPostsByTagPaging(string tag, string keyword, int page, int pageSize, string sort);
        List<PostViewModel> GetLastest(int top);

        List<PostViewModel> GetHotPost(int top);

        List<PostViewModel> GetAllByCategoryId(int categoryId);
        List<PostViewModel> GetListPaging(int page, int pageSize, string sort, out int totalRow);

        List<PostViewModel> Search(string keyword, int page, int pageSize, string sort, out int totalRow);

        List<PostViewModel> GetList(string keyword);

        List<PostViewModel> GetReatedBlogs(Guid id, int top);

        List<string> GetListByName(string name);

        List<TagViewModel> GetListTagById(Guid id);

        TagViewModel GetTag(string tagId);

        void IncreaseView(Guid id);

        List<PostViewModel> GetListByTag(string tagId, int page, int pagesize, out int totalRow);

        List<TagViewModel> GetListTag(string searchText);

       

        bool HasExistsCode(string code);

        List<PostImageViewModel> GetImages(Guid postId);

        void AddImages(Guid postId, string[] images);

        void MultiDelete(IList<string> selectedIds);

        void ImportExcel(string filePath, int categoryId);

        void MultiDeleteByCategoryId(int id);
        IList<string> GetIds(int categoryId);

        PagedResult<PostViewModel> GetPostsByTopic(int categoryId, string keyword, int page, int pageSize, string sortBy);

        List<PostExportViewModel> GetAllToExport();
        List<PostExportViewModel> GetAllToExport(int? categoryId);
    }
}