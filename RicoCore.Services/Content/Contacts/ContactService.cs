using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using RicoCore.Services.Content.Contacts.Dtos;
using RicoCore.Data.Entities;

using RicoCore.Infrastructure.Interfaces;
using RicoCore.Utilities.Dtos;
using RicoCore.Data.Entities.Content;
using Microsoft.EntityFrameworkCore;
using RicoCore.Data.EF;
using RicoCore.Utilities.Helpers;

namespace RicoCore.Services.Content.Contacts
{
    public class ContactService : WebServiceBase<ContactDetail, int, ContactDetailViewModel>, IContactService
    {
        private readonly IRepository<ContactDetail, int> _contactRepository;
        private readonly DbSet<ContactDetail> ContactDetails;
        private readonly IUnitOfWork _unitOfWork;

        public ContactService(IRepository<ContactDetail, int> contactRepository,
            AppDbContext context,
            IUnitOfWork unitOfWork)
            : base(contactRepository, unitOfWork)
        {
            _contactRepository = contactRepository;
            _unitOfWork = unitOfWork;
            ContactDetails = context.Set<ContactDetail>();
        }
       
        public PagedResult<ContactDetailViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            var query = _contactRepository.GetAll();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword));

            int totalRow = query.Count();
            var data = query.OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var paginationSet = new PagedResult<ContactDetailViewModel>()
            {
                Results = data.ProjectTo<ContactDetailViewModel>().ToList(),
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public bool ValidateAddContactDetailName(ContactDetailViewModel vm)
        {
            return _contactRepository.GetAll().Any(x => x.Name.ToLower() == vm.Name.ToLower());
        }       

        public bool ValidateUpdateContactDetailName(ContactDetailViewModel vm)
        {
            var compare = _contactRepository.GetAllIncluding(x => x.Name.ToLower() == vm.Name.ToLower());
            var result = compare.Count() > 1 ? true : false;
            return result;
        }

        public override void Add(ContactDetailViewModel contactVm)
        {            
            var contact = Mapper.Map<ContactDetailViewModel, ContactDetail>(contactVm);          
            _contactRepository.Insert(contact);
        }

        public override void Update(ContactDetailViewModel contactVm)
        {

            var contact = _contactRepository.GetById(contactVm.Id);
            Mapper.Map(contactVm, contact);
            _contactRepository.Update(contact);
        }

        public void MultiDelete(IList<string> selectedIds)
        {
            foreach (var item in selectedIds)
            {
                var contacts = ContactDetails.Where(r => r.Id == int.Parse(item)).FirstOrDefault();
                ContactDetails.Remove(contacts);
            }
            _unitOfWork.Commit();
        }
    }
}