using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using RicoCore.Data.Entities;
using RicoCore.Infrastructure.Interfaces;
using RicoCore.Data.Entities.System;
using RicoCore.Services.Systems.Announcements.Dtos;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using RicoCore.Utilities.Dtos;

namespace RicoCore.Services.Systems.Announcements
{
    public class AnnouncementService : WebServiceBase<Announcement, Guid, AnnouncementViewModel>, IAnnouncementService
    {
        private readonly IRepository<Announcement, Guid> _announcementRepository;
        private readonly IRepository<AnnouncementUser, Guid> _announcementUserRepository;        

        public AnnouncementService(IRepository<Announcement, Guid> announcementRepository,
            IRepository<AnnouncementUser, Guid> announcementUserRepository,
            IUnitOfWork unitOfWork) : base(announcementRepository, unitOfWork)
        {
            _announcementRepository = announcementRepository;
            _announcementUserRepository = announcementUserRepository;            
        }

        public List<AnnouncementViewModel> GetListByUserId(Guid userId, int pageIndex, int pageSize, out int totalRow)
        {
            var query = _announcementRepository.GetAll(x => x.OwnerId == userId);
            totalRow = query.Count();
            return query.OrderByDescending(x => x.DateCreated)
                .Skip(pageSize * (pageIndex - 1)).Take(pageSize).ProjectTo<AnnouncementViewModel>().ToList();
        }

        public List<AnnouncementViewModel> GetListByUserId(Guid userId, int top)
        {
            return _announcementRepository.GetAll(x => x.OwnerId == userId)
                .OrderByDescending(x => x.DateCreated)
                .Take(top).ProjectTo<AnnouncementViewModel>().ToList();
        }      

        public List<AnnouncementViewModel> GetListAll(int pageIndex, int pageSize, out int totalRow)
        {
            var query = _announcementRepository.GetAll();
            totalRow = query.Count();
            return query.OrderByDescending(x => x.DateCreated)
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize).ProjectTo<AnnouncementViewModel>().ToList();
        }
       
        //public AnnouncementViewModel GetDetail(Guid id)
        //{
        //    return _announcementRepository.Single(x => x.Id == id);
        //}

        public PagedResult<AnnouncementViewModel> GetAllUnreadPaging(out int totalRow, Guid userId, int pageIndex, int pageSize)
        {
            var query = from x in _announcementRepository.GetAll()
                         join y in _announcementUserRepository.GetAll()
                         on x.Id equals y.AnnouncementId
                         into xy
                         from y in xy.DefaultIfEmpty()
                         where (y.HasRead == null || y.HasRead == false)
                         && (y.UserId == null || y.UserId == userId)
                         select x;
            totalRow = query.Count();
            var model =  query.OrderByDescending(x => x.DateCreated)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize).ProjectTo<AnnouncementViewModel>().ToList();
            var paginationSet = new PagedResult<AnnouncementViewModel>()
            {
                Results = model,
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize,
            };
            return paginationSet;
        }

        public PagedResult<AnnouncementViewModel> GetAllUnReadPaging(Guid userId, int pageIndex, int pageSize)
        {
            var query = from x in _announcementRepository.GetAll()
                        join y in _announcementUserRepository.GetAll()
                            on x.Id equals y.AnnouncementId
                            into xy
                        from annonUser in xy.DefaultIfEmpty()
                        where annonUser.HasRead == false && (annonUser.UserId == null || annonUser.UserId == userId)
                        select x;
            int totalRow = query.Count();

            var model = query.OrderByDescending(x => x.DateCreated)
                .Skip(pageSize * (pageIndex - 1)).Take(pageSize).ProjectTo<AnnouncementViewModel>().ToList();

            var paginationSet = new PagedResult<AnnouncementViewModel>
            {
                Results = model,
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public bool MarkAsRead(Guid userId, Guid notificationId)
        {
            bool result = false;

            var announ = _announcementUserRepository.Single(x => x.AnnouncementId == notificationId && x.UserId == userId);
            if (announ == null)
            {
                _announcementUserRepository.Insert(new AnnouncementUser
                {
                    AnnouncementId = notificationId,
                    UserId = userId,
                    HasRead = true
                });
                result = true;
            }
            else
            {
                if (announ.HasRead == false)
                {
                    announ.HasRead = true;
                    result = true;
                }
            }
            return result;
        }
    }
}