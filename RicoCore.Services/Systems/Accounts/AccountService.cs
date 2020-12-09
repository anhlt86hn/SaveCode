using AutoMapper;
using AutoMapper.QueryableExtensions;
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
using RicoCore.Utilities.Constants;
using RicoCore.Utilities.Dtos;
using RicoCore.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RicoCore.Services.Systems.Accounts
{
    public class AccountService : WebServiceBase<Account, int, AccountViewModel>, IAccountService
    {
        private readonly IRepository<Account, int> _accountRepository;
        private readonly IRepository<Password, int> _passwordRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IRepository<Account, int> accountRepository,
            IRepository<Password, int> passwordRepository,
            IUnitOfWork unitOfWork) : base(accountRepository, unitOfWork)
        {
            _accountRepository = accountRepository;
            _passwordRepository = passwordRepository;
            _unitOfWork = unitOfWork;
        }

        public bool ValidateAddAccountOrder(AccountViewModel accountVm)
        {
            return _accountRepository.GetAll().Any(x => x.Order == accountVm.Order && x.Order != 0);
        }
        public bool ValidateUpdateAccountOrder(AccountViewModel accountVm)
        {
            var compare = _accountRepository.GetAllIncluding(x => x.Order == accountVm.Order && x.Order != 0);
            var result = compare.Count() > 1 ? true : false;
            return result;
        }

        public int SetNewOrder()
        {
            var order = 0;
            var list = _accountRepository.GetAllIncluding(x => x.Order != 0).OrderBy(x => x.Order).Select(x => x.Order).ToList();
            order = CommonMethods.GetOrder(list);
            return order;
        }

        public PagedResult<AccountViewModel> GetAllPaging(string keyword, int page, int pageSize, string sortBy)
        {
            var query = from a in _accountRepository.GetAll()
                        join p in _passwordRepository.GetAll()
                        on a.PasswordId equals p.Id
                        select new AccountViewModel {
                            Id = a.Id,
                            Domain = a.Domain,
                            UserName = a.UserName,
                            PasswordId = a.PasswordId,
                            Password = p.Content,
                            HiddenPassword = a.HiddenPassword,
                            Phone = a.Phone,
                            SecurityEmail = a.SecurityEmail,
                            Url = a.Url,                         
                            Note = a.Note,
                            Level = a.Level,
                            Order = a.Order
                        };
            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(x => x.Domain.Contains(keyword) || x.UserName.Contains(keyword));

            int totalRow = query.Count();
            switch (sortBy)
            {
                case "theo-ten-domain":
                    query = query.OrderBy(x => x.Domain);
                    break;

                default:
                    query = query.OrderByDescending(x => x.Order);
                    break;
            }
            query = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var list = query.ToList();

            var paginationSet = new PagedResult<AccountViewModel>()
            {
                Results = list,
                //Results = data.ProjectTo<AccountViewModel>().ToList(),
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
                var unit = _accountRepository.FirstOrDefault(x => x.Id == int.Parse(item));
                var unitId = unit.Id;
                _accountRepository.Delete(unitId);
            }
            _unitOfWork.Commit();
        }

        public void ImportExcel(string filePath)
        {
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                Account account;
                for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                {
                    account = new Account();                    
                    account.Domain = workSheet.Cells[i, 1].Value.ToString();
                    account.UserName = workSheet.Cells[i, 2].Value.ToString();
                    int.TryParse(workSheet.Cells[i, 3].Value.ToString(), out var passwordId);
                    account.PasswordId = passwordId;
                    account.Password = workSheet.Cells[i, 4].Value.ToString();
                    account.HiddenPassword = workSheet.Cells[i, 5].Value.ToString();
                    account.Phone = workSheet.Cells[i, 6].Value.ToString();
                    account.SecurityEmail = workSheet.Cells[i, 7].Value.ToString();
                    account.Url = workSheet.Cells[i, 8].Value.ToString();
                    account.Note = workSheet.Cells[i, 9].Value.ToString();
                    int.TryParse(workSheet.Cells[i, 10].Value.ToString(), out var order);
                    account.Order = order;
                    int.TryParse(workSheet.Cells[i, 11].Value.ToString(), out var level);
                    account.Level = level;
                        
                    //account.HotFlag = hotFlag == true ? Status.Actived : Status.InActived;                    
                    _accountRepository.Insert(account);
                }
            }
        }

        public List<AccountExportViewModel> GetAllToExport()
        {
            var list = from a in _accountRepository.GetAll()
                       select new AccountExportViewModel
                       {
                           Domain = a.Domain,
                           Level = a.Level,
                           HiddenPassword = a.HiddenPassword,
                           Note = a.Note,
                           Order = a.Order,
                           Password = a.Password,
                           PasswordId = a.PasswordId,
                           Phone = a.Phone,
                           SecurityEmail = a.SecurityEmail,
                           Url = a.Url,
                           UserName = a.UserName,
                       };
            //var result =  list.Select(x => new { x.Domain, x.Level, x.HiddenPassword, x.Note, x.Order, x.Password, x.PasswordId, x.Phone, x.SecurityEmail, x.Url, x.UserName }).ToList();
            return list.ToList();
        }

    }
}