using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using RicoCore.Data.Entities;
using RicoCore.Infrastructure.Interfaces;
using RicoCore.Utilities.Dtos;
using RicoCore.Services.Systems.Settings;
using RicoCore.Data.Entities.System;
using RicoCore.Services.Systems.Settings.Dtos;

namespace RicoCore.Services.Systems.Settings
{
    public class SystemConfigService : WebServiceBase<Setting, Guid, SystemConfigViewModel>, ISystemConfigService
    {
        private readonly IRepository<Setting, Guid> _settingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SystemConfigService(IRepository<Setting, Guid> settingRepository,
            IUnitOfWork unitOfWork) : base(settingRepository, unitOfWork)
        {
            _settingRepository = settingRepository;
            _unitOfWork = unitOfWork;
        }

        public override void Add(SystemConfigViewModel systemConfigVm)
        {
            var setting = Mapper.Map<SystemConfigViewModel, Setting>(systemConfigVm);
            _settingRepository.Insert(setting);
        }
        public override void Update(SystemConfigViewModel systemConfigVm)
        {
            var setting = Mapper.Map<SystemConfigViewModel, Setting>(systemConfigVm);
            _settingRepository.Update(setting);
        }

        public override void Delete(Guid id)
        {
            _settingRepository.Delete(id);
        }

        public override SystemConfigViewModel GetById(Guid id)
        {
            return Mapper.Map<Setting, SystemConfigViewModel>(_settingRepository.GetById(id));
        }

        public override List<SystemConfigViewModel> GetAll()
        {
            return _settingRepository.GetAll().OrderBy(x => x.Id).ProjectTo<SystemConfigViewModel>().ToList();
        }

        public PagedResult<SystemConfigViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            var query = _settingRepository.GetAll();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword));

            int totalRow = query.Count();
            var data = query.OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var paginationSet = new PagedResult<SystemConfigViewModel>()
            {
                Results = data.ProjectTo<SystemConfigViewModel>().ToList(),
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public void MultiDelete(IList<string> selectedIds)
        {
            foreach (var item in selectedIds)
            {
                var setting = _settingRepository.FirstOrDefault(x => x.Id == Guid.Parse(item));
                var settingId = setting.Id;
                _settingRepository.Delete(settingId);               
            }
            _unitOfWork.Commit();
        }
    }
}