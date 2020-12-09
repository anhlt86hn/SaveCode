using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Infrastructure.Interfaces
{
    //
    // Summary:
    //     This interface is implemented by all repositories to ensure implementation of
    //     fixed methods.
    //
    // Type parameters:
    //   TEntity:
    //     Main Entity type this repository works on
    //
    //   TPrimaryKey:
    //     Primary key type of the entity
    public interface IRepository<TEntity, TPrimaryKey> where TEntity : DomainEntity<TPrimaryKey>
    {
        #region Insert
        //
        // Summary:
        //     Inserts a new entity.
        //
        // Parameters:
        //   entity:
        //     Inserted entity
        TEntity Insert(TEntity entity);
        Task<TEntity> InsertAsync(TEntity entity);
        #endregion

        #region InsertAndGetId
        TPrimaryKey InsertAndGetId(TEntity entity);
        Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity);
        #endregion

        #region Update
        //
        // Summary:
        //     Updates an existing entity.
        //
        // Parameters:
        //   entity:
        //     Entity
        TEntity Update(TEntity entity);
        #endregion

        #region Delete
        //
        // Summary:
        //     Deletes an entity.
        //
        // Parameters:
        //   entity:
        //     Entity to be deleted
        void Delete(TEntity entity);

        //
        // Summary:
        //     Deletes an entity by primary key.
        //
        // Parameters:
        //   id:
        //     Primary key of the entity
        void Delete(TPrimaryKey id);

        //
        // Summary:
        //     Deletes many entities by function. Notice that: All entities fits to given predicate
        //     are retrieved and deleted. This may cause major performance problems if there
        //     are too many entities with given predicate.
        //
        // Parameters:
        //   predicate:
        //     A condition to filter entities
        void Delete(Expression<Func<TEntity, bool>> predicate);

        void DeleteMultiple(List<TEntity> entity);
        #endregion

        #region GetById
        //
        // Summary:
        //     Gets an entity with given primary key.
        //
        // Parameters:
        //   id:
        //     Primary key of the entity to get
        //
        // Returns:
        //     Entity
        TEntity GetById(TPrimaryKey id);

        //TEntity GetByPostIdAndTagId(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        TEntity GetById(TPrimaryKey id, params Expression<Func<TEntity, object>>[] includeProperties);

        //
        // Summary:
        //     Gets an entity with given primary key.
        //
        // Parameters:
        //   id:
        //     Primary key of the entity to get
        //
        // Returns:
        //     Entity
        Task<TEntity> GetByIdAsync(TPrimaryKey id);
        #endregion

        #region GetAll
        //
        // Summary:
        //     Used to get a IQueryable that is used to retrieve entities from entire table.
        //
        // Returns:
        //     IQueryable to be used to select entities from database
        IQueryable<TEntity> GetAll(bool isAll = true);

        /// <summary>
        /// Get all with predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="isAll"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate, bool isAll = true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetAllIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

        //
        // Summary:
        //     Used to get a IQueryable that is used to retrieve entities from entire table.
        //     One or more
        //
        // Parameters:
        //   propertySelectors:
        //     A list of include expressions.
        //
        // Returns:
        //     IQueryable to be used to select entities from database
        IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);

        //
        // Summary:
        //     Used to get all entities based on given predicate.
        //
        // Parameters:
        //   predicate:
        //     A condition to filter entities
        //
        // Returns:
        //     List of all entities
        List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate);

        //
        // Summary:
        //     Used to get all entities.
        //
        // Returns:
        //     List of all entities
        List<TEntity> GetAllList();

        //
        // Summary:
        //     Used to get all entities based on given predicate.
        //
        // Parameters:
        //   predicate:
        //     A condition to filter entities
        //
        // Returns:
        //     List of all entities
        Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate);

        //
        // Summary:
        //     Used to get all entities.
        //
        // Returns:
        //     List of all entities
        Task<List<TEntity>> GetAllListAsync();
        #endregion

        #region FirstOrDefault
        //
        // Summary:
        //     Gets an entity with given given predicate or null if not found.
        //
        // Parameters:
        //   predicate:
        //     Predicate to filter entities
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        //
        // Summary:
        //     Gets an entity with given primary key or null if not found.
        //
        // Parameters:
        //   id:
        //     Primary key of the entity to get
        //
        // Returns:
        //     Entity or null
        TEntity FirstOrDefault(TPrimaryKey id);

        //
        // Summary:
        //     Gets an entity with given given predicate or null if not found.
        //
        // Parameters:
        //   predicate:
        //     Predicate to filter entities
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        //
        // Summary:
        //     Gets an entity with given primary key or null if not found.
        //
        // Parameters:
        //   id:
        //     Primary key of the entity to get
        //
        // Returns:
        //     Entity or null
        Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id);

        #endregion

        #region Single
        //
        // Summary:
        //     Gets exactly one entity with given predicate. Throws exception if no entity or
        //     more than one entity.
        //
        // Parameters:
        //   predicate:
        //     Entity
        TEntity Single(Expression<Func<TEntity, bool>> predicate);

        //
        // Summary:
        //     Gets exactly one entity with given predicate. Throws exception if no entity or
        //     more than one entity.
        //
        // Parameters:
        //   predicate:
        //     Entity
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);

        //
        // Summary:
        //     Gets exactly one entity with given predicate. Throws exception if no entity or
        //     more than one entity.
        //
        // Parameters:
        //   predicate:
        //     Entity
        TEntity SingleIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        #endregion

        #region Count
        //
        // Summary:
        //     Gets count of all entities in this repository.
        //
        // Returns:
        //     Count of entities
        int Count();

        //
        // Summary:
        //     Gets count of all entities in this repository based on given predicate.
        //
        // Parameters:
        //   predicate:
        //     A method to filter count
        //
        // Returns:
        //     Count of entities
        int Count(Expression<Func<TEntity, bool>> predicate);

        //
        // Summary:
        //     Gets count of all entities in this repository based on given predicate.
        //
        // Parameters:
        //   predicate:
        //     A method to filter count
        //
        // Returns:
        //     Count of entities
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        //
        // Summary:
        //     Gets count of all entities in this repository.
        //
        // Returns:
        //     Count of entities
        Task<int> CountAsync();
        //
        // Summary:
        //     Gets count of all entities in this repository based on given predicate (use this
        //     overload if expected return value is greather than System.Int32.MaxValue).
        //
        // Parameters:
        //   predicate:
        //     A method to filter count
        //
        // Returns:
        //     Count of entities

        #endregion

        #region LongCount
        long LongCount(Expression<Func<TEntity, bool>> predicate);

        //
        // Summary:
        //     Gets count of all entities in this repository (use if expected return value is
        //     greather than System.Int32.MaxValue.
        //
        // Returns:
        //     Count of entities
        long LongCount();

        //
        // Summary:
        //     Gets count of all entities in this repository (use if expected return value is
        //     greather than System.Int32.MaxValue.
        //
        // Returns:
        //     Count of entities
        Task<long> LongCountAsync();

        //
        // Summary:
        //     Gets count of all entities in this repository based on given predicate (use this
        //     overload if expected return value is greather than System.Int32.MaxValue).
        //
        // Parameters:
        //   predicate:
        //     A method to filter count
        //
        // Returns:
        //     Count of entities
        Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate);
        #endregion
        

    }
}