using QShirt.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QShirt.Persistence
{
    public interface IRepository<T> : IQueryable<T>, IEnumerable<T>, IEnumerable, IQueryable, IAsyncEnumerable<T> where T : EntityBase
    {
        /// <summary>
        /// Returns query for object by Id
        /// </summary>
        public IQueryable<T> GetByIdQuery(Guid id);

        /// <summary>
        /// Returns query for instances by their ids
        /// </summary>
        /// <param name="ids">instance ids</param>
        public IQueryable<T> GetByIdsQuery(IEnumerable<Guid> ids);

        /// <summary>
        /// Returns object by Id
        /// </summary>
        public T? GetById(Guid id);

        /// <summary>
        /// Returns objects by their ids
        /// </summary>
        public List<T> GetByIds(List<Guid> ids);

        /// <summary>
        /// Returns object by its id (async)
        /// </summary>
        public Task<T?> GetByIdAsync(Guid id);

        /// <summary>
        /// Adds object
        /// </summary>
        public void Add(T objectToAdd);

        /// <summary>
        /// Adds collection of objects
        /// </summary>
        public void AddRange(IEnumerable<T> objectsToAdd);

        /// <summary>
        /// Removes instance by its id
        /// </summary>
        public void Remove(Guid instanceId);

        /// <summary>
        /// Removes instance by its id (asynchronously)
        /// </summary>
        public Task RemoveAsync(Guid instanceId);

        /// <summary>
        /// Removes set of instances by their ids
        /// </summary>
        public void RemoveRange(IEnumerable<Guid> ids);

        /// <summary>
        /// Removes set of instances by their ids (asynchronously)
        /// </summary>
        public Task RemoveRangeAsync(IEnumerable<Guid> ids);


    }
}
