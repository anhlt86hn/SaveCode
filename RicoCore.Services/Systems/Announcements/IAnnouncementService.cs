using System;
using System.Collections.Generic;
using System.Text;
using RicoCore.Data.Entities;
using RicoCore.Data.Entities.System;
using RicoCore.Services.Systems.Announcements.Dtos;
using RicoCore.Utilities.Dtos;

namespace RicoCore.Services.Systems.Announcements
{
    public interface IAnnouncementService : IWebServiceBase<Announcement, Guid, AnnouncementViewModel>
    {
        // void Add();
        // void Update();
        // void Delete(Guid id);
        // ViewModel GetById(Guid id);
        // List<ViewModel> GetAll();
        // PagedResult<ViewModel> GetAllPaging(string keyword, int page, int pageSize);
        //void Add(Announcement announcement);
        
        List<AnnouncementViewModel> GetListByUserId(Guid userId, int pageIndex, int pageSize, out int totalRow);

        List<AnnouncementViewModel> GetListByUserId(Guid userId, int top);        

        bool MarkAsRead(Guid userId, Guid notificationId);

        //AnnouncementViewModel GetDetail(Guid id);

        List<AnnouncementViewModel> GetListAll(int pageIndex, int pageSize, out int totalRow);

        PagedResult<AnnouncementViewModel> GetAllUnreadPaging(out int totalRow, Guid userId, int pageIndex, int pageSize);
        PagedResult<AnnouncementViewModel> GetAllUnReadPaging(Guid userId, int pageIndex, int pageSize);
    }

}
