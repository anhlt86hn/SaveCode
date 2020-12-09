using System.Collections.Generic;
using RicoCore.Data.Entities.Content;
using RicoCore.Services.Content.Contacts.Dtos;
using RicoCore.Utilities.Dtos;

namespace RicoCore.Services.Content.Contacts
{
    public interface IContactService : IWebServiceBase<ContactDetail, int, ContactDetailViewModel>
    {      
        PagedResult<ContactDetailViewModel> GetAllPaging(string keyword, int page, int pageSize);
        void MultiDelete(IList<string> selectedIds);

        bool ValidateAddContactDetailName(ContactDetailViewModel vm);       

        bool ValidateUpdateContactDetailName(ContactDetailViewModel vm);
    }
}