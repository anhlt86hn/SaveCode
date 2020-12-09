using RicoCore.Data.Entities.Content;
using RicoCore.Data.Entities.System;
using RicoCore.Services.Content.Posts.Dtos;
using RicoCore.Services.Content.Tags.Dtos;
using RicoCore.Services.Systems.Accounts.Dtos;
using RicoCore.Services.Systems.Passwords.Dtos;
using RicoCore.Utilities.Dtos;
using System;
using System.Collections.Generic;

namespace RicoCore.Services.Systems.Passwords
{
    public interface IPasswordService : IWebServiceBase<Password, int, PasswordViewModel>
    {
        bool ValidateAddPasswordOrder(PasswordViewModel passwordVm);
        bool ValidateUpdatePasswordOrder(PasswordViewModel passwordVm);
        //PasswordViewModel SetValueToNewPost(int postCategoryId);
        int SetNewOrder();
        PagedResult<PasswordViewModel> GetAllPaging(string keyword, int page, int pageSize, string sortBy);
        void MultiDelete(IList<string> selectedIds);
        void SetSelectListItemPasswords(AccountViewModel accountVm, List<PasswordViewModel> passwordsVm);
    }
}