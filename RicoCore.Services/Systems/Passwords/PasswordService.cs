using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using RicoCore.Data.EF;
using RicoCore.Data.Entities.Content;
using RicoCore.Data.Entities.System;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.Interfaces;
using RicoCore.Infrastructure.SharedKernel;
using RicoCore.Services.Content.Posts.Dtos;
using RicoCore.Services.Content.Tags.Dtos;
using RicoCore.Services.Systems.Accounts.Dtos;
using RicoCore.Services.Systems.Passwords.Dtos;
using RicoCore.Utilities.Constants;
using RicoCore.Utilities.Dtos;
using RicoCore.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RicoCore.Services.Systems.Passwords
{
    public class PasswordService : WebServiceBase<Password, int, PasswordViewModel>, IPasswordService
    {
        private readonly IRepository<Password, int> _passwordRepository;               
        private readonly IUnitOfWork _unitOfWork;

        public PasswordService(IRepository<Password, int> passwordRepository,
            
            IUnitOfWork unitOfWork) : base(passwordRepository, unitOfWork)
        {
            _passwordRepository = passwordRepository;           
            _unitOfWork = unitOfWork;           
        }

        public bool ValidateAddPasswordOrder(PasswordViewModel passwordVm)
        {
            return _passwordRepository.GetAll().Any(x => x.Order == passwordVm.Order && x.Order != 0);
        }
        public bool ValidateUpdatePasswordOrder(PasswordViewModel passwordVm)
        {
            var compare = _passwordRepository.GetAllIncluding(x => x.Order == passwordVm.Order && x.Order != 0);
            var result = compare.Count() > 1 ? true : false;
            return result;
        }
       
        public int SetNewOrder()
        {
            var order = 0;
            var list = _passwordRepository.GetAllIncluding(x => x.Order != 0).OrderBy(x => x.Order).Select(x => x.Order).ToList();
            order = CommonMethods.GetOrder(list);
            return order;
        }

        public PagedResult<PasswordViewModel> GetAllPaging(string keyword, int page, int pageSize, string sortBy)
        {
            var query = _passwordRepository.GetAll();
            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(x => x.Content.Contains(keyword));

            int totalRow = query.Count();
            switch (sortBy)
            {
                case "theo-ten":
                    query = query.OrderBy(x => x.Content);
                    break;
                
                default:
                    query = query.OrderByDescending(x => x.Order);
                    break;
            }
            var data = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var list = new List<PasswordViewModel>();
            foreach (var item in data)
            {
                var passwordVm = new PasswordViewModel()
                {
                    Id = item.Id,
                    Content = item.Content,
                    Level = item.Level,
                    Order = item.Order
                };               
                list.Add(passwordVm);
            }

            var paginationSet = new PagedResult<PasswordViewModel>()
            {
                Results = list,
                //Results = data.ProjectTo<PasswordViewModel>().ToList(),
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize,
            };

            return paginationSet;
        }

        public void MultiDelete(IList<string> selectedIds)
        {
            foreach (var item in selectedIds)
            {
                var unit = _passwordRepository.FirstOrDefault(x => x.Id == int.Parse(item));
                var unitId = unit.Id;
                _passwordRepository.Delete(unitId);                
            }
            _unitOfWork.Commit();
        }

        public void SetSelectListItemPasswords(AccountViewModel accountVm, List<PasswordViewModel> passwordsVm)
        {
            accountVm.Passwords = new List<SelectListItem>();
            foreach (var item in passwordsVm)
            {
                accountVm.Passwords.Add(new SelectListItem()
                {
                    Text = item.Content,
                    Value = item.Id.ToString(),
                    Selected = accountVm.PasswordId == item.Id ? true : false
                });
            }
        }
    }
}