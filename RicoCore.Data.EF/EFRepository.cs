using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.EF
{
    /// <summary>
    /// /// <summary>
    /// Serves as a generic base class for concrete repositories to support basic CRUD oprations on data in the system.
    /// </summary>
    /// </summary>
    /// <typeparam name="TEntity">The type of entity this repository works with. Must be a class inheriting DomainEntity</typeparam>
    /// <typeparam name="TPrimaryKey">The type of primary key</typeparam>
    public class EFRepository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>, IDisposable
        where TEntity : DomainEntity<TPrimaryKey>
    {
        protected readonly AppDbContext _context;

        public EFRepository(AppDbContext context)
        {
            _context = context;
        }


        #region Insert

        public TEntity Insert(TEntity entity)
        {
            return _context.Set<TEntity>().Add(entity).Entity;
        }
        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            var result = await _context.Set<TEntity>().AddAsync(entity);
            return result.Entity;
        }
        #endregion

        #region InsertAndGetId
        public TPrimaryKey InsertAndGetId(TEntity entity)
        {
            var result = _context.Set<TEntity>().Add(entity);
            return result.Entity.Id;
        }

        public async Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            var result = await _context.Set<TEntity>().AddAsync(entity);
            return result.Entity.Id;
        }
        #endregion

        #region Update
        public TEntity Update(TEntity entity)
        {
            var result = _context.Set<TEntity>().Update(entity);
            return result.Entity;
        }
        #endregion

        #region Delete
        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void Delete(TPrimaryKey id)
        {
            _context.Set<TEntity>().Remove(GetById(id));
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            _context.Set<TEntity>().RemoveRange(GetAll().Where(predicate));
        }

        public void DeleteMultiple(List<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }
        #endregion

        #region GetById
        public TEntity GetById(TPrimaryKey id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        //public TEntity GetByPostIdAndTagId(Expression<Func<TEntity, bool>> predicate)
        //{
        //    return _context.Set<TEntity>().Where(predicate).FirstOrDefault();
        //}
        public TEntity GetById(TPrimaryKey id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return GetAllIncluding(includeProperties).SingleOrDefault(x => x.Id.Equals(id));
        }
        public async Task<TEntity> GetByIdAsync(TPrimaryKey id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        #endregion

        #region GetAll
        public IQueryable<TEntity> GetAll(bool isAll = true)
        {
            if (typeof(TEntity) is IHasSoftDelete && isAll == false)
            {
                return _context.Set<TEntity>().Where(x => ((IHasSoftDelete)x).IsDeleted == false).AsQueryable();
            }
            return _context.Set<TEntity>().AsQueryable();
        }
        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate, bool isAll = true)
        {
            return GetAll(isAll).Where(predicate);
        }
        
        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> items = GetAll();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return items;
        }

        public IQueryable<TEntity> GetAllIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> items = GetAll();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return items.Where(predicate);
        }

        public List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).ToList();
        }

        public List<TEntity> GetAllList()
        {
            return GetAll().ToList();
        }

        public async Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).ToListAsync();
        }

        public async Task<List<TEntity>> GetAllListAsync()
        {
            return await GetAll().ToListAsync();
        }
        #endregion

        #region FirstOrDefault

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public TEntity FirstOrDefault(TPrimaryKey id)
        {
            return _context.Set<TEntity>().FirstOrDefault(x => x.Id.Equals(id));
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().FirstOrDefaultAsync(predicate);
        }

        public async Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        #endregion

        #region Single

        public TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Single(predicate);
        }

        public TEntity SingleIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return GetAllIncluding(includeProperties).SingleOrDefault(predicate);
        }

        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().SingleAsync(predicate);
        }
        #endregion

        #region Count

        public int Count()
        {
            return GetAll().Count();
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Count(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().CountAsync(predicate);
        }

        public async Task<int> CountAsync()
        {
            return await GetAll().CountAsync();
        }

        #endregion

        #region LongCount
        public long LongCount(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().LongCount(predicate);
        }

        public long LongCount()
        {
            return GetAll().LongCount();
        }

        public async Task<long> LongCountAsync()
        {
            return await GetAll().LongCountAsync();
        }

        public async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().LongCountAsync(predicate);
        }

        #endregion        

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

       
    }
}