using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EdaSample.Common.DataAccess
{
    /// <summary>
    /// Represents that the implemented classes are data access objects that perform
    /// CRUD operations on the given entity type.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IDataAccessObject : IDisposable
    {
        /// <summary>
        /// Gets the entity by specified identifier asynchronously.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="id">The identifier of the entity.</param>
        /// <returns>The task which performs the data retrieval operation.</returns>
        Task<TEntity> GetByIdAsync<TEntity>(Guid id)
            where TEntity : IEntity;

        /// <summary>
        /// Gets all of the entities asynchronously.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>The task which performs the data retrieval operation, and after
        /// the operation has completed, would return a list of retrieved entities.
        /// </returns>
        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>()
            where TEntity : IEntity;

        /// <summary>
        /// Adds the given entity asynchronously.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to be added.</param>
        /// <returns>The task which performs the adding operation.</returns>
        Task AddAsync<TEntity>(TEntity entity)
            where TEntity : IEntity;

        /// <summary>
        /// Updates the entity by the specified identifier asynchronously.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="id">The identifier of the entity to be updated.</param>
        /// <param name="entity">The entity which contains the updated value.</param>
        /// <returns>The task which performs the updating operation.</returns>
        Task UpdateByIdAsync<TEntity>(Guid id, TEntity entity)
            where TEntity : IEntity;

        /// <summary>
        /// Finds the entities which match the specified criteria that is defined by the given specification asynchronously.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="expr">The expression which defines the matching criteria.</param>
        /// <returns>The task which performs the data retrieval operation, and after the operation
        /// has completed, would return a list of entities that match the specified criteria.</returns>
        Task<IEnumerable<TEntity>> FindBySpecificationAsync<TEntity>(Expression<Func<TEntity, bool>> expr)
            where TEntity : IEntity;

        /// <summary>
        /// Deletes the entity by specified identifier asynchronously.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="id">The identifier which represents the entity that is going to be deleted.</param>
        /// <returns>The task which performs the deletion operation.</returns>
        Task DeleteByIdAsync<TEntity>(Guid id)
            where TEntity : IEntity;
    }
}
