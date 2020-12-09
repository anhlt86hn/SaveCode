using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.Interfaces;
using RicoCore.Infrastructure.SharedKernel;
using RicoCore.Utilities.Dtos;

namespace RicoCore.Services
{
    /// <summary>
    /// Base webserice for all services
    /// Creator: 
    /// Created Date: May 10, 2018
    /// </summary>
    /// <typeparam name="TEntity">Main entity for WS</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key type for main entity</typeparam>
    /// <typeparam name="ViewModel">View Model class</typeparam>
    public class WebServiceBase<TEntity, TPrimaryKey, ViewModel> : IWebServiceBase<TEntity, TPrimaryKey, ViewModel>
        where ViewModel : class
        where TEntity : DomainEntity<TPrimaryKey>
    {
        private readonly IRepository<TEntity, TPrimaryKey> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public WebServiceBase(IRepository<TEntity, TPrimaryKey> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        #region Add
        public virtual void Add(ViewModel viewModel)
        {
            var model = Mapper.Map<ViewModel, TEntity>(viewModel);
            _repository.Insert(model);
        }
        #endregion

        #region Update
        public virtual void Update(ViewModel viewModel)
        {
            var model = Mapper.Map<ViewModel, TEntity>(viewModel);
            _repository.Update(model);
        }
        #endregion

        #region Delete
        public virtual void Delete(TPrimaryKey id)
        {
            _repository.Delete(id);
        }
        #endregion

        #region GetById
        public virtual ViewModel GetById(TPrimaryKey id)
        {
            return Mapper.Map<TEntity, ViewModel>(_repository.GetById(id));
        }
        #endregion

        #region GetAll
        public virtual List<ViewModel> GetAll()
        {
            return _repository.GetAll().ProjectTo<ViewModel>().ToList();
        }
        #endregion

        #region GetAllPaging          
        public virtual PagedResult<ViewModel> GetAllPaging(Expression<Func<TEntity, bool>> predicate, Func<TEntity, bool> orderBy, SortDirection sortDirection, int page, int pageSize)
        {
            var query = _repository.GetAll().Where(predicate);
            int totalRow = query.Count();
            if (sortDirection == SortDirection.Ascending)
            {
                query = query.OrderBy(orderBy).Skip((page - 1) * pageSize).Take(pageSize).AsQueryable();
            }
            else
            {
                query = query.OrderByDescending(orderBy).Skip((page - 1) * pageSize).Take(pageSize).AsQueryable();
            }
            var data = query.ProjectTo<ViewModel>().ToList();
            var paginationSet = new PagedResult<ViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }
        #endregion     

        #region Save
        public virtual void Save()
        {
            _unitOfWork.Commit();
        }
        #endregion        
    }
}