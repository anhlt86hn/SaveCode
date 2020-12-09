using System;
using System.Collections.Generic;
using RicoCore.Services;
using RicoCore.Services.Content.Posts.Dtos;
using RicoCore.Data.Entities;
using RicoCore.Utilities.Dtos;
using RicoCore.Data.Entities.Content;
using RicoCore.Services.Content.Tags.Dtos;

namespace RicoCore.Services.Content.Tags
{
    public interface ITagService : IWebServiceBase<Tag, string, TagViewModel>
    {
        bool ValidateAddTagName(TagViewModel tagVm, string type);
        bool ValidateUpdateTagName(TagViewModel tagVm, string type);

        bool ValidateAddTagOrder(TagViewModel tagVm, string type);
        bool ValidateUpdateTagOrder(TagViewModel tagVm, string type);        
        int SetNewTagOrder(out List<int> list, string typeTag);


        PagedResult<TagViewModel> GetPostTagAllPaging(string keyword,int pageSize, int page, string sort);
        PagedResult<TagViewModel> GetProductTagAllPaging(string keyword, int pageSize, int page, string sort);
        void Add(TagViewModel tagVm, string typeTag);
        void CreateTagId(ref string tagId, ref List<string> listStr, ref List<int> listDuoi);
        void CreateTagAndPostTag(Post post, PostViewModel postVm);
        void CheckNewTag(out string newTagId, string oldTagId, Post post, PostViewModel postVm);
        

        
        bool IsTagNameExists(string tagName);
        void MultiDelete(IList<string> selectedIds);      
    }
}